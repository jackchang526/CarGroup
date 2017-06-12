using System;
using System.Data;
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
	/// GetJiangJiaDealer 的摘要说明
	/// 新版子品牌综述页获取降价接口 add by chengl Aug.15.2013
	/// </summary>
	public class GetJiangJiaDealer : IHttpHandler
	{
		private int _SerialId = 0;
		private int _CityId = 0;
		private int _Top = 3;

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
			if (_CityId == 0)
				_CityId = 201;
			_Top = ConvertHelper.GetInteger(_request.QueryString["top"]);
			if (_Top < 1 || _Top > 10)
			{ _Top = 3; }

		}

		/// <summary>
		/// 得到内容
		/// </summary>
		private void GetContent()
		{
			if (_SerialId < 1 || _CityId < 1) return;
			try
			{
				Dictionary<int, City> cityDic = AutoStorageService.GetCityNameIdList();
				if (!cityDic.ContainsKey(_CityId))
					return;
				CarNewsBll newsBll = new CarNewsBll();
				string cityName = cityDic[_CityId].CityName;
				//得到新闻列表
				List<JiangJiaNews> newsList = null;
				Dictionary<int, int> parentCityList = CommonFunction.GetCityRelationParentDic();
				if (parentCityList.ContainsKey(_CityId) && _CityId != parentCityList[_CityId])
				{
					newsList = newsBll.GetSerialJiangJiaTopDealer(_SerialId, _CityId, parentCityList[_CityId], _Top, 3);
				}
				else
				{
					newsList = newsBll.GetSerialJiangJiaTopDealer(_SerialId, _CityId, _Top, 3);
				}

				StringBuilder jsonContent = new StringBuilder();
				if (newsList != null && newsList.Count > 0)
				{
					int index = 0;
					foreach (JiangJiaNews entity in newsList)
					{
						if (index >= _Top) break;
						if (index > 0)
						{
							jsonContent.Append(",");
						}
						jsonContent.Append("{");
						jsonContent.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"mp\":\"{2}\",\"title\":\"{3}\",\"url\":\"{4}\",\"time\":\"{5}\""
								, entity.VendorId
								, CommonFunction.GetUnicodeByString(entity.VendorName)
								, CommonFunction.GetUnicodeByString(entity.MaxFavorablePrice.ToString("f2"))
								, CommonFunction.GetUnicodeByString(entity.Title)
								, CommonFunction.GetUnicodeByString(entity.NewsUrl)
								, entity.PublishTime.ToString("yyyy-MM-dd")
								);
						jsonContent.Append("}");
						index++;
					}
				}

				// 取经销商数量&前几条经销商报价
				StringBuilder jsonDealerContent = new StringBuilder();
				DataSet ds = newsBll.GetDealerPriceByCsIDAndCityID(_Top, _SerialId, _CityId);
				// 经销商数量 & 经销商报价3条
				int dealerCount = 0;
				if (ds != null && ds.Tables.Count > 1)
				{
					if (ds.Tables[0].Rows.Count > 0
						&& int.TryParse(ds.Tables[0].Rows[0]["dealerCount"].ToString(), out dealerCount))
					{ }
					if (ds.Tables[1].Rows.Count > 0)
					{
						int loop = 0;
						foreach (DataRow dr in ds.Tables[1].Rows)
						{
							if (loop >= _Top) break;
							var VendorName = dr["VendorName"].ToString();
							if (loop > 0)
							{
								jsonDealerContent.Append(",");
							}
							jsonDealerContent.Append("{");
							jsonDealerContent.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"price\":\"{2}\""
							, dr["VendorID"].ToString()
							, CommonFunction.GetUnicodeByString(VendorName)
							, CommonFunction.GetUnicodeByString(dr["MinPrice"].ToString() + "-" + dr["MaxPrice"].ToString() + "万")
							);
							jsonDealerContent.Append("}");
							loop++;
						}
					}
				}

				string jiangjiaNews = "var jjdealer={" + string.Format("\"name\":\"{0}\",\"count\":\"{1}\",\"nlist\":[{2}],\"dealerlist\":[{3}]"
					, CommonFunction.GetUnicodeByString(cityName)
					, dealerCount, jsonContent.ToString(), jsonDealerContent.ToString()) + "}";

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