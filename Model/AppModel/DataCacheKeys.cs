using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 缓存键值
    /// </summary>
    public class DataCacheKeys
    {
        /// <summary>
        /// 车型选配包
        /// 0-车型ID
        /// </summary>
        public static string CarSerialPackageKey = "car.appapi.carserialpackageentity_{0}";
      
        /// <summary>
        /// 参配本地缓存
        /// carids,逗号分开
        /// </summary>
        public static string CarParameterListKey = "car.appapi.carparameterlist_{0}";
        /// <summary>
        /// 参配Memcached
        /// carids,逗号分开
        /// </summary>
        public static string CarCompareDataWithOptional = "Car_Dictionary_CarCompareDataWithOptional_{0}";

        /// <summary>
        /// 参配分组模版
        /// </summary>
        public static string CarParameterJson = "car.appapi.carparameterjson";
    }
}
