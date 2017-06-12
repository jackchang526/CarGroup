using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace H5Web.V2
{
    public partial class Forum : System.Web.UI.Page
    {
        /// <summary>
        ///     论坛新闻列表
        /// </summary>
        protected string ForumNewsHtml = string.Empty;
        protected int SerialId = 0;
        protected SerialEntity BaseSerialEntity;
        readonly Car_SerialBll _serialBll = new Car_SerialBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.SetPageCache(30);//设置页面缓存

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
            MakeForumNewsHtml();
        }

        /// <summary>
        ///     论坛话题新闻
        /// </summary>
        private void MakeForumNewsHtml()
        {
            var sb = new StringBuilder();
            string baaUrl = _serialBll.GetForumUrlBySerialId(SerialId);//更过链接地址
            XmlDocument xmlDoc = _serialBll.GetSerialForumNews(SerialId);
            if (xmlDoc == null) return;
            XmlNodeList newsList = xmlDoc.SelectNodes("/root/Forum/ForumSubject");
            if (newsList != null && newsList.Count <= 1) return;
            int i = 0;
            foreach (XmlNode node in newsList)
            {
                i++;
                if (i > 3) break;
                string newsTitle = node.SelectSingleNode("title").InnerText.Trim();
                //过滤Html标签
                newsTitle = StringHelper.RemoveHtmlTag(newsTitle);
                newsTitle = StringHelper.SubString(newsTitle, 40, true);
                string tid = node.SelectSingleNode("tid").InnerText;
                string filePath = node.SelectSingleNode("url").InnerText;
                string replies = node.SelectSingleNode("replies").InnerText;
                string poster = node.SelectSingleNode("poster").InnerText;
                string pubTime = "";
                // modified by chengl Jun.15.2012
                if (node.SelectSingleNode("postdatetime") != null)
                {
                    pubTime = node.SelectSingleNode("postdatetime").InnerText;
                    pubTime = Convert.ToDateTime(pubTime).ToString("yyyy-MM-dd");
                }
                int classId = ConvertHelper.GetInteger(node.SelectSingleNode("digest").InnerText);
                string className = Enum.GetName(typeof(EnumCollection.ForumDigest), classId);
                XmlNode imglistNode = node.SelectSingleNode("imgList");

                sb.Append("<li name=''>");
                sb.AppendFormat("<a href='{0}'>", filePath.Replace("baa.bitauto.com", "baa.m.yiche.com"));
                sb.Append("        <div class='baa-list-tt'>");
                sb.AppendFormat("            <span class='{0}'>{1}</span>{2}", imglistNode!=null&&imglistNode.SelectNodes("img")!=null&&imglistNode.SelectNodes("img").Count>0?"bg2":"bg1",className,newsTitle);
                sb.Append("        </div>");
                sb.Append("        <div class='baa-list-user'>");
                sb.AppendFormat("            <em>{0}</em>", poster);
                sb.AppendFormat("            <em>发表于{0}</em>", pubTime);
                sb.Append("        </div>");
                
                if (imglistNode != null)
                {
                    XmlNodeList xmlNodeList = imglistNode.SelectNodes("img");
                    if (xmlNodeList != null && xmlNodeList.Count > 0)
                    {
                        sb.AppendFormat("<ul class='baa-list-img'>");
                        int j = 0;
                        foreach (XmlNode item in xmlNodeList)
                        {
                            if (j >= 3)
                            {
                                break;
                            }
                            sb.AppendFormat("<li><span><img src='{0}' width='216px' heigth='144px'/></span></li>",
                                item.InnerText.Replace("_120_80_", "_216_144_"));
                            j++;
                        }
                        sb.AppendFormat("</ul>");
                    }
                }
                sb.Append("    </a>");
                sb.Append("</li>");
            }
            ForumNewsHtml = sb.ToString();
        }

    }
}