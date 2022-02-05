using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class Base64Helper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="AStr">輸入字串</param>
        /// <returns></returns>
        public static string Base64Encode(string AStr)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(AStr));
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ABase64">需解密文字</param>
        /// <returns></returns>
        public static string Base64Decode(string ABase64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(ABase64));
        }
    }
}
