using System;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.BLL;
using System.Data;
using System.Text;

namespace BitAuto.CarChannel.CarchannelWeb.Test
{
	public partial class Perfmon : System.Web.UI.Page
	{
		protected int serialId1 = 0;
		protected int serialId2 = 0;

		string cacheKey = "Car_SerialCompareForPK_New";

		protected string serialNewCompareHtml = string.Empty;
		protected void Page_Load(object sender, EventArgs e)
		{
			serialId1 = ConvertHelper.GetInteger(Request.QueryString["serialid1"]);
			serialId2 = ConvertHelper.GetInteger(Request.QueryString["serialid2"]);
			RenderSeraialNewCompare();
			GetNewCompare();
		}

		private void RenderSeraialNewCompare()
		{
			StringBuilder sb = new StringBuilder();
			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				var dict = (Dictionary<string, SerialNewCompareEntity>)obj;
				foreach (var kv in dict)
				{
					sb.AppendFormat("<li><a href=\"{0}\">{1}vs{2}</a></li>", "", kv.Value.SerialShowName, kv.Value.ToSerialShowName);
				}
			}
			serialNewCompareHtml = sb.ToString();
		}

		private void GetNewCompare()
		{
			string serialCompareKey = (serialId1 < serialId2) ? (serialId1 + "_" + serialId2) : (serialId2 + "_" + serialId1);
			Dictionary<string, SerialNewCompareEntity> dict = new Dictionary<string, SerialNewCompareEntity>();

			object obj = CacheManager.GetCachedData(cacheKey);
			if (obj != null)
			{
				dict = (Dictionary<string, SerialNewCompareEntity>)obj;
				if (!dict.ContainsKey(serialCompareKey))
				{
					UpdateSerialCompare(serialCompareKey, dict);
				}
			}
			else
			{
				UpdateSerialCompare(serialCompareKey, dict);
			}

		}

		public void UpdateSerialCompare(string serialCompareKey, Dictionary<string, SerialNewCompareEntity> dict)
		{
			List<SerialNewCompareEntity> list = new List<SerialNewCompareEntity>();
			var ds = new Car_SerialBll().GetSerialInfoForPK(serialId1, serialId2);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count >= 2)
			{
				var dr1 = ds.Tables[0].Rows[0];
				var dr2 = ds.Tables[0].Rows[1];
				SerialNewCompareEntity entity = new SerialNewCompareEntity();
				entity.SerialShowName = ConvertHelper.GetString(dr1["cs_showname"]);
				entity.AllSpell = ConvertHelper.GetString(dr1["allspell"]);
				entity.ToSerialShowName = ConvertHelper.GetString(dr2["cs_showname"]);
				entity.ToAllSpell = ConvertHelper.GetString(dr2["allspell"]);
				dict.Add(serialCompareKey, entity);
				if (dict.Count > 9) { dict.Remove(dict.Keys.First()); }
				CacheManager.InsertCache(cacheKey, dict, 60 * 24);
			}
		}

		public class SerialNewCompareEntity
		{
			public int SerialId { get; set; }
			public string SerialShowName { get; set; }
			public string AllSpell { get; set; }

			public int ToSerialId { get; set; }
			public string ToSerialShowName { get; set; }
			public string ToAllSpell { get; set; }
		}

	}
}