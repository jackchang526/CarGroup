using System;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;


namespace BitAuto.CarChannel.BLL
{
	public class ProduceAndSellDataBll
	{
		//��
		private static object objTreeLock = new object();

		/// <summary>
		/// ��ȡĳ�µ���������ǰ10
		/// </summary>
		/// <param name="dataDate">ʱ��</param>
		/// <param name="dataType">��������</param>
		/// <param name="needNewData">�Ƿ��ȡ��������</param>
		/// <returns></returns>
		public string GetSellDataXml(DateTime dataDate, string dataType)
		{
			bool needNewData = false;
			List<DateTime> lastMonths = GetLastMonths();
			if (dataDate == DateTime.MinValue)
			{
				needNewData = true;
				dataDate = lastMonths[0];
			}
			dataDate = new DateTime(dataDate.Year, dataDate.Month, 1);

			List<int> levelList = new List<int>();
			dataType = dataType.Trim().ToLower();
			if (dataType == "suv")
				levelList.Add(424);
			else if (dataType == "mpv")
				levelList.Add(425);
			else if (dataType == "weixingche")
				levelList.Add(321);
			else if (dataType == "xiaoxingche")
				levelList.Add(338);
			else if (dataType == "jincouxingche")
				levelList.Add(339);
			else if (dataType == "zhongxingche")
				levelList.Add(340);
			else if (dataType == "zhongdaxingche")
				levelList.Add(341);
			else if (dataType == "haohuache")
				levelList.Add(342);
			else
			{
				dataType = "car";
				levelList.Add(321);		//΢�ͳ�
				levelList.Add(338);		//С�ͳ�
				levelList.Add(339);		//�����ͳ�
				levelList.Add(340);		//���ͳ�
				levelList.Add(341);		//�д��ͳ�
				levelList.Add(342);		//������
			}
			string cacheKey = "SellStatisticData_" + dataType + "_" + dataDate.ToString("yyyyMMdd");

			XmlDocument cacheData = (XmlDocument)CacheManager.GetCachedData(cacheKey);
			if (cacheData == null)
			{
				cacheData = GetSellDataXmlFromFile(dataDate, levelList, cacheKey, needNewData);
				if (cacheData != null)
				{
					string lastMonthData = "";
					foreach (DateTime dt in lastMonths)
					{
						lastMonthData += dt.ToString("yyyy-MM-dd") + "|";
					}
					lastMonthData = lastMonthData.Trim('|');
					XmlElement rootEle = (XmlElement)cacheData.SelectSingleNode("/SellDataList");
					if (rootEle != null)
					{
						rootEle.SetAttribute("historyData", lastMonthData);
					}
					CacheManager.InsertCache(cacheKey, cacheData, WebConfig.CachedDuration);
				}
			}

			return cacheData.OuterXml;
		}

		/// <summary>
		/// ��ȡĳ�µ���������ǰ10
		/// </summary>
		/// <param name="dataDate">ʱ��</param>
		/// <param name="levelList">��ѯ�ļ����б�</param>
		/// <param name="cacheKey">���������˴������ļ���</param>
		/// <returns></returns>
		private XmlDocument GetSellDataXmlFromFile(DateTime dataDate, List<int> levelList, string cacheKey, bool needNewData)
		{
			XmlDocument xmlDoc = null;

			string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\CarData\\" + cacheKey + ".xml");
			//�Ƿ���Ҫ��������
			bool needUpdate = false;
			if (File.Exists(fileName))
			{
				XmlDocument oldXmlDoc = new XmlDocument();
				oldXmlDoc.Load(fileName);

				if (needNewData)
				{
					XmlNode timeNode = oldXmlDoc.SelectSingleNode("/SellDataList/Time");
					string oldXmlDate = Convert.ToString(timeNode.InnerText);
					if (oldXmlDate != DateTime.Now.ToString("yyyy-MM-dd"))
						needUpdate = true;
				}

				xmlDoc = oldXmlDoc;
			}
			else
				needUpdate = true;

			if (needUpdate)
			{
				xmlDoc = RefreshSellDataXml(dataDate, levelList, cacheKey, fileName);
			}

			return xmlDoc;
		}

