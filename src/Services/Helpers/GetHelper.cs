using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class GetHelper
    {
        /// <summary>
        /// 取得guid
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 取得現在時間+8
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDateTimeNowByUTC()
        {
            return DateTime.UtcNow.AddHours(08);
        }
    }
}
