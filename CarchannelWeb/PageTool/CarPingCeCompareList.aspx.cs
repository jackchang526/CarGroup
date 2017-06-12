using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;

namespace BitAuto.CarChannel.CarchannelWeb.PageTool
{
	public partial class CarPingCeCompareList : PageBase
	{
		#region member
		protected string carIDs = string.Empty;
		protected string csIDsHtml = string.Empty;
		protected string csIDs = string.Empty;
		// 当前有效数量
		protected int validCount = 0;
		private ArrayList alCsID = new ArrayList();
		private ArrayList alValidCsID = new ArrayList();
		protected int tagID = 1;
		protected string htmlLeft = string.Empty;
		protected string htmlRight = string.Empty;

		protected string htmlLeftTitle = string.Empty;
		protected string htmlRightTitle = string.Empty;

		private List<string> listLeft = new List<string>();
		private List<string> listRight = new List<string>();

		private List<string> listLeftTitle = new List<string>();
		private List<string> listRightTitle = new List<string>();

		protected string titleLeft = string.Empty;
		protected string titleRight = string.Empty;
		protected string csNameLeft = string.Empty;
		protected string csNameRight = string.Empty;
		private bool[] tagsHas = { true, false, false, false, false, false, false, false, false, false, false, false };
		protected string pagePagination = string.Empty;
		protected string leftCSName = "";
		protected string rightCSName = "";
		protected string leftCSImg = "";
		protected string rightCSImg = "";
		protected string titleForSEO = string.Empty;// "【评测对比选车_汽车车型大全】";
		// 初始化评测的各个标签 名及匹配规则
		private Dictionary<int, EnumCollection.PingCeTag> dicAllTagInfo = new Dictionary<int, EnumCollection.PingCeTag>();

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10);
			if (!this.IsPostBack)
			{
				this.CheckCsIDGroup();
				this.GetSerialPingCeNew();
			}
		}

		// 取参数
		private void CheckCsIDGroup()
		{
			if (this.Request.QueryString["tagID"] != null && this.Request.QueryString["tagID"].ToString() != "")
			{
				string tagIDStr = this.Request.QueryString["tagID"].ToString();
				if (int.TryParse(tagIDStr, out tagID))
				{
					if (tagID < 1 || tagID > 12)
					{
						tagID = 1;
					}
				}
				else
				{
					tagID = 1;
				}
			}
			if (this.Request.QueryString["csids"] != null && this.Request.QueryString["csids"].ToString() != "")
			{
				int csid = 0;
				csIDs = this.Request.QueryString["csids"].ToString();
				if (csIDs.IndexOf(",") > 0)
				{
					string[] arrCsID = csIDs.Split(',');
					for (int i = 0; i < arrCsID.Length; i++)
					{
						if (int.TryParse(arrCsID[i].ToString(), out csid))
						{
							if (csid > 0 && !alCsID.Contains(csid.ToString()))
							{
								alCsID.Add(csid.ToString());
								csid = 0;
							}
						}
					}
				}
				else
				{
					if (int.TryParse(csIDs, out csid))
					{
						alCsID.Add(csid.ToString());
					}
				}
			}
			// 车型对比带过来的车型ID
			if (this.Request.QueryString["carIDs"] != null && this.Request.QueryString["carIDs"].ToString() != "")
			{
				carIDs = this.Request.QueryString["carIDs"].ToString();
			}
		}

		// 取新闻数据
		private void GetSerialPingCeNew()
		{
			if (alCsID.Count > 0)
			{
				dicAllTagInfo = CommonFunction.IntiPingCeTagInfo();
				int loop = 0;
				// modified by chengl Dec.28.2011
				string tagName = "";
				if (dicAllTagInfo.ContainsKey(tagID))
				{ tagName = dicAllTagInfo[tagID].tagName; }
				else
				{
					// 默认导语
					tagName = "导语";
				}
				// string tagName = this.GetTagNameByID(tagID);
				foreach (string csid in alCsID)
				{
					if (alValidCsID.Count >= 2)
					{ break; }
					string newsURL = "";
				int newID = 0;
				Dictionary<int, EnumCollection.PingCeTag> dictPingceNews = base.GetPingceTagsByCsId(BitAuto.Utils.ConvertHelper.GetInteger(csid));
				if (dictPingceNews.ContainsKey(tagID))
				{
					newsURL = dictPingceNews[tagID].url;
					string[] arrTempURL = dictPingceNews[tagID].url.Split('/');
					string pageName = arrTempURL[arrTempURL.Length - 1];
					if (pageName.Length >= 10)
					{
						if (int.TryParse(pageName.Substring(3, 7), out newID))
						{ }
					}
				}
				GetHasSomeTag(dictPingceNews);
				//int newID = base.GetPingCeNewIDByCsID(int.Parse(csid), out newsURL);
					if (newID > 0)
					{
						alValidCsID.Add(csid);
					}
					else
					{
						continue;
					}
					DataSet ds = base.GetPingCeNewByNewID(newID);
					if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("content"))
					{
						if (loop == 0)
						{
							titleLeft = ds.Tables[0].Rows[0]["facetitle"].ToString();
							GetCsName(int.Parse(csid), true, newsURL);
						}
						else
						{
							titleRight = ds.Tables[0].Rows[0]["facetitle"].ToString();
							GetCsName(int.Parse(csid), false, newsURL);
						}
						string newsContent = ds.Tables[0].Rows[0]["content"].ToString();

						string RegexString = "<div(?:[^<]*)?id=\"bt_pagebreak\"[^>]*>([^<]*)</div>";
						Regex r = new Regex(RegexString);
						string[] newsGroup = r.Split(newsContent);
						if (newsGroup.Length < 0)
						{
							continue;
						}
					//for (int i = 0; i < newsGroup.Length; i++)
					//{
					//    if (i % 2 == 1)
					//    {
					//        GetHasSomeTag(newsGroup[i].Replace(":", "："));
					//    }
					//}
						// 导语
						if (tagID == 1)
						{
							if (loop == 0)
							{
								// 左
								// htmlLeft = newsGroup[0].ToString().ToLower().Replace("<img", "<img width=420px ");
								htmlLeft = newsGroup[0].ToString();
								htmlLeft = Regex.Replace(htmlLeft, "<img ", "<img onload='changeImageHeightWidth(this)' ", RegexOptions.IgnoreCase);
								listLeft.Add(htmlLeft);
								loop++;
								continue;
							}
							else
							{
								// 右
								// htmlRight = newsGroup[0].ToString().ToLower().Replace("<img", "<img width=420px ");
								htmlRight = newsGroup[0].ToString();
								htmlRight = Regex.Replace(htmlRight, "<img ", "<img onload='changeImageHeightWidth(this)' ", RegexOptions.IgnoreCase);
								listRight.Add(htmlRight);
								continue;
							}
						}
						for (int i = 0; i < newsGroup.Length; i++)
						{
							if (i % 2 == 1)
							{
								if (newsGroup[i].IndexOf(tagName) >= 0)
								{
									if ((i + 1) <= newsGroup.Length - 1)
									{
										if (loop == 0)
										{
											// 左
											// htmlLeft = newsGroup[i + 1].ToString().ToLower().Replace("<img", "<img width=420px ");
											htmlLeft = newsGroup[i + 1].ToString();
											htmlLeft = Regex.Replace(htmlLeft, "<img ", "<img onload='changeImageHeightWidth(this)' ", RegexOptions.IgnoreCase);
											listLeft.Add(htmlLeft);
											// htmlLeft = newsGroup[i + 1].ToString();
											// loop++;
											break;
										}
										else
										{
											// 右
											// htmlRight = newsGroup[i + 1].ToString().ToLower().Replace("<img", "<img width=420px ");
											htmlRight = newsGroup[i + 1].ToString();
											htmlRight = Regex.Replace(htmlRight, "<img ", "<img onload='changeImageHeightWidth(this)' ", RegexOptions.IgnoreCase);
											listRight.Add(htmlRight);
											// loop++;
											break;
										}
									}
								}
							}
						}
						loop++;
					}

				}
				validCount = alValidCsID.Count;
				// 左右翻页
				// PageLeftAndRight();
			}

			if (alValidCsID.Count == 0)
			{
				// 2个都没有
				tagsHas[0] = false;
				listLeft.Add("<dl class=\"com_pingce_nocar\"></dl>");
				listRight.Add("<dl class=\"com_pingce_nocar\"></dl>");

				listLeftTitle.Add("<ul><li><select  name=\"selectMasterbrandForPhotoCompare\" id=\"selectMasterbrand_0\"><option>请选择品牌</option></select></li>");
				listLeftTitle.Add("<li><select name=\"selectSerialForPhotoCompare\" id=\"selectSerial_0\"><option>请选择系列</option></select></li></ul>");

				listRightTitle.Add("<ul><li><select name=\"selectMasterbrandForPhotoCompare\" disabled=\"disabled\"><option>请选择品牌</option></select></li>");
				listRightTitle.Add("<li><select name=\"selectSerialForPhotoCompare\" disabled=\"disabled\"><option>请选择系列</option></select></li></ul>");
				//// 没有选择的时候 
				//htmlLeft = "<div style=\"padding-top:100px;\" align=\"center\">";
				//htmlLeft += "<div id=\"selectForMasterSelectListSpan\" style=\"padding-bottom:5px\"><select id=\"selectForMasterSelectList\" style=\"width: 120px\"><option>选择品牌</option></select></div>";
				//htmlLeft += "<div id=\"selectForSerialSelectListSpan\" ><select id=\"selectForSerialSelectList\" style=\"width: 120px\"><option>选择车型</option></select></div>";
				//htmlLeft += "</div>";
				//// htmlRight = "";
				//htmlRight = "<div style=\"padding-top:100px;\" align=\"center\">";
				//htmlRight += "<div style=\"padding-bottom:5px\"><select disabled=\"disabled\" style=\"width: 120px\"><option>选择品牌</option></select></div>";
				//htmlRight += "<div><select disabled=\"disabled\" style=\"width: 120px\"><option>选择车型</option></select></div>";
				//htmlRight += "</div>";
			}
			else if (alValidCsID.Count == 1)
			{
				// 右侧没有
				listRight.Add("<dl class=\"com_pingce_nocar\"></dl>");

				listLeft.Insert(0, "<h4>" + titleLeft + "</h4>");
				listLeft.Insert(0, "<dl class=\"dl\"><dd class=\"dd\">");
				listLeft.Add("</dd></dl>");

				listRightTitle.Add("<ul><li><select name=\"selectMasterbrandForPhotoCompare\" id=\"selectMasterbrand_1\"><option>请选择品牌</option></select></li>");
				listRightTitle.Add("<li><select name=\"selectSerialForPhotoCompare\" id=\"selectSerial_1\"><option>请选择系列</option></select></li></ul>");

				listLeftTitle.Add("<dl><dt>");
				listLeftTitle.Add(leftCSImg);
				listLeftTitle.Add("</dt> <dd><strong>");
				listLeftTitle.Add(leftCSName);
				listLeftTitle.Add("</strong><p>&nbsp;</p>");
				listLeftTitle.Add("<p><a href=\"/pingceduibi/\" target=\"_self\" class=\"btn_com_op\">删除</a> <a target=\"_self\" href=\"javascript:changeSerial('0');\" class=\"btn_com_op\">换车</a></p>");
				listLeftTitle.Add("</dd></dl>");

				//// 右侧无选择时
				//htmlRight = "<div style=\"padding-top:100px;\" align=\"center\">";
				//htmlRight += "<div id=\"selectForMasterSelectListSpan\" style=\"padding-bottom:5px\"><select id=\"selectForMasterSelectList\" style=\"width: 120px\"><option>选择品牌</option></select></div>";
				//htmlRight += "<div id=\"selectForSerialSelectListSpan\" ><select id=\"selectForSerialSelectList\" style=\"width: 120px\"><option>选择车型</option></select></div>";
				//htmlRight += "</div>";
			}
			else if (alValidCsID.Count == 2)
			{
				// 2个都有
				listLeft.Insert(0, "<h4>" + titleLeft + "</h4>");
				listLeft.Insert(0, "<dl class=\"dl\"><dd class=\"dd\">");
				listLeft.Add("</dd></dl>");

				listRight.Insert(0, "<h4>" + titleRight + "</h4>");
				listRight.Insert(0, "<dl class=\"dl\"><dd class=\"dd\">");
				listRight.Add("</dd></dl>");

				listLeftTitle.Add("<dl><dt>");
				listLeftTitle.Add(leftCSImg);
				listLeftTitle.Add("</dt> <dd><strong>");
				listLeftTitle.Add(leftCSName);
				listLeftTitle.Add("</strong><p>&nbsp;</p>");
				listLeftTitle.Add("<p><a target=\"_self\" href=\"/pingceduibi/?csids=" + alValidCsID[1] + "&tagID=" + tagID + "\" class=\"btn_com_op\">删除</a> <a target=\"_self\" href=\"javascript:changeSerial('0');\" class=\"btn_com_op\">换车</a></p>");
				listLeftTitle.Add("</dd></dl>");

				listRightTitle.Add("<dl><dt>");
				listRightTitle.Add(rightCSImg);
				listRightTitle.Add("</dt> <dd><strong>");
				listRightTitle.Add(rightCSName);
				listRightTitle.Add("</strong><p>&nbsp;</p>");
				listRightTitle.Add("<p><a target=\"_self\" href=\"/pingceduibi/?csids=" + alValidCsID[0] + "&tagID=" + tagID + "\" class=\"btn_com_op\">删除</a> <a target=\"_self\" href=\"javascript:changeSerial('1');\" class=\"btn_com_op\">换车</a></p>");
				listRightTitle.Add("</dd></dl>");
			}
			else
			{ }

			htmlLeft = string.Concat(listLeft.ToArray());
			htmlRight = string.Concat(listRight.ToArray());

			htmlLeftTitle = string.Concat(listLeftTitle.ToArray());
			htmlRightTitle = string.Concat(listRightTitle.ToArray());

			// 同时2个评测对比时 更改title
			if (alValidCsID.Count != 2)
			{
				titleForSEO = "【汽车评测对比选车_车型评测对比】";
			}

			foreach (string csid in alValidCsID)
			{
				if (csIDsHtml != "")
				{
					csIDsHtml += "," + csid.ToString();
				}
				else
				{
					csIDsHtml = csid.ToString();
				}
			}
			// 左右翻页
			PageLeftAndRight();
		}

		// 左右翻页
		private void PageLeftAndRight()
		{
			// 上下翻页
			bool isLeft = false;
			bool isRight = false;
			int iLeft = 0;
			int iRoght = 0;
			for (int i = 0; i < tagsHas.Length; i++)
			{
				if (i + 1 < tagID && tagsHas[i])
				{
					iLeft = i + 1;
					isLeft = true;
				}
				if (i + 1 > tagID && tagsHas[i])
				{
					if (iRoght < 1)
					{
						iRoght = i + 1;
					}
					isRight = true;
				}
			}
			if (isLeft)
			{
				// pagePagination += "<a class=\"next_on\" href=\"/pingceduibi/?csids=" + csIDsHtml + "&tagID=" + iLeft.ToString() + "&carIDs=" + carIDs + "\">&lt;&lt;上一页</a>";
				pagePagination += "<a class=\"next_on\" href=\"/pingceduibi/?csids=" + csIDs + "&tagID=" + iLeft.ToString() + "&carIDs=" + carIDs + "\">&lt;&lt;上一页</a>";
				// pagePagination = ""
			}
			else
			{
				pagePagination += "<span class=\"preview_off\">&lt;&lt;上一页</span>";
			}
			if (isRight)
			{
				pagePagination += "<a class=\"next_on\" href=\"/pingceduibi/?csids=" + csIDsHtml + "&tagID=" + iRoght.ToString() + "&carIDs=" + carIDs + "\">下一页&gt;&gt;</a>";
			}
			else
			{
				pagePagination += "<span class=\"preview_off\">下一页&gt;&gt;</span>";
			}
		}

		private void GetCsName(int csID, bool isFir, string newsURL)
		{
			string otherCS = "";
			if (alCsID.Count > 0)
			{
				foreach (string csid in alCsID)
				{
					if (csid != csID.ToString())
					{
						if (otherCS == "")
						{
							otherCS = csid;
						}
						else
						{
							otherCS = otherCS + "," + csid;
						}
					}
				}
			}
			DataSet ds = base.GetAllSErialInfo();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select(" cs_id =" + csID + " ");
				if (drs != null && drs.Length > 0)
				{
					if (isFir)
					{
						// leftCSName = "易车评测：<a target=\"_blank\" href=\"" + newsURL + "\">" + drs[0]["cs_showName"].ToString().Trim() + "</a><a href=\"/pingceduibi/?csids=" + otherCS + "&tagID=" + tagID.ToString() + "\" class=\"del\"></a>";
						leftCSName = "<a target=\"_blank\" href=\"" + newsURL + "\">" + drs[0]["cs_showName"].ToString().Trim() + "</a>";
						string pic = "";
						int picCount = 0;
						base.GetSerialPicAndCountByCsID(csID, out pic, out picCount, true);
						leftCSImg = "<a href=\"/" + drs[0]["allSpell"].ToString() + "/\" target=\"_blank\"><img src=\"" + pic.Replace("_2.", "_5.") + "\" alt=\"\" /></a>";
					}
					else
					{
						// rightCSName = "易车评测：<a target=\"_blank\" href=\"" + newsURL + "\">" + drs[0]["cs_showName"].ToString().Trim() + "</a><a href=\"/pingceduibi/?csids=" + otherCS + "&tagID=" + tagID.ToString() + "\" class=\"del\"></a>";
						rightCSName = "<a target=\"_blank\" href=\"" + newsURL + "\">" + drs[0]["cs_showName"].ToString().Trim() + "</a>";
						string pic = "";
						int picCount = 0;
						base.GetSerialPicAndCountByCsID(csID, out pic, out picCount, true);
						rightCSImg = "<a href=\"/" + drs[0]["allSpell"].ToString() + "/\" target=\"_blank\"><img src=\"" + pic.Replace("_2.", "_5.") + "\" alt=\"\" /></a>";
					}
					// title for SEO 
					if (titleForSEO != "")
					{
						titleForSEO += drs[0]["cs_showName"].ToString().Trim() + "_" + GetTagNameForSEOTitle(tagID) + "对比】";
					}
					else
					{
						titleForSEO = "【" + drs[0]["cs_showName"].ToString().Trim() + "和";
					}
				}
			}
		}
	private void GetHasSomeTag(Dictionary<int, EnumCollection.PingCeTag> dictPingceNews)
	{
		int index = -1;
		foreach (KeyValuePair<int, EnumCollection.PingCeTag> kvp in dictPingceNews)
		{
			// tagID 从1开始 tagsHas 从0开始
			index = kvp.Key - 1;
			tagsHas[index] = true;
		}
	}
		private void GetHasSomeTag(string pageTitle)
		{
			int index = -1;
			if (dicAllTagInfo != null && dicAllTagInfo.Count > 0)
			{
				foreach (KeyValuePair<int, EnumCollection.PingCeTag> kvp in dicAllTagInfo)
				{
					Regex r = new Regex(kvp.Value.tagRegularExpressions);
					if (r.IsMatch(pageTitle))
					{
						// tagID 从1开始 tagsHas 从0开始
						index = kvp.Key - 1;
						break;
					}
				}
			}
			if (index > 0)
			{
				tagsHas[index] = tagsHas[index] || true;
			}
		}

		protected string HasThisTag(int index)
		{
			string temp = "";
			if (tagsHas.Length > index && tagsHas[index])
			{
				temp = "href=\"/pingceduibi/?csids=" + csIDs + "&tagID=" + Convert.ToString(index + 1) + "&carIDs=" + carIDs + "\" ";
			}
			else
			{
				temp = " class=\"nolink\" href=\"javascript:void(0);\"";
			}
			return temp;
		}

		protected string GetTagClassByID(int tagid)
		{
			string className = "";
			if (tagid == tagID && alCsID.Count > 0)
			{
				className = " class=\"on\" ";
			}
			return className;
		}

		private string GetTagNameForSEOTitle(int tagid)
		{
			string tagName = "";
			if (tagid == 1)
			{ tagName = "评测"; }
			if (tagid == 2)
			{ tagName = "外观"; }
			if (tagid == 3)
			{ tagName = "内饰"; }
			if (tagid == 4)
			{ tagName = "空间"; }
			if (tagid == 5)
			{ tagName = "视野"; }
			if (tagid == 6)
			{ tagName = "灯光"; }
			if (tagid == 7)
			{ tagName = "动力"; }
			if (tagid == 8)
			{ tagName = "操控"; }
			if (tagid == 9)
			{ tagName = "舒适性"; }
			if (tagid == 10)
			{ tagName = "油耗"; }
			if (tagid == 11)
			{ tagName = "配置"; }
			if (tagid == 12)
			{ tagName = "总结"; }
			return tagName;
		}
	}
}