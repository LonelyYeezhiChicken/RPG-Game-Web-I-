using System;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            log4net.Config.XmlConfigurator.Configure();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            if (!Request.IsAuthenticated)
            {
                return;
            }

            //取得FormsIdentity
            var id = (FormsIdentity)User.Identity;
            //再取出認證票
            FormsAuthenticationTicket ticket = id.Ticket;

            //將存在認證票裡的userData取出
            string roles = ticket.UserData; //"admin, ,editor"
            string[] arrRoles =
                roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            IPrincipal currentUser = new GenericPrincipal(User.Identity, arrRoles);

            //HttpContext.Current.User = currentUser;
            //User = currentUser; //error
            Context.User = currentUser;
        }
    }
}
