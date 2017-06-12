using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.BLL.IndexManager
{
	public class DealerIndex:AbstractIndex
	{

        #region IIndex 成员
        /// <summary>
        /// 获取购车指数的前十数据
        /// </summary>
        /// <param name="dateObj">日期数据，用于存储时间，可存储如下结构：年-周、年-季度、年-月</param>
        /// <returns></returns>
       public override Dictionary<string, List<IndexItem>> GetTopListData(DateObj dateObj)
       {
            string topFile = Path.Combine(WebConfig.IndexDataBlockPath, "Dealer\\Month\\" + dateObj.Year + "\\" + dateObj.DateNum + "\\ListTop10.xml");
            Dictionary<string, List<IndexItem>> topDic = new Dictionary<string, List<IndexItem>>();

            if (File.Exists(topFile))
            {
                XmlDocument topDoc = new XmlDocument();
                topDoc.Load(topFile);

                //厂商				
				//XmlNodeList tmpNodeList = topDoc.SelectNodes("/Root/Cp/Item");
				//topDic["CP"] = this.GetIndexItemList(IndexType.Dealer, BrandType.Producer, tmpNodeList);


                //主品牌
				XmlNodeList tmpNodeList = topDoc.SelectNodes("/Root/Bs/Item");
                topDic["BS"] = this.GetIndexItemList(IndexType.Dealer, BrandType.MasterBrand, tmpNodeList);

                //子品牌
                tmpNodeList = topDoc.SelectNodes("/Root/Cs/Item");
                topDic["CS"] = this.GetIndexItemList(IndexType.Dealer, BrandType.Serial, tmpNodeList);

                //取级别的前十
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
				//    topDic[levelSpell] = this.GetIndexItemList(IndexType.Dealer, BrandType.Serial, tmpNodeList);
				//}
            }

            return topDic;
        }


	   /// <summary>
	   /// 获取购车指数某地区的指数前十数据
	   /// </summary>
	   /// <param name="dateObj"></param>
	   /// <param name="regionType"></param>
	   /// <param name="regionId"></param>
	   /// <returns></returns>
	   public Dictionary<string, List<IndexItem>> GetRegionalTopListData(DateObj dateObj, RegionType regionType, int regionId)
	   {
		   Dictionary<string, List<IndexItem>> topDic = new Dictionary<string, List<IndexItem>>();
		   //string xmlPath = "";
		   //if (regionType == RegionType.Province)
		   //    xmlPath = "/Root/Province[@ID=" + regionId + "]/Item";
		   //else
		   //    xmlPath = "/Root/City[@ID=" + regionId + "]/Item";

		   string filePath = Path.Combine(WebConfig.IndexDataBlockPath, "Dealer\\Month\\" + dateObj.Year + "\\" + dateObj.DateNum);
		   //厂商
		   //string topFile = Path.Combine(filePath, "ListMoreCp.xml");
		   //if (File.Exists(topFile))
		   //{
		   //    XmlDocument cpDoc = new XmlDocument();
		   //    cpDoc.Load(topFile);
		   //    XmlNodeList cpNodeList = cpDoc.SelectNodes(xmlPath);
		   //    topDic["CP"] = this.GetIndexItemList(IndexType.Dealer, BrandType.Producer, cpNodeList);
		   //}
		   //品牌
		   string topFile = Path.Combine(filePath, "ListMoreBs.xml");
		   if (File.Exists(topFile))
		   {
			   //XmlDocument bsDoc = new XmlDocument();
			   //bsDoc.Load(topFile);
			   //XmlNodeList bsNodeList = bsDoc.SelectNodes(xmlPath);
			   //topDic["BS"] = this.GetIndexItemList(IndexType.Dealer, BrandType.MasterBrand, bsNodeList);
			   IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(topFile, regionType, regionId);
			   topDic["BS"] = this.GetIndexItemList(IndexType.Dealer, BrandType.MasterBrand, itemList);
		   }
		   //子品牌
		   topFile = Path.Combine(filePath, "ListMoreLevel0.xml");
		   if (File.Exists(topFile))
		   {
			   //XmlDocument csDoc = new XmlDocument();
			   //csDoc.Load(topFile);
			   //XmlNodeList csNodeList = csDoc.SelectNodes(xmlPath);
			   //topDic["CS"] = this.GetIndexItemList(IndexType.Dealer, BrandType.Serial, csNodeList);
			   IndexItem[] itemList = IndexBll.GetIndexItemsByAreaId(topFile, regionType, regionId);
			   topDic["CS"] = this.GetIndexItemList(IndexType.Dealer, BrandType.Serial, itemList);
		   }

		   //取级别的前十
		   //foreach (SerialLevelSpellEnum levelSpellEnum in Enum.GetValues(typeof(SerialLevelSpellEnum)))
		   //{
		   //    int levelId = (int)levelSpellEnum;
		   //    string levelSpell = levelSpellEnum.ToString();
		   //    topFile = Path.Combine(filePath, "ListMoreLevel" + levelId + ".xml");
		   //    if (File.Exists(topFile))
		   //    {
		   //        XmlDocument levelDoc = new XmlDocument();
		   //        levelDoc.Load(topFile);
		   //        XmlNodeList levelNodeList = levelDoc.SelectNodes(xmlPath);
		   //        topDic[levelSpell] = this.GetIndexItemList(IndexType.Dealer, BrandType.Serial, levelNodeList);
		   //    }
		   //}

		   return topDic;
	   }

        #endregion
    }
}
