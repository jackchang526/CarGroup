/*
 *Author:sk
 *Desc:左侧属性效果
 *Date:2016-04-28
 */
(function(w) {
    var addEvent = function(elm, evType, fn) {
            if (elm.addEventListener) {
                elm.addEventListener(evType, fn, false); //DOM2.0
            } else if (elm.attachEvent) {
                elm.attachEvent('on' + evType, function(e) {
                    fn.call(elm, e); //改变this指向
                });
            } else {
                elm["on" + evType] = fn;
            }
        },
        getQueryString = function(data) {
            var tdata = '';
            for (var key in data) {
                tdata += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);  
            }
            tdata = tdata.replace(/^&/g, "");
            return tdata
        },
        getData = function(e, callbackName, callbackFunc) {
            var t = document.getElementsByTagName("head")[0] || document.documentElement,
                n = document.createElement("script"),
                r = false;
            if (callbackName && callbackFunc instanceof Function) window[callbackName] = function(data) {
                callbackFunc(data);
            }
            n.src = e,
                n.charset = "utf-8",
                n.onerror = n.onload = n.onreadystatechange = function() {
                    !r && (!this.readyState || this.readyState == "loaded" || this.readyState == "complete") && (r = true, t.removeChild(n));
                },
                t.insertBefore(n, t.firstChild);
        },
        extend = function() {
            var options, name, src, copy, copyIsArray, clone, target = arguments[0] || {},
                i = 1,
                length = arguments.length,
                deep = false;
            if (typeof target === "boolean") {
                deep = target;
                target = arguments[1] || {};
                i = 2;
            }
            if (typeof target !== "object" && typeof target != "function") {
                target = {};
            }
            if (length === i) {
                target = this;
                --i;
            }
            for (; i < length; i++) {
                if ((options = arguments[i]) != null) {
                    for (var name in options) {
                        src = target[name];
                        copy = options[name];
                        if (target === copy) {
                            continue;
                        }
                        if (deep && copy && (copy instanceof Object || (copyIsArray = copy instanceof Array)) && !(copy instanceof Function)) {
                            if (copyIsArray) {
                                copyIsArray = false;
                                clone = src && src instanceof Array ? src : [];
                            } else {
                                clone = src && src instanceof Object ? src : {};
                            }
                            target[name] = extend(deep, clone, copy);
                        } else if (copy !== undefined) {
                            target[name] = copy;
                        }
                    }
                }
            }
            return target;
        },
        format = function() {
            if (arguments.length == 0)
                return null;
            var str = arguments[0];
            var obj = arguments[1];
            for (var key in obj) {
                var re = new RegExp('\\{' + key + '\\}', 'gi');
                str = str.replace(re, obj[key]);
            }
            return str;
        },
        mathWinHeigth = function() {
            var winH = 0;
            if (window.innerHeight) {
                winH = Math.min(window.innerHeight, document.documentElement.clientHeight);
            } else if (document.documentElement && document.documentElement.clientHeight) {
                winH = document.documentElement.clientHeight;
            } else if (document.body) {
                winH = document.body.clientHeight;
            }
            return winH;
        },
        mathOffsetTop = function(curNode, id) {
            var topHeight = 0;
            if (!curNode)
                return topHeight;
            while (curNode && curNode.id != id) {
                topHeight += curNode.offsetTop;
                curNode = curNode.offsetParent;
            }
            return topHeight;
        };
    var LeftTree = (function(module) {

        var self = module,
            defaults = {
                params: {
                    tagtype: "chexing"
                },
                url: "http://api.car.bitauto.com/CarInfo/getlefttreejson.ashx",
                likeCount: 6,
                likeDefCsIds: "3152,1765,2608,2370,2871,4418,2593,2694",
                likeUrl: "http://www.bitauto.com/indexWeb2/GetJsonData.aspx?cityId={cityId}&viewCsIds={viewCsIds}&defCsIds={defCsIds}&top={top}&v=news&callback=getLikeCallback",
                likeDefLink: "http://car.bitauto.com/{allspell}/",
                likeBlank: true
            };

        module.init = function(options) {

                extend(defaults, options);

                var reqUrl = defaults.url + "?" + getQueryString(defaults.params);

                getData(reqUrl, "getLeftreeCallback", JsonpCallBack);
            }
            //获取左侧树html
        module.getTreeHtml = function(jsonTree) {
                if (typeof jsonTree["char"] == "undefined") {
                    return "";
                }
                var h = [];
                h.push(getCharNavHtml(jsonTree["char"]));
                h.push("<div class=\"treeWarp\"><div class=\"treeCon\">");
                h.push("<div class=\"tree-list\" id=\"treeList\">");
                h.push("<div class=\"car-list\" id=\"carList\"></div>");
                h.push("<ul class=\"list-con\">");
                var i = 0;
                for (var firstChar in jsonTree["char"]) {
                    i++;
                    if (jsonTree.brand[firstChar] == undefined)
                        continue;
                    h.push("<li id=\"letter" + i + "\"><div class=\"num-tit\">" + firstChar + "</div>");
                    h.push("<ul class=\"brand-list\">");
                    //按字母输出主品牌
                    if (typeof jsonTree.brand[firstChar] != "undefined") {
                        h.push(getMasterHtml(jsonTree.brand[firstChar]));
                    }
                    h.push("</ul></li>");

                }
                h.push("</ul></div></div></div>");
                return h.join('');
            }
            //获取主品牌html
        var getMasterHtml = function(masterList) {
            var html = "";
            for (var j = 0; j < masterList.length; j++) {
                var className = "mainBrand";
                var curIdStr = "";
                //当前主品牌展开样式
                if (masterList[j].child != undefined) {
                    className += " active";
                }
                if (masterList[j].cur == 1) {
                    if (masterList[j].type == "mb") {
                        className = "mainBrand check current_unfold active";
                        curIdStr = " id=\"curObjTreeNode\"";
                    } else {
                        className = "mainBrand current_unfold";
                    }
                }

                html += "<li" + curIdStr + "><a href=\"" + masterList[j].url + "\" class=\"" + className + "\"><span class=\"logo-img\"><img src=\"http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_" + masterList[j].id + "_100.png\" alt=\"\"></span><div class=\"brand-name\"><span>" + masterList[j].name + "</span><em>(" + masterList[j].num + ")</em></div></a>";
                //品牌
                if (masterList[j].child != undefined) {
                    html += getBrandHtml(masterList[j].child);
                }
                html += "</li>";
            }
            return html;
        };
        //获取品牌html
        var getBrandHtml = function(brandList) {
            var html = "",
                isSerialType = (brandList.length > 0 && brandList[0].type == "cs");
            //主品牌下是否只有主品牌一级
            if (isSerialType) {
                html += "<div class=\"sub-list sub-sty\"><ul><li><ul class=\"sub-car-box\">";
            } else {
                html += "<div class=\"sub-list\"><ul>";
            }
            for (var k = 0; k < brandList.length; k++) {
                if (brandList[k].type == "cs") {
                    html += getSerialHtml(brandList[k]);
                } else {
                    var className = "brandType sub-name";
                    var curIdStr = "";
                    if (brandList[k].cur == 1) {
                        if (brandList[k].type == "cb") {
                            className += " check active";
                            curIdStr = " id=\"curObjTreeNode\"";
                        }
                    }
                    if (brandList.url == "")
                        html += "<li" + curIdStr + "><a class=\"" + className + "\"><span>" + brandList[k].name + "</span><em>(" + brandList[k].num + ")</em></a>";
                    else
                        html += "<li" + curIdStr + "><a href=\"" + brandList[k].url + "\" class=\"" + className + "\"><span>" + brandList[k].name + "</span><em>(" + brandList[k].num + ")</em></a>";
                    if (brandList[k].child != undefined) {
                        html += "<ul class=\"sub-car-box\">";
                        for (var i = 0; i < brandList[k].child.length; i++) {
                            html += getSerialHtml(brandList[k].child[i]);
                        }
                        html += "</ul>";
                    }
                    html += "</li>";
                }
            }
            if (isSerialType) {
                html += "</ul></li></ul></div>";
            } else {
                html += "</ul></div>";
            }
            return html;
        };
        //获取子品牌html
        var getSerialHtml = function(serial) {
            var html = "";
            var className = "subBrand";
            var curIdStr = "";
            if (serial.cur == 1) {
                className += " check";
                curIdStr = " id=\"curObjTreeNode\"";
            }
            if (defaults.params.tagtype == "chexing") {
                if (serial.salestate == "停销")
                    html += "<li" + curIdStr + " class=\"saleoff\"><a href=\"" + serial.url + "\" class=\"" + className + "\"><span>" + serial.name + "</sapn><b> 停售</b>";
                else
                    html += "<li" + curIdStr + "><a href=\"" + serial.url + "\" class=\"" + className + "\"><span>" + serial.name + "</span>"
                if (serial.butie == 1)
                    html += "<span class=\"green\">补贴</span>";
                html += "</a>";
            } else if (defaults.params.tagtype == "yanghu" || defaults.params.tagtype == "zhishu" || defaults.params.tagtype == "xiaoliang") {
                html += "<li" + curIdStr + "><a href=\"" + serial.url + "\" class=\"" + className + "\"><span>" + serial.name + "</span></a>";
            } else {
                html += "<li" + curIdStr + "><a href=\"" + serial.url + "\" class=\"" + className + "\"><span>" + serial.name + "</span><em>(" + serial.num + ")</em></a>";
            }
            html += "</li>";
            return html;
        };
        //获取字符html
        var getCharNavHtml = function(charObj) {
            var h = [];
            h.push("<div class=\"treeNum-box\" id=\"treeNum\"><ul>");
            var i = 0;
            for (var key in charObj) {
                i++;
                if (charObj[key] == 1)
                    h.push("<li class=\"t-" + key.toLowerCase() + "\"><a id=\"charhref_" + i + "\"  href=\"javascript:;\">" + key + "</a></li>");
                // else
                //     h.push("<li class=\"none t-" + key.toLowerCase() + "\">" + key + "</li>");
            }
            h.push("</ul></div>");
            return h.join('');
        };
        module.getLikeCar = function(viewCsIds) {
            var likeUrl = format(defaults.likeUrl, {
                "cityId": bit_locationInfo.cityId,
                "viewCsIds": viewCsIds,
                "defCsIds": defaults.likeDefCsIds,
                "top": defaults.likeCount
            });
            getData(likeUrl, "getLikeCallback", getLikeCallback);
        }
        var getLikeCallback = function(data) {
            //console.log(data);
            if (!(data && data.length > 0)) return;
            var h = [];
            h.push("<ul>");
            for (var i = 0; i < data.length; i++) {
                var url = format(defaults.likeDefLink, {
                    "serialId": data[i]["CsId"],
                    "allspell": "" + data[i]["CsSpell"] + ""
                });
                if (defaults.likeBlank) {
                    h.push("<li><div class=\"img-box\"><a href=\"" + url + "\" target=\"_blank\"><img src=\"" + data[i]["CsImage"] + "\" alt=\"\"></a></div><p><a href=\"" + url + "\" target=\"_blank\">" + data[i]["CsShowName"] + "</a></p></li>");
                } else {
                    h.push("<li><div class=\"img-box\"><a href=\"" + url + "\"><img src=\"" + data[i]["CsImage"] + "\" alt=\"\"></a></div><p><a href=\"" + url + "\">" + data[i]["CsShowName"] + "</a></p></li>");
                }
            };
            h.push("</ul>");
            document.getElementById("carList").innerHTML = h.join('');
            //LeftTreeMath.init();
        }
        return module;
    })(LeftTree || {});

    var LeftTreeMath = (function(module) {
        var self = module;

        var carList = document.getElementById("carList"), //热门车型容器
            treeNav = document.getElementById("treeNav"), //树形容器
            treeList = document.getElementById("treeList"), //主品牌容器
            navBox = document.getElementById("navBox"); //导航栏容器
        var isFixed = false;

        var navBoxOffsetTop = mathOffsetTop(navBox);

        module.init = function() {
            this.initEvent();
            this.scrollToCurrentTreeNode();
        }

        module.initEvent = function() {
                var charElement = document.getElementById("treeNum");
                addEvent(charElement, "click", mathTreeHref);
                this.mathScrollSettingTagBar();
            }
            //字母定位
        var mathTreeHref = function(e) {
                e = e || window.event;
                var target = e.srcElement || e.target;
                if (typeof (target.id) == "undefined" || target.id == "" || target.id == "treeNum") return;
                var curLitterNum = target.id.replace("charhref_", "");
                var hideItemAllHeight = 0;
                for (var i = 1; i < curLitterNum; i++) {
                    var hideItem = document.getElementById("letter" + i);
                    if (!hideItem)
                        continue;
                    var hideItemHeight = hideItem.offsetHeight;
                    hideItemAllHeight += hideItemHeight;
                }
                var treeBox = document.getElementById("treeList"); //树
                var carList = document.getElementById("carList"); 
                treeBox.scrollTop = hideItemAllHeight + carList.offsetHeight;
            }
            //计算当前节点到树形顶部距离
        var MathCurrentTreeNodeTop = function(curNode) {
            var topHeight = 0;
            if (!curNode)
                return topHeight;
            topHeight = mathOffsetTop(curNode, "treeList");
            return topHeight;
        }

        module.mathTreeBoxHeight = function (isNavFix) {
            var winH = mathWinHeigth();
            var scrollHeight = document.documentElement.scrollTop || document.body.scrollTop;
            var treeList = document.getElementById("treeList");
            var diffHeight = 0;
            if (isNavFix) {
                diffHeight = winH - document.getElementById("navBox").offsetHeight;// treeOffsetTop;
            } else {
                var treeOffsetTop = mathOffsetTop(treeList);
                diffHeight = winH - treeOffsetTop + scrollHeight;
            }
            treeList.style.height = diffHeight + "px";
        };
        //计算当前节点定位
        module.scrollToCurrentTreeNode = function() {
            var currentNode = document.getElementById("curObjTreeNode");

            var topHeight = MathCurrentTreeNodeTop(currentNode);

            //console.log(topHeight);
            var treeList = document.getElementById("treeList");
            treeList.scrollTop = topHeight;

        }

        //滚动计算树形高度
        module.mathScrollSettingTagBar = function() {

            var scrollHeight = document.documentElement.scrollTop || document.body.scrollTop;
            var navBox = document.getElementById("navBox");

            if (scrollHeight >= navBoxOffsetTop) {
                treeNav.style.top = navBox.offsetHeight + "px";
                if (!isFixed) {
                    isFixed = true;
                    treeNav.className = "treeNav treeFix";
                    navBox.className = "navBox navFix";
                    //self.mathTreeBoxHeight(true);
                    window.setTimeout(function() {
                        self.mathTreeBoxHeight(true);
                    }, 10);
                }
                
            } else {
                isFixed = false;
                treeNav.style.top = 0;
                treeNav.className = "treeNav";
                navBox.className = "navBox";
                window.setTimeout(function() {
                    self.mathTreeBoxHeight(false);
                }, 10);
            }
        }

        return module;
    })(LeftTreeMath || {});
    //树形数据回调方法
    function JsonpCallBack(data) {
            var jsonTree = data;
            var treeHtml = LeftTree.getTreeHtml(jsonTree);
            var treeBox = document.getElementById("treeNav");
            if (treeBox) {
                treeBox.innerHTML = treeHtml;
                getLikeCarByViewed();
            }
            addEvent(window, "load", LeftTreeMath.mathScrollSettingTagBar, false);
            addEvent(window, "resize", LeftTreeMath.mathScrollSettingTagBar, false);
            addEvent(window, "scroll", LeftTreeMath.mathScrollSettingTagBar, false);
            //初始化树形高度计算
            LeftTreeMath.init();
        }
        //通过浏览车型获取猜你喜欢
    function getLikeCarByViewed() {
        Bitauto.Login.onComplatedHandlers.push(function(loginResult) {
            var serialIdStr = [],
                isLogined = loginResult.isLogined;
            //已经登录，直接在回调loginResult里获取浏览过的车
            if (isLogined) {
                //服务端获取到的当前登录用户浏览过的车
                var objList = loginResult.viewedcars;
                //$.each(objList, function(i, item) {
                //    serialIdStr.push(item.CarSerialId);
                //});
                for (var i = 0; i < objList.length; i++) {
                    serialIdStr.push(objList[i].CarSerialId);
                }
            } else {
                //未登录 使用Bitauto.UserCar 对象获取cookie中的浏览过的车
                Bitauto.UserCars.getViewedCars(6, function() {
                    //cookie中记录的浏览过的车的数组 
                    serialIdStr = Bitauto.UserCars.viewedcar.arrviewedcar;
                });
            }
            viewCarList = serialIdStr;
            LeftTree.getLikeCar(viewCarList.join(','));
        });
    }

    //广告调用调整树高度
    function pullTopAd(adHeight) {
        LeftTreeMath.mathScrollSettingTagBar();
    }

    function pullFullScreenAd(adHeight) {
        LeftTreeMath.mathScrollSettingTagBar();
    }
    w.JsonpCallBack = JsonpCallBack;
    w.pullTopAd = pullTopAd;
    w.pullFullScreenAd = pullFullScreenAd;
    w.BitautoLeftTreeV2 = LeftTree;
})(window);