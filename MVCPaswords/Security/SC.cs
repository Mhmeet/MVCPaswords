using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVCPaswords.Security
{
    public static class security
    {
        public static string SHA512Crypto(string s)
        {
            try
            {
                if (s != "")
                {
                    SHA512 sHA512 = new SHA512CryptoServiceProvider();
                    return Convert.ToBase64String(sHA512.ComputeHash(Encoding.UTF8.GetBytes(s)));
                }
                else
                    return "TANIMLANAMADI";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
    }
}