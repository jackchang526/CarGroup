/**
*===============================================================
*淘车网接口调用脚本文件
*<pre>
*</pre>
*@author:  sk(songkai@bitauto.com)
*@date:    2012-05-14
*===============================================================
*/
var TaoChe = {};
//购买相关二手车
TaoChe.showBuyUCar = function (csId, cityId, csSpell, csShowName, jqcontainer) {
    var container = "#showbuyucar";
    if (typeof jqcontainer != "undefined") {
        container = jqcontainer;
    }
    $.ajax({
    	url: "http://yicheapi.taoche.cn/CarSourceInterface/ForJson/carlistforyiche.ashx?sid=" + csId + "&ctid=" + cityId,
        //cache: false,
        dataType: "jsonp",
        jsonpCallback: "buy_callback",
        success: function (data) {
            data = data.CarListInfo;
            if (data.length <= 0) return;
            var str = "";
            str += "<h3>";
            str += "    <span><a target=\"_blank\" href=\"" + data[0].MoreCarUrL + "\" rel=\"nofollow\">相关二手车</a></span><div class=\"city_list\"></div>";
            str += "</h3>";
            str += "<div class=\"table_style\">";
            str += "    <table class=\"table_dealer\">";
            str += "        <thead>";
            str += "           <tr><th>车型名称</th><th>地区</th><th>上牌日期</th><th>颜色</th><th>行驶里程</th><th>价格</th><th>经销商</th></tr>";
            str += "        </thead>";
            str += "        <tbody>";
            $.each(data, function (i, n) {
                str += "            <tr class=\"list\">";
                str += "                <td>";
                str += "                    <a target=\"_blank\" href=\"" + n.CarlistUrl + "\">" + n.BrandName + "</a>";
                str += "                </td>";
                str += "                <td>";
                str += "                    <a target=\"_blank\" href=\"" + n.CityUrL + "\" rel=\"nofollow\">" + n.cityName + "</a>";
                str += "                </td>";
                str += "                <td>" + n.BuyCarDate + "</td>";
                str += "                <td>" + n.Color + "</td>";
                str += "                <td>" + n.DrivingMileage + "公里</td>";
                str += "                <td>" + n.DisplayPrice + "</td>";
                str += "                <td>";
                str += "                    <a target=\"_blank\" href=\"" + n.VendorUrl + "\">" + n.SellerName + "</a>";
                str += "                </td>";
                str += "            </tr>";
            });
            str += "        </tbody>";
            str += "    </table>";
            str += "</div>";
            str += "<div class=\"more\"><a target=\"_blank\" href=\"" + data[0].MoreCarUrL + "\">更多&gt;&gt;</a></div>";
            $(container).html(str);
        }
    });
}
//品牌相关二手车
TaoChe.showBrandUCar = function (pids, cityId, spell, brandName, num, jqcontainer) {
    if (!num) num = 10;
    //var container = ".line-box.ucar_box";
    var container = "#ucar_box";
    if (typeof jqcontainer != "undefined") {
        container = jqcontainer;
    }
    $.ajax({
    	url: "http://yicheapi.taoche.cn/CarSourceInterface/ForJson/dealerCarSourceForJson.ashx?pids=" + pids + "&count=" + num + "&cityid=" + cityId,
        //cache: false,
        dataType: "jsonp",
        jsonpCallback: "brand_callback",
        success: function (data) {
            data = data.CarListInfo;
            if (data.length <= 0) return;
            var str = "";

            str += "<div class=\"section-header header3\"><div class=\"box\"><h2><a href=\"" + data[0].MoreCarUrL + "\" target=\"_blank\" rel=\"nofollow\">相关" + brandName + "二手车</a></h2></div></div>";
            str += "<div class=\"list-box\">";
            $.each(data, function (i, n) {
                str += "<div class=\"img-info-layout img-info-layout-14093 no-wrap\">";
                str += "<div class=\"img\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\"><img width=\"120px\" height=\"90px\" src=\"" + n.PictureUrl.replace('/1h/', '/1f/') + "\" alt=\"" + n.BrandName + n.CarName + "\"></a></div>";
                str += "<ul class=\"p-list\">";
                str += "<li class=\"name\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\"  title=\"" + n.BrandName + n.CarName + "\">" + n.BrandName + n.CarName + "</a></p>";
                str += "<li class=\"info\">" + n.BuyCarDate + "上牌 " + (n.DrivingMileage / 10000).toFixed(1) + "万公里</p>";
                str += "<li class=\"price\"><a href=\"" + n.CarlistUrl + "\" target=\"_blank\">" + n.DisplayPrice + "</a></p>";
                str += "</ul>";
                str += "</div>";
            });
            str += "</div>";

            $(container).html(str);
        }
    });
}