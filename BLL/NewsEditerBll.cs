using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL
{
    public class NewsEditerBll
    {
        /// <summary>
        /// 得到新闻编辑列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Editer> GetNewsEditerList()
        {
            string cacheKey = "newsediterbll_getnewsediterlist";
            object obj = CacheManager.GetCachedData(cacheKey);
            if (obj != null) return (Dictionary<int, Editer>)obj;

            Dictionary<int, Editer> editerlist = new NewsEditerDal().GetEditerList();
            if (editerlist == null) return null;

            CacheManager.InsertCache(cacheKey, editerlist, 60);
            return editerlist;
        }
    }
}
