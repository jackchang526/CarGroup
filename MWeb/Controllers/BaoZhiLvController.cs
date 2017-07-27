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
using BitAuto.CarChannel.Common;

namespace MWeb.Controllers
{
    public class BaoZhiLvController : Controller
    {
        //五年旧车保值率
        // GET: /BaoZhiLv/
        protected int LevelId = 0;
        protected string LevelName = string.Empty;
        protected string LevelFullName = string.Empty;
        protected string BaoZhiLvHtml = string.Empty;
        protected string LevelSpell = string.Empty;
        private int PageIndex = 1;
        private int PageSize = 10;

        private Car_SerialBll serialBll = new Car_SerialBll();

        [OutputCache(Duration = 3600, Location = OutputCacheLocation.Downstream)]
        public ActionResult Index(string level)
        {
            LevelId = (int)(SerialLevelSpellEnum)Enum.Parse(typeof(SerialLevelSpellEnum), level);
            LevelName = CarLevelDefine.GetLevelNameById(LevelId);
            LevelFullName = LevelName;
            if (LevelName == "紧凑型" || LevelName == "中大型")
            {
                LevelFullName = LevelName + "车";
            }
            LevelSpell = CarLevelDefine.GetLevelSpellById(LevelId);

            RenderLevel();
            BaoZhiLvHtml = RenderHtml();
            ViewData["BaoZhiLvHtml"] = BaoZhiLvHtml;
            ViewData["LevelFullName"] = LevelFullName;
            ViewData["LevelSpell"] = LevelSpell;
            return View();
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
                foreach (XmlElement ele in list)
                {
                    int index = list.IndexOf(ele);
                    if (index < start)
                        continue;
                    if (index > end)
                        break;

                    sb.AppendFormat("<li><i{3}>{0}</i><a href=\"/{4}/\">{1}</a><strong>{2}%</strong></li>",
                        (index + 1) < 10 ? (index + 1).ToString().PadLeft(2, '0') : (index + 1).ToString(),
                        ele.Attributes["ShowName"].InnerText,
                        Math.Round(ConvertHelper.GetDouble(ele.Attributes["ResidualRatio5"].InnerText) * 100.0, 1),
                       (index < 3) ? " class=\"n" + (index + 1) + "\"" : "",
                       ele.Attributes["AllSpell"].InnerText
                        );
                }
                return sb.ToString();
            }
            return string.Empty;
        }


        private void RenderLevel()
        {
            List<string> list = new List<string>();
            string[] array = new string[]
            {
                "微型车",
                "小型车",
                "紧凑型车",
                "中型车",
                "中大型车",
                "豪华车",
                "MPV",
                "SUV",
                "跑车",
                "面包车"
            };
            for (int i = 0; i < array.Length; i++)
            {
                string levelSpellByName = CarLevelDefine.GetLevelSpellByName(array[i]);
                list.Add(string.Format("<li class=\"{2}\"><a href=\"/{0}/baozhilv/\"><span>{1}</span></a></li>"
                    , levelSpellByName
                    , array[i]
                    , (levelSpellByName == LevelSpell) ? "current" : ""));
            }
            ViewData["LevelHtml"] = string.Concat(list.ToArray());
        }
    }
}
