using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{

    [Authorize(Roles = "God,Administrator")]
    public class AdminController : Controller
    {
        // GET: UserRoleSet
        [Authorize(Roles = "God")]
        public ActionResult UserRoleSet()
        {
            return View();
        }

        // GET: UserSet          
        public ActionResult UserSet()
        {
            return View();
        }
    }
}