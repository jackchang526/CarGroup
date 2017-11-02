using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 车型选配包
    /// </summary>
    [Serializable]
    public class CarSerialPackageEntity
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 车型ID
        /// </summary>
        public int SerialId { get; set; }
        /// <summary>
        /// 年款
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 选配包名称
        /// </summary>
        public string PackageName { get; set; }
        /// <summary>
        /// 选配包价格
        /// </summary>
        public float PackagePrice { get; set; }
        /// <summary>
        /// 选配包描述
        /// </summary>
        public string PackageDescription { get; set; }
        /// <summary>
        /// 选配包相关车款
        /// </summary>
        public List<int> CarIds { get; set; }
    }
}
