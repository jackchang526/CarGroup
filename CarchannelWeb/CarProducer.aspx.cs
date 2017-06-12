using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Data;
using System.Text;
using System.Xml;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb
{
	public partial class CarProducer : PageBase
	{
		protected string strCPName = string.Empty;
		protected string strCPSeoName = string.Empty;
		protected string strCpShortName = string.Empty;
		protected string strCPIntroduc = string.Empty;
		protected string strCPIntroducPop = string.Empty;
		protected string strMetaIntroduce = string.Empty;

		protected string strCp_InfoTop = string.Empty;
		protected string strCp_Info = string.Empty;
		protected string strCp_Introduction = string.Empty;

		protected string strCBPhotoListHTML = string.Empty;
		protected string strNewsHtml = string.Empty;
		protected int CPID;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				base.SetPageCache(10);
				GetParams();

				if (-1 != CPID)
				{
					Car_ProducerEntity carProducer = GetCarProducerByCPID(CPID);
					if (null != carProducer)
					{
						strCPName = carProducer.Cp_Name;
						strCPSeoName = carProducer.Cp_seoName;
						strCpShortName = carProducer.Cp_ShortName;

						//strCp_InfoTop = string.Format("<img alt='' src='http://img1.bitauto.com/bt/car/default/images/carimage/p_{0}_b30.jpg' /><span>{1}</span>", CPID, carProducer.Cp_Name);
						strCp_InfoTop = string.Format("<span>{0}</span>", carProducer.Cp_Name);

						CommonFunction commonFunction = new CommonFunction();

						strCPIntroduc = carProducer.Cp_Introduction;
						strCPIntroducPop = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + carProducer.Cp_Introduction.Trim().Replace("\r\n", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;").Replace(" ", "&nbsp;&nbsp;");
						if (strCPIntroduc.Length > 100)
							strMetaIntroduce = strCPIntroduc.Substring(0, 100);
						else
							strMetaIntroduce = strCPIntroduc;
						strCp_Introduction = commonFunction.DropHTML(carProducer.Cp_Introduction);
						string strCp_ShortIntroduction = commonFunction.GetShortString(strCp_Introduction);
						strCp_ShortIntroduction = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + strCp_ShortIntroduction.Replace("\r\n", "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						string strCpConnectInfo = "<span>{0}</span>";

						if (!string.IsNullOrEmpty(carProducer.Cp_Url) && !string.IsNullOrEmpty(carProducer.Cp_Phone))
						{
							strCpConnectInfo = string.Format(strCpConnectInfo, "<a target='_blank' href='" + carProducer.Cp_Url + "'>官方网站</a> | 联系电话：" + carProducer.Cp_Phone);
						}
						else if (!string.IsNullOrEmpty(carProducer.Cp_Url) && string.IsNullOrEmpty(carProducer.Cp_Phone))
						{
							strCpConnectInfo = string.Format(strCpConnectInfo, "<a target='_blank' href='" + carProducer.Cp_Url + "'>官方网站</a>");
						}
						else if (string.IsNullOrEmpty(carProducer.Cp_Url) && !string.IsNullOrEmpty(carProducer.Cp_Phone))
						{
							strCpConnectInfo = string.Format(strCpConnectInfo, "联系电话：" + carProducer.Cp_Phone);
						}
						else
							strCpConnectInfo = string.Format(strCpConnectInfo, "");
						if (strCp_ShortIntroduction != strCp_Introduction)
							strCp_Info = string.Format("{0}  <a href=\"javascript:popload('aa')\" style=\"font-family: 宋体\">[详细]</a> {1}", strCp_ShortIntroduction, strCpConnectInfo);
						else
							strCp_Info = strCp_ShortIntroduction + strCpConnectInfo;

						strCBPhotoListHTML = GetRenderedCBPhotoListHTMLByCPID(CPID);
					}
					////主品牌新闻
					strNewsHtml = RenderNewsList();
				}
			}
		}

		private void GetParams()
		{
			if (null != Request.QueryString["CPID"])
			{
				if (!string.IsNullOrEmpty(Request.QueryString["CPID"]))
				{
					CPID = Convert.ToInt32(Request.QueryString["CPID"]);
				}
			}
		}

		/// <summary>
		/// 取得当前汽车厂商品牌列表页面显示HTML
		/// </summary>
		/// <param name="nCPID"></param>
		/// <returns></returns>
		private string GetRenderedCBPhotoListHTMLByCPID(int nCPID)
		{
			string cacheKey = "serial-carproducer-cblist-" + nCPID;
			object cbPhotoListHTML = null;
			CacheManager.GetCachedData(cacheKey, out cbPhotoListHTML);
			if (null == cbPhotoListHTML)
			{
				cbPhotoListHTML = RenderCBPhotoList(nCPID);
				CacheManager.InsertCache(cacheKey, cbPhotoListHTML, WebConfig.CachedDuration);
			}

			return (string)cbPhotoListHTML;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private string RenderCBPhotoList(int nCPID)
		{
			StringBuilder strRenderedHtml = new StringBuilder();

			List<Car_BrandEntity> carBrandList = new Car_ProducerBll().GetCarBrandListByCPID(nCPID);
			List<int> notDisplayBrandList = new List<int>();
			//		10016 东风汽车-东风
			//      20055 北京奔驰-北奔三菱
			//		20011 通用-大宇
			//		20133 西安奥拓
			notDisplayBrandList.AddRange(new int[] { 10016, 20055, 20133, 20011 });

			foreach (Car_BrandEntity carBrand in carBrandList)
			{
				if (notDisplayBrandList.Contains(carBrand.Cb_Id))
					continue;
				strRenderedHtml.AppendLine(string.Format(@"<li><a href='{0}' target='_blank'><img src='http://img1.bitauto.com/bt/car/default/images/carimage/b_{1}_b100.jpg' alt='{2}'/></a><a href='{0}' target='_blank'>{2}</a></li>",
												"http://news.bitauto.com/pinpai/" + carBrand.Cb_AllSpell + "/",
												carBrand.Cb_Id,
												carBrand.Cb_Name));

			}
			return strRenderedHtml.ToString();
		}

		/// <summary>
		/// 取得当前汽车厂商信息
		/// </summary>
		/// <param name="nCPID"></param>
		/// <returns></returns>
		private Car_ProducerEntity GetCarProducerByCPID(int nCPID)
		{
			string cacheKey = "serial-carproducer-list-" + nCPID;
			object carProducer = null;
			CacheManager.GetCachedData(cacheKey, out carProducer);
			if (null == carProducer)
			{
				carProducer = (object)new Car_ProducerBll().GetCarProducerByCPID(CPID);
				if (null != carProducer)
				{
					CacheManager.InsertCache(cacheKey, carProducer, WebConfig.CachedDuration);
				}
			}

			return (Car_ProducerEntity)carProducer;
		}

		/// <summary>
		/// 生成新闻列表
		/// </summary>
		private string RenderNewsList()
		{
			StringBuilder htmlCode = new StringBuilder();
			DataSet ds = new CarNewsBll().GetTopProducerNews(CPID, BitAuto.CarChannel.Common.Enum.CarNewsType.xinwen, 20);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRowCollection rows = ds.Tables[0].Rows;
				htmlCode.AppendLine("<div class=\"col-all\">");
				htmlCode.AppendLine("<div class=\"line_box mainlist_box all_newslist\">");
				if (rows.Count >= 20)
				{
					htmlCode.AppendLine("<div class=\"more\"><a href=\"http://car.bitauto.com/producer/" + CPID + "xinwen.html\" target=\"_blank\">更多>></a></div>");
					htmlCode.AppendLine("<h3><span><a href=\"http://car.bitauto.com/producer/" + CPID + "xinwen.html\" target=\"_blank\">" + strCpShortName + "企业新闻</a></span></h3>");
				}
				else
					htmlCode.AppendLine("<h3><span class=\"s1\"><span class=\"caption\">" + strCpShortName + "企业新闻</span></span></h3>");

				htmlCode.AppendLine("<div id=\"newslist\" class=\"list_date\">");
				int counter = 0;

				foreach (DataRow row in rows)
				{
					counter++;
					string newsTitle = HttpUtility.HtmlDecode(row["title"].ToString());
					//过滤Html标签
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					//string shortNewsTitle = StringHelper.SubString(newsTitle, 34, false);
					string filePath = row["filepath"].ToString();
					string newsDate = Convert.ToDateTime(row["publishtime"]).ToString("yyyy-MM-dd");
					int position = counter % 5;
					if (position == 1)
						htmlCode.AppendLine("<ul>");
					htmlCode.AppendLine("<li><a href=\"" + filePath + "\" target=\"_blank\">" + newsTitle + "</a><small>" + newsDate + "</small></li>");
					if (position == 0 || counter == rows.Count)
						htmlCode.AppendLine("</ul>");
				}
				htmlCode.AppendLine("</div></div><div class=\"clear\"></div></div>");
			}
			return htmlCode.ToString();
		}
	}
}