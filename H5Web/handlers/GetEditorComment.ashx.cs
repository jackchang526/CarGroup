using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Common.Cache;
using BitAuto.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace H5Web.handlers
{
	/// <summary>
	///     GetEditorComment 的摘要说明
	/// </summary>
	public class GetEditorComment : H5PageBase, IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			base.SetPageCache(60);

			context.Response.ContentType = "application/json; charset=utf-8";

			if (context.Request.QueryString["csid"] == null && string.IsNullOrEmpty(context.Request.QueryString["csid"]))
			{
				return;
			}

			var serialId = ConvertHelper.GetInteger(context.Request.QueryString["csid"]);

			var cacheKey = string.Format("H5V3_GetEditorComment_{0}", serialId);

			var obj = CacheManager.GetCachedData(cacheKey);

			if (obj != null)
			{
				context.Response.Write(obj);
			}
			else
			{
				#region
				string serializeObject = "";
				if (serialId > 0)
				{
					var xmlPath = Path.Combine(WebConfig.DataBlockPath,
					string.Format(@"Data\Koubei\SerialKouBei\{0}.xml", serialId));

				    #region 

				    if (File.Exists(xmlPath))
				    {
				        var xDoc = XDocument.Load(xmlPath);

				        var root = xDoc.Root;

				        #region 

				        if (root != null)
				        {
				            var editorCommentorys = root.Descendants("EditorCommentory");
				            var editorCommentorysList = from item in editorCommentorys
				                select new
				                {
				                    Name = item.Element("Name").Value,
				                    SquarePhoto = item.Element("SquarePhoto").Value,
				                    Comment = item.Element("Comment").Value,

				                    //item.Descendants("Cars").First()
				                    //Cars = from car in item.Descendants("Cars") select new
				                    //{
				                    //    YearType = car.Element("YearType").Value,
				                    //    CarName = car.Element("CarName").Value
				                    //},

				                    Car = new
				                    {
				                        YearType = item.Descendants("Cars").First().Element("YearType").Value,
				                        CarName = item.Descendants("Cars").First().Element("CarName").Value
				                    }
				                };

				            var scoreVal = "0";
				            var score = root.Descendants("Score");
				            scoreVal = score.Elements("Rating").First().Value;

				            var fuleVal = "0";
				            var fule = root.Elements("FuleValue");
				            fuleVal = fule.First().Value;

				            var koubeiImpression = root.Descendants("KoubeiImpression");
				            var xElements = koubeiImpression as XElement[] ?? koubeiImpression.ToArray();
				            var goods = xElements.Descendants("Good");
				            var gooditems = goods.Descendants("Item");
				            var goodList = from item in gooditems
				                select new
				                {
				                    VoteCount = item.Element("VoteCount").Value,
				                    Keyword = item.Element("Keyword").Value
				                };
				            var bad = xElements.Descendants("Bad");
				            var baditems = bad.Descendants("Item");
				            var badList = from item in baditems
				                select new
				                {
				                    VoteCount = item.Element("VoteCount").Value,
				                    Keyword = item.Element("Keyword").Value
				                };
				            var koubeiImpressionList = new {goods = goodList, bad = badList};

				            var commentArtics = root.Descendants("CommentArtics");
				            var xattr = root.Element("CommentArtics").Attribute("TopicCount");
				            var topicCount = xattr.Value;

				            var artics = commentArtics.Descendants("Artic");
				            var commentArticList = from item in artics
				                select new
				                {
				                    TrimYear = item.Element("TrimYear").Value,
				                    TrimName = item.Element("TrimName").Value,
				                    Fuel = item.Element("Fuel").Value,
				                    Mileage = item.Element("Mileage").Value,
				                    TopicId = item.Element("TopicId").Value,
				                    Title = item.Element("Title").Value,
				                    Contents = item.Element("Contents").Value,
				                    CreateTime = Convert.ToDateTime(item.Element("CreateTime").Value),
				                    UserId = item.Element("UserId").Value,
				                    UserType = item.Element("UserType").Value,
				                    UserName = item.Element("UserName").Value,
				                    UserImage = item.Element("UserImage").Value,
				                    UserUrl = item.Element("UserUrl").Value,
				                    KoubeiUrl = item.Element("KoubeiUrl").Value
				                };


				            var timeConverter = new IsoDateTimeConverter(); //这里使用自定义日期格式，默认是ISO8601格式

				            timeConverter.DateTimeFormat = "yyyy-MM-dd"; //设置时间格式 
                            Car_SerialBll csb = new Car_SerialBll();
                            Dictionary<int, CsKoubeiBaseInfo> dic = csb.GetAllCsKoubeiBaseInfo();
				            Dictionary<string, decimal> dicSubKoubei=new Dictionary<string, decimal>();

                            if (dic.ContainsKey(serialId))
				            {
				                dicSubKoubei = dic[serialId].DicSubKoubei;
				            }


				            serializeObject = JsonConvert.SerializeObject(new
				            {
				                editorComment = editorCommentorysList,
				                koubeiImpression = koubeiImpressionList,
				                artic = commentArticList,
				                score = scoreVal,
				                TopicCount = topicCount,
				                GuestFuelCost = fuleVal,
                                koubei= dicSubKoubei
                            }, Formatting.Indented, timeConverter);
				            
				        }

				        #endregion
				    }

				    #endregion
				}
				CacheManager.InsertCache(cacheKey, serializeObject, WebConfig.CachedDuration);
				context.Response.Write(serializeObject);
				#endregion
			}
			context.Response.End();
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}