		/// <summary>
		/// ˢ�²������ݵ�Xml����
		/// </summary>
		/// <param name="dataDate"></param>
		/// <param name="levelList"></param>
		/// <param name="cacheKey"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private XmlDocument RefreshSellDataXml(DateTime dataDate, List<int> levelList, string cacheKey, string fileName)
		{
			XmlDocument xmlDoc = null;

			//string mutexName = cacheKey;
			//Mutex tMutex = new Mutex(false, mutexName);
			//bool isTrue = false;
			//try
			//{
			//    isTrue = tMutex.WaitOne(0, false);
			//}
			//catch (AbandonedMutexException)
			//{
			//    isTrue = true;
			//}
			//if (isTrue)
			//{
			#region ��ȡͳ������
			Dictionary<string, DataTable> sellData = new ProduceAndSellDataDal().GetSellData(dataDate, levelList);
			string curDate = dataDate.ToString("yyyy-MM-01");
			//string curDate = GetLastMonth().ToString("yyyy-MM-01");
			string preMonthDate = dataDate.AddMonths(-1).ToString("yyyy-MM-01");
			string preYearDate = dataDate.AddYears(-1).ToString("yyyy-MM-01");

			//���кϲ������
			xmlDoc = new XmlDocument();
			//�Ӹ����
			XmlElement root = xmlDoc.CreateElement("SellDataList");
			root.SetAttribute("curDate", curDate);
			Dictionary<int, XmlElement> nodeDic = new Dictionary<int, XmlElement>();
			xmlDoc.AppendChild(root);

			//ʱ����
			XmlElement timeEle = xmlDoc.CreateElement("Time");
			XmlText textNode = xmlDoc.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
			timeEle.AppendChild(textNode);
			root.AppendChild(timeEle);

			foreach (DataRow row in sellData[curDate].Rows)
			{
				int csId = Convert.ToInt32(row["CsId"]);
				string csName = Convert.ToString(row["csShowName"]);
				string csSpell = Convert.ToString(row["allSpell"]);
				int curSell = Convert.ToInt32(row["SellNum"]);
				XmlElement sellNode = xmlDoc.CreateElement("SellData");
				sellNode.SetAttribute("CsId", csId.ToString());
				sellNode.SetAttribute("CsName", csName);
				sellNode.SetAttribute("AllSpell", csSpell);
				sellNode.SetAttribute("CurrentSellData", curSell.ToString());
				sellNode.SetAttribute("preMonthSellData", "0");
				sellNode.SetAttribute("preYearSellData", "0");
				sellNode.SetAttribute("currentCount", "0");
				sellNode.SetAttribute("preYearCount", "0");
				sellNode.SetAttribute("preMonthIncrease", "--");
				sellNode.SetAttribute("preYearIncrease", "--");
				sellNode.SetAttribute("countIncrease", "--");


				root.AppendChild(sellNode);
				nodeDic[csId] = sellNode;
			}

			//����ǰһ�µ�����
			foreach (DataRow row in sellData[preMonthDate].Rows)
			{
				int csId = Convert.ToInt32(row["CsId"]);
				int preMonthSell = Convert.ToInt32(row["SellNum"]);
				if (nodeDic.ContainsKey(csId))
				{
					nodeDic[csId].SetAttribute("preMonthSellData", preMonthSell.ToString());
				}
			}

			//����ǰһ������
			foreach (DataRow row in sellData[preYearDate].Rows)
			{
				int csId = Convert.ToInt32(row["CsId"]);
				int preYearSell = Convert.ToInt32(row["SellNum"]);
				if (nodeDic.ContainsKey(csId))
				{
					nodeDic[csId].SetAttribute("preYearSellData", preYearSell.ToString());
				}
			}

			//�����ۼ�����
			foreach (DataRow row in sellData["CurrentCount"].Rows)
			{
				int csId = Convert.ToInt32(row["CsId"]);
				int sellCount = Convert.ToInt32(row["SellCount"]);
				if (nodeDic.ContainsKey(csId))
				{
					nodeDic[csId].SetAttribute("currentCount", sellCount.ToString());
				}
			}

			//������һ����ۼ�����
			foreach (DataRow row in sellData["PreYearCount"].Rows)
			{
				int csId = Convert.ToInt32(row["CsId"]);
				int sellCount = Convert.ToInt32(row["SellCount"]);
				if (nodeDic.ContainsKey(csId))
				{
					nodeDic[csId].SetAttribute("preYearCount", sellCount.ToString());
				}
			}

			//������������
			foreach (XmlElement sellNode in nodeDic.Values)
			{
				int curSell = GetAttributeValue(sellNode, "CurrentSellData");
				int preMonthSell = GetAttributeValue(sellNode, "preMonthSellData");
				int preYearSell = GetAttributeValue(sellNode, "preYearSellData");
				int curCount = GetAttributeValue(sellNode, "currentCount");
				int preYearCount = GetAttributeValue(sellNode, "preYearCount");
				//����
				string upStr = "";
				double up = 0;
				if (preMonthSell == 0)
					upStr = "--";
				else
				{
					up = (curSell - preMonthSell) / (double)preMonthSell * 10000;
					up = Math.Round(up, MidpointRounding.AwayFromZero);
					upStr = (up / 100) + "%";
				}
				sellNode.SetAttribute("preMonthIncrease", upStr);

				//ͬ��
				if (preYearSell == 0)
					upStr = "--";
				else
				{
					up = (curSell - preYearSell) / (double)preYearSell * 10000;
					up = Math.Round(up, MidpointRounding.AwayFromZero);
					upStr = (up / 100) + "%";
				}
				sellNode.SetAttribute("preYearIncrease", upStr);

				//�ۼ�����
				if (preYearCount == 0)
				{
					upStr = "--";
				}
				else
				{
					up = (curCount - preYearCount) / (double)preYearCount * 10000;
					up = Math.Round(up, MidpointRounding.AwayFromZero);
					upStr = (up / 100) + "%";
				}
				sellNode.SetAttribute("countIncrease", upStr);
			}

			//�����ļ�
			string filePath = Path.GetDirectoryName(fileName);
			if (!Directory.Exists(filePath))
				Directory.CreateDirectory(filePath);

			try
			{
				xmlDoc.Save(fileName);
			}
			catch { }
			#endregion

			//    tMutex.ReleaseMutex();
			//}

			return xmlDoc;
		}

