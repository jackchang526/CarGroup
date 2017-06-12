using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL.IndexManager
{
	public class SaleIndex:AbstractIndex
	{

        #region IIndex 成员
        /// <summary>
        /// 获取购车指数的前十数据
        /// </summary>
        /// <param name="dateObj">日期数据，用于存储时间，可存储如下结构：年-周、年-季度、年-月</param>
        /// <returns></returns>
        public override Dictionary<string, List<IndexItem>> GetTopListData(DateObj dateObj)
        {
            string topFile = Path.Combine(WebConfig.IndexDataBlockPath, "Sale\\Month\\" + dateObj.Year + "\\" + dateObj.DateNum + "\\ListTop10.xml");
            Dictionary<string, List<IndexItem>> topDic = new Dictionary<string, List<IndexItem>>();

            if (File.Exists(topFile))
            {
                XmlDocument topDoc = new XmlDocument();
                topDoc.Load(topFile);

                //厂商				
                //XmlNodeList tmpNodeList = topDoc.SelectNodes("/Root/Cp/Item");
                //topDic["CP"] = this.GetIndexItemList(IndexType.Sale, BrandType.Producer, tmpNodeList);

                //主品牌
				XmlNodeList tmpNodeList = topDoc.SelectNodes("/Root/Bs/Item");
                topDic["BS"] = this.GetIndexItemList(IndexType.Sale, BrandType.MasterBrand, tmpNodeList);

                //子品牌
                tmpNodeList = topDoc.SelectNodes("/Root/Cs/Item");
                topDic["CS"] = this.GetIndexItemList(IndexType.Sale, BrandType.Serial, tmpNodeList);

				////取级别的前十
				//foreach (SerialLevelSpellEnum levelSpellEnum in Enum.GetValues(typeof(SerialLevelSpellEnum)))
				//{
				//    int levelId = (int)levelSpellEnum;
				//    string levelSpell = levelSpellEnum.ToString();
				//    string levelName = ((SerialLevelEnum)levelId).ToString();

				//    if (levelName == "紧凑型" || levelName == "中大型")
				//    {
				//        levelName += "车";
				//    }

				//    tmpNodeList = topDoc.SelectNodes("/Root/AllLevel/Level[@Name=\"" + levelName + "\"]/Item");
				//    topDic[levelSpell] = this.GetIndexItemList(IndexType.Sale, BrandType.Serial, tmpNodeList);
				//}
            }

            return topDic;
        }


		/// <summary>
		/// 获取某时间，某地区的指数前十排行
		/// </summary>
		/// <param name="dateObj">日期</param>
		/// <param name="regionType">地区类型，省或市 City｜Province</param>
		/// <param name="regionId">省或市的ID</param>
		/// <returns></returns>
		public Dictionary<string, List<IndexItem>> GetRegionalTopListData(DateObj dateObj, RegionType regionType, int regionId)
		{
			Dictionary<string, List<IndexItem>> topDic = new Dictionary<string, List<IndexItem>>();

			string basePath = Path.Combine(WebConfig.IndexDataBlockPath, "Sale\\Month");

			basePath = Path.Combine(basePath, dateObj.Year + "\\" + dateObj.DateNum);

			string fileName = "";
			//string xmlPath = "";
			//if (regionType == RegionType.Province)
			//    xmlPath = "/Root/Province[@ID=" + regionId + "]/Item";
			//else
			//    xmlPath = "/Root/City[@ID=" + regionId + "]/Item";
			//XmlDocument moreDoc = null;
			//厂商数据
			//fileName = Path.Combine(basePath, "ListMoreCp.xml");
			//if (File.Exists(fileName))
			//{
			//    moreDoc = new XmlDocument();
			//    moreDoc.Load(fileName);
			//    XmlNodeList itemNodeList = moreDoc.SelectNodes(xmlPath);
			//    topDic["CP"] = this.GetIndexItemList(IndexType.Sale, BrandType.Producer, itemNodeList);
			//}
			//else
			//    topDic["CP"] = new List<IndexItem>();

			//主品牌数据
			fileName = Path.Combine(basePath, "ListMoreBs.xml");
			if (File.Exists(fileName))
			{
				//moreDoc = new XmlDocument();
				//moreDoc.Load(fileName);
				//XmlNodeList itemNodeList = moreDoc.SelectNodes(xmlPath);
				//topDic["BS"] = this.GetIndexItemList(IndexType.Sale, BrandType.MasterBrand, itemNodeList);
				IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(fileName, regionType, regionId);
				topDic["BS"] = this.GetIndexItemList(IndexType.Sale, BrandType.MasterBrand, itemList);
			}
			else
				topDic["BS"] = new List<IndexItem>();
			//子品牌数据
			fileName = Path.Combine(basePath, "ListMoreLevel0.xml");
			if (File.Exists(fileName))
			{
				//moreDoc = new XmlDocument();
				//moreDoc.Load(fileName);
				//XmlNodeList itemNodeList = moreDoc.SelectNodes(xmlPath);
				//topDic["CS"] = this.GetIndexItemList(IndexType.Sale, BrandType.Serial, itemNodeList);
				IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(fileName, regionType, regionId);
				topDic["CS"] = this.GetIndexItemList(IndexType.Sale, BrandType.Serial, itemList);
			}
			else
				topDic["CS"] = new List<IndexItem>();

			////各级别数据
			//foreach (SerialLevelSpellEnum levelSpellEnum in Enum.GetValues(typeof(SerialLevelSpellEnum)))
			//{
			//    int levelId = (int)levelSpellEnum;
			//    string levelSpell = levelSpellEnum.ToString();
			//    string levelPostfix = CommonFunction.GetLevelFilePostfixBySpell(levelSpell);
			//    fileName = Path.Combine(basePath, string.Format("ListMoreLevel{0}.xml", levelPostfix));
			//    XmlDocument levelDoc = new XmlDocument();
			//    levelDoc.Load(fileName);
			//    XmlNodeList itemNodeList = levelDoc.SelectNodes(xmlPath);
			//    topDic[levelSpell] = this.GetIndexItemList(IndexType.Sale, BrandType.Level, itemNodeList);
			//}


			return topDic;
		}

        #endregion
    }
}
