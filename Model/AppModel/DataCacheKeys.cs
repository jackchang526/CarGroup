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
        public static string CarParameterListKey = "car.appapi.carparameterlist_{0}_{1}";
        /// <summary>
        /// 参配Memcached
        /// carids,逗号分开
        /// </summary>
        public static string CarCompareDataWithOptional = "Car_Dictionary_CarCompareDataWithOptional_{0}";

        /// <summary>
        /// 参配分组模版
        /// </summary>
        public static string CarParameterJson = "car.appapi.carparameterjson_{0}";
        /// <summary>
        /// 车款分组列表
        /// 0-车型id
        /// 1-城市id
        /// 2-是否保护停销
        /// </summary>
        public static string CarGroupListByserialIdAndCityId= "car.appapi.cargrouplist_{0}_{1}_{2}";

        /// <summary>
        /// 车型的国别和主品牌
        /// </summary>
        public static string SerialCountry = "car.appapi.serialcountry_{0}";

        /// <summary>
        /// 车型名片
        /// 地方报价
        /// </summary>
        public static string SerialInfo= "car.appapi.serialinfo_{0}_{1}";

        /// <summary>
        ///浏览 车型推荐
        /// </summary>
        public static string SerialInfoForUser = "car.appapi.serialinfoforuser_{0}";
      
    }
}
