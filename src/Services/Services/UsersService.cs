using log4net;
using Models.Entity;
using Models.Models.enums;
using Models.Repositorys;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Services.Services
{
    public class UsersService
    {
        private ILog log;
        private ICRUD<AspNetUsers> usersRepository;
        private IRoles rolesRepository;
        public UsersService(ICRUD<AspNetUsers> ICRUDRepository, IRoles rolesRepository)
        {
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            usersRepository = ICRUDRepository;
            this.rolesRepository = rolesRepository;
        }
        //私用方法
        /// <summary>
        /// 使用者檢查資料
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        private void AspNetUsersDataCheck(string userId, string userEmail, string userName, string password, string[] userRoleId, bool isEdit)
        {
            if (String.IsNullOrEmpty(userEmail)) { throw new Exception("請輸入Email"); }
            if (String.IsNullOrEmpty(userName)) { throw new Exception("請輸入使用者名稱"); }
            if (String.IsNullOrEmpty(password) && !isEdit) { throw new Exception("請輸入密碼"); }
            if (userRoleId.Count() <= 0) { throw new Exception("請選擇角色"); }
            if (isEdit)
            {
                if (String.IsNullOrEmpty(userId)) { throw new Exception("請選擇角色"); }
            }
            else
            {
                if (usersRepository.Read(x => string.Compare(x.Email, userEmail, true) == 0).Any()) { throw new Exception("此信箱已經申請過"); }
            }
        }
        /// <summary>
        /// 權限角色檢查資料
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <param name="userRole"></param>
        /// <param name=""></param>
        /// <param name="isEdit"></param>
        private void AspNetUserRoleCheck(string userRoleId, string userRole, bool isEdit)
        {
            if (String.IsNullOrEmpty(userRole)) { throw new Exception("請輸入角色"); }
            if (isEdit)
            {
                if (String.IsNullOrEmpty(userRoleId)) { throw new Exception("請選擇角色"); }
            }
            else
            {
                if (rolesRepository.CheckUserRole(userRole)) { throw new Exception("角色已經存在"); }
            }
        }
        //登入
        /// <summary>
        /// 是否已經通過驗證
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool IsValid(string email, string password)
        {
            var item = usersRepository.Read(x => x.LockoutEnabled == false
                                                 && x.Email == email).ToList();
            if (item.Count <= 0) { return false; }
            return HashedHelper.VerifyHashedPassword(item[0].PasswordHash, password);
        }
        /// <summary>
        /// 是否為第一次登入
        /// </summary>
        /// <param name="Email">使用者信箱</param>
        /// <returns></returns>
        public bool IsFirstLogin(string email)
        {
            var item = usersRepository.Read(x => String.IsNullOrEmpty(x.SecurityStamp)
                                               && x.Email == email).ToList();
            if (item.Count <= 0) { return false; }
            return true;
        }
        /// <summary>
        /// 製作登入票證
        /// </summary>
        /// <param name="account"></param>
        /// <param name="rememberMe"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public string ProcessLogin(string email, bool rememberMe, out HttpCookie cookie)
        {
            var userInfo = rolesRepository.GetUserRoles<AspNetUsers>(email).ToList()[0];
            string roles = userInfo.AspNetRoles.FirstOrDefault().Name;

            //建立一張認證票
            FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(
                        1,          //版本別, 沒特別用處
                        $"{userInfo.UserName},{userInfo.Email}",//權限
                        DateTime.Now,   //發行日
                        DateTime.Now.AddDays(2), //到期日
                        rememberMe,     //是否續存
                        roles,          //userdata
                        "/"             //cookie位置
                    );
            //將它加密
            string value = FormsAuthentication.Encrypt(ticket);
            //存入cookie
            cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            //取得return url
            string url = FormsAuthentication.GetRedirectUrl(email, true); //第二個引數沒有用處

            return url;

        }
        //使用者
        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="userEmail">信箱</param>
        /// <param name="userName">使用者名稱</param>
        /// <param name="password">密碼</param>
        /// <param name="userRoleId">權限角色</param>
        public void AddUser(string userEmail, string userName, string password, string phone, string[] userRoleId)
        {
            //驗證資料           
            AspNetUsersDataCheck("", userEmail, userName, password, userRoleId, false);
            try
            {
                AspNetUsers netUsers = new AspNetUsers();
                netUsers.Id = GetHelper.GetGuid();
                netUsers.Email = userEmail;
                netUsers.EmailConfirmed = false;
                netUsers.UserName = userName;
                netUsers.PhoneNumber = phone;
                netUsers.PasswordHash = HashedHelper.HashPassword(password);
                netUsers.TwoFactorEnabled = false;
                netUsers.PhoneNumberConfirmed = false;
                netUsers.LockoutEnabled = false;
                netUsers.AccessFailedCount = 0;
                rolesRepository.AddUser(netUsers, userRoleId);
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "AddUser error = " + ex.ToString());
                throw new Exception($"新增使用者:{userName}失敗!");
            }
        }
        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="userId">uuid</param>
        /// <param name="userEmail">信箱</param>
        /// <param name="userName">使用者名稱</param>
        /// <param name="userRoleId">權限角色</param>
        public void EditUser(string userId, string userEmail, string userName, string phoneNumber, string[] userRoleId)
        {
            //驗證資料
            AspNetUsersDataCheck(userId, userEmail, userName, "", userRoleId, true);
            try
            {
                var user = usersRepository.Read(x => x.Id == userId).ToList()[0];
                AspNetUsers netUsers = new AspNetUsers();
                netUsers.Id = user.Id;
                netUsers.Email = userEmail;
                netUsers.EmailConfirmed = user.EmailConfirmed;
                netUsers.PasswordHash = user.PasswordHash;
                netUsers.SecurityStamp = user.SecurityStamp;
                netUsers.PhoneNumber = phoneNumber;
                netUsers.LockoutEndDateUtc = user.LockoutEndDateUtc;
                netUsers.UserName = userName;
                netUsers.TwoFactorEnabled = user.TwoFactorEnabled;
                netUsers.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                netUsers.LockoutEnabled = user.LockoutEnabled;
                netUsers.AccessFailedCount = user.AccessFailedCount;

                rolesRepository.EditUser(netUsers, userRoleId);
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "EditUser error = " + ex.ToString());
                throw new Exception($"編輯使用者:{userName}失敗!");
            }
        }
        /// <summary>
        /// 取得所有使用者
        /// </summary>
        /// <returns></returns>
        public List<AspNetUsers> LoadUser()
        {
            return rolesRepository.Load().ToList();
        }
        /// <summary>
        /// 取得所有使用者
        /// </summary>
        /// <param name="email">信箱</param>
        /// <returns></returns>
        public AspNetUsers LoadUserByEmail(string email)
        {
            return rolesRepository.LoadUserByEmail(email);
        }
        //權限角色
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="userRole">角色名稱</param>
        public void AddUserRole(string userRole)
        {
            AspNetUserRoleCheck("", userRole, false);
            try
            {
                AspNetRoles netRoles = new AspNetRoles();
                netRoles.Id = GetHelper.GetGuid();
                netRoles.Name = userRole;
                rolesRepository.AddUserRole(netRoles);
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "AddUserRole error = " + ex.ToString());
                throw new Exception($"新增角色:{userRole}失敗!");
            }
        }
        /// <summary>
        /// 編輯角色
        /// </summary>
        /// <param name="userRoleId">角色uuid</param>
        /// <param name="userRole">角色名稱</param>
        public void EditUsreRole(string userRoleId, string userRole)
        {
            AspNetUserRoleCheck(userRoleId, userRole, false);
            try
            {
                AspNetRoles netRoles = new AspNetRoles();
                netRoles.Id = userRoleId;
                netRoles.Name = userRole;
                rolesRepository.EditUserRole(netRoles);
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "EditUsreRole error = " + ex.ToString());
                throw new Exception($"編輯角色:{userRole}失敗!");
            }
        }
        //編輯密碼
        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ResetPassword(string uuid, string password)
        {
            bool _req = false;

            try
            {
                rolesRepository.ResetPasword(uuid, HashedHelper.HashPassword(password), GetHelper.GetGuid());
                _req = true;
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, $"ResetPassword = {ex.ToString()}");
            }
            return _req;
        }
    }
}
