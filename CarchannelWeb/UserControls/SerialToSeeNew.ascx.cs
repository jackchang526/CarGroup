using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.CarChannel.CarchannelWeb.UserControls
{
    public partial class SerialToSeeNew : System.Web.UI.UserControl
    {
        public int serialId = 0;
        protected string serialToSeeHtml = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder htmlCode = new StringBuilder();
            List<EnumCollection.SerialToSerial> lsts = new PageBase().GetSerialToSerialByCsID(serialId, 6, 3);
            if (lsts.Count > 0)
            {
                foreach (EnumCollection.SerialToSerial sts in lsts)
                {
                    htmlCode.Append("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-14093\">");
                    htmlCode.Append("    <div class=\"img\">");
                    htmlCode.AppendFormat("        <a href=\"/{0}/\" target=\"_blank\">", sts.ToCsAllSpell.ToString().ToLower());
                    htmlCode.AppendFormat("            <img src=\"{0}\"></a>", sts.ToCsPic);
                    htmlCode.Append("    </div>");
                    htmlCode.Append("    <ul class=\"p-list\">");
                    htmlCode.AppendFormat("        <li class=\"name no-wrap\"><a href=\"/{0}/\" target=\"_blank\">{1}</a></li>", sts.ToCsAllSpell.ToString().ToLower(), sts.ToCsShowName);
                    htmlCode.AppendFormat("        <li class=\"price\"><a href=\"/{0}/\" target=\"_blank\">{1}</a></li>", sts.ToCsAllSpell.ToString().ToLower(), string.IsNullOrWhiteSpace(sts.ToCsPriceRange) ? "&nbsp;" : sts.ToCsPriceRange);
                    htmlCode.Append("    </ul>");
                    htmlCode.Append("</div>");
                }
            }
            serialToSeeHtml = htmlCode.ToString();
        }
    }
}