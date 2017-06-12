using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;
using BitAuto.CarChannel.BLL.Template.TemplateException;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// Include类型的模块，需要读取文件，并能分析出文件中的嵌套包含
	/// </summary>
	public class IncludeModule:ModuleBase
	{
		private string m_includeFileName;

		/// <summary>
		/// 包含的文件名称
		/// </summary>
		public string IncludeFileName
		{
			get { return m_includeFileName; }
			set { m_includeFileName = value; }
		}

		public IncludeModule(string name):base(name)
		{

		}

		public IncludeModule(PageTemplate ownerTemplate,string name):base(ownerTemplate,name)
		{

		}

		public IncludeModule(string name,string fileName):base(name)
		{
			m_includeFileName = fileName;
		}



		/// <summary>
		/// 实现克隆，但本类无须克隆，只需返回自己即可
		/// </summary>
		/// <returns></returns>
		public override ModuleBase Clone(PageTemplate ownTemplate)
		{
			IncludeModule iModule = new IncludeModule(ownTemplate,m_moduleName);
			iModule.IncludeFileName = m_includeFileName;
			base.CloneBaseData(iModule);
			return iModule;
		}

		/// <summary>
		/// 实现生成Html，本类只需读取文件
		/// </summary>
		/// <returns></returns>
		public override string RenderModule()
		{
			base.RegisterCssOrScript();
			StringBuilder htmlCode = new StringBuilder();
			//模块前CSS
			string topCss = TemplateCommon.GetCssCode(m_cssList, ElementPosition.ModuleTop);
			if (topCss.Length > 0)
				htmlCode.AppendLine(topCss);
			//模块前脚本
			string topScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleTop);
			if (topScript.Length > 0)
				htmlCode.AppendLine(topScript);

			htmlCode.Append(ReadIncludeFile(m_includeFileName));
			//模块后脚本
			string endScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleEnd);
			if (endScript.Length > 0)
				htmlCode.AppendLine(endScript);
			return htmlCode.ToString();
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="mNode"></param>
		public override void InitModule(System.Xml.XmlElement mNode)
		{
			XmlNode fileNode = mNode.SelectSingleNode("FilePath");
			if (fileNode == null)
				throw (new FileFormatException("Template file format error!"));
			m_includeFileName = fileNode.InnerText;
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
				m_includeFileName = contentNode.InnerText;
			//脚本或CSS
			base.InitCssOrScript(doc.DocumentElement);
		}

		/// <summary>
		/// 分析包含文件的语法，得出包含文件的路径
		/// </summary>
		/// <param name="includeHtml"></param>
		/// <returns></returns>
		private string GetIncludeFile(string includeHtml)
		{
			//范例<!--#include file="~/html/bt_search.shtml"-->
			if (String.IsNullOrEmpty(includeHtml))
				return String.Empty;

			string fileFlag = "file=";
			int pos = includeHtml.ToLower().IndexOf(fileFlag);
			string fileName = "";
			if(pos > -1)
			{
				string afterFileString = includeHtml.Substring(pos + fileFlag.Length);
				pos = afterFileString.IndexOf("\"", 1);
				if(pos > -1)
				{
					fileName = afterFileString.Substring(0, pos);
					fileName = fileName.Trim(new char[] { '\"' });
				}
			}
			return fileName;
		}

		/// <summary>
		/// 读取包含文件，如果有嵌套文件则递归调用此方法
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		private string ReadIncludeFile(string fileName)
		{
			if (String.IsNullOrEmpty(fileName))
				return String.Empty;

			if (HttpContext.Current == null)
			{
				throw new NoHttpContextException();
			}
			string realFileName = HttpContext.Current.Server.MapPath(fileName);

			if (!File.Exists(realFileName))
				return String.Empty;

			StringBuilder htmlCode = new StringBuilder();
			string fileText = File.ReadAllText(realFileName);

			string startFlag = "<!--#include";
			string endFlag = "-->";
			int includePos = fileText.IndexOf(startFlag);

			while(includePos > -1)
			{
				string preText = fileText.Substring(0, includePos);
				htmlCode.Append(preText);
				int endPos = fileText.IndexOf(endFlag,includePos);
				if(endPos > -1)
				{
					//获取嵌套的包含文件
					string includeHtml = fileText.Substring(includePos, endPos + endFlag.Length - includePos);
					fileText = fileText.Substring(endPos + endFlag.Length);
					//递归调用本方法读取下一文件
					string inFileName = GetIncludeFile(includeHtml);
					string inFileStr = ReadIncludeFile(inFileName);
					htmlCode.Append(inFileStr);
				}
				else
				{
					throw new FileFormatException("Not found end flag at: " + includePos + ",in file:" + fileName,fileName);
				}
				includePos = fileText.IndexOf(startFlag);
			}
			//加上最后的一段文本
			htmlCode.Append(fileText);

			return htmlCode.ToString();
		}
	}
}
