using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using BitAuto.Utils;
using System.Text;
using BitAuto.CarChannel.BLL;
using System.Data;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// MasterBrandToSerialForSug 的摘要说明
    /// </summary>
    public class MasterBrandToSerialForSug : IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;

        private int pid = 0;
        private string requestType = string.Empty;
        private int type = 0;
        private string callback = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
            {
                Duration = 60 * 60 * 24,
                Location = OutputCacheLocation.Any,
                VaryByParam = "*"
            });
            page.ProcessRequest(HttpContext.Current);

            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            request = context.Request;

            GetParamsters();

            RenderContent();
        }

        private void GetParamsters()
        {
            pid = ConvertHelper.GetInteger(request.QueryString["pid"]);
            requestType = request.QueryString["rt"] ?? string.Empty;
            type = ConvertHelper.GetInteger(request.QueryString["type"]);
            callback = request.QueryString["callback"];
        }

        private void RenderContent()
        {
            string content = string.Empty;
            switch (requestType.ToLower())
            {
                case "master": content = RenderMasterContent(); break;
                case "serial": content = RenderSerialContent(); break;
                case "cartype": content = RenderCartypeContent(); break;
                default: break;
            }

            if (string.IsNullOrEmpty(callback))
                response.Write(string.Format("{0}", content));
            else
                response.Write(string.Format("{1}({0})", content, callback));
        }

        private string RenderMasterContent()
        {
            string cacheKey = string.Format("dropdownlistforsug_{0}_{1}_{2}", requestType, type, pid);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;

            DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(type);
            if (ds == null) return "";

            DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "bsallspell", "bsspell");
            if (dt == null || dt.Rows.Count < 1) return "";
            List<string> contentList = new List<string>();
            List<string> charList = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                var spell = "\"" + dr["bsspell"].ToString().ToUpper() + "\"";

                if (!charList.Contains(spell)) charList.Add(spell);
                contentList.Add(string.Format("{{\"id\":\"{0}\",\"name\":\"{1}\",\"urlSpell\":\"{2}\",\"tSpell\":\"{3}\"}}",
                    dr["bs_id"]
                                       , dr["bs_Name"].ToString().Trim()
                                       , dr["bsallspell"].ToString().ToLower()
                                       , dr["bsspell"]));
            }
            string content = string.Format("{{CharList:[{0}],DataList:[{1}]}}",
                string.Join(",", charList.ToArray()),
                string.Join(",", contentList.ToArray()));
            CacheManager.InsertCache(cacheKey, content, 60 * 24);

            return content;
        }

        private string RenderSerialContent()
        {
            if (pid < 1) return "";
            string cacheKey = string.Format("dropdownlistforsug_{0}_{1}_{2}", requestType, type, pid);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;


            DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(type);
            if (ds == null) return "";
            //根据条件不同，设置不同的查询字符串
            string condition = string.Format("bs_id={0}", pid);
            DataRow[] drList = ds.Tables[0].Select(condition);
            if (drList == null || drList.Length < 0) return "";
            StringBuilder sb = new StringBuilder();
            List<string> contentList = new List<string>();
            var query = drList.AsEnumerable().GroupBy(row => new { BrandId = ConvertHelper.GetInteger(row["cb_id"]), BrandName = ConvertHelper.GetString(row["cb_name"]), AllSpell = ConvertHelper.GetString(row["cballspell"]) }, p => p);
            foreach (var group in query)
            {
                var g = CommonFunction.Cast(group.Key, new { BrandId = 0, BrandName = "", AllSpell = "" });
                sb.AppendFormat("{{ gid:\"{0}\",gname:\"{1}\",gspell:\"{2}\",child:[", g.BrandId, g.BrandName, g.AllSpell);
                var serialList = group.ToList();
                foreach (var dr in serialList)
                {
                    int csId = Convert.ToInt32(dr["cs_id"]);
                    string name = dr["cs_name"].ToString().Trim();
                    string salestate = dr["csSaleState"].ToString().Trim();
                    if (csId == 1568) name = "索纳塔八";
                    sb.AppendFormat("{{\"id\":\"{0}\",\"name\":\"{1}\",\"urlSpell\":\"{2}\",\"showName\":\"{3}\",\"saleState\":\"{4}\"}},",
                        dr["cs_id"],
                        name,
                        dr["csallspell"].ToString().ToLower(),
                        dr["cs_ShowName"].ToString().Trim(),
                        salestate);
                }
                if (serialList.Count > 0) sb.Remove(sb.Length - 1, 1);
                sb.Append("]},");
            }
            if (query.Count() > 0) sb.Remove(sb.Length - 1, 1);
            string content = string.Format("[{0}]", sb.ToString());

            CacheManager.InsertCache(cacheKey, content, 60 * 24);
            return content;
        }

        private string RenderCartypeContent()
        {
            if (pid < 1) return "";

            string cacheKey = string.Format("dropdownlistforsug_{0}_{1}_{2}", requestType, type, pid);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;


            //StringBuilder jsonString = new StringBuilder();
            DataSet dsCar = new DataSet();
            if (type == 0 || type == 1 || type == 5 || type == 7)
            {
                dsCar = new CommonService().GetAllCarForAjaxCompareContainsStopSale(10);
            }
            else
            {
                dsCar = new CommonService().GetAllCarForAjaxCompare(10);
            }
            if (dsCar == null || dsCar.Tables.Count < 1 || dsCar.Tables[0].Rows.Count < 1) { return ""; }
            Dictionary<string, List<DataRow>> noSaleDic = new Dictionary<string, List<DataRow>>();
            StringBuilder cartypeString = new StringBuilder();
            try
            {
                DataRow[] drs = dsCar.Tables[0].Select(" cs_id=" + pid + " ");
                IOrderedEnumerable<DataRow> drsEnu = drs.OrderBy(x => x["Car_YearType"]);
                if (drs == null || drs.Length < 1) return "";
                StringBuilder tempString = new StringBuilder("[");
                //string yearType = string.Empty;// drs[0]["Car_YearType"].ToString();
                var query = drs.AsEnumerable().GroupBy(row => row["Car_YearType"].ToString());//.OrderByDescending(row => row[""].ToString());
                query = query.OrderByDescending(x => x.Key);
                foreach (var group in query)
                {
                    var carTypeList = group.ToList();
                    bool isNoSaleYear = false;
                    foreach (DataRow dr in carTypeList)
                    {
                        if (dr["Car_SaleState"].ToString().Trim() != "停销")
                        {
                            isNoSaleYear = true;
                            break;
                        }
                    }
                    if (isNoSaleYear)
                    {
                        tempString.AppendFormat("{{\"yeartype\":\"{0}\",\"nosale\":\"false\",\"child\":[", group.Key);
                    }

                    foreach (DataRow dr in carTypeList)
                    {
                        if (dr["Car_SaleState"].ToString().Trim() == "停销")
                        {
                            if (!noSaleDic.ContainsKey(group.Key))
                            {
                                List<DataRow> list = new List<DataRow>();
                                list.Add(dr);
                                noSaleDic.Add(group.Key, list);
                            }
                            else
                            {
                                noSaleDic[group.Key].Add(dr);
                            }
                            continue;
                        }
                        tempString.AppendFormat("{{\"id\":\"{0}\",\"name\":\"{1}\",\"yeartype\":\"{2}\",\"tt\":\"{3}\",\"salestate\":\"{4}\"}},"
                                            , dr["car_id"]
                                            , dr["car_name"]
                                            , string.Equals((string)dr["Car_YearType"], "无") ? "未知年款" : dr["Car_YearType"]
                                            , dr["TT"]
                                            , dr["Car_SaleState"].ToString().Trim());
                    }
                    if (isNoSaleYear)
                    {
                        tempString.Remove(tempString.Length - 1, 1).Append("]},");
                    }
                }
                foreach (string year in noSaleDic.Keys)
                {
                    tempString.AppendFormat("{{\"yeartype\":\"{0}\",\"nosale\":\"true\",\"child\":[", year);
                    var carTypeList = noSaleDic[year].ToList();
                    foreach (DataRow dr in carTypeList)
                    {
                        tempString.AppendFormat("{{\"id\":\"{0}\",\"name\":\"{1}\",\"yeartype\":\"{2}\",\"tt\":\"{3}\",\"salestate\":\"{4}\"}},"
                                             , dr["car_id"]
                                             , dr["car_name"]
                                             , string.Equals((string)dr["Car_YearType"], "无") ? "未知年款" : dr["Car_YearType"]
                                             , dr["TT"]
                                             , dr["Car_SaleState"].ToString().Trim());
                    }
                    tempString.Remove(tempString.Length - 1, 1).Append("]},");
                }
                cartypeString.Append(tempString.Remove(tempString.Length - 1, 1)).Append("]");
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
                return "";
            }
            if (string.IsNullOrEmpty(cartypeString.ToString())) return "";
            //jsonString.Append("requestDatalist[\"" + _DataName + "\"] = {" + cartypeString.ToString() + "};");
            CacheManager.InsertCache(cacheKey, cartypeString.ToString(), 30);
            return cartypeString.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private sealed class OutputCachedPage : Page
        {
            private OutputCacheParameters _cacheSettings;

            public OutputCachedPage(OutputCacheParameters cacheSettings)
            {
                ID = Guid.NewGuid().ToString();
                _cacheSettings = cacheSettings;
            }

            protected override void FrameworkInitialize()
            {
                base.FrameworkInitialize();
                InitOutputCache(_cacheSettings);
            }
        }
    }
}