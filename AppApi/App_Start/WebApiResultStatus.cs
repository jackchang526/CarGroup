using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppApi
{
    public enum WebApiResultStatus
    {
        无权限 = -2,
        未登录 = -1,
        调用失败 = 0,
        成功 = 1,
        参数错误 = 2,
        未找到数据 = 3,
        Token验证失败 = 4,
        无效的身份 = 5,
        已禁言 = 6,
        外部接口调用失败 = 7,
        无效的操作 = 8,
    }
}