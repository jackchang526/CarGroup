<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMasterPage.aspx.cs" Inherits="WirelessWeb.CarMasterPage" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>【<%=_masterName %>】<%=_masterName %>汽车报价_图片_<%=DateTime.Now.Year+_masterName %>新款车型-手机易车网</title>
    <meta name="Keywords" content="<%=_masterName %>,<%=_masterName %>汽车,<%=_masterName %>汽车报价,<%=_masterName %>新款车型,手机易车网,car.m.yiche.com" />
    <meta name="Description" content="<%=_masterName %>:易车提供最新<%=_masterName %>汽车报价,<%=_masterName %>汽车图片,<%=_masterName %>汽车新闻,视频,口碑,问答,二手车等,<%=_masterName %>在线询价、低价买车尽在手机易车网" />

    <!--#include file="~/ushtml/0000/myiche2016_cube_pinpaiye-1161.shtml"-->
</head>
<body>
    <div class="mbt-page">
        <!-- header start -->
        <div class="op-nav">
            <a id="gobackElm" href="javascript:;" class="btn-return">返回</a>

            <div class="tt-name">
                <a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%=_masterName %></h1>
            </div>
            <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
        </div>
        <div class="op-nav-mark" style="display: none;"></div>
        <!-- header end -->
        <!-- logo start -->
        <div class="car-logo-box">
            <div class="m-car-brand-logo">
                <img alt="<%=_masterName %>汽车_报价" src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%=_masterId %>_100.png">
            </div>
            <h2><%=_masterName %></h2>
            <p>共<strong><%=_serialCount %></strong>个车型，其中<strong><%=_serialSaleCount %></strong>个在售</p>
        </div>
        <!-- logo end -->
        <!-- tab start -->
        <div class="first-tags first-tags3">
            <ul id="chgtab">
                <li class="current"><a href="javascript:;"><span>车型</span></a></li>
                <li><a href="javascript:;"><span>品牌简介</span></a></li>
                <li><a href="javascript:;"><span>品牌新闻</span></a></li>
            </ul>
        </div>
        <!-- tab end -->
        <%//品牌列表 start %>
        <div name="tab-content">
            <%=_brandListHtml %>
        </div>
        <%//品牌列表 end %>
        <%//品牌简介 start %>
        <div class="car-story-con" name="tab-content" style="display: none;">
            <div class="tt-small">
                <span>品牌简介</span>
            </div>
            <%if (!string.IsNullOrEmpty(_masterInfo))
              {%><p><%=_masterInfo.Replace("\n\r","</p><p>") %></p>
            <%} %>
            <div class="tt-small">
                <span>车标故事</span>
            </div>
            <%if (!string.IsNullOrEmpty(_masterStory))
              {%><p><%=_masterStory.Replace("\n\r","</p><p>") %></p>
            <%} %>
        </div>
        <%//品牌简介 end %>
        <%//品牌新闻 start %>
        <div name="tab-content" style="display: none;">
            <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class="card-news">
                        <%= _newsListHtml%>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="m-pages b-shadow">
                <a id="prePage" href="javascript:void(0)" class="m-pages-pre m-pages-none">上一页</a>
                <div class="m-pages-num">
                    <div id="m-pages-num-con" class="m-pages-num-con">1/<%=_PageTotal %></div>
                    <div class="m-pages-num-arrow"></div>
                </div>
                <select id="m-page-select">
                    <%for (int i = 1; i <= _PageTotal; i++)
                      {%>
                    <option><%=i%></option>
                    <%} %>
                </select>
                <a id="nextPage" href="javascript:void(0)" class="m-pages-next">下一页</a>
            </div>
        </div>
        <%//品牌新闻 end %>
    </div>
    <%if (_NewsCount > _PageSize)
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
            document.getElementById("nextPage").onclick = function () {
                if ($(this).hasClass("m-pages-none")) {
                    return false;
                }
                if (!loading) {
                    loading = true;
                    jsonp("/handlers/GetMasterNews.ashx?call=loadNews&mabId=<%=_masterId %>&size=<%=_PageSize %>&i=" + (++cp));
                }
		        return false;
		    };
		    //上一页
		    document.getElementById("prePage").onclick = function () {
		        if ($(this).hasClass("m-pages-none")) {
		            return false;
		        }
		        if (!loading) {
		            loading = true;
		            jsonp("/handlers/GetMasterNews.ashx?call=loadNews&mabId=<%=_masterId %>&size=<%=_PageSize %>&i=" + (--cp));
                }
			    return false;
			};
		    //处理下拉列表页数点击事件
		    $("#m-page-select").change(function () {
		        if (!loading) {
		            loading = true;
		            var curOptionVal = $(this).val();
		            cp = curOptionVal;
		            jsonp("/handlers/GetMasterNews.ashx?call=loadNews&&mabId=<%=_masterId %>&size=<%=_PageSize %>&i=" + cp);
                }
			    return false;
			});
		})();
        //ns:数据源 count:总数量 pcount:页数
        function loadNews(ns, count, pcount) {
            if (!ns && ns.length < 0) {
                return;
            }
            var arrHtml = [];
            for (var i = 0; i < ns.length; i++) {
                var n = ns[i];
                if (n.imageUrl != "undifined" && n.imageUrl != "") {
                    arrHtml.push("<li>");
                } else {
                    arrHtml.push("<li class=\"news-noimg\">");
                }
                arrHtml.push("<a href = " + n.filePath + ">");
                if (n.imageUrl != "undifined" && n.imageUrl != "") {
                    arrHtml.push("<div class='img-box'><span><img src='" + n.imageUrl + "'></span></div>");
                }
                var title = cutStr(decodeURIComponent(n.title), 44);

                arrHtml.push("<div class=\"con-box\">");
                arrHtml.push("<h4> " + title + "</h4>");
                arrHtml.push("<em><span>" + n.publishTime + "</span>");
                arrHtml.push("<span>" + n.editorName + "</span>");
                if (n.commentNum != "undifined") {
                    arrHtml.push("<i class=\"ico-comment\">" + n.commentNum + "</i>");
                }
                arrHtml.push("</em></div></a>");
                arrHtml.push("</li>");
            }

            $("#cardNews").html(arrHtml.join(''));
            loading = false;
            var l = document.getElementById("nextPage");
            if (cp < pcount) {
                //l.innerHTML = "加载更多";
                $(l).removeClass("m-pages-none");
            } else {
                //l.parentNode.removeChild(l);
                $(l).addClass("m-pages-none");
            }
            if (cp > 1) {
                $("#prePage").removeClass("m-pages-none");
            } else {
                $("#prePage").addClass("m-pages-none");
            }
            //下拉框
            window.location.hash = "#" + cp;
            $("#m-page-select").val(cp);
            $("#m-pages-num-con").text(cp + "/" + pcount);
        }
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
    <script type="text/javascript">
        var url = "http://car.m.yiche.com/";
    </script>
    <%// --footer start--%>
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <div class="float-r-box">
        <!--#include file="/include/pd/2014/wapCommon/00001/201511_wap_common_ycfd_Manual.shtml"-->
    </div>
    <%// --footer end--%>
    <script type="text/javascript">
        (function () {
            var hashValue = location.hash;
            var tabs = document.getElementById("chgtab").getElementsByTagName("li");
            var divs = document.getElementsByName("tab-content");
            if (hashValue != "" && hashValue != "#n") {
                tabs[0].className = "";
                tabs[1].className = "";
                tabs[2].className = "current";
                if (hashValue != "#0") {
                    cp = hashValue.substr(1);
                    jsonp("/handlers/GetMasterNews.ashx?call=loadNews&mabId=<%=_masterId %>&size=<%=_PageSize %>&i=" + cp);
                }
                divs[0].style.display = "none";
                divs[1].style.display = "none";
                divs[2].style.display = "";
            }
            for (var i = 0; i < tabs.length; i++) {
                tabs[i].onclick = function () {
                    if (this.className == 'current') return false;
                    var x = 0;
                    for (var j = 0; j < tabs.length; j++) {
                        if (tabs[j] == this) {
                            x = j;
                        }
                        else {
                            tabs[j].className = "";
                            divs[j].style.display = "none";
                        }
                    }
                    this.className = "current";
                    if ($(this).find("span").text() == "品牌新闻") {
                        $(this).find("a").attr("href", "#0");
                    } else {
                        $(this).find("a").attr("href", "#n");
                    }
                    divs[x].style.display = "";
                }
            }
        })();
    </script>
    <%--<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchor.js?v=201209"></script>--%>
    <!-- 按品牌 start  从首页把这段代码复制到主品牌页面最下面隐藏掉，提升SEO(王淞提的需求)-->
    <div class="m-hot-brands" pageareaid="178" style="display: none;">
        <ul>
            <li>
                <a href="http://car.m.yiche.com/brandlist/volkswagen/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_8_55.png">
                    <p>大众</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/hyundai/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_13_55.png">
                    <p>现代</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/ford/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_17_55.png">
                    <p>福特</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/kia/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_28_55.png">
                    <p>起亚</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/buick/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_127_55.png">
                    <p>别克</p>
                </a>
            </li>

            <li>
                <a href="http://car.m.yiche.com/brandlist/greatwall/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_21_55.png">
                    <p>长城</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/audi/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_9_55.png">
                    <p>奥迪</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/cajc/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_136_55.png">
                    <p>长安</p>
                </a>
            </li>
            <li>
                <a href="http://car.m.yiche.com/brandlist/chevrolet/">
                    <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_49_55.png">
                    <p>雪佛兰</p>
                </a>
            </li>
            <li><a href="http://car.m.yiche.com/brandlist/"><i></i><i></i><i></i>
                <p>
                    更多
                </p>
            </a></li>

        </ul>
        <div class="clear"></div>
    </div>
    <!-- 按品牌 end -->
</body>
</html>
