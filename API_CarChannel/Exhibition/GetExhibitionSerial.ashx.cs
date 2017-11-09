using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace BitAuto.CarChannelAPI.Web.Exhibition
{
    /// <summary>
    /// GetExhibitionSerial 的摘要说明
    /// </summary>
    public class GetExhibitionSerial : IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;
        private int _ExhibitionID = 0;
        private int _SerialID = 0;
        private string _callback = string.Empty;

        private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
        private Dictionary<int, CarChannel.Model.Pavilion> _PavilionList = new Dictionary<int, CarChannel.Model.Pavilion>();
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
            if (!string.IsNullOrEmpty(request.QueryString["eid"]))
            {
                this._ExhibitionID = ConvertHelper.GetInteger(request.QueryString["eid"]);
            }
            if (!string.IsNullOrEmpty(request.QueryString["sid"]))
            {
                this._SerialID = ConvertHelper.GetInteger(request.QueryString["sid"]);
            }
            this._callback = ConvertHelper.GetString(request.QueryString["callback"]);
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitExhibitionData()
        {
            this._ExhibitionXmlDoc = _ExhibitionBLL.GetExhibitionXmlById(_ExhibitionID);
            if (_ExhibitionXmlDoc == null
                || _ExhibitionXmlDoc.SelectSingleNode("root") == null
                || _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
                || _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
            {
                return;
            }
            _PavilionList = _ExhibitionBLL.GetPavilionListByExhibitionId(_ExhibitionID);
            this._AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(this._ExhibitionID);
        }

        /// <summary>
        /// 初始化当前车展的车系标签
        /// </summary>
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
        /// 生成数据
        /// </summary>
        public void GenerateData()
        {
            string result = "";
            try
            {
                string json = GetJson();
                if (string.IsNullOrEmpty(json))
                {
                    result = ResultUtil.ErrorResult(-1, "没有数据", json);
                }
                else
                {
                    result = ResultUtil.SuccessResult(json);
                }
            }
            catch (Exception ex)
            {
                result = ResultUtil.ErrorResult(ex.GetHashCode(), ex.Message, "");
            }
            response.Write(string.Format("{1}({0})", result, this._callback));
        }

        /// <summary>
        /// 获取json
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            XmlNode Node = this._ExhibitionXmlDoc.SelectSingleNode("root/MasterBrand/Brand/Serial[@ID=" + this._SerialID + "]");
            if (Node == null || Node.Attributes.Count < 1)
            {
                return "";
            }
            // 图片域地址
            string imageDomain = string.Empty;
            string targetUrlBase = string.Empty;
            XmlNode albumNode = _AlbumXmlDoc.SelectSingleNode("/Data");
            if (albumNode != null)
            {
                imageDomain = albumNode.Attributes["ImageDomain"].Value;
                targetUrlBase = albumNode.Attributes["TargetUrlBase"].Value;
            }

            //string serialName = Node.Attributes["Name"].Value; 
            string serialName = new Car_SerialBll().GetSerialInfoEntity(this._SerialID).Cs_ShowName;
            int PavilionId = 0;
            if (Node.ParentNode.ParentNode.Attributes["PavilionId"] != null)
            {
                PavilionId = ConvertHelper.GetInteger(Node.ParentNode.ParentNode.Attributes["PavilionId"].Value);
            }
            string PavilionName = "";
            if (this._PavilionList.ContainsKey(PavilionId))
            {
                PavilionName = this._PavilionList[PavilionId].Name;
            }
            string masterId = Node.ParentNode.ParentNode.Attributes.GetNamedItem("ID").Value;
            int albumCount = 0;  // 图集数量
            string coverImage = string.Empty;  // 封面图
            string targetUrl = string.Empty;  // 跳转路径
            if (Node.Attributes["NC"] != null)
            {
                var nodeAlbumSerial = _AlbumXmlDoc.SelectSingleNode("/Data/NewCar/Master/Serial[@Id=" + this._SerialID + "]");
                albumCount = ConvertHelper.GetInteger(nodeAlbumSerial.Attributes["Count"].Value);
                coverImage = imageDomain + nodeAlbumSerial.Attributes["ImageUrl"].Value;
                targetUrl = targetUrlBase + nodeAlbumSerial.Attributes["TargetUrl"].Value;
            }
            else
            {
                return "";
            }
            // 车系标签
            //List<string> tags = new List<string>();
            string tag = "";
            if (Node.HasChildNodes)
            {
                // 获取全部tag
                InitSerialAttribute();
                XmlNodeList attrList = Node.SelectNodes("Attribute");
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
            string masterBrandLogo = string.Format("http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_{0}_55.png", masterId);
            return string.Format("{{\"ID\":{0},\"Name\":\"{1}\",\"CoverImage\":\"{2}\",\"AlbumCount\":{3},\"MasterBrandLogo\":\"{4}\",\"PavilionName\":\"{5}\",\"TargetUrl\":\"{6}\",\"Tag\":\"{7}\"}}", this._SerialID, serialName, coverImage, albumCount, masterBrandLogo, PavilionName, targetUrl, tag);
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