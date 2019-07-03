using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.ServiceResponses
{
    public class SuccessServiceResponse : ServiceResponse
    {
        public SuccessServiceResponse()
        {
            Status = ServiceResponseStatus.Success;
        } 

        public SuccessServiceResponse(string message) : this()
        {
            Message = message;
        }

    }
}
