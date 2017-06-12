using System;
using System.Data;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.CarchannelWeb.App_Code;

namespace BitAuto.CarChannel.CarchannelWeb.interfaceforbitauto
{
	public partial class SerialInfo : InterfacePageBase
	{
		private int csID = 0;
		private int oldCsID = 0;
		private StringBuilder sb = new StringBuilder();
		private bool isNeedComment = false;
		private bool isNeedAllSerial = false;
		//private DataSet dsPic;
		private int balance = 1;
		private string csLevel = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this.CheckSerialID();
				this.GetAllSerialInfo();
			}
		}

		// 接收子品牌参数
		private void CheckSerialID()
		{
			if (this.Request.QueryString["csID"] != null && this.Request.QueryString["csID"].ToString() != "")
			{
				string tempCsID = this.Request.QueryString["csID"].ToString();
				try
				{
					csID = int.Parse(tempCsID);
				}
				catch
				{ }
			}
			else if (this.Request.QueryString["oldCsID"] != null && this.Request.QueryString["oldCsID"].ToString() != "")
			{
				string tempOldCsID = this.Request.QueryString["oldCsID"].ToString();
				try
				{
					oldCsID = int.Parse(tempOldCsID);
				}
				catch
				{ }
			}
			// 是否显示注释
			if (this.Request.QueryString["isNeedComment"] != null && this.Request.QueryString["isNeedComment"].ToString() != "")
			{
				isNeedComment = true;
			}
			// 是否显示全部子品牌
			if (this.Request.QueryString["isNeedAll"] != null)
			{
				isNeedAllSerial = true;
				// 显示全部子品牌时不显示注释
				isNeedComment = false;
			}
		}

		// 根据ID取子品牌信息
		private void GetAllSerialInfo()
		{
			if (isNeedAllSerial)
			{
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial") != null)
				{
					Response.Write(HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial").ToString());
					return;
				}
			}
			else if (this.Context.Cache[Request.Path + oldCsID.ToString()] != null)
			{
				Response.Write(this.Context.Cache[Request.Path + oldCsID.ToString()].ToString());
				return;
			}

			sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.Append("<SerialInfo>");
			if (isNeedComment)
			{
				sb.Append("<!--请求根节点开始-->");
			}
			if (isNeedAllSerial)
			{
				//DataSet ds = base.GetAllCSForInterface();
				//if (ds != null && ds.Tables[0].Rows.Count > 0)
				//{
				//    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				//    {
				//        this.GetDataForSerial(int.Parse(ds.Tables[0].Rows[i]["cs_id"].ToString()), 0, ref sb);
				//    }
				//}
			}
			else
			{
				this.GetDataForSerial(csID, oldCsID, ref sb);
			}
			sb.Append("</SerialInfo>");
			if (isNeedAllSerial)
			{
				if (HttpContext.Current.Cache.Get("InterfaceForBitautoAllSerial") == null)
				{
					HttpContext.Current.Cache.Insert("InterfaceForBitautoAllSerial", sb.ToString(), null, DateTime.Now.AddHours(1), TimeSpan.Zero);
				}
			}
			else
			{
				if (oldCsID != 0)
				{
					HttpContext.Current.Cache.Insert(Request.Path + oldCsID.ToString(), sb.ToString(), null, DateTime.Now.AddHours(1), TimeSpan.Zero);
				}
			}
			Response.Write(sb.ToString());
		}

		private void GetDataForSerial(int _csID, int _oldCsID, ref StringBuilder _sb)
		{
			//dsPic = base.GetAllSerialPicAndCount();
			XmlDocument imgDoc = base.GetAllSerialConverImgAndCount();
			DataSet ds = null;
			if (_csID > 0)
			{
				ds = base.GetSerialInfoByIDForInterface(true, _csID);
			}
			else if (_oldCsID > 0)
			{
				ds = base.GetSerialInfoByIDForInterface(false, _oldCsID);
			}
			if (ds != null && ds.Tables[0].Rows.Count > 0)
			{
				csLevel = ds.Tables[0].Rows[0]["cs_CarLevel"].ToString();
				_sb.Append("<CurrentSerialInfo CsID=\"" + ds.Tables[0].Rows[0]["cs_id"].ToString() + "\" ");
				_sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[0]["cs_Name"].ToString()) + "\" ");
				_sb.Append(" OldCBID=\"" + ds.Tables[0].Rows[0]["oldcb_id"].ToString() + "\" ");
				//if (dsPic != null && dsPic.Tables.Count > 1 && dsPic.Tables[1].Rows.Count > 0)
				//{
				//	DataRow[] drsPic = dsPic.Tables[1].Select(" SerialId = '" + ds.Tables[0].Rows[0]["cs_id"].ToString() + "' ");
				//	if (drsPic != null && drsPic.Length > 0)
				//	{
				//		_sb.Append(" ImageCommonId=\"" + drsPic[0]["ImageId"].ToString() + "\" ");
				//	}
				//	else
				//	{
				//		_sb.Append(" ImageCommonId=\"\" ");
				//	}
				//}
				if (imgDoc != null)
				{
					XmlNode imgNode = imgDoc.SelectSingleNode("/SerialList/Serial[@SerialId='" + ds.Tables[0].Rows[0]["cs_id"].ToString() + "']");
					string imgid = string.Empty;
					if (imgNode != null)
					{
						imgid = imgNode.Attributes["ImageId"].Value;
					}
					_sb.Append(" ImageCommonId=\"" + imgid + "\"");
				}
				else
				{
					_sb.Append(" ImageCommonId=\"\" ");
				}
				// _sb.Append(" ImageCommonId=\"" + ds.Tables[0].Rows[0]["CommonClassId"].ToString() + "\" ");
				_sb.Append(" Virtue=\"" + ds.Tables[0].Rows[0]["cs_Virtues"].ToString().Replace("\"", "'").Replace("<", "《").Replace(">", "》") + "\" ");
				_sb.Append(" Defect=\"" + ds.Tables[0].Rows[0]["cs_Defect"].ToString().Replace("\"", "'").Replace("<", "《").Replace(">", "》") + "\" ");

				string defaultPic = "";
				int picCount = 0;
				base.GetSerialPicAndCountByCsID(int.Parse(ds.Tables[0].Rows[0]["cs_id"].ToString()), out defaultPic, out picCount, false);
				_sb.Append(" CsImage=\"" + defaultPic + "\"/>");
				//string imgUrl = ds.Tables[0].Rows[0]["SiteImageUrl"].ToString();
				//if (imgUrl != "")
				//{
				//    int imgId = int.Parse(ds.Tables[0].Rows[0]["SiteImageId"].ToString());
				//    string img = base.GetPublishImage(1, imgUrl, imgId);
				//    _sb.Append(" CsImage=\"" + img + "\"/>");
				//}
				//else
				//{
				//    _sb.Append(" CsImage=\"\" />");
				//}
				if (isNeedComment)
				{
					_sb.Append("<!--子品牌信息开始(CsID:子品牌ID,CsName:子品牌名,OldCBID:对应老库品牌ID,CsImage:子品牌图片)-->");
				}
				_sb.Append("<CurrentCB CbID=\"" + ds.Tables[0].Rows[0]["cb_id"].ToString() + "\" ");
				_sb.Append(" CbName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[0]["cb_name"].ToString()) + "\" ");
				_sb.Append(" OldCsID=\"" + ds.Tables[0].Rows[0]["oldcs_id"].ToString() + "\"/>");
				if (isNeedComment)
				{
					_sb.Append("<!--品牌信息(CbID:品牌ID,CbName:品牌名,OldCsID:对应老库系列ID)-->");
				}
				_sb.Append("<CurrentCP CpID=\"" + ds.Tables[0].Rows[0]["cp_id"].ToString() + "\" ");
				_sb.Append(" CpName=\"" + System.Security.SecurityElement.Escape(ds.Tables[0].Rows[0]["cp_name"].ToString()) + "\" ");
				_sb.Append(" OldCpID=\"" + ds.Tables[0].Rows[0]["oldcp_id"].ToString() + "\"/>");
				if (isNeedComment)
				{
					_sb.Append("<!--厂商信息(CpID:厂商ID,CpName:厂商名,OldCpID:对应老库厂商ID)-->");
				}

				if (isNeedComment)
				{
					_sb.Append("<!--子品牌优缺点(Virtue:子品牌优点,Defect:子品牌缺点)-->");
				}
				GetTheSameCarPrice(int.Parse(ds.Tables[0].Rows[0]["cs_id"].ToString()), ref _sb);
			}
		}

		private void GetTheSameCarPrice(int csId, ref StringBuilder _sb)
		{
			_sb.Append("<SameCarPrice>");
			if (isNeedComment)
			{
				_sb.Append("<!--同价位车型信息开始-->");
			}

			DataSet dsCarInfo = base.GetAllCarInfo();
			DataSet dsCarPrice = base.GetAllCarPrice();
			DataSet ds = base.GetAllSerialPrice();
			decimal avg = 0;
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				DataRow[] drs = ds.Tables[0].Select(" Id=" + csId.ToString() + " ");
				if (drs.Length > 0)
				{
					try
					{
						decimal min = Math.Round(decimal.Parse(drs[0]["MinPrice"].ToString()), 2);
						decimal max = Math.Round(decimal.Parse(drs[0]["MaxPrice"].ToString()), 2);
						avg = Math.Round(((min + max) / 2), 2);
						// result = min.ToString() + "万-" + max.ToString() + "万";
					}
					catch
					{ }
				}
			}
			if (avg > 0)
			{
				string csID = ",";
				int loop = 1;
				for (int i = 0; i < dsCarPrice.Tables[0].Rows.Count; i++)
				{
					DataRow[] drOtherCarinfo = dsCarInfo.Tables[0].Select(" car_id=" + dsCarPrice.Tables[0].Rows[i]["Id"].ToString() + " ");
					if (drOtherCarinfo.Length > 0)
					{
						if (drOtherCarinfo[0]["cs_id"].ToString() == csId.ToString() && drOtherCarinfo[0]["cs_CarLevel"].ToString() != csLevel)
						{
							continue;
						}
						if (loop > 4)
						{
							break;
						}
						decimal minOther = Math.Round(decimal.Parse(dsCarPrice.Tables[0].Rows[i]["MinPrice"].ToString()), 2);
						decimal maxOther = Math.Round(decimal.Parse(dsCarPrice.Tables[0].Rows[i]["MaxPrice"].ToString()), 2);
						decimal avgOther = Math.Round(((minOther + maxOther) / 2), 2);
						if (Math.Abs(avg - avgOther) <= balance)
						{
							if (csID.IndexOf("," + drOtherCarinfo[0]["cs_id"].ToString() + ",") >= 0)
							{
								continue;
							}
							csID += drOtherCarinfo[0]["cs_id"].ToString() + ",";
							// alCars.Add(ds.Tables[0].Rows[i]["Id"].ToString());
							_sb.Append("<Car CarID=\"" + drOtherCarinfo[0]["car_id"].ToString() + "\" ");
							_sb.Append(" CarName=\"" + System.Security.SecurityElement.Escape(drOtherCarinfo[0]["car_name"].ToString()) + "\" ");
							_sb.Append(" OldCarID=\"" + drOtherCarinfo[0]["OLdCar_Id"].ToString() + "\" ");
							_sb.Append(" CsID=\"" + drOtherCarinfo[0]["cs_id"].ToString() + "\" ");
							_sb.Append(" CsName=\"" + System.Security.SecurityElement.Escape(drOtherCarinfo[0]["cs_Name"].ToString()) + "\" ");
							_sb.Append(" OldCbID=\"" + drOtherCarinfo[0]["OldCb_Id"].ToString() + "\" ");
							_sb.Append(" CarPrice=\"" + avgOther.ToString() + "\" ");

							string defaultPic = "";
							int picCount = 0;
							base.GetSerialPicAndCountByCsID(int.Parse(drOtherCarinfo[0]["cs_id"].ToString()), out defaultPic, out picCount, false);
							_sb.Append(" CarImage=\"" + defaultPic + "\"/>");
							//string imgUrl = dsAbsPrice.Tables[0].Rows[i]["SiteImageUrl"].ToString();
							//if (imgUrl != "")
							//{
							//    int imgId = int.Parse(dsAbsPrice.Tables[0].Rows[i]["SiteImageId"].ToString());

							//    string img = base.GetPublishImage(1, imgUrl, imgId);

							//    _sb.Append(" CarImage=\"" + img + "\"/>");
							//}
							//else
							//{
							//    _sb.Append(" CarImage=\"\"/>");
							//}
							if (isNeedComment)
							{
								_sb.Append("<!--同价位车型信息(CarID:车型ID,CarName:车型名,OldCarID:对应老库车型ID,CsID:所属子品牌ID,CsName:所属子品牌名,OldCbID:所属子品牌对应老库品牌ID,CarPrice:车型价格,CarImage:车型图片)-->");
							}
							loop++;
						}
					}
				}
			}

			_sb.Append("</SameCarPrice>");

			//DataSet dsCarPrice = new DataSet();
			//if (HttpContext.Current.Cache.Get("InterfaceForBitautoCarPrice") != null)
			//{
			//    dsCarPrice = (System.Data.DataSet)HttpContext.Current.Cache.Get("InterfaceForBitautoCarPrice");
			//}
			//else
			//{
			//    dsCarPrice = base.GetAllCarPriceForInterface(false, 0, 0);
			//    HttpContext.Current.Cache.Insert("InterfaceForBitautoCarPrice", dsCarPrice, null, DateTime.Now.AddHours(12), TimeSpan.Zero);
			//}
			//if (dsCarPrice != null && dsCarPrice.Tables[0].Rows.Count > 0)
			//{
			//    _sb.Append("<SameCarPrice>");
			//    if (isNeedComment)
			//    {
			//        _sb.Append("<!--同价位车型信息开始-->");
			//    }
			//    decimal sumPriceForSerial = 0;
			//    decimal averagePriceForSerial = 0;
			//    int validCount = 0;
			//    DataRow[] drCurrentCS = dsCarPrice.Tables[0].Select(" cs_id = " + csId.ToString());
			//    for (int i = 0; i < drCurrentCS.Length; i++)
			//    {
			//        try
			//        {
			//            sumPriceForSerial += decimal.Parse(drCurrentCS[i]["absValue"].ToString());
			//            validCount++;

			//            // _sb.Append("<aaa aa=\"" + drCurrentCS[i]["AveragePrice"].ToString() + "\"/>");
			//        }
			//        catch
			//        { }
			//    }
			//    if (validCount > 0)
			//    {
			//        averagePriceForSerial = sumPriceForSerial / validCount;
			//        DataSet dsAbsPrice = base.GetAllCarPriceForInterface(true, averagePriceForSerial, csId);
			//        if (dsAbsPrice != null && dsAbsPrice.Tables[0].Rows.Count > 0)
			//        {
			//            int maxCount = dsAbsPrice.Tables[0].Rows.Count > 4 ? 4 : dsAbsPrice.Tables[0].Rows.Count;
			//            for (int i = 0; i < maxCount; i++)
			//            {
			//                _sb.Append("<Car CarID=\"" + dsAbsPrice.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
			//                _sb.Append(" CarName=\"" + dsAbsPrice.Tables[0].Rows[i]["car_name"].ToString() + "\" ");
			//                _sb.Append(" OldCarID=\"" + dsAbsPrice.Tables[0].Rows[i]["OLdCar_Id"].ToString() + "\" ");
			//                _sb.Append(" CsID=\"" + dsAbsPrice.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
			//                _sb.Append(" CsName=\"" + dsAbsPrice.Tables[0].Rows[i]["cs_Name"].ToString() + "\" ");
			//                _sb.Append(" OldCbID=\"" + dsAbsPrice.Tables[0].Rows[i]["OldCb_Id"].ToString() + "\" ");
			//                _sb.Append(" CarPrice=\"" + dsAbsPrice.Tables[0].Rows[i]["AveragePrice"].ToString() + "\" ");

			//                string imgUrl = dsAbsPrice.Tables[0].Rows[i]["SiteImageUrl"].ToString();
			//                if (imgUrl != "")
			//                {
			//                    int imgId = int.Parse(dsAbsPrice.Tables[0].Rows[i]["SiteImageId"].ToString());

			//                    string img = base.GetPublishImage(1, imgUrl, imgId);

			//                    _sb.Append(" CarImage=\"" + img + "\"/>");
			//                }
			//                else
			//                {
			//                    _sb.Append(" CarImage=\"\"/>");
			//                }
			//                if (isNeedComment)
			//                {
			//                    _sb.Append("<!--同价位车型信息(CarID:车型ID,CarName:车型名,OldCarID:对应老库车型ID,CsID:所属子品牌ID,CsName:所属子品牌名,OldCbID:所属子品牌对应老库品牌ID,CarPrice:车型价格,CarImage:车型图片)-->");
			//                }
			//            }
			//        }
			//    }
			//    _sb.Append("</SameCarPrice>");
			//}
		}
	}
}