using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml;
using System.Text;
using System.IO;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.DAL;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GoCarTypeAndPrice_Json 的摘要说明
	/// </summary>
	public class GoCarTypeAndPrice_Json : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Cache.SetNoServerCaching();
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(60));

			string json = "";
			//if (context.Cache[cacheKey] != null)
			//{
			//    json = Convert.ToString(context.Cache[cacheKey]);
			//}
			//else
			//{
			RequestXML requestXML = new RequestXML();
			//json = requestXML.ReturnJsonObject("");
			json = requestXML.ReturnContainsBrandJsonObject();
			//}
			context.Response.Write(json);
			//context.Response.Write("var JSonData=" + json);
			// context.Response.Write("var JSonData=" + requestXML.ReturnJsonObject(""));
			// modified by chengl Oct.13.2009 end  
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		/*完成请求XML到拼装JSON的过程*/
		public class RequestXML
		{
			private string m_RequestUrl = WebConfig.AutoDataFile;//"http://carser.bitauto.com/forpicmastertoserial/MasterToserial.xml";//
			/// <summary>
			/// 设置请求的链接
			/// </summary>
			public string RequestUrl
			{
				set
				{
					m_RequestUrl = value;
				}
			}
			/// <summary>
			/// 返回Json字符串
			/// </summary>
			/// <param name="sRequestUrl">请求的XML地址</param>
			/// <returns>返回Json字符串</returns>
			public string ReturnJsonObject(string xmlString)
			{
				//判断用户采用的加载数据方式
				if (string.IsNullOrEmpty(xmlString) && string.IsNullOrEmpty(m_RequestUrl))
				{
					return "";
				}
				if (string.IsNullOrEmpty(xmlString))
				{
					xmlString = RequestXMLFile();
				}
				if (xmlString == "-1")
				{
					return "";
				}
				try
				{
					//加载XML文件
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(xmlString);
					XmlNodeList xmlNodeList = xmlDoc.SelectNodes("Params/MasterBrand/Item");
					XmlNodeList nodeXmlNodeList;
					if (xmlNodeList == null || xmlNodeList.Count < 1)
					{
						return "";
					}
					StringBuilder returnJson = new StringBuilder("{masterBrand:[");
					string xmlPath = "Params/Serial[@BsID='";
					string xmlEndPath = "']";
					string tempPath = "";
					int xmlNodeLength = 0;
					xmlNodeLength = xmlNodeList.Count;

					for (int i = 0; i < xmlNodeLength; i++)
					{
						XmlNode xmlNode = xmlNodeList[i];
						tempPath = xmlPath + xmlNode.Attributes["ID"].InnerText.ToString() + xmlEndPath;
						nodeXmlNodeList = xmlDoc.SelectSingleNode(tempPath) == null ? null : xmlDoc.SelectSingleNode(tempPath).ChildNodes;
						if (i == xmlNodeLength - 1)
						{
							returnJson.Append("{");
							returnJson.AppendFormat("\"{0}\":\"{1}\",", "id", xmlNode.Attributes["ID"].InnerText.ToString());
							returnJson.AppendFormat("\"{0}\":\"{1}\",", "name", xmlNode.Attributes["Name"].InnerText.ToString());
							returnJson.Append("carSerial:");
							returnJson.Append(AppendNodeJson(nodeXmlNodeList));
							returnJson.Append("}");
							continue;
						}
						returnJson.Append("{");
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "id", xmlNode.Attributes["ID"].InnerText.ToString());
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "name", xmlNode.Attributes["Name"].InnerText.ToString());
						returnJson.Append("carSerial:");
						returnJson.Append(AppendNodeJson(nodeXmlNodeList));
						returnJson.Append("},");
					}
					returnJson.Append("]}");
					return returnJson.ToString();
				}
				catch
				{
					return "";
				}
			}
			/// <summary>
			/// 返回包含品牌信息的JSON对象
			/// modified by wangzt Mon.7.2010 end
			/// </summary>
			/// <param name="xmlString">请求的XML地址</param>
			/// <returns></returns>
			public string ReturnContainsBrandJsonObject()
			{
				try
				{
					XmlDocument xmlDoc = AutoStorageService.GetAutoXml();

					if (xmlDoc == null)
					{
						return "";
					}
					//取到主品牌列表
					XmlNodeList xNodeList = xmlDoc.SelectNodes("Params/MasterBrand");
					if (xNodeList == null || xNodeList.Count < 1)
					{
						return "";
					}

					//主品牌于子品牌对应关系Json对象
					StringBuilder masterBrandAndSerialJSon = new StringBuilder();
					int iMasterBrandAndSerialLength = 0;
					iMasterBrandAndSerialLength = xNodeList.Count;
					//主品牌和品牌对应关系Json对象
					StringBuilder masterBrandAndBrandJSon = new StringBuilder();

					//主品牌于子品牌对应关系Json对象--头
					masterBrandAndSerialJSon.Append("var JSonData={masterBrand:[");
					masterBrandAndBrandJSon.Append("var BrandJsonData={masterBrand:[");
					string masterBrandName = "";

					//生成主品牌对子品牌对应关系
					for (int i = 0; i < iMasterBrandAndSerialLength; i++)
					{
						XmlElement xmlElem = (XmlElement)xNodeList[i];
						if (!xmlElem.HasChildNodes || xmlElem.ChildNodes.Count < 1)
						{
							continue;
						}
						masterBrandAndSerialJSon.Append("{");
						masterBrandAndBrandJSon.Append("{");

						masterBrandAndSerialJSon.AppendFormat("\"{0}\":\"{1}\",", "id", xmlElem.GetAttribute("ID"));
						masterBrandName = xmlElem.GetAttribute("Spell").Substring(0, 1).ToUpper() + " " + xmlElem.GetAttribute("Name");
						masterBrandAndSerialJSon.AppendFormat("\"{0}\":\"{1}\",", "name", masterBrandName);
						masterBrandAndSerialJSon.Append("carSerial:");
						masterBrandAndSerialJSon.Append(AppendNodeSerialJson(xmlElem));

						masterBrandAndBrandJSon.AppendFormat("\"{0}\":\"{1}\",", "id", xmlElem.GetAttribute("ID"));
						masterBrandAndBrandJSon.AppendFormat("\"{0}\":\"{1}\",", "name", masterBrandName);
						masterBrandAndBrandJSon.Append("carBrand:");
						masterBrandAndBrandJSon.Append(AppendNodeBrandJson(xmlElem));
						//如果是最后一个主品牌
						if (i + 1 == iMasterBrandAndSerialLength)
						{
							masterBrandAndSerialJSon.Append("}");
							masterBrandAndBrandJSon.Append("}");
							continue;
						}
						masterBrandAndSerialJSon.Append("},");
						masterBrandAndBrandJSon.Append("},");
					}

					masterBrandAndSerialJSon.Append("]}");
					masterBrandAndBrandJSon.Append("]}");

					return masterBrandAndSerialJSon.ToString() + ";" + masterBrandAndBrandJSon.ToString();
				}
				catch
				{
					return "";
				}
			}
			/// <summary>
			/// 拼接子品牌列表
			/// </summary>
			/// <param name="xmlElem"></param>
			/// <returns></returns>
			private string AppendNodeSerialJson(XmlElement xmlElem)
			{
				StringBuilder returnJson = new StringBuilder("[");
				//拼接子品牌
				foreach (XmlElement xBrandElement in xmlElem.ChildNodes)
				{
					foreach (XmlElement xSerialElem in xBrandElement.ChildNodes)
					{
						returnJson.Append("{");
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "id", xSerialElem.GetAttribute("ID"));
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "name", xSerialElem.GetAttribute("ShowName"));
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "allspell", xSerialElem.GetAttribute("AllSpell"));
						//添加品牌ID
						returnJson.AppendFormat("\"{0}\":\"{1}\"", "brandid", xBrandElement.GetAttribute("ID"));
						returnJson.Append("},");
					}
				}
				returnJson.Remove(returnJson.Length - 1, 1);
				returnJson.Append("]");
				return returnJson.ToString();
			}
			/// <summary>
			/// 拼接品牌列表
			/// </summary>
			/// <param name="xmlElem"></param>
			/// <returns></returns>
			private string AppendNodeBrandJson(XmlElement xmlElem)
			{
				List<XmlElement> brandList = new List<XmlElement>();
				foreach (XmlElement xBrandElement in xmlElem.ChildNodes)
				{
					brandList.Add(xBrandElement);
				}
				brandList.Sort(NodeCompare.CompareBrandNodeSelfFirst);
				StringBuilder returnJson = new StringBuilder("[");
				//拼接品牌
				foreach (XmlElement xBrandElement in brandList)
				{
					returnJson.Append("{");
					returnJson.AppendFormat("\"{0}\":\"{1}\",", "id", xBrandElement.GetAttribute("ID"));
					returnJson.AppendFormat("\"{0}\":\"{1}\"", "name", xBrandElement.GetAttribute("Name"));
					returnJson.Append("},");
				}
				returnJson.Remove(returnJson.Length - 1, 1);
				returnJson.Append("]");
				return returnJson.ToString();
			}
			/// <summary>
			/// 拼接子品牌列表
			/// </summary>
			/// <returns>返回列表字符串</returns>
			private string AppendNodeJson(XmlNodeList xmlNodeList)
			{
				if (xmlNodeList == null || xmlNodeList.Count < 1)
				{
					return "[]";
				}
				try
				{
					int xmlNodeLength = 0;
					xmlNodeLength = xmlNodeList.Count;
					StringBuilder returnJson = new StringBuilder("[");

					for (int i = 0; i < xmlNodeLength; i++)
					{
						if (i == xmlNodeLength - 1)
						{
							returnJson.Append("{");
							returnJson.AppendFormat("\"{0}\":\"{1}\",", "id", xmlNodeList[i].Attributes["ID"].InnerText.ToString());
							returnJson.AppendFormat("\"{0}\":\"{1}\",", "name", xmlNodeList[i].Attributes["Name"].InnerText.ToString());
							returnJson.AppendFormat("\"{0}\":\"{1}\"", "allspell", xmlNodeList[i].Attributes["AllSpell"].InnerText.ToString());
							returnJson.Append("}");
							continue;
						}
						returnJson.Append("{");
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "id", xmlNodeList[i].Attributes["ID"].InnerText.ToString());
						returnJson.AppendFormat("\"{0}\":\"{1}\",", "name", xmlNodeList[i].Attributes["Name"].InnerText.ToString());
						returnJson.AppendFormat("\"{0}\":\"{1}\"", "allspell", xmlNodeList[i].Attributes["AllSpell"].InnerText.ToString());
						returnJson.Append("},");
						continue;
					}
					returnJson.Append("]");
					return returnJson.ToString();
				}
				catch
				{
					return "[]";
				}
			}
			/// <summary>
			/// 代理请求XML文件
			/// </summary>
			/// <returns>返回XML字符串</returns>
			private string RequestXMLFile()
			{
				string returnString = "-1";

				if (m_RequestUrl == null || m_RequestUrl == "")
				{
					return returnString;
				}

				try
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(m_RequestUrl);
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					using (Stream receiveStream = httpWebResponse.GetResponseStream())
					{
						Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
						using (StreamReader readStream = new StreamReader(receiveStream, encode))
						{
							StringBuilder tempStr = new StringBuilder();
							while (!readStream.EndOfStream)
							{
								tempStr.Append(readStream.ReadLine());
							}
							returnString = tempStr.ToString();
						}
					}
					httpWebResponse.Close();

				}
				catch
				{
					returnString = "-1";
				}
				return returnString;
			}
		}
	}
}