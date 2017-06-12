using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using BCC=BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.BLL
{
    public class ExhibitionAlbum
    {
        /// <summary>
        /// 得到2010车展的图片XML列表
        /// </summary>
        /// <returns></returns>
        public virtual XmlDocument getBeijing2010AlbumRelationData(int exhibitionId)
        {
            string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\beijing2010\\interfaceConfig.xml";

            if (!File.Exists(sPath))
            {
                return null;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sPath);

                if (xmlDoc == null)
                {
                    return null;
                }

                XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode("root/params/item[@id='" + exhibitionId + "']/total");
                if (xNode == null) return null;

                string sRequestUrl = xNode.GetAttribute("url") + "?" + xNode.GetAttribute("param") + "&" + new Random().Next(10000, 100000).ToString();
                string content = Common.CommonFunction.GetContentByUrl(sRequestUrl);
                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);

                return xmlDoc;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到2010车展的图片XML列表
        /// </summary>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public virtual XmlDocument getBeijing2010AlbumRelationData(int exhibitionId,int expireTime)
        {
            string cacheKey = "Exhibition_" + exhibitionId.ToString() + "_Album";

            object xmlDoc = BCC.Cache.CacheManager.GetCachedData(cacheKey);

            if (xmlDoc != null)
            {
                return (XmlDocument)xmlDoc;
            }

            XmlDocument albumXmlDoc = getBeijing2010AlbumRelationData(exhibitionId);
            if (albumXmlDoc == null) 
            {
                return null;
            }
            BCC.Cache.CacheManager.InsertCache(cacheKey, albumXmlDoc, expireTime);
            return albumXmlDoc;
        }
        /// <summary>
        /// 得到2010北京车展热点美女排列
        /// </summary>
        /// <returns></returns>
        public virtual XmlDocument getBeijing2010AlbumHotData(int exhibitionId,int Count)
        {
             string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\beijing2010\\interfaceConfig.xml";

            if (!File.Exists(sPath))
            {
                return null;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sPath);

                if (xmlDoc == null)
                {
                    return null;
                }

                XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode("root/params/item[@id='" + exhibitionId + "']/hot");
                if (xNode == null) return null;

                string sRequestUrl = xNode.GetAttribute("url") + "?" + xNode.GetAttribute("param") + "&rownum=" + Count.ToString() + "&" + new Random().Next(10000, 100000).ToString();
                string content = Common.CommonFunction.GetContentByUrl(sRequestUrl);
                if (string.IsNullOrEmpty(content))
                {
                    return null;
                }
               
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);

                return xmlDoc;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 得到上海车展滚图列表
        /// </summary>
        /// <param name="exhibitionId">车展ID</param>
        /// <param name="dt">创建时间</param>
        /// <returns></returns>
        public virtual XmlDocument getShangHai2010AlbumNewCarGunTuList(int exhibitionId, DateTime sdt,DateTime edt)
        {
            string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\beijing2010\\interfaceConfig.xml";

            if (!File.Exists(sPath))
            {
                return null;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sPath);

                if (xmlDoc == null)
                {
                    return null;
                }

                XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode("root/params/item[@id='" + exhibitionId + "']/newcar");
                if (xNode == null) return null;
                string requestUrl = xNode.GetAttribute("url");
                string startTime = sdt.ToString("yyyy-MM-dd hh:mm:ss");
                string endTime = edt.ToString("yyyy-MM-dd hh:mm:ss");
                string sRequestUrl = string.Format(requestUrl, startTime, endTime);

                xmlDoc = new XmlDocument();
                xmlDoc.Load(sRequestUrl);

                return xmlDoc;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 从小到大排
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static int OrderXmlElement(XmlElement pre, XmlElement last)
        {
            int ret = 0;
            DateTime pv1 = string.IsNullOrEmpty(pre.GetAttribute("UpdateTime")) ? DateTime.Now : Convert.ToDateTime(pre.GetAttribute("UpdateTime"));
            DateTime pv2 = string.IsNullOrEmpty(last.GetAttribute("UpdateTime")) ? DateTime.Now : Convert.ToDateTime(last.GetAttribute("UpdateTime"));
            if (pv1.CompareTo(pv2) > 0)
                ret = -1;
            else if (pv1.CompareTo(pv2) < 0)
                ret = 1;

            return ret;

		}

		#region 通用车展方法

		/// <summary>
		/// 通用车展方法
		/// </summary>
		/// <param name="exhibitionId">车展ID</param>
		/// <returns></returns>
		public virtual XmlDocument GetCommonAlbumRelationData(int exhibitionId)
		{
			string cacheKey = "ExhibitionAlbum_GetCommonAlbumRelationData_" + exhibitionId.ToString();
			object xmloObject = BCC.Cache.CacheManager.GetCachedData(cacheKey);
			if (xmloObject != null)
			{
				return (XmlDocument)xmloObject;
			}

			string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\ExhibitionConfig.xml";
			if (!File.Exists(sPath))
			{
				return null;
			}

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(sPath);
				if (xmlDoc == null)
				{
					return null;
				}
				XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode("Exhibition/item[@id='" + exhibitionId + "']/total");
				if (xNode == null) return null;

				string sRequestUrl = xNode.GetAttribute("url") + "&" + new Random().Next(10000, 100000).ToString();
				// string sRequestUrl = xNode.GetAttribute("url") + "?" + xNode.GetAttribute("param") + "&" + new Random().Next(10000, 100000).ToString();
				string content = Common.CommonFunction.GetContentByUrl(sRequestUrl);
				if (string.IsNullOrEmpty(content))
				{
					return null;
				}
				xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(content);
				BCC.Cache.CacheManager.InsertCache(cacheKey, xmlDoc, WebConfig.CachedDuration);
				return xmlDoc;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 得到通用车展热点美女排列
		/// </summary>
		/// <returns></returns>
		public virtual XmlDocument GetCommonAlbumHotData(int exhibitionId, int Count)
		{
			string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\ExhibitionConfig.xml";
			if (!File.Exists(sPath))
			{
				return null;
			}
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(sPath);
				if (xmlDoc == null)
				{
					return null;
				}
				XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode("Exhibition/item[@id='" + exhibitionId + "']/hot");
				if (xNode == null) return null;

				string sRequestUrl = xNode.GetAttribute("url") + "&" + new Random().Next(10000, 100000).ToString();
				// string sRequestUrl = xNode.GetAttribute("url") + "?" + xNode.GetAttribute("param") + "&rownum=" + Count.ToString() + "&" + new Random().Next(10000, 100000).ToString();
				string content = Common.CommonFunction.GetContentByUrl(sRequestUrl);
				if (string.IsNullOrEmpty(content))
				{
					return null;
				}
				xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(content);
				return xmlDoc;
			}
			catch
			{
				return null;
			}
		}
		/// <summary>
		/// 得到通用车展滚图列表
		/// </summary>
		/// <param name="exhibitionId">车展ID</param>
		/// <param name="dt">创建时间</param>
		/// <returns></returns>
		public virtual XmlDocument GetCommonAlbumNewCarGunTuList(int exhibitionId, DateTime sdt, DateTime edt)
		{
			string sPath = WebConfig.DataBlockPath + "Data\\Exhibition\\ExhibitionConfig.xml";
			if (!File.Exists(sPath))
			{
				return null;
			}
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(sPath);
				if (xmlDoc == null)
				{
					return null;
				}
				XmlElement xNode = (XmlElement)xmlDoc.SelectSingleNode("Exhibition/item[@id='" + exhibitionId + "']/newcar");
				if (xNode == null) return null;
				string requestUrl = xNode.GetAttribute("url");
				string startTime = sdt.ToString("yyyy-MM-dd hh:mm:ss");
				string endTime = edt.ToString("yyyy-MM-dd hh:mm:ss");
				string sRequestUrl = string.Format(requestUrl, startTime, endTime);

				xmlDoc = new XmlDocument();
				xmlDoc.Load(sRequestUrl);

				return xmlDoc;
			}
			catch
			{
				return null;
			}
		}

		#endregion

	}
}
