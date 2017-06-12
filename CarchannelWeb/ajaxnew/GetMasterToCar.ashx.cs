using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.AjaxNew
{
	/// <summary>
	/// GetMasterToCar 的摘要说明
	/// </summary>
	public class GetMasterToCar : IHttpHandler
	{
		private StringBuilder sb = new StringBuilder();
		private int type = 0;
		private int id = 0;

		public void ProcessRequest(HttpContext context)
		{
			GetParam(context);
			context.Response.ContentType = "text/xml";
			CommonService cs = new CommonService();
			//CommonFunction cf = new CommonFunction(); 
			string tempXml = "";

			if (type == 1)
			{
				// 主品牌
				sb.Append("<option value=\"-1\">选择品牌</option>");
				DataSet dsMaster = cs.GetAllMasterBrand(10);
				if (dsMaster != null && dsMaster.Tables.Count > 0 && dsMaster.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsMaster.Tables[0].Rows.Count; i++)
					{
						sb.Append("<option value=\"" + dsMaster.Tables[0].Rows[i]["bs_id"].ToString().Trim() + "\">" + dsMaster.Tables[0].Rows[i]["msname"].ToString().Trim() + "</option>");
					}
					tempXml = sb.ToString();
				}
			}
			else if (type == 2)
			{
				// 子品牌
				// sb.Append("<option value=\"-1\">选择车型</option>");
				if (id > 0)
				{
					DataSet dsSerial = cs.GetAllSerialForAjaxCompare(10);
					if (dsSerial != null && dsSerial.Tables.Count > 0 && dsSerial.Tables[0].Rows.Count > 0)
					{
						string csIDs = ",";
						DataRow[] drs = dsSerial.Tables[0].Select(" bs_id=" + id + " ");
						if (drs != null && drs.Length > 0)
						{
							// 按品牌区分子品牌
							string firstCBName = "";
							string firstCpCountry = "";
							string currentCBName = "";
							int cbCount = 0;

							foreach (DataRow dr in drs)
							{
								if (currentCBName != dr["cb_name"].ToString().Trim())
								{
									// 不同品牌
									if (currentCBName == "")
									{
										firstCBName = dr["cb_name"].ToString().Trim();
										firstCpCountry = dr["CpCountry"].ToString().Trim() == "1" ? "进口" : "";
										currentCBName = dr["cb_name"].ToString().Trim();
										cbCount++;
									}
									else
									{
										if (currentCBName != dr["cb_name"].ToString().Trim())
										{
											// 有2个以上品牌
											currentCBName = dr["cb_name"].ToString().Trim();
											cbCount++;
											// currentCBName = dr["cb_name"].ToString().Trim();
											sb.Append("<optgroup label=\"" + (dr["CpCountry"].ToString() == "1" ? "进口" : "") + dr["cb_name"].ToString().Trim() + "\" style=\"background: rgb(204, 204, 204) none repeat scroll 0% 0%; font-style: normal; -moz-background-clip: border; -moz-background-origin: padding; -moz-background-inline-policy: continuous; text-align: center;\"></optgroup>");
										}
									}
								}
								else
								{
									// 同品牌

								}

								if (csIDs.IndexOf("," + dr["cs_id"].ToString().Trim() + ",") >= 0)
								{
									continue;
								}
								sb.Append("<option value=\"" + dr["cs_id"].ToString().Trim() + "\">" + dr["cs_showname"].ToString().Trim() + "</option>");
								csIDs += dr["cs_id"].ToString().Trim() + ",";
							}
							string tempFirstCB = "<optgroup label=\"" + firstCpCountry + firstCBName + "\" style=\"background: rgb(204, 204, 204) none repeat scroll 0% 0%; font-style: normal; -moz-background-clip: border; -moz-background-origin: padding; -moz-background-inline-policy: continuous; text-align: center;\"></optgroup>";
							tempXml = ("<option value=\"-1\">选择车型</option>" + (cbCount >= 2 ? tempFirstCB + sb.ToString() : sb.ToString()));
						}
					}
				}
			}
			else if (type == 3)
			{
				//  车型
				// sb.Append("<option value=\"-1\">选择车款</option>");
				if (id > 0)
				{
					string firstYear = "";
					string currentYear = "";
					int yearCount = 0;

					DataSet dsCar = cs.GetAllCarForAjaxCompare(10);
					if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
					{
						DataRow[] drs = dsCar.Tables[0].Select(" cs_id=" + id + " ");
						if (drs != null && drs.Length > 0)
						{
							foreach (DataRow dr in drs)
							{
								if (currentYear != dr["Car_YearType"].ToString().Trim())
								{
									// 不同年款
									if (currentYear == "")
									{
										currentYear = dr["Car_YearType"].ToString().Trim();
										firstYear = dr["Car_YearType"].ToString().Trim();
										yearCount++;
										// sb.Append("<optgroup label=\"" + dr["Car_YearType"].ToString().Trim() + "\" style=\"background: rgb(204, 204, 204) none repeat scroll 0% 0%; font-style: normal; -moz-background-clip: border; -moz-background-origin: padding; -moz-background-inline-policy: continuous; text-align: center;\"></optgroup>");
									}
									else
									{
										if (currentYear != dr["Car_YearType"].ToString().Trim())
										{
											// 有2个以上年款
											currentYear = dr["Car_YearType"].ToString().Trim();
											yearCount++;
											sb.Append("<optgroup label=\"" + dr["Car_YearType"].ToString().Trim() + "\" style=\"background: rgb(204, 204, 204) none repeat scroll 0% 0%; font-style: normal; -moz-background-clip: border; -moz-background-origin: padding; -moz-background-inline-policy: continuous; text-align: center;\"></optgroup>");
										}
									}
								}
								else
								{
									// 同年款
								}
								string ee = dr["Engine_Exhaust"].ToString().Trim();
								string tt = new Car_BasicBll().GetCarParamEx(int.Parse(dr["car_id"].ToString().Trim()), 712);

								sb.Append("<option title=\"排量:" + ee + " 变速箱:" + tt + "\" value=\"" + dr["car_id"].ToString().Trim() + "\">" + dr["car_name"].ToString().Trim() + "</option>");
							}
							string tempFirstYear = "<optgroup label=\"" + firstYear + "\" style=\"background: rgb(204, 204, 204) none repeat scroll 0% 0%; font-style: normal; -moz-background-clip: border; -moz-background-origin: padding; -moz-background-inline-policy: continuous; text-align: center;\"></optgroup>";
							tempXml = ("<option value=\"-1\">选择车款</option>" + (yearCount >= 2 ? tempFirstYear + sb.ToString() : sb.ToString()));
							// tempXml = sb.ToString();
						}
					}
				}
			}
			else
			{
				tempXml = cs.GetMasterToCar();
			}
			context.Response.Write(tempXml);
		}

		private void GetParam(HttpContext context)
		{
			if (context.Request.QueryString["type"] != null && context.Request.QueryString["type"].ToString() != "")
			{
				string typestr = context.Request.QueryString["type"].ToString();
				if (int.TryParse(typestr, out type))
				{
					if (type < 0 || type > 3)
					{
						type = 0;
					}
				}
			}
			if (context.Request.QueryString["id"] != null && context.Request.QueryString["id"].ToString() != "")
			{
				string idstr = context.Request.QueryString["id"].ToString();
				if (int.TryParse(idstr, out id))
				{
					if (id < 0)
					{
						id = 0;
					}
				}
			}
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