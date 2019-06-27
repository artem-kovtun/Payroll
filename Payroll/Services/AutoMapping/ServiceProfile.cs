using AutoMapper;
using Payroll.Models;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, ServiceViewModel>();
            CreateMap<ServiceViewModel, Service>();
        }
    }
}
