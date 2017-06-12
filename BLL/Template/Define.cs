using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Template
{
	/// <summary>
	/// Html片段类型
	/// </summary>
	public enum HtmlSegmentType
	{
		/// <summary>
		/// 普通的Html
		/// </summary>
		Normal = 1,
		/// <summary>
		/// 模块的Html片段
		/// </summary>
		Module = 2
	}

	/// <summary>
	/// 脚本或CSS节点的类型
	/// </summary>
	public enum ScriptOrCssElementType
	{
		/// <summary>
		/// 未指定
		/// </summary>
		None = 0,
		/// <summary>
		/// 代码片段
		/// </summary>
		Segment = 1,

		/// <summary>
		/// 引用外部的文件
		/// </summary>
		Link = 2
	}

	/// <summary>
	/// Script节点输出的位置
	/// </summary>
	public enum ElementPosition
	{
		/// <summary>
		/// 未定义
		/// </summary>
		None = 0,
		/// <summary>
		/// 页首
		/// </summary>
		PageTop = 1,

		/// <summary>
		/// 页尾
		/// </summary>
		PageEnd = 2,

		/// <summary>
		/// 模块前
		/// </summary>
		ModuleTop = 3,

		/// <summary>
		/// 模块后
		/// </summary>
		ModuleEnd = 4
	}

	/// <summary>
	/// 代码段位置
	/// </summary>
	public enum SegmentPosition
	{
		/// <summary>
		/// 未设置
		/// </summary>
		None = 0,
		/// <summary>
		/// 在Head里
		/// </summary>
		Head = 1,
		/// <summary>
		/// 在Body里
		/// </summary>
		Body = 2
	}

	/// <summary>
	/// 模块类型
	/// </summary>
	public enum ModuleType
	{
		None = 0,
		Include = 1,
		Js = 2,
		TemplateModule = 3,
		Program = 4
	}

	/// <summary>
	/// 模板的关联设置
	/// </summary>
	public class Relation:Dictionary<string,Dictionary<string,List<string>>>
	{
		//Dictionary<关联组,Dictionary<参数名,List<参数值>>>
	}
}
