using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using Payroll.Models;
using Payroll.Models.ServiceResponses;
using Payroll.Models.ServiceResponses.Generic;
using Payroll.Models.Views;

namespace Payroll.Services.Currency
{
    public class CurrencyHandler : ICurrencyHandler
    {
        private PayrollDbContext _db { get; set; }
        private IMapper _mapper { get; set; }

        public CurrencyHandler(PayrollDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ExchangeRateViewModel>> GetHighestMonthRate()
        {
            try
            {
                var resultList = new List<Task<ServiceResponse<ExchangeRateViewModel>>>();
                var currentDate = DateTime.Now;

                for (int i = 1; i <= currentDate.Day; i++)
                {
                    var date = new DateTime(currentDate.Year, currentDate.Month, i);
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                        continue;

                    resultList.Add(GetUsdExchangeByDate(date));

                }

                await Task.WhenAll(resultList);


                return new SuccessServiceResponse<ExchangeRateViewModel>(resultList.Where(e => e.Result.Status == ServiceResponseStatus.Success)
                                                                                   .OrderByDescending(e => e.Result.Data.ExchangeRate)
                                                                                   .Select(e => e.Result.Data).FirstOrDefault());
            }
            catch(Exception e)
            {
                return new ErrorServiceResponse<ExchangeRateViewModel>(e.Message, new ExchangeRateViewModel());
            }

        }

        public async Task<ServiceResponse<ExchangeRateViewModel>> GetUsdExchangeByDate(DateTime date)
        {
            try
            {
                var rate = _db.UsdExchangeRates.FirstOrDefault(e => e.Date == date);
                if (rate != null)
                {
                    return new SuccessServiceResponse<ExchangeRateViewModel>(new ExchangeRateViewModel()
                    {
                        Date = date.ToString(),
                        ExchangeRate = rate.ExchangeRate
                    }); ;
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        var dateString = date.ToString("yyyyMMdd");
                        var response = await client.GetAsync($"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode=USD&date={dateString}");

                        response.EnsureSuccessStatusCode();

                        string content = await response.Content.ReadAsStringAsync();
                        var exchangeRateViewModel = JsonConvert.DeserializeObject<List<ExchangeRateViewModel>>(content).FirstOrDefault();

                        var exchangeRate = _mapper.Map<UsdExchangeRate>(exchangeRateViewModel);
                        await _db.AddAsync(exchangeRate);
                        await _db.SaveChangesAsync();

                        return new SuccessServiceResponse<ExchangeRateViewModel>(exchangeRateViewModel);
                    }
                }
            }
            catch (Exception)
            {
                return new ErrorServiceResponse<ExchangeRateViewModel>(new ExchangeRateViewModel());
            }
        }

        public async Task<ServiceResponse<float>> UsdToUah(float value, DateTime date)
        {
            var response = await GetUsdExchangeByDate(date);
            if (response.Status == ServiceResponseStatus.Success)
            {
                return new SuccessServiceResponse<float>(value * response.Data.ExchangeRate);
            }
            else
            {
                return new ErrorServiceResponse<float>($"Unavailable to cast usd to uah by {date}");
            }
        }
    }
}
