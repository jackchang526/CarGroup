using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace H5Web.Test.Broker
{
	public partial class Content : System.Web.UI.Page
	{
		private string filePathForBroker = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Broker\\Broker.txt";

		protected void Page_Load(object sender, EventArgs e)
		{
			// 加载数据
			if (!this.IsPostBack)
			{
				// 初始化数据
				IntiDate();
			}
		}

		private void IntiDate()
		{
			IntiBroker();
		}

		private void IntiBroker()
		{
			IntiTextBoxByFile(this.TextBoxForBroker, filePathForBroker);
		}

		/// <summary>
		/// 根据文件初始化TextBox
		/// </summary>
		/// <param name="tb"></param>
		/// <param name="fileName"></param>
		private void IntiTextBoxByFile(TextBox tb, string fileName)
		{
			if (File.Exists(fileName))
			{
				tb.Text = File.ReadAllText(fileName);
			}
		}

		protected void ButtonForBroker_Click(object sender, EventArgs e)
		{
			UpdateTextBoxToFile(this.TextBoxForBroker, filePathForBroker);
		}

		/// <summary>
		/// 根据 textBox 更新文件
		/// </summary>
		/// <param name="tb"></param>
		/// <param name="fileName"></param>
		private void UpdateTextBoxToFile(TextBox tb, string fileName)
		{
			bool isError = false;
			if (tb.Text != null)
			{
				FileStream fs = null;
				StreamWriter sw = null;
				try
				{
					fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
					sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
					sw.Write(tb.Text);
				}
				catch (Exception ex)
				{
					isError = true;
					CommonFunction.WriteLog(string.Format("{0} Error:{1}", tb.ClientID, ex.ToString()));
				}
				finally
				{
					sw.Close();
					fs.Close();
				}

				if (isError)
				{
					JavaScriptResponse("updateCarlist", "alert('更新失败,请联系chengl.')");
				}
				else
				{
					JavaScriptResponse("updateCarlist", "alert('更新成功.')");
				}
				IntiDate();
			}
		}

		private void JavaScriptResponse(string key, string script)
		{
			if (!this.IsClientScriptBlockRegistered(key))
			{
				this.RegisterClientScriptBlock(key, "<script>" + script + "</script>");
			}

		}
	}
}