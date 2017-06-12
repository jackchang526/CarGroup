using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Web;

using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.BLL
{
    public class TuJieTree : TreeData
    {
        private string _TagType = "tujie";
        public string TreeXmlData()
        {
            XmlDocument xmlDoc = AutoStorageService.GetCacheTreeXml();

            XmlNodeList xNodeList = xmlDoc.SelectNodes("data/master");
            XmlElement xElem = (XmlElement)xmlDoc.SelectSingleNode("data");
            if (xmlDoc == null) return "";
            StringBuilder xmlString = new StringBuilder("");
            StringBuilder masterInData = new StringBuilder("");
            string masterElementString = "<master id=\"{0}\" name=\"{1}\" countnum=\"{2}\" firstchar=\"{3}\" extra=\"{4}\">";
            string brandElementString = "<brand id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\">";
            string serialElementString = "<serial id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\"/>";
            int newsTotalCount = 0;

            foreach (XmlElement masterElement in xNodeList)
            {
                int masterBrandNewsCount = 0;
                StringBuilder brandInMasterBrand = new StringBuilder();
                if (masterElement.ChildNodes.Count < 1) continue;

                XmlElement xElemEntity = (XmlElement)masterElement.ChildNodes[0];
                if (xElemEntity.Name.ToLower() == "brand")
                {

                    foreach (XmlElement brandElement in masterElement.ChildNodes)
                    {
                        int BrandNewsCount = 0;
                        StringBuilder SerialInBrand = new StringBuilder();

                        foreach (XmlElement serialElement in brandElement.ChildNodes)
                        {
                            int serialNewsCount = GetSerialId(Convert.ToInt32(serialElement.GetAttribute("id")));
                            if (serialNewsCount == 0) continue;
                            SerialInBrand.AppendFormat(serialElementString
                                                , serialElement.GetAttribute("id")
                                                , serialElement.GetAttribute("name")
                                                , serialNewsCount.ToString()
                                                , serialElement.GetAttribute("extra"));

                            BrandNewsCount += serialNewsCount;
                        }
                        if (BrandNewsCount == 0) continue;
                        brandInMasterBrand.AppendFormat(brandElementString
                                                , brandElement.GetAttribute("id")
                                                , brandElement.GetAttribute("name")
                                                , BrandNewsCount.ToString()
                                                , brandElement.GetAttribute("extra"));
                        brandInMasterBrand.Append(SerialInBrand);
                        brandInMasterBrand.Append("</brand>");

                        masterBrandNewsCount += BrandNewsCount;
                    }

                }
                else
                {
                    foreach (XmlElement serialElement in masterElement.ChildNodes)
                    {
                        int serialNewsCount = GetSerialId(Convert.ToInt32(serialElement.GetAttribute("id")));
                        if (serialNewsCount == 0) continue;

                        masterBrandNewsCount += serialNewsCount;
                        brandInMasterBrand.AppendFormat(serialElementString
                                                , serialElement.GetAttribute("id")
                                                , serialElement.GetAttribute("name")
                                                , serialNewsCount.ToString()
                                                , serialElement.GetAttribute("extra"));
                    }
                }
                //给主品牌赋值
                newsTotalCount += masterBrandNewsCount;
                if (masterBrandNewsCount == 0) continue;

                masterInData.AppendFormat(masterElementString
                                    , masterElement.GetAttribute("id")
                                    , masterElement.GetAttribute("name")
                                    , masterBrandNewsCount.ToString()
                                    , masterElement.GetAttribute("firstchar")
                                    , masterElement.GetAttribute("extra"));
                masterInData.Append(brandInMasterBrand);
                masterInData.Append("</master>");
            }

            xmlString.AppendFormat("<data id=\"0\" countnum=\"{0}\">", newsTotalCount.ToString());
            xmlString.Append(masterInData);
            xmlString.Append("</data>");

            return xmlString.ToString();
        }
        /// <summary>
        /// 得到主品牌的图解文章数量
        /// </summary>
        /// <param name="masterBrandId"></param>
        /// <returns></returns>
        public int GetMasterBrandId(int masterBrandId) { return 0; }
        /// <summary>
        /// 得到品牌的图解文章数量
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public int GetBrandId(int brandId) { return 0; }
        /// <summary>
        /// 得到子品牌的图解文章数量
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public int GetSerialId(int serialId)
        {
            Dictionary<string, Dictionary<int, int>> daoGouNum = AutoStorageService.GetCacheTreeSerialNewsCount();
            if (daoGouNum == null
                || !daoGouNum.ContainsKey(_TagType)
                || !daoGouNum[_TagType].ContainsKey(serialId))
            {
                return 0;
            }

            return daoGouNum[_TagType][serialId];
        }
        /// <summary>
        /// 得到焦点图字符串
        /// </summary>
        /// <returns></returns>
        public string GetForcusImageArea()
        {
            string filePath = HttpContext.Current.Server.MapPath("/include/09gq/09gq_kan/09gq_tujie/00001/09gq_kc_tjxc_jdt_Manual.shtml");
            if (!File.Exists(filePath))
            {
                return "";
            }
            return File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
        }
        /// <summary>
        /// 得到焦点新闻字符串
        /// </summary>
        /// <returns></returns>
        public string GetForcusNewsAree()
        {
            string filePath = HttpContext.Current.Server.MapPath("/include/09gq/09gq_kan/09gq_tujie/00001/09gq_kc_tjxc_jdxw_Auto.shtml");
            if (!File.Exists(filePath))
            {
                return "";
            }
            return File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
        }
        /// <summary>
        /// 得到子品牌新闻的数据
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public DataSet GetNewsListBySerialId(int serialId, int year)
        {
            DataSet newsDs = new DataSet();
            try
            {
                string xmlFile = "";
                if (year == 0)
                    xmlFile = Path.Combine(WebConfig.DataBlockPath
                            , string.Format("Data\\SerialNews\\{0}\\Serial_All_News_{1}.xml"
                            , _TagType
                            , serialId.ToString()));
                else
                    xmlFile = Path.Combine(WebConfig.DataBlockPath
                            , string.Format("Data\\SerialNews\\{0}\\Serial_All_News_{1}_{2}.xml"
                            , _TagType
                            , serialId.ToString()
                            , year.ToString()));
                newsDs.ReadXml(xmlFile);
            }
            catch
            { }
            return newsDs;
        }
        public List<Car_MasterBrandEntity> GetHotMasterBrandEntityList(int entityCount) { return null; }
        public List<Car_SerialBaseEntity> GetHotSerailEntityList(int entityCount) { return null; }
    }
}
