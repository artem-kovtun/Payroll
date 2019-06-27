using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.ServiceResponses
{
    public class ServiceResponse
    {
        public ServiceResponseStatus Status { get; set; }
        public string Message { get; set; }
    }
}
