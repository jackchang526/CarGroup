using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System.Xml;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common;
using BitAuto.CarUtils.Define;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Data;
using Newtonsoft.Json;

namespace WirelessWeb
{
	public partial class SelectCar : WirelessTreeBase
	{
		int pageNum = 1;
		int pageSize = 20;
		protected string titleHtml = string.Empty;
		private SelectCarParameters selectParas;
        protected string adCarListData;
		protected string serialListHtml = string.Empty;
		protected int SerialNum = 0;
		protected int CarNum = 0;
		protected int pageCount = 0;
		private string sortMode;
		private string sortModeHtml;
		protected string ClearConsitionHtml = string.Empty;
		protected string CarTabText = string.Empty;
		protected string metaHtml = string.Empty;  
		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			//搜索地址 
			this._SearchUrl = InitSearchUrl("chexing");
			string queryString = Request.Url.Query.ToLower();
			//排序
			sortMode = Request.QueryString["s"];
			//if (sortMode == "1")
			//{
			//    sortMode = "guanzhu_up";
			//    sortModeHtml = "<a href=\"javascript:GotoPage('s0');\" class=\"uparrow\"><em>按关注</em></a> | <a href=\"javascript:GotoPage('s2');\" class=\"uparrow\">按价格</a>";
			//}
			if (sortMode == "2")
			{
				sortMode = "price_up";
                sortModeHtml = "<li><a href=\"javascript:GotoPage('s1');\">按关注度</a></li><li><a href=\"javascript:GotoPage('s3');\">最贵</a></li><li class=\"current\"><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>";
			}
			else if (sortMode == "3")
			{
				sortMode = "price_down";
                sortModeHtml = "<li><a href=\"javascript:GotoPage('s1');\">按关注度</a></li><li class=\"current\"><a href=\"javascript:GotoPage('s3');\">最贵</a></li><li><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>";
			}
			else
			{
				sortMode = "guanzhu_down";
				sortModeHtml = "<li class=\"current\"><a href=\"javascript:GotoPage('s1');\">按关注度</a></li><li class=\"arrow\"><a href=\"javascript:GotoPage('s3');\">最贵</a></li><li class=\"arrow\"><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>";
			}

            //if (queryString.Length > 0)
            //{
				selectParas = base.GetSelectCarParas();
                //delete by zf, 2016-01-19
				//MakeSelectCar();  //获取车款方式改为从前端调用接口
                //CarTabText = string.Format("{0}个车型（按关注排序）", SerialNum);
                //if (SerialNum <= pageSize)
                //{
                //    //litLoadMore.Visible = false;
                //}
                //pageCount = SerialNum / pageSize + (SerialNum % pageSize == 0 ? 0 : 1);

				titleHtml = "<title>【选车工具|选车中心_汽车车型大全】-手机易车网</title>";
                //if (SerialNum > 0)
                //{
					titleHtml += "<meta name=\"keywords\" content=\"选车,选车工具,易车网\" />";
					titleHtml += "<meta name=\"description\" content=\"选车工具:易车网车型频道为您提供按条件选车工具,包括按汽车价格、汽车级别、国产进口、变速方式、汽车排量等方式选择适合您的喜欢的汽车品牌……\" />";
                //}
            //}
            //else
            //{
            //    titleHtml = "<title>【汽车大全|汽车标志大全-热门车型大全】-手机易车网</title>";
            //    titleHtml += "<meta name=\"keywords\" content=\"汽车大全,汽车标志,车型大全,汽车标志大全\" />";
            //    titleHtml += "<meta name=\"description\" content=\"汽车大全:易车网车型大全频道为您提供各种汽车品牌型号信息,包括汽车报价,汽车标志,汽车图片,汽车经销商,汽车油耗,汽车资讯,汽车点评,汽车问答,汽车论坛等等……\" />";
            //    CarTabText = "热门车型";
            //    //litLoadMore.Visible = false;
            //    serialListHtml = MakeHotCarHtml();
            //}
			ucSelectTool.SearchUrl = this._SearchUrl;
            InitSelectCarAD();
		}

