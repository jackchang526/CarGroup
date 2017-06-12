using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using System.Data;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannelAPI.Web.zhihuan
{
	/// <summary>
	/// GetSerialList 的摘要说明
	/// </summary>
	public class GetSerialList : IHttpHandler
	{
		private HttpRequest _Request = null;
		private HttpResponse _Response = null;
		private int _BrandId = 0;
		private int _CityId = 0;
		private int _CityParentId = 0;
		private XmlNode _BrandNode = null;
		private int _TopNumber = 5;
		/// <summary>
		/// 输出类型 0=json，1=xml
		/// </summary>
		private int _OutputType = 0;

		public void ProcessRequest(HttpContext context)
		{
			_Request = context.Request;
			_Response = context.Response;

			BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
			_Response.ContentType = "application/x-javascript";

			GetParams();

			GetContent();
		}
		private void GetParams()
		{
			_BrandId = string.IsNullOrEmpty(_Request.QueryString["cbid"]) ? 0 : ConvertHelper.GetInteger(_Request.QueryString["cbid"]);
			_CityId = string.IsNullOrEmpty(_Request.QueryString["cityid"]) ? 0 : ConvertHelper.GetInteger(_Request.QueryString["cityid"]);

			if (_CityId < 1) return;

			Dictionary<int, CityExtend> cityList = AutoStorageService.Get350CityDicKeyCityId();
			if (cityList == null || cityList.Count < 1 || !cityList.ContainsKey(_CityId)) return;

			_CityParentId = cityList[_CityId].ParentId;

			if (_Request.QueryString["outputtype"] != null && _Request.QueryString["outputtype"].ToLower() == "xml")
			{
				_OutputType = 1;
				_TopNumber = -1;
			}
		}
		private void GetContent()
		{
			if (_BrandId < 1 || _CityParentId < 1) return;

			DataRow row = null;
			XmlNode serialNode = null;

			DataRowCollection rows = GetDataRows();
            var pageBase = new BitAuto.CarChannel.Common.PageBase();
			if (_OutputType == 0) //json 输出
			{
				_Response.Write("var zhihuanCsList= zhihuanCsList||{};");
				_Response.Write(string.Format("zhihuanCsList['{0}']=[", _CityId.ToString()));
				if (rows != null && rows.Count > 0)
				{
					for (int i = 0; i < rows.Count; )
					{
						row = rows[i];
						serialNode = _BrandNode.SelectSingleNode(string.Format("Serial[@ID='{0}']", row["serialid"].ToString()));
						_Response.Write("{");
                        _Response.Write(string.Format("id:{0},name:\"{1}\",spell:\"{2}\",price:\"{3}\",same:\"{4}\",diff:\"{5}\"",
                            serialNode.Attributes["ID"].Value,
                            serialNode.Attributes["Name"].Value,
                            serialNode.Attributes["AllSpell"].Value,
                            pageBase.GetSerialPriceRangeByID(Convert.ToInt32(row["serialid"].ToString())),
                            row["SameBrandPrivilege"].ToString(),
                            row["DiffBrandPrivilege"].ToString()
                            ));
						_Response.Write("}");
						if (++i != rows.Count)
						{
							_Response.Write(",");
						}
					}
				}
				_Response.Write("];");
			}
			else if (_OutputType == 1) //xml
			{
				_Response.ContentType = "text/xml";

				XmlDocument doc = new XmlDocument();
				doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
				XmlNode root = doc.CreateElement("Root");
				doc.AppendChild(root);

				if (rows != null && rows.Count > 0)
				{
					XmlElement infoEle = null;
					XmlElement sameEle = null;
					XmlElement diffEle = null;
					XmlElement memoEle = null;
					for (int i = 0; i < rows.Count; i++)
					{
						row = rows[i];
						serialNode = _BrandNode.SelectSingleNode(string.Format("Serial[@ID='{0}']", row["serialid"].ToString()));

						infoEle = doc.CreateElement("Serial");

						infoEle.SetAttribute("Id", serialNode.Attributes["ID"].Value);
						infoEle.SetAttribute("Cb_Id", _BrandId.ToString());
						infoEle.SetAttribute("City_Id", _CityId.ToString());

                        infoEle.SetAttribute("Price", pageBase.GetSerialPriceRangeByID(Convert.ToInt32(serialNode.Attributes["ID"].Value)));

						sameEle = doc.CreateElement("SameBrandPrivilege");
						sameEle.InnerText = HttpUtility.HtmlDecode(row["SameBrandPrivilege"].ToString());
						infoEle.AppendChild(sameEle);

						diffEle = doc.CreateElement("DiffBrandPrivilege");
						diffEle.InnerText = HttpUtility.HtmlDecode(row["DiffBrandPrivilege"].ToString());
						infoEle.AppendChild(diffEle);

						memoEle = doc.CreateElement("Memo");
						memoEle.InnerText = HttpUtility.HtmlDecode(row["Memo"].ToString());
						infoEle.AppendChild(memoEle);

						root.AppendChild(infoEle);
					}
				}

				_Response.Write(doc.OuterXml);
			}
		}
		/// <summary>
		/// 获取置换信息
		/// </summary>
		/// <param name="top">数，-1为全部</param>
		private DataRowCollection GetDataRows()
		{
			XmlDocument serialXml = BitAuto.CarChannel.Common.Interface.AutoStorageService.GetAutoXml();
			if (serialXml != null)
			{
				_BrandNode = serialXml.SelectSingleNode(string.Format("Params/MasterBrand/Brand[@ID={0}]", _BrandId.ToString()));
				if (_BrandNode != null)
				{
					XmlNodeList serials = _BrandNode.SelectNodes("Serial");
					if (serials.Count > 0)
					{

						List<int> ids = new List<int>(serials.Count);
						foreach (XmlNode serial in serials)
						{
							ids.Add(ConvertHelper.GetInteger(serial.Attributes["ID"].Value));
						}

						DataSet ds = null;
						if (_OutputType == 0)
							ds = new Car_SerialBll().GetCarReplacementInfo(ids, _CityId, _CityParentId, _TopNumber);
						else if (_OutputType == 1)
							ds = new Car_SerialBll().GetCarReplacementInfoAndMemo(ids, _CityId, _CityParentId);
						if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
						{
							return ds.Tables[0].Rows;
						}
					}
				}
			}
			return null;
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