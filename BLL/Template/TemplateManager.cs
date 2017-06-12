using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using System.Web;

using BitAuto.CarChannel.DAL;
using BitAuto.Utils;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// 模板管理类，负责模板的实例化工作
	/// </summary>
	public class TemplateManager
	{
		/// <summary>
		/// 私有构造，外部不能实例化
		/// </summary>
		private TemplateManager()
		{
		}

		/// <summary>
		/// 静态构造
		/// </summary>
		static TemplateManager()
		{
			m_instance = new TemplateManager();
		}

		private static TemplateManager m_instance;		//唯一的实例

		/// <summary>
		/// 返回页面模板的实例
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="paraList">页面参数列表</param>
		/// <returns></returns>
		public PageTemplate GetPageTemplate(int pageId,List<TemplateParameter> paraList,bool isPreview)
		{
			//获取当前页页的所有关联关系
			PTRelation ptr = GetPageRelation(pageId);
			//根据配置获取模板ID
			
			int templateId = ptr.MatchTemplateByParameters(paraList);

			return GetPageTemplateById(templateId, paraList, isPreview);
		}

		/// <summary>
		/// 根据模板ID
		/// 获取模板
		/// </summary>
		/// <param name="templateId"></param>
		/// <returns></returns>
		public PageTemplate GetPageTemplateById( int templateId, List<TemplateParameter> paraList,bool isPreview)
		{
			//先取模板的半成品
			string cacheKey = "StuffTemplate_" + templateId;
			StuffTemplate stuffTemplate = (StuffTemplate)CacheManager.GetCachedData( cacheKey );
			if (stuffTemplate == null)
			{
				stuffTemplate = new StuffTemplate();
				stuffTemplate.InitializeStuffTemplate(templateId,isPreview);

				//预览用的模板不要缓存，也不可以缓存，会影响正式的模板内容
				if(!isPreview)
					CacheManager.InsertCache( cacheKey, stuffTemplate, 3 );
			}

			//生成模板
			return stuffTemplate.CreateTemplate();
		}

		public static TemplateManager GetManager()
		{
			return m_instance;
		}

		/// <summary>
		/// 根据模板的数据获取模块实例
		/// </summary>
		/// <param name="segNode"></param>
		/// <returns></returns>
		internal static ModuleBase GetModuleById(int modId,bool isPreview)
		{
			ModuleBase module = null;
			DataSet modData = TemplateDal.GetModuleDataById(modId, isPreview);
			if (modData == null || modData.Tables.Count == 0 || modData.Tables[0].Rows.Count == 0)
				return null;
			DataRow modRow = modData.Tables[0].Rows[0];
			ModuleType moduleType = (ModuleType)ConvertHelper.GetInteger(modRow["ModuleType"]);
			string moduleName = modRow["Name"].ToString().Trim();
			string contentXml = modRow["ModuleContent"].ToString().Trim();
			switch (moduleType)
			{
				case ModuleType.Include:
					module = new IncludeModule(moduleName);
					break;
				case ModuleType.Js:
					module = new ScriptModule(moduleName);
					break;
				case ModuleType.TemplateModule:
					module = new TemplateModule(moduleName);
					break;
				case ModuleType.Program:
					module = new ProgramModule(moduleName);
					break;
			}
			module.InitModule(contentXml);
			return module;
		}

		/// <summary>
		/// 获取页面关联数据
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		private PTRelation GetPageRelation(int pageId)
		{
			string cacheKey = "pageRelation_" + pageId;
			PTRelation ptr = (PTRelation)CacheManager.GetCachedData(cacheKey);
			if(ptr == null)
			{
				DataSet ds = TemplateDal.GetTemplateRelationDataByPageId(pageId);
				ptr = new PTRelation(pageId);
				if(!ds.Tables.Contains("Page") || ds.Tables["Page"].Rows.Count == 0)
					throw(new Exception("Not found current page data:" + pageId + "!"));

				//默认模板
				DataRow pageRow = ds.Tables["Page"].Rows[0];
				ptr.DefaultTemplateId = ConvertHelper.GetInteger(pageRow["TemplateID"]);

				//关联定制
				if (ds.Tables.Contains("PTRelation"))
				{
					//按行分析数据
					foreach(DataRow row in ds.Tables["PTRelation"].Rows)
					{
						int tempId = ConvertHelper.GetInteger(row["templateid"]);
						if (tempId == 0)
							continue;
						if (!ptr.RelationDic.ContainsKey(tempId))
							ptr.RelationDic[tempId] = new Relation();
						//模板的定制关联
						Relation rela = ptr.RelationDic[tempId];
						string paraName = ConvertHelper.GetString(row["paramname"]);
						string group = ConvertHelper.GetString(row["relationgroup"]);
						string[] valueList = ConvertHelper.GetString(row["paramvalue"]).Split(new char[]{'#'}, StringSplitOptions.RemoveEmptyEntries);

						if (!rela.ContainsKey(group))
							rela[group] = new Dictionary<string, List<string>>();
						//关联组
						Dictionary<string, List<string>> grpRela = rela[group];

						//参数是否存在
						if (!grpRela.ContainsKey(paraName))
							grpRela[paraName] = new List<string>();

						//将参数值加入值列表
						foreach (string paraValue in valueList)
							grpRela[paraName].Add(paraValue);
					}
				}

				CacheManager.InsertCache(cacheKey, ptr, WebConfig.CachedDuration);
			}


			return ptr;
		}


	}
}