		//热门车型
		private string MakeHotCarHtml()
		{
			SerialNum = 0;
			Car_SerialBll serial = new Car_SerialBll();
			List<XmlElement> serialList = serial.GetHotSerial(10);
			StringBuilder sbCarList = new StringBuilder();
			sbCarList.Append("<div class=\"tt-first y2015\">");
			sbCarList.AppendFormat("<ul class=\"tags-sub tags-sub-left\"><li class=\"curren\"><a>{0}</a></li></ul>", "热门车型");
			sbCarList.Append("</div>");
            sbCarList.Append("<div class=\"buy-car\">");
            sbCarList.Append("<ul>");
			foreach (XmlElement serialNode in serialList)
			{
				SerialNum++;
				int serialId = Convert.ToInt32(serialNode.GetAttribute("ID"));
				string imgUrl = Car_SerialBll.GetSerialImageUrl(serialId);
				string serialName = "";
				serialName = serialNode.GetAttribute("ShowName");
				string serialLevel = serialNode.GetAttribute("CsLevel");
				string serialSpell = serialNode.GetAttribute("AllSpell").ToLower();
				//EnumCollection.SerialLevelEnum levelEnum = (EnumCollection.SerialLevelEnum)Enum.Parse(typeof(EnumCollection.SerialLevelEnum), serialLevel);
				string serialUrl = "/" + serialSpell + "/";
				//string levelUrl = "/" + ((EnumCollection.SerialLevelSpellEnum)levelEnum).ToString() + "/";

				string levelUrl = string.Format("/{0}/", CarLevelDefine.GetLevelSpellByName(serialLevel));

				string priceRange = new PageBase().GetSerialPriceRangeByID(Convert.ToInt32(serialId));
				if (priceRange.Trim().Length == 0)
					priceRange = "暂无报价";
				sbCarList.Append("<li>");
				sbCarList.AppendFormat("<a href=\"{0}\" class=\"car\"><img src=\"{1}\" />", serialUrl, imgUrl.Replace("_2.","_6."));
                sbCarList.AppendFormat("<strong>{0}</strong>", serialName);
				sbCarList.AppendFormat("<p><em>{0}</em></p>", priceRange);
				sbCarList.Append("</a></li>");

			}
			sbCarList.AppendLine("</ul>");
			sbCarList.AppendLine("</div>");
			return sbCarList.ToString();
		}

