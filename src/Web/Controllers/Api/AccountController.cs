using System.Web.Http;

namespace Web.Controllers.Api
{
    public class AccountController : ApiController
    {
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Login(string UserName, string Mema, bool IsAdmin)
        //{
        //    ICRUD usersRepository = new UsersRepository();
        //    IRoles rolesRepository = new UsersRepository();
        //    UsersService usersService = new UsersService(usersRepository, rolesRepository);


        //    if (!usersService.IsValid(UserName, Mema))
        //    {
        //        ModelState.AddModelError(string.Empty, "帳號或密碼有誤");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        HttpCookie cookie;
        //        var returnUrl = biz.ProcessLogin(formData.Account, false, out cookie);

        //        Response.Cookies.Add(cookie);

        //        return Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
    }
}
