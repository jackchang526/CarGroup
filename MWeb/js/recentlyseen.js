// 如果已经使用顶通js，引入下面js
//http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars
//
//如果没有使用顶通，引用下面js并加代码
//http://js.inc.baa.bitautotech.com/c/c.js?s=Bitauto.Login.Base,ibt_userCars
//
//Bitauto.Login.init()
//


//读取浏览过的车
Bitauto.Login.onComplatedHandlers.push(function (loginRes) { 
    //拿到6个浏览的车cs id 数组
    Bitauto.UserCars.getViewedCars(6, function (cars) {
        console.log(cars);

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
                    var html = "<dt>猜你喜欢：</dt>";
                    for (var i = 0; i < cars.length; i++) {
                        html += ('<dd><a href="http://car.m.yiche.com/' + data[cars[i]].allSpell + '/">' + data[cars[i]].showName+'</a></dd>'); 
                    } 
                    $(".browse-car dl").html(html);
                }
            },
            error: function (msg) { 
                console.log(msg);
            }
        });
    });
});

