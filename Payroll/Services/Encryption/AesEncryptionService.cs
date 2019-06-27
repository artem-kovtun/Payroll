using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Payroll.Services.Encryption
{
    public class AesEncryptionService : IAesEncryptionService
    {
        private readonly KeyInfo _info;

        public AesEncryptionService(KeyInfo info = null)
        {
            _info = info;
        }

        public string Encrypt(string source)
        {
            byte[] encryptedBytes;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = _info.Key;
                aesAlg.IV = _info.Iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(source);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string dectyptedSource)
        {
            var ciptherBytes = Convert.FromBase64String(dectyptedSource);

            string plaintext;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = _info.Key;
                aesAlg.IV = _info.Iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(ciptherBytes))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }
}
