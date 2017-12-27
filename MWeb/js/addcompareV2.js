Array.prototype.indexOf = function (value) {
    for (var i = 0, l = this.length; i < l; i++)
        if (this[i] == value) return i;
    return -1;
}
Array.prototype.remove = function (b) {
    var a = this.indexOf(b);
    if (a >= 0) {
        this.splice(a, 1);
        return true;
    }
    return false;
};
Array.prototype.Contains = function (element) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == element) {
            return true;
        }
    }
    return false;
};
var $body = $('body');
var arrMarkSerial = [], saleYearCount = 0,duibiCarDataIds = []; 
var currentDuibiCarId;  //从对比页面传入的车款值
var SelectCar = (function (module) {
    var self = module,
        historyCookieName = "m_historycomparecarlist",
        defaults = {
            serialId: 0,
            currentSerialName: "",
            currentCarId: null,
            url: "http://car.m.yiche.com/handlers/getcarinfoforcompare.ashx?carid=",//http://car.m.yiche.com
            cacheData: {},
            callbackFunc: null,
            footerCallback: null,
            arrCarId: []
        };
    //绑定事件
    module.bindAddEvent = function (options, addselector) {
        $.extend(true, defaults, options);
        $(addselector).each(function (index, item) {    //[".car-item-add",".car-item a.duibi-box"]
            api.brand.currentid = apiBrandId;   //品牌
            api.car.currentid = apiSerialId;  //车系
            api.model.currentid = apiCarId;//车款
            $(item).each(function (i, singleItem) {
                $(singleItem).rightSwipeAnimation({
                    fnEnd: function () {
                        var $model = this;
                        defaults.serialId = (index == 0) ? defaults.serialId : ($(singleItem).length > 0 ? $(singleItem).data("serial") : defaults.serialId);//
                        defaults.currentIndex = $(singleItem).data("index");
                        currentDuibiCarId = (index == 1) ? ($(singleItem).length > 0 ? $(singleItem).data("car") : 0) : 0;

                        $body.animate({ scrollTop: 0 }, 30);
                        var tags = $('.tag', $body);

                        if ($(singleItem).attr("class") == "car-item-add")   //如果是点击加号弹出层，则判断是否有已经选择的车款
                        {
                            var duibiBoxCount = $(".car-item a.duibi-box").length;
                            if (duibiBoxCount == 0)   //如果没有已选择车款，则
                            {
                                defaults.serialId = 0;
                            }
                            else {
                                defaults.serialId = $(singleItem).parent().prev().find(".car-item a.duibi-box").data("serial");
                            }
                        }
                        //切换标签
                        var $brandlist = $('.brandlist');
                        //$brandlist.parents('#master_container').show();
                        $brandlist.tag({
                            tagName: '.first-tags',
                            fnEnd: function (idx) {
                                tags.hide();
                                if (idx==0&&defaults.serialId == 0) {
                                    idx = 1;
                                    $(".brandlist .first-tags ul li").eq(idx).addClass("current").prev().hide();
                                    $(".brandlist .curSerialSub").hide();
                                } else {
                                    var  $brandtag = $(".brandlist .first-tags ul li");
                                    $brandtag.removeClass('current');
                                    $brandtag.eq(idx).addClass("current").show();
                                }
                                
                                tags.eq(idx).show();

                                if (idx == 0) {     //当前品牌
                                    $('.curSerialSub', $body).swipeApi({
                                        url: "http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&datatype=1&pid=" + defaults.serialId,
                                        templateid: '#curSerialTemplate',
                                        flatFn: function (data) {
                                            for (var n in data.CarList) {
                                                var SaleStateCount = 0;
                                                for (var i = 0; i < data.CarList[n].length; i++) {
                                                    if (data.CarList[n][i].SaleState != '停销') {
                                                        SaleStateCount++;
                                                    }
                                                }
                                                if (SaleStateCount > 0) {
                                                    arrMarkSerial[n.replace('s', '')] = true;
                                                    saleYearCount++;
                                                }
                                                else {
                                                    arrMarkSerial[n.replace('s', '')] = false;
                                                }
                                            }
                                            return data;
                                        },
                                        callback: function (html) {
                                            this.html(html);

                                            //已选择车款
                                            if (options.arrCarId.length > 0) {
                                                _bindSelectedEvent(options.arrCarId);
                                            }
                                            var curSerialName = $("#curSerialName").val();
                                            $('.curSerialName').html(curSerialName);

                                            $(this).find(".pic-txt-h li:not(li[class]) a").on("click", function () {
                                                var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                                                if ($(this).text().indexOf('已添加') > -1 || $(this).text().indexOf('当前') > -1) {
                                                    return;
                                                }
                                                if (defaults.callbackFunc) {
                                                    defaults.callbackFunc(curCarId, curCarName, defaults.currentIndex);
                                                    self.updateHistoryCars(curCarId);
                                                }
                                                $model.trigger('closeWindow');
                                                _commonResetBody($body);
                                            });

                                            //年款收缩与展开
                                            //获取第二层的高度值
                                            var heights = [];
                                            function toResize() {
                                                heights.length = 0;
                                                $('.tag .y2015 ul').each(function (index, curr) {
                                                    heights[index] = $(curr).height();
                                                })
                                            }

                                            $(window).resize(toResize).trigger('resize');
                                            this.find('[data-slider=pic-txt-hh]').sliderBox({
                                                heightFn: function (index) { return heights[index]; },
                                                isCloseFn: function (idx, index) {
                                                    var isopen = !this.hasClass('open');
                                                    if (isopen) {
                                                        this.prev().find('i').removeClass('up');
                                                    }
                                                    else {
                                                        this.prev().find('i').addClass('up');
                                                    }
                                                    return isopen;
                                                },
                                                onlyOne: settings.sliderBox.onlyOne,
                                                clickEnd: function (paras) {
                                                    if (settings.sliderBox.onlyOne) {
                                                        this.parent().find('.tt-small i').removeClass('up');
                                                    }
                                                    if (paras.k == 'up') {
                                                        this.find('i').removeClass('up');
                                                    } else {
                                                        this.find('i').addClass('up');
                                                    }
                                                    removeborder.call($model);
                                                }
                                            });

                                            $body.css('overflow', 'initial');
                                            _commonSlider($model, $body);
                                        }
                                    });
                                }
                                else if (idx == 1)  //按品牌查找 
                                {
                                    //已选择车款
                                    duibiCarDataIds = options.arrCarId;
                                    //初始化品牌
                                    $body.trigger('brandinit', { init: function () { $("span.brand-logo>img").lazyload({ effect: "fadeIn", threshold: 50 }) }, actionName3: '[data-action=duibi-models]', leftmaskalert: '.duibi-alert', leftmaskback: '.leftmask3', carselect: function () { }, masterselect: function () { }, selectmark: function () { } });

                                    //车款点击回调事件
                                    api.model.clickEnd = function (paras) {
                                        var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                                        if ($(this).text().indexOf('已添加') > -1 || $(this).text().indexOf('当前') > -1) {
                                            return;
                                        }
                                        if (defaults.callbackFunc) {
                                            defaults.callbackFunc(curCarId, curCarName, defaults.currentIndex);
                                            self.updateHistoryCars(curCarId);
                                        }
                                        var $back = $('.' + $leftPopupModels.attr('data-back'));
                                        //关闭浮层
                                        $back.trigger('close');
                                        $model.trigger('closeWindow');
                                        _commonResetBody($body);
                                    }
                                }
                                else if (idx == 2) //历史记录
                                {
                                    $(".brandlist .first-tags ul li").eq(idx).addClass("current").siblings().removeClass("current");
                                    //已选择车款
                                    duibiCarDataIds = options.arrCarId;
                                    self.initHistory(duibiCarDataIds);
                                }
                                else { }

                                $model.find('.btn-return').click(function (ev) {
                                    ev.preventDefault();
                                    $model.trigger('closeWindow');
                                    _commonResetBody($body);
                                })

                            }
                        });
                    }
                });
            });
        });
    };
   
    module.getData = function (url, callbackFunc, sync) {
        try {
            $.ajax({
                url: url,
                cache: true,
                async: (sync ? false : true),
                dataType: "jsonp",
                jsonpCallback: "getData",
                success: function (data) {
                    //var json = $.parseJSON(data);
                    //callbackFunc(json);

                    callbackFunc(data);
                }
            });
        }
        catch (e) {  }
    }
    module.initHistory = function (arrSelectCars) {
        var url = "/handlers/getcarinfoforcompare.ashx?carid=",  //http://car.m.yiche.com
            historyCarIds = [],
            h = [];
        var historyCookieCar = CookieHelper.getCookie(historyCookieName);
        if (historyCookieCar) {
            historyCarIds = historyCookieCar.split('|');
        }
        if (historyCarIds.length > 0) {
            self.getData(url + historyCarIds.join(','), function (data) {
                if (!(data && data.length > 0)) {
                    $(".brandlist .history").addClass("none-block").html("<p>未获取到对比车款信息...</p>");
                    return;
                }
                h.push("<ul>");
                for (var i = 0; i < data.length; i++) {
                    h.push("            <li id=\"history-" + data[i].CarId + "\"" + (arrSelectCars.Contains(data[i].CarId) ? " class='none'" : "") + ">");
                    h.push("                <a href=\"javascript:;\" data-id=\"" + data[i].CarId + "\" data-name=\"" + data[i].CarName + "\">");
                    h.push("                    <h4>" + (data[i].CarId == currentDuibiCarId ? "[当前]" : (arrSelectCars.Contains(data[i].CarId) ? "[已添加]" : "")) + data[i].CarName + "</h4>");
                    h.push("                    <p><strong>" + (data[i].CarReferPrice == 0 ? "暂无" : data[i].CarReferPrice + "万") + "</strong></p>");//
                    h.push("                </a>");
                    h.push("            </li>");
                };
                h.push("</ul>");

                $(".brandlist .history").html(h.join(''));
                $(".brandlist .history").removeClass("none-block");
                $(".brandlist .history").find("li:not(li[class]) a").on("click", function () {
                    var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                    if (defaults.callbackFunc) {
                        defaults.callbackFunc(curCarId, curCarName, defaults.currentIndex);
                        self.updateHistoryCars(curCarId);
                    }
                    $(this).trigger('closeWindow');
                    _commonResetBody($body);
                });
            });
        }
        else {
            $(".brandlist .history").addClass("none-block").html("<p>您还未对比过任何车款</p>");
        }
        _commonSlider($(".brandlist .history"), $body);
    }
    module.updateHistoryCars = function (curNewCarId) {
        var historyCookieCar = CookieHelper.getCookie(historyCookieName);
        var newHistoryCars = [];
        if (historyCookieCar) {
            newHistoryCars = historyCookieCar.split('|');
            if (!newHistoryCars.Contains(curNewCarId)) {
                if (newHistoryCars.length >= 8) {
                    newHistoryCars.splice(0, 1);
                }
            }
            else {
                return;
            }
        }
        newHistoryCars.push(curNewCarId);
        CookieHelper.setCookie(historyCookieName, newHistoryCars.join('|'));
    }
    //已选择车型
    var _bindSelectedEvent = function (arrSelectCarIds) {
        for (var i = 0; i < arrSelectCarIds.length; i++) {
            if (!isNaN(currentDuibiCarId) && currentDuibiCarId > 0 && arrSelectCarIds[i] == currentDuibiCarId) {
                $("#li_" + arrSelectCarIds[i]).attr("class", "current").find("h4").prepend("[当前]");
            }
            else {
                $("#li_" + arrSelectCarIds[i]).attr("class", "none").find("h4").prepend("[已添加]");
            }
        }
    }

    //层自适应    var _commonSlider = function ($model, $body) {
        if ($model.height() > $(document.body).height()) {
            $(document.body).height($model.height())
        } else if ($model.height() < $(document.body).height()) {
            $('#container', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight);
            $('.brandlist').height(document.documentElement.clientHeight);
            $(document.body).height(document.documentElement.clientHeight)
        }
    }
    var _commonResetBody = function ($body) {
        $body[0].style.cssText = '';
        $body.find("#container")[0].style.cssText = '';
    }
    //折叠下划线控制
    function removeborder() {
        var $smalls = this.find('.tt-small');
        $smalls.each(function (index, curr) {
            var $current = $(curr);
            setTimeout(function () {
                if (index < $smalls.length - 1) {
                    if ($current.next().height() == 0) {
                        $current.css('border-bottom', '1px solid #E9E9E9');
                    } else {
                        $current.css('border-bottom', '0px solid #E9E9E9');
                    }
                }
            }, 300)
        })
    }

    return module;
})(SelectCar || {});

