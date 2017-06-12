using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannelAPI.Web.AppCode;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetSerialOverview 的摘要说明
	/// 老接口迁移至此 http://car.bitauto.com/interface/getserialoverview.aspx?SerialId=2608 
	/// for 车易搜 liuyk
	/// </summary>
	public class GetSerialOverview : PageBase, IHttpHandler
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

			switch (op)
			{
				case "cheyisou":
				case "bitautocms":
					RenderCheYiSouData(); break;
				default: CommonFunction.EchoXml(response, "<!-- 缺少参数 -->", "Root"); break;
			}

		}

		/// <summary>
		/// 车易搜子品牌数据接口
		/// </summary>
		private void RenderCheYiSouData()
		{
			int csid = 0;
			int noSeeAlso = 0;
			Int32.TryParse(request.QueryString["SerialId"], out csid);
			Int32.TryParse(request.QueryString["NoSeeAlso"], out noSeeAlso);
			if (csid > 0)
			{
				EnumCollection.SerialInfoCard sic = new Car_SerialBll().GetSerialInfoCard(csid);

				if (sic.CsID <= 0)
				{
					CommonFunction.EchoXml(response, "<!-- 无效数据 -->", "Root");
					return;
				}

				sb.AppendFormat("<SerialOverview SerialId=\"{0}\" AllSpell=\"{1}\" Name=\"{2}\" ShowName=\"{3}\">"
					, csid, sic.CsAllSpell, sic.CsName, sic.CsShowName);

				#region 报价
				string serialPrice = sic.CsPriceRange;
				if (serialPrice.Length == 0)
					serialPrice = "暂无报价";
				sb.AppendFormat("<PriceRange>{0}</PriceRange>", serialPrice);
				#endregion

				#region 指导价
				List<EnumCollection.CarInfoForSerialSummary> ls = base.GetAllCarInfoForSerialSummaryByCsID(csid);
				double maxPrice = Double.MinValue;
				double minPrice = Double.MaxValue;
				foreach (EnumCollection.CarInfoForSerialSummary carInfo in ls)
				{
					double referPrice = 0.0;
					bool isDouble = Double.TryParse(carInfo.ReferPrice.Replace("万", ""), out referPrice);
					if (isDouble)
					{
						if (referPrice > maxPrice)
							maxPrice = referPrice;
						if (referPrice < minPrice)
							minPrice = referPrice;
					}
				}
				string serialReferPrice = "";
				if (maxPrice == Double.MinValue && minPrice == Double.MaxValue)
					serialReferPrice = "暂无";
				else
				{
					serialReferPrice = minPrice + "万-" + maxPrice + "万";
				}
				sb.AppendFormat("<OfficalPrice>{0}</OfficalPrice>", serialReferPrice);
				#endregion

				#region 颜色
				string rgbHTML = "";
				string rgbTitle = "";
				List<string> listColorName = new List<string>();
				List<string> listColorRGB = new List<string>();
				new Car_SerialBll().GetSerialColorRGBByCsID(sic.CsID, 0, 1, sic.ColorList
					, out rgbHTML, out rgbTitle, out listColorName, out listColorRGB);
				if (listColorName.Count > 0 && listColorRGB.Count > 0)
				{
					sb.AppendFormat("<Color>{0}</Color>"
									   , System.Security.SecurityElement.Escape(string.Join(",", listColorName.ToArray())));

					sb.AppendFormat("<ColorRGB>{0}</ColorRGB>"
									   , System.Security.SecurityElement.Escape(string.Join(",", listColorRGB.ToArray())));
				}
				#endregion

				#region  保修政策
				sb.AppendFormat("<RepairPolicy>{0}</RepairPolicy>"
					, System.Security.SecurityElement.Escape(sic.SerialRepairPolicy));
				#endregion

				#region 排量
				string tempExhaust = "";
				DataRow[] ExhaustArr = new Car_BasicBll().GetCarParamEx(sic.CsID, 785, false, " pvalue Asc");
				List<string> tmpList = new List<string>();
				if (ExhaustArr != null && ExhaustArr.Length > 0)
				{
					foreach (DataRow dr in ExhaustArr)
					{
						string exStr = dr["pvalue"].ToString();
						//去重
						if (tmpList.Contains(exStr))
							continue;
						tmpList.Add(exStr);
						if (tempExhaust != "")
						{
							tempExhaust += "、" + exStr + "L";
						}
						else
						{
							tempExhaust = exStr + "L";
						}
					}
				}
				sb.AppendFormat("<Exhaust>{0}</Exhaust>", System.Security.SecurityElement.Escape(tempExhaust));
				#endregion

				#region 变速器
				DataRow[] TransmissionArr = new Car_BasicBll().GetCarParamEx(sic.CsID, 712, false, "");
				if (TransmissionArr != null && TransmissionArr.Length > 0)
				{
					tmpList.Clear();
					foreach (DataRow dr in TransmissionArr)
					{
						string tranStr = dr["pvalue"].ToString();

						if (tranStr.IndexOf("手动") > -1)
							tranStr = "手动";
						else
							tranStr = "自动";

						//去重
						if (tmpList.Contains(tranStr))
							continue;
						tmpList.Add(tranStr);

						if (tmpList.Count >= 2)
							break;
					}
				}
				sb.AppendFormat("<TranmissionType>{0}</TranmissionType>"
					, System.Security.SecurityElement.Escape(String.Join("、", tmpList.ToArray())));
				#endregion

				#region 综合油耗
				sb.AppendFormat("<Fuel>{0}</Fuel>"
					, System.Security.SecurityElement.Escape(sic.CsSummaryFuelCost));
				#endregion

				#region 官方油耗
				sb.AppendFormat("<OfficalFuel>{0}</OfficalFuel>"
					, System.Security.SecurityElement.Escape(sic.CsOfficialFuelCost));
				#endregion

				#region 网友发布油耗
				sb.AppendFormat("<NetFuel>{0}</NetFuel>"
					, System.Security.SecurityElement.Escape(sic.CsGuestFuelCost));
				#endregion

				#region 子品牌论坛
				sb.AppendFormat("<ForumUrl>{0}</ForumUrl>"
					, System.Security.SecurityElement.Escape(new Car_SerialBll().GetForumUrlBySerialId(csid)));
				#endregion

				#region 非白底
				// 非白底
				Dictionary<int, string> dicPicNoneWhite = base.GetAllSerialPicURLNoneWhiteBackground();
				sb.AppendFormat("<ImgUrl>{0}</ImgUrl>"
					, System.Security.SecurityElement.Escape(dicPicNoneWhite.ContainsKey(csid) ? dicPicNoneWhite[csid] : WebConfig.DefaultCarPic));
				//add by sk 2015.04.09 白底图
				sb.AppendFormat("<WhiteImgUrl>{0}</WhiteImgUrl>"
					, System.Security.SecurityElement.Escape(Car_SerialBll.GetSerialImageUrl(csid, "2")));
				#endregion

				#region 还看过
				//还看过的子品牌
				if (noSeeAlso != 1)
				{
					List<EnumCollection.SerialToSerial> lsts = base.GetSerialToSerialByCsID(csid, 6);
					if (lsts.Count > 0)
					{
						sb.AppendLine("<AlsoSeeSerial>");
						foreach (EnumCollection.SerialToSerial sts in lsts)
						{
							sb.AppendFormat("<Serial ID=\"{0}\" Name=\"{1}\" ShowName=\"{2}\" AllSpell=\"{3}\" PriceRange=\"{4}\" Image=\"{5}\"/>"
					, sts.ToCsID
					, System.Security.SecurityElement.Escape(sts.ToCsName)
					, System.Security.SecurityElement.Escape(sts.ToCsShowName)
					, System.Security.SecurityElement.Escape(sts.ToCsAllSpell)
					, System.Security.SecurityElement.Escape(sts.ToCsPriceRange)
					, System.Security.SecurityElement.Escape(sts.ToCsPic));
						}
						sb.AppendLine("</AlsoSeeSerial>");
					}
				}
				#endregion

				sb.AppendLine("</SerialOverview>");
				CommonFunction.EchoXml(response, sb.ToString(), "");
			}
			else
			{ CommonFunction.EchoXml(response, "<!-- 缺少子品牌ID参数 -->", "Root"); }
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