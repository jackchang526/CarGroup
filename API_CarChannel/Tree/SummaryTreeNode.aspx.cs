using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.CarTreeData;

namespace BitAuto.CarChannelAPI.Web.Tree
{
    public partial class SummaryTreeNode : PageBase
    {
        private const string TreeDataKey = "chexing";
        protected int _masterId;
        private DataNode masterDataNode;
        protected List<string> htmlList;					//HTML代码

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageCache(60);

            Response.ContentType = "application/x-javascript";

            _masterId = ConvertHelper.GetInteger(Request.QueryString["masterid"]);
            if (_masterId <= 0)
                return;
            masterDataNode = GetDataNode();
            if (masterDataNode == null)
                return;

            htmlList = new List<string>();

			htmlList.Add(string.Format("<a href=\"#\" ap=\"{0}\" onclick=\"expandMaster(this, {1});return false;\" class=\"mainBrand current current_unfold\" target=\"_blank\"><big>{2}</big> <span>({3})</span></a>"
                            , masterDataNode.AllSpell, masterDataNode.Id, masterDataNode.Name, masterDataNode.Count.ToString()));

            MakeTreeHtmlByMasterbrand(masterDataNode);
        }
        /// <summary>
        /// 按主品牌生成HTML
        /// </summary>
        /// <param name="dataNode"></param>
        private void MakeTreeHtmlByMasterbrand(DataNode dataNode)
        {
            htmlList.Add("<ul>");
            //每个品牌
            foreach (DataNode childNode in dataNode.ChildNodeList)
            {
                //计算样式名
                string className = String.Empty;
                if (childNode.BrandType == "brand")
                {
                    htmlList.Add(string.Format("<li t=\"2\"><a href=\"/{0}/\" class=\"brandType\" target=\"_blank\"><big>{1}</big> <span>({2})</span></a>", childNode.AllSpell, childNode.Name, childNode.Count.ToString()));
                    //生成子品牌HTML
                    if (childNode.ChildNodeList != null)
                    {
                        htmlList.Add("<ul>");
                        foreach (DataNode serialNode in childNode.ChildNodeList)
                            MakeSerialTreeHtml(serialNode);
                        htmlList.Add("</ul>");
                    }
                    htmlList.Add("</li>");
                }
                else
                    MakeSerialTreeHtml(childNode);
            }
            htmlList.Add("</ul>");
        }
        /// <summary>
        /// 生成子品牌的HTML
        /// </summary>
        private void MakeSerialTreeHtml(DataNode dataNode)
        {
            string serialName = dataNode.Name;
            if (dataNode.Id == 1568)
            {
                serialName = "索纳塔八";
            }

            if (dataNode.SaleState == "停销")
                htmlList.Add("<li t=\"3\" class=\"saleoff\">");
            else
                htmlList.Add("<li t=\"3\">");

            htmlList.Add(string.Format("<a href=\"/{0}/\" class=\"subBrand\" target=\"_blank\"><big>{1}</big>", dataNode.AllSpell, serialName));

            if (!String.IsNullOrEmpty(dataNode.Subsidies))
                htmlList.Add("<span class=\"green\">补贴</span>");
            htmlList.Add("</a>");

            htmlList.Add("</li>");
        }

        private DataNode GetDataNode()
        {
            DataNodeCollection collection = DataNode.GetDataCollection(TreeDataKey);
            foreach (DataNode node in collection)
            {
                if (node.Id == _masterId)
                    return node;
            }
            return null;
        }
    }
}