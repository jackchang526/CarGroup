using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
    public class ProducerDal
    {
        /// <summary>
        /// 获取所有厂商
        /// </summary>
        /// <returns></returns>
        public static DataSet GetProducers()
        {
            string catchkey = "GetAllProducers";
            DataSet ds = new DataSet();
            object allProducers = CacheManager.GetCachedData(catchkey);
            if (allProducers == null)
            {
                string sqlStr = "SELECT Cp_Id,Cp_ShortName,Cp_Introduction,Spell FROM Car_Producer WHERE IsState=1"; //,Cp_ShortName
                ds = SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
                CacheManager.InsertCache(catchkey, ds, 60);
            }
            else
            {
                ds = (DataSet)allProducers;
            }
            return ds;
        }
    }
}
