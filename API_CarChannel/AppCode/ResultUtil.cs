﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BitAuto.CarChannelAPI.Web.AppCode
{

    public static class ResultUtil
    {

        public static string CallBackResult(string callback, string data)
        {
            if (!string.IsNullOrEmpty(callback))
                return string.Format("{0}({1})", callback, data);
            return "";
        }

        public static string SuccessResult(string data)
        {
            string s = "";
            if (!string.IsNullOrEmpty(data))
                s = data.Substring(0, 1);
            else
                s = data = "";
            if (s == "{" || s == "[")
            {
                return string.Format("{{\"code\":1,\"msg\":\"OK\",\"data\":{0}}}", data);
            }
            else
            {
                return string.Format("{{\"code\":1,\"msg\":\"OK\",\"data\":\"{0}\"}}", data);
            }
        }

        public static string ErrorResult(int code, string msg, string data)
        {
            string s = "";
            if (!string.IsNullOrEmpty(data))
                s = data.Substring(0, 1);
            else
                s = data = "";
            if (s == "{" || s == "[")
            {
                return string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"data\":{2}}}", code, msg, data);
            }
            else
            {
                return string.Format("{{\"code\":{0},\"msg\":\"{1}\",\"data\":\"{2}\"}}", code, msg, data);
            }
        }

        public static string PageFormat(int index,int size,int total,string data) { 
            string s = "";
            if (!string.IsNullOrEmpty(data))
                s = data.Substring(0, 1);
            else
                s = data = "";
            if (s == "{" || s == "[")
            {
                return string.Format("{{\"index\":{0},\"size\":{1},\"total\":{2},\"data\":{3}}}", index, size, total, data);
            }
            else
            {
                return string.Format("{{\"index\":{0},\"size\":{1},\"total\":{2},\"data\":\"{3}\"}}", index, size, total, data);
            }
        }

    }
}