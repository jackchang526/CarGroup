using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 子品牌近30天UV排行数据 (搜索组 李传松)
	/// </summary>
	public partial class AllSerialSortInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private DataSet dsUV = new DataSet();
		private Hashtable htLevel = new Hashtable();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Root>");
				GetSerialSortInfo();
				sb.Append("</Root>");
				Response.Write(sb.ToString());
			}
		}

		private void GetSerialSortInfo()
		{
			Dictionary<int, XmlElement> urlDic = CarSerialImgUrlService.GetImageUrlDicNew();
			string sql = "select t1.csID as cs_ID,t1.sumCount as Pv_SumNum ,cs.cs_CarLevel,cs.cs_name,cs.cs_showname,cs.allSpell from ( ";
			sql += " select csID,sum(uvcount) as sumCount from dbo.StatisticSerialPVUVCity group by csid ) t1 ";
			sql += " left join dbo.car_serial cs on t1.csID=cs.cs_id ";
			sql += " order by t1.sumCount desc ";
			dsUV = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (dsUV != null && dsUV.Tables.Count > 0 && dsUV.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsUV.Tables[0].Rows.Count; i++)
				{
					int serialId = int.Parse(dsUV.Tables[0].Rows[i]["cs_ID"].ToString().Trim());
					int levelSort = 1;
					if (dsUV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim() == "概念车")
					{ continue; }
					if (htLevel.ContainsKey(dsUV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim()))
					{
						levelSort = int.Parse(htLevel[dsUV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim()].ToString()) + 1;
						htLevel[dsUV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim()] = levelSort;
					}
					else
					{
						htLevel.Add(dsUV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim(), 1);
					}
					sb.Append("<Serial ID=\"" + dsUV.Tables[0].Rows[i]["cs_ID"].ToString().Trim() + "\" ");
					sb.Append(" CsName=\"" + dsUV.Tables[0].Rows[i]["cs_name"].ToString().Trim() + "\" ");
					sb.Append(" CsShowName=\"" + dsUV.Tables[0].Rows[i]["cs_showname"].ToString().Trim() + "\" ");
					sb.Append(" AllSpell=\"" + dsUV.Tables[0].Rows[i]["allSpell"].ToString().Trim().ToLower() + "\" ");
					sb.Append(" LevelName=\"" + dsUV.Tables[0].Rows[i]["cs_CarLevel"].ToString().Trim() + "\" ");
					sb.Append(" LevelSort=\"" + levelSort + "\" ");
					string priceRange = base.GetSerialPriceRangeByID(Convert.ToInt32(dsUV.Tables[0].Rows[i]["cs_ID"].ToString().Trim()));
					sb.Append(" PriceRange=\"" + priceRange + "\" ");

					string imgUrl = "";
					if (urlDic.ContainsKey(serialId))
					{
						// modified by chengl Jan.4.2010
						if (urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim() != "")
						{
							// 有新封面
							imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl2").ToString().Trim(), "2");
						}
						else
						{
							// 没有新封面
							if (urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim() != "")
							{
								imgUrl = string.Format(urlDic[serialId].GetAttribute("ImageUrl").ToString().Trim(), "2");
							}
							else
							{
								imgUrl = WebConfig.DefaultCarPic;
							}
						}
					}
					else
						imgUrl = WebConfig.DefaultCarPic;

					sb.Append(" CsPic=\"" + imgUrl + "\" />");
				}
			}
		}
	}
}