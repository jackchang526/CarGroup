var DomHelper = {
    cancelClick: function(e) {
        if (window.event && window.event.cancelBubble
            && window.event.returnValue) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
            return;
        }
        if (e && e.stopPropagation && e.preventDefault) {
            e.stopPropagetion();
            e.preventDefault();
        }
    },
    addEvent: function(elm, evType, fn, useCapture) {
        if (elm.addEventListener) {
            elm.addEventListener(evType, fn, useCapture);
            return true;
        }
        else if (elm.attachEvent) {
            var r = elm.attachEvent('on' + evType, fn);
            return r;
        }
        else {
            elm['on' + evType] = fn;
        }
    }
}
var statics = {
    topFrameId: 'topContents',
    topTagList: { "top_chexing": "101", "top_photo": "102", "top_price": "103", "top_delear": "104", "top_video": "105", "top_daogou": "106", "top_pingce": "107", "top_tujie": "108", "top_hangqing": "109", "top_index": "110", "top_xiaoliang": "111", "top_ask": "112", "top_anquan": "113", "top_keji": "114", "top_ucar": "115", "top_koubei": "116", "top_baoyang": "117" },
    topTagAreaId: "allTab",
    leftFrameId: 'contents',
    leftTagAreaId: 'tab',
    leftTreeAreaId: 'nav_tree',
    leftHotAreaId: ["hotMasterBrand", "hotSerial"],
    leftTagList: { "chexing": "131", "photo": "132", "price": "133", "delear": "134", "video": "135", "daogou": "136", "pingce": "137", "tujie": "138", "hangqing": "139", "index": "140", "treehotbrand": "142", "treehotSerial": "143" },
    mainFrameId: 'main',
    carAreaId: 'cartype',
    serialTagList: { "channel": "170", "cartype": "171", "carfriend": "172", "carsale": "173", "carsee": "174" },
    selectCarTagList: { "price0-5": "501", "price5-10": "502", "price10-15": "503", "price15-20": "504", "price20-25": "505", "price25-40": "506", "price40-80": "507", "price80-9999": "508", "weixingche": "510", "xiaoxingche": "511", "jincouxingche": "512", "zhongxingche": "513", "zhongdaxingche": "514", "haohuache": "515", "mpv": "516", "suv": "517", "paoche": "518", "mianbaoche": "519", "dis0-13": "530", "dis13-16": "531", "dis17-20": "532", "dis21-30": "533", "dis31-50": "534", "dis50-999": "535", "trans_mt": "540", "trans_at": "541" },
    statIFrame: 'statFrame',
    /*statFrameUrl: 'http://carser.bitauto.com/webqueuepool/autochannel/counterforautochannel.aspx?albumid=@tagId@&albumname=@tagName@&category=@sourceId@&key=tree&@math@',*/
    statFrameUrl: 'http://carstat.bitauto.com/weblogger/urlrecord.aspx?logtype=tabclick&tagid=@tagId@&currenttagid=@sourceId@&tagname=@tagName@',
    //统计标签点击
    staticsClick: function(type, element, currenttag) {
        var statFrame;
        var userClickType = element.getAttribute("tagtype");
        var tagId = "";
        var tagName = "";
        var sourceId = "";
        if (type == "top") {
            statFrame = statics.getFrameObject(statics.topFrameId);
            tagName = "top_" + userClickType;
            tagId = statics.topTagList[tagName];
            sourceId = statics.topTagList["top_" + currenttag];
        }
        else if (type == "left") {
            statFrame = statics.getFrameObject(statics.leftFrameId);
            tagName = userClickType;
            tagId = statics.leftTagList[tagName];
            sourceId = statics.leftTagList[currenttag];
        }
        statics.startStaticUrl(tagId, tagName, sourceId, statFrame);
    }, //统计树形的点击
    staticsTreeClick: function(e) {
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        if (element.nodeName.toLowerCase() == "em") return;
        if (element.nodeName.toLowerCase() == "a") {
            element = element.parentNode.childNodes[0];
        }
        else {
            element = element.parentNode;
        }
        var statFrame;
        var tagId = "141";
        var tagName = "tree";
        var sourceId = element.id.replace(/n/g, "");
        statFrame = statics.getFrameObject(statics.leftFrameId);
        statics.startStaticUrl(tagId, tagName, sourceId, statFrame);
    }, //统计热门区的点击
    staticsTreeHotClick: function(e) {
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        var elementId;
        if (element.nodeName.toLowerCase() != "a") return;
        elementId = element.id;
        var staticType = element.getAttribute("stattype");
        statFrame = statics.getFrameObject(statics.leftFrameId);
        tagName = staticType;
        tagId = statics.leftTagList[staticType];
        elementId = elementId.replace(/hot/g, "");
        statics.startStaticUrl(tagId, tagName, elementId, statFrame);

    }, //统计去子品牌的链接
    staticsGoSerialClick: function(e) {
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        var elmentId;
        if (element.nodeName.toLowerCase() != "a") {
            elmentId = element.parentNode.id;
        }
        else if (element.nodeName.toLowerCase() == "a") {
            elmentId = element.id;
        }
        var statFrame;
        elmentId = elmentId.replace(/n/g, "");
        elmentId = elmentId.replace(/m/g, "");
        statFrame = statics.getFrameObject(statics.mainFrameId);
        statics.startStaticUrl("175", "mastertocar", elmentId, statFrame);

    }, //统计子品牌内链接
    staticsSerialInClick: function(e) {
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        var elementId;
        var elementStatType;
        var tempElement;
        if (element.nodeName.toLowerCase() == "strong") {
            tempElement = element.parentNode.parentNode;
            elementId = tempElement.id;
            elementStatType = tempElement.getAttribute("stattype");
        }
        else if (element.nodeName.toLowerCase() == "span"
            || element.nodeName.toLowerCase() == "img") {
            tempElement = element.parentNode;
            elementId = tempElement.id;
            elementStatType = tempElement.getAttribute("stattype");
        }
        else if (element.nodeName.toLowerCase() == "a") {
            elementId = element.id;
            elementStatType = element.getAttribute("stattype");
        }
        var statFrame;
        var tagId = statics.serialTagList[elementStatType];
        var tagName = elementStatType;
        var sourceId = elementId.replace(/n/g, "");
        statFrame = statics.getFrameObject(statics.mainFrameId);
        statics.startStaticUrl(tagId, tagName, sourceId, statFrame);

    }, //得到统计用的地址
    startStaticUrl: function(tagId, tagName, sourceId, statFrame) {
        if (!statFrame) return;
        if (typeof tagId == 'undefined' || tagId == "") return;
        if (typeof sourceId == 'undefined' || sourceId == "") return;
        var frameUrl = statics.statFrameUrl;
        frameUrl = frameUrl.replace(/@tagId@/g, tagId);
        frameUrl = frameUrl.replace(/@tagName@/g, tagName);
        frameUrl = frameUrl.replace(/@sourceId@/g, sourceId);
        frameUrl = frameUrl.replace(/@math@/g, Math.random());
        statFrame.src = frameUrl; //alert(statFrame.src);
    }, //得到统计用iframe对象
    getFrameObject: function(mainFrameId) {
        var statFrame;
        var contentFrame;
        contentFrame = window.frames[mainFrameId].document;
        statFrame = contentFrame.getElementById(statics.statIFrame);
        if (!statFrame) {
            statFrame = contentFrame.createElement("IFRAME");
            statFrame.style.display = "none";
            contentFrame.body.appendChild(statFrame);
        }
        return statFrame;
    }, //初始化头部标签点击
    initTopTagClick: function() {
        var topFrame = window.frames[statics.topFrameId];
        if (!topFrame) return;
        var ahrefArea = topFrame.document.getElementById(statics.topTagAreaId);
        if (!ahrefArea) return;
        var link = ahrefArea.getElementsByTagName("a");
        if (link == null || link.length < 1) return;
        var selectATag = "";
        for (var i = 0; i < link.length; i++) {
            var aObject = link[i];
            if (aObject == null || aObject.nodeType != 1 || !aObject.getAttribute("tagtype")) continue;
            selectATag = aObject.getAttribute("tagtype");
            aObject.onclick = function(stattype) {
                return function() {
                    parent.statics.staticsClick("top", this, stattype);
                }
            } (selectATag);
        }
    }, //初始化左侧树形页面标签点击
    initLeftTagClick: function() {
        /*var LeftFrame = window.frames[statics.leftFrameId];
        if (!LeftFrame) return;
        var ahrefArea = LeftFrame.document.getElementById(statics.leftTagAreaId);
        if (!ahrefArea) return;
        var liList = ahrefArea.getElementsByTagName("li");
        if (liList == null || liList.length < 1) return;
        var selectATag = "";
        for (var i = 0; i < liList.length; i++) {
        if (liList[i].className.toLowerCase() == 'on') {
        var cObject = liList[i].childNodes[0];
        if (cObject != null && cObject.nodeType == 1) {
        selectATag = cObject.getAttribute("tagtype");
        }
        continue;
        }
        var aObject = liList[i].childNodes[0];
        if (aObject == null || aObject.nodeType != 1) continue;
        aObject.onclick = function() {
        parent.statics.staticsClick("left", this, selectATag);
        }
        }*/
        statics.initLeftTreeClick();
    }, //初始化左侧树形页面树形点击
    initLeftTreeClick: function() {
        var LeftFrame = window.frames[statics.leftFrameId];
        if (!LeftFrame) return;
        var url = LeftFrame.location;
        var pattern = new RegExp("/tree_chexing/", "i");
        if (!pattern.test(url)) return;
        var treeArea = LeftFrame.document.getElementById(statics.leftTreeAreaId);
        if (!treeArea) return;
        var aLinkList = treeArea.getElementsByTagName("a");
        if (aLinkList == null || aLinkList.length < 1) return;

        for (var i = 0; i < aLinkList.length; i++) {
            DomHelper.addEvent(aLinkList[i], "click", parent.statics.staticsTreeClick, false);
        }

        for (var i = 0; i < statics.leftHotAreaId.length; i++) {
            var linkArea = LeftFrame.document.getElementById(statics.leftHotAreaId[i]);
            if (!linkArea) return;
            var linkList = linkArea.getElementsByTagName("a");
            if (linkList == null || linkList.length < 1) return;
            for (var j = 0; j < linkList.length; j++) {
                DomHelper.addEvent(linkList[j], "click", parent.statics.staticsTreeHotClick, false);
            }
        }
    }, //初始化中间内容的点击
    initContentClick: function() {
        var frameObject = window.frames[statics.mainFrameId];
        if (!frameObject) return;
        var framesrc = frameObject.location.toString();
        if (framesrc == null || framesrc == "") return;
        //选车只在首页与选车页生效
        if (framesrc == "http://car.bitauto.com" || framesrc == "http://car.bitauto.com/" || framesrc.indexOf("tree_chexing") > 0) {
            statics.initSelectCarConditionClick(frameObject);
        }

        var pattern = new RegExp("/tree_chexing/m?b_\\d+/", "i");
        if (pattern.test(framesrc)) {
            statics.initBrandClick(frameObject);
            return;
        }
        pattern = new RegExp("/tree_chexing/sb_\\d+/", "i");
        if (pattern.test(framesrc)) {
            statics.initSerialClick(frameObject);
            return;
        }

    }, //初始化品牌点击
    initBrandClick: function(frameObject) {
        var divList = frameObject.document.getElementsByTagName("div");
        if (divList == null || divList.length < 1) return;
        for (var i = 0; i < divList.length; i++) {
            var cartypeValue = divList[i].getAttribute("listtype");
            if (cartypeValue == null || cartypeValue != "cartype") continue;
            var aLinkList = divList[i].getElementsByTagName("a");
            if (aLinkList == null || aLinkList.length < 1) continue;
            for (var j = 0; j < aLinkList.length; j++) {
                var stattype = aLinkList[j].getAttribute("stattype");
                if (stattype == null || stattype == "" || stattype != "car") continue;
                DomHelper.addEvent(aLinkList[j], "click", parent.statics.staticsGoSerialClick, false);
            }
        }
    }, //初始化子品牌点击
    initSerialClick: function(frameObject) {
        var aLinkList = frameObject.document.getElementsByTagName("a");
        if (aLinkList == null || aLinkList.length < 1) return;
        for (var i = 0; i < aLinkList.length; i++) {
            var stattype = aLinkList[i].getAttribute("stattype");
            if (stattype == null || stattype == "") continue;
            DomHelper.addEvent(aLinkList[i], "click", parent.statics.staticsSerialInClick, false);
        }
    },
    //初始化选车链接的点击
    initSelectCarConditionClick: function(frameObject) {
        var scRoot = frameObject.document.getElementById("showhideCon");
        if (!scRoot)
            return;
        var aLinkList = frameObject.document.getElementsByTagName("a");
        if (aLinkList == null || aLinkList.length < 1) return;
        for (var i = 0; i < aLinkList.length; i++) {
            var stattype = aLinkList[i].getAttribute("stattype");
            if (stattype == null || stattype == "") continue;
            DomHelper.addEvent(aLinkList[i], "click", parent.statics.staticsSelectCarClick, false);
        }
    },
    //选车条件被点击
    staticsSelectCarClick: function(e) {
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        var elementId;
        if (element.nodeName.toLowerCase() != "a") return;
        elementId = element.id;
        var staticType = element.getAttribute("stattype");
        statFrame = statics.getFrameObject(statics.mainFrameId);
        tagName = staticType;
        tagId = statics.selectCarTagList[staticType];

        var sourcePageId = 0;
        //判断来源页
        var frameObject = window.frames[statics.mainFrameId];
        if (frameObject) {
            var framesrc = frameObject.location.toString();
            if (framesrc != null) {
                if (framesrc == "http://car.bitauto.com" || framesrc == "http://car.bitauto.com/")
                    sourcePageId = 1;
                else if (framesrc.indexOf("/tree_chexing/search") > -1)
                    sourcePageId = 2;
                else if (framesrc.indexOf("/tree_chexing/mb_") > -1)
                    sourcePageId = 3;
                else if (framesrc.indexOf("/tree_chexing/b_") > -1)
                    sourcePageId = 4;
                else if (framesrc.indexOf("/tree_chexing/sb_") > -1)
                    sourcePageId = 5;
            }
        }
        //alert(sourcePageId + "_" + staticType + "_" + framesrc);
        statics.startStaticUrl(tagId, tagName, sourcePageId, statFrame);
    }

}