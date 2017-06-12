using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.CarChannel.Model;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL
{
    public class Car_ProducerDal
    {
        public Car_ProducerDal()
        { }

        /// <summary>
        /// 取得Car_ProducerEntity
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Car_ProducerEntity Get_Car_ProducerEntity_FromDr(DataRow row)
        {
            Car_ProducerEntity producer = new Car_ProducerEntity();
            if (null != row)
            {
                producer.Cp_Id = (DBNull.Value == (row["Cp_id"])) ? -1 : Convert.ToInt32(row["Cp_id"]);
                producer.Cp_Name = row["Cp_name"].ToString().Trim();
                producer.Cp_Introduction = row["Cp_Introduction"].ToString().Trim();
                producer.Cp_LogoUrl = row["Cp_LogoUrl"].ToString().Trim();
                producer.Cp_Url = row["Cp_Url"].ToString().Trim();
                producer.Cp_Phone = row["Cp_Phone"].ToString().Trim();
				producer.Cp_ShortName = row["Cp_ShortName"].ToString().Trim();
                producer.Cp_seoName = row["cp_seoname"].ToString().Trim();
            }
            else
            {
                return null;
            }
            return producer;
        }

        /// <summary>
        /// 取得Car_ProducerEntity
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public List<Car_BrandEntity> Get_Car_BrandEntity_FromTBL(DataTable dtCarBrandEntity)
        {
			List<Car_BrandEntity> carBrandList = new List<Car_BrandEntity>();
            if (null != dtCarBrandEntity)
            {
				foreach (DataRow dr in dtCarBrandEntity.Rows)
				{
					Car_BrandEntity carBrand = new Car_BrandEntity();

					carBrand.Cb_Id = (DBNull.Value == (dr["cb_id"])) ? -1 : Convert.ToInt32(dr["cb_id"]);
					carBrand.Cb_Name = dr["cb_name"].ToString().Trim();
					carBrand.Cb_Logo = dr["cb_logo"].ToString().Trim();
					carBrand.Cb_AllSpell = dr["allspell"].ToString().Trim();

					carBrandList.Add(carBrand);
				}
            }
            return carBrandList;
        }

        /// <summary>
        /// 根据厂商ID取其信息
        /// </summary>
        /// <param name="nCarProducerEntityID"></param>
        /// <returns></returns>
        public Car_ProducerEntity GetCarProducerByCPID(int nCarProducerEntityID)
        {
            Car_ProducerEntity carProducerEntity = null;

            string sqlStr = "SELECT cp_id, cp_name,Cp_ShortName, Cp_Introduction,Cp_LogoUrl,Cp_Url,Cp_Phone,isnull(cp_seoname,cp_name) as cp_seoname " +
                            "FROM dbo.Car_Producer WHERE IsState = 1 and cp_id = @cp_id";

            SqlParameter[] _param ={ new SqlParameter("@cp_id", SqlDbType.Int) };
            _param[0].Value = nCarProducerEntityID;
            
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    carProducerEntity = Get_Car_ProducerEntity_FromDr(ds.Tables[0].Rows[0]);
                }
            }
            catch
            {
                return carProducerEntity;
            }

            return carProducerEntity;
        }


        /// <summary>
        /// 根据厂商ID取其品牌信息
        /// </summary>
        /// <param name="nCarProducerEntityID"></param>
        /// <returns></returns>
        public List<Car_BrandEntity> GetCarBrandListByCPID(int nCarProducerEntityID)
        {
            List<Car_BrandEntity> carBrandList = null;

			string sqlStr = "SELECT cb_id, cb_name, cb_logo, allspell FROM Car_Brand WHERE IsState = 1 and cp_id = @cp_id ";
			//	+ "AND EXISTS(SELECT 1 FROM dbo.Car_Serial WHERE cs_CarLevel<>'其它' AND CsSaleState<>'停销' AND cb_Id=cb.cb_Id)";

            SqlParameter[] _param ={ new SqlParameter("@cp_id", SqlDbType.Int) };
            _param[0].Value = nCarProducerEntityID;

            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);

                if (ds != null && ds.Tables.Count > 0)
                {
                    carBrandList = Get_Car_BrandEntity_FromTBL(ds.Tables[0]);
                }
            }
            catch{}

            return carBrandList;
        }

		/// <summary>
		/// 获取所有厂商的名称与全拼
		/// </summary>
		/// <returns></returns>
		public DataSet GetProducerNameAndSpell()
		{
            string sqlStr = "SELECT [Cp_Id],[Cp_Name],[Cp_ShortName],[Spell],isnull(cp_seoname, cp_name) as cp_seoname FROM [AutoCarChannel].[dbo].[Car_Producer] where IsState=1";
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}
    }
}
