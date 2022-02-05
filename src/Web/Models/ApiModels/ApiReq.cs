using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.ApiModels
{
    public class ApiReq
    {
        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 結果
        /// </summary>
        public string msg { get; set; }
    }
}