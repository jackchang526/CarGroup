﻿using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Model.Assessment;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetPingCeTopM 的摘要说明
    /// </summary>
    public class GetPingCeTopM : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);

            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];
            int top = 6;//ConvertHelper.GetInteger(context.Request.QueryString["top"]);

            string result = "{}";
            string message = "成功";
            int status = 1;
            List<object> list = new List<object>();
            EvaluationBll evaluationBll = new EvaluationBll();
            IMongoQuery query = Query.EQ("Status", 1);
            List<string> paraList = new List<string>
            {
                "CreateDateTime",
                "CarId",
                "Status",
                "EvaluationId",
                "Score",
                "CommonInfoGroup.CommonInformationEntity.Title",
                "CommonInfoGroup.CommonInformationEntity.ImageUrl",
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
            Dictionary<string, int> sortdic = new Dictionary<string, int>();
            sortdic.Add("CreateDateTime", 0);
            List<AssessmentEntity> pingCeAllList = evaluationBll.GetPingCeAllList<AssessmentEntity>(query, top, sortdic, paraList.ToArray());
            List<int> evaluationIdList = (from item in pingCeAllList select item.EvaluationId).ToList();
            Dictionary<int, PingCeEntity> dic = evaluationBll.GetEvaluationDate(evaluationIdList);
            foreach (AssessmentEntity item in pingCeAllList)
            {
                string ImageUrl = "", Title = "", FuelType = "";
                bool Finished = true;
                if (item.CommonInfoGroup != null && item.CommonInfoGroup.CommonInformationEntity != null)
                {
                    ImageUrl = item.CommonInfoGroup.CommonInformationEntity.ImageUrl;
                    Title = item.CommonInfoGroup.CommonInformationEntity.Title;
                }
                if (dic.ContainsKey(item.EvaluationId))
                {
                    PingCeEntity temp = dic[item.EvaluationId];
                    FuelType = temp.FuelType;
                }
                Dictionary<string, double> dicGroupScore = null;
                Finished = evaluationBll.GetFinishedStatusAndGroupScore(item, out dicGroupScore);
                list.Add(
                    new
                    {
                        Score = item.Score,
                        EvaluationId = item.EvaluationId,
                        ImageUrl = ImageUrl,
                        Title = Title,
                        FuelType = FuelType,
                        Finished = Finished
                    }
                    );
            }

            var obj = new
            {
                status = status,
                message = message,
                data = list
            };
            result = JsonConvert.SerializeObject(obj);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, result) : result);
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