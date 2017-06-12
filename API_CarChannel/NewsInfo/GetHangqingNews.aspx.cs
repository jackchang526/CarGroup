using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Caching;
using System.Xml;
using System.IO;
using System.Text;

using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannelAPI.Web.NewsInfo
{
	public partial class _gethangqingnews : PageBase
	{
		private int _SerialId = 0;
		private int _CityId = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			Response.ContentType = "application/x-javascript";
			GetParams();
			GetContent();
            // Response.End();
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParams()
		{
			_SerialId = ConvertHelper.GetInteger(Request.QueryString["id"]);
			_CityId = ConvertHelper.GetInteger(Request.QueryString["cityid"]);
			if (_CityId == 0)
				_CityId = 201;
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
				string cityName = cityDic[_CityId].CityName;
                //得到新闻列表
                List<News> newsList = null;
                Dictionary<int, int> parentCityList = CommonFunction.GetCityRelationParentDic();
                if (parentCityList.ContainsKey(_CityId) && _CityId != parentCityList[_CityId])
                {
                    //newsList = new Car_SerialBll().GetCityHangQingNewsList(_SerialId, _CityId, parentCityList[_CityId]);
					newsList = new CarNewsBll().GetTopCityNews2(_SerialId, _CityId, parentCityList[_CityId],2);
                }
                else
                {
                    //newsList = new Car_SerialBll().GetCityHangQingNewsList(_SerialId, _CityId);
					newsList = new CarNewsBll().GetTopCityNews2(_SerialId, _CityId,2);
                }

				StringBuilder jsonContent = new StringBuilder();
				if (newsList != null && newsList.Count > 0)
				{
					int index = 0;
					foreach (News entity in newsList)
					{
						if (index > 1) break;
						jsonContent.AppendFormat(",\"{0},{1},{2}\""
								, HttpUtility.UrlEncode(entity.Title).Replace("+", "%20"), Server.UrlEncode(entity.PageUrl), entity.PublishTime.ToString("MM-dd"));
						index++;
					}
				}
				if (!string.IsNullOrEmpty(jsonContent.ToString()))
					jsonContent.Remove(0, 1);

				string hqingString = "var hqingCity={" + string.Format("\"name\":\"{0}\",\"nlist\":[{1}]", cityName, jsonContent.ToString()) + "}";

				Response.Write(hqingString);
				return;
			}
			catch
			{
				return;
			}
		}
	}
}
