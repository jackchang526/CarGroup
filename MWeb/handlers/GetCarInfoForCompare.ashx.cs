using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.Utils;

namespace MWeb.handlers
{
    /// <summary>
    /// GetCarInfoForCompare 的摘要说明
    /// </summary>
    public class GetCarInfoForCompare : IHttpHandler
    {
        private HttpResponse response;
        private HttpRequest request;
        
        public void ProcessRequest(HttpContext context)
        {
			BitAuto.CarChannel.Common.Cache.CacheManager.SetPageCache(30);
            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            request = context.Request;

            RenderContent();
        }
        private void RenderContent()
        {
            List<string> list = new List<string>();
            string carIds = request.QueryString["carid"];
            string callback = request.QueryString["callback"];
            if (string.IsNullOrEmpty(carIds))
            {
                if (string.IsNullOrEmpty(callback))
                    response.Write("[]");
                else
                    response.Write(string.Format("{0}([])", callback));
                return;
            }
            var arrCarId = carIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            foreach (string tempCarId in arrCarId)
            {
                int carId = ConvertHelper.GetInteger(tempCarId);
                if (carId <= 0) continue;
                CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
                if (carEntity != null && carEntity.Id > 0)
                {
					string carFullName = string.Format("{0}{1} {2}", carEntity.Serial.ShowName, (carEntity.CarYear > 0 ? string.Format(" {0}款", carEntity.CarYear) : string.Empty), carEntity.Name);
                    list.Add(string.Format("{{\"SerialId\":{0},\"SerialShowName\":\"{1}\",\"CarId\":{2},\"CarName\":\"{3}\",\"YearType\":{4},\"CarReferPrice\":{5}}}",
                        carEntity.SerialId, carEntity.Serial.ShowName, carEntity.Id, carFullName, carEntity.CarYear, carEntity.ReferPrice));
                }
            }
            if (string.IsNullOrEmpty(callback))
                response.Write(string.Format("[{0}]", string.Join(",", list.ToArray())));
            else
                response.Write(string.Format("{1}([{0}])", string.Join(",", list.ToArray()), callback));
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