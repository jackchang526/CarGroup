﻿using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model.AppApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.BLL
{
    public class Car_MasterBrandBll
    {
        private MasterBrandDal _masterBrandDal;
        public Car_MasterBrandBll()
        {
            _masterBrandDal = new MasterBrandDal();
        }
        /// <summary>
        /// 获取热门主品牌
        /// </summary>
        /// <param name="top">获取前几个主品牌</param>
        /// <returns></returns>
        public DataSet GetHotMasterBrand(int top)
        {
            string catchkey = string.Format("Car_MasterBrandBll_GetHotMasterBrand_{0}", top);
            object obj = CacheManager.GetCachedData(catchkey);
            if (obj != null)
            {
                return obj as DataSet;
            }
            DataSet ds = _masterBrandDal.GetHotMasterBrand(top);
            CacheManager.InsertCache(catchkey, ds, 60 * 24);
            return ds;
        }
        /// <summary>
        /// 获取主品牌故事
        /// </summary>
        /// <param name="masterid">主品牌ID</param>
        /// <returns></returns>
        public DataSet GetMasterBrandStory(int masterid)
        {
            string catchkey = string.Format("Car_MasterBrandBll_GetMasterBrandStory_{0}", masterid);
            object obj = CacheManager.GetCachedData(catchkey);
            if (obj != null)
            {
                return obj as DataSet;
            }
            DataSet ds = _masterBrandDal.GetMasterBrandStory(masterid);
            CacheManager.InsertCache(catchkey, ds, 60 * 24);
            return ds;
        }
        /// <summary>
        /// 根据车系编号和颜色类型获取车系颜色 
        /// </summary>
        /// <param name="modelId">车系编号</param>
        /// <param name="type">颜色类型</param>
        /// <returns></returns>
        public List<CarModelColor> GetCarModelColorByModelId(int modelId, int type)
        {
            string catchkey = string.Format("Car_MasterBrandBll_GetCarModelColorByModelId_{0}_{1}", modelId, type);
            object obj = CacheManager.GetCachedData(catchkey);
            if (obj != null)
            {
                return obj as List<CarModelColor>;
            }
            var ds = _masterBrandDal.GetCarModelColorByModelId(modelId, type);
            CacheManager.InsertCache(catchkey, ds, 60 * 24);
            return ds;
        }
    }
}
