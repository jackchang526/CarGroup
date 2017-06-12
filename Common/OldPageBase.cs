using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils;

namespace BitAuto.CarChannel.Common
{
    /// <summary>
    /// 保留老板页面及接口调用方法
    /// </summary>
    public class OldPageBase : System.Web.UI.Page
    {
        private int tickNum = 0;
        public string szConnString = WebConfig.DefaultConnectionString;
		/// <summary>
		/// 接口调用记录
		/// </summary>
		private static string logInterfaceConten = "IP:{0} URL:{1}";

		protected override void OnLoad(EventArgs e)
		{
			CommonFunction.WriteInvokeLog(string.Format(logInterfaceConten
				, BitAuto.Utils.WebUtil.GetClientIP(), this.Request.Url.ToString()));
			base.OnLoad(e);
		}

        #region

        /// <summary>
        /// 取所有厂商
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCPForInterface()
        {
            string sqlCP = "select cp_id,OldCp_Id,cp_name,Cp_Country,Spell from Car_Producer where IsState>=1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCP);
            return _ds;
        }

        /// <summary>
        /// 取所有品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCBForInterface()
        {
            string sqlCB = "select cb_id,cp_id,OldCs_Id,cb_name,Spell from Car_Brand where IsState>=1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCB);
            return _ds;
        }

        /// <summary>
        /// 取所有子品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCSForInterface()
        {
            string sqlCS = @"SELECT  a.cs_id, a.cb_id, OldCb_Id, CommonClassId, cs_name, a.Spell,
                                        ISNULL(d.cp_id, 0) AS cp_id, bat.bitautoTestURL
                                FROM    Car_Serial a
                                        INNER JOIN dbo.Car_Brand c ON c.cb_id = a.cb_id
                                        INNER JOIN dbo.Car_Producer d ON c.cp_id = d.cp_id
                                        LEFT OUTER JOIN CarImage b ON a.cs_id = b.SerialId
                                                                      AND IsDefaltImage = 1
                                        LEFT JOIN dbo.BitAutoTest bat ON a.cs_id = bat.cs_id
                                WHERE   a.IsState >= 1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCS);
            return _ds;
        }

