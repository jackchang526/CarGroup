using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.Model
{
	/// <summary>
	/// 车系评测新闻实体
	/// 对应接口：http://api.admin.bitauto.com/news3/v1/news/show?id=5009878
	/// 相关新闻暂时用不着，先不取
	/// </summary>
	public class SerialPingCeNews
	{
		/// <summary>
		/// 新闻id
		/// </summary>
		public int NewsId { get; set; }

		/// <summary>
		/// GUID
		/// </summary>
		public string UniqueId { get; set; }

		/// <summary>
		///  新闻标题
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 短标题
		/// </summary>
		public string ShortTitle { get; set; }

		/// <summary>
		/// 链接地址
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 分类id
		/// </summary>
		public int CategoryId { get; set; }

		/// <summary>
		/// 作者
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		/// 封面图
		/// </summary>
		public string ImageCoverUrl { get; set; }

		/// <summary>
		/// 版权
		/// </summary>
		public int CopyRight { get; set; }
		/// <summary>
		/// 发表时间
		/// </summary>
		public DateTime PublishTime { get; set; }

		/// <summary>
		/// 关联品牌
		/// </summary>
		public int RelateBrand { get; set; }

		/// <summary>
		/// 评论数
		/// </summary>
		public int CommentCount { get; set; }

		/// <summary>
		/// pv
		/// </summary>
		public int Pv { get; set; }

		/// <summary>
		/// 分页新闻内容
		/// </summary>
		public List<SerialPingceNewsPageContent> PageContent { get; set; }

		/// <summary>
		/// 来源
		/// </summary>
		public SerialPingceNewsSource Source { get; set; }

		/// <summary>
		/// 编辑
		/// </summary>
		public SerialPingceNewsEditor Editor { get; set; }

		/// <summary>
		/// 更多图片
		/// </summary>
		public List<string> MoreImagesList { get; set; }

	}

	/// <summary>
	/// 分页新闻内容
	/// </summary>
	public class SerialPingceNewsPageContent
	{
		/// <summary>
		/// 第几页
		/// </summary>
		public int PageIndex { get; set; }

		/// <summary>
		/// 新闻标题
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 新闻内容
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// 链接地址
		/// </summary>
		public string PageUrl { get; set; }
		/// <summary>
		/// 关联车款
		/// </summary>
		public int CarId { get; set; }

		/// <summary>
		/// 关联车系
		/// </summary>
		public int SerialId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int isEstimate { get; set; }
	}

	/// <summary>
	/// 新闻来源
	/// </summary>
	public class SerialPingceNewsSource
	{
		/// <summary>
		/// 来源id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 来源名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 来源url
		/// </summary>
		public string Url { get; set; }
	}

	/// <summary>
	/// 新闻编辑
	/// </summary>
	public class SerialPingceNewsEditor
	{
		/// <summary>
		/// id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 链接
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 显示名称
		/// </summary>
		public string ShowName { get; set; }
		/// <summary>
		/// 图像
		/// </summary>
		public string Photo { get; set; }
		/// <summary>
		/// 编辑类型
		/// </summary>
		public int EditorType { get; set; }
		/// <summary>
		/// 介绍
		/// </summary>
		public string Intro { get; set; }
	}
}
