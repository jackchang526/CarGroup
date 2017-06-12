using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	public class NameObject
	{
		private int m_objId;
		private string m_objName;
		private string m_spell;
		private string m_htmlCode;

		public NameObject(int id, string name, string spell)
		{
			m_objId = id;
			m_objName = name;
			m_spell = spell;
			m_htmlCode = "";
		}

		public int ObjectId
		{
			get { return m_objId; }
			set { m_objId = value; }
		}

		public string ObjectName
		{
			get { return m_objName; }
			set { m_objName = value; }
		}

		public string ObjectSpell
		{
			get { return m_spell; }
			set { m_spell = value; }
		}

		public string HtmlCode
		{
			get { return m_htmlCode; }
			set { m_htmlCode = value; }
		}

		/// <summary>
		/// 比较方法
		/// </summary>
		/// <param name="sh1"></param>
		/// <param name="sh2"></param>
		/// <returns></returns>
		public static int Comparer(NameObject sh1, NameObject sh2)
		{
			return String.Compare(sh1.ObjectSpell, sh2.ObjectSpell);
		}
	}

}
