using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;
using System.Data;
using BitAuto.CarChannel.BLL;
using System.Text;
using BitAuto.CarChannel.Common;

namespace WirelessWeb.handlers
{
	/// <summary>
	/// 获取子品牌新闻
	/// jsonp方式返回
	/// 参数:
	///		call: 回调方法
	///		t: 枚举 BitAuto.CarChannel.Common.Enum.CarNewsType 
	///		size: 行数
	///		i：第几页
	///		cs: 子品牌id
	///	返回：json数组及总新闻数和pagecount
	///		callback(
	///			[
	///				{
	///					id:1
	///					, tag:UrlEncode(..)
	///					, d:\"2012-01-01\"
	///					，aut:UrlEncode(..)
	///				}
	///				, ...
	///			]
	///			, newscount
	///			, pagecount
	///			) 
	/// </summary>
	public class GetCsNews : WirelessPageBase, IHttpHandler
	{
		private int _pageIndex;
		private int _pageSize;
		private int _serialId;
		private int _carNewsTypeInt;
		private string newsType;
		private string _callBack;
		private int _newsCount;

		public void ProcessRequest(HttpContext context)
		{
			BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
			context.Response.ContentType = "application/x-javascript";

			GetParam(context.Request);

			if (_serialId < 1 || _carNewsTypeInt < 0 || string.IsNullOrEmpty(_callBack))
				return;

			context.Response.Write(_callBack + "([");
			context.Response.Write(GetJosn());
			context.Response.Write(string.Format("],{0}, {1});", _newsCount.ToString(), CommonFunction.GetTotalPageNumber(_newsCount, _pageSize)));
		}

		private string GetJosn()
		{
			DataSet ds = null;
			if (newsType == "wenzhang")
			{
				List<int> carTypeIdList = new List<int>() 
				{ 
                (int)CarNewsType.serialfocus,
				(int)CarNewsType.shijia,
				(int)CarNewsType.daogou,
				(int)CarNewsType.yongche,
				(int)CarNewsType.gaizhuang,
				(int)CarNewsType.anquan,
				(int)CarNewsType.xinwen,
                (int)CarNewsType.pingce
				};
				ds = new CarNewsBll().GetSerialNewsAllData(_serialId, carTypeIdList, _pageSize, _pageIndex, ref _newsCount);
			}
			else
			{
				ds = new CarNewsBll().GetSerialNewsAllData(_serialId, (CarNewsType)_carNewsTypeInt, _pageSize, _pageIndex, ref _newsCount);
			}

			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
				return string.Empty;
			StringBuilder json = new StringBuilder();
			DataRowCollection rows = ds.Tables[0].Rows;
			for (int i = 0; i < rows.Count; i++)
			{
				DataRow row = rows[i];
				int newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
				string newsUrl = Convert.ToString(row["filepath"]).Replace("news.bitauto.com", "news.m.yiche.com");
				string firstPicUrl = string.Empty;
				if (newsType != "hangqing")
				{
					firstPicUrl = ConvertHelper.GetString(row["FirstPicUrl"]);
				}
				//if (newsType == "pingce")
				//    newsUrl = "/" + _serialAllSpell + "/pingce/p" + newsId + "/";
				DateTime publishTime = Convert.ToDateTime(row["publishtime"]);
				string author = Convert.ToString(row["author"]);
				int commentNum = row["CommentNum"] == DBNull.Value ? 0 : Convert.ToInt32(row["CommentNum"]);
				string image = string.Empty;
				if (!string.IsNullOrEmpty(firstPicUrl))
				{
					firstPicUrl = firstPicUrl.Replace("/bitauto/", "/newsimg_180_w0_1/bitauto/")
						.Replace("/autoalbum/", "/newsimg_180_w0_1/autoalbum/");
					if (firstPicUrl.IndexOf(".bitauto") != -1)
					{
						image = firstPicUrl;
					}
				}
				json.Append("{");
				json.AppendFormat("id:{0},tag:\"{1}\",d:\"{2}\",aut:\"{3}\",url:\"{4}\",com:\"{5}\",image:\"{6}\""
					, row["CmsNewsId"].ToString()
					, HttpUtility.UrlEncode(CommonFunction.NewsTitleDecode(row["title"].ToString()))
					, publishTime.ToString("yyyy-MM-dd")
					, HttpUtility.UrlEncode(StrCut(author, 6))
					, HttpUtility.UrlEncode(newsUrl)
					, commentNum.ToString()
					, HttpUtility.UrlEncode(image)
					);
				json.Append("}");
				if (i < rows.Count - 1)
					json.Append(",");
			}
			return json.ToString();
		}

		private void GetParam(HttpRequest request)
		{
			_serialId = ConvertHelper.GetInteger(request.QueryString["cs"]);
			if (_serialId < 0)
				return;
			if (!string.IsNullOrEmpty(request.QueryString["call"]))
			{
				_callBack = request.QueryString["call"].Trim();
			}
			if (string.IsNullOrEmpty(_callBack))
				return;
			if (!string.IsNullOrEmpty(request.QueryString["t"]))
			{
				newsType = request.QueryString["t"];
				if (newsType == null)
					newsType = "wenzhang";
				newsType = newsType.ToLower();

				switch (newsType)
				{
					case "wenzhang":
						_carNewsTypeInt = (int)CarNewsType.wenzhang;
						break;
					case "xinwen":
						_carNewsTypeInt = (int)CarNewsType.xinwen;
						break;
					case "daogou":
						_carNewsTypeInt = (int)CarNewsType.daogou;
						break;
					case "shijia":
						_carNewsTypeInt = (int)CarNewsType.shijia;
						break;
					case "yongche":
						_carNewsTypeInt = (int)CarNewsType.yongche;
						break;
					case "hangqing":
						_carNewsTypeInt = (int)CarNewsType.hangqing;
						break;
					case "pingce":
						_carNewsTypeInt = (int)CarNewsType.pingce;
						break;
					case "gaizhuang":
						_carNewsTypeInt = (int)CarNewsType.gaizhuang;
						break;
					case "anquan":
						_carNewsTypeInt = (int)CarNewsType.anquan;
						break;
					default:
						_carNewsTypeInt = (int)CarNewsType.xinwen;
						break;
				}
			}
			_pageIndex = ConvertHelper.GetInteger(request.QueryString["i"]);
			if (_pageIndex < 1)
				_pageIndex = 1;
			_pageSize = ConvertHelper.GetInteger(request.QueryString["size"]);
			if (_pageSize < 1)
				_pageSize = 10;
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