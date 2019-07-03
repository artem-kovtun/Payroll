using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Payroll.Models;
using Payroll.Models.ServiceResponses;
using Payroll.Models.Views;


namespace Payroll.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private PayrollDbContext Database { get; set; }

        public AuthorizationService(PayrollDbContext context)
        {
            Database = context;
        }

        public async Task<ServiceResponse> IsExistedUserAsync(AuthorizationViewModel authorizationData)
        {
            var user = await Database.Users.FirstOrDefaultAsync(u => u.Username == authorizationData.Username && u.Password == authorizationData.Password);

            if (user != null)
            {
                return new SuccessServiceResponse();
            }
            else
            {
                return new ErrorServiceResponse("Невірне ім'я користувача/пароль");
            }
        }

        public async Task<ServiceResponse> AddNewUserAsync(AuthorizationViewModel authorizationData)
        {
            try
            {
                if (await IsUsedUsername(authorizationData.Username))
                {
                    return new ErrorServiceResponse("Ім'я користувача вже існує в системі");
                }

                var user = new User()
                {
                    Username = authorizationData.Username,
                    Password = authorizationData.Password
                };

                await Database.Users.AddAsync(user);
                await Database.SaveChangesAsync();

                return new SuccessServiceResponse();
                
            }
            catch(Exception e)
            {
                return new ErrorServiceResponse(e.Message);
            }

        }

        private async Task<bool> IsUsedUsername(string username)
        {
            return await Database.Users.FirstOrDefaultAsync(user => user.Username == username) != null;
        }
    }
}


