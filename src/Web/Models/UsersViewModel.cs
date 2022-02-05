using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UsersViewModel
    {
        /// <summary>
        /// uuid
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 信箱驗證
        /// </summary>
        public bool EmailConfirmed { get; set; }
        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 安全碼
        /// </summary>
        public string SecurityStamp { get; set; }
        /// <summary>
        /// 電話號碼
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 電話驗證
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }
        /// <summary>
        /// 開啟雙重驗證
        /// </summary>
        public bool TwoFactorEnabled { get; set; }
        /// <summary>
        /// 鎖定結束日期
        /// </summary>
        public DateTime LockoutEndDateUtc { get; set; }
        /// <summary>
        /// 鎖定
        /// </summary>
        public bool LockoutEnabled { get; set; }
        /// <summary>
        /// 登入失敗次數
        /// </summary>
        public int AccessFailedCount { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 使用者權限
        /// </summary>
        public List<UserRolesViewModel> UserRolesList = new List<UserRolesViewModel>();
    }

}