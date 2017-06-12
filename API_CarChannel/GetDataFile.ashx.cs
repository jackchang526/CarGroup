using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using BitAuto.CarChannelAPI.Web.AppCode;
using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannelAPI.Web
{
    /// <summary>
    /// 获取数据文件接口
    /// </summary>
    public class GetDataFile : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            PageHelper.SetPageCache(60);

            context.Response.ContentType = "text/xml";

            //api.car.bitauto.com/getdatafile.ashx?key=1 
            //key=1 输出 Data\SerialSet\EditorComment.xml 问答机器人使用
            //暂时不做判断
            string filePath = Path.Combine(WebConfig.DataBlockPath, @"Data\SerialSet\EditorComment.xml");
            if (File.Exists(filePath))
            {
                context.Response.TransmitFile(filePath);
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