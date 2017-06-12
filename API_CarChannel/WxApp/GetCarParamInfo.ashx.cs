using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils; 

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// 访问格式：http://api.car.bitauto.com/WxApp/GetCarParamInfo.ashx?carid=121034
    /// GetCarParamInfo 的摘要说明
    /// </summary> 
    public class GetCarParamInfo : IHttpHandler 
    {
        private HttpRequest request;
        private HttpResponse response;
        private List<string> jsonResult=new List<string>(); 
        private Car_BasicBll basicBll = new Car_BasicBll(); 
        private int serialId;
        public void ProcessRequest(HttpContext context)       
        {
            context.Response.ContentType = "application/x-javascript";  
            request = context.Request;
            response = context.Response;
            RenderCarParamJson();
        }
        /// <summary>
        /// {"Info1":{"key1":"value1","key2":"value2"},"Info2":{"key21":"value21","key22":"value22"}}
        /// </summary>
        private void RenderCarParamJson()
        {
            try
            {
                string strResult = string.Empty;
                int carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
                string cacheKey = "Car_WxApp_GetCarParamInfo_" + carId;
                var obj = CacheManager.GetCachedData(cacheKey);
                if (obj != null)
                {
                    var result = (string)obj;
                    response.Write(result);
                    return;
                }
                if (carId > 0)
                {
                    List<int> listValidCarID = new List<int>();
                    listValidCarID.Add(carId);
                    Dictionary<int, Dictionary<string, string>> dic = basicBll.GetCarCompareDataByCarIDs(listValidCarID);
                    if (!dic.ContainsKey(carId) || dic[carId].Count == 0)
                    {
                        response.Write("Error:没有获取到车款基本信息!");
                        return;
                    }
                    else
                    {
                        serialId = ConvertHelper.GetInteger(dic[carId]["Cs_ID"]);
                        XmlDocument docPC = new XmlDocument();
                        string cacheConfig = "Car_WxApp_GetCarParamInfo_ParameterConfigurationNew";
                        object parameterConfiguration = null;
                        CacheManager.GetCachedData(cacheConfig, out parameterConfiguration);
                        if (parameterConfiguration != null)
                        {
                            docPC = (XmlDocument)parameterConfiguration;
                        }
                        else
                        {
                            var filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\ParameterConfigurationNew.config";
                            if (File.Exists(filePath))
                            {
                                docPC.Load(filePath);
                                CacheManager.InsertCache(cacheConfig, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
                            }
                            else
                            {
                                response.Write("Error:没有获取到车款基本信息!");
                                return;
                            }
                        }

                        #region         // 参数配置
                        if (docPC != null && docPC.HasChildNodes)
                        {
                            XmlNode rootPC = docPC.DocumentElement;
                            // 显示 参数
                            if (rootPC != null && docPC.ChildNodes.Count > 1)
                            {
                                XmlNode parameter = rootPC.ChildNodes[0];
                                foreach (XmlNode parameterList in parameter)
                                {
                                    if (parameterList.NodeType == XmlNodeType.Element)
                                    {
                                        if (parameterList.HasChildNodes && parameterList.Attributes != null && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2")
                                        {
                                            string groupName = parameterList.Attributes.GetNamedItem("Name").Value;
                                            XmlNodeList xmlNode = parameterList.ChildNodes;
                                            List<string> singleParamListJson = new List<string>();
                                            singleParamListJson.Clear();
                                            foreach (XmlNode item in xmlNode)
                                            {
                                                if (item.NodeType != XmlNodeType.Element)
                                                {
                                                    continue;
                                                }
                                                string pvalue = string.Empty;
                                                //合并参数
                                                if (item.Attributes != null && item.Attributes.GetNamedItem("Value").Value.IndexOf(",") != -1)
                                                {
                                                    string[] arrKey = item.Attributes.GetNamedItem("Value").Value.Split(',');
                                                    string[] arrUnit = item.Attributes.GetNamedItem("Unit").Value.Split(',');
                                                    string[] arrParam = item.Attributes.GetNamedItem("ParamID").Value.Split(',');
                                                    List<string> list = new List<string>();
                                                    for (var i = 0; i < arrKey.Length; i++)
                                                    {
                                                        if (!(dic[carId].ContainsKey(arrKey[i]) && dic[carId][arrKey[i]] != "待查"))
                                                            continue;
                                                        //档位数 0 不显示
                                                        if (arrParam[i] == "724")
                                                        {
                                                            var d = ConvertHelper.GetInteger(dic[carId][arrKey[i]]);
                                                            if (d <= 0) continue;
                                                        }
                                                        list.Add(string.Format("{0}{1}", dic[carId][arrKey[i]], arrUnit[i]));
                                                    }
                                                    if (list.Count <= 0) continue;
                                                    //解决2个参数 其中“有” 后面参数有值 替换成 实心圈
                                                    var you = list.Find(p => p.IndexOf("有") != -1);
                                                    if (you != null && list.Count > 1)
                                                        list.Remove(you);
                                                    //进气形式 2个参数 增压 显示 增压方式 不是则显示 进气形式
                                                    if (item.Attributes.GetNamedItem("Name").Value == "进气形式")
                                                    {
                                                        if (list.Count > 1)
                                                        {
                                                            if (list[0] == "增压")
                                                                list.RemoveAt(0);
                                                            else
                                                                list.RemoveAt(1);
                                                        }
                                                    }
                                                    pvalue = string.Join(" ", list.ToArray());
                                                }
                                                else
                                                {
                                                    if (!(dic[carId].ContainsKey(item.Attributes.GetNamedItem("Value").Value) && dic[carId][item.Attributes.GetNamedItem("Value").Value] != "待查"))
                                                    {
                                                        continue;
                                                    }
                                                    pvalue = string.Format("{0}{1}", dic[carId][item.Attributes.GetNamedItem("Value").Value], item.Attributes.GetNamedItem("Unit").Value);
                                                }
                                                // 牛B的逻辑不硬编码都不行
                                                // 燃料类型 汽油的话同时显示 燃油标号
                                                string pvalueOther;
                                                if (item.Attributes.GetNamedItem("ParamID").Value == "578" && pvalue == "汽油")
                                                {
                                                    if (dic[carId].ContainsKey("CarParams/Oil_FuelTab") && dic[carId]["CarParams/Oil_FuelTab"] != "待查")
                                                    {
                                                        pvalueOther = dic[carId]["CarParams/Oil_FuelTab"];
                                                        pvalue = pvalue + " " + pvalueOther;
                                                    }
                                                }
                                                // 进气型式 如果自然吸气直接显示，如果是增压则显示增压方式
                                                if (item.Attributes.GetNamedItem("ParamID").Value == "425" && pvalue == "增压")
                                                {
                                                    if (dic[carId].ContainsKey("CarParams/Engine_AddPressType") && dic[carId]["CarParams/Engine_AddPressType"] != "待查")
                                                    {
                                                        pvalueOther = dic[carId]["CarParams/Engine_AddPressType"];
                                                        pvalue = pvalue + " " + pvalueOther;
                                                    }
                                                }
                                                string oneItem = String.Empty;
                                                if (item.Attributes.GetNamedItem("Name").Value == "车身颜色")
                                                {
                                                    //车身颜色
                                                    string carColors = dic[carId]["Car_OutStat_BodyColorRGB"].Replace("，", ",");
                                                    oneItem = "{" + string.Format("\"{0}\":\"{1}\"",
                                                        item.Attributes.GetNamedItem("Name").Value, carColors) + "}";
                                                }
                                                else
                                                {
                                                    oneItem = "{" + string.Format("\"{0}\":\"{1}\"",
                                                        item.Attributes.GetNamedItem("Name").Value, pvalue) + "}";
                                                }
                                                singleParamListJson.Add(oneItem);
                                            }
                                            jsonResult.Add("{" + string.Format("\"{0}\":[", groupName) + string.Join(",", singleParamListJson) + "]" + "}");
                                        }
                                    }
                                }
                                if (jsonResult.Count > 0)
                                {
                                    strResult = "[" + string.Join(",", jsonResult) + "]";
                                    CacheManager.InsertCache(cacheKey, strResult, WebConfig.CachedDuration);
                                    response.Write(strResult);
                                    return;
                                }
                            }
                            else
                            {
                                response.Write("Error:没有获取到车款基本信息!");
                                return;
                            }
                        #endregion
                        }
                        else
                        {
                            response.Write("Error:没有获取到车款基本信息!");
                            return;
                        }
                    }
                }
                else
                {
                    response.Write("参数有误,请检查!");
                    return;
                }
            }
            catch(Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString() + ";StackTrace:" + ex.StackTrace);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}