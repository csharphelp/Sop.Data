using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sop.Data.Encryption
{
    /// <summary>
    /// AES对称加密(256位)
    /// </summary>
    public class Aes
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptStr">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string encryptStr, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);

            var aes = new AesManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 256;
            aes.Key = keyArray.Take(32).ToArray();

            var resultArray = aes.CreateEncryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptStr">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string decryptStr, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(decryptStr);

            var aes = new AesManaged();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = 256;
            aes.Key = keyArray.Take(32).ToArray();

            byte[] resultArray = aes.CreateDecryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }


       
    }
}
