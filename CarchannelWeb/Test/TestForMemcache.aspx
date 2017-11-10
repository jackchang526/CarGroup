<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestForMemcache.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Test.TestForMemcache" %>
启用MemCache:<%= BitAuto.CarChannel.Common.WebConfig.IsUseMemcache %><br />
批量:<%= BitAuto.CarChannel.Common.Cache.MemCache.GetMultipleMemCacheByKey(new List<string>() { Request["key"]}) %><br />
单个:<%= BitAuto.CarChannel.Common.Cache.MemCache.GetMemCacheByKey(Request["key"]) %>