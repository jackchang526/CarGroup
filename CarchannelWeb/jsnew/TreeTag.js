var TreeTag = {
    leftId: "contents",
    leftTagAreaId: "tab",
    topId: "topContents",
    topTagAreaId: "tab",
    requestType: "",
    requestPage: "",
    paramKey: [],
    paramValue: [],
    tagObject: [],
    settingParam: function(paramStr, reqType) {//设置页面参数
        if (!document.getElementById || !document.createTextNode) { return; }
        TreeTag.requestType = reqType;
        var paramSplit = paramStr.split('&');
        if (paramSplit.length < 1) {
            return;
        }

        for (var i = 0; i < paramSplit.length; i++) {
            var key = paramSplit[i].split('=')[0];
            TreeTag.paramKey[i] = key;
            TreeTag.paramValue[key] = paramSplit[i].split('=')[1];
        }
        //设置标签链接
        //TreeTag.settingLeftTagHref();
        TreeTag.settingTopTagHref();
        if (TreeTag.requestType.toLowerCase() == 'serial'
            || TreeTag.requestType.toLowerCase() == 'brand'
            || TreeTag.requestType.toLowerCase() == 'masterbrand') {
            TreeTag.settingSerialTag();
        }
    },
    settingLeftTagHref: function() {//设置链接的HREF
        if (!document.getElementById || !document.createTextNode) { return; }
        //设置左边树形
        var settingObjectArea = window.frames[TreeTag.leftId].document.getElementById(TreeTag.leftTagAreaId);
        if (typeof settingObjectArea == 'undefined'
            || settingObjectArea == null) {
            return;
        }
        var settingAObject = settingObjectArea.getElementsByTagName("a");
        if (settingAObject == null || settingAObject.length < 4) {
            return;
        }
        for (var i = 0; i < settingAObject.length; i++) {
            var tagType = settingAObject[i].getAttribute("tagtype");
            if (settingAObject[i].parentNode != null
                && settingAObject[i].parentNode.nodeType == 1
                && settingAObject[i].parentNode.className == "on") {
                continue;
            }
            settingAObject[i].setAttribute("href", TreeTag.getAUrl(TreeTag.tagObject[tagType][TreeTag.requestType]));
        }
    },
    settingTopTagHref: function() {//设置链接的HREF		
        if (!document.getElementById || !document.createTextNode) { return; }
        //设置左边树形
        var settingObjectArea;
        var settingAObject;
        //设置Top链接
        settingAObject = window.frames[TreeTag.topId].document.getElementsByTagName("a");
        if (settingAObject == null || settingAObject.length < 4) {
            return;
        }
        for (var i = 0; i < settingAObject.length; i++) {
            if (settingAObject[i].getAttribute("tagtype") == null
                || settingAObject[i].getAttribute("tagtype") == "") continue;
            if (settingAObject[i].getAttribute("tagtype") == "ucar"
                && TreeTag.requestType.toLowerCase() == "search") {
                settingAObject[i].setAttribute("href", "http://car.bitauto.com/tree_ucar/");
                continue;
            } //初始化经销商标签
            if (settingAObject[i].getAttribute("tagtype") == "delear"
                && TreeTag.requestType.toLowerCase() == "masterbrand"
                && typeof parent.mtobspell != "undefined"
                && parent.mtobspell != null
                && typeof parent.mtobspell["m" + TreeTag.paramValue["@CarID@"]] != "undefined") {
                settingAObject[i].setAttribute("href", TreeTag.getAUrl(TreeTag.tagObject[settingAObject[i].getAttribute("tagtype")][TreeTag.requestType]) + parent.mtobspell["m" + TreeTag.paramValue["@CarID@"]] + "/");
                continue;
            }
            if (settingAObject[i].parentNode != null
                && settingAObject[i].parentNode.nodeType == 1
                && settingAObject[i].parentNode.className == "current") {
                continue;
            }
            var tagType = settingAObject[i].getAttribute("tagtype");
            settingAObject[i].setAttribute("href", TreeTag.getAUrl(TreeTag.tagObject[tagType][TreeTag.requestType]));
        }
    },
    getAUrl: function(url) {//设置A的HREF
        if (!document.getElementById || !document.createTextNode) { return; }
        if (TreeTag.paramKey == null || TreeTag.paramKey.length < 1) {
            return;
        }

        for (var i = 0; i < TreeTag.paramKey.length; i++) {
            var pattern = new RegExp(TreeTag.paramKey[i], "g");
            url = url.replace(pattern, TreeTag.paramValue[TreeTag.paramKey[i]]);
        }
        pattern = new RegExp("//", "g");
        url = url.replace(pattern, "/");
        pattern = new RegExp(":/", "g");
        url = url.replace(pattern, "://");
        return url;
    },
    clossTree: function() {
        var settingObjectArea = window.frames[TreeTag.leftId];
        if (!settingObjectArea) return;
        settingObjectArea.tree_brand.collapseAll();
    },
    settingDelearTag: function() {
        var settingObjectArea = window.frames[TreeTag.topId].document.getElementById(TreeTag.topTagAreaId);
        if (typeof settingObjectArea == 'undefined'
            || settingObjectArea == null) {
            return;
        }
        settingAObject = settingObjectArea.getElementsByTagName("a");
        if (settingAObject == null || settingAObject.length < 4) {
            return;
        }
        for (var i = 0; i < settingAObject.length; i++) {
            var tagType = settingAObject[i].getAttribute("tagtype");
        }
    },
    settingSerialTag: function() {
        var settingObjectArea = window.frames[TreeTag.topId].document.getElementById(TreeTag.topTagAreaId);
        if (typeof settingObjectArea == 'undefined'
                || settingObjectArea == null) {
            return;
        }
        settingAObject = settingObjectArea.getElementsByTagName("a");
        if (settingAObject == null || settingAObject.length < 4) {
            return;
        }
        for (var i = 0; i < settingAObject.length; i++) {
            var tagType = settingAObject[i].getAttribute("tagtype");
            }
        
    }
}
/*
2010-11-25修改:树形的左侧标签已经修改，不是原车型，图片，报价，经销商的标签，所以，去掉settingDelearTag方法中此段的初始化操作
//设置树形上面的标签
var settingAObject = settingObjectArea.getElementsByTagName("a");
if (settingAObject == null || settingAObject.length < 4) {
return;
}
for (var i = 0; i < settingAObject.length; i++) {
var tagType = settingAObject[i].getAttribute("tagtype");
if (tagType == 'delear')
settingAObject[i].setAttribute("target", '_blank');
}

//设置子品牌上面的标签
        settingObjectArea = window.frames[TreeTag.leftId].document.getElementById(TreeTag.leftTagAreaId);
        if (typeof settingObjectArea == 'undefined'
                    || settingObjectArea == null) {
            return;
        }
        var settingAObject = settingObjectArea.getElementsByTagName("a");
        if (settingAObject == null || settingAObject.length < 4) {
            return;
        }
        for (var i = 0; i < settingAObject.length; i++) {
            var tagType = settingAObject[i].getAttribute("tagtype");
            if (tagType == 'delear')
                settingAObject[i].target = '_parent';
        }
*/