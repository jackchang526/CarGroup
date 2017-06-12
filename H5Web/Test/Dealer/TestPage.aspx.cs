using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using System.Linq;

namespace H5Web.Test.Dealer
{
	public partial class TestPage : H5PageBase
	{

		#region Params

		protected bool IsExistColor = false;
		protected bool IsExistAppearance = false;
		protected bool IsExistInner = false;
		protected bool IsExistPingCe = false;
		protected bool IsExistPeizhi = false;
		protected bool IsExistKouBei = false;
		protected bool IsExistBaoJia = false;
		protected bool IsExistAttention = false;



		protected int serialId;

		protected SerialEntity BaseSerialEntity;

		// protected Car_SerialEntity SerialBrandEntity;

		protected List<SerialColorForSummaryEntity> SerialColorList;
		protected string SerialColorsJson;
		protected List<SerialFourthStage> ExteriorList;
		protected List<SerialFourthStage> InteriorList;
		protected List<News> SerialNewsList;
		protected string WriterKoubei;
		protected List<EnumCollection.SerialToSerial> AttentionSerials;
		protected List<CarInfoForSerialSummaryEntity> CarModelList;
		protected int CarModelWithPriceCount = 0;
		protected List<SerialFocusImage> ExteriorImageList;
		protected List<SerialFocusImage> InteriorImageList;
		protected List<SerialSparkle> SerialSparkleList;

		protected List<News> CommerceNewsList;
		// protected Dictionary<int, string> dictSelectSerial; 
		protected List<int> listAllCsID = new List<int>();
		protected Dictionary<int, string> CarParamDictionary;

		protected string KoubeiHtml = string.Empty;
		protected string PeiZhiHtml = string.Empty;

		protected string DealerCarListHtml = string.Empty;
		protected string DealerInfoHtml = string.Empty;
		protected string DealerSaleHtml = string.Empty;
		protected string DealerNewsHtml = string.Empty;
		#endregion

		#region Services
		private Car_SerialBll carSerialBll = new Car_SerialBll();

		private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();

		private Car_BasicBll _carBLL = new Car_BasicBll();

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			// base.SetPageCache(30);
			GetParamter();
			InitData();
			#region 验证访问权限
			// dictSelectSerial = GetDictSelectSerial();
			listAllCsID = serialFourthStageBll.GetAllSerialInH5();
			if (listAllCsID == null || BaseSerialEntity == null)
			{
				Response.Redirect("http://car.h5.yiche.com/");
			}
			if (!listAllCsID.Contains(serialId) && serialId > 0)
			{
				Response.Redirect("http://car.m.yiche.com/" + BaseSerialEntity.AllSpell);
			}
			#endregion
			InitColorList();//初始化颜色列表
			InitExteriorInteriorList();//初始化外观内饰设计数据
			InitNews();//初始化新闻列表
			InitKoubei();//编辑口碑
			InitAttentions();//关注的车型
			InitPrice();//车款报价
			IntiDealerInfo();
			InitCommerceNews();//商配文章
			InitSerialSparkle();//亮点配置
		}

		private void GetParamter()
		{
			serialId = ConvertHelper.GetInteger(Request.QueryString["ID"]);
		}

