using System;
using System.Security.Cryptography;
using System.Text;

namespace Sop.Data.Encryption
{
    /// <summary>
    /// 非对称加密(SHA512)
    /// </summary>
    public class SHA
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptStr">明文</param>
        /// <param name="salt">盐</param>
        /// <returns>密文</returns>
        public static string Encrypt(string encryptStr, string salt)
        {
            return Encrypt(Encrypt(encryptStr) + Encrypt(salt));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptStr">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string encryptStr)
        {
            var _hash = new SHA512Managed();

            var bytHash = _hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(encryptStr));

            return Convert.ToBase64String(bytHash);
        }

    }
}
