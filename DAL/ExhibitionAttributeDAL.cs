using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.DAL
{
    public class ExhibitionAttributeDAL
    {
        /// <summary>
        /// 通过展会ID得到属性列表
        /// </summary>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        public virtual Dictionary<int, Model.Attribute> GetAttributeListByExhibitionId(int exhibitionId)
        {
            if (exhibitionId < 1)
            {
                return null;
            }

            string sql = "select * from dbo.CarExhibition_Attribute where ExhibitionID = @ExhibtionID and status = 1 order by quene";


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

                    Dictionary<int, Model.Attribute> attributeList = new Dictionary<int, BitAuto.CarChannel.Model.Attribute>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Model.Attribute attributeModel = new Model.Attribute();
                        attributeModel.ID = dr.IsNull("ID") ? 0 : ConvertHelper.GetInteger(dr["ID"].ToString());
                        attributeModel.Name = dr.IsNull("Name") ? "" : dr["Name"].ToString();

                        if (!attributeList.ContainsKey(attributeModel.ID))
                        {
                            attributeList.Add(attributeModel.ID, attributeModel);
                        }

                    }

                    return attributeList;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到排序的子品牌列表
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <param name="Level">类型:2:图集;3:子品牌</param>
        /// <returns></returns>
        public virtual Dictionary<int, int> GetOrderSerialTypeByAttributeId(int attributeId, int Level)
        {
            string sql = "select * from CarExhibition_Attr_Serial where AttrbibuteId="
                + attributeId + " and RelationType=" + Level.ToString() + " order by OrderQuene";

            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    {
                        return null;
                    }

                    Dictionary<int, int> serialOrder = new Dictionary<int, int>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!serialOrder.ContainsKey(ConvertHelper.GetInteger(dr["CarSerailID"].ToString())))
                        {
                            serialOrder.Add(ConvertHelper.GetInteger(dr["CarSerailID"].ToString()), 0);
                        }
                    }
                    return serialOrder;
                }
                
            }
            catch
            {
                return null;
            }
        }
    }
}
