using System;
using System.Security.Cryptography;
using System.Text;

namespace Sop.Data.Encryption
{
    /// <summary>
    /// 阿里云OSS的加密工具类
    /// </summary>
    public class OSS
    {
        public static string ComputeSignature(string key, string data)
        {
            using (var algorithm = KeyedHashAlgorithm.Create("HMACSHA1"))
            {
                algorithm.Key = Encoding.UTF8.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }
    }
}
