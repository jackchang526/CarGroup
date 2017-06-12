using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Common.Enum
{
    /// 公共枚举类 for 冯津 车款的关键报告 空间部分 的数据接口
    /// </summary>
    public class CommonEnum
    {
        /// <summary>
        /// 车辆内部空间枚举类型
        /// </summary>
        public enum CarInnerSpaceType
        {
            /// <summary>
            ///     第一排座椅
            /// </summary>
            FirstSeatToTop = 0,

            /// <summary>
            ///     第二排座椅
            /// </summary>
            SecondSeatToTop = 1,

            /// <summary>
            ///     第二排座椅距离第一排距离
            /// </summary>
            FirstSeatDistance = 2,

            /// <summary>
            ///     后备箱
            /// </summary>
            BackBoot = 3,

            /// <summary>
            ///     第三排座椅
            /// </summary>
            ThirdSeatToTop = 4
        }
    }
}
