using AutoMapper;
using Payroll.Models.ServiceResponses;
using Payroll.Models.Views;
using Payroll.Services.Currency;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class TotalPayValueResolver : IValueResolver<DocCreateFormViewModel, ActGenerationViewModel, float>
    {
        private ICurrencyHandler _currencyHandler { get; set; }

        public TotalPayValueResolver(ICurrencyHandler currencyHandler)
        {
            _currencyHandler = currencyHandler;
        }
 
        public float Resolve(DocCreateFormViewModel source, ActGenerationViewModel destination, float destMember, ResolutionContext context)
        {
            var response = _currencyHandler.GetUsdExchangeByDate(source.WorkCompletionDate).Result;
            if (response.Status == ServiceResponseStatus.Success && source.Services != null && source.Services.Count > 0 && source.CustomUSDRate != null)
            {
                return float.Parse(source.CustomUSDRate, CultureInfo.InvariantCulture) * source.Services.Sum(e => e.Hours) * (float)Math.Round(response.Data.ExchangeRate, 2);
            }
            else
            {
                return default(float);
                //throw new Exception($"Unavailable to get currency rate for {source.WorkCompletionDate.ToString("dd/mm/yyyy")}");
            }
            
        }
    }
}
