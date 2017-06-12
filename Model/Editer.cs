using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    public class Editer
    {
        private int _UserId;
        private string _UserName;
        private string _UserLoginName;
        private string _UserBlogUrl;
		private int _CityId;
		private string _CityName;
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        /// <summary>
        /// 用户登陆名
        /// </summary>
        public string UserLoginName
        {
            get { return _UserLoginName; }
            set { _UserLoginName = value; }
        }
        /// <summary>
        /// 用户博客地址
        /// </summary>
        public string UserBlogUrl
        {
            get { return _UserBlogUrl; }
            set { _UserBlogUrl = value; }
        }
		/// <summary>
		/// 用户ID
		/// </summary>
		public int CityId
		{
			get { return _CityId; }
			set { _CityId = value; }
		}
		/// <summary>
		/// 用户博客地址
		/// </summary>
		public string CityName
		{
			get { return _CityName; }
			set { _CityName = value; }
		}
    }
}
