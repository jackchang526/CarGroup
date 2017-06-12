﻿using System;
using System.Xml;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Enum;

namespace BitAuto.CarChannel.CarchannelWeb.PageMasterV2
{
	/// <summary>
	/// 主品牌新闻页面
	/// 包括内容：新闻、评测、导购、用车、行情
	/// </summary>
	public partial class CarMasterBrandPageNews : PageBase
	{
		protected string _RequestType = string.Empty;//新闻:news;行情:hangqing;导购:daogou;用车:yongche
		protected int _MasterBrandId = 0;
		protected string _MasterBrandName = string.Empty;
		protected string _MasterBrandSpell = string.Empty;
		protected string _FirstBrandSpell = string.Empty;
		protected string _Title = string.Empty;
		protected string _NewsList = string.Empty;
		protected string _TagContent = string.Empty;
		protected string _MasterBrandGuilder = string.Empty;
		protected string _WenZhangHeader = string.Empty;
		protected int _PageIndex = 0;
		protected int _PageTotal = 0;
		protected int _NewsCount = 0;
		private Dictionary<string, string> relationList = new Dictionary<string, string>();
		private int _PageSize = 10;
		private Car_SerialBll _Csb = new Car_SerialBll();
		private Car_BrandBll _Cbb = new Car_BrandBll();

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			GetParam();
			InitData();
		}
		/// <summary>
		/// 得到参数
		/// </summary>
		private void GetParam()
		{
			_MasterBrandId = ConvertHelper.GetInteger(Request.QueryString["id"]);
			_RequestType = string.IsNullOrEmpty(Request.QueryString["type"])
				? "" : Request.QueryString["type"].ToLower();
			_PageIndex = ConvertHelper.GetInteger(Request.QueryString["page"]);
			if (_PageIndex == 0)
				_PageIndex = 1;
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			InitRelation();
			InitMasterBrandEntity();
			InitTitle();
            _WenZhangHeader = _Cbb.GetWenZhangHeaderFor1200(_MasterBrandId, _MasterBrandName, _MasterBrandSpell, 0, "masterbrand", _RequestType);
			InitNewsCount();
			//InitNewsList();
			InitNewsListNew();
		}
		/// <summary>
		/// 初始化对应关系
		/// </summary>
		private void InitRelation()
		{
			relationList.Add("xinwen", "新闻");
			relationList.Add("hangqing", "行情");
			relationList.Add("daogou", "导购");
			relationList.Add("pingce", "评测");
			relationList.Add("yongche", "用车");
		}
		/// <summary>
		/// 初始化标题
		/// </summary>
		private void InitTitle()
		{
			if (relationList.Count < 1) return;
			if (string.IsNullOrEmpty(_MasterBrandName)) return;
			if (relationList.ContainsKey(_RequestType))
			{
				_TagContent = relationList[_RequestType];
			}
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.Append("【");
			strBuilder.AppendFormat("{0}汽车{1}", _MasterBrandName, _TagContent);
			strBuilder.Append("】");
			strBuilder.AppendFormat("{0}汽车", _MasterBrandName);
			strBuilder.AppendFormat("{0}_", _TagContent);

			if (_RequestType == "yongche")
			{
				strBuilder.Append("新闻_行情_导购_");
			}
			else
			{
				foreach (KeyValuePair<string, string> entity in relationList)
				{
					if (entity.Key == _RequestType || entity.Key == "yongche") continue;
					strBuilder.AppendFormat("{0}_", entity.Value);
				}
			}

			_Title = strBuilder.ToString().TrimEnd(new char[] { '_' }) + "-易车网";
		}
		/// <summary>
		/// 初始化主品牌对象
		/// </summary>
		private void InitMasterBrandEntity()
		{
			if (_MasterBrandId < 1)
				Response.Redirect("/404error.aspx");
			MasterBrandEntity mEntity = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, _MasterBrandId);
			if (mEntity == null || mEntity.Id == 0)
				Response.Redirect("/404error.aspx");
			_MasterBrandName = mEntity.Name;
			_MasterBrandSpell = mEntity.AllSpell;
		}

		/// <summary>
		/// 初始化新闻列表
		/// </summary>
		private void InitNewsListNew()
		{


			DataSet ds = null;
			int rowCount = _NewsCount;
			string newsType = _RequestType.ToLower();

			if (newsType == "pingce")
			{
				newsType = "treepingce";
				ds = new CarNewsBll().GetMasterBrandNews(_MasterBrandId, CarNewsType.treepingce, _PageSize, _PageIndex,
					ref rowCount);
			}
			else
			{
				if (newsType == "wenzhang")
				{
					List<int> carTypeIdList = new List<int>() 
					{ 
					(int)CarNewsType.daogou,
					(int)CarNewsType.yongche,
					(int)CarNewsType.xinwen
					};
					ds = new CarNewsBll().GetMasterBrandNews(_MasterBrandId,
								carTypeIdList, _PageSize, _PageIndex, ref rowCount);
				}
				else
				{
					Type enumTYpe = typeof(CarNewsType);
					foreach (string name in Enum.GetNames(enumTYpe))
					{
						if (name == newsType)
						{
							ds = new CarNewsBll().GetMasterBrandNews(_MasterBrandId,
								(CarNewsType)Enum.Parse(enumTYpe, name), _PageSize, _PageIndex, ref rowCount);
							break;
						}
					}
				}
			}

			_PageTotal = rowCount / _PageSize + (rowCount % _PageSize == 0 ? 0 : 1);
			if (_PageTotal < 1) return;
			if (_PageIndex > _PageTotal) _PageIndex = _PageTotal;
			_NewsCount = rowCount;
			GetContentNew(ds, rowCount);

		}

		/// <summary>
		/// 得到新闻内容
		/// </summary>
		/// <param name="xmlDoc"></param>
		/// <returns></returns>
		private void GetContentNew(DataSet ds, int rowCount)
		{
			if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return;
			var contentString = new StringBuilder();
            contentString.AppendFormat("<div class=\"list-box\">");
			int i = 0;
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				int newsId = ConvertHelper.GetInteger(row["cmsnewsid"]);
				string title = CommonFunction.NewsTitleDecode(row["title"].ToString());
				string filepath = row["filepath"].ToString();
				string publishTime = row["publishtime"].ToString();

				string content = row["Content"].ToString();
				string picUrl = ConvertHelper.GetString(row["Picture"]);
				string imageUrl = (!string.IsNullOrEmpty(picUrl) && picUrl.IndexOf("/not") < 0) ? picUrl : ConvertHelper.GetString(row["FirstPicUrl"]);

				string editorName = row["EditorName"].ToString();
				string sourceName = row["SourceName"].ToString();

				if (!string.IsNullOrEmpty(imageUrl)) 
				{
                    contentString.AppendFormat("<div class=\"article-card horizon\">");
                    contentString.AppendFormat("<div class=\"inner-box\">");
                    contentString.AppendFormat("<a href=\"{0}\" target=\"_blank\" class=\"figure\">", filepath);
                    contentString.AppendFormat(
                        "<img width=\"240\" height=\"160\" src=\"{0}\">", imageUrl);
                    contentString.Append("</a>");
				}
				else
				{
                    contentString.AppendFormat("<div class=\"article-card horizon text\">");
                    contentString.AppendFormat("<div class=\"inner-box\">");
				}

				//得到子品牌信息
				int serialId = ConvertHelper.GetInteger(row["serialid"]);
				Car_SerialEntity sInfo = _Csb.GetSerialInfoEntity(serialId);
                contentString.AppendFormat("<div class=\"details\">");
                contentString.AppendFormat("<h2><a href=\"{0}\" target=\"_blank\" title=\"{1}\">{1}</a></h2>", filepath, title);
                contentString.AppendFormat("<div class=\"description\"><p>{1}… <a target=\"_blank\" class=\"more type-1\" href=\"{0} \">查看更多&gt;&gt;</a></p></div>", filepath, content.Length < 106 ? content : content.Substring(0, 106));
                contentString.AppendFormat("<div class=\"info\"><div class=\"first\"><span class=\"view\" data-vnewsid=\"{4}\">0</span><span class=\"comment\" data-cnewsid=\"{4}\">{3}</span></div><div class=\"last\"><span class=\"origin\">来源：<em>{0}</em></span><span class=\"author\">作者：<em>{1}</em></span><span class=\"time\">{2}</span></div></div>",
                    sourceName,
                    editorName,
                    ConvertHelper.GetDateTime(publishTime).ToString("yyyy-MM-dd"),
                    ConvertHelper.GetInteger(row["CommentNum"]),
                    newsId);

                contentString.Append("</div>");
                contentString.Append("</div>");
                contentString.Append("</div>");
				i++;
			}

			contentString.Append("</div>");
			_NewsList += contentString.ToString();
			if (_PageTotal < 2) return;

			string pageUrl = string.Format("/{0}/{1}/@page@/", _MasterBrandSpell, _RequestType);

			this.AspNetPager1.UrlRewritePattern = pageUrl.Replace("@page@", "{0}");
			this.AspNetPager1.RecordCount = rowCount;
            this.AspNetPager1.PageDivCSS = "pagination";
			this.AspNetPager1.PageSize = _PageSize;
			this.AspNetPager1.CurrentPageIndex = _PageIndex;
			this.AspNetPager1.Visible = true;
			this.AspNetPager1.DotShowLimit = 8;
            this.AspNetPager1.PreviousTextValue = "<";
            this.AspNetPager1.PreviewClassName = _PageIndex == 1 ? "preview-off" : "preview-on";
            this.AspNetPager1.NextTextValue = ">";
            this.AspNetPager1.NextClassName = _PageIndex == _PageTotal ? "preview-off" : "next-on";
		}

		/// <summary>
		/// 初始化新闻数量
		/// </summary>
		private void InitNewsCount()
		{
			string type = string.Empty;
			switch (_RequestType)
			{
				case "xinwen":
					type = "xinwen";
					break;
				case "pingce":
					type = "treepingce";
					break;
				default:
					type = _RequestType;
					break;
			}
			Dictionary<int, Dictionary<string, int>> newCount = AutoStorageService.GetMasterBrandOrBrandNewsCountList("masterbrand");
			if (newCount != null
				&& newCount.ContainsKey(_MasterBrandId)
				&& newCount[_MasterBrandId].ContainsKey(type)
				&& newCount[_MasterBrandId][type] > 0)
			{
				_NewsCount = newCount[_MasterBrandId][type];
			}

		}
	}
}