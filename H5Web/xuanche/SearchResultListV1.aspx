<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResultListV1.aspx.cs" Inherits="H5Web.xuanche.SearchResultListV1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <!--#include file="/ushtml/0000/4th_2015-2_zhixuanche_style-1082.shtml"-->

    <title>【汽车报价|汽车报价大全】-手机易车网</title>
    <meta name="keywords" content="汽车报价,汽车报价大全,选车工具,手机易车网"/>
    <meta name="description" content="易车网报价大全，提供最新的最全的汽车报价、车价查询、帮助您方便快捷的购车!"/>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery-1.8.2.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/plugs/jquery/jquery.cookie.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/Common/util.v7.js?v=201606151539"></script>
    <script type="text/javascript">
        var summary = { serialId: 0, IsNeedShare: true };
    </script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body class="car_list_body">
<!--头开始-->
<header class="header2">
    <a href="/chebiaodang/" class="back-step">上一步</a>
</header>
<!--头结束-->

<!--容器开始-->
<div class="car_list_box" style="display: none">

    <div class="car_list_num"></div>

    <!--列表 开始-->
    <ul class="car_list">
    </ul>
    <!--列表 结束-->

    <!--翻页 开始-->
    <div class="m-pages" style="display: none">
        <a href="javascript:void(0);" class="m-pages-pre m-pages-none">上一页</a>
        <div class="m-pages-num">
            <div class="m-pages-num-con"></div>
            <div class="m-pages-num-arrow"></div>
        </div>
        <select id="drppagenum">
            <option>1</option>
        </select>
        <a href="javascript:void(0);" class="m-pages-next">下一页</a>
    </div>
    <!--翻页结束-->

</div>
<!--容器结束-->

<script type="text/javascript">
    var par = util.GetKeyValueString(["ad", "order", "lg", "ly","Keyword","page","tele","WT.mc_id"]);

    sessionStorage["listUrl"] = window.location.href;

    var page = <%= CurrentPage %>;
    var size = <%= PageSize %>;
    var keyword = "<%= Keyword %>";
    var para = <%= Para %>;
    var tempArr = [];
    var totalpage = 0;
    $(".back-step").bind("click", function() {
        var returnPar = util.GetKeyValueString(["ad", "order", "lg", "ly","tele","WT.mc_id"]);
        if (returnPar != null && returnPar !== "") {
            var href = $(this).attr("href");
            if (href.indexOf("?") >= 0) {
                $(this).attr("href", href +"&" +returnPar);
            } else {
                $(this).attr("href", href + "?" + returnPar);
            }
        }
    });

    $(".m-pages-pre").bind("click", function() {
        setPage(page - 1);
    });

    $(".m-pages-next").bind("click", function() {
        setPage(page + 1);
    });

    $("#drppagenum").bind("change", function() {
        var selpage = $(this).val();
        setPage(selpage);
    });

    function setPage(page) {
        $.each(para, function(index, item) {
            if (item != null && item.toLowerCase() !== "page") {
                tempArr.push(item + "=" + util.GetQueryStringByName(item));
            }
        });
        tempArr.push("page=" + page);
        location.href = "http://" + window.location.host + window.location.pathname + "?" + tempArr.join("&");
    }

    var setLocalStorageData = function(searchKey) {
        var viewedkey = "search_key";
        if (searchKey !== "") {
            var arrDisplay = [];
            var strStoreDate = window.localStorage ? localStorage.getItem(viewedkey) : $.cookie(viewedkey);
            if (strStoreDate) {
                arrDisplay = strStoreDate.split(",");
            }
            var keyIndex = arrDisplay.indexOf(searchKey);
            if (keyIndex < 0) {
                if (arrDisplay.length >= 10) {
                    arrDisplay.shift(); //移除最前一个元素并返回该元素值，数组中元素自动前移
                }
                arrDisplay.push(searchKey);
            } else {
                //排序历史记录
                arrDisplay.splice(keyIndex, 1);
                arrDisplay.push(searchKey);
            }
            if (window.localStorage) {
                localStorage.setItem(viewedkey, arrDisplay);
            } else {
                $.cookie(viewedkey, arrDisplay, { expires: 7 });
            }
        }
    };
    $(function() {
        setLocalStorageData(keyword);
        $.ajax({
            type: "get",
            url: "http://api.cheyisou.com/APP/BSearchCarForApp.ashx",
            data: { keyword: keyword, size: size, page: page },
            cache: true,
            dataType: "jsonp",
            jsonpCallback: "s",
            success: function(data) {
                var rescount = data["ResultCount"];
                if (rescount > 0) {

                    if (rescount > size) {
                        $(".m-pages").show();
                    }

                    $(".car_list_num").html(keyword + " 共" + rescount + "个车型");
                    totalpage = Math.ceil(rescount / size);
                    if (totalpage > 0) {
                        $(".m-pages-num-con").html(page + "/" + totalpage);
                    }
                    var drppage = "";
                    for (var i = 1; i <= totalpage; i++) {
                        if (i == page) {
                            drppage += "<option selected=true>" + i + "</option>";
                        } else {
                            drppage += "<option>" + i + "</option>";
                        }
                    }
                    $("#drppagenum").html(drppage);

                    var searchRes = "";
                    $.each(data["CarList"], function(index, item) {
                        if (util.GetQueryStringByName("ly")!=null&&util.GetQueryStringByName("ly")==="xxj") {
                            searchRes += "<li> <a href='http://dealer.h5.yiche.com/searchOrder/"+item.Csid+"/0/?h5from=search&leads_source=H001005&"+par+"'> <img src='" + item.ImgUrl.replace("_1.", "_6.") + "'> <span>" + item.Title + "</span> <p>" + item.RefPrice + "</p> </a> </li>";
                        } else {
                            searchRes += "<li> <a href='/" + item.CsSpell + "?h5from=search&"+par+"'> <img src='" + item.ImgUrl.replace("_1.", "_6.") + "'> <span>" + item.Title + "</span> <p>" + item.RefPrice + "</p> </a> </li>";
                        }
                    });
                    $(".car_list").html(searchRes);
                } else {
                    $(".car_list_box").html("<div class='message-failure message-failure2'><img src='http://img1.bitauto.com/uimg/4th/img2/failure.png' /><h2>未搜索到指定的车型！</h2><p>请调整搜索关键字或尝试<b>品牌选车</b></p></div>");
                }

            },
            complete: function() {
                $("#drppagenum option[text=" + page + "]").attr("selected", true);
                $(".car_list_box").show();
                if (page === totalpage) {
                    $(".m-pages-next").addClass("m-pages-none");
                    $(".m-pages-next").unbind("click");
                } else {
                    $(".m-pages-next").removeClass("m-pages-none");
                }
                if (page === 1) {
                    $(".m-pages-pre").addClass("m-pages-none");
                    $(".m-pages-pre").unbind("click");
                } else {
                    $(".m-pages-pre").removeClass("m-pages-none");
                }

                $(".car_list img").each(function() {
                    $(this).height($(this).width() / 1.5);
                });
            }
        });
    });