		/// <summary>
		/// ��ȡ���ֵ
		/// </summary>
		/// <param name="sellNode"></param>
		/// <param name="attrName"></param>
		private int GetAttributeValue(XmlElement sellNode, string attrName)
		{
			string attrValue = sellNode.GetAttribute(attrName);
			if (String.IsNullOrEmpty(attrValue))
				attrValue = "0";
			return Convert.ToInt32(attrValue);
		}

		/// <summary>
		/// ��ȡ���̡�Ʒ�ơ���Ʒ�ƵĲ㼶��ϵ
		/// </summary>
		/// <returns></returns>
		public string GetBrandTree()
		{
			string cacheKey = "SellDataBrandTree";
			Object cacheData = CacheManager.GetCachedData(cacheKey);
			if (cacheData == null)
			{
				string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\BrandTree.xml");
				XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(fileName);
				cacheData = xmlDoc.DocumentElement.OuterXml;
				CacheManager.InsertCache(cacheKey, cacheData, WebConfig.CachedDuration);
			}

			return (string)cacheData;
		}



		/// <summary>
		/// ��ѯ��������
		/// </summary>
		/// <param name="pId">����ID </param>
		/// <param name="bId">Ʒ��ID</param>
		/// <param name="sId">��Ʒ��ID</param>
		/// <returns></returns>
		public string GetQueryData(int pId, int bId, int sId)
		{
			string cacheKey = "";

			if (sId != 0)
				cacheKey = "QuerySellData_Serial_" + sId;
			else if (bId != 0)
				cacheKey = "QuerySellData_Brand_" + bId;
			else if (pId != 0)
				cacheKey = "QuerySellData_Producer_" + pId;
			else
				cacheKey = "QuerySellData_Brand_all";

			Object cacheData = CacheManager.GetCachedData(cacheKey);
			if (cacheData == null)
			{
				cacheData = GetQueryDataFromFile(pId, bId, sId, cacheKey);
				CacheManager.InsertCache(cacheKey, cacheData, WebConfig.CachedDuration);
			}
			return (string)cacheData;
		}

