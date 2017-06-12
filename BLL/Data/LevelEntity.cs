using System;
using System.Collections.Generic;
using System.Text;

using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarUtils.Define;

namespace BitAuto.CarChannel.BLL.Data
{
	public class LevelEntity : BaseEntity
	{
		protected CarLevelDefine.SerialLevelEnum m_level;
		protected CarLevelDefine.SerialLevelSpellEnum m_levelSpell;

		/// <summary>
		/// 初始化级别数据实体
		/// </summary>
		/// <param name="id"></param>
		public override void InitData(int id)
		{
			//m_level = (CarLevelDefine.SerialLevelEnum)id;
			//m_levelSpell = (CarLevelDefine.SerialLevelSpellEnum)id;
			m_id = id;
			m_allSpell = CarLevelDefine.GetLevelSpellById(id);// m_levelSpell.ToString();
			m_name = CarLevelDefine.GetLevelNameById(id);// m_level.ToString();
			if (m_name == "紧凑型")
				m_name = "紧凑型车";
			else if (m_name == "豪华型")
				m_name = "豪华型车";
			else if (m_name == "中大型")
				m_name = "中大型车";
		}


		/// <summary>
		/// 根据获取数据字段名称取数据
		/// </summary>
		/// <param name="dataName"></param>
		/// <returns></returns>
		public override string GetDataValue(EntityType eType, string dataName)
		{
			string dataValue = base.GetDataValue(dataName);
			return dataValue;
		}
	}
}
