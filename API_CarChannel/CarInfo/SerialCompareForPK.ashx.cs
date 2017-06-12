using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// SerialCompareForPK 的摘要说明
    /// </summary>
    public class SerialCompareForPK : PageBase, IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;

        private int serialId = 0;
        private string callback = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
            {
                Duration = 10 * 60,
                Location = OutputCacheLocation.Any,
                VaryByParam = "*"
            });
            page.ProcessRequest(HttpContext.Current);

            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            request = context.Request;
            serialId = ConvertHelper.GetInteger(request.QueryString["csid"]);
            string action = ConvertHelper.GetString(request.QueryString["action"]);
            callback = request.QueryString["callback"];
            switch (action.ToLower())
            {
                case "carcomparelist": RenderCarCompareList(); break;
                case "carbaseinfo": RenderBaseInfo(); break;
                case "carnews": RenderNews(); break;
                case "samerelatednews": RenderSameRelatedNews(); break;
                case "getvote": RenderSerialVote(); break;
                case "hotcompare": RenderHotCompareList(); break;
            }
        }
        /// <summary>
        /// 热门对比车型
        /// </summary>
        private void RenderHotCompareList()
        {
            int topN = ConvertHelper.GetInteger(request.QueryString["count"]);
            topN = topN <= 0 ? 3 : topN;
            //热门对比
            List<string> hotList = new List<string>();
            List<SerialCompareListEntity> serialCompareList = new SerialCompareListBll().GetHotSerialCompareList(topN);
            foreach (SerialCompareListEntity entity in serialCompareList)
            {
                hotList.Add(string.Format("{{serialid:{8},serialname:\"{0}\",price:\"{1}\",imgurl:\"{2}\",allspell:\"{3}\",toserialid:{9},toserialname:\"{4}\",toprice:\"{5}\",toimgurl:\"{6}\",toallspell:\"{7}\",comparecount:{10}}}",
                    entity.SerialShowName,
                    entity.SerialPriceRange,
                    entity.SerialImageUrl,
                      entity.SerialAllSpell.ToLower(),
                    entity.ToSerialShowName,
                    entity.ToSerialPriceRange,
                    entity.ToSerialImageUrl,
                    entity.ToSerialAllSpell,
                    entity.SerialId,
                    entity.ToSerialId,
                    entity.CompareCount));
            }
            string json = string.Format("[{0}]", string.Join(",", hotList.ToArray()));
            Jsonp(json, callback, HttpContext.Current);
        }
        /// <summary>
        /// 获取投票数量
        /// </summary>
        private void RenderSerialVote()
        {
            var voteBll = new SerialCompareVoteBll();
            string serialIds = ConvertHelper.GetString(request.QueryString["csid"]);
            if (string.IsNullOrEmpty(serialIds))
            {
                Jsonp("{}", callback, HttpContext.Current);
                return;
            }

            var serialIdArr = serialIds.Split(',').Select(p => ConvertHelper.GetInteger(p)).OrderBy(p => p).ToArray();
            if (serialIdArr.Length != 2)
            {
                Jsonp("{}", callback, HttpContext.Current);
                return;
            }

            string json = "{{\"{0}\":{1},\"{2}\":{3}}}";


            //string cacheKey = string.Format("{0}_{1}", string.Join("_", serialIdArr.ToArray()), DateTime.Now.ToString("yyyyMMddHH"));
            string cacheKey = string.Format("{0}", string.Join("_", serialIdArr.ToArray()));

            var currentVoteArr = MemCache.GetMemCacheByKey(cacheKey);
            if (currentVoteArr != null)
            {
                var resultArr = (int[])currentVoteArr;
                json = string.Format(json, serialIdArr[0],
                    (resultArr[2] + resultArr[0]),
                    serialIdArr[1],
                    (resultArr[3] + resultArr[1]));
            }
            else
            {
                var baseVoteArr = voteBll.GetSerialVote(serialIdArr[0], serialIdArr[1]);

                json = string.Format(json, serialIdArr[0],
                    baseVoteArr[0],
                    serialIdArr[1],
                    baseVoteArr[1]);

            }
            Jsonp(json, callback, HttpContext.Current);
        }
        /// <summary>
        /// 编辑们还看
        /// </summary>
        private void RenderSameRelatedNews()
        {
            string seriaIds = ConvertHelper.GetString(request.QueryString["csids"]);
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(seriaIds))
            {
                var serialArray = seriaIds.Split(',').Select(p => ConvertHelper.GetInteger(p)).ToArray();
                DataSet ds = new CarNewsBll().GetRelatedMoreSerialNewsData(serialArray, 8, 3);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(string.Format("{{title:\"{0}\",firstimg:\"{1}\",link:\"{2}\",source:\"{3}\",publishtime:\"{4}\",commentcount:{5},author:\"{6}\",summary:\"{7}\",newsid:\"{8}\"}}",
                        CommonFunction.GetUnicodeByString(ConvertHelper.GetString(dr["FaceTitle"])),
                        dr["FirstPicUrl"],
                        dr["FilePath"],
                        dr["SourceName"],
                        dr["PublishTime"],
                        dr["CommentNum"],
                        CommonFunction.GetUnicodeByString(ConvertHelper.GetString(dr["Author"])),
                        CommonFunction.GetUnicodeByString(ConvertHelper.GetString(dr["Summary"])),
                        dr["CmsNewsId"]));
                }
            }
            string json = string.Format("[{0}]", string.Join(",", list.ToArray()));
            Jsonp(json, callback, HttpContext.Current);
        }
        /// <summary>
        /// 对比车型
        /// </summary>
        private void RenderCarCompareList()
        {
            //看了还看
            List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(serialId, 6);
            List<string> seeList = new List<string>();
            foreach (EnumCollection.SerialToSerial sts in lsts)
            {
                seeList.Add(string.Format("{{serialid:{4},serialname:\"{0}\",price:\"{1}\",imgurl:\"{2}\",url:\"{3}\"}}",
                    sts.ToCsShowName,
                    sts.ToCsPriceRange,
                    sts.ToCsPic.Replace("_5.", "_1."),
                    "/" + sts.ToCsAllSpell.ToLower() + "/",
                    sts.ToCsID));
            }
            //大家跟谁比
            Dictionary<string, List<Car_SerialBaseEntity>> carSerialBaseList = new Car_SerialBll().GetSerialCityCompareList(serialId, HttpContext.Current);
            List<string> userCompareList = new List<string>();
            if (carSerialBaseList != null && carSerialBaseList.ContainsKey("全国"))
            {
                foreach (Car_SerialBaseEntity entity in carSerialBaseList["全国"].Take(3))
                {
                    userCompareList.Add(string.Format("{{serialid:{4},serialname:\"{0}\",price:\"{1}\",imgurl:\"{2}\",url:\"{3}\"}}",
                    entity.SerialShowName,
                    entity.SerialPrice,
                    Car_SerialBll.GetSerialImageUrl(entity.SerialId),
                    "/" + entity.SerialNameSpell.ToLower() + "/",
                    entity.SerialId));
                }
            }
            string json = string.Format("{{seetosee:[{0}],usercompare:[{1}]}}",
                string.Join(",", seeList.ToArray()),
                string.Join(",", userCompareList.ToArray()));
            Jsonp(json, callback, HttpContext.Current);
        }
        /// <summary>
        /// 导购新闻
        /// </summary>
        private void RenderNews()
        {
            var newsBLL = new CarNewsBll();
            List<string> list = new List<string>();
            var newsList = newsBLL.GetSerialNewsByCategoryId(serialId, 8, 9);
            foreach (var entity in newsList)
            {
                list.Add(string.Format("{{cmsnewsid:{2},title:\"{0}\",link:\"{1}\",publishtime:\"{3}\"}}",
                CommonFunction.GetUnicodeByString(entity.FaceTitle),
                    entity.FilePath,
                    entity.CmsNewsId,
                    entity.PublishTime));
            }
            string json = string.Format("{{relatednewslist:[{0}]}}",
                string.Join(",", list.ToArray()));
            Jsonp(json, callback, HttpContext.Current);
        }

        private void Jsonp(string content, string callbackName, HttpContext context)
        {
            if (string.IsNullOrEmpty(callbackName))
                context.Response.Write(content);
            else
                context.Response.Write(string.Format("{1}({0})", content, callbackName));
        }
        /// <summary>
        /// 对比车型基本数据
        /// </summary>
        private void RenderBaseInfo()
        {
            SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
            if (serialEntity == null)
            {
                Jsonp("{}", callback, HttpContext.Current);
                return;
            }
            int len = 0, width = 0, height = 0, wheel = 0;
            double minPrice = 0, maxPrice = 0, minFuel = 0, maxFuel = 0;
            if (serialEntity.CarList != null && serialEntity.CarList.Length > 0)
            {
                var carList = serialEntity.CarList;
                if (carList.Length > 0)
                {
                    var carPriceList = carList.Where(p => p.SaleState == "在销" && p.ReferPrice > 0).Select(p => p.ReferPrice);
                    if (carPriceList.Count() > 0)
                    {
                        minPrice = carPriceList.Min();
                        maxPrice = carPriceList.Max();
                    }
                    //最低配油耗
                    var minEntity = carList.FirstOrDefault(p => p.ReferPrice == minPrice);
                    if (minEntity != null)
                        minFuel = ConvertHelper.GetDouble(minEntity[782]);
                    //最高配油耗
                    var maxEntity = carList.FirstOrDefault(p => p.ReferPrice == maxPrice);
                    if (maxEntity != null)
                        maxFuel = ConvertHelper.GetDouble(maxEntity[782]);
                    //最热门车
                    var hotCar = carList.OrderByDescending(p => p.CarPV).First();
                    len = ConvertHelper.GetInteger(hotCar[588]);
                    width = ConvertHelper.GetInteger(hotCar[593]);
                    height = ConvertHelper.GetInteger(hotCar[586]);
                    wheel = ConvertHelper.GetInteger(hotCar[592]);
                }
            }
            //11张图片
            List<XmlNode> srcElevenImageList = new Car_SerialBll().GetSerialElevenPositionImage(serialId);
            List<string> listImage = new List<string>();
            string[] positionArr = { "47", "43", "46", "44", "94" };
            var loop = 0;
            foreach (var positionId in positionArr)
            {
                loop++;
                var xmlNode = srcElevenImageList.Find(p => (p.Attributes["PositionId"] != null && p.Attributes["PositionId"].Value == positionId));
                if (xmlNode != null)
                {
                    listImage.Add(string.Format("{{imgurl:\"{0}\",imglink:\"{1}\"}}", string.Format(xmlNode.Attributes["ImageUrl"].Value, 4),
                        xmlNode.Attributes["Link"].Value));
                }
                else
                {
                    listImage.Add(string.Format("{{imgurl:\"{0}\",imglink:\"{1}\"}}",
                        loop == 1 ? Car_SerialBll.GetSerialImageUrl(serialId).Replace("_2.", "_6.") : "http://image.bitautoimg.com/autoalbum/V2.1/images/300-200.gif",
                        loop == 1 ? "http://photo.bitauto.com/serial/" + serialId + "/" : ""));
                }
            }

            string json = string.Format("{{serialname:\"{0}\",pricerange:\"{10}\",imageurl:\"{11}\",allspell:\"{12}\",masterid:\"{13}\",minreferprice:{1},maxreferprice:{2},minfuel:\"{3}\",maxfuel:\"{4}\",len:{5},width:{6},height:{7},wheel:{8},imagelist:[{9}]}}",
                serialEntity.ShowName,
                minPrice,
                maxPrice,
                minFuel,
                maxFuel,
                len, width, height, wheel,
                string.Join(",", listImage.ToArray()),
                serialEntity.Price,
                Car_SerialBll.GetSerialImageUrl(serialId),
                serialEntity.AllSpell,
                serialEntity.Brand.MasterBrandId);

            Jsonp(json, callback, HttpContext.Current);
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