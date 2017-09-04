using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class EvaluationEntity
    {
        public int CarId { get; set; }
        public int SerialId { get; set; }
        public int EvaluationId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数ID
        /// </summary>
        public int PropertyId { get; set; }
        /// <summary>
        ///参数值
        /// </summary>
        public string PropertyValue { get; set; }

        public string Unit { get; set; }
    }
}
