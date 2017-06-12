using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    [Serializable]
    public class ExhibitionRainbow
    {
        private int m_ExhibitionID;
        private int m_SerialID;
        private int m_ItemID;
        private string m_Url;
        private DateTime m_HappenTime;

        /// <summary>
        /// 展会ID
        /// </summary>
        public int ExhibitionID
        {
            get { return m_ExhibitionID; }
            set { m_ExhibitionID = value; }
        }
        /// <summary>
        /// 车子品牌ID
        /// </summary>
        public int SerialID
        {
            get { return m_SerialID; }
            set { m_SerialID = value; }
        }
        /// <summary>
        /// 彩虹条URL
        /// </summary>
        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        /// <summary>
        /// 彩虹条发生时间
        /// </summary>
        public DateTime HappenTime
        {
            get { return m_HappenTime; }
            set { m_HappenTime = value; }
        }
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ItemID
        {
            get { return m_ItemID; }
            set { m_ItemID = value; }
        }
    }
}
