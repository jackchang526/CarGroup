using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL.Template;

namespace BitAuto.CarChannel.CarchannelWeb.App_Code
{
	/// <summary>
	///使用模板方法的页的基类
	/// </summary>
	public class TemplatePagebase : System.Web.UI.Page
	{
		protected int pageId;					//页面ID，在模板后台管理
		protected string pageName;				//页面名称
		protected PageTemplate pageTemplate;	//页面所使用的模板
		protected BaseEntity dataEntity;		//页面所属的主对象的数据实例
		protected List<TemplateParameter> paraList;			//页面上的参数列表

		public TemplatePagebase()
		{
			pageName = String.Empty;
		}

		/// <summary>
		/// 获取模板实例
		/// </summary>
		protected void GetTemplate()
		{
			TemplateManager mgr = TemplateManager.GetManager();
			GetParameters();
			pageTemplate = mgr.GetPageTemplate(pageId, paraList, false);		//此处不是预览
		}

		protected void GetParameters()
		{
			paraList = new List<TemplateParameter>();
			foreach (string paraName in Request.QueryString.AllKeys)
			{
				TemplateParameter para = new TemplateParameter(paraName, Request.QueryString[paraName]);
				paraList.Add(para);
			}
		}
	}
}