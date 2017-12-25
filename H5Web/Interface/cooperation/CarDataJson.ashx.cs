using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.Utils;
using Newtonsoft.Json;

namespace H5Web.Interface.cooperation
{
    /// <summary>
    ///     CarDataJson 的摘要说明
    /// </summary>
    public class CarDataJson : H5PageBase, IHttpHandler
    {
        /// datatype:
        /// 0:在销 待销
        /// 1:在销 待销 停销
        /// 2:在销
        private readonly int _dataType = 0;

        private string _callback = string.Empty;

        private int _level;

        private int _top = 20;

        private HttpRequest request;
        private HttpResponse response;

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);
            context.Response.ContentType = "application/json; charset=utf-8";
            response = context.Response;
            request = context.Request;

            _callback = ConvertHelper.GetString(request.QueryString["callback"]);
            //_dataType = ConvertHelper.GetInteger(request.QueryString["datatype"]);
            var action = ConvertHelper.GetString(request.QueryString["action"]);

            switch (action)
            {
                case "master":
                    MasterBrand();
                    break;
                case "serial":
                    SerialInfo();
                    break;
                case "hot":
                    HotSerial();
                    break;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        ///     主品牌
        /// </summary>
        private void MasterBrand()
        {
            var cacheKey = "Interface_CarDataJson_MasterBrand";

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                WriteDate(obj.ToString());
            }
            else
            {
                var list = new List<object>();
                var messsage = string.Empty;
                try
                {
                    //获取数据xml
                    var mbDoc = AutoStorageService.GetAutoXml();

                    //遍历所有主品牌节点
                    var mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");

                    if (mbNodeList != null)
                    {
                        for (var i = 0; i < mbNodeList.Count; i++)
                        {
                            var mbNode = (XmlElement) mbNodeList[i];

                            //全拼
                            //var masterSpell = mbNode.GetAttribute("AllSpell").ToLower();

                            //首字母
                            var firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();

                            //生成主品牌图标
                            var mbId = ConvertHelper.GetInteger(mbNode.GetAttribute("ID"));

                            //名称
                            var mbName = mbNode.GetAttribute("Name");

                            //不输出没有子品牌的主品牌信息
                            var serialNodeList = mbNode.SelectNodes("./Brand/Serial");
                            if (serialNodeList != null && serialNodeList.Count <= 0) continue;

                            list.Add(new
                            {
                                mid = mbId,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_" +
                                    mbId +
                                    "_100.png",
                                masterName = mbName,
                                firstchar = firstChar
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    messsage = ex.Message;
                }

                var res = JsonConvert.SerializeObject(new
                {
                    //code = 0,
                    msg = messsage,
                    result = new
                    {
                        hotList = new List<object>
                        {
                            new
                            {
                                mid = 8,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_8_100.png",
                                masterName = "大众",
                                firstchar = "D"
                            },
                            new
                            {
                                mid = 13,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_13_100.png",
                                masterName = "现代",
                                firstchar = "X"
                            },
                            new
                            {
                                mid = 17,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_17_100.png",
                                masterName = "福特",
                                firstchar = "F"
                            },
                            new
                            {
                                mid = 9,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_9_100.png",
                                masterName = "奥迪",
                                firstchar = "A"
                            },
                            new
                            {
                                mid = 2,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_2_100.png",
                                masterName = "奔驰",
                                firstchar = "B"
                            },
                            new
                            {
                                mid = 30,
                                imageUrl =
                                    "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_30_100.png",
                                masterName = "日产",
                                firstchar = "R"
                            }
                        },
                        allList = list
                    }
                });
                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
                WriteDate(res);
            }
        }

        /// <summary>
        ///     品牌、车系
        /// </summary>
        private void SerialInfo()
        {
            var masterId = ConvertHelper.GetInteger(request.QueryString["pid"]);

            var cacheKey = "Interface_CarDataJson_SerialInfo_" + masterId + "_" + _dataType;

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                WriteDate(obj.ToString());
            }
            else
            {
                var result = new List<object>();
                var messsage = string.Empty;
                try
                {
                    var carBrandBll = new Car_BrandBll();
                    var ds = carBrandBll.GetCarSerialSortListByBSID(masterId, _dataType);

                    var dict = new Dictionary<int, List<object>>();
                    var brandList = new Dictionary<int, string>();

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            var brandId = ConvertHelper.GetInteger(dr["cb_id"].ToString());
                            var brandName = dr["cb_name"].ToString();
                            var brandAllspell = dr["cbspell"].ToString();
                            var country = dr["Cp_Country"].ToString().Trim();
                            if (country != "中国")
                            {
                                brandName = "进口" + brandName;
                            }
                            var serialId = ConvertHelper.GetInteger(dr["cs_id"].ToString());
                            var serialName = dr["cs_ShowName"].ToString().Trim();
                            var serialAllspell = dr["csspell"].ToString();
                            var csSaleState = dr["CsSaleState"].ToString();
                            var csLevel = dr["cslevel"].ToString();
                            if (csLevel == "概念车")
                            {
                                continue;
                            }
                            //改成指导价
                            var priceRange = new PageBase().GetSerialReferPriceByID(serialId).Trim();
                            if (csSaleState.Trim() == "待销")
                                priceRange = "未上市";
                            if (string.IsNullOrEmpty(priceRange))
                                priceRange = "暂无指导价";

                            var imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");
                            if (csSaleState.Trim() == "停销" && imgUrl.IndexOf("150-100.gif") > 0)
                            {
                                continue;
                            }
                            if (!dict.ContainsKey(brandId))
                            {
                                var serailList = new List<object>
                                {
                                    new
                                    {
                                        serialId,
                                        serialName,
                                        //allspell = serialAllspell,
                                        price = priceRange,
                                        imageUrl = imgUrl,
                                        //csSaleState = csSaleState.Trim(),
                                        detailUrl =
                                            "http://car.h5.yiche.com/" + serialAllspell +
                                            "/?WT.mc_id=mjxydth5&ad=0&lg=0&tele=1&order=page7"
                                    }
                                };
                                dict.Add(brandId, serailList);

                                //brandList.Add(brandId, new { BrandId = brandId , BrandName = brandName , BrandAllspell=brandAllspell });
                                brandList.Add(brandId, brandName);
                            }
                            else
                            {
                                dict[brandId].Add(new
                                {
                                    serialId,
                                    serialName,
                                    //allspell = serialAllspell,
                                    price = priceRange,
                                    imageUrl = imgUrl,
                                    //csSaleState = csSaleState.Trim(),
                                    detailUrl =
                                        "http://car.h5.yiche.com/" + serialAllspell +
                                        "/?WT.mc_id=mjxydth5&ad=0&lg=0&tele=1&order=page7"
                                });
                            }
                        }
                    }
                    foreach (var key in dict.Keys)
                    {
                        result.Add(new
                        {
                            brandId = key,
                            brandName = brandList[key],
                            slist = dict[key]
                        });
                    }
                }
                catch (Exception ex)
                {
                    messsage = ex.Message;
                }


                var res = JsonConvert.SerializeObject(new
                {
                    //code = 0,
                    msg = messsage,
                    result = new
                    {
                        list = result
                    }
                });
                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
                WriteDate(res);
            }
        }

        /// <summary>
        ///     热销车型
        /// </summary>
        private void HotSerial()
        {
            _level = ConvertHelper.GetInteger(request.QueryString["l"]);
            var top = request.QueryString["top"];
            if (top != null)
            {
                int.TryParse(top, out _top);
            }

            var cacheKey = "Interface_CarDataJson_HotSerial_" + _level + "_" + _top;
            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                WriteDate(obj.ToString());
            }
            else
            {
                var targetList = new List<object>();
                var messsage = string.Empty;
                try
                {
                    var serialBll = new Car_SerialBll();
                    var list = serialBll.GetHotSerial(int.MaxValue); //获取热门车型

                    var dic = GetLevelDic();

                    foreach (var serialNode in list)
                    {
                        if (targetList.Count < _top)
                        {
                            #region

                            var serialId = ConvertHelper.GetInteger(serialNode.GetAttribute("ID"));
                            var showName = serialNode.GetAttribute("ShowName");
                            var serialLevel = serialNode.GetAttribute("CsLevel");
                            var serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
                            //改成指导价
                            var priceRange = GetSerialReferPriceByID(serialId);
                            var imgUrl = Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_1.");

                            if (_level > 0)
                            {
                                var tempList = dic[_level];

                                if (dic.ContainsKey(_level) && dic[_level].Contains(serialLevel))
                                {
                                    targetList.Add(new
                                    {
                                        serialId,
                                        imageUrl = imgUrl,
                                        serialName = showName,
                                        priceRange,
                                        level = serialLevel,
                                        detailUrl =
                                            "http://car.h5.yiche.com/" + serialSpell +
                                            "/?WT.mc_id=mjxydth5&ad=0&lg=0&tele=1&order=page7"
                                    });
                                }
                            }
                            else
                            {
                                targetList.Add(new
                                {
                                    serialId,
                                    imageUrl = imgUrl,
                                    serialName = showName,
                                    priceRange,
                                    level = serialLevel,
                                    detailUrl =
                                        "http://car.h5.yiche.com/" + serialSpell +
                                        "/?WT.mc_id=mjxydth5&ad=0&lg=0&tele=1&order=page7"
                                });
                            }

                            #endregion
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messsage = ex.Message;
                }

                var res = JsonConvert.SerializeObject(new
                {
                    //code = 0,
                    msg = messsage,
                    result = new
                    {
                        lastpage = 0,
                        clist = targetList
                    }
                });
                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
                WriteDate(res);
            }
        }

        private Dictionary<int, List<string>> GetLevelDic()
        {
            var dic = new Dictionary<int, List<string>>
            {
                {1, new List<string> {"微型车"}},
                {2, new List<string> {"小型车"}},
                {3, new List<string> {"紧凑型车"}},
                {5, new List<string> {"中型车"}},
                {4, new List<string> {"中大型车"}},
                {6, new List<string> {"豪华车"}},
                {9, new List<string> {"跑车"}},
                {11, new List<string> {"面包车"}},
                {7, new List<string> {"MPV"}},
                {8, new List<string> {"SUV"}},
                //{13, "小型SUV"},
                //{14, "紧凑型SUV"},
                //{15, "中型SUV"},
                //{16, "中大型SUV"},
                //{17, "全尺寸SUV"},

                {63, new List<string> {"微型", "小型", "紧凑型", "中型", "中大型", "豪华型"}}
            };
            return dic;
        }

        private void WriteDate(string content)
        {
            if (!string.IsNullOrEmpty(_callback))
                content = string.Format("{0}({1})", _callback, content);
            response.Write(content);
        }
    }
}