using BitAuto.CarChannel.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MWeb.RouteConstraint
{
    public class MasterAllSpellConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
			string allspell = values["allspell"].ToString();
			if (string.IsNullOrEmpty(allspell))
				return false;

			Dictionary<string, int> masterbrandDic = new MVCRouteBll().GetAllMasterBrandDic();
			if (masterbrandDic == null || masterbrandDic.Count == 0)
				return false;

			if (masterbrandDic.ContainsKey(allspell))
			{
				values["id"] = masterbrandDic[allspell];
				return true;
			}
			return false;
        }
    }
}