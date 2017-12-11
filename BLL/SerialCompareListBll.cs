using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL
{
    public class SerialCompareListBll
    {
        private readonly SerialCompareListDal compareDal = new SerialCompareListDal();
        public List<SerialCompareListEntity> GetHotSerialCompareList(int top)
        {
            List<SerialCompareListEntity> list = new List<SerialCompareListEntity>();
            string cacheKey = "Car_GetHotSerialCompareList_" + top;

            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null)
                return (List<SerialCompareListEntity>)obj;

            DataSet ds = compareDal.GetHotSerialCompareList(top);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var pagebase = new PageBase();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var serialId = ConvertHelper.GetInteger(dr["csid"]);
                    var toSerialId = ConvertHelper.GetInteger(dr["tocsid"]);
                    list.Add(new SerialCompareListEntity()
                    {
                        SerialId = serialId,
                        SerialShowName = ConvertHelper.GetString(dr["csShowName"]).Trim(),
                        SerialAllSpell = ConvertHelper.GetString(dr["allspell"]),
                        SerialImageUrl = Car_SerialBll.GetSerialImageUrl(serialId),
                        //改为指导价
                        SerialPriceRange = pagebase.GetSerialReferPriceByID(serialId),
                        ToSerialId = toSerialId,
                        ToSerialShowName = ConvertHelper.GetString(dr["tocsShowName"]).Trim(),
                        ToSerialAllSpell = ConvertHelper.GetString(dr["toallspell"]),
                        ToSerialImageUrl = Car_SerialBll.GetSerialImageUrl(toSerialId),
                        ToSerialPriceRange = pagebase.GetSerialReferPriceByID(toSerialId),
                        CompareCount = ConvertHelper.GetInteger(dr["CompareCount"])
                    });
                }
                CacheManager.InsertCache(cacheKey, list, 60 * 24);
            }
            return list;
        }
    }
}
