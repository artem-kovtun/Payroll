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
        [DataMember(Name = "txt")]
        public string Txt { get; set; }

        [DataMember(Name = "rate")]
        public float Rate { get; set; }

        [DataMember(Name = "cc")]
        public string Currency { get; set; }

        [DataMember(Name ="exchangedate")]
        public string ExchangeDate { get; set; }
    }

}
