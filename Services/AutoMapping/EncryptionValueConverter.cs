using AutoMapper;
using Payroll.Models.ServiceResponses;
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
            var response = _aesEnctyption.Encrypt(sourceMember.ToString());
            if (response.Status == ServiceResponseStatus.Success)
            {
                return response.Data;
            }
            else
            {
                throw new Exception(); // DataEncrytionException
            }
        }
    }
}
