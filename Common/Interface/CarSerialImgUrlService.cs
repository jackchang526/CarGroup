using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.Common.Cache;
using System.Timers;
using System.IO;
using System.Net;
using System.Web.Caching;

namespace BitAuto.CarChannel.Common.Interface
{
    public class CarSerialImgUrlService
    {
		#region ˽�г�Ա

		private static Timer    csImgTimer;
		private static object   objLock;
		private static object   objImageUrlLock;				//ͼƬURL�ļ���
		private static string   imageUrlFile;

		#endregion

		/// <summary>
		/// ��̬���캯��
		/// </summary>
        static CarSerialImgUrlService()
		{
			objLock = new object();
			objImageUrlLock = new object();

			imageUrlFile = "CarSerialImageUrl.xml";
		}


        public static void Start()
        {
            csImgTimer = new Timer(8 * 60 * 60 * 1000);			//8Сʱ����һ��

            csImgTimer.Elapsed +=new ElapsedEventHandler(csImgTimer_Elapsed);
        }

        static void csImgTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
           //  UpdateCarSerialImages();
        }

		//private static void UpdateCarSerialImages()
		//{
		//	string dataPath = Path.Combine(WebConfig.DataBlockPath, "Data\\CarSerialImageUrl");
		//	string backupPath = Path.Combine(WebConfig.DataBlockPath, "Data\\Last\\CarSerialImageUrl");

		//	WebClient wc = new WebClient();
		//	wc.Encoding = Encoding.UTF8;

		//	//ImageUrl
		//	string backupFile = Path.Combine(backupPath, imageUrlFile);
		//	string xmlFile = Path.Combine(dataPath, imageUrlFile);
		//	string xmlUrl = "http://photo.bitauto.com/service/getserialcover.aspx";
		//	string xmlStr = wc.DownloadString(xmlUrl);
		//	xmlStr = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n" + xmlStr;
		//	lock(objImageUrlLock)
		//	{
		//		//����
		//		if(File.Exists(xmlFile))
		//		{
		//			File.Copy(xmlFile, backupFile);
		//		}
		//		File.WriteAllText(xmlFile, xmlStr);
		//	}
		//}
        /// <summary>
        /// ��ȡͼƬUrl�ֵ䣨�ļ����ٸ��£�
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, XmlElement> GetImageUrlDic()
        {
            Dictionary<int, XmlElement> sImgDic = null;
            string cacheKey = "Car_Serial_Image_Url_Dic";
            object imgUrl = CacheManager.GetCachedData(cacheKey);
            if (imgUrl == null)
            {
                XmlDocument doc = new XmlDocument();
                // modified Oct.10.2009
                //string xmlFile = Path.Combine(WebConfig.WebRootPath, "Data\\CarSerialImageUrl\\" + imageUrlFile);
                string xmlFile = Path.Combine(WebConfig.DataBlockPath, "Data\\ImageUrl.xml");
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                    XmlNodeList imgUrlList = doc.SelectNodes("/SerialList/Serial");
                    sImgDic = new Dictionary<int, XmlElement>();
                    foreach (XmlElement sNode in imgUrlList)
                    {
                        int sId = Convert.ToInt32(sNode.GetAttribute("SerialId"));
                        sImgDic[sId] = sNode;
                    }

                    //���뻺��
                    CacheDependency cacheDependency = new CacheDependency(xmlFile);
                    CacheManager.InsertCache(cacheKey, sImgDic, cacheDependency, DateTime.Now.AddMinutes(WebConfig.CachedDuration));
                }
            }
            else
                sImgDic = (Dictionary<int, XmlElement>)imgUrl;

            return sImgDic;
        }

        /// <summary>
        /// ��ȡ��Ʒ���·���ͼƬUrl�ֵ�
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, XmlElement> GetImageUrlDicNew()
        {
			Dictionary<int, XmlElement> sImgDic = new Dictionary<int, XmlElement>();
            string cacheKey = "Car_Serial_Image_Url_DicNew";
            object imgUrl = CacheManager.GetCachedData(cacheKey);
            if (imgUrl == null)
            {
                XmlDocument doc = new XmlDocument();
				//ͼ��ӿڱ��ػ����� by sk 2012.12.21
				string xmlFile = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialCoverImageAndCountPath);
				//string xmlFile = WebConfig.AllSerialDefaultPicAndCount;
                if (File.Exists(xmlFile))
                {
                    doc.Load(xmlFile);
                    XmlNodeList imgUrlList = doc.SelectNodes("/SerialList/Serial");
                    // sImgDic = new Dictionary<int, XmlElement>();
                    foreach (XmlElement sNode in imgUrlList)
                    {
                        int sId = Convert.ToInt32(sNode.GetAttribute("SerialId"));
                        sImgDic[sId] = sNode;
                    }

                    //���뻺��
                    // CacheDependency cacheDependency = new CacheDependency(xmlFile);
                    CacheManager.InsertCache(cacheKey, sImgDic, WebConfig.CachedDuration);
                }
            }
            else
                sImgDic = (Dictionary<int, XmlElement>)imgUrl;

            return sImgDic;
        }

    }
}
