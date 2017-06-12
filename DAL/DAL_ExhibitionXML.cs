using System;
using System.Collections.Generic;
using System.Xml;

namespace BitAuto.CarChannel.DAL
{
    /// <summary>
    /// ExhibitionXML 的摘要说明
    /// </summary>
    public class ExhibitionXML
    {
        /// <summary>
        /// 根据XML文件得到车主品牌对象列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int,int[]> GetMasterBrandListByXMLString(string exToCarString)
        {
            if (String.IsNullOrEmpty(exToCarString))
            {
                return null;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(exToCarString);

                XmlNodeList xNodeList = xmlDoc.SelectNodes("root/MasterBrand");
                if (xNodeList.Count < 1)
                {
                    return null;
                }

                Dictionary<int, int[]> masterBrandList = new Dictionary<int, int[]>();
                foreach (XmlNode xNode in xNodeList)
                {
                    if (xNode.ChildNodes.Count < 1)
                    {
                        if (!masterBrandList.ContainsKey(Convert.ToInt32(xNode.Attributes["ID"].InnerText.ToString())))
                        {
                            masterBrandList.Add(Convert.ToInt32(xNode.Attributes["ID"].InnerText.ToString()), new int[0]);
                        }
                        continue;
                    }
                    int[] serialIntList = new int[xNode.ChildNodes.Count];
                    for (int i = 0; i < xNode.ChildNodes.Count; i++)
                    {
                        serialIntList[i] = Convert.ToInt32(xNode.ChildNodes[i].Attributes["ID"].InnerText.ToString());
                    }
                    if (!masterBrandList.ContainsKey(Convert.ToInt32(xNode.Attributes["ID"].InnerText.ToString())))
                    {
                        masterBrandList.Add(Convert.ToInt32(xNode.Attributes["ID"].InnerText.ToString())
                                           , serialIntList);
                    }
                }
                return masterBrandList;
            }
            catch
            {
                return null;
            }
        }
    }
}
