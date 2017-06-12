using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using System.Xml;
using BitAuto.CarChannel.BLL.Data;
using BitAuto.CarChannel.BLL.Template.TemplateException;
using BitAuto.CarChannel.BLL.Template.ModuleFunction;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 程序生成类的模块
	/// </summary>
	public class ProgramModule:ModuleBase
	{
		protected string m_className;
		protected string m_methodName;

		/// <summary>
		/// 类名
		/// </summary>
		public string ClassName
		{
			get { return m_className; }
			set { m_className = value; }
		}

		/// <summary>
		/// 方法名
		/// </summary>
		public string MethodName
		{
			get { return m_methodName; }
			set { m_methodName = value; }
		}

		public ProgramModule(string name):this(null,name)
		{

		}

		public ProgramModule(PageTemplate ownerTemplate, string name)
			: base(ownerTemplate, name)
		{
			m_className = String.Empty;
			m_methodName = String.Empty;
		}

		public override ModuleBase Clone(PageTemplate ownTemplate)
		{
			ProgramModule m = new ProgramModule(ownTemplate, this.Name);
			m.ClassName = m_className;
			m.MethodName = m_methodName;
			base.CloneBaseData(m);
			return m;
		}

		public override void InitModule(System.Xml.XmlElement mNode)
		{
			XmlNode cnNode = mNode.SelectSingleNode("MainClassName");
			XmlNode mnNode = mNode.SelectSingleNode("MethodName");
			if(cnNode == null || mnNode==null)
				throw (new FileFormatException("Template file format error!"));
			m_className = cnNode.InnerText;
			m_methodName = mnNode.InnerText;
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
			XmlNode classNode = doc.SelectSingleNode("/root/ClassName");
			XmlNode methodNode = doc.SelectSingleNode("/root/MethodName");
			if (classNode != null)
				m_className = classNode.InnerText;
			if (methodNode != null)
				m_methodName = methodNode.InnerText;
			//脚本或CSS
			base.InitCssOrScript(doc.DocumentElement);
		}

		/// <summary>
		/// 生成模块的Html代码
		/// </summary>
		/// <returns></returns>
		public override string RenderModule()
		{
			base.RegisterCssOrScript();

			StringBuilder htmlCode = new StringBuilder(2048);


			//模块开始处理的CSS与Script
			string topCss = TemplateCommon.GetCssCode(m_cssList, ElementPosition.ModuleTop);
			if (topCss.Length > 0)
				htmlCode.AppendLine(topCss);
			string topScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleTop);
			if (topScript.Length > 0)
				htmlCode.AppendLine(topScript);

			//利用反射技术调用各类的方法
			Type funType = null;
			switch(m_className)
			{
				case "SerialFunction":
					funType = typeof(SerialFunction);
					break;
				default:
					funType = Type.GetType("BitAuto.CarChannel.BLL.Template.ModuleFunction." + m_className);
					break;
			}
			
			MethodInfo mInfo = funType.GetMethod(m_methodName);
			string htmlStr = String.Empty;
			if(mInfo != null)
			{
				htmlStr = (string)mInfo.Invoke(null, new object[] { m_ownerTemplate.DataEntity,m_ownerTemplate.ParameterDictionary });
			}
			htmlCode.AppendLine(htmlStr);

			//模块结束处的脚本
			string endScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleEnd);
			if (endScript.Length > 0)
				htmlCode.AppendLine(endScript);

			return htmlCode.ToString();
		}

	}
}
