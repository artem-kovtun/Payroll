using Payroll.Models.ServiceResponses.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.Encryption
{
    public interface IAesEncryptionService
    {
        ServiceResponse<string> Encrypt(string source);

        ServiceResponse<string> Decrypt(string dectyptedSource);
    }
}
