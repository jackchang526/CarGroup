using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 模板中的HTML代码片段，包括模块类的内容
	/// </summary>
	public class HtmlSegment
	{
		private HtmlSegmentType m_segmentType;
		private SegmentPosition m_segmentPosition;
		private string m_segmentText;
		private string m_segmentName;

		/// <summary>
		/// 默认构造
		/// </summary>
		public HtmlSegment()
		{
			m_segmentName = String.Empty;
			m_segmentText = String.Empty;
			m_segmentType = HtmlSegmentType.Normal;
			m_segmentPosition = SegmentPosition.Body;
		}

		public HtmlSegment( string segHtml ):this()
		{
			m_segmentText = segHtml;
		}

		/// <summary>
		/// 根据类型与名称构造
		/// </summary>
		/// <param name="segTyep"></param>
		/// <param name="name"></param>
		public HtmlSegment(HtmlSegmentType segTyep,string name):this()
		{
			m_segmentType = segTyep;
			m_segmentName = name;
		}

		/// <summary>
		/// 片段名称,主要应用于模块
		/// </summary>
		public string Name
		{
			get { return m_segmentName; }
			set { m_segmentName = value; }
		}

		/// <summary>
		/// 代码段文本
		/// </summary>
		public string Text
		{
			get { return m_segmentText; }
			set { m_segmentText = value; }
		}

		/// <summary>
		/// 代码段所处位置
		/// </summary>
		public SegmentPosition Position
		{
			get { return m_segmentPosition; }
			set { m_segmentPosition = value; }
		}

		/// <summary>
		/// 代码段类型
		/// </summary>
		public HtmlSegmentType SegmentType
		{
			get { return m_segmentType; }
			set { m_segmentType = value; }
		}

		/// <summary>
		/// 克隆自己
		/// </summary>
		/// <returns></returns>
		public HtmlSegment Clone()
		{
			HtmlSegment seg = new HtmlSegment(m_segmentType, m_segmentName);
			seg.Text = m_segmentText;
			return seg;
		}
	}
}
