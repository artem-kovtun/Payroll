using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Payroll.Models.Views;

namespace Payroll.Services.Currency
{
    public class CurrencyHandler : ICurrencyHandler
    {
        public async Task<ExchangeRateViewModel> GetHighestMonthRate()
        {
            var resultList = new List<Task<ExchangeRateViewModel>>();
            var currentDate = DateTime.Now;

            for (int i = 1; i <= currentDate.Day; i++)
            {
                var date = new DateTime(currentDate.Year, currentDate.Month, i);
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                resultList.Add(GetUsdExchange(date));
            }

            await Task.WhenAll(resultList);

            return resultList.OrderByDescending(e => e.Result.Rate).First().Result;
        }

        private async Task<ExchangeRateViewModel> GetUsdExchange(DateTime date)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var dateString = date.ToString("yyyyMMdd");
                    var response = await client.GetAsync($"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json&valcode=USD&date={dateString}");

                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ExchangeRateViewModel>>(content).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return new ExchangeRateViewModel();
            }
        }

    }
}
