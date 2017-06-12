using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using BitAuto.CarChannel.BLL.Template.TemplateException;

namespace BitAuto.CarChannel.BLL.Template
{
	public abstract class ModuleBase
	{
		protected string m_moduleName;
		protected PageTemplate m_ownerTemplate;
		protected List<ScriptOrCssElement> m_scriptList;
		protected List<ScriptOrCssElement> m_cssList;

		/// <summary>
		/// 模块名称
		/// </summary>
		public string Name
		{
			get { return m_moduleName; }
			set { m_moduleName = value; }
		}

		/// <summary>
		/// 所属模板
		/// </summary>
		public PageTemplate OwnerTemplate
		{
			get { return m_ownerTemplate; }
		}

		/// <summary>
		/// 脚本列表
		/// </summary>
		public List<ScriptOrCssElement> ScriptList
		{
			get { return m_scriptList; }
		}

		/// <summary>
		/// Css列表
		/// </summary>
		public List<ScriptOrCssElement> CssList
		{
			get { return m_cssList; }
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="ownerTemplate"></param>
		/// <param name="name"></param>
		protected ModuleBase(PageTemplate ownerTemplate,string name)
		{
			m_moduleName = name;
			m_ownerTemplate = ownerTemplate;
			m_scriptList = new List<ScriptOrCssElement>();
			m_cssList = new List<ScriptOrCssElement>();
		}

		protected ModuleBase(string name)
		{
			m_moduleName = name;
			m_ownerTemplate = null;
			m_scriptList = new List<ScriptOrCssElement>();
			m_cssList = new List<ScriptOrCssElement>();
		}

		/// <summary>
		/// 注册脚本或CSS
		/// </summary>
		protected void RegisterCssOrScript()
		{
			//注册CSS
			foreach(ScriptOrCssElement css in m_cssList)
			{
				if (css.ElementPosition == ElementPosition.PageTop)
					this.OwnerTemplate.RegisterCss(css);
			}

			//注册脚本
			foreach(ScriptOrCssElement script in m_scriptList)
			{
				if (script.ElementPosition == ElementPosition.PageTop || script.ElementPosition == ElementPosition.PageEnd)
					this.OwnerTemplate.RegisterScript(script);
			}
		}

		/// <summary>
		/// 初始化脚本或CSS
		/// </summary>
		protected void InitCssOrScript(XmlElement modNode)
		{
			XmlNodeList cssNodeList = modNode.SelectNodes("Css");
			foreach(XmlElement cssNode in cssNodeList)
			{
				ScriptOrCssElement css = new ScriptOrCssElement();
				string posType = cssNode.GetAttribute("cssPos");
				string eleType = cssNode.GetAttribute("cssType");
				css.ElementPosition = (ElementPosition)Enum.Parse(typeof(ElementPosition), posType);
				css.ElementType = (ScriptOrCssElementType)Enum.Parse(typeof(ScriptOrCssElementType), eleType);
				css.ElementText = cssNode.InnerText;
				m_cssList.Add(css);
			}

			XmlNodeList scriptNodeList = modNode.SelectNodes("Js");
			foreach (XmlElement scriptNode in scriptNodeList)
			{
				ScriptOrCssElement script = new ScriptOrCssElement();
				string posType = scriptNode.GetAttribute("jsPos");
				string eleType = scriptNode.GetAttribute("jsType");
				script.ElementPosition = (ElementPosition)Enum.Parse(typeof(ElementPosition), posType);
				script.ElementType = (ScriptOrCssElementType)Enum.Parse(typeof(ScriptOrCssElementType), eleType);
				script.ElementText = scriptNode.InnerText;
				m_scriptList.Add(script);
			}
		}



		/// <summary>
		/// 克隆一个对象出来
		/// </summary>
		/// <returns></returns>
		public abstract ModuleBase Clone(PageTemplate ownTemplate);

		/// <summary>
		/// 将父类的数据复制给新对象
		/// </summary>
		/// <param name="m"></param>
		protected void CloneBaseData(ModuleBase m)
		{
			m.Name = m_moduleName;
			foreach (ScriptOrCssElement css in m_cssList)
				m.CssList.Add(css);
			foreach (ScriptOrCssElement script in m_scriptList)
				m.ScriptList.Add(script);
		}

		/// <summary>
		/// 生成模块代码
		/// </summary>
		/// <returns></returns>
		public abstract string RenderModule();

		/// <summary>
		/// 根据模板配置初始化模块
		/// </summary>
		/// <param name="mNode"></param>
		public abstract void InitModule(XmlElement mNode);

		/// <summary>
		/// 根据内容XML初始化模块
		/// </summary>
		/// <param name="contentXml"></param>
		public abstract void InitModule( string contentXml );
	}
}
