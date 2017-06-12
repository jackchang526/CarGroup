using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageSerialV2
{
    public partial class CsCompare : PageBase
    {
        #region Param
        protected int csID = 0;
        protected int year = 0;
        protected string yearForTitle = "";
        protected string carIDAndName = string.Empty;
        protected string carIDs = string.Empty;
        private int maxCount = 40;
        protected string CsHeadHTML = string.Empty;
        protected SerialEntity cbe = new SerialEntity();
        protected string JsTagForYear = string.Empty;
        #endregion

        #region private Member for api
        private string carJson = " var carCompareJson = ";
        private List<int> listCarID = new List<int>();
        private Dictionary<int, Dictionary<string, string>> dicCarParam = new Dictionary<int, Dictionary<string, string>>();
        private int topCount = 40;
        private Dictionary<int, string> dicParamIDToName = new Dictionary<int, string>();
        private StringBuilder sbForApi = new StringBuilder();
        protected string jsContent = "";
        // 参数模板
        private Dictionary<int, List<string>> dicTemp = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(15);
            if (!this.IsPostBack)
            {
                this.CheckParam();
                base.MakeSerialTopADCode(csID);
                this.GetCarIDByCsID();

                sbForApi.Append(carJson);
                //sbForApi.Append("[");
                sbForApi.Append(GetCarParamData());
                //sbForApi.Append("];");
                jsContent = sbForApi.ToString();
            }
        }

        #region Page Method

        // 取参数
        private void CheckParam()
        {
            if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
            {
                string csIDstr = this.Request.QueryString["csID"].ToString();
                if (int.TryParse(csIDstr, out csID))
                {
                    if (csID < 0 || csID > 10000)
                    {
                        csID = 0;
                    }
                }
            }
            if (this.Request.QueryString["year"] != null && this.Request.QueryString["year"].ToString() != "")
            {
                string yearstr = this.Request.QueryString["year"].ToString();
                if (int.TryParse(yearstr, out year))
                {
                    if (year < 0 || year > 3000)
                    {
                        year = 0;
                    }
                }
            }
            if (year > 0)
            {
                yearForTitle = " " + year + "款";
                JsTagForYear = "if(document.getElementById('carYearList_" + year.ToString() + "')){document.getElementById('carYearList_" + year.ToString() + "').className='current';}var interval = setInterval(function() {if(typeof changeSerialYearTag!='undefined'){changeSerialYearTag(0," + year.ToString() + ",'');clearInterval(interval);}}, 1000);";
            }
            else
            {
                JsTagForYear = " if (document.getElementById('carYearList_all')){ document.getElementById('carYearList_all').className = 'current'; }";
            }
        }

        // 取子品牌下车型ID
        private void GetCarIDByCsID()
        {
            if (csID > 0)
            {
                //modified by sk 2017-01-20 for wangsong 3746拓路者，2802萨普，4912 T60，这两个车的年款参配，车款参配，车系参配的显示上限都调到70
                if (csID == 3746 || csID == 2802 || csID == 4912)
                {
                    maxCount = 70;
                }
                cbe = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csID);
                DataSet ds = new DataSet();
                if (cbe.SaleState == "停销")
                {
                    // 停销子品牌取最新年款的车型
                    ds = base.GetCarIDAndNameForNoSaleCS(csID, WebConfig.CachedDuration);
                }
                else
                {
                    // 非停销子品牌按其原有逻辑
                    if (year > 0)
                    {
                        ds = base.GetCarIDAndNameForCSYear(csID, year, WebConfig.CachedDuration);
                    }
                    else
                    {
                        ds = base.GetCarIDAndNameForCS(csID, WebConfig.CachedDuration);
                    }
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Dictionary<int, string> dic382 = new Car_BasicBll().GetCarParamExDic(382);
                    var dic = from i in dic382 select new { key = i.Key, value = i.Value };
                    var enumTable = from a in ds.Tables[0].AsEnumerable()
                                    join b in dic382.ToList().AsEnumerable() on a.Field<int>("car_id") equals b.Key into all
                                    from ab in all.DefaultIfEmpty()
                                    orderby ab.Value == "平行进口" ? 1 : 0
                                    select a;

                    DataTable dt = enumTable.CopyToDataTable<DataRow>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > maxCount - 1)
                        {
                            break;
                        }
                        int carId = Convert.ToInt32(dt.Rows[i]["car_id"]);
                        //add by 2015.02.01 排量增加 T
                        Dictionary<int, string> dictCarAllParams = new Car_BasicBll().GetCarAllParamByCarID(carId);
                        var exhaust = dt.Rows[i]["Engine_Exhaust"].ToString().Trim();
                        if (dictCarAllParams.ContainsKey(425) && dictCarAllParams[425] == "增压")
                        {
                            exhaust = exhaust.Replace("L", "T");
                        }
                        if (carIDAndName != "" && carIDAndName.Length > 0)
                        {
                            carIDAndName += "|" + "id" + dt.Rows[i]["car_id"].ToString() + "," + dt.Rows[i]["car_name"].ToString().Trim() + "," + exhaust + "," + dt.Rows[i]["TransmissionType"].ToString().Trim();
                            listCarID.Add(int.Parse(dt.Rows[i]["car_id"].ToString()));
                            // carIDs += "," + ds.Tables[0].Rows[i]["car_id"].ToString();
                        }
                        else
                        {
                            carIDAndName = "id" + dt.Rows[i]["car_id"].ToString() + "," + dt.Rows[i]["car_name"].ToString().Trim() + "," + exhaust + "," + dt.Rows[i]["TransmissionType"].ToString().Trim();
                            listCarID.Add(int.Parse(dt.Rows[i]["car_id"].ToString()));
                        }
                    }
                }
                if (year > 0)
                {
                    CsHeadHTML = base.GetCommonNavigation("CsCompareForYear", csID).Replace("{0}", year.ToString());
                }
                else
                {
                    CsHeadHTML = base.GetCommonNavigation("CsCompare", csID);
                }
            }
        }

        /// <summary>
        /// 取车型详细参数
        /// </summary>
        private string GetCarParamData()
        {
            return new Car_BasicBll().GetValidCarJsObject(listCarID);
            /*
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
							{ sbForApi.Append(","); }

							sbForApi.Append("[");
							// 循环模板
							foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
							{
								if (kvpTemp.Key == 0)
								{
									// 基本数据
									sbForApi.Append("[\"" + kvpCar.Value["Car_ID"] + "\"");
									sbForApi.Append(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
									foreach (string param in kvpTemp.Value)
									{
										if (kvpCar.Value.ContainsKey(param))
										{ sbForApi.Append(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
										else
										{ sbForApi.Append(",\"\""); }
									}
									sbForApi.Append("]");
								}
								else
								{
									// 扩展数据
									sbForApi.Append(",[");
									int loop = 0;
									foreach (string param in kvpTemp.Value)
									{
										if (loop > 0)
										{ sbForApi.Append(","); }
										if (kvpCar.Value.ContainsKey(param))
										{ sbForApi.Append("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
										else
										{ sbForApi.Append("\"\""); }
										loop++;
									}
									sbForApi.Append("]");
								}
							}
							sbForApi.Append("]");

							loopCar++;
						}
					}
				}
				#endregion
			}
			 *  * */
        }
        #endregion

    }
}