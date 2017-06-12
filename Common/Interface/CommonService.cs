using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;

namespace BitAuto.CarChannel.Common.Interface
{

	public class CommonService
	{
		public CommonService()
		{ }

		/// <summary>
		/// 对比页选车工具
		/// </summary>
		/// <returns></returns>
		public string GetAllSerialForCompareList()
		{
			Hashtable htSUV = new Hashtable();
			Hashtable htMPV = new Hashtable();
			Hashtable htPao = new Hashtable();
			StringBuilder sb = new StringBuilder();
			StringBuilder sbLiangXiang = new StringBuilder();
			StringBuilder sbShanXiang = new StringBuilder();
			StringBuilder sbQiTa = new StringBuilder();
			StringBuilder sbSUV = new StringBuilder();
			StringBuilder sbMPV = new StringBuilder();
			StringBuilder sbPaoChe = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<Params Time=\"" + DateTime.Now.ToString() + "\">");
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(WebConfig.PriceRangeSerial);
				if (xmlDoc != null && xmlDoc.HasChildNodes)
				{
					XmlNodeList xnl = xmlDoc.SelectNodes("/Params/CsPrice");
					if (xnl != null && xnl.Count > 0)
					{
						sbSUV.Append("<Level1Group Name=\"SUV\" >");
						sbSUV.Append("<Level2Group Name=\"SUV\" >");
						sbMPV.Append("<Level1Group Name=\"MPV\" >");
						sbMPV.Append("<Level2Group Name=\"MPV\" >");
						sbPaoChe.Append("<Level1Group Name=\"跑车\" >");
						sbPaoChe.Append("<Level2Group Name=\"跑车\" >");
						for (int i = 0; i < xnl.Count; i++)
						{
							// 增加无价格的 SUV MPV 跑车 modifed by chengl Feb.21.2010
							foreach (XmlNode xn in xnl[i])
							{

								if (!IsHasCarSerial(int.Parse(xn.Attributes["ID"].Value.Trim())))
								{ continue; }

								#region SUV MPV 跑车级别
								if (xn.Attributes["CsLevel"].Value.Trim() == "SUV")
								{
									if (!htSUV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
									{
										htSUV.Add(xn.Attributes["ID"].Value.Trim(), 1);
										sbSUV.Append(xn.OuterXml);
									}
								}
								if (xn.Attributes["CsLevel"].Value.Trim() == "MPV")
								{
									if (!htMPV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
									{
										htMPV.Add(xn.Attributes["ID"].Value.Trim(), 1);
										sbMPV.Append(xn.OuterXml);
									}
								}
								if (xn.Attributes["CsLevel"].Value.Trim() == "跑车")
								{
									if (!htPao.ContainsKey(xn.Attributes["ID"].Value.Trim()))
									{
										htPao.Add(xn.Attributes["ID"].Value.Trim(), 1);
										sbPaoChe.Append(xn.OuterXml);
									}
								}
								#endregion
							}

							if (xnl[i].Attributes["PriceRange"].Value != "无价格")
							{
								sb.Append("<Level1Group Name=\"" + xnl[i].Attributes["PriceRange"].Value.Replace("-", "万-") + "\">");
								if (xnl[i].HasChildNodes)
								{
									sbLiangXiang.Append("<Level2Group Name=\"两厢轿车\" >");
									sbShanXiang.Append("<Level2Group Name=\"三厢轿车\" >");
									sbQiTa.Append("<Level2Group Name=\"其它\" >");
									foreach (XmlNode xn in xnl[i])
									{
										if (!IsHasCarSerial(int.Parse(xn.Attributes["ID"].Value.Trim())))
										{ continue; }

										#region 厢式
										if (xn.Attributes["BodyType"].Value.Trim() == "两厢")
										{
											sbLiangXiang.Append(xn.OuterXml);
										}
										else if (xn.Attributes["BodyType"].Value.Trim() == "三厢")
										{
											sbShanXiang.Append(xn.OuterXml);
										}
										else
										{
											sbQiTa.Append(xn.OuterXml);
										}
										#endregion

										#region SUV MPV 跑车级别 废弃
										//if (xn.Attributes["CsLevel"].Value.Trim() == "SUV")
										//{
										//    if (!htSUV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
										//    {
										//        htSUV.Add(xn.Attributes["ID"].Value.Trim(), 1);
										//        sbSUV.Append(xn.OuterXml);
										//    }
										//}
										//if (xn.Attributes["CsLevel"].Value.Trim() == "MPV")
										//{
										//    if (!htMPV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
										//    {
										//        htMPV.Add(xn.Attributes["ID"].Value.Trim(), 1);
										//        sbMPV.Append(xn.OuterXml);
										//    }
										//}
										//if (xn.Attributes["CsLevel"].Value.Trim() == "跑车")
										//{
										//    if (!htPao.ContainsKey(xn.Attributes["ID"].Value.Trim()))
										//    {
										//        htPao.Add(xn.Attributes["ID"].Value.Trim(), 1);
										//        sbPaoChe.Append(xn.OuterXml);
										//    }
										//}
										#endregion
									}
									sbLiangXiang.Append("</Level2Group>");
									sbShanXiang.Append("</Level2Group>");
									sbQiTa.Append("</Level2Group>");
									sb.Append(sbLiangXiang.ToString());
									sb.Append(sbShanXiang.ToString());
									sb.Append(sbQiTa.ToString());
									sbLiangXiang.Remove(0, sbLiangXiang.Length);
									sbShanXiang.Remove(0, sbShanXiang.Length);
									sbQiTa.Remove(0, sbQiTa.Length);
								}
								sb.Append("</Level1Group>");
							}
						}
						sbSUV.Append("</Level2Group>");
						sbMPV.Append("</Level2Group>");
						sbPaoChe.Append("</Level2Group>");
						sbSUV.Append("</Level1Group>");
						sbMPV.Append("</Level1Group>");
						sbPaoChe.Append("</Level1Group>");
						sb.Append(sbSUV.ToString());
						sb.Append(sbMPV.ToString());
						sb.Append(sbPaoChe.ToString());
					}
				}
			}
			catch
			{ }
			sb.Append("</Params>");
			return sb.ToString();
		}

