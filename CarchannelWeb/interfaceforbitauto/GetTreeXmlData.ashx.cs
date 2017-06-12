using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// GetTreeXmlData 的摘要说明
	/// 调用方式：http://car.bitauto.com/interfaceforbitauto/GetTreeXmlData.ashx?tagType={tagType}
	/// 例如：http://car.bitauto.com/interfaceforbitauto/GetTreeXmlData.ashx?tagType=chexing
	/// </summary>
	public class GetTreeXmlData : InterfacePageBase, IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/xml";
			string tagType = GetTagType(context);
			string xmlData = GetXmlData(tagType);
			context.Response.Write(xmlData);
		}


		/// <summary>
		/// 参数名
		/// </summary>
		const string TAGTYPE_PARAMETER_NAME = "tagType";

		/// <summary>
		/// 有补贴的子品牌
		/// 键为子品牌id
		/// </summary>
		private Dictionary<int, int> serialsHaveSubsidy = null;

		/// <summary>
		/// 是否有补贴
		/// </summary>
		/// <param name="serialBrandId">子品牌id</param>
		/// <returns></returns>
		private bool HasSubsidy(int serialBrandId)
		{
			if (serialsHaveSubsidy == null)
			{
				serialsHaveSubsidy = new Car_SerialBll().GetSubsidiesSerialList();
			}
			// modified by chengl Sep.30.2011
			if (serialsHaveSubsidy != null)
			{
				return serialsHaveSubsidy.ContainsKey(serialBrandId);
			}
			else
			{
				return false;
			}
			// return serialsHaveSubsidy.ContainsKey(serialBrandId);
		}

		/// <summary>
		/// 是否有补贴
		/// </summary>
		/// <param name="serialBrandId">子品牌id</param>
		/// <returns></returns>
		private bool HasSubsidy(string serialBrandId)
		{
			int serialId = 0;
			if (int.TryParse(serialBrandId, out serialId))
			{
				return HasSubsidy(serialId);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 从参数中获取标签名称
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private string GetTagType(HttpContext context)
		{
			return context.Request.Params[TAGTYPE_PARAMETER_NAME];
		}

		private string GetXmlData(string tagType)
		{
			if (string.IsNullOrEmpty(tagType))
				return string.Empty;

			TreeData treeData = new TreeFactory().GetTreeDataObject(tagType);
			string xmlData = treeData.TreeXmlData();
			return ProcessXmlData(xmlData);
		}

		/// <summary>
		/// 处理xml数据
		/// </summary>
		/// <param name="xmlData"></param>
		/// <returns></returns>
		private string ProcessXmlData(string xmlData)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlData);

			//有补贴的子品牌加上属性"subsidies"，属性值为"1"
			foreach (XmlElement serialNode in xmlDocument.SelectNodes("//serial"))
			{
				string serialId = serialNode.GetAttribute("id");
				if (!string.IsNullOrEmpty(serialId) && HasSubsidy(serialId))
				{
					serialNode.SetAttribute("subsidies", "1");
				}
			}
			return xmlDocument.InnerXml;
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