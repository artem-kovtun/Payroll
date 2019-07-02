using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payroll.Models.ServiceResponses;
using Payroll.Models.ServiceResponses.Generic;
using Payroll.Models.Views;

namespace Payroll.Services.Currency
{
    public class CurrencyHandler : ICurrencyHandler
    {
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
                                                                                   .OrderByDescending(e => e.Result.Data.Rate)
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
                using (var client = new HttpClient())
                {
                    var dateString = date.ToString("yyyyMMdd");
                    var response = await client.GetAsync($"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode=USD&date={dateString}");

                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();
                    return new SuccessServiceResponse<ExchangeRateViewModel>(JsonConvert.DeserializeObject<List<ExchangeRateViewModel>>(content).FirstOrDefault());
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
                return new SuccessServiceResponse<float>(value * response.Data.Rate);
            }
            else
            {
                return new ErrorServiceResponse<float>($"Unavailable to cast usd to uah by {date}");
            }
        }
    }
}
