using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using BitAuto.Utils.Data;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using System.Text;
using System.Collections;
using BitAuto.Utils;
using BitAuto.CarChannel.Model.AppApi;
using BitAuto.CarChannel.Model.AppModel;

namespace BitAuto.CarChannel.DAL
{
    public class Car_SerialDal
    {
        protected readonly string Const_Country_CN = "中国";

        public Car_SerialDal()
        { }

        /// <summary>
        /// 取所有有效子品牌 id,子品牌名
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllValidSerial()
        {
            string sql = "select cs_id,cs_name,allspell,cs_showname,CsSaleState from car_serial where isState=1 order by cs_id";
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        public Car_SerialEntity Populate_Car_SerialEntity_FromDr(DataRow row)
        {
            Car_SerialEntity Obj = new Car_SerialEntity();
            if (row != null)
            {
                Obj.Cb_Id = ((row["cb_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cb_id"]);
                Obj.Cb_Name = row["cb_name"].ToString().Trim();
                Obj.Cb_AllSpell = row["allSpell"].ToString().Trim().ToLower();
                Obj.Cp_id = ((row["cp_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cp_id"]);
                Obj.Cp_Name = row["cp_name"].ToString().Trim();
                Obj.Cs_AllSpell = row["cs_AllSpell"].ToString().Trim().ToLower();
                Obj.Cs_CarLevel = row["cs_CarLevel"].ToString().Trim();
                Obj.Cs_CarType = row["cs_CarType"].ToString().Trim();
                Obj.Cs_CreateTime = ((row["CreateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(row["CreateTime"]);
                Obj.Cs_Defect = row["cs_Defect"].ToString().Trim();
                Obj.Cs_EName = row["cs_EName"].ToString().Trim();
                Obj.Cs_Id = ((row["cs_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["cs_id"]);
                Obj.Cs_Introduction = row["cs_Introduction"].ToString().Trim();
                Obj.Cs_IsState = ((row["IsState"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["IsState"]);
                Obj.Cs_Name = row["cs_Name"].ToString().Trim();
                Obj.Cs_OldCb_Id = ((row["OldCb_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(row["OldCb_Id"]);
                Obj.Cs_OtherName = row["cs_OtherName"].ToString().Trim();
                Obj.Cs_Phone = row["cs_Phone"].ToString().Trim();
                Obj.Cs_Photo = row["cs_Photo"].ToString().Trim();
                Obj.Cs_SaleState = row["CsSaleState"].ToString().Trim();
                Obj.Cs_ShowName = row["cs_ShowName"].ToString().Trim();
                Obj.Cs_Spell = row["spell"].ToString().Trim();
                Obj.Cs_Tag = row["cs_Tag"].ToString().Trim();
                Obj.Cs_UpdateTime = ((row["UpdateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(row["UpdateTime"]);
                Obj.Cs_Url = row["cs_Url"].ToString().Trim();
                Obj.Cs_Virtues = row["cs_Virtues"].ToString().Trim();
                Obj.Cs_SeoName = row["cs_seoname"].ToString().Trim();
                Obj.MasterName = row["bs_Name"].ToString().Trim();
                Obj.MasterURL = row["urlSpell"].ToString().Trim().ToLower();
                Obj.Cp_ShortName = row["Cp_ShortName"].ToString().Trim();
            }
            else
            {
                return null;
            }
            return Obj;
        }

        public Car_SerialEntity Populate_Car_SerialEntity_FromDr(IDataReader dr)
        {
            Car_SerialEntity Obj = new Car_SerialEntity();
            Obj.Cb_Id = ((dr["cb_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cb_id"]);
            Obj.Cb_Name = dr["cb_name"].ToString().Trim();
            Obj.Cp_id = ((dr["cp_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cp_id"]);
            Obj.Cp_Name = dr["cp_name"].ToString().Trim();
            Obj.Cs_AllSpell = dr["cs_AllSpell"].ToString().Trim();
            Obj.Cs_CarLevel = dr["cs_CarLevel"].ToString().Trim();
            Obj.Cs_CarType = dr["cs_CarType"].ToString().Trim();
            Obj.Cs_CreateTime = ((dr["CreateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(dr["CreateTime"]);
            Obj.Cs_Defect = dr["cs_Defect"].ToString().Trim();
            Obj.Cs_EName = dr["cs_EName"].ToString().Trim();
            Obj.Cs_Id = ((dr["cs_id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["cs_id"]);
            Obj.Cs_Introduction = dr["cs_Introduction"].ToString().Trim();
            Obj.Cs_IsState = ((dr["IsState"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["IsState"]);
            Obj.Cs_Name = dr["cs_Name"].ToString().Trim();
            Obj.Cs_OldCb_Id = ((dr["OldCb_Id"]) == DBNull.Value) ? 0 : Convert.ToInt32(dr["OldCb_Id"]);
            Obj.Cs_OtherName = dr["cs_OtherName"].ToString().Trim();
            Obj.Cs_Phone = dr["cs_Phone"].ToString().Trim();
            Obj.Cs_Photo = dr["cs_Photo"].ToString().Trim();
            Obj.Cs_SaleState = dr["CsSaleState"].ToString().Trim();
            Obj.Cs_ShowName = dr["cs_ShowName"].ToString().Trim();
            Obj.Cs_Spell = dr["spell"].ToString().Trim();
            Obj.Cs_Tag = dr["cs_Tag"].ToString().Trim();
            Obj.Cs_UpdateTime = ((dr["UpdateTime"]) == DBNull.Value) ? Convert.ToDateTime("1900 - 1 - 1") : Convert.ToDateTime(dr["UpdateTime"]);
            Obj.Cs_Url = dr["cs_Url"].ToString().Trim();
            Obj.Cs_Virtues = dr["cs_Virtues"].ToString().Trim();
            Obj.MasterName = dr["bs_Name"].ToString();
            Obj.Cp_ShortName = dr["Cp_ShortName"].ToString().Trim();
            return Obj;
        }

        /// <summary>
        /// 取品牌下所有子品牌
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public IList<Car_SerialEntity> Get_Car_SerialByCbID(int cbID)
        {
            IList<Car_SerialEntity> Obj = new List<Car_SerialEntity>();
            string sqlStr = @"SELECT  cs.cs_id, cs.cb_Id, cs.cs_Name, cs.cs_OtherName, cs.cs_EName,
                                        cs.cs_Url, cs.cs_Phone, cs.cs_Introduction, cs.cs_Tag, cs.cs_Photo,
                                        cs.cs_Virtues, cs.cs_Defect, cs.spell, cs.CreateTime, cs.cs_CarType,
                                        cs.cs_CarLevel, cs.IsState, cs.OldCb_Id, cs.UpdateTime, cs.CsSaleState,
                                        cs.cs_ShowName, cs.allSpell AS cs_AllSpell, cb.cb_Name,
                                        ISNULL(cs.cs_seoname, cs.cs_Name) AS cs_seoname,
                                        ISNULL(cp.cp_id, 0) AS cp_id, cp.cp_name
                                FROM    dbo.Car_Serial cs
                                        LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE cs.isState = 1
                                        AND cb.isState = 1
                                        AND cp.isState = 1
                                        AND cs.cb_id = @cb_id";
            SqlParameter[] _param ={
                                      new SqlParameter("@cb_id",SqlDbType.Int)
                                  };
            _param[0].Value = cbID;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Obj.Add(Populate_Car_SerialEntity_FromDr(dr));
                    }
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }

        /// <summary>
        /// 根据子品牌ID取子品牌数据
        /// </summary>
        /// <param name="carID"></param>
        /// <returns></returns>
        public Car_SerialEntity Get_Car_SerialByCsID(int csID)
        {
            Car_SerialEntity Obj = new Car_SerialEntity();
            string sqlStr = @"SELECT cs.cs_id, cs.cb_Id, cs.cs_Name, cs.cs_OtherName, cs.cs_EName,
                                    cmb.bs_Name, cmb.urlSpell, cs.cs_Url, cs.cs_Phone, cs.cs_Introduction,
                                    cs.cs_Tag, cs.cs_Photo, cs.cs_Virtues, cs.cs_Defect, cs.spell,
                                    cs.CreateTime, cs.cs_CarType, cs.cs_CarLevel, cs.IsState, cs.OldCb_Id,
                                    cs.UpdateTime, cs.CsSaleState, cs.cs_ShowName,
                                    cs.allSpell AS cs_AllSpell,
                                    ISNULL(cs.cs_seoname, cs.cs_Name) AS cs_seoname, cb.cb_Name,
                                    cb.allSpell, ISNULL(cp.cp_id, 0) AS cp_id, cp.cp_name, cp.Cp_ShortName
                             FROM   dbo.Car_Serial cs
                                    LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmr ON cmr.cb_Id = cb.cb_id
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmb.bs_Id = cmr.bs_Id
                                    LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                             WHERE cs.isState = 1
                                    AND cb.isState = 1
                                    AND cp.isState = 1
                                    AND cs.cs_id = @cs_id";
            SqlParameter[] _param ={
                                      new SqlParameter("@cs_id",SqlDbType.Int)
                                  };
            _param[0].Value = csID;
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, _param);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Obj = Populate_Car_SerialEntity_FromDr(ds.Tables[0].Rows[0]);
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }

        /// <summary>
        /// 取所有车型
        /// </summary>
        /// <returns></returns>
        public IList<Car_SerialEntity> Get_Car_SerialAll()
        {
            IList<Car_SerialEntity> Obj = new List<Car_SerialEntity>();
            string sqlStr = @"SELECT  cs.cs_id, cs.cb_Id, cs.cs_Name, cs.cs_OtherName, cs.cs_EName,
                                        cs.cs_Url, cs.cs_Phone, cs.cs_Introduction, cs.cs_Tag, cs.cs_Photo,
                                        cs.cs_Virtues, cs.cs_Defect, cs.spell, cs.CreateTime, cs.cs_CarType,
                                        cs.cs_CarLevel, cs.IsState, cs.OldCb_Id, cs.UpdateTime, cs.CsSaleState,
                                        cs.cs_ShowName, cs.allSpell AS cs_AllSpell,
                                        ISNULL(cs.cs_seoname, cs.cs_Name) AS cs_seoname, cb.cb_Name,
                                        cb.allSpell, ISNULL(cp.cp_id, 0) AS cp_id, cp.cp_name,
                                        cb.cb_Country AS Cp_Country
                                FROM    dbo.Car_Serial cs
                                        LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                        LEFT JOIN dbo.Car_Producer cp ON cb.cp_id = cp.cp_id
                                WHERE cs.isState = 1
                                        AND cb.isState = 1
                                        AND cp.isState = 1";
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Obj.Add(Populate_Car_SerialEntity_FromDr(dr));
                    }
                }
            }
            catch
            {
                return Obj;
            }
            return Obj;
        }

        /// <summary>
        /// 根据子品牌ID取相关车型的信息
        /// </summary>
        /// <param name="csId"></param>
        /// <returns></returns>
        public DataSet GetCarExtendInfoBySerial(int csId)
        {
            DataSet ds = null;
            string sqlStr = "SELECT a.Car_Id,c.cs_Name,c.cs_ShowName,a.Car_Name,a.Car_YearType,"
                    + " b.Engine_Exhaust,b.UnderPan_TransmissionType,a.car_ReferPrice"
                    + " FROM dbo.Car_Basic a"
                    + " LEFT JOIN dbo.Car_Extend_Item b"
                    + " ON a.Car_id=b.Car_id"
                    + " LEFT JOIN dbo.Car_Serial c"
                    + " ON a.cs_Id=c.cs_id"
                    + " WHERE a.cs_id=@csId and a.isState=1 AND (a.Car_SaleState='在销' OR a.Car_SaleState='待销') "
                    + " ORDER BY b.Engine_Exhaust,a.car_ReferPrice";
            SqlParameter para = new SqlParameter("@csId", csId);
            try
            {
                ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr, para);
            }
            catch
            {

            }

            return ds;

        }

        /// <summary>
        /// 判断当前车款是否是国产车并拼接出车的全称返回输出
        /// </summary>
        /// <param name="nCSID"></param>
        /// <param name="strCarName"></param>
        /// <returns></returns>
        public bool IsDomesticCar(int nCSID)
        {
            string sql = @"SELECT  cb.cb_Country AS country
                            FROM    car_serial cs
                                    LEFT JOIN car_brand cb ON cb.cb_id = cs.cb_id
                            WHERE   cs.cs_id = @CsId";
            SqlParameter[] parameters = { new SqlParameter("@CsId", SqlDbType.Int) };
            parameters[0].Value = nCSID;

            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, parameters);

            bool isDomestic = false;
            if (null != _ds && null != _ds.Tables[0] && _ds.Tables[0].Rows.Count > 0)
            {
                if (Const_Country_CN == Convert.ToString(_ds.Tables[0].Rows[0]["country"]))
                {
                    isDomestic = true;
                }
            }
            return isDomestic;
        }

        /// <summary>
        /// 取彩虹条信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetCSRainbowsListByID(int nCSID, string strCSRBItemIDs)
        {
            // modified by chengl Aug.28.2009
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.AppendLine("select RI.RainbowItemID, RI.[Name] as RName,RE.URL,RE.UrlTime,RE.IsShow from RainbowItem RI ");
            sbSQL.AppendFormat(" left join RainbowEdit RE on RE.RainbowItemID = RI.RainbowItemID and RE.csid={0}", nCSID);
            sbSQL.AppendFormat(" where RI.RainbowItemID in ({0})  ", strCSRBItemIDs);

            //StringBuilder sbSQL = new StringBuilder();
            //sbSQL.AppendLine("select RI.RainbowItemID, RI.RName, CS.cs_id, CS.cs_name, CS.cs_showname, CS.allspell, CS.Url,	CS.UrlTime ");
            //sbSQL.AppendFormat("from(Select	RainbowItemID,[Name] AS RName From RainbowItem Where RainbowItemID in ({0})) RI left join( ", strCSRBItemIDs);
            //sbSQL.AppendLine("SELECT CS.cs_id, CS.cs_name,CS.cs_showname,CS.allspell,RE.RainbowItemID,RE.Url,RE.UrlTime FROM RainbowEdit RE, RainbowItem RI, Car_Serial	CS ");
            //sbSQL.AppendFormat("WHERE RE.csid = CS.cs_id and RE.RainbowItemID = RI.RainbowItemID and RE.isshow = 1 and CS.isState = 1 and CS.cs_id = {0}) CS on ri.RainbowItemID = CS.RainbowItemID", nCSID);

            //sbSQL.AppendLine("select cp.cp_country as country, CS.cs_id, CS.cs_name, CS.cs_showname, CS.allspell, RI.RainbowItemID, RI.[Name] as RName, RE.Url, RE.UrlTime ");
            //sbSQL.AppendLine("from RainbowEdit RE, RainbowItem RI, Car_Serial CS,	car_producer cp, car_brand cb ");
            //sbSQL.AppendFormat("where RE.RainbowItemID = RI.RainbowItemID and RE.isshow = 1 and RE.csid = CS.cs_id and cb.cb_id = cs.cb_id and cp.cp_id = cb.cp_id and CS.isState = 1 and CS.cs_id = {0}", nCSID);

            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sbSQL.ToString());
            return _ds;
        }

        /// <summary>
        /// 根据彩虹条ID取有彩虹条的子品牌ID、url
        /// </summary>
        /// <param name="rainbowItemID"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialRainbowItemByRainbowItemID(int rainbowItemID)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            string sql = " select csid,url from dbo.RainbowEdit where RainbowItemID = @rainbowItemID ";
            SqlParameter[] _param ={
                                      new SqlParameter("@rainbowItemID",SqlDbType.Int)
                                  };
            _param[0].Value = rainbowItemID;
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql, _param);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int csid = int.Parse(dr["csid"].ToString());
                    string url = dr["url"].ToString().Trim();
                    if (!dic.ContainsKey(csid))
                    { dic.Add(csid, url); }
                }
            }
            return dic;
        }

        /// <summary>
        /// [已无引用]取彩虹条信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, DataSet> GetCSRainbowsList(int nRainbowDisplayStatus)
        {
            StringBuilder sbSQL = new StringBuilder();

            Dictionary<string, DataSet> dicRainbows = new Dictionary<string, DataSet>();

            //国产
            //sbSQL.AppendFormat("SELECT * FROM ((SELECT RainbowItemID as RItemID, [name] as RBName FROM rainbowitem where RainbowItemID in ({0})) RS", DomesticCarRBItemIDs);
            //sbSQL.AppendFormat(" right join (SELECT cp.cp_country as country, CS.cs_id, CS.cs_name, CS.cs_showname, CS.allspell, RI.RainbowItemID as RIItemID, RI.[Name] as RName,  RE.UpdateTime,");
            //sbSQL.AppendFormat(" RE.Url, RE.UrlTime FROM RainbowEdit RE, RainbowItem RI, Car_Serial CS, car_producer cp, car_brand cb where RE.RainbowItemID = RI.RainbowItemID and RE.isshow = 1 ");
            //sbSQL.AppendFormat(" and RE.csid = CS.cs_id and cb.cb_id = cs.cb_id and cp.cp_id = cb.cp_id and CS.isState = 1 and cp.cp_country = '{0}') RT on RS.RItemID = RT.RIItemID) ", Const_Country_CN);
            //sbSQL.AppendFormat(" where RItemID in ({0}) order by updatetime", DomesticCarRBItemIDs);

            string DomesticCarRBItemIDs = WebConfig.DomesticCarRBItemIDs;
            sbSQL.Append("select cp.cp_country as country, CS.cs_id, CS.cs_name, CS.cs_showname, CS.allspell, RI.RainbowItemID, RI.[Name] as RName,  RE.UpdateTime, ");
            sbSQL.Append(" RE.Url, RE.UrlTime from RainbowEdit RE, RainbowItem RI, Car_Serial CS, car_producer cp, car_brand cb where ");
            sbSQL.Append(" RE.RainbowItemID = RI.RainbowItemID and RE.isshow = 1 and RE.csid = CS.cs_id and ");
            sbSQL.AppendFormat(" cb.cb_id = cs.cb_id and cp.cp_id = cb.cp_id and CS.isState = 1 and cp.cp_country = '{0}' and ", Const_Country_CN);
            sbSQL.AppendFormat(" RI.RainbowItemID in ({0}) ", DomesticCarRBItemIDs);
            sbSQL.Append(" order by updatetime desc, cs_id");

            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sbSQL.ToString());
            if (_ds != null && _ds.Tables[0] != null && _ds.Tables[0].Rows.Count > 0)
            {
                dicRainbows.Add("国产车", GetRainbowList(_ds.Tables[0]));
            }

            sbSQL.Length = 0;

            //进口
            string NDomesticCarRBItemIDs = WebConfig.NDomesticCarRBItemIDs;
            sbSQL.Append("select cp.cp_country as country, CS.cs_id, CS.cs_name, CS.cs_showname, CS.allspell, RI.RainbowItemID, RI.[Name] as RName,  RE.UpdateTime, ");
            sbSQL.Append(" RE.Url, RE.UrlTime from RainbowEdit RE, RainbowItem RI, Car_Serial CS, car_producer cp, car_brand cb where ");
            sbSQL.Append(" RE.RainbowItemID = RI.RainbowItemID and RE.isshow = 1 and RE.csid = CS.cs_id and ");
            sbSQL.AppendFormat(" cb.cb_id = cs.cb_id and cp.cp_id = cb.cp_id and CS.isState = 1 and cp.cp_country <> '{0}' and ", Const_Country_CN);
            sbSQL.AppendFormat(" RI.RainbowItemID in ({0}) ", NDomesticCarRBItemIDs);
            sbSQL.Append(" order by updatetime desc, cs_id");

            DataSet _dsN = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sbSQL.ToString());
            if (_dsN != null && _dsN.Tables[0] != null && _dsN.Tables[0].Rows.Count > 0)
            {
                dicRainbows.Add("进口车", GetRainbowList(_dsN.Tables[0]));
            }

            return dicRainbows;
        }

        private DataSet GetRainbowList(DataTable dtSource)
        {
            DataSet ds = new DataSet();

            ArrayList arrIDs = new ArrayList();

            foreach (DataRow dr in dtSource.Rows)
            {
                string strCSID = dr["cs_id"].ToString();

                if (!string.IsNullOrEmpty(strCSID))
                {
                    if (!arrIDs.Contains(strCSID))
                    {
                        arrIDs.Add(strCSID);
                    }
                }
            }

            foreach (object obj in arrIDs)
            {
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(new DataColumn("country", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("cs_id", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("cs_name", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("cs_showname", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("allspell", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("RainbowItemID", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("RName", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("Url", typeof(string)));
                dtTemp.Columns.Add(new DataColumn("UrlTime", typeof(string)));

                string strCSID = obj.ToString();
                DataRow[] drs = dtSource.Select(string.Format(" cs_id = {0} ", strCSID));
                foreach (DataRow dr in drs)
                {
                    dtTemp.ImportRow(dr);
                }

                ds.Tables.Add(dtTemp);
            }

            return ds;
        }

        public DataSet GetCSRBItemByIDs(string IDs)
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.AppendFormat("select RainbowItemID as RID, [name] as RName from rainbowitem where RainbowItemID in ({0}) ", IDs);

            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sbSQL.ToString());

            return _ds;
        }

        //public DataSet GetSerialCityHotCompare(int cityID, int csID)
        //{
        //    StringBuilder sbSQL = new StringBuilder();
        //    sbSQL.Append(" select comparecsid,sum(count) as compareCount from ( ");
        //    sbSQL.AppendFormat(" select CsID,CompareCsID,Count from dbo.StCompareCity{0} ", cityID.ToString());
        //    sbSQL.AppendFormat(" where csid = {0} ", csID.ToString());
        //    sbSQL.Append(" union ");
        //    sbSQL.AppendFormat(" select comparecsid as csID,csID as comparecsid ,Count from dbo.StCompareCity{0} ", cityID.ToString());
        //    sbSQL.AppendFormat(" where comparecsid = {0} ", csID.ToString());
        //    sbSQL.Append(") t1 ");
        //    sbSQL.Append(" group by comparecsid ");
        //    sbSQL.Append(" order by compareCount desc ");
        //    DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.PvConnectionString, CommandType.Text, sbSQL.ToString());
        //    return _ds;
        //}

        /// <summary>
        /// 得到不包含车型的子品牌列表
        /// </summary>
        /// <returns></returns>
        public List<int> GetIsNoContainsCarTypeOfSerialList()
        {
            string sql = "select cs_id from car_serial where isstate=1 and cs_id not in (select cs_id from dbo.Car_Basic where isState=1 and Car_SaleState<>'停销' group by cs_id)";
            try
            {
                DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);

                if (_ds == null || _ds.Tables.Count < 1 || _ds.Tables[0].Rows.Count < 1)
                {
                    return null;
                }

                List<int> SerialList = new List<int>();
                //添加子品牌ID
                foreach (DataRow dr in _ds.Tables[0].Rows)
                {
                    SerialList.Add(BitAuto.Utils.ConvertHelper.GetInteger(dr["cs_id"].ToString()));
                }
                return SerialList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取子品牌中所有在销车型车身颜色的数据
        /// 此处改为以生产状态 不以销售状态 取数据 modified by chengl May.5.2011
        /// 在加最新年款条件 修改自 20110925 anh
        /// </summary>
        /// <param name="serialId">子品牌ID</param>
        /// <param name="isAllProduceState">生产状态</param>
        /// <returns></returns>
        public DataSet GetCarsColorBySerialId(int serialId, bool isAllProduceState)
        {
            string sqlStr = "SELECT a.Car_Id,a.Car_YearType,b.Pvalue AS CarColor FROM Car_relation a "
                + " INNER JOIN CarDataBase b ON a.Car_Id=b.CarId AND b.paramid=598"
                + " WHERE a.Cs_Id=@Cs_Id AND a.IsState=0 ";
            if (!isAllProduceState)
            { sqlStr += " AND a.car_ProduceState=92"; }
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr, new SqlParameter("@Cs_Id", serialId));
        }
        /// <summary>
        /// 获取在产车型所有车身颜色
        /// </summary>
        /// <param name="serialId">子品牌Id</param>
        /// <returns></returns>
        //        public DataSet GetProduceCarsColorBySerialId(int serialId)
        //        {
        //            string sql = @"SELECT a.Car_Id,a.Car_YearType,b.Pvalue AS CarColor FROM Car_relation a  
        //                 INNER JOIN CarDataBase b ON a.Car_Id=b.CarId AND b.paramid=598 
        //                WHERE a.Cs_Id=@serialId AND a.IsState=0 AND a.car_ProduceState=92                   
        //                order by car_yeartype desc";

        //            SqlParameter[] _params = { new SqlParameter("@serialId", SqlDbType.Int) };
        //            _params[0].Value = serialId;
        //            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _params);
        //        }

        /// <summary>
        /// AddDate:      2015-7-31
        /// Description:  子品牌综述页的焦点区的色块的提取逻辑需要调整下，提取车款的 由 “在产” 改为 “在销”
        /// Author:       zhangfeng
        /// 获取在销车型所有车身颜色
        /// </summary>
        /// <param name="serialId">子品牌Id</param>
        /// <returns></returns>
        public DataSet GetProduceCarsColorBySerialId(int serialId)
        {
            string sql = @"SELECT a.Car_Id,a.Car_YearType,b.Pvalue AS CarColor FROM Car_relation a  
                 INNER JOIN CarDataBase b ON a.Car_Id=b.CarId AND b.paramid=598 
                WHERE a.Cs_Id=@serialId AND a.IsState=0 AND a.car_ProduceState=92                         
                order by car_yeartype desc";
            SqlParameter[] _params = { new SqlParameter("@serialId", SqlDbType.Int) };
            _params[0].Value = serialId;
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, _params);
        }

        /// <summary>
        /// 所有级别(包括概念车)所有销售状态子品牌数据(2010北京车展接口)
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialUpLevelInfo()
        {
            string sqlStr = @"SELECT  cmb.bs_Id, cmb.bs_Name, cmb.bs_EName, cb.cb_id, cb.cb_name, cs.cs_id,
                                    cs.csShowName, cs.csname, cs.oldcbid, cs.prototypeCar, cs.CsPurpose,
                                    cmb.spell AS bs_spell, cb.spell AS cb_spell, cs.spell AS cs_spell,
                                    cl2.classvalue AS cbcountry, ( CASE cb.cb_country
                                                                     WHEN 90 THEN '国产'
                                                                     ELSE '进口'
                                                                   END ) AS CpCountry,
                                    cl.classvalue AS csLevel, cmb.urlspell,
                                    bat1.bitautoTestURL AS csTestURL, cs.showSpell,
                                    vcfba.Seria_disPriceNew, vcfba.minReferPrice, vcfba.maxReferPrice,
                                    cb.allspell AS cbAllSpell, cs.allSpell AS csAllSpell, cs.CsSaleState,
                                    cl3.classvalue AS bs_Country, cmb.bs_seoname, cb.cb_seoname,
                                    cs.cs_seoname, ISNULL(cp.cp_id, 0) AS cp_id
                            FROM    dbo.Car_Serial cs
                                    LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                    LEFT JOIN dbo.Car_producer cp ON cb.cp_id = cp.cp_id
                                    LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                                                              AND cmbr.IsState = 0
                                    LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                    LEFT JOIN class cl ON cs.carlevel = cl.classid
                                    LEFT JOIN class cl2 ON cb.cb_country = cl2.classid
                                    LEFT JOIN class cl3 ON cmb.bs_Country = cl3.classid
                                    LEFT JOIN dbo.BitAutoTest bat1 ON cs.cs_id = bat1.cs_id
                                    LEFT JOIN dbo.VCar_ForBitAuto vcfba ON cs.cs_id = vcfba.cs_id
                            WHERE   cs.isState = 0
                                    AND cb.isState = 0
                                    AND cmbr.isState = 0
                            ORDER BY bs_spell, cmb.bs_Id, cb_spell, cb.cb_id, showSpell";
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
        }
        /// <summary>
        /// 得到所有PV子品牌信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetLefTreePvSerialInfo()
        {
            string sqlString = @"SELECT  cs.cs_id, cs.cs_Name, CASE cs.CsSaleState
                                                                WHEN '在销' THEN 0
                                                                WHEN '待销' THEN 1
                                                                WHEN '停销' THEN 2
                                                              END CsSaleState, cs.allspell AS csallspell,
                                        cs.cs_ShowName AS csShowName, cb.cb_id, cb.cb_Name,
                                        cb.allspell AS cballspell, cmb.bs_id, cmb.bs_Name, cmb.Spell,
                                        cmb.urlspell AS bsallspell, PATINDEX(cb.cb_Country, '中国') AS country
                                FROM    Serial_PvRank AS spr
                                        LEFT JOIN Car_Serial AS cs ON cs.cs_id = spr.cs_id
                                        LEFT JOIN Car_Brand AS cb ON cb.cb_id = cs.cb_id
                                        LEFT JOIN Car_MasterBrand_Rel AS cmbr ON cmbr.cb_id = cb.cb_id
                                        LEFT JOIN Car_MasterBrand AS cmb ON cmb.bs_id = cmbr.bs_id
                                        LEFT JOIN Car_MasterBrand_30UV AS cmbuv ON cmbuv.bs_id = cmb.bs_id
                                WHERE   cs.cs_Carlevel != '概念车'
                                        AND cs.IsState = 1
                                ORDER BY LEFT(cmb.Spell, 1) ASC, cmbuv.UVCount DESC";

            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlString))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                        return null;

                    return ds;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到销量子品牌信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetLeftTreeSaleSerialInfo()
        {
            /*string sqlString = @"select cs.cs_id,cs.csName as cs_Name,case cs.CsSaleState when '在销' then 0 when '待销' then 1 when '停销' then 2 end CsSaleState
								,cs.allspell as csallspell,cs.csShowName as csShowName
								,cb.cb_id,cb.cb_Name,cb.allspell as cballspell,cmb.bs_id
								,cmb.bs_Name,cmb.Spell,cmb.urlspell as bsallspell,left(cmb.spell,1) as singleSpell
								,case cp.cpCountry when 90 then 1 else 0 end as country from CarProduceAndSellData as cpsd
								left join Car_Serial as cs on cpsd.csid = cs.cs_id
								left join Car_Brand as cb on cs.cb_id = cb.cb_id 
								left join Car_MasterBrand_Rel as cmr on cmr.cb_id = cb.cb_id and cmr.IsState=0
								left join Car_MasterBrand as cmb on cmb.bs_id = cmr.bs_id
								left join Car_Producer as cp on cp.cp_Id = cb.cp_Id
								order by left(cmb.spell,1)";*/

            string sqlString = @"select vstb.cs_id,vstb.csName as cs_Name,case vstb.CsSaleState when '在销' then 0 when '待销' then 1 when '停销' then 2 end CsSaleState
                                ,vstb.csAllSpell,vstb.csShowName,vstb.cb_id,vstb.cb_Name,vstb.cbAllSpell,vstb.bs_id
                                ,vstb.bs_Name,vstb.bsSpell as Spell,vstb.bsAllSpell,left(vstb.bsSpell,1) as singleSpell
                                ,case vstb.cbCountry when 90 then 1 else 0 end as country
                                from CarProduceAndSellData as cps
                                left join dbo.VCar_SerialToBrandToMasterBrand  as vstb with(noEXPAND) on vstb.cs_id = cps.csid 
                                order by left(vstb.bsSpell,1)";
            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlString))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
                        return null;

                    return ds;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到最新的子品牌列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetNewsSerialList()
        {
            string sql = @"select top 12 cs.cs_id,cs.csname,cs.csshowname,cs.allspell,cdb.pvalue
                            from dbo.Car_relation car
                            left join car_serial cs on car.cs_id=cs.cs_id
                            left join dbo.CarDataBase cdb on car.car_id=cdb.CarId and cdb.paramid=385 
                            where car.isState=0 and cs.isState=0
                            group by cs.cs_id,cs.csname,cs.csshowname,cs.allspell,cdb.pvalue
                            order by cdb.pvalue desc";

            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql))
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
        //private DataSet GetRainbowListByParams(DataTable dtSource, string filter, DataTable dtRainbowItemIDs, string filterRainbowIDs)
        //{
        //    DataSet dsTemp = new DataSet();

        //    DataTable dtCSTemp = CloneTable(dtSource, filter);

        //    string strTempID = string.Empty;

        //    foreach (DataRow dr in dtCSTemp.Rows)
        //    {
        //        string strCSID = dr["cs_id"].ToString();
        //        if (strTempID != strCSID)
        //        {
        //            strTempID = strCSID;

        //            DataTable dtTemp = CloneTable(dtCSTemp, string.Format("cs_id = {0}", strCSID));
        //            DataTable dtCopied = CopyTable(dtTemp, string.Format("RainbowItemID in ({0})", filterRainbowIDs), dtRainbowItemIDs);

        //            dsTemp.Tables.Add(dtCopied);
        //        }
        //    }

        //    return dsTemp;
        //}

        //private DataTable CloneTable(DataTable dtSource, string filter)
        //{
        //    DataTable dtTarget = dtSource.Clone();

        //    DataRow[] drTemp = dtSource.Select(filter);
        //    foreach(DataRow dr in drTemp)
        //    {
        //        dtTarget.ImportRow(dr);
        //    }
        //    return dtTarget;
        //}

        //private DataTable CopyTable(DataTable dtSource, string filterRainbowIDs, DataTable dtRainbowItemIDs)
        //{ 
        //    DataTable dtTemp = new DataTable();
        //    dtTemp.Columns.Add(new DataColumn("country", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("cs_id", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("cs_name", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("cs_showname", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("allspell", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("RainbowItemID", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("RName", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("Url", typeof(string)));
        //    dtTemp.Columns.Add(new DataColumn("UrlTime", typeof(string)));

        //    ArrayList arrIDs = new ArrayList();

        //    DataRow[] drTemps = dtSource.Select(filterRainbowIDs);
        //    foreach (DataRow dr in drTemps)
        //    {
        //        dtTemp.ImportRow(dr);

        //        arrIDs.Add(dr["RainbowItemID"].ToString());
        //    }

        //    foreach(DataRow drs in dtRainbowItemIDs.Rows)
        //    {
        //        string strRainbowItemID = drs["RainbowItemID"].ToString();
        //        foreach(object obj in arrIDs)
        //        {
        //            string strItemID = Convert.ToString(obj);
        //            if (strRainbowItemID == strItemID)
        //            {
        //                dtRainbowItemIDs.Rows.Remove(drs);
        //            }
        //        }
        //    }

        //    foreach (DataRow drs in dtRainbowItemIDs.Rows)
        //    {
        //        DataRow drNew = dtTemp.NewRow();

        //        if (dtTemp.Rows.Count > 0)
        //        {
        //            drNew["country"]        = dtTemp.Rows[0]["country"];
        //            drNew["cs_id"]          = dtTemp.Rows[0]["cs_id"];
        //            drNew["cs_name"]        = dtTemp.Rows[0]["cs_name"];
        //            drNew["cs_showname"]    = dtTemp.Rows[0]["cs_showname"];
        //            drNew["allspell"]       = dtTemp.Rows[0]["allspell"];

        //            drNew["RainbowItemID"]  = drs["RainbowItemID"];
        //            drNew["RName"]          = drs["Name"];
        //        }

        //        dtTemp.Rows.Add(drNew);
        //    }

        //    return dtTemp;
        //}


        /// <summary>
        /// 取所有子品牌颜色RGB值
        /// </summary>
        /// <param name="csId">车系ID</param>
        /// <param name="type">类型0:车身有色值，1：内饰，2 车身没有色值，3：车身有没有色值都取出来</param>
        /// <returns></returns>
        public DataSet GetAllSerialColorRGB(int csId, int type)
        {
            if (type < 0 || type > 3)
            {
                return null;
            }
            DataSet ds = new DataSet();
            string sqlStr = " select autoID,cs_id,colorName,colorRGB,[type] from dbo.Car_SerialColor WHERE cs_id=" + csId + " AND {0} order by cs_id,colorRGB";
            if (type == 0)
            {
                sqlStr = string.Format(sqlStr, " [type]=0 ");
            }
            else if (type == 1)
            {
                sqlStr = string.Format(sqlStr, " [type]=1 ");
            }
            else if (type == 2)
            {
                sqlStr = string.Format(sqlStr, " [type]=2 ");
            }
            else
            {
                sqlStr = string.Format(sqlStr, " ([type]=2 OR [type]=0) ");
            }
            ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
            return ds;
        }

        /// <summary>
        /// 取所有子品牌颜色RGB值
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialColorRGB(int type)
        {
            DataSet ds = new DataSet();
            string sqlStr = " select autoID,cs_id,colorName,colorRGB from dbo.Car_SerialColor {0} order by cs_id,colorRGB";
            if (type >= 0)
            {
                // 有颜色类型条件 0:子品牌车身颜色 1:内饰颜色
                sqlStr = string.Format(sqlStr, " where type=" + type.ToString());
            }
            else
            {
                // 没有条件取全部颜色(子品牌车身颜色,内饰颜色)
                sqlStr = string.Format(sqlStr, "");
            }
            ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
            return ds;
        }

        /// <summary>
        /// 得到子品牌补贴的数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetSubsidiesDataSet()
        {
            //补贴功能临时去掉 modified by chengl Oct.24.2013
            return null;
            //            string sqlStr = @"select distinct car.cs_id,cdb.pvalue
            //                            from dbo.Car_relation car
            //                            left join car_serial cs on car.cs_id=cs.cs_id and cs.IsState = 0
            //                            left join dbo.CarDataBase cdb on car.car_id=cdb.carid and cdb.paramid=853
            //                            where car.isState=0 and cs.isState=0 and cdb.pvalue ='3000元'";

            //            try
            //            {
            //                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr))
            //                {
            //                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;
            //                    return ds;
            //                }
            //            }
            //            catch
            //            {
            //                return null;
            //            }
        }
        /// <summary>
        /// 根据条件返回相应的数据集
        /// </summary>
        /// <param name="type">  * 0:包含在销，待销，停销，待查；旗下是否有车型不限
        /// * 1:包含在销，待销，停销，待查；非概念车；旗下是否有车型不限；
        /// * 2:包含在销，待销；非概念车；旗下是否有车型不限；
        /// * 3:包含在销，待销；非概念车；旗下必须有车型；车型可以是不限销焦状态；
        /// * 4:包含在销，待销；非概念车；旗下必须有车型；车型必须是在销、待销；</param>
        /// * 5:包含：在销，待销，停销，待查，非概念车，旗下必须有车款；
        /// * 6:包含：包含在销；非概念车；旗下是否有车型不限； 
        /// * 7:包含在销，待销，停销；非概念车；旗下是否有车型不限；
        /// <returns></returns>
        public DataSet GetIsConditionsDataSet(int type)
        {
            string sqlFormater = @" {0}SELECT cs.cs_id, cs.cs_Name, cs.cs_ShowName, LEFT(cs.spell, 1) AS csspell,
                                            cs.csSaleState, cs.allSpell AS csallspell, cb.cb_id,
                                            ( CASE cb.cb_Country
                                                WHEN '中国' THEN ''
                                                ELSE '进口'
                                              END ) + cb.cb_Name AS cb_name, LEFT(cb.spell, 1) AS cbspell,
                                            cb.allSpell AS cballspell, cm.bs_id, cm.bs_Name,
                                            LEFT(cm.spell, 1) AS bsspell, cm.urlspell AS bsallspell,
                                            ( CASE cb.cb_Country
                                                WHEN '中国' THEN 0
                                                ELSE 1
                                              END ) AS CpCountry, ( CASE cs.CsSaleState
                                                                      WHEN '在销' THEN 1
                                                                      WHEN '待销' THEN 2
                                                                      ELSE 3
                                                                    END ) AS CsSaleStateSort
                                     FROM   Car_Serial AS cs
		                                    {1} 
                                            INNER JOIN Car_Brand AS cb ON cb.cb_id = cs.cb_id
                                            INNER JOIN Car_MasterBrand_Rel AS cmr ON cmr.cb_id = cb.cb_id
                                            LEFT JOIN Car_MasterBrand AS cm ON cm.bs_id = cmr.bs_id
                                            LEFT JOIN Car_MasterBrand_30UV mbuv ON cm.bs_id = mbuv.bs_id
                                     WHERE  cs.IsState = 1 {2} 
                                     ORDER BY bsspell, mbuv.UVCount DESC, cm.bs_Id, CpCountry, cb.spell, cb.cb_id,
                                            CsSaleStateSort, cs.spell";

            string sqlStr = "";

            if (type == 0)
            {
                sqlStr = string.Format(sqlFormater, "", "", "");
            }
            else if (type == 1)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车'");
            }
            else if (type == 2)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'停销'");
            }
            else if (type == 3)
            {
                string withString = @"with T(countnumber,cs_id) as (
	                                    select count(car_id),cs_id from dbo.Car_Basic where isstate = 1 group by cs_id
                                    )";
                sqlStr = string.Format(sqlFormater, withString, "inner join T on cs.cs_id=t.cs_id", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'停销' and cs.cs_id = T.cs_id and T.countnumber>0");
            }
            else if (type == 4)
            {
                string withString = @"with T(countnumber,cs_id) as (
	                                    select count(car_id),cs_id from dbo.Car_Basic where isstate = 1 and car_salestate <> '停销' group by cs_id
                                    )";
                sqlStr = string.Format(sqlFormater, withString, "inner join T on cs.cs_id=t.cs_id", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'停销' and cs.cs_id = T.cs_id and T.countnumber>0");
            }
            else if (type == 5)
            {
                string withString = @"with T(countnumber,cs_id) as (select count(car_id),cs_id from dbo.Car_Basic where isstate = 1 group by cs_id)";
                sqlStr = string.Format(sqlFormater, withString, "inner join T on cs.cs_id=t.cs_id", "and cs.cs_CarLevel<>'概念车' and cs.cs_id = T.cs_id and T.countnumber>0");
            }
            else if (type == 6)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState='在销'");
            }
            else if (type == 7)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'待查'");
            }
            else if (type == 8)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车' and (cs.csSaleState='在销' OR cs.csSaleState='停销')");
            }
            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;
                    return ds;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据条件返回相应的数据集
        /// </summary>
        /// <param name="type"> * 0:包含在销，待销，停销，待查；旗下是否有车型不限
        /// * 1:包含在销，待销，停销，待查；非概念车；旗下是否有车型不限；
        /// * 2:包含在销，待销；非概念车；旗下是否有车型不限；
        /// * 3:包含在销，待销；非概念车；旗下必须有车型；车型可以是不限销焦状态；
        /// * 4:包含在销，待销；非概念车；旗下必须有车型；车型必须是在销、待销；</param>
        /// * 5:包含：在销，待销，停销，待查，非概念车，旗下必须有车款；
        /// * 6:包含：包含在销；非概念车；旗下是否有车型不限； 
        /// <returns></returns>
        public DataSet GetProduceIsConditionsDataSet(int type)
        {
            string sqlFormater = @"{0}SELECT  cs.cs_id, cs.cs_Name, cs.cs_ShowName, LEFT(cs.spell, 1) AS csspell,
                                            cs.csSaleState, cs.allSpell AS csallspell, cb.cb_id,
                                            ( CASE cb.cb_Country
                                                WHEN '中国' THEN ''
                                                ELSE '进口'
                                              END ) + cb.cb_Name AS cb_name, LEFT(cb.spell, 1) AS cbspell,
                                            cb.allSpell AS cballspell, ISNULL(cp.cp_id, 0) AS cp_id,
                                            cp.cp_ShortName, LEFT(cp.spell, 1) AS cpspell,
                                            ( CASE cs.CsSaleState
                                                WHEN '在销' THEN 1
                                                WHEN '待销' THEN 2
                                                ELSE 3
                                              END ) AS CsSaleStateSort
                                    FROM    Car_Serial AS cs
		                                    {1} 
                                            INNER JOIN Car_Brand AS cb ON cb.cb_id = cs.cb_id
                                            INNER JOIN Car_producer cp ON cb.cp_id = cp.cp_id
                                            LEFT JOIN Car_Producer_30UV cprod ON cp.cp_id = cprod.cp_id
                                    WHERE   cs.IsState = 1 {2} 
                                    ORDER BY cpspell, cprod.UVCount DESC, cb.spell, cb.cb_id, CsSaleStateSort,
                                            cs.spell";

            string sqlStr = "";

            if (type == 0)
            {
                sqlStr = string.Format(sqlFormater, "", "", "");
            }
            else if (type == 1)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车'");
            }
            else if (type == 2)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'停销'");
            }
            else if (type == 3)
            {
                string withString = @"with T(countnumber,cs_id) as (
	                                    select count(car_id),cs_id from dbo.Car_Basic where isstate = 1 group by cs_id
                                    )";
                sqlStr = string.Format(sqlFormater, withString, "inner join T on cs.cs_id=t.cs_id", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'停销' and cs.cs_id = T.cs_id and T.countnumber>0");
            }
            else if (type == 4)
            {
                string withString = @"with T(countnumber,cs_id) as (
	                                    select count(car_id),cs_id from dbo.Car_Basic where isstate = 1 and car_salestate <> '停销' group by cs_id
                                    )";
                sqlStr = string.Format(sqlFormater, withString, "inner join T on cs.cs_id=t.cs_id", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState<>'停销' and cs.cs_id = T.cs_id and T.countnumber>0");
            }
            else if (type == 5)
            {
                string withString = @"with T(countnumber,cs_id) as (select count(car_id),cs_id from dbo.Car_Basic where isstate = 1 group by cs_id)";
                sqlStr = string.Format(sqlFormater, withString, "inner join T on cs.cs_id=t.cs_id", "and cs.cs_CarLevel<>'概念车'  and cs.cs_id = T.cs_id and T.countnumber>0");
            }
            else if (type == 6)
            {
                sqlStr = string.Format(sqlFormater, "", "", "and cs.cs_CarLevel<>'概念车' and cs.csSaleState='在销'");
            }

            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;
                    return ds;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 取所有车型的上市时间排序
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialMarkData()
        {
            DataSet ds = new DataSet();
            string sql = @"select cr.cs_id,cr.car_id--,cr.car_name,
                                ,convert(int,isnull(cdb1.Pvalue,0)) as mdyear 
								,convert(int,isnull(cdb2.Pvalue,0)) as mdmonth 
								,convert(int,isnull(cdb3.Pvalue,0)) as mdday  
                                from dbo.Car_relation cr   
                                left join dbo.CarDataBase cdb1 on cdb1.carid = cr.car_id and cdb1.ParamId=385   
                                left join dbo.CarDataBase cdb2 on cdb2.carid = cr.car_id and cdb2.ParamId=384   
                                left join dbo.CarDataBase cdb3 on cdb3.carid = cr.car_id and cdb3.ParamId=383   
                                left join dbo.Car_Serial cs on cr.cs_id = cs.cs_id   
                                where cr.IsState=0 and cs.IsState=0
                                order by mdyear desc ,mdmonth desc ,mdday desc";
            ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取所有待销子品牌 车型 上市时间
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllWaitSaleSerialMarkData()
        {
            string sql = @" SELECT    cr.cs_id,cr.car_id--,cr.car_name,
				,CONVERT(INT,ISNULL(cdb1.Pvalue,0)) AS mdyear,CONVERT(INT,ISNULL(cdb2.Pvalue,0)) AS mdmonth,CONVERT(INT,ISNULL(cdb3.Pvalue,0)) AS mdday
	  FROM      dbo.Car_relation cr
				LEFT JOIN dbo.CarDataBase cdb1 ON cdb1.carid = cr.car_id
												  AND cdb1.ParamId = 385
				LEFT JOIN dbo.CarDataBase cdb2 ON cdb2.carid = cr.car_id
												  AND cdb2.ParamId = 384
				LEFT JOIN dbo.CarDataBase cdb3 ON cdb3.carid = cr.car_id
												  AND cdb3.ParamId = 383
				LEFT JOIN dbo.Car_Serial cs ON cr.cs_id = cs.cs_id
	  WHERE     cr.IsState = 0
				AND cs.IsState = 0
				AND cdb1.Pvalue <> ''
				AND CONVERT(INT,ISNULL(cdb1.Pvalue,0)) >= 1900
				AND cs.CsSaleState = '待销'
	  ORDER BY  mdyear ASC,mdmonth ASC,mdday ASC";
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql);
        }

        /// <summary>
        /// 得到所有子品牌的官方报价
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialOfficePrice(bool isAllSale)
        {
            string sql = @"select car.cs_id,Round(min(car_referprice),2) 
                            as minprice,Round(max(car_referprice),2) 
                            as maxprice 
                            from car_relation car
                            left join car_serial cs on car.cs_id=cs.cs_id
                            where car.isState=0   and cs.isState=0 ";
            if (!isAllSale)
            { sql += " and car.car_SaleState<>96 "; }
            sql += " group by car.cs_id";
            try
            {
                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql))
                {
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;
                    return ds;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子品牌排列，通过城市ID
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataSet GetSerialUVOrderByCityId(int cityId)
        {

            return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.StoredProcedure, "dbo.GetCityUVCount", new SqlParameter("@cityId", cityId));
            //改为存储过程计算 anh 2012/04/25            
            //                string sqlString = @"select sum(UVCount) as totalcount,csID
            //                                from dbo.StatisticSerialPVUVCity 
            //                                {0}
            //                                group by csID
            //                                order by totalcount desc";
            //            //如果存在城市
            //            if (cityId > 0)
            //            {
            //                sqlString = string.Format(sqlString, "where cityId =" + cityId);
            //            }
            //            else
            //            {
            //                sqlString = string.Format(sqlString, "");
            //            }

            //            try
            //            {
            //                using (DataSet ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlString))
            //                {
            //                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;
            //                    return ds;
            //                }
            //            }
            //            catch
            //            {
            //                return null;
            //            }
        }

        /// <summary>
        /// 获取所有非停销的子品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerial()
        {
            string sqlsStr = "SELECT cs_Id,cb_Id,csName,csShowName FROM Car_Serial WHERE IsState=0 AND CsSaleState<>'停销'";
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlsStr);
        }

        /// <summary>
        /// 取所有子品牌信息，等DAL上线后用 
        /// </summary>
        /// <returns></returns>
        private DataSet GetAllSerialWithStopSale()
        {
            string sqlsStr = "SELECT cs_Id,cb_Id,csName,csShowName,carlevel FROM Car_Serial WHERE IsState=0";
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlsStr);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息
        /// </summary>
        public DataSet GetCarReplacementInfo(List<int> serialIds, int cityId, int cityParentId, int top)
        {
            SqlParameter[] param = new SqlParameter[4] {
                new SqlParameter("@serialIds", string.Join(",", serialIds.ConvertAll<string>(id => id.ToString()).ToArray()))
                , new SqlParameter("@cityId", cityId)
                , new SqlParameter("@provinceId", cityParentId)
            , new SqlParameter("@top", top)};

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure,
                "dbo.GetCarReplacementBySerialIds"
                , param);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息
        /// </summary>
        public DataSet GetCarReplacementInfo(int serialId, int cityId, int cityParentId)
        {
            SqlParameter[] param = new SqlParameter[3] {
                new SqlParameter("@serialId", serialId)
                , new SqlParameter("@cityId", cityId)
                , new SqlParameter("@provinceId", cityParentId) };

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure,
                "dbo.GetCarReplacementBySerialId"
                , param);
        }
        /// <summary>
        /// 获取子品牌置换优惠信息包含备注
        /// </summary>
        public DataSet GetCarReplacementInfoAndMemo(List<int> serialIds, int cityId, int cityParentId, int top)
        {
            SqlParameter[] param = new SqlParameter[4] {
                new SqlParameter("@serialIds", string.Join(",", serialIds.ConvertAll<string>(id => id.ToString()).ToArray()))
                , new SqlParameter("@cityId", cityId)
                , new SqlParameter("@provinceId", cityParentId)
            , new SqlParameter("@top", top)};

            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.StoredProcedure,
                "dbo.GetCarReplacementAndMemoBySerialIds"
                , param);
        }
        /// <summary>
        /// 获取有置换数据的子品牌列表
        /// </summary>
        public DataSet GetZhiHuanList()
        {
            return SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString, CommandType.Text
                , "SELECT distinct SerialId from CarReplacement");
        }
        /// <summary>
        /// 获取CNCAP数据 C-NCAP星级（后台ID：649）→E-NCAP星级（后台ID：637）→IIHS总评价（后台ID：957）→NHTSA星级（后台ID：638）
        /// </summary>
        /// <returns></returns>
        public DataSet GetCNCAPData()
        {
            string sqlStr = @"SELECT  Cs_Id ,
										CarYear ,
										ParamId,
										Pvalue
								FROM    Car_SerialYearDataBase
								WHERE   paramid IN(649,637,957,638)";
            return SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sqlStr);
        }
        /// <summary>
        /// 获取在销车款信息 根据子品牌ID
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public DataSet GetSaleCarExhaustBySerialId(int serialId)
        {
            DataSet ds = new DataSet();
            string sql = @"SELECT DISTINCT
									cei.Engine_Exhaust
							FROM    dbo.Car_Basic car
									LEFT JOIN dbo.Car_Extend_Item cei ON car.car_id = cei.car_id
									LEFT JOIN Car_serial cs ON car.cs_id = cs.cs_id
							WHERE   car.isState = 1
									AND cs.isState = 1
									AND car.cs_id = @csID
									AND car.Car_SaleState = '在销'
									AND car.Car_YearType > 0
							ORDER BY Engine_Exhaust";
            SqlParameter[] _param ={
                                      new SqlParameter("@csID",SqlDbType.Int)
                                  };
            _param[0].Value = serialId;
            ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
            return ds;
        }

        /// <summary>
        /// 取所有子品牌近30天按级别分组排名
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialUVRAngeByLevel()
        {
            string sql = @"select t1.csID,t1.UVCount,cs.cs_CarLevel
						,row_number()over(partition by cs.cs_CarLevel 
						order by t1.UVCount desc)as rank
						,cs.cs_name,cs.cs_showname,cs.cs_seoname,cs.allSpell,cs.csSalestate
						from (
						select csID,sum(UVCount) as UVCount
						from StatisticSerialPVUVCity
						group by csID
						) t1
						left join Car_Serial cs on t1.csid=cs.cs_id
						where cs.IsState=1";
            DataSet ds = new DataSet();
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql);
            return ds;
        }
        /// <summary>
        /// 取某级别的按UV倒序的子品牌数据
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public DataSet GetLevelSerialDataByUV(string level)
        {
            DataSet ds = new DataSet();
            string sql = @"select cs.cs_id,cs.cs_name,cs.allspell as csAllSpell,cs.cs_ShowName,cs.cs_CarLevel,cs.cs_seoname
                        ,cs30.uvcount,cs.CsSaleState
                        from car_serial cs
                        left join dbo.Car_Serial_30UV cs30 on cs.cs_id=cs30.cs_id
                        where cs.isState=1 and cs.cs_CarLevel = @level and cs.CsSaleState<>'停销'
                        order by cs30.uvcount desc";

            SqlParameter[] _param ={
                                      new SqlParameter("@level",SqlDbType.VarChar)
                                  };
            _param[0].Value = level;
            ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param);
            return ds;
        }

        /// <summary>
        /// 按销售状态，取某级别的按UV倒序的子品牌数据
        /// </summary>
        /// <param name="levelId">级别</param>
        /// <param name="saleState">销售状态,如果取全部状态，可为空</param>
        /// <returns></returns>
        public DataSet GetLevelSerialDataByUVAndSaleState(string level, List<string> saleState)
        {
            DataSet ds = new DataSet();
            string sql = @"select cs.cs_id,cs.cs_name,cs.allspell as csAllSpell,cs.cs_ShowName,cs.cs_CarLevel,cs.cs_seoname
                        ,cs30.uvcount,cs.CsSaleState
                        from car_serial cs
                        left join dbo.Car_Serial_30UV cs30 on cs.cs_id=cs30.cs_id
                        where cs.isState=1 and cs.cs_CarLevel = @level {0}
                        order by cs30.uvcount desc";

            List<SqlParameter> _param = new List<SqlParameter>() {
                new SqlParameter("@level",SqlDbType.VarChar)
            };
            _param[0].Value = level;
            if (saleState != null && saleState.Count > 0)
            {
                sql = string.Format(sql, " and cs.CsSaleState in (@saleState)");
                _param.Add(new SqlParameter("@saleState", SqlDbType.NVarChar));
                _param[1].Value = string.Join(",", saleState.ToArray());
            }
            else
            {
                sql = string.Format(sql, string.Empty);
            }

            ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _param.ToArray());
            return ds;
        }

        /// <summary>
        /// 获取 所有 子品牌 UV
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSerialUV()
        {
            string sql = @"SELECT cs_id,UVCount FROM [dbo].[Car_Serial_30UV]";
            return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
        }
        /// <summary>
        /// 获取车型数据 for pk页面
        /// </summary>
        /// <param name="serialId1"></param>
        /// <param name="serialId2"></param>
        /// <returns></returns>
        public DataSet GetSerialInfoForPK(int serialId1, int serialId2)
        {
            string sql = @"SELECT cs_Id,cs_ShowName,allSpell FROM dbo.Car_Serial WHERE cs_Id=@serialId1 OR cs_id=@serialId2";
            SqlParameter[] _params = new SqlParameter[]{
                new SqlParameter("@serialId1", SqlDbType.Int)  ,
                new SqlParameter("@serialId2", SqlDbType.Int)
            };
            _params[0].Value = serialId1;
            _params[1].Value = serialId2;
            return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, _params);
        }

