using System.Web.Mvc;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class SUVChannelController : Controller
    {
        /// <summary>
        /// SUV频道
        /// </summary>
        /// <param name="ver"></param>
        /// <returns></returns>
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream, VaryByParam = "*")]
        public ActionResult Index(string ver)
        {
            ViewBag.ver = ver;
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream, VaryByParam = "*")]
        public ActionResult List(int id)
        {
            ViewBag.channelId = id;
            return View();
        }
    }
}