		private void MakeSelectCar()
		{
			List<BitAuto.CarChannel.BLL.SerialInfo> tmpList = new SelectCarToolBll().SelectCarByParameters(selectParas);
			List<BitAuto.CarChannel.BLL.SerialInfo> serialList = tmpList.GetRange(0, tmpList.Count);
			//排序
			if (sortMode == "guanzhu_down")
			{
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByPVDesc);
			}
			//else if (sortMode == "guanzhu_up")
			//{
			//    serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerial);
			//}
			else if (sortMode == "price_up")
			{
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPrice);
			}
			else if (sortMode == "price_down")
			{
				serialList.Sort(BitAuto.CarChannel.BLL.SerialInfo.CompareSerialByMinPriceDesc);
			}
			SerialNum = serialList.Count;
			CarNum = 0;
			StringBuilder htmlCode = new StringBuilder();
			if (SerialNum > 0)
			{
				//modified by sk 2013.10.23 广告 
				List<SerialListADEntity> listSerialAD = new Car_SerialBll().GetSerialAD("selectcar" + selectParas.ConditionString);
				if (selectParas.BodyForm == 0 && selectParas.CarConfig == 0 && listSerialAD != null && listSerialAD.Count > 0)
				{
					foreach (SerialListADEntity serialAd in listSerialAD)
					{
						int index = serialAd.Pos - 1;
						if (index < 0)
							index = 0;
						SerialInfo serialInfo = serialList.Find((p) => { return p.SerialId == serialAd.SerialId; });
						if (serialInfo != null)
						{
							serialList.Remove(serialInfo);
							serialInfo.ADSerialUrl = serialAd.Url;
							serialInfo.IsAdvertise = true;
							serialList.Insert(index, serialInfo);
						}
					}
				}

				htmlCode.AppendLine(MakeImageModeHtml(serialList));
				htmlCode.Insert(0, MakeSelectBarHtml());
				serialListHtml = htmlCode.ToString();
			}
			else
			{
				serialListHtml = "<div class=\"tt-first y2015\"><ul class=\"tags-sub tags-sub-left\"><li><a>选车结果</a></li></ul></div>";
				serialListHtml += "<div class=\"wrap\"><div class=\"m-no-result\"><div class=\"face face-fail\"></div><dl><dt>未找到合适的车型！</dt><dd>请调整一下选车条件</dd></dl><div class=\"clear\"></div></div>";
				serialListHtml += "<a class=\"btn-one btn-gray\" href=\"javascript:location.href=document.referrer\">返回</a></div>";
			}
		}

        private void InitSelectCarAD()
        {
            //int pageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
            List<SuperSerialInfo> adCarList = new List<SuperSerialInfo>();
            List<SerialListADEntity> listSerialAD = new Car_SerialBll().GetSerialAD("selectcar" + selectParas.ConditionString);
            if (listSerialAD != null && listSerialAD.Count > 0)
            {
                foreach (SerialListADEntity serialAd in listSerialAD)
                {
                    int index = serialAd.Pos - 1;
                    if (index < 0)
                        index = 0;

                    SerialEntity serialEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, serialAd.SerialId);
                    if (serialEntity != null)
                    {
                        adCarList.Add(new SuperSerialInfo(serialAd.SerialId, serialEntity.ShowName, serialEntity.AllSpell)
                        {
                            Pos = serialAd.Pos,
                            //CarIdList = string.Join(",", serialEntity.CarList.Select(p => p.Id)),
                            ImageUrl = Car_SerialBll.GetSerialImageUrl(serialAd.SerialId, "1"),
                            PriceRange = serialEntity.Price,
                            CarNum = serialEntity.CarList.Length
                        });
                    }
                }
            }
            
            adCarListData = adCarList.Count>0 ? JsonConvert.SerializeObject(adCarList) : "[]";
        }

		/// <summary>
		/// 生成搜过栏的Html
		/// </summary>
		/// <returns></returns>
		private string MakeSelectBarHtml()
		{
			StringBuilder htmlCode = new StringBuilder();
			htmlCode.Append("<div class=\"tt-first y2015\">");
			htmlCode.AppendFormat("<ul class=\"tags-sub tags-sub-left\">{0}</ul>", sortModeHtml);
			htmlCode.AppendFormat("<span class=\"sub-tt-more\">共{0}个车型</span>", SerialNum);
			htmlCode.Append("</div>");
			return htmlCode.ToString();
		}
		/// <summary>
		/// 以大图方式显示选车结果
		/// </summary>
		/// <param name="serilaList"></param>
		private string MakeImageModeHtml(List<BitAuto.CarChannel.BLL.SerialInfo> serilaList)
		{
			int startIndex = (pageNum - 1) * pageSize + 1;
			int endIndex = startIndex + pageSize - 1;
			StringBuilder sbCarList = new StringBuilder();
			sbCarList.Append("<div class=\"buy-car\">");
			sbCarList.Append("<ul>");
			int counter = 0;
			foreach (BitAuto.CarChannel.BLL.SerialInfo info in serilaList)
			{
				counter++;
				CarNum += info.CarNum;
				if (counter < startIndex || counter > endIndex)
					continue;
				string serialUrl = "/" + info.AllSpell + "/";
				string shortName = info.ShowName.Replace("(进口)", "");
				if (info.SerialId == 1568)
				{
					shortName = "索纳塔八";
				}
                sbCarList.Append("<li>");
                sbCarList.AppendFormat("<a href=\"{0}\" class=\"car\"><div class=\"img-box\"><img src=\"{1}\" />", serialUrl, info.ImageUrl.Replace("_2.","_6."));
                if (info.IsAdvertise)   //设置“特价”标签
                    sbCarList.Append("<i class=\"recommend\"></i>");
                sbCarList.AppendFormat("</div><strong>{0}</strong>", shortName);
                sbCarList.AppendFormat("<p><em>{0}</em></p>", info.PriceRange);
                sbCarList.Append("</a></li>");
			}
			sbCarList.AppendLine("</ul>");
			sbCarList.AppendLine("</div>");
			return sbCarList.ToString();
		}

	}
    public class SuperSerialInfo
    {
        private int m_pvNum;

        public SuperSerialInfo(int id, string showName, string spell)
        {
            SerialId = id;
            ShowName = showName;
            AllSpell = spell;
            //m_carIdList = ",";
        }

        public int Pos { get; set; }

        public int MasterId { get; set; }
        public int SerialId { get; set; }

        public string ShowName { get; set; }

        public string AllSpell { get; set; }

        public string CarIdList { get; set; }

        //public List<CarInfoForSerialSummaryEntity> CarList { get; set; }
        /// <summary>
        ///     符合条件的车的数量
        /// </summary>
        public int CarNum { get; set; }

        public string ImageUrl { get; set; }

        public string PriceRange { get; set; }

    }
}