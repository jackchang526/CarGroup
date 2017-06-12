using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;

namespace BitAuto.CarChannel.CarchannelWeb.UserControls
{
	public partial class SerialToSee : System.Web.UI.UserControl
	{
		protected string serialToSeeHtml = string.Empty;


		public int SerialId { get; set; }

		public string SerialName { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			MakeSerialToSerialHtml();
		}

		private void MakeSerialToSerialHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			List<EnumCollection.SerialToSerial> lsts = new PageBase().GetSerialToSerialByCsID(SerialId, 6);
			if (lsts.Count > 0)
			{
				int loop = 0;
				foreach (EnumCollection.SerialToSerial sts in lsts)
				{
					string csName = sts.ToCsShowName.ToString();
					string shortName = StringHelper.SubString(csName, 12, true);
					if (shortName.StartsWith(csName))
						shortName = csName;

					loop++;
					htmlCode.Append("<li>");
					htmlCode.AppendFormat("<a target=\"_blank\" href=\"/{0}/\"><img src=\"{1}\" width=\"90\" height=\"60\"></a>",
						sts.ToCsAllSpell.ToString().ToLower(),
						 sts.ToCsPic.ToString());
					if (shortName != csName)
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\" title=\"{1}\">{2}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName, shortName);
					else
						htmlCode.AppendFormat("<p><a target=\"_blank\" href=\"/{0}/\">{1}</a></p>",
							sts.ToCsAllSpell.ToString().ToLower(),
							csName);
					var priceRange = StringHelper.SubString(sts.ToCsPriceRange.ToString(), 14, false);
					htmlCode.AppendFormat("<p><span>{0}</span></p>", string.IsNullOrEmpty(priceRange) ? "暂无报价" : priceRange);
					htmlCode.AppendFormat("</li>");
				}
			}
			serialToSeeHtml = htmlCode.ToString();
		}
	}
}