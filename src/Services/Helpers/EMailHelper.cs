using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class EMailHelper
    {
        private static bool IsInit = true;
        private static string UserEmail;
        private static string Password;
        private static string Host;
        private static int Port;

        public EMailHelper()
        {
            if (IsInit)
            {
                var config = new NameValueCollection();
                config = (NameValueCollection)ConfigurationManager.GetSection("SMPT");
                UserEmail = config["UserEmail"];
                Password = config["Password"];
                Host = config["Host"];
                Port = Convert.ToInt32(config["Port"]);
                IsInit = false;
            }
        }

        /// <summary>
        /// 寄送郵件
        /// </summary>
        /// <param name="setMail"></param>
        public void SmtpSeat(SetMail setMail)
        {
            try
            {
                MailMessage msg = new MailMessage();
                //收件人
                foreach (var rc in setMail.recipients)
                {
                    msg.To.Add(rc);
                }
                //cc
                foreach (var cc in setMail.ccs)
                {
                    msg.CC.Add(cc);
                }
                //寄件人,地址，發件人姓名，編碼
                msg.From = new MailAddress(setMail.senderMail, setMail.sender, System.Text.Encoding.UTF8);
                //標題
                msg.Subject = setMail.subject;
                //郵件標題編碼
                msg.SubjectEncoding = Encoding.UTF8;
                //郵件內容
                msg.Body = setMail.content;
                //郵件內容編碼 
                msg.BodyEncoding = Encoding.UTF8;
                //附件
                if (String.IsNullOrEmpty(setMail.attachmentsPath) == false)
                    msg.Attachments.Add(new Attachment(setMail.attachmentsPath));
                //是否是HTML郵件 
                msg.IsBodyHtml = true;
                //以非同步方式寄送
                Task.Factory.StartNew(() => SmtpSend(msg));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 開始寄送
        /// </summary>
        /// <param name="msg"></param>
        private void SmtpSend(MailMessage msg)
        {
            SmtpClient client = new SmtpClient();
            //這裡要填正確的帳號跟密碼
            client.Credentials = new System.Net.NetworkCredential(UserEmail, Password);
            //設定smtp Server
            client.Host = Host;
            //設定Port
            client.Port = Port;
            //gmail預設開啟驗證
            client.EnableSsl = true;
            client.Send(msg); //寄出信件
            client.Dispose();
            msg.Dispose();
        }
    }

    public class SetMail
    {
        private List<string> _recipients = new List<string>();
        /// <summary>
        /// 收件人
        /// </summary>
        public List<string> recipients
        {
            get => _recipients;
            set
            {
                if (value.Count() <= 0)
                    throw new AccessViolationException("請輸入收件人");
                foreach (var item in _recipients)
                {
                    if (new EmailAddressAttribute().IsValid(value) == false)
                        throw new AccessViolationException("收件人信箱格式不正確");
                }
                _recipients = new List<string>();
                _recipients.AddRange(value);
            }
        }

        private List<string> _ccs = new List<string>();
        /// <summary>
        /// 附件
        /// </summary>
        public List<string> ccs
        {
            get => _ccs;
            set
            {
                _ccs = new List<string>();
                if (value.Count() > 0)
                {
                    foreach (var item in _ccs)
                    {
                        if (new EmailAddressAttribute().IsValid(value) == false)
                            throw new AccessViolationException("附件信箱格式不正確");
                    }
                    _ccs.AddRange(value);
                }
            }
        }

        private string _sender;
        /// <summary>
        /// 寄件人
        /// </summary>
        public string sender
        {
            get => _sender;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入寄件人");
                _sender = value;
            }
        }

        private string _senderMail;
        /// <summary>
        /// 寄件人信箱
        /// </summary>
        public string senderMail
        {
            get => _senderMail;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入寄件人信箱");
                if (new EmailAddressAttribute().IsValid(value) == false)
                    throw new AccessViolationException("寄件人信箱格式不正確");
                _senderMail = value;
            }
        }

        private string _subject;
        /// <summary>
        /// 主旨
        /// </summary>
        public string subject
        {
            get => _subject;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new AccessViolationException("請輸入主旨");
                _subject = value;
            }
        }

        /// <summary>
        /// 內文
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string attachmentsPath { get; set; }
    }
}
