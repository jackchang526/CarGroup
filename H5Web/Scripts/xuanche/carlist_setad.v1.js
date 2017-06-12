/*
功能：H5高级选车广告投放插件
作者：songcl
日期：2016-01-07
*/
(function($) {
    $.fn.SetAd = function(options) {
        var defaults = {
            currentpagenum: 1,
            data: [
                //{ "pagenum": "1", "position": "0", "AllSpell": "ad", "serialid": "2945", "ShowName": "广告", "ImageUrl": "http://img4.baa.bitautotech.com/img/V2img4.baa.bitautotech.com/usergroup/2016/1/5/61160a3c8961443d9242d17d967df9a9_720_0_max_JPG.jpg", "PriceRange": "0.0-0.0万", "startdate": "", "enddata": "2016-10-10", "createdate": "" }
            ]
        };

        var options = $.extend(defaults, options);

        if (options.data.length !== 0) {

            var date = new Date();
            var currentDate = date.getFullYear().toString() + "/" + (date.getMonth() + 1).toString() + "/" + date.getDate().toString();

            $.each(options.data, function(index, item) {
                //将字符串转换为日期
                var enddata = new Date(item.enddata.replace(/-/g, "/"));
                var tempCurrentDate = new Date(currentDate);
                var maxIndex = $(".car_list li").length - 1;

                //判断广告是否过期(结束日期大于当前日期),过期不投放
                if (enddata - tempCurrentDate > 0 && parseInt(options.currentpagenum) === parseInt(item.pagenum)) {
                    //当前投放的广告不存在列表中，按约定投放广告
                    if ($(".car_list li[id=" + item.serialid + "]").length === 0 && maxIndex >= parseInt(item.position)) {

                        var adhtml = $("#adtmpl").tmpl(item).html();
                        $(".car_list li:eq(" + item.position + ")").attr("id", item.serialid);
                        $(".car_list li:eq(" + item.position + ")").html(adhtml);

                    } else { //当前投放的广告存在列表中，将存在的数据移动到约定位置

                        var adindex = $(".car_list li[id=" + item.serialid + "]").index();
                        //存在的数据所在位置与约定的位置不一致时
                        if (adindex !== parseInt(item.position) && maxIndex >= parseInt(item.position)) {
                            var origindata = $(".car_list li[id=" + item.serialid + "]");
                            $(".car_list li[id=" + item.serialid + "]").remove();
                            $(".car_list li:eq(" + item.position + ")").before(origindata);
                        }
                        //存在的数据所在位置与约定的位置一致时不处理

                    }
                }
            });
        }

    };
})(jQuery);