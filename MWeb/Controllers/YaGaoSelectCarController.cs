using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class YaGaoSelectCarController : Controller
    {
        //
        // GET: /YaGaoSelectCar/

		/// <summary>
		/// 牙膏选车
		/// </summary>
		/// <returns></returns>
		[OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
