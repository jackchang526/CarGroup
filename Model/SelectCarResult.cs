using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 选车工具结果类
    /// </summary>
    public class SelectCarResult
    {
        /// <summary>
        /// 车系数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 车款数量
        /// </summary>
        public int CarNumber { get; set; }

        /// <summary>
        /// 车系详情列表
        /// </summary>
        public List<SelectCarDetailInfo> ResList { get; set; }
    }

    /// <summary>
    /// 车系详情
    /// </summary>
    public class SelectCarDetailInfo
    {
        /// <summary>
        /// 主品牌id
        /// </summary>
        public int MasterId { get; set; }
        /// <summary>
        /// 车系id
        /// </summary>
        public int SerialId { get; set; }
        /// <summary>
        /// 车系显示名称
        /// </summary>
        public int ShowName { get; set; }
        /// <summary>
        /// 全拼
        /// </summary>
        public int AllSpell { get; set; }
        /// <summary>
        /// 符合条件的车款列表
        /// </summary>
        public int CarIdList { get; set; }
        /// <summary>
        /// 符合条件的车款数量
        /// </summary>
        public int CarNum { get; set; }
        /// <summary>
        /// 白底图url
        /// </summary>
        public int ImageUrl { get; set; }
        /// <summary>
        /// 指导价区间
        /// </summary>
        public int PriceRange { get; set; }
    }
}
