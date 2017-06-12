using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.LeveldropdownlistData
{
	public partial class watchcardirectdatainterface : System.Web.UI.Page
	{
		private string _Conditions = string.Empty;//用户查询数据的条件
		private int _Include = 0;//是否包含上一级的数据
		private int _ParentId = 0;//父级ID是多少
		private string _RequestType = string.Empty;//请求类型
		private string _DataName = string.Empty;//数据名称
		private string _Serias = string.Empty;//下拉列表的系列
		private XmlDocument _xmlDoc = new XmlDocument();//得到要显示数据的XML  

		protected void Page_Load(object sender, EventArgs e)
		{
			//得到页面参数
			GetParams();
			if (string.IsNullOrEmpty(_RequestType) || string.IsNullOrEmpty(_DataName)) { Response.Write(""); return; }
			//得到页面内容
			string content = GetContent();
			if (string.IsNullOrEmpty(content)) { Response.Write(""); return; }
			Response.Write(content);
		}
		/// <summary>
		/// 得到当前内容的上下文
		/// </summary>
		/// <param name="context"></param>
		private void GetParams()
		{
			_Conditions = string.IsNullOrEmpty(Request.QueryString["type"]) ? string.Empty : Request.QueryString["type"];
			_Include = string.IsNullOrEmpty(Request.QueryString["include"]) ? 0 : Convert.ToInt32(Request.QueryString["include"]);
			_ParentId = string.IsNullOrEmpty(Request.QueryString["pid"]) ? 0 : Convert.ToInt32(Request.QueryString["pid"]);
			_RequestType = string.IsNullOrEmpty(Request.QueryString["rt"]) ? string.Empty : Request.QueryString["rt"];
			_DataName = string.IsNullOrEmpty(Request.QueryString["key"]) ? string.Empty : Request.QueryString["key"];
			_Serias = string.IsNullOrEmpty(Request.QueryString["serias"]) ? string.Empty : Request.QueryString["serias"];
		}
		/// <summary>
		/// 得到页面内容
		/// </summary>
		/// <returns></returns>
		private string GetContent()
		{
			_RequestType = _RequestType.ToLower();
			switch (_RequestType)
			{
				case "serial":
					return GetSerialContent();
				case "master":
					return GetMasterContent();
			}
			return "";
		}
		/// <summary>
		/// 得到子品牌内容
		/// </summary>
		/// <returns></returns>
		private string GetSerialContent()
		{
			string cacheKey = string.Format("leveldropdownlistControl_{4}_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType, _Serias);
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (string)obj;

			if (string.IsNullOrEmpty(_Conditions)) return "";
			//创建树型类
			string xmlString = GetXmlContent(_Conditions);
			if (string.IsNullOrEmpty(xmlString)) return "";
			//得到内容
			string content = string.Empty;
			try
			{
				//加载数据的文件
				_xmlDoc.LoadXml(xmlString);
				if (_xmlDoc == null) return "";
				bool isContainsBrand = false;

				XmlNode masterNode = _xmlDoc.SelectSingleNode("data/master[@id='" + _ParentId + "']");
				if (masterNode == null || masterNode.ChildNodes.Count < 1) return "";

				XmlNodeList xNodeList;
				if (masterNode.ChildNodes[0].Name == "brand")
				{
					xNodeList = masterNode.SelectNodes("brand/serial");
					isContainsBrand = true;
				}
				else
				{
					xNodeList = masterNode.SelectNodes("serial");
				}
				if (xNodeList == null || xNodeList.Count < 1) return "";

				List<string> contentlist = new List<string>();
				foreach (XmlElement entity in xNodeList)
				{
					//XmlNode pNode = entity.ParentNode();
					contentlist.Add(",");
					contentlist.Add(string.Format("\"s{0}\":", entity.GetAttribute("id")));
					contentlist.Add("{");
					contentlist.Add(string.Format("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{4}\",\"urlSpell\":\"{2}\",\"showName\":\"{3}\",\"csSale\":\"{5}\", \"goid\":\"{6}\",\"goname\":\"{7}\""
										 , entity.GetAttribute("id")
										 , entity.GetAttribute("name")
										 , entity.GetAttribute("extra")
										 , ""
										 , ((XmlElement)masterNode).GetAttribute("id")
										 , "0"
										 , isContainsBrand ? ((XmlElement)entity.ParentNode).GetAttribute("id") : "0"
										 , isContainsBrand ? ((XmlElement)entity.ParentNode).GetAttribute("name") : "无"));
					contentlist.Add("}");
				}
				contentlist.RemoveAt(0);
				contentlist.Insert(0, "{");
				contentlist.Insert(0, string.Format("requestDatalist[\"{0}\"]=", _DataName));
				contentlist.Add("}");
				content = string.Concat(contentlist.ToArray());

				CacheManager.InsertCache(cacheKey, content, 60);
				return content;
			}
			catch
			{
				return "";
			}
		}
		/// <summary>
		/// 得到主品牌内容
		/// </summary>
		/// <returns></returns>
		private string GetMasterContent()
		{
			string cacheKey = string.Format("leveldropdownlistControl_{4}_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType, _Serias);
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null) return (string)obj;

			if (string.IsNullOrEmpty(_Conditions)) return "";
			//创建树型类
			string xmlString = GetXmlContent(_Conditions);
			if (string.IsNullOrEmpty(xmlString)) return "";
			//得到内容
			string content = string.Empty;
			try
			{
				//加载数据的文件
				_xmlDoc.LoadXml(xmlString);
				if (_xmlDoc == null) return "";
				//查询要显示的数据
				XmlNodeList xNodeList = _xmlDoc.SelectNodes("data/master");
				if (xNodeList == null || xNodeList.Count < 1) return "";

				List<string> contentlist = new List<string>();
				foreach (XmlElement entity in xNodeList)
				{
					contentlist.Add(",");
					contentlist.Add(string.Format("\"m{0}\":", entity.GetAttribute("id")));
					contentlist.Add("{");
					contentlist.Add(string.Format("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"0\",\"urlSpell\":\"{2}\",\"tSpell\":\"{3}\",\"nl\":[{4}]"
										   , entity.GetAttribute("id")
										   , entity.GetAttribute("name")
										   , entity.GetAttribute("extra")
										   , entity.GetAttribute("firstchar")
										   , ""));
					contentlist.Add("}");
				}
				contentlist.RemoveAt(0);
				contentlist.Insert(0, "{");
				contentlist.Insert(0, string.Format("requestDatalist[\"{0}\"]=", _DataName));
				contentlist.Add("}");
				content = string.Concat(contentlist.ToArray());

				CacheManager.InsertCache(cacheKey, content, 60);
				return content;
			}
			catch
			{
				return "";
			}
		}
		/// <summary>
		/// 得到XML内容
		/// </summary>
		/// <returns></returns>
		private string GetXmlContent(string conditions)
		{
			switch (conditions)
			{
				case "chexing":
					return GetCarXmlContent(conditions);
				case "daogou":
					return GetCarXmlContent(conditions);
				case "shipin":
					return GetOtherXmlContent(conditions);
				case "tujie":
					return GetOtherXmlContent(conditions);
				case "pingce":
					return GetCarXmlContent(conditions);
				case "keji":
					return GetCarXmlContent(conditions);
				case "anquan":
					return GetCarXmlContent(conditions);
				case "koubei":
					return GetOtherXmlContent(conditions);
				case "weixiu":
					return GetOtherXmlContent(conditions);
				case "dayi":
					return GetOtherXmlContent(conditions);
			}
			return "";
		}
		/// <summary>
		/// 得到车型所属的xml内容
		/// </summary>
		/// <param name="conditions"></param>
		/// <returns></returns>
		private string GetCarXmlContent(string conditions)
		{
			TreeData treeData = new TreeFactory().GetTreeDataObject(conditions);
			return treeData.TreeXmlData();
		}
		/// <summary>
		/// 得到其他所属的xml内容
		/// </summary>
		/// <param name="conditions"></param>
		/// <returns></returns>
		private string GetOtherXmlContent(string conditions)
		{
			string urlPage = string.Empty;

			switch (conditions)
			{
				case "shipin":
					urlPage = "http://v.bitauto.com/car/videotree.ashx";
					break;
				case "tujie":
					urlPage = "http://imgsvr.bitauto.com/photo/imageservice.aspx?dataname=treedatabygroup";
					break;
				case "koubei":
					urlPage = "http://koubei.bitauto.com/inc/debris/koubei/3.0/index/treedata_v2.xml";
					break;
				case "weixiu":
					urlPage = "http://car.bitauto.com/interfaceforbitauto/tree/treeXmlContent.aspx?dept=pricetree";
					break;
				case "dayi":
					urlPage = "http://ask.bitauto.com/inc/debris/index/treedata_v2.xml";
					break;
			}

			if (string.IsNullOrEmpty(urlPage)) return "";


			return CommonFunction.GetContentByUrl(urlPage);
		}
	}
}