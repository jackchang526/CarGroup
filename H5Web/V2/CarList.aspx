<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarList.aspx.cs" Inherits="H5Web.V2.CarList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>【<%= BaseSerialEntity.SeoName %>】车款-易车</title>
    <meta charset="utf-8"/>
    <meta name="Keywords" content="<%= BaseSerialEntity.SeoName %>,<%= BaseSerialEntity.SeoName %>报价,<%= BaseSerialEntity.SeoName %>图片,<%= BaseSerialEntity.SeoName %>口碑"/>
    <meta name="Description" content="易车网提供<%= BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%= BaseSerialEntity.SeoName %>优惠行情、<%= BaseSerialEntity.SeoName %>导购信息，最新<%= BaseSerialEntity.SeoName %>降价促销活动尽在易车网。"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期/css/style.css"/>
</head>
<body>
<div class="context_scroll" data-name="carlist">
    <div class="context_scroll_box">
        <!--内容开始-->
        <% if (Dealerid > 0)
           { %>
            <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
            <script type="text/javascript">
                var cityId = bit_locationInfo.cityId;
                //经销商车款列表
                document.write(unescape("%3Cscript src='/handlers/GetDateAsync.ashx?service=dealer&method=carsprice&csid=<%= SerialId %>&dealerid=<%= Dealerid %>' type='text/javascript'%3E%3C/script%3E"));
            </script>
        <% }
           else
           { %>
            <%= CarListHtml %>
        <% } %>
        <!--内容结束-->
        <div class="h60"></div>
        <!--#include file="~/inc/footer_script_common.shtml"-->
    </div>
</div>
</body>
</html>
<% if (Dealerid <= 0)
   { %>
    <script type="text/javascript">
        var currentYear = 0;
        $(function() {
            //初始化标签状态
            var maxyear = <%= MaxYear %>;
            $("#" + maxyear).addClass("current");
            $("#yearDiv" + maxyear).show();

            $("#yeartag li").bind("click", function() {
                $("#yeartag li").removeClass("current");
                $(this).addClass("current");
                currentYear = $(this).attr("id");
                $("div[id^='yearDiv']").hide();
                $("#yearDiv" + currentYear).show();
            });
        });
    </script>
    <script type="text/javascript">
        var pcount = <%= PageCount %>;
        var brokerid = <%= Brokerid %>;
        var dealerid = <%= Dealerid %>;
        var serialAllSpell = "<%= SerialAllSpell %>";
        var tabs = document.getElementById("yeartag").getElementsByTagName("li");
        var btnMore = {};
        $("a[id^='btnMoreCar']").click(function() {
            $(this).html("正在加载...");
            var rqString = "", self = $(this);
            var page = self.attr("page");
            var unionChar = rqString.indexOf("?") != -1 ? "&" : "?";
            if (rqString.indexOf("page") != -1) {
                rqString = rqString.replace("page=", "page=" + page);
            } else {
                rqString = rqString + unionChar + "page=" + page;
            }
            rqString = rqString + "&id=" + <%= SerialId %>;
            rqString = rqString + "&year=" + currentYear + "&serialAllSpell=" + serialAllSpell + "&brokerid=" + brokerid + "&dealerid=" + dealerid;
            $.ajax({
                url: "../handlers/H5GetMoreCars.ashx" + rqString,
                cache: true,
                success: function(data) {
                    if (data != "") {
                        var tempdiv = document.createElement("div");
                        tempdiv.innerHTML = data;
                        var childs = tempdiv.childNodes;
                        var doc = document.createDocumentFragment();
                        while (childs.length) {
                            doc.appendChild(childs[0]);
                        }
                        self.before(doc); //附加的位置
                        if (self.parent().attr("pagecount") > page) {
                            self.html("<i>加载更多</i>");
                        } else {
                            self.get(0).parentNode.removeChild(self.get(0));
                        }
                        page++;
                        self.attr("page", page);
                        ////start 加载更多后设置链接 20150716
                        //for (var k in baoxiaoOrImport) {
                        //    $("a[id^='car_filter_id_" + baoxiaoOrImport[k] + "']").attr("href", "http://m.yichemall.com/car/Detail/Index?carId=" + baoxiaoOrImport[k]+ "&source=myc-zs-loan-01");
                        //    $("a[id^='car_filterzuidi_id_" + baoxiaoOrImport[k] + "']").attr("href", "http://m.yichemall.com/car/Detail/Index?carId=" + baoxiaoOrImport[k] + "&source=myc-zs-onsale-01").html("商城购买");
                        //}
                        ////end
                    } else {
                        self.get(0).parentNode.removeChild(self.get(0));
                    }
                }
            });
        });
    </script>
<% } %>