        /// <summary>
        /// 根据一级车身级别,返回车系最近30天uv排行;
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetAllSerialUvRank30Days()
        {
            Dictionary<int, int> uvDic = null;
            string sql = @"SELECT  uv.cs_id AS csID,
									row_number() OVER ( PARTITION BY cs.cs_CarLevel ORDER BY uv.UVCount DESC )
									AS rank
							FROM    dbo.Car_Serial_30UV uv
									LEFT JOIN Car_Serial cs ON uv.cs_id = cs.cs_Id";
            DataSet ds = SqlHelper.ExecuteDataset(
               WebConfig.DefaultConnectionString, CommandType.Text, sql);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                return uvDic;

            uvDic = new Dictionary<int, int>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var csid = ConvertHelper.GetInteger(dr["csid"].ToString());
                var rank = ConvertHelper.GetInteger(dr["rank"].ToString());
                if (!uvDic.ContainsKey(csid))
                {
                    uvDic.Add(csid, rank);
                }
            }

            return uvDic;
        }

        /// <summary>
        /// 子品牌PV (UV)
        /// </summary>
        /// <returns></returns>
        public Hashtable GetAllSerialPV()
        {
            var htPV = new Hashtable();

            //            const string sql = @"SELECT  uv.cs_id AS csID, uv.UVCount, cs.cs_CarLevel,
            //									row_number() OVER ( PARTITION BY cs.cs_CarLevel ORDER BY uv.UVCount DESC )
            //									AS rank
            //							FROM    dbo.Car_Serial_30UV uv
            //									LEFT JOIN Car_Serial cs ON uv.cs_id = cs.cs_Id";

            const string sql = @"            SELECT  uv.cs_id AS csID, uv.UVCount,
        CASE WHEN cs.cs_CarLevel = 'SUV' THEN CASE cs.ModelLevelSecond
                                                WHEN 1 THEN '小型SUV'
                                                WHEN 2 THEN '紧凑型SUV'
                                                WHEN 3 THEN '中型SUV'
                                                WHEN 4 THEN '中大型SUV'
                                                WHEN 5 THEN '大型SUV'
                                                ELSE 'SUV'
                                              END
             ELSE cs.cs_CarLevel
        END carlevel,
        row_number() OVER ( PARTITION BY CASE WHEN cs.cs_CarLevel = 'SUV'
                                              THEN CASE cs.ModelLevelSecond
                                                     WHEN 1 THEN '小型SUV'
                                                     WHEN 2 THEN '紧凑型SUV'
                                                     WHEN 3 THEN '中型SUV'
                                                     WHEN 4 THEN '中大型SUV'
                                                     WHEN 5 THEN '大型SUV'
                                                   END
                                              ELSE cs.cs_CarLevel
                                         END ORDER BY uv.UVCount DESC ) AS rank
FROM    dbo.Car_Serial_30UV uv
        LEFT JOIN Car_Serial cs ON uv.cs_id = cs.cs_Id";



            var ds = SqlHelper.ExecuteDataset(
                WebConfig.DefaultConnectionString, CommandType.Text, sql);
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return htPV;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var csid = int.Parse(dr["csid"].ToString());
                var rank = int.Parse(dr["rank"].ToString());
                var carlevel = dr["carlevel"].ToString();
                if (!htPV.ContainsKey(csid))
                {
                    htPV.Add(csid, string.Format("{0}第{1}名", carlevel, rank));
                }
            }
            return htPV;
        }

        /// <summary>
        /// 取全国子品牌级别排行
        /// </summary>
        /// <param name="csID"></param>
        /// <returns></returns>
        public string GetSerialTotalPV(int csID)
        {
            string totalPV = "";
            var htSerialPV = GetAllSerialPV();
            if (htSerialPV == null || htSerialPV.Count <= 0) return totalPV;
            if (htSerialPV.ContainsKey(csID))
            {
                totalPV = Convert.ToString(htSerialPV[csID]);
            }
            return totalPV;
        }

        /// <summary>
        /// 根据子品牌ID、城市ID 取易车商城 包销 平行进口
        /// </summary>
        /// <returns></returns>
        public DataSet GetCarParallelAndSell(int csid, int cityid)
        {
            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@CsId", SqlDbType.Int) { Value = csid },
                new SqlParameter("@CityId", SqlDbType.Int) { Value = cityid }
            };
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.CarDataUpdateConnectionString
                , CommandType.StoredProcedure, "SP_ParallelAndSell_Get", param);
            return ds;
        }

        /// <summary>
        /// 取所有子品牌口碑数、口碑综合评分、细项评分
        /// add by chengl Dec.9.2015
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCsKoubei()
        {
            DataSet ds = new DataSet();
            string sql = "select * from Car_CsKouBeiBaseInfo";
            ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 获取热门车（口碑对比页使用）
        /// </summary>
        /// <returns></returns>
        public DataSet GetHotCarTop10()
        {
            string sql = "SELECT top 10 cs.cs_id,cs.cs_ShowName,allSpell FROM[dbo].[Car_Serial] cs " +
                         "left join[dbo].[Car_Serial_30UV] cs30 on cs.cs_Id = cs30.cs_id order by cs30.UVCount desc ";
            var ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString
                , CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 查询车系选配包信息
        /// </summary>
        /// <param name="serialId">车系id</param>
        /// <returns></returns>
        public DataSet GetSerialOptionalPackage(int serialId)
        {
            string sql = @"SELECT autoid,cs_id,year,packagename,packageprice,packagedescription
                            FROM Car_SerialPackage WITH(NOLOCK)
                            WHERE cs_id = @csid

                            SELECT carId, SerialPackageId
                            FROM carJoinserialpackage WITH(NOLOCK)
                            WHERE SerialPackageId in (
                                SELECT autoid
                                FROM Car_SerialPackage WITH(NOLOCK)
                                WHERE cs_id = @csid
                            )";
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@csid",serialId)
            };
            DataSet ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, param);
            return ds;
        }
        /// <summary>
        /// 获取车型选配包数据
        /// </summary>
        /// <param name="serialId"></param>
        /// <returns></returns>
        public List<CarSerialPackageEntity> GetCarSerialPackageListBySerialId(int serialId)
        {
            var result = new List<CarSerialPackageEntity>();
            using (DataSet ds = GetSerialOptionalPackage(serialId))
            {
                if (ds != null && ds.Tables.Count > 1)
                {
                    CarSerialPackageEntity package;
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        package = new CarSerialPackageEntity();
                        package.AutoId = TypeParse.StrToInt(item["autoid"], 0);
                        package.SerialId = TypeParse.StrToInt(item["cs_id"], 0);
                        package.Year = TypeParse.StrToInt(item["year"], 0);
                        package.PackageName = item["packagename"] == DBNull.Value ? "" : item["packagename"].ToString();
                        package.PackageDescription = item["packagedescription"] == DBNull.Value ? "" : item["packagedescription"].ToString();
                        package.PackagePrice = TypeParse.StrToFloat(item["packageprice"], 0);
                        package.CarIds = new List<int>();
                        foreach (DataRow car in ds.Tables[1].Rows)
                        {
                            if (TypeParse.StrToInt(car["SerialPackageId"], 0) == package.AutoId)
                            {
                                package.CarIds.Add(TypeParse.StrToInt(car["carId"], 0));
                            }
                        }
                        result.Add(package);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 获取车款属性
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public CarStyleInfoEntity GetStyleInfoById(int styleId)
        {
            var sql = @"
                    SELECT TOP 1 [Car_Name],[Car_YearType],[Cs_Id],
				   carSerialName=
				   (
						SELECT TOP 1 [cs_Name] FROM [dbo].[Car_Serial] WITH(NOLOCK) WHERE IsState=1 AND  [cs_Id]=[dbo].[Car_Basic].Cs_Id
				   )
					FROM [dbo].[Car_Basic] WITH(NOLOCK)
					WHERE [IsState]=1
					AND [Car_Id]=@Id
                    ";
            SqlParameter[] parms =
            {
                new SqlParameter("@Id", styleId )
            };
            var dataTable = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, parms).Tables[0];
            CarStyleInfoEntity csi = null;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                csi = new CarStyleInfoEntity();
                csi.Name = dataTable.Rows[0]["Car_Name"] + "";
                csi.Year = TypeParse.StrToInt(dataTable.Rows[0]["Car_YearType"] + "", 0);
                csi.ModelId = TypeParse.StrToInt(dataTable.Rows[0]["Cs_Id"] + "", 0);
                csi.CarSerialName = dataTable.Rows[0]["carSerialName"] + "";
            }
            return csi;
        }



        /// <summary>
        /// 	根据车款ID获取相关信息
        /// </summary>
        /// <param name="Id">车款ID</param>
        /// <returns></returns>
        public Dictionary<string, string> GetCarStylePropertyById(int id)
        {
            var sql = @"
                            SELECT [ParamId] AS PropertyId
                                  ,[Pvalue] AS value
                              FROM [dbo].[CarDataBase] WITH(NOLOCK) 
                              WHERE [ParamId]  in(423,785,895,665,986,578,782,883) 
                              AND [CarId]=@id


                             SELECT [cb_country] AS CountryId 
                             FROM [dbo].[Car_Brand] WITH(NOLOCK) 
                             WHERE [cb_Id]=
                             (
	                            SELECT [cb_Id] FROM [dbo].[Car_Serial]  WITH(NOLOCK) WHERE [cs_Id]=
	                            (
		                            SELECT [Cs_Id] FROM [dbo].[Car_relation] WITH(NOLOCK) WHERE [Car_Id]=@id
	                            )
                             )
                            ";
            var commandParameters = new SqlParameter[]
            {
                new SqlParameter("@id", id)
            };
            Dictionary<string, string> result = new Dictionary<string, string>();
            var ds = SqlHelper.ExecuteDataset(WebConfig.AutoStorageConnectionString, CommandType.Text, sql, commandParameters);
            var dtp = ds.Tables[0];
            foreach (DataRow item in dtp.Rows)
            {
                var pro = TypeParse.StrToInt(item["PropertyId"], 0);
                switch (pro)
                {
                    case 423:
                        result.Add("engine", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 665:
                        result.Add("seatNum", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 785:
                        result.Add("exhaustforfloat", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 895:
                        result.Add("traveltax", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 986:
                        result.Add("taxrelief", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 578:
                        result.Add("fuelType", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 782:
                        result.Add("zongHeYouHao", item["value"] == null ? "" : item["value"].ToString());
                        break;
                    case 883:
                        result.Add("mustMileageconstant", item["value"] == null ? "" : item["value"].ToString());
                        break;
                }
            }
            var dtc = ds.Tables[1];
            if (dtc.Rows.Count > 0)
            {
                result.Add("isGuoChan", TypeParse.StrToInt(dtc.Rows[0]["CountryId"], 0) == 90 ? "True" : "False");
            }

            return result;
        }

        /// <summary>
        /// 获取车系级别，如果有二级级别取二级级别
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllSerialLevelsWithSecondLevel()
        { 
            var sql = @"SELECT cs.cs_Id,cs.cs_Name,CASE WHEN cs.cs_CarLevel = 'SUV' THEN CASE cs.ModelLevelSecond
                                                                        WHEN 1 THEN '小型SUV'
                                                                        WHEN 2 THEN '紧凑型SUV'
                                                                        WHEN 3 THEN '中型SUV'
                                                                        WHEN 4 THEN '中大型SUV'
                                                                        WHEN 5 THEN '大型SUV'
                                                                        ELSE 'SUV'
                                                                      END
                                     ELSE cs.cs_CarLevel
                                END carlevel
                          from Car_Serial cs
                          where  isstate=1";
            var ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            Dictionary<int, string> result = new Dictionary<int, string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
            {
                var dtp = ds.Tables[0];
                foreach (DataRow item in dtp.Rows)
                {
                    int csId = TypeParse.StrToInt(item["cs_Id"], 0);
                    if (csId != 0 && !result.ContainsKey(csId))
                    {
                        result.Add(csId, item["carlevel"] == DBNull.Value ? "" : item["carlevel"].ToString());
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取车型国别和主品牌id
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public SerialCountryEntity GetSerialCountryById(int serialId)
        {
            var sql = @"SELECT [cs_Id], cb.cb_Country,cm.bs_Id 
	                          FROM [Car_Serial] cs  inner join [Car_Brand] cb on cs.cb_Id=cb.cb_Id inner join [Car_MasterBrand_Rel] cm on cb.cb_Id=cm.cb_Id
	                          where cs.cs_id=@Id
                    ";
            SqlParameter[] parms =
            {
                new SqlParameter("@Id", serialId )
            };
            var dataTable = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql, parms).Tables[0];
            SerialCountryEntity serialCountry = new SerialCountryEntity();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {

                serialCountry.SerialID = TypeParse.StrToInt(dataTable.Rows[0]["cs_Id"], 0);
                serialCountry.MasterID = TypeParse.StrToInt(dataTable.Rows[0]["bs_Id"], 0);
                serialCountry.Country = dataTable.Rows[0]["cb_Country"].ToString();
            }
            return serialCountry;
        }
    }
}
