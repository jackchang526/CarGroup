using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 子品牌图片信息
    /// </summary>
    [Serializable]
    public class CarSerialPhotoEntity
    {
        public CarSerialPhotoEntity()
        { }

		private int m_serialId = 0;
        private string _cs_Name     = String.Empty;
		private string m_csShowName = String.Empty;
        private string _cs_ImageUrl = String.Empty;
        private string _cs_AllSpell = String.Empty;
		private string m_saleState = String.Empty;
		private string m_serialLevel = String.Empty;
        private string m_CS_SeoName = string.Empty;
        private string m_cs_spell = string.Empty;
       

        /// <summary>
        /// 子品牌SEO名
        /// </summary>
        public string CS_SeoName
        {
            get { return m_CS_SeoName; }
            set { m_CS_SeoName = value; }
        }

		public int SerialId
		{
			get { return m_serialId; }
			set { m_serialId = value; }
		}

        public string CS_ImageUrl
        {
            get { return _cs_ImageUrl; }
            set { _cs_ImageUrl = value; }
        }

        public string CS_Name
        {
            get { return _cs_Name; }
            set { _cs_Name = value; }
        }

		public string ShowName
		{
			get { return m_csShowName; }
			set { m_csShowName = value; }
		}

        public string CS_AllSpell
        {
            get { return _cs_AllSpell; }
            set { _cs_AllSpell = value; }
        }

		public string SaleState
		{
			get { return m_saleState; }
			set { m_saleState = value; }
		}

		public string SerialLevel
		{
			get { return m_serialLevel; }
			set { m_serialLevel = value; }
		}
        /// <summary>
        /// 子品牌拼音
        /// </summary>
        public string Cs_spell
        {
            get { return m_cs_spell; }
            set { m_cs_spell = value; }
        }

    }
}
