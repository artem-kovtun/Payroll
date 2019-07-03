using AutoMapper;
using Payroll.Models;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class AssignerProfile : Profile
    {
        public AssignerProfile()
        {
            CreateMap<AssignerViewModel, Assigner>();
            CreateMap<Assigner, AssignerViewModel>();
        }
    }
}
