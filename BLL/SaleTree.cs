using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Web.Caching;

using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL
{
    /// <summary>
    /// 销量数据树形
    /// </summary>
    public class SaleTree : TreeData
    {
        private readonly Car_SerialBll _csb = new Car_SerialBll();
        private readonly Car_BrandBll _cbb = new Car_BrandBll();
        string masterElementString = "<master id=\"{0}\" name=\"{1}\" countnum=\"{2}\" firstchar=\"{3}\" extra=\"{4}\">";
        string brandElementString = "<brand id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\">";
        string serialElementString = "<serial id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\"/>";
        string totalString = "<data id=\"0\" countnum=\"{0}\">";
        string _FilePath = Path.Combine(WebConfig.DataBlockPath, "data\\tree_xiaoliang.xml");
        /// <summary>
        /// 产生销量数据的XML树形结构
        /// </summary>
        /// <returns></returns>
        public string TreeXmlData()
        {
            string cacheKeys = "carSalelefttreestring";
            string leftTreeString = (string)CacheManager.GetCachedData(cacheKeys);
            if (string.IsNullOrEmpty(leftTreeString))
            {
                leftTreeString = GetNoCacheTreeXmlData();
                if (string.IsNullOrEmpty(leftTreeString)) return "";
                CacheDependency cd = new CacheDependency(_FilePath);
                CacheManager.InsertCache(cacheKeys, leftTreeString, cd, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
            }

            return leftTreeString;
        }
        #region Delete GetNoCacheTreeXmlData
        
        /// <summary>
        /// 得到没有缓存的树
        /// </summary>
        /// <returns></returns>
        private string GetNoCacheTreeXmlData()
        {
            DataSet treeDs = _csb.GetLeftTreeSaleSerialInfo();
            if (treeDs == null || treeDs.Tables.Count < 1 || treeDs.Tables[0].Rows.Count < 1) return "";
            DataTable dt = BuildMasterBrandPv(treeDs.Tables[0]);
            if (dt == null || dt.Rows.Count < 1) return "";
            DataRow[] drList = dt.Select("", "singleSpell asc ,pvcount desc");
            StringBuilder treeString = new StringBuilder();
            int totalCount = 0;
            foreach (DataRow masterBr in drList)
            {
                if (masterBr.IsNull("bs_id")) continue;
                int bsId = ConvertHelper.GetInteger(masterBr["bs_id"].ToString());
                string bsName = masterBr["bs_Name"].ToString();
                string bsSpell = masterBr["Spell"].ToString();
                string bsallspell = masterBr["bsallspell"].ToString();
                int brandNumber = 0;
                DataView brandDv = treeDs.Tables[0].DefaultView;
                brandDv.RowFilter = "bs_id=" + bsId.ToString();
                DataTable brandDt = brandDv.ToTable(true, "cb_id", "cb_Name", "cballspell", "country");
                brandDv.RowFilter = "";

                if (brandDt == null || brandDt.Rows.Count < 1) continue;
                StringBuilder brandString = new StringBuilder();

                if (brandDt.Rows.Count > 1 ||
                    (!brandDt.Rows[0].IsNull("cb_Name")
                    && brandDt.Rows[0]["cb_Name"].ToString() != bsName))
                {
                    DataRow[] brandList = brandDt.Select("", "country desc,cb_Name asc");

                    foreach (DataRow brandBr in brandList)
                    {
                        if (brandBr.IsNull("cb_id")) continue;
                        int brandId = ConvertHelper.GetInteger(brandBr["cb_id"].ToString());
                        string brandName = brandBr["cb_Name"].ToString();
                        string cballspell = brandBr["cballspell"].ToString();
                        int country = ConvertHelper.GetInteger(brandBr["country"].ToString());
                        int brandCount = 0;

                        DataView serialDv = treeDs.Tables[0].DefaultView;
                        brandDv.RowFilter = "cb_id=" + brandId.ToString();
                        DataTable serialDt = brandDv.ToTable(true, "cs_id", "cs_Name", "csallspell", "CsSaleState");
                        brandDv.RowFilter = "";

                        if (serialDt == null || serialDt.Rows.Count < 1) continue;

                        DataRow[] serialList = serialDt.Select("", "CsSaleState asc,cs_Name asc");
                        StringBuilder serialString = new StringBuilder();
                        foreach (DataRow serialdr in serialList)
                        {
                            if (serialdr.IsNull("cs_id")) continue;
                            brandCount++;

                            int csId = ConvertHelper.GetInteger(serialdr["cs_id"].ToString());
                            string csName = serialdr["cs_Name"].ToString();
                            int csSaleState = ConvertHelper.GetInteger(serialdr["CsSaleState"].ToString());
                            string csallspell = serialdr["csallspell"].ToString();
                            serialString.AppendFormat(serialElementString
                                                    , csId.ToString()
                                                    , csSaleState == 2 ? csName + "(停产)" : csName
                                                    , 0
                                                    , csallspell);
                        }
                        brandString.AppendFormat(brandElementString
                                                , brandId.ToString()
                                                , country == 0 ? "进口" + brandName : brandName
                                                , brandCount.ToString()
                                                , cballspell);
                        brandString.AppendLine(serialString.ToString());
                        brandString.AppendLine("</brand>");
                        brandNumber += brandCount;
                    }
                }
                else
                {
                    if (brandDt.Rows[0].IsNull("cb_id")) continue;
                    DataView serialDv = treeDs.Tables[0].DefaultView;
                    brandDv.RowFilter = "cb_id=" + brandDt.Rows[0]["cb_id"].ToString();
                    DataTable serialDt = brandDv.ToTable(true, "cs_id", "cs_Name", "csallspell", "CsSaleState");
                    brandDv.RowFilter = "";

                    if (serialDt == null || serialDt.Rows.Count < 1) continue;

                    DataRow[] serialList = serialDt.Select("", "CsSaleState asc,cs_Name asc");
                    StringBuilder serialString = new StringBuilder();
                    foreach (DataRow serialdr in serialList)
                    {
                        if (serialdr.IsNull("cs_id")) continue;
                        brandNumber++;

                        int csId = ConvertHelper.GetInteger(serialdr["cs_id"].ToString());
                        string csName = serialdr["cs_Name"].ToString();
                        int csSaleState = ConvertHelper.GetInteger(serialdr["CsSaleState"].ToString());
                        string csallspell = serialdr["csallspell"].ToString();
                        brandString.AppendFormat(serialElementString
                                                , csId.ToString()
                                                , csSaleState == 2 ? csName + "(停产)" : csName
                                                , 0
                                                , csallspell);
                    }
                }
                totalCount += brandNumber;
                treeString.AppendFormat(masterElementString, bsId.ToString(), bsName, brandNumber.ToString(), bsSpell.Substring(0, 1).ToUpper(), bsallspell);
                treeString.AppendLine(brandString.ToString());
                treeString.AppendLine("</master>");
            }

            return string.Format(totalString, totalCount.ToString()) + treeString.ToString() + "</data>";
        }
        #endregion
        /// <summary>
        /// 得到没有缓存的树
        /// </summary>
        /// <returns></returns>
        /*private string GetNoCacheTreeXmlData()
        {
            if (!File.Exists(_FilePath)) return "";
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                //加载文件的内容
                xmlDoc.Load(_FilePath);

                if (xmlDoc == null) return "";
                XmlNodeList xNodeList = xmlDoc.SelectNodes("Params/MasterBrand");
                if (xNodeList == null || xNodeList.Count < 1) return "";
                XmlDocument treeXml = new XmlDocument();
                //创建data元素
                XmlElement dataEleme = treeXml.CreateElement("data");
                dataEleme.SetAttribute("id", "0");
                int datanum = 0;
                foreach (XmlElement masterElement in xNodeList)
                {
                    int masternumber = 0;
                    XmlElement masterElem = treeXml.CreateElement("master");
                    masterElem.SetAttribute("id", masterElement.GetAttribute("ID"));
                    masterElem.SetAttribute("name", masterElement.GetAttribute("Name"));
                    masterElem.SetAttribute("firstchar", masterElement.GetAttribute("Spell").Substring(0, 1).ToUpper());
                    masterElem.SetAttribute("extra", masterElement.GetAttribute("AllSpell"));
                    //如果主品牌没有结点
                    if (!masterElement.HasChildNodes)
                    {
                        //设置主品牌结点的数量属性
                        masterElem.SetAttribute("countnum", "0");
                        //将主品牌结点添加到数据结点
                        dataEleme.AppendChild(masterElem);
                        continue;
                    }
                    //创建品牌列表
                    List<XmlElement> brandXElemList = new List<XmlElement>();
                    foreach (XmlElement xNode in masterElement.ChildNodes)
                    {
                        brandXElemList.Add(xNode);
                    }
                    //循环品牌列表
                    foreach (XmlElement entity in brandXElemList)
                    {
                        int brandCount = 0;
                        bool IsContainsBrand = true;
                        if (brandXElemList.Count == 1
                           && (entity.GetAttribute("Name") == masterElement.GetAttribute("Name")
                           || entity.GetAttribute("Name") == "进口" + masterElement.GetAttribute("Name"))
                           && entity.HasChildNodes)
                        {
                            IsContainsBrand = false;
                        }
                        //创建品牌结点
                        XmlElement brandElem = treeXml.CreateElement("brand");
                        brandElem.SetAttribute("id", entity.GetAttribute("ID"));
                        brandElem.SetAttribute("name", entity.GetAttribute("Name"));
                        brandElem.SetAttribute("extra", entity.GetAttribute("AllSpell"));
                       
                        if (!entity.HasChildNodes && IsContainsBrand)
                        {
                            //设置主品牌结点的数量属性
                            brandElem.SetAttribute("countnum", "0");
                            masterElem.AppendChild(brandElem);
                            continue;
                        }
                        else if (!entity.HasChildNodes)
                        {
                            continue;
                        }

                        List<XmlElement> xSerialElemList = new List<XmlElement>();
                        foreach (XmlElement xSerialElement in entity.ChildNodes)
                        {
                            xSerialElemList.Add(xSerialElement);
                        }
                        //如果结点数量大于1
                        if (xSerialElemList.Count > 1)
                        {
                            xSerialElemList.Sort(NodeCompare.TreeSerialCompare);
                        }
                        foreach (XmlElement serialElement in xSerialElemList)
                        {
                            string serialName = serialElement.GetAttribute("ShowName");
                            if (serialElement.GetAttribute("CsSaleState") == "停销")
                            {
                                serialName += " 停产";
                            } 
                            brandCount++;
                            //创建品牌结点
                            XmlElement serialElem = treeXml.CreateElement("serial");
                            serialElem.SetAttribute("id", serialElement.GetAttribute("ID"));
                            serialElem.SetAttribute("name", serialElement.GetAttribute("Name"));
                            serialElem.SetAttribute("extra", serialElement.GetAttribute("AllSpell"));
                            serialElem.SetAttribute("countnum", "0");
                            //如果存在品牌级别
                            if (IsContainsBrand)
                            {
                                brandElem.AppendChild(serialElem);
                                continue;
                            }
                            masterElem.AppendChild(serialElem);
                        }
                        //如果存在品牌级别
                        if (IsContainsBrand)
                        {
                            //设置主品牌结点的数量属性
                            brandElem.SetAttribute("countnum", brandCount.ToString());
                            masterElem.AppendChild(brandElem);
                        }
                        masternumber += brandCount;
                    }
                    //设置主品牌结点的数量属性
                    masterElem.SetAttribute("countnum", masternumber.ToString());
                    //将主品牌结点添加到数据结点
                    dataEleme.AppendChild(masterElem);
                    //添加数量到总数量
                    datanum += masternumber;
                }
                dataEleme.SetAttribute("countnum", datanum.ToString());
                treeXml.AppendChild(dataEleme);

                return treeXml.InnerXml.ToString();
            }
            catch
            {
                return "";
            }
        }*/
        /// <summary>
        /// 生成包含PV的数据表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable BuildMasterBrandPv(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1) return null;
            if (!dt.Columns.Contains("pvcount"))
                dt.Columns.Add("pvcount", typeof(System.Int32));
            DataTable tempDt = dt.Clone();
            Dictionary<int, int> pvList = _cbb.GetAllMasterDicOrderByUV();
            foreach (DataRow dr in dt.DefaultView.ToTable(true, "bs_id", "bs_Name", "Spell", "bsallspell", "singleSpell", "pvcount").Rows)
            {
                if (dr.IsNull("bs_id")) continue;
                int bsId = ConvertHelper.GetInteger(dr["bs_id"].ToString());
                string bsName = dr["bs_Name"].ToString();
                string bsSpell = dr["Spell"].ToString();
                string bsAllSpell = dr["bsallspell"].ToString();
                string singSpell = dr["singleSpell"].ToString();
                int pvcount = 0;
                if (pvList != null && pvList.ContainsKey(bsId))
                {
                    pvcount = pvList[bsId];
                }
                DataRow newdr = tempDt.NewRow();
                newdr["bs_id"] = bsId;
                newdr["bs_Name"] = bsName;
                newdr["Spell"] = bsSpell;
                newdr["bsallspell"] = bsAllSpell;
                newdr["pvcount"] = pvcount;
                newdr["singleSpell"] = singSpell;
                tempDt.Rows.Add(newdr);
            }
            return tempDt;

        }
        public int GetMasterBrandId(int masterBrandId) { return 0; }
        public int GetBrandId(int brandId) { return 0; }
        public int GetSerialId(int serialId) { return 0; }
        public string GetForcusImageArea() { return null; }
        public string GetForcusNewsAree() { return null; }
        public DataSet GetNewsListBySerialId(int serialId, int year) { return null; }
        public List<Car_MasterBrandEntity> GetHotMasterBrandEntityList(int entityCount) { return null; }
        public List<Car_SerialBaseEntity> GetHotSerailEntityList(int entityCount) { return null; }
    }
}
