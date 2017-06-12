using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class AwardInfo
	{
		public int AwardId { get; set; }

		public string AwardLogo { get; set; }

		public string AwardName { get; set; }

		public string AwardIntro { get; set; }

		public string AwardOfficialUrl { get; set; }

		public List<YearInfo> YearInfos { get; set; }
	}

	public class YearInfo
	{
		public string YearName { get; set; }

		public List<ChildAwardSerialCar> ChildAwardSerialCars { get; set; }
	}

	public class ChildAwardSerialCar
	{
		public string ChildAwardName { get; set; }

		public List<SerialCarInfo> SerialCarInfos { get; set; }
	}

	public class SerialCarInfo
	{
		public int? CsId { get; set; }

		public string CsName { get; set; }

		public string AllSpell { get; set; }

		public string CsImageUrl { get; set; }
		/// <summary>
		/// 报价区间
		/// </summary>
		public string Price { get; set; }
	}
}
