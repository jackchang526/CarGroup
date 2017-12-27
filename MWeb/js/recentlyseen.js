//读取浏览过的车
Bitauto.Login.onComplatedHandlers.push(function (loginRes) { 
    //拿到6个浏览的车cs id 数组
    Bitauto.UserCars.getViewedCars(6, function (cars) {
        cars = Bitauto.UserCars.viewedcar.arrviewedcar;
        if (cars == undefined || cars == null) {
            $(".browse-car").hide();
            return;
        }
        var serialInfoUrl = "http://api.car.bitauto.com/carinfo/getserialinfobycsids.ashx?csids=" + cars.join(",");
        $.ajax({
            type: "get",
            url: serialInfoUrl,
            cache: true,
            dataType: 'jsonp',
            jsonp: "callback",
            jsonpCallback: "RecentSeenSerialInfoCallBack",
            timeout: 3000,
            contentType: "application/json",
            success: function (data) {
                if (data != null) {
                    var html = "";
                    for (var i = 0; i < cars.length; i++) {
                        if (data[cars[i]] != undefined && data[cars[i]] != null) {
                            html += ('<dd><a href="http://car.m.yiche.com/' + data[cars[i]].allSpell + '/">' + data[cars[i]].showName + '</a></dd>');
                        }
                    }
                    if (html.length > 0) {
                        $(".browse-car dl").html("<dt>最近看过：</dt>" + html);
                        $(".browse-car").show();
                    } else {
                        $(".browse-car").hide();
                    }
                } else {
                    $(".browse-car").hide();
                }
            },
            error: function (msg) { 
                //console.log(msg);
                $(".browse-car").hide();
            }
        });
    });
});

