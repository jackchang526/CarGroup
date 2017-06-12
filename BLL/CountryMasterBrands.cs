using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BitAuto.CarChannel.BLL
{
	/// <summary>
	/// ĳ�����е���Ʒ��
	/// </summary>
	public class CountryMasterBrands:List<XmlElement>
	{
		private string m_label;							//���ұ�ǩ
		private string m_countryName;					//������ʾ����
		private static Dictionary<string, string> m_labelInfo;	//���Ҷ�Ӧ������

		/// <summary>
		/// ���ұ�ǩ
		/// </summary>
		public string Label
		{
			get { return m_label; }
		}
		/// <summary>
		/// ������ʾ����
		/// </summary>
		public string CountryName
		{
			get { return m_countryName; }
		}

		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="countryName"></param>
		public CountryMasterBrands(string label)
		{
			m_label = label;
			m_countryName = m_labelInfo[label];
		}

		/// <summary>
		/// ��̬����
		/// </summary>
		static CountryMasterBrands()
		{
			m_labelInfo = new Dictionary<string, string>();
			m_labelInfo["zz"] = "����";
			m_labelInfo["dx"] = "�¹�";
			m_labelInfo["rx"] = "�ձ�";
			m_labelInfo["mx"] = "����";
			m_labelInfo["hx"] = "����";
			m_labelInfo["fx"] = "����";
			m_labelInfo["yx"] = "Ӣ��";
			m_labelInfo["yx2"] = "�����";
			m_labelInfo["other"] = "����";
		}

		/// <summary>
		/// ��ȡ�������Ƶı�ǩ
		/// </summary>
		/// <param name="countryName"></param>
		/// <returns></returns>
		public static string GetCountryLabel(string countryName)
		{
			string label = "other";
			switch(countryName)
			{
				case "�й�":
					label = "zz";
					break;
				case "�¹�":
					label = "dx";
					break;
				case "�ձ�":
					label = "rx";
					break;
				case "����":
					label = "mx";
					break;
				case "����":
					label = "hx";
					break;
				case "����":
					label = "fx";
					break;
				case "Ӣ��":
					label = "yx";
					break;
				case "�����":
					label = "yx2";
					break;
				default:
					label = "other";
					break;
			}
			return label;
		}
	}
}
