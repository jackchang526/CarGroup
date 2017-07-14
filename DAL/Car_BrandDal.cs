using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.CarChannel.Model;
using System.Data.SqlClient;
using System.Data;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;
using System.Xml;

namespace BitAuto.CarChannel.DAL
{
	public class Car_BrandDal
	{
		public Car_BrandDal()
		{ }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="strCBSpName"></param>
		/// <returns></returns>
		public DataRow GetCarMasterBrandInfoByBSID(int nBSID)
		{
			DataRow drCarMasterBrandInfo = null;

			string sqlStr = "SELECT bs.bs_id, bs.bs_name, bs.bs_logo, bs_logoinfo,bs_introduction,urlspell,bs_Country,isnull(bs.bs_seoname,bs.bs_name) as bs_seoname " +
							"FROM Car_MasterBrand bs " +
							"WHERE bs.isstate = 1 AND bs.bs_id = @bs_id";

			SqlParameter[] _param = { new SqlParameter("@bs_id", SqlDbType.Int) };
			_param[0].Value = nBSID;

			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					drCarMasterBrandInfo = ds.Tables[0].Rows[0];
				}
			}
			catch
			{
				return null;
			}

			return drCarMasterBrandInfo;
		}
		/// <summary>
		/// 得到主品牌对象通过主品牌ID
		/// </summary>
		/// <param name="bsId">主品牌ID</param>
		/// <returns></returns>
		public Car_MasterBrandEntity GetCarMasterBrandEntityByBSID(int bsId)
		{
			if (bsId < 1)
			{
				return null;
			}
			string cacheKey = "serial-carmasterbrandInfo-list-" + bsId;
			DataRow carMasterBrandInfo = (DataRow)CacheManager.GetCachedData(cacheKey);

			if (null == carMasterBrandInfo)
			{
				carMasterBrandInfo = GetCarMasterBrandInfoByBSID(bsId);
				if (null != carMasterBrandInfo)
				{
					CacheManager.InsertCache(cacheKey, carMasterBrandInfo, WebConfig.CachedDuration);
				}
			}

			if (carMasterBrandInfo == null)
			{
				return null;
			}

			Car_MasterBrandEntity cmbe = new Car_MasterBrandEntity();
			cmbe.Bs_Id = ConvertHelper.GetInteger(carMasterBrandInfo["bs_id"].ToString());
			cmbe.Bs_Name = carMasterBrandInfo["bs_name"].ToString();
			cmbe.Bs_Logo = carMasterBrandInfo["bs_logo"].ToString();
			cmbe.Bs_LogoInfo = carMasterBrandInfo["bs_logoinfo"].ToString();
			cmbe.Bs_introduction = carMasterBrandInfo["bs_introduction"].ToString();
			cmbe.Bs_allSpell = carMasterBrandInfo["urlspell"].ToString();
			cmbe.Bs_Country = carMasterBrandInfo["bs_Country"].ToString();
			cmbe.Bs_seoname = carMasterBrandInfo["bs_seoname"].ToString();
			return cmbe;
		}
		/// <summary>
		/// 得到品牌对象通过品牌ID
		/// </summary>
		/// <param name="brandID"></param>
		/// <returns></returns>
		public Car_BrandEntity GetBrandEntityByBrandID(int brandID)
		{
			if (brandID < 1)
			{
				return null;
			}

			string cacheKey = "serial-carbrand-list-" + brandID.ToString();
			object carBrandInfo = null;
			CacheManager.GetCachedData(cacheKey, out carBrandInfo);
			if (null == carBrandInfo)
			{
				carBrandInfo = GetCarBrandInfoByCBID(brandID);
				if (null != carBrandInfo)
				{
					CacheManager.InsertCache(cacheKey, carBrandInfo, WebConfig.CachedDuration);
				}
			}

			if (carBrandInfo == null
				|| ((DataSet)carBrandInfo).Tables.Count < 1
				|| ((DataSet)carBrandInfo).Tables[0].Rows.Count < 1)
			{
				return null;
			}

			DataRow brandRow = ((DataSet)carBrandInfo).Tables[0].Rows[0];
			Car_BrandEntity cbe = new Car_BrandEntity();
			cbe.Cb_Id = ConvertHelper.GetInteger(brandRow["cb_id"].ToString());
			cbe.Cb_Name = brandRow["cb_name"].ToString();
			cbe.Cb_Logo = brandRow["cb_logo"].ToString();
			cbe.Cb_AllSpell = brandRow["allSpell"].ToString();
			cbe.Cb_introduction = brandRow["cb_introduction"].ToString();
			cbe.Cb_SEOName = brandRow["cb_seoname"].ToString();
			cbe.Cb_Phone = brandRow["cb_Phone"].ToString();
			cbe.Cb_url = brandRow["cb_url"].ToString();
			cbe.Bs_Id = ConvertHelper.GetInteger(brandRow["bs_id"].ToString());
			cbe.Bs_Name = brandRow["bs_name"].ToString();
			cbe.Bs_UrlSpell = brandRow["MasterSpell"].ToString();
			cbe.Bs_LogoInfo = brandRow["bs_logoinfo"].ToString();
			cbe.Cp_Id = ConvertHelper.GetInteger(brandRow["cp_id"].ToString());
			cbe.Cp_ShortName = brandRow["Cp_ShortName"].ToString();
			cbe.Cp_LogoUrl = brandRow["Cp_LogoUrl"].ToString();
			cbe.Cp_Name = brandRow["cp_name"].ToString();
			cbe.Cp_Url = brandRow["cp_url"].ToString();
			cbe.Cp_Phone = brandRow["cp_phone"].ToString();
			cbe.Cp_Introduction = brandRow["cp_introduction"].ToString();
			cbe.Cp_Country = brandRow["Cp_Country"].ToString();
			return cbe;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="strCBSpName"></param>
		/// <returns></returns>
		public DataRow GetCarBrandInfoByCBSpellName(string strCBSpName)
		{
			DataRow drCarBrandInfo = null;

			string sqlStr = @"SELECT  cb.cb_id, cb.cb_name, cb.cb_logo, ISNULL(cp.cp_id, 0) AS cp_id,
                                        cp.Cp_LogoUrl, cp.cp_name, cp.cp_url, cp.cp_phone, cp.cp_introduction,
                                        bs.bs_id, bs.bs_name, bs.bs_logoinfo
                                FROM    Car_MasterBrand bs,
                                        Car_MasterBrand_Rel cr ,
                                        Car_Producer cp,
                                        Car_Brand cb
                                WHERE   cr.bs_id = bs.bs_id
                                        AND cr.cb_id = cb.cb_id
                                        AND cb.cp_id = cp.cp_id
                                        AND cb.isstate = 1
                                        AND cb.allspell = @allspell";

			SqlParameter[] _param = { new SqlParameter("@allspell", SqlDbType.VarChar) };
			_param[0].Value = strCBSpName;

			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					drCarBrandInfo = ds.Tables[0].Rows[0];
				}
			}
			catch
			{
				return null;
			}

			return drCarBrandInfo;
		}


		public DataSet GetCarBrandInfoByCBID(int cbID)
		{
			DataSet dsCarBrandInfo = null;

			string sqlStr = @"SELECT  cb.cb_id, cb.cb_name, cb.cb_logo, cb.allSpell, cb.cb_introduction,
                                    ISNULL(cb.cb_seoname, cb.cb_name) AS cb_seoname, cb.cb_Phone,
                                    cb.cb_url, cb.brandPic, ISNULL(cp.cp_id, 0) AS cp_id, cp.Cp_ShortName,
                                    cp.Cp_LogoUrl, cp.cp_name, cp.cp_url, cp.cp_phone, cp.cp_introduction,
                                    cb.cb_Country AS Cp_Country, bs.bs_id, bs.bs_name,
                                    bs.urlspell AS MasterSpell, bs.bs_logoinfo
                            FROM    Car_MasterBrand bs,
                                    Car_MasterBrand_Rel cr ,
                                    Car_Producer cp,
                                    Car_Brand cb
                            WHERE   cr.bs_id = bs.bs_id
                                    AND cr.cb_id = cb.cb_id
                                    AND cb.cp_id = cp.cp_id
                                    AND cb.isstate = 1
                                    AND cb.cb_id = @cbid";

			SqlParameter[] _param = { new SqlParameter("@cbid", SqlDbType.VarChar) };
			_param[0].Value = cbID;

			try
			{
				dsCarBrandInfo = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
			}
			catch
			{
				return null;
			}

			return dsCarBrandInfo;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nCBID"></param>
		/// <returns></returns>
		public List<CarSerialPhotoEntity> GetCarSerialPhotoListByCBID(int nCBID, bool isAll)
		{
			List<CarSerialPhotoEntity> carSerialPhotoList = new List<CarSerialPhotoEntity>();

			string sqlStr = "SELECT cs.cs_id, cs_name, cs_ShowName,spell,allspell,CsSaleState,cs_CarLevel,cs_seoname FROM Car_Serial cs "
				+ " LEFT JOIN Serial_PvRank spv ON  cs.cs_id=spv.CS_Id"
				+ " WHERE isstate = 1 AND cb_id = @cb_id ";
			if (!isAll)
				sqlStr += " AND CsSaleState<>'停销'";

			sqlStr += " ORDER BY cs.CsSaleState DESC,spv.Pv_SumNum DESC,cs.allspell";

			SqlParameter[] _param = { new SqlParameter("@cb_id", SqlDbType.Int) };
			_param[0].Value = nCBID;

			try
			{
				DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					// Dictionary<int, XmlElement> dicCSPhoto = CarSerialImgUrlService.GetImageUrlDic();
					Dictionary<int, XmlElement> dicCSPhoto = CarSerialImgUrlService.GetImageUrlDicNew();
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						int nCSID = (DBNull.Value == dr["cs_id"]) ? -1 : Convert.ToInt32(dr["cs_id"]);
						CarSerialPhotoEntity csPhotoEntity = new CarSerialPhotoEntity();
						csPhotoEntity.SerialId = nCSID;
						if (dicCSPhoto.ContainsKey(nCSID))
						{
							// modified by chengl Jan.4.2010
							if (dicCSPhoto[nCSID].Attributes["ImageUrl2"].Value.ToString().Trim() != "")
							{
								// 有新封面
								csPhotoEntity.CS_ImageUrl = string.Format(dicCSPhoto[nCSID].Attributes["ImageUrl2"].Value.ToString().Trim(), "1");
							}
							else
							{
								// 没有新封面
								if (dicCSPhoto[nCSID].Attributes["ImageUrl"].Value.ToString().Trim() != "")
								{
									csPhotoEntity.CS_ImageUrl = string.Format(dicCSPhoto[nCSID].Attributes["ImageUrl"].Value.ToString().Trim(), "1");
								}
								else
								{
									csPhotoEntity.CS_ImageUrl = WebConfig.DefaultCarPic;
								}
							}
							//int imgId = Convert.ToInt32(dicCSPhoto[nCSID].Attributes["ImageId"].Value);
							//string imgUrl = dicCSPhoto[nCSID].Attributes["ImageUrl"].Value.ToString();
							//if (imgUrl.Trim().Length == 0)
							//    csPhotoEntity.CS_ImageUrl = WebConfig.DefaultCarPic;
							//else
							//    csPhotoEntity.CS_ImageUrl = new CommonFunction().GetPublishImage(1, imgUrl, imgId);
						}
						else
							csPhotoEntity.CS_ImageUrl = WebConfig.DefaultCarPic;
						csPhotoEntity.CS_Name = Convert.ToString(dr["cs_name"]).Trim();
						csPhotoEntity.ShowName = Convert.ToString(dr["cs_ShowName"]).Trim();
						csPhotoEntity.CS_AllSpell = Convert.ToString(dr["allspell"]).Trim().ToLower();
						csPhotoEntity.SaleState = Convert.ToString(dr["CsSaleState"]).Trim();
						csPhotoEntity.SerialLevel = Convert.ToString(dr["cs_CarLevel"]).Trim();
						csPhotoEntity.CS_SeoName = Convert.ToString(dr["cs_seoname"]).Trim();
						csPhotoEntity.Cs_spell = Convert.ToString(dr["spell"]).Trim();

						carSerialPhotoList.Add(csPhotoEntity);
					}
				}
			}
			catch
			{
				return carSerialPhotoList;
			}

			return carSerialPhotoList;
		}

		/// <summary>
		/// 根据品牌ID 取旗下子品牌
		/// </summary>
		/// <param name="cbID">品牌ID</param>
		/// <param name="isAll">是否包括停销</param>
		/// <returns></returns>
		public DataSet GetCarSerialListByBrandID(int cbID, bool isAll)
		{
			DataSet ds = new DataSet();
			string sql = @"select 	cb.cb_id, cb.cb_name, cb.allspell as cbspell 
				, cs.cs_id, cs.cs_name,cs.cs_ShowName, cs.allspell as csspell 
				,cs.cs_CarLevel as cslevel,cs.CsSaleState,cs.spell as spell 
				from car_serial cs 
				left join car_brand cb on cs.cb_id=cb.cb_id
				left join Car_Serial_30UV cs30 on cs.cs_id=cs30.CS_Id 
				where cs.isState=1 and cs.cb_id=@cbid {0} 	
				ORDER BY cs.CsSaleState 
				DESC,cs30.UVCount
				DESC,csspell	";
			SqlParameter[] _param = { new SqlParameter("@cbid", SqlDbType.Int) };
			_param[0].Value = cbID;
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.DefaultConnectionString, CommandType.Text
				, string.Format(sql, isAll ? "" : "and cs.CsSaleState<>'停销' "), _param);
			return ds;
		}

		/// <summary>
		/// 根据主品牌ID 取旗下子品牌
		/// </summary>
		/// <param name="nBSID"></param>
		/// <param name="isAll"></param>
		/// <returns></returns>
		public DataSet GetCarSerialListByBSID(int nBSID, bool isAll)
		{
			DataSet ds = new DataSet(); 
            string sql = string.Format(@"SELECT  cb.cb_id, cb.cb_name, cb.allspell AS cbspell,cb.cb_Country AS cp_Country, cs.cs_id,
                                    cs.cs_name, cs.cs_ShowName, cs.allspell AS csspell,
                                    cs.cs_CarLevel AS cslevel, cs.CsSaleState, cs.spell AS spell
                            FROM    Car_MasterBrand bs
                                    LEFT JOIN Car_MasterBrand_Rel cmr ON cmr.bs_id = bs.bs_id
                                    LEFT JOIN Car_Brand cb ON cmr.cb_id = cb.cb_id
                                    INNER JOIN Car_Serial cs ON cb.cb_id = cs.cb_id {0} AND cs.IsState = 1
                                    LEFT JOIN Car_Serial_30UV cs30 ON cs.cs_id = cs30.CS_Id
                            WHERE   bs.bs_id = @bs_id
                            ORDER BY cs.CsSaleState DESC, cs30.UVCount DESC, csspell", (!isAll ? " and cs.CsSaleState<>'停销'" : ""));

            SqlParameter[] _param = { new SqlParameter("@bs_id", SqlDbType.Int) };
			_param[0].Value = nBSID;
			ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
				WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
			return ds;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nCBID"></param>
		/// <returns></returns>
		public DataSet GetCarSerialPhotoListByBSID(int nBSID, bool isAll)
		{
			DataSet carSerialPhotoList = new DataSet();

			//StringBuilder sbCBSpellList = new StringBuilder();
			//sbCBSpellList.Append("SELECT cb.cb_name, cb.allspell as cbspell,cp.Cp_Country FROM Car_Brand cb, Car_MasterBrand bs, Car_MasterBrand_Rel cmr ,Car_Producer cp ");
			//sbCBSpellList.Append("WHERE cmr.bs_id = bs.bs_id AND cmr.cb_id = cb.cb_id AND cb.isstate = 1 And bs.isstate = 1 AND bs.bs_id = @bs_id AND cb.cp_id=cp.cp_id ");
			//sbCBSpellList.Append(" ORDER BY cp.Cp_Country DESC ,cb.spell");

            string cbSpellList = @"SELECT  cb.cb_name, cb.allspell AS cbspell, cb.cb_Country AS Cp_Country
                                    FROM    Car_Brand cb ,
                                            Car_MasterBrand bs ,
                                            Car_MasterBrand_Rel cmr
                                    WHERE   cmr.bs_id = bs.bs_id
                                            AND cmr.cb_id = cb.cb_id
                                            AND cb.isstate = 1
                                            AND bs.isstate = 1
                                            AND bs.bs_id = @bs_id
                                    ORDER BY cb.weight desc, cb.spell";


            //string sqlResult = "select cb.cb_id, cb.cb_name, cb.allspell as cbspell"
            //				+ ", cs.cs_id, cs.cs_name,cs.cs_ShowName, cs.allspell as csspell"
            //				+ ",cs.cs_CarLevel as cslevel,cs.CsSaleState,cs.spell as spell"
            //				+ " from Car_MasterBrand bs"
            //				+ " left join Car_MasterBrand_Rel cmr on cmr.bs_id = bs.bs_id"
            //				+ " left join Car_Brand cb on cmr.cb_id = cb.cb_id"
            //				+ " inner join Car_Serial cs on cb.cb_id = cs.cb_id" + (!isAll ? " and cs.CsSaleState<>'停销'" : "") + " and cs.IsState=1"
            //				+ " left join Serial_PvRank spv on cs.cs_id=spv.CS_Id"
            //				+ " where bs.bs_id  = @bs_id"
            //				+ " ORDER BY cs.CsSaleState"
            //				+ " DESC,spv.Pv_SumNum"
            //				+ " DESC,csspell";

            string sqlResult = string.Format(@"SELECT  cb.cb_id, cb.cb_name, cb.allspell AS cbspell, cs.cs_id, cs.cs_name,
									cs.cs_ShowName, cs.allspell AS csspell, cs.cs_CarLevel AS cslevel,
									cs.CsSaleState, cs.spell AS spell,cs.cs_seoname,csi.ReferPriceRange, ( CASE cs.CsSaleState
										WHEN '在销' THEN 1
										WHEN '待销' THEN 1
										ELSE 3
										END ) AS CsSaleStateSort
							FROM    Car_MasterBrand bs
									LEFT JOIN Car_MasterBrand_Rel cmr ON cmr.bs_id = bs.bs_id
									LEFT JOIN Car_Brand cb ON cmr.cb_id = cb.cb_id
									INNER JOIN Car_Serial cs ON cb.cb_id = cs.cb_id {0} 
																AND cs.IsState = 1
									LEFT JOIN dbo.Car_Serial_30UV uv ON cs.cs_Id = uv.cs_id
									LEFT JOIN dbo.Car_Serial_Item csi on cs.cs_Id=csi.cs_Id
							WHERE   bs.bs_id = @bs_id
							ORDER BY 
			cb.weight desc,cb.spell,cb.cb_id,
			CsSaleStateSort,cs.weight desc,cs.spell,cs.cs_id", (!isAll ? " and cs.CsSaleState<>'停销'" : ""));
			//StringBuilder sqlResult = new StringBuilder();

			//sqlResult.Append("SELECT cb.cb_id, cb.cb_name, cb.allspell as cbspell, cs.cs_id, cs.cs_name,cs.cs_ShowName, cs.allspell as csspell,cs.cs_CarLevel as cslevel,cs.CsSaleState ");
			//sqlResult.Append("FROM Car_Serial cs, Car_Brand cb, Car_MasterBrand bs, Car_MasterBrand_Rel cmr ,Serial_PvRank spv");
			//sqlResult.Append(" WHERE cmr.bs_id = bs.bs_id AND cmr.cb_id = cb.cb_id AND cb.cb_id = cs.cb_id AND bs.isstate = 1 AND cs.cs_id=spv.CS_Id ");
			//if (!isAll)
			//    sqlResult.Append(" AND cs.CsSaleState<>'停销' ");
			//sqlResult.Append(" AND cs.IsState=1 AND bs.bs_id  = @bs_id ");
			//sqlResult.Append(" ORDER BY cs.CsSaleState DESC,spv.Pv_SumNum DESC,csspell");

			SqlParameter[] _param = { new SqlParameter("@bs_id", SqlDbType.Int) };
			_param[0].Value = nBSID;

			try
			{
				//主品牌旗下品牌列表（spell）
				DataSet dsCBSpellList = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, cbSpellList, _param);
				//主品牌旗下各品牌的子品牌列表
				DataSet dsResult = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlResult.ToString(), _param);

				if ((dsCBSpellList != null && dsCBSpellList.Tables.Count > 0 && dsCBSpellList.Tables[0].Rows.Count > 0) &&
					(dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0))
				{
					//取得子品牌车标URL XML文件
					// Dictionary<int, XmlElement> dicCSPhoto = CarSerialImgUrlService.GetImageUrlDic();
					Dictionary<int, XmlElement> dicCSPhoto = CarSerialImgUrlService.GetImageUrlDicNew();

					foreach (DataRow drCSspell in dsCBSpellList.Tables[0].Rows)
					{
						//按各品牌AllSpell分组 便于页面显示
						DataRow[] drCSInfo = dsResult.Tables[0].Select(string.Format("cbspell ='{0}'", drCSspell["cbspell"].ToString().Trim()));

						if (drCSInfo.Length > 0)
						{
							string tableName = drCSspell["cb_name"].ToString();
							string country = drCSspell["Cp_Country"].ToString().Trim();
							if (country != "中国")
								tableName = "进口" + tableName;
							DataTable dtCSPhotoList = new DataTable(tableName);

							dtCSPhotoList.Columns.Add(new DataColumn("cbspell", typeof(string)));
							dtCSPhotoList.Columns.Add(new DataColumn("cs_id", typeof(int)));
							dtCSPhotoList.Columns.Add(new DataColumn("cb_id", typeof(int)));
							dtCSPhotoList.Columns.Add(new DataColumn("cs_name", typeof(string)));
							dtCSPhotoList.Columns.Add(new DataColumn("csspell", typeof(string)));
							dtCSPhotoList.Columns.Add(new DataColumn("csImageUrl", typeof(string)));
							dtCSPhotoList.Columns.Add(new DataColumn("cslevel", typeof(string)));
							dtCSPhotoList.Columns.Add(new DataColumn("CsSaleState", typeof(string)));
							dtCSPhotoList.Columns.Add(new DataColumn("cs_ShowName", typeof(string)));
                            dtCSPhotoList.Columns.Add(new DataColumn("spell", typeof(string)));
                            dtCSPhotoList.Columns.Add(new DataColumn("cs_seoname", typeof(string)));
                            dtCSPhotoList.Columns.Add(new DataColumn("cs_IsWhiteCover", typeof(bool)));//是否白底封面图
							dtCSPhotoList.Columns.Add(new DataColumn("ReferPriceRange", typeof(string)));

							foreach (DataRow dr in drCSInfo)
							{
								int nCSID = (DBNull.Value == dr["cs_id"]) ? -1 : Convert.ToInt32(dr["cs_id"]);

								DataRow drCSPhoto = dtCSPhotoList.NewRow();

								drCSPhoto["cbspell"] = Convert.ToString(dr["cbspell"].ToString().Trim());
								drCSPhoto["cs_id"] = nCSID;
								drCSPhoto["cb_id"] = Convert.ToInt32(dr["cb_id"]);
								drCSPhoto["cs_name"] = Convert.ToString(dr["cs_name"].ToString().Trim());
								drCSPhoto["csspell"] = Convert.ToString(dr["csspell"].ToString().Trim());
								drCSPhoto["cslevel"] = Convert.ToString(dr["cslevel"].ToString().Trim());
								drCSPhoto["CsSaleState"] = Convert.ToString(dr["CsSaleState"].ToString().Trim());
								drCSPhoto["cs_ShowName"] = Convert.ToString(dr["cs_ShowName"].ToString().Trim());
                                drCSPhoto["spell"] = Convert.ToString(dr["spell"].ToString().Trim());
                                drCSPhoto["cs_seoname"] = Convert.ToString(dr["cs_seoname"].ToString().Trim());
                                drCSPhoto["cs_IsWhiteCover"] = false;
								drCSPhoto["ReferPriceRange"] = dr["ReferPriceRange"].ToString();
								if (dicCSPhoto.ContainsKey(nCSID))
								{
									// modified by chengl Jan.4.2010
									if (dicCSPhoto[nCSID].Attributes["ImageUrl2"].Value.ToString().Trim() != "")
									{
										// 有新封面
                                        drCSPhoto["csImageUrl"] = string.Format(dicCSPhoto[nCSID].Attributes["ImageUrl2"].Value.ToString().Trim(), "1");
                                        drCSPhoto["cs_IsWhiteCover"] = true;
									}
									else
									{
										// 没有新封面
										if (dicCSPhoto[nCSID].Attributes["ImageUrl"].Value.ToString().Trim() != "")
										{
											drCSPhoto["csImageUrl"] = string.Format(dicCSPhoto[nCSID].Attributes["ImageUrl"].Value.ToString().Trim(), "1");
										}
										else
										{
											drCSPhoto["csImageUrl"] = WebConfig.DefaultCarPic;
										}
									}

									//int imgId = Convert.ToInt32(dicCSPhoto[nCSID].Attributes["ImageId"].Value);
									//string imgUrl = dicCSPhoto[nCSID].Attributes["ImageUrl"].Value.ToString();
									//if (imgUrl.Trim().Length == 0)
									//    drCSPhoto["csImageUrl"] = WebConfig.DefaultCarPic;
									//else
									//    drCSPhoto["csImageUrl"] = new CommonFunction().GetPublishImage(1, imgUrl, imgId);
								}
								else
								{
									drCSPhoto["csImageUrl"] = WebConfig.DefaultCarPic;
								}

								dtCSPhotoList.Rows.Add(drCSPhoto);
							}

							carSerialPhotoList.Tables.Add(dtCSPhotoList);
						}
					}
				}
			}
			catch
			{
				return carSerialPhotoList;
			}

			return carSerialPhotoList;
		}

		/// <summary>
		/// 取主品牌按UV排序
		/// </summary>
		/// <returns></returns>
		public DataSet Get_Car_MasterOrderByUV()
		{
			DataSet ds = new DataSet();
			string cacheKey = "CB_Dal_Get_Car_MasterOrderByUV";
			Object get_Car_MasterOrderByUV = null;
			get_Car_MasterOrderByUV = CacheManager.GetCachedData(cacheKey);
			if (get_Car_MasterOrderByUV == null)
			{
				string sql = @"select bs.bs_Id,bs.bs_Name,bs.bs_Country,bs.bs_seoname,bs.urlspell,uv.UVCount
                                        from dbo.Car_MasterBrand bs
                                        left join dbo.Car_MasterBrand_30UV uv on bs.bs_id=uv.bs_id
                                        where bs.isState=1 order by UVCount desc";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				CacheManager.InsertCache(cacheKey, ds, WebConfig.CachedDuration);
			}
			else
			{
				ds = (DataSet)get_Car_MasterOrderByUV;
			}
			return ds;
		}

		/// <summary>
		/// 获取有品牌图片的品牌数据
		/// </summary>
		/// <returns></returns>
		public DataSet GetBrandPicData()
		{
			string sqlStr = "Select cb_Id,brandPic FROM Car_Brand WHERE IsState=1 AND (brandPic<>'' OR brandPic IS NOT NULL)";
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}
		/// <summary>
		/// 得到在销的主品牌
		/// </summary>
		/// <returns></returns>
		public DataSet GetSaleingMasterBrand()
		{
			string sqlStr = @"select distinct cmb.bs_id,cmb.bs_name,cmb.urlspell as bsurlspell from Car_Serial as cs
                                left join Car_MasterBrand_Rel as cmr on cs.cb_id = cmr.cb_id 
                                left join Car_MasterBrand as cmb on cmr.bs_id = cmb.bs_id
                                where cs.cssaleState = '在销' and cs.isstate = 1";

			try
			{
				using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr))
				{
					if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
					{
						return null;
					}
					return ds;
				}
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 得到指定数量的品牌PV排序
		/// </summary>
		/// <param name="topNum">指定的数量</param>
		/// <returns></returns>
		public DataSet GetBrandPvRanking(int topNum)
		{
			if (topNum < 1) return null;
			string sqlStr = @"with carbrandpv as (
                            select top {0} cb_id,sum(uvcount) as countnum 
                            from Car_Brand_30UV 
                            group by cb_id
                            order by countnum desc)
                            select cb.cb_id,cb.cb_name,cb.allspell 
                            from carbrandpv as cbpv
                            left join Car_Brand as cb on cb.cb_id = cbpv.cb_id";
			sqlStr = string.Format(sqlStr, topNum);

			try
			{
				using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr))
				{
					if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
					{
						return null;
					}
					return ds;
				}
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 获取所有非停销品牌信息
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllBrand()
		{
			string sqlStr = "SELECT cb_Id,cb_Name,allSpell FROM Car_Brand WHERE IsState=0 and CbSaleState<>'停销'";
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
		}

		/// <summary>
		/// 取所有品牌信息
		/// </summary>
		/// <returns></returns>
		private DataSet GetAllBrandWithStopSale()
		{
			string sqlStr = "select cb.cb_Id,cb.cb_Name,cmr.bs_Id from dbo.Car_Brand cb left join dbo.Car_MasterBrand_Rel cmr ON cb.cb_Id=cmr.cb_Id where cb.isState=0 and cmr.isState=0";
			return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
		}

		/// <summary>
		/// 获取有置换信息的城市
		/// </summary>
		public DataSet GetZhiHuanCityIdList(List<int> serialIds)
		{
			if (serialIds == null || serialIds.Count < 1)
				return null;
			string select = string.Format("SELECT distinct CityId FROM CarReplacement WHERE SerialId IN ({0})",
				string.Join(",", serialIds.ConvertAll<string>(id => id.ToString()).ToArray()));

			return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text, select);
		}
       /// <summary>
        /// 根据主品牌ID 取旗下品牌（按子品牌最大UV降序），子品牌（销售状态， 关注度降序，音序）
       /// </summary>
       /// <param name="nBSID"></param>
       /// <param name="type"></param>
       /// <returns></returns>
        public DataSet GetCarSerialSortListByBSID(int nBSID, int type)
        {
            DataSet ds = new DataSet();
			string sql = @"SELECT  cb.cb_Id, cb.cb_Name, cb.allspell AS cbspell, cs.cs_Id, cs.cs_Name,
                                    cs.cs_ShowName, cs.allSpell AS csspell, cs.cs_CarLevel AS cslevel,
                                    cs.CsSaleState, cs.spell, cmbr.bs_Id, cb.cb_Country AS Cp_Country,
                                    csi.ReferPriceRange, ( CASE cs.CsSaleState
                                                             WHEN '在销' THEN 0
                                                             WHEN '待销' THEN 0
                                                             WHEN '停销' THEN 2
                                                             ELSE 3
                                                           END ) AS salestate,
									cb.weight as cb_weight,cs.weight cs_weight
									
                            FROM    dbo.Car_Serial cs
                                    INNER JOIN dbo.Car_Brand cb ON cs.cb_Id = cb.cb_Id
                                                                   AND cb.IsState = 1
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cmbr.cb_Id = cb.cb_Id
                                    --LEFT JOIN dbo.Car_Serial_30UV cs30 ON cs30.cs_id = cs.cs_Id
                                    LEFT JOIN dbo.Car_Serial_Item csi ON cs.cs_Id = csi.cs_Id
                            WHERE   cmbr.bs_Id = @bs_id {0} 
                                    AND cs.IsState = 1
                            ORDER BY cb_weight desc,cb.spell, salestate ,cs.weight desc,cs.spell,cs.cs_id";
            if (type <= 0)
            {
                sql = string.Format(sql, "and cs.csSaleState<>'停销'");
            }
            else if (type == 2)
            {
                sql = string.Format(sql, "and cs.csSaleState='在销'");
            }
            else
            {
                sql = string.Format(sql, "");
            }
            SqlParameter[] _param = { new SqlParameter("@bs_id", SqlDbType.Int) };
            _param[0].Value = nBSID;
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(
                WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
            return ds;
        }
	}
}
