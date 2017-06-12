using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 页面模板类，负责整个页面模板的分析与输出
	/// </summary>
	public class PageTemplate
	{
		private Dictionary<string, TemplateParameter> m_paraDic;			//参数字典
		private List<HtmlSegment> m_htmlSegmentList;						//Html片段列表
		private Dictionary<string, HtmlSegment> m_moduleSegmentDic;			//模块片段字典
		private List<ModuleBase> m_moduleList;								//模块列表
		private List<ScriptOrCssElement> m_scriptList;						//脚本列表
		private List<ScriptOrCssElement> m_cssList;							//Css列表
		private string m_documentType;										//页面文档类型
		private BaseEntity m_dataEntity;									//数据实例

		private string m_pageTitle;
		private string m_pageKeyWords;
		private string m_pageDescription;

		public PageTemplate()
		{
			m_paraDic = new Dictionary<string, TemplateParameter>();
			m_htmlSegmentList = new List<HtmlSegment>();
			m_moduleSegmentDic = new Dictionary<string, HtmlSegment>();
			m_moduleList = new List<ModuleBase>();
			m_scriptList = new List<ScriptOrCssElement>();
			m_cssList = new List<ScriptOrCssElement>();
		}

		/// <summary>
		/// 页面标题
		/// </summary>
		public string PageTitle
		{
			get { return m_pageTitle; }
			set { m_pageTitle = value; }
		}

		/// <summary>
		/// 页面关键字
		/// </summary>
		public string PageKeywords
		{
			get { return m_pageKeyWords; }
			set { m_pageKeyWords = value; }
		}

		/// <summary>
		/// 页面说明
		/// </summary>
		public string PageDescription
		{
			get { return m_pageDescription; }
			set { m_pageDescription = value; }
		}

		/// <summary>
		/// 参数字典
		/// </summary>
		public Dictionary<string,TemplateParameter> ParameterDictionary
		{
			get { return m_paraDic; }
		}

		/// <summary>
		/// 页面文档类型
		/// </summary>
		public string DocumentType
		{
			get { return m_documentType; }
			set { m_documentType = value; }
		}

		/// <summary>
		/// 模板拆分后的片段列表
		/// </summary>
		public List<HtmlSegment> HtmlSegmentList
		{
			get { return m_htmlSegmentList; }
		}

		/// <summary>
		/// 模板拆分后的模块字典
		/// </summary>
		public Dictionary<string,HtmlSegment> ModuleSegmentDictionary
		{
			get { return m_moduleSegmentDic; }
		}

		/// <summary>
		/// 模块列表
		/// </summary>
		public List<ModuleBase> ModuleList
		{
			get { return m_moduleList; }
		}

		/// <summary>
		/// 数据实例
		/// </summary>
		public BaseEntity DataEntity
		{
			get { return m_dataEntity; }
			set { m_dataEntity = value; }
		}

		/// <summary>
		/// 调用所有模块的生成功能
		/// </summary>
		protected void RenderModules()
		{
			foreach(ModuleBase m in m_moduleList)
			{
				if (m_moduleSegmentDic.ContainsKey(m.Name))
					m_moduleSegmentDic[m.Name].Text = m.RenderModule();
			}
		}

		/// <summary>
		/// 生成Html的头内容
		/// </summary>
		/// <returns></returns>
		protected string RenderHtmlHead()
		{
			StringBuilder headCode = new StringBuilder(1000);

			headCode.AppendLine("<title>" + m_pageTitle + "</title>");
			headCode.AppendLine("<meta name=\"Keywords\" content=\"" + m_pageKeyWords + "\" />");
			headCode.AppendLine("<meta name=\"Description\" content=\"" + m_pageDescription + "\" />");

			

			foreach (HtmlSegment seg in m_htmlSegmentList)
			{
				if(seg.Position == SegmentPosition.Head)
					headCode.AppendLine(seg.Text);
			}

			//页顶CSS
			string topCss = TemplateCommon.GetCssCode(m_cssList, ElementPosition.PageTop);
			if (topCss.Length > 0)
				headCode.AppendLine(topCss);
			//页顶脚本
			string topScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.PageTop);
			if (topScript.Length > 0)
				headCode.AppendLine(topScript);

			return headCode.ToString();
		}

		private void RenderCssAndScriptInHeader( HttpResponse res )
		{
			//页顶CSS
			string topCss = TemplateCommon.GetCssCode(m_cssList, ElementPosition.PageTop);
			if (topCss.Length > 0)
				res.Write(topCss);
			//页顶脚本
			string topScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.PageTop);
			if (topScript.Length > 0)
				res.Write(topScript);
		}

		/// <summary>
		/// 生成模板代码
		/// </summary>
		/// <returns></returns>
		public void RenderTemplate(HttpResponse res)
		{
			//生成所有的模块的代码
			RenderModules();
			//页面代码
			foreach(HtmlSegment seg in m_htmlSegmentList)
			{
				//header中的CSS或脚本
				if (seg.Name == "HeaderScriptOrCss")
					RenderCssAndScriptInHeader(res);
				else
					res.Write(seg.Text);
			}
			//页底脚本
			string endScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.PageEnd);
			if (endScript.Length > 0)
				res.Write(endScript);
		}

		/// <summary>
		/// 注册一个Css段
		/// </summary>
		/// <param name="css"></param>
		public void RegisterCss(ScriptOrCssElement css)
		{
			m_cssList.Add(css);
		}

		/// <summary>
		/// 注册一个脚本段
		/// </summary>
		/// <param name="script"></param>
		public void RegisterScript(ScriptOrCssElement script)
		{
			m_scriptList.Add(script);
		}
	}
}
