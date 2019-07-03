using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime WorkCompletionDate { get; set; }
        public User Creator { get; set; }
        public Assigner Assigner { get; set; }

        public virtual ICollection<DocumentService> Services { get; set; }
    }
}
