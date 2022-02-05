using log4net;
using Models.Dtos;
using Models.Models.enums;
using Models.Models.ViewModels;
using Models.Repositorys;
using Services.Helpers;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Models.ApiModels;

namespace Web.Controllers.Api
{
    public class MessageBoardController : ApiController
    {
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IMessageBoardRepository messageBoardrepo = new MessageBoardRepository();
        private MessageBoardService messageBoardService { get; set; }
        /// <summary>
        /// 傳送訊息
        /// </summary>
        /// <returns></returns>
        [Route("api/MessageBoard/CreatMessage/")]
        [HttpPost]
        public ApiReq CreatMessage(MessageBoardViewModel messageData)
        {
            ApiReq req = new ApiReq() { Code = HttpStatusCode.OK.ToString(), msg = "新增成功" };
            try
            {
                MessageBoardDto message = new MessageBoardDto();
                message.Name = messageData.Name;
                message.Email = messageData.Email;
                message.Title = messageData.Title;
                message.Comtent = messageData.Comtent;

                messageBoardService = new MessageBoardService(messageBoardrepo);
                messageBoardService.CreateMessag(message);
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "CreatMessage error:" + ex.ToString());
                req.Code = HttpStatusCode.BadRequest.ToString();
                req.msg = ex.Message;
                return req;
            }
            return req;
        }

        /// <summary>
        /// 訊息提示
        /// </summary>
        /// <returns></returns>
        [Route("api/MessageBoard/MessageStatus/")]
        [HttpGet]
        [Authorize(Roles = "God")]
        public List<MessageBoardStatusViewModel> MessageStatus()
        {
            List<MessageBoardStatusViewModel> req = new List<MessageBoardStatusViewModel>();
            try
            {
                messageBoardService = new MessageBoardService(messageBoardrepo);
                req = messageBoardService.GetMessageBoardStatusList(false, 1, 7);
            }
            catch (Exception ex)
            {
                log.Error("MessageStatus error:" + ex.ToString());
            }
            return req;
        }
    }
}
