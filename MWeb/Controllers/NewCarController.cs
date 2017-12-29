using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MWeb.Controllers
{
    public class NewCarController : Controller
    {
        //
        // GET: /NewCar/

        //[OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
