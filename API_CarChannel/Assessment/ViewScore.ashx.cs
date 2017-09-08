﻿using BitAuto.CarChannel.BLL;
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
    /// ViewScore 的摘要说明
    /// </summary>
    public class ViewScore : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            string callback = context.Request.QueryString["callback"];

            int EvaluationId = 0;
            string evaluationId = context.Request.QueryString["evaluationId"];
            int.TryParse(evaluationId, out EvaluationId);


            //List<string> paraList = new List<string>();
            ////ok
            //paraList.Add("CommonInfoGroup.CommonInformationEntity");
            ////ok
            //paraList.Add("BodyAndSpaceGroup.SpaceEntity");            
            //paraList.Add("BodyAndSpaceGroup.TrunkEntity");
            //paraList.Add("BodyAndSpaceGroup.StoragespaceEntity");
            //paraList.Add("BodyAndSpaceGroup.ConvenienceEntity");
            //paraList.Add("BodyAndSpaceGroup.QualityEntity");
            ////ok
            //paraList.Add("RidingComfortGroup.ChairEntity");
            //paraList.Add("RidingComfortGroup.AirConditionerEntity");
            //paraList.Add("RidingComfortGroup.HangComfortEntity");
            //paraList.Add("RidingComfortGroup.NoiseEntity");
            //paraList.Add("RidingComfortGroup.NoiseFeelEntity");
            ////ok
            //paraList.Add("DynamicPerformanceGroup.EditorCommentEntity");
            //paraList.Add("DynamicPerformanceGroup.AccelerateEntity");
            //paraList.Add("DynamicPerformanceGroup.EngineEntity");
            //paraList.Add("DynamicPerformanceGroup.MotilityEntity");
            //paraList.Add("DynamicPerformanceGroup.EngineSmoothnessEntity");
            //paraList.Add("DynamicPerformanceGroup.GearboxEntity");
            ////ok
            //paraList.Add("SafetyGroup.BrakeEntity");
            //paraList.Add("SafetyGroup.ActiveSafetyEntity");
            //paraList.Add("SafetyGroup.VisualFieldEntity");
            //paraList.Add("SafetyGroup.LightEntity");
            ////ok
            //paraList.Add("YhBaseGroup.YhAirEntity");
            //paraList.Add("YhBaseGroup.YhGeneralEntity");
            ////ok
            //paraList.Add("JsBaseGroup.JsDriverAbilityEntity");
            //paraList.Add("JsBaseGroup.JsDriverFunEntity");
            //paraList.Add("JsBaseGroup.JsTurnSystemEntity");
            ////ok
            //paraList.Add("CostBaseGroup.CostGuaranPeriodEntity");
            //paraList.Add("CostBaseGroup.CostGuaranQualityEntity");
            //paraList.Add("CostBaseGroup.CostProducerEntity");
            //paraList.Add("CostBaseGroup.CostSixYearEntity");
            ////ok
            //paraList.Add("GeneralBaseGroup.GeneralSummaryEntity");


            IMongoQuery query = Query.EQ("EvaluationId", EvaluationId);            
            EvaluationBll evaluationBll = new EvaluationBll();            
            AssessmentEntity assessmentEntity = evaluationBll.GetOne<AssessmentEntity>(query);

            List<ViewScoreEntity> list = new List<ViewScoreEntity>();
            if (assessmentEntity != null)
            {
                if (assessmentEntity.CommonInfoGroup != null)
                {
                    if (assessmentEntity.CommonInfoGroup.Score > 0)
                    {
                        ViewScoreEntity commonInfoGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        commonInfoGroup.GroupName = assessmentEntity.CommonInfoGroup.GroupName; 
                        commonInfoGroup.Score = assessmentEntity.CommonInfoGroup.Score;
                        if (assessmentEntity.CommonInfoGroup.CommonInformationEntity != null)
                        {
                            //处理逻辑
                        }
                        commonInfoGroup.ItemList = ItemList;
                        list.Add(commonInfoGroup);                        
                    }
                }
                if (assessmentEntity.BodyAndSpaceGroup != null)
                {
                    if (assessmentEntity.BodyAndSpaceGroup.Score > 0)
                    {
                        ViewScoreEntity bodyAndSpaceGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        bodyAndSpaceGroup.GroupName = assessmentEntity.BodyAndSpaceGroup.GroupName;
                        bodyAndSpaceGroup.Score = assessmentEntity.BodyAndSpaceGroup.Score;
                        if (assessmentEntity.BodyAndSpaceGroup.SpaceEntity != null)
                        {
                            new ParaKeyValue() { Name = "", Score = 0 };
                            ItemList.Add(new ParaKeyValue() { Name = "车内空间测量值", Score = assessmentEntity.BodyAndSpaceGroup.SpaceEntity.ActualScore });
                            ItemList.Add(new ParaKeyValue() { Name = "车内空间印象", Score = assessmentEntity.BodyAndSpaceGroup.SpaceEntity.ImpressionScore });
                        }
                        if (assessmentEntity.BodyAndSpaceGroup.TrunkEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "行李厢容积", Score = assessmentEntity.BodyAndSpaceGroup.TrunkEntity.Cubage });            
                        }
                        if (assessmentEntity.BodyAndSpaceGroup.StoragespaceEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "空间变化", Score = assessmentEntity.BodyAndSpaceGroup.StoragespaceEntity.Score });
                        }
                        if (assessmentEntity.BodyAndSpaceGroup.ConvenienceEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "设备操作便利性", Score = assessmentEntity.BodyAndSpaceGroup.ConvenienceEntity.Equipment });
                            ItemList.Add(new ParaKeyValue() { Name = "多媒体操作便利性", Score = assessmentEntity.BodyAndSpaceGroup.ConvenienceEntity.Multimedia });
                        }

                        if (assessmentEntity.BodyAndSpaceGroup.QualityEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "制造质量", Score = assessmentEntity.BodyAndSpaceGroup.QualityEntity.Score });
                        }                        
                        bodyAndSpaceGroup.ItemList = ItemList;
                        list.Add(bodyAndSpaceGroup);
                    }
                }
                if (assessmentEntity.RidingComfortGroup != null)
                {
                    if (assessmentEntity.RidingComfortGroup.Score > 0)
                    {
                        ViewScoreEntity ridingComfortGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        ridingComfortGroup.GroupName = assessmentEntity.RidingComfortGroup.GroupName;
                        ridingComfortGroup.Score = assessmentEntity.RidingComfortGroup.Score;

                        if (assessmentEntity.RidingComfortGroup.ChairEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "前排座椅", Score = assessmentEntity.RidingComfortGroup.ChairEntity.FrontChair });
                            ItemList.Add(new ParaKeyValue() { Name = "后排座椅", Score = assessmentEntity.RidingComfortGroup.ChairEntity.BehindChair });
                        }

                        if (assessmentEntity.RidingComfortGroup.AirConditionerEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "空调感受", Score = assessmentEntity.RidingComfortGroup.AirConditionerEntity.AirConditionerFeel });
                            ItemList.Add(new ParaKeyValue() { Name = "空调降温", Score = assessmentEntity.RidingComfortGroup.AirConditionerEntity.Refrigeration });
                        }

                        if (assessmentEntity.RidingComfortGroup.HangComfortEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "悬挂舒适性", Score = assessmentEntity.RidingComfortGroup.HangComfortEntity.SuspensionComfort });
                        }

                        if (assessmentEntity.RidingComfortGroup.NoiseEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "车内噪音测量值", Score = assessmentEntity.RidingComfortGroup.NoiseEntity.NoiseScore });
                        }

                        if (assessmentEntity.RidingComfortGroup.NoiseFeelEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "车内噪音印象", Score = assessmentEntity.RidingComfortGroup.NoiseFeelEntity.NoiseFeel });
                            ItemList.Add(new ParaKeyValue() { Name = "怠速噪音", Score = assessmentEntity.RidingComfortGroup.NoiseFeelEntity.IdlingNoise });
                            ItemList.Add(new ParaKeyValue() { Name = "行驶噪音", Score = assessmentEntity.RidingComfortGroup.NoiseFeelEntity.RunNoise });
                        }

                        ridingComfortGroup.ItemList = ItemList;
                        list.Add(ridingComfortGroup);
                    }
                }
                if (assessmentEntity.DynamicPerformanceGroup != null)
                {
                    if (assessmentEntity.DynamicPerformanceGroup.Score > 0)
                    {
                        ViewScoreEntity dynamicPerformanceGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        dynamicPerformanceGroup.GroupName = assessmentEntity.DynamicPerformanceGroup.GroupName;
                        dynamicPerformanceGroup.Score = assessmentEntity.DynamicPerformanceGroup.Score;
                        if (assessmentEntity.DynamicPerformanceGroup.AccelerateEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "加速", Score = assessmentEntity.DynamicPerformanceGroup.AccelerateEntity.AccelerateScroe });                            
                        }
                        if (assessmentEntity.DynamicPerformanceGroup.EngineEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "扭矩表现", Score = assessmentEntity.DynamicPerformanceGroup.EngineEntity.Torque });
                            ItemList.Add(new ParaKeyValue() { Name = "牵引力", Score = assessmentEntity.DynamicPerformanceGroup.EngineEntity.TractiveForce });
                            ItemList.Add(new ParaKeyValue() { Name = "功率表现", Score = assessmentEntity.DynamicPerformanceGroup.EngineEntity.Power });
                        }
                        if (assessmentEntity.DynamicPerformanceGroup.MotilityEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "运动性", Score = assessmentEntity.DynamicPerformanceGroup.MotilityEntity.Score });
                        }

                        if (assessmentEntity.DynamicPerformanceGroup.EngineSmoothnessEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "发动机平顺性", Score = assessmentEntity.DynamicPerformanceGroup.EngineSmoothnessEntity.Score });
                        }

                        if (assessmentEntity.DynamicPerformanceGroup.GearboxEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "换挡及变速箱", Score = assessmentEntity.DynamicPerformanceGroup.GearboxEntity.Score });
                        }
                        dynamicPerformanceGroup.ItemList = ItemList;
                        list.Add(dynamicPerformanceGroup);
                    }
                }
                if (assessmentEntity.SafetyGroup != null)
                {
                    if (assessmentEntity.SafetyGroup.Score > 0)
                    {
                        ViewScoreEntity safetyGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        safetyGroup.GroupName = assessmentEntity.SafetyGroup.GroupName;
                        safetyGroup.Score = assessmentEntity.SafetyGroup.Score;
                        if (assessmentEntity.SafetyGroup.BrakeEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "空载刹车测量值", Score = assessmentEntity.SafetyGroup.BrakeEntity.EmptyScore });
                            ItemList.Add(new ParaKeyValue() { Name = "满载刹车测量值", Score = assessmentEntity.SafetyGroup.BrakeEntity.FullScore });
                            ItemList.Add(new ParaKeyValue() { Name = "刹车踏板感受", Score = assessmentEntity.SafetyGroup.BrakeEntity.BrakeFeel });
                        }
                        if (assessmentEntity.SafetyGroup.ActiveSafetyEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "驾驶安全辅助（主动安全）", Score = assessmentEntity.SafetyGroup.ActiveSafetyEntity.DriveSafety });
                            ItemList.Add(new ParaKeyValue() { Name = "安全装备（被动安全）", Score = assessmentEntity.SafetyGroup.ActiveSafetyEntity.SafetyDevice });
                        }
                        if (assessmentEntity.SafetyGroup.VisualFieldEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "车内视野", Score = assessmentEntity.SafetyGroup.VisualFieldEntity.ViewScore });                            
                        }
                        if (assessmentEntity.SafetyGroup.LightEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "灯光", Score = assessmentEntity.SafetyGroup.LightEntity.LightScore });
                        }
                        safetyGroup.ItemList = ItemList;
                        list.Add(safetyGroup);
                    }
                }
                if (assessmentEntity.YhBaseGroup != null)
                {
                    if (assessmentEntity.YhBaseGroup.Score > 0)
                    {
                        ViewScoreEntity yhBaseGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        yhBaseGroup.GroupName = assessmentEntity.YhBaseGroup.GroupName;
                        yhBaseGroup.Score = assessmentEntity.YhBaseGroup.Score;

                        if (assessmentEntity.YhBaseGroup.YgEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "低速油耗", Score = assessmentEntity.YhBaseGroup.YgEntity.LowSpeedYhScore });
                            ItemList.Add(new ParaKeyValue() { Name = "高速油耗", Score = assessmentEntity.YhBaseGroup.YgEntity.HighSpeedYhScore });
                            ItemList.Add(new ParaKeyValue() { Name = "工信部油耗", Score = assessmentEntity.YhBaseGroup.YgEntity.MiitYhScore });
                        }

                        if (assessmentEntity.YhBaseGroup.YaEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "空气质量", Score = assessmentEntity.YhBaseGroup.YaEntity.AirQualityScore });                            
                        }

                        yhBaseGroup.ItemList = ItemList;
                        list.Add(yhBaseGroup);
                    }
                }
                if (assessmentEntity.JsBaseGroup != null)
                {
                    if (assessmentEntity.JsBaseGroup.Score > 0)
                    {
                        ViewScoreEntity jsBaseGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        jsBaseGroup.GroupName = assessmentEntity.JsBaseGroup.GroupName;
                        jsBaseGroup.Score = assessmentEntity.JsBaseGroup.Score;
                        if (assessmentEntity.JsBaseGroup.JdaEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "操控性", Score = assessmentEntity.JsBaseGroup.JdaEntity.ControllabilityScore });
                            ItemList.Add(new ParaKeyValue() { Name = "动态驾驶感受", Score = assessmentEntity.JsBaseGroup.JdaEntity.DriverSenseScore });
                            ItemList.Add(new ParaKeyValue() { Name = "抓地力", Score = assessmentEntity.JsBaseGroup.JdaEntity.GripScore });
                        }

                        if (assessmentEntity.JsBaseGroup.JtsEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "转弯直径", Score = assessmentEntity.JsBaseGroup.JtsEntity.TurnDiameterScore });
                            ItemList.Add(new ParaKeyValue() { Name = "方向盘", Score = assessmentEntity.JsBaseGroup.JtsEntity.SteeringWheelScore });
                            ItemList.Add(new ParaKeyValue() { Name = "直线行驶稳定性", Score = assessmentEntity.JsBaseGroup.JtsEntity.StraightPerferScore });
                        }
                        if (assessmentEntity.JsBaseGroup.JdfEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "驾驶乐趣", Score = assessmentEntity.JsBaseGroup.JdfEntity.DriveFunScore });                            
                        }
                        jsBaseGroup.ItemList = ItemList;
                        list.Add(jsBaseGroup);
                    }
                }
                if (assessmentEntity.CostBaseGroup != null)
                {
                    if (assessmentEntity.CostBaseGroup.Score > 0)
                    {
                        ViewScoreEntity costBaseGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        costBaseGroup.GroupName = assessmentEntity.CostBaseGroup.GroupName;
                        costBaseGroup.Score = assessmentEntity.CostBaseGroup.Score;
                        if (assessmentEntity.CostBaseGroup.CpEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "厂商指导价", Score = assessmentEntity.CostBaseGroup.CpEntity.ProducerPriceScore });
                        }
                        if (assessmentEntity.CostBaseGroup.CsyEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "燃油费用", Score = assessmentEntity.CostBaseGroup.CsyEntity.OilPayScore });
                            ItemList.Add(new ParaKeyValue() { Name = "保险费用", Score = assessmentEntity.CostBaseGroup.CsyEntity.InsuranceExpenseScore });
                            ItemList.Add(new ParaKeyValue() { Name = "保养费用", Score = assessmentEntity.CostBaseGroup.CsyEntity.MaintenanceCostScore });
                        }
                        if (assessmentEntity.CostBaseGroup.CgpEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "质保期", Score = assessmentEntity.CostBaseGroup.CgpEntity.GuaranPeriodScore });
                        }
                        if (assessmentEntity.CostBaseGroup.CgqEntity != null)
                        {
                            ItemList.Add(new ParaKeyValue() { Name = "保值", Score = assessmentEntity.CostBaseGroup.CgqEntity.GuaranQualityScore });
                        }
                        costBaseGroup.ItemList = ItemList;
                        list.Add(costBaseGroup);
                    }
                }
                if (assessmentEntity.GeneralBaseGroup != null)
                {
                    if (assessmentEntity.GeneralBaseGroup.Score > 0)
                    {
                        ViewScoreEntity generalBaseGroup = new ViewScoreEntity();
                        List<ParaKeyValue> ItemList = new List<ParaKeyValue>();
                        generalBaseGroup.GroupName = assessmentEntity.GeneralBaseGroup.GroupName;
                        generalBaseGroup.Score  = assessmentEntity.GeneralBaseGroup.Score;
                        //处理逻辑                     
                        generalBaseGroup.ItemList = ItemList;
                        list.Add(generalBaseGroup);
                    }
                }
            }
            var json = JsonConvert.SerializeObject(list);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, json) : json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class ViewScoreEntity
    {
        public string GroupName { get; set; }
        public double Score { get; set; }
        public List<ParaKeyValue> ItemList { get; set; }

    }
    public class ParaKeyValue
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}