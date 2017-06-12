using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Interface;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
    /// <summary>
    /// 获取分类子品牌数据
    /// </summary>
    public class GetCategorySerialJson : IHttpHandler
    {
        private HttpRequest request = null;
        private HttpResponse response = null;
        /// <summary>
        /// 0 "微型车" wx
        /// 1 "小型车" xx
        /// 2 "紧凑型车" jcx
        /// 3 "中型车" zx
        /// 4 "中大型车" zdx
        /// 5 "豪华车" hh
        /// 6 "SUV" suv
        /// 7 "MPV" mpv
        /// 8 "跑车" pc
        /// 9 "全部" all
        /// </summary>
        private List<EnumCollection.SerialSortForInterface>[] _LevelArrSSfi = null;
        /// <summary>
        /// new
        /// </summary>
        private EnumCollection.SerialSortForInterface[] _NewSerialArrSSfi = null;

        public void ProcessRequest(HttpContext context)
        {
            CacheManager.SetPageCache(60 * 24);
            request = context.Request;
            response = context.Response;

            response.ContentType = "application/x-javascript";

            InitLevelData();
            InitNewSerialData();

            WriteData();
        }

        private void WriteData()
        {
            response.Write("var csCateDataJson={");

            bool isLevel = false;
            #region 级别
            if (_LevelArrSSfi != null)
            {
                for (int i = 0; i < _LevelArrSSfi.Length; i++)
                {
                    string key = null;
                    switch (i)
                    {
                        case 0: key = "wx"; break;
                        case 1: key = "xx"; break;
                        case 2: key = "jcx"; break;
                        case 3: key = "zx"; break;
                        case 4: key = "zdx"; break;
                        case 5: key = "hh"; break;
                        case 6: key = "suv"; break;
                        case 7: key = "mpv"; break;
                        case 8: key = "pc"; break;
                        case 9: key = "all"; break;
                    }
                    if (string.IsNullOrEmpty(key))
                        continue;
                    if (!isLevel)
                        isLevel = true;
                    response.Write(string.Format("\"{0}\":[", key));
                    List<EnumCollection.SerialSortForInterface> topCss = _LevelArrSSfi[i];
                    for (int j = 0; j < topCss.Count; j++)
                    {
                        EnumCollection.SerialSortForInterface csData = topCss[j];
                        response.Write("{");
                        response.Write(string.Format("\"name\":\"{0}\",\"url\":\"http://car.bitauto.com/{1}/\"", CommonFunction.GetUnicodeByString(csData.CsShowName), csData.CsAllSpell));
                        response.Write("}");
                        if (j < topCss.Count - 1)
                            response.Write(",");
                    }

                    response.Write("]");
                    if (i < _LevelArrSSfi.Length - 1)
                        response.Write(",");
                }
            }
            #endregion

            #region 新车
            if (_NewSerialArrSSfi != null)
            {
                if (isLevel)
                    response.Write(",");
                response.Write("\"newcs\":[");
                for (int i = 0; i < _NewSerialArrSSfi.Length; i++)
                {
                    EnumCollection.SerialSortForInterface csData = _NewSerialArrSSfi[i];
                    response.Write("{");
                    response.Write(string.Format("\"name\":\"{0}\",price:\"{1}\",\"url\":\"http://car.bitauto.com/{2}/\"", CommonFunction.GetUnicodeByString(csData.CsShowName), CommonFunction.GetUnicodeByString(csData.CsPriceRange), csData.CsAllSpell));
                    response.Write("}");
                    if( i < _NewSerialArrSSfi.Length-1 )
                        response.Write(",");
                }
                response.Write("]");
            }
            #endregion

            response.Write("};");
        }
        /// <summary>
        /// 初始化新车推荐
        /// </summary>
        private void InitNewSerialData()
        {
            string newserialcacheKeys = "api_GetCategorySerialJson_carnewserialcache";
            
            _NewSerialArrSSfi = (EnumCollection.SerialSortForInterface[])CacheManager.GetCachedData(newserialcacheKeys);
            if (_NewSerialArrSSfi == null || _NewSerialArrSSfi.Length < 1)
            {
                _NewSerialArrSSfi = new EnumCollection.SerialSortForInterface[10];
                Dictionary<int, string> dict = new Car_SerialBll().GetAllSerialMarkDay();
                List<int> csList = new List<int>(10);
                foreach (KeyValuePair<int, string> key in dict)
                {
                    if (CommonFunction.DateDiff("d", ConvertHelper.GetDateTime(key.Value), DateTime.Now) >= 0)
                    {
                        csList.Add(key.Key);
                    }
                    if (csList.Count > 20)
                        break;
                }
                int index = 0;
                XmlElement serialEle = null;
                XmlDocument autoXml = AutoStorageService.GetAutoXml();
                string searchQuery = "Params/MasterBrand/Brand/Serial[@ID='{0}']";
                foreach (int csId in csList)
                {
                    serialEle = autoXml.SelectSingleNode(string.Format(searchQuery, csId.ToString())) as XmlElement;
                    if (serialEle == null)
                        continue;

                    _NewSerialArrSSfi[index] = new EnumCollection.SerialSortForInterface()
                    {
                        CsID = csId,
                        CsAllSpell = serialEle.GetAttribute("AllSpell"),
                        CsLevel = serialEle.GetAttribute("CsLevel"),
                        CsShowName = serialEle.GetAttribute("ShowName"),
                        CsName = serialEle.GetAttribute("Name"),
                        CsPV = ConvertHelper.GetInteger(serialEle.GetAttribute("CsPV"))
                    };

                    double minP = ConvertHelper.GetDouble(serialEle.GetAttribute("MinP"));
                    double maxP = ConvertHelper.GetDouble(serialEle.GetAttribute("MaxP"));
                    if (minP <= 0 || maxP <= 0)
                        _NewSerialArrSSfi[index].CsPriceRange = "暂无报价";
                    else
                        _NewSerialArrSSfi[index].CsPriceRange = string.Format("{0}万-{1}万"
                            , GetShowPrice(minP)
                            , GetShowPrice(maxP));

                    if (index >= 9)
                        break;
                    index++;
                }
                CacheManager.InsertCache(newserialcacheKeys, _NewSerialArrSSfi, 60);
            }
        }
        private string GetShowPrice(double price)
        {
            return price >= 100 ? Math.Round(price, 0).ToString() : price.ToString("#.#0");
        }
        /// <summary>
        /// 初始化级别
        /// </summary>
        private void InitLevelData()
        {
            string levelcacheKeys = "api_GetCategorySerialJson_carlevelpvorderdefault";

            _LevelArrSSfi = (List<EnumCollection.SerialSortForInterface>[])CacheManager.GetCachedData(levelcacheKeys);

            if (_LevelArrSSfi == null
                || _LevelArrSSfi.Length < 1)
            {
                List<EnumCollection.SerialSortForInterface> lssfi = new BitAuto.CarChannel.Common.PageBase().GetAllSerialNewly30DayToList();
                _LevelArrSSfi = new List<EnumCollection.SerialSortForInterface>[10];
                for (int i = 0; i < 10; i++)
                {
                    _LevelArrSSfi[i] = new List<EnumCollection.SerialSortForInterface>(10);
                }

                #region Delete


                foreach (EnumCollection.SerialSortForInterface ssfi in lssfi)
                {
                    if (_LevelArrSSfi[9].Count < 10)//全部
                    { _LevelArrSSfi[9].Add(ssfi); }

                    if (_LevelArrSSfi[0].Count < 10 && ssfi.CsLevel == "微型车")
                    { _LevelArrSSfi[0].Add(ssfi); continue; }
                    if (_LevelArrSSfi[1].Count < 10 && ssfi.CsLevel == "小型车")
                    { _LevelArrSSfi[1].Add(ssfi); continue; }
                    if (_LevelArrSSfi[2].Count < 10 && ssfi.CsLevel == "紧凑型车")
                    { _LevelArrSSfi[2].Add(ssfi); continue; }
                    if (_LevelArrSSfi[3].Count < 10 && ssfi.CsLevel == "中型车")
                    { _LevelArrSSfi[3].Add(ssfi); continue; }
                    if (_LevelArrSSfi[4].Count < 10 && ssfi.CsLevel == "中大型车")
                    { _LevelArrSSfi[4].Add(ssfi); continue; }
                    if (_LevelArrSSfi[5].Count < 10 && ssfi.CsLevel == "豪华车")
                    { _LevelArrSSfi[5].Add(ssfi); continue; }
                    if (_LevelArrSSfi[6].Count < 10 && ssfi.CsLevel == "SUV")
                    { _LevelArrSSfi[6].Add(ssfi); continue; }
                    if (_LevelArrSSfi[7].Count < 10 && ssfi.CsLevel == "MPV")
                    { _LevelArrSSfi[7].Add(ssfi); continue; }
                    if (_LevelArrSSfi[8].Count < 10 && ssfi.CsLevel == "跑车")
                    { _LevelArrSSfi[8].Add(ssfi); continue; }
                }

                #endregion

                CacheManager.InsertCache(levelcacheKeys, _LevelArrSSfi, 60);
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