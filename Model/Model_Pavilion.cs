using System;
using System.Collections.Generic;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// Pavilion 的摘要说明
    /// </summary>
    [Serializable]
    public class Pavilion
    {
        private int m_ID;
        private string m_Name;
        private Dictionary<int, int[]> m_MasterBrandList;

        /// <summary>
        /// 展馆ID
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        /// <summary>
        /// 展馆名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// 展馆主品牌列表
        /// </summary>
        public Dictionary<int, int[]> MasterBrandList
        {
            get { return m_MasterBrandList; }
            set { m_MasterBrandList = value; }
        }


    }
}
