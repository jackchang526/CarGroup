using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.CarTree
{
	public partial class CX_Error : TreePageBase
	{
		protected string _BrandUrl = string.Empty;
		private string _TagType = "chexing";
		private string _MainUrl = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			InitData();
			NavbarHtml = base.GetTreeNavBarHtml("home", "chexing", 0);
			//搜索地址
			this._SearchUrl = InitSearchUrl("chexing");
			//生成条件Html
			this.MakeConditionsHtml("按条件选车", false, false);
		}
		/// <summary>
		/// 绑定页面数据
		/// </summary>
		private void InitData()
		{
			InitUrl();
		}
		/// <summary>
		/// 初始化页面的URL
		/// </summary>
		private void InitUrl()
		{
			if (_TTCEntityList == null
				|| _TTCEntityList.Count < 1
				|| !_TTCEntityList.ContainsKey(_TagType))
			{
				return;
			}

			Dictionary<string, string> urlList = _TTCEntityList[_TagType].ErrorUrl;

			if (urlList == null
				|| urlList.Count < 1
				|| !urlList.ContainsKey("sourceurl"))
			{
				return;
			}

			_SourceUrl = urlList["sourceurl"];
			urlList = _TTCEntityList["chexing"].MainUrl;
			if (urlList == null
				|| urlList.Count < 1
				|| !urlList.ContainsKey("borderurl"))
			{
				return;
			}

			_MainUrl = urlList["borderurl"];
		}
		/// <summary>
		/// 得到页面导航条
		/// </summary>
		/// <returns></returns>
		protected string GetPageGuilder()
		{
			StringBuilder guilderString = new StringBuilder();
			guilderString.AppendLine("<div class=\"breadcrumbs\">");
			guilderString.AppendLine("<a href=\"http://www.bitauto.com/\" target=\"_blank\">易车</a> &gt; ");
			guilderString.AppendLine(string.Format("<a href=\"{0}\" target=\"_top\">车型</a> &gt; ", string.IsNullOrEmpty(_MainUrl) ? "#" : _MainUrl));
			guilderString.AppendLine("车型未找到");
			guilderString.AppendLine("</div>");
			return guilderString.ToString();
		}
	}
}