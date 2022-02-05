using log4net;
using Models.Models.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class Log4netHelper
    {
        /// <summary>
        /// 使用非同步方式寫log
        /// </summary>
        /// <param name="logEnums"></param>
        /// <param name="className"></param>
        /// <param name="msg"></param>
        public static void logger(LogEnums logEnums, ILog ilog, object msg)
        {
            ILog log = ilog;
            switch (logEnums)
            {
                case LogEnums.Info:
                    Task.Factory.StartNew(() => log.Info(msg));
                    break;
                case LogEnums.Debug:
                    Task.Factory.StartNew(() => log.Debug(msg));
                    break;
                case LogEnums.Fatal:
                    Task.Factory.StartNew(() => log.Fatal(msg));
                    break;
                case LogEnums.Error:
                    Task.Factory.StartNew(() => log.Error(msg));
                    break;
            }
        }
    }
}
