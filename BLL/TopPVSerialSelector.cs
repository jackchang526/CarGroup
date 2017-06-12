using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BitAuto.CarChannel.BLL
{
	public class TopPVSerialSelector
	{
		private List<XmlElement> m_eleList;
		private List<XmlElement> m_newCarList;
		private int m_topNum;
		private int m_newCarNum;
		private bool m_selectNewCar;

		public TopPVSerialSelector()
		{
			m_eleList = new List<XmlElement>();
			m_newCarList = new List<XmlElement>();
			m_topNum = 10;
			m_newCarNum = 4;
			m_selectNewCar = false;
		}

		public TopPVSerialSelector(int top)
			:this()
		{
			if (top <= 0)
				throw (new Exception("选择数量不可为零或负数"));
			m_topNum = top;
		}

		/// <summary>
		/// 是否统计热门新车
		/// </summary>
		public bool SelectNewCar
		{
			get { return m_selectNewCar; }
			set { m_selectNewCar = value; }
		}

		/// <summary>
		/// 获取新车的数量
		/// </summary>
		public int NewCarNum
		{
			get { return m_newCarNum; }
			set { m_newCarNum = value; }
		}

		/// <summary>
		/// 获取选择结果
		/// </summary>
		/// <returns></returns>
		public List<XmlElement> GetTopSerialList()
		{
			if (m_eleList.Count > m_topNum)
				return m_eleList.GetRange(0, m_topNum);
			else
				return m_eleList;
		}

		/// <summary>
		/// 取热门新车
		/// </summary>
		/// <returns></returns>
		public List<XmlElement> GetNewCarList()
		{
			if (m_newCarList.Count >= m_newCarNum)
				return m_newCarList.GetRange(0, m_newCarNum);
			else
			{
				//加入其他高点击车型
				foreach(XmlElement ele in m_eleList)
				{
					try
					{
						int isNewCar = Convert.ToInt32(ele.GetAttribute("CsHasNew"));
						if (isNewCar == 1)
							continue;

						m_newCarList.Add(ele);
						if (m_newCarList.Count >= m_newCarNum)
							break;
					}
					catch {}
				}
				return m_newCarList;
			}
		}

		/// <summary>
		/// 增加一个子品牌
		/// </summary>
		/// <param name="ele"></param>
		public void AddSerial(XmlElement ele)
		{
			AddToSerialList(ele);
			if(m_selectNewCar)
				AddToNewCarList(ele);
		}

		/// <summary>
		/// 加到热门车型列表中
		/// </summary>
		/// <param name="ele"></param>
		private void AddToSerialList(XmlElement ele)
		{
			if (ele == null)
				return;
			try
			{
				bool isInsert = false;
				int pvNum = Convert.ToInt32(ele.GetAttribute("CsPV"));
				for (int i = 0; i < m_topNum && i < m_eleList.Count; i++)
				{
					int tempNum = Convert.ToInt32(m_eleList[i].GetAttribute("CsPV"));
					if (pvNum > tempNum)
					{
						m_eleList.Insert(i, ele);
						isInsert = true;
						break;
					}
				}

				if (!isInsert && m_eleList.Count < m_topNum)
					m_eleList.Add(ele);
			}
			catch { }
		}

		/// <summary>
		/// 加到新车列表里
		/// </summary>
		/// <param name="ele"></param>
		private void AddToNewCarList(XmlElement ele)
		{
			if(ele == null)
			{
				return;
			}

			try
			{
				bool isInsert = false;
				int isNewCar = Convert.ToInt32(ele.GetAttribute("CsHasNew"));
				if(isNewCar == 1)
				{
					int pvNum = Convert.ToInt32(ele.GetAttribute("CsPV"));
					for (int i = 0; i < m_newCarNum && i < m_newCarList.Count; i++)
					{
						int tempNum = Convert.ToInt32(m_newCarList[i].GetAttribute("CsPV"));
						if (pvNum > tempNum)
						{
							m_newCarList.Insert(i, ele);
							isInsert = true;
							break;
						}
					}

					if (!isInsert && m_newCarList.Count < m_newCarNum)
						m_newCarList.Add(ele);
				}
			}
			catch{}
		}

		/// <summary>
		/// 清除结果，以便开始新的选择
		/// </summary>
		public void Clear()
		{
			m_eleList.Clear();
		}
	}
}
