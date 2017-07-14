using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Web;
using System.Web.Caching;
using System.Text.RegularExpressions;
using System.Configuration;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.CarTreeData
{
	public class DataNode
	{
		public string BrandType;
		public int Id;
		public string Name;
		public int Count;
		public string AllSpell;
		public char FirstChar;
		public string SaleState;
		public string Subsidies;

		/// <summary>
		/// 树形接口请求超时时间
		/// </summary>
		private static int LeftTreeRequestTimeOut
		{
			get {
				int timeout = ConvertHelper.GetInteger(ConfigurationManager.AppSettings["LeftTreeUrlTimeOut"]);
				if (timeout == 0)
					timeout = 10000;
				return timeout;
			}
		}

		public DataNodeCollection ChildNodeList;

		#region 车型数据源直接读取数据源 modified by sk 2013.04.01
		//车型数据源直接读取数据源
		public static string GetCarXmlData(string tagType)
		{
			string cacheKey = string.Format("car_tree_xmlData_autodata_{0}", tagType);
			string xmlCarData = (string)CacheManager.GetCachedData(cacheKey);
			if (xmlCarData == null)
			{
				Dictionary<int, int> serialsHaveSubsidy = new Car_SerialBll().GetSubsidiesSerialList();

				TreeData treeData = new TreeFactory().GetTreeDataObject(tagType);
				string xmlData = treeData.TreeXmlData();

				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlData);
				//有补贴的子品牌加上属性"subsidies"，属性值为"1"
				foreach (XmlElement serialNode in xmlDocument.SelectNodes("//serial"))
				{
					int serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("id"));
					if (serialId > 0 && serialsHaveSubsidy != null && serialsHaveSubsidy.ContainsKey(serialId))
					{
						serialNode.SetAttribute("subsidies", "1");
					}
				}
				xmlCarData = xmlDocument.InnerXml;
				CacheManager.InsertCache(cacheKey, xmlCarData, WebConfig.CachedDuration);
			}
			return xmlCarData;
		}
		#endregion

		/// <summary>
		/// 左侧树形数据，先从接口读取数据，如果接口读取超时，再从本地文件读取
		/// </summary>
		/// <param name="tagType"></param>
		/// <returns></returns>
		public static DataNodeCollection GetDataCollectionFromUrl(string tagType)
		{
			string cacheKey = "car_tree_xmlData_fromUrl_" + tagType;
			DataNodeCollection masterList = (DataNodeCollection)CacheManager.GetCachedData(cacheKey);
			if (masterList == null)
			{
				string url = ConfigurationManager.AppSettings["LeftTreeUrl_" + tagType];
				if (string.IsNullOrWhiteSpace(url))
					return null;

				XmlDocument xmlDoc = CommonFunction.ReadXmlFromUrl(url, LeftTreeRequestTimeOut);
				if (xmlDoc == null || string.IsNullOrWhiteSpace(xmlDoc.OuterXml))
					return null;
				StringReader strReader = new StringReader(xmlDoc.OuterXml);
				XmlReader reader = XmlReader.Create(strReader);
				if (reader == null)
					return null;

				masterList = GetDataCollectionFromXmlReader(reader);
				CacheManager.InsertCache(cacheKey, masterList, 10);
			}
			return masterList;
		}

		public static DataNodeCollection GetDataCollection(string tagType)
		{
			string cacheKey = "car_tree_xmlData_Collection_" + tagType;
			DataNodeCollection masterList = (DataNodeCollection)CacheManager.GetCachedData(cacheKey);
			if (masterList == null)
			{
				masterList = new DataNodeCollection();

				//从文件中分析
				var dataFile = Path.Combine(WebConfig.DataBlockPath, "Data\\CarTree\\TreeData\\" + tagType + ".xml");
				if (File.Exists(dataFile))
				{
					XmlReader reader = null;
					if (tagType == "chexing") //车型直接从数据源读取数据
					{
						string xmlData = GetCarXmlData(tagType);
						reader = XmlReader.Create(new StringReader(xmlData));
					}
					else
					{
						//modified by sk 2013.05.08 文件占用情况，从memcache中取数据
						//reader = XmlReader.Create(dataFile);
						reader = CommonFunction.ReadXmlReaderFromFile(dataFile);
						if (reader == null)
							return masterList;
					}

					masterList = GetDataCollectionFromXmlReader(reader);
					//reader.Close();

					//存入缓存
					CacheDependency cacheDependency = new CacheDependency(dataFile);
					CacheManager.InsertCache(cacheKey, masterList, cacheDependency, DateTime.Now.AddDays(1));
				}
			}

			return masterList;
		}

		/// <summary>
		/// 解析XmlReader，获取DataNodeCollection
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private static DataNodeCollection GetDataCollectionFromXmlReader(XmlReader reader)
		{
			if (reader == null)
				return null;
			DataNodeCollection masterList = new DataNodeCollection();
			DataNodeCollection curBrandList = null;
			DataNodeCollection curSerialList = null;
			string preLevelNodeName = String.Empty;
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element)
					continue;
				string readerName = reader.LocalName.ToLower();
				if (readerName == "master" || readerName == "brand" || readerName == "serial")
				{
					DataNode dataNode = new DataNode();
					dataNode.BrandType = readerName;
					reader.MoveToAttribute("id");
					dataNode.Id = ConvertHelper.GetInteger(reader.Value);
					reader.MoveToAttribute("name");
					dataNode.Name = reader.Value.Trim();
					reader.MoveToAttribute("countnum");
					dataNode.Count = ConvertHelper.GetInteger(reader.Value);
					reader.MoveToAttribute("firstchar");
					if (String.IsNullOrEmpty(reader.Value))
						dataNode.FirstChar = '0';
					else
						dataNode.FirstChar = reader.Value.ToUpper()[0];
					reader.MoveToAttribute("extra");
					dataNode.AllSpell = reader.Value;

					if (reader.MoveToAttribute("salestate"))
						dataNode.SaleState = reader.Value;
					if (reader.MoveToAttribute("subsidies"))
						dataNode.Subsidies = reader.Value;

					switch (readerName)
					{
						case "master":
							preLevelNodeName = "master";
							masterList.Add(dataNode);
							dataNode.ChildNodeList = new DataNodeCollection();
							curBrandList = dataNode.ChildNodeList;
							break;
						case "brand":
							preLevelNodeName = "brand";
							curBrandList.Add(dataNode);
							dataNode.ChildNodeList = new DataNodeCollection();
							curSerialList = dataNode.ChildNodeList;
							break;
						case "serial":
							//因为有些主品牌下直接为子品牌，所以需要跳过品牌这一级
							if (preLevelNodeName == "master")
								curBrandList.Add(dataNode);
							else
								curSerialList.Add(dataNode);
							break;
					}
				}
			}
			reader.Close();
			return masterList;
		}
	}

	public class TagUrl
	{
		public string UrlType;
		public string UrlRule;
		public string OtherParas;
		public string[] OtherParaNames;
		public string EndString;
	}

	/// <summary>
	/// 获取各标签的URL设置
	/// </summary>
	public class TagData
	{
		public string Title;
		public string TagType;
		public string BaseUrl;
		public Dictionary<string, TagUrl> UrlDictionary;

		public static Dictionary<string, TagData> GetTagDataDictionary()
		{
			string cacheKey = "TreeTagData_Dictionary_byTagType";
			Dictionary<string, TagData> tagDic = (Dictionary<string, TagData>)CacheManager.GetCachedData(cacheKey);
			if (tagDic == null)
			{
				tagDic = new Dictionary<string, TagData>();
				string tagFile = Path.Combine(WebConfig.DataBlockPath, "Data\\CarTree\\treeTagUrl.xml");
				XmlReader reader = XmlReader.Create(tagFile);
				Dictionary<string, TagUrl> tagUrlDic = null;
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element)
						continue;
					string readerName = reader.LocalName.ToLower();
					if (readerName == "tag")
					{
						TagData tag = new TagData();
						reader.MoveToAttribute("title");
						tag.Title = reader.Value.Trim();
						reader.MoveToAttribute("type");
						tag.TagType = reader.Value.Trim().ToLower();
						reader.MoveToAttribute("baseUrl");
						tag.BaseUrl = reader.Value.Trim().ToLower();
						tagDic[tag.TagType] = tag;
						tag.UrlDictionary = new Dictionary<string, TagUrl>();
						tagUrlDic = tag.UrlDictionary;
					}
					else if (readerName == "url")
					{
						TagUrl tagUrl = new TagUrl();
						reader.MoveToAttribute("type");
						tagUrl.UrlType = reader.Value.Trim().ToLower();
						reader.MoveToAttribute("urlRule");
						tagUrl.UrlRule = reader.Value.Trim().ToLower();
						if (reader.MoveToAttribute("otherParas"))
						{
							tagUrl.OtherParas = reader.Value.Trim().ToLower();
							//分析都有什么参数
							Regex regx = new Regex(@"@(.+?)@");
							MatchCollection matches = regx.Matches(tagUrl.OtherParas);
							List<string> paraNames = new List<string>();
							foreach (Match match in matches)
							{
								paraNames.Add(match.Result("$1").ToLower());
							}
							tagUrl.OtherParaNames = paraNames.ToArray();
						}
						else
							tagUrl.OtherParas = String.Empty;
						if (reader.MoveToAttribute("endStr"))
						{
							tagUrl.EndString = reader.Value.ToLower();
						}
						else
							tagUrl.EndString = String.Empty;
						tagUrlDic[tagUrl.UrlType] = tagUrl;
					}
				}
				reader.Close();

				//存入缓存中
				CacheDependency cacheDependency = new CacheDependency(tagFile);
				CacheManager.InsertCache(cacheKey, tagDic, cacheDependency, DateTime.Now.AddDays(1));
			}
			return tagDic;
		}
	}

	public class DataNodeCollection : ICollection<DataNode>
	{
		private List<DataNode> m_nodeList;
		private Dictionary<int, DataNode> m_nodeDic;
		private Dictionary<char, int> m_charDic;
		public DataNodeCollection()
		{
			m_nodeDic = new Dictionary<int, DataNode>();
			m_nodeList = new List<DataNode>();
			m_charDic = new Dictionary<char, int>();
		}

		public DataNode this[int index]
		{
			get
			{
				if (index >= m_nodeList.Count || index < 0)
					throw (new IndexOutOfRangeException("访问数据超出索引最大值！"));
				return m_nodeList[index];
			}
		}

		public DataNode GetDataNodeById(int dataId)
		{
			if (m_nodeDic.ContainsKey(dataId))
				return m_nodeDic[dataId];
			else
				return null;
		}

		#region ICollection<DataNode> 成员

		public void Add(DataNode item)
		{
			if (m_nodeDic.ContainsKey(item.Id))
				throw (new Exception("已经存在该对象！"));
			m_nodeDic[item.Id] = item;
			m_nodeList.Add(item);
			m_charDic[item.FirstChar] = 1;
		}

		public void Clear()
		{
			m_nodeDic.Clear();
			m_nodeList.Clear();
			m_charDic.Clear();
		}

		public bool Contains(DataNode item)
		{
			return m_nodeDic.ContainsKey(item.Id);
		}

		/// <summary>
		/// 是否包含某字符
		/// </summary>
		/// <param name="firstChar"></param>
		/// <returns></returns>
		public bool ContainsChar(char firstChar)
		{
			return m_charDic.ContainsKey(firstChar);
		}

		public void CopyTo(DataNode[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { return m_nodeList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(DataNode item)
		{
			if (m_nodeDic.ContainsKey(item.Id))
			{
				m_nodeDic.Remove(item.Id);
				m_nodeList.Remove(item);
				return true;
			}
			else
				return false;
		}

		#endregion

		#region IEnumerable<DataNode> 成员

		public IEnumerator<DataNode> GetEnumerator()
		{
			return m_nodeList.GetEnumerator();
		}

		#endregion

		#region IEnumerable 成员

		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_nodeList.GetEnumerator();
		}

		#endregion
	}



}