		/// <summary>
		/// 对比页选车工具按价格区间
		/// </summary>
		/// <returns></returns>
		public List<XmlElement> GetAllSerialForCompareListForPrice(int type)
		{
			List<XmlElement> lxe = new List<XmlElement>();
			//获取数据xml
			XmlDocument mbDoc = AutoStorageService.GetAutoXml();
			if (type >= 1 && type <= 8)
			{
				//遍历所有子品牌节点
				XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
				//将所有子品牌按价格,箱式，分类列表
				Dictionary<int, List<XmlElement>> allSerialNodes = new Dictionary<int, List<XmlElement>>();
				foreach (XmlElement serialNode in serialNodeList)
				{
					string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					//按报价加入
					foreach (string priceId in prices)
					{
						int pId = Convert.ToInt32(priceId);
						//没有报价按未上市处理
						if (pId == 0)
							pId = 9;
						//if (!priceIdDic.ContainsKey(pId))
						//    continue;
						if (!allSerialNodes.ContainsKey(pId))
							allSerialNodes[pId] = new List<XmlElement>();
						allSerialNodes[pId].Add(serialNode);
					}
				}
				if (allSerialNodes.ContainsKey(type))
				{
					lxe = allSerialNodes[type];
					lxe.Sort(NodeCompare.CompareSerialByPvDesc);
				}
			}
			else if (type == 9 || type == 10 || type == 11 || type == 12)
			{
				string levelName = "";
				switch (type)
				{
					case 9: levelName = "SUV"; break;
					case 10: levelName = "MPV"; break;
					case 11: levelName = "跑车"; break;
					case 12: levelName = "面包车"; break;
					default: levelName = ""; break;
				}
				//遍历所有子品牌节点
				XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial[@CsLevel='" + levelName + "']");
				foreach (XmlElement serialNode in serialNodeList)
				{
					lxe.Add(serialNode);
				}
				lxe.Sort(NodeCompare.CompareSerialByPvDesc);
			}

			return lxe;
		}

		///// <summary>
		///// 图片对比 选车工具
		///// add by chengl Jan.29.2012
		///// </summary>
		///// <returns></returns>
		//public Dictionary<int, List<XmlElement>> GetAllSerialForPhotoCompareListNew()
		//{
		//	Dictionary<int, List<XmlElement>> dic = new Dictionary<int, List<XmlElement>>();
		//	string cacheKey = "CommonService_GetAllSerialForPhotoCompareListNew";
		//	object getAllSerialForPhotoCompareListNew = null;
		//	CacheManager.GetCachedData(cacheKey, out getAllSerialForPhotoCompareListNew);
		//	if (getAllSerialForPhotoCompareListNew == null)
		//	{
		//		Hashtable ht = GetPhotoCompareSerialList();
		//		if (ht != null && ht.Count > 0)
		//		{
		//			XmlDocument xmlDoc = new XmlDocument();
		//			xmlDoc.Load(WebConfig.PriceRangeSerial);
		//			if (xmlDoc != null && xmlDoc.HasChildNodes)
		//			{
		//				Dictionary<int, XmlElement> dicSUV = new Dictionary<int, XmlElement>();
		//				Dictionary<int, XmlElement> dicMPV = new Dictionary<int, XmlElement>();
		//				Dictionary<int, XmlElement> dicPaoChe = new Dictionary<int, XmlElement>();
		//				Dictionary<int, XmlElement> dicQiTa = new Dictionary<int, XmlElement>();

		//				XmlNodeList xnlCsPrice = xmlDoc.SelectNodes("/Params/CsPrice");
		//				if (xnlCsPrice != null && xnlCsPrice.Count > 0)
		//				{
		//					// 循环价格区间 0-8
		//					int type = 0;
		//					foreach (XmlNode xnCsPrice in xnlCsPrice)
		//					{
		//						Dictionary<int, XmlElement> dicCsPrice = new Dictionary<int, XmlElement>();
		//						if (xnCsPrice != null && xnCsPrice.HasChildNodes)
		//						{
		//							foreach (XmlElement xe in xnCsPrice.ChildNodes)
		//							{
		//								int csid = int.Parse(xe.GetAttribute("ID"));
		//								if (!ht.ContainsKey(csid.ToString()))
		//								{ continue; }
		//								if (!dicCsPrice.ContainsKey(csid))
		//								{ dicCsPrice.Add(csid, xe); }

		//								// 如果是SUV
		//								if (xe.GetAttribute("CsLevel") == "SUV" && !dicSUV.ContainsKey(csid))
		//								{ dicSUV.Add(csid, xe); }
		//								// 如果是MPV
		//								if (xe.GetAttribute("CsLevel") == "MPV" && !dicMPV.ContainsKey(csid))
		//								{ dicMPV.Add(csid, xe); }
		//								// 如果是跑车
		//								if (xe.GetAttribute("CsLevel") == "跑车" && !dicPaoChe.ContainsKey(csid))
		//								{ dicPaoChe.Add(csid, xe); }
		//								// 如果是其他
		//								if (xe.GetAttribute("CsLevel") == "其它" && !dicQiTa.ContainsKey(csid))
		//								{ dicQiTa.Add(csid, xe); }
		//							}
		//						}
		//						// 将价格区间加入结果集
		//						GetSomeLevelToListForPhotoCompare(dic, type, dicCsPrice);
		//						type++;
		//					}
		//					#region 将 SUV MPV 跑车 其他 加入结果集
		//					// 将 SUV MPV 跑车 其他 加入结果集
		//					GetSomeLevelToListForPhotoCompare(dic, 9, dicSUV);
		//					GetSomeLevelToListForPhotoCompare(dic, 10, dicMPV);
		//					GetSomeLevelToListForPhotoCompare(dic, 11, dicPaoChe);
		//					GetSomeLevelToListForPhotoCompare(dic, 12, dicQiTa);
		//					#endregion
		//				}
		//			}
		//		}
		//		CacheManager.InsertCache(cacheKey, dic, 60);
		//	}
		//	else
		//	{ dic = (Dictionary<int, List<XmlElement>>)getAllSerialForPhotoCompareListNew; }
		//	return dic;
		//}

		/// <summary>
		/// 图片对比数据 选车工具 将各个价格区间及级别加入数据源
		/// </summary>
		/// <param name="dic"></param>
		/// <param name="index"></param>
		/// <param name="dicItem"></param>
		private void GetSomeLevelToListForPhotoCompare(Dictionary<int, List<XmlElement>> dic, int index, Dictionary<int, XmlElement> dicItem)
		{
			if (!dic.ContainsKey(index) && dicItem.Count > 0)
			{
				List<XmlElement> list = new List<XmlElement>();
				foreach (KeyValuePair<int, XmlElement> kvp in dicItem)
				{
					list.Add(kvp.Value);
				}
				list.Sort(NodeCompare.CompareSerialByPvDesc);
				dic.Add(index, list);
			}
		}

