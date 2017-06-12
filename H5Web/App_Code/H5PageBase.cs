using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using System.Web.UI;
using System.Web.Caching;
using BitAuto.CarChannel.Common.Enum;

namespace H5Web
{
    public class H5PageBase : PageBase
    {
        #region handler page cache

        protected sealed class OutputCachedPage : Page
        {
            private OutputCacheParameters _cacheSettings;

            /// <summary>
            /// 默认构造 时间默认 缓存客户端和代理 参数全部
            /// </summary>
            public OutputCachedPage()
            {
                OutputCacheParameters cacheSettings = new OutputCacheParameters
                {
                    Duration = 60*WebConfig.CachedDuration,
                    Location = OutputCacheLocation.Any,
                    VaryByParam = "*"
                };
                // Tracing requires Page IDs to be unique.
                ID = Guid.NewGuid().ToString();
                _cacheSettings = cacheSettings;
            }

            public OutputCachedPage(OutputCacheParameters cacheSettings)
            {
                // Tracing requires Page IDs to be unique.
                ID = Guid.NewGuid().ToString();
                _cacheSettings = cacheSettings;
            }

            protected override void FrameworkInitialize()
            {
                // when you put the <%@ OutputCache %> directive on a page, the generated code calls InitOutputCache() from here
                base.FrameworkInitialize();
                InitOutputCache(_cacheSettings);
            }
        }

        #endregion

