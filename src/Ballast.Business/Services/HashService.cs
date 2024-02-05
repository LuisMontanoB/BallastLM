using System.Security.Cryptography;
using System.Text;
using Ballast.Business.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Ballast.Business.Services
{
    public class HashService : IHashService
    {
        private string hashStringComplement = string.Empty;
        public HashService(IConfiguration configuration)
        {
            hashStringComplement = configuration["HashStringComplement"] ?? throw new ArgumentException("HashStringComplement");
        }

        public string HashPassword(string password)
        {
            return CreateSHA512($"{password}{hashStringComplement}");
        }

        private string CreateSHA512(string strData)
        {
            var message = Encoding.UTF8.GetBytes(strData);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }
    }
}
