using System;
using System.Timers;
using System.IO;
using System.Xml;
using System.Net;
using System.Web.Caching;
using System.Text;

using BitAuto.CarChannel.Common.Cache;
using System.Collections.Generic;
using System.Data;

namespace BitAuto.CarChannel.Common.Interface
{
	public class CarMasterToSerialService
	{
		#region 私有成员
		private static object objLock;
		#endregion

		/// <summary>
		/// 静态构造函数
		/// </summary>
        static CarMasterToSerialService()
		{
			objLock = new object();

		}

        public static XmlDocument GetMasterToSerialXml()
		{
            XmlDocument masterToSerialXML = null;

            string cachekey = "MasterToSerialCache";

            object masterToSerialCache = null;
            CacheManager.GetCachedData(cachekey, out masterToSerialCache);

            if (masterToSerialCache == null)
			{
                try
                {
                //if(File.Exists(WebConfig.MasterToSerialXMLPath))
                //{
                //    lock (objLock)
                //    { 
                        masterToSerialXML = new XmlDocument();
                        masterToSerialXML.Load(WebConfig.MasterToSerialXMLPath);
                    //}
                    CacheManager.InsertCache(cachekey, masterToSerialXML,10);
                    //CacheDependency cacheDependency = new CacheDependency(WebConfig.MasterToSerialXMLPath);
                    //CacheManager.InsertCache(cachekey, masterToSerialXML, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
                catch
                {}
                //}
			}
			else
			{
                masterToSerialXML = (XmlDocument)masterToSerialCache;
			}

            return masterToSerialXML;
		}

        public static DataTable GetDTMasterBrand()
        {
            DataTable dtMasterBrand = new DataTable();

            dtMasterBrand.Columns.Add(new DataColumn("ID", typeof(string)));
            dtMasterBrand.Columns.Add(new DataColumn("Name", typeof(string)));

            XmlDocument masterToSerialDoc = GetMasterToSerialXml();

            XmlNodeList mbNodeList = masterToSerialDoc.SelectNodes("/Params/MasterBrand/Item");
            foreach (XmlNode node in mbNodeList)
            {
                DataRow dr = dtMasterBrand.NewRow();

                dr["ID"] = node.Attributes["ID"].Value;
                dr["Name"] = node.Attributes["Name"].Value;

                dtMasterBrand.Rows.Add(dr);
            }
            return dtMasterBrand;
        }

        public static DataTable GetDTSerialByMasterBrandID(int nMasterBrandID)
        {
            DataTable dtSerial = new DataTable();
            if (-1!=nMasterBrandID && -2 != nMasterBrandID)
            {
                dtSerial.Columns.Add(new DataColumn("ID", typeof(string)));
                dtSerial.Columns.Add(new DataColumn("Name", typeof(string)));

                XmlDocument masterToSerialDoc = GetMasterToSerialXml();

                XmlNodeList xnlSerials = masterToSerialDoc.SelectNodes("/Params/Serial");

                foreach (XmlNode xnSerialItems in xnlSerials)
                {
                    if (Convert.ToInt32(xnSerialItems.Attributes["BsID"].Value) == nMasterBrandID)
                    {
                        foreach (XmlNode xnSerialItem in xnSerialItems.ChildNodes)
                        {
                            DataRow drSerial = dtSerial.NewRow();

                            drSerial["ID"] = xnSerialItem.Attributes["ID"].Value;
                            drSerial["Name"] = xnSerialItem.Attributes["Name"].Value;

                            dtSerial.Rows.Add(drSerial);
                        }
                    }
                }
            }
            return dtSerial;
        }
       
	}
}
