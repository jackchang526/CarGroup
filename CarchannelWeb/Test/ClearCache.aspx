<%@ Page Language="C#" AutoEventWireup="true" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <title>清除缓存</title>
</head>
<body>
    <script runat="server" language="C#">
        protected void Page_Load(object sender, EventArgs e)
        {
            string key = Request["key"];
            if (!string.IsNullOrWhiteSpace(key))
            {
                if (BitAuto.CarChannel.Common.Cache.CacheManager.IsCachedExist(key))
                {
                    if (BitAuto.CarChannel.Common.Cache.CacheManager.RemoveCachedData(key))
                    {
                        Response.Write("清除缓存成功");
                    }
                    else
                    {
                        Response.Write("清除缓存失败");
                    }
                }
                else
                {
                    Response.Write("缓存不存在");
                }
            }
            else
            {
                Response.Write("关键字不能为空");
            }
        }
       
    </script>
</body>
</html>
