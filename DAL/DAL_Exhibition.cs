using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using BitAuto.CarChannel.Common;
using BUD = BitAuto.Utils.Data;
using BitAuto.Utils;

namespace BitAuto.CarChannel.DAL
{
	/// <summary>
	/// 展会类 
	/// </summary>
	public class Exhibition
	{
		/// <summary>
		/// 得到展会的列表
		/// </summary>
		/// <returns>返回展会列表</returns>
		public static List<Model.Exhibition> GetExhibtionList()
		{
			string sqlString = "select ce.ID,ce.Name,ce.Status,nodece.ID as nodeID,nodece.Name as nodeName "
							  + "from CarExhibition as ce left join CarExhibition as nodece on nodece.parentID = ce.ID and nodece.status=1 where ce.parentID = 0"
							  + " select ExhibitionID,RelationCar from Exhibition_Relation_Car "
							  + "select cas.CarSerailID, attr.ID,attr.Name,attr.ExhibitionID,attr.quene from CarExhibition_Attr_Serial as cas left join "
							  + "(select ID,Name,ExhibitionID,quene from CarExhibition_Attribute where status = 1 ) as attr on cas.AttrbibuteID = attr.ID";
			try
			{
				DataSet ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
														, CommandType.Text
														, sqlString);

				if (ds == null || ds.Tables.Count < 3 || ds.Tables[0].Rows.Count < 1)
				{
					return null;
				}

				List<Model.Exhibition> model_ExhibtionList = new List<BitAuto.CarChannel.Model.Exhibition>();

				DataView ExhibitionDv = ds.Tables[0].DefaultView;
				DataTable ExhibitionDT = ExhibitionDv.ToTable(true, "ID", "Name", "Status");
				DataView brandDv = new DataView();
				DataView attributeDv = new DataView();
				foreach (DataRow dr in ExhibitionDT.Rows)
				{
					Model.Exhibition modelExhibition = new BitAuto.CarChannel.Model.Exhibition();
					modelExhibition.ID = Convert.ToInt32(dr["ID"]);
					modelExhibition.Name = dr["Name"].ToString();
					modelExhibition.Status = Convert.ToInt32(dr["Status"]);

					DataView pavilionDV = ds.Tables[0].DefaultView;
					pavilionDV.RowFilter = "ID=" + dr["ID"].ToString();
					modelExhibition.PavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();
					//绑定展会包括的主品牌ID列表
					brandDv = ds.Tables[1].DefaultView;
					brandDv.RowFilter = "ExhibitionID=" + modelExhibition.ID.ToString();
					if (brandDv.Count == 1)
					{
						modelExhibition.MasterBrandList = new Dictionary<int, int[]>();
						modelExhibition.MasterBrandList = ExhibitionXML.GetMasterBrandListByXMLString("<root>"
																									+ brandDv.ToTable().Rows[0]["RelationCar"].ToString()
																									+ "</root>");

					}
					//判断展会的展馆
					foreach (DataRow pavdr in pavilionDV.ToTable(true, "nodeID", "nodeName").Rows)
					{
						if (pavdr.IsNull("nodeID"))
						{
							continue;
						}
						Model.Pavilion modelpavilion = new BitAuto.CarChannel.Model.Pavilion();
						modelpavilion.ID = Convert.ToInt32(pavdr["nodeID"]);
						modelpavilion.Name = pavdr["nodeName"].ToString();
						brandDv = ds.Tables[1].DefaultView;
						brandDv.RowFilter = "ExhibitionID=" + modelpavilion.ID.ToString();

						if (brandDv.Count == 1)
						{
							modelpavilion.MasterBrandList = new Dictionary<int, int[]>();
							modelpavilion.MasterBrandList = ExhibitionXML.GetMasterBrandListByXMLString("<root>"
																										+ brandDv.ToTable().Rows[0]["RelationCar"].ToString()
																										+ "</root>");
							//按品牌ID匹配品牌列表
							Dictionary<int, int[]> tempMasterBrandList = new Dictionary<int, int[]>();
							foreach (KeyValuePair<int, int[]> keyValue in modelpavilion.MasterBrandList)
							{
								if (modelExhibition.MasterBrandList.ContainsKey(keyValue.Key))
								{
									tempMasterBrandList.Add(keyValue.Key, modelExhibition.MasterBrandList[keyValue.Key]);
									continue;
								}
							}

							modelpavilion.MasterBrandList = tempMasterBrandList;
						}
						if (!modelExhibition.PavilionList.ContainsKey(modelpavilion.ID))
						{
							modelExhibition.PavilionList.Add(modelpavilion.ID, modelpavilion);
						}
					}

					//绑定展会的属性列表
					attributeDv = ds.Tables[2].DefaultView;
					attributeDv.RowFilter = "ExhibitionID=" + modelExhibition.ID.ToString();
					attributeDv.Sort = "quene";
					modelExhibition.AttributeList = new Dictionary<int, BitAuto.CarChannel.Model.Attribute>();

					if (attributeDv.Count > 0)
					{
						foreach (DataRow attrDr in attributeDv.ToTable(true, "ID", "Name").Rows)
						{
							Model.Attribute attribute = new BitAuto.CarChannel.Model.Attribute();
							attribute.ID = Convert.ToInt32(attrDr["ID"]);
							attribute.Name = attrDr["Name"].ToString();
							attribute.SerialIDList = new Dictionary<int, int>();
							//绑定属性中的车子品牌
							DataView attrCarSerail = ds.Tables[2].DefaultView;
							attrCarSerail.RowFilter = "ID=" + attribute.ID;
							if (attributeDv.Count > 0)
							{
								foreach (DataRow attrserialDr in attrCarSerail.ToTable(true, "CarSerailID").Rows)
								{
									if (!attribute.SerialIDList.ContainsKey(Convert.ToInt32(attrserialDr["CarSerailID"])))
									{
										attribute.SerialIDList.Add(Convert.ToInt32(attrserialDr["CarSerailID"]), Convert.ToInt32(attrserialDr["CarSerailID"]));
									}
								}
							}
							if (!modelExhibition.AttributeList.ContainsKey(attribute.ID))
							{
								modelExhibition.AttributeList.Add(attribute.ID, attribute);
							}
						}
					}


					//添加展会到展会列表
					model_ExhibtionList.Add(modelExhibition);
				}
				return model_ExhibtionList;

			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 通过展会ID得到展会对象
		/// </summary>
		/// <returns>返回展会对象</returns>
		public static Model.Exhibition GetExhibtionByExhibitionID(int exhibtionID)
		{
			string sqlString = "select ce.ID,ce.Name,ce.Status,nodece.ID as nodeID,nodece.Name as nodeName "
							   + "from CarExhibition as ce left join CarExhibition as nodece on nodece.parentID = ce.ID and nodece.status=1 "
							   + "where ce.parentID = 0 and ce.ID = @ExhibtionID "
							   + "select ExhibitionID,RelationCar from Exhibition_Relation_Car as erc ,"
							   + "(select ID from CarExhibition where ID = @ExhibtionID or parentID = @ExhibtionID) as r where erc.ExhibitionID = r.ID "
							   + "select cas.CarSerailID, attr.ID,attr.Name,attr.ExhibitionID,attr.quene from CarExhibition_Attr_Serial as cas join "
							   + "(select ID,Name,ExhibitionID,quene from CarExhibition_Attribute where status = 1 and ExhibitionID=@ExhibtionID ) as attr on cas.AttrbibuteID = attr.ID";

			SqlParameter[] _param = { new SqlParameter("@ExhibtionID", SqlDbType.Int) };
			_param[0].Value = exhibtionID;

			try
			{
				DataSet ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString
													  , CommandType.Text
													  , sqlString
													  , _param);

				if (ds == null || ds.Tables.Count < 2 || ds.Tables[0].Rows.Count < 1)
				{
					return null;
				}

				Model.Exhibition modelExhibition = new BitAuto.CarChannel.Model.Exhibition();
				modelExhibition.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
				modelExhibition.Name = ds.Tables[0].Rows[0]["Name"].ToString();
				DataView brandDv = new DataView();
				DataView attributeDv = new DataView();
				DataView pavilionDV = ds.Tables[0].DefaultView;
				pavilionDV.RowFilter = "ID=" + modelExhibition.ID.ToString();
				modelExhibition.PavilionList = new Dictionary<int, BitAuto.CarChannel.Model.Pavilion>();
				//绑定展会的主品牌列表
				brandDv = ds.Tables[1].DefaultView;
				brandDv.RowFilter = "ExhibitionID=" + modelExhibition.ID.ToString();
				if (brandDv.Count == 1)
				{
					modelExhibition.MasterBrandList = new Dictionary<int, int[]>();
					modelExhibition.MasterBrandList = ExhibitionXML.GetMasterBrandListByXMLString("<root>"
																								+ brandDv.ToTable().Rows[0]["RelationCar"].ToString()
																								+ "</root>");
				}
				//绑定展会的展馆
				foreach (DataRow pavdr in pavilionDV.ToTable(true, "nodeID", "nodeName").Rows)
				{
					if (pavdr.IsNull("nodeID"))
					{
						continue;
					}
					Model.Pavilion modelpavilion = new BitAuto.CarChannel.Model.Pavilion();
					modelpavilion.ID = Convert.ToInt32(pavdr["nodeID"]);
					modelpavilion.Name = pavdr["nodeName"].ToString();
					brandDv = ds.Tables[1].DefaultView;
					brandDv.RowFilter = "ExhibitionID=" + modelpavilion.ID.ToString();

					if (brandDv.Count == 1)
					{
						modelpavilion.MasterBrandList = new Dictionary<int, int[]>();
						modelpavilion.MasterBrandList = ExhibitionXML.GetMasterBrandListByXMLString("<root>"
																									+ brandDv.ToTable().Rows[0]["RelationCar"].ToString()
																									+ "</root>");

						//按品牌ID匹配品牌列表
						Dictionary<int, int[]> tempMasterBrandList = new Dictionary<int, int[]>();
						foreach (KeyValuePair<int, int[]> keyValue in modelpavilion.MasterBrandList)
						{
							if (modelExhibition.MasterBrandList.ContainsKey(keyValue.Key))
							{
								tempMasterBrandList.Add(keyValue.Key, modelExhibition.MasterBrandList[keyValue.Key]);
								continue;
							}
						}

						modelpavilion.MasterBrandList = tempMasterBrandList;
					}


					if (!modelExhibition.PavilionList.ContainsKey(modelpavilion.ID))
					{
						modelExhibition.PavilionList.Add(modelpavilion.ID, modelpavilion);
					}
				}


				//绑定展会的属性列表
				attributeDv = ds.Tables[2].DefaultView;
				attributeDv.RowFilter = "ExhibitionID=" + modelExhibition.ID.ToString();
				attributeDv.Sort = "quene";
				modelExhibition.AttributeList = new Dictionary<int, BitAuto.CarChannel.Model.Attribute>();

				if (attributeDv.Count > 0)
				{
					foreach (DataRow attrDr in attributeDv.ToTable(true, "ID", "Name").Rows)
					{
						Model.Attribute attribute = new BitAuto.CarChannel.Model.Attribute();
						attribute.ID = Convert.ToInt32(attrDr["ID"]);
						attribute.Name = attrDr["Name"].ToString();
						attribute.SerialIDList = new Dictionary<int, int>();
						//绑定属性中的车子品牌
						DataView attrCarSerail = ds.Tables[2].DefaultView;
						attrCarSerail.RowFilter = "ID=" + attribute.ID;
						if (attributeDv.Count > 0)
						{
							foreach (DataRow attrserialDr in attrCarSerail.ToTable(true, "CarSerailID").Rows)
							{
								if (!attribute.SerialIDList.ContainsKey(Convert.ToInt32(attrserialDr["CarSerailID"])))
								{
									attribute.SerialIDList.Add(Convert.ToInt32(attrserialDr["CarSerailID"]), Convert.ToInt32(attrserialDr["CarSerailID"]));
								}
							}
						}
						if (!modelExhibition.AttributeList.ContainsKey(attribute.ID))
						{
							modelExhibition.AttributeList.Add(attribute.ID, attribute);
						}
					}
				}
				return modelExhibition;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 通过展会ID得到展会对象
		/// </summary>
		/// <param name="exhibitionId">展会ID</param>
		/// <returns></returns>
		public virtual Model.Exhibition GetExhibitionByID(int exhibitionId)
		{
			if (exhibitionId < 1)
			{
				return null;
			}

			string sql = "select * from CarExhibition as ce left join Exhibition_Relation_Car as erc on erc.ExhibitionID = ce.ID"
					   + " where ce.ID = @ExhibtionID and ce.status = 1 order by ce.ID";

			SqlParameter[] _param = { new SqlParameter("@ExhibtionID", SqlDbType.Int) };
			_param[0].Value = exhibitionId;

			try
			{
				using (DataSet ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param))
				{
					if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
					{
						return null;
					}

					Model.Exhibition exhibitionModel = new BitAuto.CarChannel.Model.Exhibition();

					exhibitionModel.ID = ConvertHelper.GetInteger(ds.Tables[0].Rows[0]["ID"].ToString());
					exhibitionModel.Name = ds.Tables[0].Rows[0]["Name"].ToString();
					exhibitionModel.XmlContent = ds.Tables[0].Rows[0]["RelationCar"].ToString();

					return exhibitionModel;
				}
			}
			catch
			{
				return null;
			}
		}

        /// <summary>
        /// 获取PavilionMaster xml，解决一个品牌存在多个展馆下的问题  2015-11-17 广州车展
        /// </summary>
        /// <param name="exhibitionId"></param>
        /// <returns></returns>
        public string GetPavilionMasterByExhibitionId(int exhibitionId)
        {
            if (exhibitionId < 1)
            {
                return null;
            }
            string sql = "select * from CarExhibition as ce left join Exhibition_Relation_Car as erc on erc.ExhibitionID = ce.ID"
                       + " where ce.ID = @ExhibtionID and ce.status = 1 order by ce.ID";

            SqlParameter[] _param = { new SqlParameter("@ExhibtionID", SqlDbType.Int) };
            _param[0].Value = exhibitionId;

            try
            {
                using (DataSet ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                    {
                        return null;
                    }
                    if (ds.Tables[0].Rows[0]["PavilionMaster"] != DBNull.Value
                        && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PavilionMaster"].ToString()))
                    {
                        return ds.Tables[0].Rows[0]["PavilionMaster"].ToString();
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
		/// <summary>
		/// 得到展会列表
		/// </summary>
		/// <returns></returns>
		public virtual DataSet GetExhibitionList()
		{
			string sqlString = "select c.id,c.Name from CarExhibition as c where c.parentID = 0 and c.Status <> 2 order by c.StartTime desc";
			try
			{
				using (DataSet ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlString))
				{
					return ds;
				}
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 得到展会对应的子品牌彩虹条列表
		/// </summary>
		/// <param name="serialIdList"></param>
		/// <param name="exhibtionId"></param>
		/// <returns></returns>
		public virtual Dictionary<int, Dictionary<int, Model.ExhibitionRainbow>> GetExhibitionRainbowBySerialIDList(List<int> serialIdList, int exhibtionId)
		{
			if (exhibtionId < 1 || serialIdList == null || serialIdList.Count < 1)
			{
				return null;
			}

			string sqlWhere = Common.CommonFunction.GetSqlInString(serialIdList);
			string sql = "select * from Exhibition_Rainbow where ExhibitionId ="
						+ exhibtionId.ToString() + " and SerialId in (" + sqlWhere + ")";

			try
			{
				using (DataSet ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql))
				{
					if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
					{
						return null;
					}

					Dictionary<int, Dictionary<int, Model.ExhibitionRainbow>> exhibitionRainbow = new Dictionary<int, Dictionary<int, Model.ExhibitionRainbow>>();

					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						Model.ExhibitionRainbow rainbowEntity = new Model.ExhibitionRainbow();
						rainbowEntity.SerialID = ConvertHelper.GetInteger(dr["SerialId"].ToString());
						rainbowEntity.ItemID = ConvertHelper.GetInteger(dr["ItemId"].ToString());
						rainbowEntity.Url = dr["Url"].ToString();
						rainbowEntity.HappenTime = ConvertHelper.GetDateTime(dr["HappenTime"].ToString());
						if (!exhibitionRainbow.ContainsKey(rainbowEntity.SerialID))
						{
							Dictionary<int, Model.ExhibitionRainbow> nodeRainbow = new Dictionary<int, Model.ExhibitionRainbow>();
							nodeRainbow.Add(rainbowEntity.ItemID, rainbowEntity);
							exhibitionRainbow.Add(rainbowEntity.SerialID, nodeRainbow);
						}
						else if (!exhibitionRainbow[rainbowEntity.SerialID].ContainsKey(rainbowEntity.ItemID))
						{
							exhibitionRainbow[rainbowEntity.SerialID].Add(rainbowEntity.ItemID, rainbowEntity);
						}
					}

					return exhibitionRainbow;
				}
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 根据车展ID取展馆
		/// </summary>
		/// <param name="exhibitionId">车展ID</param>
		/// <returns></returns>
		public static DataSet GetAllPravilionByExhibitionId(int exhibitionId)
		{
			DataSet ds = new DataSet();
			string sql = "select id,name from CarExhibition where parentid=@ExhibtionID and status=1 order by id ";

			SqlParameter[] _param = { new SqlParameter("@ExhibtionID", SqlDbType.Int) };
			_param[0].Value = exhibitionId;
			ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
			return ds;
		}

		/// <summary>
		/// 根据车展ID取属性
		/// </summary>
		/// <param name="exhibitionId">车展ID</param>
		/// <returns></returns>
		public static DataSet GetAllAttributeByExhibitionId(int exhibitionId)
		{
			DataSet ds = new DataSet();
			string sql = "select ID,Name,status,quene from CarExhibition_Attribute where ExhibitionID=@ExhibtionID and status=1 order by quene ";

			SqlParameter[] _param = { new SqlParameter("@ExhibtionID", SqlDbType.Int) };
			_param[0].Value = exhibitionId;
			ds = BUD.SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _param);
			return ds;
		}

	}
}
