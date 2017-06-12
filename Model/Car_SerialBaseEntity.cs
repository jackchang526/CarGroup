using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class Car_SerialBaseEntity
	{
		#region ˽���ֶ�
		private int m_serialId;
		private string m_serialName;
		private string m_serialShowName;
		private string m_serialNameSpell;
		private string m_serialLevel;
        private string m_serialLevelSpell;
        private string m_serialSeoName;
		private string m_brandName;
		private string m_brandUrlSpell;
		private int m_masterBrandId;
		private string m_masterBrandName;
		private string m_masterBrandSpell;
        private string m_serialPrice;
		#endregion

		#region ��������
		/// <summary>
		/// ��Ʒ��ID
		/// </summary>
		public int SerialId
		{
			get { return m_serialId; }
			set { m_serialId = value; }
		}

		/// <summary>
		/// ��Ʒ������
		/// </summary>
		public string SerialName
		{
			get { return m_serialName; }
			set { m_serialName = value; }
		}

		/// <summary>
		/// ��Ʒ����ʾ��
		/// </summary>
		public string SerialShowName
		{
			get{return m_serialShowName;}
			set{m_serialShowName = value;}
		}

		/// <summary>
		/// ��Ʒ��ȫƴ
		/// </summary>
		public string SerialNameSpell
		{
			get { return m_serialNameSpell; }
			set { m_serialNameSpell = value;}
		}

		/// <summary>
		/// ��Ʒ�Ƽ���
		/// </summary>
		public string SerialLevel
		{
			get { return m_serialLevel; }
			set{m_serialLevel = value;}
		}

		/// <summary>
		/// ��Ʒ�Ƽ���ȫƴ
		/// </summary>
		public string SerialLevelSpell
		{
			get { return m_serialLevelSpell; }
			set { m_serialLevelSpell = value; }
		}
		/// <summary>
		/// ����Ʒ������
		/// </summary>
		public string BrandName
		{
			get { return m_brandName; }
			set{m_brandName=value;}
		}

		/// <summary>
		/// ����Ʒ�Ƶ�URLȫƴ
		/// </summary>
		public string BrandSpell
		{
			get { return m_brandUrlSpell; }
			set { m_brandUrlSpell = value; }
		}

		/// <summary>
		/// ��Ʒ��ID
		/// </summary>
		public int MasterbrandId
		{
			get{return m_masterBrandId;}
			set { m_masterBrandId = value; }
		}
		/// <summary>
		/// ������Ʒ������
		/// </summary>
		public string MasterBrandName
		{
			get { return m_masterBrandName; }
			set { m_masterBrandName = value; }
		}

		/// <summary>
		/// ��Ʒ��ȫƴ
		/// </summary>
		public string MasterbrandSpell
		{
			get { return m_masterBrandSpell; }
			set { m_masterBrandSpell = value; }
		}
        /// <summary>
        /// ��Ʒ��SEO����
        /// </summary>
        public string SerialSeoName
        {
            get { return m_serialSeoName; }
            set { m_serialSeoName = value; }
        }
        /// <summary>
        /// ��Ʒ�Ʊ���
        /// </summary>
        public string SerialPrice
        {
            get { return m_serialPrice; }
            set { m_serialPrice = value; }
        }
		#endregion
	}
}
