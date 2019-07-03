using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.Views
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime WorkCompletionDate { get; set; }
        public AssignerViewModel Assigner { get; set; }

        public List<ServiceViewModel> Services { get; set; }
    }
}
