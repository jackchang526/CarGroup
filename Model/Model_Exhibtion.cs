using System;
using System.Collections.Generic;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// BitAuto 的摘要说明
    /// </summary>
    [Serializable]
    public class Exhibition
    {
        private int m_ID;
        private string m_Name;
        private int m_Status;
        private Dictionary<int, Model.Pavilion> m_PavilionList;
        private Dictionary<int, int[]> m_MasterBrandList;
        private Dictionary<int, Model.Attribute> m_AttributeList;
        private string m_XmlContent;

        /// <summary>
        /// 展会ID
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        /// <summary>
        /// 展会名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// 展会状态
        /// </summary>
        public int Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
        /// <summary>
        /// 展馆列表
        /// </summary>
        public Dictionary<int, Model.Pavilion> PavilionList
        {
            get { return m_PavilionList; }
            set { m_PavilionList = value; }
        }
        /// <summary>
        /// 主品牌列表
        /// </summary>
        public Dictionary<int, int[]> MasterBrandList
        {
            get { return m_MasterBrandList; }
            set { m_MasterBrandList = value; }
        }
        /// <summary>
        /// 展会属性列表
        /// </summary>
        public Dictionary<int, Model.Attribute> AttributeList
        {
            get { return m_AttributeList; }
            set { m_AttributeList = value; }
        }
        /// <summary>
        /// XML内容
        /// </summary>
        public string XmlContent
        {
            get { return m_XmlContent; }
            set { m_XmlContent = value; }
        }
    }
}
