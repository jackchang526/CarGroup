using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using BitAuto.CarChannel.Common.Cache;
using System.Xml;
using System.Web.Caching;

namespace BitAuto.CarChannel.Common
{
	public class PhotoImageConfig
	{
		private static NameValueCollection nvcConfig;
		static PhotoImageConfig()
		{
			LoadConfig();
		}
		public static string SavePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SavePath"]) ? "" : nvcConfig["SavePath"]; }
		}
		public static string SerialColorPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialColorPath"]) ? "" : nvcConfig["SerialColorPath"]; }
		}
		public static string SerialColorAllPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialColorAllPath"]) ? "" : nvcConfig["SerialColorAllPath"]; }
		}
		public static string SerialPhotoListPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialPhotoListPath"]) ? "" : nvcConfig["SerialPhotoListPath"]; }
		}
		public static string SerialYearPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialYearPath"]) ? "" : nvcConfig["SerialYearPath"]; }
		}
		public static string SerialComparePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialComparePath"]) ? "" : nvcConfig["SerialComparePath"]; }
		}
		//废除 子品牌 图片对比 by 2014.08.08
		public static string SerialPhotoComparePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialPhotoComparePath"]) ? "" : nvcConfig["SerialPhotoComparePath"]; }
		}
		//add by sk 图片对比 按车款 2014.08.08
		public static string CarPhotoComparePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["CarPhotoComparePath"]) ? "" : nvcConfig["CarPhotoComparePath"]; }
		}
		public static string SerialClassPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialClassPath"]) ? "" : nvcConfig["SerialClassPath"]; }
		}
		public static string SerialCoverPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialCoverPath"]) ? "" : nvcConfig["SerialCoverPath"]; }
		}
		public static string SerialCoverWithoutPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialCoverWithoutPath"]) ? "" : nvcConfig["SerialCoverWithoutPath"]; }
		}
		//车系封面图及图片数量
		public static string SerialCoverImageAndCountPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialCoverImageAndCountPath"]) ? "": nvcConfig["SerialCoverImageAndCountPath"]; }
		}
		public static string SerialStandardImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialStandardImagePath"]) ? "" : nvcConfig["SerialStandardImagePath"]; }
		}
		public static string CarStandardImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["CarStandardImagePath"]) ? "" : nvcConfig["CarStandardImagePath"]; }
		}
		public static string CarCoverImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["CarCoverImagePath"]) ? "" : nvcConfig["CarCoverImagePath"]; }
		}
		public static string CarFocusImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["CarFocusImagePath"]) ? "" : nvcConfig["CarFocusImagePath"]; }
		}
		public static string SerialYearFocusImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialYearFocusImagePath"]) ? "" : nvcConfig["SerialYearFocusImagePath"]; }
		}
		public static string SerialDefaultCarPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialDefaultCarPath"]) ? "" : nvcConfig["SerialDefaultCarPath"]; }
		}
		public static string SerialDefaultCarImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialDefaultCarImagePath"]) ? "" : nvcConfig["SerialDefaultCarImagePath"]; }
		}
		public static string SerialFocusImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialFocusImagePath"]) ? "" : nvcConfig["SerialFocusImagePath"]; }
		}
		public static string SerialColorCountPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialColorCountPath"]) ? "" : nvcConfig["SerialColorCountPath"]; }
		}
		public static string SerialPhotoHtmlPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialPhotoHtmlPath"]) ? "" : nvcConfig["SerialPhotoHtmlPath"]; }
		}
        public static string SerialPhotoHtmlPathNew
        {
            get { return string.IsNullOrEmpty(nvcConfig["SerialPhotoHtmlPathNew"]) ? "" : nvcConfig["SerialPhotoHtmlPathNew"]; }
        }
		public static string SerialYearPhotoHtmlPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialYearPhotoHtmlPath"]) ? "" : nvcConfig["SerialYearPhotoHtmlPath"]; }
		}
        public static string SerialYearPhotoHtmlPathNew
        {
            get { return string.IsNullOrEmpty(nvcConfig["SerialYearPhotoHtmlPathNew"]) ? "" : nvcConfig["SerialYearPhotoHtmlPathNew"]; }
        }
		public static string CarPhotoHtmlPath
		{
			get { return string.IsNullOrEmpty(nvcConfig["CarPhotoHtmlPath"]) ? "" : nvcConfig["CarPhotoHtmlPath"]; }
		}
        public static string CarPhotoHtmlPathNew
        {
            get { return string.IsNullOrEmpty(nvcConfig["CarPhotoHtmlPathNew"]) ? "" : nvcConfig["CarPhotoHtmlPathNew"]; }
        }
		public static string SerialPositionImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialPositionImagePath"]) ? "" : nvcConfig["SerialPositionImagePath"]; }
		}

		public static string SerialColorImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialColorImagePath"]) ? "" : nvcConfig["SerialColorImagePath"]; }
		}
		public static string SerialElevenImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialElevenImagePath"]) ? "" : nvcConfig["SerialElevenImagePath"]; }
		}
		public static string SerialDefaultCarFillImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialDefaultCarFillImagePath"]) ? "" : nvcConfig["SerialDefaultCarFillImagePath"]; }
		}
		public static string SerialReallyColorImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialReallyColorImagePath"]) ? "" : nvcConfig["SerialReallyColorImagePath"]; }
		}
		public static string SerialFourthStageImagePath
		{
			get { return string.IsNullOrEmpty(nvcConfig["SerialFourthStageImagePath"]) ? "" : nvcConfig["SerialFourthStageImagePath"]; }
		}
        public static string SerialCarReallyImagePath
        {
            get { return string.IsNullOrEmpty(nvcConfig["SerialCarReallyImagePath"]) ? "" : nvcConfig["SerialCarReallyImagePath"]; }
        }
        public static string SerialSlidePageImagePath
        {
            get { return string.IsNullOrEmpty(nvcConfig["SerialSlidePageImagePath"]) ? "" : nvcConfig["SerialSlidePageImagePath"]; }
        }
        private static void LoadConfig()
		{
			nvcConfig = new NameValueCollection();
			string cacheLevelKey = "BITA_CAR_PHOTO_CONFIG";
			string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), @"config\PhotoImage.config");
			if (!File.Exists(filePath))
				return;
			try
			{
				XmlDocument cacheData = CacheManager.GetCachedData(cacheLevelKey) as XmlDocument;
				if (cacheData == null)
				{
					cacheData = new XmlDocument();
					cacheData.Load(filePath);
					CacheManager.InsertCache(cacheLevelKey, cacheData, new CacheDependency(filePath), DateTime.Now.AddDays(5));
				}
				XmlNode root = cacheData.DocumentElement;
				XmlNodeList nodeList = root.SelectNodes("//Item");
				foreach (XmlNode node in nodeList)
				{
					nvcConfig.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.Message + ex.StackTrace);
			}

		}
	}
}
