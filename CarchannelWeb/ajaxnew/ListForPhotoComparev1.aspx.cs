using System;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	public partial class ListForPhotoComparev1 : PageBase
	{
		#region member
		private int type = 0;
		private StringBuilder sb = new StringBuilder();
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			//if (!this.IsPostBack)
			//{
			//	GetPageParam();
			//	GetPhotoListData();
			//	Response.Write(sb.ToString());
			//}
		}

		#region private Method

		///// <summary>
		///// 取参数
		///// </summary>
		//private void GetPageParam()
		//{
		//	string strtype = this.Request.QueryString["type"];
		//	if (!string.IsNullOrEmpty(strtype) && int.TryParse(strtype, out type))
		//	{ }
		//}

		///// <summary>
		///// 取图片对比莫个块
		///// </summary>
		//private void GetPhotoListData()
		//{
		//	Dictionary<int, List<XmlElement>> dic = (new BitAuto.CarChannel.Common.Interface.CommonService()).GetAllSerialForPhotoCompareListNew();
		//	if (dic != null && dic.Count > 0 && dic.ContainsKey(type)
		//		&& type >= 0)
		//	{
		//		List<string> listLeft = new List<string>();

		//		List<string> listHot = new List<string>();
		//		List<string> listLaingXiang = new List<string>();
		//		List<string> listShanXiang = new List<string>();
		//		List<string> listSUV = new List<string>();
		//		List<string> listQiTa = new List<string>();

		//		int loopHot = 1;

		//		#region 循环子品牌
		//		foreach (XmlElement xe in dic[type])
		//		{
		//			int csid = int.Parse(xe.GetAttribute("ID"));
		//			string csName = xe.GetAttribute("Name").Replace("'", "‘").Replace("\"", "“");
		//			string csShowName = xe.GetAttribute("ShowName").Replace("'", "‘").Replace("\"", "“");

		//			// 循环子品牌
		//			if (type < 9)
		//			{
		//				if (loopHot <= 20)
		//				{
		//					// 热门块只显示20个
		//					listHot.Add("<li><a href=\"javascript:selectThisSerial("
		//						+ csid + ",'" + csShowName + "',-1);\">" + csShowName + "</a></li>");
		//				}
		//			}
		//			else
		//			{
		//				listHot.Add("<li><a href=\"javascript:selectThisSerial("
		//						+ csid + ",'" + csShowName + "',-1);\">" + csShowName + "</a></li>");
		//			}

		//			// 两厢
		//			if (xe.GetAttribute("BodyType") == "两厢" && type <= 8)
		//			{
		//				listLaingXiang.Add("<li><a href=\"javascript:selectThisSerial("
		//					+ csid + ",'" + csShowName + "',-1);\">" + csShowName + "</a></li>");
		//			}

		//			// 三厢
		//			if (xe.GetAttribute("BodyType") == "三厢" && type <= 8)
		//			{
		//				listShanXiang.Add("<li><a href=\"javascript:selectThisSerial("
		//					+ csid + ",'" + csShowName + "',-1);\">" + csShowName + "</a></li>");
		//			}

		//			// SUV
		//			if (xe.GetAttribute("CsLevel") == "SUV" && type <= 8)
		//			{
		//				listSUV.Add("<li><a href=\"javascript:selectThisSerial("
		//					+ csid + ",'" + csShowName + "',-1);\">" + csShowName + "</a></li>");
		//			}

		//			// 其他
		//			if (xe.GetAttribute("CsLevel") == "其它" && type <= 8)
		//			{
		//				listQiTa.Add("<li><a href=\"javascript:onclick=\"selectThisSerial("
		//					+ csid + ",'" + csShowName + "',-1);\">" + csShowName + "</a></li>");
		//			}
		//			loopHot++;
		//		}
		//		#endregion

		//		#region 左侧标签
		//		listLeft.Add("<dt><ul id=\"SerialLevelListUL\" class=\"tab2\">");
		//		// 热门
		//		if (listHot.Count > 0)
		//		{
		//			listLeft.Add("<li class=\"current\" id=\"left_0\"><a href=\"javascript:photeCompareLeftSelect(0);\">热门车型</a></li>");
		//		}
		//		// 两厢
		//		if (listLaingXiang.Count > 0)
		//		{
		//			listLeft.Add("<li class=\"\" id=\"left_1\"><a href=\"javascript:photeCompareLeftSelect(1);\">两厢</a></li>");
		//		}
		//		// 三厢
		//		if (listShanXiang.Count > 0)
		//		{
		//			listLeft.Add("<li class=\"\" id=\"left_2\"><a href=\"javascript:photeCompareLeftSelect(2);\">三厢</a></li>");
		//		}
		//		// SUV
		//		if (listSUV.Count > 0)
		//		{
		//			listLeft.Add("<li class=\"\" id=\"left_3\"><a href=\"javascript:photeCompareLeftSelect(3);\">SUV</a></li>");
		//		}
		//		// 其他
		//		if (listQiTa.Count > 0)
		//		{
		//			listLeft.Add("<li class=\"\" id=\"left_4\"><a href=\"javascript:photeCompareLeftSelect(4);\">其它</a></li>");
		//		}
		//		listLeft.Add("</ul></dt>");
		//		#endregion

		//		#region 热门
		//		if (listHot.Count > 0)
		//		{
		//			listHot.Insert(0, "<ul id=\"SerialListForLevelUL_0\" class=\"list\">");
		//			listHot.Add("</ul>");
		//		}
		//		#endregion

		//		#region 两厢
		//		if (listLaingXiang.Count > 0)
		//		{
		//			listLaingXiang.Insert(0, "<ul id=\"SerialListForLevelUL_1\" style=\"display:none;\" class=\"list\">");
		//			listLaingXiang.Add("</ul>");
		//		}
		//		#endregion

		//		#region 三厢
		//		if (listShanXiang.Count > 0)
		//		{
		//			listShanXiang.Insert(0, "<ul id=\"SerialListForLevelUL_2\" style=\"display:none;\" class=\"list\">");
		//			listShanXiang.Add("</ul>");
		//		}
		//		#endregion

		//		#region SUV
		//		if (listSUV.Count > 0)
		//		{
		//			listSUV.Insert(0, "<ul id=\"SerialListForLevelUL_3\" style=\"display:none;\" class=\"list\">");
		//			listSUV.Add("</ul>");
		//		}
		//		#endregion

		//		#region 其他
		//		if (listQiTa.Count > 0)
		//		{
		//			listQiTa.Insert(0, "<ul id=\"SerialListForLevelUL_4\" style=\"display:none;\" class=\"list\">");
		//			listQiTa.Add("</ul>");
		//		}
		//		#endregion

		//		sb.AppendLine("<dl>");
		//		sb.AppendLine(string.Concat(listLeft.ToArray()));
		//		sb.AppendLine("<dd>");
		//		// 热门
		//		if (listHot.Count > 0)
		//		{
		//			sb.AppendLine(string.Concat(listHot.ToArray()));
		//		}
		//		// 两厢
		//		if (listLaingXiang.Count > 0)
		//		{
		//			sb.AppendLine(string.Concat(listLaingXiang.ToArray()));
		//		}
		//		// 三厢
		//		if (listShanXiang.Count > 0)
		//		{
		//			sb.AppendLine(string.Concat(listShanXiang.ToArray()));
		//		}
		//		// SUV
		//		if (listSUV.Count > 0)
		//		{
		//			sb.AppendLine(string.Concat(listSUV.ToArray()));
		//		}
		//		// 其他
		//		if (listQiTa.Count > 0)
		//		{
		//			sb.AppendLine(string.Concat(listQiTa.ToArray()));
		//		}
		//		sb.AppendLine("</dl><div class=\"clear\"></div>");

		//	}
		//}

		#endregion

	}
}