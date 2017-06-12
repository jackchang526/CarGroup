using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetCarList 的摘要说明
    /// 按子品牌id查询查款信息
    /// </summary>
    public class GetCarList : PageBase, IHttpHandler
    {
        /// <summary>
        /// 子品牌id
        /// </summary>
        private string serialIds = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private string carIds = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private string callback = string.Empty;
        /// <summary>
        /// 销售状态 0：全部，1：在销
        /// </summary>
        private int saleState = 0;

        StringBuilder sb = new StringBuilder();

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParams(context);
            string serialStr = GetSerialCarList();
            string carStr = GetCarInfoList();
            sb.Append(serialStr);
            if (carStr.Length > 0)
            {
                if (serialStr.Length > 0)
                {
                    sb.Append(",");
                }
                sb.Append(carStr);
            }
            if (string.IsNullOrWhiteSpace(callback))
            {
                context.Response.Write(sb.ToString());
            }
            else
            {
                context.Response.Write(string.Format("{0}({{{1}}})", callback, sb.ToString()));
            }
        }

        private string GetCarInfoList()
        {
            if (string.IsNullOrWhiteSpace(carIds))
            {
                return string.Empty;
            }
            string[] carIdArr = carIds.Split(',');

            List<string> list = new List<string>();
            // CarImg
            Dictionary<int, string> dicCarPhoto = new Car_BasicBll().GetCarDefaultPhotoDictionary(1);
            // 白底图
            Dictionary<int, string> dicWhitePhoto = base.GetAllSerialPicURLWhiteBackground();

            foreach (string carIdStr in carIdArr)
            {
                int carId = ConvertHelper.GetInteger(carIdStr);
                if (carId <= 0)
                {
                    continue;
                }
                CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (carEntity == null)
                {
                    continue;
                }

                var carPic = WebConfig.DefaultCarPic;
                var whitePic = WebConfig.DefaultCarPic;
               

                if (dicCarPhoto.ContainsKey(carEntity.Id))
                {
                    carPic = dicCarPhoto[carEntity.Id];
                }
                else
                {
                    carPic = Car_SerialBll.GetSerialImageUrl(carEntity.SerialId, 1, false);
                }
                if (dicWhitePhoto.ContainsKey(carEntity.SerialId))
                {
                    whitePic = dicWhitePhoto[carEntity.SerialId];
                    whitePic = whitePic.Replace("_2.", "_6.");
                }
                var serialName = carEntity.Serial == null ? "" : carEntity.Serial.ShowName;
                var carName = carEntity.CarYear + "款 " + carEntity.Name;
                list.Add(string.Format("\"{3}\":{{\"SerialId\":{0},\"SerialName\":\"{1}\",\"AllSpell\":\"{6}\",\"ImageUrl\":\"{2}\",\"CarName\":\"{4}\",\"CarPrice\":{5},\"CsWhiteImg\":\"{7}\"}}",
                    carEntity.SerialId,
                    serialName,
                    carPic,
                    carEntity.Id,
                    carName,
                 carEntity.ReferPrice,
                 carEntity.Serial.AllSpell,
                 whitePic));
            }
            return string.Format("\"car\": {{{0}}}", string.Join(",", list.ToArray()));
            //if (sbCar.Length > 0)
            //{
            //    if (sb.Length > 0)
            //    {
            //        sb.Append(",");
            //    }
            //    sb.AppendFormat("\"car\": {{0}}", sbCar.ToString().TrimEnd(',')).Append("}");
            //}
        }

        /// <summary>
        /// 输出车款列表
        /// </summary>
        private string GetSerialCarList()
        {
            //string carListStr = string.IsNullOrWhiteSpace(callback) ? "{0}" : callback + "({{{0}}})";
            if (string.IsNullOrWhiteSpace(serialIds))
            {
                return string.Empty;
            }

            Dictionary<int, string> dicCarPhoto = new Car_BasicBll().GetCarDefaultPhotoDictionary(1);
            string[] csIdArr = serialIds.Split(',');
            List<string> list = new List<string>();
            //sb.Append("\"serial\": {");
            for (int i = 0; i < csIdArr.Length; i++)
            {
                int csid = ConvertHelper.GetInteger(csIdArr[i]);
                if (csid < 1) continue;
                DataSet carDataset = new Car_BasicBll().GetAllCarInfoForSerialSummary(csid);
                if (carDataset == null || carDataset.Tables.Count == 0 || carDataset.Tables[0].Rows.Count == 0)
                {
                    continue;
                }
                DataRow[] drs = null;
                if (saleState == 0)
                {
                    drs = carDataset.Tables[0].Select();
                }
                else
                {
                    drs = carDataset.Tables[0].Select("Car_SaleState='在销'");
                }
                //list.Add(string.Format("\"{0}\":[", csid));
                List<string> listCar = new List<string>();

                foreach (DataRow dr in drs)
                {
                    var carName = string.IsNullOrWhiteSpace(dr["Car_YearType"].ToString()) ? dr["car_name"] : (dr["Car_YearType"] + "款 " + dr["car_name"]);
                    listCar.Add(string.Format("{{\"carId\":{0},\"carName\":\"{1}\",\"referPrice\":{2},\"pv\":{3},\"csName\":\"{4}\",\"allSpell\":\"{5}\",\"carPic\":\"{6}\"}}"
                        , dr["car_id"]
                        , carName
                        , dr["car_ReferPrice"]
                        , ConvertHelper.GetInteger(dr["Pv_SumNum"])
                        , dr["cs_name"]
                        , dr["allSpell"]
                        , GetCarPic(ConvertHelper.GetInteger(dr["car_id"]), csid, dicCarPhoto)));
                }
                list.Add(string.Format("\"{0}\":[{1}]", csid, string.Join(",", listCar.ToArray())));
            }
            return string.Format("\"serial\": {{{0}}}", string.Join(",", string.Join(",", list.ToArray())));
        }


        private string GetCarPic(int carId,int serialId, Dictionary<int, string> dicCarPhoto)
        {
            var carPic = WebConfig.DefaultCarPic;
            if (dicCarPhoto == null || dicCarPhoto.Count == 0)
            {
                return carPic;
            }
            // CarImg
            if (dicCarPhoto.ContainsKey(carId))
            {
                carPic = dicCarPhoto[carId];
            }
            else
            {
                carPic = Car_SerialBll.GetSerialImageUrl(serialId, 1, false);
            }
            return carPic;
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="context"></param>
        private void GetParams(HttpContext context)
        {
            serialIds = context.Request["csids"];
            carIds = context.Request["carids"];
            callback = context.Request["callback"];
            saleState = ConvertHelper.GetInteger(context.Request["salestate"]);
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