//依赖文件paowuxian.js
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
            duibiCookieName: "m_comparecarlist",
            duibiLocalName: "car_m_localcomaprelist",
            historyCookieName: "m_historycomparecarlist",
            historyLocalName: "car_m_localhistorycomparelist",
            url: "/handlers/getcarinfoforcompare.ashx?carid=",  //http://car.m.yiche.com/handlers/getcarinfoforcompare.ashx?carid=114462
            selector: "a[id^='car-compare']",//绑定所有点击事件
            oneSelector: "#car-compare-",//绑定单个点击事件
            bind: function () {
                //绑定事件
                $(defaults.selector).off("click.addCompare").on("click.addCompare", function () {
                    if ($(this).parent().hasClass("btn-gray")) { return false; }
                    var carId = $(this).data("id"), carName = $(this).data("name");
                    self.addCompareCar(carId, carName, $(this));
                });
            },
            selectedFunc: function (carId) {
                //已添加的样式修改
                //$(defaults.oneSelector + carId).html("已加入").off("click").closest("li").addClass("btn-gray");
                $("a[id^='car-compare-" + carId + "']").html("已加入").off("click").parent().addClass("btn-gray");
            },
            delFunc: function (carId) {
                //删除对比的调整
                //$(defaults.oneSelector + carId).html("加入对比").parent().removeClass("btn-gray");
                //$(defaults.oneSelector + carId).off("click.addCompare").on("click.addCompare", function () {
                //    var carId = $(this).data("id"), carName = $(this).data("name");
                //    WaitCompare.addCompareCar(carId, carName, $(this));
                //});

                $("a[id^='car-compare-" + carId + "']").html("加对比").parent().removeClass("btn-gray");
                $("a[id^='car-compare-" + carId + "']").off("click.addCompare").on("click.addCompare", function () {
                    var carId = $(this).data("id"), carName = $(this).data("name");
                    self.addCompareCar(carId, carName, $(this));
                });
            },
            clearFunc: function () {
                //清空对比数据
                $(defaults.selector).off("click.addCompare").html("加对比").on("click.addCompare", function () {
                    var carId = $(this).data("id"), carName = $(this).data("name");
                    self.addCompareCar(carId, carName, $(this));
                }).parent().removeClass("btn-gray");
            },
            selectCarIdFunc: function (id, name) {
                //选择车款的事件
                var $addElem = $(defaults.oneSelector + id);
                self.updateHistoryCars(id);  //添加到历史记录缓存
                self.addCompareCar(id, name, $addElem);
            },
            myParabola: function () {//抛物线效果
                var thisX = $(event.target).offset().left, // 获取当前btn位置
                    thisY = $(event.target).offset().top,
                    endX = $('.float-r-pk').offset().left, // 获取对比球位置
                    endY = $('.float-r-pk').offset().top,
                    thisWidth = $(event.target).width(); // 获取当前btn宽度
                // 飞行元素位置
                $('.add-duibi-box').css({
                    top: thisY + 9,
                    left: thisX + (thisWidth / 2) - 30,
                    opacity: 0.6,
                    zindex:100000000
                });

                // 飞行动画
                $('.add-duibi-box').show().animate(
                    { left: endX - 5, top: endY + 10, opacity: 0 }, 500, function () {
                        $('.add-duibi-box').hide();
                    });
            }
        };
    //添加对比
    module.addCompareCar = function (id, name, elem) {
        var cookieCar = LocalStorageData.getData(defaults.duibiLocalName);// CookieHelper.getCookie(defaults.duibiCookieName),
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
        //CookieHelper.setCookie(defaults.duibiCookieName, arrCarId.join('|'));
        LocalStorageData.setData(defaults.duibiLocalName, arrCarId.join('|'));

        self.updateHistoryCars(id);
        compareData.push({
            CarId: id,
            CarName: name
        });
        drawDuibiBtnUI();
        self.updateCount(true);
        //加对比效果
        defaults.myParabola();
        if (defaults.selectedFunc && defaults.selectedFunc instanceof Function) {
            defaults.selectedFunc(id);
        }
        CarCompareAd.initAd(compareData);
    };
    //清空对比
    module.clearCompareCarAll = function () {
        LocalStorageData.clearData(defaults.duibiLocalName);
        compareData = [];
        drawDuibiBtnUI(compareData);
        self.updateCount(true);
        //rightSwipe();
        if (defaults.clearFunc && defaults.clearFunc instanceof Function) {
            defaults.clearFunc();
        }
        CarCompareAd.initAd(compareData);

    };
    //删除对比车款
    module.delCompareCar = function (carId) {
        var cookieCar = LocalStorageData.getData(defaults.duibiLocalName); //CookieHelper.getCookie(defaults.duibiCookieName),
            arrCarId = [],
            newCompareData = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
            if (arrCarId.length > 0) {
                arrCarId.remove(carId);
            }
        }
        //CookieHelper.setCookie(defaults.duibiCookieName, arrCarId.join('|'));
        LocalStorageData.setData(defaults.duibiLocalName, arrCarId.join('|'));
        for (var i = 0; i < compareData.length; i++) {
            if (compareData[i].CarId == carId) continue;
            newCompareData.push(compareData[i]);
        };
        compareData = newCompareData;
        drawDuibiBtnUI();
        
        self.updateCount(true);
        if (defaults.delFunc && defaults.delFunc instanceof Function) {
            defaults.delFunc(carId);
        }

        CarCompareAd.initAd(compareData);
    };
    //开始对比
    module.submitCompare = function () {
        var cookieCar = LocalStorageData.getData(defaults.duibiLocalName);// CookieHelper.getCookie(defaults.duibiCookieName),
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
        window.location.href = '/chexingduibi/?carids=' + arrCarId.join(',');
    }
    //初始化 对比数据
    module.initCompreData = function (options) {
        $.extend(true, defaults, options);

        var cookieCar = CookieHelper.getCookie(defaults.duibiCookieName), //将cookie存到storage,并清空cookie
			arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
            LocalStorageData.setData(defaults.duibiLocalName, cookieCar);
            CookieHelper.clearCookie(defaults.duibiCookieName);
        }
        var storageCar = LocalStorageData.getData(defaults.duibiLocalName);
        if (storageCar) {
            arrCarId = storageCar.split('|');
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
                            CarName: data[i].CarName,
                            ReferPrice: data[i].CarReferPrice,
                            CsId:data[i].SerialId
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
    module.updateCount = function (isanimation) {
        var count = 0;
        var cookieCar = LocalStorageData.getData(defaults.duibiLocalName);// CookieHelper.getCookie(defaults.duibiCookieName),
           arrCarId = [];
        if (cookieCar) {
            arrCarId = cookieCar.split('|');
            count = arrCarId.length;
        }
        if (isanimation) {
            $('#duibiAti').addClass('float-pk-ati');
            setTimeout(function () {
                $('#duibiAti').removeClass('float-pk-ati');
            }, 1000);
            setTimeout(function () {
                $("#compare-pk i").html(count);
            }, 500);
        }
        else {
            $("#compare-pk i").html(count);
        }
    };
    module.updateHistoryCars = function (curNewCarId) {
        var historyCookieCar = CookieHelper.getCookie(defaults.historyCookieName);
        var newHistoryCars = [];
        if (historyCookieCar) {
            LocalStorageData.setData(defaults.historyLocalName, historyCookieCar); //将历史记录保存到localstorage
            CookieHelper.clearCookie(defaults.historyCookieName);
        }
        historyCookieCar = LocalStorageData.getData(defaults.historyLocalName);
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
        LocalStorageData.setData(defaults.historyLocalName, newHistoryCars.join('|'));
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
                //$("#master_container").scrollTop = 0;
                var tags = $('.tag', $body);
                //切换标签
                var $brandlist = $('.brandlist');
                var $duibimask = $brandlist.parents('#master_container').parent().find('.duibi-leftmask');
                $brandlist.tag({
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
                                        defaults.selectCarIdFunc(curCarId, curCarName);
                                        
                                        $model.trigger('closeWindow');
                                        $duibimask.trigger('close');
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
                            var historyCookieCar = LocalStorageData.getData(defaults.duibiLocalName); //CookieHelper.getCookie(defaults.duibiCookieName);
                            if (historyCookieCar) {
                                duibiCarDataIds = historyCookieCar.split('|');
                            }
                            //初始化品牌
                            $body.trigger('brandinit', { init: function () { $("span.brand-logo>img").lazyload({ effect: "fadeIn", threshold: 50 }); }, actionName3: '[data-action=duibi-models]', leftmaskalert: '.duibi-alert', leftmaskback: '.leftmask3', carselect: function () { }, masterselect: function () { }, selectmark: function () { } });
                            //车款点击回调事件
                            api.model.clickEnd = function (paras) {
                                var curCarId = $(this).data("id"), curCarName = $(this).data("name");
                                if ($(this).text().indexOf('已添加') > -1 || $(this).text().indexOf('当前') > -1) {
                                    return;
                                }
                                defaults.selectCarIdFunc(curCarId, curCarName);
                                var $back = $('.' + $leftPopupModels.attr('data-back'));
                                //关闭浮层
                                $back.trigger('close');
                                $model.trigger('closeWindow');
                                $duibimask.trigger('close');
                            }
                            $body.css('overflow', 'initial');
                            _commonSlider($model, $body);
                        }
                        else if (idx == 2) //历史记录
                        {
                            initHistory();
                            $body.css('overflow', 'initial');
                            _commonSlider($model, $body);
                            $duibimask.trigger('close');
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
            $(".duibicar .btn-comparison").off("click").addClass("disable");
            $(".duibicar .btn-clear").off("click").addClass("disable");
        } else {
            $(".duibicar .btn-comparison").off("click").removeClass("disable").on("click", function (e) {
                e.preventDefault();
                self.submitCompare();
            });
            $(".duibicar .btn-clear").off("click").removeClass("disable").on("click", function (e) {
                e.preventDefault();
                self.clearCompareCarAll();
            });
        }
    };

    //层自适应
    var _commonSlider = function ($model, $body) {
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
        var container = $body.find("#container");
        if ($(container).length > 0) {
            $body.find("#container")[0].style.cssText = '';
        }
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
                CarCompareAd.initAd(compareData);
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
        var duibiCookieCar = LocalStorageData.getData(defaults.duibiLocalName); //CookieHelper.getCookie(defaults.duibiCookieName);
        if (duibiCookieCar) {
            duibiCarDataIds = duibiCookieCar.split('|');
        }
        else {
            duibiCarDataIds = [];
        }
        var historyCookieCar = LocalStorageData.getData(defaults.historyLocalName); //CookieHelper.getCookie(defaults.historyCookieName);
        if (historyCookieCar) {
            arrCarId = historyCookieCar.split('|');
        }
        if (arrCarId.length > 0) {
            //获取对比数据
            getData(url + arrCarId.join(','), function (data) {
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
                    defaults.selectCarIdFunc(curCarId, curCarName);
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

//对比广告
var CarCompareAd = (function(module){
    var self = module,
        compareData = [],//对比车款
        adData = null,//所有
        adCarObj = null,//广告车款对象
        carDataUrl = "/handlers/getcarinfoforcompare.ashx?carid={carids}",
        adCarDataUrl = "http://api.car.bitauto.com/CarInfo/GetCarList.ashx?saleState=1&csids={csids}&carids={carids}"; //测试地址

    module.initAd = function (carObjs) {
        //console.log("init ad");
        try {
            if ($(".duibi-leftPopup").is(":hidden")) {
                return;
            }
            if (typeof (carObjs) == 'undefined' || carObjs.length > 3 || carObjs.length == 0 || typeof (CarCompareAdJson) == 'undefined' || CarCompareAdJson.length == 0) {
                self.adCarObj = null;
                $("#compareAdBox").html("");
                return;
            }
            this.compareData = carObjs;
            self.getData();
        } catch (e) {
            console.log("error");
        }
    };
    module.getData = function () {
        self.adCarObj = null;
        if (self.adData == null) {
            var csIdArr = new Array(),
                carIdArr = new Array();
            CarCompareAdJson.forEach(function (obj) {
                if (typeof (obj.adCsId) != "undefined" && obj.adCsId > 0) {
                    csIdArr.push(obj.adCsId);
                }
                if (typeof (obj.adCarId) != "undefined" && obj.adCarId > 0) {
                    carIdArr.push(obj.adCarId);
                }
            });
            var tempAdCarUrl = adCarDataUrl.replace("{carids}", carIdArr.join(",")).replace("{csids}", csIdArr.join(","));
            $.ajax({
                url: tempAdCarUrl,
                cache: true,
                dataType: 'jsonp',
                jsonpCallback: "adCarCallback",
                success: function (data) {
                    self.adData = data;

                    self.getCompareCarData();
                }
            });
        }
        else {
            self.getCompareCarData();
        }
    };
    module.getCompareCarData = function () {
        var carIdArr = new Array();
        self.compareData.forEach(function (obj) {
            if ((typeof (obj.CsId) == "undefined" || obj.CsId == 0) || (typeof (obj.ReferPrice) == "undefined" || obj.ReferPrice == 0)) {
                carIdArr.push(obj.CarId);
            }
        });
        if (carIdArr.length > 0) {
            var tempCarCompareUrl = carDataUrl.replace("{carids}", carIdArr.join(","));

            var carCompreAjax = $.ajax({
                url: tempCarCompareUrl,
                cache: true,
                dataType: 'json',
                success: function (data) {
                    for (var j = 0; j < data.length; j++) {
                        for (var i = 0; i < self.compareData.length; i++) {
                            if (data[j].CarId == self.compareData[i].CarId) {
                                self.compareData[i].CsId = data[j].SerialId;
                                self.compareData[i].ReferPrice = data[j].CarReferPrice;
                                break;
                            }
                        }
                    }
                    self.getAdData(self.compareData,self.adData);
                }
            });
        }
        else {
            self.getAdData(self.compareData, self.adData);
        }
    };
   
    module.isCarCompare = function (carcompareData,carId) {//竞品车款是否在对比中
        for (var j = 0, carCount = carcompareData.length; j < carCount; j++) {
            if (carId == carcompareData[j].CarId) {
                return carcompareData[j];
            }
        }
        return null;
    };
    module.isCsCompare = function (carcompareData,csId) {//竞品车系是否在对比中
        for (var j = 0, carCount = carcompareData.length; j < carCount; j++) {
            if (csId == carcompareData[j].CsId) {
                return carcompareData[j];
            }
        }
        return null;
    };
    module.isAdCarCompare = function (carcompareData,adCarId) {//广告车款是否在对比中
        for (var j = 0, carCount = carcompareData.length; j < carCount; j++) {
            if (adCarId == carcompareData[j].CarId) {
                return true;
            }
        }
        return false;
    };
    module.sortByReferPriceAndPv = function (csCarList) {//按指导价和pv排序
        return csCarList.sort(function (a, b) {//排序
            var rp1 = parseFloat(a["referPrice"]),
                rp2 = parseFloat(b["referPrice"]),
                pv1 = parseInt(a["pv"]),
                pv2 = parseInt(b["pv"]);
            return ((rp1 < rp2) ? -1 : ((rp1 > rp2) ? 1 : ((pv1 < pv2) ? 1 : ((pv1 > pv2) ? -1 : 0))));
        });
    };
    module.getNearCarArray = function (csCarList, compareCarObj) {//返回和金品车款指导价最接近的车款数组
        var nearCarArray = new Array();
        for (var i = 0; i < csCarList.length; i++) {
            var obj = {};
            obj.carId = csCarList[i].carId;
            obj.diffPrice = Math.abs(parseFloat(csCarList[i].referPrice) - parseFloat(compareCarObj.ReferPrice));
            obj.pv = csCarList[i].pv;
            nearCarArray.push(obj);
        }
        return nearCarArray.sort(function (a, b) {
            var df1 = a.diffPrice,
                df2 = b.diffPrice,
                pv1 = a.pv,
                pv2 = b.pv;
            return df1 > df2 ? 1 : df1 < df2 ? -1 : pv1 > pv2 ? - 1 : pv1 < pv2 ? 1 : 0;
        });
    };
    module.sortByPv = function (csCarList) {//按pv排序
        if (csCarList == null || csCarList.length == 0) {
            return csCarList;
        }
        return csCarList.sort(function (a, b) {
            var pv1 = parseInt(a["pv"]),
                pv2 = parseInt(b["pv"]);
            return ((pv1 < pv2) ? 1 : ((pv1 > pv2) ? -1 : 0));
        });
    };
    module.getAdByReferPriceOrPv = function (compareCarObj,carcompareData, adCarData,adObj) {
        if (!isNaN(compareCarObj.ReferPrice) && parseFloat(compareCarObj.ReferPrice) > 0) {//有指导价，按指导价和pv排序
            var csCarList = self.sortByReferPriceAndPv(adCarData.serial[adObj.adCsId]);
            var nearPriceArray = self.getNearCarArray(csCarList, compareCarObj);
            for (var j = 0; j < nearPriceArray.length; j++) {
                if (!self.isAdCarCompare(carcompareData, nearPriceArray[j].carId)) {
                    var adCarId = nearPriceArray[j].carId;
                    for (var k = 0; k < adCarData.serial[adObj.adCsId].length; k++) {
                        var obj = adCarData.serial[adObj.adCsId][k];
                        if (obj.carId == adCarId) {
                            self.getAdObj(adCarId, obj.csName + " " + obj.carName);
                            break;
                        }
                    }
                    if (self.adCarObj != null) {
                        break;
                    }
                }
            }
        }
        else {//没有指导价
            var csCarList = self.sortByPv(adCarData.serial[adObj.adCsId]);
            for (var j = 0; j < csCarList.length; j++) {
                if (!self.isAdCarCompare(carcompareData, csCarList[j].carId)) {
                    var obj = csCarList[j];
                    self.getAdObj(obj.carId, obj.csName + " " + obj.carName);
                    break;
                }
            }
        }
    };
    module.getAdData = function (carcompareData, adCarData) {
        if (carcompareData.length == 0 || ($.isEmptyObject(adCarData.car) && $.isEmptyObject(adCarData.serial))) {
            return;
        }
        for (var i = 0, adCount = CarCompareAdJson.length; i < adCount; i++) {
            var adObj = CarCompareAdJson[i];
            if ((typeof (adObj.csId) == "undefined" || adObj.csId == 0) && adObj.carId >0) {//按车款投
                if ((typeof (adObj.adCsId) == 'undefined' || adObj.adCsId == 0) && adObj.adCarId >= 0) {//投放车款
                    var isShowAd = self.isCarCompare(carcompareData, adObj.carId);
                    if (isShowAd) {
                        if (!self.isAdCarCompare(carcompareData, adObj.adCarId)) {
                            self.getAdObj(adObj.adCarId, adCarData.car[adObj.adCarId].SerialName + " " + adCarData.car[adObj.adCarId].CarName);
                        }
                    }
                }
                else if (adObj.adCsId > 0)
                {
                    var compareCarObj = self.isCarCompare(carcompareData, adObj.carId);
                    if (compareCarObj != null && compareCarObj.CarId > 0) {
                        if (typeof (adObj.adCarId) != "undefined" && adObj.adCarId > 0) {//默认车款
                            if (!self.isAdCarCompare(carcompareData, adObj.adCarId)) {
                                self.getAdObj(adObj.adCarId, adCarData.car[adObj.adCarId].SerialName + " " + adCarData.car[adObj.adCarId].CarName);
                            }
                        }
                        if (self.adCarObj == null) {
                            self.getAdByReferPriceOrPv(compareCarObj, carcompareData, adCarData, adObj);
                        }
                    }
                    
                }
            }
            else if (typeof (adObj.csId) != "undefined" && adObj.csId > 0) {//按车系投
                if ((typeof (adObj.adCsId) == "undefined" || adObj.adCsId == 0) && adObj.adCarId > 0) { //投车款
                    var compareCarObj = self.isCsCompare(carcompareData, adObj.csId);
                    if (compareCarObj != null && compareCarObj.CarId > 0) {
                        if (!self.isAdCarCompare(carcompareData, adObj.adCarId)) {
                            self.getAdObj(adObj.adCarId, adCarData.car[adObj.adCarId].SerialName + " " + adCarData.car[adObj.adCarId].CarName);
                        }
                    }
                }
                else if (typeof(adObj.adCsId) != "undefined" && adObj.adCsId > 0) {
                    var compareCarObj = self.isCsCompare(carcompareData, adObj.csId);
                    if (compareCarObj != null && compareCarObj.CarId > 0) {//有广告
                        if (typeof (adObj.adCarId) != "undefined" && adObj.adCarId > 0) {//默认车款
                            if (!self.isAdCarCompare(carcompareData, adObj.adCarId)) {
                                self.getAdObj(adObj.adCarId, adCarData.car[adObj.adCarId].SerialName + " " + adCarData.car[adObj.adCarId].CarName);
                            }
                        }
                        if (self.adCarObj == null)
                        {
                            self.getAdByReferPriceOrPv(compareCarObj, carcompareData, adCarData, adObj);
                        }
                    }
                }
            }

            if (self.adCarObj != null) {
                var h = new Array();
                h.push("<div class=\"ad-title\"><h6>推荐对比</h6><span class=\"rb\">广告</span></div>");
                h.push("<div class=\"line-box\"><a href=\"#compare\" id=\"car-compare_" + self.adCarObj.carId + "\">" + self.adCarObj.carName + "</a></div>");
                $("#compareAdBox").html(h.join(""));
                $("#car-compare_" + self.adCarObj.carId).click(function () {
                    WaitCompare.addCompareCar(self.adCarObj.carId, self.adCarObj.carName);
                });
                break;
            }
        }
    };
    module.getAdObj = function(carId,carName){
        self.adCarObj = {};
        self.adCarObj.carId = carId;
        self.adCarObj.carName = carName;
    }
    return module;
})(CarCompareAd || {});

var LocalStorageData = function (module) {
    var self = module,
        defaults = {
            domain: window.location.host
        };
    module.getData = function (name, options) {
        Object.extend(defaults, options);
        if (defaults.domain)
            document.domain = defaults.domain;
        var compareCar = localStorage[name];
        if (compareCar != null) {
            return unescape(compareCar);
        }
        return null;
    };
    module.setData = function (name,value,options) {
        Object.extend(defaults, options);
        if (typeof value != 'undefined') {
            options = options || {};
            if (value === null) {
                value = '';
            }
            if (defaults.domain)
                document.domain = defaults.domain;
            localStorage[name] = value;
        }
    };
    module.clearData = function (name, options) {
        Object.extend(defaults, options);
        if (defaults.domain) {
            document.domain = defaults.domain;
        }
        localStorage.removeItem(name);
    };
    return module;
}(LocalStorageData || {});

var CookieHelper = (function (module) {
    var self = module,
        defaults = {
            domain: window.location.host,
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