		///// <summary>
		///// 图片对比页选车工具
		///// </summary>
		///// <returns></returns>
		//[Obsolete("此方法过期！")]
		//public string GetAllSerialForPhotoCompareList()
		//{
		//	Hashtable ht = GetPhotoCompareSerialList();
		//	if (ht == null || ht.Count < 1)
		//	{
		//		return "";
		//	}
		//	Hashtable htSUV = new Hashtable();
		//	Hashtable htMPV = new Hashtable();
		//	Hashtable htPao = new Hashtable();
		//	StringBuilder sb = new StringBuilder();
		//	StringBuilder sbLiangXiang = new StringBuilder();
		//	StringBuilder sbShanXiang = new StringBuilder();
		//	StringBuilder sbQiTa = new StringBuilder();
		//	StringBuilder sbSUV = new StringBuilder();
		//	StringBuilder sbMPV = new StringBuilder();
		//	StringBuilder sbPaoChe = new StringBuilder();
		//	sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
		//	sb.Append("<Params Time=\"" + DateTime.Now.ToString() + "\">");
		//	XmlDocument xmlDoc = new XmlDocument();
		//	try
		//	{
		//		xmlDoc.Load(WebConfig.PriceRangeSerial);
		//		if (xmlDoc != null && xmlDoc.HasChildNodes)
		//		{
		//			XmlNodeList xnl = xmlDoc.SelectNodes("/Params/CsPrice");
		//			if (xnl != null && xnl.Count > 0)
		//			{
		//				sbSUV.Append("<Level1Group Name=\"SUV\" >");
		//				sbSUV.Append("<Level2Group Name=\"SUV\" >");
		//				sbMPV.Append("<Level1Group Name=\"MPV\" >");
		//				sbMPV.Append("<Level2Group Name=\"MPV\" >");
		//				sbPaoChe.Append("<Level1Group Name=\"跑车\" >");
		//				sbPaoChe.Append("<Level2Group Name=\"跑车\" >");
		//				for (int i = 0; i < xnl.Count; i++)
		//				{
		//					if (xnl[i].Attributes["PriceRange"].Value != "无价格")
		//					{
		//						sb.Append("<Level1Group Name=\"" + xnl[i].Attributes["PriceRange"].Value.Replace("-", "万-") + "\">");
		//						if (xnl[i].HasChildNodes)
		//						{
		//							sbLiangXiang.Append("<Level2Group Name=\"两厢轿车\" >");
		//							sbShanXiang.Append("<Level2Group Name=\"三厢轿车\" >");
		//							sbQiTa.Append("<Level2Group Name=\"其它\" >");
		//							foreach (XmlNode xn in xnl[i])
		//							{
		//								// 判断是否在审核列表
		//								if (!ht.ContainsKey(xn.Attributes["ID"].Value.Trim()))
		//								{
		//									continue;
		//								}

		//								#region 厢式
		//								if (xn.Attributes["BodyType"].Value.Trim() == "两厢")
		//								{
		//									sbLiangXiang.Append(xn.OuterXml);
		//								}
		//								else if (xn.Attributes["BodyType"].Value.Trim() == "三厢")
		//								{
		//									sbShanXiang.Append(xn.OuterXml);
		//								}
		//								else
		//								{
		//									sbQiTa.Append(xn.OuterXml);
		//								}
		//								#endregion

		//								#region SUV MPV 跑车级别
		//								if (xn.Attributes["CsLevel"].Value.Trim() == "SUV")
		//								{
		//									if (!htSUV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
		//									{
		//										htSUV.Add(xn.Attributes["ID"].Value.Trim(), 1);
		//										sbSUV.Append(xn.OuterXml);
		//									}
		//								}
		//								if (xn.Attributes["CsLevel"].Value.Trim() == "MPV")
		//								{
		//									if (!htMPV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
		//									{
		//										htMPV.Add(xn.Attributes["ID"].Value.Trim(), 1);
		//										sbMPV.Append(xn.OuterXml);
		//									}
		//								}
		//								if (xn.Attributes["CsLevel"].Value.Trim() == "跑车")
		//								{
		//									if (!htPao.ContainsKey(xn.Attributes["ID"].Value.Trim()))
		//									{
		//										htPao.Add(xn.Attributes["ID"].Value.Trim(), 1);
		//										sbPaoChe.Append(xn.OuterXml);
		//									}
		//								}
		//								#endregion
		//							}
		//							sbLiangXiang.Append("</Level2Group>");
		//							sbShanXiang.Append("</Level2Group>");
		//							sbQiTa.Append("</Level2Group>");
		//							sb.Append(sbLiangXiang.ToString());
		//							sb.Append(sbShanXiang.ToString());
		//							sb.Append(sbQiTa.ToString());
		//							sbLiangXiang.Remove(0, sbLiangXiang.Length);
		//							sbShanXiang.Remove(0, sbShanXiang.Length);
		//							sbQiTa.Remove(0, sbQiTa.Length);
		//						}
		//						sb.Append("</Level1Group>");
		//					}
		//				}
		//				sbSUV.Append("</Level2Group>");
		//				sbMPV.Append("</Level2Group>");
		//				sbPaoChe.Append("</Level2Group>");
		//				sbSUV.Append("</Level1Group>");
		//				sbMPV.Append("</Level1Group>");
		//				sbPaoChe.Append("</Level1Group>");
		//				sb.Append(sbSUV.ToString());
		//				sb.Append(sbMPV.ToString());
		//				sb.Append(sbPaoChe.ToString());
		//			}
		//		}
		//	}
		//	catch
		//	{ }
		//	sb.Append("</Params>");
		//	return sb.ToString();
		//}

