using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// GetCarListInfoForCompare 的摘要说明
    /// </summary>
    public class GetCarListInfoForCompare : PageBase,IHttpHandler
    {
        private string carids = string.Empty;
        private string callback = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            GetParams(context);
            RenderCarInfo(context);
        }


        private void RenderCarInfo(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(carids))
            {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            string[] carIdArray = carids.Split(',');
            foreach(string carIdStr in carIdArray)
            {
                int carId = ConvertHelper.GetInteger(carIdStr);
                if (carId > 0)
                {
                    CarEntity ce = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                    if (ce != null && ce.Id > 0)
                    {
                        sb.AppendFormat("{{\"CarID\":\"{0}\",\"CarName\":\"{1}\",\"YearType\":\"{2}\",\"CsAllSpell\":\"{3}\",\"CsName\":\"{4}\"}},", ce.Id, ce.Name, ce.CarYear > 0 ? (ce.CarYear + "款") : string.Empty, ce.Serial.AllSpell, ce.Serial.Name);
                    }
                }
            }

            if (sb.Length > 1)
            {
                sb = sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            context.Response.Write(callback + "(" + sb.ToString() + ");");
        }

        private void GetParams(HttpContext context)
        {
            carids = context.Request["carids"];
            callback = context.Request["callback"];
            callback = string.IsNullOrWhiteSpace(callback) ? "getCarData" : callback;
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