using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BitAuto.CarChannel.Model
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
	public class SerialForum
	{
		public SerialForum()
		{
		}
		/// <summary>
		/// 子品牌id
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 论坛id
		/// </summary>
		public int FgId { get; set; }
		/// <summary>
		/// 论坛地址
		/// </summary>
		public string Url { get; set; }
		/// <summary>
		/// 主品牌id
		/// </summary>
		public int BsId { get; set; }
		/// <summary>
		/// 品牌id
		/// </summary>
		public int CbId { get; set; }
		/// <summary>
		/// 发帖数
		/// </summary>
		public int TopicCount { get; set; }
		/// <summary>
		/// 回帖数
		/// </summary>
		public int PostCount { get; set; }
		/// <summary>
		/// 论坛名
		/// </summary>
		public string forumName { get; set; }

		/// <summary>
		/// 获取列表
		/// key=子品牌id，value=SerialForum对象
		/// </summary>
		public static Dictionary<int, SerialForum> GetSerialForumListForXmlReader(XmlReader xmlReader)
		{
			if (xmlReader == null)
				return null;

			Dictionary<int, SerialForum> result = new Dictionary<int, SerialForum>();
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.ToLower() == "row")
				{
					SerialForum serialForum = GetSerialForumForXmlReader(xmlReader.ReadSubtree());
					if (serialForum.Id > 0 && !result.ContainsKey(serialForum.Id))
						result.Add(serialForum.Id, serialForum);
				}
			}
			return result;
		}
		/// <summary>
		/// 获取SerialForum对象
		/// </summary>
		/// <param name="rowReader"></param>
		/// <returns></returns>
		private static SerialForum GetSerialForumForXmlReader(XmlReader rowReader)
		{
			SerialForum result = new SerialForum();
			while (rowReader.Read())
			{
				if (rowReader.NodeType != XmlNodeType.Element)
					continue;
				switch (rowReader.Name.ToLower())
				{
					case "id":
						result.Id = Convert.ToInt32(rowReader.ReadString());
						break;
					case "fgid":
						result.FgId = Convert.ToInt32(rowReader.ReadString());
						break;
					case "bs_id":
						result.BsId = Convert.ToInt32(rowReader.ReadString());
						break;
					case "cb_id":
						result.CbId = Convert.ToInt32(rowReader.ReadString());
						break;
					case "topiccount":
						result.TopicCount = Convert.ToInt32(rowReader.ReadString());
						break;
					case "postcount":
						result.PostCount = Convert.ToInt32(rowReader.ReadString());
						break;
					case "url":
						result.Url = rowReader.ReadString().Trim();
						break;
					case "forumname":
						result.forumName = rowReader.ReadString().Trim();
						break;;
				}
			}
			return result;
		}
	}
}
