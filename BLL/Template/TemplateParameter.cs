using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 页面参数
	/// </summary>
	public class TemplateParameter
	{
		private string m_parameterName;
		private string m_parameterValue;
		private bool m_isMust;

		/// <summary>
		/// 参数名称
		/// </summary>
		public string Name
		{
			get { return m_parameterName; }
			set { m_parameterName = value; }
		}

		/// <summary>
		/// 参数值
		/// </summary>
		public string ParameterValue
		{
			get { return m_parameterValue; }
			set { m_parameterValue = value; }
		}

		/// <summary>
		/// 必须有值
		/// </summary>
		public bool MustHasValue
		{
			get { return m_isMust; }
			set { m_isMust = value; }
		}

		public TemplateParameter(){}

		/// <summary>
		/// 根据名称与参数值初始化参数信息
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pValue"></param>
		public TemplateParameter(string name,string pValue)
		{
			m_parameterName = name;
			m_parameterValue = pValue;
		}

		/// <summary>
		/// 根据名称与是否必须初始化参数信息
		/// </summary>
		/// <param name="name"></param>
		/// <param name="isMust"></param>
		public TemplateParameter(string name,bool isMust)
		{
			m_parameterName = name;
			m_isMust = isMust;
		}

		public TemplateParameter Clone()
		{
			TemplateParameter para = new TemplateParameter(m_parameterName, m_parameterValue);
			para.MustHasValue = m_isMust;
			return para;
		}

		public override string ToString()
		{
			return "TemplateParameter:" + m_parameterName + "=" + m_parameterValue;
		}
	}
}
