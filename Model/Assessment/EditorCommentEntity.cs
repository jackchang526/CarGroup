using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model.Assessment
{
    /// <summary>
    /// 编辑点评
    /// </summary>
    public class EditorCommentEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int IdNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// 点评人
        /// </summary>
        public string Editor { get; set; }
        /// <summary>
        /// 编辑登录名称
        /// </summary>
        public string EditorAccount { get; set; }
        /// <summary>
        /// 点评内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 优点
        /// </summary>
        public string[] Good { get; set; }
        /// <summary>
        /// 缺点
        /// </summary>
        public string[] Bad { get; set; }        
    }
}
