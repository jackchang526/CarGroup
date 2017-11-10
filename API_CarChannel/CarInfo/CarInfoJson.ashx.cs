using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
    /// <summary>
    /// CarInfoJson 的摘要说明
    /// add by chengl Jun.10.2015 子品牌还关注jsonp接口 for zhuyx
    /// </summary>
    public class CarInfoJson : PageBase, IHttpHandler
    {
        HttpResponse response;
        HttpRequest request;

        private string callback = string.Empty;

        public void ProcessRequest(HttpContext context)
        {
            OutputCachedPage page = new OutputCachedPage(new OutputCacheParameters
            {
                Duration = 60 * 60,
                Location = OutputCacheLocation.Any,
                VaryByParam = "*"
            });
            page.ProcessRequest(HttpContext.Current);

            context.Response.ContentType = "application/x-javascript";
            response = context.Response;
            request = context.Request;
            string action = ConvertHelper.GetString(request.QueryString["action"]);
            callback = request.QueryString["callback"];
            switch (action.ToLower())
            {
                case "baseinfo": RenderCarInfo(); break;
                case "getcstocsbyid": RenderCsToCsByID(); break;
                case "getcarparamsforpingcedata": RenderCarParams(); break;
                default: break;
            }
        }

        private void RenderCarParams()
        {
            int carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
            if (carId <= 0)
            {
                Jsonp("{}", callback, HttpContext.Current);
            }
            string sql = @"SELECT  car.Car_Id, cs.cs_Id, cs.carlevel, cs.ModelLevelSecond,
                            car.car_ReferPrice, cdb1.Pvalue AS OutSet_Length,
                            cdb2.Pvalue AS OutSet_Width, cdb3.Pvalue AS OutSet_Height,
                            cdb4.Pvalue AS OutSet_WheelBase, cdb5.Pvalue AS Engine_Location,
                            cdb6.Pvalue AS Perf_DriveType,
                            cdb7.Pvalue AS UnderPan_FrontTyreStandard,
                            cdb8.Pvalue AS UnderPan_RearTyreStandard,
                            cdb9.Pvalue AS UnderPan_RimMaterial
                    FROM    dbo.Car_relation car
                            LEFT JOIN dbo.Car_Serial cs ON car.Cs_Id = cs.cs_Id
                            LEFT JOIN dbo.CarDataBase cdb1 ON car.Car_Id = cdb1.CarId
                                                              AND cdb1.ParamId = 588
                            LEFT JOIN dbo.CarDataBase cdb2 ON car.Car_Id = cdb2.CarId
                                                              AND cdb2.ParamId = 593
                            LEFT JOIN dbo.CarDataBase cdb3 ON car.Car_Id = cdb3.CarId
                                                              AND cdb3.ParamId = 586
                            LEFT JOIN dbo.CarDataBase cdb4 ON car.Car_Id = cdb4.CarId
                                                              AND cdb4.ParamId = 592
                            LEFT JOIN dbo.CarDataBase cdb5 ON car.Car_Id = cdb5.CarId
                                                              AND cdb5.ParamId = 428
                            LEFT JOIN dbo.CarDataBase cdb6 ON car.Car_Id = cdb6.CarId
                                                              AND cdb6.ParamId = 655
                            LEFT JOIN dbo.CarDataBase cdb7 ON car.Car_Id = cdb7.CarId
                                                              AND cdb7.ParamId = 729
                            LEFT JOIN dbo.CarDataBase cdb8 ON car.Car_Id = cdb8.CarId
                                                              AND cdb8.ParamId = 721
                            LEFT JOIN dbo.CarDataBase cdb9 ON car.Car_Id = cdb9.CarId
                                                              AND cdb9.ParamId = 704
                    WHERE   car.Car_Id = @CarId
                            AND car.IsState = 0
                            AND cs.IsState = 0";
            SqlParameter[] _params = {
                                     new SqlParameter("@CarId",SqlDbType.Int)
                                     };
            _params[0].Value = carId;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _params);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int carLevel = ConvertHelper.GetInteger(dr["carlevel"]);
                int secondLevel = ConvertHelper.GetInteger(dr["ModelLevelSecond"]);
                var json = new
                {
                    CarId = carId,
                    CsId = ConvertHelper.GetInteger(dr["cs_id"]),
                    CarLevel = secondLevel > 0 ? CarLevelDefine.GetSelectCarLevelNameByClassId(secondLevel) : CarLevelDefine.GetSelectCarLevelNameByClassId(carLevel),
                    CarReferPrice = ConvertHelper.GetDouble(dr["car_ReferPrice"]),
                    OutSet_Length = dr["OutSet_Length"],
                    OutSet_Width = dr["OutSet_Width"],
                    OutSet_Height = dr["OutSet_Height"],
                    OutSet_WheelBase = dr["OutSet_WheelBase"],
                    Engine_Location = dr["Engine_Location"],
                    Perf_DriveType = dr["Perf_DriveType"],
                    UnderPan_FrontTyreStandard = dr["UnderPan_FrontTyreStandard"],
                    UnderPan_RearTyreStandard = dr["UnderPan_RearTyreStandard"],
                    UnderPan_RimMaterial = dr["UnderPan_RimMaterial"]
                };
                Jsonp(JsonConvert.SerializeObject(json), callback, HttpContext.Current);
                return;
            }
            Jsonp("{}", callback, HttpContext.Current);
        }

        /// <summary>
        /// 子品牌还关注数据
        /// </summary>
        private void RenderCsToCsByID()
        {
            int csid = ConvertHelper.GetInteger(request.QueryString["csid"]);
            if (csid <= 0)
            {
                Jsonp("", callback, HttpContext.Current);
                return;
            }
            SerialEntity csEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
            if (csEntity == null)
            {
                Jsonp("", callback, HttpContext.Current);
                return;
            }

            int top = ConvertHelper.GetInteger(request.QueryString["top"]);
            if (top <= 0 || top > 10)
            { top = 6; }

            Dictionary<int, string> dicPicWhite = base.GetAllSerialPicURLWhiteBackground();
            List<EnumCollection.SerialToSerial> listSTS = base.GetSerialToSerialByCsID(csid, top);
            string json = "";
            if (listSTS != null && listSTS.Count > 0)
            {
                List<string> listTemp = new List<string>();
                int loop = 0;
                string stringJsonTemp = "{{\"CsID\":{0},\"Name\":\"{1}\",\"ShowName\":\"{2}\",\"Pic\":\"{3}\",\"PriceRange\":\"{4}\",\"AllSpell\":\"{5}\"}}";
                foreach (EnumCollection.SerialToSerial sts in listSTS)
                {
                    if (loop >= top)
                    { break; }
                    loop++;
                    listTemp.Add(string.Format(stringJsonTemp
                        , sts.ToCsID
                        , CommonFunction.GetUnicodeByString(sts.ToCsName)
                        , CommonFunction.GetUnicodeByString(sts.ToCsShowName)
                        , (dicPicWhite.ContainsKey(sts.ToCsID) ? dicPicWhite[sts.ToCsID] : WebConfig.DefaultCarPic)
                        , sts.ToCsPriceRange
                        , sts.ToCsAllSpell));
                }
                if (listTemp.Count > 0)
                {
                    json = string.Concat("[", string.Join(",", listTemp.ToArray()), "]");
                }
            }
            Jsonp(json, callback, HttpContext.Current);
        }

        private void RenderCarInfo()
        {
            int carId = ConvertHelper.GetInteger(request.QueryString["carid"]);
            if (carId <= 0)
            {
                Jsonp("", callback, HttpContext.Current);
                return;
            }
            CarEntity carEntity = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
            if (carEntity == null)
            {
                Jsonp("", callback, HttpContext.Current);
                return;
            }

            var carPic = WebConfig.DefaultCarPic;

            // CarImg
            Dictionary<int, string> dicCarPhoto = new Car_BasicBll().GetCarDefaultPhotoDictionary(1);
            if (dicCarPhoto.ContainsKey(carEntity.Id))
            {
                carPic = dicCarPhoto[carEntity.Id];
            }
            else
            {
                carPic = Car_SerialBll.GetSerialImageUrl(carEntity.SerialId, 1, false);
            }

            var serialName = carEntity.Serial == null ? "" : carEntity.Serial.ShowName;
            var carName = carEntity.CarYear + "款 " + carEntity.Name;
            var json = string.Format("{{SerialId:{0},SerialName:\"{1}\",AllSpell:\"{6}\",ImageUrl:\"{2}\",CarId:{3},CarName:\"{4}\",CarPrice:{5}}}",
                carEntity.SerialId,
                serialName,
                carPic,
                carEntity.Id,
             CommonFunction.GetUnicodeByString(carName),
             carEntity.ReferPrice,
             carEntity.Serial.AllSpell);
            Jsonp(json, callback, HttpContext.Current);
        }

        private void Jsonp(string content, string callbackName, HttpContext context)
        {
            if (string.IsNullOrEmpty(callbackName))
                context.Response.Write(content);
            else
                context.Response.Write(string.Format("{1}({0})", content, callbackName));
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