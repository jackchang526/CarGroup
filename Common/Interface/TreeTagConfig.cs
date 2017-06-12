using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Timers;
using System.IO;
using System.Xml;
using System.Net;
using System.Web.Caching;
using System.Text;

using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.com.bitauto.dealer;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.cn.com.baa.api;
using BitAuto.CarChannel.Common.com.bitauto.news;


namespace BitAuto.CarChannel.Common.Interface
{
    public class TreeTagConfigService
    {
        /// <summary>
        /// 得到树形配置标签页
        /// </summary>
        /// <returns></returns>
        public static XmlDocument GetTreeTagConfigXml()
        {
            string xmlPath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "config\\guideConfig.xml");
            if (!File.Exists(xmlPath))
            {
                return null;
            }

            XmlDocument treeTagConfigXml = CommonFunction.ReadXmlFromFile(xmlPath);
            
            return treeTagConfigXml;
        }
    }
}
