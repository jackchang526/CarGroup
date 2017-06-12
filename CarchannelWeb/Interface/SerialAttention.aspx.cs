using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.Interface
{
	public partial class SerialAttention : InterfacePageBase
	{
		private int csID = 0;
		private int top = 10;
		private IList<Car_SerialItemEntity> csItem = null;
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckParameter();

				this.GetDate();

				this.GenerateXML();
			}
		}

		// 取参数
		private void CheckParameter()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string cs_id = this.Request.QueryString["csID"].ToString();
				try
				{
					csID = int.Parse(cs_id);
				}
				catch
				{ }
			}

			if (this.Request.QueryString["top"] != null && this.Request.QueryString["top"].ToString() != "")
			{
				string topNum = this.Request.QueryString["top"].ToString();
				try
				{
					top = int.Parse(topNum);
					if (top > 10 || top < 0)
					{
						top = 10;
					}
				}
				catch
				{ }
			}
		}

		// 取数据
		private void GetDate()
		{
			if (HttpContext.Current.Cache.Get("InterfaceForSerial_" + csID.ToString() + "_" + top.ToString()) != null)
			{
				csItem = (IList<Car_SerialItemEntity>)HttpContext.Current.Cache.Get("InterfaceForSerial_" + csID.ToString() + "_" + top.ToString());
			}
			else
			{
				Car_SerialItemBll csib = new Car_SerialItemBll();
				csItem = csib.Get_SerialToSerial(top, csID);
				if (csItem != null && csItem.Count > 0)
				{
					HttpContext.Current.Cache.Insert("InterfaceForSerial_" + csID.ToString() + "_" + top.ToString(), csItem, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
				}
			}
		}

		// 生成XML内容
		private void GenerateXML()
		{
			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<brands>");
			if (csItem != null && csItem.Count > 0)
			{
				for (int i = 0; i < csItem.Count; i++)
				{
					sb.Append("<item>" + csItem[i].cs_Id.ToString() + "</item>");
				}
			}
			sb.Append("</brands>");
			Response.Write(sb.ToString());
		}
	}
}