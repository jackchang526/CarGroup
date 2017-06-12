using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class CNCAPEntity
	{
		public int SerialId { get; set; }
		public string Name { get; set; }
		public int ParamId { get; set; }
		public string ParamValue { get; set; }
	}
}
