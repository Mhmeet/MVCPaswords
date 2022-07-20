using crypto;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCPaswords.Security;
namespace MVCPaswords
{
    public interface ICipherService
    {
        string Encrypt(string ciphertext);
        string Decrypt(string ciphertext);
    }
    public class sh512 : ICipherService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string Key = "cut-the-night-with-the-light";

        public sh512(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Protect(input);
        }

        public string Decrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(Key);
            return protector.Unprotect(input);
        }
    }
}