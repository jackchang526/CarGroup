using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BitAuto.CarChannel.BLL.Template.TemplateException;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 脚本类型模块
	/// </summary>
	public class ScriptModule:ModuleBase
	{
		private string m_baseHtml;

		public ScriptModule(string name):this(null,name)
		{

		}

		public ScriptModule(PageTemplate ownerTemplate, string name)
			: base(ownerTemplate, name)
		{
			m_baseHtml = String.Empty;
		}

		/// <summary>
		/// 基础HTML
		/// </summary>
		public string BaseHtml
		{
			get { return m_baseHtml; }
			set { m_baseHtml = value; }
		}

		/// <summary>
		/// 实现克隆，但本类无须克隆，只需返回自己即可
		/// </summary>
		/// <returns></returns>
		public override ModuleBase Clone(PageTemplate ownTemplate)
		{
			ScriptModule m = new ScriptModule(ownTemplate, this.Name);
			m.BaseHtml = m_baseHtml;
			base.CloneBaseData(m);
			return m;
		}

		/// <summary>
		/// 初始化模块
		/// </summary>
		/// <param name="mNode"></param>
		public override void InitModule(XmlElement mNode)
		{
			//脚本块的框架代码
			XmlNode htmlNode = mNode.SelectSingleNode("BaseHtml");
			if (htmlNode != null)
				m_baseHtml = htmlNode.InnerText;

			//脚本或CSS
			base.InitCssOrScript(mNode);
		}

		/// <summary>
		/// 根据内容XML初始化
		/// </summary>
		/// <param name="contentXml"></param>
		public override void InitModule( string contentXml )
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(contentXml);
			XmlNode contentNode = doc.SelectSingleNode("/root/Content");
			if (contentNode != null)
				m_baseHtml = contentNode.InnerText;
			//脚本或CSS
			base.InitCssOrScript(doc.DocumentElement);
		}

		/// <summary>
		/// 生成模块HTML
		/// </summary>
		/// <returns></returns>
		public override string RenderModule()
		{
			base.RegisterCssOrScript();
			StringBuilder htmlCode = new StringBuilder();
			//模块前CSS
			string topCss = TemplateCommon.GetCssCode(m_cssList,ElementPosition.ModuleTop);
			if (topCss.Length > 0)
				htmlCode.AppendLine(topCss);
			//模块前脚本
			string topScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleTop);
			if (topScript.Length > 0)
				htmlCode.AppendLine(topScript);
			//模板代码
			htmlCode.AppendLine(m_baseHtml);
			//模块后脚本
			string endScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleEnd);
			if (endScript.Length > 0)
				htmlCode.AppendLine(endScript);
			return htmlCode.ToString();
		}
	}
}
