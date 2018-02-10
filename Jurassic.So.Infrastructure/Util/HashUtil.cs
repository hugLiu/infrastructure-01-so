using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Jurassic.So.Infrastructure
{
    /// <summary>Hash工具</summary>
    public static class HashUtil
    {
        /// <summary>生成MD5</summary>
        public static string ToMD5(this string data)
        {
            var md5 = new MD5CryptoServiceProvider();
            var buffer = Encoding.UTF8.GetBytes(data);
            var hash = md5.ComputeHash(buffer);
            return hash.ToHexString();
        }
    }
}
