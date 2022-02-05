using log4net;
using Models.Entity;
using Models.Models.enums;
using Models.Repositorys;
using Services.Helpers;
using Services.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Models;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: UserManage
        public ActionResult UserManage()
        {
            return View();
        }

        // GET: Logout
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        // GET: ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        // GET: Logout
        public ActionResult ChangePassword(string email)
        {
            ICRUD<AspNetUsers> aspNetRolesRepository = new CRUDRepository<AspNetUsers>();
            var result = aspNetRolesRepository.Load().Where(x => x.Email == email).FirstOrDefault();
            if (result == null)
                return RedirectToAction("Login", "Account");
            var model = new MemberChangePasswordViewModel()
            {
                Email = result.Email,
                UserName = result.UserName,
                Password = null,
                ConfirmPassword = null,
                OldPassword = null
            };
            return View(model);
        }


        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            ICRUD<AspNetUsers> usersRepository = new CRUDRepository<AspNetUsers>();
            IRoles rolesRepository = new UsersRepository();
            UsersService usersService = new UsersService(usersRepository, rolesRepository);
            if (String.IsNullOrEmpty(model.Email) || String.IsNullOrEmpty(model.Password)) { return View(); }

            Log4netHelper.logger(LogEnums.Info, log, $"{model.Email} 登入");
            if (!usersService.IsValid(model.Email, model.Password))
            {
                ModelState.AddModelError(string.Empty, "帳號或密碼有誤");
                return View();
            }
            if (usersService.IsFirstLogin(model.Email))
            {
                return RedirectToAction("ChangePassword", new { email = model.Email });
            }

            if (ModelState.IsValid)
            {
                HttpCookie cookie;
                var returnUrl = usersService.ProcessLogin(model.Email, model.RememberMe, out cookie);
                Response.Cookies.Add(cookie);
                Log4netHelper.logger(LogEnums.Info, log, $"{model.Email} 登入成功");
                return Redirect(returnUrl);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(MemberChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ICRUD<AspNetUsers> usersRepository = new CRUDRepository<AspNetUsers>();
            IRoles rolesRepository = new UsersRepository();
            UsersService usersService = new UsersService(usersRepository, rolesRepository);
            var user = usersService.LoadUser().Where(x => x.Email == model.Email).FirstOrDefault();
            if (user == null)
            {
                // 不顯示使用者不存在
                return RedirectToAction("Login", "Account");
            }
            //檢查舊密碼
            //檢查舊密碼
            if (!usersService.IsValid(model.Email, model.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "舊密碼錯誤！");
                return View(model);
            }

            if (!usersService.ResetPassword(user.Id, model.Password))
            {
                ModelState.AddModelError("ConfirmPassword", "密碼重設失敗請重試");
                return View(model);
            }

            return RedirectToAction("ChangePasswordSuccess");
        }

    }
}