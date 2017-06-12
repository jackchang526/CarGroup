using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.BLL.Template;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Template
{
	public partial class PreviewTemplate : TemplatePagebase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			GetParameters();
			base.pageTemplate.RenderTemplate(Response);
		}

		private void GetParameters()
		{
			this.pageId = ConvertHelper.GetInteger(Request.QueryString["pageId"]);
			if (this.pageId == 0)
			{
				Response.Write("请提供页面ID！");
				Response.End();
			}

			CarPage page = CarPage.GetCarPage(pageId);

			int templateId = ConvertHelper.GetInteger(Request.QueryString["templateId"]);
			if (templateId == 0)
				templateId = page.DefaultTemplateId;

			if (templateId == 0)
			{
				Response.Write("需要模板ID！");
				Response.End();
			}

			base.GetParameters();

			TemplateManager mgr = TemplateManager.GetManager();

			//获取预览方式的模板
			base.pageTemplate = mgr.GetPageTemplateById(templateId, base.paraList, true);

			int groupNum = ConvertHelper.GetInteger(Request.QueryString["groupNum"]);

			if (groupNum > 0)
			{
				//取关联关系的参数列表进行预览
				Dictionary<string, string> paraDic = CarPage.GetRelationParameter(pageId, templateId, groupNum);
				foreach (string paraName in paraDic.Keys)
				{
					base.pageTemplate.ParameterDictionary.Add(paraName, new TemplateParameter(paraName, paraDic[paraName]));
				}
			}
			else
			{
				foreach (TemplateParameter para in base.paraList)
				{
					if (para.Name == "groupNum" || para.Name == "pageId" || para.Name == "templateId")
						continue;
					base.pageTemplate.ParameterDictionary[para.Name] = para;
				}
			}
			string idParaName = String.Empty;
			EntityType eType = EntityType.Serial;
			switch (page.PageType)
			{
				case 1://厂商
					idParaName = "pId";
					eType = EntityType.Producer;
					break;
				case 2://主品牌
					idParaName = "masterId";
					eType = EntityType.MasterBrand;
					break;
				case 3://品牌
					idParaName = "brandId";
					eType = EntityType.Brand;
					break;
				case 4://级别
					idParaName = "levelId";
					eType = EntityType.Level;
					break;
				case 5://子品牌
				case 6://年款
					idParaName = "serialId";
					eType = EntityType.Serial;
					break;
				case 7://车型
					idParaName = "carId";
					eType = EntityType.Car;
					break;
			}

			if (!base.pageTemplate.ParameterDictionary.ContainsKey(idParaName))
			{
				Response.Write("需要参数：" + idParaName);
				Response.End();
			}
			int entityId = ConvertHelper.GetInteger(base.pageTemplate.ParameterDictionary[idParaName].ParameterValue);
			base.pageTemplate.DataEntity = DataManager.GetDataEntity(eType, entityId);
		}
	}
}