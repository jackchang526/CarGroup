using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.CarChannelAPI.Web.AppCode;
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