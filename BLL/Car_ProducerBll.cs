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
        /// ���ݳ���IDȡ����Ϣ
        /// </summary>
        /// <param name="nCarProducerID"></param>
        /// <returns></returns>
        public Car_ProducerEntity GetCarProducerByCPID(int nCarProducerID)
        {
            return CarProducerDal.GetCarProducerByCPID(nCarProducerID);
        }

        /// <summary>
        /// ���ݳ���IDȡ��Ʒ����Ϣ
        /// </summary>
        /// <param name="nCarProducerID"></param>
        /// <returns></returns>
        public List<Car_BrandEntity> GetCarBrandListByCPID(int nCarProducerID)
        {
            return CarProducerDal.GetCarBrandListByCPID(nCarProducerID);
        }
    }
}
