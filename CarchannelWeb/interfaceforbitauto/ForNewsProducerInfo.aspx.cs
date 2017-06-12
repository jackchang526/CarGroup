using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 厂商信息(王晓山)
	/// </summary>
	public partial class ForNewsProducerInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();
		private StringBuilder sbGuo = new StringBuilder();
		private StringBuilder sbJin = new StringBuilder();
		private string delCPID = ",20634,10009,10490,10010,10375,10470,10480,10420,20637,20652,20636,20638,20646,20639,10510,20640,20648,20641,20642,20650,10260,10560,20643,10270,10271,10430,10300,20683,10321,10550,10340,10341,20644,10352,10372,10373,20645,10374,10460,10520,10574,10450,10530,20530,20440,20647,20655,";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetProducerInfo();
				Response.Write(sb.ToString());
			}
		}

		private void GetProducerInfo()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

			string catchkey = "interfaceforbitauto_ForNewsProducerInfo";
			object interfaceforbitauto_ForNewsProducerInfo = null;
			DataSet dsP = new DataSet();
			CacheManager.GetCachedData(catchkey, out interfaceforbitauto_ForNewsProducerInfo);
			if (interfaceforbitauto_ForNewsProducerInfo == null)
			{
                //string sql = " select cp.cp_id,cp.Cp_ShortName, ";
                //sql += " (case cp.Cp_Country when '中国' then '国产' else '进口' end) as CpCountry ";
                //sql += " from car_serial cs ";
                //sql += " left join car_brand cb on cs.cb_id = cb.cb_id ";
                //sql += " left join dbo.Car_Producer cp on cb.cp_id = cp.cp_id ";
                //sql += " where cs.isState=1 and cb.isState=1 and cs.CsSaleState<>'停销' ";
                //sql += " and cs.cs_CarLevel <> '其它' ";
                ////sql += " and cs.cs_CarLevel <> '其它' and cs.cs_CarLevel <> 'SUV' ";
                ////sql += " and cs.cs_CarLevel <> 'MPV' and cs.cs_CarLevel <> '跑车' ";
                //sql += " order by CpCountry,cp.Spell ";
                string sql = @"SELECT  ISNULL(cp.cp_id, 0) AS cp_id, cp.Cp_ShortName, ( CASE cb.cb_Country
                                                                                           WHEN '中国' THEN '国产'
                                                                                           ELSE '进口'
                                                                                         END ) AS CpCountry
                                FROM    car_serial cs
                                        LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE   cs.isState = 1
                                        AND cb.isState = 1
                                        AND cs.CsSaleState <> '停销'
                                        AND cs.cs_CarLevel <> '其它'
                                ORDER BY CpCountry, cp.Spell";
				dsP = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				CacheManager.InsertCache(catchkey, dsP, 10);
			}
			else
			{
				dsP = (DataSet)interfaceforbitauto_ForNewsProducerInfo;
			}
			if (dsP != null && dsP.Tables.Count > 0 && dsP.Tables[0].Rows.Count > 0)
			{
				sbGuo.Append("<ProducerInfo Country=\"国产\" >");
				sbJin.Append("<ProducerInfo Country=\"进口\" >");
				int befCPID = 0;
				for (int i = 0; i < dsP.Tables[0].Rows.Count; i++)
				{
					if (befCPID != int.Parse(dsP.Tables[0].Rows[i]["cp_id"].ToString()))
					{
						if (delCPID.IndexOf("," + dsP.Tables[0].Rows[i]["cp_id"].ToString() + ",") >= 0)
						{
							continue;
						}
						// 国产
						if (dsP.Tables[0].Rows[i]["CpCountry"].ToString() == "国产")
						{
							sbGuo.Append("<ProducerGuoChan CpID=\"" + dsP.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
							sbGuo.Append("CpShortName=\"" + System.Security.SecurityElement.Escape(dsP.Tables[0].Rows[i]["Cp_ShortName"].ToString()) + "\" ");
							sbGuo.Append("CpLogo=\"http://image.bitautoimg.com/bt/car/default/images/carimage/p_" + dsP.Tables[0].Rows[i]["cp_id"].ToString() + "_b100.jpg\" ");
							sbGuo.Append("CpURL=\"http://car.bitauto.com/producer/" + dsP.Tables[0].Rows[i]["cp_id"].ToString() + ".html\" />");
						}
						// 进口
						if (dsP.Tables[0].Rows[i]["CpCountry"].ToString() == "进口")
						{
							sbJin.Append("<ProducerJinKou CpID=\"" + dsP.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
							sbJin.Append("CpShortName=\"" + System.Security.SecurityElement.Escape(dsP.Tables[0].Rows[i]["Cp_ShortName"].ToString()) + "\" ");
							sbJin.Append("CpLogo=\"http://image.bitautoimg.com/bt/car/default/images/carimage/p_" + dsP.Tables[0].Rows[i]["cp_id"].ToString() + "_b100.jpg\" ");
							sbJin.Append("CpURL=\"http://car.bitauto.com/producer/" + dsP.Tables[0].Rows[i]["cp_id"].ToString() + ".html\" />");
						}
						befCPID = int.Parse(dsP.Tables[0].Rows[i]["cp_id"].ToString());
					}
				}
				sbGuo.Append("</ProducerInfo>");
				sbJin.Append("</ProducerInfo>");
			}
			sb.Append("<ProducerList>");
			sb.Append(sbGuo.ToString());
			sb.Append(sbJin.ToString());
			sb.Append("</ProducerList>");
		}
	}
}