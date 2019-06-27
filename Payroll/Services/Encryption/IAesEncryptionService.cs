using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.Encryption
{
    public interface IAesEncryptionService
    {
        string Encrypt(string source);

        string Decrypt(string dectyptedSource);
    }
}
