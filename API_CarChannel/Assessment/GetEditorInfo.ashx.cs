using BitAuto.CarChannel.Common;
using BitAuto.CarChannelAPI.Web.AppCode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BitAuto.CarChannelAPI.Web.Assessment
{
    /// <summary>
    /// GetEditorInfo 的摘要说明
    /// </summary>
    public class GetEditorInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "text/plain";
            string callback = context.Request.QueryString["callback"];
            string editorAccount = context.Request.QueryString["editoraccount"];
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data\EidtorUserUrl.xml");
            string UserId = "", UserName = "", UserLoginName = "", UserBlogUrl = "", UserImageUrl = "", SquarePhoto = "", CityId = "", CityName = "";
            if (File.Exists(xmlFile))
            {
                XDocument doc = XDocument.Load(xmlFile);
                var query = from p in doc.Element("Root").Elements("User")
                            where p.Element("UserLoginName").Value == editorAccount
                            select p;
                //假设query只查出一条数据
                query.ToList().ForEach(item =>
                {
                    UserId = item.Element("UserId").Value;
                    UserName = item.Element("ShowName").Value;
                    UserLoginName = item.Element("UserLoginName").Value;
                    UserBlogUrl = item.Element("UserBlogUrl").Value;
                    UserImageUrl = item.Element("Avatar").Value;
                    SquarePhoto = item.Element("SquarePhoto").Value;
                    CityId = item.Element("CityId").Value;
                    CityName = item.Element("CityName").Value;
                });
            }
            var data = new { UserId = UserId, UserName = UserName, UserLoginName = UserLoginName, UserBlogUrl = UserBlogUrl, UserImageUrl = UserImageUrl, SquarePhoto = SquarePhoto, CityId = CityId, CityName = CityName };
            var json = JsonConvert.SerializeObject(data);
            context.Response.Write(!string.IsNullOrEmpty(callback) ? string.Format("{0}({1})", callback, json) : json);
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