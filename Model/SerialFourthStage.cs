
namespace BitAuto.CarChannel.Model
{
	/// <summary>
	/// 子品牌焦点图的数据
	/// </summary>
	public class SerialFourthStage
	{
        private int _SerialId;
        private string _Title;
        private string _Description;
        private string _ImageUrl;
        private int _OrderId;
        private int _TypeId;
        private int _State;

        /// <summary>
        /// 子品牌ID
        /// </summary>
        public int SerialId
        {
            get { return _SerialId; }
            set { _SerialId = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        /// <summary>
        ///描述
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
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
        /// 类型：0 外观 1 内饰
        /// </summary>
        public int TypeId
        {
            get { return _TypeId; }
            set { _TypeId = value; }
        }
        /// <summary>
        /// 是否有效字段
        /// </summary>
        public int State
        {
            get { return _State; }
            set { _State = value; }
        }
	}
}