</script>

<script type="text/javascript">
    var XCWebLogCollector = { area: '201' };
    if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
        XCWebLogCollector.area = bit_locationInfo.cityId;
    }
    if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
        XCWebLogCollector.serial = summary.serialId;
    }
</script>
<script type="text/javascript">
    var __zp_tag_params = {
        modelId: summary.serialId,
        carId: 0
    };
</script>
<script type="text/javascript">
    var forweixinObj = {
        debug: false,
        appId: 'wx0c56521d4263f190',
        jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo']
    };
    var pageShareContent = {
        title: '越野党？上班族？家人多？个性选车，就用来一车！',
        keywords: '汽车报价,汽车报价大全,选车工具,手机易车网',
        desc: '选车、买车、用车，易车-来一车，你喜欢的车子我都有。',
        link: 'http://car.h5.yiche.com/?',
        imgUrl: 'http://image.bitautoimg.com/carchannel/pic/laiyiche.png'
    };
    try {
        pageShareContent.link = window.location.href;
    } catch (err) {
    }
</script>
<!--#include file="~/inc/The3rdStat.html"-->
<!--#include file="~/inc/WeiXinJs.shtml"-->
<!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
<%--<script type="text/javascript">
    (function(param) {
        var c = { query: [], args: param || {} };
        c.query.push(["_setAccount", "12"]);
        c.query.push(["_setSiteID", "1"]);
        (window.__zpSMConfig = window.__zpSMConfig || []).push(c);
        var zp = document.createElement("script");
        zp.type = "text/javascript";
        zp.async = true;
        zp.src = ("https:" == document.location.protocol ? "https:" : "http:") + "//cdn.zampda.net/s.js";
        var s = document.getElementsByTagName("script")[0];
        s.parentNode.insertBefore(zp, s);
    })(window.__zp_tag_params);
</script>--%>

</body>
</html>