using Payroll.Models.ServiceResponses;
using Payroll.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.Authorization
{
    public interface IAuthorizationService
    {
        Task<ServiceResponse> AddNewUserAsync(AuthorizationViewModel authorizationData);
        Task<ServiceResponse> IsExistedUserAsync(AuthorizationViewModel authorizationData);

    }
}
