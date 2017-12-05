<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarSummary.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageCarV2.CarSummary" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>【<%= cbe.Serial.ShowName%><%=cbe.Name %>报价_参数_油耗_图片】_<%= cbe.Serial.Brand.Name %>-易车网BitAuto.com</title>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />

	<!--#include file="~/ushtml/0000/yiche_2016_cube_chexingzongshu_style-1264.shtml"-->
	<meta name="Keywords" content="<%= cbe.Serial.ShowName%>,<%= cbe.Serial.ShowName%>报价,<%= cbe.Serial.ShowName%>论坛,<%= cbe.Serial.ShowName%>图片,<%= cbe.Serial.ShowName%>油耗,<%= cbe.Serial.ShowName%>口碑,<%= cbe.Serial.ShowName%>视频,<%= cbe.Serial.ShowName%>参数" />
	<meta name="Description" content="<%= cbe.Serial.ShowName%>:易车网(BitAuto.com)车型频道为您提供全国最新<%= cbe.Serial.ShowName%>报价,海量<%= cbe.Serial.ShowName%>图片,热门<%= cbe.Serial.ShowName%>论坛,权威<%= cbe.Serial.ShowName%>参数配置、安全评测、油耗、口碑、问答、视频、经销商等,是全国数千万购车意向客户首选汽车导购网站。" />
	<link rel="canonical" href="http://car.bitauto.com/<%=cbe.Serial.AllSpell %>/m<%=carID %>/" />
	<meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%= cbe.Serial.AllSpell %>/m<%= carID %>/" />
	<meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/carserial.aspx?modelid=<%= carID %>" />
