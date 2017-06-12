using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml.Linq;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web.handlers
{
    /// <summary>
    ///     GetImageList 的摘要说明
    /// </summary>
    public class GetImageList : H5PageBase, IHttpHandler
    {
        private Dictionary<int, List<SerialPositionImagesEntity>> _serialImageDictionary =
            new Dictionary<int, List<SerialPositionImagesEntity>>();

        public void ProcessRequest(HttpContext context)
        {
            //SetPageCache(60);
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            int[] positionIds = {6, 7, 8};
            var index = 0;
            var serialId = 0;
            if (!string.IsNullOrEmpty(context.Request.QueryString["csid"]))
            {
                serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);
                InitSerialImage(serialId);
            }

            var sb = new StringBuilder();

            if (_serialImageDictionary.Count > 0)
            {
                #region 

                foreach (var positionId in positionIds)
                {
                    if (!_serialImageDictionary.ContainsKey(positionId))
                        continue;
                    var imageList = _serialImageDictionary[positionId];
                    var imageFirst = imageList.Count > 0 ? imageList[0] : null;
                    var imageSecond = imageList.Count > 1 ? imageList[1] : null;
                    var imageThird = imageList.Count > 2 ? imageList[2] : null;
                    index++;

                    sb.AppendFormat("<div class='slide' data-anchor='slide3-{0}'>", index);
                    sb.Append("<header><h2>");
                    switch (positionId)
                    {
                        case 6:
                            sb.Append("外观设计");
                            break;
                        case 7:
                            sb.Append("内饰设计");
                            break;
                        case 8:
                            sb.Append("空间设计");
                            break;
                    }
                    sb.Append("</h2></header>");
                    sb.Append("     <div class='slide_box'>");

                    sb.AppendFormat("   <a href='http://photo.m.yiche.com/picture/{0}/{1}/'>",serialId, imageFirst.ImageId);
                    sb.AppendFormat("       <img src='{0}'/>", string.Format(imageFirst.ImageUrl, 8));
                    sb.Append("         </a>");

                    if (imageSecond != null)
                    {
                        sb.Append("     <div class='pic-wall'>");
                        sb.Append("         <div>");
                        sb.AppendFormat("       <a href='http://photo.m.yiche.com/picture/{0}/{1}/'>",serialId, imageSecond.ImageId);
                        sb.AppendFormat("           <img src='{0}'/>", string.Format(imageSecond.ImageUrl, 4));
                        sb.Append("             </a>");
                        sb.Append("         </div>");

                        if (imageThird != null)
                        {
                            sb.Append("     <div>");
                            sb.AppendFormat("   <a href='http://photo.m.yiche.com/picture/{0}/{1}/'>",serialId, imageThird.ImageId);
                            sb.AppendFormat("       <img src='{0}'/>", string.Format(imageThird.ImageUrl, 4));
                            sb.Append("         </a>");
                            sb.Append("     </div>");
                        }
                        sb.Append("      </div>");
                    }

                    sb.Append("     </div>");
                    sb.Append("</div>");

                }

                #endregion

            }
            else
            {
                sb.Append("<div class='message-failure'>");
                sb.Append("    <img src='http://img1.bitautoimg.com/uimg/4th/img2/failure.png'/>");
                sb.Append("    <h2>很遗憾！</h2>");
                sb.Append("    <p>数据抓紧完善中，敬请期待！</p>");
                sb.Append("</div>");
            }
            sb.Append("<div class='arrow_down'></div>");

            context.Response.Write(sb.ToString());
            context.Response.End();
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
    }
}