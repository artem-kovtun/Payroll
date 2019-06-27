using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models
{
    public class Assigner
    {
        public int AssignerId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string FullnameInAblative { get; set; }
        public string Position { get; set; }
        public string PositionInAblative { get; set; }
        public string OperateBasis { get; set; }
    }
}
