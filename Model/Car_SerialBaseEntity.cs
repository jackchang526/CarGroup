using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class Car_SerialBaseEntity
	{
		#region 私有字段
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

		#region 公有属性
		/// <summary>
		/// 子品牌ID
		/// </summary>
		public int SerialId
		{
			get { return m_serialId; }
			set { m_serialId = value; }
		}

		/// <summary>
		/// 子品牌名称
		/// </summary>
		public string SerialName
		{
			get { return m_serialName; }
			set { m_serialName = value; }
		}

		/// <summary>
		/// 子品牌显示名
		/// </summary>
		public string SerialShowName
		{
			get{return m_serialShowName;}
			set{m_serialShowName = value;}
		}

		/// <summary>
		/// 子品牌全拼
		/// </summary>
		public string SerialNameSpell
		{
			get { return m_serialNameSpell; }
			set { m_serialNameSpell = value;}
		}

		/// <summary>
		/// 子品牌级别
		/// </summary>
		public string SerialLevel
		{
			get { return m_serialLevel; }
			set{m_serialLevel = value;}
		}

		/// <summary>
		/// 子品牌级别全拼
		/// </summary>
		public string SerialLevelSpell
		{
			get { return m_serialLevelSpell; }
			set { m_serialLevelSpell = value; }
		}
		/// <summary>
		/// 所属品牌名称
		/// </summary>
		public string BrandName
		{
			get { return m_brandName; }
			set{m_brandName=value;}
		}

		/// <summary>
		/// 所属品牌的URL全拼
		/// </summary>
		public string BrandSpell
		{
			get { return m_brandUrlSpell; }
			set { m_brandUrlSpell = value; }
		}

		/// <summary>
		/// 主品牌ID
		/// </summary>
		public int MasterbrandId
		{
			get{return m_masterBrandId;}
			set { m_masterBrandId = value; }
		}
		/// <summary>
		/// 所属主品牌名称
		/// </summary>
		public string MasterBrandName
		{
			get { return m_masterBrandName; }
			set { m_masterBrandName = value; }
		}

		/// <summary>
		/// 主品牌全拼
		/// </summary>
		public string MasterbrandSpell
		{
			get { return m_masterBrandSpell; }
			set { m_masterBrandSpell = value; }
		}
        /// <summary>
        /// 子品牌SEO名称
        /// </summary>
        public string SerialSeoName
        {
            get { return m_serialSeoName; }
            set { m_serialSeoName = value; }
        }
        /// <summary>
        /// 子品牌报价
        /// </summary>
        public string SerialPrice
        {
            get { return m_serialPrice; }
            set { m_serialPrice = value; }
        }
		#endregion
	}
}
