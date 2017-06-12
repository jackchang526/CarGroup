using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Common.Cache
{
	public class CacheStatisticsObj
	{
		private string m_cacheKey;
		private int m_insertTimes;
		private int m_totalHits;
		private int m_maxHits;
		private int m_currentHits;		//当前生命周期中被命中的次数

		//缓存的键值
		public string CacheKey
		{
			get { return m_cacheKey; }
		}

		/// <summary>
		/// 缓存项在统计期间被插入的次数
		/// </summary>
		public int InsertTimes
		{
			get { return m_insertTimes; }
		}

		/// <summary>
		/// 在统计期间缓存的总命中次数
		/// </summary>
		public int TotalHits
		{
			get { return m_totalHits; }
		}

		/// <summary>
		/// 在缓存项的统计期间，所有的生成周期中，命中次数最多的次数
		/// </summary>
		public int MaxHits
		{
			get { return m_maxHits; }
		}

		/// <summary>
		/// 初始化函数
		/// </summary>
		/// <param name="cacheKey"></param>
		public CacheStatisticsObj( string cacheKey )
		{
			m_cacheKey = cacheKey;
			m_insertTimes = 0;
			m_totalHits = 0;
			m_maxHits = 0;
		}

		/// <summary>
		/// 缓存插入一次，也就是重新生成一次
		/// </summary>
		public void InsertOnce()
		{
			if (m_currentHits > m_maxHits)
				m_maxHits = m_currentHits;
			m_currentHits = 0;
			m_insertTimes++;
		}

		/// <summary>
		/// 被命中一次
		/// </summary>
		public void Hit()
		{
			m_currentHits++;
			m_totalHits++;
			if (m_currentHits > m_maxHits)
				m_maxHits = m_currentHits;
		}

		public override string ToString()
		{
			return m_cacheKey + "," + m_insertTimes + "," + m_totalHits + "," + m_maxHits;
		}

	}
}
