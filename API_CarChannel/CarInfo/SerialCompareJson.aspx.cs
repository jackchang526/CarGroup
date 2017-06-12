using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// 输出子品牌对比接口 add by chengl Feb.9.2012
	/// 输入子品牌ID，输出数量
	/// </summary>
	public partial class SerialCompareJson : PageBase
	{

		#region member

		private string callback = string.Empty;
		private string jsonVarName = string.Empty;
		private List<string> jsonString = new List<string>();
		private int csID = 0;
		private int top = 6;
		private string jsonType = string.Empty;

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			this.Response.ContentType = "application/x-javascript";
			if (!this.IsPostBack)
			{
				base.SetPageCache(60 * 24);
				GetPageParam();
				GetCsCompareJsonData();
				ResponseJson();
			}
		}

		#region private Method

		/// <summary>
		/// 取参数
		/// </summary>
		private void GetPageParam()
		{
			string strCsID = this.Request.QueryString["csID"];
			if (!string.IsNullOrEmpty(strCsID) && int.TryParse(strCsID, out csID))
			{ }

			string strTop = this.Request.QueryString["top"];
			if (!string.IsNullOrEmpty(strTop) && int.TryParse(strTop, out top))
			{ }
			if (top < 1 || top > 10)
			{ top = 6; }

			callback = this.Request.QueryString["callback"];

			jsonVarName = this.Request.QueryString["jsonVarName"];

			// 新参数
			jsonType = this.Request.QueryString["type"];
		}

		/// <summary>
		/// 取子品牌对比数据
		/// </summary>
		private void GetCsCompareJsonData()
		{
			string fileName = Path.Combine(WebConfig.DataBlockPath, "Data\\AllSerialCompareTop20.xml");
			if (File.Exists(fileName))
			{
				using (XmlReader xmlReader = XmlReader.Create(fileName))
				{
					while (xmlReader.ReadToFollowing("CS"))
					{
						// if (xmlReader.MoveToAttribute("ID"))
						{
							// int csid = int.Parse(xmlReader.Value);
							int csid = int.Parse(xmlReader.GetAttribute("ID"));
							if (csID > 0 && csid != csID)
							{ continue; }
							if (jsonString.Count > 1)
							{ jsonString.Add(","); }
							// 如果 新方式 ID 做key
							if (!string.IsNullOrEmpty(jsonType) && jsonType.ToLower() == "new")
							{
								jsonString.Add("\"" + csid.ToString() + "\":[");
							}
							else
							{
								jsonString.Add("{CsID:\"" + csid.ToString() + "\",");
								jsonString.Add("HotIDList:[");
							}
							int loop = 1;
							List<string> listCs = new List<string>(top * 2);
							// modified by chengl Aug.10.2012
							XmlReader inner = xmlReader.ReadSubtree();
							// while (xmlReader.ReadToFollowing("Compare"))
							while (inner.ReadToFollowing("Compare"))
							{
								if (xmlReader.MoveToAttribute("ID"))
								{
									int compareCsID = int.Parse(xmlReader.Value);
									if (listCs.Count > 0)
									{ listCs.Add(","); }
									listCs.Add("\"" + compareCsID + "\"");
									if (loop >= top)
									{ break; }
									loop++;
								}
							}
							if (listCs.Count > 0)
							{ jsonString.Add(string.Concat(listCs.ToArray())); }
							if (!string.IsNullOrEmpty(jsonType) && jsonType.ToLower() == "new")
							{
								jsonString.Add("]");
							}
							else
							{
								jsonString.Add("]}");
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 输出
		/// </summary>
		private void ResponseJson()
		{
			if (jsonString.Count > 0)
			{
				if (csID <= 0)
				{
					// 如果 新方式 ID 做key
					if (!string.IsNullOrEmpty(jsonType) && jsonType.ToLower() == "new")
					{
						jsonString.Insert(0, "{");
						jsonString.Add("}");
					}
					else
					{
						jsonString.Insert(0, "[");
						jsonString.Add("]");
					}
				}

				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Insert(0, callback + "(");
					jsonString.Add(")");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Insert(0, "var " + jsonVarName + " = ");
					jsonString.Add(";");
				}
				else
				{ }
			}
			else
			{
				// JsonP
				if (!string.IsNullOrEmpty(callback))
				{
					jsonString.Add(callback + "({})");
				}
				else if (!string.IsNullOrEmpty(jsonVarName))
				{
					// 非jsonP
					jsonString.Add("var " + jsonVarName + " = null;");
				}
				else
				{ jsonString.Add("参数错误"); }
			}
			Response.Write(string.Concat(jsonString.ToArray()));
		}

		#endregion

	}
}