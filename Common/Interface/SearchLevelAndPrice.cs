using System;
using System.Collections.Generic;
using System.Text;

using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannel.Common.Interface
{
    public class SearchLevelAndPrice
    {
        public string _Index = string.Empty;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index"></param>
        public SearchLevelAndPrice(string index)
        {
            _Index = index;
        }
        /// <summary>
        /// 搜索报价
        /// </summary>
        /// <param name="ssfInterface"></param>
        /// <returns></returns>
        public bool SearchPrice(EnumCollection.SerialSortForInterface ssfInterface)
        {
            if (ssfInterface.CsPriceRange.IndexOf(_Index) >= 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 搜索级别
        /// </summary>
        /// <param name="ssfInterface"></param>
        /// <returns></returns>
        public bool SearchLevel(EnumCollection.SerialSortForInterface ssfInterface)
        {
            if (ssfInterface.CsLevel == _Index)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 得到报价排序
        /// </summary>
        /// <param name="ssfi1"></param>
        /// <param name="ssfi2"></param>
        /// <returns></returns>
        public static int GetPricePvSort(EnumCollection.SerialSortForInterface ssfi1, EnumCollection.SerialSortForInterface ssfi2)
        {
            if (ssfi1.CsPV == ssfi2.CsPV)
            {
                return 0;
            }
            if (ssfi1.CsPV < ssfi2.CsPV)
            {
                return 1;
            }
            return -1;
        }
        /// <summary>
        /// 得到报价排序
        /// </summary>
        /// <param name="ssfi1"></param>
        /// <param name="ssfi2"></param>
        /// <returns></returns>
        public static int GetLevelPvSort(EnumCollection.SerialSortForInterface ssfi1, EnumCollection.SerialSortForInterface ssfi2)
        {
            if (ssfi1.CsPV == ssfi2.CsPV)
            {
                return 0;
            }
            if (ssfi1.CsPV < ssfi2.CsPV) 
            {
                return 1;
            }
            return -1;
        }
        
    }
}
