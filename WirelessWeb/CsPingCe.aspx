<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsPingCe.aspx.cs" Inherits="WirelessWeb.CsPingCe" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>
        <%=pageTitle%>
    </title>
    <meta name="Keywords" content="<%=pageKeywords%>" />
    <meta name="Description" content="<%=pageDescription%>" />
    <!--#include file="~/ushtml/0000/myiche2015_cube_wenzhangpindao-1071.shtml"-->
</head>
<body>
    <!-- header start -->
    <div class="op-nav">
        <a id="gobackElm" href="javascript:;" class="btn-return">返回</a>
        <div class="tt-name">
            <a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%= _serialShowName %></h1>
        </div>
        <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
    </div>
    <div class="op-nav-mark" style="display: none;"></div>
    <!-- header end -->
    <%//-- 互联互通 start --%>
    <%=CsHeadHTML %>
    <%//-- 互联互通 end --%>
    <%=_newsNavHtml %>
    <%if (string.IsNullOrEmpty(_newsHtml))
      {%>
    <div class="wrap">
        <div class="m-no-result">
            <div class="face face-fail"></div>
            <dl>
                <dt>暂无文章！</dt>
            </dl>
            <div class="clear"></div>
        </div>
        <a href="javascript:location.href=document.referrer" class="btn-one btn-gray">返回</a>
    </div>
    <%}
      else
      {%>
    <%=_newsHtml %>

    <%} %>
    <script type="text/javascript">
        var url = "http://car.m.yiche.com/<%=_serialAllSpell%>/";
    </script>
    <%// --footer start--%>
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <div class="breadcrumb">
            <div class="breadcrumb-box">
                <a href="http://m.yiche.com/">首页</a> &gt; <a href="http://car.m.yiche.com/brandlist/">选车</a> &gt; <a href="http://car.m.yiche.com/brandlist/<%= _serialEntity.Brand.MasterBrand.AllSpell %>/"><%= _serialEntity.Brand.MasterBrand.Name %></a> &gt; <a href="http://car.m.yiche.com/<%= _serialEntity.AllSpell %>/"><%= _serialEntity.Name %></a> &gt; <span>文章</span>
            </div>
        </div>
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <div class="float-r-box">
        <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
    </div>
    <%// --footer end--%>
    <%if (_currentNewsCount > _pageSize)
      {%>
    <script type="text/javascript">
        var cp = 1;
        var loading = false;
        var jsonp = function (url) {
            var head = document.getElementsByTagName("head")[0] || document.documentElement;
            var script = document.createElement("script");
            script.src = url;
            script.onload = script.onreadystatechange = function () {
                if (!this.readyState || this.readyState === "loaded" || this.readyState === "complete") {
                    script.onload = script.onreadystatechange = null;
                    if (head && script.parentNode) {
                        head.removeChild(script);
                    }
                }
            };
            head.insertBefore(script, head.firstChild);
        };
        (function () {
            //下一页
            document.getElementById("m-pages-next").onclick = function () {
                if ($(this).hasClass("m-pages-none")) {
                    return false;
                }
                if (!loading) {
                    loading = true;
                    jsonp("/handlers/getcsnews.ashx?call=loadNews&ts=20140912&cs=<%=_serialId %>&t=<%=newsType %>&size=<%=_pageSize %>&i=" + (++cp));
                }
                $(window).scrollTop(0);
                return false;
            };
            //上一页
            document.getElementById("m-pages-pre").onclick = function () {
                if ($(this).hasClass("m-pages-none")) {
                    return false;
                }
                if (!loading) {
                    loading = true;
                    jsonp("/handlers/getcsnews.ashx?call=loadNews&ts=20140912&cs=<%=_serialId %>&t=<%=newsType %>&size=<%=_pageSize %>&i=" + (--cp));
                }
		        $(window).scrollTop(0);
		        return false;
		    };
            //处理下拉列表页数点击事件
            $("#m-page-select").change(function () {
                if (!loading) {
                    loading = true;
                    var curOptionVal = $(this).val();
                    cp = curOptionVal;
                    jsonp("/handlers/getcsnews.ashx?call=loadNews&ts=20140912&cs=<%=_serialId %>&t=<%=newsType %>&size=<%=_pageSize %>&i=" + cp);
                }
		        $(window).scrollTop(0);
		        return false;
		    });
        })();

        function loadNews(ns, count, pcount) {
            if (!ns && ns.length < 0) {
                return;
            }
            var arrHtml = [], ids = [];
            for (var i = 0; i < ns.length; i++) {
                var n = ns[i];
                ids.push("0_" + n.id);
                arrHtml.push("<li>");
                arrHtml.push("<a href = " + decodeURIComponent(n.url) + ">");
                if (n.image != "undifined" && n.image != "") {
                    arrHtml.push("<div class='img-box'><img src='" + decodeURIComponent(n.image) + "'></div>");
                }
                var title = cutStr(decodeURIComponent(n.tag), 44);
                arrHtml.push("<h4> " + title + "</h4>");
                arrHtml.push("<em><span>" + n.d + "</span>");
                arrHtml.push("<span>" + decodeURIComponent(n.aut) + "</span>");
                if (n.com != "undifined") {
                    arrHtml.push("<i class='ico-comment huifu comment_0_" + n.id + "'></i>");
                }
                arrHtml.push("</em></a>");
                arrHtml.push("</li>");
            }
            if (ids.length > 0) {
                // 提供jsonp服务的url地址（不管是什么类型的地址，最终生成的返回值都是一段javascript代码）
                var url = "http://api.admin.bitauto.com/comment/CommentAPI.ashx?token=3ace31a2-1482-4062-91fc-a3d7df4059aa&method=GetCommentCountByTypeIds&jsoncallback=successHandlerCC&ids=" + ids.join(',');
                jsonp(url);
            }

            $("#newsd").html(arrHtml.join(''));
            loading = false;
            var l = document.getElementById("m-pages-next");
            if (cp < pcount) {
                //l.innerHTML = "加载更多";
                $(l).removeClass("m-pages-none");
            } else {
                //l.parentNode.removeChild(l);
                $(l).addClass("m-pages-none");
            }
            if (cp > 1) {
                $("#m-pages-pre").removeClass("m-pages-none");
            } else {
                $("#m-pages-pre").addClass("m-pages-none");
            }
            //下拉框
            $("#m-page-select").val(cp);
            $("#m-pages-num-con").text(cp + "/" + pcount);
        }
        /*
         * param str 要截取的字符串
         * param L 要截取的字节长度，注意是字节不是字符，一个汉字两个字节
         * return 截取后的字符串
         */
        function cutStr(str, len) {
            var result = '',
                strlen = str.length,  
                chrlen = str.replace(/[^\x00-\xff]/g, '**').length;  

            if (chrlen <= len) { return str; }

            for (var i = 0, j = 0; i < strlen; i++) {
                var chr = str.charAt(i);
                if (/[\x00-\xff]/.test(chr)) {
                    j++;  
                } else {
                    j += 2;  
                }
                if (j <= len) {  
                    result += chr;
                } else {  
                    return result;
                }
            }
            return result;
        }
    </script>
    <%} %>
    <!--评论数JS-->
    <!--#include file="/include/pd/2014/common/00001/201506_typls_js_Manual.shtml"-->
    <%//-- 导航 start--%>
    <%-- <!--#include file="~/include/pd/2012/wap/00001/201503_wap_zsy_cd_js_Manual.shtml" -->
    <script type="text/javascript">
		<%// 默认元素ID "m-car-nav"%>
        CarNavForWireless.DivID = "m-car-nav";
		<%// 导航当前标签索引(0:综述,1:配置,2:图片,3:油耗,4:详解,5:口碑,6:视频,7:论坛)%>
        CarNavForWireless.CurrentTagIndex = 4;
    </script>
    <script type="text/javascript" src="http://api.car.bitauto.com/CarInfo/SerialBaseInfo.aspx?csid=<%= _serialId %>&op=GetCsForWireless&callback=CarNavForWireless.GenerateNav" charset="utf-8"></script>--%>
    <%//-- 导航 end--%>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchor.js?v=201209"></script>--%>
    <script type="text/javascript">
        var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
			modelStr = '<%=_serialId%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
        var zamplus_tag_params = {
            modelId: modelStr,
            carId: 0
        };
    </script>
    <script type="text/javascript">
        var _zampq = [];
        _zampq.push(["_setAccount", "12"]);
        (function () {
            var zp = document.createElement("script"); zp.type = "text/javascript"; zp.async = true;
            zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
            var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(zp, s);
        })();
    </script>
</body>
</html>
