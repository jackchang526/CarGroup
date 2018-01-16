using System.Web.Mvc;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream, VaryByParam = "*")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新能源选车首页
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream, VaryByParam = "*")]
        public ActionResult Elec()
        {
            return View();
        }
    }
}
