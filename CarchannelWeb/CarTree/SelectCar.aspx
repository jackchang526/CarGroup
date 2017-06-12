<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCar.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTree.Cartree_SelectCar" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%=pageTitle%></title>
    <meta name="keywords" content="<%=metaKeywords %>" />
    <meta name="description" content="<%=metaDescription %>" />
    <!--#include file="~/ushtml/0000/yiche_2014_cube_car-685.shtml" -->

    <script type="text/javascript">
        var _SearchUrl = '<%=this._SearchUrl %>';
    </script>
</head>
<body>
    <span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--顶通-->
    <%
        if (!string.IsNullOrEmpty(fullAd))
        {
    %>
    <div style="margin: 0 auto; width: 960px">
        <%=fullAd%>
    </div>
    <%} %>
    <!--#include file="~/html/tree_header2014.shtml"-->
    <div id="topAd" class="tree_wrap_box">
        <%=topAd %>
    </div>
    <!--头部导航-->
    <%--<%=NavbarHtml%>--%>
    <!--#include file="~/include/pd/2014/common/00001/201402_shuxing_nav_chexing_Manual.shtml" -->
    <!--左侧树形-->
    <div id="tree_wrap_box" class="tree_wrap_box">
        <div id="leftTreeBox" class="treeBoxv1">
        </div>
        <!--右侧内容-->
        <div id="treeMainv1" class="treeMainv1">
            <!-- 广告 -->
            <%--<%= ADTopHtml %>--%>
            <a id="anchorTitle"></a>
            <div class="tree_navigate">
                <div>
                    <span>您当前的位置：</span><a href="http://www.bitauto.com/">易车</a> &gt; <a href="http://car.bitauto.com/">车型</a> &gt; <strong>按条件选车</strong>
                </div>
            </div>
            <div id="tempNoborder" class="line-box" style="z-index: 2">
                <div class="title-con">
                    <div class="title-box">
                        <h3>按条件选车</h3>
                        <span id="lastSelectCar"></span>
                        <div class="more">
                            <%--		<a target="_blank" href="http://www.bitauto.com/feedback/FAQ.aspx?col=9&tab=5">温馨提示</a>
							| --%><a target="_blank" href="http://www.bitauto.com/zhuanti/daogou/gsqgl/ ">购车流程</a>
                        </div>
                    </div>
                </div>
                <%=this._ConditionsHtml %>
            </div>
            <script type="text/javascript">
                var ad_carlistdata = <%=adCarListData%>;
            </script>
            <div class="line-box" id="params-styleList">
                <%=serialListHtml%>
                <!--选车结果-->
                <div id="divContent" class="carpic_list y2015"></div>
                <div class="the_pages pages_top">
                    <div id="divPage"></div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="line-box" id="noResult" style="display: none">
                <div class="title-con" style="z-index: 1">
                    <div class="title-box title-box2">
                        <h4>选新车</h4>
                    </div>
                </div>
                <div class="no-txt-box">
                    <p class="tit">抱歉，未找到合适的车型</p>
                    <p>请修改条件再次查询，或者去 <a href="http://www.taoche.com/all/" target="_blank">易车二手车</a> 看看</p>
                </div>
                <div class="clear">
                </div>
            </div>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/gouche/pc/jquery.pagination.js?v=20150425"></script>
            <script type="text/javascript">
                conditionObj.Type = "car";
                conditionObj.InitPageCondition();
            </script>

            <script type="text/javascript">
                var params = {};
                params.tagtype = "chexing";
                params.pagetype = "search";
            </script>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/lefttreenew.js?v=20140515"></script>
            <%=hotSerialHtml%>
            <div class="line-box" id="gouchelist">
            </div>
            <div class="line-box" id="ucarlist">
            </div>
            <script type="text/javascript">
                (function () {
                    var cookiesName = "car_superlink", cObj = CookieHelper.readCookie(cookiesName), toolAlert = document.getElementById("tool-alert");
                    ((cObj != null && cObj == "1") ? toolAlert.style.display = "none" : toolAlert.style.display = "block");
                    SelectCar.Tools.addEvent(document.getElementById("tool-close"), "click", function () {
                        CookieHelper.setCookie(cookiesName, "1", { "expires": 3600, "path": "/", "domain": "car.bitauto.com" }); toolAlert.style.display = "none";
                    }, false);
                })();
            </script>
            <!-- footer -->
            <!--#include file="~/html/treefooter2014.shtml"-->
        </div>
    </div>
    <!-- 对比浮动框 -->
    <!-- baidu-tc begin {"action":"DELETE"} -->
    <!-- 对比浮动框 -->
    <script type="text/javascript">
        if (typeof (bitLoadScript) == "undefined") {
            bitLoadScript = function (url, callback, charset) {
                var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
                if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
                else { s.onload = function () { if (callback) callback(); }; }
                s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
            };
        }
        //二手车
        //ModifyDate:2015-09-16
        //Desc: 如果没有节点数据，则取接口中 RecommendCarList节点的数据
        (function () {
            var cityId = 201;
            if (typeof (bit_locationInfo) != 'undefined') {
                cityId = bit_locationInfo.cityId;
            }
            var queryUrl = window.location.search.substr(1);
            queryUrl = queryUrl == "" ? "" : "&" + queryUrl;
            //级别重写请求
            if(queryUrl=="" && conditionObj && conditionObj.Level && conditionObj.Level > 0){
                queryUrl = queryUrl + "&l=" + conditionObj.Level;
            }
            $.ajax({
                url: "http://yicheapi.taoche.cn/carsourceinterface/forjson/searchcarlist.ashx?cityid=" + cityId + "&count=24" + queryUrl, cache: true, dataType: "jsonp", jsonpCallback: "tcCallback", success: function (data) {
                    var dataCarListInfo = data.CarListInfo;
                    var moreTaoCheUrl = data.MoreTaoCheUrL;
                    var dataCount = data.DataCount;
                    var tsMsg='';//当没有符合选车条件的车款时，显示的信息
				
                    if (dataCount <= 0) {
                        tsMsg = '<div class="ts-msg"><p>没有找到符合条件的车源，为您推荐这些二手车。</p></div>';
                        var dataRecommendCarList=data.RecommendCarList;
                        if (dataRecommendCarList.length <= 0)
                        {
                            $("#ucarlist").hide();
                            return;
                        }
                        dataCarListInfo = dataRecommendCarList;
                        dataCount = dataCarListInfo.length;
                    }
                    var strHtml = [];
                    strHtml.push("<div class=\"title-con\">");     
                    strHtml.push("<div class=\"title-box title-box2\">");
                    strHtml.push("<h4 id=\"ucar-title\"><a target=\"_blank\" href=\"" + moreTaoCheUrl + "\">选二手车</a></h4>");
                    if (data.DataCount > 0) {
                        strHtml.push("<span>共" + dataCount + "个二手车源</span>");
                    }
                    strHtml.push("<div id=\"ucar-more\" class=\"more\"><a target=\"_blank\" href=\"" + moreTaoCheUrl + "\">更多&gt;&gt;</a></div>");
                    strHtml.push("</div>");
                    strHtml.push("</div>");

                    strHtml.push("<div class=\"cxdb-box taoche-box taoche-box-mline esc-rk-box\">");
                    if (tsMsg.length > 0)
                    {
                        strHtml.push(tsMsg);
                    }
                    strHtml.push("<ul id=\"ucar-carlist\">");
                    $.each(dataCarListInfo, function (i, n) {
                        if (i > 11) return;
                        loadCarDetailInfo(strHtml, i, n);
                    });
                    strHtml.push("</ul>");
					
                    if (dataCarListInfo.length > 12) {
                        strHtml.push("<div class=\"addd_more_link load_more\"><a href=\"javascript:void(0);\">加载更多车源<em></em></a></div>");
                    }
                    strHtml.push("</div>");
			
                    $("#ucarlist").html(strHtml.join(''));

                    //加载更多车源事件
                    $(".addd_more_link").one("click", function () {
                        if (dataCarListInfo && dataCarListInfo.length > 12) {
                            var strMoreCarHtml = [];
                            strMoreCarHtml.push("<ul>");
                            $.each(dataCarListInfo, function (i, n) {
                                if (i > 11) {
                                    loadCarDetailInfo(strMoreCarHtml, i, n);
                                }
                            });
                            strMoreCarHtml.push("</ul>");
                            $("#ucar-carlist").after(strMoreCarHtml.join(''));
                            if (dataCarListInfo.length >= 24) {
                                $(this).removeClass('load_more').find('a').html('查看更多车源<em></em>');
                            }
                            $(this).on("click", function () { window.open(moreTaoCheUrl); })
                        }
                        else {
                            window.open(moreTaoCheUrl);
                        }
                    });
                    //淘车统计
                    statTaoche();
                }
            });
            //优惠购车
            $.ajax({
                url: "http://api.gouche.yiche.com/selectcar/getpreference?cityid=" + cityId + "&page=1&pagesize=4" + queryUrl, cache: true, dataType: "jsonp", jsonpCallback: "goucheCallback", success: function (data) {
                    if(data&&data.SerialList.length<=0) return;
                    var strHtml=[];
                    strHtml.push("<div class=\"title-con\">");     
                    strHtml.push("<div class=\"title-box title-box2\">");
                    strHtml.push("<h4 id=\"gouche-title\"><a target=\"_blank\" href=\"http://gouche.yiche.com/\">优惠推荐</a></h4>");
                    strHtml.push("</div>");
                    strHtml.push("</div>");

                    strHtml.push("<div class=\"carpic_list\" id=\"gouche-carlist\">");
                    strHtml.push("<ul>");
                    $.each(data.SerialList,function(i,n){
                        //var url="http://gouche.yiche.com/"+bit_locationInfo.engName+"/sb"+n.SerialId+"/m"+n.CarId+"/";
                        var ref = "";
                        if (n.ProductType == 1) {
                            ref = "leads_source=p045001"
                        }
                        else if (n.ProductType == 2) {
                            ref = "leads_source=p045002"
                        }
                        else if (n.ProductType == 3) {
                            ref = "leads_source=p045003"
                        }
                        else if (n.ProductType == 4) {
                            ref = "leads_source=p045004"
                        }
                        var url = n.Url.indexOf("?") > 0 ? n.Url + "&" + ref : n.Url + "?" + ref;
                        //易车惠URL
                        if (n.ProductType == 4) {
                            url = n.Url + "?" + ref;
                        }
                        strHtml.push("<li>");
                        strHtml.push("    <a target=\"_blank\" href=\""+url+"\">");
                        strHtml.push("           <img alt=\"\" src=\""+n.ImageUrl.replace("_6.","_1.")+"\">");
                        strHtml.push("    </a>");
                        strHtml.push("                    <div class=\"title\"><a target=\"_blank\" href=\""+url+"\">"+n.SerialName+"</a></div>");
                        strHtml.push("                    <div class=\"txt\">"+(n.ReducedRate*10).toFixed(1)+"折起</div>");
                        strHtml.push("                    <div class=\"txt huizi txt3\">"+n.ShortRemarks+"</div>");
                        strHtml.push("</li>");
                    });
                    strHtml.push("</ul>");
                    strHtml.push("</div>");
                    $("#gouchelist").html(strHtml.join(''));
                    //优惠购车统计
                    statGouche();
                }});
        })();
        function statTaoche() {
            bitLoadScript('http://image.bitautoimg.com/stat/PageAreaStatistics.js', function () {
                PageAreaStatistics.init("294,295,296,297");
            }, 'utf-8');
        }
        function statGouche() {
            bitLoadScript('http://image.bitautoimg.com/stat/PageAreaStatistics.js', function () {
                PageAreaStatistics.init("364,363");
            }, 'utf-8');
        }
        //将"1023.45万"转换为1023.4万
        function specialFloatToStr(SaveMoney) {
            var saveMoney = parseFloat(SaveMoney.substr(0, SaveMoney.length - 1));
            if (saveMoney > 0) {
                var strSaveMoney = SaveMoney;
                var indexPoint = SaveMoney.indexOf('.');
                if (saveMoney >= 1000) {
                    strSaveMoney = SaveMoney.substr(0, indexPoint + 2) + "万";
                }
                return strSaveMoney;
            }
            else { return 0;}
        }
        //加载每个车款详情
        function loadCarDetailInfo(strHtml,i, n) {
            var className = "";
            if (i % 4 == 0) className = "first";
            if ((i + 1) % 4 == 0) className = "last";
            strHtml.push("<li class=\"" + className + "\"><div class=\"img-box\"><a href=\"" + n.CarlistUrl + "&leads_source=p045005\" target=\"_blank\" class=\"pic\"><img width=\"150\" height=\"100\" src=\"" + n.PictureUrl + "\"></a></div>");
            strHtml.push("<div class=\"txt-box\"><a href=\"" + n.CarlistUrl + "&leads_source=p045005\" target=\"_blank\">" + n.BrandName + "</a></div>");

            var strDisplayPrice = specialFloatToStr(n.DisplayPrice);
            strHtml.push("<div class=\"esc-yd-link\"><strong>" + strDisplayPrice + "</strong>");
            var strSaveMoney = specialFloatToStr(n.SaveMoney);
            if (strSaveMoney != 0) {
                strHtml.push("<div class=\"jg-tap\"><em>省</em><span>" + strSaveMoney + "</span></div>");
            }
            strHtml.push("</div>");
            strHtml.push("<span>" + (n.BuyCarDate == "未上牌" ? "未上牌" : (n.BuyCarDate + "上牌")) + " " + n.DrivingMileage + "公里</span></li>");
        }
    </script>

    <!-- 对比浮动框 -->
    <div id="divWaitCompareLayer" class="tc-popup-box y2015-rightfixed right-fixed" data-drag="true" style="display: none; margin-right: -360px;" data-page="compare" animateright="-533" animatebottom="229">
        <div class="tt" id="bar_minicompare" style="cursor: move;">
            <h6>车型对比</h6>
            <a class="b-close" href="javascript:void(0);" id="b-close">隐藏<i></i></a>
        </div>
        <div class="content">
            <ul id="idListULForWaitCompare" class="fixed-list"></ul>
            <div class="fixed-box">
                <div class="fixed-input" id="CarSelectSimpleContainerParent">
                    <input type="text" value="请选择车款" userful="showcartypesim" readonly="readonly" />
                    <%--<a class="right" href="javascript:void(0);"  onclick="javascript:WaitCompareObj.GetYiCheSug();" ><div class="star"><i></i></div></a>--%>
                    <div class="right" userful="showcartypesim">
                        <div class="star">
                            <i class="star-i"></i>
                        </div>
                    </div>
                    <div class="zcfcbox h398 clearfix" id="CarSelectSimpleContainer" style="display: none;"></div>
                </div>
                <div class="clear"></div>
                <div class="btn-sty button_orange"><a href="javascript:;" onclick="WaitCompareObj.NowCompare();">开始对比</a></div>
            </div>
            <div class="wamp">
                <em class="fixed-l">最多对比10个车款</em><a class="fixed-r" id="waitForClearBut" href="javascript:WaitCompareObj.DelAllWaitCompare();">清空车款</a>
                <div class="clear"></div>
            </div>
            <div class="alert-center" id="AlertCenterDiv" style="display: none;">
                <p>最多对比10个车款</p>
            </div>
        </div>
    </div>
    <!--漂浮层模板start-->
    <div class="effect" style="display: none;">
        <div class="car-summary-btn-duibi button_gray"><a href="javascript:;" target="_self"><span>对比</span></a></div>
    </div>
    <!--漂浮层模板end-->
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/jsnew/commons.js,carchannel/jsnew/carcompareforminiV3.min.js,carchannel/jsnew/carSelectSimpleV2.min.js?v=20160715"></script>
    <%--<script type="text/javascript" src="/jsnew/commons.js?v=20150724"></script>
    <script type="text/javascript" src="/jsnew/carcompareforminiV3.js?v=20150733"></script>
    <script type="text/javascript" src="/jsnew/carSelectSimpleV2.js"></script>--%>
    <script type="text/javascript">
        $(function(){
            WaitCompareObj.Init();
        });
    </script>
    <script type="text/javascript">
			<%
        if (Request.QueryString.Count == 1 && Request.QueryString["p"] == "18-25")
        { %>
        $("#divSerialSummaryMianBaoAD").html('<ins id="div_e722ad2a-4f5b-42e9-9e52-0dd4fe42e9c3" type="ad_play" adplay_IP="" adplay_AreaName=""  adplay_CityName=""    adplay_BrandID=""  adplay_BrandName=""  adplay_BrandType=""  adplay_BlockCode="e722ad2a-4f5b-42e9-9e52-0dd4fe42e9c3"> </ins>');
				<%} %>        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <%= bottomAd %>
    <!-- Baidu Button BEGIN -->
    <asp:Literal ID="litLevelBaiduShare" runat="server"></asp:Literal>
    <!-- Baidu Button END -->
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
    <%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
