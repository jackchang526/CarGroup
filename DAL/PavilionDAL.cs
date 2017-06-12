using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BitAuto.CarChannel.Common;

using BitAuto.Utils;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
    public class PavilionDAL
    {
        /// <summary>
        /// 通过展会ID得到标签列表
        /// </summary>
        /// <param name="exhibitionId">展会ID</param>
        /// <returns></returns>
        public virtual Dictionary<int, Model.Pavilion> GetPavilionListByExhibitionId(int exhibitionId)
        {
            if (exhibitionId < 1)
            {
                return null;
            }

            string sql = "select * from dbo.CarExhibition where parentId = @ExhibtionID and status = 1 order by ID";

            SqlParameter[] _param = { new SqlParameter("@ExhibtionID", SqlDbType.Int) };
            _param[0].Value = exhibitionId;

            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    {
                        return null;
                    }

                    Dictionary<int, Model.Pavilion> pavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Model.Pavilion pavilionModel = new Model.Pavilion();
                        pavilionModel.ID = dr.IsNull("ID") ? 0 : ConvertHelper.GetInteger(dr["ID"].ToString());
                        pavilionModel.Name = dr.IsNull("Name") ? "" : dr["Name"].ToString();

                        if (!pavilionList.ContainsKey(pavilionModel.ID))
                        {
                            pavilionList.Add(pavilionModel.ID, pavilionModel);
                        }

                    }

                    return pavilionList;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
