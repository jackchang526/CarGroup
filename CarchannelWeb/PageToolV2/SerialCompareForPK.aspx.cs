using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.PageToolV2
{
    public partial class SerialCompareForPK : PageBase
    {
        private SerialNewCompareEntity entity = null;

        protected string pageTitle = string.Empty;
        protected string pageKeywords = string.Empty;
        protected string pageDescription = string.Empty;
        protected string canonical = string.Empty;
        protected string navigate = "&gt; <strong>综合对比</strong>";

        public string MonthStr = "";
        protected int serialId1 = 0;
        protected int serialId2 = 0;
        protected SerialEntity serialEntity1;
        protected SerialEntity serialEntity2;

        string cacheKey = "Car_SerialCompareForPK_NewV2";

        protected string serialNewCompareHtml = string.Empty;

        protected string carIds = string.Empty;
        protected string csIds = string.Empty;

        protected string NewsHtml = string.Empty;
        protected string UserCompareHtml = string.Empty;
        protected string HotCompareHtml = string.Empty;


        public void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(20);

            serialId1 = ConvertHelper.GetInteger(Request.QueryString["serialid1"]);
            serialId2 = ConvertHelper.GetInteger(Request.QueryString["serialid2"]);

            carIds = ConvertHelper.GetString(Request.QueryString["carids"]);
            csIds = ConvertHelper.GetString(Request.QueryString["csids"]);

            if (serialId1 > 0 && serialId2 > 0)
            {
                canonical = serialId1 + "-" + serialId2 + "/";

                serialEntity1 = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId1);
                serialEntity2 = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId2);

                InitData();

                RenderMonth();

                UpdateSerialCompare();

                RenderNews();

                RenderUserCompare();
            }
            RenderHotCompare();

            RenderSeraialNewCompare();

            InitTitle();
        }

        private void InitData()
        {
            var ds = new Car_SerialBll().GetSerialInfoForPK(serialId1, serialId2);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 2)
            {
                var dr1 = ds.Tables[0].Rows[0];
                var dr2 = ds.Tables[0].Rows[1];
                entity = new SerialNewCompareEntity();
                entity.SerialShowName = ConvertHelper.GetString(dr1["cs_showname"]).Trim();
                entity.AllSpell = ConvertHelper.GetString(dr1["allspell"]);
                entity.ToSerialShowName = ConvertHelper.GetString(dr2["cs_showname"]).Trim();
                entity.ToAllSpell = ConvertHelper.GetString(dr2["allspell"]);
                entity.SerialId = ConvertHelper.GetInteger(dr1["cs_Id"]);
                entity.ToSerialId = ConvertHelper.GetInteger(dr2["cs_Id"]);
            }
            if (string.IsNullOrEmpty(csIds))
            {
                csIds = serialId1 + "," + serialId2;
            }
        }

        private void InitTitle()
        {
            pageTitle = "【车型对比_车型比较_汽车车型对比大全】_易车网";
            pageKeywords = "车型对比,汽车车型对比,车型对比大全";
            pageDescription = "车型对比平台为您提供详细的车型对比数据,通过对不同车型比较,包括最新报价，口碑，销量，基本性能的对比信息，更多的汽车综合对比信息尽在易车网！";
            if (serialEntity1 != null && serialEntity2 != null)
            {
                pageTitle = string.Format("【{0}和{1}汽车哪个好_优缺点_有什么区别】 -易车网", serialEntity1.ShowName, serialEntity2.ShowName);
                pageKeywords = string.Format("{0}和{1}优缺点,{0}和{1}哪个好,车型对比", serialEntity1.ShowName, serialEntity2.ShowName);
                pageDescription = string.Format("易车网车型对比平台为您提供{0}和{1}汽车的详细报价、口碑、销量，基本性能的对比，{0}和{1}的区别及优缺点，供您参考{0}和{1}哪个好。", serialEntity1.ShowName, serialEntity2.ShowName);
                navigate = string.Format("&gt; <a href=\"http://car.bitauto.com/duibi/\" target=\"_blank\">综合对比</a>&gt; <strong>{0}和{1}哪个好</strong>", serialEntity1.ShowName, serialEntity2.ShowName);
            }

        }
        /// <summary>
        /// 输出月份
        /// </summary>
        private void RenderMonth()
        {
            var dateTime = DateTime.Now;
            for (int i = 1; i < 7; i++)
            {
                dateTime = dateTime.AddMonths(-1);
                var thisYear = dateTime.Year.ToString();
                var thisMonth = dateTime.Month.ToString();
                if (thisMonth.Length == 1)
                {
                    thisMonth = "0" + thisMonth;
                }
                var yearMonth = thisYear + thisMonth;
                if (i == 1)
                {
                    MonthStr += "<li class=\"current\" value=\"" + yearMonth + "\"><a href=\"javascript:;\"> " + thisMonth + "月</a></li>";
                }
                else
                {
                    MonthStr += "<li value=\"" + yearMonth + "\"><a href=\"javascript:;\">" + thisMonth + "月</a></li>";
                }
            }
        }
        /// <summary>
        /// 输出最新对比数据
        /// </summary>
        private void RenderSeraialNewCompare()
        {
            StringBuilder sb = new StringBuilder();
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                var dict = (Dictionary<string, SerialNewCompareEntity>)obj;
                foreach (var kv in dict)
                {
                    sb.AppendFormat("<li><div class=\"txt\"><a target=\"_blank\" href=\"{0}\">{1} VS {2}</a></div></li>", "/duibi/" + kv.Value.SerialId + "-" + kv.Value.ToSerialId + "/", kv.Value.SerialShowName, kv.Value.ToSerialShowName);
                }
            }
            serialNewCompareHtml = sb.ToString();
        }
        /// <summary>
        /// 更新最新对比车型
        /// </summary>
        private void UpdateSerialCompare()
        {
            string serialCompareKey = (serialId1 < serialId2) ? (serialId1 + "_" + serialId2) : (serialId2 + "_" + serialId1);
            Dictionary<string, SerialNewCompareEntity> dict = new Dictionary<string, SerialNewCompareEntity>();

            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
            {
                // modified by chengl Apr.28.2015 字典复制 不同源，避免多线程问题 
                // dict = (Dictionary<string, SerialNewCompareEntity>)obj;
                dict = new Dictionary<string, SerialNewCompareEntity>((Dictionary<string, SerialNewCompareEntity>)obj);
                if (!dict.ContainsKey(serialCompareKey))
                {
                    AddSerialCompare(serialCompareKey, dict);
                }
            }
            else
            {
                AddSerialCompare(serialCompareKey, dict);
            }
        }

        public void AddSerialCompare(string serialCompareKey, Dictionary<string, SerialNewCompareEntity> dict)
        {
            List<SerialNewCompareEntity> list = new List<SerialNewCompareEntity>();
            //var ds = new Car_SerialBll().GetSerialInfoForPK(serialId1, serialId2);
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 2)
            //{
            //	var dr1 = ds.Tables[0].Rows[0];
            //	var dr2 = ds.Tables[0].Rows[1];
            //	SerialNewCompareEntity entity = new SerialNewCompareEntity();
            //	entity.SerialShowName = ConvertHelper.GetString(dr1["cs_showname"]);
            //	entity.AllSpell = ConvertHelper.GetString(dr1["allspell"]);
            //	entity.ToSerialShowName = ConvertHelper.GetString(dr2["cs_showname"]);
            //	entity.ToAllSpell = ConvertHelper.GetString(dr2["allspell"]);
            //	dict.Add(serialCompareKey, entity);
            //	if (dict.Count > 9) { dict.Remove(dict.Keys.First()); }
            //	CacheManager.InsertCache(cacheKey, dict, 60 * 24);
            //}
            if (entity != null)
            {
                if (!dict.ContainsKey(serialCompareKey))
                    dict.Add(serialCompareKey, entity);
                if (dict.Count > 9) { dict.Remove(dict.Keys.First()); }
                CacheManager.InsertCache(cacheKey, dict, 60 * 24 * 30);
            }
        }

        public class SerialNewCompareEntity
        {
            public int SerialId { get; set; }
            public string SerialShowName { get; set; }
            public string AllSpell { get; set; }

            public int ToSerialId { get; set; }
            public string ToSerialShowName { get; set; }
            public string ToAllSpell { get; set; }
        }

        /// <summary>
        /// 相关文章
        /// </summary>
        public void RenderNews()
        {
            var newsBLL = new CarNewsBll();
            var newsList1 = newsBLL.GetSerialNewsByCategoryId(serialId1, 8, 9);
            var newsList2 = newsBLL.GetSerialNewsByCategoryId(serialId2, 8, 9);

            foreach (var entity in newsList2)
            {
                if (!newsList1.Contains(entity))
                {
                    newsList1.Add(entity);
                }
            }
            if (!newsList1.Any()) return;
            var newsList = newsList1.OrderByDescending(m => m.PublishTime).ToArray();
            var list = new List<string>();
            list.Add("<div id=\"div_carnews\"><div class=\"section-header header2\">");
            list.Add("<div class=\"box\"><h2>相关文章</h2></div></div>");
            list.Add("<div class=\"article\">");
            list.Add("<div class=\"list-txt list-txt-s list-txt-default list-txt-style4\">");
            list.Add("<ul id=\"carNews\">");

            var newsCount = newsList.Count() >= 9 ? 9 : newsList.Count();
            for (int i = 0; i < newsCount; i++)
            {
                var entity = newsList[i];
                var newsStr = string.Format("<li><div class=\"txt\"><a target=\"_blank\" href=\"{0}\">{1}</a></div></li>", entity.FilePath, entity.Title);
                list.Add(newsStr);
            }
            list.Add("</ul></div></div></div>");
            NewsHtml = string.Join("", list);
        }

        /// <summary>
        /// 相关对比
        /// </summary>
        public void RenderUserCompare()
        {
            //大家跟谁比
            var carSerialBaseList1 = new Car_SerialBll().GetSerialCityCompareList(serialId1, HttpContext.Current);
            var carSerialBaseList2 = new Car_SerialBll().GetSerialCityCompareList(serialId2, HttpContext.Current);
            var compareEntityList = new List<SerialCompareEntity>();

            for (int i = 0; i < 3; i++)
            {
                if (carSerialBaseList1 != null && carSerialBaseList1.ContainsKey("全国") && i < carSerialBaseList1["全国"].Count)
                {
                    var entity1 = carSerialBaseList1["全国"][i];

                    var serialCompareEntity = GetSerialCompareEntity(serialEntity1, entity1);
                    if (!((serialCompareEntity.SerialIdL == serialId1 && serialCompareEntity.SerialIdR == serialId2)
                        || (serialCompareEntity.SerialIdL == serialId2 && serialCompareEntity.SerialIdR == serialId1)))
                    {
                        compareEntityList.Add(serialCompareEntity);
                    }
                }
                if (carSerialBaseList2 != null && carSerialBaseList2.ContainsKey("全国") && i < carSerialBaseList2["全国"].Count)
                {
                    var entity2 = carSerialBaseList2["全国"][i];
                    var serialCompareEntity = GetSerialCompareEntity(serialEntity2, entity2);
                    if (!((serialCompareEntity.SerialIdL == serialId1 && serialCompareEntity.SerialIdR == serialId2)
                            || (serialCompareEntity.SerialIdL == serialId2 && serialCompareEntity.SerialIdR == serialId1)))
                    {
                        compareEntityList.Add(serialCompareEntity);
                    }
                }
            }
            if (compareEntityList.Count == 0) return;
            var userCompareStrList = new List<string>();
            userCompareStrList.Add(string.Format("<div id=\"div_usercompare\"><div class=\"section-header header2\">"));
            userCompareStrList.Add(string.Format("<div class=\"box\"><h2>相关对比</h2></div></div>"));
            userCompareStrList.Add(string.Format("<div id=\"userCompare\" class=\"relevant-cont-box\">"));
            for (int i = 0; i < 3 && i < compareEntityList.Count; i++)
            {

                userCompareStrList.Add("<div class=\"relevant-contrast\">");
                var item = compareEntityList[i];
                var imgAltStr = string.Format("{0}和{1}哪个好", item.SerialnameL, item.SerialnameR);
                userCompareStrList.Add("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120 fl\">");
                userCompareStrList.Add(string.Format("<div class=\"img\"><a title=\"{2}\"  target=\"_blank\" href=\"/duibi/{0}-{1}/\">", item.SerialIdL, item.SerialIdR, imgAltStr));
				userCompareStrList.Add(string.Format("<img src=\"{0}\"/></a></div>", !string.IsNullOrWhiteSpace(item.ImageUrlL) ? item.ImageUrlL.Replace("_2.", "_3.") : WebConfig.DefaultCarPic));
                userCompareStrList.Add("<ul class=\"p-list\">");
                userCompareStrList.Add(string.Format("<li class=\"name\"><a href=\"/duibi/{0}-{1}/\">{2}</a></li>",item.SerialIdL, item.SerialIdR, item.SerialnameL));
                if (!string.IsNullOrEmpty(item.PriceL))
                {
                    userCompareStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">{0}</a></li>", item.PriceL));
                }
                else
                {
                    userCompareStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">暂无报价</a></li>"));
                }
                userCompareStrList.Add("</ul></div>");
                userCompareStrList.Add("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120 fr\">");
                userCompareStrList.Add(string.Format("<div class=\"img\"><a title=\"{2}\"  target=\"_blank\" href=\"/duibi/{0}-{1}/\">", item.SerialIdL, item.SerialIdR, imgAltStr));
                userCompareStrList.Add(string.Format("<img src=\"{0}\"/></a></div>",item.ImageUrlR !="" ? item.ImageUrlR.Replace("_2.", "_3."):""));
                userCompareStrList.Add("<ul class=\"p-list\">");
                userCompareStrList.Add(string.Format("<li class=\"name\"><a href=\"/duibi/{0}-{1}/\">{2}</a></li>",item.SerialIdL, item.SerialIdR, item.SerialnameR));
                if (!string.IsNullOrEmpty(item.PriceR))
                {
                    userCompareStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">{0}</a></li>", item.PriceR));
                }
                else
                {
                    userCompareStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">暂无报价</a></li>"));
                }
                userCompareStrList.Add("</ul></div>");
                userCompareStrList.Add("<div class=\"vs-small\"></div></div>");

            }
            userCompareStrList.Add(string.Format("</div></div>"));
            UserCompareHtml = string.Join("", userCompareStrList.ToArray());
        }

        private SerialCompareEntity GetSerialCompareEntity(SerialEntity serialEntity, Car_SerialBaseEntity baseEntity)
        {
            var priceR = new PageBase().GetSerialPriceRangeByID(baseEntity.SerialId);
            if (string.IsNullOrEmpty(priceR))
            {
                priceR = "暂无报价";
            }

            SerialCompareEntity serialCompareEntity = new SerialCompareEntity();
            if (serialEntity != null)
            {
                serialCompareEntity.SerialIdL = serialEntity.Id;
                serialCompareEntity.SerialnameL = serialEntity.ShowName;
                serialCompareEntity.PriceL = serialEntity.Price;
                serialCompareEntity.AllSpellL = serialEntity.AllSpell;
                serialCompareEntity.ImageUrlL = Car_SerialBll.GetSerialImageUrl(serialEntity.Id);
            }
            serialCompareEntity.SerialIdR = baseEntity.SerialId;
            serialCompareEntity.SerialnameR = baseEntity.SerialShowName;
            serialCompareEntity.PriceR = priceR;
            serialCompareEntity.AllSpellR = baseEntity.SerialNameSpell;
            serialCompareEntity.ImageUrlR = Car_SerialBll.GetSerialImageUrl(baseEntity.SerialId);

            return serialCompareEntity;
        }

        /// <summary>
        /// 热门对比
        /// </summary>
        public void RenderHotCompare()
        {
            //热门对比
            List<string> hotStrList = new List<string>();
            List<SerialCompareListEntity> serialCompareList = new SerialCompareListBll().GetHotSerialCompareList(3);
            if (serialCompareList.Count == 0) return;
            hotStrList.Add(string.Format("<div id=\"div_hotcompare\"><div class=\"section-header header2\">"));
            hotStrList.Add(string.Format("<div class=\"box\"><h2>热门对比</h2></div></div>"));
            hotStrList.Add(string.Format("<div id=\"hotCompare\" class=\"relevant-cont-box\">"));
            
            foreach (SerialCompareListEntity entity in serialCompareList)
            {
                var imgAltStr = string.Format("{0}和{1}哪个好", entity.SerialShowName, entity.ToSerialShowName);
                hotStrList.Add("<div class=\"relevant-contrast\">");
                hotStrList.Add("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120 fl\">");
                hotStrList.Add(string.Format("<div class=\"img\"><a title=\"{2}\"  target=\"_blank\" href=\"/duibi/{0}-{1}/\">", entity.SerialId, entity.ToSerialId, imgAltStr));
                hotStrList.Add(string.Format("<img src=\"{0}\"/></a></div>",entity.SerialImageUrl !="" ? entity.SerialImageUrl.Replace("_2.", "_3."):""));
                hotStrList.Add("<ul class=\"p-list\">");
                hotStrList.Add(string.Format("<li class=\"name\"><a href=\"/duibi/{0}-{1}/\">{2}</a></li>",entity.SerialId, entity.ToSerialId, entity.SerialShowName));
             
                if (!string.IsNullOrEmpty(entity.SerialPriceRange))
                {
                    hotStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">{0}</a></li>", entity.SerialPriceRange));
                }
                else
                {
                    hotStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">暂无报价</a></li>"));
                }
                hotStrList.Add("</ul></div>");

                hotStrList.Add("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120 fr\">");
                hotStrList.Add(string.Format("<div class=\"img\"><a title=\"{2}\"  target=\"_blank\" href=\"/duibi/{0}-{1}/\">", entity.SerialId, entity.ToSerialId, imgAltStr));
                hotStrList.Add(string.Format("<img src=\"{0}\"/></a></div>",entity.ToSerialImageUrl !="" ? entity.ToSerialImageUrl.Replace("_2.", "_3."):""));
                hotStrList.Add("<ul class=\"p-list\">");
                hotStrList.Add(string.Format("<li class=\"name\"><a href=\"/duibi/{0}-{1}/\">{2}</a></li>", entity.SerialId, entity.ToSerialId, entity.ToSerialShowName));
                if (!string.IsNullOrEmpty(entity.ToSerialPriceRange))
                {
                    hotStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">{0}</a></li>", entity.ToSerialPriceRange));
                }
                else
                {
                    hotStrList.Add(string.Format("<li class=\"price\"><a href=\"#\">暂无报价</a></li>"));
                }

                hotStrList.Add("</ul></div>");
                hotStrList.Add("<div class=\"vs-small\"></div></div>");
            }

            hotStrList.Add(string.Format("</div></div>"));

            HotCompareHtml = string.Join("", hotStrList.ToArray());
        }

        public class SerialCompareEntity
        {
            public int SerialIdL { get; set; }

            public string SerialnameL { get; set; }

            public string PriceL { get; set; }

            public string AllSpellL { get; set; }

            public string ImageUrlL { get; set; }

            public int SerialIdR { get; set; }

            public string SerialnameR { get; set; }

            public string PriceR { get; set; }

            public string AllSpellR { get; set; }

            public string ImageUrlR { get; set; }

        }

    }
}