using log4net;
using Models.Entity;
using Models.Models.enums;
using Models.Repositorys;
using Services.Helpers;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Web.Models;
using Web.Models.ApiModels;

namespace Web.Controllers.Api
{
    [Authorize]
    public class AdminController : ApiController
    {
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ICRUD<AspNetUsers> usersRepository = new CRUDRepository<AspNetUsers>();
        private IRoles rolesRepository = new UsersRepository();
        private UsersService usersService { get; set; }

        /// <summary>
        /// 載入權限角色
        /// </summary>
        /// <returns></returns>
        [Route("api/Admin/LoadUserRoles/")]
        [HttpGet]
        [Authorize(Roles = "God,Administrator")]
        public List<UserRolesViewModel> LoadUserRoles()
        {
            List<UserRolesViewModel> viewModel = new List<UserRolesViewModel>();
            try
            {
                ICRUD<AspNetRoles> aspNetRolesRepository = new CRUDRepository<AspNetRoles>();
                var result = aspNetRolesRepository.Load().Where(x => x.Name != "God").ToList();
                foreach (var item in result)
                {
                    viewModel.Add(new UserRolesViewModel
                    {
                        Id = item.Id,
                        Name = item.Name
                    });
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "LoadUserRoles error:" + ex.ToString());
            }
            return viewModel;
        }

        /// <summary>
        /// 載入使用者
        /// </summary>
        /// <returns></returns>
        [Route("api/Admin/LoadUsers/")]
        [HttpGet]
        [Authorize(Roles = "God,Administrator")]
        public List<UsersViewModel> LoadUsers()
        {
            List<UsersViewModel> viewModel = new List<UsersViewModel>();
            try
            {
                usersService = new UsersService(usersRepository, rolesRepository);
                var result = usersService.LoadUser();
                foreach (var item in result)
                {
                    var user = new UsersViewModel();
                    user.Id = item.Id;
                    user.Email = item.Email;
                    user.EmailConfirmed = item.EmailConfirmed;
                    user.SecurityStamp = item.SecurityStamp;
                    user.PhoneNumber = item.PhoneNumber;
                    user.PhoneNumberConfirmed = item.PhoneNumberConfirmed;
                    user.LockoutEnabled = item.LockoutEnabled;
                    user.AccessFailedCount = item.AccessFailedCount;
                    user.UserName = item.UserName;
                    foreach (var role in item.AspNetRoles)
                    {
                        user.UserRolesList.Add(new UserRolesViewModel()
                        {
                            Id = role.Id,
                            Name = role.Name
                        });
                    }

                    viewModel.Add(user);
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "LoadUserRoles error:" + ex.ToString());
            }
            return viewModel;
        }

        /// <summary>
        /// 載入登入的使用者資訊
        /// </summary>
        /// <returns></returns>
        [Route("api/Admin/LoadUserByLoginUser/")]
        [HttpGet]
        public UsersViewModel LoadUserByLoginUser()
        {
            UsersViewModel viewModel = new UsersViewModel();
            try
            {
                var Identity = (System.Web.Security.FormsIdentity)System.Web.HttpContext.Current.User.Identity;
                var userEmail = Identity.Ticket.Name.Split(',')[1];
                usersService = new UsersService(usersRepository, rolesRepository);
                var result = usersService.LoadUserByEmail(userEmail);
                viewModel.Id = result.Id;
                viewModel.Email = result.Email;
                viewModel.EmailConfirmed = result.EmailConfirmed;
                viewModel.SecurityStamp = result.SecurityStamp;
                viewModel.PhoneNumber = result.PhoneNumber;
                viewModel.PhoneNumberConfirmed = result.PhoneNumberConfirmed;
                viewModel.LockoutEnabled = result.LockoutEnabled;
                viewModel.AccessFailedCount = result.AccessFailedCount;
                viewModel.UserName = result.UserName;
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "LoadUserRoles error:" + ex.ToString());
            }
            return viewModel;
        }

        /// <summary>
        /// 新增或編輯權限角色
        /// </summary>
        /// <param name="UserRolesData"></param>
        /// <returns></returns>
        [Route("api/Admin/AddOrEditUserRole")]
        [HttpPost]
        [Authorize(Roles = "God,Administrator")]
        public ApiReq AddOrEditUserRole([FromBody]UserRolesViewModel UserRolesData)
        {
            ApiReq req = new ApiReq() { Code = HttpStatusCode.OK.ToString(), msg = "新增成功" };
            try
            {
                usersService = new UsersService(usersRepository, rolesRepository);
                if (String.IsNullOrEmpty(UserRolesData.Id))
                {
                    usersService.AddUserRole(UserRolesData.Name);
                }
                else
                {
                    usersService.EditUsreRole(UserRolesData.Id, UserRolesData.Name);
                }
                return req;
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "AddUserRole error:" + ex.ToString());
                req.Code = HttpStatusCode.BadRequest.ToString();
                req.msg = ex.Message;
                return req;
            }
        }

        /// <summary>
        /// 新增或編輯使用者
        /// </summary>
        /// <param name="UsersData"></param>
        /// <returns></returns>
        [Route("api/Admin/AddOrEditUser")]
        [HttpPost]
        public ApiReq AddOrEditUser([FromBody]UsersViewModel UsersData)
        {
            ApiReq req = new ApiReq() { Code = HttpStatusCode.OK.ToString(), msg = "新增成功" };
            try
            {
                List<string> userRoleId = new List<string>();
                foreach (var item in UsersData.UserRolesList)
                {
                    userRoleId.Add(item.Id.ToString());
                }
                usersService = new UsersService(usersRepository, rolesRepository);
                if (String.IsNullOrEmpty(UsersData.Id))
                {
                    usersService.AddUser(UsersData.Email, UsersData.UserName, UsersData.Password, UsersData.PhoneNumber, userRoleId.ToArray());
                }
                else
                {
                    usersService.EditUser(UsersData.Id, UsersData.Email, UsersData.UserName, UsersData.PhoneNumber, userRoleId.ToArray());
                }
                return req;
            }
            catch (Exception ex)
            {
                Log4netHelper.logger(LogEnums.Error, log, "AddUserRole error:" + ex.ToString());
                req.Code = HttpStatusCode.BadRequest.ToString();
                req.msg = ex.Message;
                return req;
            }
        }
    }
}
