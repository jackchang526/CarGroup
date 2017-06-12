using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Template
{
	public class ScriptOrCssElement
	{
		private ScriptOrCssElementType m_elementType;
		private ElementPosition m_elementPosition;
		private string m_Text;

		/// <summary>
		/// 节点类型，是引用还是文本块
		/// </summary>
		public ScriptOrCssElementType ElementType
		{
			get { return m_elementType; }
			set { m_elementType = value; }
		}

		/// <summary>
		/// 节点位置
		/// </summary>
		public ElementPosition ElementPosition
		{
			get { return m_elementPosition; }
			set { m_elementPosition = value; }
		}

		/// <summary>
		/// 节点的文本
		/// </summary>
		public string ElementText
		{
			get { return m_Text; }
			set { m_Text = value; }
		}
	}
}
