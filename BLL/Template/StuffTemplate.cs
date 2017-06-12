using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.DAL;
using BitAuto.CarChannel.BLL.Template.TemplateException;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 模板的分析结果，一个主要的功能就是缓存分析结果
	/// </summary>
	internal class StuffTemplate
	{
		private List<TemplateParameter> m_paraLsit;
		private List<HtmlSegment> m_htmlSegmentList;
		private List<ModuleBase> m_moduleList;
		private string m_documentType;
		
		/// <summary>
		/// 初始化
		/// </summary>
		internal StuffTemplate()
		{
			//m_documentType = "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
			m_paraLsit = new List<TemplateParameter>();
			m_htmlSegmentList = new List<HtmlSegment>();
			m_moduleList = new List<ModuleBase>();
		}

		/// <summary>
		/// 创建一个模板实例
		/// </summary>
		/// <returns></returns>
		internal PageTemplate CreateTemplate()
		{
			PageTemplate pageTemplate = new PageTemplate();

			//Html片段加入
			foreach(HtmlSegment seg in m_htmlSegmentList)
			{
				HtmlSegment tmpSeg = seg.Clone();
				pageTemplate.HtmlSegmentList.Add(tmpSeg);
				if (tmpSeg.SegmentType == HtmlSegmentType.Module)
					pageTemplate.ModuleSegmentDictionary[tmpSeg.Name] = tmpSeg;
			}

			//加入模块列表
			foreach(ModuleBase module in m_moduleList)
			{
				ModuleBase m = module.Clone(pageTemplate);
				pageTemplate.ModuleList.Add(m);
			}
			return pageTemplate;
		}

		/// <summary>
		/// 根据模板文件初始化模板
		/// </summary>
		/// <param name="fileName"></param>
		[Obsolete( "模板已经不再从文件初始化，也不再使用XML格式，此方法已经不适用了。" )]
		internal void InitializeStuffTemplate(string fileName)
		{
			string tmpFile = Path.Combine(WebConfig.DataBlockPath, "Data\\Template\\" + fileName);
			if (!File.Exists(tmpFile))
				throw (new FileNotFoundException("Template file not found", tmpFile));

			XmlDocument tmpDoc = new XmlDocument();
			tmpDoc.Load(tmpFile);

			//模板参数
			XmlNodeList paraNodeList = tmpDoc.SelectNodes("/Template/Parameters/Parameter");
			foreach(XmlElement paraNode in paraNodeList)
			{
				TemplateParameter para = new TemplateParameter();
				para.Name = paraNode.GetAttribute("name");
				para.MustHasValue = ConvertHelper.GetBoolean(paraNode.GetAttribute("isMust"));
				this.m_paraLsit.Add(para);
			}

			//文档类型
			XmlNode docTypeNode = tmpDoc.SelectSingleNode("/Template/DocumentType");
			if (docTypeNode != null)
				m_documentType = docTypeNode.InnerText;

			//代码块列表
			XmlNodeList segNodeList = tmpDoc.SelectNodes("/Template/Segments/HtmlSegment");
			foreach (XmlElement segNode in segNodeList)
			{
				HtmlSegment seg = new HtmlSegment();
				string moduleType = segNode.GetAttribute("moduleType");
				if (moduleType.Length == 0)
				{
					//普通Html块
					seg.SegmentType = HtmlSegmentType.Normal;
					seg.Text = segNode.InnerText;
				}
				else
				{
					//模块
					//ModuleBase module = GetModuleFromData(segNode);
					//m_moduleList.Add(module);
					//seg.Name = module.Name;
					//seg.SegmentType = HtmlSegmentType.Module;
				}
				m_htmlSegmentList.Add(seg);
			}
		}

		/// <summary>
		/// 根据模板ID与是否预览来初始化一个模板的根
		/// </summary>
		/// <param name="templateId"></param>
		/// <param name="isPreview"></param>
		internal void InitializeStuffTemplate( int templateId ,bool isPreview)
		{
			string tempContent = TemplateDal.GetTemplateContent(templateId, isPreview);

			string modFlag = "#mod!{";		//模块标志
			string modEndFlag = "}";		//模块结束标志

			//第一个标志
			int flagPos = tempContent.IndexOf(modFlag);

			while (flagPos > -1)
			{
				//一个文本段
				string tmpHtml = tempContent.Substring(0, flagPos);
				if (tmpHtml.Length > 0)
				{
					m_htmlSegmentList.Add(new HtmlSegment(tmpHtml));
				}

				//分析模块
				int endPos = tempContent.IndexOf(modEndFlag, flagPos);
				string dataCode = tempContent.Substring(flagPos + modFlag.Length, endPos - flagPos - modFlag.Length);
				string[] dataDefine = dataCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (dataDefine.Length >= 2)
				{
					int modId = ConvertHelper.GetInteger(dataDefine[0]);
					string modName = dataDefine[1];
					if (modName == "HeaderScriptOrCss")
					{
						//此名称的模块为一个约定，这是页面header里放CSS或者脚本的地方
						HtmlSegment hs = new HtmlSegment();
						hs.Name = modName;
						m_htmlSegmentList.Add(hs);
					}
					else
					{
						ModuleBase module = TemplateManager.GetModuleById(modId, isPreview);
						m_moduleList.Add(module);
						m_htmlSegmentList.Add(new HtmlSegment(HtmlSegmentType.Module, module.Name));
					}
				}
				tempContent = tempContent.Substring(endPos + 1);
				flagPos = tempContent.IndexOf(modFlag);
			}

			//最后的文本也加入列表中
			m_htmlSegmentList.Add(new HtmlSegment(tempContent));
		}		
	}
}
