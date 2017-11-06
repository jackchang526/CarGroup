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
    /// GetExhibitionMasterBrand 的摘要说明
    /// 没有缓存，待添加
    /// </summary>
    public class GetExhibitionMasterBrand : IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;
        private bool _IsWithFirstCode = false;
        private int _ExhibitionID = 0;
        private string _callback = string.Empty;
        private string _type = string.Empty;

        string charString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
        private BitAuto.CarChannel.BLL.Exhibition _ExhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
        private XmlDocument _AlbumXmlDoc = new XmlDocument();
        private ExhibitionAlbum _AlbumBLL = new ExhibitionAlbum();

        // 车系封面图字典
        //private Dictionary<int, XmlElement> urlDic = new Dictionary<int, XmlElement>();
        // 当前车展的车系标签
        private Dictionary<int, string> serialAttribute = new Dictionary<int, string>();

        // 图片域地址
        private string imageDomain = string.Empty;
        //private string targetUrlBase = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            //OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
            //{
            //    Duration = 60 * 10,
            //    Location = OutputCacheLocation.Any,
            //    VaryByParam = "*"
            //});
            //page.ProcessRequest(HttpContext.Current);

            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            request = context.Request;
            GetPageParam();
            if (this._type == "withbrand")
            {
                InitExhibitionDataForWithBrand(); 
                GenerateDataWithCodeAndSerial();
            }
            else
            {
                InitExhibitionData();
                if (this._IsWithFirstCode)
                {
                    GenerateDataWithCode();
                }
                else
                {
                    GenerateData();
                }
            }
        }

        /// <summary> 
        /// 获取参数
        /// </summary>
        private void GetPageParam()
        {
            //如果展会ID不等于空 
            this._ExhibitionID = ConvertHelper.GetInteger(request.QueryString["eid"]);
            this._IsWithFirstCode = ConvertHelper.GetBoolean(request.QueryString["fc"]);
            this._callback = ConvertHelper.GetString(request.QueryString["callback"]);
            this._type = ConvertHelper.GetString(request.QueryString["type"]);
        }
         
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitExhibitionData()
        {
            this._ExhibitionXmlDoc = _ExhibitionBLL.GetMasterExhibitionXmlById(_ExhibitionID);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitExhibitionDataForWithBrand()
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
            //AddMasterBrandAttribute();
            //this.urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
            //InitSerialAttribute();
            this._AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(this._ExhibitionID);

            // 图片域地址 
            XmlNode albumNode = _AlbumXmlDoc.SelectSingleNode("/Data");
            if (albumNode != null && albumNode.Attributes["ImageDomain"] != null)
            {
                imageDomain = albumNode.Attributes["ImageDomain"].Value;
                //targetUrlBase = albumNode.Attributes["TargetUrlBase"].Value;
            }
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

        /// <summary>
        /// 添加xml所需 数据
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void AddMasterBrandAttribute()
        {
            XmlNodeList xNodeList = this._ExhibitionXmlDoc.SelectNodes("root/MasterBrand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return;
            }
            foreach (XmlElement xEleme in xNodeList)
            {
                int masterId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
                xEleme.SetAttribute("LogoUrl", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + xEleme.GetAttribute("ID") + "_55.png");
            }
        }

        #region 主品牌
        /// <summary>
        /// 生成没有首字母的主品牌数据
        /// </summary>
        private void GenerateData()
        {
            string result = "";
            try
            {
                int isNode = 0;
                result = ResultUtil.SuccessResult(GetMasterData('-', out isNode));
            }
            catch (Exception ex)
            {
                result = ResultUtil.ErrorResult(ex.GetHashCode(), ex.Message, "");
            }
            response.Write(string.Format("{1}({0})", result, this._callback));
        }

        /// <summary>
        /// 生成有首字母的主品牌数据
        /// </summary>
        private void GenerateDataWithCode()
        {
            string result = "";
            try
            {
                result = GetBrandByChar();
            }
            catch (Exception ex)
            {
                //result = ResultUtil.ErrorResult(ex.GetHashCode(), ex.Message, "");
                result = ex.Message;
            }
            response.Write(string.Format("{1}({0})", result, _callback));
        }

        /// <summary>
        /// 获取主品牌数据 根据字母
        /// </summary>
        /// <param name="curChar">根据字母</param>
        /// <param name="isNode">是否有节点 0:代表没有</param>
        /// <returns></returns>
        private string GetMasterData(char curChar, out int isNode)
        {
            XmlNodeList nodeList;
            if (curChar == '-')
                nodeList = this._ExhibitionXmlDoc.SelectNodes("root/MasterBrand");
            else
                nodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@Firstchar='" + curChar + "']");

            isNode = 1;
            if (nodeList.Count <= 0)
                isNode = 0;

            List<string> list = new List<string>();
            foreach (XmlNode node in nodeList)
            {
                var sNodes = node.SelectNodes("Brand/Serial[@NC=1]");
                if (sNodes != null && sNodes.Count > 0)
                {
                    int masterId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
                    list.Add(string.Format("{{\"MasterId\":{0},\"MasterName\":\"{1}\",\"AllSpell\":\"{2}\"}}",
                        node.Attributes["ID"].Value,
                        node.Attributes["Name"].Value,
                        node.Attributes["AllSpell"].Value
                        ));
                }
            }
            if (list.Count > 0)
            {
                return string.Format("[{0}]", string.Join(",", list.ToArray()));
            }
            else
            {
                isNode = 0;
                return "[]";
            }
        }

        /// <summary>
        /// 按品牌获取树形数据
        /// </summary>
        /// <returns></returns>
        private string GetBrandByChar()
        {
            List<string> listMaster = new List<string>();
            StringBuilder codeStr = new StringBuilder();
            int isNode = 1;
            for (int i = 0; i < charString.Length; i++)
            {
                char curChar = charString[i];
                string charMaster = GetMasterData(curChar, out isNode);
                if (isNode == 1)
                {
                    listMaster.Add(string.Format("\"{0}\":{1}", curChar, charMaster));
                    codeStr.AppendFormat("\"{0}\": 1,", curChar);
                }
                else
                {
                    codeStr.AppendFormat("\"{0}\": 0,", curChar);
                }
            }
            return string.Format("{{\"CharList\":{{{0}}}, \"MsterList\": {{{1}}}}}", codeStr.ToString().Substring(0, codeStr.Length - 1), string.Join(",", listMaster.ToArray()));
        }

        #endregion

        #region 主品牌带品牌和车系信息
        /// <summary>
        /// 生成带首字母且嵌套品牌和车系的数据
        /// </summary>
        private void GenerateDataWithCodeAndSerial()
        {
            string result = "";
            try
            {
                result = GetDataWithCodeAndSerial();
            }
            catch (Exception ex)
            {
                //result = ResultUtil.ErrorResult(ex.GetHashCode(), ex.Message, "");
                result = ex.Message;
            }
            response.Write(string.Format("{1}({0})", result, _callback));
        }

        /// <summary>
        /// 生成带首字母的主品牌到车系的数据
        /// </summary>
        /// <returns></returns>
        private string GetDataWithCodeAndSerial()
        {
            List<string> listMaster = new List<string>();
            StringBuilder codeStr = new StringBuilder();
            int isNode = 1;
            for (int i = 0; i < charString.Length; i++)
            {
                char curChar = charString[i];
                string charMaster = GetMasterDataAndSerial(curChar, out isNode);
                if (isNode == 1)
                {
                    listMaster.Add(string.Format("\"{0}\":{1}", curChar, charMaster));
                    codeStr.AppendFormat("\"{0}\": 1,", curChar);
                }
                else
                {
                    codeStr.AppendFormat("\"{0}\": 0,", curChar);
                }
            }
            return string.Format("{{\"CharList\":{{{0}}}, \"MsterList\": {{{1}}}}}", codeStr.ToString().Substring(0, codeStr.Length - 1), string.Join(",", listMaster.ToArray()));
        }

        /// <summary>
        /// 获取主品牌数据 根据字母
        /// </summary>
        /// <param name="curChar">根据字母</param>
        /// <param name="isNode">是否有节点 0:代表没有</param>
        /// <returns></returns>
        private string GetMasterDataAndSerial(char curChar, out int isNode)
        {
            XmlNodeList nodeList;
            if (curChar == '-')
                nodeList = this._ExhibitionXmlDoc.SelectNodes("root/MasterBrand");
            else
                nodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@Firstchar='" + curChar + "']");

            isNode = 1;
            if (nodeList.Count <= 0)
                isNode = 0;

            List<string> list = new List<string>();
            foreach (XmlNode node in nodeList)
            {
                int masterId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
                string brandData = GetBrandData(node);
                if (string.IsNullOrEmpty(brandData))
                {
                    continue;
                }
                list.Add(string.Format("{{\"MasterId\":{0},\"MasterName\":\"{1}\",\"AllSpell\":\"{2}\",\"Child\":{3}}}",
                    node.Attributes["ID"].Value,
                    node.Attributes["Name"].Value,
                    node.Attributes["AllSpell"].Value,
                    brandData
                    ));
            }
            if (list.Count > 0)
            {
                return string.Format("[{0}]", string.Join(",", list.ToArray()));
            }
            else
            {
                isNode = 0;
                return "[]";
            }
        }

        public string GetBrandData(XmlNode brandNode)
        {
            XmlNodeList xNodeList = brandNode.SelectNodes("Brand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return "";
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
                    //string targetUrl = string.Empty;
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

                        if (nodeAlbumSerial != null && nodeAlbumSerial.Attributes["Count"] != null && nodeAlbumSerial.Attributes["Count"].Value != "0")
                        { }
                        else
                        {
                            continue;
                        }

                        if (nodeAlbumSerial.Attributes["ImageUrl"] != null && !string.IsNullOrEmpty(nodeAlbumSerial.Attributes["ImageUrl"].Value))
                            imageUrl = imageDomain + nodeAlbumSerial.Attributes["ImageUrl"].Value;

                        //if (!string.IsNullOrEmpty(targetUrlBase) && nodeAlbumSerial.Attributes["TargetUrl"] != null && !string.IsNullOrEmpty(nodeAlbumSerial.Attributes["TargetUrl"].Value))
                        //    targetUrl = targetUrlBase + nodeAlbumSerial.Attributes["TargetUrl"].Value;
                    }
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        continue;
                        //imageUrl = GetSerialCoverImage(serialId);
                    }

                    //if (string.IsNullOrEmpty(targetUrl))
                    //{
                    //    continue;
                    //    //targetUrl = string.Format("http://car.m.yiche.com/{0}/", allspell);//GetSerialCoverImage(serialId);
                    //}
                     
                    //xEleme.SetAttribute("LogoUrl", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + xEleme.GetAttribute("ID") + "_55.png");
                    //serialList.Add(string.Format("{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"Allspell\":\"{2}\",\"ImageUrl\":\"{3}\",\"Tag\":\"{4}\",\"CsSaleState\":\"{5}\",\"targetUrl\":\"{6}\"}}",
                    //    serialId,
                    //    name,
                    //    allspell,
                    //    imageUrl,
                    //    tag,
                    //    "",
                    //    targetUrl));
                    serialList.Add(string.Format("{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"Allspell\":\"{2}\",\"ImageUrl\":\"{3}\"}}",
                        serialId,
                        name,
                        allspell,
                        imageUrl));
                    //string.Join(";", tags), ""));
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
            if (brnadList.Count > 0)
            {
                return string.Format("[{0}]", string.Join(",", brnadList));
            }
            else
            {
                return "";
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

        #endregion

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