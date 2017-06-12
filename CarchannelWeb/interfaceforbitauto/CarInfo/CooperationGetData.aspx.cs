using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.CarInfo
{
	/// <summary>
	/// 对外合作接口 
	/// dept = qirui (奇瑞)
	/// </summary>
	public partial class CooperationGetData : InterfacePageBase
	{
		#region member
		private string dept = "";
		private StringBuilder sb = new StringBuilder();
		// 车型显示参数ID
		private readonly string paramConfig = "398,436,423,430,433,429,432,418,428,425,408,417,437,415,434,414,416,421,785,791,712,724,576,578,577,580,465,466,548,547,552,534,518,500,531,553,798,799,545,702,703,528,800,893,574,563,581,582,585,586,588,589,591,592,593,669,668,616,595,627,598,597,441,567,821,649,637,663,659,658,650,653,665,661,662,590,782,783,784,854,855,862,890,699,683,682,697,690,679,691,680,677,678,701,675,676,681,713,494,469,538,835,836,837,493,735,726,718,716,728,720,704,729,721,707,655,490,489,510,509,559,488,477,473,523,809,810,516,554,479,806,816,817,818,819,820,841,842,542,544,546,543,504,506,508,507,505,503,502,514,475,481,483,495,482,801,804,805,803,672,673,700,684,685,687,698,714,811,812,813,815,470,471,555,478,485,838,839,840,614,613,611,607,609,612,608,626,618,620,794,795,846,796,797,601,594,600,621,622,623,624,625,596,606,512,617,786,787,788,789,857,790,858,859,860,861,888,889,892";
		// 子品牌
		private readonly string sqlCsTemp = @"select cs.cs_id,cs.csname,cs.csshowname,cb.cb_name,cmb.bs_name,cl.classvalue as carlevel
					from car_serial cs
					left join car_brand cb on cs.cb_id=cb.cb_id
					left join Car_MasterBrand_Rel cmbr on cb.cb_id=cmbr.cb_id and cmbr.isState=0
					left join Car_MasterBrand cmb on cmb.bs_id=cmbr.bs_id
					left join class cl on cs.carlevel=cl.classid
					where cs.isState=0 
					and cs.cs_id in ({0})";
		// 车型
		private readonly string sqlCarTemp = @"select car.car_id,car.car_name,car.cs_id
						from car_relation car
						left join car_serial cs on car.cs_id=cs.cs_id
						where car.isState=0 and cs.isState=0 
						and car.car_SaleState in (95,97)
						and car.cs_id in ({0})
						order by car.cs_id,car.car_id";
		private string csidList = "";
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				if (dept == "qirui")
				{
					csidList = "3250,3251,3252,3373";
					GetDataForQiRui();
				}
				else if (dept == "changcheng")
				{
					// add by chengl Oct.11.2012
					csidList = "3383,3023,3103,1650,1649,3263,1648,2865,3152,2833,2866,3382,2675,1635,2683,2767,2587,2857";
					GetDataForQiRui();
				}
				Response.Write(sb.ToString());
			}
		}

		#region private Method

		/// <summary>
		/// 取页面参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}
		}

		#region 对外合作接口 奇瑞

		/// <summary>
		/// 对外合作接口 奇瑞 dept = qirui
		/// </summary>
		private void GetDataForQiRui()
		{

			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<Root>");

			#region 子品牌信息
			sb.AppendLine("<CsList>");
			string sqlCsInfo = string.Format(sqlCsTemp, csidList);
			DataSet dsCsInfo = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCsInfo);

			Dictionary<int, string> dicPic = base.GetAllSerialPicURLWhiteBackground();

			if (dsCsInfo != null && dsCsInfo.Tables.Count > 0 && dsCsInfo.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsCsInfo.Tables[0].Rows)
				{
					int csid = int.Parse(dr["cs_id"].ToString());
					sb.Append("<Cs ID=\"" + csid + "\" ");
					sb.Append(" Name=\"" + dr["csname"].ToString().Trim() + "\" ");
					sb.Append(" ShowName=\"" + dr["csshowname"].ToString().Trim() + "\" ");
					sb.Append(" CbName=\"" + dr["cb_name"].ToString().Trim() + "\" ");
					sb.Append(" BsName=\"" + dr["bs_name"].ToString().Trim() + "\" ");
					sb.Append(" ReferPrice=\"" + GetSaleCarRange(csid) + "\" ");
					string tempDealerPrice = base.GetSerialPriceRangeByID(csid);
					sb.Append(" DealerPrice=\"" + (tempDealerPrice == "" ? "暂无" : tempDealerPrice) + "\" ");
					sb.Append(" CarLevel=\"" + dr["carlevel"].ToString().Trim() + "\" ");
					if (dicPic.ContainsKey(csid))
					{
						sb.Append(" Img=\"" + dicPic[csid] + "\"");
					}
					else
					{
						sb.Append(" Img=\"\"");
					}
					sb.AppendLine("/>");
				}
			}

			sb.AppendLine("</CsList>");
			#endregion

			#region 车型信息
			sb.AppendLine("<CarList>");
			string sqlCar = string.Format(sqlCarTemp, csidList);
			DataSet dsCar = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.AutoStorageConnectionString, CommandType.Text, sqlCar);
			if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsCar.Tables[0].Rows)
				{
					int carid = int.Parse(dr["car_id"].ToString());
					string carName = dr["car_name"].ToString().Trim();
					int csid = int.Parse(dr["cs_id"].ToString());
					sb.Append("<Car ID=\"" + carid + "\" ");
					sb.Append(" Name=\"" + System.Security.SecurityElement.Escape(carName) + "\" ");
					sb.Append(" CsID=\"" + csid + "\" ");
					sb.AppendLine(">");
					#region 扩展参数
					sb.AppendLine("<Param>");
					sb.Append(GetCarParambyCarID(carid));
					sb.AppendLine("</Param>");
					#endregion
					sb.AppendLine("</Car>");
				}
			}
			sb.AppendLine("</CarList>");
			#endregion

			sb.Append("</Root>");

		}

		/// <summary>
		/// 取在销车型的指导价区间
		/// </summary>
		/// <param name="csid">子品牌ID</param>
		/// <returns></returns>
		private string GetSaleCarRange(int csid)
		{
			string referPrice = "暂无";
			string sql = @"select car.cs_id,
					Round(min(car_referprice),2) as minprice,
					Round(max(car_referprice),2) as maxprice 
					from dbo.Car_relation car
					left join car_serial cs on car.cs_id=cs.cs_id
					where car.cs_id = {0} and car.isState=0
					and car.car_SaleState=95
					and cs.isState=0 and car.car_ReferPrice>0
					group by car.cs_id";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.AutoStorageConnectionString, CommandType.Text, string.Format(sql, csid));
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				decimal minP = 0;
				decimal maxP = 0;
				if (decimal.TryParse(ds.Tables[0].Rows[0]["minprice"].ToString(), out minP))
				{ }
				if (decimal.TryParse(ds.Tables[0].Rows[0]["maxprice"].ToString(), out maxP))
				{ }
				if (minP > 0 && maxP >= minP)
				{
					referPrice = minP.ToString() + "万-" + maxP.ToString() + "万";
				}
			}
			return referPrice;
		}

		/// <summary>
		/// 根据车型ID取车型扩展参数
		/// </summary>
		/// <param name="carid"></param>
		/// <returns></returns>
		private string GetCarParambyCarID(int carid)
		{
			List<string> listTemp = new List<string>();
			string sqlCarParam = @"select cdb.carid,cdb.paramid,cdb.pvalue,pl.paramname,pl.ModuleDec
								from cardatabase cdb
								left join paramlist pl on cdb.paramid=pl.paramid
								where cdb.carid={0}";
			DataSet dsCarParam = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.AutoStorageConnectionString, CommandType.Text, string.Format(sqlCarParam, carid));
			if (dsCarParam != null && dsCarParam.Tables.Count > 0 && dsCarParam.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsCarParam.Tables[0].Rows)
				{
					int paramid = int.Parse(dr["paramid"].ToString());
					string paramName = dr["paramname"].ToString().Trim();
					string paramValue = dr["pvalue"].ToString().Trim();
					string Unit = dr["ModuleDec"].ToString().Trim();
					if (paramConfig.IndexOf(paramid.ToString()) >= 0)
					{
						listTemp.Add("<Item Pid=\"" + paramid + "\" ");
						listTemp.Add(" PName=\"" + System.Security.SecurityElement.Escape(paramName) + "\" ");
						listTemp.Add(" PValue=\"" + System.Security.SecurityElement.Escape(paramValue) + "\" ");
						listTemp.Add(" Unit=\"" + System.Security.SecurityElement.Escape(Unit) + "\" />");
					}
				}
			}
			return string.Concat(listTemp.ToArray());
		}

		#endregion

		#endregion
	}
}