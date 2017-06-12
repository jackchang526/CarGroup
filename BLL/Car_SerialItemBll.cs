using System;
using System.Collections.Generic;

using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.BLL
{
    public class Car_SerialItemBll
    {
        private static readonly Car_SerialItemDal csid = new Car_SerialItemDal();

        public Car_SerialItemBll()
        { }

        /// <summary>
        /// 取所有子品牌扩展信息
        /// </summary>
        /// <returns></returns>
        public IList<Car_SerialItemEntity> Get_Car_SerialItemAll()
        {
            return csid.Get_Car_SerialItemAll();
        }

        /// <summary>
        /// 根据子品牌ID取子品牌扩展信息
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public Car_SerialItemEntity Get_Car_SerialItemByCsID(int csID)
        {
            return csid.Get_Car_SerialItemByCsID(csID);
        }

        /// <summary>
        /// 获取子品牌关注对应的子品牌
        /// </summary>
        /// <param name="topNum"></param>
        /// <param name="cs_id"></param>
        /// <returns></returns>
        public IList<Car_SerialItemEntity> Get_SerialToSerial(int topNum, int cs_id)
        {
            return csid.Get_SerialToSerial(topNum, cs_id);
        }
    }
}
