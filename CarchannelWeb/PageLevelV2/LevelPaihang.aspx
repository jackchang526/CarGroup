<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LevelPaihang.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageLevelV2.LevelPaihang" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=LevelFullName%>_排行榜】-易车网BitAuto.com</title>
    <meta name="Keywords" content="车型数据库, 汽车最新报价,车型导购,汽车评测,汽车新闻,汽车图片,汽车问答,汽车点评，汽车经销商，汽车论坛" />
	<meta name="Description" content="易车网(BitAuto.com) 汽车车型为您提供各种汽车车型所有信息。包括汽车报价、最新报价、汽车图片、汽车参数、汽车配置、汽车资讯、汽车点评、汽车问答、汽车论坛等等……" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    
    <!--#include file="~/ushtml/0000/yiche_2016_cube_qicheguanzhupaihang_style-1352.shtml"-->
    <script type="text/javascript">
        var tagIframe = null;
        var currentTagId = 61; 	//当前页的标签ID
	</script>
</head>
    <body>
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <!--头部开始-->
    <header class="header-main special-header2">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <div class="section-left">
                    <h1 class="logo"><a href="http://www.bitauto.com">易车yiche.com</a></h1>
                    <h2 class="title">网友关注排行</h2>
                </div>
            </div>
            <div class="col-xs-8 right-box">
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
        <div id="navBox">
        <nav class="header-main-nav">
            <div class="container">
                <div class="col-auto left secondary">
                    <ul class="list">
                        <%=LevelHtml %>
                    </ul>
                </div>
            </div>
        </nav>
    </div>
    </header>
    <!--头部结束-->

    <!--排行-->
    <div class="container paihang-section">
        <div class="row head-box">
            <h3 class="title col-auto"><%=LevelFullName %>关注度排行</h3>
            <%--<div class="col-auto city drop-layer-box">
                <a id="currentCity" href="javascript:;" class="arrow-down"><%=CityName %></a>
                <div id="popCity" class="drop-layer">
                    <ul class="layer-txt-list">
                        <%=CityListHtml%>
                    </ul>
                </div>
            </div>
            <div class="col-auto note">7日排行</div>--%>
        </div>
        <div class="ol-list">
            <ol class="list">
                <%=SerialsHtml%>
            </ol>
        </div>
    </div>
    <!--/排行-->
    <!--#include file="~/htmlv2/footer2016.shtml"-->

    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <!-- 艾瑞 -->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>
