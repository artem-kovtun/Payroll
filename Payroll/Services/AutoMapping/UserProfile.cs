using AutoMapper;
using Payroll.Models.Views;
using Payroll.Services.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {      
            CreateMap<LoginViewModel, AuthorizationViewModel>();
            CreateMap<SignupViewModel, AuthorizationViewModel>();

            CreateMap<Models.UserProfile, UserProfileViewModel>()
                .ForAllMembers(options => options.ConvertUsing<DecryptionValueConverter, string>());
            CreateMap<UserProfileViewModel, Models.UserProfile>()
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForAllMembers(options => options.ConvertUsing<EncryptionValueConverter, string>());

        }
    }
}
