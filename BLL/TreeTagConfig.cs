using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Web;
using System.Web.Caching;

using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.BLL
{
    public class TreeTagConfig
    {
        /// <summary>
        /// 得到标签配置列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, TreeTagConfigEntity> GetNoCacheTagConfigList()
        {
            XmlDocument xmlDoc = TreeTagConfigService.GetTreeTagConfigXml();
            if (xmlDoc == null)
            {
                return null;
            }

            XmlNodeList xNodeList = xmlDoc.SelectNodes("root/tags/tag");

            if (xNodeList == null 
                || xNodeList.Count < 1)
            {
                return null;
            }

            Dictionary<string, TreeTagConfigEntity> treeTagConfig = new Dictionary<string, TreeTagConfigEntity>();

            foreach (XmlElement xElem in xNodeList)
            {
                if (treeTagConfig.ContainsKey(xElem.GetAttribute("type")))
                {
                    continue;
                }

                TreeTagConfigEntity ttcEntity = new TreeTagConfigEntity();
                XmlNode xNode = xElem.SelectSingleNode("main");

                ttcEntity.MainUrl = AddChildElementInUrl(xElem.SelectSingleNode("main"));
                ttcEntity.SearchUrl = AddChildElementInUrl(xElem.SelectSingleNode("search"));
                ttcEntity.MasterBrandUrl = AddChildElementInUrl(xElem.SelectSingleNode("MasterBrand"));
                ttcEntity.BrandUrl = AddChildElementInUrl(xElem.SelectSingleNode("Brand"));
                ttcEntity.SerialUrl = AddChildElementInUrl(xElem.SelectSingleNode("Serial"));
                ttcEntity.ErrorUrl = AddChildElementInUrl(xElem.SelectSingleNode("error")); 

                ttcEntity.TagType = xElem.GetAttribute("type");
                ttcEntity.TagName = xElem.GetAttribute("Name");

                treeTagConfig.Add(ttcEntity.TagType, ttcEntity);
            }

            return treeTagConfig;
        }
        /// <summary>
        /// 得到标签配置列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, TreeTagConfigEntity> GetTagConfigList()
        {
            string cachekey = "TreeTagConfig";

            Dictionary<string, TreeTagConfigEntity> tagTree = (Dictionary<string, TreeTagConfigEntity>)CacheManager.GetCachedData(cachekey);

            if (tagTree == null || tagTree.Count < 1)
            {
                string xmlPath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "config\\guideConfig.xml");
                if (File.Exists(xmlPath))
                {
                    tagTree = GetNoCacheTagConfigList();
                    if (tagTree == null || tagTree.Count < 1) return null;
                    CacheDependency cacheDependency = new CacheDependency(xmlPath);
                    CacheManager.InsertCache(cachekey, tagTree, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return tagTree;
        }
        /// <summary>
        /// 得到搜索参数列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetNoCacheSearchParamList()
        {
            XmlDocument xmlDoc = TreeTagConfigService.GetTreeTagConfigXml();
            if (xmlDoc == null)
            {
                return null;
            }

            XmlNodeList xNodeList = xmlDoc.SelectNodes("root/register/r[requestType='search']");

            if (xNodeList == null
                || xNodeList.Count < 1)
            {
                return null;
            }

            List<string> paramList = new List<string>();

            foreach (XmlElement xElem in xNodeList)
            {
                paramList.Add(xElem.GetAttribute("value"));
            }

            return paramList;
        }
        /// <summary>
        /// 得到搜索参数列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSearchParamList()
        {
            string cachekey = "TreeTagConfigSearchList";
            List<string> paramList = (List<string>)CacheManager.GetCachedData(cachekey);

            if (paramList == null || paramList.Count < 1)
            {
                string xmlPath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "config\\guideConfig.xml");
                if (File.Exists(xmlPath))
                {
                    paramList = GetNoCacheSearchParamList();
                    if (paramList == null || paramList.Count < 1) return null;
                    CacheDependency cacheDependency = new CacheDependency(xmlPath);
                    CacheManager.InsertCache(cachekey, paramList, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            return paramList;
        }
        /// <summary>
        /// 添加子元素进列表
        /// </summary>
        /// <param name="xNode"></param>
        /// <param name="elementUrlList"></param>
        private Dictionary<string, string> AddChildElementInUrl(XmlNode xNode)
        {
            if (xNode == null || !xNode.HasChildNodes)
            {
                return null;
            }

            Dictionary<string, string> urlList = new Dictionary<string, string>();

            foreach (XmlElement xElem in xNode.ChildNodes)
            {
                if (urlList.ContainsKey(xElem.Name))
                {
                    continue;
                }
                urlList.Add(xElem.Name, xElem.InnerText);
            }

            return urlList;
        }
    }
}
