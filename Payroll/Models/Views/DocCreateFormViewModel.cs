using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.Views
{
    public class DocCreateFormViewModel
    {
        public DateTime WorkCompletionDate { get; set; }
        public string CustomUSDRate {get; set;}
        public List<ServiceViewModel> Services { get; set; } 
        public Assigner Assigner { get; set; }
    }
}
