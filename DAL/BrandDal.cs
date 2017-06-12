using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.DAL
{
    public class BrandDal
    {
        /// <summary>
        /// 获取所有品牌
        /// </summary>
        /// <returns></returns>
        public static DataSet GetBrands()
        {
            string catchkey = "GetAllBrands";
            
            if (CacheManager.GetCachedData(catchkey) == null)
            {
                string sqlStr = "SELECT cb.cb_Id,cb.cp_Id,cmr.bs_Id,cb.cb_Name,cb.allSpell,cb.cb_seoname  FROM Car_Brand cb "
                    + "LEFT JOIN Car_MasterBrand_Rel cmr ON cb.cb_Id=cmr.cb_Id WHERE cb.IsState=1";

                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                CacheManager.InsertCache(catchkey, ds, 60);
            }

            return (CacheManager.GetCachedData(catchkey) as DataSet);
        }
    }
}
