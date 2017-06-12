using System;
using System.Collections.Generic;
using System.Text;

using BitAuto.CarChannel.Model;

namespace BitAuto.CarChannel.Common
{
    public class CompareMethod
    {
        /// <summary>
        /// 比较变速器类型
        /// </summary>
        /// <param name="trans1"></param>
        /// <param name="trans2"></param>
        /// <returns></returns>
        public static int CompareTransmissionType(string trans1, string trans2)
        {
            if (trans1.IndexOf("手动") > -1)
                trans1 = "a";
            else if (trans1.IndexOf("自动") > -1)
                trans1 = "b";
            else if (trans1.IndexOf("手自一体") > -1)
                trans1 = "c";
            else if (trans1.IndexOf("CVT") > -1)
                trans1 = "d";
            else trans1 = "e";



            if (trans2.IndexOf("手动") > -1)
                trans2 = "a";
            else if (trans2.IndexOf("自动") > -1)
                trans2 = "b";
            else if (trans2.IndexOf("手自一体") > -1)
                trans2 = "c";
            else if (trans2.IndexOf("CVT") > -1)
                trans2 = "d";
            else
                trans2 = "e";

            return String.Compare(trans1, trans2);
        }

        /// <summary>
        /// 比较排量信息
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static int CompareExhaust(string e1, string e2)
        {
            int ret = 0;
            e1 = e1.ToUpper().Replace("L", "");
            e2 = e2.ToUpper().Replace("L", "");
            double ex1 = 0;
            double ex2 = 0;
            Double.TryParse(e1, out ex1);
            Double.TryParse(e2, out ex2);
            if (ex1 > ex2)
                ret = 1;
            else if (ex1 < ex2)
                ret = -1;
            else
                ret = 0;
            return ret;
        }

        /// <summary>
        /// 比较指数项
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static int CompareIndexItem(IndexItem item1, IndexItem item2)
        {
            if (item1.Index > item2.Index)
                return -1;
            else if (item1.Index < item2.Index)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 对城市指数进行排序
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static int CompareCitySortItem(CitySortItem item1, CitySortItem item2)
        {
            if (item1.IndexNum > item2.IndexNum)
                return -1;
            else if (item1.IndexNum < item2.IndexNum)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// 比较厂商，按首母排序
        /// </summary>
        /// <param name="prd1"></param>
        /// <param name="prd2"></param>
        /// <returns></returns>
        public static int CompareProducerInfo(ProducerInfo prd1, ProducerInfo prd2)
        {
            return String.Compare(prd1.Spell, prd2.Spell);
        }

        //比较主品牌，按首字母排序
        public static int CompareMasterbrandInfo(MasterBrandInfo mInfo1, MasterBrandInfo mInfo2)
        {
            return String.Compare(mInfo1.UrlSpell, mInfo2.UrlSpell);
        }

        /// <summary>
        /// 比较品牌信息
        /// </summary>
        /// <param name="bInfo1"></param>
        /// <param name="bInfo2"></param>
        /// <returns></returns>
        public static int CompareBrandInfo(BrandInfo bInfo1, BrandInfo bInfo2)
        {
            if (bInfo1.Country != "中国" && bInfo2.Country == "中国")
                return 1;
            else if (bInfo1.Country == "中国" && bInfo2.Country != "中国")
                return -1;
            else
                return String.Compare(bInfo1.Name, bInfo2.Name);
        }

    }
}
