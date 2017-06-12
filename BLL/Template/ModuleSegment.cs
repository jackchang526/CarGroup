using System;
using System.Collections.Generic;
using System.Text;

using BitAuto.CarChannel.BLL.Data;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 模板式模块中的代码段
	/// </summary>
	public class ModuleSegment
	{
		private bool m_isDataSegment;
		private EntityType m_dataType;
		private string m_dataName;
		private string m_text;

		/// <summary>
		/// 是否为数据域
		/// </summary>
		public bool IsDataSegment
		{
			get { return m_isDataSegment; }
			set { m_isDataSegment = value; }
		}

		/// <summary>
		/// 数据类型
		/// </summary>
		public EntityType DataEntityType
		{
			get { return m_dataType; }
			set { m_dataType = value; }
		}

		/// <summary>
		/// 数据域名称
		/// </summary>
		public string DataName
		{
			get { return m_dataName; }
			set { m_dataName = value; }
		}

		/// <summary>
		/// 文本
		/// </summary>
		public string Text
		{
			get { return m_text; }
			set { m_text = value; }
		}

		public ModuleSegment()
		{}

		public ModuleSegment(string text)
		{
			m_isDataSegment = false;
			m_text = text;
		}

	}
}
