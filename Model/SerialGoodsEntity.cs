using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SerialGoodsEntity
	{
		public int GoodsId { get; set; }
		public int SerialId { get; set; }
		public string GoodsUrl { get; set; }
		public string CoverImageUrl { get; set; }
		public string PromotTitle { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public decimal MinMarketPrice { get; set; }
		public decimal MinBitautoPrice { get; set; }
	}
}
