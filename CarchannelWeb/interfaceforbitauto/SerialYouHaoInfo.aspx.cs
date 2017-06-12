using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	/// 子品牌百公里油耗、综合工况油耗(熊玉辉)
	/// </summary>
	public partial class SerialYouHaoInfo : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<SerialYouHao>");
				GetSerialYouHaoData();
				sb.Append("</SerialYouHao>");
				Response.Write(sb.ToString());
			}
		}

		private void GetSerialYouHaoData()
		{
			string sql = @"select car.car_id,car.cs_id,cdb1.pvalue as Fuel100,
cdb2.pvalue as Zonghe
from dbo.Car_relation car 
left join Car_Serial cs on  cs.cs_id=car.cs_id
left join dbo.CarDataBase cdb1 on car.car_id=cdb1.carid and cdb1.paramid=658
left join dbo.CarDataBase cdb2 on car.car_id=cdb2.carid and cdb2.paramid=782
where car.isState=0 and cs.isState=0
order by cs_id";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentCsID = "";
				string minZhongHe = "0";
				string maxZhongHe = "0";
				string minBaiGongLi = "0";
				string maxBaiGongLi = "0";

				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString())
					{
						// 另一个子品牌
						if (currentCsID != "")
						{
							// 不是第一个子品牌
							sb.Append("<Serial ID=\"" + currentCsID + "\" ");
							string BaiGongLi = "";
							if (minBaiGongLi == "0")
							{
								if (maxBaiGongLi == "0")
								{
								}
								else
								{
									BaiGongLi = maxBaiGongLi + "L";
								}
							}
							else
							{
								if (maxBaiGongLi == "0")
								{
									BaiGongLi = minBaiGongLi + "";
								}
								else
								{
									BaiGongLi = minBaiGongLi + "-" + maxBaiGongLi + "L";
								}
							}

							string ZhongHe = "";
							if (minZhongHe == "0")
							{
								if (maxZhongHe == "0")
								{
								}
								else
								{
									ZhongHe = maxZhongHe + "L";
								}
							}
							else
							{
								if (maxZhongHe == "0")
								{
									ZhongHe = minZhongHe + "";
								}
								else
								{
									ZhongHe = minZhongHe + "-" + maxZhongHe + "L";
								}
							}
							sb.Append(" Fuel100=\"" + BaiGongLi + "\" ");
							sb.Append(" ZhongHe=\"" + ZhongHe + "\" />");
							currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString();
							minZhongHe = "0";
							maxZhongHe = "0";
							minBaiGongLi = "0";
							maxBaiGongLi = "0";
						}
						else
						{
							// 第一个子品牌
							currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString();
						}
					}
					else
					{
						// 同一个子品牌
					}
					// 综合工况油耗
					GetYouHao(ds.Tables[0].Rows[i]["Zonghe"].ToString().Trim(), ref minZhongHe, ref maxZhongHe);
					// 百公里油耗
					GetYouHao(ds.Tables[0].Rows[i]["Fuel100"].ToString().Trim(), ref minBaiGongLi, ref maxBaiGongLi);
				}
				string BaiGongLiEnd = "";
				if (minBaiGongLi == "0")
				{
					if (maxBaiGongLi == "0")
					{
					}
					else
					{
						BaiGongLiEnd = maxBaiGongLi + "L";
					}
				}
				else
				{
					if (maxBaiGongLi == "0")
					{
						BaiGongLiEnd = minBaiGongLi + "";
					}
					else
					{
						BaiGongLiEnd = minBaiGongLi + "-" + maxBaiGongLi + "L";
					}
				}

				string ZhongHeEnd = "";
				if (minZhongHe == "0")
				{
					if (maxZhongHe == "0")
					{
					}
					else
					{
						ZhongHeEnd = maxZhongHe + "L";
					}
				}
				else
				{
					if (maxZhongHe == "0")
					{
						ZhongHeEnd = minZhongHe + "";
					}
					else
					{
						ZhongHeEnd = minZhongHe + "-" + maxZhongHe + "L";
					}
				}
				sb.Append("<Serial ID=\"" + currentCsID + "\" ");
				sb.Append(" Fuel100=\"" + BaiGongLiEnd + "\" ");
				sb.Append(" ZhongHe=\"" + ZhongHeEnd + "\" />");
			}
		}

		private void GetYouHao(string youhao, ref string min, ref string max)
		{
			// 最小值
			if (youhao.Trim() != "")
			{
				if (min == "0")
				{
					min = youhao;
				}
				else
				{
					if (decimal.Parse(youhao) < decimal.Parse(min))
					{
						min = youhao;
					}
				}

				// 最大值
				if (max == "0")
				{
					max = youhao;
				}
				else
				{
					if (decimal.Parse(youhao) > decimal.Parse(max))
					{
						max = youhao;
					}
				}
			}
		}
	}
}