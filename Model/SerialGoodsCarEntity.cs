using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SerialGoodsCarEntity
	{
		public int GoodsId { get; set; }
		public int CarId { get; set; }
		public decimal MarketPrice { get; set; }
		public decimal BitautoPrice { get; set; }
		public string GoodsUrl { get; set; }
	}
}
