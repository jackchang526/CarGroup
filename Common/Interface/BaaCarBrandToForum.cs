using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.Common.Cache;
using System.Xml;

namespace BitAuto.CarChannel.Common.Interface
{
	/// <summary>
	/// 子品牌对应论坛信息
	/// http://api.baa.bitauto.com/CarBrandToForumUrl.aspx
	/// <contetns>
	///		<row>
	///			<id>2618</id>
	///			<fgid>57</fgid>
	///			<url>http://baa.bitauto.com/f3/</url>
	///			<bs_id>15</bs_id>
	///			<cb_id>10007</cb_id>
	///			<TopicCount>6780</TopicCount>
	///			<PostCount>48134</PostCount>
	///			<forumName>比亚迪F3</forumName>
	///		</row>
	///		...
	///	</contetns>	
	/// </summary>
	public static class BaaCarBrandToForum
	{
		/// <summary>
		/// 子品牌对应论坛信息
		/// key=子品牌id，value=SerialForum对象
		/// 因为接口中存在一个子品牌对应多个论坛的情况，所以字典中只记录了第一个
		/// </summary>
		public static Dictionary<int, SerialForum> GetSerialForumList()
		{
			string cacheKey = "BaaCarBrandToForum_GetSerialForumList";
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj == null)
			{
				XmlReader reader = null;
				try
				{
					reader = XmlReader.Create(WebConfig.BaaCarBrandToForumUrl);
					obj = SerialForum.GetSerialForumListForXmlReader(reader);
				}
				catch {}
				finally
				{
					if (reader != null && reader.ReadState != ReadState.Closed)
						reader.Close();
				}
				if (obj == null)
					obj = new Dictionary<int, SerialForum>();
				CacheManager.InsertCache(cacheKey, obj, WebConfig.CachedDuration);
			}
			return obj as Dictionary<int, SerialForum>;
		}
	}
}
