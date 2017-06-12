using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL.Template;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Template
{
	public partial class DefaultTemplate : TemplatePagebase
	{
		private int serialId;		//子品牌ID

		protected void Page_Load(object sender, EventArgs e)
		{
			Getparameter();
			this.pageId = 1;
			base.GetTemplate();
			base.pageTemplate.DataEntity = DataManager.GetDataEntity(EntityType.Serial, serialId);
			MakeSeoInfo();
			//Response.Write(pageTemplate.RenderTemplate());
			pageTemplate.RenderTemplate(Response);
			//Response.End();
		}

		private void Getparameter()
		{
			Int32.TryParse(Request.QueryString["SerialId"], out serialId);
			if (serialId <= 0)
				Response.Redirect("/404error.aspx?info=Parameter error!");
		}

		/// <summary>
		/// 生成Seo信息
		/// </summary>
		private void MakeSeoInfo()
		{
			SerialEntity se = (SerialEntity)base.pageTemplate.DataEntity;
			base.pageTemplate.PageTitle = new PageBase().GetSerialSummaryTitleByID(serialId, "【" + se.SeoName + "】" + se.Brand.MasterBrand.Name + se.Name + "_" + se.SeoName + "报价_" + se.SeoName + "论坛_油耗-易车网 ");
			base.pageTemplate.PageKeywords = se.SeoName + "," + se.SeoName + "报价," + se.SeoName + "价格," + se.SeoName + "参数," + se.SeoName + "论坛," + se.SeoName + "图片," + se.SeoName + "油耗," + se.SeoName + "口碑," + se.Brand.MasterBrand.Name + se.SeoName;
			base.pageTemplate.PageDescription = se.SeoName + "," + se.Brand.MasterBrand.Name + se.SeoName + ":易车网提供最新" + se.Brand.MasterBrand.Name + se.SeoName + "报价/价格,"
				+ se.SeoName + "论坛,图片/" + se.PicCount + "张," + se.SeoName + "油耗,参数配置," + se.SeoName + "怎么样,答疑/" + se.AskCount + "条,二手" + se.SeoName + ",评测,视频,经销商等" + se.SeoName + "汽车信息";
		}
	}
}