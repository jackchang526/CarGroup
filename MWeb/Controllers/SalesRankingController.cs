using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace MWeb.Controllers
{
    public class SalesRankingController : Controller
    {
        //
        // GET: /SalesRanking/ 
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index()
        {
            return View();
        }

    }
}
