﻿using System.Security.Cryptography;
using System.Text;

namespace Ionta.OSC.App.Services.HashingPassword
{
    public static class HashingPasswordService 
    {
        public static string Hash(string password)
        {
            SHA512 shaM = new SHA512Managed();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            var result = shaM.ComputeHash(bytes);

            var hashedInputStringBuilder = new System.Text.StringBuilder(128);
            foreach (var b in result)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
}
