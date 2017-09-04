
using System.Collections.Generic;

namespace BitAuto.CarChannel.Model.Assessment
{
    public class JsBaseGroup
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// 排序编号
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 总分
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 编辑点评
        /// </summary>
        public EditorCommentEntity EcEntity { get; set; }
        /// <summary>
        /// 驾驭能力
        /// </summary>
        public JsDriverAbilityEntity JdaEntity { get; set; }
        /// <summary>
        /// 转向系统
        /// </summary>
        public JsTurnSystemEntity JtsEntity { get; set; }
        /// <summary>
        /// 驾驶乐趣
        /// </summary>
        public JsDriverFunEntity JdfEntity { get; set; }
        /// <summary>
        /// 知识点
        /// </summary>
        public IList<WikiEntity> WikiList { get; set; }
    }
    public class JsDriverAbilityEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 操控性评分
        /// </summary>
        public int ControllabilityScore { get; set; }
        /// <summary>
        /// 动态驾驶感受评分
        /// </summary>
        public int DriverSenseScore { get; set; }
        /// <summary>
        /// 抓地力评分
        /// </summary>
        public int GripScore { get; set; }
        /// <summary>
        /// 驾驭能力总评
        /// </summary>
        public string ControllabilityCmt { get; set; }
        /// <summary>
        /// 110m变线成绩(时速km/h)
        /// </summary>
        public string ChangeLine110mScore { get; set; }
        /// <summary>
        /// 110m变线时间(单位s)
        /// </summary>
        public int ChangeLine110mTime { get; set; }
        /// <summary>
        /// 110m变线视频集合
        /// </summary>
        public List<string> ChangeLine110mVideos { get; set; }
        /// <summary>
        /// 110m变线图片集合
        /// </summary>
        public List<CommonImageVideoEntity> ChangeLine110mPics { get; set; }
        /// <summary>
        /// 18m绕桩成绩（单位km/h）
        /// </summary>
        public string AroundPile18mScore { get; set; }
        /// <summary>
        /// 18m绕桩时间（单位s）
        /// </summary>
        public int AroundPile18mTime { get; set; }
        /// <summary>
        /// 18m绕桩上传视频集合
        /// </summary>
        public List<string> AroundPile18mVideos { get; set; }
        /// <summary>
        /// 18m绕桩上传图片集合
        /// </summary>
        public List<CommonImageVideoEntity> AroundPile18mPics { get; set; }
        /// <summary>
        /// 18m绕桩评论区
        /// </summary>
        public string AroundPile18mCmt { get; set; }
        /// <summary>
        /// ams操控圈圈速
        /// </summary>
        public string AmsLapScore { get; set; }
        /// <summary>
        /// ams操控圈时间
        /// </summary>
        public int AmsLapTime { get; set; }
        /// <summary>
        ///  ams操控圈上传视频集合
        /// </summary>
        public List<string> AmsLapVideos { get; set; }
        /// <summary>
        ///  ams操控圈上传图片集合
        /// </summary>
        public List<CommonImageVideoEntity> AmsLapPics { get; set; }
        /// <summary>
        /// ams操控圈评论区
        /// </summary>
        public string AmsLapCmt { get; set; }
    }
    public class JsDriverFunEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 驾驶乐趣评分
        /// </summary>
        public int DriveFunScore { get; set; }
        /// <summary>
        /// 空驾驶乐趣总评
        /// </summary>
        public string DriveFunGeneralCmt { get; set; }
        /// <summary>
        /// 驾驶图片集合
        /// </summary>
        public List<string> DriveFunPics { get; set; }
        /// <summary>
        /// 驾驶图片集合和描述
        /// </summary>
        public List<CommonImageVideoEntity> DriverPics { get; set; }
        /// <summary>
        /// 驾驶图片下方备注区
        /// </summary>
        public string DriveFunRemarks { get; set; }
    }
    public class JsTurnSystemEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 转弯直径评分
        /// </summary>
        public int TurnDiameterScore { get; set; }
        /// <summary>
        /// 方向盘评分
        /// </summary>
        public int SteeringWheelScore { get; set; }
        /// <summary>
        /// 直线行驶稳定性评分
        /// </summary>
        public int StraightPerferScore { get; set; }
        /// <summary>
        /// 转身系统总评
        /// </summary>
        public string TurnSysGeneralCmt { get; set; }
        /// <summary>
        /// 转变直径（单位m)
        /// </summary>
        public int TurnDiameterLength { get; set; }
        /// <summary>
        /// 转弯时间s
        /// </summary>
        public int TurnTime { get; set; }
        /// <summary>
        /// 转变图片集合
        /// </summary>
        public List<CommonImageVideoEntity> TurnPics { get; set; }
        /// <summary>
        /// 转弯评论区
        /// </summary>
        public string TurnWheelCmt { get; set; }
    }
}