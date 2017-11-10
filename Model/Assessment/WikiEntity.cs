using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    public class WikiEntity
    {
        /// <summary>
        /// 二级项Id
        /// </summary>
        public int EntityId { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
    }
}
