using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	/// <summary>
	/// GetTagsAllSerialInfoNum 的摘要说明
	/// </summary>
	public class GetTagsAllSerialInfoNum : InterfacePageBase, IHttpHandler
	{

		public override void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(15);
			context.Response.ContentType = "Text/XML";
			string numFile = Path.Combine(WebConfig.DataBlockPath, "data\\cartree\\TagNum.xml");

			if (File.Exists(numFile))
				context.Response.WriteFile(numFile);
			else
				context.Response.Write("<SerialList></SerialList>");
		}
	}
}