using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using System.Text;
using BitAuto.Utils;


namespace WirelessWeb
{
	public class WirelessPageBase : BitAuto.CarChannel.Common.PageBase
	{

		/// <summary>
		/// 取车型综述页广告配置
		/// </summary>
		/// <returns></returns>
		protected Dictionary<int, ADLink> GetAllCarSummaryAD()
		{
			string cachekey = "WirelessPageBase_GetAllCarSummaryAD";
			Dictionary<int, ADLink> dicCarAD = (Dictionary<int, ADLink>)CacheManager.GetCachedData(cachekey);
			if (dicCarAD == null)
			{
				string path = HttpContext.Current.Server.MapPath("~/App_Data/CarSummaryAD.xml");
				if (File.Exists(path))
				{
					XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
					dicCarAD = new Dictionary<int, ADLink>();
					XmlNodeList nodeList = xmlDoc.SelectNodes("/Root/AD");
					if (nodeList != null && nodeList.Count > 0)
					{
						foreach (XmlNode node in nodeList)
						{
							string title = node.SelectSingleNode("Title").InnerText.Trim();
							string link = node.SelectSingleNode("Link").InnerText.Trim();
							ADLink adLink = new ADLink();
							adLink.Title = title;
							adLink.Link = link;
							string ids = node.SelectSingleNode("IDs").InnerText.Trim();
							if (ids != "")
							{
								string[] idArray = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
								if (idArray.Length > 0)
								{
									foreach (string idStr in idArray)
									{
										int id = 0;
										if (int.TryParse(idStr, out id))
										{
											if (id > 0 && !dicCarAD.ContainsKey(id))
											{ dicCarAD.Add(id, adLink); }
										}
									}
								}
							}
						}
					}
					CacheManager.InsertCache(cachekey, dicCarAD, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return dicCarAD;
		}

		/// <summary>
		/// 取子品牌综述页广告配置
		/// </summary>
		/// <returns></returns>
		protected Dictionary<int, ADLink> GetAllSerialSummaryAD()
		{
			string cachekey = "WirelessPageBase_GetAllSerialSummaryAD";
			Dictionary<int, ADLink> dicCsAD = (Dictionary<int, ADLink>)CacheManager.GetCachedData(cachekey);
			if (dicCsAD == null)
			{
				string path = HttpContext.Current.Server.MapPath("~/App_Data/CsSummaryAD.xml");
				if (File.Exists(path))
				{
					XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
					dicCsAD = new Dictionary<int, ADLink>();
					XmlNodeList nodeList = xmlDoc.SelectNodes("/Root/AD");
					if (nodeList != null && nodeList.Count > 0)
					{
						foreach (XmlNode node in nodeList)
						{
							string title = node.SelectSingleNode("Title").InnerText.Trim();
							string link = node.SelectSingleNode("Link").InnerText.Trim();
							ADLink adLink = new ADLink();
							adLink.Title = title;
							adLink.Link = link;
							string ids = node.SelectSingleNode("IDs").InnerText.Trim();
							if (ids != "")
							{
								string[] idArray = ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
								if (idArray.Length > 0)
								{
									foreach (string idStr in idArray)
									{
										int id = 0;
										if (int.TryParse(idStr, out id))
										{
											if (id > 0 && !dicCsAD.ContainsKey(id))
											{ dicCsAD.Add(id, adLink); }
										}
									}
								}
							}
						}
					}
					CacheManager.InsertCache(cachekey, dicCsAD, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return dicCsAD;
		}

		/// <summary>
		/// 所有配置了主品牌ID 的车贷指定广告，广告地址去
		/// </summary>
		/// <returns></returns>
		protected List<int> GetAllMasterAD()
		{
			string cachekey = "WirelessPageBase_GetAllMasterAD";
			List<int> list = (List<int>)CacheManager.GetCachedData(cachekey);
			if (list == null)
			{
				string path = HttpContext.Current.Server.MapPath("~/App_Data/MasterCheDaiAD.xml");
				if (File.Exists(path))
				{
					XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
					list = new List<int>();
					XmlNodeList nodeList = xmlDoc.SelectNodes("/Root/BsID");
					if (nodeList != null && nodeList.Count > 0)
					{
						foreach (XmlNode xn in nodeList)
						{
							int bsid = 0;
							if (int.TryParse(xn.InnerText, out bsid))
							{
								if (bsid > 0 && !list.Contains(bsid))
								{ list.Add(bsid); }
							}
						}
					}
					CacheManager.InsertCache(cachekey, list, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
				}
			}
			return list;
		}

		/// <summary>
		/// link 类型广告
		/// </summary>
		protected class ADLink
		{
			/// <summary>
			/// 广告A标签标题
			/// </summary>
			public string Title;
			/// <summary>
			/// 广告A标签地址
			/// </summary>
			public string Link;
		}
        /// <summary>
        /// 截取指定长度字符串(按字节算)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string StrCut(string str, int length)
        {
            int len = 0;
            byte[] b;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;

                if (len > length)
                {
                    sb.Append("...");
                    break;
                }

                sb.Append(str[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 取车型综述页看了又看配置
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, XmlNode> GetAllSerialToSerial()
        {
            string cachekey = "WirelessPageBase_GetAllSerialToSerial";
            Dictionary<int, XmlNode> dicSerialToSerial = (Dictionary<int, XmlNode>)CacheManager.GetCachedData(cachekey);
            if (dicSerialToSerial == null)
            {
                string path = HttpContext.Current.Server.MapPath("~/config/SerialToSee.xml");
                if (File.Exists(path))
                {
                    XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
                    dicSerialToSerial = new Dictionary<int, XmlNode>();
                    XmlNodeList nodeList = xmlDoc.SelectNodes("/Root/Serial");
                    if (nodeList != null && nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            int serialId = ConvertHelper.GetInteger(node.Attributes["Id"].Value);
                            XmlNodeList itemNodeList = node.SelectNodes("Item");
                           
                            if (itemNodeList != null && itemNodeList.Count > 0 && serialId > 0 && !dicSerialToSerial.ContainsKey(serialId))
                            {
                                dicSerialToSerial.Add(serialId, node);                                
                            }                           
                        }
                    }
                    CacheManager.InsertCache(cachekey, dicSerialToSerial, new CacheDependency(path), DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return dicSerialToSerial;
        }
	}
}