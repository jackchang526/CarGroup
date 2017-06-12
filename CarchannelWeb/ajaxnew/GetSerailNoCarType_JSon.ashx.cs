using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.DAL;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetSerailNoCarType_JSon 的摘要说明
	/// </summary>
	public class GetSerailNoCarType_JSon : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write(GetIsNoContainsCarTypeOfSerialJson());
		}

		/// <summary>
		/// 得到不包含车型的子品牌JSON对象
		/// </summary>
		/// <returns></returns>
		private string GetIsNoContainsCarTypeOfSerialJson()
		{
			List<int> serialList = new Car_SerialBll().GetIsNoContainsCarTypeSerialList();
			StringBuilder serialStrBuilder = new StringBuilder("var IsNoContainsTypeSerial=");

			if (serialList == null || serialList.Count < 1)
			{
				return (serialStrBuilder.Append("[]")).ToString();
			}

			serialStrBuilder.Append("[");
			//子品牌列表
			for (int i = 0; i < serialList.Count; i++)
			{
				serialStrBuilder.Append("{");
				serialStrBuilder.AppendFormat("\"id\":\"{0}\"", serialList[i].ToString());
				if (i + 1 == serialList.Count)
				{
					serialStrBuilder.Append("}");
					continue;
				}
				serialStrBuilder.Append("},");
			}
			serialStrBuilder.Append("]");

			return serialStrBuilder.ToString();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}