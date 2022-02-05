using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.ViewModels
{
    public class MessageBoardViewModel : PageViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 內文
        /// </summary>
        public string Comtent { get; set; }
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatTime { get; set; }
        /// <summary>
        /// 是否已讀
        /// </summary>
        public bool IsRead { get; set; }
        /// <summary>
        /// 已讀時間
        /// </summary>
        public DateTime ReadTime { get; set; }
    }
}
