using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SerialCompareListEntity
	{
		public int SerialId { get; set; }
		public string SerialShowName { get; set; }
		public string SerialAllSpell { get; set; }
		public string SerialPriceRange { get; set; }
		public string SerialImageUrl { get; set; }

		public int ToSerialId { get; set; }
		public string ToSerialShowName { get; set; }
		public string ToSerialAllSpell { get; set; }
		public string ToSerialPriceRange { get; set; }
		public string ToSerialImageUrl { get; set; }

        public int CompareCount { get; set; }

    }
}
