using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Proyecto
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public class SessionTimeoutAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                HttpContext ctx = HttpContext.Current;
                if (HttpContext.Current.Session["Logueado"] != null && HttpContext.Current.Session["Usuario"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Logout" } });
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