		/// <summary>
		/// ��ѯ��������
		/// </summary>
		/// <param name="pId">����ID </param>
		/// <param name="bId">Ʒ��ID</param>
		/// <param name="sId">��Ʒ��ID</param>
		/// <param name="cacheKey">�����</param>
		/// <returns></returns>
		private string GetQueryDataFromFile(int pId, int bId, int sId, string cacheKey)
		{
			string xmlStr = "";
			string mutexName = cacheKey;
			string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\QueryData\\" + mutexName + ".xml");

			bool needUpdate = false;
			if (File.Exists(fileName))
			{
				XmlDocument oldDoc = new XmlDocument();

				//����
				//Mutex tMutex = new Mutex(false, mutexName);
				//tMutex.WaitOne();
				oldDoc.Load(fileName);
				//tMutex.ReleaseMutex();

				XmlElement oldTimeEle = (XmlElement)oldDoc.SelectSingleNode("/root/Time");
				string oldDate = oldTimeEle.InnerText;
				if (oldDate != DateTime.Now.ToString("yyyy-MM-dd"))
				{
					needUpdate = true;
				}
				else
					xmlStr = oldDoc.OuterXml;
				xmlStr = oldDoc.OuterXml;
			}
			else
				needUpdate = true;

			if (needUpdate)
			{
				ProduceAndSellDataDal psDal = new ProduceAndSellDataDal();
				//��ѯ����
				DataSet ds = null;
				if (sId != 0)
					ds = psDal.GetQueryDataBySerial(sId);
				else if (bId != 0)
					ds = psDal.GetQueryDataByBrand(bId);
				else if (pId != 0)
					ds = psDal.GetQueryDataByProducer(pId);
				else
					ds = psDal.GetQueryDataAll();

				//����Xml
				XmlDocument xmlDoc = new XmlDocument();
				XmlElement root = xmlDoc.CreateElement("root");
				xmlDoc.AppendChild(root);
				XmlElement sellDataRoot = xmlDoc.CreateElement("SellData");
				root.AppendChild(sellDataRoot);
				XmlElement timeEle = xmlDoc.CreateElement("Time");
				timeEle.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
				root.AppendChild(timeEle);

				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string monthDate = Convert.ToDateTime(row["DataDate"]).ToString("yyyy-MM");
					string sellCount = Convert.ToString(row["SellCount"]);
					XmlElement rowEle = xmlDoc.CreateElement("r");
					XmlElement dateEle = xmlDoc.CreateElement("d");
					XmlElement sellEle = xmlDoc.CreateElement("s");
					dateEle.InnerText = monthDate;
					sellEle.InnerText = sellCount;
					rowEle.AppendChild(sellEle);
					rowEle.AppendChild(dateEle);
					sellDataRoot.InsertBefore(rowEle, sellDataRoot.FirstChild);
				}

				//��ȡ�������
				string newsXml = GetNewsForProduceAndSellData(pId, bId, sId);
				XmlElement newsEle = xmlDoc.CreateElement("NewsData");
				newsEle.InnerXml = newsXml;
				root.AppendChild(newsEle);

				//�������
				XmlNodeList titleNodes = newsEle.SelectNodes("NewDataSet/listNews/facetitle");
				foreach (XmlElement titleNode in titleNodes)
				{
					string newsTitle = titleNode.InnerText;
					newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
					//newsTitle = StringHelper.SubString(newsTitle, 30,true);
					titleNode.InnerText = newsTitle;
				}

				string filePath = Path.GetDirectoryName(fileName);
				if (!Directory.Exists(filePath))
					Directory.CreateDirectory(filePath);

				//����
				//Mutex tMutex = new Mutex(false, mutexName);
				//tMutex.WaitOne();
				try
				{
					xmlDoc.Save(fileName);
				}
				catch { }
				//tMutex.ReleaseMutex();

				xmlStr = xmlDoc.OuterXml;
			}

			return xmlStr;
		}

		/// <summary>
		/// Ϊ�������ݲ�ѯ����
		/// </summary>
		/// <param name="pId"></param>
		/// <param name="bId"></param>
		/// <param name="sId"></param>
		/// <returns></returns>
		public string GetNewsForProduceAndSellData(int pId, int bId, int sId)
		{
			string xmlFile = "";
			if (sId != 0)
				xmlFile = "Serial\\Serial_" + sId;
			else if (bId != 0)
				xmlFile = "Brand\\Brand_" + bId;
			else if (pId != 0)
				xmlFile = "Producer\\Producer_" + pId;
			else
				xmlFile = "All";
			xmlFile += ".xml";
			xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\" + xmlFile);
			// modify by chengl May.20.2010
			if (File.Exists(xmlFile))
			{
				XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
				return xmlDoc.DocumentElement.OuterXml;
			}
			else
			{ return ""; }
		}

