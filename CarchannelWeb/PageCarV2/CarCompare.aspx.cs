using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.CarchannelWeb.PageCarV2
{
    public partial class CarCompare : PageBase
    {
        #region Param
        protected int carID = 0;
        protected string carIDAndName = string.Empty;
        protected string CarHeadHTML = string.Empty;
        protected CarEntity cbe;
        protected string carIDs = string.Empty;
        private string firstCarIDAndName = string.Empty;
        #endregion

        #region private Member for api
        private string carJson = " var carCompareJson = ";
        private string packageJson = "var optionalPackageJson = ";
        private List<int> listCarID = new List<int>();
        private Dictionary<int, Dictionary<string, string>> dicCarParam = new Dictionary<int, Dictionary<string, string>>();
        private Dictionary<int, string> dicParamIDToName = new Dictionary<int, string>();
        private StringBuilder sbForApi = new StringBuilder();
        protected string jsContent = "";
        protected string packageJsContent = string.Empty;
        // 参数模板
        private Dictionary<int, List<string>> dicTemp = null;
        Car_SerialBll carSerialBll = new Car_SerialBll();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageCache(60);
            if (!this.IsPostBack)
            {
                this.CheckParam();
                this.GetCarDataAndSerialData();

                sbForApi.Append(carJson);
                sbForApi.Append("[");
                sbForApi.Append(GetCarParamData());
                sbForApi.Append("];");
                jsContent = sbForApi.ToString();
                GetSerialOptionalPackageData();
            }
        }

        #region Page Method

        // 检查参数
        private void CheckParam()
        {
            if (this.Request.QueryString["carID"] != null && this.Request.QueryString["carID"].ToString() != "")
            {
                string carIDStr = this.Request.QueryString["carID"].ToString();
                if (int.TryParse(carIDStr, out carID))
                {
                }
            }
        }

        /// <summary>
        /// 取车型数据
        /// </summary>
        private void GetCarDataAndSerialData()
        {
            if (carID > 0)
            {
                cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carID);
                if (cbe != null)
                {
                    // 广告
                    base.MakeSerialTopADCode(cbe.Serial.Id);
                }
                else
                    Response.Redirect("/car/404error.aspx?info=无效车款");

                // 取10个车型 按厂商指导价排序
                DataSet ds = base.GetAllCarInfo();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string csID = "";
                    bool isHasFirstCarInfo = false;
                    DataRow[] drsCar = ds.Tables[0].Select(" car_id = " + carID.ToString() + " ");
                    if (drsCar != null && drsCar.Length > 0)
                    {
                        csID = drsCar[0]["cs_id"].ToString();
                        firstCarIDAndName = "id" + carID.ToString() + "," + drsCar[0]["car_name"].ToString().Trim();
                        listCarID.Add(carID);
                        // carIDs = carID.ToString();
                    }
                    if (csID != "")
                    {
                        DataSet dsCar = base.GetCarIDAndNameForCS(int.Parse(csID), WebConfig.CachedDuration);
                        if (dsCar != null && dsCar.Tables.Count > 0 && dsCar.Tables[0].Rows.Count > 0)
                        {
                            int loop = 0;
                            foreach (DataRow dr in dsCar.Tables[0].Rows)
                            {
                                //add by 2015.02.01 排量增加 T
                                int carId = Convert.ToInt32(dr["car_id"]);
                                Dictionary<int, string> dictCarAllParams = new Car_BasicBll().GetCarAllParamByCarID(carId);
                                var exhaust = dr["Engine_Exhaust"].ToString().Trim();
                                if (dictCarAllParams.ContainsKey(425) && dictCarAllParams[425].Contains("增压"))
                                {
                                    exhaust = exhaust.Replace("L", "T");
                                }

                                if (dr["car_id"].ToString() != carID.ToString())
                                {
                                    if (loop >= 50)
                                    {
                                        break;
                                    }
                                    if (dr["car_id"].ToString() == carID.ToString())
                                    {
                                        continue;
                                    }
                                    carIDAndName += "|id" + dr["car_id"].ToString() + "," + dr["car_name"].ToString().Trim();
                                    carIDAndName += "," + exhaust + "," + dr["TransmissionType"].ToString().Trim();
                                    listCarID.Add(int.Parse(dr["car_id"].ToString()));
                                    // carIDs += "," + dr["car_id"].ToString();
                                    loop++;
                                }
                                else
                                {
                                    isHasFirstCarInfo = true;
                                    firstCarIDAndName += "," + exhaust + "," + dr["TransmissionType"].ToString().Trim();
                                }
                            }
                        }
                        // add by chengl Jul.29.2013
                        if (!isHasFirstCarInfo)
                        {
                            // 如果非停销车型信息中不包含首车信息，则再取下首车排量和变速器
                            Dictionary<int, string> dicCar = new Car_BasicBll().GetCarAllParamByCarID(carID);
                            // 排量
                            firstCarIDAndName += "," + ((dicCar.ContainsKey(785) && dicCar[785] != "") ? dicCar[785] + "L" : "");
                            // 变速器
                            firstCarIDAndName += "," + SwithTransmissionType(dicCar.ContainsKey(712) ? dicCar[712] : "");
                        }
                    }
                    carIDAndName = firstCarIDAndName + carIDAndName;
                }
                string subDir = Convert.ToString(carID / 1000);
                // CarHeadHTML = base.GetCommonNavigation("CarCompare\\" + subDir, carID);
                CarHeadHTML = base.GetCommonNavigation("CarCompare", carID);
            }
        }

        /// <summary>
        /// 转换变速器
        /// </summary>
        /// <param name="transmissionType"></param>
        /// <returns></returns>
        private string SwithTransmissionType(string transmissionType)
        {
            string tt = "";
            if (transmissionType.EndsWith("手动"))
            { tt = "1"; }
            else if (transmissionType.EndsWith("自动"))
            { tt = "2"; }
            else if (transmissionType.EndsWith("手自一体"))
            { tt = "3"; }
            else if (transmissionType.EndsWith("半自动"))
            { tt = "4"; }
            else if (transmissionType.EndsWith("CVT无级变速"))
            { tt = "5"; }
            else if (transmissionType.EndsWith("双离合"))
            { tt = "6"; }
            else { }
            return tt;
        }

        /// <summary>
        /// 取车型详细参数
        /// </summary>
        private string GetCarParamData()
        {
            return (new Car_BasicBll()).GetValidCarJsObject(listCarID);
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
                                        if (param == "InStat_MultiFuncsSteer")
                                        {
                                        }
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
            */
        }
        /// <summary>
        /// 获取车系选配包json
        /// </summary>
        private void GetSerialOptionalPackageData()
        {
            packageJsContent = packageJson + carSerialBll.GetSerialOptionalPackageJson(cbe.SerialId);
        }
        #endregion
    }
}