        /// <summary>
        /// 取所有车型
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarForInterface()
        {
            string sqlCar = @"SELECT  car_id, a.cs_id, OldCar_Id, car_name, c.cb_id,
                                        ISNULL(d.cp_id, 0) AS cp_id, a.Car_SaleState
                                FROM    Car_Basic a
                                        INNER JOIN Car_Serial b ON a.cs_id = b.cs_id
                                        INNER JOIN dbo.Car_Brand c ON c.cb_id = b.cb_id
                                        INNER JOIN dbo.Car_Producer d ON c.cp_id = d.cp_id
                                WHERE   a.IsState >= 1
                                        AND b.IsState >= 1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCar);
            return _ds;
        }

        // 取子品牌排行前10(不分级别)
        protected DataSet GetAllSerialSort()
        {
            string sql = "select top 10 spr.FeignedPV,cs.cs_id,cs.cs_name  ";
            sql += " from dbo.Serial_PvRank spr ";
            sql += " left join dbo.Car_Serial cs on spr.cs_id = cs.cs_id ";
            sql += " order by FeignedPV desc ";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        ///  取国产和进口排行
        /// </summary>
        /// <param name="isImport">是否是进口</param>
        /// <returns></returns>
        protected DataSet GetAllSerialSort(int top, bool isImport)
        {
            string sql = "select top " + top.ToString() + " spr.FeignedPV,cs.cs_id,cs.cs_name,csi.Prices,cs.oldcb_id  ";
            sql += " from dbo.Serial_PvRank spr ";
            sql += " left join dbo.VCar_SerialFullInfo cs on spr.cs_id = cs.cs_id ";
            sql += " left join dbo.Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
            if (isImport)
            { sql += " where cs.Cp_Country <> '中国' and cs.isState = 1 "; }
            else
            { sql += " where cs.Cp_Country = '中国' and cs.isState = 1 "; }
            sql += " order by FeignedPV desc ";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 易车网老对比跳转页
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarIDForCompare()
        {
            string sql = " select cb.car_id,cb.car_name,cb.oldcar_id,cs.cs_id,cs.cs_Name ";
            sql += " from dbo.Car_Basic cb ";
            sql += " left join dbo.Car_Serial cs on cb.cs_id = cs.cs_id ";
            sql += " where cb.isState >= 1 and cs.isState >= 1 ";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取特定厂商信息
        /// </summary>
        public DataSet GetProducerByCpID(int cpID)
        {
            string sql = " select * from dbo.Car_Producer where isState=1 and cp_id= " + cpID.ToString();
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取所有厂商信息
        /// </summary>
        public DataSet GetProducerAll()
        {
            string sql = " select * from dbo.Car_Producer where isState=1 ";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        ///// <summary>
        ///// 取子品牌信息
        ///// </summary>
        ///// <param name="isNewID">是新库子品牌ID还是老库品牌ID</param>
        ///// <param name="csID">ID(新库子品牌ID或者老库品牌ID)</param>
        ///// <returns></returns>
        //public DataSet GetSerialInfoByIDForInterface(bool isNewID, int csID)
        //{
        //    string sql = " select cb.cb_id,cb.cb_name,cb.oldcs_id,cp.cp_id,cp.cp_name,cp.oldcp_id,ci.SiteImageId,ci.SiteImageUrl,ci.CommonClassId ";
        //    sql += " ,cs.cs_id,cs.cs_Name,cs.oldcb_id,cs.cs_Virtues,cs.cs_Defect,csi.Prices,csi.ReferPriceRange,csi.Engine_Exhaust,csi.UnderPan_Num_Type,csi.Body_Doors,cmbr.bs_id,cs.cs_CarLevel,bat.bitautoTestURL,cs.allSpell as CSAllSpell,cb.allSpell as CBAllSpell";
        //    sql += " from dbo.Car_Serial cs ";
        //    sql += " left join dbo.Car_Serial_Item csi on cs.cs_id = csi.cs_id ";
        //    sql += " inner join dbo.Car_Brand cb on cs.cb_id = cb.cb_id ";
        //    sql += " inner join dbo.Car_MasterBrand_Rel cmbr on cmbr.cb_id = cb.cb_id ";
        //    sql += " inner join dbo.Car_Producer cp on cb.cp_id = cp.cp_id ";
        //    sql += " left join dbo.BitAutoTest bat on cs.cs_id = bat.cs_id ";
        //    sql += " left join CarImage ci on cs.cs_id = ci.SerialId and ci.IsDefaltImage=1 where cs.IsState>=0 and cb.IsState>=0 and cp.IsState>=0 ";
        //    if (isNewID)
        //    {
        //        sql = sql + " and cs.cs_id = " + csID.ToString();
        //    }
        //    else
        //    {
        //        sql = sql + " and cs.oldcb_id = " + csID.ToString();
        //    }
        //    DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
        //    return ds;
        //}

        /// <summary>
        /// 取所有车型的价格
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarPriceForInterface(bool isNeedAbs, decimal absValue, int csid)
        {
            string sql = "";
            if (isNeedAbs)
            {
                sql = " select cp.car_id,abs(cp.AveragePrice-" + absValue.ToString() + ") as absValue ,cp.AveragePrice as AveragePrice,cb.car_name,cs.cs_id,cs.cs_Name,ci.SiteImageId,ci.SiteImageUrl,cs.OldCb_Id,cb.OLdCar_Id   ";
            }
            else
            {
                sql = " select cp.car_id,cp.AveragePrice as absValue,cb.car_name,cs.cs_id,cs.cs_Name,ci.SiteImageId,ci.SiteImageUrl,cs.OldCb_Id,cb.OLdCar_Id  ";
            }
            sql += " from dbo.Car_Price cp ";
            sql += " inner join dbo.Car_Basic cb on cp.car_id = cb.car_id and cb.IsState=1";
            sql += " inner join Car_Serial cs on cb.cs_id = cs.cs_id  and cs.IsState=1";
            sql += " left join CarImage ci on cs.cs_id = ci.SerialId and ci.IsDefaltImage=1 ";
            if (isNeedAbs)
            {
                sql += " where cs.cs_id <> " + csid.ToString() + " order by absValue ";
            }
            else
            {
                sql += " order by absValue ";
            }
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取子品牌时间段内特定级别排行(level为空时取所有排行)
        /// </summary>
        /// <param name="level">级别</param>
        /// <param name="currentStartTime">开始时间</param>
        /// <param name="currentEndTime">结束时间</param>
        /// <returns></returns>
        public DataSet GetSerialSortListByTimeAndCarLevel(string level, DateTime currentStartTime, DateTime currentEndTime)
        {
            string sql = " select csp.cs_id,Sum(Pv_SumNum) as Total,Rank() OVER(ORDER  BY Sum(Pv_SumNum) desc) AS Rank ";
            sql += " from  Chart_Serial_Pv csp ";
            if (level != "")
            {
                sql += "inner join Car_Serial cs on csp.cs_id = cs.cs_id and cs.IsState>=1 and cs.cs_carlevel ='" + level + "'";
            }
            sql += "where csp.createDateStr>='" + currentStartTime.ToShortDateString() + "' and csp.createDateStr<'" + currentEndTime.ToShortDateString() + "' group by csp.cs_id";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 取子品牌关注(Pv倒序)
        /// </summary>
        /// <param name="csid"></param>
        /// <returns></returns>
        public DataSet GetSerialForSerialByID(int csid)
        {

            string sql = "select a.* from dbo.VCar_SerialDA a inner join (";
            sql += "select cs_id,PCs_Id,Pv_Num from dbo.Serial_To_Serial where cs_id=" + csid.ToString();
            sql += ") b on a.cs_id= b.pcs_id order by Pv_Num desc";

            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 分别去子品牌外观,内饰,默认图(10:外观,11:内饰)
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <returns></returns>
        public DataSet GetSerialImgByIDForInterface(int csID)
        {
            string sql = " select * from dbo.CarImage ";
            sql += " where serialid = " + csID.ToString() + " and CHARINDEX(',11,',ImageProperties) > 0 ";
            sql += " order by SiteImageOrder desc ";
            sql += " select * from dbo.CarImage ";
            sql += " where serialid = " + csID.ToString() + " and CHARINDEX(',10,',ImageProperties) > 0 ";
            sql += " order by SiteImageOrder desc ";
            sql += " select * from dbo.CarImage where serialid = " + csID.ToString() + " and IsDefaltImage=1 ";
            DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
            return ds;
        }

        /// <summary>
        /// 根据类型获取发布的路径
        /// </summary>
        /// <param name="publishType">发布类型</param>
        /// <param name="imgUrl">图片路径</param>
        /// <returns></returns>
        public string GetPublishImage(int publishType, string imgUrl, int imgId)
        {
            return CommonFunction.GetPublishHashImgUrl(publishType, imgUrl, imgId);
        }

        /// <summary>
        /// 获取log图片
        /// </summary>
        /// <param name="_id">id</param>
        /// <param name="_os">类型 p厂商，b品牌</param>
        /// <param name="_ot">图片类型 b大 ，s小</param>
        /// <returns></returns>
        public string GetLogImage(string _id, string _os, string _ot)
        {
            //string filename = "default/CarImage/" + _os + "_" + _id + "_" + _ot + ".jpg";
            //if (File.Exists(Server.MapPath(filename)))
            //{
			return "http://image.bitautoimg.com/bt/car/default/CarImage/" + _os + "_" + _id + "_" + _ot + ".jpg";
            //}
            //else
            //{
            //    //return TemplateConfig.ResourceFilePath + "CarImage/default_" + _ot + ".gif";
            //    return "http://img1.bitauto.com/bt/car/default/CarImage/default_" + _ot + ".gif";
        }

        public string GetFlashChartData(DataTable dt)
        {
            string xml = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                xml += string.Format("<r><c>{0}</c><d>{1}</d></r>", dr[0], ((DateTime)dr[1]).ToString("yyyy-MM-dd"));
            }

            return string.Format("<root>{0}</root>", xml);
        }

        /// <summary>
        /// 获取图型流量图表数据
        /// </summary>
        /// <param name="car_id">车型iD</param>
        /// <param name="xString">横坐标数据</param>
        /// <param name="yString">纵坐标数据</param>
        public DataTable Get_SerialPVChartData(int car_id, int row_num)
        {
            DataTable dt = null;
            string sql = "AI_GetSerialChartPVData";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@car_id",car_id),
                new SqlParameter("@row_num", row_num)
            };
            try
            {
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.StoredProcedure, sql, paras);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch
            {
                dt = new DataTable();
            }

            return dt;
        }

        /// <summary>
        /// 获取车型价格图表图表数据
        /// </summary>
        /// <param name="car_id">车型iD</param>
        /// <param name="xString">横坐标数据</param>
        /// <param name="yString">纵坐标数据</param>
        public DataTable Get_PriceChartData(int car_id, int row_num)
        {
            DataTable dt = null;
            string sql = "AI_GetCarChartPRData";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@car_id",car_id),
                new SqlParameter("@row_num", row_num)
            };
            try
            {
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.StoredProcedure, sql, paras);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch
            {
                dt = new DataTable();
            }
            return dt;
        }

        /// <summary>
        /// 获取图型报价图表数据
        /// </summary>
        /// <param name="car_id">车型iD</param>
        /// <param name="xString">横坐标数据</param>
        /// <param name="yString">纵坐标数据</param>
        public DataTable Get_CarPVCharData(int car_id, int row_num)
        {
            DataTable dt = null;
            string sql = "AI_GetCarChartPVData";
            SqlParameter[] paras = new SqlParameter[] { 
                new SqlParameter("@car_id",car_id),
                new SqlParameter("@row_num", row_num)
            };
            try
            {
                DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.StoredProcedure, sql, paras);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }
            catch
            {
                dt = new DataTable();
            }
            return dt;
        }

        #endregion

        #region 经销商相关

        /// <summary>
        /// 取全国城市
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        /// <param name="cacheTimeHour">缓存时间(小时)</param>
        /// <returns></returns>
        protected Hashtable GetAllCity(string cacheName, int cacheTimeHour)
        {
            Hashtable ht = new Hashtable();
            DataSet ds = new DataSet();
            if (HttpContext.Current.Cache[cacheName] != null)
            {
                ht = (Hashtable)HttpContext.Current.Cache[cacheName];
            }
            else
            {
                string sql = " select city_id,city_name from [BITAUTODB].[BitAuto2006].dbo.City ";
                try
                {
                    ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (!ht.ContainsKey(ds.Tables[0].Rows[i]["city_name"].ToString()))
                            {
                                ht.Add(ds.Tables[0].Rows[i]["city_name"].ToString(), ds.Tables[0].Rows[i]["city_id"].ToString());
                            }
                        }
                    }
                    this.Cache.Insert(cacheName, ht, null, DateTime.Now.AddHours(cacheTimeHour), new TimeSpan(0));
                }
                catch
                { }
            }
            return ht;
        }

        /// <summary>
        /// 取全国省份
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        /// <param name="cacheTimeHour">缓存时间(小时)</param>
        /// <returns></returns>
        protected Hashtable GetAllProvince(string cacheName, int cacheTimeHour)
        {
            Hashtable ht = new Hashtable();
            DataSet ds = new DataSet();
            if (HttpContext.Current.Cache[cacheName] != null)
            {
                ht = (Hashtable)HttpContext.Current.Cache[cacheName];
            }
            else
            {
                string sql = " select pvc_id,pvc_name from [BITAUTODB].[BitAuto2006].dbo.Province ";
                try
                {
                    ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (!ht.ContainsKey(ds.Tables[0].Rows[i]["pvc_name"].ToString()))
                            {
                                ht.Add(ds.Tables[0].Rows[i]["pvc_name"].ToString(), ds.Tables[0].Rows[i]["pvc_id"].ToString());
                            }
                        }
                    }
                    this.Cache.Insert(cacheName, ht, null, DateTime.Now.AddHours(cacheTimeHour), new TimeSpan(0));
                }
                catch
                { }
            }
            return ht;
        }

        /// <summary>
        /// 取DataSet对象(经销商信息)
        /// </summary>
        /// <param name="cacheName">缓存名</param>
        /// <param name="csID">子品牌</param>
        /// <param name="top">取多少条</param>
        /// <param name="cacheTimeHour">缓存时间(小时)</param>
        /// <param name="cityID">城市ID</param>
        /// <param name="provinceID">省份ID</param>
        /// <returns></returns>
        protected DataSet GetDataSetByURLForCache(string cacheName, int csID, int top, int cacheTimeHour, int cityID, int provinceID)
        {
            DataSet ds = new DataSet();
            if (HttpContext.Current.Cache.Get(cacheName) != null)
            {
                ds = (System.Data.DataSet)HttpContext.Current.Cache.Get(cacheName);
            }
            else
            {
                try
                {
                    ds = this.GetDealerAndInfo(csID, top, cityID, provinceID);
                }
                catch
                { }
                HttpContext.Current.Cache.Insert(cacheName, ds, null, DateTime.Now.AddHours(cacheTimeHour), TimeSpan.Zero);
            }
            return ds;
        }

        /// <summary>
        /// 根据子品牌ID取经销商及信息
        /// </summary>
        /// <param name="csID">子品牌ID</param>
        /// <param name="top">条数</param>
        /// <param name="cityID">城市ID</param>
        /// <param name="provinceID">省份ID</param>
        /// <returns></returns>
        protected DataSet GetDealerAndInfo(int csID, int top, int cityID, int provinceID)
        {
            com.bitauto.dealer.VendorSearch vs = new BitAuto.CarChannel.Common.com.bitauto.dealer.VendorSearch();
            DataSet ds = vs.GetVendorNewsList(csID, provinceID, cityID, top, "");
            return ds;
        }

        /// <summary>
        /// 取经销商400
        /// </summary>
        /// <param name="vendorID">经销商ID</param>
        /// <returns>返回400，无则返回空</returns>
        protected string GetDealerFor400(string vendorID)
        {
            string dealer400 = "";
            Hashtable hsDealer400 = new Hashtable();
            if (HttpContext.Current.Cache.Get("DealerFor400") != null)
            {
                hsDealer400 = (System.Collections.Hashtable)HttpContext.Current.Cache.Get("DealerFor400");
            }
            else
            {
                try
                {
                    com.bitauto.das.Das dasNf = new BitAuto.CarChannel.Common.com.bitauto.das.Das();
                    DataSet ds = dasNf.GetAllNumber();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (!hsDealer400.ContainsKey(ds.Tables[0].Rows[i]["VendorID"].ToString()))
                            {
                                hsDealer400.Add(ds.Tables[0].Rows[i]["VendorID"].ToString(), ds.Tables[0].Rows[i]["User400Number"].ToString());
                            }
                        }
                        HttpContext.Current.Cache.Insert("DealerFor400", hsDealer400, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            if (hsDealer400 != null && hsDealer400.Count > 0)
            {
                if (hsDealer400.ContainsKey(vendorID))
                {
                    dealer400 = Convert.ToString(hsDealer400[vendorID]);
                }
            }
            return dealer400;
        }

        #endregion

        #region 大连信息港接口
        /// <summary>
        /// 取所有品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCBForDLXXInterface()
        {
            string sqlCB = "select cb_id,cb_name from Car_Brand where IsState>=1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCB);
            return _ds;
        }

        /// <summary>
        /// 取所有子品牌
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCSForDLXXInterface()
        {
            string sqlCS = @"SELECT  cs.cs_id, cs.cb_id, cs.cs_name, cs.cs_OtherName, cs.cs_EName,
                                    cs.cs_ShowName, cs.allSpell, cs.OldCb_Id, cs.spell,
                                    (CASE cb.cb_Country
                                       WHEN '中国' THEN '国产'
                                        ELSE '进口'
                                      END ) AS CpCountry
                            FROM Car_Serial cs
                                 LEFT JOIN dbo.CarImage ci ON cs.cs_id = ci.SerialId
                                                                 AND ci.IsDefaltImage = 1
                                    LEFT JOIN Car_brand cb ON cs.cb_id = cb.cb_id
                            WHERE cs.IsState >= 1
                                    AND cb.IsState >= 1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCS);
            return _ds;
        }

        /// <summary>
        /// 取所有车型
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllCarForDLXXInterface()
        {
            string sqlCar = "select car_id,a.cs_id,car_name,CommonClassId,SiteImageUrl,SiteImageId,OldCb_Id from Car_Basic a LEFT OUTER JOIN Car_Serial b on a.cs_id=b.cs_id  LEFT OUTER JOIN CarImage c on a.cs_id=c.SerialId AND IsDefaltImage = 1 where b.IsState>=1 and a.IsState>=1";
            DataSet _ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlCar);
            return _ds;
        }
        #endregion
    }
}
