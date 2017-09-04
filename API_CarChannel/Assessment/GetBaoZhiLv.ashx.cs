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
    /// GetBaoZhiLv 的摘要说明
    /// </summary>
    public class GetBaoZhiLv : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);
            context.Response.ContentType = "text/plain";
            string callback = context.Request.QueryString["callback"];
            int SerialId = 0;
            string csid = context.Request.QueryString["csid"];
            int.TryParse(csid, out SerialId);

            //http://car.bitauto.com/data/serialset/baozhilv.xml
            string xmlFile = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialSet\BaoZhiLv.xml");

            string Id = "", ShowName = "", AllSpell = "", Level = "", ResidualRatio1 = "", ResidualRatio2 = "", ResidualRatio3 = "", ResidualRatio4 = "", ResidualRatio5 = "";

            if (File.Exists(xmlFile))
            {
                XDocument doc = XDocument.Load(xmlFile);
                var query = from p in doc.Element("Root").Elements("Serial")
                            where (int)p.Attribute("Id") == SerialId
                            select p;

                //假设query只查出一条数据
                query.ToList().ForEach(item =>
                {
                    Id = item.Attribute("Id").Value;
                    ShowName = item.Attribute("ShowName").Value;
                    AllSpell = item.Attribute("AllSpell").Value;
                    Level = item.Attribute("Level").Value;
                    ResidualRatio1 = item.Attribute("ResidualRatio1").Value;
                    ResidualRatio2 = item.Attribute("ResidualRatio2").Value;
                    ResidualRatio3 = item.Attribute("ResidualRatio3").Value;
                    ResidualRatio4 = item.Attribute("ResidualRatio4").Value;
                    ResidualRatio5 = item.Attribute("ResidualRatio5").Value;
                });
            }

            var data = new { SerialId = Id, ShowName = ShowName, AllSpell = AllSpell, Level = Level, ResidualRatio1 = ResidualRatio1, ResidualRatio2 = ResidualRatio2, ResidualRatio3 = ResidualRatio3, ResidualRatio4 = ResidualRatio4, ResidualRatio5 = ResidualRatio5 };
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