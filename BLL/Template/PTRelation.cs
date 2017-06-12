using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Template
{
	public class PTRelation
	{
		private int m_pageId;										//页面ID
		private int m_defaultTemplateId;							//模板ID
		private Dictionary<int, Relation> m_relationDic;			//模板关联字典,<模板ID，关联关系>
		
		/// <summary>
		/// 用页面ID构造
		/// </summary>
		/// <param name="m_pageId">页面ID</param>
		/// <param name="templateId">默认模板ID</param>
		public PTRelation(int pageId,int templateId)
		{
			m_pageId = pageId;
			m_defaultTemplateId = templateId;
			m_relationDic = new Dictionary<int, Relation>();
		}

		/// <summary>
		/// 默认构造
		/// </summary>
		public PTRelation(int pageId)
			: this(pageId,0)
		{
		}

		/// <summary>
		/// 模板ID
		/// </summary>
		public int DefaultTemplateId
		{
			get { return m_defaultTemplateId; }
			set { m_defaultTemplateId = value; }
		}

		/// <summary>
		/// 页面ID
		/// </summary>
		public int PageId
		{
			get { return m_pageId; }
			set { m_pageId = value; }
		}

		/// <summary>
		/// 关联关系
		/// </summary>
		public Dictionary<int,Relation> RelationDic
		{
			get { return m_relationDic; }
		}

		/// <summary>
		/// 计算是否与指定的参数列表匹配
		/// </summary>
		/// <param name="paras"></param>
		/// <returns></returns>
		public int MatchTemplateByParameters(List<TemplateParameter> paras)
		{
			//默认使用默认模板
			int templateId = m_defaultTemplateId;
			if (paras == null)
				return templateId;

			//检查是否有匹配的定制模板
			foreach(int tmpId in m_relationDic.Keys)
			{
				Relation rela = m_relationDic[tmpId];

				bool isMatch = false;
				//检查是否全部参数都匹配
				foreach(TemplateParameter para in paras)
				{
					//按关联组判断
					foreach(string grpName in rela.Keys)
					{
						if (!rela[grpName].ContainsKey(para.Name))
							continue;
						if (!rela[grpName][para.Name].Contains(para.ParameterValue))
						{
							isMatch = false;
							break;
						}
						else
							isMatch = true;
					}

					if (isMatch)
						break;
				}

				//已经匹配
				if(isMatch)
				{
					templateId = tmpId;
					break;
				}
			}

			return templateId;
		}
	}
}
