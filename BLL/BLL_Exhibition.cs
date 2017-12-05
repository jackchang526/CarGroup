using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.BLL
{
    /// <summary>
    /// Exhibition 的摘要说明
    /// </summary>
    public class Exhibition
    {
        /// <summary>
        /// 通过展会ID返回展会的接口XML字符串
        /// </summary>
        /// <param name="ExhibitionID">展会ID</param>
        /// <returns>XML字符串</returns>
        public static string ExhibitionXMLString(int exhibitionID, int expireTime)
        {
            if (exhibitionID < 1)
            {
                return ExhibitionListXMLString(GetModelExhibitionList(expireTime));
            }
            return ExhibitionXMLString(GetModelExhibitionByExhibitionID(exhibitionID, expireTime));
        }
        /// <summary>
        /// 返回展会的全列表XML
        /// </summary>
        /// <param name="modelExhibitionList">展会列表</param>
        /// <returns>返回XML字符串</returns>
        protected static string ExhibitionListXMLString(List<Model.Exhibition> modelExhibitionList)
        {
            StringBuilder xmlBuilder = new StringBuilder("<ExhibitionList>");
            foreach (Model.Exhibition modelExhibition in modelExhibitionList)
            {
                xmlBuilder.Append(GetExhibitionSpanXMLString(modelExhibition));
            }
            xmlBuilder.Append("</ExhibitionList>");
            return xmlBuilder.ToString();
        }
        /// <summary>
        /// 返回展会的XML
        /// </summary>
        /// <param name="modelExhibitionList">展会对象</param>
        /// <returns>返回XML字符串</returns>
        protected static string ExhibitionXMLString(Model.Exhibition modelExhibition)
        {
            if (modelExhibition == null || modelExhibition.ID < 1)
            {
                return "";
            }
            StringBuilder xmlBuilder = new StringBuilder("<ExhibitionList>");
            xmlBuilder.Append(GetExhibitionSpanXMLString(modelExhibition));
            xmlBuilder.Append("</ExhibitionList>");
            return xmlBuilder.ToString();
        }
        /// <summary>
        /// 得到展会的XML片段
        /// </summary>
        /// <param name="modelExhibtion">展会对象</param>
        /// <returns>XML片段</returns>
        protected static string GetExhibitionSpanXMLString(Model.Exhibition modelExhibition)
        {
            if (modelExhibition == null || modelExhibition.ID < 1)
            {
                return "";
            }
            StringBuilder xmlBuilder = new StringBuilder("");
            xmlBuilder.AppendFormat("<Exhibition id=\"{0}\" Name=\"{1}\" Status=\"{2}\">", modelExhibition.ID.ToString()
                                    , modelExhibition.Name, modelExhibition.Status.ToString());

            //绑定展馆结点
            if (modelExhibition.PavilionList == null || modelExhibition.PavilionList.Count < 1)
            {
                xmlBuilder.AppendFormat("<Pavilion />");
            }
            else
            {
                xmlBuilder.AppendFormat("<Pavilion>");
            }
            int index = 0;
            foreach (KeyValuePair<int, Model.Pavilion> pavKeyValue in modelExhibition.PavilionList)
            {
                if ((index + 1) == modelExhibition.PavilionList.Count)
                {
                    xmlBuilder.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\"></Item>", pavKeyValue.Key, pavKeyValue.Value.Name);
                    xmlBuilder.AppendFormat("</Pavilion>");
                    continue;
                }
                xmlBuilder.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\"></Item>", pavKeyValue.Key, pavKeyValue.Value.Name);
                index++;
            }
            //绑定展会属性结点
            if (modelExhibition.AttributeList == null || modelExhibition.AttributeList.Count < 1)
            {
                xmlBuilder.AppendFormat("<Attibute />");
            }
            else
            {
                xmlBuilder.AppendFormat("<Attibute>");
            }
            index = 0;
            foreach (KeyValuePair<int, Model.Attribute> attrKeyValue in modelExhibition.AttributeList)
            {
                if ((index + 1) == modelExhibition.AttributeList.Count)
                {
                    xmlBuilder.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\"></Item>", attrKeyValue.Key, attrKeyValue.Value.Name);
                    xmlBuilder.AppendFormat("</Attibute>");
                    continue;
                }
                xmlBuilder.AppendFormat("<Item ID=\"{0}\" Name=\"{1}\"></Item>", attrKeyValue.Key, attrKeyValue.Value.Name);
                index++;
            }
            //绑定展会中车属性结点
            if (modelExhibition.MasterBrandList == null || modelExhibition.MasterBrandList.Count < 1)
            {
                xmlBuilder.AppendFormat("<Car />");
            }
            else
            {
                xmlBuilder.AppendFormat("<Car>");
            }
            index = 0;
            foreach (KeyValuePair<int, int[]> brandKeyValue in modelExhibition.MasterBrandList)
            {
                string masterCountry = MasterBrandCountry(brandKeyValue.Key);
                string masterSpell = MasterBrandSpell(brandKeyValue.Key);
                if ((index + 1) == modelExhibition.MasterBrandList.Count)
                {
                    xmlBuilder.AppendFormat("<MasterBrand ID=\"{0}\" Name=\"{1}\" IsPav=\"{2}\" AllSpell=\"{3}\" >"
                                            , brandKeyValue.Key
                                            , MasterBrandName(brandKeyValue.Key)
                                            , MasterBrandIsPav(brandKeyValue.Key, modelExhibition.PavilionList)
                                            , masterSpell);
                    //绑定子品牌
                    foreach (int serialID in brandKeyValue.Value)
                    {
                        xmlBuilder.AppendFormat("<Serial ID=\"{0}\" Name=\"{1}\" IsPav=\"{2}\" cssLevel=\"{4}\" IsAttr=\"{3}\" MasterCountry=\"{5}\" AllSpell=\"{6}\" MasterSpell=\"{7}\" PriceRange=\"{8}\" ShowName=\"{9}\" ></Serial>"
                                            , serialID
                                            , SerialBrandName(serialID)
                                            // modifed by chengl Nov.25.2009
                                            , MasterBrandIsPav(brandKeyValue.Key, modelExhibition.PavilionList)
                                            // , SerialBrandIsPav(brandKeyValue.Key, serialID, modelExhibition.PavilionList)
                                            , SerialBrandIsAttr(brandKeyValue.Key, serialID, modelExhibition.AttributeList)
                                            , SerialBrandCssLevel(serialID)
                                            , masterCountry
                                            , SerialBrandAllSpell(serialID)
                                            , masterSpell
                                            , GetSerialPriceRange(serialID)
                                            , SerialBrandShowName(serialID));
                    }
                    xmlBuilder.Append("</MasterBrand>");
                    xmlBuilder.AppendFormat("</Car>");
                    continue;
                }
                xmlBuilder.AppendFormat("<MasterBrand ID=\"{0}\" Name=\"{1}\" IsPav=\"{2}\" AllSpell=\"{3}\">"
                                          , brandKeyValue.Key
                                          , MasterBrandName(brandKeyValue.Key)
                                          , MasterBrandIsPav(brandKeyValue.Key, modelExhibition.PavilionList)
                                          , masterSpell);
                foreach (int serialID in brandKeyValue.Value)
                {
                    xmlBuilder.AppendFormat("<Serial ID=\"{0}\" Name=\"{1}\" IsPav=\"{2}\" cssLevel=\"{4}\" IsAttr=\"{3}\" MasterCountry=\"{5}\" AllSpell=\"{6}\" MasterSpell=\"{7}\" PriceRange=\"{8}\" ShowName=\"{9}\" ></Serial>"
                                             , serialID
                                             , SerialBrandName(serialID)
                                            // modifed by chengl Nov.25.2009
                                            , MasterBrandIsPav(brandKeyValue.Key, modelExhibition.PavilionList)
                                             // , SerialBrandIsPav(brandKeyValue.Key, serialID, modelExhibition.PavilionList)
                                             , SerialBrandIsAttr(brandKeyValue.Key, serialID, modelExhibition.AttributeList)
                                             , SerialBrandCssLevel(serialID)
                                             , masterCountry
                                             , SerialBrandAllSpell(serialID)
                                             , masterSpell
                                             , GetSerialPriceRange(serialID)
                                             , SerialBrandShowName(serialID));
                }

                xmlBuilder.Append("</MasterBrand>");
                index++;
            }

            xmlBuilder.Append("</Exhibition>");
            return xmlBuilder.ToString();
        }
        /// <summary>
        /// 通过主品牌ID得到它所属展馆
        /// </summary>
        /// <param name="brandID">主品牌ID</param>
        /// <param name="modelPavList">展馆对象</param>
        /// <returns>车所属展馆</returns>
        protected static string MasterBrandIsPav(int brandID, Dictionary<int, Model.Pavilion> modelPavList)
        {
            if (modelPavList == null || modelPavList.Count < 1)
            {
                return "0";
            }
            string isProv = "";
            int index = 0;
            foreach (KeyValuePair<int, Model.Pavilion> pavKeyValue in modelPavList)
            {
                if (pavKeyValue.Value.MasterBrandList != null
                    && pavKeyValue.Value.MasterBrandList.Count > 0
                    && pavKeyValue.Value.MasterBrandList.ContainsKey(brandID))
                {
                    if (index == 0)
                    {
                        isProv += pavKeyValue.Key;
                        index++;
                    }
                    else
                    {
                        isProv += "," + pavKeyValue.Key;
                    }
                }
            }
            if (isProv == "")
            {
                return "0";
            }
            return isProv;
        }
        /// <summary>
        /// 通过主品牌ID和子品牌ID得到它所属展馆
        /// </summary>
        /// <param name="brandID">主品牌ID</param>
        /// <param name="SerialID">子品牌ID</param>
        /// <param name="modelPavList">展馆列表</param>
        /// <returns>所属展馆</returns>
        protected static string SerialBrandIsPav(int brandID, int SerialID, Dictionary<int, Model.Pavilion> modelPavList)
        {
            if (modelPavList == null || modelPavList.Count < 1)
            {
                return "0";
            }
            string isProv = "";
            int index = 0;
            foreach (KeyValuePair<int, Model.Pavilion> pavKeyValue in modelPavList)
            {
                if (pavKeyValue.Value.MasterBrandList != null
                    && pavKeyValue.Value.MasterBrandList.Count > 0
                    && pavKeyValue.Value.MasterBrandList.ContainsKey(brandID))
                {
                    foreach (int tSerialID in pavKeyValue.Value.MasterBrandList[brandID])
                    {
                        if (SerialID == tSerialID && index == 0)
                        {
                            isProv += pavKeyValue.Key;
                            index++;
                        }
                        else if (SerialID == tSerialID)
                        {
                            isProv += "," + pavKeyValue.Key;
                        }
                    }
                }
            }
            if (isProv == "")
            {
                return "0";
            }
            return isProv;
        }
        /// <summary>
        /// 取子品牌价格区间
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        protected static string GetSerialPriceRange(int csID)
        {
            string priceRange = "";
            XmlDocument doc = new XmlDocument();
            string cacheKey = "GetAllAutoDataFromDataDir";
            object GetAllAutoDataFromDataDir = null;
            CacheManager.GetCachedData(cacheKey, out GetAllAutoDataFromDataDir);
            if (GetAllAutoDataFromDataDir == null)
            {
                //add by sk 2013.04.26 增加文件读取失败，换数据源
                //string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\AllAutoData.xml");
                //if (File.Exists(xmlPath))
                //{
                //    doc.Load(xmlPath);
                //}
                doc = AutoStorageService.GetAllAutoXml();
                CacheManager.InsertCache(cacheKey, doc, 60);
            }
            else
            {
                doc = (XmlDocument)GetAllAutoDataFromDataDir;
            }

            if (doc != null && doc.HasChildNodes)
            {
                XmlNode xn = doc.SelectSingleNode("/Params/MasterBrand/Brand/Serial[@ID=\"" + csID.ToString() + "\"]");
                if (xn != null)
                {
                    if (xn.Attributes["MultiPriceRange"].Value.ToString().Trim() != "")
                    {
                        string priceRangeTemp = xn.Attributes["MultiPriceRange"].Value.ToString().Trim();
                        if (priceRangeTemp.IndexOf(',') >= 0)
                        {
                            string[] arrPriceRange = priceRangeTemp.Split(',');
                            if (arrPriceRange.Length > 0)
                            {
                                for (int i = 0; i < arrPriceRange.Length; i++)
                                {
                                    if (priceRange != "")
                                    {
                                        if (arrPriceRange[i].ToString().Trim().Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            priceRange += "," + arrPriceRange[i].Trim();
                                        }
                                    }
                                    else
                                    {
                                        if (arrPriceRange[i].ToString().Trim().Length == 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            priceRange += arrPriceRange[i].Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (priceRange == "")
            {
                priceRange = "0";
            }
            return priceRange;
        }
        /// <summary>
        /// 通过主品牌ID和子品牌ID得到它所属属性
        /// </summary>
        /// <param name="brandID">主品牌ID</param>
        /// <param name="SerialID">子品牌ID</param>
        /// <param name="modelPavList">展馆列表</param>
        /// <returns>所属展馆</returns>
        protected static string SerialBrandIsAttr(int brandID, int SerialID, Dictionary<int, Model.Attribute> modelAttrList)
        {
            if (modelAttrList == null || modelAttrList.Count < 1)
            {
                return "0";
            }
            string isProv = "";
            int index = 0;
            foreach (KeyValuePair<int, Model.Attribute> attrKeyValue in modelAttrList)
            {
                if (attrKeyValue.Value.SerialIDList.ContainsKey(SerialID))
                {
                    if (index == 0)
                    {
                        isProv += attrKeyValue.Key;
                        index++;
                    }
                    else
                    {
                        isProv += "," + attrKeyValue.Key;
                    }
                }
            }
            if (isProv == "")
            {
                return "0";
            }
            return isProv;
        }
        /// <summary>
        /// 根据主品牌ID返回主品牌名称
        /// </summary>
        /// <returns>名称</returns>
        protected static string MasterBrandName(int brandID)
        {
            string masterName = "";
            DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(brandID);
            if (drInfo != null)
            {
                masterName = drInfo["bs_name"].ToString().Trim();
            }
            return masterName;
        }
        protected static string MasterBrandSpell(int brandID)
        {
            string masterSpell = "";
            DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(brandID);
            if (drInfo != null)
            {
                masterSpell = drInfo["urlSpell"].ToString().Trim().ToLower();
            }
            return masterSpell;
        }
        /// <summary>
        /// 根据主品牌ID返回主品牌名称
        /// </summary>
        /// <returns>名称</returns>
        protected static string MasterBrandCountry(int brandID)
        {
            string masterCountry = "";
            string tempMasterCountry = "";
            DataRow drInfo = new Car_BrandBll().GetCarMasterBrandInfoByBSID(brandID);
            if (drInfo != null)
            {
                tempMasterCountry = drInfo["bs_Country"].ToString().Trim();
            }

            if (tempMasterCountry == "中国")
            {
                masterCountry = "本土新车";
            }
            else if (tempMasterCountry == "美国")
            {
                masterCountry = "美系新车";
            }
            else if (tempMasterCountry == "日本" || tempMasterCountry == "韩国")
            {
                masterCountry = "日韩新车";
            }
            else
            {
                masterCountry = "欧系新车";
            }

            return masterCountry;
        }
        /// <summary>
        /// 通过子品牌ID得到子品牌名称
        /// </summary>
        /// <param name="serialID"></param>
        /// <returns></returns>
        protected static string SerialBrandName(int serialID)
        {
            string csName = "";
            Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(serialID);
            if (cse.Cs_Id > 0)
            {
                csName = cse.Cs_Name;
            }
            return csName;
        }
        protected static string SerialBrandShowName(int serialID)
        {
            string csShowName = "";
            Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(serialID);
            if (cse.Cs_Id > 0)
            {
                csShowName = cse.Cs_ShowName.Trim();
            }
            return csShowName;
        }
        protected static string SerialBrandAllSpell(int serialID)
        {
            string allSpell = "";
            Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(serialID);
            if (cse.Cs_Id > 0)
            {
                allSpell = cse.Cs_AllSpell.ToLower();
            }
            return allSpell;
        }
        /// <summary>
        /// 通过子品牌ID得到车级别
        /// </summary>
        /// <param name="serialID"></param>
        /// <returns></returns>
        protected static string SerialBrandCssLevel(int serialID)
        {
            string carLevel = "";
            Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(serialID);
            if (cse.Cs_Id > 0)
            {
                carLevel = cse.Cs_CarLevel;
            }
            return carLevel;
        }
        /// <summary>
        /// 得到展会的列表
        /// </summary>
        /// <returns>得到展会列表</returns>
        public static List<Model.Exhibition> GetModelExhibitionList(int expireTime)
        {
            List<Model.Exhibition> modelExhibtionList = new List<BitAuto.CarChannel.Model.Exhibition>();
            string cacheKey = "Exhibition_ID_0";
            object GetModelExhibitionList = null;
            CacheManager.GetCachedData(cacheKey, out GetModelExhibitionList);
            if (GetModelExhibitionList == null)
            {
                modelExhibtionList = DAL.Exhibition.GetExhibtionList();
                CacheManager.InsertCache(cacheKey, modelExhibtionList, expireTime);
            }
            else
            {
                modelExhibtionList = (List<Model.Exhibition>)GetModelExhibitionList;
            }
            return modelExhibtionList;
            // return Common.Cache.Exhibition.GetExhibtionList(expireTime);
        }
        /// <summary>
        /// 得到展会的列表
        /// </summary>
        /// <returns>得到展会列表</returns>
        public static Model.Exhibition GetModelExhibitionByExhibitionID(int exhibitionID, int expireTime)
        {
            Model.Exhibition modelExhibtion = new BitAuto.CarChannel.Model.Exhibition();
            string cacheKey = "Exhibition_ID_" + exhibitionID.ToString();
            object GetModelExhibitionByExhibitionID = null;
            CacheManager.GetCachedData(cacheKey, out GetModelExhibitionByExhibitionID);
            if (GetModelExhibitionByExhibitionID == null)
            {
                modelExhibtion = BitAuto.CarChannel.DAL.Exhibition.GetExhibtionByExhibitionID(exhibitionID);
                CacheManager.InsertCache(cacheKey, modelExhibtion, expireTime);
            }
            else
            {
                modelExhibtion = (Model.Exhibition)GetModelExhibitionByExhibitionID;
            }
            return modelExhibtion;
            // return Common.Cache.Exhibition.GetExhibitionByExhibitionID(exhibitionID, expireTime);
        }
        /// <summary>
        /// 根据主品牌ID和展会ID得到展馆对象
        /// </summary>
        /// <param name="masterBrandID">主品牌ID</param>
        /// <param name="exhibitionID">展会ID</param>
        /// <param name="modelExhibition">展会对象</param>
        /// <returns>展馆对象</returns>
        public static Model.Pavilion GetPavilionByMasterBrandID(int masterBrandID, int exhibitionID, int ExpireTime, out Model.Exhibition modelExhibition)
        {
            modelExhibition = new BitAuto.CarChannel.Model.Exhibition();
            if (masterBrandID < 1 || exhibitionID < 1)
            {
                return null;
            }
            modelExhibition = GetModelExhibitionByExhibitionID(exhibitionID, ExpireTime);
            if (modelExhibition == null || modelExhibition.ID < 1)
            {
                return null;
            }
            Dictionary<int, Model.Pavilion> modelPavilionList = modelExhibition.PavilionList;
            if (modelPavilionList == null || modelPavilionList.Count < 1)
            {
                return null;
            }

            foreach (KeyValuePair<int, Model.Pavilion> pavKeyValue in modelPavilionList)
            {
                if (pavKeyValue.Value.MasterBrandList != null
                    && pavKeyValue.Value.MasterBrandList.Count > 1
                    && pavKeyValue.Value.MasterBrandList.ContainsKey(masterBrandID))
                {
                    return pavKeyValue.Value;
                }
            }
            return null;
        }
        /// <summary>
        /// 根据子品牌ID和展会ID得到属性列表
        /// </summary>
        /// <param name="SerialID">车品牌ID</param>
        /// <param name="exhibitionID">展会ID</param>
        /// <param name="ExpireTime">缓存时间</param>
        /// <returns>属性列表</returns>
        public static List<Model.Attribute> GetAttributeListBySerialID(int SerialID, int exhibitionID, int ExpireTime)
        {
            Model.Exhibition modelExhibtion = GetModelExhibitionByExhibitionID(exhibitionID, ExpireTime);
            if (modelExhibtion == null || modelExhibtion.ID < 1)
            {
                return null;
            }
            Dictionary<int, Model.Attribute> modelAttributeList = modelExhibtion.AttributeList;
            if (modelAttributeList == null || modelAttributeList.Count < 1)
            {
                return null;
            }
            List<Model.Attribute> attributeList = new List<BitAuto.CarChannel.Model.Attribute>();
            foreach (KeyValuePair<int, Model.Attribute> attrValue in modelAttributeList)
            {
                if (attrValue.Value.SerialIDList == null || attrValue.Value.SerialIDList.Count < 1)
                {
                    continue;
                }
                if (attrValue.Value.SerialIDList.ContainsKey(SerialID))
                {
                    attributeList.Add(attrValue.Value);
                }
            }
            return attributeList;
        }

        /// <summary>
        /// 通过展会ID，得到展馆列表
        /// </summary>
        /// <param name="exhibitionId">展会ID</param>
        /// <param name="ExpireTime">过期时间</param>
        /// <returns></returns>
        public virtual Dictionary<int, Model.Pavilion> GetPavilionListByExhibitionId(int exhibitionId, int expireTime)
        {
            if (exhibitionId < 1)
            {
                return null;
            }

            string cacheKey = "Exhibition_" + exhibitionId.ToString() + "_Pavilion";
            object getpavilionList = null;
            bool IsSuccess = CacheManager.GetCachedData(cacheKey, out getpavilionList);

            if (IsSuccess && getpavilionList != null)
            {
                return (Dictionary<int, Model.Pavilion>)getpavilionList;
            }

            Dictionary<int, Model.Pavilion> pavilionList = new Dictionary<int, Pavilion>();
            pavilionList = GetPavilionListByExhibitionId(exhibitionId);

            if (pavilionList == null || pavilionList.Count < 1)
            {
                return null;
            }

            CacheManager.InsertCache(cacheKey, pavilionList, expireTime);
            return pavilionList;
        }
        /// <summary>
        /// 通过展会ID，得到展会XML
        /// </summary>
        /// <param name="exhibitionId">展会ID</param>
        /// <param name="ExpireTime">过期时间</param>
        /// <returns></returns>
        public virtual XmlDocument GetExibitionXmlByExhibitionId(int exhibitionId, int expireTime)
        {
            if (exhibitionId < 1)
            {
                return null;
            }
            string cacheKey = "Exhibition_" + exhibitionId.ToString();
            object getExhibitionXml = null;
            bool IsSuccess = CacheManager.GetCachedData(cacheKey, out getExhibitionXml);

            if (IsSuccess && getExhibitionXml != null)
            {
                return (XmlDocument)getExhibitionXml;
            }
            XmlDocument xmlDocExhibition = new XmlDocument();
            xmlDocExhibition = GetExibitionXmlByExhibitionId(exhibitionId);
            if (xmlDocExhibition == null)
            {
                return null;
            }
            CacheManager.InsertCache(cacheKey, xmlDocExhibition, expireTime);
            return xmlDocExhibition;
        }
        /// <summary>
        /// 通过展会ID，得到展会标签
        /// </summary>
        /// <param name="exhibitionId">展会ID</param>
        /// <param name="ExpireTime">过期时间</param>
        /// <returns></returns>
        public virtual Dictionary<int, Model.Attribute> GetAttributeListByExhibitionId(int exhibitionId, int expireTime)
        {
            if (exhibitionId < 1)
            {
                return null;
            }

            string cacheKey = "Exhibition_" + exhibitionId.ToString() + "_Attibute";
            object getattributeList = null;

            bool IsSuccess = CacheManager.GetCachedData(cacheKey, out getattributeList);

            if (IsSuccess && getattributeList != null)
            {
                return (Dictionary<int, Model.Attribute>)getattributeList;
            }

            Dictionary<int, Model.Attribute> attributeList = new Dictionary<int, BitAuto.CarChannel.Model.Attribute>();
            attributeList = GetAttributeListByExhibitionId(exhibitionId);

            if (attributeList == null || attributeList.Count < 1)
            {
                return null;
            }

            CacheManager.InsertCache(cacheKey, attributeList, expireTime);

            return attributeList;
        }
        /// <summary>
        /// 得到属性列表
        /// </summary>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        public virtual Dictionary<int, Model.Attribute> GetAttributeListByExhibitionId(int exhibitionId)
        {
            if (exhibitionId < 1)
            {
                return null;
            }

            Dictionary<int, Model.Attribute> attributeList = new Dictionary<int, BitAuto.CarChannel.Model.Attribute>();
            attributeList = new DAL.ExhibitionAttributeDAL().GetAttributeListByExhibitionId(exhibitionId);

            if (attributeList == null || attributeList.Count < 1)
            {
                return null;
            }

            return attributeList;
        }
        /// <summary>
        /// 获取车展主品牌数据 根据车展ID
        /// </summary>
        /// <param name="exhibitionId">车展ID</param>
        /// <returns></returns>
        public XmlDocument GetMasterExhibitionXmlById(int exhibitionId)
        {
            //初始化展馆XML
            XmlDocument xmlDocExhibition = new XmlDocument();
            if (exhibitionId < 1)
            {
                return null;
            }
            //初始化车型XML
            XmlDocument xmlDocSerial = new XmlDocument();
            Model.Exhibition exhibitionModel = new Model.Exhibition();
            exhibitionModel = new DAL.Exhibition().GetExhibitionByID(exhibitionId);

            if (exhibitionModel == null || string.IsNullOrEmpty(exhibitionModel.XmlContent))
            {
                return null;
            }
            Car_SerialBll csb = new Car_SerialBll();
            DataSet dsSerial = csb.GetAllSerialUpLevelInfo();

            xmlDocExhibition.LoadXml("<root>" + exhibitionModel.XmlContent + "</root>");
            //填充主品牌元素
            XmlNodeList xNodeList = xmlDocExhibition.SelectNodes("root/MasterBrand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }
            foreach (XmlElement xEleme in xNodeList)
            {
                if (dsSerial == null || dsSerial.Tables.Count < 1 || dsSerial.Tables[0].Rows.Count < 1) continue;
                DataRow[] drs = dsSerial.Tables[0].Select("bs_Id='" + xEleme.GetAttribute("ID") + "'");
                if (drs == null || drs.Length < 1)
                {
                    xEleme.ParentNode.RemoveChild(xEleme);
                    continue;
                }
                xEleme.SetAttribute("Name", drs[0]["bs_Name"].ToString());
                xEleme.SetAttribute("AllSpell", drs[0]["urlspell"].ToString().ToLower());
                xEleme.SetAttribute("MasterSEOName", drs[0]["bs_seoname"].ToString());
                xEleme.SetAttribute("Firstchar", drs[0]["bs_spell"].ToString().Substring(0, 1).ToUpper());
            }
            return xmlDocExhibition;
        }

        /// <summary>
        /// 获取车展主品牌数据 根据车展ID。2015广州车展新增展馆-主品牌对应关系，
        /// </summary>
        /// <param name="exhibitionId">车展ID</param>
        /// <returns></returns>
        public XmlDocument GetPavilionMasterXmlById(int exhibitionId)
        {
            //初始化展馆XML
            var xmlDocPavilion = new XmlDocument();
            if (exhibitionId < 1)
            {
                return null;
            }
            //初始化车型XML
            var str = new DAL.Exhibition().GetPavilionMasterByExhibitionId(exhibitionId);

            if (str == null || string.IsNullOrEmpty(str))
            {
                return null;
            }
            Car_SerialBll csb = new Car_SerialBll();
            DataSet dsSerial = csb.GetAllSerialUpLevelInfo();

            xmlDocPavilion.LoadXml("<root>" + str + "</root>");
            //填充主品牌元素
            XmlNodeList xNodeList = xmlDocPavilion.SelectNodes("root/Pavilion/MasterBrand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }
            foreach (XmlElement xEleme in xNodeList)
            {
                if (dsSerial == null || dsSerial.Tables.Count < 1 || dsSerial.Tables[0].Rows.Count < 1) continue;
                DataRow[] drs = dsSerial.Tables[0].Select("bs_Id='" + xEleme.GetAttribute("ID") + "'");
                if (drs == null || drs.Length < 1)
                {
                    xEleme.ParentNode.RemoveChild(xEleme);
                    continue;
                }
                xEleme.SetAttribute("Name", drs[0]["bs_Name"].ToString());
                xEleme.SetAttribute("AllSpell", drs[0]["urlspell"].ToString().ToLower());
                xEleme.SetAttribute("MasterSEOName", drs[0]["bs_seoname"].ToString());
                xEleme.SetAttribute("Firstchar", drs[0]["bs_spell"].ToString().Substring(0, 1).ToUpper());
            }
            return xmlDocPavilion;
        }
        /// <summary>
        /// 通过展会ID，得到展会XML
        /// </summary>
        /// <param name="exhibitionId">展会ID</param>
        /// <param name="ExpireTime">过期时间</param>
        /// <returns></returns>
        public virtual XmlDocument GetExibitionXmlByExhibitionId(int exhibitionId)
        {
            //初始化展馆XML
            XmlDocument xmlDocExhibition = new XmlDocument();

            //string cacheKey = "GetExibitionXmlByExhibitionId_" + exhibitionId.ToString();
            //object getExibitionXmlByExhibitionId = CacheManager.GetCachedData(cacheKey);

            //if (getExibitionXmlByExhibitionId != null)
            //{
            //    xmlDocExhibition = (XmlDocument)getExibitionXmlByExhibitionId;
            //}
            //else
            //{
            if (exhibitionId < 1)
            {
                return null;
            }
            //初始化车型XML
            XmlDocument xmlDocSerial = new XmlDocument();
            Model.Exhibition exhibitionModel = new Model.Exhibition();
            exhibitionModel = new DAL.Exhibition().GetExhibitionByID(exhibitionId);

            if (exhibitionModel == null || string.IsNullOrEmpty(exhibitionModel.XmlContent))
            {
                return null;
            }
            Car_SerialBll csb = new Car_SerialBll();
            DataSet dsSerial = csb.GetAllSerialUpLevelInfo();

            //modified by sk 2013.04.26 增加文件读取失败，换数据源
            //xmlDocSerial.Load(WebConfig.DataBlockPath + "Data/MasterToBrandToSerialAllSaleAndLevel.xml");
            xmlDocSerial = AutoStorageService.GetAllAutoAndLevelXml();
            if (xmlDocSerial == null || !xmlDocSerial.HasChildNodes)
            {
                return null;
            }

            xmlDocExhibition.LoadXml("<root>" + exhibitionModel.XmlContent + "</root>");
            //填充主品牌元素
            XmlNodeList xNodeList = xmlDocExhibition.SelectNodes("root/MasterBrand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }
            XmlElement xNode;
            foreach (XmlElement xEleme in xNodeList)
            {
                if (dsSerial == null || dsSerial.Tables.Count < 1 || dsSerial.Tables[0].Rows.Count < 1) continue;
                DataRow[] drs = dsSerial.Tables[0].Select("bs_Id='" + xEleme.GetAttribute("ID") + "'");
                if (drs == null || drs.Length < 1)
                {
                    xEleme.ParentNode.RemoveChild(xEleme);
                    continue;
                }

                xEleme.SetAttribute("Name", drs[0]["bs_Name"].ToString());
                xEleme.SetAttribute("AllSpell", drs[0]["urlspell"].ToString().ToLower());
                xEleme.SetAttribute("MasterSEOName", drs[0]["bs_seoname"].ToString());
                xEleme.SetAttribute("Firstchar", drs[0]["bs_spell"].ToString().Substring(0, 1).ToUpper());
            }
            //填充品牌元素
            xNodeList = xmlDocExhibition.SelectNodes("root/MasterBrand/Brand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }

            foreach (XmlElement xEleme in xNodeList)
            {
                if (dsSerial == null || dsSerial.Tables.Count < 1 || dsSerial.Tables[0].Rows.Count < 1) continue;
                DataRow[] drs = dsSerial.Tables[0].Select("cb_id='" + xEleme.GetAttribute("ID") + "'");
                if (drs == null || drs.Length < 1) continue;


                xEleme.SetAttribute("Name", drs[0]["cb_name"].ToString());
                xEleme.SetAttribute("AllSpell", drs[0]["cbAllSpell"].ToString().ToLower());
                // add by chengl Apr.15.2015
                xEleme.SetAttribute("CpID", drs[0]["cp_id"].ToString());
            }
            //填充子品牌元素
            xNodeList = xmlDocExhibition.SelectNodes("root/MasterBrand/Brand/Serial");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return null;
            }

            // modified by chengl Apr.23.2010
            foreach (XmlElement xEleme in xNodeList)
            {
                if (dsSerial != null && dsSerial.Tables.Count > 0 && dsSerial.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = dsSerial.Tables[0].Select("cs_id='" + xEleme.GetAttribute("ID") + "'");
                    if (drs != null && drs.Length > 0)
                    {
                        xNode = (XmlElement)xmlDocSerial.SelectSingleNode("Params/MasterBrand/Brand/Serial[@ID='" + xEleme.GetAttribute("ID") + "']");
                        xEleme.SetAttribute("Name", drs[0]["csShowName"].ToString().Trim());
                        xEleme.SetAttribute("sName", drs[0]["csname"].ToString().Trim());
                        xEleme.SetAttribute("AllSpell", drs[0]["csAllSpell"].ToString().Trim().ToLower());
                        //if (drs[0]["csLevel"].ToString().Trim() == "中大型车")
                        //{ xEleme.SetAttribute("CsLevel", "中大型"); }
                        //else if (drs[0]["csLevel"].ToString().Trim() == "紧凑型车")
                        //{ xEleme.SetAttribute("CsLevel", "紧凑型"); }
                        //else
                        //{ xEleme.SetAttribute("CsLevel", drs[0]["csLevel"].ToString().Trim()); }
                        xEleme.SetAttribute("CsLevel", drs[0]["csLevel"].ToString().Trim());
                        xEleme.SetAttribute("SerialSEOName", drs[0]["cs_seoname"].ToString().Trim() == "" ? drs[0]["csShowName"].ToString().Trim() : drs[0]["cs_seoname"].ToString().Trim());
                        xEleme.SetAttribute("Country", drs[0]["CpCountry"].ToString().Trim());
                        if (xNode == null)
                        { xEleme.SetAttribute("MultiPriceRange", ",0,"); }
                        else
                        { xEleme.SetAttribute("MultiPriceRange", xNode.GetAttribute("MultiPriceRange")); }
                    }
                }

            }
            //    CacheManager.InsertCache(cacheKey, xmlDocExhibition, WebConfig.CachedDuration);
            //}

            return xmlDocExhibition;
        }
        /// <summary>
        /// 通过展会ID，得到展馆列表
        /// </summary>
        /// <param name="exhibitionId">展会ID</param>
        /// <param name="ExpireTime">过期时间</param>
        /// <returns></returns>
        public virtual Dictionary<int, Model.Pavilion> GetPavilionListByExhibitionId(int exhibitionId)
        {
            if (exhibitionId < 1)
            {
                return null;
            }

            Dictionary<int, Model.Pavilion> pavilionList = new Dictionary<int, Pavilion>();
            pavilionList = new DAL.PavilionDAL().GetPavilionListByExhibitionId(exhibitionId);

            if (pavilionList == null || pavilionList.Count < 1)
            {
                return null;
            }

            return pavilionList;
        }
        /// <summary>
        /// 得到排序的结果，子品牌
        /// </summary>
        /// <param name="pavilionId">展馆ID</param>
        /// <param name="Level">级别</param>
        /// <returns></returns>
        public virtual Dictionary<int, int> GetOrderSerialTypeByAttributeId(int attributeId, int Level)
        {
            if (attributeId < 1)
            {
                return null;
            }
            if (Level != 2 && Level != 3)
            {
                return null;
            }
            return new DAL.ExhibitionAttributeDAL().GetOrderSerialTypeByAttributeId(attributeId, Level);
        }
        /// <summary>
        /// 得到展会列表
        /// </summary>
        /// <returns></returns>
        public virtual DataSet GetExhibitionList()
        {
            return new DAL.Exhibition().GetExhibitionList();
        }
        /// <summary>
        /// 得到展会对应的子品牌彩虹条列表
        /// </summary>
        /// <param name="serialIdList"></param>
        /// <param name="exhibtionId"></param>
        /// <returns></returns>
        public virtual Dictionary<int, Dictionary<int, Model.ExhibitionRainbow>> GetExhibitionRainbowBySerialIDList(List<int> serialIdList, int exhibtionId)
        {
            if (exhibtionId < 1
                || serialIdList == null
                || serialIdList.Count < 1)
            {
                return null;
            }

            return new DAL.Exhibition().GetExhibitionRainbowBySerialIDList(serialIdList, exhibtionId);
        }
        /// <summary>
        /// 从小到大排
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static int OrderXmlElement(XmlElement pre, XmlElement last)
        {
            int ret = 0;
            int pv1 = Convert.ToInt32(string.IsNullOrEmpty(pre.GetAttribute("PavilionSort")) ? "999" : pre.GetAttribute("PavilionSort"));
            int pv2 = Convert.ToInt32(string.IsNullOrEmpty(last.GetAttribute("PavilionSort")) ? "999" : last.GetAttribute("PavilionSort"));
            if (pv1 > pv2)
                ret = 1;
            else if (pv1 < pv2)
                ret = -1;

            return ret;

        }
        /// <summary>
        /// 从小到大排
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static int OuterOrderXMLElement(XmlElement pre, XmlElement last)
        {
            int ret = 0;
            int pv1 = Convert.ToInt32(string.IsNullOrEmpty(pre.GetAttribute("RBS")) ? "999" : pre.GetAttribute("RBS"));
            int pv2 = Convert.ToInt32(string.IsNullOrEmpty(last.GetAttribute("RBS")) ? "999" : last.GetAttribute("RBS"));
            if (pv1 > pv2)
                ret = 1;
            else if (pv1 < pv2)
                ret = -1;

            return ret;
        }

        /// <summary>
        /// 根据车展ID取展馆
        /// </summary>
        /// <param name="exhibitionId">车展ID</param>
        /// <returns></returns>
        public static DataSet GetAllPravilionByExhibitionId(int exhibitionId)
        {
            DataSet ds = new DataSet();
            //string cacheKey = "GetAllPravilionByExhibitionId_" + exhibitionId.ToString();
            //object getAllPravilionByExhibitionId = null;
            //CacheManager.GetCachedData(cacheKey, out getAllPravilionByExhibitionId);
            //if (getAllPravilionByExhibitionId == null)
            //{
            ds = DAL.Exhibition.GetAllPravilionByExhibitionId(exhibitionId);
            //    CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            //}
            //else
            //{
            //    ds = (DataSet)getAllPravilionByExhibitionId;
            //}
            return ds;
        }

        /// <summary>
        /// 根据车展ID取属性
        /// </summary>
        /// <param name="exhibitionId">车展ID</param>
        /// <returns></returns>
        public static DataSet GetAllAttributeByExhibitionId(int exhibitionId)
        {
            DataSet ds = new DataSet();
            //string cacheKey = "GetAllAttributeByExhibitionId_" + exhibitionId.ToString();
            //object getAllAttributeByExhibitionId = null;
            //CacheManager.GetCachedData(cacheKey, out getAllAttributeByExhibitionId);
            //if (getAllAttributeByExhibitionId == null)
            //{
            ds = DAL.Exhibition.GetAllAttributeByExhibitionId(exhibitionId);
            //    CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
            //}
            //else
            //{
            //    ds = (DataSet)getAllAttributeByExhibitionId;
            //}
            return ds;
        }

        /// <summary>
        /// 获取车展原始XML数据 根据车展ID
        /// </summary>
        /// <param name="exhibitionId">车展ID</param>
        /// <returns></returns>
        public XmlDocument GetExhibitionXmlById(int exhibitionId)
        {
            //初始化展馆XML
            XmlDocument xmlDocExhibition = new XmlDocument();
            if (exhibitionId < 1)
            {
                return null;
            }
            //初始化车型XML
            XmlDocument xmlDocSerial = new XmlDocument();
            Model.Exhibition exhibitionModel = new Model.Exhibition();
            exhibitionModel = new DAL.Exhibition().GetExhibitionByID(exhibitionId);

            if (exhibitionModel == null || string.IsNullOrEmpty(exhibitionModel.XmlContent))
            {
                return null;
            }
            Car_SerialBll csb = new Car_SerialBll();
            DataSet dsSerial = csb.GetAllSerialUpLevelInfo();

            xmlDocExhibition.LoadXml("<root>" + exhibitionModel.XmlContent + "</root>");
            
            return xmlDocExhibition;
        }
        
    }
     

}