</body>
</html>
<script type="text/javascript" src="http://gimg.bitauto.com/js/senseNew.js"></script>
<script type="text/javascript">
    //$("#feedbackDiv").before("<li class=\"w4 d11-backtop\"><a href=\"http://survey01.sojump.com/jq/7431792.aspx\"  title=\"\" target=\"_blank\">问卷调查</a></li>");
    $(function(){
        //按口碑评分start
        $(".kb-px-b").hover(function(){
            $(this).removeClass("hover0");
            $("#c_koubei").addClass("curt");//
            $(this).show();
        },function(){
            $(this).addClass("hover0");
            var btn=$(this).parent().children("#c_koubei");
            setTimeout(function(){
                if(btn.hasClass("hover1")){
                    $("#c_koubei").removeClass("curt");//
                    $(this).hide();
                }
            },100);
            $(this).hide();
        });
        $("#c_koubei").hover(function(){
            $(this).removeClass("hover1");
            $(this).addClass("curt");//
            $(this).parent().children(".kb-px-b").show();
        },function(){
            var divThis=$(this).parent().children(".kb-px-b");
            setTimeout(function(){
                if(divThis.hasClass('hover0'))
                {
                    $(this).removeClass("curt");//
                    divThis.hide();
                }
            },100);
            $(this).addClass("hover1");
            $(this).removeClass("curt");//
        });
        //按口碑评分end
    });
</script>
<script>
    var _hmt = _hmt || [];
    (function() {
        var hm = document.createElement("script");
        hm.src = "https://hm.baidu.com/hm.js?7b86db06beda666182190f07e1af98e3";
        var s = document.getElementsByTagName("script")[0]; 
        s.parentNode.insertBefore(hm, s);
    })();
</script>

