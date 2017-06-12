using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.Utils;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.App_Code
{
	public class InterfacePageBase : PageBase
	{

		/// <summary>
		/// 接口调用记录
		/// </summary>
		private static string logInterfaceConten = "IP:{0} URL:{1}";

		protected override void OnLoad(EventArgs e)
		{
			// CommonFunction.WriteInvokeLog(string.Format(logInterfaceConten
			// 	, BitAuto.Utils.WebUtil.GetClientIP(), this.Request.Url.ToString()));
			base.OnLoad(e);
		}

	}
}