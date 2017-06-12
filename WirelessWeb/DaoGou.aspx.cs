using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.cn.com.baa.api;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace WirelessWeb
{
    public partial class DaoGou : WirelessPageBase
    {
        protected List<DaoGouEntity> DaoGouEntities=new List<DaoGouEntity>();
        protected string Topic = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            Topic = Request.QueryString["topic"];

            InitData(Topic);
        }

        private void InitData(string topic)
        {
            string path = HttpContext.Current.Server.MapPath("~/config/DaoGouData.xml");
            if (!File.Exists(path)) return;

            XmlDocument xmlDoc = CommonFunction.ReadXmlFromFile(path);
            XmlNodeList nodeList = xmlDoc.SelectNodes("/Root/Group");
            if (nodeList == null || nodeList.Count <= 0) return;

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes == null) continue;

                string currentTopic = node.Attributes["topic"].Value;
                if (currentTopic != topic) continue;

                XmlNodeList itemNodeList = node.SelectNodes("Item");
                if (itemNodeList == null) continue;

                List<DaoGouEntity> list = new List<DaoGouEntity>();

                foreach (XmlNode subXmlNode in itemNodeList)
                {
                    #region 

                    var daoGouEntity = new DaoGouEntity();
                    if (subXmlNode.Attributes != null)
                    {
                        int serialId = ConvertHelper.GetInteger(subXmlNode.Attributes["csid"].Value);
                        daoGouEntity.SerialId = serialId;

                        var serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
                        daoGouEntity.BrandName = serialEntity.Brand.Name;
                        daoGouEntity.BrandId = serialEntity.Brand.MasterBrandId;
                        daoGouEntity.AllSpell = serialEntity.AllSpell;
                        EnumCollection.SerialInfoCard serialInfo =
                            new Car_SerialBll().GetSerialInfoCard(serialId);
                        var serialPrice = serialInfo.CsPriceRange;
                        if (serialPrice.Length == 0)
                            serialPrice = "暂无报价";
                        daoGouEntity.ReferPrice = serialPrice;

                        daoGouEntity.ImageUrl = GetSerialCoverHashImgUrl(serialId);
                    }
                    if (subXmlNode.Attributes != null)
                    {
                        daoGouEntity.DataDescription = subXmlNode.Attributes["value"].Value;
                    }

                    if (subXmlNode.Attributes != null)
                    {
                        daoGouEntity.Description = subXmlNode.Attributes["description"].Value;
                    }
                    if (subXmlNode.Attributes != null)
                    {
                        daoGouEntity.ShowName = subXmlNode.Attributes["name"].Value;
                    }
                    if (subXmlNode.Attributes != null)
                    {
                        daoGouEntity.ChannelId = subXmlNode.Attributes["channelid"].Value;
                    }
                    list.Add(daoGouEntity);

                    #endregion
                }

                if (list.Count > 0)
                {
                    DaoGouEntities.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 获取子品牌的白底封面图的散列域名的Url
        /// </summary>
        /// <returns></returns>
        public string GetSerialCoverHashImgUrl(int serialId)
        {
            string imgUrl = WebConfig.DefaultCarPic;
            Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
            if (urlDic.ContainsKey(serialId))
            {
                int imgId = ConvertHelper.GetInteger(urlDic[serialId].GetAttribute("ImageId"));
                string domainName = "img" + (imgId % 4 + 1).ToString() + ".bitautoimg.com";
                imgUrl = urlDic[serialId].GetAttribute("ImageUrl2").Trim();
                if (imgUrl.Length > 0)
                {
                    // 有新封面
                    imgUrl = string.Format(imgUrl, "6").Replace("image.bitautoimg.com", domainName);
                }
                else
                {
                    imgUrl = urlDic[serialId].GetAttribute("ImageUrl").Trim();
                    // 没有新封面
                    if (imgUrl.Length > 0)
                    {
                        imgUrl = string.Format(imgUrl, "6").Replace("image.bitautoimg.com", domainName);
                    }
                    else
                    {
                        imgUrl = WebConfig.DefaultCarPic;
                    }
                }
            }

            return imgUrl;
        }

    }


}