using BitAuto.CarUtils.Define;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitAuto.Utils;
using System.Web.UI;
using BitAuto.CarChannel.BLL;
using System.Xml;
using System.Text;

namespace MWeb.Controllers
{
    public class BaoZhiLvController : Controller
    {
        //五年旧车保值率
        // GET: /BaoZhiLv/
        protected int LevelId = 0;
        protected string LevelName = string.Empty;
        protected string LevelFullName = string.Empty;
        protected string LevelSpell = string.Empty;
        protected string BaoZhiLvHtml = string.Empty;

        private int PageIndex = 1;
        private int PageSize = 10;

        private Car_SerialBll serialBll = new Car_SerialBll();

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index()
        {
            GetParams();

            return View();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParams()
        {
            LevelId = ConvertHelper.GetInteger(Request.QueryString["Level"]);
            LevelName = CarLevelDefine.GetLevelNameById(LevelId);
            LevelFullName = this.LevelName;
            if (LevelName == "紧凑型" || LevelName == "中大型")
            {
                LevelFullName = LevelName + "车";
            }
            LevelSpell = CarLevelDefine.GetLevelSpellById(this.LevelId);

            PageIndex = ConvertHelper.GetInteger(Request.QueryString["pageindex"]);
            PageSize = ConvertHelper.GetInteger(Request.QueryString["pagesize"]);
            if (PageIndex <= 0)
            {
                PageIndex = 1;
            }
            if (PageSize <= 0)
            {
                PageSize = 10;
            }
        }

        /// <summary>
        /// 生成html代码
        /// </summary>
        private string RenderHtml()
        {
            List<XmlElement> list = serialBll.GetSerialBaoZhiLv(LevelName);
            if (list != null && list.Count > 0)
            {
                int count = list.Count;
                int start = (PageIndex - 1) * PageSize;
                int end = start + PageSize - 1;

                if (start > count)
                {
                    return string.Empty;
                }
                if (count < end)
                {
                    end = count;
                }

                StringBuilder sb = new StringBuilder();
                foreach(XmlElement ele in list)
                for (int i = start; i < end; i++)
                {
                    sb.AppendFormat("<li><i{3}>{0}</i>{1}<strong>{2}%</strong></li>"
                        );
                }
            }
            return string.Empty;
        }



    }
}
