using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class AllSerialName : InterfacePageBase
	{
		private StringBuilder sb = new StringBuilder();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				GetAllSerialName();
			}
		}

		private void GetAllSerialName()
		{
			DataSet ds = new DataSet();

			if (HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial") != null)
			{
				ds = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial");
			}
			else
			{
				ds = base.GetAllCSForDLXXInterface();
				HttpContext.Current.Cache.Insert("InterfaceForBitautoAllSerial", ds, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
			}

			Hashtable htPic = new Hashtable();
			//DataSet dsPic = base.GetAllSerialPicAndCount();
			//if (dsPic != null && dsPic.Tables.Count > 0 && dsPic.Tables[0].Rows.Count > 0)
			//{
			//    for (int i = 0; i < dsPic.Tables[0].Rows.Count; i++)
			//    {
			//        if (!htPic.ContainsKey(dsPic.Tables[0].Rows[i]["SerialId"].ToString()))
			//        {
			//            htPic.Add(dsPic.Tables[0].Rows[i]["SerialId"].ToString(), dsPic.Tables[0].Rows[i]["ImageId"].ToString());
			//        }
			//    }
			//}

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialInfo>");
			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					sb.Append("<Serial csid=\"" + ds.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					sb.Append(" OldCbId=\"" + ds.Tables[0].Rows[i]["OldCb_Id"].ToString() + "\" ");
					sb.Append(" csName=\"" + ds.Tables[0].Rows[i]["cs_name"].ToString().Replace("&", "&amp;") + "\" ");
					sb.Append(" csOtherName=\"" + ds.Tables[0].Rows[i]["cs_OtherName"].ToString().Replace("&", "&amp;") + "\" ");
					sb.Append(" csEName=\"" + ds.Tables[0].Rows[i]["cs_EName"].ToString() + "\" ");
					sb.Append(" csShowName=\"" + ds.Tables[0].Rows[i]["cs_ShowName"].ToString().Replace("&", "&amp;") + "\" ");
					sb.Append(" SEOName=\"" + ds.Tables[0].Rows[i]["cs_seoname"].ToString().Trim().Replace("&", "&amp;") + "\" ");
					//if (htPic != null && htPic.Count > 0 && htPic.ContainsKey(ds.Tables[0].Rows[i]["cs_id"].ToString()))
					//{
					//    sb.Append(" CommonClassId=\"" + Convert.ToString(htPic[ds.Tables[0].Rows[i]["cs_id"].ToString()]) + "\" ");
					//}
					//else
					//{
					sb.Append(" CommonClassId=\"\" ");
					//}
					// sb.Append(" CommonClassId=\"" + ds.Tables[0].Rows[i]["CommonClassId"].ToString() + "\" ");
					sb.Append(" CsSpell=\"" + ds.Tables[0].Rows[i]["spell"].ToString() + "\" ");
					sb.Append(" CpCountry=\"" + ds.Tables[0].Rows[i]["CpCountry"].ToString() + "\" ");
					sb.Append(" AllSpell=\"" + ds.Tables[0].Rows[i]["allSpell"].ToString().Trim().ToLower() + "\" />");
				}
			}
			sb.Append("</SerialInfo>");
			Response.Write(sb.ToString());
		}
	}
}