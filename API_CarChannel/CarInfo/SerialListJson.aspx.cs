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
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 车型频道移动站 根据主品牌ID取旗下 品牌、子品牌 json 数据
	/// </summary>
	public partial class SerialListJson : PageBase
	{
		#region member

		private int bsID = 0;
		private string callback = string.Empty;
		private string jsonVarName = string.Empty;
		private List<string> jsonString = new List<string>();
		// 是否所有级别(包含概念车) 默认不包含
		private bool isAllLevel = false;
		// 是否所有销售状态(包含停销)默认不包含
		private bool isAllSale = false;
		// {0} 主品牌ID,{1} 是否所有级别,{2} 是否所有销售
		private string memCacheTemp = "Car_List_BrandByBsID_{0}_{1}_{2}";

		/// <summary>
		/// 请求的主品牌ID列表 可以是1个 也可以是前30主品牌
		/// </summary>
		private List<int> wantBsIDs = new List<int>();

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			this.Response.ContentType = "application/x-javascript";
			if (!this.IsPostBack)
			{
				base.SetPageCache(60 * 12);
				GetPageParam();
				GetMasterToSerialData();
				ResponseJson();
			}
		}

		#region private Method

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{

			if (!string.IsNullOrEmpty(this.Request.QueryString["bsID"]))
			{
				if (int.TryParse(this.Request.QueryString["bsID"].ToString(), out bsID))
				{
					if (bsID > 0 && !wantBsIDs.Contains(bsID))
					{ wantBsIDs.Add(bsID); }
				}
			}

			// 是否要保护概念车 默认不包含
			if (!string.IsNullOrEmpty(this.Request.QueryString["isAllLevel"])
				&& this.Request.QueryString["isAllLevel"].ToString() == "1")
			{ isAllLevel = true; }

			// 是否所有销售状态(包含停销)默认不包含
			if (!string.IsNullOrEmpty(this.Request.QueryString["isAllSale"])
				&& this.Request.QueryString["isAllSale"].ToString() == "1")
			{ isAllSale = true; }

			// 是否是取前多少主品牌
			if (!string.IsNullOrEmpty(this.Request.QueryString["top"])
				&& this.Request.QueryString["top"].ToString() != "")
			{
				int top = 30;
				if (int.TryParse(this.Request.QueryString["top"].ToString(), out top))
				{
				}
				if (top <= 0 || top > 30)
				{
					top = 30;
				}
				List<int> bsUV = new Car_BrandBll().GetAllMasterOrderByUV();
				if (bsUV != null && bsUV.Count > 0)
				{
					foreach (int bsidUV in bsUV)
					{
						if (!wantBsIDs.Contains(bsidUV))
						{
							wantBsIDs.Add(bsidUV);
							if (wantBsIDs.Count >= top)
							{ break; }
						}
					}
				}
			}

			callback = this.Request.QueryString["callback"];

			jsonVarName = this.Request.QueryString["jsonVarName"];
		}

		/// <summary>
		/// 取数据生成json
		/// </summary>
		private void GetMasterToSerialData()
		{
			// 用ID列表做key
			string key = string.Format(memCacheTemp, string.Join(",", wantBsIDs.ToArray()), isAllLevel, isAllSale);
			// string key = string.Format(memCacheTemp, bsID, isAllLevel, isAllSale);
			object objMemCache = MemCache.GetMemCacheByKey(key);
			if (objMemCache != null)
			{
				jsonString = (List<string>)objMemCache;
			}
			else
			{
				#region 取数据
				// 主品牌及旗下品牌、子品牌数据源，包括所有销售状态和概念车，排序同车型频道
				//string filePath = WebConfig.CarDataBaseNASPath
				//    + @"CarChannel\BaseData\MasterToBrandToSerialAllSaleAndLevel.xml";

				// modifed by chengl May.9.2014 数据源从WebConfig.CarDataBaseNASPath迁移至WebConfig.DataBlockPath
				XmlDocument doc = AutoStorageService.GetAllAutoAndLevelXml();
				//if (File.Exists(filePath))
				//{
				// doc.Load(filePath);
				if (doc != null && doc.HasChildNodes)
				{
					List<string> listBS = new List<string>();
					// 循环中请求的主品牌列表
					foreach (int masterID in wantBsIDs)
					{
						List<string> listCB = new List<string>();
						XmlNodeList xnlBs = doc.SelectNodes("/Params/MasterBrand[@ID='" + masterID.ToString() + "']");
						if (xnlBs != null && xnlBs.Count > 0 && xnlBs[0].HasChildNodes)
						{
							// 
							foreach (XmlNode xnCB in xnlBs[0].ChildNodes)
							{
								string[] tempCB = new string[6];
								// 循环旗下品牌
								int cbID = int.Parse(xnCB.Attributes["ID"].Value.ToString().Trim());
								string cbName = CommonFunction.GetUnicodeByString(xnCB.Attributes["Name"].Value.ToString().Trim());
								string cbSpell = xnCB.Attributes["AllSpell"].Value.ToString().Trim();

								tempCB[0] = "{";
								tempCB[1] = string.Format("ID:\"{0}\",Name:\"{1}\",AllSpell:\"{2}\"", cbID, cbName, cbSpell);
								tempCB[2] = ",CsList:[";
								if (xnCB.ChildNodes.Count > 0)
								{
									int loopCs = 0;
									List<string> tempCS = new List<string>();
									foreach (XmlNode xnCs in xnCB.ChildNodes)
									{
										int csID = int.Parse(xnCs.Attributes["ID"].Value.ToString().Trim());
										string csName = CommonFunction.GetUnicodeByString(xnCs.Attributes["Name"].Value.ToString().Trim());
										string csAllSpell = xnCs.Attributes["AllSpell"].Value.ToString().Trim();
										string csSaleState = CommonFunction.GetUnicodeByString(xnCs.Attributes["CsSaleState"].Value.ToString().Trim());
										string csLevel = CommonFunction.GetUnicodeByString(xnCs.Attributes["CsLevel"].Value.ToString().Trim());
										if (!isAllLevel && xnCs.Attributes["CsLevel"].Value.ToString().Trim() == "概念车")
										{ continue; }
										if (!isAllSale && xnCs.Attributes["CsSaleState"].Value.ToString().Trim() == "停销")
										{ continue; }
										if (loopCs > 0)
										{ tempCS.Add(","); }
										tempCS.Add("{");
										tempCS.Add(string.Format("ID:\"{0}\",Name:\"{1}\",Sale:\"{2}\",AllSpell:\"{3}\""
											, csID, csName, csSaleState, csAllSpell));
										tempCS.Add("}");
										loopCs++;
									}
									if (tempCS.Count > 0)
									{ tempCB[3] = string.Concat(tempCS.ToArray()); }
									else
									{ continue; }
								}
								tempCB[4] = "]";
								tempCB[5] = "}";
								if (listCB.Count > 0)
								{ listCB.Add(","); }
								listCB.Add(string.Concat(tempCB));
							}
						}
						// 如果结果是多个主品牌 则json结构有主品牌信息 
						// 如果结果只有1个主品牌 则结果保存不变
						if (listBS.Count > 0)
						{
							listBS.Add(",");
						}
						if (wantBsIDs.Count > 1)
						{
							listBS.Add("{");
							listBS.Add(string.Format("ID:\"{0}\",CBList:[", masterID));
						}
						listBS.Add(string.Concat(listCB));
						if (wantBsIDs.Count > 1)
						{
							listBS.Add("]}");
						}

					}
					if (listBS.Count > 0)
					{ jsonString.Add(string.Concat(listBS.ToArray())); }
				}
				// }
				#endregion
				// 加入memcache
				MemCache.SetMemCacheByKey(key, jsonString, 24 * 60 * 60 * 1000);
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