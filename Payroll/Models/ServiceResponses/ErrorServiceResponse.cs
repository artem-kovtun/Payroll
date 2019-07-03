using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.ServiceResponses
{
    public class ErrorServiceResponse : ServiceResponse
    {
        public ErrorServiceResponse()
        {
            Status = ServiceResponseStatus.Error;
        }

        public ErrorServiceResponse(string message) : this()
        {
            Message = message;
        }
    }
}
