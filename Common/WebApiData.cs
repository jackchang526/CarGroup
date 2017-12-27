using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BitAuto.CarChannel.Common
{
    public class WebApiData
    {
       
        /// <summary>
        /// 获取HttpWebRequest数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public JObject GetRequestJson(string url, string method, string data = null, Encoding encoding = null, int timeOut = 60000, string contentType = "application/x-www-form-urlencoded;charset=UTF-8")
        {
            string result = GetRequestString(url,method,data,encoding,timeOut,contentType);
            JObject obj = !string.IsNullOrWhiteSpace(result) ? JsonHelper.Deserialize<JObject>(result) : null;
            return obj;
        }
        /// <summary>
        /// 获取HttpWebRequest数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public string GetRequestString(string url, string method, string data = null, Encoding encoding = null, int timeOut = 60000, string contentType = "application/x-www-form-urlencoded;charset=UTF-8")
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string strAdd = string.Empty;
            try
            {
                // System.GC.Collect();
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                if (ServicePointManager.DefaultConnectionLimit < 1024)
                {
                    ServicePointManager.DefaultConnectionLimit = 1024;
                } //设置最大连接数为1024
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";

                request.ContentLength = bytes.Length;
                request.ContentType = contentType;
                request.Timeout = timeOut;
                using (Stream reqstream = request.GetRequestStream())
                {
                    reqstream.Write(bytes, 0, bytes.Length);
                }
                request.Headers.Set("Pragma", "no-cache");
                request.KeepAlive = false;
                request.ServicePoint.Expect100Continue = false;
                response = (HttpWebResponse)request.GetResponse();
                Encoding encodingT = Encoding.UTF8;
                using (Stream streamReceiveT = response.GetResponseStream())
                {
                    using (StreamReader streamReaderT = new StreamReader(streamReceiveT, encodingT))
                    {
                        strAdd = streamReaderT.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return strAdd;
        }
    }
}
