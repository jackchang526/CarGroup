using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.DAL.Data;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Data
{
	/// <summary>
	/// 品牌数据
	/// </summary>
	public class BrandEntity:BaseEntity
	{
		protected int m_masterbrandId;
		protected int m_producerId;
		protected string m_showName;
		protected string m_country;
		protected string m_producerCountry;
		protected string m_introduction;
		protected string m_officialUrl;
		protected WeakReference m_masterEntity;
		protected WeakReference m_producer;
        protected SerialEntity[] m_serialList;
		protected bool m_isImport;
		/// <summary>
		/// 所属主品牌ID
		/// </summary>
		public int MasterBrandId
		{
			get { return m_masterbrandId; }
		}

		/// <summary>
		/// 所属主品牌
		/// </summary>
		public MasterBrandEntity MasterBrand
		{
			get
			{
				MasterBrandEntity master = null;
				if (m_masterEntity != null)
					master = m_masterEntity.Target as MasterBrandEntity;
				if (master == null)
				{
					master = (MasterBrandEntity)DataManager.GetDataEntity(EntityType.MasterBrand, m_masterbrandId);
					if (m_masterEntity == null)
						m_masterEntity = new WeakReference(master);
					else
						m_masterEntity.Target = master;
				}
				return master;
			}
		}

		/// <summary>
		/// 所属厂商ID
		/// </summary>
		public int ProducerId
		{
			get { return m_producerId; }
		}

		/// <summary>
		/// 品牌所属厂商
		/// </summary>
		public ProducerEntity Producer
		{
			get
			{
				ProducerEntity prod = null;
				if (m_producer != null)
					prod = m_producer.Target as ProducerEntity;
				if (prod == null)
				{
					prod = (ProducerEntity)DataManager.GetDataEntity(EntityType.Producer, m_producerId);
					if (m_producer == null)
						m_producer = new WeakReference(prod);
					else
						m_producer.Target = prod;
				}
				return prod;
			}
		}

		/// <summary>
		/// 品牌的显示名，规则：如果是进口车型，名称前加“进口”两字
		/// </summary>
		public string ShowName
		{
			get
			{
				if (String.IsNullOrEmpty(m_showName))
				{
					if (m_country != "中国")
						m_showName = "进口" + m_name;
					else
						m_showName = m_name;
				}
				return m_showName; 
			}
		}

		/// <summary>
		/// 生产国家
		/// </summary>
		public string Country
		{
			get { return m_country; }
		}

		/// <summary>
		/// 品牌厂商的国家
		/// </summary>
		public string ProducerCountry
		{
			get { return m_producerCountry; }
		}

		/// <summary>
		/// 是否进口品牌
		/// </summary>
		public bool IsImport
		{
			get { return m_isImport; }
		}

		/// <summary>
		/// 品牌介绍
		/// </summary>
		public string Introduction
		{
			get { return m_introduction; }
		}
		/// <summary>
		/// 官方网站
		/// </summary>
		public string OfficialUrl
		{
			get { return m_officialUrl; }
		}
        /// <summary>
        /// 子品牌列表
        /// </summary>
        public SerialEntity[] SerialList
        {
            get
            {
				if (m_serialList == null)
				{
					//string mutexName = "mutex_seriallist_" + this.Id;
					//Mutex m = new Mutex(false, mutexName);
					//m.WaitOne();
					if (m_serialList == null)
						InitSerialList();
					//m.ReleaseMutex();
					//m.Close();
				}
                return m_serialList;
            }
        }

        public BrandEntity()
        {
        }
        public BrandEntity(DataRow dataRow)
        {
            SetValueByDataRow(dataRow);
        }

		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="id"></param>
		public override void InitData(int id)
		{
			DataSet ds = new TBrandDAL().GetBrandDataById(id);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow row = ds.Tables[0].Rows[0];
                SetValueByDataRow(row);
			}
			else
				m_id = 0;
		}

        private void SetValueByDataRow(DataRow dataRow)
        {
            if (dataRow == null)
                return;
            m_id = ConvertHelper.GetInteger(dataRow["cb_Id"]);
            m_name = dataRow["cb_Name"].ToString().Trim();
            m_seoName = dataRow["cb_seoname"].ToString().Trim();
			// modified by chengl Mar.20.2012
			// 品牌国别继承厂商的国别
			m_country = dataRow["cb_country"].ToString().Trim();
			//m_country = dataRow["cp_country"].ToString().Trim();
			m_producerCountry = dataRow["cp_country"].ToString().Trim();
            m_introduction = dataRow["cb_introduction"].ToString().Trim();
            m_spell = dataRow["spell"].ToString().Trim();
            m_allSpell = dataRow["allSpell"].ToString().Trim().ToLower();
			m_officialUrl = dataRow["cb_url"] == DBNull.Value ? String.Empty : dataRow["cb_url"].ToString().Trim().ToLower();
            m_masterbrandId = ConvertHelper.GetInteger(dataRow["bs_id"]);
            m_producerId = ConvertHelper.GetInteger(dataRow["cp_Id"]);
			m_isImport = m_country != "中国";
        }

		/// <summary>
		/// 根据获取数据字段名称取数据
		/// </summary>
		/// <param name="dataName"></param>
		/// <returns></returns>
		public override string GetDataValue(EntityType eType, string dataName )
		{
			string dataValue = String.Empty;
			if (eType != EntityType.Brand)
				dataValue = this.MasterBrand.GetDataValue(eType, dataName);
			else
			{
				dataValue = base.GetDataValue(dataName);
				if (dataValue.Length == 0)
				{
					switch (dataName)
					{
						case "Introduction":
							dataValue = m_introduction;
							break;
						case "Country":
							dataValue = m_country;
							break;
					}
				}
			}
			return dataValue;
		}

        /// <summary>
        /// 初始化子品牌列表
        /// </summary>
        private void InitSerialList()
        {
            List<SerialEntity> serialList = new List<SerialEntity>();

            DataSet ds = new TSerialDAL().GetSerailDataByBrandId(m_id);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dataRow in dt.Rows)
                {
                    serialList.Add(new SerialEntity(dataRow));
                }
            }

            m_serialList = serialList.ToArray();
        }
	}
}
