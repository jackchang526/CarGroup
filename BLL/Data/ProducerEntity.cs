using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL.Data;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Data
{
	public class ProducerEntity:BaseEntity
	{
		protected string m_shortName;
		protected string m_country;
		protected string m_producerUrl;
		protected string m_introduction;
        protected BrandEntity[] m_brandList;

		/// <summary>
		/// 厂商的简称
		/// </summary>
		public string ShortName
		{
			get { return m_shortName; }
		}

		/// <summary>
		/// 厂商所在国家
		/// </summary>
		public string Country
		{
			get { return m_country; }
		}

		/// <summary>
		/// 厂商的官网
		/// </summary>
		public string ProdcuerUrl
		{
			get { return m_producerUrl; }
		}

		/// <summary>
		/// 厂商介绍
		/// </summary>
		public string Introduction
		{
			get { return m_introduction; }
		}

        /// <summary>
        /// 品牌列表
        /// </summary>
        public BrandEntity[] BrandList
        {
            get
            {
                if (m_brandList == null)
                    InitBrandList();
                return m_brandList;
            }
        }

		public override void InitData(int id)
		{
			DataSet ds = new TProducerDAL().GetProducerDataById(id);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				m_id = id;
				DataRow row = ds.Tables[0].Rows[0];
				m_name = row["Cp_Name"].ToString().Trim();
				m_shortName = row["Cp_ShortName"].ToString().Trim();
				m_spell = row["Spell"].ToString().Trim();
				m_seoName = row["cp_seoname"].ToString().Trim();
				m_producerUrl = row["Cp_Url"].ToString().Trim().ToLower();
				m_country = row["Cp_Country"].ToString().Trim();
				m_isState = ConvertHelper.GetInteger(row["IsState"]);
			}
			else
				m_id = 0;
		}


		/// <summary>
		/// 根据获取数据字段名称取数据
		/// </summary>
		/// <param name="dataName"></param>
		/// <returns></returns>
		public override string GetDataValue(EntityType eType, string dataName )
		{
			string dataValue = base.GetDataValue(dataName);
			if(dataValue.Length == 0)
			{
				switch(dataName)
				{
					case "ShortName":
						dataValue = m_shortName;
						break;
					case "Country":
						dataValue = m_country;
						break;
					case "ProdcuerUrl":
						dataValue = m_producerUrl;
						break;
					case "Introduction":
						dataValue = m_introduction;
						break;
				}
			}
			return dataValue;
		}

        /// <summary>
        /// 初始化品牌列表
        /// </summary>
        private void InitBrandList()
        {
            List<BrandEntity> brandList = new List<BrandEntity>();
            DataSet ds = new TBrandDAL().GetBrandDataByProducerId(m_id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dataRow in dt.Rows)
                {
                    brandList.Add(new BrandEntity(dataRow));
                }
            }
            m_brandList = brandList.ToArray();
        }
	}
}
