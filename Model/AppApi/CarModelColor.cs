using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppApi
{
    /// <summary>
    /// 车系颜色
    /// </summary>
    public class CarModelColor
    {
        public int OrderId { get; set; }

        public int ModelId { get; set; }

        public DateTime CreateTime { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
        public int ColorType { get; set; }

        public string Value { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
