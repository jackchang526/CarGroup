using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;

using BitAuto.Utils;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL
{
    public class CarIndex : TreeData
    {
        private readonly Car_SerialBll _csb = new Car_SerialBll();
        string masterElementString = "<master id=\"{0}\" name=\"{1}\" countnum=\"{2}\" firstchar=\"{3}\" extra=\"{4}\">";
        string brandElementString = "<brand id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\">";
        string serialElementString = "<serial id=\"{0}\" name=\"{1}\" countnum=\"{2}\" extra=\"{3}\"/>";
        string totalString = "<data id=\"0\" countnum=\"{0}\">";

        public string TreeXmlData()
        {
            string cacheKeys = "carindexlefttreestring";
            string leftTreeString = (string)CacheManager.GetCachedData(cacheKeys);
            if (string.IsNullOrEmpty(leftTreeString))
            {
                leftTreeString = GetNoCacheTreeXmlData();
                if (string.IsNullOrEmpty(leftTreeString)) return "";
                CacheManager.InsertCache(cacheKeys, leftTreeString, WebConfig.CachedDuration);
            }

            return leftTreeString;            
        }
        /// <summary>
        /// 得到没有缓存的树
        /// </summary>
        /// <returns></returns>
        private string GetNoCacheTreeXmlData()
        {
            DataSet treeDs = _csb.GetLefTreePvSerialInfo();
            if (treeDs == null || treeDs.Tables.Count < 1 || treeDs.Tables[0].Rows.Count < 1) return "";

            DataTable dt = treeDs.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "Spell", "bsallspell");
            if (dt == null || dt.Rows.Count < 1) return "";
            StringBuilder treeString = new StringBuilder();
            int totalCount = 0;
            foreach (DataRow masterBr in dt.Rows)
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
        public int GetMasterBrandId(int masterBrandId) { return 0; }
        public int GetBrandId(int brandId) { return 0; }
        public int GetSerialId(int serialId) { return 0; }
        public string GetForcusImageArea() { return ""; }
        public string GetForcusNewsAree() { return ""; }
        public DataSet GetNewsListBySerialId(int serialId, int year) { return null; }
        public List<Car_MasterBrandEntity> GetHotMasterBrandEntityList(int entityCount) { return null; }
        public List<Car_SerialBaseEntity> GetHotSerailEntityList(int entityCount) { return null; }
    }
}
