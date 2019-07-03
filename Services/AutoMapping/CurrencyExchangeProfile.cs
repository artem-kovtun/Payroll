using AutoMapper;
using Payroll.Models;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class CurrencyExchangeProfile : Profile
    {
        public CurrencyExchangeProfile()
        {
            CreateMap<UsdExchangeRate, ExchangeRateViewModel>();
            CreateMap<ExchangeRateViewModel, UsdExchangeRate>();
        }
    }
}
