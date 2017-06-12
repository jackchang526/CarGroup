using BitAuto.CarChannel.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MWeb.RouteConstraint
{
    public class BrandAllSpellConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
			string allspell = values["allspell"].ToString();
			if (string.IsNullOrEmpty(allspell))
				return false;

			Dictionary<string, int> brandDic = new MVCRouteBll().GetAllBrandDic();
			if (brandDic == null || brandDic.Count == 0)
				return false;

			if (brandDic.ContainsKey(allspell))
			{
				values["id"] = brandDic[allspell];
				return true;
			}
			return false;
        }
    }
}