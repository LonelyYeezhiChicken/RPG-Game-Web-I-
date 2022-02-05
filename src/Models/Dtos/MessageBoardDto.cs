using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
    public class MessageBoardDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        private string _Name;
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name
        {
            get => _Name;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入名稱");
                _Name = value;
            }
        }

        private string _Email;
        /// <summary>
        /// 信箱
        /// </summary>
        public string Email
        {
            get => _Email;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入信箱");
                if (new EmailAddressAttribute().IsValid(value) == false)
                    throw new AccessViolationException("信箱格式不正確");
                _Email = value;
            }
        }

        private string _Title;
        /// <summary>
        /// 標題
        /// </summary>
        public string Title
        {
            get => _Title;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入標題");
                if (value.Length > 50)
                    throw new AccessViolationException("標題不可超過50個字");
                _Title = value;
            }
        }

        private string _Comtent;
        /// <summary>
        /// 內文
        /// </summary>
        public string Comtent
        {
            get => _Comtent;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入內文");
                if (value.Length > 500)
                    throw new AccessViolationException("內文不可超過500個字");
                _Comtent = value;
            }
        }
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
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }
    }
}
