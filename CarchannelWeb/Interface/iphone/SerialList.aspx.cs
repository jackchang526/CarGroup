using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.iphone
{
	public partial class SerialList : InterfacePageBase
	{
		private int type = 1; // 1:按价格,2:按字母,3:按级别
		private int range = 0; // 价格范围
		private string priceData = "http://carser.bitauto.com/forpicmastertoserial/list/PriceForList.xml";
		private string charData = "http://carser.bitauto.com/forpicmastertoserial/list/charForList.xml";
		private string levelData = "http://carser.bitauto.com/forpicmastertoserial/list/levelForList.xml";
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetSortParam();
				GetData();
				Response.Write(sb.ToString());
			}
		}

		private void GetSortParam()
		{
			if (this.Request.QueryString["type"] != null && this.Request.QueryString["type"].ToString() != "")
			{
				string tempType = this.Request.QueryString["type"].ToString();
				if (int.TryParse(tempType, out type))
				{
					if (type < 1 || type > 3)
					{
						type = 1;
					}
				}
			}

			// 
			if (this.Request.QueryString["range"] != null && this.Request.QueryString["range"].ToString() != "")
			{
				string tempRange = this.Request.QueryString["range"].ToString();
				if (int.TryParse(tempRange, out range))
				{
				}
			}
		}

		private void GetData()
		{
			// 按价格
			#region 按价格
			if (type == 1)
			{
				XmlDocument doc = new XmlDocument();
				try
				{
					doc.Load(priceData);
				}
				catch
				{ }
				if (doc != null && doc.HasChildNodes)
				{
					string nodName = "";
					switch (range)
					{
						case 1: nodName = "5万以内"; break;
						case 2: nodName = "5-8万"; break;
						case 3: nodName = "8-12万"; break;
						case 4: nodName = "12-18万"; break;
						case 5: nodName = "18-25万"; break;
						case 6: nodName = "25-40万"; break;
						case 7: nodName = "40-80万"; break;
						case 8: nodName = "80万以上"; break;
						default: break;
					}
					XmlNodeList xnl = doc.SelectNodes("Params/CsPrice[@PriceRange = '" + nodName + "']");
					if (xnl != null && xnl.Count > 0)
					{
						int loop = 0;
						sb.Append("[");
						foreach (XmlNode xn in xnl[0].ChildNodes)
						{
							if (loop != 0)
							{
								sb.Append(",");
							}
							sb.Append("{CsID:\"" + xn.Attributes["ID"].Value.ToString().Trim() + "\",");
							sb.Append("CsName:\"" + xn.Attributes["Name"].Value.ToString().Trim() + "\"}");
							loop++;
						}
						sb.Append("]");
					}
				}
			}
			#endregion

			#region 按字母
			else if (type == 2)
			{
				XmlDocument doc = new XmlDocument();
				try
				{
					doc.Load(charData);
				}
				catch
				{ }
				XmlNodeList xnl = doc.SelectNodes("Params/CsChar");
				if (xnl != null && xnl.Count > 0)
				{
					int loopChar = 0;
					sb.Append("[");
					foreach (XmlNode xn in xnl)
					{
						if (loopChar != 0)
						{
							sb.Append(",");
						}
						int loop = 0;
						sb.Append("{CharName:\"" + xn.Attributes["Char"].Value.ToString() + "\",CharList:[");
						foreach (XmlNode xnChild in xn.ChildNodes)
						{
							if (loop != 0)
							{
								sb.Append(",");
							}
							sb.Append("{CsID:\"" + xnChild.Attributes["ID"].Value.ToString().Trim() + "\",");
							sb.Append("CsName:\"" + xnChild.Attributes["Name"].Value.ToString().Trim() + "\"}");
							loop++;
						}
						sb.Append("]}");
						loopChar++;
					}
					sb.Append("]");
				}
			}
			#endregion

			#region 按级别
			else if (type == 3)
			{
				XmlDocument doc = new XmlDocument();
				try
				{
					doc.Load(levelData);
				}
				catch
				{ }
				XmlNodeList xnl = doc.SelectNodes("Params/CsLevel");
				if (xnl != null && xnl.Count > 0)
				{
					int loopChar = 0;
					sb.Append("[");
					foreach (XmlNode xn in xnl)
					{
						if (loopChar != 0)
						{
							sb.Append(",");
						}
						int loop = 0;
						sb.Append("{LevelName:\"" + xn.Attributes["LevelName"].Value.ToString() + "\",LevelList:[");
						foreach (XmlNode xnChild in xn.ChildNodes)
						{
							if (loop != 0)
							{
								sb.Append(",");
							}
							sb.Append("{CsID:\"" + xnChild.Attributes["ID"].Value.ToString().Trim() + "\",");
							sb.Append("CsName:\"" + xnChild.Attributes["Name"].Value.ToString().Trim() + "\"}");
							loop++;
						}
						sb.Append("]}");
						loopChar++;
					}
					sb.Append("]");
				}
			}
			#endregion
		}
	}
}