		private void InitData()
		{
			BaseSerialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialId);
			// del by chengl 2015-6-30
			// SerialBrandEntity = carSerialBll.GetSerialInfoEntity(serialId);
		}

		private void InitColorList()
		{
			var colorList = serialFourthStageBll.GetSerialColorList(serialId);
			if (colorList != null && colorList.Count > 0)
			{
				IsExistColor = true;
				if (colorList.Count > 12)
					SerialColorList = colorList.GetRange(0, 12);
				else
				{
					SerialColorList = colorList;
				}
			}
		}

		private void InitExteriorInteriorList()
		{
			//外观设计
			ExteriorList = serialFourthStageBll.GetExteriorInteriorList(serialId, 0);
			if (ExteriorList != null && ExteriorList.Count > 0)
				IsExistAppearance = true;
			//获取外观图片列表
			ExteriorImageList = serialFourthStageBll.GetPhtotoAtlas(serialId, 6);
			if (ExteriorImageList != null && ExteriorImageList.Count > 0)
				IsExistAppearance = true;
			//内饰设计
			InteriorList = serialFourthStageBll.GetExteriorInteriorList(serialId, 1);
			if (InteriorList != null && InteriorList.Count > 0)
				IsExistInner = true;
			//获取内饰图片列表
			InteriorImageList = serialFourthStageBll.GetPhtotoAtlas(serialId, 7);
			if (InteriorImageList != null && InteriorImageList.Count > 0)
				IsExistInner = true;
		}

		/// <summary>
		/// 初始化新闻列表
		/// </summary>
		private void InitNews()
		{
			SerialNewsList = serialFourthStageBll.GetSerialNewsWithCreative(serialId, 4);
			if (SerialNewsList != null && SerialNewsList.Count > 0)
				IsExistPingCe = true;
		}
		/// <summary>
		/// 初始化口碑信息
		/// </summary>
		private void InitKoubei()
		{
			WriterKoubei = serialFourthStageBll.MakeEditorCommentHtml(serialId);
			KoubeiHtml = serialFourthStageBll.MakeKoubeiDianpingHtml(serialId);
			if (!string.IsNullOrEmpty(WriterKoubei) || !string.IsNullOrEmpty(KoubeiHtml))
				IsExistKouBei = true;
		}
		/// <summary>
		/// 初始化车型报价信息
		/// </summary>
		private void InitPrice()
		{
			// 经销商车款报价
			// demo 测试页
			string filrPath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealerCar.txt";
			if (File.Exists(filrPath))
			{
				DealerCarListHtml = File.ReadAllText(filrPath, Encoding.UTF8);
				if (DealerCarListHtml.Trim() != "")
				{
					IsExistBaoJia = true;
				}
			}
		}

		/// <summary>
		/// 初始化经销商信息
		/// </summary>
		private void IntiDealerInfo()
		{
			string filrPath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealer.txt";
			if (File.Exists(filrPath))
			{
				DealerInfoHtml = File.ReadAllText(filrPath, Encoding.UTF8);
			}
		}

		/// <summary>
		/// 车型报价
		/// </summary>
		/// <param name="pageIndex"></param>
		/// <returns></returns>
		protected List<CarInfoForSerialSummaryEntity> GetCarModelListByPage(int pageIndex)
		{
			int start = (pageIndex - 1) * 4,
				end = pageIndex * 4,
				count = pageIndex == 5 ? 3 : 4;
			count = start + count > CarModelWithPriceCount ? CarModelWithPriceCount - start : count;
			return CarModelList.GetRange(start, count);
		}

		/// <summary>
		/// 关注的车型
		/// </summary>
		private void InitAttentions()
		{
			// 经销商还卖车
			// demo 测试页
			string filrPath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealerSale.txt";
			if (File.Exists(filrPath))
			{
				DealerSaleHtml = File.ReadAllText(filrPath, Encoding.UTF8);
				if (DealerSaleHtml.Trim() != "")
				{
					IsExistAttention = true;
				}
			}

			//AttentionSerials = serialFourthStageBll.MakeSerialToSerialHtml(serialId);
			//if (AttentionSerials != null && AttentionSerials.Count > 0)
			//	IsExistAttention = true;
		}

		protected string GetforwardGearNum(int carModelId)
		{
			if (CarParamDictionary != null && CarParamDictionary.ContainsKey(carModelId))
			{
				var paramValue = CarParamDictionary[carModelId];
				return (!string.IsNullOrEmpty(paramValue) && paramValue != "无级" && paramValue != "待查") ? paramValue + "挡" : "";
			}
			return string.Empty;
		}

		/// <summary>
		/// 商配文章
		/// </summary>
		private void InitCommerceNews()
		{
			// 经销商新闻
			// demo 测试页
			string filrPath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealerNews.txt";
			if (File.Exists(filrPath))
			{
				DealerNewsHtml = File.ReadAllText(filrPath, Encoding.UTF8);
			}
			// CommerceNewsList = serialFourthStageBll.GetCommerceNews(serialId);
		}

		private void InitSerialSparkle()
		{
			string allPeizhi = string.Format("<span><a href=\"http://car.m.yiche.com/{0}/peizhi/\" ><img src=\"http://image.bitautoimg.com/carchannel/pic/sparkle/0.png\" />全部配置</a></span>"
				, BaseSerialEntity.AllSpell);
			SerialSparkleList = serialFourthStageBll.GetSerialSparkle(serialId);
			if (SerialSparkleList != null && SerialSparkleList.Count > 0)
			{
				IsExistPeizhi = true;
				List<string> tempList = new List<string>();
				int loop = 0;
				foreach (SerialSparkle ss in SerialSparkleList)
				{
					if (loop >= 12)
					{ break; }
					if ((loop % 4) == 0)
					{
						if (loop > 0)
						{ tempList.Add("</li>"); }
						tempList.Add("<li>");
					}
					if (loop == 11)
					{
						// 如果满12个 第12个位置 用默认更多，到移动站参数配置页
						tempList.Add(allPeizhi);
					}
					else
					{
						tempList.Add(string.Format("<span><img src=\"http://image.bitautoimg.com/carchannel/pic/sparkle/{0}.png\" />{1}</span>"
							, ss.H5SId, ss.Name));
					}
					loop++;
				}
				if (SerialSparkleList.Count < 12)
				{
					// 如果不够12个，补更多
					if ((SerialSparkleList.Count % 4) == 0)
					{
						tempList.Add(string.Format("</li><li>{0}", allPeizhi));
					}
					else
					{
						tempList.Add(allPeizhi);
					}
				}
				if (tempList.Count > 0)
				{
					tempList.Add("</li>");
				}
				PeiZhiHtml = string.Join("", tempList.ToArray());
			}
		}

		/// <summary>
		/// 获取压缩图片地址
		/// </summary>
		/// <param name="imageUrl"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		protected string GetCompressImageUrl(string imageUrl, int width)
		{
			string tempUrl = string.Empty;
			if (imageUrl.StartsWith("http://"))
			{
				tempUrl = imageUrl.Substring(7);
			}
			else
			{
				tempUrl = imageUrl;
			}
			tempUrl = tempUrl.Substring(tempUrl.IndexOf('/'));
			return string.Format("http://image.bitautoimg.com/newsimg-{0}-w0-1-q80{1}", width, tempUrl);
		}


	}
}