using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class SelectCarOfSceneController : Controller
    {
        //
        // GET: /SelectCarOfScene/
        [OutputCache(Duration = 1800, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
