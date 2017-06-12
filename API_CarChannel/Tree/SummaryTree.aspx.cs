using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannelAPI.Web.Tree
{
    public partial class SummaryTree : PageBase
    {
        private const string TreeDataKey = "chexing";

        protected string TreeHtml
        {
            get
            {
                if (htmlList != null && htmlList.Count > 0)
                    return string.Concat(htmlList.ToArray());
                return string.Empty;
            }
        }

        private List<string> htmlList;					//HTML代码
        private DataNodeCollection dataCollection;		//标签中各品牌数据

        private string charString;
        protected int masterId;
        protected int brandId;
        protected int serialId;
        private int dataIndex;							//因dataCollection是按字母排序的，所以按字母生成时，用此变量做各字母的开始索引
        private string pageType;
        protected int _LoggerType;

        protected void Page_Load(object sender, EventArgs e)
        {
            /* 有参数的：
             //* type = masterbrand：主品牌页；CarMasterPage.aspx 
             //* type = brand：品牌页；CarBrandPage.aspx 
             //* type = serial：子品牌-综述页， 
             *                  子品牌-参数配置页，CsCompare.aspx
             *                  子品牌-车型详解页（易车测试）；CsPingCe.aspx
             //*                年款-综述页；CsSummaryYear.aspx
             *                  年款-参数配置页；   CsCompare.aspx
             *                  年款-车型详解页（易车测试）；SerialYearNews.aspx
             //*                车型-综述页； CarSummary.aspx
             *                  车型-参数配置页；CarCompare.aspx
             * 无参数的：
             * 级别页 CsLevelNew.aspx 
             * 
             * 统计代码类型说明
             * pagetype:
             * 级别页 1
             * 主品牌 2
             * 品牌 3
             * 子品牌-综述页 4
             * 子品牌-参数配置页 5
             * 子品牌-车型详解页（易车测试） 6
             * 年款-综述页 7
             * 年款-参数配置页 8
             * 年款-车型详解页（易车测试） 9
             * 车型-综述页 10
             * 车型-参数配置页 11
             * nodetype
             * 主品牌 1
             * 品牌 2
             * 子品牌 3
             */

            this.SetPageCache(60);

            Response.ContentType = "application/x-javascript";
            charString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            masterId = ConvertHelper.GetInteger(this.Request.QueryString["masterId"]);
            brandId = ConvertHelper.GetInteger(this.Request.QueryString["brandId"]);
            serialId = ConvertHelper.GetInteger(this.Request.QueryString["serialId"]);

            _LoggerType = ConvertHelper.GetInteger(this.Request.QueryString["pageType"]);
            switch (_LoggerType)
            {
                case 2:
                    pageType = "masterbrand";
                    break;
                case 3:
                    pageType = "brand";
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                    pageType = "serial";
                    break;
                default:
                    _LoggerType = 1;
                    pageType = string.Empty;
                    break;
            }

            dataCollection = DataNode.GetDataCollection(TreeDataKey);

            htmlList = new List<string>();

            MakeLeftTreeHtml();
        }

        /// <summary>
        /// 生成树形的HTML
        /// </summary>
        private void MakeLeftTreeHtml()
        {
            MakeCharNav();
            MakeTreeHtml();
        }

        /// <summary>
        /// 生成字符导航
        /// </summary>
        private void MakeCharNav()
        {
            htmlList.Add("<div class=\"treeSubNavv1\"><ul id=\"tree_letter\">");
            for (int i = 0; i < charString.Length; i++)
            {
                char curChar = charString[i];
                if (dataCollection.ContainsChar(curChar))
                    htmlList.Add(string.Format("<li><a href=\"#\" onclick=\"treeHref({0});return false;\">{1}</a></li>", (i + 1).ToString(), curChar));
                else
                    htmlList.Add(string.Format("<li class=\"none\">{0}</li>", curChar));
            }
            htmlList.Add("</ul></div>");
        }

        /// <summary>
        /// 生成树内的数据HTML
        /// </summary>
        private void MakeTreeHtml()
        {
            htmlList.Add("<div class=\"treev1\" id=\"treev1\"><ul>");
            for (int i = 0; i < charString.Length; i++)
            {
                char curChar = charString[i];
                if (!dataCollection.ContainsChar(curChar))
                    continue;
                htmlList.Add(string.Format("<li class=\"root\" id=\"letter{0}\"><b>{1}</b>",(i + 1).ToString(), curChar));
                htmlList.Add("<ul>");
                MakeTreeHtmlByChar(curChar);
                htmlList.Add("</ul></li>");
            }
            htmlList.Add("<li style=\"height:300px; overflow:hidden\"></li>");
            htmlList.Add("</ul></div>");
        }

        /// <summary>
        /// 按字母生成HTML
        /// </summary>
        private void MakeTreeHtmlByChar(char curChar)
        {
            for (int i = dataIndex; i < dataCollection.Count; i++)
            {
                DataNode dataNode = dataCollection[i];
                if (dataNode.FirstChar != curChar)
                {
                    dataIndex = i;
                    break;
                }
				string className = "mainBrand";
				string curIdStr = String.Empty;
                //主品牌Url
                if (dataNode.Id == masterId)
                {
					if (pageType == "masterbrand")
					{
						className = "mainBrand current current_unfold";
						curIdStr = " id=\"curObjTreeNode\"";
					}
					else
						className = "mainBrand current_unfold";

                    htmlList.Add(string.Format("<li" + curIdStr + " t=\"1\"><a href=\"#\" ap=\"{0}\" onclick=\"expandMaster(this, {1});return false;\" class=\"" + className + "\" target=\"_blank\"><big>{2}</big> <span>({3})</span></a>"
                            , dataNode.AllSpell, dataNode.Id, dataNode.Name, dataNode.Count.ToString()));
                }
                else
                {
                    htmlList.Add(string.Format("<li t=\"1\"><a href=\"#\" ap=\"{0}\" onclick=\"expandMaster(this, {1});return false;\"class=\"" + className + "\"><big>{2}</big> <span>({3})</span></a>"
                            , dataNode.AllSpell, dataNode.Id, dataNode.Name, dataNode.Count.ToString()));
                }

                //只有在当前主品牌时才展开
                if (dataNode.Id == masterId)
                    MakeTreeHtmlByMasterbrand(dataNode);

                htmlList.Add("</li>");
            }
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
                string curIdStr = string.Empty;
                if (childNode.BrandType == "brand")
                {
                    className = "brandType";
                    if (childNode.Id == brandId && pageType == "brand")
                    {
                        //如果是子品牌页面，且此子品牌不在此树中，选中该品牌
                        if (pageType == "brand" || (pageType == "serial" && childNode.ChildNodeList != null
                            && childNode.ChildNodeList.GetDataNodeById(serialId) == null))
                        {
                            className += " current";
                            curIdStr = " id=\"curObjTreeNode\"";
                        }
                    }

                    htmlList.Add(string.Format("<li t=\"2\"{4}><a href=\"/{0}/\" class=\"{1}\" target=\"_blank\"><big>{2}</big> <span>({3})</span></a>", childNode.AllSpell, className, childNode.Name, childNode.Count.ToString(), curIdStr));
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
            string className = "subBrand";
            string curIdStr = string.Empty;
            if (dataNode.Id == serialId)
            {
                className += " current";
                curIdStr = " id=\"curObjTreeNode\"";
            }
            string serialName = dataNode.Name;
            if (dataNode.Id == 1568)
            {
                serialName = "索纳塔八";
            }

            if (dataNode.SaleState == "停销")
                htmlList.Add(string.Format("<li t=\"3\" class=\"saleoff\"{0}>", curIdStr));
            else
                htmlList.Add(string.Format("<li t=\"3\" {0}>", curIdStr));

            htmlList.Add(string.Format("<a href=\"/{0}/\" class=\"{1}\" target=\"_blank\"><big>{2}</big>", dataNode.AllSpell, className, serialName));

            if (!String.IsNullOrEmpty(dataNode.Subsidies))
                htmlList.Add("<span class=\"green\">补贴</span>");
            htmlList.Add("</a>");

            htmlList.Add("</li>");
        }
    }
}