</head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<input id="hidCarType" type="hidden" value="默认" />
	<input id="hidCarID" type="hidden" value="<%=carID %>" />

	<input id="isElectric" type="hidden" value="<%=isElectrombile%>" />
	<input id="isTingShou" type="hidden" value="<%=cbe.SaleState.Trim()%>" />
	<script type="text/javascript">
	    if (document.getElementById("isElectric").value == "True") {
	        document.getElementById("hidCarType").value="电动";
	    } else if (document.getElementById("isTingShou").value == "停销") {
	        document.getElementById("hidCarType").value="停销";
	    }
	    //if ($("#isElectric").val() == "True") {
	    //    $("#hidCarType").val("电动");
	    //} else if ($("#isTingShou").val() == "停销") {
	    //    $("#hidCarType").val("停销");
	    //}
	</script>

	<!--#include file="~/htmlv2/header2016.shtml"-->
	<!--a_d start-->
	<div style="width:1200px;margin:0 auto;">
		<%--<%=serialTopAdCode%>--%>
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" data-type="ad_play" data-adplay_IP="" data-adplay_AreaName="" data-adplay_CityName="" data-adplay_BrandID="" data-adplay_BrandName="" data-adplay_BrandType="" data-adplay_BlockCode="7e48ab6a-f563-413a-8427-5578aa3416f9"> </ins>
	</div>
	<!--a_d end-->
	<%= CarHeadHTML %>

    <!--内容 start-->
    <div class="container cardetail-section summary">
        <div class="row section-layout">
            <div class="col-xs-9">
                <div class="section-main">
                    <div class="layout-1 card-section">
                        <div class="card-layout special-card1">
                            <div class="section-header header1">
                                <div class="box">
                                    <h2>车款</h2>
                                    <div class="col-auto header-car-select">
                                        <a class="arrow-down" id="car-pop" href="javascript:;"><%=carName%></a>
                                         <%if (!string.IsNullOrEmpty(carListHtml))
                                              { %>
								                     <div class="drop-layer" id="car-popbox" style="display: none">
                                                         <%=carListHtml%>
                                                     </div>
                                            <%} %>
                                    </div>
                                </div>
                                <div id="carcompare_btn_new_<%=carID %>"><a class="btn btn-secondary btn-xs compare" target="_self"  data-use="compare" data-id="<%=carID %>" href="javascript:;">+对比</a></div>
                            </div>
                            <div class="content clearfix">
                                <div class="img">
                                    <a href="<%=ImgLink %>" target="_blank"><img src="<%=PicUrl %>" alt="<%=CarPicName %>" width="300" height="200"></a>
                                    <% if (PhotoCount>0)
                                       { %>
                                    <a href="<%= ImgLink %>" class="img-link" target="_blank"><%= PhotoCount %>张实拍图&gt;&gt;</a>
                                    <% }else
                                       { %>
                                    <a href="<%= ImgLink %>" class="img-link no-img" target="_blank"><%= CarPicName %></a>
                                    <% } %>
                                </div>
                                <div class="desc">
                                    <div class="top">
                                        <h5 id="policyTag">
                                            <%=cbe.SaleState=="停销"?"二手车报价：<em>"+ucarPrice+"</em>":"全国参考价：<em>"+carPrice +"</em>" %>
                                            <%if (TaxContent != "" && cbe.SaleState == "在销")
                                              { %>
								                     <a  title="<%=TaxContent %>" href="http://news.bitauto.com/sum/20170105/1406774416.html"  class="ico-shangchengtehui"><%= TaxContent=="免征购置税"?"免税":"减税" %></a>
                                            <%} %>
                                        </h5> 
                                    </div>
                                    <div class="mid row">
                                        <div class="col-xs-4">
                                            <em><%=cbe.SaleState=="待销"?"预售价":"厂商指导价" %></em>
                                            <h5><%= cfcs.ReferPrice == "" ? "暂无" : cfcs.ReferPrice + "万元"%></h5>
                                        </div>
                                        <%if (cbe.SaleState.Trim() != "停销")
                                          {%>
                                        <div class="col-xs-4">
                                            <em id="quanKuan">全款购车（供参考）</em>
                                            <h5 class="calc-title">
                                                <a class="em"><%=cfcs.CarTotalPrice == "" ? "暂无" : cfcs.CarTotalPrice + "元"%></a>
                                                <a href="/gouchejisuanqi/?carid=<%= carID.ToString() %>" class="calculator" target="_blank"></a>
                                            </h5>
                                           <%if (!string.IsNullOrEmpty(cfcs.CarTotalPrice.ToString().Trim()))
                                          {%>
                                            <!-- 贷款弹层 start-->
                                            <div class="popup-layout-1">
                                                <p>裸车价格： <em><%=priceComputer.FormatCarPrice %></em></p>
                                                <p>必要花费： <em><%=priceComputer.FormatEssentialPrice %></em></p>
                                                <p class="note">包含购置税、上牌、车船使用税、交强险</p>
                                                <p class="last">商业保险： <em><%=priceComputer.FormatInsurance %></em></p>
                                                <p>全款购车价：<em><%=cfcs.CarTotalPrice %></em></p>
                                                <p class="note bottom">以上均为预估费用，仅供参考。</p>
                                            </div>
                                            <!--弹层 end-->
                                            <%}%>
                                            <script type="text/javascript">
                                                (function() {
                                                    $("#quanKuan").mouseover(function() {
                                                        $("#loanLayer").show();
                                                    }).mouseout(function() {
                                                        if ($(this).closest("#loanLayer").length == 0) {
                                                            $("#loanLayer").hide();    
                                                        }
                                                    });
                                                })();
                                            </script>
                                        </div>

                                         <div class="col-xs-4">
                                            <em>贷款购车（30%首付）</em>
                                            <h5><a class="em" target="_blank" href="http://fenqi.taoche.com/www/<%=cbe.Serial.AllSpell%>/m<%=cbe.Id%>/?from=yc18&amp;leads_source=p003003">首付<%= priceComputer.LoanFirstDownPayments > 0 ? ((double)(priceComputer.LoanFirstDownPayments + priceComputer.AcquisitionTax + priceComputer.Compulsory + priceComputer.Insurance + priceComputer.VehicleTax + priceComputer.Chepai) / 10000).ToString("F2") + "万元" : "暂无"%></a></h5>
                                        </div>
                                        <%}%>
                                    </div>
                                    <div class="foot">
                                        <div class="btn-group">
                                            <%if (cbe.SaleState == "停销")
                                              {%>
                                            <a class="btn" target="_blank" href="http://www.taoche.com/buycar/b-<%= cbe.Serial.AllSpell %>/?page=1&leads_source=p003005&carid=<%= carID %>&ref=pc_yc_cxzs_gs_escbuycar">买二手车</a>
                                            <a class="btn" target="_blank" href="http://www.taoche.com/pinggu/?ref=pc_yc_cxzs_gs_gujia">二手车估价</a>
                                            <%}
                                              else if (cbe.SaleState == "在销")
                                              { %>
                                            <a class="btn" target="_blank" href="http://dealer.bitauto.com/zuidijia/nb<%=cbe.SerialId%>/nc<%=cbe.Id%>/?T=2&leads_source=p003001">询底价</a>
                                   <%--         <a class="btn" target="_blank" href="http://www.huimaiche.com/<%=cbe.Serial.AllSpell%>?carid=<%=cbe.Id%>&amp;tracker_u=609_ckzs&leads_source=p003002">买新车</a>--%>
                                            <a class="btn" target="_blank" href="http://sq.taoche.com/yiche/index?carid=<%= cbe.Id %>&from=yc18&leads_source=p003003">贷款</a>
                                            <a class="btn" target="_blank" href="http://zhihuan.taoche.com/?leads_source=p003004&ref=pc_yc_cxzs_gs_zhihuan&serial=<%=cbe.SerialId%>">置换</a>
                                            <a class="btn" target="_blank" href="http://www.taoche.com/<%=cbe.Serial.AllSpell%>/?leads_source=p003005&ref=pc_yc_cxzs_gs_esc">二手车</a>
                                            <%} %>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                     <%if (!string.IsNullOrEmpty(carConfigData))
                    {%>
                        <div class="layout-2 config-section">
                        <div class="section-header header2">
                            <div class="box">
                                <h2>参数配置</h2>
                                <span class="header-note1">注: ● 标配&nbsp;&nbsp;○ 选配&nbsp;&nbsp;- 无</span>
                            </div>
                        </div>
                        <%= carConfigData %>
                        <div class="btn-box1">
                            <a class="btn btn-default" href="javascript:void(0);"><span class="more">更多参数配置</span></a>
                        </div>
                    </div>
                     <%} %>

                    <script type="text/javascript">
                        var pageCarLevel='<%=cfcs.CarLevel%>';
                        function showNewsInsCode(dxc, xxc, mpv, suv) {
                            var adBlockCode = xxc;
                            if (pageCarLevel == '中大型车' || pageCarLevel == '中型车' || pageCarLevel == '跑车' || pageCarLevel == '豪华车') {
                                adBlockCode = dxc;
                            }
                            else if (pageCarLevel == '微型车' || pageCarLevel == '小型车' || pageCarLevel == '紧凑型车') {
                                adBlockCode = xxc;
                            }
                            else if (pageCarLevel == '概念车' || pageCarLevel == 'MPV' || pageCarLevel == '面包车' || pageCarLevel == '皮卡'|| pageCarLevel == '客车' || pageCarLevel == '卡车'|| pageCarLevel == '其它') {
                                adBlockCode = mpv;
                            }
                            else if (pageCarLevel == 'SUV') {
                                adBlockCode = suv;
                            }
                            document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
                        }
                        showNewsInsCode('540f1a0b-4eaf-4297-804e-f703866ef3d6','57869922-fb6f-4044-be64-27d3dfa13e80', 'd3032347-a8df-4fc4-883e-7737baf660f4', 'c6856633-1ab1-49f7-b442-995c53d7559d');
                    </script>

                    <!-- 经销商start -->
                    <script type="text/javascript">
                        document.write('<ins Id=\"ep_union_138\" Partner=\"1\" Version=\"\" isUpdate=\"1\" type=\"4\" city_type=\"1\" city_id=\"'+bit_locationInfo.cityId+'\" city_name=\"0\" car_type=\"3\" brandId=\"0\" serialId=\"0\" carId=\"<%= carID %>\"></ins>');
                    </script>
                    <!-- 经销商end -->
                    <!-- 网友用它和谁比start -->
                    <%= carHotCompareHtml %>
                    <!-- 网友用它和谁比end -->
                    <!-- SEO底部热门 -->
                     <!-- SEO底部热门 -->
                        <!--#include file="~/include/special/seo/00001/201701_pinpaiye_tj_Manual.shtml"-->
                        <!-- SEO底部热门 -->
                    <!-- SEO底部热门 -->
                </div>
            </div>
            <div class="col-xs-3">
                 <div class="section-right">
                     <!-- ad -->
                     <div class="layout-1 adv-sidebar">
                         <ins id="div_20b7e84d-7a5d-461b-b484-b3d0fec5a81e" data-type="ad_play" data-adplay_ip="" data-adplay_BrandID="<%=cbe.Serial.Id %>"  data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="20b7e84d-7a5d-461b-b484-b3d0fec5a81e"></ins>
                     </div>
                     <div class="layout-1 adv-sidebar">
                         <ins id="div_9e066309-54d0-4928-a251-b2f3c86ddc4d" data-type="ad_play" data-adplay_ip="" data-adplay_BrandID="<%=cbe.Serial.Id %>"  data-adplay_areaname="" data-adplay_cityname="" data-adplay_brandid="" data-adplay_brandname="" data-adplay_brandtype="" data-adplay_blockcode="9e066309-54d0-4928-a251-b2f3c86ddc4d"></ins>
                     </div>
                     <!-- 热门车型 -->
                     <div class="layout-1 samebrandcar-sidebar">
                         <%= hotCars %>
                     </div>
                     <!-- 子品牌还关注 -->
                     <div class="layout-1 looking-sidebar">
                         <h3 class="top-title">看了还看</h3>
                         <%= SerialToSerialHtml %>
                     </div>
                     <!--此品牌下其别子品牌-->
                     <div class="layout-1 samebrandcar-sidebar">
                         <%=GetBrandOtherSerial() %>
                     </div>
                  <%--   <%if (cbe.SaleState.Trim() != "停销")
                       { %>
                     <div class="layout-1 relateoldcar-sidebar" id="line_boxforucar_box" style="display: none;">
                         <h3 class="top-title"><a target="_blank" href=" http://www.taoche.com/<%=cbe.Serial.AllSpell %>/">
                             <%= cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>二手车
                         </a></h3>
                         <div class="list-box" id="ucar_serialcity">
                         </div>
                     </div>
                     <%}
                     %>--%>
                      <div class="layout-1 relateoldcar-sidebar" id="line_boxforucar_box" style="display: none;">
                         <h3 class="top-title"><a target="_blank" href=" http://www.taoche.com/<%=cbe.Serial.AllSpell %>/?ref=pc_yc_cxzs_tj_esc">
                             <%= cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>二手车
                         </a></h3>
                         <div class="list-box" id="ucar_serialcity">
                         </div>
                     </div>

                     <%--<div class="layout-1">
                         <script type="text/javascript" id="zp_script_246" src="http://mcc.chinauma.net/static/scripts/p.js?id=246&w=240&h=220&sl=1&delay=5"
                             zp_type="1"></script>
                     </div>--%>
                 </div>
            </div>
        </div>
    </div>

    <!-- 调用尾start -->
    <script type="text/javascript">
        var CarCommonBSID = "<%= (cbe.Serial == null || cbe.Serial.Brand == null) ? 0 : cbe.Serial.Brand.MasterBrandId %>"; //大数据组统计用
        var CarCommonCBID = '<%= cbe.Serial == null ? 0 : cbe.Serial.BrandId %>';
        var CarCommonCSID = '<%= cbe.Serial == null ? 0 : cbe.Serial.Id %>';
        var CarCommonCarID = '<%= carID.ToString() %>';
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript">
        OldPVStatistic.ID1 = "<%=cbe.Serial.Id %>";
        OldPVStatistic.ID2 = "<%=carID.ToString() %>";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    </script>
    <!-- baa 浏览过的车型-->
    <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
    <script type="text/javascript">
        try {
            Bitauto.UserCars.addViewedCars('<%=cbe.Serial.Id %>');
        }
        catch (err)
        { }
    </script>
     <!-- 调用尾end -->
    <!--#include file="~/htmlv2/rightbar.shtml"-->
	<!--#include file="~/htmlv2/footer2016.shtml"-->
	<%if (carID == 101688 || carID == 103759)
   { %>
	<!--百度热力图-->
	<div style="display: none">
		<script type="text/javascript">
		    var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		    document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
		</script>
	</div>
	<%} %>
	<!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
	<script type="text/javascript">
	    var  zamCityId= (typeof bit_locationInfo !="undefined")?bit_locationInfo.cityId:"201",
			modelStr = '<%=cbe.Serial.Id%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
		var zamplus_tag_params = {
		    modelId:modelStr,
		    carId:<%=carID%>
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
	<!--内容 end-->
</body>
</html>

<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"> </script>
<script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_iframedialog,ibt_useractivatordialog"> </script>
<script type="text/javascript">
    String.prototype.trim = function() {  
        return this.replace(/(^\s*)|(\s*$)/g, "");  
    }  

    $(function() {
        //绑定“加载更多配置”事件
        $(".btn-box1 .btn-default").bind("click",function(){
            $(".moreinfo").show();
            $(this).hide();
        });
        //$(".ico-shangchengtehui").hover(function () {$(".tc-xunjia").show(); }, function () { $(".tc-xunjia").hide(); });
    });
</script>

<script src="http://image.bitautoimg.com/carchannel/jsnewV2/ucarserialcity.min.js?v=2016122815" type="text/javascript"></script>
<script type="text/javascript">
    (function(){
        var timer;
        $("#car-pop,#car-popbox").hover(function(){clearTimeout(timer);$("#car-popbox").show();},function(){timer = setTimeout(function () { $("#car-popbox").hide(); }, 500);});
        $("#color-sty em").hover(function(){$(this).addClass("current").find(".tc.tc-color-box").show();},function(){$(this).removeClass("current").find(".tc.tc-color-box").hide();});
        var cityId = 201,saleState="<%=cbe.SaleState.Trim()%>";
		if(typeof (bit_locationInfo) != 'undefined'){
		    cityId = bit_locationInfo.cityId;
		}
		if(typeof(showUCar)!="undefined"){ 
		    //if(saleState!="停销"){
		    showUCar(<%=cbe.Serial.Id %>, cityId,'<%=cbe.Serial.AllSpell %>','<%=cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>',getUCarForSider,undefined,undefined,undefined,'chekuan');
		    <%--}
		    else{
		        showUCar(<%=cbe.Serial.Id %>, cityId,'<%=cbe.Serial.AllSpell %>','<%=cbe.Serial.ShowName.Replace("(进口)", "").Replace("（进口）", "")%>',getUCarForBottom,undefined,undefined,undefined,'chekuan'); 
		    } --%>
		}

        //补贴
        $.ajax({
            url: "http://cdn.partner.bitauto.com/NewEnergyCar/CarSubsidy.ashx?op=getcscarsunsidy&csid=" + CarCommonCSID + "&cityid=" + cityId,
            dataType: "jsonp",
            jsonpCallback: "getSubsidyCallback",
            cache: true,
            success: function (data) {
                if (!(data && data.length > 0)) return;
                $.each(data, function (i, n) {
                    if (!(n.StateSubsidies > 0 && n.LocalSubsidy > 0)) return;
                    if (n.CarId && n.CarId == CarCommonCarID) {
                        var carLine = $("#policyTag");
                        var sub = [];
                        if (n.StateSubsidies > 0)
                            sub.push("国家补贴" + n.StateSubsidies + "万元");
                        if (n.LocalSubsidy > 0)
                            sub.push("地方补贴" + n.LocalSubsidy + "万元");
                        var b = " <a href=\"http://news.bitauto.com/zcxwtt/20140723/1206470614.html\" class=\"ico-shangchengtehui\" title=\"" + sub.join(",") + "\" target=\"_blank\">补贴</a>";
                        if (carLine.find("em:first").length > 0) {
                            carLine.find("em:first").after(b);
                        }
                    }
                });
            }
        });

	})();
    //二手车数据，此方法html输出未作修改，为1200之前版本;  
    function getUCarForBottom(data, csId, csSpell, csShowName) {
        try {
            data = data.CarListInfo;
            if (data.length <= 0) return;
            var strHtml = [];
            strHtml.push("<div class=\"title-con\">");
            strHtml.push("<div class=\"title-box title-box2\">");
            strHtml.push("<h4><a target=\"_blank\" href=\"http://car.bitauto.com/" + csSpell + "/ershouche/?ref=pc_yc_cxzs_tj_esc\">相关二手车</a></h4>");
            strHtml.push("</div>");
            strHtml.push("</div>");
            strHtml.push("<div class=\"cxdb-box taoche-box\">");
            strHtml.push("<ul>");
            $.each(data, function (i, n) {
                if (i > 3) return;
                var className = "";
                if (i == 0) className = "first";
                if (i == 3) className = "last";
                strHtml.push("<li class=\"" + className + "\"><div class=\"img-box\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\" class=\"pic\"><img width=\"150\" height=\"100\" src=\"" + n.PictureUrl + "\"></a></div>");
                strHtml.push("<div class=\"txt-box\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\">" + n.BrandName + "</a></div>");
                strHtml.push("<strong>" + n.DisplayPrice + "</strong>");
                strHtml.push("<span>" + n.BuyCarDate.substring(2) + "上牌 " + n.DrivingMileage + "公里</span></li>");
            });
            strHtml.push("</ul>");
            strHtml.push("</div>");
            strHtml.push("<div class=\"clear\"></div>");

            $("#ucarlist").html(strHtml.join(''));
        } catch (e) { }
    }
    //此方法中的html输出已改成1200页面
    function getUCarForSider(data, csId, csSpell, csShowName) {
        try {
            data = data.CarListInfo;
            if (data.length <= 0) return;
            var strHtml = [];
            $.each(data, function (i, n) {
                if (i > 5) return;
                strHtml.push("<div class=\"img-info-layout img-info-layout-14093\">");
                strHtml.push("<div class=\"img\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "&leads_source=p003010\">");
                strHtml.push("<img src=\"" + n.PictureUrl + "\"></a>");
                strHtml.push("</div>");
                strHtml.push("<ul class=\"p-list\">");   //应顾晓要求，去掉no-wrap样式； 2016-12-14  
                strHtml.push("<li class=\"name\"><a title=\"" + n.BrandName + "\" target=\"_blank\" href=\"" + n.CarlistUrl + "&leads_source=p003010\">" + n.BrandName + "</a></p>");
                strHtml.push("<li class=\"info\">" + n.BuyCarDate + "上牌 " + n.DrivingMileage + "公里</p>");
                strHtml.push("<li class=\"price\"><a target=\"_blank\" href=\"" + n.CarlistUrl + "&leads_source=p003010\">" + n.DisplayPrice + "</a></p>");
                strHtml.push("</ul>");
                strHtml.push("</div>");
            });
            //有二手车数据才显示此块,否则不显示
            if(strHtml.length>0)
            {
                $("#line_boxforucar_box").show().find("#ucar_serialcity").html(strHtml.join(''));
            }
        } catch (e) { }
    }
    (function () {
        $("#more-color").toggle(function () { $(this).addClass("more-current"); $("#more-color-sty").show(); }, function () { $(this).removeClass("more-current"); $("#more-color-sty").hide(); });
        //纠错
        var strError = "车款名称：<%=carFullName %>\n问题描述：\n联系方式(电话/QQ)：";
		$("a[name='correcterror']").click(function () { $("#popup-box").showCenter(); $("#correctError").val(strError); });
		$("#popup-box .btn-close,#btnErrorCancel").click(function() {
		    $("#fade").hide();
		    $("#popup-box").hide();
		});
		$("#popup-box-success .btn-close,#btn-success-close").click(function() {
		    $("#fade").hide();
		    $("#popup-box-success").hide();
		});
		$("#popup-box a[name='btnCorrectError']").click(function (e) {
		    e.preventDefault();
		    var content = $("#correctError").val();
		    if (content == "" || content == strError) {
		        $("#popup-box .alert span").html("请输入提交内容。");
		        return;
		    } $("#popup-box .alert span").html("");
		    var url = "http://www.bitauto.com/FeedBack/api/CommentNo.ashx?txtContent=" + encodeURIComponent(content) + "&satisfy=1&ProductId=5&categorytype=2&csid=" + (typeof (CarCommonCSID) != "undefined" ? CarCommonCSID : 0);
		    $.ajax({ url: url, dataType: 'jsonp', jsonp: "XSS_HTTP_REQUEST_CALLBACK", jsonpCallback: "errorCallback", success: function (data) {
		        if (data.status == "success") {
		            $("#popup-box").hide();
		            $("#popup-box-success").showCenter().find(".tit").html("提交成功！").siblings("p").html("侦察兵，好样的！");
		        } else {
		            $("#popup-box-success").showCenter().find(".tit").html("提交失败！").siblings("p").html(data.message);
		        }
		    }
		    });
		});
	})();
    //元素居中
    !function ($) {
        $.fn.showCenter = function() {
            $("#fade").show().css({ width: $(document).width(), height: $(document).height() });
            var top = ($(window).height() - this.height()) / 2;
            var left = ($(window).width() - this.width()) / 2;
            var scrollTop = $(document).scrollTop();
            var scrollLeft = $(document).scrollLeft();

            if (!-[1,] && !window.XMLHttpRequest) {
                return this.css({ position: 'absolute', 'z-index': 10020, 'top': top + scrollTop, left: left + scrollLeft }).show();
            } else {
                return this.css({ position: 'fixed', 'z-index': 10020, 'top': "150px", left: left }).show();
            }
        };
    } (jQuery);
