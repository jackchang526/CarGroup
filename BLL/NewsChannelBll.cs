using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

using BitAuto.Utils;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL
{
	public class NewsChannelBll
	{
		/// <summary>
		/// 获取厂商，主品牌，品牌的名称与全拼
		/// </summary>
		/// <param name="newsType"></param>
		/// <param name="typeId"></param>
		/// <returns></returns>
		public string GetNameByTypeId(string newsType,int typeId,out string spell,out string prdShortName)
		{
			string typeName = "";
			spell = "";
			prdShortName = "";
			XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
			string xmlPath = "";
			switch(newsType)
			{
				case "producer":
					typeName = GetProducerName(typeId, out spell,out prdShortName);
					break;
				case "masterbrand":
					xmlPath = "/Params/MasterBrand[@ID=\"" + typeId + "\"]";
					typeName = ((XmlElement)xmlDoc.SelectSingleNode(xmlPath)).GetAttribute("Name");
					spell = ((XmlElement)xmlDoc.SelectSingleNode(xmlPath)).GetAttribute("AllSpell");
					break;
				case "brand":
					xmlPath = "";
					xmlPath = "/Params/MasterBrand/Brand[@ID=\"" + typeId + "\"]";
					XmlElement brandNode = (XmlElement)xmlDoc.SelectSingleNode(xmlPath);
					typeName = brandNode.GetAttribute("Name");
					spell = brandNode.GetAttribute("AllSpell");
					break;
			}
			return typeName;
		}
        /// <summary>
        /// 获取厂商，主品牌，品牌的名称与全拼
        /// </summary>
        /// <param name="newsType"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public string GetNameByTypeId(string newsType, int typeId, out string spell, out string prdShortName,out string typeSeoName)
        {
            string typeName = "";
            spell = "";
            prdShortName = "";
            typeSeoName = "";
            // XmlDocument xmlDoc = AutoStorageService.GetAllAutoXml();//.GetAutoXml();
			// modified by chengl Sep.6.2012
			XmlDocument xmlDoc = AutoStorageService.GetAllAutoAndLevelXml();
            string xmlPath = "";
            switch (newsType)
            {
                case "producer":
                    typeName = GetProducerName(typeId, out spell, out prdShortName, out typeSeoName);
                    break;
                case "masterbrand":
                    xmlPath = "/Params/MasterBrand[@ID=\"" + typeId + "\"]";
                    typeName = ((XmlElement)xmlDoc.SelectSingleNode(xmlPath)).GetAttribute("Name");
                    spell = ((XmlElement)xmlDoc.SelectSingleNode(xmlPath)).GetAttribute("AllSpell");
                    typeSeoName = ((XmlElement)xmlDoc.SelectSingleNode(xmlPath)).GetAttribute("MasterSEOName");
                    break;
                case "brand":
                    xmlPath = "";
                    xmlPath = "/Params/MasterBrand/Brand[@ID=\"" + typeId + "\"]";
                    XmlElement brandNode = (XmlElement)xmlDoc.SelectSingleNode(xmlPath);
					if (brandNode != null)
					{
						typeName = brandNode.GetAttribute("Name");
						spell = brandNode.GetAttribute("AllSpell");
						typeSeoName = brandNode.GetAttribute("BrandSEOName");
					}
                    break;
            }
            return typeName;
        }

		/// <summary>
		/// 根据品牌ID获取厂商的名称与ID
		/// </summary>
		/// <param name="bid"></param>
		/// <param name="spell"></param>
		/// <returns></returns>
		public string GetProducerBrand(int bId,out int pId)
		{
			string typeName = "";
			pId = 0;
			DataSet ds = new Car_BrandBll().GetCarBrandInfoByCBID(bId);
			if(ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				typeName = Convert.ToString(ds.Tables[0].Rows[0]["cp_name"]);
				pId = Convert.ToInt32(ds.Tables[0].Rows[0]["cp_id"]);
			}
			return typeName;
		}

		/// <summary>
		/// 获取厂商的名称与全拼
		/// </summary>
		/// <param name="pid"></param>
		/// <param name="spell"></param>
		/// <returns></returns>
		private string GetProducerName(int pid,out string spell,out string shortName)
		{
			string producerName = "";
			shortName = "";
			spell = "";
			string cacheKey = "producer_name_spell_fornews";

			DataSet pDs = (DataSet)CacheManager.GetCachedData(cacheKey);
			if(pDs == null)
			{
				pDs = new Car_ProducerDal().GetProducerNameAndSpell();
				CacheManager.InsertCache(cacheKey, pDs, WebConfig.CachedDuration);
			}

			if(pDs.Tables.Count > 0)
			{
				DataRow[] rows = pDs.Tables[0].Select("Cp_Id=" + pid);
				if(rows.Length > 0)
				{
					producerName = Convert.ToString(rows[0]["Cp_Name"]);
					shortName = Convert.ToString(rows[0]["Cp_ShortName"]);
					spell = Convert.ToString(rows[0]["Spell"]);
				}
			}
			
			return producerName;
		}
        /// <summary>
        /// 获取厂商的名称与全拼
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="spell"></param>
        /// <returns></returns>
        private string GetProducerName(int pid, out string spell, out string shortName,out string seoName)
        {
            string producerName = "";
            shortName = "";
            spell = "";
            seoName = "";
            string cacheKey = "producer_name_spell_fornews";

            DataSet pDs = (DataSet)CacheManager.GetCachedData(cacheKey);
            if (pDs == null)
            {
                pDs = new Car_ProducerDal().GetProducerNameAndSpell();
                CacheManager.InsertCache(cacheKey, pDs, WebConfig.CachedDuration);
            }

            if (pDs.Tables.Count > 0)
            {
                DataRow[] rows = pDs.Tables[0].Select("Cp_Id=" + pid);
                if (rows.Length > 0)
                {
                    producerName = Convert.ToString(rows[0]["Cp_Name"]);
                    shortName = Convert.ToString(rows[0]["Cp_ShortName"]);
                    spell = Convert.ToString(rows[0]["Spell"]);
                    seoName = Convert.ToString(rows[0]["cp_seoname"]);
                }
            }

            return producerName;
        }

		/// <summary>
		/// 获取新闻列表 
		/// </summary>
		/// <param name="newsType"></param>
		/// <param name="typeId"></param>
		/// <returns></returns>
        [Obsolete("服务上线后，不再使用。")]
        public XmlNodeList GetNewsList(string newsType,int typeId)
		{
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data");
			switch(newsType)
			{
				case "producer":
					xmlFile = Path.Combine(xmlFile, "ProducerNews\\Producer_News_" + typeId + ".xml");
					break;
				case "masterbrand":
					xmlFile = Path.Combine(xmlFile, "MasterBrand\\News\\" + typeId + ".xml");
					break;
				case "brand":
					xmlFile = Path.Combine(xmlFile, "Brand\\News\\" + typeId + ".xml");
					break;
			}
			XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
			return xmlDoc.SelectNodes("/NewDataSet/listNews");
		}
		/// <summary>
		/// 获取新闻的排序顺序字典
		/// </summary>
		/// <param name="sortNode"></param>
		/// <returns></returns>
		public static Dictionary<int,int> GetNewsSortDic(XmlElement sortRoot)
		{
			Dictionary<int, int> sortDic = new Dictionary<int, int>();
			if(sortRoot != null)
			{
				XmlNodeList sortNodeList = sortRoot.SelectNodes("Sort");
				foreach(XmlElement sortNode in sortNodeList)
				{
					DateTime startDate = Convert.ToDateTime(sortNode.GetAttribute("StartDate"));
					DateTime endDate = Convert.ToDateTime(sortNode.GetAttribute("EndDate"));
					DateTime curDate = DateTime.Now;
					if(curDate >= startDate && curDate <= endDate)
					{
						int newsId = Convert.ToInt32(sortNode.GetAttribute("NewsId"));
						int sortNum = Convert.ToInt32(sortNode.GetAttribute("SortNum"));
						sortDic[sortNum] = newsId;
					}
				}
			}
			return sortDic;
		}
		/// <summary>
		/// 获取所有主品牌，品牌，子品牌的导购新闻的数量
		/// </summary>
		/// <returns></returns>
		public static Dictionary<string,Dictionary<int,int>> GetDaogouNewsNumDic()
		{
			string cacheKey = "master_brand_SerialDaogouNewsNumDic";
			Dictionary<string, Dictionary<int, int>> numDic = (Dictionary<string, Dictionary<int, int>>)CacheManager.GetCachedData(cacheKey);
			if(numDic == null)
			{
				numDic = new Dictionary<string, Dictionary<int, int>>();
				numDic["Master"] = new Dictionary<int, int>();
				numDic["Brand"] = new Dictionary<int, int>();
				numDic["Serial"] = new Dictionary<int, int>();
				string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\treenewscount.xml");
				if(File.Exists(xmlFile))
				{
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(xmlFile);
					//主品牌
					XmlNodeList mbNodeList = xmlDoc.SelectNodes("/root/MasterBrand");
					Dictionary<int, int> tmpDic = numDic["Master"];
					foreach(XmlElement mbNode in mbNodeList)
					{
						int mbId = Convert.ToInt32(mbNode.GetAttribute("ID"));
						int mbNewsNum = Convert.ToInt32(mbNode.GetAttribute("daogou"));
						tmpDic[mbId] = mbNewsNum;
					}

					//品牌
					XmlNodeList brandNodeList = xmlDoc.SelectNodes("/root/MasterBrand/Brand");
					tmpDic = numDic["Brand"];
					foreach (XmlElement brandNode in brandNodeList)
					{
						int brandId = Convert.ToInt32(brandNode.GetAttribute("ID"));
						int newsNum = Convert.ToInt32(brandNode.GetAttribute("daogou"));
						tmpDic[brandId] = newsNum;
					}

					//子品牌
					XmlNodeList serialNodeList = xmlDoc.SelectNodes("/root/MasterBrand/Brand/Serial");
					tmpDic = numDic["Serial"];
					foreach (XmlElement serialNode in serialNodeList)
					{
						int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
						int newsNum = Convert.ToInt32(serialNode.GetAttribute("daogou"));
						tmpDic[serialId] = newsNum;
					}
					CacheManager.InsertCache(cacheKey, numDic, 60);
				}
			}
			return numDic;
		}
		/// <summary>
		/// 获取子品牌的导购新闻数量
		/// </summary>
		/// <param name="serialId"></param>
		/// <returns></returns>
		public static int GetSerialDaogouNewsNum(int serialId)
		{
			Dictionary<int, int> tmpDic = GetDaogouNewsNumDic()["Serial"];
			int newsNum = 0;
			if (tmpDic.ContainsKey(serialId))
				newsNum = tmpDic[serialId];
			return newsNum;
		}

		/// <summary>
		/// 获取品牌的导购新闻数量
		/// </summary>
		/// <param name="brandId"></param>
		/// <returns></returns>
		public static int GetBrandDaogouNewsNum(int brandId)
		{
			Dictionary<int, int> tmpDic = GetDaogouNewsNumDic()["Brand"];
			int newsNum = 0;
			if (tmpDic.ContainsKey(brandId))
				newsNum = tmpDic[brandId];
			return newsNum;
		}
		/// <summary>
		/// 获取主品牌的导购新闻数量
		/// </summary>
		/// <param name="brandId"></param>
		/// <returns></returns>
		public static int GetMasterBrandDaogouNewsNum(int mbId)
		{
			Dictionary<int, int> tmpDic = GetDaogouNewsNumDic()["Master"];
			int newsNum = 0;
			if (tmpDic.ContainsKey(mbId))
				newsNum = tmpDic[mbId];
			return newsNum;
		}
        /// <summary>
        /// 得到子品牌城市列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public static Dictionary<int,int> GetTreeHangQingSerialCityNumber(int serialId)
        {
            string FileXml = Path.Combine(WebConfig.DataBlockPath
                            ,string.Format("Data\\SerialNews\\hangqing\\Serial_All_News_CityNum_{0}.xml",serialId.ToString()));

            if (!File.Exists(FileXml)) return null;
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(FileXml);
                if (xmlDoc == null) return null;
                XmlNodeList xNodeList = xmlDoc.SelectNodes("root/City");
                if (xNodeList == null || xNodeList.Count < 1) return null;

                Dictionary<int, int> cityList = new Dictionary<int, int>();
                foreach (XmlElement xElem in xNodeList)
                {
                    int newscount = ConvertHelper.GetInteger(xElem.GetAttribute("newscount"));
                    int id = ConvertHelper.GetInteger(xElem.GetAttribute("id"));
                    if (newscount == 0 || cityList.ContainsKey(id)) continue;

                    cityList.Add(id, newscount);
                }
                return cityList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子品牌城市列表
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public static XmlNodeList GetTreeHangQingSerialCityNumber(int serialId,string type)
        {
            string FileXml = Path.Combine(WebConfig.DataBlockPath
                            , string.Format("Data\\SerialNews\\hangqing\\Serial_All_News_CityNum_{0}.xml", serialId.ToString()));

            if (!File.Exists(FileXml)) return null;
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(FileXml);
                if (xmlDoc == null) return null;
                XmlNodeList xNodeList = xmlDoc.SelectNodes("root/Province/City");
                if (xNodeList == null || xNodeList.Count < 1) return null;


                return xNodeList;
            }
            catch
            {
                return null;
            }
        }
    }
}
