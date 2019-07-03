using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.ServiceResponses.Generic
{
    public class SuccessServiceResponse<T> : ServiceResponse<T> 
    {
        public SuccessServiceResponse()
        {
            Status = ServiceResponseStatus.Success;
        }

        public SuccessServiceResponse(string message) : this()
        {
            Message = message;
        }
       
        public SuccessServiceResponse(string message, T data) : this(message)
        {
            Data = data;
        }

        public SuccessServiceResponse(T data) : this()
        {
            Data = data;
        }
    }
}
