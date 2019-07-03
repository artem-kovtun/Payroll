using Payroll.Services.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models
{
    public class UserProfile
    {
        public User User { get; set; }
        public string USDRate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Middlename { get; set;}
        public string FullnameInAblative { get; set; }
        public string ContractNumber { get; set; }
        public string ContractDate { get; set; } 
        public string AddressIndex { get; set; }
        public string AddressStreet { get; set; }
        public string AccountNumber { get; set; }
        public string RecipientBank { get; set; }
        public string RegisterNumber { get; set; }
        public string IBAN { get; set; }
        public string PaymentPurpose { get; set; }
        public string VAT { get; set; }

    }
}
