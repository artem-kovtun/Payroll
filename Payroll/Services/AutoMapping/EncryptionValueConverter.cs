using AutoMapper;
using Payroll.Models.Views;
using Payroll.Services.Encryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Services.AutoMapping
{
    public class EncryptionValueConverter : IValueConverter<string, string>
    {
        private IAesEncryptionService _aesEnctyption { get; set; }

        public EncryptionValueConverter(IAesEncryptionService aes)
        {
            _aesEnctyption = aes;
        }

        public string Convert(string sourceMember, ResolutionContext context)
        {
            return _aesEnctyption.Encrypt(sourceMember.ToString());
        }
    }
}
