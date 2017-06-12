using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.Common.Interface;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace H5Web
{
	public partial class Default : PageBase
	{

		private SerialFourthStageBll serialFourthStageBll = new SerialFourthStageBll();
		protected string HtmlAllSerial = string.Empty;
		protected string HtmlAllChar = string.Empty;
		// 所有有第4极子品牌ID
		private List<int> listAllCsID = new List<int>();

		private int lastBsID = 0;
		private int lastCbID = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(30);
			GetAllSerial();
			GetAllMasterToSerial();
		}

		public void GetAllMasterToSerial()
		{
			XmlDocument doc = AutoStorageService.GetAllAutoXml();
			if (doc != null && doc.HasChildNodes)
			{
				// 已有字母
				List<string> listFirstChar = new List<string>();
				List<string> listHtmlForMasterToSerial = new List<string>();
				XmlNodeList xnlCs = doc.SelectNodes("/Params/MasterBrand/Brand/Serial");
				foreach (XmlNode xn in xnlCs)
				{
					int csid = BitAuto.Utils.ConvertHelper.GetInteger(xn.Attributes["ID"].Value);
					if (!listAllCsID.Contains(csid))
					{ continue; }
					XmlNode BsNode = xn.ParentNode.ParentNode;
					XmlNode CbNode = xn.ParentNode;
					//首字母
					string firstChar = "";
					if (BsNode.Attributes["Spell"] != null)
					{
						firstChar = BsNode.Attributes["Spell"].Value.Substring(0, 1).ToUpper();
					}
					if (firstChar == "")
					{ continue; }
					bool isNewChar = false;
					if (!listFirstChar.Contains(firstChar))
					{
						listFirstChar.Add(firstChar);
						isNewChar = true;
					}

					int currentBsID = BitAuto.Utils.ConvertHelper.GetInteger(BsNode.Attributes["ID"].Value);
					int currentCbID = BitAuto.Utils.ConvertHelper.GetInteger(CbNode.Attributes["ID"].Value);
					if (currentBsID != lastBsID)
					{
						if (lastBsID > 0)
						{
							listHtmlForMasterToSerial.Add("</dl></div>");
						}
						// 不同主品牌
						listHtmlForMasterToSerial.Add(string.Format("<div class=\"brand\" {0}>"
							, !isNewChar ? "" : string.Format("id=\"char_nav_{0}\"", firstChar)));
						listHtmlForMasterToSerial.Add(string.Format("<img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_{0}_100.png\" />", currentBsID));
						listHtmlForMasterToSerial.Add(string.Format("<h2>{0}</h2>", BsNode.Attributes["Name"].Value.Trim()));
						listHtmlForMasterToSerial.Add("<dl>");
						if (CbNode.Attributes["Name"].Value.Trim() != BsNode.Attributes["Name"].Value.Trim())
						{
							// listHtmlForMasterToSerial.Add(string.Format("<dt>{0}</dt>", CbNode.Attributes["Name"].Value.Trim()));
						}
						lastBsID = currentBsID;
						lastCbID = currentCbID;
					}
					if (currentCbID != lastCbID)
					{
						// 不同品牌
						// listHtmlForMasterToSerial.Add(string.Format("<dt>{0}</dt>", CbNode.Attributes["Name"].Value.Trim()));
						lastCbID = currentCbID;
					}
					// 子品牌节点
					listHtmlForMasterToSerial.Add(string.Format("<dd><a href=\"/{0}/\">{1}</a></dd>"
						, xn.Attributes["AllSpell"].Value, xn.Attributes["ShowName"].Value.Trim()));

				}
				if (listHtmlForMasterToSerial.Count > 0)
				{
					listHtmlForMasterToSerial.Add("</dl></div>");
				}
				HtmlAllSerial = string.Join("", listHtmlForMasterToSerial.ToArray());

				if (listFirstChar.Count > 0)
				{
					List<string> listTempChar = new List<string>();
					foreach (string charBs in listFirstChar)
					{
						listTempChar.Add(string.Format("<li id=\"char_{0}\"><a href=\"#char_nav_{0}\">{0}</a></li>", charBs));
					}
					HtmlAllChar = string.Join("", listTempChar.ToArray());
				}
			}
		}


		private void GetAllSerial()
		{
			List<string> listTemp = new List<string>();
			// List<int> listAllCsID = serialFourthStageBll.GetAllSerialInH5();
			listAllCsID = serialFourthStageBll.GetAllSerialInH5();
			//if (listAllCsID != null && listAllCsID.Count > 0)
			//{
			//	foreach (int csid in listAllCsID)
			//	{
			//		SerialEntity csEntity = (SerialEntity)DataManager.GetDataEntity(EntityType.Serial, csid);
			//		if (csid > 0 && csEntity != null && csEntity.Id > 0)
			//			listTemp.Add(string.Format("<a href=\"/{0}/\" /><b>{1}</b></a><br/>"
			//				, csEntity.AllSpell, csEntity.ShowName));
			//	}
			//}
			//if (listTemp.Count > 0)
			//{
			//	HtmlAllSerial = string.Join("", listTemp.ToArray());
			//}
		}

	}
}