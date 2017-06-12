using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Template.TemplateException
{
	/// <summary>
	/// 当前无Http上下文件的异常
	/// </summary>
	public class NoHttpContextException:Exception
	{
		public NoHttpContextException():base("HttpContex.Current为null！")
		{			
		}
		public NoHttpContextException(string msg):base(msg)
		{
		}
		public NoHttpContextException(string msg, Exception innerException) : base(msg, innerException)
		{
		}
	}

	/// <summary>
	/// 文件格式错误异常
	/// </summary>
	public class FileFormatException:Exception
	{
		private string m_filename;
		public string FileName
		{
			get { return m_filename; }
		}
		public FileFormatException(string msg):base(msg)
		{
		}

		public FileFormatException(string msg,Exception innerException):base(msg,innerException)
		{

		}
		public FileFormatException(string msg,string fileName):base(msg)
		{
			m_filename = fileName;
		}
	}

	/// <summary>
	/// 枚举值错误
	/// </summary>
	public class EnumValueException:Exception
	{
		public EnumValueException(string msg):base(msg)
		{
		}

		public EnumValueException(string msg, Exception innerException)
			: base(msg, innerException)
		{

		}
	}
}
