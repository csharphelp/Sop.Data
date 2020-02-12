using System;
using System.Text;

namespace Sop.Data.Tests.Utlity
{

    public class StringHelper
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="useNum"></param>
        /// <param name="useLow"></param>
        /// <param name="useUpp"></param>
        /// <param name="useSpe"></param>
        /// <param name="custom"></param>
        /// <returns></returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSymbol, string custom = "")
        {
            byte[] b = new byte[4];

            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum) { str += "0123456789"; }
            if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSymbol) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            return GetRandomString(length, true, true, true, false);
        }

   
       

    }
}
