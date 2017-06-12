using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.Model
{
    /// <summary>
    /// 品牌级别信息及其级别信息
    /// </summary>
    [Serializable]
    public class Car_MasterBrandEntity
    {
        #region 变量定义
        private int _bs_Id;
        private string _bs_Name;
        private string _bs_OtherName;
        private string _bs_Country;
        private string _bs_EName;
        private string _bs_Logo;
        private string _bs_Logo2;
        private string _bs_LogoInfo;
        private string _bs_spell;
        private string _CreateTime;
        private string _bs_allSpell;
        private string _bs_introduction;
        private string _bs_seoname;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public Car_MasterBrandEntity()
        { }

        public Car_MasterBrandEntity(int bs_Id
                                 , string bs_Name
                                 , string bs_OtherName
                                 , string bs_Country
                                 , string bs_EName
                                 , string bs_Logo
                                 , string bs_Logo2
                                 , string bs_LogoInfo
                                 , string bs_spell
                                 , string CreateTime
                                 , string bs_allSpell
                                 , string bs_introduction
                                 , string bs_seoname)
        {
            _bs_Id = bs_Id;
            _bs_Name = bs_Name;
            _bs_OtherName = bs_OtherName;
            _bs_Country = bs_Country;
            _bs_EName = bs_EName;
            _bs_Logo = bs_Logo;
            _bs_Logo2 = bs_Logo2;
            _bs_LogoInfo = bs_LogoInfo;
            _bs_spell = bs_spell;
            _CreateTime = CreateTime;
            _bs_allSpell = bs_allSpell;
            _bs_introduction = bs_introduction;
            _bs_seoname = bs_seoname;
        }


        #endregion

        #region 属性
        /// <summary>
        /// 主品牌ID
        /// </summary>
        public int Bs_Id
        {
            get { return _bs_Id; }
            set { _bs_Id = value; }
        }
        /// <summary>
        /// 主品牌名称
        /// </summary>
        public string Bs_Name
        {
            get { return _bs_Name; }
            set { _bs_Name = value; }
        }
        /// <summary>
        /// 主品牌别名
        /// </summary>
        public string Bs_OtherName
        {
            get { return _bs_OtherName; }
            set { _bs_OtherName = value; }
        }
        /// <summary>
        /// 主品牌国别
        /// </summary>
        public string Bs_Country
        {
            get { return _bs_Country; }
            set { _bs_Country = value; }
        }
        /// <summary>
        /// 主品牌英文名
        /// </summary>
        public string Bs_EName
        {
            get { return _bs_EName; }
            set { _bs_EName = value; }
        }
        /// <summary>
        /// 主品牌LOGO
        /// </summary>
        public string Bs_Logo
        {
            get { return _bs_Logo; }
            set { _bs_Logo = value; }
        }
        /// <summary>
        /// 主品牌次要LOGO
        /// </summary>
        public string Bs_Logo2
        {
            get { return _bs_Logo2; }
            set { _bs_Logo2 = value; }
        }
        /// <summary>
        /// 主品牌介绍
        /// </summary>
        public string Bs_LogoInfo
        {
            get { return _bs_LogoInfo; }
            set { _bs_LogoInfo = value; }
        }
        /// <summary>
        /// 主品牌拼音缩写
        /// </summary>
        public string Bs_spell
        {
            get { return _bs_spell; }
            set { _bs_spell = value; }
        }
        /// <summary>
        /// 主品牌创建时间
        /// </summary>
        public string CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; }
        }
        /// <summary>
        /// 主品牌全拼，用于做url
        /// </summary>
        public string Bs_allSpell
        {
            get { return _bs_allSpell; }
            set { _bs_allSpell = value; }
        }
        /// <summary>
        /// 主品牌介绍
        /// </summary>
        public string Bs_introduction
        {
            get { return _bs_introduction; }
            set { _bs_introduction = value; }
        }
        /// <summary>
        /// 主品牌Seo名称
        /// </summary>
        public string Bs_seoname
        {
            get { return _bs_seoname; }
            set { _bs_seoname = value; }
        }
        #endregion

    }
}
