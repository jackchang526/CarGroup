﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DealerMapDetial.aspx.cs" Inherits="H5Web.V4.Dealers.DealerMapDetial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>经销商地图</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
	<meta content="yes" name="apple-mobile-web-app-capable" />
	<meta content="black" name="apple-mobile-web-app-status-bar-style" />
	<meta content="telephone=no" name="format-detection" />
</head>
<body>
	<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
	<% if (DealerId > 0)
	{ %>
	<script type="text/javascript">
		var cityId = bit_locationInfo.cityId;
		//经销商车款列表
		document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=dealer&method=dealermapdetail&csid=<%= SerialId %>&dealerid=<%=DealerId %>' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<script type="text/javascript">
		$("#map_list").attr("href", "/<%=BaseSerialEntity.AllSpell%>/dealer/<%=DealerId%>/#page9");
	</script>
	<%}
	else
	{ %>
	<script type="text/javascript">
		var cityId = bit_locationInfo.cityId;
		//经销商车款列表
		document.write(unescape("%3Cscript src='/handlers/GetDataAsynV3.ashx?service=dealer&method=userdealermapdetail&csid=<%= SerialId %>&cityid=<%=CityId%>' type='text/javascript'%3E%3C/script%3E"));
	</script>
	<script type="text/javascript">
		$("#map_list").attr("href", "/<%=BaseSerialEntity.AllSpell%>/#page9");
	</script>
	<%} %>
</body>
</html>