		/// <summary>
		/// 评测对比 选车工具
		/// add by chengl Feb.13.2012
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, List<XmlElement>> GetAllSerialForPingCeCompareListNew()
		{
			Dictionary<int, List<XmlElement>> dic = new Dictionary<int, List<XmlElement>>();
			string cacheKey = "CommonService_GetAllSerialForPingCeCompareListNew";
			object getAllSerialForPingCeCompareListNew = null;
			CacheManager.GetCachedData(cacheKey, out getAllSerialForPingCeCompareListNew);
			if (getAllSerialForPingCeCompareListNew == null)
			{
				Hashtable ht = GetPingCeCompareSerialList();
				if (ht != null && ht.Count > 0)
				{
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(WebConfig.PriceRangeSerial);
					if (xmlDoc != null && xmlDoc.HasChildNodes)
					{
						Dictionary<int, XmlElement> dicSUV = new Dictionary<int, XmlElement>();
						Dictionary<int, XmlElement> dicMPV = new Dictionary<int, XmlElement>();
						Dictionary<int, XmlElement> dicPaoChe = new Dictionary<int, XmlElement>();
						Dictionary<int, XmlElement> dicQiTa = new Dictionary<int, XmlElement>();

						XmlNodeList xnlCsPrice = xmlDoc.SelectNodes("/Params/CsPrice");
						if (xnlCsPrice != null && xnlCsPrice.Count > 0)
						{
							// 循环价格区间 0-8
							int type = 0;
							foreach (XmlNode xnCsPrice in xnlCsPrice)
							{
								Dictionary<int, XmlElement> dicCsPrice = new Dictionary<int, XmlElement>();
								if (xnCsPrice != null && xnCsPrice.HasChildNodes)
								{
									foreach (XmlElement xe in xnCsPrice.ChildNodes)
									{
										int csid = int.Parse(xe.GetAttribute("ID"));
										if (!ht.ContainsKey(csid.ToString()))
										{ continue; }
										if (!dicCsPrice.ContainsKey(csid))
										{ dicCsPrice.Add(csid, xe); }

										// 如果是SUV
										if (xe.GetAttribute("CsLevel") == "SUV" && !dicSUV.ContainsKey(csid))
										{ dicSUV.Add(csid, xe); }
										// 如果是MPV
										if (xe.GetAttribute("CsLevel") == "MPV" && !dicMPV.ContainsKey(csid))
										{ dicMPV.Add(csid, xe); }
										// 如果是跑车
										if (xe.GetAttribute("CsLevel") == "跑车" && !dicPaoChe.ContainsKey(csid))
										{ dicPaoChe.Add(csid, xe); }
										// 如果是其他
										if (xe.GetAttribute("CsLevel") == "其它" && !dicQiTa.ContainsKey(csid))
										{ dicQiTa.Add(csid, xe); }
									}
								}
								// 将价格区间加入结果集
								GetSomeLevelToListForPhotoCompare(dic, type, dicCsPrice);
								type++;
							}
							#region 将 SUV MPV 跑车 其他 加入结果集
							// 将 SUV MPV 跑车 其他 加入结果集
							GetSomeLevelToListForPhotoCompare(dic, 9, dicSUV);
							GetSomeLevelToListForPhotoCompare(dic, 10, dicMPV);
							GetSomeLevelToListForPhotoCompare(dic, 11, dicPaoChe);
							GetSomeLevelToListForPhotoCompare(dic, 12, dicQiTa);
							#endregion
						}
					}
				}
				CacheManager.InsertCache(cacheKey, dic, 60);
			}
			else
			{ dic = (Dictionary<int, List<XmlElement>>)getAllSerialForPingCeCompareListNew; }
			return dic;
		}

		/// <summary>
		/// 评测对比页选车工具
		/// </summary>
		/// <returns></returns>
		public string GetAllSerialForPingCeCompareList()
		{
			Hashtable ht = GetPingCeCompareSerialList();
			if (ht == null || ht.Count < 1)
			{
				return "";
			}
			Hashtable htSUV = new Hashtable();
			Hashtable htMPV = new Hashtable();
			Hashtable htPao = new Hashtable();
			StringBuilder sb = new StringBuilder();
			StringBuilder sbLiangXiang = new StringBuilder();
			StringBuilder sbShanXiang = new StringBuilder();
			StringBuilder sbQiTa = new StringBuilder();
			StringBuilder sbSUV = new StringBuilder();
			StringBuilder sbMPV = new StringBuilder();
			StringBuilder sbPaoChe = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<Params Time=\"" + DateTime.Now.ToString() + "\">");
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.Load(WebConfig.PriceRangeSerial);
				if (xmlDoc != null && xmlDoc.HasChildNodes)
				{
					XmlNodeList xnl = xmlDoc.SelectNodes("/Params/CsPrice");
					if (xnl != null && xnl.Count > 0)
					{
						sbSUV.Append("<Level1Group Name=\"SUV\" >");
						sbSUV.Append("<Level2Group Name=\"SUV\" >");
						sbMPV.Append("<Level1Group Name=\"MPV\" >");
						sbMPV.Append("<Level2Group Name=\"MPV\" >");
						sbPaoChe.Append("<Level1Group Name=\"跑车\" >");
						sbPaoChe.Append("<Level2Group Name=\"跑车\" >");
						for (int i = 0; i < xnl.Count; i++)
						{
							if (xnl[i].Attributes["PriceRange"].Value != "无价格")
							{
								sb.Append("<Level1Group Name=\"" + xnl[i].Attributes["PriceRange"].Value.Replace("-", "万-") + "\">");
								if (xnl[i].HasChildNodes)
								{
									sbLiangXiang.Append("<Level2Group Name=\"两厢轿车\" >");
									sbShanXiang.Append("<Level2Group Name=\"三厢轿车\" >");
									sbQiTa.Append("<Level2Group Name=\"其它\" >");
									foreach (XmlNode xn in xnl[i])
									{
										// 判断是否在审核列表
										if (!ht.ContainsKey(xn.Attributes["ID"].Value.Trim()))
										{
											continue;
										}

										#region 厢式
										if (xn.Attributes["BodyType"].Value.Trim() == "两厢")
										{
											sbLiangXiang.Append(xn.OuterXml);
										}
										else if (xn.Attributes["BodyType"].Value.Trim() == "三厢")
										{
											sbShanXiang.Append(xn.OuterXml);
										}
										else
										{
											sbQiTa.Append(xn.OuterXml);
										}
										#endregion

										#region SUV MPV 跑车级别
										if (xn.Attributes["CsLevel"].Value.Trim() == "SUV")
										{
											if (!htSUV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
											{
												htSUV.Add(xn.Attributes["ID"].Value.Trim(), 1);
												sbSUV.Append(xn.OuterXml);
											}
										}
										if (xn.Attributes["CsLevel"].Value.Trim() == "MPV")
										{
											if (!htMPV.ContainsKey(xn.Attributes["ID"].Value.Trim()))
											{
												htMPV.Add(xn.Attributes["ID"].Value.Trim(), 1);
												sbMPV.Append(xn.OuterXml);
											}
										}
										if (xn.Attributes["CsLevel"].Value.Trim() == "跑车")
										{
											if (!htPao.ContainsKey(xn.Attributes["ID"].Value.Trim()))
											{
												htPao.Add(xn.Attributes["ID"].Value.Trim(), 1);
												sbPaoChe.Append(xn.OuterXml);
											}
										}
										#endregion
									}
									sbLiangXiang.Append("</Level2Group>");
									sbShanXiang.Append("</Level2Group>");
									sbQiTa.Append("</Level2Group>");
									sb.Append(sbLiangXiang.ToString());
									sb.Append(sbShanXiang.ToString());
									sb.Append(sbQiTa.ToString());
									sbLiangXiang.Remove(0, sbLiangXiang.Length);
									sbShanXiang.Remove(0, sbShanXiang.Length);
									sbQiTa.Remove(0, sbQiTa.Length);
								}
								sb.Append("</Level1Group>");
							}
						}
						sbSUV.Append("</Level2Group>");
						sbMPV.Append("</Level2Group>");
						sbPaoChe.Append("</Level2Group>");
						sbSUV.Append("</Level1Group>");
						sbMPV.Append("</Level1Group>");
						sbPaoChe.Append("</Level1Group>");
						sb.Append(sbSUV.ToString());
						sb.Append(sbMPV.ToString());
						sb.Append(sbPaoChe.ToString());
					}
				}
			}
			catch
			{ }
			sb.Append("</Params>");
			return sb.ToString();
		}