</script>
<script type="text/javascript">
    function ImageMouseoverHandler() {
        var bigImg = document.getElementById(this.id.replace("Small", "Big"));
        var curSmallImg = document.getElementById(curImg.id.replace("Big", "Small"));
        var current = document.getElementById(this.id.replace("Small", "Current"));
        curImg.style.display = "none";
        bigImg.style.display = "block";
        //curSmallImg.className = "";
        //this.className = "current";
        curImg = bigImg;
    }
    var curImg = document.getElementById("focusBigImg_0");
    for (j = 0; j < 3; j++) {
        var imgId = "focusSmallImg_" + j;
        var img = document.getElementById(imgId);
        if (img)
            img.onmouseover = ImageMouseoverHandler;
        else
            break;
    }
    $(".img-list > div").hover(function () { $(this).find("a>span").show(); $(this).siblings().find("a>span").hide(); }, function () { })
</script>
<script type="text/javascript">
    if (typeof (bitLoadScript) == "undefined"){
        bitLoadScript = function (url, callback, charset) {
            var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
            if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
            else { s.onload = function () { if (callback) callback(); }; }
            s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
        };
    }
</script>

<!-- 经销商代码 -->
<script type="text/javascript" src="http://js.bitauto.com/dealer/union/script/ads.js"></script>
