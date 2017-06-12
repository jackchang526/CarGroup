using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using System.Data;
using BitAuto.CarChannel.BLL;
using BitAuto.Utils;

namespace WirelessWeb
{
	public partial class CarCompareTool : WirelessPageBase
	{
		private List<int> listCarID = new List<int>();
		protected List<int> listValidCarID = new List<int>();

		protected string allCarJsArray = string.Empty;
		protected string compareHTMLForSeo = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			//GetPageParam();
			//InitData();
		}

		private void GetPageParam()
		{
			if (this.Request.QueryString["carIDs"] != null && this.Request.QueryString["carIDs"].ToString() != "")
			{
				string tempCarIDs = this.Request.QueryString["carIDs"].ToString();
				string[] arrCarID = tempCarIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				Array.ForEach(arrCarID, (pair) =>
				{
					int carId = ConvertHelper.GetInteger(pair);
					if (carId > 0 && !listCarID.Contains(carId))
					{
						listCarID.Add(carId);
					}
				});
				listValidCarID = listCarID;
			}
		}

		private void InitData()
		{
			allCarJsArray = this.GetValidCarJsObject();
		}


		/// <summary>
		/// 取车型完整对比数据
		/// </summary>
		/// <returns></returns>
		private string GetValidCarJsObject()
		{
			List<string> jsArray = new List<string>();
			// 页面隐藏域输出for SEO
			List<string> pageHTMLForSEO = new List<string>();
			if (listValidCarID.Count > 0)
			{
				Dictionary<int, Dictionary<string, string>> dic = (new Car_BasicBll()).GetCarCompareDataByCarIDs(listValidCarID);
				// Dictionary<int, List<string>> dicTemp = base.GetCarParameterJsonConfig();
				// 输出隐藏域 for SEO
				Dictionary<int, Dictionary<string, CarParam>> dicTemp = base.GetCarParameterJsonConfigDictionary();
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
				#region 车型数据js输出
				if (dicTemp != null && dicTemp.Count > 0)
				{
					int loopCar = 0;
					// 循环车
					foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dic)
					{
						if (loopCar > 0)
						{ jsArray.Add(","); }
						jsArray.Add("[");

						// 循环模板
						foreach (KeyValuePair<int, Dictionary<string, CarParam>> kvpTemp in dicTemp)
						{
							if (kvpTemp.Key == 0)
							{
								// 基本数据
								jsArray.Add("[\"" + kvpCar.Value["Car_ID"] + "\"");
								jsArray.Add(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
								foreach (KeyValuePair<string, CarParam> param in kvpTemp.Value)
								{
									if (kvpCar.Value.ContainsKey(param.Key))
									{
										jsArray.Add(",\"" + kvpCar.Value[param.Key].Replace("\"", "'") + "\"");
									}
									else
									{
										jsArray.Add(",\"\"");
									}
								}
								jsArray.Add("]");
							}
							else
							{
								// 扩展数据
								jsArray.Add(",[");
								int loop = 0;
								foreach (KeyValuePair<string, CarParam> param in kvpTemp.Value)
								{
									if (loop > 0)
									{
										jsArray.Add(",");
									}
									if (kvpCar.Value.ContainsKey(param.Key))
									{
										jsArray.Add("\"" + kvpCar.Value[param.Key].Replace("\"", "'") + "\"");
									}
									else
									{
										jsArray.Add("\"\"");
									}
									loop++;
								}
								jsArray.Add("]");
							}
						}
						jsArray.Add("]");
						loopCar++;
					}
				}
				#endregion
				if (jsArray.Count > 0)
				{
					jsArray.Insert(0, "var carCompareJson = [");
					jsArray.Add("];");
				}
				else
				{
					jsArray.Add("var carCompareJson = null;");
				}
			}
			else
			{
				jsArray.Add("var carCompareJson =null;");
			}
			return string.Concat(jsArray.ToArray());
		}
	}
}