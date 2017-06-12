<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_Error.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTree.CX_Error" %>
<% Response.ContentType = "text/html"; %>
<%@ OutputCache Duration="600" VaryByParam="*" Location="Any" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
   <!--#include file="~/ushtml/car/bitauto_car_treeview.shtml" -->
    <script type="text/javascript">
    	var _SearchUrl = '<%=this._SearchUrl %>';
    </script>
</head>
<body>
<!--头部导航-->
<div class="treeNavv1">
	<%=NavbarHtml%>
</div>
<!--左侧树形-->
<div id="leftTreeBox" class="treeBoxv1">
</div>
<!--右侧内容-->
<div class="treeMainv1">
        <!-- 出错信息 -->
        <div class="line_box c0623_01">
            <div class="error_page">
                <h1>
                    抱歉，车型信息未找到！</h1>
                <p>
                </p>
                <h4>
                    建议您：</h4>
                <ul>
                    <li>在左侧树形菜单中或通过下方工具来找到您需要的车型内容</li></ul>
            </div>
            <div class="clear">
            </div>
        </div>
        <div style="border-bottom:1px solid #DEE3E7" id="tempNoborder" class="line_box c0622_01">
            <h3 class="car"><span>按条件选车</span></h3>
        <%=this._ConditionsHtml %>
        <div class="clear"></div>           
        </div>
		<script type="text/javascript">
			var params = {};
			params.tagtype = "chexing";
 		</script>
		<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/lefttree.js?v=20130319" ></script>
        <%--<script type="text/javascript" src="http://car.bitauto.com/interface/tree/lefttree.js?tagtype=chexing" ></script>--%>
		<!-- 工具条 -->
        <!--#include file="~/html/toolbarForCartree.shtml"-->
          <!-- footer -->
        <!--#include file="~/html/treefooter.shtml"-->
    </div>
	<!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>

