define(["jquery", "util"], function ($, util) {

    //start 获得页面默认容器
    var defaultAuchors = [];
    var pageList = $(".section");
    $.each(pageList, function (index, item) {
        var pagename = $(item).attr("data-anchor");
        defaultAuchors.push(pagename);
    });
    //end 获得页面默认容器

    //start 定制版
    var targetAuchors = [];
    var orderStr = util.GetQueryStringByName("order");

    if (orderStr != null&&orderStr!=="") {

        var orderList = orderStr.split(',');

        //通过参数初始化 auchors 数组
        for (var i = 0; i < orderList.length; i++) {
            //保证存在于标准列表中且auchors中排重
            if (defaultAuchors.indexOf(orderList[i].toLowerCase()) >= 0 && targetAuchors.indexOf(orderList[i].toLowerCase()) < 0) {
                targetAuchors.push(orderList[i]);
            }
        }

    } else {

        targetAuchors = defaultAuchors;

    }
    //start 定制版

    return { Auchors: targetAuchors, DefaultAuchors: defaultAuchors };
});
