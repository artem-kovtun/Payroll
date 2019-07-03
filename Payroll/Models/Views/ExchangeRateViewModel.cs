using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Payroll.Models.Views
{
    [DataContract]
    public class ExchangeRateViewModel
    {
        [DataMember(Name = "rate")]
        public float ExchangeRate { get; set; }

        [DataMember(Name ="exchangedate")]
        public string Date { get; set; }
    }

}
