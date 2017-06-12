using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class NewsForSerialSummaryEntity
	{
		public int CmsNewsId { get; set; }
		public string Title { get; set; }
		public string FaceTitle { get; set; }
		public string Picture { get; set; }
		public string FilePath { get; set; }
		public DateTime PublishTime { get; set; }
	}
}
