using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class VideoEntity
	{
		public int VideoId { get; set; }
		public string ShortTitle { get; set; }
		public string ImageLink { get; set; }
 		public string ShowPlayUrl { get; set; }
		public int Duration { get; set; }
        //视频来源，1是社区，0是视频库
        public int Source { get; set; }
	}
}
