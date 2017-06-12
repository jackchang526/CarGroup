using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// TreeXml 的摘要说明
	/// </summary>
	public class TreeXml : InterfacePageBase, IHttpHandler
	{
		private string _TagType = String.Empty;
		private TreeData _TreeData;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "Text/XML";
			GetParam(context);
			context.Response.Write(InitData());
		}

		/// <summary>
		/// 得到页面参数
		/// </summary>
		/// <param name="context"></param>
		private void GetParam(HttpContext context)
		{
			_TagType = string.IsNullOrEmpty(context.Request.QueryString["tagtype"])
						? "" : context.Request.QueryString["tagtype"].ToString();
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <returns></returns>
		private string InitData()
		{
			if (string.IsNullOrEmpty(_TagType))
			{
				return "";
			}
			_TreeData = new TreeFactory().GetTreeDataObject(_TagType);

			return _TreeData.TreeXmlData();
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