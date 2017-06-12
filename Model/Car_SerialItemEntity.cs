using System;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 子品牌扩展信息
    /// </summary>
    [Serializable]
    public class Car_SerialItemEntity
    {
        #region 变量定义
        ///<summary>
        /// 子品牌ID
        ///</summary>
        private int _cs_Id;
        ///<summary>
        /// 子品牌车门
        ///</summary>
        private string _body_Doors = String.Empty;
        ///<summary>
        /// 子品牌排量
        ///</summary>
        private string _engine_Exhaust = String.Empty;
        ///<summary>
        /// 子品牌挡位&变速器类型
        ///</summary>
        private string _underPan_Num_Type = String.Empty;
        ///<summary>
        /// 子品牌报价
        ///</summary>
        private string _prices = String.Empty;
        ///<summary>
        /// 子品牌上市时间
        ///</summary>
        private string _car_MarketDate = String.Empty;
        ///<summary>
        /// 子品牌报价区间
        ///</summary>
        private string _pricesRange = String.Empty;
        ///<summary>
        /// 子品牌厂商指导价区间
        ///</summary>
        private string _referPriceRange = String.Empty;
       
        #endregion

        #region 构造函数
        ///<summary>
        ///
        ///</summary>
        public Car_SerialItemEntity()
        {
        }
        ///<summary>
        ///
        ///</summary>
        public Car_SerialItemEntity
        (
            int cs_Id,
            string body_Doors,
            string engine_Exhaust,
            string underPan_Num_Type,
            string prices,
            string car_MarketDate,
            string pricesRange,
            string referPriceRange
        )
        {
            _cs_Id = cs_Id;
            _body_Doors = body_Doors;
            _engine_Exhaust = engine_Exhaust;
            _underPan_Num_Type = underPan_Num_Type;
            _prices = prices;
            _car_MarketDate = car_MarketDate;
            _pricesRange = pricesRange;
            _referPriceRange = referPriceRange;
        }

        #endregion

        #region 公共属性

        ///<summary>
        /// 子品牌ID
        ///</summary>
        public int cs_Id
        {
            get { return _cs_Id; }
            set { _cs_Id = value; }
        }

        ///<summary>
        /// 子品牌车门厢式
        ///</summary>
        public string Body_Doors
        {
            get { return _body_Doors; }
            set { _body_Doors = value; }
        }

        ///<summary>
        /// 子品牌排量
        ///</summary>
        public string Engine_Exhaust
        {
            get { return _engine_Exhaust; }
            set { _engine_Exhaust = value; }
        }

        ///<summary>
        /// 子品牌变速器
        ///</summary>
        public string UnderPan_Num_Type
        {
            get { return _underPan_Num_Type; }
            set { _underPan_Num_Type = value; }
        }

        ///<summary>
        /// 子品牌报价
        ///</summary>
        public string Prices
        {
            get { return _prices; }
            set { _prices = value; }
        }

        ///<summary>
        /// 子品牌上市时间
        ///</summary>
        public string Car_MarketDate
        {
            get { return _car_MarketDate; }
            set { _car_MarketDate = value; }
        }

        private string _car_RepairPolicy = string.Empty;
        public string Car_RepairPolicy
        {
            get { return _car_RepairPolicy; }
            set { _car_RepairPolicy = value; }
        }


        ///<summary>
        /// 子品牌报价区间
        ///</summary>
        public string PricesRange
        {
            get { return _pricesRange; }
            set { _pricesRange = value; }
        }

        ///<summary>
        /// 子品牌指导价区间
        ///</summary>
        public string ReferPriceRange
        {
            get { return _referPriceRange; }
            set { _referPriceRange = value; }
        }

        #endregion
    }
}
