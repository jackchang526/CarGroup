using System;
using System.IO;
using System.Data;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BCB = BitAuto.CarChannel.BLL;
using BCM = BitAuto.CarChannel.Model;
using BCC = BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class beijing_2010_LevelQuene : beijing_2010_PageBase
	{
		private int iType;//1:车型级别;2:标签;3:价格区间
		private int QueneId;
		private DataSet _QueneDs = new DataSet();
		private XmlDocument _ExhibitionXmlDoc = new XmlDocument();
		private BCB.Exhibition _ExhibibitonBLL = new BitAuto.CarChannel.BLL.Exhibition();
		private int StandardValue = 10000;

		protected void Page_Load(object sender, EventArgs e)
		{
			GetParam();
			ValidatoryParam();
			InitData();
		}
		/// <summary>
		/// 得到页面参数
		/// </summary>
		private void GetParam()
		{
			iType = string.IsNullOrEmpty(Request.QueryString["type"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["type"].ToString());
			QueneId = string.IsNullOrEmpty(Request.QueryString["quene"])
				? 0 : ConvertHelper.GetInteger(Request.QueryString["quene"].ToString());
		}
		/// <summary>
		/// 验证参数是否正确
		/// </summary>
		private void ValidatoryParam()
		{
			if (iType == 0)
			{
				Response.Write("");
				Response.End();
				return;
			}
			_ExhibitionXmlDoc = _ExhibibitonBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			if (_ExhibitionXmlDoc == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root") == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes == null
				|| _ExhibitionXmlDoc.SelectSingleNode("root").ChildNodes.Count < 1)
			{
				Response.Write("");
				Response.End();
				return;
			}
			string QueneData = BCC.CommonFunction.GetContentByUrl(GetUserVoteUrl());
			if (string.IsNullOrEmpty(QueneData))
			{
				Response.Write("");
				Response.End();
				return;
			}

			try
			{
				using (StringReader sr = new StringReader(QueneData))
				{
					_QueneDs.ReadXml(sr, XmlReadMode.InferTypedSchema);
				}

				if (_QueneDs == null || _QueneDs.Tables.Count < 1 || _QueneDs.Tables[0].Rows.Count < 1)
				{
					Response.Write("");
					Response.End();
					return;
				}
			}
			catch
			{
				Response.Write("");
				Response.End();
				return;
			}
		}
		/// <summary>
		/// 初始化数据
		/// </summary>
		private void InitData()
		{
			switch (iType)
			{
				case 1:
					Response.Write(InitCarLevel());
					break;
				case 2:
					Response.Write(InitCarAttribute());
					break;
				case 3:
					Response.Write(InitCarPrice());
					break;
				default:
					Response.Write("");
					break;
			}
		}
		/// <summary>
		/// 打印车的级别
		/// </summary>
		/// <returns></returns>
		private string InitCarLevel()
		{
			if (!CsLevelReverse.ContainsKey(QueneId.ToString()))
			{
				return "";
			}

			StringBuilder htmlString = new StringBuilder();
			int index = 1;

			foreach (DataRow dr in _QueneDs.Tables[0].Rows)
			{
				if (index > 10)
				{
					break;
				}
				string selectPath = "root/MasterBrand/Brand/Serial[@ID='"
								   + dr["ukey"].ToString()
								   + "' and @CsLevel='"
								   + CsLevelReverse[QueneId.ToString()] + "']";

				XmlElement xEleme = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode(selectPath);

				if (xEleme != null)
				{
					htmlString.Append(PrintfHTML(xEleme, dr["pv"].ToString(), index));
					index++;
				}
			}

			return htmlString.ToString();
		}
		/// <summary>
		/// 打印车的标签
		/// </summary>
		/// <returns></returns>
		private string InitCarAttribute()
		{
			if (!AttributeUrlReverse.ContainsKey(QueneId.ToString()))
			{
				return "";
			}

			StringBuilder htmlString = new StringBuilder();
			int index = 1;

			foreach (DataRow dr in _QueneDs.Tables[0].Rows)
			{
				if (index > 10)
				{
					break;
				}

				string selectPath = "root/MasterBrand/Brand/Serial[@ID='"
								  + dr["ukey"].ToString()
								  + "']/Attribute[@ID='"
								  + AttributeUrlReverse[QueneId.ToString()] + "']";

				XmlElement xEleme = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode(selectPath);

				if (xEleme != null)
				{
					htmlString.Append(PrintfHTML((XmlElement)xEleme.ParentNode, dr["pv"].ToString(), index));
					index++;
				}
			}

			return htmlString.ToString();

		}
		/// <summary>
		/// 打印车的价格
		/// </summary>
		/// <returns></returns>
		private string InitCarPrice()
		{
			if (QueneId < 1 || QueneId > 8)
			{
				return "";
			}

			StringBuilder htmlString = new StringBuilder();
			int index = 1;

			foreach (DataRow dr in _QueneDs.Tables[0].Rows)
			{
				if (index > 10)
				{
					break;
				}

				string selectPath = "root/MasterBrand/Brand/Serial[@ID='"
						+ dr["ukey"].ToString()
						+ "' and contains(@MultiPriceRange,',"
						+ QueneId.ToString() + ",')]";

				XmlElement xEleme = (XmlElement)_ExhibitionXmlDoc.SelectSingleNode(selectPath);

				if (xEleme != null)
				{
					htmlString.Append(PrintfHTML(xEleme, dr["pv"].ToString(), index));
					index++;
				}
			}

			return htmlString.ToString();
		}
		/// <summary>
		/// 打印HTML
		/// </summary>
		/// <param name="name">子品牌名</param>
		/// <param name="url">子品牌url</param>
		/// <param name="count">数量</param>
		/// <returns></returns>
		private string PrintfHTML(XmlElement xElem, string count, int index)
		{
			//子品牌链接地址
			string serialUrl = GetSerialUrl(xElem);

			//string url = "http://chezhan.bitauto.com/beijing/2010/"
			//            + ((XmlElement)xElem.ParentNode.ParentNode).GetAttribute("AllSpell") + "/"
			//            + xElem.GetAttribute("AllSpell") + "/";

			StringBuilder liString = new StringBuilder();
			liString.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a><small>{2}</small></li>"
								  , serialUrl
								  , xElem.GetAttribute("Name")
								  , (StandardValue + ConvertHelper.GetInteger(count) * 100 + ConvertHelper.GetInteger(10 - index) * (10 - index) - new Random().Next(2, 10)));
			return liString.ToString();
		}
	}
}