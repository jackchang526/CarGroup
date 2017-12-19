using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model.AppModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.WxApp
{
    /// <summary>
    /// GetCarParamList 的摘要说明
    /// </summary>
    public class GetCarParamList : IHttpHandler
    {
        string carIds = string.Empty;
        string dataType = "paramgroup";
        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "application/json";// "application/x-javascript";
            carIds = context.Request.QueryString["carIds"];
            dataType = context.Request.QueryString["dataType"] ?? "paramgroup";
            List<int> carList = new List<int>();
            List<CarParameterListEntity> carParams = null;
            List<ParameterGroupEntity> carParamGroup = null;
            Car_BasicBll basicbll = new Car_BasicBll();
            if (dataType.ToLower() != "group")
            {
                if (string.IsNullOrWhiteSpace(carIds) || (!Regex.IsMatch(carIds, @"([,0-9]*)")))
                {
                    context.Response.Write("");
                    context.Response.End();
                }
                string[] carArr = carIds.Split(new char[] { ',' });
                foreach (var item in carArr)
                {
                    int carid = TypeParse.StrToInt(item, 0);
                    if (carid > 0)
                    {
                        carList.Add(carid);
                    }
                }
                carParams = basicbll.GetCarParamterListWithWebCacheByCarIds(carList, true);
            }
            if (dataType.ToLower() != "param")
            {
                carParamGroup = basicbll.GetCarParameterJsonConfig(true);
            }
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore , ContractResolver=new CamelCasePropertyNamesContractResolver() };
            var result = JsonConvert.SerializeObject(new
            {
                status = 1,
                message = "ok",
                data = new
                {
                    carParams = carParams,
                    carParamGroup = carParamGroup
                }
            }, jSetting);
            context.Response.Write(result);
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