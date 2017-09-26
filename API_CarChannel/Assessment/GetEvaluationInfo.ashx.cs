using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// 评测参数数据接口
    /// </summary>
    public class GetEvaluationInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "application/x-javascript";
            EvaluationBll evaluationBll = new EvaluationBll();
            int evaluationId = 0;
            string id = context.Request.QueryString["evaluationId"];
            int.TryParse(id, out evaluationId);
            string callback = context.Request.QueryString["callback"];
            List<EvaluationEntity> list = evaluationBll.GetByStyleEvaluation(evaluationId);
            TestBaseEntity testBaseEntity=evaluationBll.GetTestBaseEntityById(evaluationId);
            CarEntity cbe = (CarEntity)DataManager.GetDataEntity(EntityType.Car, testBaseEntity.StyleId);
            if (cbe != null)
            {
                testBaseEntity.ShowName = CommonFunction.GetUnicodeByString(cbe.Serial.ShowName + cbe.CarYear + "款" + cbe.Name);
            }
            else
            {
                testBaseEntity.ShowName = "";
            }
            var groupList = list.GroupBy(m => m.GroupId);
            Dictionary<string, string> groupdic = new Dictionary<string, string>();
            Dictionary<string, List<object>> dataDic = new Dictionary<string, List<object>>();
            foreach (var item in groupList)
            {
                groupdic.Add("s"+item.Key.ToString(), list.Where(i => i.GroupId == item.Key).Select(j => j.GroupName).First());
                dataDic.Add("s" + item.Key.ToString(), (from i in list where i.GroupId == item.Key select new { ParaName = i.Name, PropertyId = i.PropertyId, PropertyValue = i.PropertyValue, Unit = i.Unit }).ToList<object>());
            }
            var data = new
            {
                Group = groupdic,
                TestBaseEntity = testBaseEntity,
                List = dataDic
            };
            var json = JsonConvert.SerializeObject(data);
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
}