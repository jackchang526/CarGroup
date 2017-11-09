using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System.Xml;
using BitAuto.CarChannel.BLL;
using System.Text;
using System.Web.UI;

namespace BitAuto.CarChannelAPI.Web.Exhibition
{
    /// <summary>
    /// GetExhibitionLeftTreeJson 的摘要说明
    /// </summary>
    public class GetExhibitionLeftTreeJson : ExhibitionPageBase, IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;

        string tagType = string.Empty;
        int objId = 0;

        string charString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
        private XmlDocument _PavilionMasterXml = new XmlDocument();
        private XmlDocument _AlbumXmlDoc = new XmlDocument();

        private BitAuto.CarChannel.BLL.Exhibition _ExhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();
        private ExhibitionAlbum _AlbumBLL = new ExhibitionAlbum();


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
            //获取参数
            GetPageParam();
            //初始数据
            InitExhibitionData();
            //生成数据
            GenerateData();
        }
        /// <summary>
        /// 生成数据
        /// </summary>
        private void GenerateData()
        {
            List<string> list = new List<string>();
            list.Add(GetPavilionElement());	//按展厅
            list.Add(GetBrandByChar());	//按品牌
            response.Write(string.Format("JsonpCallBack({{{0}}})", string.Join(",", list.ToArray())));
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetPageParam()
        {
            _ExhibitionID = ConvertHelper.GetInteger(request.QueryString["eid"]);
            objId = ConvertHelper.GetInteger(request.QueryString["objid"]);
            tagType = request.QueryString["tagtype"];
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitExhibitionData()
        {
            base.IntiExhibitionByID(_ExhibitionID);
            _ExhibitionXmlDoc = _ExhibitionBLL.GetMasterExhibitionXmlById(_ExhibitionID);
            if (_ExhibitionXmlDoc == null
                || _ExhibitionXmlDoc.SelectSingleNode("root") == null
                || _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
                || _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
            {
                return;
            }
            _PavilionMasterXml = _ExhibitionBLL.GetPavilionMasterXmlById(_ExhibitionID);
            AddMasterBrandAttribute(ref _ExhibitionXmlDoc);
            _AlbumXmlDoc = _AlbumBLL.GetCommonAlbumRelationData(_ExhibitionID);
        }
        /// <summary>
        /// 添加xml所需 数据
        /// </summary>
        /// <param name="xmlDoc"></param>
        private void AddMasterBrandAttribute(ref XmlDocument xmlDoc)
        {
            XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand");
            if (xNodeList == null || xNodeList.Count < 1)
            {
                return;
            }
            foreach (XmlElement xEleme in xNodeList)
            {
                int masterId = ConvertHelper.GetInteger(xEleme.GetAttribute("ID"));
                //xEleme.SetAttribute("LogoUrl", "http://img1.bitauto.com/bt/car/default/images/carimage/autoshanghai2013/b_" + xEleme.GetAttribute("ID") + "_30.png");
                xEleme.SetAttribute("LogoUrl", "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + xEleme.GetAttribute("ID") + "_55.png");
            }
        }

        /// <summary>
        /// 获取展馆数据
        /// </summary>
        /// <returns></returns>
        private string GetPavilionElement()
        {
            Dictionary<int, BitAuto.CarChannel.Model.Pavilion> pavilionList = _ExhibitionBLL.GetPavilionListByExhibitionId(_ExhibitionID);
            List<string> listPavilion = new List<string>();
            List<string> listMasterPavilion = new List<string>();
            int isNode = 1;
            foreach (KeyValuePair<int, BitAuto.CarChannel.Model.Pavilion> entity in pavilionList)
            {
                string pavilionMaster = GetMasterByPavilionId(entity.Value.ID, out isNode);
                listPavilion.Add(string.Format("{0}:\"{1}\"", entity.Value.ID.ToString(), entity.Value.Name));
                if (isNode == 1)
                    listMasterPavilion.Add(string.Format("{0}:[{1}]", entity.Value.ID, pavilionMaster));
            }
            string pavilionStr = string.Format("pavilion:{{{0}}}", string.Join(",", listPavilion.ToArray()));
            string masterStr = string.Format("pbrand:{{{0}}}", string.Join(",", listMasterPavilion.ToArray()));
            return string.Format("{0},{1}", pavilionStr, masterStr);
        }
        /// <summary>
        /// 获取主品牌数据，根据展厅ID
        /// </summary>
        /// <param name="pavilionId">展厅ID</param>
        /// <param name="isNode">是否有节点 0：代码没有节点</param>
        /// <returns></returns>
        private string GetMasterByPavilionId(int pavilionId, out int isNode)
        {
            XmlNodeList nodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand/Brand[@PavilionId=" + pavilionId + "]");
            List<string> list = new List<string>();
            isNode = 1;
            if (nodeList.Count <= 0)
                isNode = 0;
            //Dictionary<int, XmlNode> masterDict = new Dictionary<int, XmlNode>();
            //foreach (XmlNode node in nodeList)
            //{ 
            //    XmlNode masterNode=node.ParentNode;
            //    if (masterNode == null)
            //        continue;
            //    int masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
            //    if (!masterDict.ContainsKey(masterId))
            //        masterDict.Add(masterId, masterNode);
            //}
            List<int> tempList = new List<int>();
            foreach (XmlNode node in nodeList)
            {
                XmlNode masterNode = node.ParentNode;
                if (masterNode == null)
                    continue;
                int masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
                if (tempList.Contains(masterId)) //排重主品牌
                    continue;
                tempList.Add(masterId);
                int cur = 0;
                if (masterId == objId
                    && masterNode.Attributes["PavilionId"] != null
                    && pavilionId == ConvertHelper.GetInteger(masterNode.Attributes["PavilionId"].Value)) //增加当前主品牌表示
                    cur = 1;
                list.Add(string.Format("{{id:{0},name:\"{1}\",allspell:\"{2}\",logo:\"{3}\",url:\"{4}\",cur:{5}}}",
                    masterNode.Attributes["ID"].Value,
                    masterNode.Attributes["Name"].Value,
                    masterNode.Attributes["AllSpell"].Value,
                    masterNode.Attributes["LogoUrl"].Value,
                    GetUrl(tagType, masterNode),
                    cur));
            }
            //补充一个品牌在多个展馆的情况
            if (_PavilionMasterXml != null && _PavilionMasterXml.HasChildNodes)
            {
                var pavilion = _PavilionMasterXml.SelectSingleNode("/root/Pavilion[@ID=" + pavilionId + "]");
                if (pavilion != null && pavilion.HasChildNodes)
                {
                    var nodePavilionList2 = pavilion.SelectNodes("MasterBrand");
                    foreach (XmlNode masterNode in nodePavilionList2)
                    {
                        var masterId = ConvertHelper.GetInteger(masterNode.Attributes["ID"].Value);
                        if (tempList.Contains(masterId)) //排重主品牌
                            continue;
                        tempList.Add(masterId);
                        int cur = 0;
                        if (masterId == objId) //增加当前主品牌表示
                            cur = 1;
                        list.Add(string.Format("{{id:{0},name:\"{1}\",allspell:\"{2}\",logo:\"{3}\",url:\"{4}\",cur:{5}}}",
                            masterNode.Attributes["ID"].Value,
                            masterNode.Attributes["Name"].Value,
                            masterNode.Attributes["AllSpell"].Value,
                            "http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_" + masterNode.Attributes["ID"].Value + "_55.png",
                            GetUrl(tagType, masterNode),
                            cur));
                    }
                }
            }
            return string.Join(",", list.ToArray());
        }

        /// <summary>
        /// 按品牌获取树形数据
        /// </summary>
        /// <returns></returns>
        private string GetBrandByChar()
        {
            List<string> listChar = new List<string>();
            List<string> listMaster = new List<string>();
            int isNode = 1;
            for (int i = 0; i < charString.Length; i++)
            {
                char curChar = charString[i];
                string charMaster = GetMasterData(curChar, out isNode);
                if (isNode == 1)
                    listMaster.Add(string.Format("{0}:[{1}]", curChar, charMaster));
                listChar.Add(string.Format("{0}:{1}", curChar, isNode));
            }
            string charStr = string.Format("char:{{{0}}}", string.Join(",", listChar.ToArray()));
            string masterStr = string.Format("brand:{{{0}}}", string.Join(",", listMaster.ToArray()));
            return string.Format("{0},{1}", charStr, masterStr);
        }
        /// <summary>
        /// 获取主品牌数据 根据字母
        /// </summary>
        /// <param name="curChar">根据字母</param>
        /// <param name="isNode">是否有节点 0:代表没有</param>
        /// <returns></returns>
        private string GetMasterData(char curChar, out int isNode)
        {
            XmlNodeList nodeList = _ExhibitionXmlDoc.SelectNodes("root/MasterBrand[@Firstchar='" + curChar + "']");
            List<string> list = new List<string>();
            isNode = 1;
            if (nodeList.Count <= 0)
                isNode = 0;
            foreach (XmlNode node in nodeList)
            {
                int masterId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
                int cur = 0;
                if (masterId == objId) //增加当前主品牌表示
                    cur = 1;
                list.Add(string.Format("{{id:{0},name:\"{1}\",allspell:\"{2}\",logo:\"{3}\",url:\"{4}\",cur:{5}}}",
                    node.Attributes["ID"].Value,
                    node.Attributes["Name"].Value,
                    node.Attributes["AllSpell"].Value,
                    node.Attributes["LogoUrl"].Value,
                    GetUrl(tagType, node),
                    cur));
            }
            return string.Join(",", list.ToArray());
        }
        /// <summary>
        /// 获取各个业务url规则
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        private string GetUrl(string tagType, XmlNode node)
        {
            string result = "";
            if (string.IsNullOrEmpty(tagType))
                return result;
            if (_DicExhibitionBaseInfo.ContainsKey(_ExhibitionID))
            {
                ExhibitionBaseInfo exhibitionConfig = _DicExhibitionBaseInfo[_ExhibitionID];
                switch (tagType.ToLower())
                {
                    case "chexing":
                        result = string.Format(exhibitionConfig.UrlFormat, node.Attributes["AllSpell"].Value);
                        break;
                    case "cmsmod":
                        result = string.Format(exhibitionConfig.CMSModelUrl, node.Attributes["ID"].Value);
                        break;
                    case "newcar":
                        result = string.Format(exhibitionConfig.NewCarUrl, node.Attributes["AllSpell"].Value);
                        break;
                    default: result = string.Format("/{0}/", node.Attributes["AllSpell"].Value); break;
                }
            }
            return result;
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