		/// <summary>
		/// 对比页选车工具
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetAllSerialPriceRangeForCompareList()
		{
			XmlDocument xmlResult = new XmlDocument();
			string cachekeyResult = "GetAllSerialPriceRangeForCompareList";
			object getAllSerialPriceRangeForCompareList = null;
			CacheManager.GetCachedData(cachekeyResult, out getAllSerialPriceRangeForCompareList);
			if (getAllSerialPriceRangeForCompareList == null)
			{
				StringBuilder sb = new StringBuilder();
				StringBuilder sbLiangXiang = new StringBuilder();
				StringBuilder sbShanXiang = new StringBuilder();
				StringBuilder sbQiTa = new StringBuilder();
				StringBuilder sbSUV = new StringBuilder();
				StringBuilder sbMPV = new StringBuilder();
				StringBuilder sbPaoChe = new StringBuilder();
				sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
				sb.Append("<Params Time=\"" + DateTime.Now.ToString() + "\">");
				XmlDocument xmlDoc = new XmlDocument();
				try
				{
					string cachekey = "AllPriceRangeSerialForCompare";
					object allPriceRangeSerialForCompare = null;
					CacheManager.GetCachedData(cachekey, out allPriceRangeSerialForCompare);
					if (allPriceRangeSerialForCompare == null)
					{
						xmlDoc.Load(WebConfig.PriceRangeSerial);
						CacheManager.InsertCache(cachekey, xmlDoc, 60);
					}
					else
					{
						xmlDoc = (XmlDocument)allPriceRangeSerialForCompare;
					}

					if (xmlDoc != null && xmlDoc.HasChildNodes)
					{
						XmlNodeList xnl = xmlDoc.SelectNodes("/Params/CsPrice");
						if (xnl != null && xnl.Count > 0)
						{
							sbSUV.Append("<Level1Group Name=\"SUV\" >");
							sbSUV.Append("<Level2Group Name=\"SUV\" >");
							sbMPV.Append("<Level1Group Name=\"MPV\" >");
							sbMPV.Append("<Level2Group Name=\"MPV\" >");
							sbPaoChe.Append("<Level1Group Name=\"跑车\" >");
							sbPaoChe.Append("<Level2Group Name=\"跑车\" >");
							for (int i = 0; i < xnl.Count; i++)
							{
								if (xnl[i].Attributes["PriceRange"].Value != "无价格")
								{
									sb.Append("<Level1Group Name=\"" + xnl[i].Attributes["PriceRange"].Value.Replace("-", "万-") + "\">");
									if (xnl[i].HasChildNodes)
									{
										sbLiangXiang.Append("<Level2Group Name=\"两厢轿车\" >");
										sbShanXiang.Append("<Level2Group Name=\"三厢轿车\" >");
										sbQiTa.Append("<Level2Group Name=\"其它轿车\" >");
										foreach (XmlNode xn in xnl[i])
										{
											#region 厢式
											if (xn.Attributes["BodyType"].Value.Trim() == "两厢")
											{
												sbLiangXiang.Append(xn.OuterXml);
											}
											else if (xn.Attributes["BodyType"].Value.Trim() == "三厢")
											{
												sbShanXiang.Append(xn.OuterXml);
											}
											else
											{
												sbQiTa.Append(xn.OuterXml);
											}
											#endregion

											#region SUV MPV 跑车级别
											if (xn.Attributes["CsLevel"].Value.Trim() == "SUV")
											{
												sbSUV.Append(xn.OuterXml);
											}
											if (xn.Attributes["CsLevel"].Value.Trim() == "MPV")
											{
												sbMPV.Append(xn.OuterXml);
											}
											if (xn.Attributes["CsLevel"].Value.Trim() == "跑车")
											{
												sbPaoChe.Append(xn.OuterXml);
											}
											#endregion
										}
										sbLiangXiang.Append("</Level2Group>");
										sbShanXiang.Append("</Level2Group>");
										sbQiTa.Append("</Level2Group>");
										sb.Append(sbLiangXiang.ToString());
										sb.Append(sbShanXiang.ToString());
										sb.Append(sbQiTa.ToString());
										sbLiangXiang.Remove(0, sbLiangXiang.Length);
										sbShanXiang.Remove(0, sbShanXiang.Length);
										sbQiTa.Remove(0, sbQiTa.Length);
									}
									sb.Append("</Level1Group>");
								}
							}
							sbSUV.Append("</Level2Group>");
							sbMPV.Append("</Level2Group>");
							sbPaoChe.Append("</Level2Group>");
							sbSUV.Append("</Level1Group>");
							sbMPV.Append("</Level1Group>");
							sbPaoChe.Append("</Level1Group>");
							sb.Append(sbSUV.ToString());
							sb.Append(sbMPV.ToString());
							sb.Append(sbPaoChe.ToString());
						}
					}
				}
				catch
				{ }
				sb.Append("</Params>");
				xmlResult.LoadXml(sb.ToString());

				// xmlDoc.Load(WebConfig.PriceRangeSerial);
				CacheManager.InsertCache(cachekeyResult, xmlResult, 20);
			}
			else
			{
				xmlResult = (XmlDocument)getAllSerialPriceRangeForCompareList;
			}
			return xmlResult;
		}

		/// <summary>
		/// 取子品牌下车型(对比页)
		/// </summary>
		/// <returns></returns>
		public string GetAllSerialToCarForCompare()
		{
			StringBuilder sb = new StringBuilder();
			XmlDocument xmlDoc = new XmlDocument();
			string cachekey = "SerialToCarForCompare";
			object SerialToCarForCompare = null;
			CacheManager.GetCachedData(cachekey, out SerialToCarForCompare);

			if (SerialToCarForCompare == null)
			{
				try
				{
					xmlDoc.Load(WebConfig.SerialToCar);
					sb.Append(xmlDoc.InnerXml);
					CacheManager.InsertCache(cachekey, xmlDoc, 60);
				}
				catch
				{
				}
			}
			else
			{
				xmlDoc = (XmlDocument)SerialToCarForCompare;
				sb.Append(xmlDoc.InnerXml);
			}
			return sb.ToString();
		}

