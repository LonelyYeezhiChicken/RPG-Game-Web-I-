using log4net;
using Models.Cash;
using Models.Dtos;
using Models.Entity;
using Models.Models.enums;
using Models.Models.ViewModels;
using Models.Repositorys;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Services.Services
{
    public class MessageBoardService
    {
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IMessageBoardRepository messageBoardrepo;
        public MessageBoardService(IMessageBoardRepository messageBoardrepo)
        {
            this.messageBoardrepo = messageBoardrepo;
        }
        /// <summary>
        /// 寄mail通知
        /// </summary>
        /// <param name="msg"></param>
        private void SendMsg2Email(MessageBoard msg)
        {
            var config = new NameValueCollection();
            config = (NameValueCollection)ConfigurationManager.GetSection("SMPT");
            string userEmail = config["UserEmail"];

            EMailHelper eMail = new EMailHelper();
            SetMail setMail = new SetMail();
            setMail.senderMail = userEmail;
            setMail.sender = userEmail;
            setMail.subject = $"{msg.Title} by {msg.Name}";
            setMail.content = msg.Comtent;
            setMail.recipients = new List<string>() { userEmail };
            eMail.SmtpSeat(setMail);
        }
        /// <summary>
        /// 將訊息更新到Cahe
        /// </summary>
        /// <param name="message"></param>
        private void SaveToCahe(MessageBoardDto message)
        {
            ICaheDao caheDao = new CaheDao();
            try
            {
                if (caheDao.isSet("MessageBoardStatus"))
                {
                    List<MessageBoardStatusViewModel> viewModels = (List<MessageBoardStatusViewModel>)caheDao.Load("MessageBoardStatus");

                    DateTime dateTimeNow = GetHelper.GetDateTimeNowByUTC();
                    TimeSpan timeSpan = dateTimeNow - message.CreatTime;

                    viewModels.Add(new MessageBoardStatusViewModel
                    {
                        id = message.Id,
                        name = message.Name,
                        title = message.Title,
                        hours = Convert.ToInt32(timeSpan.TotalHours)
                    });

                    if (viewModels.Count() > 0)
                    {
                        //存入記憶體
                        caheDao.SaveOrUpadte("MessageBoardStatus", viewModels, ExpirationEnums.Sliding, 5);
                    }
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "SaveToCahe error = " + ex.ToString());
            }
        }

        /// <summary>
        /// 新增訊息
        /// </summary>
        /// <param name="message">訊息物件</param>
        public void CreateMessag(MessageBoardDto message)
        {
            try
            {
                MessageBoard newData = new MessageBoard();
                newData.Name = message.Name;
                newData.Email = message.Email;
                newData.Title = message.Title;
                newData.CreatTime = GetHelper.GetDateTimeNowByUTC();
                newData.IsRead = false;
                newData.IsEnable = true;
                //加密內文
                newData.Comtent = Base64Helper.Base64Encode(message.Comtent);
                SendMsg2Email(newData);
                messageBoardrepo.Create(newData);
                SaveToCahe(message);
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, $"CreateMessag {message.Name} error {ex.ToString()}");
                throw new Exception("訊息發送失敗");
            }
        }
        /// <summary>
        /// 取得信件訊息
        /// </summary>
        /// <param name="isRead">是否已讀</param>
        /// <param name="pageNo">總頁數</param>
        /// <param name="count">取得筆數</param>
        /// <returns></returns>
        public List<MessageBoardStatusViewModel> GetMessageBoardStatusList(bool isRead, int pageNo, int count)
        {
            List<MessageBoardStatusViewModel> viewModels = new List<MessageBoardStatusViewModel>();
            try
            {
                ICaheDao caheDao = new CaheDao();
                //先從記憶體取資料
                if (caheDao.isSet("MessageBoardStatus"))
                {
                    //從記憶體取得
                    viewModels.AddRange((List<MessageBoardStatusViewModel>)caheDao.Load("MessageBoardStatus"));
                }
                else//沒資料從DB撈取
                {
                    //從DB撈完存進記憶體
                    List<MessageBoardDto> messageBoardDtos = messageBoardrepo.LoadByStatus(isRead, pageNo, count);
                    DateTime dateTimeNow = GetHelper.GetDateTimeNowByUTC();
                    foreach (var data in messageBoardDtos)
                    {
                        TimeSpan timeSpan = dateTimeNow - data.CreatTime;
                        viewModels.Add(new MessageBoardStatusViewModel
                        {
                            id = data.Id,
                            name = data.Name,
                            title = data.Title,
                            hours = Convert.ToInt32(timeSpan.TotalHours)
                        });
                    }
                    if (viewModels.Count() > 0)
                    {
                        //存入記憶體
                        caheDao.SaveOrUpadte("MessageBoardStatus", viewModels, ExpirationEnums.Sliding, 5);
                    }
                }

            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "GetMessageBoardList error = " + ex.ToString());
            }
            return viewModels;
        }
    }
}
