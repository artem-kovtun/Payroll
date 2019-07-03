using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.ServiceResponses.Generic
{
    public class ServiceResponse<T> 
    {
        public ServiceResponseStatus Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
