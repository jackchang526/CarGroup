using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class News
    {
		private string _FaceTitle;
        private string _Title;
        private DateTime _PublishTime;
        private int _RelatedMainSerialID;
        private string _SerialName;
        private string _PageUrl;
        private string _CategoryName;
        private string _CategoryUrl;
        private int _NewsId;
		private string _SourceName;
		private string _Author;
		private int _CommentNum;
        private int _CityId;
        private string _CarImage;

        /// <summary>
        /// 新闻ID
        /// </summary>
        public int NewsId
        {
            get { return _NewsId; }
            set { _NewsId = value; }
        }
        /// <summary>
        /// 关联的主要子品牌ID
        /// </summary>
        public int RelatedMainSerialID
        {
            get { return _RelatedMainSerialID; }
            set { _RelatedMainSerialID = value; }
        }
        /// <summary>
        /// 关联的主要子品牌名称
        /// </summary>
        public string SerialName
        {
            get { return _SerialName; }
            set { _SerialName = value; }
        }        
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime
        {
            get { return _PublishTime; }
            set { _PublishTime = value; }
        }
        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

		/// <summary>
		/// 短标题 add by chengl Jan.30.2012
		/// </summary>
		public string FaceTitle
		{
			get { return _FaceTitle; }
			set { _FaceTitle = value; }
		}

        /// <summary>
        /// 页面链接
        /// </summary>
        public string PageUrl
        {
            get { return _PageUrl; }
            set { _PageUrl = value; }
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }
        /// <summary>
        /// 分类地址
        /// </summary>
        public string CategoryUrl
        {
            get { return _CategoryUrl; }
            set { _CategoryUrl = value; }
        }

		/// <summary>
		/// 资源名称
		/// </summary>
		public string SourceName
		{
			get { return _SourceName; }
			set { _SourceName = value; }
		}

		/// <summary>
		/// 作者
		/// </summary>
		public string Author
		{
			get { return _Author; }
			set { _Author = value; }
		}

		/// <summary>
		/// 评论数量
		/// </summary>
		public int CommentNum
		{
			get { return _CommentNum; }
			set { _CommentNum = value; }
		}
        /// <summary>
		/// 城市id
		/// </summary>
        public int CityId
		{
            get { return _CityId; }
            set { _CityId = value; }
		}
		/// <summary>
		/// 经销商ID
		/// </summary>
		public int VendorId { get; set; }
        /// <summary>
        /// 图片 add by zhangll Aug.8.22
        /// </summary>
        public string CarImage
        {
            get { return _CarImage; }
            set { _CarImage = value; }
        }
    }
}
