using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Model;
 
namespace BitAuto.CarChannel.BLL
{
    public class Car_ProducerBll
    {
        private static readonly Car_ProducerDal CarProducerDal = new Car_ProducerDal();

        public Car_ProducerBll()
        { }

        /// <summary>
        /// 根据厂商ID取其信息
        /// </summary>
        /// <param name="nCarProducerID"></param>
        /// <returns></returns>
        public Car_ProducerEntity GetCarProducerByCPID(int nCarProducerID)
        {
            return CarProducerDal.GetCarProducerByCPID(nCarProducerID);
        }

        /// <summary>
        /// 根据厂商ID取其品牌信息
        /// </summary>
        /// <param name="nCarProducerID"></param>
        /// <returns></returns>
        public List<Car_BrandEntity> GetCarBrandListByCPID(int nCarProducerID)
        {
            return CarProducerDal.GetCarBrandListByCPID(nCarProducerID);
        }
    }
}
