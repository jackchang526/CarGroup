using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Xml;

namespace BitAuto.CarChannel.CarchannelWeb.PageLevelV2
{
	public partial class BaoZhiLvPaiHang : PageBase
	{
		protected string LevelHtml = string.Empty;

		protected string LevelSpell = string.Empty;

		protected int LevelId;

		protected string LevelName = "";

		protected string LevelFullName = "";

		protected string SerialHtml = string.Empty;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.SetPageCache(10);
			this.Getparameters();
			this.RenderLevel();
			this.RenderSerial();
		}

		private void RenderSerial()
		{
			System.Collections.Generic.List<XmlElement> serialBaoZhiLv = new Car_SerialBll().GetSerialBaoZhiLv(this.LevelName);
			if (serialBaoZhiLv != null && serialBaoZhiLv.Count > 0)
			{
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				foreach (XmlElement current in serialBaoZhiLv)
				{
					int num = serialBaoZhiLv.IndexOf(current);
					list.Add(string.Format("<li><i class=\"{0}\">{1}</i><a class=\"car\" href=\"/{2}/\" target=\"_blank\">{3}<em class=\"hrto\">{4}%</em></a></li>", new object[]
					{
						(num < 3) ? "hot" : "",
						num + 1,
						current.Attributes["AllSpell"].InnerText,
						current.Attributes["ShowName"].InnerText,
						System.Math.Round(ConvertHelper.GetDouble(current.Attributes["ResidualRatio5"].InnerText) * 100.0, 1)
					}));
				}
				this.SerialHtml = string.Join("", list.ToArray());
			}
		}

		private void Getparameters()
		{
			this.LevelId = ConvertHelper.GetInteger(base.Request.QueryString["Level"]);
			this.LevelName = CarLevelDefine.GetLevelNameById(this.LevelId);
			this.LevelFullName = this.LevelName;
			if (this.LevelName == "紧凑型" || this.LevelName == "中大型")
			{
				this.LevelFullName = this.LevelName + "车";
			}
			this.LevelSpell = CarLevelDefine.GetLevelSpellById(this.LevelId);
		}

		private void RenderLevel()
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
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
				list.Add(string.Format("<li class=\"{2}\"><a href=\"/{0}/baozhilv/\">{1}</a></li>", levelSpellByName, array[i], (levelSpellByName == this.LevelSpell) ? "active" : ""));
			}
			this.LevelHtml = string.Concat(list.ToArray());
		}
	}
}
