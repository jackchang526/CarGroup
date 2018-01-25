using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetPingCe 的摘要说明
    /// </summary>
    public class GetPingCe : IHttpHandler
    {
        private HttpRequest request;
        private HttpResponse response;
        private string overview = "";
        private bool isExpiredTime = false;
        private string evaluationId = "";
        private int EvaluationId = 0;

        public void ProcessRequest(HttpContext context)
        {
            //PageHelper.SetPageCache(60);

            response = context.Response;
            request = context.Request;
            response.ContentType = "application/x-javascript";
            evaluationId = request.QueryString["evaluationId"];
            int.TryParse(evaluationId, out EvaluationId);
            overview = request.QueryString["overview"];
            isExpiredTime = IsExpiredTime(overview);
            var action= request.QueryString["action"];

            switch (action)
            {   
                case "intr"://introduction 超级评测2.0封面+导语
                    Introduction();
                    break;
                case "ass"://assessment 获取Assessment对象信息
                    GetAssessmentInfo();
                    break;
                case "menu"://导航
                    GetMenuInfo();
                    break;
                default:
                    //GetAssessmentInfo();
                    break;
            }
            
        }

        /// <summary>
        /// 超级评测2.0封面+导语
        /// </summary>
        private void Introduction()
        {
            int status = 0;
            string message = "success";
            List<string> paraList = new List<string>
            {
                "CreateDateTime",
                "UpdateDateTime",
                "SerialId",
                "CarId",
                "Status",
                "EvaluationId",
                "Score",
                "CommonInfoGroup.CommonInformationEntity",
                "BodyAndSpaceGroup.Score",
                "RidingComfortGroup.Score",
                "DynamicPerformanceGroup.Score",
                "JsBaseGroup.Score",
                "SafetyGroup.Score",
                "YhBaseGroup.Score",
                "YhBaseGroup.YaEntity",
                "CostBaseGroup.Score",
                "GeneralBaseGroupl.Score"
            };

            Dictionary<string, int> sortdic = new Dictionary<string, int>
            {
                { "CreateDateTime", 0 }
            };
            EvaluationBll evaluationBll = new EvaluationBll();
            AssessmentEntity assessmentEntity = null;
            IMongoQuery query = null;
            if (!string.IsNullOrWhiteSpace(overview))//预览
            {
                if (isExpiredTime)// 没有过期
                {
                    query = Query.EQ("EvaluationId", EvaluationId);
                    try
                    {
                        assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
                    }
                    catch (Exception e)
                    {
                        status = 1;
                        message = e.Message;
                    }
                }
            }
            else//非预览
            {
                query = Query.And(Query.EQ("EvaluationId", EvaluationId), Query.EQ("Status", 1));
                try
                {
                    assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
                }
                catch (Exception e)
                {
                    status = 1;
                    message = e.Message;
                }

            }

            Dictionary<string, double> dicGroupScore = null;

            bool finished = true;

            WriteResult<object> writeResult = new WriteResult<object>();

            if (assessmentEntity != null)
            {
                var carName=GetCarName(assessmentEntity.CarId);

                finished = evaluationBll.GetFinishedStatusAndGroupScore(assessmentEntity, out dicGroupScore);

                writeResult.Data = new {
                    ImageUrl = assessmentEntity.CommonInfoGroup.CommonInformationEntity.ImageUrl,
                    MainImageUrl = assessmentEntity.CommonInfoGroup.CommonInformationEntity.MainImageUrl,
                    VideoUrl = assessmentEntity.CommonInfoGroup.CommonInformationEntity.VideoUrl,
                    Preface = assessmentEntity.CommonInfoGroup.CommonInformationEntity.Preface,
                    Score =assessmentEntity.Score,
                    Finished = finished,
                    CarName = carName,
                    GroupScore = dicGroupScore
                };
            }

            writeResult.Status = status;
            writeResult.Message = message;
            PingCeWrite(writeResult);
        }
        
        /// <summary>
        /// 获取评测报告信息
        /// </summary>
        private void GetAssessmentInfo()
        {
            int status = 0;
            string message = "success";
            string groupname = request.QueryString["groupname"];
            List<string> paraList = new List<string>
            {
                "CreateDateTime",
                "UpdateDateTime",
                "SerialId",
                "CarId",
                "Status",
                "EvaluationId",
                "Score"               
            };
            if (!string.IsNullOrEmpty(groupname))
            {
                foreach (string item in groupname.Split(','))
                {
                    if (item != "" && !paraList.Contains(item))
                    {
                        paraList.Add(item);
                    }
                }
            }
            Dictionary<string, int> sortdic = new Dictionary<string, int>
            {
                { "CreateDateTime", 0 }
            };
            EvaluationBll evaluationBll = new EvaluationBll();
            AssessmentEntity assessmentEntity = null;
            IMongoQuery query = null;
            if (!string.IsNullOrWhiteSpace(overview))//预览
            {
                if (isExpiredTime)// 没有过期
                {
                    query = Query.EQ("EvaluationId", EvaluationId);
                    try
                    {
                        assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
                    }
                    catch (Exception e)
                    {
                        status = 1;
                        message = e.Message;
                    }
                }
            }
            else//非预览
            {
                query = Query.And(Query.EQ("EvaluationId", EvaluationId), Query.EQ("Status", 1));
                try
                {
                    assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
                }
                catch (Exception e)
                {
                    status = 1;
                    message = e.Message;
                }

            }

            WriteResult<AssessmentEntity> writeResult = new WriteResult<AssessmentEntity>();

            if (assessmentEntity != null)
            {
                var carName = GetCarName(assessmentEntity.CarId);                
                assessmentEntity.CarName = carName;
                writeResult.Data = assessmentEntity;
            }

            writeResult.Status = status;
            writeResult.Message = message;
            PingCeWrite(writeResult);
        }
        
        /// <summary>
        /// M站菜单接口
        /// </summary>
        private void GetMenuInfo()
        {
            //AssessmentEntity assessmentEntity = null;
            BsonDocument bson = null;

            int status = 0;
            string message = "success";
            List<string> paraList = new List<string>
            {
                //"CreateDateTime",
                //"UpdateDateTime",
                //"SerialId",
                //"CarId",
                //"Status",
                //"EvaluationId",
                //"Score",

                "BodyAndSpaceGroup.SpaceEntity.ImpressionScore",
                "BodyAndSpaceGroup.SpaceEntity.ActualScore",
                "BodyAndSpaceGroup.TrunkEntity.Cubage",
                "BodyAndSpaceGroup.StoragespaceEntity.Score",
                "BodyAndSpaceGroup.ConvenienceEntity.Equipment",
                "BodyAndSpaceGroup.ConvenienceEntity.Multimedia",
                "BodyAndSpaceGroup.QualityEntity.Score",

                "RidingComfortGroup.ChairEntity.FrontChair",
                "RidingComfortGroup.ChairEntity.BehindChair",
                "RidingComfortGroup.AirConditionerEntity.AirConditionerFeel",
                "RidingComfortGroup.AirConditionerEntity.Refrigeration",
                "RidingComfortGroup.NoiseEntity.NoiseScore",
                "RidingComfortGroup.NoiseFeelEntity.IdlingNoise",
                "RidingComfortGroup.NoiseFeelEntity.RunNoise",
                "RidingComfortGroup.NoiseFeelEntity.NoiseFeel",
                "RidingComfortGroup.HangComfortEntity.SuspensionComfort",

                "DynamicPerformanceGroup.AccelerateEntity.AccelerateScroe",
                "DynamicPerformanceGroup.EngineEntity.Torque",
                "DynamicPerformanceGroup.EngineEntity.TractiveForce",
                "DynamicPerformanceGroup.EngineEntity.Power",
                "DynamicPerformanceGroup.MotilityEntity.Score",
                "DynamicPerformanceGroup.EngineSmoothnessEntity.Score",
                "DynamicPerformanceGroup.GearboxEntity.Score",

                "JsBaseGroup.JdaEntity.GripScore",
                "JsBaseGroup.JdaEntity.ControllabilityScore",
                "JsBaseGroup.JdaEntity.DriverSenseScore",
                "JsBaseGroup.JtsEntity.TurnDiameterScore",
                "JsBaseGroup.JtsEntity.SteeringWheelScore",
                //"JsBaseGroup.JtsEntity.StraightPerferScore",
                "JsBaseGroup.JdfEntity.DriveFunScore",

                "SafetyGroup.BrakeEntity.EmptyScore",
                "SafetyGroup.BrakeEntity.FullScore",
                "SafetyGroup.BrakeEntity.BrakeFeel",

                "SafetyGroup.ActiveSafetyEntity.DriveSafety",
                "SafetyGroup.ActiveSafetyEntity.SafetyDevice",
                "SafetyGroup.VisualFieldEntity.ViewScore",
                "SafetyGroup.LightEntity.LightScore",


                "YhBaseGroup.YgEntity.LowSpeedYhScore",
                "YhBaseGroup.YgEntity.HighSpeedYhScore",
                "YhBaseGroup.YgEntity.MiitYhScore",

                "YhBaseGroup.YaEntity.AirQualityGeneralCmt",
                "YhBaseGroup.YaEntity.AirQualityScore",

                "CostBaseGroup.CpEntity.ProducerPriceScore",
                "CostBaseGroup.CsyEntity.OilPayScore",
                "CostBaseGroup.CsyEntity.InsuranceExpenseScore",
                "CostBaseGroup.CsyEntity.MaintenanceCostScore",
                "CostBaseGroup.CgpEntity.GuaranPeriodScore",

                "CostBaseGroup.CgqEntity.GuaranQualityScore"
                

            };

            Dictionary<string, int> sortdic = new Dictionary<string, int>
            {
                { "CreateDateTime", 0 }
            };
            EvaluationBll evaluationBll = new EvaluationBll();
            IMongoQuery query = null;
            if (!string.IsNullOrWhiteSpace(overview))//预览
            {
                if (isExpiredTime)// 没有过期
                {
                    query = Query.EQ("EvaluationId", EvaluationId);
                    try
                    {
                        //assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
                        bson = evaluationBll.GetOne<BsonDocument>(query, paraList.ToArray(), sortdic);
                    }
                    catch (Exception e)
                    {
                        status = 1;
                        message = e.Message;
                    }
                }
            }
            else//非预览
            {
                query = Query.And(Query.EQ("EvaluationId", EvaluationId), Query.EQ("Status", 1));
                try
                {
                    //assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query, paraList.ToArray(), sortdic);
                    bson = evaluationBll.GetOne<BsonDocument>(query, paraList.ToArray(), sortdic);
                }
                catch (Exception e)
                {
                    status = 1;
                    message = e.Message;
                }
            }
            
            if (bson != null)
            {
                bson.RemoveAt(0);
            }
            BsonElement bs_data = new BsonElement("Data", bson);
            BsonElement bs_status = new BsonElement("Status", status);
            BsonElement bs_message = new BsonElement("Message", message);
            BsonWrite(bs_status, bs_message,bs_data);
        }       

        private string GetCarName(int carId)
        {
            var carName = "";
            CarEntity cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, carId);
            if (cbe != null)
            {
                carName = CommonFunction.GetUnicodeByString(cbe.Serial.ShowName + " " + cbe.CarYear + "款" + cbe.Name);
            }
            return carName;
        }

        private void BsonWrite(BsonElement status, BsonElement message, BsonElement data)
        {
            BsonDocument bsonDocument = new BsonDocument();
            bsonDocument.InsertAt(0, status);
            bsonDocument.InsertAt(1, message);
            bsonDocument.InsertAt(2, data);
            var json = bsonDocument.ToJson();
            string callback = request.QueryString["callback"];
            response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, json) : json);
        }

        private void PingCeWrite<T>(WriteResult<T> result)
        {
            string callback = request.QueryString["callback"];
            var json = JsonConvert.SerializeObject(result);
            response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, json) : json);
        }

        private bool IsExpiredTime(string overview)
        {
            bool isOverView = false;
            try
            {
                //string res = Decrypt(overview, "bitautocom");
                string res = CarChannel.Common.DES.DecryptDES(overview, "bitautocom");
                DateTime dt = Convert.ToDateTime(res);
                if (DateTime.Compare(dt, DateTime.Now) >= 0)
                {
                    isOverView = true;
                }
            }
            catch (Exception e)
            {
                var msg = e.Message;
            }
            return isOverView;
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