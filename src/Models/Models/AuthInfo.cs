using System.Collections.Generic;

namespace Models.Models
{
    public class AuthInfo
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 角色列表，可以用於記錄該使用者的角色
        /// </summary>
        public List<string> Roles { get; set; }
        /// <summary>
        /// 是否是管理員
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}