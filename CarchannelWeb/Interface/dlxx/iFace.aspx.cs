using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.CarchannelWeb.Interface.dlxx
{
	public partial class iFace : OldPageBase
	{
		protected readonly static string xmlFilename = "Interface/dlxx/";

		protected void Page_Load(object sender, EventArgs e)
		{
			string filename = Server.MapPath("brand.xml");

			FileInfo fi = new FileInfo(filename);

			if (IsFileNew(fi, 1))
			{
				Response.Write("文件是最新");
			}
			else
			{
				CreateXml();
				Response.Write("文件最新ok!!");
			}
		}

		private bool CreateXml()
		{
			DataSet dsCB = new DataSet();
			DataSet dsCS = new DataSet();
			DataSet dsCar = new DataSet();

			dsCB = base.GetAllCBForDLXXInterface();
			dsCS = base.GetAllCSForDLXXInterface();
			dsCar = base.GetAllCarForDLXXInterface();

			#region 生成数据
			StringBuilder _sbhead = new StringBuilder();
			StringBuilder _sb = new StringBuilder();

			_sbhead.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			_sbhead.Append("<BitAutoForCarData>");
			_sb.Append(_sbhead);

			if (dsCB != null && dsCB.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < dsCB.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item cb_id=\"" + dsCB.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					_sb.Append(" cb_name=\"" + dsCB.Tables[0].Rows[i]["cb_name"].ToString() + "\" ");
					_sb.Append(" cb_log=\"http://car.bitauto.com/Theme/default/CarImage/b_" + dsCB.Tables[0].Rows[i]["cb_id"].ToString() + "_b.jpg\" />");
				}
			}

			_sb.Append("</BitAutoForCarData>");
			this.createFile("brand.xml", _sb.ToString());
			_sb = new StringBuilder(_sbhead.ToString());

			if (dsCS != null && dsCS.Tables[0].Rows.Count > 0)
			{

				for (int i = 0; i < dsCS.Tables[0].Rows.Count; i++)
				{
					_sb.Append("<Item cs_id=\"" + dsCS.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					_sb.Append(" cb_id=\"" + dsCS.Tables[0].Rows[i]["cb_id"].ToString() + "\" ");
					_sb.Append(" cs_name=\"" + dsCS.Tables[0].Rows[i]["cs_name"].ToString() + "\" ");
					_sb.Append(" cs_infoUrl=\"http://car.bitauto.com/serial/" + dsCS.Tables[0].Rows[i]["cs_id"].ToString() + ".html\" />");
				}

			}

			_sb.Append("</BitAutoForCarData>");
			this.createFile("serial.xml", _sb.ToString());
			_sb = new StringBuilder(_sbhead.ToString());


			if (dsCar != null && dsCar.Tables[0].Rows.Count > 0)
			{

				for (int i = 0; i < dsCar.Tables[0].Rows.Count; i++)
				{

					string imgurl = dsCar.Tables[0].Rows[i]["SiteImageUrl"].ToString();

					string SiteImageId = "_" + dsCar.Tables[0].Rows[i]["SiteImageId"].ToString() + "_1";

					if (imgurl.LastIndexOf('.') > 0)
					{
						string st = imgurl.Substring(imgurl.LastIndexOf('.'));
						imgurl = imgurl.Substring(0, imgurl.LastIndexOf('.'));
						imgurl += SiteImageId + st;
						//imgurl.Insert(imgurl.LastIndexOf('.'), SiteImageId);
					}

					_sb.Append("<Item car_id=\"" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + "\" ");
					_sb.Append(" cs_id=\"" + dsCar.Tables[0].Rows[i]["cs_id"].ToString() + "\" ");
					_sb.Append(" car_name=\"" + dsCar.Tables[0].Rows[i]["car_name"].ToString() + "\" ");
					_sb.Append(" car_imgUrl=\"http://img1.bitauto.com/autoalbum/" + imgurl + "\" ");
					_sb.Append(" car_imgWebUrl=\"http://photo.bitauto.com/car/classalbum-" + dsCar.Tables[0].Rows[i]["CommonClassId"].ToString() + ".html\" ");
					_sb.Append(" car_infoUrl=\"http://car.bitauto.com/car/" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + ".html\" ");
					_sb.Append(" car_reviewUrl=\"http://review.bitauto.com/Car_" + dsCar.Tables[0].Rows[i]["oldcb_id"].ToString() + "/\" ");
					_sb.Append(" car_peizhiUrl=\"http://car.bitauto.com/car/" + dsCar.Tables[0].Rows[i]["car_id"].ToString() + ".html#op\" />");
				}

			}

			_sb.Append("</BitAutoForCarData>");
			this.createFile("car.xml", _sb.ToString());
			_sb = new StringBuilder(_sbhead.ToString());

			return true;

			#endregion
		}

		private void createFile(string _filename, string s)
		{
			FileInfo fi = new FileInfo(Server.MapPath(_filename));
			FileStream fs = fi.Create();
			StreamWriter strw = new StreamWriter(fs, System.Text.Encoding.UTF8);
			strw.Write(s);
			strw.Close();
			fs.Close();
		}

		/// <summary>
		/// 判断文件是否最新
		/// </summary>
		/// <param name="fi"></param>
		/// <param name="days"></param>
		/// <returns></returns>
		private bool IsFileNew(FileInfo fi, int days)
		{
			if (fi.Exists)
			{
				TimeSpan ts = fi.CreationTime - DateTime.Now;
				if (ts.Days < days)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}