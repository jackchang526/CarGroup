//中国广播网合作-分类排行与新车推荐块
(function () {
    var cateSign = "yc_carhot", cateBlockId = cateSign + "_block", cateBlockTagId = cateBlockId + "_tag", cateLevelId = cateSign + "_lv", cateLevelTagId = cateLevelId + "_tag", cateLevelContId = cateLevelId + "_cont", cateNewCsId = cateSign + "_newcs",
    head = document.getElementsByTagName("head")[0], script = document.createElement('script'),
    buildLevelHtml = function () {

        if (typeof csCateDataJson === 'undefined') return;

        var tag = ["<div class=\"col_tab\"><ul id=\"" + cateLevelTagId + "\">"], cont = ["<div id=\"" + cateLevelContId + "\">"], isCur = true, cates = [{ c: "all", v: "全部" },
            { c: "wx", v: "微型", url: "weixingche" },
            { c: "xx", v: "小型", url: "xiaoxingche" },
            { c: "jcx", v: "紧凑型", url: "jincouxingche" },
            { c: "zx", v: "中型", url: "zhongxingche" },
            { c: "zdx", v: "中大型", url: "zhongdaxingche" },
            { c: "hh", v: "豪华型", url: "haohuaxingche" },
            { c: "suv", v: "SUV", url: "suv" },
            { c: "mpv", v: "MPV", url: "mpv" },
            { c: "pc", v: "跑车", url: "paoche"}];
        for (var i = 0, count = cates.length; i < count; i++) {
            if (typeof csCateDataJson[cates[i].c] === 'undefined') continue;

            var curTag = cates[i], curCates = csCateDataJson[curTag.c];
            tag.push("<li" + (isCur ? " class=\"current\"" : "") + "><a target=\"_blank\" href=\"" + (curTag.c == "all" ? "http://news.bitauto.com/hot/#rank4" : "http://car.bitauto.com/" + curTag.url + "/") + "\">" + curTag.v + "</a></li>");
            cont.push("<div id=\"" + (cateLevelId + "_" + curTag.c) + "\" style=\"display: " + (isCur ? "block" : "none") + ";\"><ol class=\"hot_ranking col_ranking\">");

            for (var j = 0, carCount = curCates.length; j < carCount; j++) {
                var num = j + 1;
                cont.push("<li" + (j < 3 ? " class=\"fist\"" : "") + "><em>" + (num < 10 ? "0" + num : num) + "</em><a target=\"_blank\" href=\"" + curCates[j].url + "\">" + curCates[j].name + "</a></li>");
            }

            cont.push("</ol></div>");
            if (isCur)
                isCur = false;
        }

        tag.push("</ul></div>");
        cont.push("</div>");
        document.getElementById(cateLevelId).innerHTML = (tag.join('') + cont.join(''));
    }
    , buildNewsCsHtml = function () {
        if (typeof csCateDataJson === 'undefined' || typeof csCateDataJson["newcs"] === 'undefined') return;
        var conts = [], newcss = csCateDataJson["newcs"]; // class=\"end\"
        for (var i = 0, count = newcss.length; i < count; i++) {
            var num = i + 1, curcs = newcss[i], price = curcs.price;
            if (price.indexOf('万') > 0) {
                price = price.replace('万','');
            }
            conts.push("<li" + (num < count ? "" : " class=\"end\"") + "><span class=\"" + (i < 3 ? "num-top" : "num-normal") + "\">" + (num > 9 ? num : "0" + num) + "</span><a target=\"_blank\" title=\"" + curcs.name + "\" href=\"" + curcs.url + "\">" + curcs.name + "</a><span class=\"clicks\">" + price + "</span></li>");
        }
        document.getElementById(cateNewCsId).getElementsByTagName("ul")[0].innerHTML = conts.join('');

    }
    , mouseover = function (e, tags, conts) {
        var e = (e || window.event), obj = (e.srcElement || e.target);
        if (obj.tagName.toLowerCase() != "li") return;
        for (var x = 0; x < tags.length; x++) {
            if (tags[x] == obj) {
                tags[x].className = "current";
                conts[x].style.display = "block";
            }
            else {
                tags[x].className = "";
                conts[x].style.display = "none";
            }
        }
    }
    , bindEvent = function () {
        var blocks = document.getElementById(cateBlockId).childNodes,
            tags = document.getElementById(cateBlockTagId).childNodes;
        for (var i = 0, count = tags.length; i < count; i++) {
            tags[i].onmouseover = function (e) {
                mouseover(e, tags, blocks);
            }
        }
    }
    , bindLevelEvent = function () {
        var tags = document.getElementById(cateLevelTagId).getElementsByTagName("li"),
    conts = document.getElementById(cateLevelContId).getElementsByTagName("div");
        for (var i = 0, count = tags.length; i < count; i++) {
            tags[i].onmouseover = function (e) {
                mouseover(e, tags, conts);
            }
        }
    };

    document.writeln("<link rel=\"stylesheet\" type=\"text/css\" href=\"http://image.bitautoimg.com/carchannel/Cooperation/cnr2/css/style.css\" />");
    document.writeln(["<div class=\"bt_page\">"
    , "<div class=\"line_box select_car_l\">"
    , "<div class=\"h3_tab\">"
    , "<ul id=\"" + cateBlockTagId + "\"><li class=\"current\"><a target=\"_blank\" href=\"http://news.bitauto.com/hot/#rank4\">关注排行</a></li><li><a target=\"_blank\" href=\"http://car.bitauto.com/xuanchegongju/\">新车推荐</a></li></ul>"
    , "</div>"
    , "<div id=\"" + cateBlockId + "\" class=\"hot_rank_box\">"

    , "<div id=\"" + cateLevelId + "\" style=\"display: block;\"></div>"
    , "<div id=\"" + cateNewCsId + "\" style=\"display: none;\"><div id=\"list\"><div class=\"box-cont hd_border\"><div class=\"bd\"><div class=\"tab-bd\"><ul class=\"item-list\"></ul></div></div></div></div></div>"

    , "</div>"
    , "<div class=\"clear\"></div>"
    , "</div>"
    , "</div>"].join(''));

    script.onload = script.onreadystatechange = function () {
        if (script && script.readyState && script.readyState != 'loaded' && script.readyState != 'complete') return;

        script.onload = script.onreadystatechange = script.onerror = null, script.src = '', script.parentNode.removeChild(script), script = null;

        buildLevelHtml();
        buildNewsCsHtml();
        bindEvent();
        bindLevelEvent();
    };

    script.charset = 'utf-8';
    script.src = "http://api.car.bitauto.com/Cooperation/GetCategorySerialJson.ashx";

    head.appendChild(script);
})();