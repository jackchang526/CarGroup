using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class SelectCarParameters
	{
		private int m_minPrice;
		private int m_maxPrice;
		private int m_minReferPrice;
		private int m_maxReferPrice;
		private double m_minDis;
		private double m_maxDis;
		private int m_transmissionType;
		private int m_bodyForm;
		private int m_brandType;
		private int m_level;
		private int m_purpose;
		private int m_country;
		private int m_comfortable;
		private int m_safety;
		private int m_pageIndex;
		private int m_carConfig;

		private string m_conditionStr;		//��ѯ����˵��
		private int m_priceFlag;			//���б��۲�ѯ�ı�־
        //�ͺ�
        private double m_minFuel;
        private double m_maxFuel;

		/// <summary>
		/// ��С����ֵ����λ����Ԫ��
		/// </summary>
		public int MinPrice
		{
			get { return m_minPrice; }
			set { m_minPrice = value; }
		}
		/// <summary>
		/// ��󱨼�ֵ����λ����Ԫ��
		/// </summary>
		public int MaxPrice
		{
			get { return m_maxPrice; }
			set { m_maxPrice = value; }
		}

		/// <summary>
		/// ��Сָ���ۣ���λ����Ԫ��
		/// </summary>
		public int MinReferPrice
		{
			get { return m_minReferPrice; }
			set { m_minReferPrice = value; }
		}

		/// <summary>
		/// ���ָ����(��Ԫ)
		/// </summary>
		public int MaxReferPrice
		{
			get { return m_maxReferPrice; }
			set { m_maxReferPrice = value; }
		}
		/// <summary>
		/// ��С����ֵ
		/// </summary>
		public double MinDis
		{
			get { return m_minDis; }
			set { m_minDis = value; }
		}
		/// <summary>
		/// �������ֵ
		/// </summary>
		public double MaxDis
		{
			get { return m_maxDis; }
			set { m_maxDis = value; }
		}
		/// <summary>
		/// ����������
		/// </summary>
		public int TransmissionType
		{
			get { return m_transmissionType; }
			set { m_transmissionType = value; }
		}
		/// <summary>
		/// ������ʽ
		/// </summary>
		public int BodyForm
		{
			get { return m_bodyForm; }
			set { m_bodyForm = value; }
		}

		/// <summary>
		/// Ʒ�����ͣ����������ʣ�����
		/// </summary>
		public int BrandType
		{
			get { return m_brandType; }
			set { m_brandType = value; }
		}
		/// <summary>
		/// ���ͼ���
		/// </summary>
		public int Level
		{
			get { return m_level; }
			set { m_level = value; }
		}
		/// <summary>
		/// ������;
		/// </summary>
		public int Purpose
		{
			get { return m_purpose; }
			set { m_purpose = value; }
		}
		/// <summary>
		/// ��ϵ����
		/// </summary>
		public int Country
		{
			get { return m_country; }
			set { m_country = value; }
		}
		/// <summary>
		/// ����������
		/// </summary>
		public int ComfortableConfig
		{
			get { return m_comfortable; }
			set { m_comfortable = value; }
		}
		/// <summary>
		/// ��ȫ������
		/// </summary>
		public int SafetyConfig
		{
			get { return m_safety; }
			set { m_safety = value; }
		}
		/// <summary>
		/// ҳ��
		/// </summary>
		public int PageIndex
		{
			get { return m_pageIndex; }
			set { m_pageIndex = value; }
		}

		/// <summary>
		/// ��������
		/// </summary>
		public int CarConfig
		{
			get { return m_carConfig; }
			set { m_carConfig = value; }
		}

		/// <summary>
		/// ��ѯ����˵��
		/// </summary>
		public string ConditionString
		{
			get { return m_conditionStr; }
			set { m_conditionStr = value; }
		}

		/// <summary>
		/// ���б��������ı�־
		/// </summary>
		public int PriceFlag
		{
			get { return m_priceFlag; }
			set { m_priceFlag = value; }
		}
		/// <summary>
		/// ������ʽ
		/// </summary>
		public int DriveType { get; set; }
		/// <summary>
		/// ȼ������
		/// </summary>
		public int FuelType { get; set; }
		/// <summary>
		/// ��С������
		/// </summary>
		public int MinBodyDoors { get; set; }
		/// <summary>
		/// �������
		/// </summary>
		public int MaxBodyDoors { get; set; }
		/// <summary>
		///	��С��Ա��������˾����
		/// </summary>
		public int MinPerfSeatNum { get; set; }
		/// <summary>
		/// ����Ա��������˾����
		/// </summary>
		public int MaxPerfSeatNum { get; set; }
		/// <summary>
		/// �Ƿ����а�
		/// </summary>
		public int IsWagon { get; set; }
        /// <summary>
        /// ��С�ͺ�ֵ
        /// </summary>
        public double MinFuel
        {
            get { return m_minFuel; }
            set { m_minFuel = value; }
        }
        /// <summary>
        /// ����ͺ�ֵ
        /// </summary>
        public double MaxFuel
        {
            get { return m_maxFuel; }
            set { m_maxFuel = value; }
        }
	}
}


