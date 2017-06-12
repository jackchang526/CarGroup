using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MWeb.ViewEngines
{
    public class CustomRazorViewEngine : RazorViewEngine
    {
        public CustomRazorViewEngine()
            : base()
        {
            ////Area视图路径其中{2},{1},{0}分别代表Area名，Controller名，Action名 
            //AreaViewLocationFormats = new[] { "~/{2}/{1}/{0}.cshtml", "~/Shared/{0}.cshtml" };
            ////Area模版路径 
            //AreaMasterLocationFormats = new[] { "~/Shared/{0}.cshtml" };
            ////Area的分部视图路径 
            //AreaPartialViewLocationFormats = new[] { "~/{2}/{1}/{0}.cshtml", "~/Shared/{0}.cshtml" };
            ////主视图路径 
            //ViewLocationFormats = new[] { "~/{1}/{0}.cshtml", "~/Shared/{0}.cshtml" };
            ////主模版路径 
            //MasterLocationFormats = new[] { "~/Shared/{0}.cshtml" };
            ////主分部视图路径 
            //PartialViewLocationFormats = new[] { "~/{1}/{0}.cshtml", "~/Shared/{0}.cshtml" };
            ViewLocationFormats = new[]      {  
				"~/Views/{1}/{0}.cshtml",  
				"~/Views/Shared/{0}.cshtml"
			};
            PartialViewLocationFormats = new[]   {  
						"~/Views/{1}/{0}.cshtml", 
						"~/Views/Shared/{0}.cshtml" 
					};

            FileExtensions = new[] { "cshtml" };

        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

    }
}