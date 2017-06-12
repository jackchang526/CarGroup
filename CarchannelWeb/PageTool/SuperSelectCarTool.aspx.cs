using System;
using System.IO;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using System.Collections.Specialized;

namespace BitAuto.CarChannel.CarchannelWeb.PageTool
{
	/// <summary>
	/// 高级选车
	/// </summary>
	public partial class SuperSelectCarTool : PageBase
	{
		protected string configParaHtml = string.Empty;
		private string sortMode;
		protected string sortModeHtml;
		private int pageNum;
		private int pageSize;


		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(10, true);
			GetParameters();
			configParaHtml = GenerateSearchQueryInitScript();
			//自定义参数
			//RenderConfigPara();
		}

		/// <summary>
		/// 获取参数
		/// </summary>
		private void GetParameters()
		{
			//排序
			sortMode = Request.QueryString["s"];
			if (sortMode == "1")
			{
				sortMode = "guanzhu_up";
				sortModeHtml = "<a id=\"moRen\" class= \"current-uparrow\" href=\"javascript:void(0)\"><em>按关注</em></a> | <a  id=\"jiaGe\" href=\"javascript:void(0)\"class=\"uparrow\">按价格</a>";
			}
			else if (sortMode == "2")
			{
				sortMode = "price_up";
				sortModeHtml = "<a id=\"moRen\" class= \"downarrow\" href=\"javascript:void(0)\">按关注</a> | <a  id=\"jiaGe\" href=\"javascript:void(0)\"class=\"current-uparrow\"><em>按价格</em></a>";
			}
			else if (sortMode == "3")
			{
				sortMode = "price_down";
				sortModeHtml = "<a id=\"moRen\" class= \"downarrow\" href=\"javascript:void(0)\">按关注</a> | <a  id=\"jiaGe\" href=\"javascript:void(0)\"class=\"current-downarrow\"><em>按价格</em></a>";
			}
			else
			{
				sortMode = "guanzhu_down";
				sortModeHtml = "<a id=\"moRen\" class= \"current-downarrow\" href=\"javascript:void(0)\"><em>按关注</em></a> | <a  id=\"jiaGe\" href=\"javascript:void(0)\"class=\"uparrow\">按价格</a>";
			}

			//页号
			var tmpStr = Request.QueryString["page"];
			bool isPage = Int32.TryParse(tmpStr, out pageNum);
			if (!isPage)
				pageNum = 1;
			pageSize = 10;
		}

		//初始化SuperSelectCarTool参数
		protected string GenerateSearchQueryInitScript()
		{
			string resultString = "[";
			NameValueCollection nvcQuery = Request.QueryString;
			foreach (var queKey in nvcQuery.AllKeys)
			{
				if (string.IsNullOrEmpty(queKey)) continue;

 				if (queKey == "more")
				{
					var valueList = nvcQuery[queKey].Split('_');
					foreach (var value in valueList)
					{
						resultString += "'" + queKey.ToString() + "=" + value + "',";
					}
				}
				else if (nvcQuery[queKey].Split(',').Length > 1)
				{
					var valueList = nvcQuery[queKey].Split(',');
					foreach (var value in valueList)
					{
						resultString += "'" + queKey + "=" + value + "',";
					}
				}
				else
				{
					resultString += "'" + queKey + "=" + nvcQuery[queKey] + "',";
				}
 			}
			resultString = resultString.TrimEnd(',') + "]";
			return resultString;
		}
		#region  暂时不用，生成自定义参数HTML
		/// <summary>
		/// 生成自定义参数
		/// </summary>
		private void RenderConfigPara()
		{
			StringBuilder sbParameter = new StringBuilder();
			StringBuilder sbTemp = new StringBuilder();
			XmlDocument docPC = new XmlDocument();
			string cache = "SuperSelectCar_ParameterConfiguration";
			object parameterConfiguration = null;
			CacheManager.GetCachedData(cache, out parameterConfiguration);
			if (parameterConfiguration != null)
			{
				docPC = (XmlDocument)parameterConfiguration;
			}
			else
			{
				var filePath = System.Web.HttpContext.Current.Server.MapPath("~") + "\\config\\SelectCarParams.config";
				if (File.Exists(filePath))
				{
					docPC.Load(filePath);
					CacheManager.InsertCache(cache, docPC, new System.Web.Caching.CacheDependency(filePath), DateTime.Now.AddMinutes(60));
				}
			}

			// 更多参数
			if (docPC != null && docPC.HasChildNodes)
			{
				XmlNode rootPC = docPC.DocumentElement;
				if (docPC.ChildNodes.Count > 1)
				{
					foreach (XmlNode parameterList in rootPC.ChildNodes)
					{
						if (parameterList.NodeType == XmlNodeType.Element)
						{
							// && parameterList.Attributes.GetNamedItem("Type").Value.ToString() == "2"
							if (parameterList.HasChildNodes)
							{
								sbTemp.AppendLine("<h6>" + parameterList.Attributes.GetNamedItem("Name").Value + "</h6>");
								XmlNodeList xmlNode = parameterList.ChildNodes;
								foreach (XmlNode item in xmlNode)
								{
									if (item.NodeType != XmlNodeType.Element)
									{ continue; }
									sbTemp.AppendLine("<dl><dt>" + item.Attributes.GetNamedItem("Name").Value + "</dt><dd><ul class=\"tj_list\">"); ;
									foreach (XmlNode itemFilter in item)
									{
										if (itemFilter.NodeType != XmlNodeType.Element)
										{ continue; }
										sbTemp.AppendLine("<li><label><input type=\"checkbox\" id=\"more_" + itemFilter.Attributes.GetNamedItem("Id").Value + "\">" + itemFilter.Attributes.GetNamedItem("Name").Value + "</label></li>");
									}
									sbTemp.AppendLine("</ul></dd></dl>");
								}
							}
						}
					}
				}
				configParaHtml = sbTemp.ToString();
			}
		}

		#endregion
	}
}