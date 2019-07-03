using Payroll.Models.ServiceResponses.Generic;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.Currency
{
    public interface ICurrencyHandler
    {
        Task<ServiceResponse<ExchangeRateViewModel>> GetHighestMonthRate();

        Task<ServiceResponse<ExchangeRateViewModel>> GetUsdExchangeByDate(DateTime date);

        Task<ServiceResponse<float>> UsdToUah(float value, DateTime date);
    }
}
