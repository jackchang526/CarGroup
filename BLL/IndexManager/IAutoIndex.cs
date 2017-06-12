using System;
using System.Collections.Generic;

using BitAuto.CarChannel.Common;

namespace BitAuto.CarChannel.BLL.IndexManager
{
   public interface IAutoIndex
    {
        /// <summary>
        /// 获取购车指数的前十数据
        /// </summary>
        /// <param name="dateObj">日期数据，用于存储时间，可存储如下结构：年-周、年-季度、年-月</param>
        /// <returns>Dictionary<string, List<IndexItem>></returns>
       Dictionary<string, List<IndexItem>> GetTopListData(DateObj dateObj);
    }
}
