using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models
{
    public class DocumentService
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public int DocumentId { get; set; }
        public Document Document { get; set; }
    }
}
