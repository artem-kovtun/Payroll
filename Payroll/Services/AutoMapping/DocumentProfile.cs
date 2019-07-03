using AutoMapper;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Models.Document, DocumentViewModel>().ForMember(dest => dest.Services, options => options.Ignore());
            CreateMap<DocumentViewModel, Models.Document>().ForMember(dest => dest.Services, options => options.Ignore());
        }
    }
}
