﻿using BitAuto.CarChannel.Common;
using BitAuto.CarChannel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.CarChannel.BLL
{
    /// <summary>
    /// 选车工具相关
    /// </summary>
    public class SelectCarToolNewBll
    {
        /// <summary>
        /// 请求选车工具接口
        /// </summary>
        /// <param name="param">参数字段，如果一个参数有多个值，需要拼好</param>
        /// <returns></returns>
        public SelectCarResult GetSelectCarResult(Dictionary<string,string> param)
        {
            string paramStr = string.Empty;
            SelectCarResult selectCarResult = null;
            if (param != null && param.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in param)
                {
                    paramStr = string.Format("&{0}={1}");
                }
            }
            string url = WebConfig.SelectCarUrl;
            if (string.IsNullOrEmpty(url))
            {
                return selectCarResult;
            }
            url = string.Concat(url, paramStr);
            string content = CommonFunction.GetContentByUrl(url);
            if (!string.IsNullOrEmpty(content))
            {
                selectCarResult = JsonHelper.Deserialize<SelectCarResult>(content);
            }
            return selectCarResult;
        }
    }
}
