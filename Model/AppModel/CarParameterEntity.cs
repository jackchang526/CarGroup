using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.AppModel
{
    [Serializable]
    public class CarParameterEntity
    {
        /// <summary>
        /// 属性建名称
        /// </summary>
        public string ParamKey { get; set; }
        /// <summary>
        /// 键值项
        /// </summary>
        public List<ParamItemEntity> ItemList { get; set; }
    }
    [Serializable]
    public class ParamItemEntity
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述、价格等
        /// </summary>
        public string Des { get; set; }
    }
    [Serializable]
    public class CarParameterListEntity
    {
        /// <summary>
        /// 属性建名称
        /// </summary>
        public int CarId { get; set; }
        /// <summary>
        /// 键值项
        /// </summary>
        public List<CarParameterEntity> CarParameterList { get; set; }
    }
}
