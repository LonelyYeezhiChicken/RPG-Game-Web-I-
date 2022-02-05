using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.ViewModels
{
    public class MessageBoardStatusViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; } 
        /// <summary>
        /// 傳送者名稱
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 訊息標題
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 訊息標題
        /// </summary>
        public int hours { get; set; }
    }
}
