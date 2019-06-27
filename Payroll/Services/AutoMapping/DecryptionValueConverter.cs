using AutoMapper;
using Payroll.Services.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class DecryptionValueConverter : IValueConverter<string,string>
    {
        private IAesEncryptionService _aesEnctyption { get; set; }

        public DecryptionValueConverter(IAesEncryptionService aes)
        {
            _aesEnctyption = aes;
        }

        public string Convert(string sourceMember, ResolutionContext context)
        {
            return _aesEnctyption.Decrypt(sourceMember.ToString());
        }
    }
}