var CookieHelper = (function (module) {
    var self = module,
        defaults = {
            domain: "car.m.yiche.com",
            expires: 30 * 30,
            path: "/"
        };
    // module.setCookie = function(name, value, expires, path, domain, secure) {
    //     expiryday = new Date();
    //     expiryday.setTime(expiryday.getTime() + 30 * 30 * 24 * 60 * 60 * 1 * 1000);
    //     document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() +
    //         ((path) ? "; path=" + path : "; path=/") +
    //         "; domain=" + defaults.domoin + "" +
    //         ((secure) ? "; secure" : "");
    // };
    module.setCookie = function (name, value, options) {
        Object.extend(defaults, options);

        if (typeof value != 'undefined') { // name and value given, set cookie
            options = options || {};
            if (value === null) {
                value = '';
                options.expires = -1;
            }
            var expires = '';
            if (defaults.expires && (typeof defaults.expires == 'number' || defaults.expires.toUTCString)) {
                var date;
                if (typeof defaults.expires == 'number') {
                    date = new Date();
                    date.setTime(date.getTime() + (defaults.expires * 24 * 60 * 60 * 1000));
                } else {
                    date = defaults.expires;
                }
                expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
            }
            var path = defaults.path ? '; path=' + defaults.path : '';
            var domain = defaults.domain ? '; domain=' + defaults.domain : '';
            var secure = defaults.secure ? '; secure' : '';
            document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
        }
    };
    module.getCookie = function (name) {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));

        if (arr != null) {
            return unescape(arr[2]);
        }
        return null;
    };
    module.clearCookie = function (name, path, domain) {
        if (self.getCookie(name)) {
            document.cookie = name + "=" +
                ((path) ? "; path=" + path : "; path=/") +
               (defaults.domain ? '; domain=' + defaults.domain : '') +
                ";expires=Fri, 02-Jan-1970 00:00:00 GMT";
        }
    }
    return module;
})(CookieHelper || {});
