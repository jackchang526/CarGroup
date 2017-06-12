using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 树形配置类
    /// </summary>
    [Serializable]
    public class TreeTagConfigEntity
    {
        #region 变量定义
        private Dictionary<string, string> _MainUrl;
        private Dictionary<string, string> _SearchUrl;
        private Dictionary<string, string> _MasterBrandUrl;
        private Dictionary<string, string> _BrandUrl;
        private Dictionary<string, string> _SerialUrl;
        private Dictionary<string, string> _ErrorUrl;
        private string _TagType = string.Empty;
        private string _TagName = string.Empty;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TreeTagConfigEntity()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainUrl">主页地址</param>
        /// <param name="sereachUrl">搜索页面地址</param>
        /// <param name="masterBrandUrl">主品牌页面地址</param>
        /// <param name="brandUrl">品牌页面地址</param>
        /// <param name="serialUrl">搜索地址</param>
        /// <param name="tagType">标签类型</param>
        public TreeTagConfigEntity(string tagType,
                                   string tagName)
        {           
            _TagType = tagType;
            _TagName = tagName;
        }

        #endregion

        #region 公共属性
        /// <summary>
        /// 主页地址
        /// </summary>
        public Dictionary<string, string> MainUrl
        {
            get { return _MainUrl; }
            set { _MainUrl = value; }
        }
        /// <summary>
        /// 搜索地址
        /// </summary>
        public Dictionary<string, string> SearchUrl
        {
            get { return _SearchUrl; }
            set { _SearchUrl = value; }
        }
        /// <summary>
        /// 主品牌地址
        /// </summary>
        public Dictionary<string, string> MasterBrandUrl
        {
            get { return _MasterBrandUrl; }
            set { _MasterBrandUrl = value; }
        }
        /// <summary>
        /// 品牌地址
        /// </summary>
        public Dictionary<string, string> BrandUrl
        {
            get { return _BrandUrl; }
            set { _BrandUrl = value; }
        }
        /// <summary>
        /// 子品牌地址
        /// </summary>
        public Dictionary<string, string> SerialUrl
        {
            get { return _SerialUrl; }
            set { _SerialUrl = value; }
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public Dictionary<string, string> ErrorUrl
        {
            get { return _ErrorUrl; }
            set { _ErrorUrl = value; }
        }
        /// <summary>
        /// 标签地址
        /// </summary>
        public string TagType
        {
            get { return _TagType; }
            set { _TagType = value; }
        }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName
        {
            get { return _TagName; }
            set { _TagName = value; }
        }
        #endregion
    }
}
