using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.PageLevelV2
{
	/// <summary>
	/// SUV频道
	/// </summary>
	public partial class SuvChannel : PageBase
	{
		private Car_SerialBll _serialBLL;

		protected string suvXinWenHtml = string.Empty;
		protected string suvPingCeHtml = string.Empty;
		protected string suvDaoGouHtml = string.Empty;
		protected string suvHotHtml = string.Empty;
		protected string suvHotNewsHtml = string.Empty;
		protected string suvSalesRankHtml = string.Empty;
		protected string suvSalesUpdateDate = string.Empty;
		protected int levelClassId = 0;
		protected string level = "SUV";
		private int pageSize = 6;
		protected CarNewsType _CarNewsType;

		public SuvChannel()
		{
			_serialBLL = new Car_SerialBll();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10, true);
			//新闻区
			RenderNews();
			//热门SUV
			RenderHotSuv();
			//热门文章
			RenderHotNews();
			//销量排行
			RenderSuvSalesRank();
		}

		/// <summary>
		/// 生成新闻区内容
		/// </summary>
		private void RenderNews()
		{
			levelClassId = CarLevelDefine.GetLevelClassIdById(8);
			suvXinWenHtml = RenderNewsListNew(levelClassId, CarNewsType.xinwen);
			suvPingCeHtml = RenderNewsListNew(levelClassId, CarNewsType.treepingce);
			suvDaoGouHtml = RenderNewsListNew(levelClassId, CarNewsType.daogou);
		}

		/// <summary>
		/// 生成具体新闻块数据
		/// </summary>
		/// <param name="levelClassId"></param>
		/// <param name="newsType"></param>
		/// <returns></returns>
		private string RenderNewsListNew(int levelClassId, CarNewsType newsType)
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<ul id=\"ul_themeList\">");
			DataSet newsDs = new CarNewsBll().GetLevelNewsWithComment(levelClassId, newsType, pageSize);
			if (newsDs == null || newsDs.Tables.Count < 1 || newsDs.Tables[0].Rows.Count < 1)
			{
				return string.Empty;
			}

			Car_SerialBll serialBll = new Car_SerialBll();
			DataRowCollection rows = newsDs.Tables[0].Rows;

			foreach (DataRow row in rows)
			{
				int newsId = ConvertHelper.GetInteger(row["CmsNewsId"]);
				string newsTitle = CommonFunction.NewsTitleDecode(row["title"].ToString());
				string newsUrl = row["filepath"].ToString();
				DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
				string author = Convert.ToString(row["author"]);
				if (!string.IsNullOrEmpty(author))
				{
					author = "作者：" + author + "&nbsp; &nbsp;";
				}
				int commentNum = row["CommentNum"] == DBNull.Value ? 0 : Convert.ToInt32(row["CommentNum"]);
				string picUrl = ConvertHelper.GetString(row["Picture"]);
				string firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);
				string pic = string.Empty;
				if (!string.IsNullOrEmpty(picUrl) && !picUrl.Contains("/not"))
				{
					pic = picUrl;
				}
				else if (!string.IsNullOrEmpty(firstPicUrl))
				{
					pic = firstPicUrl.Replace("/bitauto/", "/newsimg_300_w0_1/bitauto/")
						.Replace("/autoalbum/", "/newsimg_300_w0_1/autoalbum/");
				}
				else
				{
					pic = WebConfig.DefaultCarPic;
				}

				#region 子品牌链接
				//string serialHtml = string.Empty;
				//if (_CarNewsType == CarNewsType.daogou || _CarNewsType == CarNewsType.hangqing)
				//{
				//    int serialId = ConvertHelper.GetInteger(row["SerialId"]);
				//    if (serialId > 0)
				//    {
				//        Car_SerialEntity sInfo = serialBll.GetSerialInfoEntity(serialId);
				//        if (sInfo != null)
				//        {
				//            serialHtml = "<a href=\"/" + sInfo.Cs_AllSpell.Trim().ToLower() + "/\" target=\"_blank\">[" + sInfo.Cs_ShowName + "]</a>";
				//        }
				//    }
				//}
				#endregion
              
				htmlCode.AppendFormat("<li><div class=\"img-info-layout-vertical img-info-layout-vertical-240160\"><div class=\"img\"><a href=\"{0}\"target=\"_blank\" class=\"img\"><img src=\"{1}\"></a></div>", newsUrl, pic);
                htmlCode.AppendFormat("<ul class=\"p-list\"><li class=\"name\"><a href=\"{0}\">{1}</a></li></ul>", newsUrl, newsTitle);
                htmlCode.Append("<div class=\"info\">");
                htmlCode.AppendFormat("<div> <span class=\"view\" data-vnewsid=\"{0}\"></span> <span class=\"comment\" data-cnewsid=\"{0}\">{1}</span></div>", newsId, commentNum);
                htmlCode.AppendFormat("<div><span class=\"author\"><em>{0}</em></span><span class=\"time\">{1}</span></div>", author, publishTime.ToString("yyyy-MM-dd"));
                htmlCode.Append("</div>");
                htmlCode.Append("</div>");
                htmlCode.Append("</li>");
			}

			htmlCode.AppendLine("</ul>");
			return htmlCode.ToString();
		}

		/// <summary>
		/// 生成热门SUV数据
		/// </summary>
		private void RenderHotSuv()
		{
			DataSet ds = _serialBLL.GetLevelSerialDataByUV(level);
			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
				return;
			//add by sk ad 2015.06.05
			var serialList = ds.Tables[0].AsEnumerable().ToList();
			List<SerialListADEntity> serialAdList = _serialBLL.GetSuvChannelAdData();
			if (serialAdList != null && serialAdList.Any())
			{
				foreach (SerialListADEntity serialAd in serialAdList)
				{
					int index = serialAd.Pos - 1;
					if (index < 0)
						index = 0;
					DataRow dr = serialList.Find((row) => { return ConvertHelper.GetInteger(row["cs_id"]) == serialAd.SerialId; });
					if (dr != null)
					{
						serialList.Remove(dr);
						serialList.Insert(index, dr);
					}
				}
			}


			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<ul>");
			int SerialNum = 0;
			foreach (DataRow row in serialList.Take(8))
			{
				if (SerialNum > 7)
				{
					break;
				}
				int serialId = Convert.ToInt32(row["cs_id"]);
				string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
				string serialName = "";
				serialName = Convert.ToString(row["cs_ShowName"]);
				string serialSpell = Convert.ToString(row["csAllSpell"]);
				string serialUrl = "/" + serialSpell + "/";

				string priceRange = new PageBase().GetSerialPriceRangeByID(serialId);
				if (priceRange.Trim().Length == 0)
					priceRange = "暂无报价";
                htmlCode.Append("<li>");
                htmlCode.Append("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                htmlCode.AppendFormat("<div class=\"img\"><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"/></a></div>", serialUrl, imgUrl.Replace("_2.", "_6."));
                htmlCode.Append("<ul class=\"p-list\">");
                htmlCode.AppendFormat("<li class=\"name\"><a href=\"{0}\"  target=\"_blank\">{1}</a></li>", serialUrl, serialName);
                htmlCode.AppendFormat("<li class=\"price\"><a href=\"{0}\"  target=\"_blank\">{1}</a></li>", serialUrl, priceRange);
                htmlCode.Append("</ul>");
                htmlCode.Append("</div>");
                htmlCode.Append("</li>");
				SerialNum++;
			}
			htmlCode.AppendLine("</ul>");
			suvHotHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 生成热门新闻
		/// </summary>
		private void RenderHotNews()
		{
			levelClassId = CarLevelDefine.GetLevelClassIdById(8);
			List<int> carTypeIdList = new List<int>() 
			{
			    (int)CarNewsType.daogou,
                (int)CarNewsType.treepingce
			};
			DataSet newsDs = new CarNewsBll().GetLevelNewsWithComment(levelClassId, carTypeIdList, 8);
			if (newsDs == null || newsDs.Tables.Count < 1 || newsDs.Tables[0].Rows.Count < 1)
			{
				return;
			}

			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<ul>");
			foreach (DataRow row in newsDs.Tables[0].Rows)
			{
				string newsTitle = CommonFunction.NewsTitleDecode(row["FaceTitle"].ToString());
				string newsUrl = row["filepath"].ToString();
				htmlCode.AppendFormat("<li><a href=\"{0}\"target=\"_blank\">{1}</a></li>", newsUrl, newsTitle);
			}

			htmlCode.AppendLine("</ul>");
			suvHotNewsHtml = htmlCode.ToString();
		}

		/// <summary>
		/// 生成SUV销量排行
		/// </summary>
		private void RenderSuvSalesRank()
		{
			Car_SerialBll serialBll = new Car_SerialBll();
			Dictionary<int, Dictionary<int, string[]>> dicSuvSalesRank = serialBll.GetEPSUVSalesRank();
			if (dicSuvSalesRank != null && dicSuvSalesRank.Count < 1)
			{
				return;
			}
         
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.AppendLine("<ul>");

			foreach (var csId in dicSuvSalesRank.Keys)
			{
				int serialId = Convert.ToInt32(csId);
				int count = Convert.ToInt32(dicSuvSalesRank[serialId].FirstOrDefault().Key);
				if (serialId == -1 && count == 0 && dicSuvSalesRank[serialId].FirstOrDefault().Value.Length > 1)
				{
					suvSalesUpdateDate = dicSuvSalesRank[serialId].FirstOrDefault().Value[0];
					continue;
				}
				string serialName = "";
				string serialSpell = string.Empty;
				if (dicSuvSalesRank[serialId].FirstOrDefault().Value.Length > 1)
				{
					serialName = Convert.ToString(dicSuvSalesRank[serialId].FirstOrDefault().Value[1]);
					serialSpell = Convert.ToString(dicSuvSalesRank[serialId].FirstOrDefault().Value[0]);
				}

				string serialUrl = "/" + serialSpell + "/";
				htmlCode.Append("<li>");
				htmlCode.AppendFormat("<a href=\"{0}\" target=\"_blank\">{1}</a><span class=\"youhao\">{2}</span></li>", serialUrl, serialName, count);
			}
			htmlCode.AppendLine("</ul>");
			suvSalesRankHtml = htmlCode.ToString();
		}

	}
}