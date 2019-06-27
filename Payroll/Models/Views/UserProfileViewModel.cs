using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.Views
{
    public class UserProfileViewModel
    {
        [Required()]
        public string USDRate { get; set; }
        [Required()]
        public string Firstname { get; set; }
        [Required()]
        public string Lastname { get; set; }
        [Required()]
        public string Middlename { get; set; }
        [Required()]
        public string FullnameInAblative { get; set; }
        [Required()]
        public string ContractNumber { get; set; }
        [Required()]
        public string ContractDate { get; set; }
        [Required()]
        public string AddressIndex { get; set; }
        [Required()]
        public string AddressStreet { get; set; }
        [Required()]
        public string AccountNumber { get; set; }
        [Required()]
        public string RecipientBank { get; set; }
        [Required()]
        public string RegisterNumber { get; set; }
        [Required()]
        public string IBAN { get; set; }
        [Required()]
        public string PaymentPurpose { get; set; }
        [Required()]
        public string VAT { get; set; }
    }
}
