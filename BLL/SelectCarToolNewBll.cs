using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.BLL
{
    /// <summary>
    /// 选车工具相关
    /// </summary>
    public class SelectCarToolNewBll
    {
        /// <summary>
        /// 请求选车工具接口
        /// </summary>
        /// <param name="param">参数字段，如果一个参数有多个值，需要拼好</param>
        /// <returns></returns>
        public SelectCarResult GetSelectCarResult(Dictionary<string,string> param)
        {
            string paramStr = string.Empty;
            SelectCarResult selectCarResult = null;
            string cachekey = "Car_SelectCarToolNewBLL";
            if (param != null && param.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in param)
                {
                    cachekey = string.Format("{0}_{1}{2}", cachekey, kv.Key, kv.Value);
                }
            }
            object cacheObj = CacheManager.GetCachedData(cachekey);
            if (cacheObj != null)
            {
                selectCarResult = (SelectCarResult)cacheObj;
            }
            if (selectCarResult == null)
            {
                if (param != null && param.Count > 0)
                {
                    foreach (KeyValuePair<string, string> kv in param)
                    {
                        paramStr = string.Format("{2}&{0}={1}", kv.Key, kv.Value, paramStr);
                    }
                }
                string url = WebConfig.SelectCarUrl;
                if (string.IsNullOrEmpty(url))
                {
                    return selectCarResult;
                }
                url = string.Concat(url, paramStr);
                string content = CommonFunction.GetContentByUrl(url);
                if (!string.IsNullOrEmpty(content))
                {
                    selectCarResult = JsonHelper.Deserialize<SelectCarResult>(content);
                }
                CacheManager.InsertCache(cachekey, selectCarResult, 2 * 60);
            }
            return selectCarResult;
        }

        /// <summary>
        /// 请求选车工具接口,并附加电动车扩展信息
        /// </summary>
        /// <param name="param">参数字段，如果一个参数有多个值，需要拼好</param>
        /// <returns></returns>
        public SelectCarResult GetSelectCarResultWithElecInfo(Dictionary<string, string> param)
        {
            SelectCarResult selectCarResult = GetSelectCarResult(param);
            if (selectCarResult != null && selectCarResult.ResList.Count > 0)
            {
                Dictionary<int, Car_SerialItemEntity> extendDic = new Car_SerialItemBll().GetSerialItemAll();
                // 非白底
                Dictionary<int, string> dicPicNoneWhite = new PageBase().GetAllSerialPicURLNoneWhiteBackground();               

                for (int i = 0; i < selectCarResult.ResList.Count; i++)
                {
                    SelectCarDetailInfo item = selectCarResult.ResList[i];
                    if (extendDic != null && extendDic.Count > 0 && extendDic.ContainsKey(item.SerialId))
                    {
                        Car_SerialItemEntity extendItem = extendDic[item.SerialId];
                        item.NormalChargeTime = extendItem.NormalChargeTime;
                        item.BatteryLife = extendItem.BatteryLife;
                    }
                    if (dicPicNoneWhite.Count > 0 && dicPicNoneWhite.ContainsKey(item.SerialId))
                    {
                        item.NoneWhiteImageUrl = dicPicNoneWhite[item.SerialId];
                    }
                    else
                    {
                        item.NoneWhiteImageUrl = WebConfig.DefaultCarPic;
                    }
                }
            }
            return selectCarResult;
        }
    }
}
