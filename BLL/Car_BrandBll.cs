using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using System.Data;
using System.IO;
using System.Web.Caching;

using System.Xml;
using System.Xml.XPath;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL
{
    public class Car_BrandBll
    {
        private static readonly Car_BrandDal CarBrandDal = new Car_BrandDal();
        // private static readonly string _MasterBrandNewsPath = "Data\\MasterBrand\\News\\{0}.xml";
        // private static readonly string _MasterBrandHangQingPath = "Data\\MasterBrand\\HangQing\\{0}.xml";
        // private static readonly string _MasterBrandPingCePath = "Data\\MasterBrand\\PingCe\\{0}.xml";
        // private static readonly string _MasterBrandDaoGou = "Data\\MasterBrand\\DaoGou\\{0}.xml";
        // private static readonly string _MasterBrandYongChe = "Data\\MasterBrand\\YongChe\\{0}.xml";
        private static readonly string _MasterBrandVideoNew = "Data\\MasterBrand\\Video\\New\\{0}.xml";
        private static readonly string _MasterBrandVideoOrder = "Data\\MasterBrand\\Video\\Order\\{0}.xml";

        // private static readonly string _BrandNewsPath = "Data\\Brand\\News\\{0}.xml";
        // private static readonly string _BrandHangQingPath = "Data\\Brand\\HangQing\\{0}.xml";
        // private static readonly string _BrandPingCePath = "Data\\Brand\\PingCe\\{0}.xml";
        // private static readonly string _BrandDaoGou = "Data\\Brand\\DaoGou\\{0}.xml";
        // private static readonly string _BrandYongChe = "Data\\Brand\\YongChe\\{0}.xml";
        // private static readonly string _BrandVideoNew = "Data\\Brand\\Video\\New\\{0}.xml";
        // private static readonly string _BrandVideoOrder = "Data\\Brand\\Video\\Order\\{0}.xml";

        public Car_BrandBll()
        { }

        /// <summary>
        /// ����Ʒ��ȫ��ȡ����Ӧ���̼���Ʒ����Ϣ 
        /// </summary>
        /// <param name="strCBSpName"></param>
        /// <returns></returns>
        public DataRow GetCarBrandInfoByCBSpellName(string strCBSpName)
        {
            return CarBrandDal.GetCarBrandInfoByCBSpellName(strCBSpName);
        }

        public DataSet GetCarBrandInfoByCBID(int cbID)
        {
            string cacheKey = "serial-carbrand-list-" + cbID.ToString();
            object carBrandInfo = null;
            CacheManager.GetCachedData(cacheKey, out carBrandInfo);
            if (null == carBrandInfo)
            {
                carBrandInfo = CarBrandDal.GetCarBrandInfoByCBID(cbID);
                // carBrandInfo = (object)new Car_BrandBll().GetCarBrandInfoByCBSpellName(strcbID);
                if (null != carBrandInfo)
                {
                    CacheManager.InsertCache(cacheKey, carBrandInfo, WebConfig.CachedDuration);
                }
            }

            return (DataSet)carBrandInfo;
        }

		/// <summary>
		/// ��ȡ����Ʒ����Ϣ
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllBrand()
		{
			return CarBrandDal.GetAllBrand();
		}

        /// <summary>
        /// ����Ʒ��ID��ȡ��Ʒ�Ƶ�ID��Urlȫƴ������
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="masterSpell"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public int GetMasterbrandByBrand(int brandId, out string masterSpell, out string masterName)
        {
            // modifed by chengl Feb.17.2010
            // XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
            XmlDocument xmlDoc = AutoStorageService.GetAllAutoXml();
            string xmlPath = "/Params/MasterBrand/Brand[@ID=\"" + brandId + "\"]";
            XmlElement masterNode = (XmlElement)xmlDoc.SelectSingleNode(xmlPath).ParentNode;
            int masterId = Convert.ToInt32(masterNode.GetAttribute("ID"));
            masterSpell = masterNode.GetAttribute("AllSpell").ToLower();
            masterName = masterNode.GetAttribute("Name");
            return masterId;
        }

        /// <summary>
        /// ������Ʒ�Ƶ�ȫƴ��ȡ��Ʒ�Ƶ�������ID
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        public int GetMasterbrandBySpell(string spell, out string masterName)
        {
            // modified by chengl Nov.20.2009
            // XmlDocument mbDoc = AutoStorageService.GetAutoXml();
            XmlDocument mbDoc = AutoStorageService.GetAllAutoXml();
            XmlElement masterNode = (XmlElement)mbDoc.SelectSingleNode("/Params/MasterBrand[@AllSpell=\"" + spell.Trim().ToLower() + "\"]");
            masterName = "";
            int masterId = 0;
            if (masterNode != null)
            {
                masterId = Convert.ToInt32(masterNode.GetAttribute("ID"));
                masterName = masterNode.GetAttribute("Name");
            }
            return masterId;
        }

        ///// <summary>
        ///// ��ȡUCar���ֳ���Ϣ
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="brandType"></param>
        ///// <returns></returns>
        //public XmlDocument GetUCarInfo(int id, string brandType)
        //{
        //    brandType = brandType.Trim().ToLower();
        //    string xmlFile = "";
        //    if (brandType == "masterbrand")
        //        xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\UsedCarInfo\\Masterbrand\\Usecar_Masterbrand_" + id + ".xml");
        //    else
        //        xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\UsedCarInfo\\Brand\\Usecar_Brand_" + id + ".xml");
        //    return CommonFunction.ReadXmlFromFile(xmlFile);
        //}

        /// <summary>
        /// ȡ�õ�ǰƷ�Ƶ���Ʒ�Ƴ�����Ϣ
        /// </summary>
        /// <param name="nCBID"></param>
        /// <returns></returns>
        public List<CarSerialPhotoEntity> GetCarSerialPhotoListByCBID(int nCBID)
        {
            return CarBrandDal.GetCarSerialPhotoListByCBID(nCBID, false);
        }

        /// <summary>
        /// ȡ�õ�ǰƷ�Ƶ���Ʒ�Ƴ�����Ϣ
        /// </summary>
        /// <param name="nCBID"></param>
        /// <param name="isAll">�Ƿ����ͣ������Ʒ��</param>
        /// <returns></returns>
        public List<CarSerialPhotoEntity> GetCarSerialPhotoListByCBID(int nCBID, bool isAll)
        {
            return CarBrandDal.GetCarSerialPhotoListByCBID(nCBID, isAll);
        }

        /// <summary>
        /// ȡ�õ�ǰ��Ʒ�Ƶ������������Ʒ�Ƴ�����Ϣ
        /// </summary>
        /// <param name="nCBID"></param>
        /// <returns></returns>
        public DataSet GetCarSerialPhotoListByBSID(int nBSID)
        {
            return CarBrandDal.GetCarSerialPhotoListByBSID(nBSID, false);
        }

        /// <summary>
        /// ����Ʒ��ID ȡ������Ʒ��
        /// </summary>
        /// <param name="bcID">Ʒ��ID</param>
        /// <param name="isAll">�Ƿ����ͣ��</param>
        /// <returns></returns>
        public DataSet GetCarSerialListByBrandID(int bcID, bool isAll)
        {
            return CarBrandDal.GetCarSerialListByBrandID(bcID, isAll);
        }
        /// <summary>
        /// ������Ʒ��ID ȡ����Ʒ�ƣ�����Ʒ�����UV���򣩣���Ʒ�ƣ�����״̬�� ��ע�Ƚ�������
        /// </summary>
        /// <param name="nBSID"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public DataSet GetCarSerialSortListByBSID(int nBSID, int type)
        {
            return CarBrandDal.GetCarSerialSortListByBSID(nBSID, type);
        }

        /// <summary>
        /// ������Ʒ��ID ȡ������Ʒ��
        /// </summary>
        /// <param name="nBSID"></param>
        /// <param name="isAll"></param>
        /// <returns></returns>
        public DataSet GetCarSerialListByBSID(int nBSID, bool isAll)
        {
            return CarBrandDal.GetCarSerialListByBSID(nBSID, isAll);
        }

        /// <summary>
        /// ȡ�õ�ǰ��Ʒ�Ƶ���Ʒ�Ƴ�����Ϣ
        /// </summary>
        /// <param name="nCBID"></param>
        /// <returns></returns>
        public DataSet GetCarSerialPhotoListByBSID(int nBSID, bool isAll)
        {
            return CarBrandDal.GetCarSerialPhotoListByBSID(nBSID, isAll);
        }

        /// <summary>
        /// ��ȡ��Ʒ�Ƶ���Ƶ
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public XmlNodeList GetMasterBrandVideos(int masterId)
        {
            int count = 0;
            return GetMasterBrandVideos(masterId, out count);
        }

        /// <summary>
        /// ��ȡ��Ʒ�Ƶ���Ƶ,�����Ƶ����
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="videCount"></param>
        /// <returns></returns>
        public XmlNodeList GetMasterBrandVideos(int masterId, out int videCount)
        {
            videCount = 0;
            //string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\MasterBrandVideos\\MasterBrand_Videos_" + masterId + ".xml");
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(_MasterBrandVideoNew, masterId));
            XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);

            try
            {
                XmlNode countNode = xmlDoc.SelectSingleNode("/NewDataSet/newsAllCount/allcount");
                videCount = ConvertHelper.GetInteger(countNode.InnerText);
            }
            catch (System.Exception ex)
            {

            }
            return xmlDoc.SelectNodes("/NewDataSet/listNews");
        }

        /// <summary>
        /// ��ȡ���̵�����
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        [Obsolete("�������ߺ󣬽���GetProducerNewsNew�������档")]
        public XmlNodeList GetProducerNews(int pId)
        {
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ProducerNews\\Producer_News_" + pId + ".xml");
            XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
            return xmlDoc.SelectNodes("/NewDataSet/listNews");
        }

		///// <summary>
		///// ��ȡƷ�Ƶ�����
		///// </summary>
		///// <param name="pId"></param>
		///// <returns></returns>
		//public XmlNodeList GetBrandNews(int bId)
		//{
		//	//string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\BrandNews\\Brand_News_" + bId + ".xml");
		//	string xmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(_BrandNewsPath, bId));
		//	XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
		//	return xmlDoc.SelectNodes("/NewDataSet/listNews");
		//}

		///// <summary>
		///// ��ȡƷ�Ƶ�����XML
		///// </summary>
		///// <param name="brandId"></param>
		///// <returns></returns>
		//public XmlDocument GetBrandNewsXml(int brandId)
		//{
		//	//string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\BrandNews\\Brand_News_" + brandId + ".xml");
		//	string xmlFile = Path.Combine(WebConfig.DataBlockPath, string.Format(_BrandNewsPath, brandId));
		//	XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
		//	return xmlDoc;
		//}

		///// <summary>
		///// Ϊ��Ʒ��ҳ��ȡ����������
		///// </summary>
		///// <param name="masterId"></param>
		///// <param name="topNum">������������</param>
		///// <param name="count">ȡ��������</param>
		///// <returns></returns>
		//public List<XmlElement> GetBrandNews(int brandId, int topNum, int count)
		//{
		//	string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\BrandNews\\Brand_News_" + brandId + ".xml");
		//	XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(xmlFile);
		//	XmlNodeList newsList = xmlDoc.SelectNodes("/NewDataSet/listNews");
		//	List<XmlElement> newsRet = new List<XmlElement>();
		//	foreach (XmlElement newsNode in newsList)
		//	{
		//		if (newsRet.Count <= topNum)
		//			new Car_SerialBll().AppendNewsInfo(newsNode);
		//		newsRet.Add(newsNode);
		//		if (newsRet.Count >= count)
		//			break;
		//	}
		//	return newsRet;
		//}

        /// <summary>
        /// ȡ�õ�ǰ������Ʒ����Ϣ
        /// </summary>
        /// <param name="nCPID"></param>
        /// <returns></returns>
        public DataRow GetCarMasterBrandInfoByBSID(int masterId)
        {
            string cacheKey = "serial-carmasterbrandInfo-list-" + masterId;
            DataRow carMasterBrandInfo = (DataRow)CacheManager.GetCachedData(cacheKey);

            if (null == carMasterBrandInfo)
            {
                carMasterBrandInfo = CarBrandDal.GetCarMasterBrandInfoByBSID(masterId);
                if (null != carMasterBrandInfo)
                {
                    CacheManager.InsertCache(cacheKey, carMasterBrandInfo, WebConfig.CachedDuration);
                }
            }

            return carMasterBrandInfo;
        }
        /// <summary>
        /// ͨ����Ʒ��ID�õ���Ʒ��ʵ��
        /// ע����������Բ�Ҫ���ã����ã�(MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand,_MasterBrandId)
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public Car_MasterBrandEntity GetCarMasterBrandEntityByBsId(int masterId)
        {
            if (masterId < 1)
            {
                return null;
            }
            return CarBrandDal.GetCarMasterBrandEntityByBSID(masterId);
        }
        /// <summary>
        /// ͨ��Ʒ��ID�õ�Ʒ��ʵ��
        /// ע����������Բ�Ҫ���ã����ã�(MasterBrandEntity)DataManager.GetDataEntity
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public Car_BrandEntity GetBrandEntityByBrandId(int brandId)
        {
            if (brandId < 1)
            {
                return null;
            }
            return CarBrandDal.GetBrandEntityByBrandID(brandId);
        }

        /// <summary>
        /// ��ȡƷ�Ƶ�ƴд�ֵ�
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetBrandSpellDictionary()
        {
            string cacheKey = "BrandSpellDictionaryToId";
            Dictionary<string, int> dic = (Dictionary<string, int>)CacheManager.GetCachedData(cacheKey);
            if (dic == null)
            {
                dic = new Dictionary<string, int>();
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(WebConfig.AllSpellList);
                    XmlNodeList brandList = xmlDoc.SelectNodes("/Params/Brand/Item");
                    foreach (XmlElement brandNode in brandList)
                    {
                        int brandId = Convert.ToInt32(brandNode.GetAttribute("ID"));
                        string brandSpell = brandNode.GetAttribute("AllSpell").Trim().ToLower();
                        dic[brandSpell] = brandId;
                    }

                    CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
                }
                catch { }
            }

            return dic;
        }

        /// <summary>
        /// ��ȡ��Ʒ�ƻ���Ʒ�Ƶ���̳��Ϣ
		/// modified by chengl May.5.2015 �ļ����ٸ��£�����Ƶ����ʹ��
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bid"></param>
        /// <returns></returns>
        public BrandForum GetBrandForm(string brandType, int bid)
        {
            string cacheKey = brandType + "_ForumInfo_" + bid;
            BrandForum bf = (BrandForum)CacheManager.GetCachedData(cacheKey);
            if (bf == null)
            {
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ForumInfo");
                if (brandType.ToLower() == "brand")
                {
                    xmlFile = Path.Combine(xmlFile, "Brand\\ForumInfo_Brand_" + bid + ".xml");
                }
                else
                    xmlFile = Path.Combine(xmlFile, "MasterBrand\\ForumInfo_Masterbrand_" + bid + ".xml");

                bf = new BrandForum(xmlFile);

                if (bf != null)
                {
                    CacheManager.InsertCache(cacheKey, bf, 60);
                }
            }

            return bf;
        }

        /// <summary>
        /// ��ȡ����չ����
        /// </summary>
        /// <param name="brandType"></param>
        /// <param name="bId"></param>
        /// <returns></returns>
        public List<XmlElement> GetCarshowTopNews(string brandType, int bId)
        {
            brandType = brandType.Trim().ToLower();
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\CarShow");
            if (brandType == "masterbrand")
                xmlFile = Path.Combine(xmlFile, "Masterbrand\\Carshow_Masterbrand_" + bId + ".xml");
            else if (brandType == "serial")
                xmlFile = Path.Combine(xmlFile, "Serial\\Carshow_Serial_" + bId + ".xml");

            List<XmlElement> newsList = new List<XmlElement>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNodeList nodeList = xmlDoc.SelectNodes("/NewDataSet/listNews");
                foreach (XmlElement ele in nodeList)
                    newsList.Add(ele);
            }
            catch { }
            return newsList;
        }
        /// <summary>
        /// ����XML�ļ��õ���Ʒ���б�
        /// </summary>
        /// <returns></returns>
        public Dictionary<char, Dictionary<int, string[]>> GetMasterBrandListByXML()
        {
            XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
            string spath = "Params/MasterBrand";

            XmlNodeList xNodeList = xmlDoc.SelectNodes(spath);
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }

            Dictionary<char, Dictionary<int, string[]>> masterBrandDic = new Dictionary<char, Dictionary<int, string[]>>();

            foreach (XmlElement xElement in xNodeList)
            {
                char masterSpellTrim = xElement.GetAttribute("Spell").ToUpper().ToCharArray()[0];
                int masterID = Convert.ToInt32(xElement.GetAttribute("ID"));
                string masterName = xElement.GetAttribute("Name").ToString();
                string masterAllSpell = xElement.GetAttribute("AllSpell").ToString();
                Dictionary<int, string[]> masterItem = new Dictionary<int, string[]>();
                masterItem.Add(masterID, new string[] { masterName, masterAllSpell });

                if (masterBrandDic.ContainsKey(masterSpellTrim))
                {
                    masterBrandDic[masterSpellTrim].Add(masterID, new string[] { masterName, masterAllSpell });
                    continue;
                }
                masterBrandDic.Add(masterSpellTrim, masterItem);
            }
            return masterBrandDic;
        }
        /// <summary>
        /// ����XML�ļ��õ��������ҵ���Ʒ���б�
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, string[]>> GetCountryMasterBrandListByXML()
        {
            XmlDocument xmlDoc = AutoStorageService.GetAutoXml();
            string spath = "Params/MasterBrand";

            XmlNodeList xNodeList = xmlDoc.SelectNodes(spath);
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }

            Dictionary<string, Dictionary<int, string[]>> masterBrandDic = new Dictionary<string, Dictionary<int, string[]>>();

            foreach (XmlElement xElement in xNodeList)
            {
                string masterCountryName = xElement.GetAttribute("Country").Trim();
                int masterID = Convert.ToInt32(xElement.GetAttribute("ID"));
                string masterName = xElement.GetAttribute("Name").ToString();
                string masterAllSpell = xElement.GetAttribute("AllSpell").ToString();
                Dictionary<int, string[]> masterItem = new Dictionary<int, string[]>();
                masterItem.Add(masterID, new string[] { masterName, masterAllSpell });

                if (masterBrandDic.ContainsKey(masterCountryName))
                {
                    masterBrandDic[masterCountryName].Add(masterID, new string[] { masterName, masterAllSpell });
                    continue;
                }
                masterBrandDic.Add(masterCountryName, masterItem);
            }
            return masterBrandDic;
        }
        /// <summary>
        /// �õ���ĸ�б�
        /// </summary>
        /// <param name="letterList"></param>
        /// <returns></returns>
        public Dictionary<char, char> GetCharLetter(out char[] letterList)
        {
            letterList = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            Dictionary<char, char> charLetter = new Dictionary<char, char>();
            //��ȡ����xml
            XmlDocument mbDoc = AutoStorageService.GetAutoXml();

            //����������Ʒ�ƽڵ�
            XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
            foreach (XmlElement xElem in mbNodeList)
            {
                string masterSpell = xElem.GetAttribute("AllSpell").ToLower();
                //����ĸ
                char firstChar = xElem.GetAttribute("Spell").Substring(0, 1).ToUpper().ToCharArray()[0];

                //������ĸͷ
                if (charLetter.ContainsKey(firstChar))
                {
                    continue;
                }
                charLetter.Add(firstChar, firstChar);
            }
            return charLetter;
        }
        /// <summary>
        /// �õ�ָ��������Ʒ������
        /// </summary>
        /// <param name="topNum">Ҫ�õ�Ʒ�Ƶ�����</param>
        /// <returns></returns>
        public DataSet GetBrandPvRanking(int topNum)
        {
            return CarBrandDal.GetBrandPvRanking(topNum);
        }

        #region ��Ʒ��UV���

        /// <summary>
        /// ȡ��Ʒ��ID ��UV����
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllMasterOrderByUV()
        {
            List<int> lm = new List<int>();
            DataSet ds = new Car_BrandDal().Get_Car_MasterOrderByUV();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!lm.Contains(int.Parse(dr["bs_id"].ToString())))
                    {
                        lm.Add(int.Parse(dr["bs_id"].ToString()));
                    }
                }
            }
            return lm;
        }

        /// <summary>
        /// ȡ��Ʒ��ID��UV�� ��UV����
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetAllMasterDicOrderByUV()
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            DataSet ds = new Car_BrandDal().Get_Car_MasterOrderByUV();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!dic.ContainsKey(int.Parse(dr["bs_id"].ToString())))
                    {
                        if (dr["UVCount"].ToString() != "")
                        {
                            dic.Add(int.Parse(dr["bs_id"].ToString()), int.Parse(dr["UVCount"].ToString()));
                        }
                        else
                        {
                            dic.Add(int.Parse(dr["bs_id"].ToString()), 0);
                        }
                    }
                }
            }
            return dic;
        }

        #endregion

        #region ��Ʒ�ƺ�Ʒ�ƻ�����ͨ
        /// <summary>
        /// �õ�ѡ������ͨͷ
        /// </summary>
        /// <param name="brandId">Ʒ��ID</param>
        /// <param name="brandType">Ʒ������</param>
        /// <param name="currentSelectType">��ǰѡ������</param>
        /// <returns>������ͨͷ�ַ���</returns>
        public string GetRelationHeader(int brandId, string brandName, string brandSpell, int parentBrandId, string brandType, string currentSelectType)
        {
            string logoImgUrl = "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/30/m_{0}_30.png";
            Dictionary<int, Dictionary<string, int>> newCount = new Dictionary<int, Dictionary<string, int>>();
            if (brandType.ToLower() == "masterbrand")
            {
                logoImgUrl = string.Format(logoImgUrl, brandId);
                newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList(brandType.ToLower());
            }
            else
            {
                logoImgUrl = string.Format(logoImgUrl, parentBrandId);
                newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList(brandType.ToLower());
            }
            StringBuilder liList = new StringBuilder();
            liList.AppendFormat("<li id=\"treeNav_chexing\" class=\"{1}\"><a target=\"_self\" href=\"/{0}/\" rel=\"nofollow\">��ҳ</a></li>"
                                 , brandSpell
                                , string.IsNullOrEmpty(currentSelectType) ? "current" : "");

            if (currentSelectType.ToLower() == "xinwen" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("xinwen") && newCount[brandId]["xinwen"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/xinwen/\">����</a></li>"
                               , brandSpell
                               , currentSelectType == "xinwen" ? "current" : "");
            }
           
            if (currentSelectType.ToLower() == "daogou" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("daogou") && newCount[brandId]["daogou"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/daogou/\">����</a></li>"
                               , brandSpell
                               , currentSelectType == "daogou" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "pingce" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("treepingce") && newCount[brandId]["treepingce"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/pingce/\">����</a></li>"
                               , brandSpell
                               , currentSelectType == "pingce" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "yongche" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("yongche") && newCount[brandId]["yongche"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/yongche/\">�ó�</a></li>"
                               , brandSpell
                               , currentSelectType == "yongche" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "tupian" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("image") && newCount[brandId]["image"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/tupian/\">ͼƬ</a></li>"
                               , brandSpell
                               , currentSelectType == "tupian" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "video" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("video") && newCount[brandId]["video"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/video/\">��Ƶ</a></li>"
                               , brandSpell
                               , currentSelectType == "video" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "review" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("koubei") && newCount[brandId]["koubei"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/review/\">�ڱ�</a></li>"
                               , brandSpell
                               , currentSelectType == "review" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "dayi" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("ask") && newCount[brandId]["ask"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/dayi/\">�ʴ�</a></li>"
                               , brandSpell
                               , currentSelectType == "dayi" ? "current" : "");
            }
            ////�Ƿ���ʾ�û���ǩ
            //bool isshowZh = false;
            //if (brandType.ToLower() == "brand")
            //{
            //    isshowZh = true;
            //}
            //else
            //{
            //    //�����Ʒ����ֻ��һ��Ʒ�ƣ�����ʾ�û���ǩ
            //    XmlDocument autoxml = AutoStorageService.GetAutoXml();
            //    if (autoxml != null 
            //        && autoxml.SelectNodes(string.Format("Params/MasterBrand[@ID={0}]/Brand", brandId.ToString())).Count == 1)
            //    {
            //        isshowZh = true;
            //    }
            //}
            //if (isshowZh)
            //{
            //    if (brandType.ToLower() == "masterbrand")
            //    {
            //        liList.AppendFormat("<li><a target=\"_blank\" href=\"http://www.maichebao.com/zhihuan/?master={0}\">�û�</a></li>"
            //                   , brandId);
            //    }
            //    else if (brandType.ToLower() == "brand")
            //    {
            //        liList.AppendFormat("<li><a target=\"_blank\" href=\"http://www.maichebao.com/zhihuan/?brand={0}\">�û�</a></li>"
            //                   , brandId);
            //    }
            //    else
            //    {}

            //    //liList.AppendFormat("<li class=\"{1}\"><a target=\"_self\" href=\"/{0}/zhihuan/\">�û�</a></li>"
            //    //               , brandSpell
            //    //               , currentSelectType == "zhihuan" ? "on" : "");
            //}
            return liList.ToString();
        }

        /// <summary>
        /// ��ȡ�����б��ͷ������ ���� ���� �ó�
        /// </summary>
        /// <param name="brandId"></param>
        /// <param name="brandName"></param>
        /// <param name="brandSpell"></param>
        /// <param name="parentBrandId"></param>
        /// <param name="brandType"></param>
        /// <param name="currentSelectType"></param>
        /// <returns></returns>
        public string GetWenZhangHeader(int brandId, string brandName, string brandSpell, int parentBrandId, string brandType, string currentSelectType)
        {
            Dictionary<int, Dictionary<string, int>> newCount = new Dictionary<int, Dictionary<string, int>>();
            if (brandType.ToLower() == "masterbrand")
            {
                newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList(brandType.ToLower());
            }
            else
            {
                newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList(brandType.ToLower());
            }
            StringBuilder liList = new StringBuilder();
            liList.AppendFormat("<li class=\"{1}\"><a href=\"/{0}/wenzhang/\">ȫ��</a><em>|</em></li>"
                                 , brandSpell
                                , currentSelectType=="wenzhang" ? "current" : "");

            if (currentSelectType.ToLower() == "xinwen" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("xinwen") && newCount[brandId]["xinwen"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a href=\"/{0}/xinwen/\">����</a><em>|</em></li>"
                               , brandSpell
                               , currentSelectType == "xinwen" ? "current" : "");
            }

            if (currentSelectType.ToLower() == "daogou" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("daogou") && newCount[brandId]["daogou"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a  href=\"/{0}/daogou/\">����</a><em>|</em></li>"
                               , brandSpell
                               , currentSelectType == "daogou" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "pingce" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("treepingce") && newCount[brandId]["treepingce"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a  href=\"/{0}/pingce/\">����</a><em>|</em></li>"
                               , brandSpell
                               , currentSelectType == "pingce" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "yongche" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("yongche") && newCount[brandId]["yongche"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a  href=\"/{0}/yongche/\">�ó�</a></li>"
                               , brandSpell
                               , currentSelectType == "yongche" ? "current" : "");
            }
            return liList.ToString();
        }
        public string GetWenZhangHeaderFor1200(int brandId, string brandName, string brandSpell, int parentBrandId, string brandType, string currentSelectType)
        {
            Dictionary<int, Dictionary<string, int>> newCount = new Dictionary<int, Dictionary<string, int>>();
            if (brandType.ToLower() == "masterbrand")
            {
                newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList(brandType.ToLower());
            }
            else
            {
                newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList(brandType.ToLower());
            }
            StringBuilder liList = new StringBuilder();
            liList.AppendLine("<div class=\"section-header header2 h-default mbl\">");
            liList.AppendLine("<div class=\"box\">");
            liList.AppendLine("<ul class=\"nav\">");

            liList.AppendFormat("<li class=\"{1}\"><a href=\"/{0}/wenzhang/\">ȫ��</a></li>"
                                 , brandSpell
                                , currentSelectType == "wenzhang" ? "current" : "");

            if (currentSelectType.ToLower() == "xinwen" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("xinwen") && newCount[brandId]["xinwen"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a href=\"/{0}/xinwen/\">����</a></li>"
                               , brandSpell
                               , currentSelectType == "xinwen" ? "current" : "");
            }

            if (currentSelectType.ToLower() == "daogou" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("daogou") && newCount[brandId]["daogou"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a  href=\"/{0}/daogou/\">����</a></li>"
                               , brandSpell
                               , currentSelectType == "daogou" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "pingce" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("treepingce") && newCount[brandId]["treepingce"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a  href=\"/{0}/pingce/\">����</a></li>"
                               , brandSpell
                               , currentSelectType == "pingce" ? "current" : "");
            }
            if (currentSelectType.ToLower() == "yongche" || (newCount != null && newCount.ContainsKey(brandId) && newCount[brandId].ContainsKey("yongche") && newCount[brandId]["yongche"] > 0))
            {
                liList.AppendFormat("<li class=\"{1}\"><a  href=\"/{0}/yongche/\">�ó�</a></li>"
                               , brandSpell
                               , currentSelectType == "yongche" ? "current" : "");
            }
            liList.AppendLine("</ul>");
            liList.AppendLine("</div>");
            liList.AppendLine("</div>");
            return liList.ToString();
        }
        #endregion

        #region ��Ʒ�� Ʒ�� ������Ʒ��

        /// <summary>
        /// ȡƷ����������Ʒ��
        /// </summary>
        /// <param name="cbid">Ʒ��ID</param>
        /// <param name="top">ȡǰ����</param>
        /// <returns></returns>
        public DataSet GetBrandHotSerial(int cbid, int top)
        {
            if (top < 1 || top > 50)
            { top = 10; }
            DataSet ds = new DataSet();
            string sql = @"select top {0} cs.cb_id,cs.allSpell,cs.cs_id,cs.cs_name,cs.cs_showname,cs3.UVCount
                                            from dbo.Car_Serial cs 
                                            left join dbo.Car_Serial_30UV cs3 on cs.cs_id=cs3.cs_id
                                            where cs.isState=1 and cs.cb_id={1}
                                            order by cs3.UVCount desc";
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, string.Format(sql, top, cbid));
            return ds;
        }

        /// <summary>
        /// ȡ��Ʒ����������Ʒ��
        /// </summary>
        /// <param name="bsid">��Ʒ��ID</param>
        /// <param name="top">ȡǰ����</param>
        /// <returns></returns>
        public DataSet GetMasterHotSerial(int bsid, int top)
        {
            if (top < 1 || top > 50)
            { top = 10; }
            DataSet ds = new DataSet();
            string sql = @"select top {0} cmbr.bs_id,cs.cb_id,cs.allSpell,cs.cs_id,cs.cs_name,
                                                cs.cs_showname,cs3.UVCount
                                                from dbo.Car_Serial cs 
                                                left join car_brand cb on cs.cb_id=cb.cb_id  
                                                left join Car_MasterBrand_Rel cmbr on cb.cb_id=cmbr.cb_id                                    
                                                left join dbo.Car_Serial_30UV cs3 on cs.cs_id=cs3.cs_id 
                                                where cs.isState=1 and cmbr.bs_id={1}                       
                                                order by cs3.UVCount desc";
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, string.Format(sql, top, bsid));
            return ds;
        }

        #endregion

        #region �û���Ϣ
        /// <summary>
        /// ������ʾ�û���ǩ��Ʒ���б�
        /// </summary>
        public List<int> GetZhiHuanList()
        {
            string cacheKey = "Car_BrandBll_GetZhiHuanList";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                List<int> data = null;
                XmlDocument autoDataXml = AutoStorageService.GetAutoXml();
                if (autoDataXml != null)
                {
                    List<int> serialIds = new Car_SerialBll().GetAllZhiHuanCsID();
                    if (serialIds != null && serialIds.Count > 0)
                    {
                        XmlNode brandIdNode = null;
                        int brandId;
                        data = new List<int>();
                        foreach (int serialId in serialIds)
                        {
                            brandIdNode = autoDataXml.SelectSingleNode(string.Format("/Params/MasterBrand/Brand[Serial[@ID='{0}']]/@ID", serialId.ToString()));
                            if (brandIdNode != null)
                            {
                                brandId = ConvertHelper.GetInteger(brandIdNode.Value);
                                if (!data.Contains(brandId))
                                    data.Add(brandId);
                            }
                        }
                    }
                }
                if (data == null)
                    obj = new object();
                else
                    obj = data;
                CacheManager.InsertCache(cacheKey, obj, 15);
            }
            return obj as List<int>;
        }
        /// <summary>
        /// ��ȡ���û���Ϣ�ĳ���
        /// </summary>
        public List<int> GetZhiHuanCityIdList(int brandId)
        {
            if (brandId < 1) return null;
            string cacheKey = "Car_BrandBll_GetZhiHuanCityIdList_" + brandId.ToString();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj == null)
            {
                List<int> data = null;
                List<int> brindList = GetZhiHuanList();
                if (brindList != null && brindList.Contains(brandId))
                {
                    XmlDocument doc = AutoStorageService.GetAutoXml();
                    if (doc != null)
                    {
                        XmlNodeList nodes = doc.SelectNodes(string.Format("//Params/MasterBrand/Brand[@ID='{0}']/Serial", brandId.ToString()));
                        if (nodes.Count > 0)
                        {
                            List<int> serialIds = new List<int>(nodes.Count);
                            foreach (XmlNode node in nodes)
                            {
                                serialIds.Add(ConvertHelper.GetInteger(node.Attributes["ID"].Value));
                            }
                            DataSet ds = CarBrandDal.GetZhiHuanCityIdList(serialIds);
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                data = new List<int>(ds.Tables[0].Rows.Count);
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    data.Add(ConvertHelper.GetInteger(row["cityid"].ToString()));
                                }
                            }
                        }
                    }
                }
                if (data == null)
                    obj = new object();
                else
                    obj = data;
                CacheManager.InsertCache(cacheKey, obj, 30);
            }
            return obj as List<int>;
        }
        #endregion

        #region Ʒ����������Ʒ��
        /// <summary>
        /// �õ�Ʒ���µ�������Ʒ��
        /// </summary>
        /// <returns></returns>
        public string GetBrandOtherSerial(int brandId, int currentCsId)
        {
            Car_BrandEntity brandEntity = GetBrandEntityByBrandId(brandId);
            if (brandEntity == null)
            {
                return string.Empty;
            }
            List<CarSerialPhotoEntity> carSerialPhotoList = GetCarSerialPhotoListByCBID(brandId, false);
            carSerialPhotoList.Sort(Car_SerialBll.CompareSerialName);
            if (carSerialPhotoList == null || carSerialPhotoList.Count < 1)
            {
                return string.Empty;
            }

            int forLastCount = 0;
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                if (entity.SerialLevel == "���" || entity.SerialId == currentCsId)
                {
                    continue;
                }
                forLastCount++;
            }

            StringBuilder contentBuilder = new StringBuilder(40);
            //string serialUrl = "<a target=\"_blank\" href=\"http://car.bitauto.com/{0}/\">{1}</a>";
            //int index = 0;
            PageBase pageBase = new PageBase();
            foreach (CarSerialPhotoEntity entity in carSerialPhotoList)
            {
                bool IsExitsUrl = true;
                if (entity.SerialLevel == "���" || entity.SerialId == currentCsId)
                {
                    continue;
                }
                string priceRange = pageBase.GetSerialPriceRangeByID(entity.SerialId);
                if (entity.SaleState == "����")
                {
                    IsExitsUrl = false;
                    priceRange = "δ����";
                }
                else if (priceRange.Trim().Length == 0)
                {
                    IsExitsUrl = false;
                    priceRange = "���ޱ���";
                }
                if (IsExitsUrl)
                {
                    priceRange = string.Format("<a href='http://price.bitauto.com/brand.aspx?newbrandid={0}'>{1}</a>", entity.SerialId, priceRange);
                }
                string tempCsSeoName = string.IsNullOrEmpty(entity.CS_SeoName) ? entity.ShowName : entity.CS_SeoName;
                contentBuilder.Append("<li>");
                contentBuilder.AppendFormat("    <div class=\"txt\"><a href=\"/{0}/\" target=\"_blank\">{1}</a></div>", entity.CS_AllSpell, tempCsSeoName);
                contentBuilder.AppendFormat("    <span>{0}</span>", priceRange);
                contentBuilder.Append("</li>");
            }

            StringBuilder brandOtherSerial = new StringBuilder(string.Empty);
            if (contentBuilder.Length > 0)
            {
                brandOtherSerial.Append("<div class=\"section-header header3\">");
                brandOtherSerial.Append("    <div class=\"box\">");
				brandOtherSerial.AppendFormat("<h2><a href=\"/{0}/\" target=\"_blank\">ͬƷ�Ƴ���</a></h2>", brandEntity.Cb_AllSpell);
                brandOtherSerial.Append("    </div>");
                brandOtherSerial.Append("</div>");
                brandOtherSerial.Append("<div class=\"list-txt list-txt-s list-txt-default list-txt-style5\" data-channelid=\"2.21.834\">");
                brandOtherSerial.Append("    <ul>");
                brandOtherSerial.Append(contentBuilder);
                brandOtherSerial.Append("    </ul>");
                brandOtherSerial.Append("</div>");
            }

            return brandOtherSerial.ToString();
        }
        #endregion

    }

    public class BrandForum
    {
        private string m_campForum;
        private DataTable m_forumList;
        private DataTable m_subjectList;

        public BrandForum()
        {
            m_campForum = String.Empty;
            m_forumList = new DataTable();
            m_subjectList = new DataTable();
        }

        /// <summary>
        /// ����Դ�����ļ���ʼ������
        /// </summary>
        /// <param name="sourceFile"></param>
        public BrandForum(string sourceFile)
            : this()
        {
            InitData(sourceFile);
        }

        /// <summary>
        /// ��Ӫ��̳��ַ
        /// </summary>
        public string CampForumUrl
        {
            get { return m_campForum; }
            set { m_campForum = value; }
        }

        /// <summary>
        /// �����̳�б�
        /// </summary>
        public DataTable ForumList
        {
            get { return m_forumList; }
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable SubjectList
        {
            get { return m_subjectList; }
        }

        /// <summary>
        /// ����Դ�����ļ���ʼ������
        /// </summary>
        /// <param name="sourceFile"></param>
        public void InitData(string sourceFile)
        {
            if (!File.Exists(sourceFile))
                return;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(sourceFile);
                XmlNode campNode = xmlDoc.SelectSingleNode("/ForumInfo/CampUrl");
                m_campForum = campNode.InnerText;

                XmlNode forumsNode = xmlDoc.SelectSingleNode("/ForumInfo/Forums");
                StringReader sr = null;
                if (forumsNode.FirstChild != null && forumsNode.FirstChild.ChildNodes.Count > 0)
                {
                    sr = new StringReader(forumsNode.InnerXml);
                    m_forumList.ReadXml(sr);
                    sr.Close();
                }

                XmlNode subjectsNode = xmlDoc.SelectSingleNode("/ForumInfo/Subjects");
                if (subjectsNode.FirstChild != null && subjectsNode.FirstChild.ChildNodes.Count > 0)
                {
                    sr = new StringReader(subjectsNode.InnerXml);
                    m_subjectList.ReadXml(sr);
                    sr.Close();
                }
            }
            catch { }
        }
    }
}
