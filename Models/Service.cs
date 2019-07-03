using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Hours { get; set; }  
        public bool IsFinished { get; set; }
        public virtual ICollection<DocumentService> Documents { get; set; }

    }
}
