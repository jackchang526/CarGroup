using System;
using System.Collections.Generic;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 展会属性
    /// </summary>
    [Serializable]
    public class Attribute
    {
        private int m_ID;
        private string m_Name;
        private Dictionary<int,int> m_SerialIDList;

        /// <summary>
        /// 属性ID
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// 属性包括子品牌列表
        /// </summary>
        public Dictionary<int, int> SerialIDList
        {
            get { return m_SerialIDList; }
            set { m_SerialIDList = value; }
        }

    }
}
