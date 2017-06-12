using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.Utils;

namespace H5Web.V2
{
    public partial class Commentary : H5PageBase
    {
        protected int SerialId = 0;
        protected SerialEntity BaseSerialEntity;
        protected string GuestFuelCost;
        protected string EditorComment;

        protected string Koubei;
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.SetPageCache(30);//设置页面缓存

            if (Request.QueryString["csid"] != null)
            {
                int.TryParse(Request.QueryString["csid"], out SerialId);
            }

            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
            EditorComment = new CommonHtmlBll().GetCommonHtmlByBlockId(SerialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.H5SerialSummary, CommonHtmlEnum.BlockIdEnum.EditorCommentV2);

            Koubei = new CommonHtmlBll().GetCommonHtmlByBlockId(SerialId, CommonHtmlEnum.TypeEnum.Serial, CommonHtmlEnum.TagIdEnum.H5SerialSummary, CommonHtmlEnum.BlockIdEnum.H5KoubeiV2);
            GuestFuelCost = GetSerialDianPingYouHaoByCsID(SerialId);
        }
    }
}