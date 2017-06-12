using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class JiangJiaNews
	{
		private int _NewsId;
		private string _Title;
		private string _NewsUrl;
		private DateTime _PublishTime;
		private decimal _MaxFavorablePrice;
		private int _VendorId;
		private string _VendorName;

		/// <summary>
		/// 新闻ID
		/// </summary>
		public int NewsId
		{
			get { return _NewsId; }
			set { _NewsId = value; }
		}

		/// <summary>
		/// 新闻标题
		/// </summary>
		public string Title
		{
			get { return _Title; }
			set { _Title = value; }
		}

		/// <summary>
		/// 页面链接
		/// </summary>
		public string NewsUrl
		{
			get { return _NewsUrl; }
			set { _NewsUrl = value; }
		}

		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime PublishTime
		{
			get { return _PublishTime; }
			set { _PublishTime = value; }
		}

		/// <summary>
		/// 最大降幅
		/// </summary>
		public decimal MaxFavorablePrice
		{
			get { return _MaxFavorablePrice; }
			set { _MaxFavorablePrice = value; }
		}

		/// <summary>
		/// 经销商ID
		/// </summary>
		public int VendorId
		{
			get { return _VendorId; }
			set { _VendorId = value; }
		}

		/// <summary>
		/// 经销商名
		/// </summary>
		public string VendorName
		{
			get { return _VendorName; }
			set { _VendorName = value; }
		}

	}
}
