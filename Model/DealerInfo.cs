using System;
using System.Collections.Generic;
using System.Text;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace BitAuto.CarChannel.Model
{
	public class DealerInfo
	{
		public DealerInfo()
		{
		}

        //[BsonId(IdGenerator = typeof(CombGuidGenerator))]
        //public Guid Id { get; set; }

		private object _dealerId;
		public object DealerId
		{
			get { return _dealerId; }
			set { _dealerId = value; }
		}

		private string _vendorName;
		public string VendorName
		{
			get { return _vendorName; }
			set { _vendorName = value; }
		}


		private string _companyName;
		public string CompanyName
		{
			get { return _companyName; }
			set { _companyName = value; }
		}

		private decimal _salePrice;
		public decimal SalePrice
		{
			get { return _salePrice; }
			set { _salePrice = value; }
		}

		private string _address;
		public string Address
		{
			get { return _address; }
			set { _address = value; }
		}

		private decimal _evaluatePrice;
		public decimal EvaluatePrice
		{
			get { return _evaluatePrice; }
			set { _evaluatePrice = value; }
		}

		private string _phoneNumber;
		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { _phoneNumber = value; }
		}

		private int _provinceId;

		public int ProvinceId
		{
			get { return _provinceId; }
			set { _provinceId = value; }
		}

		private int _cityId;
		public int CityId
		{
			get { return _cityId; }
			set { _cityId = value; }
		}

		private int _districtId;
		public int DistrictId
		{
			get { return _districtId; }
			set { _districtId = value; }
		}

		private int _orderrWeight;
		public int OrderWeight
		{
			get { return _orderrWeight; }
			set { _orderrWeight = value; }
		}

		private int _dealerType;
        /// <summary>
        /// 经销商类型 2=4s
        /// </summary>
		public int DealerType
		{
			get { return _dealerType; }
			set { _dealerType = value; }
		}

		private string _dealerurl;
		public string DealerUrl
		{
			get { return _dealerurl; }
			set { _dealerurl = value; }
		}
	}

}
