using System;
using System.Collections.Generic;
using System.Text;

using BitAuto.CarChannel.BLL.Template.TemplateException;

namespace BitAuto.CarChannel.BLL.Template
{
	public class TemplateCommon
	{
		/// <summary>
		/// 获取Css代码段
		/// </summary>
		/// <param name="cssList"></param>
		/// <param name="cssPos"></param>
		/// <returns></returns>
		public static string GetCssCode(List<ScriptOrCssElement> cssList,ElementPosition cssPos)
		{
			if (cssList.Count == 0)
				return String.Empty;

			StringBuilder cssCode = new StringBuilder();
			StringBuilder cssLinkCode = new StringBuilder();
			foreach(ScriptOrCssElement css in cssList)
			{
				if (css.ElementPosition != cssPos)
					continue;

				if (css.ElementType == ScriptOrCssElementType.Link)
					cssLinkCode.AppendLine(css.ElementText);
				else if (css.ElementType == ScriptOrCssElementType.Segment)
					cssCode.AppendLine(css.ElementText);
			}
			if (cssCode.Length > 0)
			{
				cssLinkCode.AppendLine("<style type=\"text/css\" >");
				cssLinkCode.AppendLine(cssCode.ToString());
				cssLinkCode.AppendLine("</style>");
			}

			return cssLinkCode.ToString();
		}

		/// <summary>
		/// 获取Css代码段
		/// </summary>
		/// <param name="cssList"></param>
		/// <param name="cssPos"></param>
		/// <returns></returns>
		public static string GetScriptCode(List<ScriptOrCssElement> scriptList, ElementPosition cssPos)
		{
			if (scriptList.Count == 0)
				return String.Empty;

			StringBuilder scriptCode = new StringBuilder();
			StringBuilder scriptLinkCode = new StringBuilder();
			foreach (ScriptOrCssElement css in scriptList)
			{
				if (css.ElementPosition != cssPos)
					continue;

				if (css.ElementType == ScriptOrCssElementType.Link)
					scriptLinkCode.AppendLine(css.ElementText);
				else if (css.ElementType == ScriptOrCssElementType.Segment)
					scriptCode.AppendLine(css.ElementText);
			}
			if (scriptCode.Length > 0)
			{
				scriptLinkCode.AppendLine("<script type=\"text/javascript\">");
				scriptLinkCode.AppendLine(scriptCode.ToString());
				scriptLinkCode.AppendLine("</script>");
			}

			return scriptLinkCode.ToString();
		}

		/// <summary>
		/// 转换为枚举
		/// </summary>
		/// <param name="typeStr"></param>
		/// <returns></returns>
		public static ScriptOrCssElementType ConvertToElementType(string typeStr)
		{
			ScriptOrCssElementType socType = ScriptOrCssElementType.None;
			switch (typeStr)
			{
				case "Link":
					socType = ScriptOrCssElementType.Link;
					break;
				case "Segment":
					socType = ScriptOrCssElementType.Segment;
					break;
			}
			if (socType == ScriptOrCssElementType.None)
				throw (new EnumValueException("Enum value not be found:" + typeStr));
			return socType;
		}

		/// <summary>
		/// 转换为枚举
		/// </summary>
		/// <param name="typeStr"></param>
		/// <returns></returns>
		public static ElementPosition ConvertToPositionType(string typeStr)
		{
			ElementPosition posTyep = ElementPosition.None;
			switch (typeStr)
			{
				case "PageTop":
					posTyep = ElementPosition.PageTop;
					break;
				case "PageEnd":
					posTyep = ElementPosition.PageEnd;
					break;
				case "ModuleTop":
					posTyep = ElementPosition.ModuleTop;
					break;
				case "ModuleEnd":
					posTyep = ElementPosition.ModuleEnd;
					break;
			}
			if (posTyep == ElementPosition.None)
				throw (new EnumValueException("Enum value not be found:" + typeStr));
			return posTyep;
		}
	}
}
