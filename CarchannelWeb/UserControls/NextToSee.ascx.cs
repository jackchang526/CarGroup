using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.CarChannel.CarchannelWeb.UserControls
{
    public partial class NextToSee : System.Web.UI.UserControl
    {
        public int serialId = 0;
        protected string nextSeePingceHtml = string.Empty;
        protected string nextSeeDaogouHtml = string.Empty;
        protected string serialShowName = string.Empty;
        protected string serialSpell = string.Empty;
        protected string baaUrl = string.Empty;
		protected string pingceTagHtml = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            SerialEntity serialEntity = null;
            if (serialId > 0)
            {
                serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
            }
            if (serialEntity == null)
            {
                Response.End();
            }
            if (serialEntity != null)
            {
                serialShowName = serialEntity.ShowName;
                serialSpell = serialEntity.AllSpell;
                CarNewsBll newsBll = new CarNewsBll();
                if (newsBll.IsSerialNews(serialEntity.Id, 0, CarNewsType.pingce))
                    nextSeePingceHtml = "<li><a rel=\"nofollow\" href=\"/" + serialEntity.AllSpell + "/pingce/\">" + serialEntity.ShowName + "车型详解</a></li>";

                if (newsBll.IsSerialNews(serialEntity.Id, 0, CarNewsType.daogou))
                    nextSeeDaogouHtml = "<li><a rel=\"nofollow\" href=\"/" + serialEntity.AllSpell + "/daogou/\">" + serialEntity.ShowName + "导购</a></li>";
                baaUrl = new Car_SerialBll().GetForumUrlBySerialId(serialEntity.Id);

				List<string> list = new List<string>();
				list.Add("<li><a href=\"/" + serialSpell + "/jiangjia/\" target=\"_self\">" + serialShowName + "优惠</a></li>");
				Dictionary<int, EnumCollection.PingCeTag>  dictPingceTag = new Common.PageBase().GetPingceTagsByCsId(serialId);
				if (dictPingceTag != null && dictPingceTag.Count > 0)
				{
					Dictionary<int, string> dictTag = new Dictionary<int, string>(){ 
			{2,"外观"}, {3,"内饰"}, {4,"空间"}, {7,"动力"}, {8,"操控性"} };
					int pageIndex = 1;
					foreach (int key in dictPingceTag.Keys)
					{
						if (dictTag.ContainsKey(key))
						{
							list.Add("<li><a href=\"/" + serialSpell + "/pingce/" + pageIndex + "/\" target=\"_self\">" + serialShowName + dictTag[key] + "</a></li>");
						}
						pageIndex++;
					}
				}
				pingceTagHtml = string.Concat(list.ToArray());
            }
        }
    }
}