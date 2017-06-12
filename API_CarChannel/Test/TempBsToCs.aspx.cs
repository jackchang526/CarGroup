using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.Caching;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.Test
{
	public partial class TempBsToCs : PageBase
	{
		protected string HtmlBsToCs = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			if (!this.IsPostBack)
			{
				GetMasterToSerial();
			}
		}

		private void GetMasterToSerial()
		{
			string cachekey = "TempBsToCs_GetMasterToSerial";
			XmlDocument autoXml = (XmlDocument)CacheManager.GetCachedData(cachekey);
			if (autoXml == null)
			{
				string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\MasterToBrandToSerialAllSaleAndLevel.xml");
				if (File.Exists(xmlPath))
				{
					autoXml = CommonFunction.ReadXmlFromFile(xmlPath);

					//add by sk 2013.04.26 增加文件读取失败，换数据源
					if (autoXml == null || !autoXml.HasChildNodes)
					{
						autoXml = CommonFunction.ReadXml(WebConfig.BaseAllAutoDataAndLevelUrl);
					}
					CacheDependency cacheDependency = new CacheDependency(xmlPath);
					CacheManager.InsertCache(cachekey, autoXml, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			if (autoXml != null && autoXml.HasChildNodes)
			{
				StringBuilder sb = new StringBuilder();
				XmlNodeList masterNodes = autoXml.SelectNodes("/Params/MasterBrand");
				if (masterNodes != null && masterNodes.Count > 0)
				{
					foreach (XmlNode bsNode in masterNodes)
					{
						XmlNodeList cbNodes = bsNode.SelectNodes("Brand");
						if (cbNodes != null && cbNodes.Count > 0)
						{
							foreach (XmlNode cbNode in cbNodes)
							{
								XmlNodeList csNodes = cbNode.SelectNodes("Serial");
								if (csNodes != null && csNodes.Count > 0)
								{
									foreach (XmlNode csNode in csNodes)
									{
										sb.AppendLine(string.Format("<tr style=\"background-color: #F5FAFF;\"><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td></tr>"
											, bsNode.Attributes["ID"].Value
											, bsNode.Attributes["Name"].Value
											, cbNode.Attributes["ID"].Value
											, cbNode.Attributes["Name"].Value
											, csNode.Attributes["ID"].Value
											, csNode.Attributes["Name"].Value
											, csNode.Attributes["ShowName"].Value)
											);
									}
								}
							}
						}
					}
				}
				HtmlBsToCs = sb.ToString();
			}
		}

	}
}