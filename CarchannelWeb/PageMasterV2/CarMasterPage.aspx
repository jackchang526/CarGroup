<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarMasterPage.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.PageMasterV2.CarMasterPage" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%=masterName%>】<%=masterName%>汽车报价_图片_<%=DateTime.Now.Year+masterName%>新款车型-易车网</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->	<meta name="Keywords" content="<%=masterName%>,<%=masterName%>汽车,<%=masterName%>汽车报价,<%=masterName%>新款车型,易车网,car.bitauto.com" />
	<meta name="Description" content="<%=masterName%>:易车提供最新<%=masterName%>汽车报价,<%=masterName%>汽车图片,<%=masterName%>汽车新闻,视频,口碑,问答,二手车等,<%=masterName%>在线询价、低价买车尽在易车网。" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=masterUrlSpell %>/" />
	<%--<meta name="mobile-agent" content="format= xhtml; url=http://m.bitauto.com/g/carbrand.aspx?bigbrandid=<%=masterId %>" />--%>
	<!--#include file="~/ushtml/0000/yiche_2016_cube_pinpai_style-1248.shtml"-->
	<!--#include file="~/ushtml/0000/all_logo_style-322.shtml" -->
	<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--#include file="~/htmlV2/header2016.shtml"-->
	<div style="width:990px;margin:0 auto;">
		<ins id="div_372efa2f-ebc3-42b7-b13b-14bdc1a4f3a1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname="" adplay_brandid="" adplay_brandname="<%=masterName %>" adplay_brandtype="<%=masterCountry %>" adplay_blockcode="372efa2f-ebc3-42b7-b13b-14bdc1a4f3a1"></ins>
	</div>
    <header class="header-main header-type1">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <h1 class="brand-logo">
                    <a href="/<%=masterUrlSpell %>/">
                        <img class="logo" src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/55/m_<%=masterId %>_55.png" onerror="javascript:this.src=''" alt="<%=masterName%>汽车"></a>
                    <a href="/<%=masterUrlSpell %>/"><%=masterName%></a>
                </h1>
            </div>
            <div class="col-xs-8 right-box">
                <ul class="list keyword-list">
                    <li class="dt">热门搜索：</li>
                    <li><a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a></li>
                    <li><a href="http://v.bitauto.com/ycjm.shtml" target="_blank">原创节目</a></li>
                    <li><a href="http://jiangjia.bitauto.com/" target="_blank">降价</a></li>
                </ul>
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
        <div id="navBox">
            <nav class="header-main-nav">
                <div class="container">
                    <div class="col-auto left secondary">
                        <ul class="list" id="ulForTempClickStat">
                            <li id="treeNav_chexing" class="active"><a target="_self" href="http://car.bitauto.com/<%=masterUrlSpell%>/">首页</a></li>
                            <li id="treeNav_wenzhang"><a target="_self" href="http://car.bitauto.com/<%=masterUrlSpell%>/wenzhang/">文章</a></li>
                            <li id="treeNav_tupian"><a target="_blank" href="http://photo.bitauto.com/master/<%=masterId%>/">图片</a></li>
                            <li id="treeNav_shipin"><a target="_blank" href="http://v.bitauto.com/car/master/<%=masterId%>_0_0.html">视频</a></li>
                            <li id="treeNav_pirce"><a target="_blank" href="http://price.bitauto.com/mb<%=masterId%>/">报价</a></li>
                            <li id="treeNav_jiangjia"><a target="_blank" href="http://jiangjia.bitauto.com/mb<%=masterId%>/">降价</a></li>
                            <li id="treeNav_dealer"><a target="_blank" href="http://dealer.bitauto.com/<%=masterUrlSpell%>/">经销商</a></li>
                            <li id=""><a target="_blank" href="http://www.daikuan.com/china/brand/<%=masterUrlSpell%>/?from=2156">贷款</a></li>
                            <li id="treeNav_zhihuan"><a target="_blank" href="http://maiche.taoche.com/zhihuan/?master=<%=masterId%>/">置换</a></li>
                            <li id="treeNav_yanghu"><a target="_blank" href="http://yanghu.bitauto.com/?source=1">养护</a></li>
                            <li id="treeNav_ershouche"><a target="_blank" href="http://www.taoche.com/<%=masterUrlSpell %>/">二手车</a></li>
                            <li id="treeNav_jinkouche"><a target="_blank" href="http://koubei.bitauto.com/tree/xuanche/?mid=<%=masterId%>">口碑</a></li>
                            <li id="treeNav_wenda"><a target="_blank" href="http://ask.bitauto.com/browse/<%=masterId%>/solved/">问答</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
        </div>
    </header>

    <div class="container carbrand-page">
		<div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                     <!--品牌简介-->
                     <%if (!string.IsNullOrEmpty(masterIntroduce))
                          { %>
                            <div class="row story-section margin-bottom-xlg">
                                <%=masterIntroduce%>
                            </div>
                        <%}
                        %>
                    <!--品牌最新资讯-->
                      <%if (!string.IsNullOrEmpty(masterTopNews))
                          { %>
                            <div class="row news-section margin-bottom-xlg">
                                <%=masterTopNews%>
                            </div>
                        <%}
                        %>
                   
                    <!--主品牌下每个品牌中的子品牌列表-->
                    <%if (!string.IsNullOrEmpty(serialListByBrand))
                      { %>
                    <div class="row cartype-section margin-bottom-xlg <%=longTitleCss %>">
                        <%=serialListByBrand %>
                        <%if (OnlyOneCbID != 0)
                          { %>
                        <div class="more ad22017">
                            <ins id="Ins1" type="ad_play" adplay_ip="" adplay_areaname="" adplay_cityname=""
                                adplay_brandid="<%= OnlyOneCbID %>" adplay_brandname="" adplay_brandtype="" adplay_blockcode="e2e467ea-524d-4424-8368-03cf82f82a43"></ins>
                        </div>
                        <%} %>
                    </div>
                    <%}
                    %>
                   
                    <!--视频-->
                    <%if (!string.IsNullOrEmpty(videoHtml))
                      { %>
                        <%=videoHtml%>
                    <%}
                    %>
                  
                    <!--经销商-->
                     <%if (!string.IsNullOrEmpty(brandListDealerHtml))
                      { %>
                    <div class="row recommend-agent-section margin-bottom-sm">
                        <%=brandListDealerHtml %>
                    </div>
                    <%}
                    %>
                    <%--问答块--%>
                    <!-- # include file="~/include/pd/2012/wenda/00001/201405_ask_common_gb2312_Manual.shtml"-->
                   <%-- <%=askEntriesHtml %>--%>
                   <!-- SEO底部热门 -->
                        <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                        <!-- SEO底部热门 -->
                </div>
            </div>
			<div class="col-xs-3 section-right">
                <!-- 热门品牌 -->
                <div class="row hotbrand-sidebar margin-bottom-sm">
                    <div class="section-header header3">
                        <div class="box">
                            <h2><a href="http://car.bitauto.com/brandlist.html" target="_blank">热门品牌</a></h2>
                        </div>
                    </div>
                    <div class="list-box col3-55-box clearfix">
                        <%=hotMasterBrandHtml %>
                    </div>
                </div>
				<!--二手车-->
				<div class="row oldcar-sidebar margin-bottom-sm line_box" id="ucar_box">
				</div>
				<!--百度推广-->
				<div class="line-box baidupush">
					<script type="text/javascript">
						/*bitauto 200*200，导入创建于2011-10-17*/
					    var cpro_id = 'u646188';
					</script>
					<script src="http://cpro.baidu.com/cpro/ui/c.js" type="text/javascript"></script>
				</div>
				<!--百度推广-End -->
			</div>
		</div>
	</div>
    <input type="hidden" id="cheyisou" value="http://www.cheyisou.com/qiche/" />	<script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnewV2/taochev2.min.js?v=2016111614" type="text/javascript"></script>
