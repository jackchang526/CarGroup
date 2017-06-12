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
    public partial class Article : H5PageBase
    {
        private CommonHtmlBll _commonhtmlBll;
        private Dictionary<int, string> _dictSerialBlockHtml; //静态块内容
        protected int SerialId = 0;
        protected string Artical=string.Empty;
        protected SerialEntity BaseSerialEntity;
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.SetPageCache(30);//设置页面缓存

            _commonhtmlBll = new CommonHtmlBll();
            SerialId = ConvertHelper.GetInteger(Request.QueryString["csid"]);
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, SerialId);
            //静态块内容
            _dictSerialBlockHtml = _commonhtmlBll.GetCommonHtml(SerialId, CommonHtmlEnum.TypeEnum.Serial,
                CommonHtmlEnum.TagIdEnum.H5SerialSummary);
            const int articaEnuml = (int)CommonHtmlEnum.BlockIdEnum.H5Artical;
            if (_dictSerialBlockHtml.ContainsKey(articaEnuml))
                Artical = _dictSerialBlockHtml[articaEnuml];
        }

    }
}