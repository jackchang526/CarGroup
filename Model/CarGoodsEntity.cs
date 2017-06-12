using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class CarGoodsEntity
	{
		public int SerialId { get; set; }
		public int CarId { get; set; }
		public int CityId { get; set; }
		public string GoodsUrl { get; set; }
		public decimal MinMarketPrice { get; set; }
		public decimal MinBitautoPrice { get; set; }
	}

	/// <summary>
	/// 购车返现
	/// </summary>
	public class CarCashBack
	{
		public int SerialId { get; set; }
		public int CarId { get; set; }
		public int CityId { get; set; }
		public decimal BackPrice { get; set; }
		public string Url { get; set; }
	}
}
