using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 根据价格区间、级别 取子品牌
	/// </summary>
	public partial class RangeSerialList : PageBase
	{

		#region member
		private string callback = string.Empty;
		private string jsonVarName = string.Empty;
		private List<string> jsonString = new List<string>();
		EnumCollection.FlagsSerialPrice fsp = EnumCollection.FlagsSerialPrice.All;
		EnumCollection.FlagsSerialLeve fsl = EnumCollection.FlagsSerialLeve.全部;
		List<XmlElement> listCs = new List<XmlElement>();
		private bool isHasNoSale = false;	// 是否包含停销子品牌
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			this.Response.ContentType = "application/x-javascript";
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetCsRangeJsonData();
				ResponseJson();
			}
		}

		#region private Method

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			// 价格区间
			int priceRange = 0;
			string strpriceRange = this.Request.QueryString["priceRange"];
			if (!string.IsNullOrEmpty(strpriceRange) && int.TryParse(strpriceRange, out priceRange))
			{
				fsp = (EnumCollection.FlagsSerialPrice)priceRange;
			}

			// 级别
			int levelRange = 0;
			string strlevelRange = this.Request.QueryString["levelRange"];
			if (!string.IsNullOrEmpty(strlevelRange) && int.TryParse(strlevelRange, out levelRange))
			{
				fsl = (EnumCollection.FlagsSerialLeve)levelRange;
			}

			// 是否包含停销子品牌
			string strsale = this.Request.QueryString["sale"];
			if (!string.IsNullOrEmpty(strsale) && (strsale.ToLower() == "sale"))
			{ isHasNoSale = true; }

			callback = this.Request.QueryString["callback"];

			jsonVarName = this.Request.QueryString["jsonVarName"];
		}

		/// <summary>
		/// 取子品牌对比数据
		/// </summary>
		private void GetCsRangeJsonData()
		{
			XmlDocument doc = AutoStorageService.GetAllAutoXml();
			if (doc != null && doc.HasChildNodes)
			{
				XmlNodeList xnlCs = doc.SelectNodes("/Params/MasterBrand/Brand/Serial");
				foreach (XmlElement xe in xnlCs)
				{
					CheckNodeIsNeed(xe);
				}


				if (listCs != null && listCs.Count > 0)
				{
					listCs.Sort(NodeCompare.CompareSerialByPvDesc);

					foreach (XmlElement xe in listCs)
					{
						if (jsonString.Count > 0)
						{ jsonString.Add(","); }
						jsonString.Add("{");
						jsonString.Add("CsID:\"" + xe.GetAttribute("ID") + "\"");
						jsonString.Add(",Name:\"" + xe.GetAttribute("Name").Replace("\"","") + "\"");
						jsonString.Add(",ShowName:\"" + xe.GetAttribute("ShowName").Replace("\"", "") + "\"");
						jsonString.Add(",CsLevel:\"" + xe.GetAttribute("CsLevel") + "\"");
						jsonString.Add(",MultiPriceRange:\"" + xe.GetAttribute("MultiPriceRange") + "\"");
						jsonString.Add(",BodyType:\"" + xe.GetAttribute("BodyType") + "\"");
						jsonString.Add(",SaleState:\"" + xe.GetAttribute("CsSaleState") + "\"");
						jsonString.Add(",CsPV:\"" + xe.GetAttribute("CsPV") + "\"");
						jsonString.Add(",AllSpell:\"" + xe.GetAttribute("AllSpell") + "\"");
						jsonString.Add("}");
					}
				}
			}
		}

		/// <summary>
		/// 检查节点是否满足条件
		/// </summary>
		/// <param name="xe"></param>
		private void CheckNodeIsNeed(XmlElement xe)
		{
			bool isCheckPrice = false;
			bool isCheckLevel = false;
			bool isCheckSale = true;

			string multiPriceRange = xe.GetAttribute("MultiPriceRange");
			string level = xe.GetAttribute("CsLevel");
			string saleState = xe.GetAttribute("CsSaleState");

			#region 销售状态
			if (!isHasNoSale)
			{
				// 只取在销子品牌
				if (saleState == "停销")
				{ isCheckSale = false; }
			}
			#endregion

			#region 级别
			if (fsl == EnumCollection.FlagsSerialLeve.全部)
			{
				isCheckLevel = true;
			}
			else
			{
				// 有级别条件
				if (level == "MPV" &&
					((fsl & EnumCollection.FlagsSerialLeve.MPV) == EnumCollection.FlagsSerialLeve.MPV))
				{ isCheckLevel = true; }
				if (level == "SUV" &&
					((fsl & EnumCollection.FlagsSerialLeve.SUV) == EnumCollection.FlagsSerialLeve.SUV))
				{ isCheckLevel = true; }
				if (level == "豪华车" &&
					((fsl & EnumCollection.FlagsSerialLeve.豪华车) == EnumCollection.FlagsSerialLeve.豪华车))
				{ isCheckLevel = true; }
				if (level == "紧凑型" &&
					((fsl & EnumCollection.FlagsSerialLeve.紧凑型车) == EnumCollection.FlagsSerialLeve.紧凑型车))
				{ isCheckLevel = true; }
				if (level == "面包车" &&
					((fsl & EnumCollection.FlagsSerialLeve.面包车) == EnumCollection.FlagsSerialLeve.面包车))
				{ isCheckLevel = true; }
				if (level == "跑车" &&
					((fsl & EnumCollection.FlagsSerialLeve.跑车) == EnumCollection.FlagsSerialLeve.跑车))
				{ isCheckLevel = true; }
				if (level == "皮卡" &&
					((fsl & EnumCollection.FlagsSerialLeve.皮卡) == EnumCollection.FlagsSerialLeve.皮卡))
				{ isCheckLevel = true; }
				if (level == "其它" &&
					((fsl & EnumCollection.FlagsSerialLeve.其他) == EnumCollection.FlagsSerialLeve.其他))
				{ isCheckLevel = true; }
				if (level == "微型车" &&
					((fsl & EnumCollection.FlagsSerialLeve.微型车) == EnumCollection.FlagsSerialLeve.微型车))
				{ isCheckLevel = true; }
				if (level == "小型车" &&
					((fsl & EnumCollection.FlagsSerialLeve.小型车) == EnumCollection.FlagsSerialLeve.小型车))
				{ isCheckLevel = true; }
				if (level == "中大型" &&
					((fsl & EnumCollection.FlagsSerialLeve.中大型车) == EnumCollection.FlagsSerialLeve.中大型车))
				{ isCheckLevel = true; }
				if (level == "中型车" &&
					((fsl & EnumCollection.FlagsSerialLeve.中型车) == EnumCollection.FlagsSerialLeve.中型车))
				{ isCheckLevel = true; }
			}
			#endregion

			#region 价格区间
			if (fsp == EnumCollection.FlagsSerialPrice.All)
			{
				isCheckPrice = true;
			}
			else
			{
				// 有价格区间条件
				if (multiPriceRange.IndexOf(",1,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P5) == EnumCollection.FlagsSerialPrice.P5))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",2,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P5_8) == EnumCollection.FlagsSerialPrice.P5_8))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",3,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P8_12) == EnumCollection.FlagsSerialPrice.P8_12))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",4,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P12_18) == EnumCollection.FlagsSerialPrice.P12_18))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",5,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P18_25) == EnumCollection.FlagsSerialPrice.P18_25))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",6,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P25_40) == EnumCollection.FlagsSerialPrice.P25_40))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",7,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P40_80) == EnumCollection.FlagsSerialPrice.P40_80))
				{ isCheckPrice = true; }
				if (multiPriceRange.IndexOf(",8,") >= 0 &&
					((fsp & EnumCollection.FlagsSerialPrice.P80) == EnumCollection.FlagsSerialPrice.P80))
				{ isCheckPrice = true; }
			}
			#endregion

			// 价格区间、级别、销售状态 都满足条件
			if (isCheckPrice && isCheckLevel && isCheckSale)
			{
				listCs.Add(xe);
			}
		}

		/// <summary>
		/// 输出
		/// </summary>
		private void ResponseJson()
		{
			if (jsonString.Count > 0)
			{
				jsonString.Insert(0, "[");
				jsonString.Add("]");

				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Insert(0, callback + "(");
					jsonString.Add(")");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Insert(0, "var " + jsonVarName + " = ");
					jsonString.Add(";");
				}
				else
				{ }
			}
			else
			{
				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Add(callback + "({})");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Add("var " + jsonVarName + " = null;");
				}
				else
				{ jsonString.Add("参数错误"); }
			}
			Response.Write(string.Concat(jsonString.ToArray()));
		}

		#endregion

	}
}