using System;
using System.Collections.Generic;
using System.Xml;
using System.Data;

using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.BLL.IndexManager
{
   public abstract class AbstractIndex:IAutoIndex
    {
        /// <summary>
        /// 获取购车指数的前十数据
        /// </summary>
        /// <param name="dateObj">日期数据，用于存储时间，可存储如下结构：年-周、年-季度、年-月</param>
        /// <returns></returns>
       public abstract Dictionary<string, List<IndexItem>> GetTopListData(DateObj dateObj);

       /// <summary>
       /// 生成前十条指数数据的显示数据
       /// </summary>
       /// <param name="itemType">项目类型（厂商，主品牌，子品牌）</param>
       /// <param name="itemNodeList"></param>
       /// <returns></returns>
       protected virtual  List<IndexItem> GetIndexItemList(IndexType indexType, BrandType itemType, XmlNodeList itemNodeList)
       {
           Dictionary<int, string> nameDic = null;
           string urlSegment = string.Empty;
           string idPrefix = string.Empty;

           //指数类型
           switch (indexType)
           {
               case IndexType.Dealer:
                   urlSegment = "gouche";
                   break;
               case IndexType.Media:
                   urlSegment = "meiti";
                   break;
               case IndexType.Sale:
                   urlSegment = "xiaoliang";
                   break;
               case IndexType.UV:
                   urlSegment = "guanzhu";
                   break;
               case IndexType.Compare:
                   urlSegment = "duibi";
                   break;
           }

           //厂商
           if (itemType == BrandType.Producer)
           {
               nameDic = this.GetProducerNameDic();
               idPrefix = "p";
           }
           else if (itemType == BrandType.MasterBrand)
           {
               nameDic = this.GetMasterBrandNameDic();//主品牌
               idPrefix = "mb";
           }
           else
           {
               nameDic = this.GetSerialNameDic();//子品牌
               idPrefix = "sb";
           }

           List<IndexItem> itemList = new List<IndexItem>();
           IndexItem item = null;

           foreach (XmlElement itemNode in itemNodeList)
           {
               item = new IndexItem();
               item.ID = Convert.ToInt32(itemNode.GetAttribute("ID"));

               if (nameDic.ContainsKey(item.ID))
               {
                   item.ItmeName = nameDic[item.ID];
               }
               else
               {
                   continue;
               }

               item.Index = Convert.ToInt32(itemNode.GetAttribute("Count"));

               //计算指数
               item.Index = CommonFunction.ComputeIndex(item.Index, indexType);
               item.ItemUrl = "/" + urlSegment + "/" + idPrefix + "_" + item.ID + "/";
               itemList.Add(item);
           }

           return itemList;
       }

	   /// <summary>
	   /// 生成前十条指数数据的显示数据
	   /// </summary>
	   /// <param name="itemType">项目类型（厂商，主品牌，子品牌）</param>
	   /// <param name="itemNodeList"></param>
	   /// <returns></returns>
	   protected virtual List<IndexItem> GetIndexItemList( IndexType indexType, BrandType itemType, IndexItem[] itemNodeList )
	   {
		   Dictionary<int, string> nameDic = null;
		   string urlSegment = string.Empty;
		   string idPrefix = string.Empty;

		   //指数类型
		   switch (indexType)
		   {
			   case IndexType.Dealer:
				   urlSegment = "gouche";
				   break;
			   case IndexType.Media:
				   urlSegment = "meiti";
				   break;
			   case IndexType.Sale:
				   urlSegment = "xiaoliang";
				   break;
			   case IndexType.UV:
				   urlSegment = "guanzhu";
				   break;
			   case IndexType.Compare:
				   urlSegment = "duibi";
				   break;
		   }

		   //厂商
		   if (itemType == BrandType.Producer)
		   {
			   nameDic = this.GetProducerNameDic();
			   idPrefix = "p";
		   }
		   else if (itemType == BrandType.MasterBrand)
		   {
			   nameDic = this.GetMasterBrandNameDic();//主品牌
			   idPrefix = "mb";
		   }
		   else
		   {
			   nameDic = this.GetSerialNameDic();//子品牌
			   idPrefix = "sb";
		   }
		   List<IndexItem> itemList = new List<IndexItem>();
		   foreach (IndexItem item in itemNodeList)
		   {
			   if (nameDic.ContainsKey(item.ID))
			   {
				   item.ItmeName = nameDic[item.ID];
			   }
			   else
			   {
				   continue;
			   }

			   //计算指数
			   item.Index = CommonFunction.ComputeIndex(item.Index, indexType);
			   item.ItemUrl = "/" + urlSegment + "/" + idPrefix + "_" + item.ID + "/";
			   itemList.Add(item);
		   }

		   return itemList;
	   }

       /// <summary>
       /// 获取所有厂商的名称的字典
       /// </summary>
       /// <returns></returns>
       protected virtual  Dictionary<int, string> GetProducerNameDic()
       {
           string cacheKey = "All_Producer_ID_Name_Dic";
           Dictionary<int, string> prdNameDic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, string>;

           if (prdNameDic == null)
           {
               DataSet ds = ProducerDal.GetProducers();

               if (ds != null && ds.Tables.Count > 0)
               {
                   prdNameDic = new Dictionary<int, string>();

                   foreach (DataRow row in ds.Tables[0].Rows)
                   {
                       int prdId = Convert.ToInt32(row["Cp_Id"]);
                       prdNameDic[prdId] = row["Cp_ShortName"].ToString();
                   }

                   CacheManager.InsertCache(cacheKey, prdNameDic, 30);
               }
           }

           return prdNameDic;
       }

       /// <summary>
       /// 获取所有主品牌的名称的字典
       /// </summary>
       /// <returns></returns>
       protected virtual  Dictionary<int, string> GetMasterBrandNameDic()
       {
           string cacheKey = "All_MasterBrand_ID_Name_Dic";
           Dictionary<int, string> nameDic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, string>;

           if (nameDic == null)
           {
               DataSet ds = MasterBrandDal.GetMasterBrands();

               if (ds != null && ds.Tables.Count > 0)
               {
                   nameDic = new Dictionary<int, string>();

                   foreach (DataRow row in ds.Tables[0].Rows)
                   {
                       int bsId = Convert.ToInt32(row["bs_Id"]);
                       nameDic[bsId] = row["bs_Name"].ToString();
                   }

                   CacheManager.InsertCache(cacheKey, nameDic, 30);
               }
           }

           return nameDic;
       }

       /// <summary>
       /// 获取所有子品牌的名称的字典
       /// </summary>
       /// <returns></returns>
       protected virtual Dictionary<int, string> GetSerialNameDic()
       {
           string cacheKey = "All_Serial_ID_Name_Dic";
           Dictionary<int, string> nameDic = CacheManager.GetCachedData(cacheKey) as Dictionary<int, string>;

           if (nameDic == null)
           {
               DataSet ds = SerialDal.GetSerials();

               if (ds != null && ds.Tables.Count > 0)
               {
                   nameDic = new Dictionary<int, string>();

                   foreach (DataRow row in ds.Tables[0].Rows)
                   {
                       int csId = Convert.ToInt32(row["cs_Id"]);
                       nameDic[csId] = row["cs_ShowName"].ToString().Trim();
                   }

                   CacheManager.InsertCache(cacheKey, nameDic, 30);
               }
           }

           return nameDic;
       }
    }
}