		/// <summary>
		/// ��ѯ�ڲ����������Ƿ���ָ����Ʒ�Ƶ�����
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public bool HasSerialData(int serialId)
		{
			string cacheKey = "BrandTree";
			List<int> serialIds = (List<int>)CacheManager.GetCachedData(cacheKey);
			if (serialIds == null)
			{
				serialIds = new List<int>();
				string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\BrandTree.xml");
				if (File.Exists(fileName))
				{
					using (XmlReader xmlReader = XmlReader.Create(fileName))
					{
						while (xmlReader.ReadToFollowing("Serial"))
						{
							if (xmlReader.MoveToAttribute("id"))
							{
								serialIds.Add(Convert.ToInt32(xmlReader.Value));
							}
						}
					}
				}
				CacheManager.InsertCache(cacheKey, serialIds, 60);
			}
			for (int i = 0; i < serialIds.Count; i++)
			{
				if (serialId == serialIds[i])
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// ��ȡ����������ݵ��·�
		/// </summary>
		/// <returns></returns>
		public List<DateTime> GetLastMonths()
		{
			string cacheKey = "SellDataLast12Months";
			List<DateTime> months = (List<DateTime>)CacheManager.GetCachedData(cacheKey);
			if (months == null)
			{
				months = new ProduceAndSellDataDal().GetLastDataMonths();
				CacheManager.InsertCache(cacheKey, months, 10);
			}
			return months;
		}

		/// <summary>
		/// ��ȡ�������ݵ�ͼ������
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetSellDataMapXml()
		{
			string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\SellDataMap.xml");
			XmlDocument mapDoc = null;
			if (!File.Exists(xmlFile))
				mapDoc = UpdateSellDataMap(xmlFile);
			else
			{
				mapDoc = new XmlDocument();
				mapDoc.Load(xmlFile);
			}
			return mapDoc;
		}

		/// <summary>
		/// ��ȡ������ͼ����
		/// </summary>
		/// <returns></returns>
		public string GetSellDataMap()
		{
			bool isNeedUpdate = false;
			string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\SellDataMap.xml");
			XmlDocument mapDoc = null;
			if (!File.Exists(xmlFile))
				isNeedUpdate = true;
			else
			{
				try
				{
					mapDoc = new XmlDocument();
					mapDoc.Load(xmlFile);
					DateTime oldTime = Convert.ToDateTime(mapDoc.DocumentElement.GetAttribute("updateTime"));
					if (oldTime.AddMinutes(60) < DateTime.Now)
						isNeedUpdate = true;
				}
				catch { }
			}

			if (isNeedUpdate)
				mapDoc = UpdateSellDataMap(xmlFile);

			//�Ƴ���������
			XmlNodeList serialList = mapDoc.SelectNodes("/Data/Month/Level/Serial");
			foreach (XmlElement serialNode in serialList)
			{
				serialNode.RemoveAttribute("ImageUrl");
				serialNode.RemoveAttribute("ImageName");
				//serialNode.RemoveAttribute("ClassId");
			}
			return mapDoc.DocumentElement.OuterXml;
		}

		/// <summary>
		/// ����������ͼ�����ļ�
		/// </summary>
		/// <param name="xmlFile"></param>
		/// <returns></returns>
		private XmlDocument UpdateSellDataMap(string xmlFile)
		{
			XmlDocument mapDoc = new XmlDocument();
			XmlDocument serialDoc = new XmlDocument();
			try
			{
				mapDoc.Load(WebConfig.SellDataMapUrl);

				//add by sk 2013.04.26 �����ļ���ȡʧ�ܣ�������Դ
				//string autoFile = Path.Combine(WebConfig.DataBlockPath, "Data\\AllAutoData.xml");
				//serialDoc.Load(autoFile);
				serialDoc = AutoStorageService.GetAllAutoXml();
				mapDoc.DocumentElement.SetAttribute("updateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				XmlNodeList monthList = mapDoc.SelectNodes("/Data/Month");
				//����������Ʒ�Ƶļ���
				foreach (XmlElement monthNode in monthList)
				{
					Dictionary<string, XmlElement> levelNodeDic = new Dictionary<string, XmlElement>();
					for (int i = monthNode.ChildNodes.Count - 1; i >= 0; i--)
					{
						XmlElement serialNode = (XmlElement)monthNode.ChildNodes[i];
						monthNode.RemoveChild(serialNode);
						int serialId = Convert.ToInt32(serialNode.GetAttribute("SerialId"));
						//ȡ����ȫƴ
						XmlElement templNode = (XmlElement)serialDoc.SelectSingleNode("/Params/MasterBrand/Brand/Serial[@ID=\"" + serialId + "\"]");
						if (templNode == null)
							continue;
						string level = templNode.GetAttribute("CsLevel");
						string spell = templNode.GetAttribute("AllSpell").ToLower();
						string showName = templNode.GetAttribute("ShowName");
						serialNode.SetAttribute("allSpell", spell);
						serialNode.SetAttribute("SerialName", showName);
						serialNode.RemoveAttribute("ClassId");
						if (!levelNodeDic.ContainsKey(level))
						{
							XmlElement levelRoot = mapDoc.CreateElement("Level");
							levelRoot.SetAttribute("level", level);
							monthNode.AppendChild(levelRoot);
							levelNodeDic[level] = levelRoot;
						}
						levelNodeDic[level].AppendChild(serialNode);
					}

					foreach (XmlElement levelNode in levelNodeDic.Values)
					{
						monthNode.AppendChild(levelNode);
					}
				}

				mapDoc.Save(xmlFile);
			}
			catch { }
			return mapDoc;
		}

		#region ��ȡ���̡�Ʒ�ơ���Ʒ�ƵĲ㼶��ϵ������json�ַ���
		/// <summary>
		/// ��ȡ���̡�Ʒ�ơ���Ʒ�ƵĲ㼶��ϵ������json�ַ���
		/// </summary>
		/// <returns></returns>
		public string GetBrandTreeToJson()
		{
			string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\BrandTree.xml");
			StringBuilder result = new StringBuilder();
			if (File.Exists(fileName))
			{
				using (XmlReader reader = XmlReader.Create(fileName))
				{
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "Producer")
						{
							result.Append(",{");
							result.AppendFormat("id:\"{0}\",name:\"{1}\",Brands:[{2}]", reader["id"], reader["name"], GetBrandTreeBrandsToJson(reader));
							result.Append("}");
						}
					}
				}
			}
			return result.Length <= 0 ? "[]" : result.Remove(0, 1).Insert(0, "[").Append("]").ToString();
		}
		/// <summary>
		/// ��ȡ��Ʒ�ƣ�����json�ַ���
		/// </summary>
		/// <returns></returns>
		private string GetBrandTreeBrandsToJson(XmlReader reader)
		{
			if (reader == null)
				return string.Empty;

			StringBuilder result = new StringBuilder();
			using (XmlReader subReader = reader.ReadSubtree())
			{
				while (subReader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "Brand")
					{
						result.Append(",{");
						result.AppendFormat("id:\"{0}\",name:\"{1}\",Serials:[{2}]", reader["id"], reader["name"], GetBrandTreeSerialsToJson(reader));
						result.Append("}");
					}
				}
			}
			return result.Length <= 0 ? string.Empty : result.Remove(0, 1).ToString();
		}
		/// <summary>
		/// ��ȡƷ�ƣ�����json�ַ���
		/// </summary>
		/// <returns></returns>
		private string GetBrandTreeSerialsToJson(XmlReader reader)
		{
			if (reader == null)
				return string.Empty;

			StringBuilder result = new StringBuilder();
			using (XmlReader subReader = reader.ReadSubtree())
			{
				while (subReader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "Serial")
					{
						result.Append(",{");
						result.AppendFormat("id:\"{0}\",name:\"{1}\"", reader["id"], reader["name"]);
						result.Append("}");
					}
				}
			}
			return result.Length <= 0 ? string.Empty : result.Remove(0, 1).ToString();
		}
		#endregion

		#region ��ѯ�������ƣ�����json��ʽ
		/// <summary>
		/// ��ѯ�������ƣ�����json��ʽ
		/// ��������HttpUtility.HtmlEncode��ǰ̨��Ҫdecode
		/// </summary>
		/// <param name="producerId">����ID </param>
		/// <param name="brandId">Ʒ��ID</param>
		/// <param name="serialId">��Ʒ��ID</param>
		/// <returns></returns>
		public string GetQueryDataToJson(int producerId, int brandId, int serialId)
		{
			StringBuilder result = new StringBuilder();
			ProduceAndSellDataDal psDal = new ProduceAndSellDataDal();
			//��ѯ����
			DataSet ds = null;
			if (serialId != 0)
				ds = psDal.GetQueryDataBySerial(serialId);
			else if (brandId != 0)
				ds = psDal.GetQueryDataByBrand(brandId);
			else if (producerId != 0)
				ds = psDal.GetQueryDataByProducer(producerId);
			else
				ds = psDal.GetQueryDataAll();

			#region json��ʽ
			/*
             {
                SellData:[
             *          {s:18130,d:"2010-07"},
             *          {s:20227,d:"2010-08"}
             *      ],
                Time:"2011-07-25",
                listNews:[
             *          {},
             *          
             *      ]
             }
            */

			#endregion

			//����Xml
			result.AppendFormat("Time:\"{0}\"", DateTime.Now.ToString("yyyy-MM-dd"));

			result.Append(",SellDatas:[");
			for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
			{
				DataRow row = ds.Tables[0].Rows[i];
				string monthDate = Convert.ToDateTime(row["DataDate"]).ToString("yyyy-MM");
				string sellCount = Convert.ToString(row["SellCount"]);
				result.Append("{");
				result.AppendFormat("s:\"{0}\",d:\"{1}\"", sellCount, monthDate);
				result.Append("},");
			}
			if (ds.Tables[0].Rows.Count > 0)
				result.Remove(result.Length - 1, 1);
			result.Append("]");

			#region ��ȡ�������
			//��ȡ�������
			string xmlFile = "";
			if (serialId != 0)
				xmlFile = "Serial\\Serial_" + serialId.ToString();
			else if (brandId != 0)
				xmlFile = "Brand\\Brand_" + brandId.ToString();
			else if (producerId != 0)
				xmlFile = "Producer\\Producer_" + producerId.ToString();
			else
				xmlFile = "All";
			xmlFile += ".xml";
			xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\" + xmlFile);
			if (File.Exists(xmlFile))
			{
				result.Append(",listNews:[");
				using (XmlReader reader = XmlReader.Create(xmlFile))
				{
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "listNews")
						{
							result.Append("{");
							using (XmlReader subReader = reader.ReadSubtree())
							{
								if (subReader.Read())
								{
									while (subReader.Read())
									{
										if (reader.NodeType == XmlNodeType.Element)
										{
											if (reader.Name == "newsid" || reader.Name == "filepath")
											{
												result.AppendFormat("{0}:\"{1}\",", reader.Name, reader.ReadString());
											}
											else if (reader.Name == "facetitle")
											{
												string value = reader.ReadString();
												value = StringHelper.RemoveHtmlTag(value);
												value = System.Web.HttpUtility.HtmlEncode(value);
												result.AppendFormat("{0}:\"{1}\",", reader.Name, value);
											}
										}
									}
								}
							}
							if (result[result.Length - 1] == ',')
								result.Remove(result.Length - 1, 1);
							result.Append("},");
						}
					}
				}
				if (result[result.Length - 1] == ',')
					result.Remove(result.Length - 1, 1);
				result.Append("]");
			}
			#endregion

			return result.Insert(0, "{").Append("}").ToString();
		}
		#endregion

		#region ��ȡ������ͼ���� ����json��ʽ
		/// <summary>
		/// ��ȡ������ͼ���� ����json��ʽ
		/// </summary>
		/// <returns></returns>
		public string GetSellDataMapToJson()
		{
			/*[
				{
					Value:"2010-07",
					Levels:
					[
						{
							level:"������",
							Serials:
							[
								{SerialId:",.....",....},
								{SerialId:",.....",....}
							]
						}
					]
				}
			]*/
			StringBuilder result = new StringBuilder("[");
			string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\SellDataMap.xml");
			if (File.Exists(xmlFile))
			{
				using (XmlReader reader = XmlReader.Create(xmlFile))
				{
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "Month")
						{
							result.Append("{");
							result.AppendFormat("Value:\"{0}\",Levels:[{1}]", reader["Value"], GetSellDataMapLevelToJson(reader));
							result.Append("},");
						}
					}
					if (result[result.Length - 1] == ',')
						result.Remove(result.Length - 1, 1);
				}
			}
			return result.Append("]").ToString();
		}
		private string GetSellDataMapLevelToJson(XmlReader reader)
		{
			if (reader == null)
				return string.Empty;

			StringBuilder result = new StringBuilder();
			using (XmlReader subReader = reader.ReadSubtree())
			{
				while (subReader.Read())
				{
					if (subReader.NodeType == XmlNodeType.Element && subReader.Name == "Level")
					{
						result.Append("{");
						result.AppendFormat("level:\"{0}\", Serials:[{1}]", subReader["level"], GetSellDataMapSerialToJson(subReader));
						result.Append("},");
					}
				}
			}
			return result.Length <= 0 ? string.Empty : result.Remove(result.Length - 1, 1).ToString();
		}
		private string GetSellDataMapSerialToJson(XmlReader reader)
		{
			if (reader == null)
				return string.Empty;

			StringBuilder result = new StringBuilder();
			using (XmlReader subReader = reader.ReadSubtree())
			{
				while (subReader.Read())
				{
					if (subReader.NodeType == XmlNodeType.Element && subReader.Name == "Serial")
					{
						result.Append("{");
						while (subReader.MoveToNextAttribute())
						{
							if (subReader.Name == "ImageUrl")
								continue;
							result.AppendFormat("{0}:\"{1}\",", subReader.Name, subReader.Value);
						}
						if (result[result.Length - 1] == ',')
							result.Remove(result.Length - 1, 1);
						result.Append("},");
					}
				}
			}
			return result.Length <= 0 ? string.Empty : result.Remove(result.Length - 1, 1).ToString();
		}
		#endregion

		#region ��ȡĳ�µ���������ǰ10 ����json��ʽ
		/// <summary>
		/// ��ȡĳ�µ���������ǰ10 ����json��ʽ
		/// </summary>
		/// <param name="dataDate">ʱ��</param>
		/// <param name="dataType">��������</param>
		/// <param name="needNewData">�Ƿ��ȡ��������</param>
		/// <returns></returns>
		public string GetSellDataJson(DateTime dataDate, string dataType)
		{
			List<DateTime> lastMonths = GetLastMonths();
			if (dataDate == DateTime.MinValue)
			{
				dataDate = lastMonths[0];
			}
			dataDate = new DateTime(dataDate.Year, dataDate.Month, 1);

			List<int> levelList = new List<int>();
			dataType = dataType.Trim().ToLower();
			if (dataType == "suv")
				levelList.Add(424);
			else if (dataType == "mpv")
				levelList.Add(425);
			else if (dataType == "weixingche")
				levelList.Add(321);
			else if (dataType == "xiaoxingche")
				levelList.Add(338);
			else if (dataType == "jincouxingche")
				levelList.Add(339);
			else if (dataType == "zhongxingche")
				levelList.Add(340);
			else if (dataType == "zhongdaxingche")
				levelList.Add(341);
			else if (dataType == "haohuache")
				levelList.Add(342);
			else
			{
				dataType = "car";
				levelList.Add(321);		//΢�ͳ�
				levelList.Add(338);		//С�ͳ�
				levelList.Add(339);		//�����ͳ�
				levelList.Add(340);		//���ͳ�
				levelList.Add(341);		//�д��ͳ�
				levelList.Add(342);		//������
			}
			//{ 
			//    curDate:
			//    historyData:
			//    Time:
			//    SellDatas:[
			//        {CsId:"2370", CsName:"����"},
			//        {CsId:"2388", CsName:"��Խ"},
			//        ...
			//    ]
			//}
			StringBuilder result = new StringBuilder();
			string cacheKey = "SellStatisticData_" + dataType + "_" + dataDate.ToString("yyyyMMdd");
			string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\ProduceAndSell\\CarData\\" + cacheKey + ".xml");
			if (File.Exists(fileName))
			{
				string lastMonthData = "";
				foreach (DateTime dt in lastMonths)
				{
					lastMonthData += dt.ToString("yyyy-MM-dd") + "|";
				}
				lastMonthData = lastMonthData.Trim('|');
				result.AppendFormat("historyData:\"{0}\",", lastMonthData);

				using (XmlReader reader = XmlReader.Create(fileName))
				{
					while (reader.Read())
					{
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "SellDataList")
						{
							result.AppendFormat("curDate:\"{0}\",", reader["curDate"]);
							StringBuilder sellData = new StringBuilder();
							using (XmlReader subReader = reader.ReadSubtree())
							{
								while (subReader.Read())
								{
									if (reader.Name == "SellData")
									{
										sellData.Append("{");
										while (subReader.MoveToNextAttribute())
										{
											sellData.AppendFormat("{0}:\"{1}\",", subReader.Name, subReader.Value);
										}
										if (sellData[sellData.Length - 1] == ',')
											sellData.Remove(sellData.Length - 1, 1);
										sellData.Append("},");
									}
									else if (reader.Name == "Time")
									{
										result.AppendFormat("Time:\"{0}\",", reader.ReadString());
									}
								}
							}
							if (sellData[sellData.Length - 1] == ',')
								sellData.Remove(sellData.Length - 1, 1);
							result.AppendFormat("SellDatas:[{0}]", sellData.ToString());
							break;
						}
					}
				}
			}

			return result.Insert(0, "{").Append("}").ToString();
		}
		#endregion
	}
}
