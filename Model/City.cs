using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class City
	{
		private int m_id;
		private string m_name;
		private string m_spell;
		private int m_level;

		public City()
		{
		
		}

		public City(int cityId,string cityName,string citySpell,int level)
		{
			m_id = cityId;
			m_name = cityName;
			m_spell = citySpell;
			m_level = level;
		}

        public City(int cityId, string cityName, string citySpell)
        {
            m_id = cityId;
            m_name = cityName;
            m_spell = citySpell;
        }

		/// <summary>
		/// ����ID
		/// </summary>
		public int CityId
		{
			get { return m_id; }
			set { m_id = value; }
		}

		/// <summary>
		/// �������� 
		/// </summary>
		public string CityName
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// <summary>
		/// ������ȫƴ
		/// </summary>
		public string CitySpell
		{
			get { return m_spell; }
			set { m_spell = value; }
		}
		/// <summary>
		/// ���м���
		/// </summary>
		public int Level
		{
			get { return m_level; }
			set { m_level = value; }
		}
	}
	public class CityExtend : City
	{
		private int m_parentId;
		/// <summary>
		/// �ϼ�
		/// </summary>
		public int ParentId
		{
			get { return m_parentId; }
			set { m_parentId = value; }
		}
		public CityExtend(int cityId,string cityName,string citySpell,int level,int parentId)
			:base(cityId,cityName,citySpell,level)
		{
			m_parentId = parentId;
		}

		public CityExtend(int cityId, string cityName, string citySpell, int parentId)
			: base(cityId, cityName, citySpell)
        {
			m_parentId = parentId;
        }
	}
}
