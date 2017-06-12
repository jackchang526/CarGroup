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
var arrMarkSerial = [], saleYearCount = 0,
    compareData = [],  //用于加号层显示车款全称+车款id
    duibiCarDataIds = [],//车款id集合，用于对比三级连选车款层
    currentDuibiCarId;  //从对比页面传入的车款值
var WaitCompare = (function (module) {
    var self = module,
		arrSelectCarId = [],  //已经选择过的车款
        defaults = {
            count: 4,
            duibiCookieName:"m_comparecarlist",
            historyCookieName: "m_historycomparecarlist",
            url: "http://car.m.yiche.com/handlers/getcarinfoforcompare.ashx?carid=",  //http://car.m.yiche.com/handlers/getcarinfoforcompare.ashx?carid=114462            selector: "a[id^='car-compare']",//绑定所有点击事件            oneSelector: "#car-compare-",//绑定单个点击事件            bind: function () {
                //绑定事件
                $(defaults.selector).off("click.addCompare").on("click.addCompare", function () {
                    if ($(this).parent().hasClass("btn-gray")) { return false; }
                    var carId = $(this).data("id"), carName = $(this).data("name");                    self.addCompareCar(carId, carName, $(this));
                });
            },            selectedFunc: function (carId) {
                //已添加的样式修改
                //$(defaults.oneSelector + carId).html("已加入").off("click").closest("li").addClass("btn-gray");
                $("a[id^='car-compare-"+carId+"']").html("已加入").off("click").closest("li").addClass("btn-gray");
            },            delFunc: function (carId) {
                //删除对比的调整
                //$(defaults.oneSelector + carId).html("加入对比").parent().removeClass("btn-gray");                //$(defaults.oneSelector + carId).off("click.addCompare").on("click.addCompare", function () {
                //    var carId = $(this).data("id"), carName = $(this).data("name");                //    WaitCompare.addCompareCar(carId, carName, $(this));
                //});

                $("a[id^='car-compare-" + carId + "']").html("加入对比").parent().removeClass("btn-gray");                $("a[id^='car-compare-" + carId + "']").off("click.addCompare").on("click.addCompare", function () {
                    var carId = $(this).data("id"), carName = $(this).data("name");                    self.addCompareCar(carId, carName, $(this));
                });
            },            clearFunc: function () {
                //清空对比数据
                $(defaults.selector).off("click.addCompare").html("加入对比").on("click.addCompare", function () {
                    var carId = $(this).data("id"), carName = $(this).data("name");                    self.addCompareCar(carId, carName, $(this));
                }).parent().removeClass("btn-gray");
            },            selectCarIdFunc: function (id, name,from) {
                //选择车款的事件
                var $addElem = $(defaults.oneSelector + id);                self.updateHistoryCars(id);  //添加到历史记录缓存                self.addCompareCar(id, name, $addElem,from);
            }
        };
    //添加对比
    module.addCompareCar = function (id, name, elem,from) {
        if (!from) {   //从选车页面中选择车款时，不加动画效果 
            self.addDuibiAnimation(elem);
        }
        var cookieCar = CookieHelper.getCookie(defaults.duibiCookieName),
            arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
        }
        if (arrCarId.length >= 4) {
            //最多4款
            alert("最多添加4款车");
            return;
        }
        if (arrCarId.indexOf(id) != -1) {
            //已存在
            alert("添加车款已经存在");
            return;
        }
        arrCarId.push(id);
        CookieHelper.setCookie(defaults.duibiCookieName, arrCarId.join('|'));
        self.updateHistoryCars(id);
        compareData.push({
            CarId: id,
            CarName: name
        });
        drawDuibiBtnUI();
        self.updateCount();
        if (defaults.selectedFunc && defaults.selectedFunc instanceof Function) {
            defaults.selectedFunc(id);
        }
    };
    //清空对比
    module.clearCompareCarAll = function () {
        CookieHelper.clearCookie(defaults.duibiCookieName);
        compareData = [];
        drawDuibiBtnUI(compareData);
        self.updateCount();
        //rightSwipe();
        if (defaults.clearFunc && defaults.clearFunc instanceof Function) {
            defaults.clearFunc();
        }
    };
    //删除对比车款
    module.delCompareCar = function (carId) {
        var cookieCar = CookieHelper.getCookie(defaults.duibiCookieName),
            arrCarId = [],
            newCompareData = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
            if (arrCarId.length > 0)
            {
                arrCarId.remove(carId);
            }
        }
        CookieHelper.setCookie(defaults.duibiCookieName, arrCarId.join('|'));
        for (var i = 0; i < compareData.length; i++) {
            if (compareData[i].CarId == carId) continue;
            newCompareData.push(compareData[i]);
        };
        compareData = newCompareData;
        drawDuibiBtnUI();
        self.updateCount();
        if (defaults.delFunc && defaults.delFunc instanceof Function) {
            defaults.delFunc(carId);
        }
    };
    //开始对比
    module.submitCompare = function () {
        var cookieCar = CookieHelper.getCookie(defaults.duibiCookieName),
			arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
        }
        if (arrCarId.length < 1) {
            //请添加车款
            alert("至少选择1款车对比");
            return;
        }
        // alert("提交对比车款："+arrCarId.join(','))
        window.location.href = '/chexingduibi/?carids='+arrCarId.join(',');
    }
    //初始化 对比数据
    module.initCompreData = function (options) {
        $.extend(true, defaults, options);
        var cookieCar = CookieHelper.getCookie(defaults.duibiCookieName),
			arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
        }
        if (arrCarId.length > 0) {
            $(arrCarId).each(function (index, item) {
                defaults.selectedFunc(item);
            });
            getData(defaults.url + arrCarId.join(','), function (data) {
                if (data && data.length > 0) {
                    compareData.length = 0;
                    for (i = 0; i < (data.length) ; i++) {
                        compareData.push({
                            CarId: data[i].CarId,
                            CarName: data[i].CarName
                        });
                    }
                }
                rightSwipe();
                self.updateCount();
            });
        } else {
            rightSwipe();
            self.updateCount();
        }
        if (defaults.bind && defaults.bind instanceof Function) {
            defaults.bind();
        }
    };
    //更新pk数量
    module.updateCount = function () {
        var count = 0;
        var cookieCar = CookieHelper.getCookie(defaults.duibiCookieName),
           arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
            count = arrCarId.length;
        }
        $("#compare-pk i").html(count);
    };
    module.addDuibiAnimation = function ($btn) {
        //添加“加对比”飞行效果start
        var thisX = $($btn).offset().left, // 获取当前btn位置
            thisY = $($btn).offset().top,
            endX = $('#itemDuibi').offset().left, // 获取对比球位置
            endY = $('#itemDuibi').offset().top,
            thisWidth = $($btn).width(); // 获取当前btn宽度
        // 切换文字
        //$(this).parent().addClass('btn-gray');
        //$(this).text('已加入');
        // 创建飞行元素
        $('body').append('<div class="add-duibi-box"></div>');
        // 飞行元素位置
        $('.add-duibi-box').css({
            top: thisY + 9,
            left: thisX + (thisWidth / 2) - 30
        });
        // 飞行动画
        $('.add-duibi-box').animate(
            { left: endX, top: endY, opacity: 0 }, 500, function () {
                $('.add-duibi-box').remove();
            });
        // 对比按钮动画
        $('#duibiAti').addClass('float-pk-ati');
        setTimeout(function () {
            $('#duibiAti').removeClass('float-pk-ati');
        }, 1000);
        //添加“加对比”飞行效果end
    }
    module.updateHistoryCars = function (curNewCarId) {
        var historyCookieCar = CookieHelper.getCookie(defaults.historyCookieName);
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
        CookieHelper.setCookie(defaults.historyCookieName, newHistoryCars.join('|'));
    }
    //重画加号所在层的dom
    var drawDuibiBtnUI = function () {
        var resultlist = {
            list: {
                SelectedCars: compareData,
                AddLables: { Count: 4 - compareData.length }
            }
        };
        $(".duibi-leftPopup .swipeLeft").swipeData({
            data: resultlist,
            templateid: '#duibibtnTemplate',
            callback: function (data) {
                this.html(data);
                _headerSlider.call(this);
                //$body.find("#container")[0].style.cssText = '';
                bindEvent();
            }
        });
    }
    //绑定事件
    var bindEvent = function () {
        api.brand.currentid = apiBrandId;   //品牌
        api.car.currentid = apiSerialId;  //车系
        api.model.currentid = apiCarId;//车款
        $(".first-list .add a:not(.select)").rightSwipeAnimation({
            fnEnd: function () {
                var curSerialId = defaults.serialid;//
                var $model = this;
                $body.animate({ scrollTop: 0 }, 30);
                var tags = $('.tag', $body);
                //切换标签
                $('.brandlist').tag({
                    tagName: '.first-tags',
                    fnEnd: function (idx) {
                        tags.hide();
                        if ((api.model.currentid == 0 || api.car.currentid == 0 || api.brand.currentid == 0)) {
                        }
                        tags.eq(idx).show();

                        if (idx == 0) {     //当前品牌
                            $('.curSerialSub', $body).swipeApi({
                                url: "http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&datatype=1&pid=" + curSerialId,
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
                                        } else {
                                            arrMarkSerial[n.replace('s', '')] = false;
                                        }
                                    }
                                    return data;
                                },
                                callback: function (html) {
                                    this.html(html);
                                    _bindSelectedEvent();
                                    var curSerialName = $("#curSerialName").val();
                                    $('.curSerialName').html(curSerialName);

                                    $(this).find(".pic-txt-h li:not(li[class]) a").on("click", function () {
                                        var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                                        defaults.selectCarIdFunc(curCarId, curCarName,true);
                                        $model.trigger('closeWindow');
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
                            var historyCookieCar = CookieHelper.getCookie(defaults.duibiCookieName);
                            if (historyCookieCar) {
                                duibiCarDataIds = historyCookieCar.split('|');
                            }
                            //初始化品牌
                            $body.trigger('brandinit', { actionName3: '[data-action=duibi-models]', leftmaskalert: '.duibi-alert', leftmaskback: '.leftmask3', carselect: function () { }, masterselect: function () { }, selectmark: function () { } });
                            //车款点击回调事件
                            api.model.clickEnd = function (paras) {
                                var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                                if ($(this).text().indexOf('已添加') > -1 || $(this).text().indexOf('当前') > -1) {
                                    return;
                                }
                                defaults.selectCarIdFunc(curCarId, curCarName,true);
                                var $back = $('.' + $leftPopupModels.attr('data-back'));
                                //关闭浮层
                                $back.trigger('close');
                                $model.trigger('closeWindow');
                                
                            }
                            $body.css('overflow', 'initial');
                            _commonSlider($model, $body);
                        }
                        else if (idx == 2) //历史记录
                        {
                            
                            initHistory();
                            _commonSlider($model, $body);
                        }
                        else { }

                        $model.find('.btn-return').click(function (ev) {
                            ev.preventDefault();
                            $model.trigger('closeWindow');
                        })

                    }
                });
            }
        });
        $(".first-list a.select").on("click", function () {
            return false;
        });
        $(".swipeLeft .first-list li .btn-close").each(function (index, curr) {
            $(curr).on("click", function (ev) {
                ev.preventDefault();
                var carId = $(this).data("carid");
                self.delCompareCar(carId);
            })
        })
        if (compareData.length <= 0) {
            $(".btn-comparison").off("click").addClass("disable");
            $(".btn-clear").off("click").addClass("disable");
        } else {
            $(".btn-comparison").off("click").removeClass("disable").on("click", function (e) {
                e.preventDefault();
                self.submitCompare();
            });
            $(".btn-clear").off("click").removeClass("disable").on("click", function (e) {
                e.preventDefault();
                self.clearCompareCarAll();
            });
        }
    };

    //层自适应    var _commonSlider = function ($model, $body) {
        if ($model.height() > $(document.body).height()) {
            $(document.body).height($model.height())
        } else if ($model.height() < $(document.body).height()) {
            $('#container', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight - $('.b-return', $model).height());
            $('.brandlist').height(document.documentElement.clientHeight - $('.b-return', $model).height());
            $(document.body).height(document.documentElement.clientHeight);
        }
    }
    var _commonResetBody = function ($body) {
        $body[0].style.cssText = '';
        $body.find("#container")[0].style.cssText = '';
    }
    //头部自适应
    var _headerSlider = function () {
        var $swipeLeft = this,
              $leftPopup = $swipeLeft.parent();
        var $back = $('.' + $leftPopup.attr('data-back'))
        $back.touches({ touchstart: function (ev) { ev.preventDefault(); }, touchmove: function (ev) { ev.preventDefault(); } });
        var $swipeLeft = $leftPopup.find('.swipeLeft');
        $swipeLeft.touches({ touchmore: function (ev) { ev.preventDefault(); } });
        var $y2015 = $swipeLeft.find('.y2015-car-01');
        var $cbox = $y2015.children(0);
        $cbox.height(document.documentElement.clientHeight - 50);
        $cbox.iScroll({ snap: 'li' });
        $swipeLeft.find('.swipeLeft-header').show();
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
    //已选择车型
    var _bindSelectedEvent = function () {
        for (var i = 0; i < compareData.length; i++) {
            if (!isNaN(currentDuibiCarId) && currentDuibiCarId > 0 && compareData[i].CarId == currentDuibiCarId) {
                $("#li_" + compareData[i].CarId).attr("class", "current").find("h4").prepend("[当前]");
            }
            else {
                $("#li_" + compareData[i].CarId).attr("class", "none").find("h4").prepend("[已添加]");
            }
        }
    }
    //右侧弹出效果
    var rightSwipe = function () {
        $body.trigger('publicswipe2', {
            actionName: '[data-action=duibicar]',
            templateid: '#duibibtnTemplate',
            back: '.duibi-leftmask',
            alert: '.duibi-leftPopup',
            swipeLeftChildren: '.y2015-car-01',
            ds: {},
            fliterData: function (ds, paras) {
                var templateListObject = {
                    list: {
                        SelectedCars: compareData,
                        AddLables: { Count: 4 - compareData.length }
                    }
                };
                ds = templateListObject;
                return ds;
            },
            fnEnd: function () {
                var $swipeLeft = this, $back = $('.' + $swipeLeft.parent().attr('data-back'));
                //调用绑定加号事件
                bindEvent();
                //$back.trigger('close');
            },
            closeEnd: function () {
                this.parents("#container").css({ height: '100%', overflow: 'inherit' });
                _commonResetBody($body);
            }
        });
    };
    //历史记录
    var initHistory = function () {
        var url = "/handlers/getcarinfoforcompare.ashx?carid=",  //http://car.m.yiche.com
            arrCarId = [],
            h = [];
        //已选择车款
        var duibiCookieCar = CookieHelper.getCookie(defaults.duibiCookieName);
        if (duibiCookieCar) {
            duibiCarDataIds = duibiCookieCar.split('|');
        }
        else {
            duibiCarDataIds = [];
        }
        var historyCookieCar = CookieHelper.getCookie(defaults.historyCookieName);
        if (historyCookieCar) {
            arrCarId = historyCookieCar.split('|');  
        }
        if (arrCarId.length > 0) {
            //获取对比数据
            getData(url+arrCarId.join(','), function (data) {
                if (!(data && data.length > 0)) return;
                h.push("<ul>");
                for (var i = 0; i < data.length; i++) {
                    h.push("            <li id=\"history-" + data[i].CarId + "\"" + (duibiCarDataIds.Contains(data[i].CarId) ? " class='none'" : "") + ">");
                    h.push("                <a href=\"javascript:;\" data-id=\"" + data[i].CarId + "\" data-name=\"" + data[i].CarName + "\">");
                    h.push("                    <h4>" + (data[i].CarId == currentDuibiCarId ? "[当前]" : (duibiCarDataIds.Contains(data[i].CarId) ? "[已添加]" : "")) + data[i].CarName + "</h4>");
                    h.push("                    <p><strong>" + (data[i].CarReferPrice == 0 ? "暂无" : data[i].CarReferPrice + "万") + "</strong></p>");//
                    h.push("                </a>");
                    h.push("            </li>");
                };
                h.push("</ul>");
                $(".brandlist .history").html(h.join(''));
                $(".brandlist .history").removeClass("none-block");
                $(".brandlist .history").find("li:not(li[class]) a").on("click", function () {
                    var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                    defaults.selectCarIdFunc(curCarId, curCarName,true);
                    $(this).trigger('closeWindow');
                });
            });
        } else {
            $(".brandlist .history").addClass("none-block").html("<p>您还未对比过任何车款</p>");
        }
        _commonSlider($(".brandlist .history"), $body);
    }

    var getData = function (url, callbackFunc) {
            $.ajax({
                url: url,
                cache: true,
                dataType: "jsonp",
                jsonpCallback: "getData",
                success: function (data) {
                    callbackFunc(data);
                },
                error: function (errorMsg) {
                }
            });
    };
    return module;
})(WaitCompare || {});

Object.extend = function (destination, source) {
    if (!destination) return source;
    for (var property in source) {
        if (!destination[property]) {
            destination[property] = source[property];
        }
    }
    return destination;
}

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
