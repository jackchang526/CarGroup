using System;
using System.IO;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 取子品牌对比 子品牌数据(陆军)
	/// </summary>
	public partial class ForSerialCompareList : InterfacePageBase
	{
		private int csID = 0;
		private int topCount = 5;
		private string temp = string.Empty;
		private string tempCurrent = string.Empty;
		private StringBuilder sb = new StringBuilder();
		//private string xmlPath = "http://carser.bitauto.com/forpicmastertoserial/SerialCompareStat/{0}.xml";
		//private List<CsData> lcd = new List<CsData>();
		//private List<int> lcount = new List<int>();
		// private Hashtable htCs = new Hashtable();
		private string allCityCompareInterface = "AllSerialCompareTop20.xml";

		private Dictionary<int, BitAuto.CarChannel.Common.Enum.EnumCollection.SerialSortForInterface>
			dicCsInfo = new Dictionary<int, EnumCollection.SerialSortForInterface>();

		protected void Page_Load(object sender, EventArgs e)
		{
			// temp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			if (!this.IsPostBack)
			{
				this.CheckPageParam();
				if (csID > 0 && topCount > 0)
				{
					GetSerialNameToHash();
					if (dicCsInfo.ContainsKey(csID))
					{
						EnumCollection.SerialSortForInterface ssfi = dicCsInfo[csID];
						sb.Append("<Serial ID=\"" + csID.ToString() + "\" Name=\"" + ssfi.CsName + "\" >");
						GetSerialCompareByCity();
						sb.Append("</Serial>");
					}
					else
					{
						// add by chengl Oct.26.2012
						sb.Append("<Serial/>");
					}
				}
			}
			Response.Write(sb.ToString());
			// Response.Write(string.Format(temp, tempCurrent, sb.ToString()));
		}

		private void CheckPageParam()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string csIDStr = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(csIDStr, out csID))
				{ }
				else
				{
					csID = 0;
				}
			}
			if (this.Request.QueryString["top"] != null && this.Request.QueryString["top"].ToString() != "")
			{
				string top = this.Request.QueryString["top"].ToString();
				if (int.TryParse(top, out topCount))
				{
					if (topCount < 0 || topCount > 1000)
					{
						topCount = 5;
					}
				}
				else
				{
					topCount = 0;
				}
			}
		}

		private void GetSerialCompareByCity()
		{
			if (csID > 0)
			{

				try
				{
					string keyCache = "ForSerialCompareList_allCityCompareInterface";
					XmlDocument doc = new XmlDocument();
					if (HttpContext.Current.Cache.Get(keyCache) != null)
					{
						doc = (XmlDocument)HttpContext.Current.Cache.Get(keyCache);
					}
					else
					{
						string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\" + allCityCompareInterface);
						if (File.Exists(filePath))
						{
							doc.Load(filePath);
							HttpContext.Current.Cache.Insert(keyCache, doc, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
						}
					}

					if (doc != null && doc.HasChildNodes)
					{
						XmlNodeList xnl = doc.SelectNodes("/AllSerialCompare/CS[@ID=\"" + csID + "\"]/Compare");
						if (xnl != null && xnl.Count > 0)
						{
							int loop = 0;
							foreach (XmlNode xn in xnl)
							{
								if (dicCsInfo.ContainsKey(int.Parse(xn.Attributes["ID"].Value.ToString())))
								{
									EnumCollection.SerialSortForInterface ssfi = dicCsInfo[int.Parse(xn.Attributes["ID"].Value.ToString())];

									sb.Append("<Item CsID=\"" + ssfi.CsID.ToString() + "\" ");
									sb.Append(" CsName=\"" + ssfi.CsName + "\" ");
									sb.Append(" CsShowName=\"" + ssfi.CsShowName + "\" ");
									sb.Append(" CsAllSpell=\"" + ssfi.CsAllSpell + "\" />");
									loop++;
									if (loop >= topCount)
									{ break; }
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					CommonFunction.WriteLog("interfaceforbitauto_ForSerialCompareList:\r\n:URL:" + this.Request.Url.ToString() + "\r\n" + ex);
				}
			}
		}

		private void GetSerialNameToHash()
		{
			string sql = " select cs.cs_id,cs.cs_name,cs.allspell,cs.cs_showname ";
			sql += " from car_serial cs ";
			sql += " where cs.isState=1 ";
			DataSet ds = new DataSet();

			if (HttpContext.Current.Cache.Get("ForSerialCompareList_AllDataNew") != null)
			{
				ds = (DataSet)HttpContext.Current.Cache.Get("ForSerialCompareList_AllDataNew");
			}
			else
			{
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				HttpContext.Current.Cache.Insert("ForSerialCompareList_AllDataNew", ds, null, DateTime.Now.AddHours(1), TimeSpan.Zero);
			}
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					int csid = int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString());
					string csName = ds.Tables[0].Rows[i]["cs_name"].ToString().Trim();
					string csAllSpell = ds.Tables[0].Rows[i]["allspell"].ToString().Trim().ToLower();
					string csShowName = ds.Tables[0].Rows[i]["cs_showname"].ToString().Trim();
					EnumCollection.SerialSortForInterface ssfi = new EnumCollection.SerialSortForInterface();
					ssfi.CsID = csid;
					ssfi.CsName = csName;
					ssfi.CsAllSpell = csAllSpell;
					ssfi.CsShowName = csShowName;
					if (!dicCsInfo.ContainsKey(csid))
					{ dicCsInfo.Add(csid, ssfi); }
				}
			}
		}
	}
}