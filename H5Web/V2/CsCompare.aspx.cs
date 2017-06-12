using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace H5Web.V2
{
    public partial class CsCompare : H5PageBase
    {
        //第四级
        private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();
        //子品牌下车型id列表
        private List<int> listCarID = new List<int>();
        private Car_BasicBll carBasicBll = new Car_BasicBll();

        #region 页面变量
        //子品牌id
        protected int serialId = 0;
        //子品牌实体
        protected SerialEntity BaseSerialEntity;
        //车型json数据
        protected StringBuilder sbForApi = new StringBuilder(" var carCompareJson = ");
        //初始需要比较的2款热门车
        protected StringBuilder sbForHotCompare = new StringBuilder(" var hotCompareCarIdArr = ");
        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetPageCache(30);

            GetParamter();
            if (serialId < 1)
            {
                Response.Redirect("http://car.h5.yiche.com/");
            }
            InitData();
            //List<int> listAllCsID = serialFourthStageBll.GetAllSerialInH5();
            //if (listAllCsID == null || BaseSerialEntity == null)
            //{
            //    Response.Redirect("http://car.h5.yiche.com/");
            //}
            //if (!listAllCsID.Contains(serialId) && serialId > 0)
            //{
            //    Response.Redirect("http://car.m.yiche.com/" + BaseSerialEntity.AllSpell);
            //}

            GetCarIDByCsID();

            sbForApi.Append("[");
            GetCarParamData();
            sbForApi.Append("];");
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        private void GetParamter()
        {
            serialId = ConvertHelper.GetInteger(Request.QueryString["csID"]);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
        }

        // 取子品牌下车型ID
        private void GetCarIDByCsID()
        {
            if (BaseSerialEntity != null)
            {
                DataSet ds = new DataSet();
                if (BaseSerialEntity.SaleState == "停销")
                {
                    // 停销子品牌取最新年款的车型
                    ds = base.GetCarIDAndNameForNoSaleCS(serialId, WebConfig.CachedDuration);
                }
                else
                {
                    ds = base.GetCarIDAndNameForCS(serialId, WebConfig.CachedDuration);
                }
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Dictionary<int, string> dic382 = carBasicBll.GetCarParamExDic(382);
                    var dic = from i in dic382 select new { key = i.Key, value = i.Value };
                    var enumTable = from a in ds.Tables[0].AsEnumerable()
                                    join b in dic382.ToList().AsEnumerable() on a.Field<int>("car_id") equals b.Key into all
                                    from ab in all.DefaultIfEmpty()
                                    orderby ab.Value == "平行进口" ? 1 : 0
                                    select a;

                    DataTable dt = enumTable.CopyToDataTable<DataRow>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listCarID.Add(int.Parse(dt.Rows[i]["car_id"].ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// 取车型详细参数
        /// </summary>
        private void GetCarParamData()
        {
            if (listCarID.Count > 0)
            {
                Dictionary<int, Dictionary<string, string>> dicCarParam = carBasicBll.GetCarCompareDataByCarIDs(listCarID);

                #region 生成车型详细参数js数组
                // 生成车型详细参数js数组
                if (dicCarParam.Count > 0)
                {
                    Dictionary<int, List<string>> dicTemp = base.GetCarParameterJsonConfigNew();
                    if (dicTemp != null && dicTemp.Count > 0)
                    {
                        GetCarByHot(dicCarParam);
                        int loopCar = 0;
                        foreach (KeyValuePair<int, Dictionary<string, string>> kvpCar in dicCarParam)
                        {
                            if (loopCar > 0)
                            { sbForApi.Append(","); }

                            sbForApi.Append("[");
                            // 循环模板
                            foreach (KeyValuePair<int, List<string>> kvpTemp in dicTemp)
                            {
                                if (kvpTemp.Key == 0)
                                {
                                    // 基本数据
                                    sbForApi.Append("[\"" + kvpCar.Value["Car_ID"] + "\"");
                                    sbForApi.Append(",\"" + kvpCar.Value["Car_Name"].Replace("\"", "'") + "\"");
                                    foreach (string param in kvpTemp.Value)
                                    {
                                        if (kvpCar.Value.ContainsKey(param))
                                        { sbForApi.Append(",\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                        else
                                        { sbForApi.Append(",\"\""); }
                                    }
                                    sbForApi.Append("]");
                                }
                                else
                                {
                                    // 扩展数据
                                    sbForApi.Append(",[");
                                    int loop = 0;
                                    foreach (string param in kvpTemp.Value)
                                    {
                                        if (loop > 0)
                                        { sbForApi.Append(","); }
                                        if (kvpCar.Value.ContainsKey(param))
                                        { sbForApi.Append("\"" + kvpCar.Value[param].Replace("\"", "'") + "\""); }
                                        else
                                        { sbForApi.Append("\"\""); }
                                        loop++;
                                    }
                                    sbForApi.Append("]");
                                }
                            }
                            sbForApi.Append("]");
                            loopCar++;
                        }
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 按热度获取2款需要比较的车款
        /// </summary>
        /// <param name="dicCarParam"></param>
        /// <returns></returns>
        private void GetCarByHot(Dictionary<int, Dictionary<string, string>> dicCarParam)
        {
            List<EnumCollection.CarInfoForSerialSummary> listHotCar = base.GetAllCarInfoForSerialSummaryByCsID(serialId,true);
            //Dictionary<int, Dictionary<string, string>> tempDicCarParam = new Dictionary<int, Dictionary<string, string>>();
            List<int> hotCarIdList = new List<int>();
            if (listHotCar != null && listHotCar.Count > 0)
            {
                listHotCar = listHotCar.OrderByDescending(x => x.CarPV).ToList<EnumCollection.CarInfoForSerialSummary>();
                foreach (EnumCollection.CarInfoForSerialSummary cifs in listHotCar)
                {
                    if (dicCarParam.ContainsKey(cifs.CarID) && !hotCarIdList.Contains(cifs.CarID))
                    {
                        hotCarIdList.Add(cifs.CarID);
                        if(hotCarIdList.Count == 2)
                        {
                            break;
                        }
                    }
                }
            }

            sbForHotCompare.Append("[").Append(string.Join(",", hotCarIdList.ToArray())).Append("]");
        }
    }
}