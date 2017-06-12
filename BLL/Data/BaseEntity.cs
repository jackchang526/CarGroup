using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Data
{
	/// <summary>
	/// 所有数据实体的基类
	/// </summary>
	public abstract class BaseEntity
	{
		protected int m_id;
		protected EntityType m_entityType;
		protected string m_name;
		protected string m_seoName;
		protected string m_spell;
		protected string m_allSpell;
		protected int m_isState;

		/// <summary>
		/// 数据实体ID
		/// </summary>
		public int Id
		{
			get { return m_id; }
			set { m_id = value; }
		}

		/// <summary>
		/// 数据实体名称
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// <summary>
		/// 数据实体拼写
		/// </summary>
		public string Spell
		{
			get { return m_spell; }
			set { m_spell = value; }
		}

		/// <summary>
		/// 数据实体全拼
		/// </summary>
		public string AllSpell
		{
			get { return m_allSpell; }
			set { m_allSpell = value; }
		}

		/// <summary>
		/// 是否可用
		/// </summary>
		public int IsState
		{
			get { return m_isState; }
			set { m_isState = value; }
		}

		/// <summary>
		/// SEO使用的名称
		/// </summary>
		public string SeoName
		{
			get { return m_seoName; }
		}

		/// <summary>
		/// 初始化数据
		/// </summary>
		/// <param name="id"></param>
		public abstract void InitData(int id);

		/// <summary>
		/// 按指定类型获取数据
		/// </summary>
		/// <param name="eTyep"></param>
		/// <param name="dataName"></param>
		/// <returns></returns>
		public abstract string GetDataValue( EntityType eTyep, string dataName );

		/// <summary>
		/// 按数据名称取实体中的数据
		/// </summary>
		/// <param name="dataName"></param>
		/// <returns></returns>
		protected string GetDataValue( string dataName )
		{
			string dataValue = String.Empty;
			switch (dataName)
			{
				case "Id":
					dataValue = m_id.ToString();
					break;
				case "Name":
					dataValue = m_name;
					break;
				case "AllSpell":
					dataValue = m_allSpell;
					break;
				case "Spell":
					dataValue = m_spell;
					break;
				case "IsState":
					dataValue = m_isState.ToString();
					break;
				case "SeoName":
					dataValue = m_seoName;
					break;
			}
			return dataValue;
		}

	}
}
