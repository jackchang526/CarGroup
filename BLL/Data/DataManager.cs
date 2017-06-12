using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.BLL.Data
{
    public class DataManager
    {
        /// <summary>
        /// 获取实体数据,根据提供的实体类型获取数据，如果命中缓存，则从缓存中取，否则初始化一个数据实体。
        /// </summary>
        /// <param name="eType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BaseEntity GetDataEntity(EntityType eType, int id)
        {
            string cacheKey = eType.ToString() + "_dataEntity_" + id;
            BaseEntity se = (BaseEntity)CacheManager.GetCachedData(cacheKey);
            if (se == null)
            {
                // Mutex m = new Mutex(false,cacheKey);
                // m.WaitOne();
                se = (BaseEntity)CacheManager.GetCachedData(cacheKey);
                if (se == null)
                {
                    switch (eType)
                    {
                        case EntityType.Car:
                            se = new CarEntity();
                            break;
                        case EntityType.Serial:
                            se = new SerialEntity();
                            break;
                        case EntityType.Brand:
                            se = new BrandEntity();
                            break;
                        case EntityType.MasterBrand:
                            se = new MasterBrandEntity();
                            break;
                        case EntityType.Producer:
                            se = new ProducerEntity();
                            break;
                        case EntityType.Level:
                            se = new LevelEntity();
                            break;
                    }
                    se.InitData(id);
                    if (se.Id == 0)
                        se = null;
                    else
                        CacheManager.InsertCache(cacheKey, se, WebConfig.CachedDuration);
                }
                // m.ReleaseMutex();
                // m.Close();
            }

            return se;
        }

        /// <summary>
        /// 将一个数据对象加入缓存
        /// </summary>
        /// <param name="eType"></param>
        /// <param name="se"></param>
        public static void AddDataEntity(EntityType eType, BaseEntity se)
        {
            string cacheKey = eType.ToString() + "_dataEntity_" + se.Id;
            CacheManager.InsertCache(cacheKey, se, WebConfig.CachedDuration);
        }
    }
}
