using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web.Interface.carinformation
{
    /// <summary>
    ///     图片和视频
    /// </summary>
    public class ImageAndVideoData : H5PageBase, IHttpHandler
    {
        private Dictionary<int, List<SerialPositionImagesEntity>> _serialImageDictionary =
            new Dictionary<int, List<SerialPositionImagesEntity>>();

        public void ProcessRequest(HttpContext context)
        {
            SetPageCache(60);
            context.Response.ContentType = "text/plain";

            try
            {
                var serialId = 0;
                if (!string.IsNullOrEmpty(context.Request.QueryString["csid"]))
                {
                    serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);
                }

                var keyList = new List<int> {serialId};
                var cacheKey = "Interface_ImageAndVideoData_" + string.Join("_", keyList);

                var obj = CacheManager.GetCachedData(cacheKey);

                if (obj != null)
                {
                    context.Response.Write(obj);
                }
                else
                {
                    InitSerialImage(serialId);
                    var targetImgList = new List<SerialPositionImagesEntity>();
                    const int takeCount = 3;
                    int[] positionIds = {6, 7, 8}; //注意：该数组元素顺序不能随意修改

                    if (_serialImageDictionary.Count <= 0) return;

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

                    for (var i = 0; i < targetImgList.Count; i++)
                    {
                        targetImgList[i].ImageUrl = string.Format(targetImgList[i].ImageUrl, i == 0 ? 8 : 4);
                    }

                    var videoList = GetNewAndHotVideo(serialId, 3, 1);

                    //SerialEntity baseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);

                    #region

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append(" <div class='slide' data-anchor='slide3-1'>");
                    stringBuilder.Append("        <header>");
                    stringBuilder.Append("            <h2>车型图片</h2>");
                    stringBuilder.Append("        </header>");
                    if (targetImgList.Count > 0)
                    {
                        #region

                        stringBuilder.Append("<div class='slide_box'>");
                        for (var i = 0; i < targetImgList.Count; i++)
                        {
                            if (i == 0)
                            {
                                stringBuilder.AppendFormat(
                                    "<a href='http://photo.m.yiche.com/picture/{0}/{1}/' class='pic-wall-re'>", serialId,
                                    targetImgList[i].ImageId);
                                stringBuilder.Append("<span class='gradient-bg'>");
                                stringBuilder.AppendFormat("<em>{0}</em>",
                                    targetImgList[i].PositionId == 6
                                        ? "外观"
                                        : targetImgList[i].PositionId == 7
                                            ? "内饰"
                                            : targetImgList[i].PositionId == 8 ? "空间" : "");
                                stringBuilder.Append("</span>");
                                stringBuilder.AppendFormat("<img src='{0}'/>",
                                    targetImgList[i].ImageUrl.Replace("autoalbum", "newsimg-750-w0-1-q80/autoalbum"));
                                stringBuilder.Append("</a>");
                            }
                            if (i == 1)
                            {
                                stringBuilder.Append("<div class='pic-wall'>");
                            }
                            if (i >= 1)
                            {
                                stringBuilder.Append("<div>");
                                stringBuilder.AppendFormat(
                                    "<a href='http://photo.m.yiche.com/picture/{0}/{1}/' class='pic-wall-re'>", serialId,
                                    targetImgList[i].ImageId);
                                stringBuilder.Append("<span class='gradient-bg'>");
                                stringBuilder.AppendFormat("<em>{0}</em>",
                                    targetImgList[i].PositionId == 6
                                        ? "外观"
                                        : targetImgList[i].PositionId == 7
                                            ? "内饰"
                                            : targetImgList[i].PositionId == 8 ? "空间" : "");
                                stringBuilder.Append("                        </span>                        ");
                                stringBuilder.AppendFormat("<img src='{0}'/>",
                                    targetImgList[i].ImageUrl.Replace("autoalbum", "newsimg-750-w0-1-q80/autoalbum"));
                                stringBuilder.Append("</a>");
                                stringBuilder.Append("</div>");
                            }
                            if (i == targetImgList.Count - 1 && i >= 1)
                            {
                                stringBuilder.Append("</div>");
                            }
                        }

                        #endregion

                        stringBuilder.Append("</div>");
                    }
                    else
                    {
                        stringBuilder.Append("        <div class='message-failure'>");
                        stringBuilder.Append(
                            "            <img src='http://img1.bitautoimg.com/uimg/4th/img2/failure.png'/>");
                        stringBuilder.Append("            <h2>很遗憾！</h2>");
                        stringBuilder.Append("            <p>数据抓紧完善中，敬请期待！</p>");
                        stringBuilder.Append("        </div>");
                    }

                    stringBuilder.Append("        <!--广告 start -->");
                    stringBuilder.Append("        <div id='adimg' class='fullscreen mt5'></div>        ");
                    stringBuilder.Append("        <!--广告 end -->");
                    stringBuilder.Append("    </div>");

                    if (videoList.Count > 0)
                    {
                        stringBuilder.Append("    <div class='slide' data-anchor='slide3-2'>        ");
                        stringBuilder.Append("        <header>");
                        stringBuilder.Append("            <h2>车型视频</h2>");
                        stringBuilder.Append("        </header>       ");
                        stringBuilder.Append("        <div class='slide_box' id='video-pic-wall'>");

                        for (var i = 0; i < videoList.Count; i++)
                        {
                            if (i == 0)
                            {
                                stringBuilder.AppendFormat("<a href='{0}' class='pic-wall-re'>",
                                    videoList[0].ShowPlayUrl);
                                stringBuilder.Append("                <span class='icon-video'></span>               ");

                                stringBuilder.AppendFormat("<img src='{0}' />",
                                    videoList[0].ImageLink.Replace("Video", "newsimg-367-w0/Video"));
                                stringBuilder.Append("            </a>");
                                stringBuilder.Append("            <span class='pic-video-txt'>");
                                stringBuilder.AppendFormat("<a href='{0}'>", videoList[0].ShowPlayUrl);
                                stringBuilder.AppendFormat("<em>{0}</em>", videoList[0].ShortTitle);
                                stringBuilder.Append("                </a>");
                                stringBuilder.Append("            </span>   ");
                            }


                            if (i == 1)
                            {
                                stringBuilder.Append("<div class='pic-wall'>");
                            }

                            if (i >= 1)
                            {
                                stringBuilder.Append("<div>");
                                stringBuilder.AppendFormat("<a href='{0}' class='pic-wall-re'>",
                                    videoList[0].ShowPlayUrl);
                                stringBuilder.Append("<span class='icon-video'></span>");
                                stringBuilder.AppendFormat("<img src='{0}' />",
                                    videoList[0].ImageLink.Replace("Video", "newsimg-367-w0/Video"));
                                stringBuilder.Append("</a>");
                                stringBuilder.Append("<span class='pic-video-txt'>");
                                stringBuilder.AppendFormat("<a href='{0}'>", videoList[0].ShowPlayUrl);
                                stringBuilder.AppendFormat("<em>{0}</em>", videoList[0].ShortTitle);
                                stringBuilder.Append("</a>");
                                stringBuilder.Append("</span>");
                                stringBuilder.Append("</div>  ");
                            }

                            if (i == videoList.Count - 1 && i >= 1)
                            {
                                stringBuilder.Append("</div>");
                            }
                        }
                        stringBuilder.Append("        </div>");
                        stringBuilder.Append("        <div class='big_bg'>");
                        stringBuilder.Append("            <button class='button_gray'>");
                        stringBuilder.AppendFormat("<a href='http://v.m.yiche.com/car/serial/{0}_0_0.html'>查看更多视频</a>",
                            serialId);
                        stringBuilder.Append("            </button>");
                        stringBuilder.Append("        </div>       ");
                        stringBuilder.Append("    </div>");
                    }
                    stringBuilder.Append("    <div class='arrow_down'></div>");


                    var res = stringBuilder.ToString();

                    //CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);

                    context.Response.Write(res);

                    #endregion
                }
            }
            catch
            {
                context.Response.Write("");
            }
        }

        public bool IsReusable
        {
            get { return false; }
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

        /// <summary>
        /// </summary>
        /// <param name="serialId"></param>
        /// <param name="totalVideo">共提取视频数</param>
        /// <param name="hotVideoCount">共提取视频数中可最多包含最热视频数</param>
        /// <returns></returns>
        private static List<VideoEntity> GetNewAndHotVideo(int serialId, int totalVideo = 7, int hotVideoCount = 2)
        {
            var resultList = new List<VideoEntity>();
            try
            {
                var newList = VideoBll.GetVideoBySerialId(serialId); //最新 默认取5个
                var hotList = VideoBll.GetHotVideoBySerialId(serialId); //最热 默认取2个

                resultList.AddRange(newList.Take(totalVideo));

                if (resultList.Count == totalVideo && hotList.Any())
                {
                    resultList.RemoveRange(totalVideo - hotVideoCount - 1, hotVideoCount);
                }

                foreach (var entity in hotList.Take(hotVideoCount))
                {
                    if (resultList.Count >= totalVideo)
                    {
                        break;
                    }
                    if (resultList.Find(p => p.VideoId == entity.VideoId) == null)
                    {
                        resultList.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.WriteLog(ex.ToString());
            }
            return resultList;
        }
    }
}