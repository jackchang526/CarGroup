using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class UCarInfoEntity
	{
		private string cityName;
		private string producerName;
		private string brandName;
		private string carName;
		private string buyCarDate;
		private string color;
		private string drivingMileage;
		private string displayPrice;
		private string vendorFullName;
		private string carlistUrl;
		// add by chengl Sep.29.2011
		private string vendorUrl;
		private string cityUrL;

		public string CityName
		{
			get { return cityName; }
			set { cityName = value; }
		}
		public string ProducerName
		{
			get { return producerName; }
			set { producerName = value; }
		}
		public string BrandName
		{
			get { return brandName; }
			set { brandName = value; }
		}
		public string CarName
		{
			get { return carName; }
			set { carName = value; }
		}
		public string BuyCarDate
		{
			get { return buyCarDate; }
			set { buyCarDate = value; }
		}
		public string Color
		{
			get { return color; }
			set { color = value; }
		}
		public string DrivingMileage
		{
			get { return drivingMileage; }
			set { drivingMileage = value; }
		}
		public string DisplayPrice
		{
			get { return displayPrice; }
			set { displayPrice = value; }
		}
		public string VendorFullName
		{
			get { return vendorFullName; }
			set { vendorFullName = value; }
		}
		public string CarlistUrl
		{
			get { return carlistUrl; }
			set { carlistUrl = value; }
		}

		public string VendorUrl
		{
			get { return vendorUrl; }
			set { vendorUrl = value; }
		}

		public string CityUrL
		{
			get { return cityUrL; }
			set { cityUrL = value; }
		}

	}
}
