using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 模板式模块
	/// </summary>
	public class TemplateModule:ModuleBase
	{
		protected List<ModuleSegment> m_segmentList;

		/// <summary>
		/// HTML与数据域的列表
		/// </summary>
		public List<ModuleSegment> SegmentList
		{
			get { return m_segmentList; }
		}

		public TemplateModule(string name) : this(null,name)
		{
		}

		public TemplateModule(PageTemplate ownerTemplate, string name):base(ownerTemplate,name)
		{
			m_segmentList = new List<ModuleSegment>();
		}

		/// <summary>
		/// 初始化模块
		/// </summary>
		/// <param name="mNode"></param>
		public override void InitModule(XmlElement mNode)
		{
			XmlElement tmpNode = (XmlElement)mNode.SelectSingleNode("TemplateCode");
			if(tmpNode != null)
			{
				AnalyseTemplateCode(tmpNode.InnerText);
			}

			base.InitCssOrScript(mNode);
		}

		protected void AnalyseTemplateCode( string tmpCode )
		{
			//分析模板中的文本与数据域
			string dataFlag = "#d{";			//数据域起始标志
			string dataEndFlag = "}";			//数据域结束标志
			int flagPos = tmpCode.IndexOf(dataFlag);

			while (flagPos > -1)
			{
				string tmpHtml = tmpCode.Substring(0, flagPos);
				//一个文本段
				if (tmpHtml.Length > 0)
				{
					m_segmentList.Add(new ModuleSegment(tmpHtml));
				}

				//数据段
				int endPos = tmpCode.IndexOf(dataEndFlag, flagPos);
				string dataCode = tmpCode.Substring(flagPos + dataFlag.Length, endPos - flagPos - dataFlag.Length);
				string[] dataDefine = dataCode.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				if (dataDefine.Length >= 2)
				{
					ModuleSegment ms = new ModuleSegment();
					ms.DataEntityType = (EntityType)Enum.Parse(typeof(EntityType), dataDefine[0]);
					ms.DataName = dataDefine[1];
					ms.IsDataSegment = true;
					m_segmentList.Add(ms);
				}
				tmpCode = tmpCode.Substring(endPos + 1);
				flagPos = tmpCode.IndexOf(dataFlag);
			}

			//最后的文本也加入列表中
			m_segmentList.Add(new ModuleSegment(tmpCode));
		}

		public override void InitModule( string contentXml )
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(contentXml);
			XmlNode contentNode = doc.SelectSingleNode("/root/Content");
			if (contentNode != null)
				AnalyseTemplateCode(contentNode.InnerText);

			//脚本或CSS
			base.InitCssOrScript(doc.DocumentElement);
		}

		/// <summary>
		/// 克隆模块
		/// </summary>
		/// <param name="ownTemplate"></param>
		/// <returns></returns>
		public override ModuleBase Clone(PageTemplate ownTemplate)
		{
			TemplateModule m = new TemplateModule(ownTemplate,this.Name);
			base.CloneBaseData(m);
			foreach (ModuleSegment seg in m_segmentList)
				m.SegmentList.Add(seg);
			return m;
		}

		/// <summary>
		/// 生成模块内容Html
		/// </summary>
		/// <returns></returns>
		public override string RenderModule()
		{
			base.RegisterCssOrScript();

			StringBuilder htmlCode = new StringBuilder();

			//模块开始处理的CSS与Script
			string topCss = TemplateCommon.GetCssCode(m_cssList, ElementPosition.ModuleTop);
			if(topCss.Length > 0)
				htmlCode.AppendLine(topCss);
			string topScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleTop);
			if (topScript.Length > 0)
				htmlCode.AppendLine(topScript);

			//模块Html代码
			foreach(ModuleSegment ms in m_segmentList)
			{
				if(ms.IsDataSegment)
				{
					ms.Text = this.m_ownerTemplate.DataEntity.GetDataValue(ms.DataEntityType,ms.DataName);
				}
				htmlCode.Append(ms.Text);
			}

			//模块结束处的脚本
			string endScript = TemplateCommon.GetScriptCode(m_scriptList, ElementPosition.ModuleEnd);
			if (endScript.Length > 0)
				htmlCode.AppendLine(endScript);

			return htmlCode.ToString();
		}
	}
}