        /// <summary>
        ///     第四级 经销商、经纪人接口
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> GetH5ServiceDic()
        {
            var dic = new Dictionary<string, string>();
            var cacheKey = "H5PageBase_GetH5ServiceDic";
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<string, string>) obj;
            }
            else
            {
                try
                {
                    var filePath = HttpContext.Current.Server.MapPath("~") + "\\config\\H5config.config";
                    if (File.Exists(filePath))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        var entries = xmlDoc.SelectNodes("/root/service");
                        if (entries != null && entries.Count > 0)
                        {
                            foreach (XmlNode xn in entries)
                            {
                                var serverName = xn.Attributes["name"].Value.Trim();
                                var interfaceURL = xn.SelectSingleNode("interface").Attributes["url"].Value.Trim();
                                if (!dic.ContainsKey(serverName))
                                {
                                    dic.Add(serverName, interfaceURL);
                                }
                            }
                        }
                        CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
                    }
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog(ex.ToString());
                }
            }
            return dic;
        }

        /// <summary>
        ///     获取看过还看过广告
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, string>> GetSeeToSeeAd(int serialId)
        {
            var key = "H5PageBase_serialToSeeAD";
            var curDate = DateTime.Now;

            var cacheData =
                CacheManager.GetCachedData(key) as Dictionary<int, Dictionary<int, Dictionary<string, string>>>;
            if (cacheData == null)
            {
                var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "config\\ad\\SerialToSeeAD.xml");
                if (!File.Exists(filePath))
                    return null;
                cacheData = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();

                var cacheDependency = new CacheDependency(filePath);
                CacheManager.InsertCache(key, cacheData, cacheDependency,
                    DateTime.Now.AddMinutes(30));

                //CacheManager.InsertCache(key, cacheData, 30);

                Dictionary<int, Dictionary<string, string>> serialData = null;
                FileStream stream = null;
                XmlReader reader = null;
                try
                {
                    stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    reader = XmlReader.Create(stream);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "AD")
                        {
                            using (var subReader = reader.ReadSubtree())
                            {
                                reader.MoveToAttribute("ad_serialid");
                                var adserailid = reader.Value;
                                while (subReader.Read())
                                {
                                    if (subReader.NodeType == XmlNodeType.Element && subReader.Name == "Serial")
                                    {
                                        string title = string.Empty,
                                            url = string.Empty,
                                            imgUrl = string.Empty,
                                            priceRange = string.Empty;
                                        int postion = 0, toSerialId = 0;
                                        var needAd = true;
                                        while (subReader.Read())
                                        {
                                            if (subReader.NodeType == XmlNodeType.EndElement &&
                                                subReader.Name == "Serial")
                                                break;
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                if (reader.Name == "Position")
                                                {
                                                    postion = ConvertHelper.GetInteger(reader.ReadString().Trim());
                                                }
                                                else if (reader.Name == "SerialID")
                                                {
                                                    toSerialId = ConvertHelper.GetInteger(reader.ReadString().Trim());
                                                }
                                                else if (reader.Name == "Title")
                                                {
                                                    title = reader.ReadString().Trim();
                                                }
                                                else if (reader.Name == "Url")
                                                {
                                                    url = reader.ReadString().Trim();
                                                }
                                                else if (reader.Name == "ImgUrl")
                                                {
                                                    imgUrl = reader.ReadString().Trim();
                                                }
                                                else if (reader.Name == "StartDate")
                                                {
                                                    var startDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    if (startDate > curDate)
                                                    {
                                                        needAd = false;
                                                        break;
                                                    }
                                                }
                                                else if (reader.Name == "EndData")
                                                {
                                                    var endDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    if (endDate.AddDays(1) < curDate)
                                                    {
                                                        needAd = false;
                                                        break;
                                                    }
                                                }
                                                else if (reader.Name == "PriceRange")
                                                {
                                                    priceRange = reader.ReadString().Trim();
                                                }
                                            }
                                        }

                                        if (needAd)
                                        {
                                            if (cacheData.ContainsKey(toSerialId))
                                                serialData = cacheData[toSerialId];
                                            else
                                            {
                                                serialData = new Dictionary<int, Dictionary<string, string>>();
                                                cacheData.Add(toSerialId, serialData);
                                            }

                                            serialData[postion] = new Dictionary<string, string>();
                                            serialData[postion]["AD_SerialID"] = adserailid;
                                            serialData[postion]["Title"] = title;
                                            serialData[postion]["Url"] = url;
                                            serialData[postion]["ImgUrl"] = imgUrl;
                                            serialData[postion]["PriceRange"] = priceRange;
                                            //{{ , },{ , },{, },{, }};
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (stream != null)
                        stream.Dispose();
                }
            }
            return cacheData.ContainsKey(serialId) ? cacheData[serialId] : null;
        }

        /// <summary>
        ///     获取看过还看过广告（按级别投放）
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, Dictionary<int, Dictionary<string, string>>> GetSeeAgainByLevel()
        {
            var key = "H5PageBase_serialToSeeAD";
            var curDate = DateTime.Now;

            var cacheData =
                CacheManager.GetCachedData(key) as Dictionary<string, Dictionary<int, Dictionary<string, string>>>;
            if (cacheData != null)
            {
                return cacheData;
            }
            else
            {
                var filePath = Path.Combine(HttpContext.Current.Server.MapPath("~"),
                    "config\\ad\\LevelAdForSeeAgain.xml");
                if (!File.Exists(filePath))
                    return null;

                var adDictionary = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();
                FileStream stream = null;
                XmlReader reader = null;
                try
                {
                    stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    reader = XmlReader.Create(stream);

                    #region

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "ad")
                        {
                            using (var subReader = reader.ReadSubtree())
                            {
                                reader.MoveToAttribute("level");
                                var levelStr = reader.Value; //eg. 中大型车_中型车_跑车_豪华车
                                Dictionary<int, Dictionary<string, string>> tempdic =
                                    new Dictionary<int, Dictionary<string, string>>();

                                #region

                                while (subReader.Read())
                                {
                                    if (subReader.NodeType == XmlNodeType.Element && subReader.Name == "serial")
                                    {
                                        string title = string.Empty;
                                        string url = string.Empty;
                                        string imgUrl = string.Empty;
                                        int postion = 0;
                                        string serialId = string.Empty;
                                        DateTime startDate = DateTime.Now;
                                        DateTime endDate = DateTime.Now;
                                        Dictionary<string, string> tempDictionary = new Dictionary<string, string>();
                                        while (subReader.Read())
                                        {
                                            if (subReader.NodeType == XmlNodeType.EndElement &&
                                                subReader.Name == "serial")
                                                break;

                                            if (reader.NodeType != XmlNodeType.Element) continue;

                                            #region

                                            switch (reader.Name)
                                            {
                                                case "Position":
                                                    postion = ConvertHelper.GetInteger(reader.ReadString().Trim());
                                                    tempDictionary.Add(reader.Name, postion.ToString());
                                                    break;
                                                case "Title":
                                                    title = reader.ReadString().Trim();
                                                    tempDictionary.Add(reader.Name, title);
                                                    break;
                                                case "SerialID":
                                                    serialId = reader.ReadString().Trim();
                                                    tempDictionary.Add(reader.Name, serialId);
                                                    break;
                                                case "Url":
                                                    url = reader.ReadString().Trim();
                                                    tempDictionary.Add(reader.Name, url);
                                                    break;
                                                case "ImgUrl":
                                                    imgUrl = reader.ReadString().Trim();
                                                    tempDictionary.Add(reader.Name, imgUrl);
                                                    break;
                                                case "StartDate":
                                                    startDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    break;
                                                case "EndData":
                                                    endDate = ConvertHelper.GetDateTime(reader.ReadString().Trim());
                                                    break;
                                            }

                                            #endregion
                                        }
                                        if (startDate <= curDate && curDate <= endDate)
                                        {
                                            tempdic.Add(postion, tempDictionary);
                                        }
                                    }
                                }

                                #endregion

                                if (tempdic.Count > 0)
                                {
                                    adDictionary.Add(levelStr, tempdic);
                                }
                            }
                        }
                    }

                    #endregion
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (stream != null)
                        stream.Dispose();
                }

                var cacheDependency = new CacheDependency(filePath);
                CacheManager.InsertCache(key, adDictionary, cacheDependency,
                    DateTime.Now.AddMinutes(30));
                return adDictionary;
            }
        }

        /// <summary>
        ///     接口地址初始化，支持多业务线多个接口，具体配置参考H5configV2.config
        ///     2015-09-02 songcl
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, Dictionary<string, string>> GetH5ServiceDicV3()
        {
            var dic = new Dictionary<string, Dictionary<string, string>>();
            const string cacheKey = "H5PageBase_GetH5ServiceDicV3";
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<string, Dictionary<string, string>>) obj;
            }
            else
            {
                try
                {
                    var filePath = HttpContext.Current.Server.MapPath("~") + "\\config\\H5configV3.config";
                    if (File.Exists(filePath))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(filePath);
                        var entries = xmlDoc.SelectNodes("/root/service");
                        if (entries != null && entries.Count > 0)
                        {
                            foreach (XmlNode xn in entries)
                            {
                                var itemList = xn.SelectNodes("interface");

                                var serverName = xn.Attributes["name"].Value.Trim();

                                #region

                                if (dic.ContainsKey(serverName))
                                {
                                    foreach (XmlNode item in itemList)
                                    {
                                        var methodName = item.Attributes["name"].Value.Trim();

                                        if (!dic[serverName].ContainsKey(methodName))
                                        {
                                            var interfaceURL = item.Attributes["url"].Value.Trim();
                                            dic[serverName].Add(methodName, interfaceURL);
                                        }
                                    }
                                }
                                else
                                {
                                    var interfaceDictionary = new Dictionary<string, string>();
                                    foreach (XmlNode item in itemList)
                                    {
                                        var methodName = item.Attributes["name"].Value.Trim();
                                        var interfaceURL = item.Attributes["url"].Value.Trim();
                                        if (!interfaceDictionary.ContainsKey(methodName))
                                        {
                                            interfaceDictionary.Add(methodName, interfaceURL);
                                        }
                                    }
                                    dic.Add(serverName, interfaceDictionary);
                                }

                                #endregion
                            }
                            CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog(ex.ToString());
                }
            }
            return dic;
        }

        /// <summary>
        /// 获取h5车款对比参数
        /// </summary>
        /// <param name="carId1"></param>
        /// <param name="carId2"></param>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, string>> GetH5CarCompareParams(params int[] carIds)
        {
            string cacheKey = "H5PageBase_GetH5CarCompareParsms_" + string.Join("_", carIds);
            Dictionary<int, Dictionary<string, string>> dic = new Dictionary<int, Dictionary<string, string>>();
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                dic = (Dictionary<int, Dictionary<string, string>>) obj;
            }
            else
            {
                Dictionary<int, Dictionary<string, string>> allParamDic = GetAllH5CarCompareParams();
                if (allParamDic == null) return dic;
                foreach (int carId in carIds)
                {
                    if (!allParamDic.ContainsKey(carId)) continue;
                    if (!dic.ContainsKey(carId))
                    {
                        dic.Add(carId, allParamDic[carId]);
                    }
                }
                CacheManager.InsertCache(cacheKey, dic, WebConfig.CachedDuration);
            }
            return dic;
        }

        /// <summary>
        /// 获取h5车款对比参数
        /// </summary>
        /// <returns></returns>
        protected Dictionary<int, Dictionary<string, string>> GetAllH5CarCompareParams()
        {
            Dictionary<int, Dictionary<string, string>> carDic = null;
            string cacheKey = "H5PageBase_GetH5CarCompareParsms";
            var obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                carDic = (Dictionary<int, Dictionary<string, string>>) obj;
            }
            else
            {
                string fileName = Path.Combine(WebConfig.DataBlockPath, "Data/Compare/CarComparePrice.xml");
                if (!File.Exists(fileName))
                {
                    CommonFunction.WriteLog("H5车型对比参数文件不存在，路径：" + fileName);
                    return null;
                }
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(fileName);
                    carDic = new Dictionary<int, Dictionary<string, string>>();
                    XmlNodeList carNodeList = xmlDoc.SelectNodes("root/car");
                    foreach (XmlNode node in carNodeList)
                    {
                        Dictionary<string, string> paramDic = new Dictionary<string, string>();
                        //paramDic.Add("ReferPrice", node.Attributes["referPrice"].Value);
                        paramDic.Add("GouZhiShui", node.Attributes["gouZhiShui"].Value);
                        paramDic.Add("CheChuanShui", node.Attributes["cheChuanShui"].Value);
                        paramDic.Add("BaoXian", node.Attributes["baoXian"].Value);
                        paramDic.Add("ChePai", node.Attributes["chePai"].Value);
                        paramDic.Add("Koubei", node.Attributes["koubei"].Value);
                        paramDic.Add("Price3", node.Attributes["price3"].Value);
                        carDic.Add(ConvertHelper.GetInteger(node.Attributes["id"].Value), paramDic);
                    }
                    CacheManager.InsertCache(cacheKey, carDic, 60*24);
                }
                catch (Exception ex)
                {
                    CommonFunction.WriteLog(ex.ToString());
                }
            }
            return carDic;
        }
    }
}