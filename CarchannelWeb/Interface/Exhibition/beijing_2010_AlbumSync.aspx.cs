using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BCB = BitAuto.CarChannel.BLL;
using BCM = BitAuto.CarChannel.Model;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.Exhibition
{
	public partial class beijing_2010_AlbumSync : beijing_2010_PageBase
	{
		private BCB.Exhibition exhibitionBLL = new BitAuto.CarChannel.BLL.Exhibition();

		protected void Page_Load(object sender, EventArgs e)
		{
			base.SetPageCache(60);
			ExhibitionCarTypeList();
		}
		/// <summary>
		/// 打印展会列表
		/// </summary>
		private void ExhibitionCarTypeList()
		{
			Response.ContentType = "XML";
			if (_ExhibitionID < 1)
			{
				Response.Write("");
				return;
			}

			XmlDocument xmlDoc = new XmlDocument();

			// modified by chengl 先读Data下xml文件，没有再读库
			string eidXml = Path.Combine(WebConfig.DataBlockPath, string.Format("Data\\Exhibition\\{0}.xml", _ExhibitionID));
			if (File.Exists(eidXml))
			{
				xmlDoc.Load(eidXml);
			}
			else
			{
				xmlDoc = exhibitionBLL.GetExibitionXmlByExhibitionId(_ExhibitionID);
			}

			if (xmlDoc == null || !xmlDoc.HasChildNodes)
			{
				Response.Write("");
				return;
			}

			Response.Write(xmlDoc.InnerXml);
		}
	}
}