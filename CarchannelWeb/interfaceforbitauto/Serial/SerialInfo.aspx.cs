using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto.Serial
{
	/// <summary>
	/// 取子品牌信息
	/// dept:使用业务类型(ucar:赵国雄,citysite:城市站,serialInfoForNews:胡利军 新闻页,search:子品牌所有组合名
	/// cmsjinkouserialrankrate:旭旭进口子品牌前10的排行变化
	/// photo:图库 按主品牌、品牌、子品牌 取 VCar_SerialToBrandToMasterBrand 视图的相应数据
	/// checkdata:检查新增子品牌 op=addcs 刘荣伟检测系统
	/// )
	/// csID:子品牌ID
	/// </summary>
	public partial class SerialInfo : InterfacePageBase
	{
		/// <summary>
		/// 子品牌油耗数量
		/// </summary>
		private readonly string YouHaoCountInterface = "http://koubei.bitauto.com/Api/Bitauto/HandlerModelFuel.ashx";
		private string dept = "";
		private int csID = 0;
		private StringBuilder sb = new StringBuilder();
		private string type = ""; // 取层级类型: MB:主品牌，CB:品牌，CS:子品牌
		private int typeID = 0; // 取层级ID

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				GetPageParam();
				GetSerialInfoBydDeptAndCsID();
				Response.Write(sb.ToString());
			}
		}

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["dept"] != null && this.Request.QueryString["dept"].ToString() != "")
			{
				dept = this.Request.QueryString["dept"].ToString().ToLower();
			}

			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string strCsID = this.Request.QueryString["csID"].ToString();
				if (int.TryParse(strCsID, out csID))
				{ }
			}
		}

		/// <summary>
		/// 根据业务和子品牌ID取子品牌信息
		/// </summary>
		private void GetSerialInfoBydDeptAndCsID()
		{
			////if (dept == "citysite")
			////{
			////    // 城市站子品牌地址
			////    // GetAllOnSaleSerialLinkForCity();
			////}
			////else 
			if (dept == "ucar")
			{
				base.SetPageCache(10);
				if (csID > 0)
				{
					// 二手车
					GetSerialInfoByCsIDForUCar();
				}
			}
			else if (dept == "serialinfofornews")
			{
				base.SetPageCache(10);
				if (csID > 0)
				{
					// 胡利军 新闻页
					GetSerialInfoForNews();
				}
			}
			else if (dept == "search")
			{
				base.SetPageCache(10);
				// 所有子品牌的组合名
				GetAllSerialNameList();
			}
			else if (dept == "cmsjinkouserialrankrate")
			{
				base.SetPageCache(10);
				// cmsserialrank:旭旭进口子品牌前10的排行变化
				GetCMSJinKouSerialRankRate();
			}
			else if (dept == "photo")
			{
				GetCarBrandForPhoto();
			}
			else if (dept == "checkdata")
			{
				// 检查
				CheckSerial();
			}
			else
			{ }
		}
		//获取主品牌、品牌、子品牌信息
		private void GetCarBrandForPhoto()
		{
			string type = ConvertHelper.GetString(Request.QueryString["type"]);
			int typeId = ConvertHelper.GetInteger(Request.QueryString["typeid"]);
			Response.ContentType = "text/xml";
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<CarInfo>");
			if (string.IsNullOrEmpty(type) ||
				Array.IndexOf(new string[] { "mb", "cb", "cs" }, type) == -1)
			{
				sb.AppendLine("</CarInfo>");
				Response.Write(sb.ToString());
				return;
			}
			string strField = string.Empty, strWhere = string.Empty;
			switch (type.ToLower())
			{
				case "mb":
					strField = "distinct bs_id AS bsId,bs_Name,bsSpell,bsIsState,bsallSpell,bsSeoName";
					strWhere = "WHERE bs_id=@typeid";
					break;
				case "cb":
					strField = "distinct cb_id AS cbId,cb_Name,cbSpell,cbAllSpell,cbSeoName,cbIsState,cbCountry,bs_id AS bsId";
					strWhere = "WHERE cb_id=@typeid";
					break;
				case "cs":
					strField = "cs_id as csId,csName,csSpell,csIsState,csSaleState,csShowName,csSeoName,csAllSpell,cb_id AS cbId,carlevel";
					strWhere = "WHERE cs_id=@typeid";
					break;
			}
			if (typeId <= 0)
				strWhere = "";
			string sql = string.Format(@"SELECT {0} FROM VCar_SerialToBrandToMasterBrand {1} ", strField, strWhere);
			SqlParameter[] param = { 
							   new SqlParameter("@typeid",SqlDbType.Int)
							   };
			param[0].Value = typeId;
			DataTable dt = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, param).Tables[0];

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				sb.Append("<item>");
				foreach (DataColumn dc in dt.Columns)
				{
					sb.AppendFormat("<{0}>{1}</{0}>", dc.ColumnName, dt.Rows[i][dc.ColumnName]);
				}
				sb.Append("</item>");
			}
			sb.AppendLine("</CarInfo>");
			Response.Write(sb.ToString());
		}
		#region 业务

		private void CheckSerial()
		{
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialInfo>");
			string op = this.Request.QueryString["op"];
			if (!string.IsNullOrEmpty(op))
			{
				if (op.ToLower() == "addcs")
				{
					DateTime dt = new DateTime();
					string date = this.Request.QueryString["date"];
					if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out dt))
					{
						string sql = @"select cs_id,csName,csShowname,cs_Seoname,allSpell,createtime
					from car_serial where isState=0 and createtime>='{0}'";
						DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
							WebConfig.AutoStorageConnectionString, CommandType.Text
							, string.Format(sql, dt.ToString()));
						if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
						{
							foreach (DataRow dr in ds.Tables[0].Rows)
							{
								sb.AppendLine("<Cs ID=\"" + dr["cs_id"].ToString() + "\" ");
								sb.Append("Name=\"" + System.Security.SecurityElement.Escape(dr["csName"].ToString().Trim()) + "\" ");
								sb.Append("Showname=\"" + System.Security.SecurityElement.Escape(dr["csShowname"].ToString().Trim()) + "\" ");
								sb.Append("Seoname=\"" + System.Security.SecurityElement.Escape(dr["cs_Seoname"].ToString().Trim()) + "\" ");
								sb.Append("allSpell=\"" + System.Security.SecurityElement.Escape(dr["allSpell"].ToString().Trim()) + "\" />");
							}
						}

					}
				}
			}
			sb.Append("</SerialInfo>");
		}

		/// <summary>
		/// 取所有子品牌的组合名
		/// </summary>
		private void GetAllSerialNameList()
		{
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialInfo>");

			DataSet ds = GetSerialName();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.AppendLine("<CS ID=\"" + dr["cs_id"].ToString() + "\"");
					sb.AppendLine(" Name=\"" + GetSerialAllGroupName(dr) + "\" />");
				}
			}

			sb.Append("</SerialInfo>");
		}

		/// <summary>
		/// 取2手车子品牌信息
		/// </summary>
		private void GetSerialInfoByCsIDForUCar()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialInfo>");

			EnumCollection.SerialInfoCard sic = base.GetSerialInfoCardByCsID(csID);
			if (sic.CsID > 0)
			{
				sb.Append("<CsID>" + sic.CsID.ToString() + "</CsID>");
				sb.Append("<CsName>" + sic.CsName + "</CsName>");
				sb.Append("<CsShowName>" + sic.CsShowName + "</CsShowName>");
				sb.Append("<CsPriceRange>" + sic.CsPriceRange + "</CsPriceRange>");
				sb.Append("<CsLevel>" + sic.CsLevel + "</CsLevel>");

				string[] exhaustList = sic.CsEngineExhaust.Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
				StringBuilder sbExhaust = new StringBuilder();
				Array.Sort(exhaustList);
				foreach (string el in exhaustList)
				{
					if (sbExhaust.Length > 0)
					{ sbExhaust.Append("|"); }
					sbExhaust.Append(el);
				}

				sb.Append("<CsEngineExhaust>" + sbExhaust.ToString() + "</CsEngineExhaust>");
				// 综合工况
				sb.Append("<CsSummaryFuelCost>" + sic.CsSummaryFuelCost + "</CsSummaryFuelCost>");
				// 网友油耗
				sb.Append("<CsGuestFuelCost>" + sic.CsGuestFuelCost + "</CsGuestFuelCost>");
				// 取燃料类型
				sb.Append("<CsOilFuelType>" + GetStringByCsIDAndParamID(sic.CsID, 578, "|") + "</CsOilFuelType>");
				// 取燃油型号
				sb.Append("<CsOilFuelTab>" + GetStringByCsIDAndParamID(sic.CsID, 577, "|") + "</CsOilFuelTab>");
				// 取变速箱类型
				sb.Append("<CsTransmissionType>" + GetStringByCsIDAndParamID(sic.CsID, 712, "|") + "</CsTransmissionType>");
				// 油耗
				// sb.Append("<CsOfficialFuelCost>" + sic.CsOfficialFuelCost + "</CsOfficialFuelCost>");
				// 用途
				StringBuilder sbpurpose = new StringBuilder();
				List<string> purposeList = sic.PurposeList;
				if (purposeList.Count > 0)
				{
					foreach (string pl in purposeList)
					{
						if (sbpurpose.Length > 0)
						{
							sbpurpose.Append("|");
						}
						sbpurpose.Append(pl);
					}
				}
				sb.Append("<CsPurpose>" + sbpurpose.ToString() + "</CsPurpose>");
			}
			sb.Append("</SerialInfo>");
		}

		/// <summary>
		/// 
		/// </summary>
		private void GetSerialInfoForNews()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialInfo>");

			EnumCollection.SerialInfoCard sic = new EnumCollection.SerialInfoCard();
			sic = base.GetSerialInfoCardByCsID(csID);

			if ((sic.CsID > 0) && (sic.CsLevel != "概念车"))
			{
				Car_SerialEntity cse = new Car_SerialBll().GetSerialInfoEntity(csID);
				sb.AppendLine("<CsID>" + sic.CsID.ToString() + "</CsID>");
				sb.AppendLine("<CsAllSpell>" + sic.CsAllSpell.ToLower() + "</CsAllSpell>");
				sb.AppendLine("<CsName>" + sic.CsName + "</CsName>");
				sb.AppendLine("<CsShowName>" + sic.CsShowName + "</CsShowName>");
				// sb.AppendLine("<CsSEOName>" + cse.Cs_SeoName.Trim() + "</CsSEOName>");
				sb.AppendLine("<CsImg>" + sic.CsDefaultPic.Replace("_2.", "_1.") + "</CsImg>");
				sb.AppendLine("<CsPriceRange>" + sic.CsPriceRange + "</CsPriceRange>");
				sb.AppendLine("<CsLevel>" + sic.CsLevel + "</CsLevel>");

				sb.AppendLine("<CbID>" + cse.Cb_Id.ToString() + "</CbID>");
				sb.AppendLine("<CbName>" + cse.Cb_Name.ToString().Trim() + "</CbName>");
				sb.AppendLine("<CbAllSpell>" + cse.Cb_AllSpell.ToString().Trim().ToLower() + "</CbAllSpell>");

				sb.AppendLine("<LinkList>");
				sb.AppendLine("<Link Name=\"综述\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/\" Count=\"\" />");
				sb.AppendLine("<Link Name=\"参数配置\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/peizhi/\" Count=\"\" />");
				sb.AppendLine("<Link Name=\"图片\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/tupian/\" Count=\"" + sic.CsPicCount.ToString() + "\" />");
				sb.AppendLine("<Link Name=\"报价\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/baojia/\" Count=\"" + GetPriceCountByCsID(csID) + "\" />");
				sb.AppendLine("<Link Name=\"口碑\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/koubei/\" Count=\"" + sic.CsDianPingCount + "\" />");
				sb.AppendLine("<Link Name=\"车型详解\" URL=\"" + GetSerialNewsURLByCsID(csID, sic.CsAllSpell.ToLower()) + "\" Count=\"\" />");
				sb.AppendLine("<Link Name=\"油耗\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/youhao/\" Count=\"" + GetYouHaoCountByCsID(csID) + "\" />");
				sb.AppendLine("<Link Name=\"问答\" URL=\"http://ask.bitauto.com/browse/" + sic.CsID.ToString() + "/\" Count=\"" + sic.CsAskCount.ToString() + "\" />");
				string baaUrl = new Car_SerialBll().GetForumUrlBySerialId(csID);
				sb.AppendLine("<Link Name=\"论坛\" URL=\"" + baaUrl + "\" Count=\"\" />");
				sb.AppendLine("<Link Name=\"视频\" URL=\"http://car.bitauto.com/" + sic.CsAllSpell.ToLower() + "/shipin/\" Count=\"" + GetSerialNewsCountByCsIDAndName(csID, "video") + "\" />");
				sb.AppendLine("</LinkList>");
			}

			sb.AppendLine("</SerialInfo>");
		}

		/// <summary>
		/// 取子品牌报价数
		/// </summary>
		/// <param name="csid"></param>
		/// <returns></returns>
		private string GetPriceCountByCsID(int csid)
		{
			string priceCount = "";
			DataSet dsPrice = GetAllSerialPrice();
			if (dsPrice != null && dsPrice.Tables.Count > 0 && dsPrice.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = dsPrice.Tables[0].Select(" Id='" + csid.ToString() + "'");
				if (drs != null && drs.Length > 0)
				{
					priceCount = drs[0]["PriceCount"].ToString();
				}
			}
			return priceCount;
		}

		/// <summary>
		/// 取子品牌油耗数量
		/// </summary>
		/// <param name="csid"></param>
		/// <returns></returns>
		private string GetYouHaoCountByCsID(int csid)
		{
			string youhaoCount = "";
			try
			{
				XmlDocument docYouHao = new XmlDocument();

				object youHaoCountInterface = null;
				string cacheKey = "interfaceforbitauto_Serial_SerialInfo_YouHaoCountInterface";
				CacheManager.GetCachedData(cacheKey, out youHaoCountInterface);
				if (youHaoCountInterface == null)
				{
					docYouHao.Load(YouHaoCountInterface);
					CacheManager.InsertCache(cacheKey, docYouHao, 60);
				}
				else
				{
					docYouHao = (XmlDocument)youHaoCountInterface;
				}

				if (docYouHao != null)
				{
					XmlNodeList xnl = docYouHao.SelectNodes("/root/model[@id='" + csid.ToString() + "']");
					if (xnl != null && xnl.Count > 0)
					{
						youhaoCount = xnl[0].Attributes["fuelcount"].Value.ToString();
					}
				}
			}
			catch (Exception ex)
			{ }
			return youhaoCount;
		}

		/// <summary>
		/// 取车型详解URL
		/// </summary>
		/// <param name="csid"></param>
		/// <param name="newsCount"></param>
		/// <param name="url"></param>
		private string GetSerialNewsURLByCsID(int csid, string allSpell)
		{
			string url = "";
			string filePath = WebConfig.DataBlockPath + "Data\\SerialNews\\newsNum.xml";
			if (File.Exists(filePath))
			{
				try
				{
					//modified by sk 2013.04.28 统一读取文件方法
					XmlDocument docCount = CommonFunction.ReadXmlFromFile(filePath);

					XmlNode xn = docCount.SelectSingleNode("/SerilaList/Serial[@id='" + csid.ToString() + "']");
					if (xn != null)
					{
						// 评测
						if (xn.Attributes["pingce"] != null && xn.Attributes["pingce"].Value.ToString() != "0")
						{
							url = "http://car.bitauto.com/" + allSpell + "/pingce/";
						}
						else if (xn.Attributes["shijia"] != null && xn.Attributes["shijia"].Value.ToString() != "0")
						{
							// 有试驾文章
							url = "http://car.bitauto.com/" + allSpell + "/shijia/";
						}
						// 导购
						else if (xn.Attributes["daogou"] != null && xn.Attributes["daogou"].Value.ToString() != "0")
						{
							// 导购
							url = "http://car.bitauto.com/" + allSpell + "/daogou/";
						}
						// 新闻
						else if (xn.Attributes["xinwen"] != null && xn.Attributes["xinwen"].Value.ToString() != "0")
						{
							// 新闻
							url = "http://car.bitauto.com/" + allSpell + "/xinwen/";
						}
						// 行情
						else if (xn.Attributes["hangqing"] != null && xn.Attributes["hangqing"].Value.ToString() != "0")
						{
							// 行情
							url = "http://car.bitauto.com/" + allSpell + "/hangqing/";
						}
						// 用车
						else if (xn.Attributes["yongche"] != null && xn.Attributes["yongche"].Value.ToString() != "0")
						{
							// 用车
							url = "http://car.bitauto.com/" + allSpell + "/yongche/";
						}
						else
						{ }
					}
				}
				catch
				{ }
			}
			return url;
		}

		/// <summary>
		/// 根据子品牌ID 文件节点名 取文章数量
		/// </summary>
		/// <param name="csid"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private string GetSerialNewsCountByCsIDAndName(int csid, string name)
		{
			string count = "";
			string filePath = WebConfig.DataBlockPath + "Data\\SerialNews\\newsNum.xml";
			if (File.Exists(filePath))
			{
				try
				{
					//modified by sk 2013.04.28 统一读取文件方法
					XmlDocument docCount = CommonFunction.ReadXmlFromFile(filePath);

					XmlNode xn = docCount.SelectSingleNode("/SerilaList/Serial[@id='" + csid.ToString() + "']");
					if (xn != null && xn.Attributes[name] != null && xn.Attributes[name].Value.ToString() != "")
					{
						count = xn.Attributes[name].Value.ToString();
					}
				}
				catch
				{ }
			}
			return count;
		}

		/// <summary>
		/// 生成子品牌的所有组合名
		/// </summary>
		/// <param name="dr">子品牌数据</param>
		/// <returns></returns>
		private string GetSerialAllGroupName(DataRow dr)
		{
			string gn = "";
			List<string> listName = new List<string>();

			string csName = dr["csName"].ToString().Trim();
			string cbName = dr["cb_Name"].ToString().Trim();
			string bsName = dr["bs_name"].ToString().Trim();
			string cpName = dr["CpShortName"].ToString().Trim();
			string csSeoName = dr["cs_seoname"].ToString().Trim();
			string csOtherName = dr["csOtherName"].ToString().Trim().Replace("，", ",");

			string csEName = dr["csEName"].ToString().Trim();
			string cb_EName = dr["cb_EName"].ToString().Trim();
			string bs_EName = dr["bs_EName"].ToString().Trim();
			string cb_OtherName = dr["cb_OtherName"].ToString().Trim().Replace("，", ",");
			string bs_OtherName = dr["bs_OtherName"].ToString().Trim().Replace("，", ",");

			#region 子品牌

			// 子品牌名
			GetGroupName(ReplaceSomeCode(csName), ref listName);

			// 子品牌SEO名
			GetGroupName(ReplaceSomeCode(csSeoName), ref listName);

			// 子品牌英文名
			GetGroupName(ReplaceSomeCode(csEName), ref listName);

			// 子品牌别名
			if (csOtherName.Replace("，", ",") != "")
			{
				string[] otherName = csOtherName.Replace("，", ",").Split(',');
				if (otherName.Length > 0)
				{
					foreach (string name in otherName)
					{
						if (name != "")
						{
							GetGroupName(ReplaceSomeCode(name), ref listName);

							// 增加主品牌名
							if (!CheckNameIsContainOther(ReplaceSomeCode(name), ReplaceSomeCode(bsName)))
							{
								GetGroupName(ReplaceSomeCode(bsName) + ReplaceSomeCode(name), ref listName);
							}

							// 增加品牌名
							if (!CheckNameIsContainOther(ReplaceSomeCode(name), ReplaceSomeCode(cbName)))
							{
								GetGroupName(ReplaceSomeCode(cbName) + ReplaceSomeCode(name), ref listName);
							}
						}
					}
				}
			}

			#endregion

			#region 品牌
			// 品牌名+子品牌名
			if (cbName != "" && !CheckNameIsContainOther(csName, cbName))
			{
				GetGroupName(ReplaceSomeCode(cbName + csName), ref listName);
			}
			// 品牌英文名+子品牌名
			if (cb_EName != "" && !CheckNameIsContainOther(csName, cb_EName))
			{
				GetGroupName(ReplaceSomeCode(cb_EName + csName), ref listName);
			}
			// 品牌英文名+子品牌英文名
			if (cb_EName != "" && csEName != "" && !CheckNameIsContainOther(csEName, cb_EName))
			{
				GetGroupName(ReplaceSomeCode(cb_EName + csName), ref listName);
			}
			// 品牌别名+子品牌名
			if (cb_OtherName != "")
			{
				string[] otherName = cb_OtherName.Split(',');
				if (otherName.Length > 0)
				{
					foreach (string name in otherName)
					{
						//+子品牌名
						if (name != "" && !CheckNameIsContainOther(csName, name))
						{
							GetGroupName(ReplaceSomeCode(name + csName), ref listName);
						}
						// +子品牌英文名
						if (csEName != "" && name != "" && !CheckNameIsContainOther(csEName, name))
						{
							GetGroupName(ReplaceSomeCode(name + csEName), ref listName);
						}
					}
				}
			}
			#endregion

			#region 主品牌
			// 主品牌+子品牌名
			if (bsName != "" && !CheckNameIsContainOther(csName, bsName))
			{
				GetGroupName(ReplaceSomeCode(bsName + csName), ref listName);
			}
			// 主品牌英文名+子品牌名
			if (bs_EName != "" && !CheckNameIsContainOther(csName, bs_EName))
			{
				GetGroupName(ReplaceSomeCode(bs_EName + csName), ref listName);
			}
			// 主品牌英文名+子品牌英文名
			if (bs_EName != "" && csEName != "" && !CheckNameIsContainOther(csEName, bs_EName))
			{
				GetGroupName(ReplaceSomeCode(bs_EName + csEName), ref listName);
			}
			// 主品牌别名+子品牌名
			if (bs_OtherName != "")
			{
				string[] otherName = bs_OtherName.Split(',');
				if (otherName.Length > 0)
				{
					foreach (string name in otherName)
					{
						//+子品牌名
						if (name != "" && !CheckNameIsContainOther(csName, name))
						{
							GetGroupName(ReplaceSomeCode(name + csName), ref listName);
						}
						// +子品牌英文名
						if (csEName != "" && name != "" && !CheckNameIsContainOther(csEName, name))
						{
							GetGroupName(ReplaceSomeCode(name + csEName), ref listName);
						}
					}
				}
			}
			#endregion

			#region 厂商

			if (cpName != "" && !CheckNameIsContainOther(csName, cpName))
			{
				GetGroupName(ReplaceSomeCode(cpName + csName), ref listName);
			}

			#endregion

			if (listName.Count > 0)
			{
				foreach (string name in listName)
				{
					if (name != "")
					{
						if (gn != "")
						{ gn += ","; }
						gn += name;
					}
				}
			}

			return gn;
		}

		private void GetGroupName(string name, ref List<string> list)
		{
			if (!list.Contains(name))
			{
				list.Add(name);
			}
			if (IsContainSpecialCode(name))
			{
				if (!list.Contains(ReplaceSpecialCode(name)))
				{
					list.Add(ReplaceSpecialCode(name));
				}
			}
		}

		///// <summary>
		///// 取子品牌的城市站地址
		///// </summary>
		//private void GetAllOnSaleSerialLinkForCity()
		//{
		//    sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
		//    sb.AppendLine("<SerialInfo>");

		//    DataSet ds = GetAllOnSaleSerial();
		//    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		//    {
		//        foreach (DataRow dr in ds.Tables[0].Rows)
		//        {
		//            sb.AppendLine("<Cs ID=\"" + dr["cs_id"].ToString() + "\" ");
		//            sb.AppendLine("CsName=\"" + dr["cs_name"].ToString().Trim() + "\" ");
		//            sb.AppendLine("CsShowName=\"" + dr["cs_showName"].ToString().Trim() + "\" ");
		//            sb.AppendLine("Link=\"http://{0}.bitauto.com/car/" + dr["allSpell"].ToString().Trim().ToLower() + "/\" />");
		//        }
		//    }

		//    sb.AppendLine("</SerialInfo>");
		//}

		/// <summary>
		/// CMS进口子品牌前10排行变化了
		/// </summary>
		private void GetCMSJinKouSerialRankRate()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<SerialInfo>");
			sb.AppendLine("<Serial ID=\"2046\" Name=\"Cayenne\" AllSpell=\"baoshijiecayenne\" Rate=\"2\"/>");
			sb.AppendLine("<Serial ID=\"3245\" Name=\"揽胜极光\" AllSpell=\"luhulrxgainianche\" Rate=\"-1\"/>");
			sb.AppendLine("<Serial ID=\"2390\" Name=\"X6\" AllSpell=\"baomax6\" Rate=\"-1\"/>");
			sb.AppendLine("<Serial ID=\"1994\" Name=\"Q7\" AllSpell=\"aodiq7\" Rate=\"0\"/>");
			sb.AppendLine("<Serial ID=\"2070\" Name=\"A8L\" AllSpell=\"aodia8l\" Rate=\"0\"/>");
			sb.AppendLine("<Serial ID=\"2065\" Name=\"辉腾\" AllSpell=\"dazhonghuiteng\" Rate=\"6\"/>");
			sb.AppendLine("<Serial ID=\"1832\" Name=\"指南者\" AllSpell=\"jipuzhinanzhe\" Rate=\"1\"/>");
			sb.AppendLine("<Serial ID=\"2762\" Name=\"X1\" AllSpell=\"baomax1\" Rate=\"1\"/>");
			sb.AppendLine("<Serial ID=\"2259\" Name=\"S级\" AllSpell=\"benchisji\" Rate=\"1\"/>");
			sb.AppendLine("<Serial ID=\"2610\" Name=\"尚酷\" AllSpell=\"shangku\" Rate=\"-3\"/>");
			sb.AppendLine("</SerialInfo>");
		}

		#endregion

		#region 取数据

		private DataSet GetSerialName()
		{
			string sql = @"SELECT  cs.cs_id, LOWER(cs.csname) AS csname,
                                    LOWER(cs.csOtherName) AS csOtherName, LOWER(cs.csEName) AS csEName,
                                    LOWER(cs.Url) AS Url, LOWER(cs.cs_seoname) AS cs_seoname, cs.virtues,
                                    cs.defect, cs.CsSaleState, LOWER(cs.allSpell) AS allSpell, cb.cb_id,
                                    LOWER(cb.cb_name) AS cb_name, LOWER(cb.cb_seoname) AS cb_seoname,
                                    cmb.bs_id, LOWER(cmb.bs_name) AS bs_name, ISNULL(cp.cp_id, 0) AS cp_id,
                                    LOWER(cp.cp_name) AS cp_name, LOWER(cp.CpShortName) AS CpShortName,
                                    cs3.UVCount, LOWER(cs.csEName) AS csEName,
                                    LOWER(cb.cb_EName) AS cb_EName, LOWER(cb.cb_OtherName) AS cb_OtherName,
                                    LOWER(cmb.bs_EName) AS bs_EName,
                                    LOWER(cmb.bs_OtherName) AS bs_OtherName
                            FROM    car_serial cs
                                    LEFT JOIN dbo.Car_Serial_30UV cs3 ON cs.cs_id = cs3.cs_id
                                    LEFT JOIN car_brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                                                              AND cmbr.isState = 0
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                    LEFT JOIN dbo.Car_producer cp ON cb.cp_id = cp.cp_id
                            WHERE   cs.isState = 0
                                    AND cs.carlevel <> 481
                                    AND cb.isState = 0
                            ORDER BY cs3.UVCount DESC, cs.cs_id";
			DataSet ds = new DataSet();
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
			return ds;
		}

		/// <summary>
		/// 取子品牌的扩展参数
		/// </summary>
		/// <param name="csID"></param>
		/// <param name="paramID"></param>
		/// <returns></returns>
		private DataSet GetSerialParamByCsIDAndParamID(int csID, int paramID)
		{
			DataSet ds = new DataSet();
			string sql = @"select cdb.pvalue
                                from dbo.Car_relation car
                                left join car_serial cs on car.cs_id=cs.cs_id
                                left join dbo.CarDataBase cdb on car.car_id=cdb.carid and cdb.paramid= @paramID
                                where car.isState=0 and cs.isState=0 and car.cs_id = @csID and pvalue is not null
                                group by pvalue
                                order by pvalue";
			SqlParameter[] _param ={
			new SqlParameter("@paramID",SqlDbType.Int),
            new SqlParameter("@csID",SqlDbType.Int)
			};
			_param[0].Value = paramID;
			_param[1].Value = csID;
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
			return ds;
		}

		private string GetStringByCsIDAndParamID(int csID, int paramID, string splitString)
		{
			StringBuilder _temp = new StringBuilder();
			DataSet ds = GetSerialParamByCsIDAndParamID(csID, paramID);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					if (_temp.Length > 0)
					{
						_temp.Append(splitString);
					}
					_temp.Append(dr["pvalue"].ToString());
				}
			}
			return _temp.ToString();
		}

		///// <summary>
		///// 取所有在销非概念车字频
		///// </summary>
		///// <returns></returns>
		//private DataSet GetAllOnSaleSerial()
		//{
		//    string sql = " select cs_id,cs_name,cs_showName,allSpell from car_serial  ";
		//    sql += " where isState=1 and CsSaleState<>'停销' and cs_CarLevel<>'概念车' ";
		//    sql += " order by cs_showName ";
		//    DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
		//    return ds;
		//}

		#endregion

		#region 检查方法

		/// <summary>
		/// 名字中是否包含另一个名字
		/// </summary>
		/// <param name="name"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		private bool CheckNameIsContainOther(string name, string other)
		{
			bool isContain = false;
			if (name.IndexOf(other) >= 0)
			{ isContain = true; }
			return isContain;
		}

		/// <summary>
		/// 名字中是在另一个名字开始位置
		/// </summary>
		/// <param name="name"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		private bool CheckNameIsFirstPlaceOnOther(string name, string other)
		{
			bool isContain = false;
			if (name != "" && other != "" && name.IndexOf(other) == 0)
			{ isContain = true; }
			return isContain;
		}

		/// <summary>
		/// 是否包含特殊字符
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private bool IsContainSpecialCode(string name)
		{
			bool isContain = false;
			string RegexString = @"[(|)|·|\-|\.|（|）|！|\!]";
			Regex r = new Regex(RegexString);
			isContain = r.IsMatch(name);
			return isContain;
		}

		/// <summary>
		/// 替换特殊字符
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private string ReplaceSpecialCode(string name)
		{
			string replaceName = "";
			string RegexString = @"[(|)|·|\-|\.|（|）|！|\!]";
			Regex r = new Regex(RegexString);
			replaceName = r.Replace(name, "");
			return replaceName;
		}

		/// <summary>
		/// 替换一些字符
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private string ReplaceSomeCode(string name)
		{
			string replaceName = name.ToLower().Replace("（", "(").Replace("）", ")").Replace("&", "").Replace("\"", "");
			return replaceName;
		}

		#endregion
	}
}