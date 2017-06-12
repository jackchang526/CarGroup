using System.Collections.Generic;
using System.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL
{
    public class CommonHtmlBll
    {
        private static readonly CommonHtmlDal CommonhtmlDal = new CommonHtmlDal();

        /// <summary>
        ///     获取生成块内容
        /// </summary>
        /// <param name="id">主品牌、品牌、子品牌、车型 ID</param>
        /// <param name="type">Id参数的类型</param>
        /// <param name="tag">页面标示</param>
        /// <returns></returns>
        public Dictionary<int, string> GetCommonHtml(int id, CommonHtmlEnum.TypeEnum type, CommonHtmlEnum.TagIdEnum tag)
        {
            var typeId = (int) type;
            var tagId = (int) tag;

            //string cacheKey = string.Format("Car_CommonHtml_{0}_{1}_{2}", Id, TypeId, TagId);
            //object allTagContent = CacheManager.GetCachedData(cacheKey);
            //if (allTagContent != null)
            //    return (Dictionary<int, string>)allTagContent;

            var dict = new Dictionary<int, string>();

            DataSet ds = CommonhtmlDal.GetCommonHtml(id, typeId, tagId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int blockId = ConvertHelper.GetInteger(dr["blockid"]);
                    dict.Add(blockId, dr["htmlcontent"].ToString());
                }
                //CacheManager.InsertCache(cacheKey, dict, WebConfig.CachedDuration);
            }
            return dict;
        }

        /// <summary>
        ///     获取单个块内容 唯一
        /// </summary>
        /// <param name="id">主品牌、品牌、子品牌、车型 ID</param>
        /// <param name="tag">页面标示</param>
        /// <param name="block">块标示</param>
        /// <param name="type">参数的类型</param>
        /// <returns></returns>
        public string GetCommonHtmlByBlockId(int id, CommonHtmlEnum.TypeEnum type, CommonHtmlEnum.TagIdEnum tag,
                                             CommonHtmlEnum.BlockIdEnum block)
        {
            string result = string.Empty;
            var typeId = (int) type;
            var tagId = (int) tag;
            var blockId = (int) block;
            DataSet ds = CommonhtmlDal.GetCommonHtml(id, typeId, tagId, blockId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                result = ds.Tables[0].Rows[0]["htmlcontent"].ToString();
            }
            return result;
        }
    }
}