<%--     <script src="/jsnewV2/taochev2.js" type="text/javascript"></script>--%>
	<script>
	    TaoChe.showBrandUCar('<%=string.Join(",",brandIds.ToArray()) %>', bit_locationInfo.cityId, '<%=masterUrlSpell %>', '<%=masterName %>', 5);
	</script>
	<!--本站统计代码-->
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
	<script type="text/javascript">
	    OldPVStatistic.ID1 = "<%=masterId.ToString() %>";
	    OldPVStatistic.ID2 = "0";
	    OldPVStatistic.Type = 2;
	    mainOldPVStatisticFunction();
	</script>
	<script type="text/javascript" src="http://dealer.bitauto.com/dealerinfo/Common/Control/TelControlTop.js"></script>
	<!-- 调用尾 -->
	<script type="text/javascript">
	    var CarCommonBSID = '<%= masterId.ToString() %>';
	</script>
	<!--#include file="~/htmlV2/footer2016.shtml"-->
	
	<script type="text/javascript">
	    function BrandZhanKai() {
	        $("#shortBrandStory").hide();
	        $("#detailBrandStory").show();
	    }
	    
	    function BrandShouQi() {
	        $("#shortBrandStory").show();
	        $("#detailBrandStory").hide();
	    }
	    
	    function CheBiaoZhanKai() {
	        $("#shortLogoStory").hide();
	        $("#detailLogoStory").show();
	    }

	    function CheBiaoShouQi() {
	        $("#shortLogoStory").show();
	        $("#detailLogoStory").hide();
	    }

	    // JavaScript Document
	    function addLoadEvent(func) {
	        var oldonload = window.onload;
	        if (typeof window.onload != 'function') {
	            window.onload = func;
	        } else {
	            window.onload = function () {
	                oldonload();
	                func();
	            }
	        }
	    }

	    function addClass(element, value) {
	        if (!element.className) {
	            element.className = value;
	        } else {
	            newClassName = element.className;
	            newClassName += " ";
	            newClassName += value;
	            element.className = newClassName;
	        }
	    }

	    function removeClass(element, value) {
	        var removedClass = element.className;
	        var pattern = new RegExp("(^| )" + value + "( |$)");
	        removedClass = removedClass.replace(pattern, "$1");
	        removedClass = removedClass.replace(/ $/, "");
	        element.className = removedClass;
	        return true;
	    }
	    function all_func() {
	        tabs("car_tab_MasterNews_ul", "data_table_MasterNews", "best_car", true);
	        tabs("car_MasterSerialList_ul", "data_table_MasterSerialList", "best_car", true);
	        tabs("car_MasterDealer_tab_ul", "data_table_MasterDealer", "best_car", true);
            //经销商处切换
	        tabs2("salesAgentRec", "data_table_MasterDealer", "best_car", true);
	    }
	    addLoadEvent(all_func)

	    /*=======================tab=============================*/
	    function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
	    function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

	    function tabsRemove(index, head, divs, div2s) {
	        if (!document.getElementById(head)) return false;
	        var tab_heads = document.getElementById(head);
	        if (tab_heads) {
	            var alis = tab_heads.getElementsByTagName("li");
	            for (var i = 0; i < alis.length; i++) {
	                removeClass(alis[i], "current");
	                hide(divs + "_" + i);
	                if (div2s) { hide(div2s + "_" + i) };
	                if (i == index) {
	                    addClass(alis[i], "current");
	                }
	            }
	            show(divs + "_" + index);
	            if (div2s) { show(div2s + "_" + index) };
	        }
	    }
	    var thetabstime;
	    function tabs(head, divs, div2s, over) {
	        if (!document.getElementById(head)) return false;
	        var tab_heads = document.getElementById(head);
	        if (tab_heads) {
	            var alis = tab_heads.getElementsByTagName("li");
	            for (var i = 0; i < alis.length; i++) {
	                alis[i].num = i;
	                if (over) {
	                    alis[i].onmouseover = function () {
	                        var thisobj = this;
	                        changetab(thisobj);
	                        //thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
	                    }
	                    alis[i].onmouseout = function () {
	                        clearTimeout(thetabstime);
	                    }
	                }
	                else {
	                    alis[i].onclick = function () {
	                        if (this.className == "current" || this.className == "last current") {
	                            changetab(this);
	                            return true;
	                        }
	                        else {
	                            changetab(this);
	                            return false;
	                        }
	                    }
	                }
	                function changetab(thebox) {
	                    tabsRemove(thebox.num, head, divs, div2s);
	                }
	            }
	        }
	    }
	    function tabs2(head, divs, div2s, over) {
	        if (!document.getElementById(head)) return false;
	        var tab_heads = document.getElementById(head);
	        if (tab_heads) {
	            var alis = tab_heads.getElementsByClassName("tabBrand");
	            for (var i = 0; i < alis.length; i++) {
	                alis[i].num = i;
	                if (over) {
	                    alis[i].onmouseover = function () {
	                        var thisobj = this;
	                        changetab(thisobj);
	                    }
	                    alis[i].onmouseout = function () {
	                        clearTimeout(thetabstime);
	                    }
	                }
	                else {
	                    alis[i].onclick = function () {
	                        if (this.className == "current" || this.className == "last current") {
	                            changetab(this);
	                            return true;
	                        }
	                        else {
	                            changetab(this);
	                            return false;
	                        }
	                    }
	                }
	                function changetab(thebox) {
	                    tabsRemove2(thebox.num, head, divs, div2s);
	                }
	            }
	        }
	    }
	    function tabsRemove2(index, head, divs, div2s) {
	        if (!document.getElementById(head)) return false;
	        var tab_heads = document.getElementById(head);
	        if (tab_heads) {
	            var alis = tab_heads.getElementsByClassName("tabBrand");
	            for (var i = 0; i < alis.length; i++) {
	                removeClass(alis[i], "current");
	                hide(divs + "_" + i);
	                if (div2s) { hide(div2s + "_" + i) };
	                if (i == index) {
	                    addClass(alis[i], "current");
	                }
	                if (alis.length > 3)
	                {
	                    if (index >= 2) {
	                        $("#moreSalesAgentRec").addClass("current");
	                    }
	                    else {
	                        $("#moreSalesAgentRec").removeClass("current");
	                    }
	                }
	            }
	            show(divs + "_" + index);
	            if (div2s) { show(div2s + "_" + index) };
	        }
	    }
	</script>
	<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
	<!--#include file="~/htmlV2/CommonBodyBottom.shtml"-->
