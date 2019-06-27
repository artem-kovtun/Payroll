using Payroll.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.Views
{
    public class AuthorizationViewModel
    {
        private string HashedPassword;

        public string Username { get; set; }
        public string Password
        {
            get => HashedPassword;
            set => HashedPassword = value.SHA256Hash();
        }
    }
}
