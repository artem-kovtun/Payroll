using AutoMapper;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class ActGenerationProfile : Profile
    {
        public ActGenerationProfile()
        {
            CreateMap<DocCreateFormViewModel, ActGenerationViewModel>().ForMember(dest => dest.Services, options => options.MapFrom(source => source.Services.Where(service => service.IsChecked)))
                                                                       .ForMember(dest => dest.TotalPay, options => options.MapFrom<TotalPayValueResolver>());
        }
    }
}
