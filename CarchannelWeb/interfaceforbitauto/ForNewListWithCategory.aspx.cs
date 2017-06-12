using System;
using System.IO;
using System.Data;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.CarchannelWeb.App_Code;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	/// <summary>
	///  买车 各列表
	/// </summary>
	public partial class ForNewListWithCategory : InterfacePageBase
	{
		#region param
		private string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\autodata.xml");
		// private string xmlPath = Path.Combine(WebConfig.DataBlockPath, "Data\\AllAutoData.xml");//"http://carser.bitauto.com/forpicmastertoserial/MasterToBrandToSerialAllSale.xml";
		private XmlDocument mbDoc = new XmlDocument();
		private Hashtable ht = new Hashtable();
		private Hashtable htBrand = new Hashtable();
		private Hashtable htMaster = new Hashtable();
		private StringBuilder sb = new StringBuilder();
		private int cate = 1;
		// 安全 导购 科技 评测 维修养护 装饰改装
		private int rainbowID = 44;
		private int tag = 1;
		// tag:按字母 按品牌 按国别 按级别 按价格 按用途
		private string navHtml = string.Empty;
		protected string html = string.Empty;
		int masterBrandNum = 0;
		int masterCounter = 0;

		Dictionary<string, string> levelLabelDic;	//级别字典
		Dictionary<int, string> priceDic;			//报价字典
		bool isFirstPrice;							//是否是页面上第一个报价
		bool isLastLevel;							//是否是页面上最后一个级别，为了生成最后的<div class="hideline"></div>

		Dictionary<int, string> priceIdDic;		//报价ID字典
		Dictionary<int, string> priceTextDic;	//报价文本字典
		bool isLastPrice;						//是否是页面上最后一个报价，为了生成最后的<div class="hideline"></div>

		Dictionary<string, string> functionLabelDic;	//级别字典
		// Dictionary<int, string> priceDic;				//报价字典
		// bool isFirstPrice;								//是否是页面上第一个报价
		bool isLastFunction;								//是否是页面上最后一个功能，为了生成最后的<div class="hideline"></div>
		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			// Response.Charset = "GB2312";
			if (!this.IsPostBack)
			{
				GetParam();
				GetSerialRainbowData();
				GetPageHTML();
			}
			// Response.Write(navHtml + sb.ToString());
			html = navHtml + sb.ToString();
		}

		// 取参数
		private void GetParam()
		{
			if (this.Request.QueryString["cate"] != null && this.Request.QueryString["cate"].ToString() != "")
			{
				string strCate = this.Request.QueryString["cate"].ToString();
				if (int.TryParse(strCate, out cate))
				{
					if (cate < 1 || cate > 6)
					{
						cate = 1;
					}
				}
				switch (cate)
				{
					// 安全
					case 1: rainbowID = 44; break;
					// 导购
					case 2: rainbowID = 57; break;
					// 科技
					case 3: rainbowID = 41; break;
					// 评测(易车评测)
					case 4: rainbowID = 43; break;
					// 维修养护
					case 5: rainbowID = 40; break;
					// 装饰改装
					case 6: rainbowID = 58; break;
					default: rainbowID = 44; break;
				}
			}

			if (this.Request.QueryString["tag"] != null && this.Request.QueryString["tag"].ToString() != "")
			{
				string strTag = this.Request.QueryString["tag"].ToString().ToLower();
				if (int.TryParse(strTag, out tag))
				{
					if (tag < 1 || tag > 6)
					{
						tag = 1;
					}
				}
			}
		}

		// 取彩虹条维护
		private void GetSerialRainbowData()
		{
			if (cate == 2)
			{
				// 导购
				string sql = " select cs.cs_ID as csID,'http://car.bitauto.com/'+LOWER(cs.allSpell)+'/daogou/' as url,cb.cb_id,cmb.bs_id ";
				sql += " from car_serial cs ";
				sql += " left join car_brand cb on cs.cb_id = cb.cb_id ";
				sql += " left join Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id ";
				sql += " left join Car_MasterBrand cmb on cmbr.bs_id=cmb.bs_id ";
				sql += " where cs.isState=1 and cb.isState=1 ";
				DataSet dsRain = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				if (dsRain != null && dsRain.Tables.Count > 0 && dsRain.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsRain.Tables[0].Rows.Count; i++)
					{
						if (!ht.ContainsKey(dsRain.Tables[0].Rows[i]["csID"].ToString()))
						{
							ht.Add(dsRain.Tables[0].Rows[i]["csID"].ToString(), dsRain.Tables[0].Rows[i]["url"].ToString().ToLower());
						}
						if (!htBrand.ContainsKey(dsRain.Tables[0].Rows[i]["cb_id"].ToString()))
						{
							htBrand.Add(dsRain.Tables[0].Rows[i]["cb_id"].ToString(), 1);
						}
						if (!htMaster.ContainsKey(dsRain.Tables[0].Rows[i]["bs_id"].ToString()))
						{
							htMaster.Add(dsRain.Tables[0].Rows[i]["bs_id"].ToString(), 1);
						}
					}
				}
			}
			else
			{
				// string sql = " select csID,URL from RainbowEdit  where RainbowItemID= " + rainbowID.ToString();
				string sql = "";
				if (rainbowID == 43)
				{
					// 评测
					sql = " select re.csID,'http://car.bitauto.com/'+LOWER(cs.allSpell)+'/pingce/' as url,cb.cb_id,cmb.bs_id ";
					sql += " from RainbowEdit re ";
					sql += " left join car_serial cs on re.csid = cs.cs_id ";
					sql += " left join car_brand cb on cs.cb_id = cb.cb_id ";
					sql += " left join Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id ";
					sql += " left join Car_MasterBrand cmb on cmbr.bs_id=cmb.bs_id ";
					sql += " where RainbowItemID=43";
				}
				else
				{
					// 非评测
					sql = " select re.csID,re.URL,cb.cb_id,cmb.bs_id ";
					sql += " from RainbowEdit re ";
					sql += " left join car_serial cs on re.csid = cs.cs_id ";
					sql += " left join car_brand cb on cs.cb_id = cb.cb_id ";
					sql += " left join Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id ";
					sql += " left join Car_MasterBrand cmb on cmbr.bs_id=cmb.bs_id ";
					sql += " where RainbowItemID=" + rainbowID.ToString();
				}
				DataSet dsRain = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
				if (dsRain != null && dsRain.Tables.Count > 0 && dsRain.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsRain.Tables[0].Rows.Count; i++)
					{
						if (!ht.ContainsKey(dsRain.Tables[0].Rows[i]["csID"].ToString()))
						{
							ht.Add(dsRain.Tables[0].Rows[i]["csID"].ToString(), dsRain.Tables[0].Rows[i]["url"].ToString());
						}
						if (!htBrand.ContainsKey(dsRain.Tables[0].Rows[i]["cb_id"].ToString()))
						{
							htBrand.Add(dsRain.Tables[0].Rows[i]["cb_id"].ToString(), 1);
						}
						if (!htMaster.ContainsKey(dsRain.Tables[0].Rows[i]["bs_id"].ToString()))
						{
							htMaster.Add(dsRain.Tables[0].Rows[i]["bs_id"].ToString(), 1);
						}
					}
				}

				#region 如果是评测 检查是否有试驾文章
				// 如果是评测 检查是否有试驾文章
				if (rainbowID == 43)
				{
					string sqlSaleShijia = @" select cs.cs_id,cs.allSpell,cb.cb_id,cmb.bs_id
from car_serial cs
left join car_brand cb on cs.cb_id = cb.cb_id
left join Car_MasterBrand_Rel cmbr on cb.cb_id = cmbr.cb_id
left join Car_MasterBrand cmb on cmbr.bs_id = cmb.bs_id
where cs.isState=1 and cb.isState=1 and cs.CsSaleState<>'停销' ";

					DataSet dsIsHasShijia = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlSaleShijia);
					if (dsIsHasShijia != null && dsIsHasShijia.Tables.Count > 0 && dsIsHasShijia.Tables[0].Rows.Count > 0)
					{
						string filePath = Path.Combine(WebConfig.DataBlockPath, "Data\\SerialNews\\shijia\\Serial_All_News_{0}.xml");
						for (int i = 0; i < dsIsHasShijia.Tables[0].Rows.Count; i++)
						{
							// 如果已在列表中
							if (ht.ContainsKey(dsIsHasShijia.Tables[0].Rows[i]["cs_ID"].ToString()))
							{ continue; }

							if (File.Exists(string.Format(filePath, dsIsHasShijia.Tables[0].Rows[i]["cs_id"].ToString())))
							{
								DataSet dsShiJia = new DataSet();
								dsShiJia.ReadXml(string.Format(filePath, dsIsHasShijia.Tables[0].Rows[i]["cs_id"].ToString()));
								if (dsShiJia != null && dsShiJia.Tables.Count > 1)
								{
									// 有试驾文章
									if (!ht.ContainsKey(dsIsHasShijia.Tables[0].Rows[i]["cs_ID"].ToString()))
									{
										ht.Add(dsIsHasShijia.Tables[0].Rows[i]["cs_ID"].ToString(), "http://car.bitauto.com/" + dsIsHasShijia.Tables[0].Rows[i]["allSpell"].ToString() + "/shijia/");
									}
									if (!htBrand.ContainsKey(dsIsHasShijia.Tables[0].Rows[i]["cb_id"].ToString()))
									{
										htBrand.Add(dsIsHasShijia.Tables[0].Rows[i]["cb_id"].ToString(), 1);
									}
									if (!htMaster.ContainsKey(dsIsHasShijia.Tables[0].Rows[i]["bs_id"].ToString()))
									{
										htMaster.Add(dsIsHasShijia.Tables[0].Rows[i]["bs_id"].ToString(), 1);
									}
								}
							}
						}
					}
					// string filePath = Path.Combine(BlockDataPath, "SerialNews\\shijia\\Serial_All_News_" + csID.ToString() + ".xml");

				}
				#endregion
			}
		}

		// 取内容
		private void GetPageHTML()
		{
			mbDoc.Load(xmlPath);
			// mbDoc = AutoStorageService.GetAutoXml();
			// 按字母
			if (tag == 1)
			{
				RenderCharList();
			}
			// 按品牌
			if (tag == 2)
			{
				RenderBrandList();
			}
			// 按国别
			if (tag == 3)
			{
				RenderCountry();
			}
			// 按级别
			if (tag == 4)
			{
				RenderByLevel();
			}
			// 按价格
			if (tag == 5)
			{
				RenderByPrice();
			}
			// 按用途
			if (tag == 6)
			{
				RenderByFunction();
			}
		}

		#region 按用途

		// 按用途
		private void RenderByFunction()
		{
			InitDataForFunction();
			string[] funcs = new string[] { "越野", "时尚", "家用", "代步", "休闲", "运动", "商务", "cross", "多功能" };

			//获取数据xml
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有子品牌节点
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

			//将所有子品牌按用途，价格，分类列表
			Dictionary<string, Dictionary<int, List<XmlElement>>> allSerialNodes = new Dictionary<string, Dictionary<int, List<XmlElement>>>();
			foreach (XmlElement serialNode in serialNodeList)
			{
				if (!ht.ContainsKey(serialNode.GetAttribute("ID")))
				{ continue; }
				EnumCollection.SerialPurposeForInterface funComplex = (EnumCollection.SerialPurposeForInterface)Convert.ToInt32(serialNode.GetAttribute("CsPurpose"));

				string[] funcHas = funComplex.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);



				foreach (string func in funcHas)
				{
					//不在字典中则不显示
					if (!functionLabelDic.ContainsKey(func))
						continue;
					//按用途加入列表
					if (!allSerialNodes.ContainsKey(func))
						allSerialNodes[func] = new Dictionary<int, List<XmlElement>>();

					Dictionary<int, List<XmlElement>> priceElements = allSerialNodes[func];

					//按报价加入
					foreach (string priceId in prices)
					{
						int pId = Convert.ToInt32(priceId);
						if (pId == 0)
							pId = 9;
						if (!priceDic.ContainsKey(pId))
							continue;
						if (!priceElements.ContainsKey(pId))
							priceElements[pId] = new List<XmlElement>();

						priceElements[pId].Add(serialNode);
					}
				}
			}

			//生成Html
			// StringBuilder htmlCode = new StringBuilder();
			sb.AppendLine("<dl class=\"byletters_list byprice_list\">");
			bool isFirstFunc = true;
			int functionCounter = 0;
			foreach (string func in funcs)
			{
				if (!allSerialNodes.ContainsKey(func))
					continue;

				//级别个数计数，用以确定最后一个用途
				functionCounter++;
				if (allSerialNodes[func].Count == 0)
					continue;

				if (functionCounter == allSerialNodes.Count)
					isLastFunction = true;

				if (isFirstFunc)
				{
					sb.AppendLine("<dd class=\"h2\"><h2 id=\"" + functionLabelDic[func] + "\" class=\"fir\">" + func + "</h2></dd>");
					isFirstFunc = false;
				}
				else
					sb.AppendLine("<dd class=\"h2\"><h2 id=\"" + functionLabelDic[func] + "\">" + func + "</h2></dd>");

				RenderByPriceForFunction(sb, allSerialNodes[func]);
			}

			sb.AppendLine("</dl>");
			navHtml += "<div class=\"car_top_tit tit_byprice\" id=\"pageTop\">";
			navHtml += "<ul class=\"l\">";
			navHtml += "<li><a href=\"#yy\">越野</a></li>";
			navHtml += "<li><a href=\"#ss\">时尚</a></li>";
			navHtml += "<li><a href=\"#jy\">家用</a></li>";
			navHtml += "<li><a href=\"#db\">代步</a></li>";
			navHtml += "<li><a href=\"#xx\">休闲</a></li>";
			navHtml += "<li><a href=\"#yd\">运动</a></li>";
			navHtml += "<li><a href=\"#sw\">商务</a></li>";
			// navHtml += "<li><a href=\"#cr\">cross</a></li>";
			navHtml += "<li><a href=\"#dgn\">多功能</a></li>";
			navHtml += "</ul></div>";
			// return htmlCode.ToString();
		}

		// 按用途
		private void InitDataForFunction()
		{
			functionLabelDic = new Dictionary<string, string>();
			functionLabelDic["越野"] = "yy";
			functionLabelDic["时尚"] = "ss";
			functionLabelDic["家用"] = "jy";
			functionLabelDic["代步"] = "db";
			functionLabelDic["休闲"] = "xx";
			functionLabelDic["运动"] = "yd";
			functionLabelDic["商务"] = "sw";
			functionLabelDic["cross"] = "cr";
			functionLabelDic["多功能"] = "dgn";
			priceDic = new Dictionary<int, string>();
			priceDic[1] = "5万以下";
			priceDic[2] = "5万-8万";
			priceDic[3] = "8万-12万";
			priceDic[4] = "12万-18万";
			priceDic[5] = "18万-25万";
			priceDic[6] = "25万-40万";
			priceDic[7] = "40万-80万";
			priceDic[8] = "80万以上";
			priceDic[9] = "未上市";
			isFirstPrice = true;
			isLastFunction = false;
		}

		// 按用途
		private void RenderByPriceForFunction(StringBuilder htmlCode, Dictionary<int, List<XmlElement>> pricesNodes)
		{
			int priceCounter = 0;

			for (int i = 1; i <= 9; i++)
			{
				if (!pricesNodes.ContainsKey(i))
					continue;

				priceCounter++;

				if (isFirstPrice)
				{
					htmlCode.AppendLine("<dt><label>" + priceDic[i] + "</label></dt>");
					isFirstPrice = false;
				}
				else
					htmlCode.AppendLine("<dt><label>" + priceDic[i] + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");

				htmlCode.AppendLine("<dd>");

				// new Car_SerialBll().RenderSerialsByPv(htmlCode, pricesNodes[i], true);
				RenderSerials(ref htmlCode, pricesNodes[i], "PV", true, true);

				//最后一个级别和最后一个报价才有这个
				if (isLastFunction && priceCounter == pricesNodes.Count)
				{
					htmlCode.AppendLine("<div class=\"hideline\"></div>");
				}
				htmlCode.AppendLine("</dd>");

			}
		}

		#endregion

		#region 按价格

		// 按价格
		private void RenderByPrice()
		{
			InitDataForPrice();
			//获取数据xml
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有子品牌节点
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

			//将所有子品牌按价格,箱式，分类列表
			Dictionary<int, List<XmlElement>> allSerialNodes = new Dictionary<int, List<XmlElement>>();
			foreach (XmlElement serialNode in serialNodeList)
			{
				if (!ht.ContainsKey(serialNode.GetAttribute("ID")))
				{ continue; }
				string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

				//按报价加入
				foreach (string priceId in prices)
				{
					int pId = Convert.ToInt32(priceId);
					//没有报价按未上市处理
					if (pId == 0)
						pId = 9;
					if (!priceIdDic.ContainsKey(pId))
						continue;

					if (!allSerialNodes.ContainsKey(pId))
						allSerialNodes[pId] = new List<XmlElement>();

					allSerialNodes[pId].Add(serialNode);
				}
			}

			//生成Html
			// StringBuilder htmlCode = new StringBuilder();
			sb.AppendLine("<dl class=\"byletters_list byprice_list\">");
			bool isFirstPrice = true;
			int priceCounter = 0;
			for (int i = 1; i <= 9; i++)
			{
				if (!allSerialNodes.ContainsKey(i))
					continue;

				//报价区间个数计数，用以确定最后一个区间
				priceCounter++;
				if (priceCounter == allSerialNodes.Count)
					isLastPrice = true;

				if (isFirstPrice)
				{
					sb.AppendLine("<dd class=\"h2\"><h2 id=\"" + priceIdDic[i] + "\" class=\"fir\">" + priceTextDic[i] + "</h2></dd>");
				}
				else
					sb.AppendLine("<dd class=\"h2\"><h2 id=\"" + priceIdDic[i] + "\">" + priceTextDic[i] + "</h2></dd>");

				RenderForPrice(sb, allSerialNodes[i], i, isFirstPrice, isLastPrice);

				if (isFirstPrice)
					isFirstPrice = false;
			}

			sb.AppendLine("</dl>");
			navHtml += "<div class=\"car_top_tit tit_byprice\"><ul>";
			navHtml += "<li><a href=\"#a\">5万以下</a></li>";
			navHtml += "<li><a href=\"#b\">5万-8万</a></li>";
			navHtml += "<li><a href=\"#c\">8万-12万</a></li>";
			navHtml += "<li><a href=\"#d\">12万-18万</a></li>";
			navHtml += "<li><a href=\"#e\">18万-25万</a></li>";
			navHtml += "<li><a href=\"#f\">25万-40万</a></li>";
			navHtml += "<li><a href=\"#g\">40万-80万</a></li>";
			navHtml += "<li><a href=\"#h\">80万以上</a></li>";
			if (cate != 4)
			{
				navHtml += "<li><a href=\"#i\">未上市</a></li>";
			}
			navHtml += "</ul></div>";
			// return htmlCode.ToString();
		}

		// 按价格
		public void RenderForPrice(StringBuilder htmlCode, List<XmlElement> serialNodes, int priceId, bool isFirstPice, bool isLastPrice)
		{
			if (priceId <= 3 || priceId > 9)
				RenderForPrice1(htmlCode, serialNodes, isFirstPice, isLastPrice);
			else
				RenderForPrice2(htmlCode, serialNodes, isFirstPice, isLastPrice);
		}

		// 按价格
		private void RenderForPrice1(StringBuilder htmlCode, List<XmlElement> serialNodes, bool isFirstPrice, bool isLastPrice)
		{
			string[] serialClasses = new string[] { "两厢轿车", "三厢轿车", "跑车", "SUV", "MPV", "其它" };
			Dictionary<string, List<XmlElement>> serialList = new Dictionary<string, List<XmlElement>>();
			foreach (XmlElement serialNode in serialNodes)
			{
				//取级别
				string level = serialNode.GetAttribute("CsLevel").ToUpper();
				if (level == "跑车" || level == "SUV" || level == "MPV" || level == "其它")
				{
					if (!serialList.ContainsKey(level))
						serialList[level] = new List<XmlElement>();
					serialList[level].Add(serialNode);
				}
				else
				{
					//取车身样式
					string bodyType = serialNode.GetAttribute("BodyType");
					if (bodyType.IndexOf("两厢") > -1)
					{
						bodyType = "两厢轿车";
					}
					else if (bodyType.IndexOf("三厢") > -1)
					{
						bodyType = "三厢轿车";
					}
					else
						bodyType = "其它";

					if (!serialList.ContainsKey(bodyType))
						serialList[bodyType] = new List<XmlElement>();
					serialList[bodyType].Add(serialNode);
				}
			}
			RenderForClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice);
		}

		// 按价格
		private void RenderForPrice2(StringBuilder htmlCode, List<XmlElement> serialNodes, bool isFirstPrice, bool isLastPrice)
		{
			string[] serialClasses = new string[] { "日系轿车", "美系轿车", "欧系轿车", "韩系轿车", "自主品牌", "跑车", "SUV", "MPV", "其它" };
			Dictionary<string, List<XmlElement>> serialList = new Dictionary<string, List<XmlElement>>();
			foreach (XmlElement serialNode in serialNodes)
			{
				//取级别
				string level = serialNode.GetAttribute("CsLevel").ToUpper();
				if (level == "跑车" || level == "SUV" || level == "MPV" || level == "其它")
				{
					if (!serialList.ContainsKey(level))
						serialList[level] = new List<XmlElement>();
					serialList[level].Add(serialNode);
				}
				else
				{
					//取品牌国家
					string countryName = serialNode.ParentNode.ParentNode.Attributes["Country"].Value;

					if (countryName == "日本")
					{
						countryName = "日系轿车";
					}
					else if (countryName == "美国")
					{
						countryName = "美系轿车";
					}
					else if (countryName == "德国" || countryName == "法国" || countryName == "英国"
				   || countryName == "意大利" || countryName == "瑞典" || countryName == "捷克")
					{
						countryName = "欧系轿车";
					}
					else if (countryName == "韩国")
					{
						countryName = "韩系轿车";
					}
					else if (countryName == "中国")
					{
						countryName = "自主品牌";
					}
					else
						countryName = "其它";

					if (!serialList.ContainsKey(countryName))
						serialList[countryName] = new List<XmlElement>();
					serialList[countryName].Add(serialNode);
				}
			}

			RenderForClassification(serialClasses, htmlCode, serialList, isFirstPrice, isLastPrice);
		}

		// 按价格
		private void RenderForClassification(string[] serialClasses, StringBuilder htmlCode,
				Dictionary<string, List<XmlElement>> serialList, bool isFirstPrice, bool isLastPrice)
		{
			//生成Html
			bool isFirstClass = true;
			int classCounter = 0;
			foreach (string serClass in serialClasses)
			{
				if (!serialList.ContainsKey(serClass))
					continue;

				string anchor = "";
				switch (serClass)
				{
					case "日系轿车":
						anchor = " id=\"rx\"";
						break;
					case "欧系轿车":
						anchor = " id=\"ox\"";
						break;
					case "跑车":
						anchor = " id=\"L9\"";
						break;
					case "SUV":
						anchor = " id=\"L8\"";
						break;
					case "两厢轿车":
						anchor = " id=\"lx\"";
						break;
					case "三厢轿车":
						anchor = " id=\"sx\"";
						break;
					case "其它":
						anchor = " id=\"l10\"";
						break;
				}

				classCounter++;

				if (isFirstClass && isFirstPrice)
				{
					htmlCode.AppendLine("<dt" + anchor + "><label>" + serClass + "</label></dt>");
					isFirstClass = false;
				}
				else
					htmlCode.AppendLine("<dt" + anchor + "><label>" + serClass + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");

				htmlCode.AppendLine("<dd>");
				// RenderSerialsByPv(htmlCode, serialList[serClass], true);
				RenderSerials(ref htmlCode, serialList[serClass], "PV", true, true);

				if (isLastPrice && classCounter == serialList.Count)
					htmlCode.AppendLine("<div class=\"hideline\"></div>");

				htmlCode.AppendLine("</dd>");
			}
		}

		// 按价格
		private void InitDataForPrice()
		{
			priceIdDic = new Dictionary<int, string>();
			priceIdDic[1] = "a";
			priceIdDic[2] = "b";
			priceIdDic[3] = "c";
			priceIdDic[4] = "d";
			priceIdDic[5] = "e";
			priceIdDic[6] = "f";
			priceIdDic[7] = "g";
			priceIdDic[8] = "h";
			priceIdDic[9] = "i";
			priceTextDic = new Dictionary<int, string>();
			priceTextDic[1] = "5万以下";
			priceTextDic[2] = "5万-8万";
			priceTextDic[3] = "8万-12万";
			priceTextDic[4] = "12万-18万";
			priceTextDic[5] = "18万-25万";
			priceTextDic[6] = "25万-40万";
			priceTextDic[7] = "40万-80万";
			priceTextDic[8] = "80万以上";
			priceTextDic[9] = "未上市";
			isLastPrice = false;
		}

		#endregion

		#region 按级别

		// 按级别
		private void RenderByLevel()
		{
			InitData();
			string[] levels = new string[] { "微型车", "小型车", "紧凑型", "中型车", "中大型", "豪华车", "MPV", "SUV", "跑车", "面包车", "其它" };
			Dictionary<string, string> levelNameDic = new Dictionary<string, string>();
			levelNameDic["微型车"] = "微型车";
			levelNameDic["小型车"] = "小型车";
			levelNameDic["紧凑型"] = "紧凑型车";
			levelNameDic["中型车"] = "中型车";
			levelNameDic["中大型"] = "中大型车";
			levelNameDic["豪华车"] = "豪华车";
			levelNameDic["MPV"] = "MPV";
			levelNameDic["SUV"] = "SUV";
			levelNameDic["跑车"] = "跑车";
			levelNameDic["面包车"] = "面包车";
			levelNameDic["其它"] = "其它";

			//获取数据xml
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有子品牌节点
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");

			//将所有子品牌按级别，价格，分类列表
			Dictionary<string, Dictionary<int, List<XmlElement>>> allSerialNodes = new Dictionary<string, Dictionary<int, List<XmlElement>>>();
			foreach (XmlElement serialNode in serialNodeList)
			{
				if (!ht.ContainsKey(serialNode.GetAttribute("ID")))
				{ continue; }
				string level = serialNode.GetAttribute("CsLevel").ToUpper();
				//不在字典中则不显示
				if (!levelLabelDic.ContainsKey(level))
					continue;
				string[] prices = serialNode.GetAttribute("MultiPriceRange").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				//按级别加入列表
				if (!allSerialNodes.ContainsKey(level))
					allSerialNodes[level] = new Dictionary<int, List<XmlElement>>();

				Dictionary<int, List<XmlElement>> priceElements = allSerialNodes[level];
				//按报价加入
				foreach (string priceId in prices)
				{
					int pId = Convert.ToInt32(priceId);
					if (!priceDic.ContainsKey(pId))
						continue;
					if (!priceElements.ContainsKey(pId))
						priceElements[pId] = new List<XmlElement>();

					priceElements[pId].Add(serialNode);
				}
			}

			//生成Html
			// StringBuilder htmlCode = new StringBuilder();
			sb.AppendLine("<dl class=\"byletters_list byprice_list\">");
			bool isFirstLevel = true;
			int levelCounter = 0;
			foreach (string level in levels)
			{
				if (!allSerialNodes.ContainsKey(level))
					continue;

				//级别个数计数，用以确定最后一个级别
				levelCounter++;
				if (levelCounter == allSerialNodes.Count)
					isLastLevel = true;

				if (isFirstLevel)
				{
					sb.AppendLine("<dd class=\"h2\"><h2 id=\"" + levelLabelDic[level] + "\" class=\"fir\">" + levelNameDic[level] + "</h2></dd>");
					isFirstLevel = false;
				}
				else
					sb.AppendLine("<dd class=\"h2\"><h2 id=\"" + levelLabelDic[level] + "\">" + levelNameDic[level] + "</h2></dd>");

				RenderByPrice(ref sb, allSerialNodes[level]);
			}

			sb.AppendLine("</dl>");
			navHtml += "<div class=\"car_top_tit tit_byprice\"><ul>";
			navHtml += "<li><a href=\"#a\">微型车</a></li>";
			navHtml += "<li><a href=\"#b\">小型车</a></li>";
			navHtml += "<li><a href=\"#c\">紧凑型车</a></li>";
			navHtml += "<li><a href=\"#d\">中型车</a></li>";
			navHtml += "<li><a href=\"#e\">中大型车</a></li>";
			navHtml += "<li><a href=\"#f\">豪华车</a></li>";
			navHtml += "<li><a href=\"#g\">MPV</a></li>";
			navHtml += "<li><a href=\"#h\">SUV</a></li>";
			navHtml += "<li><a href=\"#i\">跑车</a></li>";
			navHtml += "<li><a href=\"#m\">面包车</a></li>";
			navHtml += "</ul></div>";
			// return htmlCode.ToString();
		}

		// 按级别
		private void InitData()
		{
			levelLabelDic = new Dictionary<string, string>();
			levelLabelDic["微型车"] = "a";
			levelLabelDic["小型车"] = "b";
			levelLabelDic["紧凑型"] = "c";
			levelLabelDic["中型车"] = "d";
			levelLabelDic["中大型"] = "e";
			levelLabelDic["豪华车"] = "f";
			levelLabelDic["MPV"] = "g";
			levelLabelDic["SUV"] = "h";
			levelLabelDic["跑车"] = "i";
			levelLabelDic["面包车"] = "m";
			levelLabelDic["其它"] = "j";
			priceDic = new Dictionary<int, string>();
			priceDic[1] = "5万以下";
			priceDic[2] = "5万-8万";
			priceDic[3] = "8万-12万";
			priceDic[4] = "12万-18万";
			priceDic[5] = "18万-25万";
			priceDic[6] = "25万-40万";
			priceDic[7] = "40万-80万";
			priceDic[8] = "80万以上";
			isFirstPrice = true;
			isLastLevel = false;
		}

		// 按级别
		private void RenderByPrice(ref StringBuilder htmlCode, Dictionary<int, List<XmlElement>> pricesNodes)
		{
			int priceCounter = 0;

			for (int i = 1; i <= 8; i++)
			{
				if (!pricesNodes.ContainsKey(i))
					continue;

				priceCounter++;

				if (isFirstPrice)
				{
					htmlCode.AppendLine("<dt><label>" + priceDic[i] + "</label></dt>");
					isFirstPrice = false;
				}
				else
					htmlCode.AppendLine("<dt><label>" + priceDic[i] + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");

				//第个子品牌
				htmlCode.AppendLine("<dd>");
				// new Car_SerialBll().RenderSerialsByPVNoLevel(htmlCode, pricesNodes[i], true);
				RenderSerials(ref htmlCode, pricesNodes[i], "PV", false, true);

				//最后一个级别和最后一个报价才有这个
				if (isLastLevel && priceCounter == pricesNodes.Count)
				{
					htmlCode.AppendLine("<div class=\"hideline\"></div>");
				}
				htmlCode.AppendLine("</dd>");
			}
		}

		#endregion

		#region 按国别

		// 按国别
		private void RenderCountry()
		{
			//获取数据xml
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
			masterBrandNum = mbNodeList.Count;

			string[] countryList = new string[] { "zz", "dx", "rx", "mx", "hx", "fx", "yx", "yx2", "other" };
			Dictionary<string, CountryMasterBrands> cmDic = new Dictionary<string, CountryMasterBrands>();

			foreach (string cLabel in countryList)
				cmDic[cLabel] = new CountryMasterBrands(cLabel);

			//将主品牌加入字典
			foreach (XmlElement mbNode in mbNodeList)
			{
				if (!htMaster.ContainsKey(mbNode.GetAttribute("ID")))
				{ continue; }
				string newCountryName = mbNode.GetAttribute("Country");
				string countryLabel = CountryMasterBrands.GetCountryLabel(newCountryName);
				cmDic[countryLabel].Add(mbNode);
			}
			//生成Html
			// StringBuilder htmlCode = new StringBuilder();
			sb.AppendLine("<dl class=\"bybrand_list bynation_list\">");
			bool isFirstCountry = true;
			foreach (string cLabel in countryList)
			{
				if (cmDic[cLabel].Count < 1)
				{ continue; }
				CountryMasterBrands cmBrand = cmDic[cLabel];
				//国系
				if (isFirstCountry)
				{
					sb.AppendLine("<dt><label id=\"" + cLabel + "\">" + cmBrand.CountryName + "</label></dt>");
					isFirstCountry = false;
				}
				else
					sb.AppendLine("<dt><label id=\"" + cLabel + "\">" + cmBrand.CountryName + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");

				RenderMasterBandForCountry(ref sb, cmBrand);
			}
			sb.AppendLine("</dl>");
			navHtml = "<div class=\"car_top_tit tit_byprice tit_bynation\">";
			navHtml += "<ul>";
			foreach (string cLabel in countryList)
			{
				if (cmDic[cLabel].Count < 1)
				{
					continue;
					// navHtml += "<li class=\"n08\"><a href=\"#hx\">韩国</a></li>";
				}
				else
				{
					string classid = "";
					string countName = "";
					GetCountryName(cLabel, out classid, out countName);
					navHtml += "<li class=\"" + classid + "\"><a href=\"#" + cLabel + "\">" + countName + "</a></li>";
				}
			}
			navHtml += "</ul>";
			navHtml += "</div>";
			// return htmlCode.ToString();
		}

		// 按国别
		private void GetCountryName(string code, out string classid, out string countryName)
		{
			classid = "0";
			countryName = "";
			if (code == "zz")
			{
				classid = "n01";
				countryName = "自主";
			}
			if (code == "dx")
			{
				classid = "n04";
				countryName = "德国";
			}
			if (code == "rx")
			{
				classid = "n03";
				countryName = "日本";
			}
			if (code == "mx")
			{
				classid = "n02";
				countryName = "美国";
			}
			if (code == "hx")
			{
				classid = "n08";
				countryName = "韩国";
			}
			if (code == "fx")
			{
				classid = "n05";
				countryName = "法国";
			}
			if (code == "yx")
			{
				classid = "n06";
				countryName = "英国";
			}
			if (code == "yx2")
			{
				classid = "n07";
				countryName = "意大利";
			}
			if (code == "other")
			{
				classid = "other";
				countryName = "其他";
			}
		}

		// 按国别
		private void RenderMasterBandForCountry(ref StringBuilder htmlCode, CountryMasterBrands masterBrandList)
		{
			foreach (XmlElement mbNode in masterBrandList)
			{
				masterCounter++;
				//生成主品牌图标
				string mbId = mbNode.GetAttribute("ID");
				string mbName = mbNode.GetAttribute("Name");
				string mbAllSpell = mbNode.GetAttribute("AllSpell");
				htmlCode.AppendLine("<dd class=\"b\">");
				htmlCode.AppendLine("<a target=\"_blank\" href=\"http://car.bitauto.com/" + mbAllSpell.ToLower() + "/\"><div class=\"brand m_" + mbId + "_b\"></div></a>");
				htmlCode.AppendLine("<div class=\"brandname\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + mbAllSpell.ToLower() + "/\">" + mbName + "</a></div>");
				htmlCode.AppendLine("</dd>");
				//生成主品牌列表
				RenderBrandsForCountry(ref htmlCode, mbNode);
			}
		}

		// 按国别
		private void RenderBrandsForCountry(ref StringBuilder htmlCode, XmlElement mbNode)
		{
			htmlCode.AppendLine("<dd class=\"have\">");
			//获取品牌信息
			List<XmlElement> brandList = new List<XmlElement>();
			foreach (XmlElement ele in mbNode.SelectNodes("Brand"))
			{
				if (!htBrand.ContainsKey(ele.GetAttribute("ID")))
				{ continue; }
				brandList.Add(ele);
			}
			//添加排序条件
			brandList.Sort(NodeCompare.CompareBrandNodeSelfFirst);

			bool isFirstBrand = true;
			int brandCounter = 0;
			foreach (XmlElement brandNode in brandList)
			{
				brandCounter++;
				//生成品牌Html
				string brandId = brandNode.GetAttribute("ID");
				string brandName = brandNode.GetAttribute("Name");
				string brandSpell = brandNode.GetAttribute("AllSpell");
				if (isFirstBrand)
				{
					htmlCode.AppendLine("<h2><a target=\"_blank\" href=\"http://car.bitauto.com/" + brandSpell.ToLower() + "/\">" + brandName + "</a></h2>");
					isFirstBrand = false;
				}
				else
					htmlCode.AppendLine("<h2 class=\"border\"><a target=\"_blank\" href=\"http://car.bitauto.com/" + brandSpell.ToLower() + "/\">" + brandName + "</a></h2>");

				//加入列表
				XmlNodeList serialNodeList = brandNode.SelectNodes("Serial");
				List<XmlElement> serialList = new List<XmlElement>();
				foreach (XmlElement serialNode in serialNodeList)
				{
					if (!ht.ContainsKey(serialNode.GetAttribute("ID")))
					{ continue; }
					serialList.Add(serialNode);
				}

				//生成子品牌列表
				// new Car_SerialBll().RenderSerialsBySpell(htmlCode, serialList, false);
				RenderSerials(ref htmlCode, serialList, "SPELL", true, false);
			}

			htmlCode.AppendLine("</dd>");

			//只在全页最后一个品牌后不输出此行
			if (masterCounter != masterBrandNum || brandCounter != brandList.Count)
				htmlCode.AppendLine("<dd class=\"line\"></dd>");
		}

		#endregion

		#region 按品牌

		// 按品牌
		private void RenderBrandList()
		{
			//获取数据xml
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();

			Dictionary<string, bool> hasChar = new Dictionary<string, bool>();
			//Html
			sb.AppendLine("<dl class=\"bybrand_list\">");

			//第一个字母处不加回到顶部
			bool isFirstChar = true;

			//遍历所有主品牌节点
			XmlNodeList mbNodeList = mbDoc.SelectNodes("/Params/MasterBrand");
			for (int i = 0; i < mbNodeList.Count; i++)
			{

				XmlElement mbNode = (XmlElement)mbNodeList[i];
				if (!htMaster.ContainsKey(mbNode.GetAttribute("ID")))
				{ continue; }
				string masterSpell = mbNode.GetAttribute("AllSpell");
				//首字母
				string firstChar = mbNode.GetAttribute("Spell").Substring(0, 1).ToUpper();

				//生成字母头
				if (!hasChar.ContainsKey(firstChar))
				{
					if (isFirstChar)
					{
						sb.AppendLine("<dt><label id=\"" + firstChar + "\">" + firstChar + "</label></dt>");
						isFirstChar = false;
					}
					else
						sb.AppendLine("<dt><label id=\"" + firstChar + "\">" + firstChar + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");
					hasChar[firstChar] = true;
				}
				//生成主品牌图标
				string mbId = mbNode.GetAttribute("ID");
				string mbName = mbNode.GetAttribute("Name");
				sb.AppendLine("<dd class=\"b\">");
				sb.AppendLine("<a href=\"http://car.bitauto.com/" + masterSpell.ToLower() + "/\" target=\"_blank\"><div class=\"brand m_" + mbId + "_b\"></div></a>");
				sb.AppendLine("<div class=\"brandname\"><a href=\"http://car.bitauto.com/" + masterSpell.ToLower() + "/\" target=\"_blank\">" + mbName + "</a></div>");
				sb.AppendLine("</dd>");
				//生成品牌列表
				RenderBrands(ref sb, mbNode);

				//一条线
				if (i < mbNodeList.Count - 1)
					sb.AppendLine("<dd class=\"line\"></dd>");
			}
			sb.AppendLine("</dl>");

			//字母导航
			// string charNavHtml = CommonFunction.RenderCharNav(hasChar);
			navHtml = CommonFunction.RenderCharNav(hasChar);
			// return charNavHtml + htmlCode.ToString();
		}

		// 按品牌
		private void RenderBrands(ref StringBuilder htmlCode, XmlElement mbNode)
		{
			htmlCode.AppendLine("<dd class=\"have\">");
			//获取品牌信息
			List<XmlElement> brandList = new List<XmlElement>();
			foreach (XmlElement ele in mbNode.SelectNodes("Brand"))
			{
				if (!htBrand.ContainsKey(ele.GetAttribute("ID")))
				{ continue; }
				brandList.Add(ele);
			}
			//添加排序条件
			brandList.Sort(NodeCompare.CompareBrandNodeSelfFirst);

			bool isFirstBrand = true;

			foreach (XmlElement brandNode in brandList)
			{
				//生成品牌Html
				string brandId = brandNode.GetAttribute("ID");
				string brandName = brandNode.GetAttribute("Name");
				string brandSpell = brandNode.GetAttribute("AllSpell");
				if (isFirstBrand)
				{
					htmlCode.AppendLine("<h2><a href=\"http://car.bitauto.com/" + brandSpell.ToLower() + "/\" target=\"_blank\">" + brandName + "</a></h2>");
					isFirstBrand = false;
				}
				else
					htmlCode.AppendLine("<h2 class=\"border\"><a href=\"http://car.bitauto.com/" + brandSpell.ToLower() + "/\" target=\"_blank\">" + brandName + "</a></h2>");

				//加入列表
				XmlNodeList serialNodeList = brandNode.SelectNodes("Serial");
				List<XmlElement> serialList = new List<XmlElement>();
				foreach (XmlElement serialNode in serialNodeList)
				{
					if (!ht.ContainsKey(serialNode.GetAttribute("ID")))
					{ continue; }
					serialList.Add(serialNode);
				}

				//生成子品牌列表
				// new Car_SerialBll().RenderSerialsBySpell(htmlCode, serialList, false);
				RenderSerials(ref htmlCode, serialList, "SPELL", true, false);
			}

			htmlCode.AppendLine("</dd>");
		}

		#endregion

		#region 按字母

		// 按字母
		private void RenderCharList()
		{
			// XmlDocument mbDoc = AutoStorageService.GetAutoXml();
			Dictionary<string, List<XmlElement>> serialDic = new Dictionary<string, List<XmlElement>>();
			XmlNodeList serialNodeList = mbDoc.SelectNodes("/Params/MasterBrand/Brand/Serial");
			foreach (XmlElement serialNode in serialNodeList)
			{
				if (!ht.ContainsKey(serialNode.GetAttribute("ID")))
				{ continue; }
				//首字母
				string[] firstChars = serialNode.GetAttribute("CsMultiChar").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string firstChar in firstChars)
				{
					if (!serialDic.ContainsKey(firstChar))
					{
						serialDic[firstChar] = new List<XmlElement>();
					}
					serialDic[firstChar].Add(serialNode);
				}
			}

			string[] charList = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q",
			"R","S","T","U","V","W","X","Y","Z"};

			sb.AppendLine("<dl class=\"byletters_list\">");
			//已处理字母计数
			int charCounter = 0;
			foreach (string fChar in charList)
			{
				if (!serialDic.ContainsKey(fChar))
					continue;
				charCounter++;

				if (charCounter == 1)
					sb.AppendLine("<dt><label id=\"" + fChar + "\">" + fChar + "</label></dt>");
				else
					sb.AppendLine("<dt><label id=\"" + fChar + "\">" + fChar + "</label><a href=\"#pageTop\" class=\"gotop\">返回顶部↑</a></dt>");

				sb.AppendLine("<dd>");

				//生成子品牌列表
				RenderSerials(ref sb, serialDic[fChar], "SPELL", true, true);
				// new Car_SerialBll().RenderSerialsBySpell(sb, serialDic[fChar], true);

				if (charCounter == serialDic.Count)
					sb.AppendLine("<div class=\"hideline\"></div>");
				sb.AppendLine("</dd>");

			}
			sb.AppendLine("</dl>");

			//字母导航
			navHtml = CommonFunction.RenderCharNav(serialDic);
		}

		// 按字母
		private void RenderSerials(ref StringBuilder htmlCode, List<XmlElement> serialList, string sort, bool hasLevel, bool isShowName)
		{
			htmlCode.AppendLine("<ul>");

			if (sort.ToUpper() == "PV")
			{
				//按关注度排序
				serialList.Sort(NodeCompare.CompareSerialByPvDesc);
			}
			else
			{
				serialList.Sort(NodeCompare.CompareSerialBySpellAsc);
			}


			foreach (XmlElement serialNode in serialList)
			{
				htmlCode.AppendLine("<li>");
				string serialId = serialNode.GetAttribute("ID");
				string serialName = "";
				if (isShowName)
					serialName = serialNode.GetAttribute("ShowName");
				else
					serialName = serialNode.GetAttribute("Name");

				string serialLevel = serialNode.GetAttribute("CsLevel");
				string serialSpell = serialNode.GetAttribute("AllSpell");
				//EnumCollection.SerialLevelEnum levelEnum = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), serialLevel);
				int hasNew = Convert.ToInt32(serialNode.GetAttribute("CsHasNew"));
				if (ht.ContainsKey(serialId))
				{
					htmlCode.Append("<a href=\"" + Convert.ToString(ht[serialId]).ToLower() + "\" target=\"_blank\">" + serialName + "</a>");
				}
				else
				{
					htmlCode.Append("<a href=\"http://car.bitauto.com/" + serialSpell.ToLower() + "/\" target=\"_blank\">" + serialName + "</a>");
				}

				//是否带车型级别
				if (hasLevel && serialLevel != "其它")
				{
					//htmlCode.Append("<a href=\"http://car.bitauto.com/" + ((EnumCollection.SerialLevelSpellEnum)levelEnum).ToString().ToLower() + "/\" class=\"classify\" target=\"_blank\">[" + serialLevel + "]</a>");
					var levelSpell = CarLevelDefine.GetLevelSpellByName(serialLevel);
					htmlCode.Append("<a href=\"http://car.bitauto.com/" + levelSpell.ToLower() + "/\" class=\"classify\" target=\"_blank\">[" + serialLevel + "]</a>");
				}
				if (hasNew == 1)
				{
					// modified by chengl Jan.20.2010
					// htmlCode.Append("<span class=\"new\">新</span>");
				}
				//htmlCode.Append("<div><a href=\"http://price.bitauto.com/c/cbrand.aspx?newbrandid=" + serialId + "\" target=\"_blank\">报价</a>");
				// htmlCode.Append("<div><a href=\"http://car.bitauto.com/" + serialSpell + "/baojia/\" target=\"_blank\">报价</a>");
				// htmlCode.Append("<a href=\"http://photo.bitauto.com/serial/" + serialId + "\" target=\"_blank\">图片</a>");
				// htmlCode.AppendLine("<a href=\"http://api.baa.bitauto.com/go2baa.aspx?brandid=" + serialId + "\" target=\"_blank\">论坛</a></div>");

				htmlCode.AppendLine("</li>");
			}
			htmlCode.AppendLine("</ul>");
		}

		#endregion
	}
}