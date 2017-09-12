using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.BLL;
using System.Web.UI;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// GetCarParameter 的摘要说明
	/// </summary>
	public class GetCarParameter : PageBase, IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;

 		private List<int> listCarID = new List<int>();
		private Dictionary<int, Dictionary<string, string>> dicCarParam = new Dictionary<int, Dictionary<string, string>>();
		private bool isParamPage = false;
		private int topCount = 10;
		//private Dictionary<int, string> dicParamIDToName = new Dictionary<int, string>();
 		// 参数模板
		private Dictionary<int, List<string>> dicTemp = null;

		public void ProcessRequest(HttpContext context)
		{
			OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
			{
				Duration = 60 * 15,
				Location = OutputCacheLocation.Any,
				VaryByParam = "*"
			});
			page.ProcessRequest(HttpContext.Current);

			context.Response.ContentType = "application/x-javascript";
 			response = context.Response;
			request = context.Request;

 			GetPageParam();
 			response.Write(" var carCompareJson = [");
			GetCarParamData();
			response.Write("];");
		}

		/// <summary>
		/// 取车型ID 
		/// </summary>
		private void GetPageParam()
		{
			// 最多对比车型数量 对比页10个 参数配置页40个
			if (!string.IsNullOrEmpty(request.QueryString["isParamPage"]))
			{
				isParamPage = true;
				topCount = 50;
			}

			string carIDsStr = request.QueryString["carIDs"];
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
					dicTemp = base.GetCarParameterJsonConfigNew();
					if (dicTemp != null && dicTemp.Count > 0)
					{
						int loopCar = 0;
						foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dicCarParam)
						{
							if (loopCar > 0)
							{ response.Write(","); }

							response.Write("[");
							// 循环模板
							foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
							{
								if (kvpTemp.Key == 0)
								{
									// 基本数据
									response.Write("[\"" + kvpCar.Value["Car_ID"] + "\"");
									response.Write(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
									foreach (string param in kvpTemp.Value)
									{
										if (kvpCar.Value.ContainsKey(param))
										{ response.Write(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
										else
										{ response.Write(",\"\""); }
									}
									response.Write("]");
								}
								else
								{
									// 扩展数据
									response.Write(",[");
									int loop = 0;
									foreach (string param in kvpTemp.Value)
									{
										if (loop > 0)
										{ response.Write(","); }
										if (kvpCar.Value.ContainsKey(param))
										{ response.Write("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
										else
										{ response.Write("\"\""); }
										loop++;
									}
									response.Write("]");
								}
							}
							response.Write("]");

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

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		private sealed class OutputCachedPage : Page
		{
			private OutputCacheParameters _cacheSettings;

			public OutputCachedPage(OutputCacheParameters cacheSettings)
			{
				ID = Guid.NewGuid().ToString();
				_cacheSettings = cacheSettings;
			}

			protected override void FrameworkInitialize()
			{
				base.FrameworkInitialize();
				InitOutputCache(_cacheSettings);
			}
		}
	}
}