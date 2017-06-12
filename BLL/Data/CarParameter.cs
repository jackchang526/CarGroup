using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Data
{
	/// <summary>
	/// 车型参数
	/// </summary>
	public class CarParameter
	{
		private int m_paraId;
		private string m_paraName;
		private string m_paraValue;
		private string m_paraEnName;

		/// <summary>
		/// 参数ID
		/// </summary>
		public int Id
		{
			get { return m_paraId; }
		}

		/// <summary>
		/// 参数名称(中文名称)
		/// </summary>
		public string Name
		{
			get { return m_paraName; }
		}

		/// <summary>
		/// 参数英文名
		/// </summary>
		public string EnName
		{
			get { return m_paraEnName; }
		}

		/// <summary>
		/// 参数值
		/// </summary>
		public string ParameterValue
		{
			get { return m_paraValue; }
		}
	}
}
