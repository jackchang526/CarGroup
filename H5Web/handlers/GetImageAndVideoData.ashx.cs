using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using Newtonsoft.Json;

namespace H5Web.handlers
{
    /// <summary>
    ///     GetImageAndVideoData 的摘要说明
    /// </summary>
    public class GetImageAndVideoData : H5PageBase, IHttpHandler
    {
        private Dictionary<int, List<SerialPositionImagesEntity>> _serialImageDictionary =
            new Dictionary<int, List<SerialPositionImagesEntity>>();

        public void ProcessRequest(HttpContext context)
        {
            //SetPageCache(60);

            //context.Response.ContentType = "text/plain";
            context.Response.ContentType = "application/json; charset=utf-8";

            var targetImgList = new List<SerialPositionImagesEntity>();
            var videoList = new List<VideoEntity>();
            const int takeCount = 3;
            try
            {
                int[] positionIds = {6, 7, 8}; //注意：该数组元素顺序不能随意修改
                var serialId = 0;
                if (!string.IsNullOrEmpty(context.Request.QueryString["csid"]))
                {
                    serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);
                    InitSerialImage(serialId);
                }
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
                    targetImgList.AddRange(tempImgList.Take(3-targetImgList.Count));
                }

                #endregion

                for (var i = 0; i < targetImgList.Count; i++)
                {
                    targetImgList[i].ImageUrl = string.Format(targetImgList[i].ImageUrl, i == 0 ? 8 : 4);
                }

                videoList = GetNewAndHotVideo(serialId, 3, 1);

                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    img = targetImgList,
                    video = videoList
                }));
            }
            catch
            {
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    img = targetImgList,
                    video = videoList,
                }));
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