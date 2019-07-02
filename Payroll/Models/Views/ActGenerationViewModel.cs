using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.Views
{
    public class ActGenerationViewModel
    {
        public UserProfileViewModel Profile { get; set; }
        public Assigner Assigner { get; set; }
        public DateTime WorkCompletionDate { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public string CustomUSDRate { get; set; }
        public float TotalPay { get; set; }
        public float UsdExchangeRate { get; set; }
    }
}
