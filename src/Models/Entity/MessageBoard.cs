//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class MessageBoard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Comtent { get; set; }
        public System.DateTime CreatTime { get; set; }
        public bool IsRead { get; set; }
        public Nullable<System.DateTime> ReadTime { get; set; }
        public string ReaderId { get; set; }
        public bool IsEnable { get; set; }
    }
}
