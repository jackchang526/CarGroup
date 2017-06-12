using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// H5 子品牌亮点
    /// </summary>
    public class SerialSparkle
    {
        private int _SerialId;
        private int _H5SId;
        private string _Name;
        private string _ImageUrl;
        private int _OrderId;
        private int _IsState;

        /// <summary>
        /// 子品牌ID
        /// </summary>
        public int SerialId
        {
            get { return _SerialId; }
            set { _SerialId = value; }
        }

        /// <summary>
        /// 亮点ID
        /// </summary>
        public int H5SId
        {
            get { return _H5SId; }
            set { _H5SId = value; }
        }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string ImageUrl
        {
            get { return _ImageUrl; }
            set { _ImageUrl = value; }
        }
        /// <summary>
        /// 顺序编号
        /// </summary>
        public int OrderId
        {
            get { return _OrderId; }
            set { _OrderId = value; }
        }

        /// <summary>
        /// 是否有效字段
        /// </summary>
        public int IsState
        {
            get { return _IsState; }
            set { _IsState = value; }
        }
    }
}
