using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 车型对比页 车型详细数据接口 输出js数组
	/// </summary>
	public partial class GetCarInfoForCompare : PageBase
	{

		#region private Member
		private string carJson = " var carCompareJson = ";
		private List<int> listCarID = new List<int>();
		private Dictionary<int, Dictionary<string, string>> dicCarParam = new Dictionary<int, Dictionary<string, string>>();
		private bool isParamPage = false;
		private int topCount = 10;
		private Dictionary<int, string> dicParamIDToName = new Dictionary<int, string>();

		// 参数模板
		private Dictionary<int, List<string>> dicTemp = null;
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			this.Response.ContentType = "application/x-javascript";
			if (!this.IsPostBack)
			{
				base.SetPageCache(60);
				GetPageParam();
				Response.Write(carJson);
				Response.Write("[");
				GetCarParamData();
				Response.Write("];");
			}
		}

		#region private Method

		/// <summary>
		/// 取车型ID 
		/// </summary>
		private void GetPageParam()
		{
			// 最多对比车型数量 对比页10个 参数配置页40个
			if (!string.IsNullOrEmpty(this.Request.QueryString["isParamPage"]))
			{
				isParamPage = true;
				topCount = 50;
			}

			string carIDsStr = this.Request.QueryString["carIDs"];
			if (!string.IsNullOrEmpty(carIDsStr))
			{
				string[] carIDArray = carIDsStr.Split(',');
				if (carIDArray != null && carIDArray.Length > 0)
				{
					foreach (string id in carIDArray)
					{
						int carid = 0;
						if (int.TryParse(id.Trim(), out carid))
						{
							if (carid > 0 && !listCarID.Contains(carid))
							{
								listCarID.Add(carid);
								if (listCarID.Count > topCount)
								{ break; }
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 取车型详细参数
		/// </summary>
		private void GetCarParamData()
		{
			if (listCarID.Count > 0)
			{
				dicCarParam = (new Car_BasicBll()).GetCarCompareDataByCarIDs(listCarID);

				#region 生成车型详细参数js数组
				// 生成车型详细参数js数组
				if (dicCarParam.Count > 0)
				{
					dicTemp = base.GetCarParameterJsonConfig();
					if (dicTemp != null && dicTemp.Count > 0)
					{
						int loopCar = 0;
						foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dicCarParam)
						{
							if (loopCar > 0)
							{ Response.Write(","); }

							Response.Write("[");
							// 循环模板
							foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
							{
								if (kvpTemp.Key == 0)
								{
									// 基本数据
									Response.Write("[\"" + kvpCar.Value["Car_ID"] + "\"");
									Response.Write(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
									foreach (string param in kvpTemp.Value)
									{
										if (kvpCar.Value.ContainsKey(param))
										{ Response.Write(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
										else
										{ Response.Write(",\"\""); }
									}
									Response.Write("]");
								}
								else
								{
									// 扩展数据
									Response.Write(",[");
									int loop = 0;
									foreach (string param in kvpTemp.Value)
									{
										if (loop > 0)
										{ Response.Write(","); }
										if (kvpCar.Value.ContainsKey(param))
										{ Response.Write("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
										else
										{ Response.Write("\"\""); }
										loop++;
									}
									Response.Write("]");
								}
							}
							Response.Write("]");

							loopCar++;

							//// 循环车型
							//foreach (KeyValuePair<string, string> kvpParam in kvpCar.Value)
							//{
							//    // 循环车型的参数
							//}
						}
					}
				}
				#endregion

				#region 清除数据
				//dicCarParam.Clear();
				//dsCarBaseInfo.Clear();
				//dsParam.Clear();
				//sbCarID.Remove(0, sbCarID.Length);
				//listCarID.Clear();
				#endregion

			}

		}

		#endregion

	}
}