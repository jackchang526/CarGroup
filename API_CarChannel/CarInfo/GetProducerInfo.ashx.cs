using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.DAL.Data;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using System.Xml;
using System.Web.Caching;
using System.Collections.Specialized;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetProducerInfo 的摘要说明
	/// add op = getcp for zhouxf Jun.27.2013 迁移car域名接口： http://car.bitauto.com/interfaceforbitauto/alldbforcar.aspx
	/// add op = getproducerbrandserial for fangxc May.14.2014 迁移接口：http://car.bitauto.com/Car/interfaceforbitauto/ProducerBrandSerial.aspx
	/// </summary>
	public class GetProducerInfo : PageBase, IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;
		private StringBuilder sb = new StringBuilder();

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(60);
			response = context.Response;
			request = context.Request;
			string op = request.QueryString["op"];
			if (!string.IsNullOrEmpty(op))
			{ op = op.Trim().ToLower(); }
			switch (op)
			{
				case "getcp": RenderCPData(); break;
				case "getproducerbrandserial": RenderProducerBrandSerial(); break;
				default: break;
			}
		}

		private void RenderProducerBrandSerial()
		{
			string sql = @"SELECT  ISNULL(cp.cp_id, 0) AS cp_id, cp.Cp_Name, cp.Cp_ShortName,
                                    cp.Cp_Byname, cp.Cp_EName, cb.cb_Country AS Cp_Country, cp.Cp_Url,
                                    cp.Cp_Phone, cb.cb_id, cb.cb_name, cb.allSpell AS cbAllSpell,
                                    cb.cb_OtherName
                            FROM    Car_Brand cb
                                    LEFT JOIN Car_Producer cp ON cb.cp_id = cp.cp_id
                            WHERE   cp.isState = 1
                                    AND cb.isState = 1
                            ORDER BY cp.cp_id, cb.cb_id ";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				int currentCPID = 0;
				int currentCBID = 0;
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					// 不同厂商
					if (ds.Tables[0].Rows[i]["cp_id"].ToString() != currentCPID.ToString())
					{
						if (currentCPID != 0)
						{
							sb.Append("</Brand>");
							sb.Append("</Producer>");
						}
						currentCPID = int.Parse(ds.Tables[0].Rows[i]["cp_id"].ToString());
						sb.Append("<Producer>");
						sb.Append("<Cp_ID>" + ds.Tables[0].Rows[i]["cp_id"].ToString().Trim() + "</Cp_ID>");
						sb.Append("<Cp_Name>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Name"].ToString().Trim()) + "</Cp_Name>");
						sb.Append("<Cp_ShortName>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_ShortName"].ToString().Trim()) + "</Cp_ShortName>");
						sb.Append("<Cp_Byname>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Byname"].ToString().Trim()) + "</Cp_Byname>");
						sb.Append("<Cp_EName>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_EName"].ToString().Trim()) + "</Cp_EName>");
						sb.Append("<Cp_Url>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Url"].ToString().Trim()) + "</Cp_Url>");
						sb.Append("<Cp_Phone>" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[i]["Cp_Phone"].ToString().Trim()) + "</Cp_Phone>");
						sb.Append("<Cp_Country>" + (ds.Tables[0].Rows[i]["Cp_Country"].ToString().Trim() == "中国" ? "国产" : "进口") + "</Cp_Country>");
						// 不同品牌
						if (ds.Tables[0].Rows[i]["cb_id"].ToString() != currentCBID.ToString())
						{
							currentCBID = int.Parse(ds.Tables[0].Rows[i]["cb_id"].ToString());
							sb.Append("<Brand>");
							sb.Append("<Cb_ID>" + ds.Tables[0].Rows[i]["cb_id"].ToString().Trim() + "</Cb_ID>");
						}
					}
					// 相同厂商
					else
					{
						// 不同品牌
						if (ds.Tables[0].Rows[i]["cb_id"].ToString() != currentCBID.ToString())
						{
							currentCBID = int.Parse(ds.Tables[0].Rows[i]["cb_id"].ToString());
							sb.Append("</Brand>");
							sb.Append("<Brand>");
							sb.Append("<Cb_ID>" + ds.Tables[0].Rows[i]["cb_id"].ToString().Trim() + "</Cb_ID>");
						}
					}
				}
				sb.Append("</Brand>");
				sb.Append("</Producer>");
			}
			CommonFunction.EchoXml(response, sb.ToString(), "ProducerBrandSerial");
		}

		private void RenderCPData()
		{
			DataSet dsCP = base.GetAllCPForInterface();
			if (dsCP != null && dsCP.Tables[0].Rows.Count > 0)
			{
				sb.Append("<Producer>");
				for (int i = 0; i < dsCP.Tables[0].Rows.Count; i++)
				{
					sb.AppendLine("<Item Cp_Id=\"" + dsCP.Tables[0].Rows[i]["cp_id"].ToString() + "\" ");
					sb.AppendLine(" OldCp_Id=\"" + dsCP.Tables[0].Rows[i]["OldCp_Id"].ToString() + "\" ");
					sb.AppendLine(" Cp_Country=\"" + dsCP.Tables[0].Rows[i]["Cp_Country"].ToString() + "\" ");
					sb.AppendLine(" Spell=\"" + System.Security.SecurityElement.Escape(dsCP.Tables[0].Rows[i]["Spell"].ToString()) + "\" ");
					sb.AppendLine(" ShortName=\"" + System.Security.SecurityElement.Escape(dsCP.Tables[0].Rows[i]["cp_shortname"].ToString()) + "\" ");
					sb.AppendLine(" Cp_Name=\"" + System.Security.SecurityElement.Escape(dsCP.Tables[0].Rows[i]["cp_name"].ToString()) + "\" />");
				}
				sb.Append("</Producer>");
			}
			CommonFunction.EchoXml(response, sb.ToString(), "AutoCarChannel");
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}