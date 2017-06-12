using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.CarChannel.Model;
using BitAuto.Utils;

namespace BitAuto.CarChannelAPI.Web.CarInfo
{
	/// <summary>
	/// SerialCompareVote 的摘要说明
	/// </summary>
	public class SerialCompareVote : IHttpHandler
	{
		HttpResponse response;
		HttpRequest request;

 		private string callback = string.Empty;
		private int voteFlag = 0;
		private int[] serialIdArr;
		private string json = string.Empty;
		private string cacheKey = string.Empty;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "application/x-javascript";
			response = context.Response;
			request = context.Request;

			callback = request.QueryString["callback"];
			string serialIds = ConvertHelper.GetString(request.QueryString["csids"]);
			voteFlag = ConvertHelper.GetInteger(request.QueryString["flag"]);

			json = "{{\"{0}\":{1},\"{2}\":{3}}}";

			if (string.IsNullOrEmpty(serialIds) || voteFlag <= 0)
			{
				Jsonp("{}", callback, HttpContext.Current);
				return;
			}

			serialIdArr = serialIds.Split(',').Select(p => ConvertHelper.GetInteger(p)).OrderBy(p => p).ToArray();
			if (serialIdArr.Length != 2)
			{
				Jsonp("{}", callback, HttpContext.Current);
				return;
			}
			cacheKey = string.Format("{0}", string.Join("_", serialIdArr.ToArray()));

			SerialVoteV2();
		}

		private void SerialVoteV2()
		{
			var currentVoteArr = MemCache.GetMemCacheByKey(cacheKey);
			if (currentVoteArr == null)
			{
				int[] newVoteArr = { 0, 0, 0, 0 };
				var baseVoteArr = new SerialCompareVoteBll().GetSerialVote(serialIdArr[0], serialIdArr[1]);
				newVoteArr[2] = baseVoteArr[0];
				newVoteArr[3] = baseVoteArr[1];
				//第一次投票 设置初始值
				if (baseVoteArr[0] == 0 && baseVoteArr[1] == 0)
				{
					newVoteArr[0] = 1;
					newVoteArr[1] = 1;
				}
				// voteFlag 1：小 2：大
				if (voteFlag == 1)
					newVoteArr[0]++;
				else
					newVoteArr[1]++;
				 
				MemCache.SetMemCacheByKey(cacheKey, newVoteArr, (1000 * 60 * 60 * 8));

				json = string.Format(json,
					serialIdArr[0],
					(baseVoteArr[0] + newVoteArr[0]),
					serialIdArr[1],
					(baseVoteArr[1] + newVoteArr[1]));
			}
			else
			{
				var serialsVoteCount = (int[])currentVoteArr;
				var serial1Vote = serialsVoteCount[0];
				var serial2Vote = serialsVoteCount[1];
				if ((serial1Vote + serial2Vote) < 1000)
				{
					if (voteFlag == 1)
						serialsVoteCount[0]++;
					else
						serialsVoteCount[1]++;

					MemCache.SetMemCacheByKey(cacheKey, serialsVoteCount);
				}
				json = string.Format(json, serialIdArr[0],
					(serialsVoteCount[2] + serialsVoteCount[0]),
					serialIdArr[1],
					(serialsVoteCount[3] + serialsVoteCount[1]));
			}
			var obj = CacheManager.GetCachedData(cacheKey);
			if (obj == null)
			{
				CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(UpdateVote);
				HttpRuntime.Cache.Insert(cacheKey, "", null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, callBack);
			}  
			Jsonp(json, callback, HttpContext.Current);
		}
		/// <summary>
		/// 更新数据库 投票增量
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="cirr"></param>
		private void UpdateVote(string key, object value, CacheItemRemovedReason cirr)
		{
			var currentVoteObj = MemCache.GetMemCacheByKey(cacheKey);
			if (currentVoteObj != null)
			{
				var currentVoteArr = (int[])currentVoteObj;
				var success = new SerialCompareVoteBll().UpdateVote(new SerialCompareVoteEntity()
				{
					Serial1 = serialIdArr[0],
					Serial2 = serialIdArr[1],
					VoteCount1 = currentVoteArr[0],
					VoteCount2 = currentVoteArr[1]
				});
				if (success) MemCache.DelMemCacheByKey(cacheKey);
			}
		}

