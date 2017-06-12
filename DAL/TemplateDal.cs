using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using BitAuto.CarChannel.Common;
using BitAuto.Utils;
using BitAuto.Utils.Data;

namespace BitAuto.CarChannel.DAL
{
	/// <summary>
	/// 模板的数据获取类
	/// </summary>
	public class TemplateDal
	{
		/// <summary>
		/// 根据页ID获取模板配置
		/// </summary>
		/// <param name="pageId"></param>
		/// <returns></returns>
		public static DataSet GetTemplateRelationDataByPageId(int pageId)
		{
			string sqlStr = "SELECT ID,Name,TemplateID FROM Page WHERE status=1 AND ID=" + pageId;
			sqlStr += "\r\nSELECT templateid,paramname,paramvalue,relationgroup FROM PTrelation WHERE pageid=" + pageId;
			DataSet ds = new DataSet();//SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
			SqlHelper.FillDataset(WebConfig.DefaultConnectionString,CommandType.Text,sqlStr,ds,new string[]{"Page","PTRelation"});
			return ds; 
		}

		/// <summary>
		/// 获取指定模板的模板文件名称
		/// </summary>
		/// <param name="templateId"></param>
		/// <returns></returns>
		[Obsolete("此方法已经不适用！")]
		public static string GetTemplateFilePath(int templateId)
		{
			string sqlStr = "SELECT TemplateFile FROM Template WHERE ID=" + templateId;
			return SqlHelper.ExecuteScalar(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr).ToString();
		}

		/// <summary>
		/// 获取一个模板的模板内容
		/// </summary>
		/// <param name="templateId"></param>
		/// <param name="isPreview"></param>
		/// <returns></returns>
		public static string GetTemplateContent( int templateId,bool isPreview)
		{
			string sqlStr = "SELECT TmpContent FROM Template WHERE ID=" + templateId;
			if (isPreview)
				sqlStr = "SELECT TOP 1 TmpContent FROM TMP_TemplateHistory WHERE TemplateId=" + templateId + "ORDER BY ModifyTime DESC";
			return ConvertHelper.GetString(SqlHelper.ExecuteScalar(WebConfig.DefaultConnectionString,CommandType.Text,sqlStr));
		}

		/// <summary>
		/// 获取一个模块的内容
		/// </summary>
		/// <param name="modId"></param>
		/// <param name="isPreview"></param>
		/// <returns></returns>
		public static DataSet GetModuleDataById( int modId, bool isPreview )
		{
			string sqlStr = "SELECT * FROM TMP_Module WHERE ID=" + modId;
			if (isPreview)
				sqlStr = "SELECT TOP 1 * FROM TMP_ModuleHistory WHERE MoudleId=" + modId + "ORDER BY ModifyTime DESC";
			return SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sqlStr);
		}

		/// <summary>
		/// 获取一个页面的信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DataSet GetPageData( int id )
		{
			string sqlStr = "SELECT * FROM TMP_Page WHERE iD=" + id;
			return SqlHelper.ExecuteDataset( WebConfig.DefaultConnectionString, CommandType.Text, sqlStr );
		}

		/// <summary>
		/// 取指定的关联的参数列表
		/// </summary>
		/// <param name="pageId"></param>
		/// <param name="templateId"></param>
		/// <param name="groupNum"></param>
		/// <returns></returns>
		public static DataSet GetRelationParameter( int pageId, int templateId, int groupNum )
		{
			string sqlStr = "SELECT ParamName,ParamValue FROM TMP_PTrelation WHERE PageId=" + pageId + " AND TemplateId=" + templateId + " AND RelationGroup=" + groupNum;
			return SqlHelper.ExecuteDataset( WebConfig.DefaultConnectionString, CommandType.Text, sqlStr );
		}

	}
}
