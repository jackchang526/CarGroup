using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using BitAuto.CarChannel.DAL;
using BitAuto.Utils;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 车型频道页面类
	/// </summary>
	public class CarPage
	{
		private int m_Id;
		private string m_name;
		private string m_memo;
		private int m_defaultTemplateId;
		private int m_pageType;
		private bool m_isNews;
		private int m_createUser;
		private DateTime m_createTime;
		private int m_status;
		private bool m_paraTypeChanged;
		private string m_queryString;

		/// <summary>
		/// 页面ID
		/// </summary>
		public int ID
		{
			get { return m_Id; }
			set { m_Id = value; }
		}
		/// <summary>
		/// 页面名称
		/// </summary>
		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		/// <summary>
		/// 页面说明
		/// </summary>
		public string Memo
		{
			get { return m_memo; }
			set { m_memo = value; }
		}

		/// <summary>
		/// 预览用的参数字符串
		/// </summary>
		public string QueryString
		{
			get { return m_queryString; }
			set { m_queryString = value; }
		}

		/// <summary>
		/// 默认模板ID
		/// </summary>
		public int DefaultTemplateId
		{
			get { return m_defaultTemplateId; }
			set { m_defaultTemplateId = value; }
		}

		/// <summary>
		/// 参数类型
		/// </summary>
		public int PageType
		{
			get { return m_pageType; }
			set
			{
				if (m_pageType != value)
					m_paraTypeChanged = true;
				m_pageType = value;
			}
		}

		/// <summary>
		/// 是否为新闻页
		/// </summary>
		public bool IsNewsPage
		{
			get { return m_isNews; }
			set
			{
				if (m_isNews != value)
					m_paraTypeChanged = true;
				m_isNews = value;
			}
		}

		/// <summary>
		/// 创建人
		/// </summary>
		public int CreateUser
		{
			get { return m_createUser; }
			set { m_createUser = value; }
		}
		/// <summary>
		/// 记录创建时间
		/// </summary>
		public DateTime CreateTime
		{
			get { return m_createTime; }
			set { m_createTime = value; }
		}
		/// <summary>
		/// 记录状态
		/// </summary>
		public int Status
		{
			get { return m_status; }
			set { m_status = value; }
		}

		/// <summary>
		/// 参数类型是否发生了变化
		/// </summary>
		public bool ParameterTypeChanged
		{
			get { return m_paraTypeChanged; }
		}

		public CarPage()
		{
			m_name = String.Empty;
			m_memo = String.Empty;
		}

		/// <summary>
		/// 获取一个页面的信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static CarPage GetCarPage( int id )
		{
			if (id <= 0)
				return null;
			CarPage page = null;
			DataSet ds = TemplateDal.GetPageData( id );
			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				page = new CarPage();
				DataRow row = ds.Tables[0].Rows[0];
				page.ID = ConvertHelper.GetInteger( row["Id"] );
				page.Name = row["Name"].ToString();
				page.Memo = row["Memo"].ToString();
				page.QueryString = row["PreviewQueryString"].ToString();
				page.DefaultTemplateId = ConvertHelper.GetInteger( row["DefaultTemplateId"] );
				page.m_pageType = ConvertHelper.GetInteger( row["PageType"] );
				page.m_isNews = ConvertHelper.GetBoolean( row["IsNews"] );
				page.CreateUser = ConvertHelper.GetInteger( row["CreateUser"] );
			}
			return page;
		}

		public static string GetParameterNameByPageType( int pageType )
		{
			string paraName = String.Empty;
			switch (pageType)
			{
				case 1://厂商
					paraName = "pId";
					break;
				case 2://主品牌
					paraName = "masterId";
					break;
				case 3://品牌
					paraName = "brandId";
					break;
				case 4://级别
					paraName = "levelId";
					break;
				case 5://子品牌
				case 6://年款
					paraName = "serialId";
					break;
				case 7://车型
					paraName = "carId";
					break;
			}
			return paraName;
		}

		/// <summary>
		/// 取指定的关联的参数列表
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="templateId"></param>
		/// <param name="groupNum"></param>
		/// <returns></returns>
		public static Dictionary<string,string> GetRelationParameter( int pageId, int templateId, int groupNum )
		{
			Dictionary<string, string> paraDic = new Dictionary<string, string>();
			DataSet ds = TemplateDal.GetRelationParameter( pageId, templateId, groupNum );
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					paraDic[row["ParamName"].ToString().Trim()] = row["ParamValue"].ToString().Trim();
				}
			}
			return paraDic;
		}
	}
}
