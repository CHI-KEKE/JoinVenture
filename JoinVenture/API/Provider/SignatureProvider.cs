using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Provider
{
    public static class SignatureProvider
    {
        public static string HMACSHA256(string key, string message)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            byte[] keyBytes = encoding.GetBytes(key);

            HMACSHA256 ThisKeyHmacSHA256 = new HMACSHA256(keyBytes);

            byte[] messageBytes = encoding.GetBytes(message);

            byte[] hashedMessage = ThisKeyHmacSHA256.ComputeHash(messageBytes);

            return Convert.ToBase64String(hashedMessage);
        }
    }
}