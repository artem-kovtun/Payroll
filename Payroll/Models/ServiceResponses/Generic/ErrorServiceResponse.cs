using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models.ServiceResponses.Generic
{
    public class ErrorServiceResponse<T> : ServiceResponse<T> 
    {
        public ErrorServiceResponse()
        {
            Status = ServiceResponseStatus.Error;
        }

        public ErrorServiceResponse(string message) : this()
        {
            Message = message;
        }

        public ErrorServiceResponse(string message, T data) : this(message)
        {
            Data = data;
        }

        public ErrorServiceResponse(T data) : this()
        {
            Data = data;
        }
    }
}
