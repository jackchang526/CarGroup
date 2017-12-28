using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.BLL
{
    public class NewCarIntoMarketBll
    {
        private readonly NewCarIntoMarketDal newCarIntoMarketDal = new NewCarIntoMarketDal();
        private readonly Car_SerialBll car_SerialBll = new Car_SerialBll();
        /// <summary>
        /// 上市新车数据分别根据根据上市时间倒序,车系Id正序和上市时间,车系Id正序排列
        /// </summary>
        /// <param name="isMarketed"></param>
        /// <returns></returns>
        public IList<Model.NewCarIntoMarketVMEntity> GetNewCarIntoMarketList(bool isMarketed)
        {
            string cacheKey = "GetNewCarIntoMarketList_" + isMarketed;
            object cacheValue = CacheManager.GetCachedData(cacheKey);
            if (cacheValue != null)
            {
                return (IList<Model.NewCarIntoMarketVMEntity>)cacheValue;
            }
            else
            {
                var list = newCarIntoMarketDal.GetNewCarIntoMarketList(isMarketed);
                var vmList = new List<NewCarIntoMarketVMEntity>();
                var levelDic = car_SerialBll.GetAllSerialLevelsWithSecondLevel();
                foreach (var item in list)
                {
                    var entity = new NewCarIntoMarketVMEntity();
                    entity.CsId = item.CsId;
                    entity.Type = item.Type;
                    entity.YearType = item.YearType;
                    if (item.MarketDay == DateTime.MinValue)
                    {
                        entity.MarketDay = "";
                    }
                    else
                    {
                        entity.MarketDay = item.MarketDay.ToString("yyyy-MM-dd");
                    }
                    if (levelDic.ContainsKey(item.CsId))
                    {
                        entity.Level = levelDic[item.CsId];
                    }
                    else
                    {
                        entity.Level = "";
                    } 
                    var serialInfo = car_SerialBll.GetSerialInfoEntity(item.CsId);
                    
                    entity.AllSpell = serialInfo.Cs_AllSpell;
                    entity.CsName = serialInfo.Cs_ShowName;
                    entity.RefPrice = car_SerialBll.GetSerialOfficePriceBySaleState(item.CsId, true);
                    entity.CoverImage = Car_SerialBll.GetSerialImageUrl(item.CsId);
                    vmList.Add(entity);
                }
                CacheManager.InsertCache(cacheKey, vmList, WebConfig.CachedDuration);
                return vmList;
            }
        }
    }
}
