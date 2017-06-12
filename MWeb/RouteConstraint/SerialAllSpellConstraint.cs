using BitAuto.CarChannel.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MWeb.RouteConstraint
{
    public class SerialAllSpellConstraint : IRouteConstraint
    {

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string allspell = values["allspell"].ToString();
            if (string.IsNullOrEmpty(allspell))
                return false;

			Dictionary<string, int> serialDic = new MVCRouteBll().GetAllSerialDic();
			if (serialDic == null || serialDic.Count == 0)
				return false;

			if (serialDic.ContainsKey(allspell))
			{
				values["id"] = serialDic[allspell];
				return true;
			}
			return false;
        }
    }
}