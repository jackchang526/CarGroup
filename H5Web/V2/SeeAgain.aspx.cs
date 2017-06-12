using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace H5Web.V2
{
    public partial class SeeAgain : H5PageBase
    {
        protected string SerialToSee = string.Empty;
        protected int SerialId = 0;
        protected SerialEntity BaseSerialEntity;
        // 经纪人ID
        protected int Brokerid = 0;
        // 经销商ID
        protected int Dealerid = 0;
        //城市ID
        protected int CityId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.SetPageCache(30);//设置页面缓存
            
            if (Request.QueryString["brokerid"] != null)
            {
                int.TryParse(Request.QueryString["brokerid"], out Brokerid);
            }

            if (Request.QueryString["dealerid"] != null)
            {
                int.TryParse(Request.QueryString["dealerid"], out Dealerid);
            }

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }

            if (Request.QueryString["cityid"] != null)
            {
                int.TryParse(Request.QueryString["cityid"], out CityId);
            }

            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
            MakeSerialToSerialHtml();
        }


        /// <summary>
        ///     子品牌还关注
        /// </summary>
        /// <returns></returns>
        private void MakeSerialToSerialHtml()
        {
            var htmlCode = new StringBuilder();
            List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(SerialId, 6);

            if (lsts.Count > 0)
            {
                htmlCode.Append("<ul>");
                for (int i = 0; i < lsts.Count && i < 6; i++)
                {
                    EnumCollection.SerialToSerial sts = lsts[i];
                    string csName = sts.ToCsShowName;

                    htmlCode.Append("<li>");
                    htmlCode.AppendFormat("<a href='/{0}' target='_parent'>", sts.ToCsAllSpell.ToLower());
                    htmlCode.AppendFormat("<img src='{0}' />",
                        sts.ToCsPic.ToString(CultureInfo.InvariantCulture).Replace("_5.", "_3."));
                    htmlCode.AppendFormat("<strong>{0}</strong>", csName);
                    htmlCode.AppendFormat("<p>{0}</p>",
                        string.IsNullOrEmpty(sts.ToCsPriceRange) ? "暂无报价" : sts.ToCsPriceRange);
                    htmlCode.Append("</a>");
                    htmlCode.Append("</li>");
                }
                htmlCode.Append("</ul>");
            }
            SerialToSee = htmlCode.ToString();
        }
    }
}