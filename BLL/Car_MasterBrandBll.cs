using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.DAL;
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
	}
}