		/// <summary>
		/// 取主品牌到车型数据
		/// </summary>
		/// <returns></returns>
		public string GetMasterToCar()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<Params Time=\"" + DateTime.Now.ToString() + "\">");
			DataSet ds = new DataSet();
			string cachekey = "MasterToCarForSelectList";
			object MasterToCarForSelectList = null;
			CacheManager.GetCachedData(cachekey, out MasterToCarForSelectList);
			if (MasterToCarForSelectList == null)
			{
				string sql = " select cmb.bs_Id,cmb.bs_Name,cs.cs_id,cs.cs_name,cs.cs_ShowName,cs.allSpell as csAllSpell, ";
				sql += " car.car_id,car.car_name,car.Car_YearType,cmb.spell,cmb.spell as bsspell from dbo.Car_Basic car ";
				sql += " left join Car_serial cs on car.cs_id = cs.cs_id ";
				sql += " left join Car_brand cb on cs.cb_id = cb.cb_id ";
				sql += " left join dbo.Car_MasterBrand_Rel cmbr on cb.cb_id=cmbr.cb_Id ";
				sql += " left join dbo.Car_MasterBrand cmb on cmbr.bs_Id = cmb.bs_Id ";
				sql += " where car.isState=1 and cs.isState=1 and cb.isState=1 and cmb.IsState=1 ";
				sql += " order by cmb.spell,cmb.bs_Id,cs.allSpell,car.car_name ";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				CacheManager.InsertCache(cachekey, ds, 60);
			}
			else
			{
				ds = (DataSet)MasterToCarForSelectList;
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				string currentBsID = "0";
				string currentCsID = "0";
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					// 不同主品牌
					if (currentBsID != ds.Tables[0].Rows[i]["bs_id"].ToString())
					{
						if (currentBsID != "0")
						{
							sb.Append("</Serial>");
							sb.Append("</Master>");
						}
						sb.Append("<Master ID=\"" + ds.Tables[0].Rows[i]["bs_id"].ToString() + "\" ");
						sb.Append(" Name=\"" + ds.Tables[0].Rows[i]["bs_Name"].ToString() + "\" ");
						sb.Append(" Spell=\"" + ds.Tables[0].Rows[i]["bsspell"].ToString().Substring(0, 1) + "\" >");

						sb.Append("<Serial ID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
						sb.Append(" CsName=\"" + ds.Tables[0].Rows[i]["cs_Name"].ToString() + "\" ");
						sb.Append(" CsShowName=\"" + ds.Tables[0].Rows[i]["cs_ShowName"].ToString() + "\" ");
						sb.Append(" CsAllSpell=\"" + ds.Tables[0].Rows[i]["csAllSpell"].ToString() + "\" >");
					}
					else
					{
						// 同一主品牌下不同子品牌
						if (currentCsID != ds.Tables[0].Rows[i]["cs_id"].ToString())
						{
							// 不同子品牌
							if (currentCsID != "0")
							{
								sb.Append("</Serial>");
							}
							sb.Append("<Serial ID=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
							sb.Append(" CsName=\"" + ds.Tables[0].Rows[i]["cs_Name"].ToString() + "\" ");
							sb.Append(" CsShowName=\"" + ds.Tables[0].Rows[i]["cs_ShowName"].ToString() + "\" ");
							sb.Append(" CsAllSpell=\"" + ds.Tables[0].Rows[i]["csAllSpell"].ToString() + "\" >");
						}
					}
					sb.Append("<Car ID=\"" + ds.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					sb.Append(" CarName=\"" + ds.Tables[0].Rows[i]["car_name"].ToString() + "\" ");
					if (ds.Tables[0].Rows[i]["Car_YearType"].ToString().Length >= 4)
					{
						sb.Append(" CarYear=\"" + ds.Tables[0].Rows[i]["Car_YearType"].ToString().Substring(2, 2) + "款\" />");
					}
					else
					{
						sb.Append(" CarYear=\"\" />");
					}

					currentBsID = ds.Tables[0].Rows[i]["bs_id"].ToString();
					currentCsID = ds.Tables[0].Rows[i]["cs_id"].ToString();
				}
				if (sb.Length > 0)
				{
					sb.Append("</Serial>");
					sb.Append("</Master>");
				}
			}
			sb.Append("</Params>");
			return sb.ToString();
		}

		/// <summary>
		/// 取子品牌图片对比数据
		/// </summary>
		/// <param name="csid">子品牌ID</param>
		/// <param name="minTime">缓存时间</param>
		/// <returns></returns>
		public DataSet GetSerialPhotoDataForCompare(int csid, int minTime)
		{
			DataSet ds = new DataSet();
			string cachekey = "SerialPhotoForCompare_" + csid.ToString();
			object photoDataForCompare = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out photoDataForCompare);
				if (photoDataForCompare == null)
				{
					//图库接口本地化更改 by sk 2012.12.21
					string photoComparePath = Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialPhotoComparePath);
					ds.ReadXml(string.Format(photoComparePath, csid.ToString()));
					//ds.ReadXml(string.Format(WebConfig.PhotoCompareService, csid.ToString()));
					CacheManager.InsertCache(cachekey, ds, minTime);
				}
				else
				{
					ds = (DataSet)photoDataForCompare;
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return ds;
		}

		/// <summary>
		/// 获取 车款 图片对比数据 add by sk 2014.08.08
		/// </summary>
		/// <param name="carId"></param>
		/// <param name="minTime"></param>
		/// <returns></returns>
		public DataSet GetCarPhotoDataForCompare(int carId, int minTime)
		{
			DataSet ds = new DataSet();
			string cachekey = "GetCarPhotoDataForCompare_" + carId;
			object photoDataForCompare = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out photoDataForCompare);
				if (photoDataForCompare == null)
				{
					//图库接口本地化更改 by sk 2012.12.21
					string photoComparePath = string.Format(Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.CarPhotoComparePath), carId);
					if (File.Exists(photoComparePath))
					{
						ds.ReadXml(string.Format(photoComparePath, carId));
						CacheManager.InsertCache(cachekey, ds, minTime);
					}
				}
				else
				{
					ds = (DataSet)photoDataForCompare;
				}
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return ds;
		}

		/// <summary>
		/// 取所有主品牌(车型对比页)
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllMasterBrand(int minTime)
		{
			DataSet ds = new DataSet();
			string cachekey = "GetAllMasterBrand";
			object getAllMasterBrand = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out getAllMasterBrand);
				if (getAllMasterBrand == null)
				{
					// string sql = " select bs_id,left(spell,1) + ' ' + bs_name as msname,left(spell,1) as mspell from dbo.Car_MasterBrand where isState=1 order by mspell,bs_id ";
					string sql = " select cmb.bs_id,left(cmb.spell,1) + ' ' + cmb.bs_name as msname,left(cmb.spell,1) as mspell ";
					sql += " from ( ";
					sql += " select cmb.bs_id ";
					sql += " from car_basic car ";
					sql += " left join car_serial cs on car.cs_id = cs.cs_id ";
					sql += " left join Car_brand cb on cs.cb_id=cb.cb_id ";
					sql += " left join dbo.Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id ";
					sql += " left join dbo.Car_MasterBrand cmb on cmbr.bs_id = cmb.bs_id ";
					sql += " where cs.isState=1 and cb.isState=1 and cs.CsSaleState<>'停销' and car.Car_SaleState<>'停销' ";
					sql += " group by cmb.bs_id ";
					sql += " ) t1 ";
					sql += " left join Car_MasterBrand cmb on t1.bs_id=cmb.bs_id ";
					sql += " order by cmb.spell,bs_id ";
					ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
					CacheManager.InsertCache(cachekey, ds, minTime);
				}
				else
				{
					ds = (DataSet)getAllMasterBrand;
				}
			}
			catch
			{ }
			return ds;
		}

		/// <summary>
		/// 取所有子品牌(车型对比页)
		/// </summary>
		/// <returns></returns>
		public DataSet GetAllSerialForAjaxCompare(int minTime)
		{
			DataSet ds = new DataSet();
			string cachekey = "GetAllSerialForAjaxCompare";
			object getAllSerialForAjaxCompare = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out getAllSerialForAjaxCompare);
				if (getAllSerialForAjaxCompare == null)
				{
					string sql = @"SELECT  cs.cs_id, cs.cs_name, cs.cs_showname, cs.allspell, cmb.bs_id, cb.cb_id,
                                            cb.cb_name, (CASE cb.cb_Country
                                                           WHEN '中国' THEN 0
                                                            ELSE 1
                                                          END ) AS CpCountry
                                    FROM dbo.Car_Basic car
                                            LEFT JOIN dbo.Car_Serial cs ON car.cs_id = cs.cs_id
                                            LEFT JOIN dbo.Car_Brand cb ON cs.cb_id = cb.cb_id
                                            LEFT JOIN dbo.Car_MasterBrand_Rel cmbr ON cb.cb_id = cmbr.cb_id
                                            LEFT JOIN dbo.Car_MasterBrand cmb ON cmbr.bs_id = cmb.bs_id
                                    WHERE car.isState = 1
                                            AND cs.isState = 1
                                            AND cb.isState = 1
                                            AND car.Car_SaleState <> '停销'
                                            AND cs.CsSaleState <> '停销'
                                            AND cmb.bs_id IS NOT NULL
                                    ORDER BY cmb.bs_id, CpCountry, cb.cb_id, cs.allspell";
					ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
					CacheManager.InsertCache(cachekey, ds, minTime);
				}
				else
				{
					ds = (DataSet)getAllSerialForAjaxCompare;
				}
			}
			catch
			{ }
			return ds;
		}

		/// <summary>
		/// 取所有车型(车型对比页)
		/// </summary>
		/// <param name="minTime"></param>
		/// <returns></returns>
		public DataSet GetAllCarForAjaxCompare(int minTime)
		{
			DataSet ds = new DataSet();
			string cachekey = "GetAllCarForAjaxCompare";
			object getAllCarForAjaxCompare = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out getAllCarForAjaxCompare);
				if (getAllCarForAjaxCompare == null)
				{
					//string sql = " select car.car_id,car.car_name,cs.cs_id ";
					//sql += " from car_basic car ";
					//sql += " left join car_serial cs on car.cs_id=cs.cs_id ";
					//sql += " where car.isState=1 and cs.isState=1 order by cs_id,car_name ";
					string sql = " select car.car_id,car.car_name,cs.cs_id,car.Car_SaleState, ";
					sql += " (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust ";
					sql += " ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 ";
					sql += " when cei.UnderPan_TransmissionType like '%自动' then 2 ";
					sql += " when cei.UnderPan_TransmissionType like '%手自一体' then 3  ";
					sql += " else 4 end) as TransmissionType,cei.UnderPan_TransmissionType AS TT ";
					sql += " ,car.car_ReferPrice ";
					sql += " from car_basic car ";
					sql += " left join car_serial cs on car.cs_id=cs.cs_id ";
					sql += " left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id ";
					sql += " where car.isState=1 and cs.isState=1 and car.Car_SaleState<>'停销' ";
					sql += " order by cs_id,car.Car_YearType desc,cei.Engine_Exhaust,TransmissionType,car.car_ReferPrice ";
					ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
					CacheManager.InsertCache(cachekey, ds, minTime);
				}
				else
				{
					ds = (DataSet)getAllCarForAjaxCompare;
				}
			}
			catch
			{ }
			return ds;
		}
		/// <summary>
		/// 取所有车型包括停销车型(级联菜单)
		/// </summary>
		/// <param name="minTime"></param>
		/// <returns></returns>
		public DataSet GetAllCarForAjaxCompareContainsStopSale(int minTime)
		{
			DataSet ds = new DataSet();
			string cachekey = "GetAllCarForAjaxCompareContainsStopSale";
			object getAllCarForAjaxCompare = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out getAllCarForAjaxCompare);
				if (getAllCarForAjaxCompare == null)
				{
					string sql = " select car.car_id,car.car_name,cs.cs_id,car.Car_SaleState, ";
					sql += " (case when car.Car_YearType is null then '无' else CONVERT(varchar(50),car.Car_YearType)+ ' 款' end) as Car_YearType,cei.Engine_Exhaust ";
					sql += " ,(case when cei.UnderPan_TransmissionType like '%手动' then 1 ";
					sql += " when cei.UnderPan_TransmissionType like '%自动' then 2 ";
					sql += " when cei.UnderPan_TransmissionType like '%手自一体' then 3  ";
					sql += " else 4 end) as TransmissionType,cei.UnderPan_TransmissionType AS TT ";
					sql += " ,car.car_ReferPrice ";
					sql += " from car_basic car ";
					sql += " left join car_serial cs on car.cs_id=cs.cs_id ";
					sql += " left join dbo.Car_Extend_Item cei on car.car_id = cei.car_id ";
					sql += " where car.isState=1 and cs.isState=1 ";
					sql += " order by cs_id,car.Car_YearType desc,cei.Engine_Exhaust,TransmissionType,car.car_ReferPrice ";
					ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
					CacheManager.InsertCache(cachekey, ds, minTime);
				}
				else
				{
					ds = (DataSet)getAllCarForAjaxCompare;
				}
			}
			catch
			{ }
			return ds;
		}


		/// <summary>
		/// 取子品牌图片对比模板
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetPhotoCompareTemplate()
		{
			XmlDocument doc = new XmlDocument();
			string cachekey = "PhotoCompareTemplate";
			object photoCompareTemplate = null;
			try
			{
				CacheManager.GetCachedData(cachekey, out photoCompareTemplate);
				if (photoCompareTemplate == null)
				{
					string file = HttpContext.Current.Server.MapPath("~") + "\\config\\PhotoCompareStruct.xml";
					// string file = Path.Combine(WebConfig.DataBlockPath, "Data\\PhotoCompareStruct.xml");
					doc.Load(file);
					System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(file);
					CacheManager.InsertCache(cachekey, doc, dep, System.Web.Caching.Cache.NoAbsoluteExpiration);
				}
				else
				{
					doc = (XmlDocument)photoCompareTemplate;
				}
			}
			catch
			{ }

			return doc;
		}
		/// <summary>
		/// 获取 图片对比 子品牌 及 车款
		/// </summary>
		/// <returns></returns>
		public Dictionary<int, List<int>> GetPhotoCompareSerialAndCarList()
		{
			string cacheKey = "Car_CommonService_GetPhotoCompareSerialAndCarList";
			object getPhotoCompareSerialList = null;
			CacheManager.GetCachedData(cacheKey, out getPhotoCompareSerialList);
			if (getPhotoCompareSerialList != null)
				return (Dictionary<int, List<int>>)getPhotoCompareSerialList;

			Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
			try
			{
				//图库接口本地化更改 by sk 2012.12.21
				string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialComparePath);
				XmlDocument xmlDoc = CommonFunction.ReadXml(xmlFile);
				XmlNodeList nodeList = xmlDoc.SelectNodes("//Serial");
				foreach (XmlNode node in nodeList)
				{
					int serialId = ConvertHelper.GetInteger(node.Attributes["ID"].Value);
					List<int> carList = new List<int>();
					XmlNodeList nodeCarList = node.SelectNodes("./CarModel");
					foreach (XmlNode nodeCar in nodeCarList)
					{
						int carId = ConvertHelper.GetInteger(nodeCar.Attributes["ID"].Value);
						carList.Add(carId);
					}
					if (!dict.ContainsKey(serialId))
						dict.Add(serialId, carList);
				}
				CacheManager.InsertCache(cacheKey, dict, 60);
			}
			catch (Exception ex)
			{
				CommonFunction.WriteLog(ex.ToString());
			}
			return dict;
		}

		///// <summary>
		///// 取审核通过的图片对比子品牌
		///// </summary>
		///// <returns></returns>
		//public Hashtable GetPhotoCompareSerialList()
		//{
		//	Hashtable ht = new Hashtable();
		//	DataSet ds = new DataSet();
		//	string cachekey = "GetPhotoCompareSerialList";
		//	object getPhotoCompareSerialList = null;
		//	CacheManager.GetCachedData(cachekey, out getPhotoCompareSerialList);
		//	if (getPhotoCompareSerialList == null)
		//	{
		//		try
		//		{
		//			//图库接口本地化更改 by sk 2012.12.21
		//			string xmlFile = System.IO.Path.Combine(PhotoImageConfig.SavePath, PhotoImageConfig.SerialComparePath);
		//			ds.ReadXml(xmlFile);
		//			//ds.ReadXml(WebConfig.PhotoCompareSerialList);
		//			CacheManager.InsertCache(cachekey, ds, 10);
		//		}
		//		catch
		//		{ }
		//	}
		//	else
		//	{
		//		ds = (DataSet)getPhotoCompareSerialList;
		//	}
		//	if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		//	{
		//		for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
		//		{
		//			if (!ht.ContainsKey(ds.Tables[0].Rows[i]["SerialId"].ToString()))
		//			{
		//				ht.Add(ds.Tables[0].Rows[i]["SerialId"].ToString(), 0);
		//			}
		//		}
		//	}
		//	return ht;
		//}

		/// <summary>
		/// 取审有评测对比文章子品牌
		/// </summary>
		/// <returns></returns>
		public Hashtable GetPingCeCompareSerialList()
		{
			Hashtable ht = new Hashtable();
			string cachekey = "AllPingCeNewsURL";
			object allPingCeNewsURL = null;
			DataSet ds = new DataSet();
			CacheManager.GetCachedData(cachekey, out allPingCeNewsURL);
			if (allPingCeNewsURL == null)
			{
				string sql = " select csid,url from dbo.RainbowEdit where RainbowItemID = 43 ";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				CacheManager.InsertCache(cachekey, ds, 60);
			}
			else
			{
				ds = (DataSet)allPingCeNewsURL;
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					if (!ht.ContainsKey(ds.Tables[0].Rows[i]["csid"].ToString()))
					{
						ht.Add(ds.Tables[0].Rows[i]["csid"].ToString(), ds.Tables[0].Rows[i]["url"].ToString());
					}
				}
			}
			return ht;
		}

		/// <summary>
		/// 是否是在销子品牌 并且有在销车型
		/// </summary>
		/// <param name="csID"></param>
		/// <returns></returns>
		public bool IsHasCarSerial(int csID)
		{
			bool isHas = false;
			string cachekey = "CommonService_IsHasCarSerial";
			object commonService_IsHasCarSerial = null;
			DataSet ds = new DataSet();
			CacheManager.GetCachedData(cachekey, out commonService_IsHasCarSerial);
			if (commonService_IsHasCarSerial == null)
			{
				string sql = " select cs.cs_id ";
				sql += " from Car_basic car ";
				sql += " left join Car_Serial cs on car.cs_id=cs.cs_id ";
				sql += " where car.isState = 1 and cs.isState = 1 and car.Car_SaleState<>'停销' and cs.CsSaleState<>'停销' ";
				sql += " group by cs.cs_id ";
				ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				CacheManager.InsertCache(cachekey, ds, 60);
			}
			else
			{
				ds = (DataSet)commonService_IsHasCarSerial;
			}

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select(" cs_id = '" + csID.ToString() + "' ");
				if (drs != null && drs.Length > 0)
				{
					isHas = true;
				}
			}
			return isHas;
		}

	}
}
