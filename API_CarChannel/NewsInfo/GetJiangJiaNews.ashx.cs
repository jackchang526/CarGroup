using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using System.Text;

namespace BitAuto.CarChannelAPI.Web.NewsInfo
{
	/// <summary>
	/// 获取降价接口
	/// </summary>
	public class GetJiangJiaNews : IHttpHandler
	{
		private int _SerialId = 0;
		private int _CityId = 0;
		private int _Top;
		private int _isWireless = 0;//无线

		private HttpRequest _request;
		private HttpResponse _response;

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60 * 4);
			context.Response.ContentType = "application/x-javascript";

			_response = context.Response;
			_request = context.Request;

			GetParams();
			GetContent();
		}

		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParams()
		{
			_SerialId = ConvertHelper.GetInteger(_request.QueryString["id"]);
			_CityId = ConvertHelper.GetInteger(_request.QueryString["cityid"]);
			_isWireless = ConvertHelper.GetInteger(_request.QueryString["iswireless"]);
			if (_CityId == 0)
				_CityId = 201;
			_Top = ConvertHelper.GetInteger(_request.QueryString["top"]);
			if (_Top < 1)
				_Top = 2;
		}
		/// <summary>
		/// 得到内容
		/// </summary>
		private void GetContent()
		{
			if (_SerialId < 1 || _CityId < 1) return;
			try
			{
				//XmlDocument xmlDoc = GetCityXmlDocument();
				//if (xmlDoc == null) return;
				//XmlNode xNode = xmlDoc.SelectSingleNode(string.Format("root/Province/City[@ID={0}]", _CityId));
				//if (xNode == null) return;
				Dictionary<int, City> cityDic = AutoStorageService.GetCityNameIdList();
				if (!cityDic.ContainsKey(_CityId))
					return;
				CarNewsBll newsBll = new CarNewsBll();
				string cityName = cityDic[_CityId].CityName;
				//得到新闻列表
				List<News> newsList = null;
				Dictionary<int, int> parentCityList = CommonFunction.GetCityRelationParentDic();
				if (parentCityList.ContainsKey(_CityId) && _CityId != parentCityList[_CityId])
				{
					newsList = newsBll.GetSerialJiangJiaTopNews(_SerialId, _CityId, parentCityList[_CityId], _Top, 3);
				}
				else
				{
					newsList = newsBll.GetSerialJiangJiaTopNews(_SerialId, _CityId, _Top, 3);
				}

				//edit anh 20130507
				//目前看“行情”已不在单独发布，都是使用已有的降价转载，这样的话显示出来的“行情”都是过期的，对用户来说意义不大 by gaoyan
				//// add by chengl Jan.8.2013
				//// 当降价新闻1条都没有时 取行情新闻
				//if (newsList == null || newsList.Count == 0)
				//{
				//    if (parentCityList.ContainsKey(_CityId) && _CityId != parentCityList[_CityId])
				//    {
				//        newsList = new CarNewsBll().GetTopCityNews2(_SerialId, _CityId, parentCityList[_CityId], _Top);
				//    }
				//    else
				//    {
				//        newsList = new CarNewsBll().GetTopCityNews2(_SerialId, _CityId, _Top);
				//    }
				//}

				SerialJiangJiaNewsSummary summary = newsBll.GetSerialJiangJiaNewsSummary(_SerialId, _CityId);
				decimal maxFav = decimal.Zero;
				int newsCount = 0;
				if (summary != null)
				{
					maxFav = summary.MaxFavorablePrice;
					newsCount = summary.NewsCount;
				}
				StringBuilder jsonContent = new StringBuilder();
				if (newsList != null && newsList.Count > 0)
				{
					int index = 0;
					foreach (News entity in newsList)
					{
						if (index >= _Top) break;
						//modified by sk 增加移动版参数 行情地址规则修改 2013.08.26
						if (_isWireless == 1)
						{
                            var imageUrl = entity.CarImage.ToString();
                            if (imageUrl.ToLower().IndexOf(".bitauto") == -1)
                            {
                                imageUrl = "";
                            }
                            if (imageUrl.Length > 7)
                            {
                                imageUrl = imageUrl.Insert(imageUrl.IndexOf('/', 7) + 1, "newsimg-150-w0/");
                            }
                            jsonContent.AppendFormat(",\"{0},{1},{2},{3},{4}\"",
                                HttpUtility.UrlEncode(entity.Title).Replace("+", "%20"),
                                HttpUtility.UrlEncode(string.Format("http://m.qiche4s.cn/{0}/news_{1}.html", entity.VendorId, entity.NewsId)),
                                entity.PublishTime.ToString("MM-dd"),
                                    entity.PublishTime.ToString("yyyy-MM-dd"),
                                HttpUtility.UrlEncode(imageUrl));
						}
						else
						{
							jsonContent.AppendFormat(",\"{0},{1},{2},{3}\""
									, HttpUtility.UrlEncode(entity.Title).Replace("+", "%20"), HttpUtility.UrlEncode(entity.PageUrl), entity.PublishTime.ToString("MM-dd"),
									entity.PublishTime.ToString("yyyy-MM-dd"));
						}
						index++;
					}
				}
				if (!string.IsNullOrEmpty(jsonContent.ToString()))
					jsonContent.Remove(0, 1);

				string jiangjiaNews = "var jjnews={" + string.Format("\"name\":\"{0}\",\"num\":\"{1}\",maxfav:\"{2}\",\"nlist\":[{3}]"
					, cityName, newsCount.ToString(), maxFav.ToString("0.##"), jsonContent.ToString()) + "}";

				_response.Write(jiangjiaNews);
				return;
			}
			catch
			{
				return;
			}
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