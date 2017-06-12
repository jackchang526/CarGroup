using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SerialPositionEntity
	{
		public int ImageCount { get; set; }
		public string Url { get; set; }
		public List<SerialCategoryEntity> SerialCategoryList { get; set; }
		public List<SerialPositionImagesEntity> SerialPositionImageList { get; set; }
	}
	public class SerialCategoryEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ImageCount { get; set; }
		public string Url { get; set; }
	}
	public class SerialPositionImagesEntity
	{
		public int ImageId { get; set; }
		public string ImageName { get; set; }
		public string ImageUrl { get; set; }
		public int CarModelId { get; set; }
		public int PositionId { get; set; }
		public string PositionName { get; set; }
		public string Url { get; set; }
	}
}
