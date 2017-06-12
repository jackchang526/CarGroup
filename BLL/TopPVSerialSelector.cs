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
				throw (new Exception("ѡ����������Ϊ�����"));
			m_topNum = top;
		}

		/// <summary>
		/// �Ƿ�ͳ�������³�
		/// </summary>
		public bool SelectNewCar
		{
			get { return m_selectNewCar; }
			set { m_selectNewCar = value; }
		}

		/// <summary>
		/// ��ȡ�³�������
		/// </summary>
		public int NewCarNum
		{
			get { return m_newCarNum; }
			set { m_newCarNum = value; }
		}

		/// <summary>
		/// ��ȡѡ����
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
		/// ȡ�����³�
		/// </summary>
		/// <returns></returns>
		public List<XmlElement> GetNewCarList()
		{
			if (m_newCarList.Count >= m_newCarNum)
				return m_newCarList.GetRange(0, m_newCarNum);
			else
			{
				//���������ߵ������
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
		/// ����һ����Ʒ��
		/// </summary>
		/// <param name="ele"></param>
		public void AddSerial(XmlElement ele)
		{
			AddToSerialList(ele);
			if(m_selectNewCar)
				AddToNewCarList(ele);
		}

		/// <summary>
		/// �ӵ����ų����б���
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
		/// �ӵ��³��б���
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
		/// ���������Ա㿪ʼ�µ�ѡ��
		/// </summary>
		public void Clear()
		{
			m_eleList.Clear();
		}
	}
}
