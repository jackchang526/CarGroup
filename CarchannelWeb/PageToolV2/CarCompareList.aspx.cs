using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageToolV2
{
	public partial class CarCompareList : PageBase
	{
		#region Member
		protected string carIDs = "";
		protected string csIDs = "";
		private StringBuilder sbCarIDs = new StringBuilder(60);
		private StringBuilder sbCsIDs = new StringBuilder(50);
		private List<int> listCarID = new List<int>();
		private List<int> listCsID = new List<int>();

		protected List<int> listValidCsID = new List<int>();
		private List<int> listValidCarID = new List<int>();

		private Dictionary<int, MasterEntity> dictCarList = new Dictionary<int, MasterEntity>();
		protected string allCarInfo = string.Empty;

		// 前2个车型的对比数据 HTML for SEO
		protected string compareHTMLForSeo = string.Empty;

		protected string allCarJsArray = string.Empty;

		protected string titleForSEO = "【车型对比选车_汽车车型大全】-易车网";
		// 是否是推荐link 不统计
		protected bool isRec = false;

		// 子品牌的按级别 关注指数、销量指数排行
		protected string serialLevelIndexRank = "";
        #endregion
        Car_BasicBll carBasicBll = new Car_BasicBll();
        protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			if (!this.IsPostBack)
			{
				this.GetPageParam();
				this.GetValidCarList();
				allCarInfo = JsonHelper.Serialize(this.dictCarList);
				this.GetValidCarJsObject();
				// add by chengl Aug.1.2012
				serialLevelIndexRank = this.GetSerialLevelIndexRank();
				if (sbCarIDs.Length > 0)
				{ 
					carIDs = sbCarIDs.ToString(); 
				}
				if (sbCsIDs.Length > 0)
				{ 
					csIDs = sbCsIDs.ToString(); 
				}
			}
		}

		#region private Method

		/// <summary>
		/// 取车型参数
		/// </summary>
		private void GetPageParam()
		{
			if (this.Request.QueryString["isRec"] != null)
			{
				isRec = true; 
			}

			if (this.Request.QueryString["carIDs"] != null && this.Request.QueryString["carIDs"].ToString() != "")
			{
				string tempCarIDs = this.Request.QueryString["carIDs"].ToString();
				string[] arrCarID = tempCarIDs.Split(',');
				if (arrCarID.Length > 0)
				{
					for (int i = 0; i < arrCarID.Length; i++)
					{
						if (listCarID.Count > 9)
						{ break; }
						int carid = 0;
						if (int.TryParse(arrCarID[i].ToString(), out carid))
						{
							if (carid > 0 && !listCarID.Contains(carid))
							{
								listCarID.Add(carid);
							}
						}
					}
				}
			}
			// 子品牌ID
			if (this.Request.QueryString["csIDs"] != null && this.Request.QueryString["csIDs"].ToString() != "")
			{
				string tempCsIDs = this.Request.QueryString["csIDs"].ToString();
				string[] arrCsID = tempCsIDs.Split(',');
				if (arrCsID.Length > 0)
				{
					for (int i = 0; i < arrCsID.Length; i++)
					{
						if (listCsID.Count > 9)
						{ break; }
						int csid = 0;
						if (int.TryParse(arrCsID[i].ToString(), out csid))
						{
							if (csid > 0 && !listCsID.Contains(csid))
							{
								listCsID.Add(csid);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 取有效的车型列表
		/// </summary>
		private void GetValidCarList()
		{
			// 当有车型ID时
			if (listCarID.Count > 0)
			{
				int loop = 0;
				foreach (int carid in listCarID)
				{
					// modified by chengl Oct.24.2011
					CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carid);
					if (ce == null || ce.Id <= 0)
					{ continue; }

					if (carid > 0 && !listValidCarID.Contains(carid))
					{
						listValidCarID.Add(carid);
						//添加车型对应 主品牌信息
						if (!dictCarList.ContainsKey(carid))
							dictCarList.Add(carid, new MasterEntity() { MasterId = ce.Serial.Brand.MasterBrand.Id, MasterName = ce.Serial.Brand.MasterBrand.Name });
					}
					if (ce.Serial.Id > 0 && !listValidCsID.Contains(ce.Serial.Id))
					{ listValidCsID.Add(ce.Serial.Id); }
					if (loop == 0)
					{
						// 第1个车型
						titleForSEO = "【" + ce.Serial.Name + " " + ce.Name;
					}
					if (loop == 1)
					{
						// 第2个车型
						titleForSEO += "_" + ce.Serial.Name + " " + ce.Name + "_对比结果】-易车网";
					}
					loop++;
				}
				if (loop <= 1)
				{ 
					titleForSEO = "【车型对比选车_汽车车型大全】"; 
				}

				if (listValidCsID.Count > 0)
				{
					foreach (int csid in listValidCsID)
					{
						if (sbCsIDs.Length > 0)
						{ sbCsIDs.Append(","); }
						sbCsIDs.Append(csid);
					}
				}
				if (listValidCarID.Count > 0)
				{
					foreach (int carid in listValidCarID)
					{
						if (sbCarIDs.Length > 0)
						{ sbCarIDs.Append(","); }
						sbCarIDs.Append(carid);
					}
				}
			}
			// 当没有车型的时候 取子品牌的1个热门车型
			if (listCarID.Count == 0 && listCsID.Count > 0)
			{
				StringBuilder tempSb = new StringBuilder(60);
				string tempCsIDs = "";
				foreach (int csid in listCsID)
				{
					if (tempSb.Length > 0)
					{ tempSb.Append(","); }
					tempSb.Append(csid);
				}
				tempCsIDs = tempSb.ToString();
				if (tempCsIDs != "")
				{
					DataSet ds = carBasicBll.GetCarBaseInfoForCompareByCsIDs(tempCsIDs);
					if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
					{
						int loop = 0;
						foreach (DataRow dr in ds.Tables[0].Rows)
						{
							int csid = int.Parse(dr["cs_id"].ToString());
							int carid = int.Parse(dr["car_id"].ToString());
							if (listValidCsID.Contains(csid))
							{ 
								continue; 
							}
							if (!listValidCarID.Contains(carid))
							{
								//添加车型对应 主品牌信息
								if (!dictCarList.ContainsKey(carid))
								{
									// modified by chengl Oct.24.2011
									CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carid);
									if (ce == null || ce.Id <= 0)
									{ 
										continue; 
									}

									dictCarList.Add(carid, new MasterEntity() { MasterId = ce.Serial.Brand.MasterBrand.Id, MasterName = ce.Serial.Brand.MasterBrand.Name });
								}
								listValidCarID.Add(carid);
							}
							if (!listValidCsID.Contains(csid))
							{ 
								listValidCsID.Add(csid); 
							}

							if (loop == 0)
							{
								// 第1个车型
								titleForSEO = "【" + dr["cs_name"].ToString() + " " + dr["car_name"].ToString();
							}
							if (loop == 1)
							{
								// 第2个车型
								titleForSEO += "_" + dr["cs_name"].ToString() + " " + dr["car_name"].ToString() + "_对比结果】-易车网";
							}
							loop++;
						}
						if (loop <= 1)
						{
							titleForSEO = "【车型对比选车_汽车车型大全】"; 
						}
						if (listValidCsID.Count > 0)
						{
							foreach (int csid in listValidCsID)
							{
								if (sbCsIDs.Length > 0)
								{ sbCsIDs.Append(","); }
								sbCsIDs.Append(csid);
							}
						}
						if (listValidCarID.Count > 0)
						{
							foreach (int carid in listValidCarID)
							{
								if (sbCarIDs.Length > 0)
								{ sbCarIDs.Append(","); }
								sbCarIDs.Append(carid);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 取车型完整对比数据
		/// </summary>
		/// <returns></returns>
		private void GetValidCarJsObject()
		{
            allCarJsArray = carBasicBll.GetValidCarJsObject(listCarID);
            if (string.IsNullOrEmpty(allCarJsArray))
            {
                allCarJsArray = "var carCompareJson = null;";
            }
            else
            {
                allCarJsArray = string.Format("var carCompareJson = {0};", allCarJsArray);
            }
            /*
			Car_BasicBll carBasicBll = new Car_BasicBll();
			// 页面隐藏域输出for SEO
			List<string> pageHTMLForSEO = new List<string>();
			if (listValidCarID.Count > 0)
			{
				Dictionary<int, Dictionary<string, string>> dic = carBasicBll.GetCarCompareDataByCarIDs(listValidCarID);
				// Dictionary<int, List<string>> dicTemp = base.GetCarParameterJsonConfig();
				// 输出隐藏域 for SEO
				Dictionary<int, Dictionary<string, CarParam>> dicTemp = base.GetCarParameterJsonConfigDictionaryNew();
				// 页面隐藏域输出for SEO
				#region 页面隐藏域输出for SEO
				int firstCarID = 0;
				int firstCsID = 0;
				if (dicTemp != null && dicTemp.Count > 0)
				{
					pageHTMLForSEO.Add("<table>");
					// 循环大类
					foreach (KeyValuePair<int, Dictionary<string, CarParam>> kvp in dicTemp)
					{
						// 循环小类
						foreach (KeyValuePair<string, CarParam> kvpParam in kvp.Value)
						{
							pageHTMLForSEO.Add("<tr>");
							pageHTMLForSEO.Add("<th>" + kvpParam.Value.ParamName + "</th>");
							// 循环车型 循环前2个车型
							int loopMax = 0;
							foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dic)
							{
								if (loopMax == 0 && firstCarID <= 0 && firstCsID <= 0)
								{
									firstCarID = int.Parse(kvpCar.Value["Car_ID"]);
									firstCsID = int.Parse(kvpCar.Value["Cs_ID"]);
								}
								string carPvalue = kvpCar.Value.ContainsKey(kvpParam.Key) ? kvpCar.Value[kvpParam.Key] : "";
								pageHTMLForSEO.Add("<td>" + (carPvalue == "" ? "" : carPvalue + kvpParam.Value.ModuleDec) + "</td>");
								loopMax++;
								if (loopMax >= 2)
								{ break; }
							}
							pageHTMLForSEO.Add("</tr>\r");
						}
					}
					pageHTMLForSEO.Add("</table>\r");
					#region 首选车型的热门URL for SEO
					int hotCompare = 1;
					int hotCompareTop = 6;
					DataSet dsCompare = new Car_BasicBll().GetCarCompareListByCarID(firstCarID);
					if (dsCompare != null && dsCompare.Tables.Count > 0 && dsCompare.Tables[0].Rows.Count > 0)
					{
						pageHTMLForSEO.Add("<ul>\r");
						for (int i = 0; i < dsCompare.Tables[0].Rows.Count; i++)
						{
							if (hotCompare > hotCompareTop)
							{ break; }
							if (dsCompare.Tables[0].Rows[i]["cs_id"].ToString() == firstCsID.ToString())
							{ continue; }
							hotCompare++;
							pageHTMLForSEO.Add("<li><a href=\"/chexingduibi/?carIDs=" + dsCompare.Tables[0].Rows[i]["cCarID"].ToString().Trim() + "," + firstCarID + "\">热门对比</a></li>\r");
						}
						pageHTMLForSEO.Add("</ul>");
					}
					#endregion
					compareHTMLForSeo = string.Concat(pageHTMLForSEO.ToArray());
					pageHTMLForSEO.Clear();
				}
				#endregion
				// 车型数据js输出
				allCarJsArray = carBasicBll.GetValidCarJsObject(listValidCarID);
				if (string.IsNullOrWhiteSpace(allCarJsArray))
				{
					allCarJsArray = "var carCompareJson = null;";
				}
				else
				{
					allCarJsArray = string.Format("var carCompareJson = {0};", allCarJsArray);
				}
			}
			else
			{
				allCarJsArray = "var carCompareJson = null;";
			}
            */
        }

		/// <summary>
		/// 取当前子品牌的关注指数、销量指数 不分地区 按级别排名
		/// </summary>
		/// <returns></returns>
		private string GetSerialLevelIndexRank()
		{
			Dictionary<int, int> dicUV = IndexBll.GetSerialLevelRankByIndexType(IndexType.UV);
			Dictionary<int, int> dicSale = IndexBll.GetSerialLevelRankByIndexType(IndexType.Sale);

			List<string> jsArray = new List<string>();
			if (listValidCsID.Count > 0)
			{
				foreach (int csid in listValidCsID)
				{
					if (jsArray.Count > 0)
					{ 
						jsArray.Add(",");
					}
					jsArray.Add("\"" + csid.ToString() + "\":{");
					if (dicUV.ContainsKey(csid))
					{ 
						jsArray.Add("\"UVRank\":" + dicUV[csid]); 
					}
					else
					{ 
						jsArray.Add("\"UVRank\":0"); 
					}
					if (dicSale.ContainsKey(csid))
					{ 
						jsArray.Add(",\"SaleRank\":" + dicSale[csid]);
					}
					else
					{ 
						jsArray.Add(",\"SaleRank\":0"); 
					}
					jsArray.Add("}");
				}
			}

			if (jsArray.Count > 0)
			{
				jsArray.Insert(0, "var carIndexJson = {");
				jsArray.Add("};");
			}
			else
			{
				jsArray.Add("var carIndexJson = null;");
			}

			return string.Concat(jsArray.ToArray());
		}

		#endregion

		public class MasterEntity
		{
			public int MasterId { get; set; }
			public string MasterName { get; set; }
		}
	}
}