		private void SerialVote()
		{
			//string serialIds = ConvertHelper.GetString(request.QueryString["csids"]);
			//int voteFlag = ConvertHelper.GetInteger(request.QueryString["flag"]);
			//string json = "{{\"{0}\":{1},\"{2}\":{3}}}";

			//if (string.IsNullOrEmpty(serialIds) || voteFlag <= 0)
			//{
			//	Jsonp("{}", callback, HttpContext.Current);
			//	return;
			//}

			//var serialIdArr = serialIds.Split(',').Select(p => ConvertHelper.GetInteger(p)).OrderBy(p => p).ToArray();
			//if (serialIdArr.Length != 2)
			//{
			//	Jsonp("{}", callback, HttpContext.Current);
			//	return;
			//}


			string cacheKey = string.Format("{0}_{1}", string.Join("_", serialIdArr.ToArray()), DateTime.Now.ToString("yyyyMMddHH"));

			string prevCacheKey = string.Format("{0}_{1}", string.Join("_", serialIdArr.ToArray()), DateTime.Now.AddHours(-1).ToString("yyyyMMddHH"));
			var currentVoteArr = MemCache.GetMemCacheByKey(cacheKey);
			if (currentVoteArr == null)
			{
				int[] newVoteArr = { 1, 1, 0, 0 };
				//取上个时间段的数据
				var prevVoteObj = MemCache.GetMemCacheByKey(prevCacheKey);
				if (prevVoteObj != null)
				{
					var prevVoteArr = (int[])prevVoteObj;
					var success = new SerialCompareVoteBll().UpdateVote(new SerialCompareVoteEntity()
					{
						Serial1 = serialIdArr[0],
						Serial2 = serialIdArr[1],
						VoteCount1 = prevVoteArr[0],
						VoteCount2 = prevVoteArr[1]
					});
					if (success) MemCache.DelMemCacheByKey(prevCacheKey);
				}
				var baseVoteArr = new SerialCompareVoteBll().GetSerialVote(serialIdArr[0], serialIdArr[1]);
				newVoteArr[2] = baseVoteArr[0];
				newVoteArr[3] = baseVoteArr[1];

				if (voteFlag == 1)
					newVoteArr[0]++;
				else
					newVoteArr[1]++;


				//缓存1小时20分钟
				MemCache.SetMemCacheByKey(cacheKey, newVoteArr, (1000 * 60 * 60 + 1000 * 60 * 20));

				json = string.Format(json,
					serialIdArr[0],
					(baseVoteArr[0] + newVoteArr[0]),
					serialIdArr[1],
					(baseVoteArr[1] + newVoteArr[1]));
			}
			else
			{
				var serialsVoteCount = (int[])currentVoteArr;
				var serial1Vote = serialsVoteCount[0];
				var serial2Vote = serialsVoteCount[1];
				if ((serial1Vote + serial2Vote) < 1000)
				{
					if (voteFlag == 1)
						serialsVoteCount[0]++;
					else
						serialsVoteCount[1]++;

					MemCache.SetMemCacheByKey(cacheKey, serialsVoteCount);
				}
				json = string.Format(json, serialIdArr[0],
					(serialsVoteCount[2] + serialsVoteCount[0]),
					serialIdArr[1],
					(serialsVoteCount[3] + serialsVoteCount[1]));
			}
			Jsonp(json, callback, HttpContext.Current);
		}

		private void Jsonp(string content, string callbackName, HttpContext context)
		{
			if (string.IsNullOrEmpty(callbackName))
				context.Response.Write(content);
			else
				context.Response.Write(string.Format("{1}({0})", content, callbackName));
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