</body>
</html>
<script type="text/javascript">
    var adplay_CityName = '';//城市
    var adplay_AreaName = '';//区域
    var adplay_BrandID = '<%=masterId%>';//品牌id 
    var adplay_BrandType = '<%=masterCountry%>';//品牌类型：（国产）或（进口）
    var adplay_BrandName = '<%=masterName%>';//品牌
    var adplay_BlockCode = '1e746e70-1cbb-48c6-81c9-0163808f7a70';//广告块编号
</script>
<script type="text/javascript">
      //获取视频播放数
      var $videos = $("div[data-videoid]");
      var videoArr = [],
          fvideoArr = [];
      $.each($videos, function (index,item) {
          var curVideoId = $(item).attr("data-videoid");
          if ($(item).attr("data-type") == "vf") {
              fvideoArr.push(curVideoId);
          }
          else {
              videoArr.push(curVideoId);
          }
      })
      var videoStr = videoArr.join(",");
      var fvideoStr = fvideoArr.join(",");
      $.ajax({
          url: "http://api.admin.bitauto.com/videoforum/Promotion/GetVideoByVideoIds?vIds=" + videoStr + "&vfids=" + fvideoStr,
          dataType: "jsonp",
          jsonpCallback: "getvedionumcallback",
          cache:true,
          success: function (data) {
              if (data && data.length > 0)
              {
                  $.each(data, function (index,item) {
                      var curVideo = item["VideoId"];
                      var formatPlayCount = item["FormatPlayCount"];
                      var replyCount = item["ReplyCount"];
                      var $curVideoElem= $("div[data-videoid='" + curVideo + "']");
                      if($curVideoElem)
                      {
                          $curVideoElem.find(".p-list .num").find(".play").html(formatPlayCount).end().find(".comment").html(replyCount);
                      }
                  });
              }
          }
      });
      $(function () {
          var $serialListTable = $("[id='data_table_MasterSerialList_0'");
          var length = 0;
          if ($serialListTable)
          {
              length=$serialListTable.html().trim().length;
          }
          if (length <= 0)
          {
              $("[id='data_table_MasterSerialList_0'").html('<div>暂无车型!</div>');
          }
      })
      //无图片时的处理
      function nofind(imgSize) {
          var img = event.srcElement;
          if (imgSize == 1) {
              img.src = "http://www.cnblogs.com/sys/common/image/fileoperation/icon/default1.gif";
          }
          else if (imgSize == 2) {
              img.src = "http://www.cnblogs.com/sys/common/image/fileoperation/icon/default2.gif";
          }
          else {
          }
          img.onerror = null;// 控制不要一直跳动
      }
     
</script>
<script type="text/javascript" src="http://d2.yiche.com/js/sense.js"></script>
<!--百度热力图,与webtrents对比用，过几天可删（2011-11-29）-->
<script type="text/javascript">
    var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
    document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
</script>
