using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SerialListADEntity
	{
		public SerialListADEntity()
		{ 
			
		}
		/// <summary>
		/// 广告位置
		/// </summary>
		public int Pos { get; set; }
		/// <summary>
		/// 子品牌ID
		/// </summary>
		public int SerialId { get; set; }
		/// <summary>
		/// 广告链接地址
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 子品牌图片地址
		/// </summary>
		public string ImageUrl { get; set; }
		/// <summary>
		/// 广告开始时间
		/// </summary>
		public DateTime StartTime { get; set; }
		/// <summary>
		/// 广告结束时间
		/// </summary>
		public DateTime EndTime { get; set; }
	}
}
