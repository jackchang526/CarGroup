using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL.Data;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Data
{
	public class MasterBrandEntity:BaseEntity
	{
		protected string m_logoInfo;
		protected string m_country;
		protected string m_introduction;

        protected BrandEntity[] m_brandList;

		/// <summary>
		/// 车标故事
		/// </summary>
		public string LogoInfo
		{
			get { return m_logoInfo; }
		}

		/// <summary>
		/// 主品牌所属国家
		/// </summary>
		public string Country
		{
			get { return m_country; }
		}

		/// <summary>
		/// 主品牌介绍
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
			DataSet ds = new TMasterBrandDAL().GetMasterBrandDataById(id);
			if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				m_id = id;
				DataRow row = ds.Tables[0].Rows[0];
				m_name = row["bs_Name"].ToString().Trim();
				m_seoName = row["bs_seoname"].ToString().Trim();
				m_spell = row["spell"].ToString().Trim();
				m_allSpell = row["urlspell"].ToString().Trim().ToLower();
				m_isState = ConvertHelper.GetInteger(row["IsState"]);
				m_introduction = row["bs_introduction"].ToString().Trim();
				m_logoInfo = row["bs_LogoInfo"].ToString().Trim();
				m_country = row["bs_Country"].ToString().Trim();
			}
			else
				m_id = 0;
		}

		private void InitBrandList()
		{
            List<BrandEntity> brandList = new List<BrandEntity>();
            DataSet ds = new TBrandDAL().GetBrandDataByMasterBrandId(m_id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dataRow in dt.Rows)
                {
                    brandList.Add(new BrandEntity(dataRow));
                }
            }
			brandList.Sort(EntityCompare.BrandDefaultCompare);
            m_brandList = brandList.ToArray();
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
					case "LogoInfo":
						dataValue = m_logoInfo;
						break;
					case "Country":
						dataValue = m_country;
						break;
					case "Introduction":
						dataValue = m_introduction;
						break;
				}
			}
			return dataValue;
		}
	}
}
