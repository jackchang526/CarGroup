using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	/// <summary>
	/// 子品牌焦点图的数据
	/// </summary>
	public class SerialFocusImage
	{
		private int m_imgId;
		private string m_imgName;
		private string m_imgUrl;
		private string m_targetUrl;

		public string GroupName { get; set; }

		/// <summary>
		/// 图片ID
		/// </summary>
		public int ImageId
		{
			get { return m_imgId; }
			set { m_imgId = value; }
		}

		/// <summary>
		/// 图片名称
		/// </summary>
		public string ImageName
		{
			get { return m_imgName; }
			set { m_imgName = value; }
		}


		/// <summary>
		/// 图片URL
		/// </summary>
		public string ImageUrl
		{
			get { return m_imgUrl; }
			set { m_imgUrl = value; }
		}


		/// <summary>
		/// 图片指向链接
		/// </summary>
		public string TargetUrl
		{
			get { return m_targetUrl; }
			set { m_targetUrl = value; }
		}

	}
}
