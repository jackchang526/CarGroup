<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPhoto.aspx.cs" Inherits="WirelessWeb.CarPhoto" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE HTML>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>【<%=ce.Serial.SeoName %>图片】<%= ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name %>图片_<%= ce.Serial.SeoName%>图片大全-手机易车网
    </title>
    <meta name="Keywords" content="<%= ce.Serial.SeoName%>图片,<%= ce.Serial.SeoName%>外观,<%= ce.Serial.SeoName%>内饰图,<%= ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name%>,手机易车网,car.m.yiche.com" />
    <meta name="Description" content="<%= ce.Serial.Brand.MasterBrand.Name+ce.Serial.Name%>图片,展示各种角度<%= ce.Serial.SeoName%>图片,包括<%= ce.Serial.SeoName%>外观图片,<%= ce.Serial.SeoName%>内饰,<%= ce.Serial.SeoName%>内部空间,<%= ce.Serial.SeoName%>行驶等最新<%= ce.Serial.SeoName%>图片壁纸。" />
    <!--#include file="~/ushtml/0000/myiche2014_cube_summary-792.shtml"-->
</head>
<body>
    <script type="text/javascript">
        var carlevel = '<%=ce.Serial.Level.Name %>';
    </script>
    <div class="mbt-page">
        <!--#include file="~/html/header.shtml"-->
        <div class="b-return">
            <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a> <span>
                <%=ce.Serial.Name%>车款</span><a href="/<%=ce.Serial.AllSpell %>" class="opt">看车型</a>
        </div>
        <!-- 选择车款 start -->
        <a class="pic-change-car" href="javascript:selectCarMenu('m-popup-item');"><span>按车款</span><strong><%=ce.CarYear>0?ce.CarYear+"款 ":"" %><%=ce.Name %></strong>
        </a>
        <!-- 选择车款 end -->
        <!-- 车款卡片 start -->
        <div class="chek-card">
            <div class="wrap">
                <div class="fir-item">
                    参考成交价：<strong><%= cankaoPrice%></strong>
                </div>
                <ul>
                    <li>厂商指导价：<span><%= ce.ReferPrice > 0 ? ce.ReferPrice.ToString()+"万元" : "暂无"%></span></li>
                    <li>全款：<strong><%=totalPay%></strong><em>(仅供参考)</em><a href="/gouchejisuanqi/?carid=<%=carID %>"
                        class="m-ico-calculator"></a></li>
                    <li>贷款：<span><%=loanDownPay %></span><em>(首付)，</em><span><%=monthPay %></span><em>(3年)</em><a
                        href="/qichedaikuanjisuanqi/?carid=<%=carID %>" class="m-ico-calculator"></a></li>
                </ul>
            </div>
            <div class="sort sort4">
                <ul>
                    <li class="first"><a href="http://price.m.yiche.com/zuidijia/nc<%= carID%>/?leads_source=13301">
                        询价</a></li>
                    <li id="liDemand"></li>
                    <li><a href="http://m.huimaiche.com/<%=ce.Serial.AllSpell%>/?tracker_u=13_ycydsk">底价</a></li>
                    <li><a href="http://chedai.m.yiche.com/<%=ce.Serial.AllSpell%>/m<%=carID%>/chedai/?from=ycm4">贷款</a></li>
                </ul>
            </div>
        </div>
        <!-- 车款卡片 end -->
        <!-- tabs start -->
        <div class="tags-first mgt15">
            <ul>
                <li><a href="http://price.m.yiche.com/nc<%= carID %>/"><span>报价<s></s></span></a></li>
                <li><a href="/<%= ce.Serial.AllSpell %>/m<%= carID %>"><span>配置<s></s></span></a></li>
                <li class="current"><a href="/<%= ce.Serial.AllSpell %>/m<%= carID %>/tupian"><span>
                    图片<s></s></span></a></li>
            </ul>
        </div>
        <!-- tabs end -->
        <div>
            <%=carAllPhotoHTML%>
        </div>
    </div>
    <!-- black popup start -->
    <div class="leftmask" style="display: none;">
    </div>
    <div class="leftPopup" id="m-popup-item" style="display: none;">
        <div class="swipeLeft">
            <%=carList%>
        </div>
    </div>
    <!-- black popup end -->
    <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
    <script type="text/javascript">
        function selectCarMenu(PopId) {
            $("#" + PopId).show();
            $("#" + PopId + " .swipeLeft").addClass("swipeLeft-block");
            $(".leftmask").show();
            $(".leftPopup").css("zIndex", 199);

            var documentHeight = $(document).height(); // 页面内容的高度
            var leftPopupHeight = $("#" + PopId + " .swipeLeft").height();
            leftPopupHeight = (documentHeight > leftPopupHeight) ? documentHeight : leftPopupHeight; // 弹出层高度
            $('.leftmask, .leftPopup').css('height', leftPopupHeight);

        }
        $(".leftmask").click(function () {
            $(".swipeLeft").removeClass("swipeLeft-block");
            $(this).hide();
            $(".leftPopup").css("zIndex", 0).hide();
        });
    </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchor.js?v=201209"></script>
    <script type="text/javascript">
        var CarCommonCarID = '<%= carID.ToString() %>';
        var url = "http://car.m.yiche.com/<%= ce.Serial.AllSpell %>/";
    </script>
    <!--footer start-->
    <div class="footer15">
        <!--搜索框-->
        <!--#include file="~/html/footersearch.shtml"-->
        <!--#include file="~/html/footerV3.shtml"-->
    </div>
    <!--footer end-->
    <script type="text/javascript">
        var citySpell = "beijing",cityId=201;
        if (typeof bit_locationInfo != 'undefined' && bit_locationInfo != null && typeof bit_locationInfo.engName != "undefined")
        { citySpell = bit_locationInfo.engName; cityId = bit_locationInfo.cityId;}
        function getDemand(serialId, serialSpell, cityId) {
            $.ajax({ url: 'http://api.car.bitauto.com/mai/GetSerialDemand.ashx?serialId=' + serialId + '&cityid=' + cityId
			, async: false, cache: true, dataType: "jsonp", jsonpCallback: "newDemandCallback", success:
			function (data) {
			    var hasDemand = false;
			    if (typeof data != 'undefined' && data != null
				&& typeof data.DealerCount != 'undefined' && data.DealerCount != null && data.DealerCount > 0)
			    { hasDemand = true; }
			    if (hasDemand) {
			        var cityName = "北京";
			        if (typeof cityIDMapName != 'undefined' && cityIDMapName != null && typeof cityIDMapName[cityId] != "undefined")
			        { cityName = cityIDMapName[cityId]; }
			        var citySpell = "beijing";
			        if (typeof cityIDMapSpell != 'undefined' && cityIDMapSpell != null && typeof cityIDMapSpell[cityId] != "undefined")
			        { citySpell = cityIDMapSpell[cityId]; }
			        //添加车型特卖
                    var carId =<%=carID%>;
			        if (typeof data.CarList != 'undefined' && data.CarList != null) {
			            $.each(data.CarList, function (i, n) {
			               if ( n.CarID ==carId)
                           {
                             $("#liDemand").html("<a href=\"http://mai.m.yiche.com/detail-<%= ce.Serial.Id %>.html?leads_source=21102&city=" + citySpell + "#<%=carID %>\">特卖</a>");
                           }
			            });
			        }
			        
			    }
			}
            });
        }
    	getDemand(<%=ce.Serial.Id %>,'<%=ce.Serial.AllSpell %>',bit_locationInfo.cityId);
    	(function(){
    		//购车服务 2015.5.16
    		$.ajax({
    			url: "http://api.gouche.yiche.com/PreferentialCar/GetCarByCarId?carid=" + CarCommonCarID + "&cityid=" + cityId + "", cache: true, dataType: "jsonp", jsonpCallback: "guocheCallback", success: function (data) {
    				if (data != null) {
    					$(".chek-card .fir-item").append("<a href=\"http://gouche.m.yiche.com/" + citySpell + "/sb<%=ce.Serial.Id %>/m"+CarCommonCarID+"/\" class=\"ico-66gouche\">66购车节</a>");
					}
				}
			});
		})();
    </script>
    <script type="text/javascript">
        var zamplus_tag_params = {
        modelId:<%=ce.SerialId%>,
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
</body>
</html>
