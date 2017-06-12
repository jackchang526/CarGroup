using System.Web;
using System.Web.Mvc;
using MWeb.Filters;

namespace MWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilterAttribute());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}