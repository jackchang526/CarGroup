using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace H5Web.Test.Dealer
{
	public partial class Content : H5PageBase
	{

		private string filePathForCarList = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealerCar.txt";
		private string filePathForDealer = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealer.txt";
		private string filePathForDealerSale = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealerSale.txt";
		private string filePathForDealerNews = System.Web.HttpContext.Current.Server.MapPath("~") + "\\test\\Dealer\\dealerNews.txt";

		protected void Page_Load(object sender, EventArgs e)
		{
			// 加载数据
			if (!this.IsPostBack)
			{
				// 初始化数据
				IntiDate();
			}
		}

		#region event

		/// <summary>
		/// 更新经销商车款列表
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ButtonForDealerCarList_Click(object sender, EventArgs e)
		{
			UpdateTextBoxToFile(this.TextBoxForDealerCarList, filePathForCarList);
		}

		/// <summary>
		/// 经销商块更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ButtonForDealer_Click(object sender, EventArgs e)
		{
			UpdateTextBoxToFile(this.TextBoxForDealer, filePathForDealer);
		}

		/// <summary>
		/// 经销商还卖块更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ButtonForDealerSale_Click(object sender, EventArgs e)
		{
			UpdateTextBoxToFile(this.TextBoxForDealerSale, filePathForDealerSale);
		}

		/// <summary>
		/// 经销商新闻块更新
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ButtonForDealerNews_Click(object sender, EventArgs e)
		{
			UpdateTextBoxToFile(this.TextBoxForDealerNews, filePathForDealerNews);
		}

		#endregion

		#region private Method

		private void IntiDate()
		{
			// 初始化经销商车款列表
			IntiDealerCarList();
			// 初始化经销商信息
			intiYouHui();
			// 初始化经销商还卖块
			IntiDealerSale();
			// 初始化经销商新闻块
			IntiDealerNews();
		}

		/// <summary>
		/// 初始化经销商信息
		/// </summary>
		private void intiYouHui()
		{
			IntiTextBoxByFile(this.TextBoxForDealer, filePathForDealer);
		}

		/// <summary>
		/// 初始化经销商车款列表
		/// </summary>
		private void IntiDealerCarList()
		{
			IntiTextBoxByFile(this.TextBoxForDealerCarList, filePathForCarList);
		}

		/// <summary>
		/// 初始化经销商还卖块
		/// </summary>
		private void IntiDealerSale()
		{
			IntiTextBoxByFile(this.TextBoxForDealerSale, filePathForDealerSale);
		}

		/// <summary>
		/// 初始化经销商新闻块
		/// </summary>
		private void IntiDealerNews()
		{
			IntiTextBoxByFile(this.TextBoxForDealerNews, filePathForDealerNews);
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

		#endregion



	}
}