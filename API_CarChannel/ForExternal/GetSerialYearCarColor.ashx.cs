using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using BitAuto.CarChannel.DAL;
using BitAuto.CarChannelAPI.Web.AppCode;


namespace BitAuto.CarChannelAPI.Web.ForExternal
{
	/// <summary>
	/// GetSerialYearCarColor 的摘要说明
	/// </summary>
	public class GetSerialYearCarColor : IHttpHandler
	{
		private Dictionary<string, Dictionary<string, string>> colorDic = new Dictionary<string, Dictionary<string, string>>();

		public void ProcessRequest(HttpContext context)
		{
			PageHelper.SetPageCache(100);
			GetColor();
			context.Response.ContentType = "Text/XML";
			context.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			ColorXml(context.Response);
		}

		//<ColorInfo serialId="" year="" OutStat_BodyColor="," InStat_InteriorColor="" />
		private void GetColor()
		{

			DataSet ds = SerialDal.GetSerialYearCarColor();

			if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string dicKey = Convert.ToString(row["cs_id"]) + "_" + Convert.ToString(row["caryear"]);
					string paramid = Convert.ToString(row["paramid"]);
					string dicValue = Convert.ToString(row["pvalue"]);
					if (colorDic.ContainsKey(dicKey))
					{
						Dictionary<string, string> valueDic = colorDic[dicKey];
						string[] values = dicValue.Split(',');
						foreach (string value in values)
						{
							if ((!valueDic.ContainsKey(value + "_" + paramid)))
								valueDic.Add(value + "_" + paramid, paramid);
						}
						dicValue = colorDic[dicKey] + dicValue;
						colorDic[dicKey] = valueDic;
					}
					else
					{
						Dictionary<string, string> valueDic = new Dictionary<string, string>();
						string[] values = dicValue.Split(',');
						foreach (string value in values)
						{
							if ((!valueDic.ContainsKey(value + "_" + paramid)))
								valueDic.Add(value + "_" + paramid, paramid);
						}
						colorDic.Add(dicKey, valueDic);
					}
				}
			}
		}

		private void ColorXml(HttpResponse response)
		{
			if (colorDic.Count > 0)
			{
				response.Write("<AllColor>");
				foreach (KeyValuePair<string, Dictionary<string, string>> color in colorDic)
				{
					Dictionary<string, string> paramcolor = color.Value;
					string bodyColor = "";
					string interiorColor = "";
					foreach (KeyValuePair<string, string> param in paramcolor)
					{
						if (Convert.ToInt32(param.Value) == 598)
							bodyColor += param.Key.Split('_')[0] + ",";
						else if (Convert.ToInt32(param.Value) == 801)
							interiorColor += param.Key.Split('_')[0] + ",";
					}
					if (bodyColor.Length > 0)
						bodyColor = bodyColor.Substring(0, bodyColor.Length - 1);
					if (interiorColor.Length > 0)
						interiorColor = interiorColor.Substring(0, interiorColor.Length - 1);
					string[] keys = color.Key.Split('_');
					response.Write("<ColorInfo serialId=\"" + keys[0] + "\" year=\"" + keys[1] + "\" BodyColor=\"" + bodyColor + "\" InteriorColor=\"" + interiorColor + "\"/>");
				}
				response.Write("</AllColor>");
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}