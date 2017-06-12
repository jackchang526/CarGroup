using System;
using System.Text;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using System.Security.Cryptography;

namespace BitAuto.CarChannelAPI.Web.Cooperation
{
	/// <summary>
	/// WeiXinConfig 的摘要说明
	/// </summary>
	public class WeiXinConfig : IHttpHandler
	{
		private HttpRequest request;
		private HttpResponse response;
		/// <summary>
		/// 请求Token接口
		/// </summary>
		private static string getTokenURL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
		/// <summary>
		/// 通过Token 取jsapi_ticket 接口
		/// </summary>
		private static string getTicketURL = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

		private static readonly string cacheKey = "WeiXin_LastTicket";
		private static readonly string AppID = "wx0c56521d4263f190";
		private static readonly string AppSecret = "fe2d5753c1589627e42f0305b3630300";
		private WeiXinAccessToken wxAT = new WeiXinAccessToken();
		private WeiXinJsapiTicket wxJT = new WeiXinJsapiTicket();
		private string callback = string.Empty;
		private string urlForSha1 = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			request = context.Request;
			response = context.Response;
			response.ContentType = "application/x-javascript";
			GetParam();
			if (!string.IsNullOrEmpty(callback) && callback.Length <= 15
				&& !string.IsNullOrEmpty(urlForSha1))
			{
				GetWeiXinTokenAndTicket();
				GenerateSignatureContent();
			}
			CommonFunction.WriteInvokeLog("share", string.Format("[WeiXinConfig] url :{0}", request.Url));
			response.End();
		}

		private void GetParam()
		{
			callback = BitAuto.Utils.ConvertHelper.GetString(request.QueryString["callback"]);
			if (!string.IsNullOrEmpty(request.QueryString["url"])
				&& request.QueryString["url"].IndexOf("yiche.com") > 0
				&& request.QueryString["url"].IndexOf("yiche.com") < 50)
			{
				urlForSha1 = request.QueryString["url"].ToString();
			}
		}

