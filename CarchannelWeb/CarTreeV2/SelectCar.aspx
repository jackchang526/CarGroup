<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCar.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTreeV2.Cartree_SelectCar" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <title><%=pageTitle%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="keywords" content="<%=metaKeywords %>" />
    <meta name="description" content="<%=metaDescription %>" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <!--#include file="~/ushtml/0000/yiche_2016_cube_chexingshuxing_style-1259.shtml"-->
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
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <div id="topAd" class="top-adv-box">
        <%=topAd %>
    </div>
    <!--头部导航-->
    <!--#include file="~/include/pd/2016/common/00001/201607_ejdh_common_Manual.shtml"-->
    <!--左侧树形-->

    <div class="container cartype-section screen-after">
        <div class="col-xs-3">
            <div class="treeNav" id="treeNav">
            </div>
        </div>
        <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
        <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/lefttree.v2.min.js"></script>
        <%--<script type="text/javascript" src="/jsnewv2/lefttree.v2.js"></script>--%>
        <script type="text/javascript">
            BitautoLeftTreeV2.init({
                likeDefLink: "http://car.bitauto.com/{allspell}/",
                params: {
                    tagtype: "chexing",
                    pagetype: "masterbrand",
                    objid: 0
                }
            });
        </script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/newselectcartoolv4.min.js?d=201707041043"></script>
        <%--<script type="text/javascript" src="/jsnewv2/newselectcartoolv4.js?d=20161228"></script>--%>
        <div class="col-xs-9">
            <div class="cartype-section-main">
                <!--面包屑-->
                <div class="crumbs h-line">
                    <a id="anchorTitle"></a>
                    <div class="crumbs-txt">
                        <span>当前位置：</span><a href="http://www.bitauto.com/">易车</a> &gt; <a href="http://car.bitauto.com/">车型</a> &gt; <strong>按条件选车</strong>
                    </div>
                </div>
                <!--/面包屑-->
                <!-- AD by chengl 2017-6-2 -->
                <script type="text/javascript">
                    function GetQueryString(name)
                    {
                        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
                        var r = window.location.search.substr(1).match(reg);
                        if(r!=null)return  unescape(r[2]); return null;
                    }
                    var levelID = GetQueryString("l")
                    var adBlockCode = "";
                    var pathname = location.pathname&&location.pathname.toLowerCase();
                    if(pathname.indexOf("zhongxingche")>=0 ||levelID=="5")
                    {
                        adBlockCode = 'b9f32607-f7bc-4ed9-8216-b97a461f6a21';
                    }
                    else if(pathname.indexOf("jincouxingche")>=0 || levelID=="3"){
                        adBlockCode="31aa4e85-9407-42e1-887f-e811a587568c";
                    }
                    else if(pathname.indexOf("zhongdaxingche")>=0 || levelID=="4"){
                        adBlockCode="cf912398-f6e3-4b12-8a89-c2af9f9a2e16";
                    }
                    else if (pathname.indexOf("haohuaxingche")>=0 || levelID=="6"){
                        adBlockCode="636406f2-7ec7-41bb-8afa-24cf23b2d5d4";
                    }
                    if(adBlockCode != ""){
                        document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
                    }
                </script>
                <!-- -->
                <div class="main-inner-section condition-selectcar clearfix">
                    <div class="section-header header1">
                        <div class="box">
                            <h2>按条件选车</h2>
                        </div>
                        <div class="more" id="selectcarmore"><a href="http://www.bitauto.com/zhuanti/daogou/gsqgl/" target="_blank">购车流程</a></div>
                    </div>
                    <div class="treeMainv1">
                        <%=this._ConditionsHtml %>
                    </div>
                </div>
                <!--车型列表-->
                <div class="main-inner-section cartype-list">
                    <div class="section-header header2 h-default2">
                        <%=serialListHtml%>
                    </div>
                    <div class="row block-4col-180" id="divContent">
                    </div>
                    <div class="note-box note-empty type-2" id="noResult" style="display: none; margin-top: 30px;">
                        <div class="ico"></div>
                        <div class="info">
                            <h3>抱歉，未找到合适的车型</h3>
                            <p class="tip">请修改条件再次查询</p>
                            <div class="more">
                                <span>您还可以查看：</span>
                                <ul class="list list-gapline sm">
                                    <li>
                                        <a href="http://www.taoche.com/all/" target="_blank">易车二手车</a>
                                    </li>

                                </ul>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="divPageRow">
                        <div class="pagination">
                            <div id="divPage">
                            </div>
                        </div>
                    </div>

                </div>
                <%--<div class="line-box">
                    <div class="no-txt-box">
                        <p class="tit">抱歉，未找到合适的车型</p>
                        <p>请修改条件再次查询，或者去 <a href="http://www.taoche.com/all/" target="_blank">易车二手车</a> 看看</p>
                    </div>
                    <div class="clear">
                    </div>
                </div>--%>
                <!--热门新车-->
                <%=hotSerialHtml%>
                <!--/热门新车-->
                <!--优惠推荐-->
                <%--<div class="main-inner-section reduce-recommend" id="gouchelist">
                </div>--%>
                <!--/优惠推荐-->
                <!--选二手车-->
                <div class="main-inner-section choose-oldcar" id="ucarlist">
                </div>
                <!--/选二手车-->
            </div>
            <!--#include file="~/htmlv2/rightbar.shtml"-->
            <!--#include file="~/htmlv2/treefooter2016.shtml"-->
        </div>
    </div>
    <script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js?v=20170307"></script>
    <script type="text/javascript">
        var ad_carlistdata = <%=adCarListData%>;
    </script>

    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/jquery.pagination.min.js"></script>
    <script type="text/javascript">
        conditionObj.Type = "car";
        conditionObj.InitPageCondition();
    </script>

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
                    strHtml.push("<div class=\"section-header header2\">");
                    strHtml.push("    <div class=\"box\">");
                    strHtml.push("        <h2><a href=\""+moreTaoCheUrl+"\" target=\"_blank\">选二手车</a></h2>");
                    if (data.DataCount > 0) {
                        strHtml.push("        <span class=\"header-note1\">共"+dataCount+"个车源</span>");
                    }
                    strHtml.push("    </div>");
                    strHtml.push("    <div class=\"more\"><a href=\""+moreTaoCheUrl+"\" target=\"_blank\">更多&gt;&gt;</a></div>");
                    strHtml.push("</div>");
                    strHtml.push("<div class=\"row block-4col-180\" id=\"ucar-carlist\">");
                    if (tsMsg.length > 0)
                    {
                        strHtml.push(tsMsg);
                    }
                    $.each(dataCarListInfo, function (i, n) {
                        if (i > 11) return;
                        loadCarDetailInfo(strHtml, i, n);
                    });
                    
                    strHtml.push("</div>");
                    if (dataCarListInfo.length > 12) {
                        strHtml.push("<div class=\"row load-more\"><a class=\"btn btn-default\" href=\"javascript:;\">加载更多车源</a></div>");
                    }
			
                    $("#ucarlist").html(strHtml.join(''));

                    //加载更多车源事件
                    $(".btn-default").one("click", function () {
                        if (dataCarListInfo && dataCarListInfo.length > 12) {
                            var strMoreCarHtml = [];
                            $.each(dataCarListInfo, function (i, n) {
                                if (i > 11) {
                                    loadCarDetailInfo(strMoreCarHtml, i, n);
                                }
                            });
                            $("#ucar-carlist").append(strMoreCarHtml.join(''));
                            if (dataCarListInfo.length >= 24) {
                                $(this).html('查看更多车源<em></em>');
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
            /*
            //优惠购车
            $.ajax({
                url: "http://api.gouche.yiche.com/selectcar/getpreference?cityid=" + cityId + "&page=1&pagesize=4" + queryUrl, cache: true, dataType: "jsonp", jsonpCallback: "goucheCallback", success: function (data) {
                    if(data&&data.SerialList.length<=0) return;
                    var strHtml=[];
                    strHtml.push("<div class=\"section-header header2\">");
                    strHtml.push("<div class=\"box\">");
                    strHtml.push("    <h2>优惠推荐</h2>");
                    strHtml.push("</div>");
                    strHtml.push("</div>");
                    strHtml.push("<div  class=\"row block-4col-180\" id=\"gouche-carlist\">");
                    $.each(data.SerialList,function(i,n){
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
                        strHtml.push("<div class=\"col-xs-3\">");
                        strHtml.push("<div class=\"img-info-layout-vertical img-info-layout-vertical-center img-info-layout-vertical-180120\">");
                        strHtml.push("    <div class=\"img\">");
                        strHtml.push("        <a target=\"_blank\" href=\""+url+"\"><img src=\""+n.ImageUrl.replace("_6.","_3.")+ "\"></a>");
                        strHtml.push("   </div>");
                        strHtml.push("   <ul class=\"p-list\">");
                        strHtml.push("       <li class=\"name\"><a href=\""+url+"\" target=\"_blank\">"+n.SerialName+"</a></li>");
                        strHtml.push("       <li class=\"price\"><a href=\""+url+"\" target=\"_blank\">"+(n.ReducedRate*10).toFixed(1)+"折起</a></li>");
                        strHtml.push("       <li class=\"info\">"+n.ShortRemarks+"</li>");
                        strHtml.push("   </ul>");
                        strHtml.push("</div>");
                        strHtml.push("</div>");                    });
                    //strHtml.push("</ul>");
                    strHtml.push("</div>");
                    $("#gouchelist").html(strHtml.join(''));
                    //优惠购车统计
                    statGouche();
                }});
        */

        })();
        function statTaoche() {
            bitLoadScript('http://image.bitautoimg.com/stat/PageAreaStatistics.js', function () {
                PageAreaStatistics.init("294,295,296,297");
            }, 'utf-8');
        }
        //function statGouche() {
        //    bitLoadScript('http://image.bitautoimg.com/stat/PageAreaStatistics.js', function () {
        //        PageAreaStatistics.init("364,363");
        //    }, 'utf-8');
        //}
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
            var url =  n.CarlistUrl + "&leads_source=p045005";
            var strDisplayPrice = specialFloatToStr(n.DisplayPrice);
            var strSaveMoney = specialFloatToStr(n.SaveMoney);
            strHtml.push("<div class=\"col-xs-3\">");
            strHtml.push("    <div class=\"img-info-layout-vertical  img-info-layout-vertical-180120-2\">");
            strHtml.push("        <div class=\"img\">");
            strHtml.push("            <a href=\""+url+"\" target=\"_blank\"><img src=\""+n.PictureUrl.replace("/1f/","/1d/")+"\"></a>");
            strHtml.push("        </div>");
            strHtml.push("        <ul class=\"p-list\">");
            strHtml.push("            <li class=\"name\"><a href=\""+url+"\" target=\"_blank\">"+n.BrandName +"</a></li>");
            strHtml.push("            <li class=\"price\">"+strDisplayPrice+ (strSaveMoney == "0" ? "" :" <span class=\"tag\">省"+strSaveMoney+"</span>") + "</li>");
            strHtml.push("            <li class=\"info\">"+(n.BuyCarDate == "未上牌" ? "未上牌" : (n.BuyCarDate + "上牌")) + " " + n.DrivingMileage +"公里</li>");
            strHtml.push("        </ul>");
            strHtml.push("    </div>");
            strHtml.push("</div>");        }
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
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
</body>
</html>
<script type="text/javascript" src="http://gimg.bitauto.com/js/senseNew.js"></script>
<script type="text/javascript">
    //$("#feedbackDiv").before("<li class=\"w4 d11-backtop\"><a href=\"http://survey01.sojump.com/jq/8460070.aspx\"  title=\"\" target=\"_blank\">问卷调查</a></li>");
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
