using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Common.Enum
{
    /// <summary>
    /// 车型新闻类型
    /// </summary>
    [Flags]
    public enum CarNewsType
    {
        wenzhang = 0,//shijia | daogou | yongche | gaizhuang | anquan | xinwen,
        /// <summary>
        /// 焦点新闻配置id
        /// </summary>
        serialfocus = 1,
        /// <summary>
        /// 试驾-(子品牌、年款)
        /// </summary>
        shijia = 2,
        /// <summary>
        /// 改装-(子品牌、年款)
        /// </summary>
        gaizhuang = 3,
        /// <summary>
        /// 安全-(树形)
        /// </summary>
        anquan = 4,
        /// <summary>
        /// 科技-(树形)
        /// </summary>
        keji = 5,
        /// <summary>
        /// 视频-(主品牌、品牌)
        /// </summary>
        video = 6,
        /// <summary>
        /// 级别焦点新闻
        /// </summary>
        leveltopnews = 7,
        /// <summary>
        /// 导购-(树形、主品牌、品牌、子品牌、年款、级别)
        /// </summary>
        daogou = 8,
        /// <summary>
        /// 评测-(子品牌、年款)
        /// </summary>
        pingce = 9,
        /// <summary>
        /// 评测-(树形、主品牌、品牌 、级别)
        /// </summary>
        treepingce = 10,
        /// <summary>
        /// 用车-(主品牌、品牌、子品牌、年款)
        /// </summary>
        yongche = 11,
        /// <summary>
        /// 行情-(树形、主品牌、品牌、子品牌、年款、级别、区域购买)
        /// </summary>
        hangqing = 12,
        /// <summary>
        /// 新闻-(主品牌、品牌、子品牌、年款、级别、厂商)
        /// </summary>
        xinwen = 13,
        /// <summary>
        /// 保养-(子品牌)
        /// </summary>
        baoyang = 14,
        /// <summary>
        /// 置换-(品牌、子品牌)
        /// </summary>
        zhihuan = 15,
        /// <summary>
        /// 话题|文化
        /// </summary>
        huati = 16,
        /// <summary>
        /// 口碑报告
        /// </summary>
        koubei = 17,
        /// <summary>
        /// 文化
        /// </summary>
        wenhua = 18
    }

    public class CarNewsTypeName
    {
        public static string GetCarNewsTypeName(int type)
        {
            string resultStr;
            switch (type)
            {
                case (int)CarNewsType.daogou:
                    resultStr = "导购";
                    break;
                case (int)CarNewsType.shijia:
                    resultStr = "试驾";
                    break;
                case (int)CarNewsType.yongche:
                    resultStr = "用车";
                    break;
                case (int)CarNewsType.gaizhuang:
                    resultStr = "改装";
                    break;
                case (int)CarNewsType.anquan:
                    resultStr = "安全";
                    break;
                case (int)CarNewsType.xinwen:
                    resultStr = "新闻";
                    break;
                case (int)CarNewsType.pingce:
                    resultStr = "评测";
                    break;
                case (int)CarNewsType.treepingce:
                    resultStr = "评测";
                    break;
                default:
                    resultStr = "文章";
                    break;
            }
            return resultStr;
        }
    }
}
