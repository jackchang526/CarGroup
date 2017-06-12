using System.Collections.Generic;
using System.Data;
using System.Security;
using System.Web;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using Newtonsoft.Json;

namespace H5Web.Interface.cooperation
{
    /// <summary>
    ///     车一百合作接口
    /// </summary>
    public class CarDataSerial : H5PageBase, IHttpHandler
    {
        private readonly Car_BasicBll _carBasic = new Car_BasicBll();
        private readonly Car_SerialBll _csCarSerialBll = new Car_SerialBll();
        private string _callback = string.Empty;
        private HttpRequest request;
        private HttpResponse response;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            response = context.Response;
            request = context.Request;
            _callback = ConvertHelper.GetString(request.QueryString["callback"]);
            var action = ConvertHelper.GetString(request.QueryString["action"]);
            switch (action)
            {
                case "serial":
                    SerialInfo();
                    break;
                case "car":
                    var csid = ConvertHelper.GetInteger(request.QueryString["csid"]);
                    CarInfo(csid);
                    break;
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private void SerialInfo()
        {
            var cacheKey = "Interface_CarDataSerial_SerialInfo";

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                WriteDate(obj.ToString());
            }
            else
            {
                var serailList = new List<object>();
                var dsCs = _csCarSerialBll.GetAllValidSerial();
                if (dsCs != null && dsCs.Tables.Count > 0 && dsCs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCs.Tables[0].Rows)
                    {
                        var csSaleState = dr["CsSaleState"].ToString();
                        if (csSaleState.Trim() == "在销")
                        {
                            var csid = int.Parse(dr["cs_id"].ToString());
                            var csallSpell = SecurityElement.Escape(dr["allspell"].ToString().Trim());
                            serailList.Add(
                                new
                                {
                                    Id = csid,
                                    Url = "http://car.h5.yiche.com/" + csallSpell + "/?WT.mc_id=mcyb7221h5&order=page7"
                                });
                        }
                    }
                }

                var res = JsonConvert.SerializeObject(new
                {
                    list = serailList
                });
                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
                WriteDate(res);
            }
        }

        private void CarInfo(int csid)
        {
            var cacheKey = "Interface_CarDataSerial_CarInfo_" + csid;

            var obj = CacheManager.GetCachedData(cacheKey);

            if (obj != null)
            {
                WriteDate(obj.ToString());
            }
            else
            {
                var serailList = new List<object>();
                var allCarOrderbyCs = _carBasic.Get_Car_BasicByCsID(csid);
                foreach (var item in allCarOrderbyCs)
                {
                    if (item.Car_SaleState == "在销")
                    {
                        serailList.Add(
                        new
                        {
                            Id = item.Car_Id,
                            Url =
                                "http://dealer.h5.yiche.com/MultiOrder/" + csid + "/" + item.Car_Id +
                                "/?leads_source=H001005&WT.mc_id=mcyb7222h5"
                        });
                    }
                }
                var res = JsonConvert.SerializeObject(new
                {
                    list = serailList
                });
                CacheManager.InsertCache(cacheKey, res, WebConfig.CachedDuration);
                WriteDate(res);
            }
        }

        private void WriteDate(string content)
        {
            if (!string.IsNullOrEmpty(_callback))
                content = string.Format("{0}({1})", _callback, content);
            response.Write(content);
        }
    }
}