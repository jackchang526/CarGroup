using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace H5Web.Interface
{
    /// <summary>
    /// Forbmw 的摘要说明 
    /// </summary>
    public class Forbmw : H5PageBase, IHttpHandler
    {
        private Dictionary<int, List<SerialPositionImagesEntity>> _serialImageDictionary =
            new Dictionary<int, List<SerialPositionImagesEntity>>();

        private readonly Car_BasicBll _carBll = new Car_BasicBll();
        private readonly SerialFourthStageBll _serialFourthStageBll = new SerialFourthStageBll();
        protected Dictionary<int, string> CarParamDictionary;

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);

            context.Response.ContentType = "text/xml";

            var serialId = 0;
            if (!string.IsNullOrEmpty(context.Request.QueryString["csid"]))
            {
                serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);
            }

            var dataType = "";
            if (!string.IsNullOrEmpty(context.Request.QueryString["type"]))
            {
                dataType = context.Request.QueryString["type"];
            }

            //创建XDocument文档
            XDocument xDoc = new XDocument();
            XElement root = new XElement("root");

            //i(图片),c(评测),p(报价),s(配置)
            switch (dataType)
            {
                case "i":
                    #region 图片

                    List<SerialPositionImagesEntity> imgageList = GetImgageList(serialId);

                    if (imgageList.Count > 0)
                    {
                        XElement images = new XElement("Images");
                        for (var i = 0; i < imgageList.Count; i++)
                        {
                            var imgae = new XElement("image",
                                new XAttribute("PositionId", imgageList[i].PositionId),
                                new XAttribute("ImageUrl", string.Format(imgageList[i].ImageUrl, i == 0 ? 8 : 4))
                                );
                            images.Add(imgae);
                        }
                        root.Add(images);
                    }

                    #endregion
                    break;
                case "c":
                    #region 评测
                    XElement news = GetNewList(serialId);
                    if (news != null)
                    {
                        root.Add(news);
                    }
                    #endregion
                    break;
                case "p":
                    #region 报价

                    IEnumerable<CarInfoForSerialSummaryEntity> carList = GetCarList(serialId);

                    var carInfoForSerialSummaryEntities = carList as CarInfoForSerialSummaryEntity[] ?? carList.ToArray();
                    if (carInfoForSerialSummaryEntities.Any())
                    {
                        XElement carsElement = new XElement("Cars");
                        foreach (var carInfoForSerialSummaryEntity in carInfoForSerialSummaryEntities)
                        {
                            var car = new XElement("car",
                                new XAttribute("CarID", carInfoForSerialSummaryEntity.CarID),
                                new XAttribute("CarName", carInfoForSerialSummaryEntity.CarName),
                                new XAttribute("CarPV", carInfoForSerialSummaryEntity.CarPV),
                                new XAttribute("CarPriceRange", carInfoForSerialSummaryEntity.CarPriceRange),
                                new XAttribute("TransmissionType", carInfoForSerialSummaryEntity.TransmissionType),
                                new XAttribute("Engine_Exhaust", carInfoForSerialSummaryEntity.Engine_Exhaust),
                                new XAttribute("ReferPrice", carInfoForSerialSummaryEntity.ReferPrice),
                                new XAttribute("UnderPan_ForwardGearNum", carInfoForSerialSummaryEntity.UnderPan_ForwardGearNum),
                                new XAttribute("Engine_MaxPower", carInfoForSerialSummaryEntity.Engine_MaxPower),
                                new XAttribute("Electric_Peakpower", carInfoForSerialSummaryEntity.Electric_Peakpower),
                                new XAttribute("Engine_InhaleType", carInfoForSerialSummaryEntity.Engine_InhaleType),
                                new XAttribute("Engine_AddPressType", carInfoForSerialSummaryEntity.Engine_AddPressType),
                                new XAttribute("Oil_FuelType", carInfoForSerialSummaryEntity.Oil_FuelType),
                                new XAttribute("CarYear", carInfoForSerialSummaryEntity.CarYear),
                                new XAttribute("SaleState", carInfoForSerialSummaryEntity.SaleState),
                                new XAttribute("ProduceState", carInfoForSerialSummaryEntity.ProduceState),
                                new XAttribute("IsImport", carInfoForSerialSummaryEntity.IsImport)
                                );
                            carsElement.Add(car);
                        }
                        root.Add(carsElement);
                    }

                    #endregion
                    break;
                case "s":
                    #region 配置

                    List<SerialSparkle> serialSparkleList = GetSerialSparkle(serialId);

                    if (serialSparkleList.Count > 0)
                    {
                        XElement serialSparkleElement = new XElement("SerialSparkle");

                        foreach (var serialSparkle in serialSparkleList.Take(20))
                        {
                            var sparkle = new XElement("Sparkle",
                                new XAttribute("H5SId", serialSparkle.H5SId),
                                new XAttribute("Name", serialSparkle.Name)
                                );
                            serialSparkleElement.Add(sparkle);
                        }
                        root.Add(serialSparkleElement);
                    }

                    #endregion
                    break;
            }
            
            xDoc.Add(root);
            context.Response.Write(xDoc);
        }


        /// <summary>
        ///     初始化子品牌颜色列表
        /// </summary>
        protected void InitSerialImage(int serialId)
        {
            var xmlPath =
                string.Format(Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialFourthStageImagePath),
                    serialId);
            if (!File.Exists(xmlPath))
            {
                return;
            }
            var document = XDocument.Load(xmlPath);
            var imageEles = document.Descendants("Images");
            var dic = new Dictionary<int, List<SerialPositionImagesEntity>>();
            foreach (var ele in imageEles)
            {
                var groupId = Convert.ToInt32(ele.Attribute("GroupId").Value);
                if (groupId == 12)
                    continue;
                var imgEles = ele.Descendants("Image");
                var imageList = new List<SerialPositionImagesEntity>();
                foreach (var imgEle in imgEles)
                {
                    imageList.Add(
                        new SerialPositionImagesEntity
                        {
                            ImageId = Convert.ToInt32(imgEle.Attribute("ImageID").Value),
                            ImageName = imgEle.Attribute("ImageName").Value,
                            ImageUrl = imgEle.Attribute("ImageUrl").Value,
                            PositionId = groupId
                        }
                        );
                }
                if (imageList.Count > 0)
                {
                    dic[groupId] = imageList;
                }
            }
            if (dic.Count > 0)
            {
                _serialImageDictionary = dic;
            }
        }

        protected List<SerialPositionImagesEntity> GetImgageList(int serialId)
        {
            InitSerialImage(serialId);

            var targetImgList = new List<SerialPositionImagesEntity>();
            int[] positionIds = { 6, 7, 8 }; //注意：该数组元素顺序不能随意修改
            const int takeCount = 3;

            #region 用外观图片初始化图片列表

            var waiGuan = new List<SerialPositionImagesEntity>();
            if (_serialImageDictionary.ContainsKey(positionIds[0]))
            {
                waiGuan = _serialImageDictionary[positionIds[0]];
            }

            var neiShi = new List<SerialPositionImagesEntity>();
            if (_serialImageDictionary.ContainsKey(positionIds[1]))
            {
                neiShi = _serialImageDictionary[positionIds[1]];
            }

            var kongJian = new List<SerialPositionImagesEntity>();
            if (_serialImageDictionary.ContainsKey(positionIds[2]))
            {
                kongJian = _serialImageDictionary[positionIds[2]];
            }


            var tempImgList = new List<SerialPositionImagesEntity>();

            if (waiGuan.Count > 0)
            {
                targetImgList.Add(waiGuan.First());
                waiGuan.RemoveAt(0);
                tempImgList.AddRange(waiGuan.Take(2));
            }
            if (neiShi.Count > 0)
            {
                targetImgList.Add(neiShi.First());
                neiShi.RemoveAt(0);
                tempImgList.AddRange(neiShi.Take(2));
            }
            if (kongJian.Count > 0)
            {
                targetImgList.Add(kongJian.First());
                kongJian.RemoveAt(0);
                tempImgList.AddRange(kongJian.Take(2));
            }
            if (targetImgList.Count < takeCount)
            {
                targetImgList.AddRange(tempImgList.Take(3 - targetImgList.Count));
            }

            #endregion

            return targetImgList;
        }

        protected IEnumerable<CarInfoForSerialSummaryEntity> GetCarList(int serialId)
        {
            var topCount = 10;

            var carModelList = _serialFourthStageBll.MakeCarList(serialId);

            var targetList = carModelList.Take(topCount);

            var carInfoForSerialSummaryEntities = targetList as CarInfoForSerialSummaryEntity[] ??
                                                  targetList.ToArray();

            var carids = carInfoForSerialSummaryEntities.Select(p => p.CarID);

            CarParamDictionary = _carBll.GetCarParamValueByCarIds(carids.ToArray(), 724);

            foreach (var item in carInfoForSerialSummaryEntities)
            {
                if (CarParamDictionary != null && CarParamDictionary.ContainsKey(item.CarID))
                {
                    var paramValue = CarParamDictionary[item.CarID];
                    item.UnderPan_ForwardGearNum = (!string.IsNullOrEmpty(paramValue) && paramValue != "无级" &&
                                                    paramValue != "待查")
                        ? paramValue + "挡"
                        : "";
                }
                else
                {
                    item.UnderPan_ForwardGearNum = (!string.IsNullOrEmpty(item.UnderPan_ForwardGearNum) &&
                                                    item.UnderPan_ForwardGearNum != "无级" &&
                                                    item.UnderPan_ForwardGearNum != "待查")
                        ? item.UnderPan_ForwardGearNum
                        : "";
                }
            }
            return carInfoForSerialSummaryEntities;
        }

        protected List<SerialSparkle> GetSerialSparkle(int serialId)
        {
            List<SerialSparkle> serialSparkleList = _serialFourthStageBll.GetSerialSparkle(serialId);
            return serialSparkleList;
        }

        protected XElement GetNewList(int serialId)
        {

            XElement news = new XElement("News");

            #region 

            if (serialId <= 0) return null;
            var xmlPath = Path.Combine(WebConfig.DataBlockPath,string.Format(@"Data\SerialNews\H5V3News\{0}.xml", serialId));
            if (!File.Exists(xmlPath)) return null;
            var xDoc = XDocument.Load(xmlPath);
            var root = xDoc.Root;
            if (root == null) return null;

            var descendants = root.Descendants("Item");

            var xElements = descendants as XElement[] ?? descendants.ToArray();
            if (xElements.Any())
            {
                foreach (XElement item in xElements)
                {
                    news.Add(item);
                }
            }

            #endregion

            return news;

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