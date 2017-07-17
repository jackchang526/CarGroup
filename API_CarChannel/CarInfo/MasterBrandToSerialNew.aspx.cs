using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /*
    * 0:包含在销，待销，停销，待查；旗下是否有车型不限
    * 1:包含在销，待销，停销，待查；非概念车；旗下是否有车型不限；
    * 2:包含在销，待销；非概念车；旗下是否有车型不限；
    * 3:包含在销，待销；非概念车；旗下必须有车型；车型可以是不限销焦状态；
    * 4:包含在销，待销；非概念车；旗下必须有车型；车型必须是在销、待销；
     * 5:包含：在销，待销，停销，待查，非概念车，旗下必须有车款；
     * 6:包含：包含在销；非概念车；旗下是否有车型不限；
     * 7:包含在销，待销，停销；非概念车；旗下是否有车型不限；
     * 8:包含在销，停销；非概念车；旗下车款 在销，停销；
    */
    public partial class MasterBrandToSerialNew : PageBase
    {
        private int _Conditions = 4;//用户查询数据的条件
        private int _Include = 0;//是否包含上一级的数据
        private int _ParentId = 0;//父级ID是多少
        private string _RequestType = string.Empty;//请求类型
        private string _DataName = string.Empty;//数据名称
        private string _Serias = string.Empty;//下拉列表的系列

        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(60);
            this.Response.ContentType = "application/x-javascript";
            //得到页面参数
            GetParams();
            if (string.IsNullOrEmpty(_RequestType) || string.IsNullOrEmpty(_DataName)) { Response.Write(string.Empty); return; }
            //得到页面内容
            string content = GetContent();
            //if (string.IsNullOrEmpty(content)) { Response.Write(string.Empty); return; }
            //Response.Write(content);
            Response.Write(string.Format("requestDatalist[\"{0}\"]={{{1}}}", _DataName, content));
        }
        /// <summary>
        /// 得到当前内容的上下文
        /// </summary>
        /// <param name="context"></param>
        private void GetParams()
        {
            _Conditions = string.IsNullOrEmpty(Request.QueryString["type"]) ? 4 : ConvertHelper.GetInteger(Request.QueryString["type"]);
            _Include = string.IsNullOrEmpty(Request.QueryString["include"]) ? 0 : ConvertHelper.GetInteger(Request.QueryString["include"]);
            _ParentId = string.IsNullOrEmpty(Request.QueryString["pid"]) ? 0 : ConvertHelper.GetInteger(Request.QueryString["pid"]);
            _RequestType = string.IsNullOrEmpty(Request.QueryString["rt"]) ? string.Empty : Request.QueryString["rt"];
            _DataName = string.IsNullOrEmpty(Request.QueryString["key"]) ? string.Empty : Request.QueryString["key"];
            _Serias = string.IsNullOrEmpty(Request.QueryString["serias"]) ? string.Empty : Request.QueryString["serias"];
        }
        /// <summary>
        /// 得到页面内容
        /// </summary>
        /// <returns></returns>
        private string GetContent()
        {
            _RequestType = _RequestType.ToLower();
            switch (_RequestType)
            {
                case "serial":
                    return GetSerialContent();
                case "master":
                    return GetMasterBrandContent();
                case "brand":
                    return GetBrandContent();
                case "cartype":
                    return GetCarTypeContent();
            }
            return "";
        }
        /// <summary>
        /// 得到子品牌的内容
        /// </summary>
        /// <returns></returns>
        private string GetSerialContent()
        {
            if (_ParentId < 1) return "";
            string cacheKey = string.Format("leveldropdownlistControl_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;


            DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(_Conditions);
            if (ds == null) return "";
            //根据条件不同，设置不同的查询字符串
            string condition = string.Format("bs_id={0}", _ParentId);
            if (_Include != 1)
            {
                condition = string.Format("cb_id={0}", _ParentId);
            }

            DataRow[] drList = ds.Tables[0].Select(condition);
            if (drList == null || drList.Length < 0) return "";

            List<string> contentlist = new List<string>();
            foreach (DataRow dr in drList)
            {
                int csId = Convert.ToInt32(dr["cs_id"]);
                string name = dr["cs_name"].ToString().Trim();
                if (csId == 1568) name = "索纳塔八";
                contentlist.Add(",");
                contentlist.Add(string.Format("\"s{0}\":", dr["cs_id"]));
                contentlist.Add("{");
                contentlist.Add(string.Format("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{4}\",\"urlSpell\":\"{2}\",\"showName\":\"{3}\",\"csSale\":\"{5}\" {6}"
                                     , csId
                                     , name
                                     , dr["csallspell"].ToString().ToLower()
                                     , dr["cs_ShowName"].ToString().Trim()
                                     , _Include == 1 ? dr["bs_id"] : dr["cb_id"]
                                     , GetSerialSaleState(dr["csSaleState"].ToString().Trim())
                                     , GetIncludeContent(dr, "cb_id", new string[] { "cb_name" })));
                contentlist.Add("}");
            }
            if (contentlist != null && contentlist.Count > 0)
            {
                // modified by chengl Apr.3.2014 exception when contentlist count = 0
                contentlist.RemoveAt(0);
            }
            //contentlist.Insert(0, "{");
            //contentlist.Insert(0, string.Format("requestDatalist[\"{0}\"]=", _DataName));
            //contentlist.Add("}");
            string content = string.Concat(contentlist.ToArray());

            CacheManager.InsertCache(cacheKey, content, 60);
            return content;
        }
        /// <summary>
        /// 得到主品牌的内容
        /// </summary>
        /// <returns></returns>
        private string GetMasterBrandContent()
        {
            string cacheKey = string.Format("leveldropdownlistControl_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;


            DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(_Conditions);
            if (ds == null) return "";

            DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "bs_id", "bs_Name", "bsallspell", "bsspell");
            if (dt == null || dt.Rows.Count < 1) return "";

            List<string> contentlist = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                contentlist.Add(string.Format("\"m{0}\":{{\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"0\",\"urlSpell\":\"{2}\",\"tSpell\":\"{3}\",\"nl\":[{4}]}}"
                                       , dr["bs_id"]
                                       , dr["bs_Name"].ToString().Trim()
                                       , dr["bsallspell"].ToString().ToLower()
                                       , dr["bsspell"].ToString().ToUpper()
                                       , ""));
            }
            //contentlist.RemoveAt(0);
            //contentlist.Insert(0, "{");
            //contentlist.Insert(0, string.Format("requestDatalist[\"{0}\"]=", _DataName));
            //contentlist.Add("}");
            string content = string.Join(",", contentlist.ToArray());

            CacheManager.InsertCache(cacheKey, content, 60);
            return content;
        }
        /// <summary>
        /// 得到品牌的内容
        /// </summary>
        /// <returns></returns>
        private string GetBrandContent()
        {
            string cacheKey = string.Format("leveldropdownlistControl_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;

            DataSet ds = new Car_SerialBll().GetIsConditionsDataSet(_Conditions);
            if (ds == null) return "";
            DataTable dt = ds.Tables[0].DefaultView.ToTable(true, "cb_id", "cb_name", "cballspell", "bs_id", "bs_Name", "bsallspell", "bsspell");
            if (dt == null || dt.Rows.Count < 1) return "";
            DataView dv = dt.DefaultView;
            if (_ParentId > 0)
            {
                dv.RowFilter = "bs_id=" + _ParentId;
            }

            List<string> contentlist = new List<string>();
            foreach (DataRow dr in dv.ToTable().Rows)
            {
                contentlist.Add(string.Format("\"b{0}\":{{\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{3}\",\"urlSpell\":\"{2}\",\"nl\":[] {4}}}"
                                        , dr["cb_id"]
                                        , dr["cb_Name"].ToString().Trim()
                                        , dr["cballspell"].ToString().ToLower()
                                        , dr["bs_id"]
                                        , GetIncludeContent(dr, "bs_id", new string[] { "bsspell", "bs_Name" })));
            }

            string content = string.Join(",", contentlist.ToArray());

            CacheManager.InsertCache(cacheKey, content, 60);
            return content;
        }
        /// <summary>
        /// 得到车型的内容
        /// </summary>
        /// <returns></returns>
        private string GetCarTypeContent()
        {
            string cacheKey = string.Format("leveldropdownlistControl_{0}_{1}_{2}_{3}", _Conditions, _Include, _ParentId, _RequestType);
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (string)obj;

            if (_ParentId < 1) return "";
            //StringBuilder jsonString = new StringBuilder();
            DataSet dsCar = new DataSet();
            if (_Conditions == 0 || _Conditions == 5 || _Conditions == 1 || _Conditions == 8)
            {
                dsCar = new CommonService().GetAllCarForAjaxCompareContainsStopSale(10);
            }
            else
            {
                dsCar = new CommonService().GetAllCarForAjaxCompare(10);
            }
            if (dsCar == null || dsCar.Tables.Count < 1 || dsCar.Tables[0].Rows.Count < 1) return "";
            StringBuilder cartypeString = new StringBuilder();
            try
            {
                List<int> csidlist = new List<int>();
                csidlist.Add(_ParentId);

                foreach (int id in csidlist)
                {
                    DataRow[] drs = dsCar.Tables[0].Select(" cs_id=" + id + " ");
                    if (drs == null || drs.Length < 1) continue;
                    StringBuilder tempString = new StringBuilder();
                    foreach (DataRow dr in drs)
                    {
                        string saleState = dr["Car_SaleState"].ToString().Trim();
                        if (_Conditions == 8 && !(saleState == "在销" || saleState == "停销")) continue;
                        tempString.AppendFormat(",\"t{0}\":", dr["car_id"]);
                        tempString.Append("{");
                        tempString.AppendFormat("\"id\":\"{0}\",\"name\":\"{1}\",\"pid\":\"{3}\",\"goid\":\"{2}\",\"goname\":\"{2}\",\"referprice\":\"{4}\",\"tt\":\"{5}\",\"salestate\":\"{6}\""
                                            , dr["car_id"]
                                            , dr["car_name"]
                                            , string.Equals((string)dr["Car_YearType"], "无") ? "未知年款" : dr["Car_YearType"]
                                            , id
                                            , dr["car_ReferPrice"]
                                            , dr["TT"]
                                            , saleState);
                        tempString.Append("}");
                    }
                    tempString = tempString.Length > 0 ? tempString.Remove(0, 1) : tempString;
                    cartypeString.Append(tempString.ToString());
                }
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
        /// <summary>
        /// 得到包含关系
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetIncludeContent(DataRow dr, string id, string[] name)
        {
            if (dr == null) return "";

            if (_Include != 1) return "";

            string elementName = string.Empty;
            foreach (string n in name)
            {
                elementName += " " + dr[n];
            }
            elementName = elementName.Trim();
            return string.Format(",\"goid\":\"{0}\",\"goname\":\"{1}\"", dr[id], elementName);
        }
        /// <summary>
        /// 得到子品牌销售状态
        /// </summary>
        /// <param name="state"></param>
        private string GetSerialSaleState(string state)
        {
            switch (state)
            {
                case "在销":
                    return "0";
                case "待销":
                    return "1";
                case "停销":
                    return "2";
                default: return "3";
            }
        }
    }
}