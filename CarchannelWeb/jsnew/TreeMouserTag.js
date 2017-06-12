var MouserTag = {
    leftId: "contents",
    mainId: "main",
    leftTagId: ["brandTag", "HotbrandTag", "HotSerialTag"],
    brandTag: { "display": ["nav_tree", "tree_letter"] },
    HotbrandTag: { "display": ["hotMasterBrand"] },
    HotSerialTag: { "display": ["hotSerial"] },
    hotMasterBrand: "HotbrandTag",
    hotSerial: "HotSerialTag",
    LeftScrollTop: 0,
    GetParentDiv: function(elem) {
        if (elem.nodeName.toLowerCase() == "div") {
            return elem;
        }
        elem = elem.parentNode;
        return MouserTag.GetParentDiv(elem);
    },
    //鼠标移动事件
    OnMouserOver: function(e) {
        var frameObj = window.frames[MouserTag.leftId];
        if (!frameObj) return;
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        if (element.nodeName.toLowerCase() == "a") {
            element = element.parentNode;
        }
        var elementId = element.id;
        frameObj.document.documentElement.scrollTop = 0;
        for (var i = 0; i < MouserTag.leftTagId.length; i++) {
            var tagName = MouserTag.leftTagId[i];
            var liObj = frameObj.document.getElementById(tagName);
            //保存车型的滚动条高
            if (liObj.className == "on" && tagName == "brandTag") {
                MouserTag.LeftScrollTop = frameObj.document.documentElement.scrollTop;
            }
            //设置样式为空
            liObj.className = "";
            var displayStyle = "none";
            if (MouserTag.leftTagId[i] == elementId) {
                displayStyle = "block";
            }
            //隐藏元素显示样式
            for (var j = 0; j < MouserTag[tagName]["display"].length; j++) {
                var controlObj = frameObj.document.getElementById(MouserTag[tagName]["display"][j]);
                controlObj.style.display = displayStyle;
            }
        }
        element.className = "on";
        if (elementId == "brandTag") MouserTag.initTree();
    },
    LinkClick: function(e) {
        return;
        //以下程序内容因业务需要已经不用
        var frameObj = window.frames[MouserTag.leftId];
        if (!frameObj) return;
        var evt = e || window.event;
        var element = evt.target || evt.srcElement;
        var divObj = MouserTag.GetParentDiv(element);
        frameObj.document.getElementById("brandTag").className = "on";
        for (var i = 0; i < MouserTag["brandTag"]["display"].length; i++) {
            var objName = MouserTag["brandTag"]["display"][i];
            var controlObj = frameObj.document.getElementById(objName);
            controlObj.style.display = "block";
        }

        frameObj.document.getElementById(MouserTag[divObj.id]).className = "";
        for (var i = 0; i < MouserTag[MouserTag[divObj.id]]["display"].length; i++) {
            var objName = MouserTag[MouserTag[divObj.id]]["display"][i];
            var controlObj = frameObj.document.getElementById(objName);
            controlObj.style.display = "none";
        }
        frameObj.resizeBy(1, 0);
    },
    initTree: function() {
        var serialId = 0;
        var pattern = /\/tree_chexing\/[sm]?b_(\d+)/g;
        var url;
        try { url = window.frames[MouserTag.mainId].location;} catch (e) { return; }
        if (url == null || url == "") return;
        var r;
        while ((r = pattern.exec(url)) != null) {
            serialId = parseInt(r[1]);
            break;
        }
        if (serialId < 1) return;
        window.frames[MouserTag.leftId].tree_brand.search(serialId);
    },
    initLinkClickEvent: function(tagName) {
        var frameObj = window.frames[MouserTag.leftId];
        if (!frameObj) return;
        if (tagName == "brandTag") return;
        var linkList = frameObj.document.getElementById(MouserTag[tagName]["display"][0]).getElementsByTagName("a");
        if (linkList == null || linkList.length < 1) return;
        for (var i = 0; i < linkList.length; i++) {
            DomHelper.addEvent(linkList[i], "click", MouserTag.LinkClick, false);
        }
    }, //初始化标签
    initTagEvent: function() {
        var frameObj = window.frames[MouserTag.leftId];
        if (!frameObj) return;
        var url = frameObj.location;
        var pattern = new RegExp("/tree_chexing/", "i");
        if (!pattern.test(url)) return;
        for (var i = 0; i < MouserTag.leftTagId.length; i++) {
            var tagName = MouserTag.leftTagId[i];
            if (tagName != "brandTag") {
                for (var j = 0; j < MouserTag[tagName]["display"].length; j++) {
                    var controlObj = frameObj.document.getElementById(MouserTag[tagName]["display"][j]);
                    controlObj.style.display = "none";
                }
                MouserTag.initLinkClickEvent(tagName);
            }
            DomHelper.addEvent(frameObj.document.getElementById(tagName), "mouseover", MouserTag.OnMouserOver, false);
        }
    }
}