		/// <summary>
		/// 生成 wx.config timestamp，nonceStr，signature
		/// </summary>
		private void GenerateSignatureContent()
		{
			if (!string.IsNullOrEmpty(callback) && !string.IsNullOrEmpty(urlForSha1) && !string.IsNullOrEmpty(wxJT.ticket))
			{
				string noncestr = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
				string jsapi_ticket = string.IsNullOrEmpty(wxJT.ticket) ? "" : wxJT.ticket;
				int timestamp = CommonFunction.ConvertDateTimeInt(DateTime.Now);
				string url = urlForSha1;
				string signature = CommonFunction.GenerateSHA1Hash(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}"
					, jsapi_ticket, noncestr, timestamp, url)).ToLower();
				response.Write(string.Format("{0}({{\"yiche.timestamp\":{1},\"yiche.nonceStr\":\"{2}\",\"yiche.signature\":\"{3}\"}})"
					, callback, timestamp, noncestr, signature));
			}
			else
			{
				response.Write("{}");
			}
		}

		/// <summary>
		/// 取微信有效 Token and Ticket 2000秒一次，每天限制2000次
		/// </summary>
		private void GetWeiXinTokenAndTicket()
		{
			// 过期时间
			bool hasCacheAndCanUsed = false;
			object cacheWeiXinTicket = CacheManager.GetCachedData(cacheKey);
			if (cacheWeiXinTicket != null)
			{
				wxJT = (WeiXinJsapiTicket)cacheWeiXinTicket;
				hasCacheAndCanUsed = true;
			}
			else
			{
				GetLastWeiXinTokenAndTicketInDB();
			}
			// 如果Ticket的到期时间还有至少10分钟 则不用请求新的Token 和 Ticket
			if (wxJT.ExpiresTime.AddMinutes(-10) >= DateTime.Now)
			{
				// Ticket 还能用
				hasCacheAndCanUsed = hasCacheAndCanUsed & true;
			}
			else
			{
				hasCacheAndCanUsed = false;
				WebClient wcToken = new WebClient();
				string tokenStr = wcToken.DownloadString(string.Format(getTokenURL, AppID, AppSecret));
				// string tokenStr = "{\"errcode\":40013,\"errmsg\":\"invalid appid\"}";
				// string tokenStr = "{\"access_token\":\"FR3Gg_UUn9ebtxHwWjUjzmEh5JiXXSbb45UvADuPLCIvFDNJ92cxL_6hwwEFn3fbECj4ngzOIBVPnk2OFDD8uHhFOrF7DgIs8lbIV1dmtfE\",\"expires_in\":7200}";
				wxAT = JsonHelper.Deserialize<WeiXinAccessToken>(tokenStr);
				CommonFunction.WriteInvokeLog("share", string.Format("[WeiXin] token:{0}", tokenStr));
				if (string.IsNullOrEmpty(wxAT.access_token) || wxAT.expires_in <= 0)
				{
					// 微信返回token异常
					CommonFunction.WriteInvokeLog("share", string.Format("[WeiXin] token error :{0}", tokenStr));
					return;
				}

				WebClient wcTicket = new WebClient();
				string ticketStr = wcToken.DownloadString(string.Format(getTicketURL, wxAT.access_token));
				// string ticketStr = "{\"errcode\":0,\"errmsg\":\"ok\",\"ticket\":\"bxLdikRXVbTPdHSM05e5u5sUoXNKd8-41ZO3MhKoyN5OfkWITDGgnr2fwJ0m9E8NYzWKVZvdVtaUgWvsdshFKA\",\"expires_in\":7200}";
				wxJT = JsonHelper.Deserialize<WeiXinJsapiTicket>(ticketStr);
				CommonFunction.WriteInvokeLog("share", string.Format("[WeiXin] ticket:{0}", ticketStr));
				if (string.IsNullOrEmpty(wxJT.ticket) || wxJT.expires_in <= 0)
				{
					// 微信返回ticket异常
					CommonFunction.WriteInvokeLog("share", string.Format("[WeiXin ticket] error :{0}", ticketStr));
					return;
				}
				wxJT.ExpiresTime = DateTime.Now.AddSeconds(wxJT.expires_in);
				// 取回的Token和Ticket入库
				InsertWeiXinTokenAndTicketToDB();
			}
			if (!hasCacheAndCanUsed)
			{
				CacheManager.InsertCache(cacheKey, wxJT, 5);
			}
		}

		/// <summary>
		/// 返回上一个数据库里的  JsapiTicket 的过期时间
		/// </summary>
		/// <returns></returns>
		private void GetLastWeiXinTokenAndTicketInDB()
		{
			string sql = "SELECT top 1 [Token],[TokenExpiresSecond] ,[JsapiTicket],[JsapiTicketExpiresSecond],[DateTime] FROM [WeiXin_TokenAndTicket] order by [AutoID] desc";
			DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(WebConfig.DefaultConnectionString, CommandType.Text, sql);
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{

				wxJT.ticket = ds.Tables[0].Rows[0]["JsapiTicket"].ToString().Trim();
				wxJT.expires_in = BitAuto.Utils.ConvertHelper.GetInteger(ds.Tables[0].Rows[0]["JsapiTicketExpiresSecond"].ToString());
				DateTime dtLastTicket = BitAuto.Utils.ConvertHelper.GetDateTime(ds.Tables[0].Rows[0]["DateTime"].ToString());
				wxJT.ExpiresTime = dtLastTicket.AddSeconds(wxJT.expires_in);

				//wxAT.access_token = ds.Tables[0].Rows[0]["Token"].ToString().Trim();
				//wxAT.expires_in = BitAuto.Utils.ConvertHelper.GetInteger(ds.Tables[0].Rows[0]["TokenExpiresSecond"].ToString());
				//DateTime dtLast = BitAuto.Utils.ConvertHelper.GetDateTime(ds.Tables[0].Rows[0]["DateTime"].ToString());
				//wxAT.ExpiresTime = dtLast.AddSeconds(wxAT.expires_in);
			}
		}

		/// <summary>
		/// 微信 Token 入库
		/// </summary>
		private void InsertWeiXinTokenAndTicketToDB()
		{
			if (!string.IsNullOrEmpty(wxAT.access_token) && wxAT.expires_in > 0
				&& !string.IsNullOrEmpty(wxJT.ticket) && wxJT.expires_in > 0)
			{
				string sqlInsert = "INSERT INTO [WeiXin_TokenAndTicket] ([Token],[TokenExpiresSecond],[JsapiTicket],[JsapiTicketExpiresSecond],[DateTime]) VALUES (@Token,@TokenExpiresSecond,@JsapiTicket,@JsapiTicketExpiresSecond,getdate())";
				SqlParameter[] param = { 
										   new SqlParameter("@Token", SqlDbType.VarChar), 
										   new SqlParameter("@TokenExpiresSecond", SqlDbType.Int) ,
										   new SqlParameter("@JsapiTicket", SqlDbType.VarChar) ,
										   new SqlParameter("@JsapiTicketExpiresSecond", SqlDbType.Int) 
									   };
				param[0].Value = wxAT.access_token;
				param[1].Value = wxAT.expires_in;
				param[2].Value = wxJT.ticket;
				param[3].Value = wxJT.expires_in;
				int res = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(WebConfig.DefaultConnectionString, CommandType.Text, sqlInsert, param);
				CommonFunction.WriteInvokeLog("share", string.Format("[WeiXin token and ticket into DB] res:{0} token:{1} expires:{2} ticket:{3} expires:{4} ", res, wxAT.access_token, wxAT.expires_in, wxJT.ticket, wxJT.expires_in));
			}
		}


		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// 微信 AccessToken
		/// </summary>
		public class WeiXinAccessToken
		{
			public WeiXinAccessToken()
			{
				ExpiresTime = new DateTime(1900, 1, 1);
			}
			/// <summary>
			/// 微信接口返回json access_token
			/// </summary>
			public string access_token { get; set; }

			/// <summary>
			/// 微信接口返回json expires_in 有效时间 (秒)
			/// </summary>
			public int expires_in { get; set; }

			/// <summary>
			/// token 的到期时间(根据有效时间计算)
			/// </summary>
			public DateTime ExpiresTime { get; set; }
		}

		/// <summary>
		/// 微信 jsapi_ticket
		/// </summary>
		public class WeiXinJsapiTicket
		{
			public WeiXinJsapiTicket()
			{
				ExpiresTime = new DateTime(1900, 1, 1);
			}
			/// <summary>
			/// 微信接口返回json jsapi_ticket
			/// </summary>
			public string ticket { get; set; }

			/// <summary>
			/// 微信接口返回json expires_in 有效时间 (秒)
			/// </summary>
			public int expires_in { get; set; }

			public int errcode { get; set; }

			public string errmsg { get; set; }

			/// <summary>
			/// jsapi_ticket 的到期时间(根据有效时间计算)
			/// </summary>
			public DateTime ExpiresTime { get; set; }
		}

	}

}