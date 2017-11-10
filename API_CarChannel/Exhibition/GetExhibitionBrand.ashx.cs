using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.Exhibition
{
    /// <summary>
    /// GetExhibitionBrand 的摘要说明
    /// </summary>
    public class GetExhibitionBrand : IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;
        private int _ExhibitionID = 0;
        private int _MasterBrandID = 0;
        private bool _IsGroupByBrand = false;
        private string _callback = string.Empty;

        private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
        private BitAuto.CarChannel.BLL.Exhibition _ExhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
        private XmlDocument _AlbumXmlDoc = new XmlDocument();
        private ExhibitionAlbum _AlbumBLL = new ExhibitionAlbum(); 

        // 当前车展的车系标签
        private Dictionary<int, string> serialAttribute = new Dictionary<int, string>();

        public void ProcessRequest(HttpContext context)
        {
            OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
            {
                Duration = 60 * 10,
                Location = OutputCacheLocation.Any,
                VaryByParam = "*"
            });
            page.ProcessRequest(HttpContext.Current);

            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            request = context.Request;
            GetPageParam();
            InitExhibitionData();
            GenerateData();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetPageParam()
        {
            //如果展会ID不等于空 
            this._ExhibitionID = ConvertHelper.GetInteger(request.QueryString["eid"]);
            this._MasterBrandID = ConvertHelper.GetInteger(request.QueryString["mbid"]);
            this._IsGroupByBrand = ConvertHelper.GetBoolean(request.QueryString["gb"]);
            this._callback = ConvertHelper.GetString(request.QueryString["callback"]);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitExhibitionData()
        {
            // 缓存10分钟
            this._ExhibitionXmlDoc = _ExhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID, 10); 
            if (_ExhibitionXmlDoc == null
                || _ExhibitionXmlDoc.SelectSingleNode("root") == null
                || _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
                || _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
            {
                return;
            } 
            InitSerialAttribute();
            this._AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(this._ExhibitionID);
        }

        private void InitSerialAttribute()
        {
            DataSet ds = BitAuto.CarChannel.BLL.Exhibition.GetAllAttributeByExhibitionId(_ExhibitionID);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int Id = ConvertHelper.GetInteger(ds.Tables[0].Rows[i]["ID"].ToString().Trim());
                    if (Id > 0 && !serialAttribute.ContainsKey(Id))
                    {
                        serialAttribute.Add(Id, ds.Tables[0].Rows[i]["Name"].ToString().Trim());
                    }
                }
            }
        }

        /*
        /// <summary>
        /// 获取车系封面图
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public string GetSerialCoverImage(int serialId)
        {
            string imgUrl = "";
            if (urlDic.ContainsKey(serialId))
            {
                // modified by chengl Jan.4.2010
                if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
                {
                    // 有新封面
                    imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), "1");
                }
                else
                {
                    // 没有新封面
                    if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
                    {
                        imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), "1");
                    }
                    else
                    {
                        imgUrl = WebConfig.DefaultCarPic;
                    }
                }
                //int imgId = Convert.ToInt32(urlDic[serialId].GetAttribute("ImageId"));
                //imgUrl = urlDic[serialId].GetAttribute("ImageUrl");
                //if (imgId == 0 || imgUrl == "")
                //    imgUrl = WebConfig.DefaultCarPic;
                //else
                //    imgUrl = new OldPageBase().GetPublishImage(5, imgUrl, imgId);
            }
            else
                imgUrl = WebConfig.DefaultCarPic;
            return imgUrl;
        }
        */

        public string GetXMLWithBrand()
        {
            XmlNodeList xNodeList = this._ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@ID=" + this._MasterBrandID + "]/Brand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return "";
            }
            // 图片域地址
            string imageDomain = string.Empty;
            string targetUrlBase = string.Empty;
            XmlNode albumNode = _AlbumXmlDoc.SelectSingleNode("/Data");
            if (albumNode != null && albumNode.Attributes["ImageDomain"] != null)
            {
                imageDomain = albumNode.Attributes["ImageDomain"].Value;
                //targetUrlBase = albumNode.Attributes["TargetUrlBase"].Value;
            }

            List<string> brnadList = new List<string>();

            foreach (XmlElement xEleme in xNodeList)
            {
                int brandId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
                //var ss = "{\"ID\":{0},\"Name\":\"{1}\",\"AllSpell\":\"{2}\",\"Serial\":{3}}";
                List<string> serialList = new List<string>();
                XmlNodeList sNodeList = xEleme.SelectNodes("Serial[@NC=1]");
                if (sNodeList == null || sNodeList.Count < 1)
                {
                    break;
                }
                var sList = SortSerial(sNodeList);
                foreach (XmlElement sEleme in sList)
                {
                    if (!sEleme.HasAttribute("ID"))
                        continue;
                    if (!sEleme.HasAttribute("NC"))
                        continue;
                    int serialId = ConvertHelper.GetInteger(sEleme.Attributes["ID"].Value);
                    string imageUrl = string.Empty;
                    string name = string.Empty; 
                    string allspell = "";

                    if (!sEleme.HasAttribute("Name"))
                    {
                        continue;
                    }
                    else
                    {
                        name = sEleme.Attributes["Name"].Value;
                    }
                    if (!sEleme.HasAttribute("AllSpell"))
                    {
                        continue;
                    }
                    else
                    {
                        allspell = sEleme.Attributes["AllSpell"].Value;
                    }
                    if (sEleme.Attributes["NC"] != null && !string.IsNullOrEmpty(imageDomain))
                    {
                        var nodeAlbumSerial = _AlbumXmlDoc.SelectSingleNode("/Data/NewCar/Master/Serial[@Id=" + serialId + "]");
                        if (nodeAlbumSerial.Attributes["ImageUrl"] != null && !string.IsNullOrEmpty(nodeAlbumSerial.Attributes["ImageUrl"].Value))
                            imageUrl = imageDomain + nodeAlbumSerial.Attributes["ImageUrl"].Value;
                         
                    } 
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrl = WebConfig.DefaultCarPic;
                        //imageUrl = GetSerialCoverImage(serialId);
                    }
                      
                    // 车系标签
                    //List<string> tags = new List<string>();
                    string tag = "";
                    if (sEleme.HasChildNodes)
                    {
                        XmlNodeList attrList = sEleme.SelectNodes("Attribute");
                        if (attrList != null && attrList.Count > 0)
                        {
                            foreach (XmlElement attrItem in attrList)
                            {
                                if (attrItem.HasAttribute("ID"))
                                {
                                    int tagId = ConvertHelper.GetInteger(attrItem.Attributes["ID"].Value);
                                    if (serialAttribute.ContainsKey(tagId))
                                    {
                                        //tags.Add(serialAttribute[tagId]);
                                        tag = serialAttribute[tagId];
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    //xEleme.SetAttribute("LogoUrl", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + xEleme.GetAttribute("ID") + "_55.png");
                    serialList.Add(string.Format("{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"Allspell\":\"{2}\",\"ImageUrl\":\"{3}\",\"Price\":\"{4}\"}}",
                        serialId,
                        name,
                        allspell,
                        imageUrl,
                        tag)); 
                }
                if (serialList.Count <= 0)
                {
                    //brnadList.Add(string.Format(""));
                }
                else
                {
                    brnadList.Add(string.Format("{{\"BrandId\":{0},\"BrandName\":\"{1}\",\"BrandAllspell\":\"{2}\",\"Child\":[{3}]}}",
                        brandId,
                        xEleme.GetAttribute("Name"),
                        xEleme.GetAttribute("allSpell"),
                        string.Join(",", serialList)
                        ));
                }
            }
            string data = string.Format("[{0}]", string.Join(",", brnadList));
            return data;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public string GetXML()
        {
            XmlNodeList xNodeList = this._ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@ID=" + this._MasterBrandID + "]/Brand/Serial[@NC=1]");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return "";
            }
            // 图片域地址
            string imageDomain = string.Empty;
            XmlNode albumNode = _AlbumXmlDoc.SelectSingleNode("/Data");
            if (albumNode != null && albumNode.Attributes["ImageDomain"] != null)
            {
                imageDomain = albumNode.Attributes["ImageDomain"].Value;
            }
            var nodeList = SortSerial(xNodeList);
            List<string> serialList = new List<string>();
            foreach (XmlElement sEleme in nodeList)
            { 
                if (!sEleme.HasAttribute("ID"))
                    continue;
                if (!sEleme.HasAttribute("NC"))
                    continue;
                int serialId = ConvertHelper.GetInteger(sEleme.Attributes["ID"].Value);
                string imageUrl = string.Empty;
                string name = string.Empty;

                if (!sEleme.HasAttribute("Name"))
                {
                    continue;
                }
                else
                {
                    name = sEleme.Attributes["Name"].Value;
                }
                string allspell = "";
                if (!sEleme.HasAttribute("AllSpell"))
                {
                    continue;
                }
                else
                {
                    allspell = sEleme.Attributes["AllSpell"].Value;
                }

                if (!string.IsNullOrEmpty(imageDomain))
                {
                    var nodeAlbumSerial = _AlbumXmlDoc.SelectSingleNode("/Data/NewCar/Master/Serial[@Id=" + serialId + "]");
                    if (nodeAlbumSerial.Attributes["ImageUrl"] != null && !string.IsNullOrEmpty(nodeAlbumSerial.Attributes["ImageUrl"].Value))
                        imageUrl = imageDomain + nodeAlbumSerial.Attributes["ImageUrl"].Value;
                }
                else
                {
                    continue;
                }
                if (string.IsNullOrEmpty(imageUrl))
                {
                    continue; 
                }

                serialList.Add(string.Format("{{\"ID\":{0},\"Name\":\"{1}\",\"AllSpell\":\"{2}\",\"Image\":\"{3}\"}}",
                    serialId,
                    name,
                    allspell,
                    imageUrl));
                if (serialList.Count >= 4)
                    break;
            }
            string masterName = nodeList.FirstOrDefault().ParentNode.ParentNode.Attributes["Name"].Value;
            return string.Format("{{\"MasterId\":{1},\"MasterName\":\"{2}\",\"Serial\":[{0}]}}", string.Join(",", serialList), this._MasterBrandID, masterName);
        }

        public void GenerateData()
        {
            string result = "";
            try
            {
                if (this._IsGroupByBrand)
                {
                    result = GetXMLWithBrand();
                }
                else
                {
                    result = GetXML();
                }
            }
            catch (Exception ex)
            {
                result = ResultUtil.ErrorResult(ex.GetHashCode(), ex.Message, "");
            }
            response.Write(ResultUtil.CallBackResult(this._callback, result));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 子品牌排序
        /// </summary>
        /// <param name="nodeSerialList"></param>
        /// <returns></returns>
        private List<XmlNode> SortSerial(XmlNodeList nodeSerialList)
        {
            List<XmlNode> listSerial = new List<XmlNode>();
            foreach (XmlNode tempNode in nodeSerialList)
                listSerial.Add(tempNode);
            //排序 新车排前 新车有图在前无图在后 旧车排后
            listSerial.Sort((p1, p2) =>
            {
                if (p1.Attributes["NC"] == null)
                {
                    if (p2.Attributes["NC"] == null)
                        return 0;
                    else
                        return 1;
                }
                else
                {
                    if (p2.Attributes["NC"] == null)
                        return -1;
                    else
                    {
                        string serialImgurl1 = string.Empty;
                        string serialImgurl2 = string.Empty;
                        XmlNode nodeAlbumSerial1 = _AlbumXmlDoc.SelectSingleNode("/Data/NewCar/Master/Serial[@Id=" + p1.Attributes["ID"].Value + "]");
                        if (nodeAlbumSerial1 != null)
                        {
                            serialImgurl1 = nodeAlbumSerial1.Attributes["ImageUrl"].Value;
                        }
                        XmlNode nodeAlbumSerial2 = _AlbumXmlDoc.SelectSingleNode("/Data/NewCar/Master/Serial[@Id=" + p2.Attributes["ID"].Value + "]");
                        if (nodeAlbumSerial2 != null)
                        {
                            serialImgurl2 = nodeAlbumSerial2.Attributes["ImageUrl"].Value;
                        }
                        if (string.IsNullOrEmpty(serialImgurl1))
                        {
                            if (string.IsNullOrEmpty(serialImgurl2))
                                return CompareXmlNodeUpdateTime(p1, p2);
                            else
                                return 1;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(serialImgurl2))
                                return CompareXmlNodeUpdateTime(p1, p2);
                            else
                                return -1;
                        }
                    }
                }
            });
            return listSerial;
        }

        public static int CompareXmlNodeUpdateTime(XmlNode xn1, XmlNode xn2)
        {
            int ret = 0;
            DateTime dt1 = new DateTime(1900, 1, 1);
            DateTime dt2 = new DateTime(1900, 1, 1);
            if (xn1.Attributes["UpdateTime"] != null)
            {
                dt1 = Convert.ToDateTime(xn1.Attributes["UpdateTime"].Value);
            }
            if (xn2.Attributes["UpdateTime"] != null)
            {
                dt2 = Convert.ToDateTime(xn2.Attributes["UpdateTime"].Value);
            }
            if (dt1 > dt2)
                ret = -1;
            else if (dt1 < dt2)
                ret = 1;
            return ret;
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