var arrMarkSerial = [], saleYearCount = 0,
    compareData = [],  //用于加号层显示车款全称+车款id
    duibiCarDataIds = [],//已添加对比车款id集合
    currentDuibiCarId;  //当前要换的车款

Array.prototype.indexOf = function (value) {
    for (var i = 0, l = this.length; i < l; i++)
        if (this[i] == value) return i;
    return -1;
}
Array.prototype.remove = function (b) {
    var a = this.indexOf(b);
    if (a >= 0) {
        this.splice(a, 1);
        return this;
    }
    return this;
};

var changecar = {
    serialId: 0,
    callback: null,
    currentADataIndex : 0,
    carDataUrl: "http://api.car.bitauto.com/CarInfo/GetCarList.ashx?saleState=1&carids={carids}",
    LocalKey: "CarH5CompareHistory",
    init: function (parms) {
        var self = this;
        self.serialId = parms["serialId"];
        self.callback = parms["callback"];
        if (parms["carId"] > 0) {
            self.currentADataIndex = 1;
            self.SelectCarCallback(parms["carId"]);
        }
        self.bindevent();
    },
    //层自适应    commonSlider: function ($model, $body) {
        if ($model.height() > $(document.body).height()) {
            $(document.body).height($model.height());
        } else if ($model.height() < $(document.body).height()) {
            $('#container', $body).css({ 'overflow': 'hidden' }, { width: '100%' }).height(document.documentElement.clientHeight - $('.b-return', $model).height());
            $('.brandlist').height(document.documentElement.clientHeight - $('.b-return', $model).height());
            $(document.body).height(document.documentElement.clientHeight);
        }
    },
    bindevent: function () {
        var self = this;
        var $body = $('body');
        $("[data-action=changecar]").rightSwipeAnimation({
            fnEnd: function ($current) {
                if ($($current).attr("carid")) {
                    currentDuibiCarId = $($current).attr("carid");
                } else {
                    currentDuibiCarId = 0;
                }
                if ($($current).attr("csid")) {
                    self.serialId = $($current).attr("csid");
                }
                var $model = this;
                self.currentADataIndex = $($current).attr("data-index");
                $body.animate({ scrollTop: 0 }, 30);
                var tags = $('.tag', $body);
                //切换标签
                $('.brandlist').tag({
                    tagName: '.first-tags',
                    fnEnd: function (idx) {
                        tags.hide();
                        if (idx == 0 && (changecar.serialId == 0 || changecar.serialId == undefined)) {
                            idx = 1;
                            $(".brandlist .first-tags ul li").eq(idx).addClass("current").prev().hide();
                            $(".brandlist .curSerialSub").hide();
                        } else {
                            var $brandtag = $(".brandlist .first-tags ul li");
                            $brandtag.removeClass('current');
                            $brandtag.eq(idx).addClass("current").show();
                        }
                        if ($(".market-p ul li").length == 0 &&(localStorage.getItem(self.LocalKey) == null ||  localStorage.getItem(self.LocalKey) == "")) {
                            $(".brandlist .first-tags ul li").eq(2).hide();
                        }
                        $(".brandlist div.tag").eq(idx).show();
                        

                        if (idx == 0) {     //当前品牌
                            $('.curSerialSub', $body).swipeApi({
                                url: "http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&datatype=0&pid=" + changecar.serialId,
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
                                    //self.bindSelectedEvent();
                                    var curSerialName = $("#curSerialName").val();
                                    $('.curSerialName').html(curSerialName);

                                    $(this).find(".pic-txt-h li:not(li[class]) a").on("click", function () {
                                        var curCarId = $(this).data("id");
                                        self.callback && self.callback.call(this, curCarId);
                                        $model.trigger('closeWindow');
                                        $body.removeClass('overflow');
                                        $("#container").attr("style", "");
                                        $("body").attr("style", "");
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
                                            self.removeborder.call($model);
                                        }
                                    });

                                    $body.css('overflow', 'initial');
                                    self.commonSlider($model, $body);
                                }
                            });
                        }
                        else if (idx == 1)  //按品牌查找 
                        {
                            //初始化品牌
                            $body.trigger('brandinit', { actionName3: '[data-action=duibi-models]', leftmaskalert: '.duibi-alert', leftmaskback: '.leftmask3', carselect: function () { }, masterselect: function () { }, selectmark: function () { } });
                            //车款点击回调事件
                            api.model.clickEnd = function (paras) {
                                var curCarId = $(this).data("id");
                                if ($(this).text().indexOf('已添加') > -1 || $(this).text().indexOf('当前') > -1) {
                                    return;
                                }
                                
                                self.callback && self.callback.call(this, curCarId);
                                var $back = $('.' + $leftPopupModels.attr('data-back'));
                                //关闭浮层
                                $back.trigger('close');
                                $model.trigger('closeWindow');
                                $("#container").attr("style", "");
                                $("body").attr("style", "");
                            }
                            $body.css('overflow', 'initial');
                            self.commonSlider($model, $body);
                        }
                        else if (idx == 2) //历史记录
                        {
                            $(".history").html("");
                            //self.InitTuiJian($model);
                            self.InitHistory($model);
                            $body.css('overflow', 'initial');
                            self.commonSlider($model, $body);
                        }

                        $model.find('.btn-return').click(function (ev) {
                            ev.preventDefault();
                            $model.trigger('closeWindow');
                            $("#container").attr("style", "");
                            $("body").attr("style", "");
                        })
                        $model.show();
                        var tagCount = $(".first-tags li:not(:hidden)").length;
                        $(".brandlist div.first-tags")[0].className = "first-tags tags-" + tagCount;
                    }
                });
            }
        });
    },
    ChangeCarCallback: function (curCarId) {
        var self = this;
        var carids = "";
        if (changecar.currentADataIndex == 1) {
            carids = curCarId + "," + duibiCarDataIds[1];
        }
        else {
            carids = duibiCarDataIds[0] + "," + curCarId;
        }
        window.location.href = "/chexingduibi/?carids=" + carids;
    },
    SelectCarCallback: function (curCarId) {
        var self = changecar;
        var reUrl = self.carDataUrl.replace("{carids}", curCarId);
        var $current = $("a[data-index='" + self.currentADataIndex + "']");
        $.ajax({
            url: reUrl,
            dataType: "jsonp",
            jsonpCallback: "cardata",
            cache: true,
            success: function (result) {
                if (result && result.car && result.car[curCarId]) {
                    var obj = result.car[curCarId];
                    var html = new Array();
                    var carname = obj.SerialName + " " + obj.CarName;
                    if (carname.length > 20) {
                        carname = carname.substring(0, 20) + "...";
                    }
                    var price = parseFloat(obj.CarPrice);
                    if (price > 100) {
                        price = Math.round(price);
                    }
                    html.push("<a href=\"javascript:;\" data-action=\"changecar\" data-index=\"" + self.currentADataIndex + "\" carid=\"" + curCarId + "\" csid=\"" + obj.SerialId + "\" class=\"after\">");
                    html.push("<img src=\"" + obj.CsWhiteImg + "\" />");
                    html.push("<div class=\"right\">");
                    html.push("<h6>" + carname + "</h6>");
                    html.push("<em>厂商指导价：" + (price == "" ? "暂无" : (price + "万")) + "</em>");
                    html.push("</div>");
                    html.push("<i class=\"refresh\" data-action=\"changecar\" data-index=\"" + self.currentADataIndex + "\" carid=\"" + curCarId + "\" csid=\"" + obj.SerialId + "\"></i>");
                    html.push("</a>");
                    $($current).hide().after(html.join("")).remove();
                    duibiCarDataIds.remove(currentDuibiCarId).push(curCarId);
                    if (duibiCarDataIds.length == 2) {
                        if (par.length > 0) {
                            $("#buttoncompare").attr("href", "/chexingduibi/?carids=" + duibiCarDataIds.join(",")+"&"+par).removeClass("gray");
                        } else {
                            $("#buttoncompare").attr("href", "/chexingduibi/?carids=" + duibiCarDataIds.join(",")).removeClass("gray");
                        }
                        
                    }
                    changecar.serialId = obj.SerialId;
                    self.bindevent();
                }
            }
        });
    },
    InitTuiJian: function ($model) {
        var self = this;
        var htmlArray = new Array();
        htmlArray.push("<div class=\"tt-small min-small\"><span>推荐车型</span></div>");
        
        htmlArray.push("<div class=\"market-p market-p-block\">");
        htmlArray.push("<ul>");
        var isCurrent = currentDuibiCarId == 114062;
        var isCompare = duibiCarDataIds.indexOf(114062) > -1;
        htmlArray.push("<li" + ((isCompare || isCurrent) ? " class=\"none\"" : "") + ">");
        htmlArray.push("<a href=\"javascript:;\" id=\"114062\" csid=\"2420\">");
        htmlArray.push("<div class=\"img-box\">");
        htmlArray.push("<img src=\"http://img1.bitautoimg.com/autoalbum/files/20120306/635/0502316356_6.jpg\">");
        htmlArray.push("</div>");
        htmlArray.push("<dl>");
        htmlArray.push("<dt>" + (isCurrent ? "[当前]" : isCompare ? "[已添加]" : "") + "比亚迪 2015款 1.0L 手动 悦酷型</dt>");
        htmlArray.push("<dd><em>厂商指导价：</em><strong>4.19万</strong></dd>");
        htmlArray.push("</dl>");
        htmlArray.push("</a>");
        htmlArray.push("</li>");
        htmlArray.push("</ul>");
        htmlArray.push("</div>");
        $(".history").prepend(htmlArray.join(""));
        $(".market-p").find("li:not(class) a").on("click", function (ev) {
            ev.preventDefault();
            var curCarId = $(this).attr("id");
            self.callback && self.callback.call(this, curCarId);
            changecar.serialId = $(this).attr("csid");
            $model.trigger('closeWindow');
            $("#container").attr("style", "");
            $("body").attr("style", "");
        });
    },
    InitHistory: function ($model) {
        var self = this;
        var historyCar = localStorage.getItem(self.LocalKey);
        if (historyCar == null) {
            return;
        }
        var historyCarArr = historyCar.split(",");
        var self = changecar;
        var reUrl = self.carDataUrl.replace("{carids}", historyCar);
        $.ajax({
            url: reUrl,
            dataType: "jsonp",
            jsonpCallback: "cardata",
            cache: true,
            success: function (result) {
                if (!result || !result.car || result.car.length == 0) return;
                var htmlArray = new Array();
                htmlArray.push("<div class=\"tt-small min-small\"><span>历史记录</span></div><div class=\"pic-txt-h pic-txt-9060 y2015\"><ul class=\"no\">");
                for (var i = 0; i < historyCarArr.length; i++) {
                    if (result.car[historyCarArr[i]]) {
                        var obj = result.car[historyCarArr[i]];
                        var isCurrent = currentDuibiCarId == historyCarArr[i];
                        var isCompare = duibiCarDataIds.indexOf(historyCarArr[i]) > -1;
                        htmlArray.push("<li " + ((isCompare || isCurrent) ? "class='none right-box'" : "class='right-box'") + ">");
                        htmlArray.push("<a href=\"javascript:;\" id=\"" + historyCarArr[i] + "\" csid=\"" + obj.SerialId + "\">");
                        htmlArray.push("<h4>" + (isCurrent ? "[当前]": isCompare ? "[已添加]" : "") + obj.SerialName + " " + obj.CarName + "</h4>");
                        htmlArray.push("<p>厂商指导价：<strong>" + (obj.CarPrice == "" ? "暂无" : (obj.CarPrice + "万")) + "</strong></p>");
                        htmlArray.push("</a>");
                        htmlArray.push("<a href=\"javascript:;\" carid=\"" + historyCarArr[i] + "\" class=\"remove\"></a>");
                    }
                }
                htmlArray.push("</ul></div>");
                $(".history").append(htmlArray.join(""));
                $(".history").find("li[class='right-box'] a[class!='remove']").on("click", function (ev) {
                    ev.preventDefault();
                    var curCarId = $(this).attr("id");
                    self.callback && self.callback.call(this, curCarId);
                    changecar.serialId = $(this).attr("csid");
                    $model.trigger('closeWindow');
                    $("#container").attr("style", "");
                    $("body").attr("style", "");
                    //$(".wrap20").show();
                });
                $(".history").find("a[class='remove']").on("click", function (ev) {
                    ev.preventDefault();
                    var curCarId = $(this).attr("carid");
                    var historyCar = localStorage.getItem(self.LocalKey);
                    if (historyCar == null) {
                        return;
                    }
                    var historyCarArr = historyCar.split(",");
                    historyCarArr.remove(curCarId);
                    localStorage.setItem(self.LocalKey, historyCarArr.join(","));
                    $(this).parent().remove();
                });
            }
        });
    },
    //折叠下划线控制
    removeborder : function() {
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
}

function a(data) {
    return;
}