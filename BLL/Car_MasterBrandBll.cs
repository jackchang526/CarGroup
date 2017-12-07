using BitAuto.CarChannel.Common.Cache;
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
        public List<CarModelColorEntity> GetCarModelColorByModelId(int modelId, int type)
        {
            string catchkey = string.Format("Car_MasterBrandBll_GetCarModelColorByModelId_{0}_{1}", modelId, type);
            object obj = CacheManager.GetCachedData(catchkey);
            if (obj != null)
            {
                return obj as List<CarModelColorEntity>;
            }
            var ds = _masterBrandDal.GetCarModelColorByModelId(modelId, type);
            CacheManager.InsertCache(catchkey, ds, 60 * 24);
            return ds;
        }

        /// <summary>
        /// 获取主品牌列表
        /// </summary>
        /// <param name="allMasterBrand">是否是所有主品牌(旗下停销车型)</param>
        /// <returns></returns>
        public List<CarMasterBrandEntity> GetCarMasterBrandList(bool allMasterBrand)
        {
            string cacheKey = "ycapp.carmasterbrandlist";
            var getCarMasterBrandList = (List<CarMasterBrandEntity>)CacheManager.GetCachedData(cacheKey);
            if (getCarMasterBrandList == null)
            {
                getCarMasterBrandList = _masterBrandDal.GetCarMasterBrandList();
                if (getCarMasterBrandList != null && getCarMasterBrandList.Count > 0)
                {
                    getCarMasterBrandList.ForEach(x =>
                    {
                        x.SaleStatus = GetMasterBrandSaleStateById(x.MasterID);
                    });
                }
                CacheManager.InsertCache(cacheKey, getCarMasterBrandList, 60 * 24);
            }
            if (!allMasterBrand)
            {
                getCarMasterBrandList.RemoveAll(l => l.SaleStatus < 0);
            }
            return getCarMasterBrandList;
        }

        /// <summary>
        /// 根据车款编号和颜色类型获取车款颜色 
        /// </summary>
        /// <param name="styleId">车款编号</param>
        /// <param name="type">颜色类型 0车身颜色 1内饰颜色</param>
        /// <returns></returns>
        public List<CarModelColorEntity> GetCarStyleColorById(int styleId, int type)
        {
            return _masterBrandDal.GetCarStyleColorById(styleId, type);
        }

        /// <summary>
        /// 根据主品牌ID获取销售状态
        /// </summary>
        /// <param name="masterid"></param>
        /// <returns></returns>
        public int GetMasterBrandSaleStateById(int masterid)
        {
            int salestate = 2;
            string cacheKey = "ycapp.gemasterbrandsalestatebyId";
            var getCarMasterBrandList = (List<MasterBrandSaleState>)CacheManager.GetCachedData(cacheKey);
            if (getCarMasterBrandList == null)
            {
                getCarMasterBrandList = _masterBrandDal.GetAllMasterBrandSaleState();
                CacheManager.InsertCache(cacheKey, getCarMasterBrandList, 60 * 24);
            }
            if (getCarMasterBrandList.Any())
            {
                if (getCarMasterBrandList.Exists(x => x.bs_Id == masterid && x.CbSaleState == "在销"))
                {
                    salestate = 1;
                }
                else if (getCarMasterBrandList.Exists(x => x.bs_Id == masterid && x.CbSaleState == "待销"))
                {
                    salestate = 0;
                }
                else if (getCarMasterBrandList.Exists(x => x.bs_Id == masterid && x.CbSaleState == "停销"))
                {
                    salestate = -1;
                }
                else if (getCarMasterBrandList.Exists(x => x.bs_Id == masterid && x.CbSaleState == "待查"))
                {
                    salestate = 2;
                }
            }
            return salestate;
        }

    }
}
