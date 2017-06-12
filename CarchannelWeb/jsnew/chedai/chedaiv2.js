var carloan = function (options) {
    this.options = { loading: false, year: 0, referprice: 0, csid: 0, carid: 0, payrate: .3, loanmonths: 36, page: 1, pagelength: 20, stateid: 2, cityid: 201, provdropdownlist: '#prov-list', citydropdownlist: '#city-list', orderfield: "MonthlyPayment", agentid: 0 };
    this.options.api_get_package_list = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetPackages/?downPaymentRate={4}&repaymentPeriod={5}&carModelId={2}&cityId={6}&orderBy={7}&pageNum={8}";
    this.options.api_get_unique_package = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetUniquePackagesByCompanyAndAppointFirst/?downPaymentRate={4}&repaymentPeriod={5}&carModelId={2}&cityId={6}&orderBy={7}&pageNum={8}&packageId={3}";
    this.options.api_get_package = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetPackageWithApplicationCount/?packageId={3}&downPaymentRate={4}&carModelId={2}&cityId={6}";
    this.options.api_submit_apply = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/SubmitApplication/?callback=?";
    this.options.api_get_province_list = 'http://mai.bitauto.com/InsuranceAndLoan/Region/GetProvinces';
    this.options.api_get_city_list = 'http://mai.bitauto.com/InsuranceAndLoan/Region/GetCities?provinceid={0}';
    this.options.api_check_mobile = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/UserHasApplied/?callback=?";
    this.options.api_get_apply_count = 'http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetApplicationCountForSerialBrand?serialBrandId={0}&callback=?';
    this.options.apply_success_url = false;
    //this.options.applyposturl = "/{1}/chedai/m{2}-{3}-{4}-1/?cityid={6}&months={5}";
    this.options.applyposturl = "/{1}/chedai/sq/m{2}/?pakid={3}&payrate={4}&cityid={6}&months={5}";
    //this.options.loandetailurl = "/{1}/chedai/m{2}-{3}-{4}/?cityid={6}&months={5}";
    this.options.loandetailurl = "/{1}/chedai/xq/m{2}/?pakid={3}&payrate={4}&cityid={6}&months={5}";

    this.options.single_select = false; //是否为单选

    this.options.carlist_init_callback = false; //获取车型列表回调
    this.options.provinces_init_callback = false; //获取省份列表回调
    this.options.cities_init_callback = false; //获取城市列表回调
    this.options.before_load_callback = false;
    this.options.after_load_callback = false;
    this.options.get_package_callback = false;
    this.options.on_city_change = false;

    //直辖市
    this.options.municipals = { s2: { name: '北京', cityid: 201 }, s26: { name: '天津', cityid: 2601 }, s31: { name: '重庆', cityid: 3101 }, s24: { name: '上海', cityid: 2401} };

    this.options = $.extend(this.options, options);
    this.data = {};
    this.submiting = false;
};

//套餐特点
carloan.features = {
    f101: { "class": "fc", title: "本地房产" }, f102: { "class": "fk", title: "2日放款" }, f103: { "class": "sxbj", title: "手续便捷" },
    f1: { "class": "dll", title: "低利率" }, f2: { "class": "cdll", title: "超低利率" }, f3: { "class": "wll", title: "零利率" },
    f4: { "class": "lcsf", title: "两成首付" }, f5: { "class": "zcwn", title: "最长五年" }, f6: { "class": "txwk", title: "弹性尾款" },
    f7: { "class": "wyg", title: "无月供" }, f8: { "class": "dyg", title: "低月供" }, f9: { "class": "jssp", title: "机速审批" },
    f10: { "class": "zljb", title: "资料简便" }, f11: { "class": "mdy", title: "免抵押" }, f12: { "class": "mjf", title: "免家访" },
    f13: { "class": "lpzs", title: "礼品赠送" }, f14: { "class": "tshd", title: "特殊活动" }, f15: { "class": "xscx", title: "限时促销" },
    f16: { "class": "cxzx", title: "车型专享" }, f17: { "class": "zzfw", title: "增值服务" }
};

carloan.errors = {
    error1: { code: 1, message: '未选择套餐' },
    error2: { code: 2, message: '您已申请过该车型，请查看其他车型' },
    error4: { code: 4, message: '保存失败' }
};

//帮助类
carloan.common = {
    format: function (str) {
        var args = arguments;
        var arr = new Array();
        for (var i = 1; i < args.length; i++) {
            arr.push(args[i]);
        }
        return str.replace(/\{(\d+)\}/g, function (m, i) {
            return arr[i];
        });
    },
    isnumber: function () {
        return !isNaN(this);
    },
    formatMonths: function (months) {
        var str = "";
        var years = Math.floor(months / 12);
        if (years > 0) str = years + "年";
        var last = months % 12;
        switch (last) {
            case 6: str += '半'; break;
            case 0: break;
            default: str += last + "个月"; break;
        }
        return str;
    }
};

window.json_callback_for_prov = function (data) { };
window.json_callback_for_city = function (data) { };
window.json_callback_for_load = function (data) { };
window.json_callback_for_getpak = function (data) { };

//加载城市
carloan.prototype.getcities = function (paras) {
    var _this = this;
    var opts = _this.options;
    $.ajax({ url: carloan.common.format(opts.api_get_city_list, opts.stateid), cache: true, dataType: "jsonp", jsonpCallback: "json_callback_for_city",
        success: function (data) {
            var $city = $(opts.citydropdownlist);
            if ($city.length) {
                var html = [];
                html.push('<option value="0">请选择</option>');
                for (var i = 0; i < data.length; i++) {
                    var o = data[i];
                    html.push(carloan.common.format('<option value="{0}">{1}</option>', o.ID, o.Name));
                }
                $city.html(html.join(''));
                $city.change(function () {
                    opts.cityid = $city.val();
                    opts.on_city_change(_this, opts.cityid, this);
                    if (opts.pagetype == 'apply') {
                        _this.loadunique();
                    } else {
                        _this.load();
                    }
                });
                if (opts.cityid > 0) {
                    $city.val(opts.cityid);
                }
            }
            opts.cities_init_callback && _this.options.cities_init_callback(_this, data, paras); //回调
        },
        error: function (jqXHR, textStatus, errorThrown) { },
        complete: function (jqXHR, textStatus) { }
    });
};

//初始化
carloan.prototype.init = function () {
    var _this = this;
    var opts = _this.options;
    //获取IP定向
    if (typeof bit_locationInfo != "undefined" && bit_locationInfo.cityId) {
        opts.cityid = bit_locationInfo.cityId;
        opts.stateid = Math.floor(opts.cityid / 100)
    }
    //加载车型
    typeof (BitA) != 'undefined' && BitA.DropDownList && typeof BitA.DropDownList({
        container: { cartype: "cars" },
        include: { cartype: "1" },
        parent: { cartype: opts.csid },
        //cartype值为其类别的父ID
        dvalue: { cartype: opts.carid },
        callback: {
            cartype: function (data) {
                _this.data.carlist = data;
                _this.options.carlist_init_callback && _this.options.carlist_init_callback(_this, data); //回调
                //需要车型数据再加载
                if (opts.pagetype == 'apply') {
                    _this.loadunique();
                } else {
                    _this.load();
                }
            }
        }
    });

    //加载省份
    $.ajax({ url: opts.api_get_province_list, cache: true, dataType: "jsonp", jsonpCallback: "json_callback_for_prov",
        success: function (data) {
            var $prov = $(opts.provdropdownlist);
            var html = [];
            if ($prov.length) {
                html.push('<option value="0">请选择</option>');
                for (var i = 0; i < data.length; i++) {
                    var o = data[i];
                    html.push(carloan.common.format('<option value="{0}">{1}</option>', o.ID, o.Name));
                }
                $prov.html(html.join(''));
                if (opts.stateid > 0) {
                    $prov.val(opts.stateid);
                }
                $prov.change(function () {
                    opts.stateid = $prov.val();
                    _this.getcities();
                });
                $prov.change();
            }
            opts.provinces_init_callback && _this.options.provinces_init_callback(_this, data); //回调
        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function (jqXHR, textStatus) {
        }
    });
};

//输出debug数据
carloan.log = function (str) {
    typeof console != 'undefined' && console.log && console.log(str);
};

//加载套餐列表
carloan.prototype.load = function (paras) {
    var _this = this;
    var opts = _this.options;
    this.loadinternal($.extend(paras, { dataapi: opts.api_get_package_list }));
};

//加载排重套餐
carloan.prototype.loadunique = function (paras) {
    var _this = this;
    var opts = _this.options;
    this.loadinternal($.extend(paras, { dataapi: opts.api_get_unique_package }));
};

//加载套餐
carloan.prototype.loadinternal = function (paras) {
    var _this = this;
    var opts = _this.options;

    var url = opts.api_get_unique_package;
    if (paras) {
        opts.page = paras.page && paras.page ? paras.page : 1;
        url = paras.dataapi && paras.dataapi.length ? paras.dataapi : opts.api_get_unique_package;
    }

    //加载前
    opts.before_load_callback && opts.before_load_callback(_this);

    var dataUrl = carloan.common.format(url, opts.csid, opts.csspell, opts.carid, opts.agentid, opts.payrate, opts.loanmonths, opts.cityid, opts.orderfield, opts.page);

    if (!(opts.carid > 0) || !(opts.loanmonths > 0) || !(opts.payrate > 0) || !(opts.cityid > 0)) {
        opts.after_load_callback && opts.after_load_callback(_this, null);
        return;
    }

    carloan.log(dataUrl);

    $.ajax({ url: dataUrl,
        dataType: 'jsonp',
        cache: true,
        jsonpCallback: "json_callback_for_load",
        success: function (data) {
            opts.after_load_callback && opts.after_load_callback(_this, data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            opts.load_failed_callback && opts.load_failed_callback(_this, jqXHR, textStatus, errorThrown);
        },
        complete: function (jqXHR, textStatus) {
            opts.load_complete_callback && opts.load_complete_callback(_this, jqXHR, textStatus);
        }
    });
};

carloan.prototype.getpackage = function () {
    var _this = this;
    var opts = _this.options;
    //获取月供及利率
    var urlStr = carloan.common.format(opts.api_get_package, opts.csid, opts.csspell, opts.carid, opts.agentid, opts.payrate, opts.loanmonths, opts.cityid);
    carloan.log(urlStr);
    $.ajax({
        type: "get",
        cache: true,
        url: urlStr,
        dataType: "jsonp",
        jsonpCallback: "json_callback_for_getpak",
        success: function (data) {
            opts.get_package_callback && opts.get_package_callback(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function (jqXHR, textStatus) {
        }
    });
};

carloan.prototype.checkmobile = function (queryData, callback) {
    var _this = this;
    //验证手机号
    $.ajax({
        url: _this.options.api_check_mobile,
        data: queryData,
        dataType: "jsonp",
        success: function (data) {
            callback(_this, data);
        },
        error: function () {
            alert("暂时无法提交申请，请稍候再试！");
        },
        complete: function (jqXHR, textStatus) {
        }
    });
};

//保存订单
carloan.prototype.submitapply = function (data, ajaxOpts) {
    var _this = this;
    var opts = _this.options;
    if (_this.submiting == true) {
        alert('请等待上一次提交申请结束');
    }
    _this.submiting = true;
    $('body').css({ 'cursor': 'wait' });
    var submitData = $.extend({ CityId: opts.cityid, CarModelId: opts.carid, Age: 0, Ref: opts.ref ? opts.ref : '' }, data || {});
    $.ajax({
        url: opts.api_submit_apply,
        data: submitData,
        dataType: "jsonp",
        success: function (data, textStatus, jqXHR) {
            ajaxOpts && ajaxOpts.success && ajaxOpts.success(data, textStatus, jqXHR);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ajaxOpts && ajaxOpts.error && ajaxOpts.error(jqXHR, textStatus, errorThrown);
            _this.submiting = false;
        },
        complete: function (jqXHR, textStatus) {
            ajaxOpts && ajaxOpts.complete && ajaxOpts.complete(jqXHR, textStatus);
            $('body').css({ 'cursor': 'default' });
            _this.submiting = false;
        }
    });
};

carloan.prototype.getapplycount = function (callback) {
    var _this = this;
    $.ajax({
        url: carloan.common.format(_this.options.api_get_apply_count, _this.options.csid),
        dataType: "jsonp",
        success: function (data, textStatus, jqXHR) {
            callback && callback(data);
        },
        error: function (jqXHR, textStatus, errorThrown) { },
        complete: function (jqXHR, textStatus) { }
    });
};

jQuery.fn.pagination = function (maxentries, opts) {
    opts = jQuery.extend({
        items_per_page: 10,
        num_display_entries: 10,
        current_page: 0,
        num_edge_entries: 0,
        link_to: "#",
        prev_text: "Prev",
        next_text: "Next",
        ellipse_text: "...",
        prev_show_always: true,
        next_show_always: true,
        prev_enabled_css: false,
        prev_disabled_css: false,
        next_enabled_css: false,
        next_disabled_css: false,
        enabled_css: false,
        callback: function () {
            return false;
        },
        callback_after_init: true
    }, opts || {});
    return this.each(function () {
        /**
        * 计算最大分页显示数目
        */
        function numPages() {
            return Math.ceil(maxentries / opts.items_per_page);
        }
        /**
        * 极端分页的起始和结束点，这取决于current_page 和 num_display_entries.
        * @返回 {数组(Array)}
        */
        function getInterval() {
            var ne_half = Math.ceil(opts.num_display_entries / 2);
            var np = numPages();
            var upper_limit = np - opts.num_display_entries;
            var start = current_page > ne_half ? Math.max(Math.min(current_page - ne_half, upper_limit), 0) : 0;
            var end = current_page > ne_half ? Math.min(current_page + ne_half, np) : Math.min(opts.num_display_entries, np);
            return [start, end];
        }
        /**
        * 分页链接事件处理函数
        * @参数 {int} page_id 为新页码
        */
        function pageSelected(page_id, evt) {
            current_page = page_id;
            drawLinks();
            var continuePropagation = opts.callback(page_id, panel);
            if (!continuePropagation) {
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                } else {
                    evt.cancelBubble = true;
                }
            }
            return continuePropagation;
        }
        /**
        * 此函数将分页链接插入到容器元素中
        */
        function drawLinks() {
            panel.empty();
            var interval = getInterval();
            var np = numPages();
            // 这个辅助函数返回一个处理函数调用有着正确page_id的pageSelected。
            var getClickHandler = function (page_id) {
                return function (evt) {
                    return pageSelected(page_id, evt);
                };
            };
            //辅助函数用来产生一个单链接(如果不是当前页则产生span标签)
            var appendItem = function (page_id, appendopts) {
                page_id = page_id < 0 ? 0 : page_id < np ? page_id : np - 1;
                // 规范page id值
                appendopts = jQuery.extend({
                    text: page_id + 1,
                    classes: ""
                }, appendopts || {});
                if (page_id == current_page) {
                    var lnk = jQuery("<span class='current'>" + appendopts.text + "</span>");
                    appendopts.prev_disabled_css && lnk.addClass(appendopts.prev_disabled_css);
                } else {
                    var lnk = jQuery("<a>" + appendopts.text + "</a>").bind("click", getClickHandler(page_id)).attr("href", opts.link_to.replace(/__id__/, page_id));
                }
                if (appendopts.classes) {
                    lnk.addClass(appendopts.classes);
                }
                panel.append(lnk);
            };
            // 产生"Previous"-链接
            if (opts.prev_text && (current_page > 0 || opts.prev_show_always)) {
                appendItem(current_page - 1, {
                    text: opts.prev_text,
                    classes: "prev" + opts.prev_disabled_css ? " " + opts.prev_disabled_css : ""
                });
            }
            // 产生起始点
            if (interval[0] > 0 && opts.num_edge_entries > 0) {
                var end = Math.min(opts.num_edge_entries, interval[0]);
                for (var i = 0; i < end; i++) {
                    appendItem(i);
                }
                if (opts.num_edge_entries < interval[0] && opts.ellipse_text) {
                    jQuery("<span>" + opts.ellipse_text + "</span>").appendTo(panel);
                }
            }
            // 产生内部的些链接
            for (var i = interval[0]; i < interval[1]; i++) {
                appendItem(i);
            }
            // 产生结束点
            if (interval[1] < np && opts.num_edge_entries > 0) {
                if (np - opts.num_edge_entries > interval[1] && opts.ellipse_text) {
                    jQuery("<span>" + opts.ellipse_text + "</span>").appendTo(panel);
                }
                var begin = Math.max(np - opts.num_edge_entries, interval[1]);
                for (var i = begin; i < np; i++) {
                    appendItem(i);
                }
            }
            // 产生 "Next"-链接
            if (opts.next_text && (current_page < np - 1 || opts.next_show_always)) {
                appendItem(current_page + 1, {
                    text: opts.next_text,
                    classes: "next" + opts.next_disabled_css ? " " + opts.next_disabled_css : ""
                });
            }
        }
        //从选项中提取current_page
        var current_page = opts.current_page;
        //创建一个显示条数和每页显示条数值
        maxentries = !maxentries || maxentries < 0 ? 1 : maxentries;
        opts.items_per_page = !opts.items_per_page || opts.items_per_page < 0 ? 1 : opts.items_per_page;
        //存储DOM元素，以方便从所有的内部结构中获取
        var panel = jQuery(this);
        // 获得附加功能的元素
        this.selectPage = function (page_id) {
            pageSelected(page_id);
        };
        this.prevPage = function () {
            if (current_page > 0) {
                pageSelected(current_page - 1);
                return true;
            } else {
                return false;
            }
        };
        this.nextPage = function () {
            if (current_page < numPages() - 1) {
                pageSelected(current_page + 1);
                return true;
            } else {
                return false;
            }
        };
        // 所有初始化完成，绘制链接
        drawLinks();
        // 回调函数
        opts.callback_after_init && opts.callback(current_page, this);
    });
};


//View
$(function () {
    var curCarsTab = false, curCar = false, curRate = false, curMonths = false, curSortTab = false, curProv = false, curCity = false, cityinit = false;
    var city_list = $("#city_list"), city_layer = $(".city_layer");
    var options = typeof chedai_config != 'undefined' ? chedai_config : {};
    //省份回调
    options.provinces_init_callback = function (sender, data) {
        var opts = sender.options;
        var povs = $("#div_provinces").empty();
        for (var i = 0; i < data.length; i++) {
            var opt = $("<p></p>").append($('<a href="#"></a>').attr("pid", data[i].ID).attr("title", data[i].Name).text(data[i].Name.substring(0, 5))).appendTo(povs);
            if (opts.stateid == data[i].ID) {
                if (curProv) curProv.removeClass("act");
                curProv = opt.find('a').addClass('act');
                var cities = $("#div_cities").empty().append($("<h6></h6>").text(curProv.attr("title")));
                sender.getcities();
            }
        }
        povs.find("a").click(function (event) {
            event.preventDefault();
            if (curProv) curProv.removeClass("act");
            curProv = $(this).addClass("act");
            var cities = $("#div_cities").empty().append($("<h6></h6>").text(curProv.attr("title")));
            sender.options.stateid = $(this).attr("pid");
            sender.getcities(sender.options.stateid);
        });

        //设置下拉按钮打开和关闭城市层
        city_list.find('.xiala').click(function () {
            var b = city_layer.css('display') == 'none';
            if (b) {
                city_layer.show();
            }
            else {
                city_layer.hide();
            }
        });
    };
    //城市回调
    options.cities_init_callback = function (sender, data, paras) {
        var opts = sender.options;
        var cities = $("#div_cities");
        var pid = sender.options.stateid;
        for (var i = 0; i < data.length; i++) {
            var opt = $("<p></p>").append($('<a href="#"></a>').attr("pid", data[i].ID).attr("title", data[i].Name).text(data[i].Name.substring(0, 5))).appendTo(cities);
            if (sender.options.municipals['s' + pid]) opt.hide();
            if (opts.cityid == data[i].ID) {
                if (curCity) curCity.removeClass("curr");
                curCity = opt.find('a').addClass('curr');
                $(".city_input span").text(curCity.attr("title").substr(0, 5));
            }
        }
        cities.find("a").click(function (event, extra) {
            event.preventDefault();
            if (curCity) curCity.removeClass("curr");
            curCity = $(this).parent().addClass("curr");
            $(".city_layer").hide();
            $(".city_input span").text($(this).attr("title").substr(0, 5));
            sender.options.cityid = $(this).attr("pid");
            $('input[name="cities"]').val(opts.cityid);
            if (cityinit) {
                if (typeof chedai_config != 'undefined' && chedai_config.pagetype == 'apply') {
                    sender.loadunique();
                }
                else {
                    sender.load();
                }
            }
        });
        if (sender.options.municipals['s' + pid]) {
            cities.find("a:eq(0)").click();
        }
        cityinit = true;
    };

    //显示城市选择列表
    //    city_list.mouseenter(function () {
    //        city_layer.one("mouseleave", function () {
    //            $(this).hide();
    //        }).show();
    //    });
    city_list.mouseenter(function () {
        city_layer.show();
    }).mouseleave(function () {
        city_layer.hide();
    });

    $(document).click(function (event) {
        event.stopPropagation();
        if (event.target != city_layer[0] && city_layer.has(event.target).length === 0 && event.target != city_list.find('.xiala')[0]) {
            city_layer.hide();
        }
    });

    function init_cs() {
        var carBox = $('.xiala_box'), carList = carBox.find('.relation_tab_new'), packBox = $("ul.taochan"), loadingDiv = $('.tc_loging'), emptyDiv = $('.no_mess').clone(true), pagerbox = $(".fanye_box");
        //获取IP定向
        if (typeof bit_locationInfo != "undefined" && bit_locationInfo.cityId) {
            options.cityid = bit_locationInfo.cityId;
            options.stateid = Math.floor(options.cityid / 100)
        }
        //车型初始化完成回调
        options.carlist_init_callback = function (sender, data) {
            loadingDiv.show();
            var groups = {};
            var maxPv = 0;
            for (var key in data) {
                var o = data[key];
                if (!(o.referprice > 0)) continue;
                if (!groups[o.goid]) groups[o.goid] = [];
                o.pv = options.pvdata && options.pvdata[key] ? options.pvdata[key] : 0;
                maxPv = o.pv > maxPv ? o.pv : maxPv;
                groups[o.goid].push(o);
            }
            var html = [];
            for (var key in groups) {
                html.push('<h5>' + key + '</h5>');
                html.push('<ul year="' + key + '">');
                for (var i = 0; i < groups[key].length; i++) {
                    var o = groups[key][i];
                    html.push('<li carid="' + o.id + '" title="' + key + ' ' + (options.csname ? options.csname + ' ' : '') + o.name + '"><div class="w1"><a href="#">' + o.name + '</a></div><div class="wbox"><p class="bfb" style="width:' + (maxPv > 0 ? o.pv * 1.0 / maxPv : 0) + '%"></p></div><div class="bs">手动</div><div class="jg">' + o.referprice + '万</div></li>');
                }
                html.push('</ul>');
            }
            //carList.html(html.join(''));
        };
        //显示套餐
        options.after_load_callback = function (sender, data) {
            packBox.find("li:gt(0)").remove(); //移除套餐行
            var hasRows = data && data.Total > 0; //是否包含数据
            $(".map_link em").text(hasRows ? data.Total : '0');
            if (!hasRows) {
                if (!hasRows) {
                    emptyDiv.show().appendTo(packBox); //显示无数据提示
                    $(".fanye_box").hide();
                    return;
                }
            }
            var html = [], features = carloan.features, arr = data.Packages, opts = sender.options;
            html.push(packBox.html());
            var dicComs = {};
            for (var i = 0; i < data.Companies.length; i++) {
                dicComs[data.Companies[i].Id] = data.Companies[i];
            }
            for (var i = 0; i < arr.length; i++) {
                var row = arr[i];
                var tj = row.PromotionTitle && row.PromotionTitle.length ? '<span class="tuijian">推荐</span>' : "";
                var prmo = ''; //row.PromotionTitle && row.PromotionTitle.length ? '<p class="txt">{0}</p>'.format(row.PromotionTitle) : ""; (不显示促销了)
                var feath = "";
                for (var j = 0; row.Features && j < 3 && j < row.Features.length; j++) {
                    var frow = row.Features[j];
                    if (features["f" + frow.Id]) {
                        feath += '<a class="' + features["f" + frow.Id]["class"] + '" href="#">' + frow.Name + '<div class="box300" style="display:none;"><span class="sm_txtbox"><em class="jt"></em>' + frow.Description + "</span></div></a>";
                    }
                }
                var appUrl = carloan.common.format(opts.applyposturl, opts.csid, opts.csspell, opts.carid, row.Id, opts.payrate * 100, opts.loanmonths, opts.cityid);
                var url = carloan.common.format(opts.loandetailurl, opts.csid, opts.csspell, opts.carid, row.Id, opts.payrate * 100, opts.loanmonths, opts.cityid);
                var s = carloan.common.format('<li url="' + url + '"><div class="jg"><p class="tit"><a href="' + url + '" target="_blank">{6}</a>{8}</p><p class="cont"><a href="' + url + '" target="_blank">{1}</a></p>{9}</div>'
                            + '<div class="zlx">{3}</div>'
                            + '<div class="yugong"><span class="red">{4}元</span><a class="sm" href="#">说明<p class="sm_txtbox" style="display:none;"><em class="jt"></em>月利率 {7}%</p></a></div>' + '<div class="tedian" style="height:60px;">{10}</div>'
                            + '<div class="shenqin"><a target="_blank" href="' + appUrl + '&ref=sq2' + '" class="select_b">免费申请</a></div>'
                            + '<div class="clear"></div>'
                            + "</li>", row.Id, row.Name, row.PromotionTitle, row.TotalInterest == 0 ? "0元" : (row.TotalInterest * 1 / 1e4).toFixed(2) + "万元", row.MonthlyPayment.toFixed(0),
                                row.CompanyId, (dicComs[row.CompanyId] ? dicComs[row.CompanyId].Name : ''), (row.InterestRate / 0.12).toFixed(2), '', prmo, feath);
                html.push(s);
            }
            packBox.html(html.join(""));
            packBox.find('li:gt(0)')
                .mouseenter(function () {
                    if (curPakage) curPakage.removeClass("current");
                    curPakage = $(this).addClass("current");
                })
                .mouseleave(function () {
                    curPakage = curPakage.removeClass("current");
                })
            //提示
            $(".tedian>a").click(function (event) {
                event.preventDefault();
            }).mouseenter(function () {
                $(this).find(".box300").show();
            }).mouseleave(function () {
                $(this).find(".box300").hide();
            });
            $(".yugong .sm").click(function (event) {
                event.preventDefault();
            }).mouseenter(function () {
                $(this).find(".sm_txtbox").show();
            }).mouseleave(function () {
                $(this).find(".sm_txtbox").hide();
            });

            if (data.Total <= opts.pagelength) return;
            $(".fanye_box").show();
            $("#div-pager").pagination(data.Total, {
                current_page: opts.page - 1,
                items_per_page: opts.pagelength,
                prev_text: "上一页",
                next_text: "下一页",
                prev_disabled_css: "nolink",
                next_disabled_css: "nolink",
                callback_after_init: false,
                callback: function (page, $el) {
                    car_loan.load({ page: page + 1 });
                    return false;
                }
            });
        }
        options.before_load_callback = function (sender) {
            packBox.find("li:gt(0)").remove(); //移除套餐行
            loadingDiv.show();

            var opts = sender.options;
            $("#price-total").text(opts.referprice + "万");
            $("#price-firstpay").text((opts.referprice * opts.payrate).toFixed(2) + "万");
            $("#loan-money").text((opts.referprice * (1 - opts.payrate)).toFixed(2) + "万");
            $("#loan-time").text($("#filter-months a.curr").text());

            //快速申请
            var appUrl = carloan.common.format(opts.applyposturl, opts.csid, opts.csspell, opts.carid, 0, opts.payrate * 100, opts.loanmonths, opts.cityid);
            $('#quckapply').attr('href', appUrl + '&ref=sq1');
        };
        options.load_complete_callback = function () {
            loadingDiv.hide();
        };

        //显示隐藏车型下接列表
        carBox.mouseenter(function (event) {
            event.stopPropagation();
            carBox.find('.xiala').addClass('current');
            carList.show();
        }).mouseleave(function (event) {
            event.stopPropagation();
            carBox.find('.xiala').removeClass('current');
            carList.hide();
        });
        $(document).bind('click', function (event) {
            event.stopPropagation();
            if (event.target != carBox[0] && carBox.has(event.target).length === 0) {
                carBox.find('.xiala').removeClass('current');
                carList.hide();
            }
        });
        carBox.find('.xiala').click(function () {
            var b = carList.css('display') == 'none';
            if (b) {
                carList.show();
            }
            else {
                carList.hide();
            }
        });

        //选择车型
        var trCar = false;
        carList.find('tr').mouseenter(function () {
            trCar = $(this).css("background-color", 'rgb(235, 235, 235)');
        })
        .mouseleave(function () {
            trCar && trCar.css("background-color", '');
        })
        .click(function (event) {
            event.stopPropagation();
            event.preventDefault();
            var self = $(this);
            selCar && selCar.removeClass('current');
            selCar = self.addClass('current');
            carBox.find('.xiala').text(self.attr('carname')).removeClass('current');
            carList.hide();
            car_loan.options.carid = self.attr('carid');
            car_loan.options.referprice = self.attr('price');
            car_loan.load();
        });
        //默认选择
        var selCar = carList.find('tr[carid="' + options.carid + '"]')
        if (options.carid == 0 || !selCar.length) {
            selCar = carList.find('tr:first');
        }
        if (selCar.length) {
            selCar.addClass('current');
            carBox.find('.xiala').text(selCar.attr('carname'));
            options.carid = selCar.attr('carid');
            options.referprice = selCar.attr('price');
        }

        //选择利率
        $("#filter-rate a").click(function (event) {
            event.preventDefault();
            curRate && curRate.removeClass("curr");
            curRate = $(this).addClass("curr");
            car_loan.options.payrate = parseFloat($(this).attr("val"));
            car_loan.load();
        });

        //选择月份
        $("#filter-months a").click(function (event) {
            event.preventDefault();
            curMonths && curMonths.removeClass("curr");
            curMonths = $(this).addClass("curr");
            car_loan.options.loanmonths = parseFloat(curMonths.attr("val"));
            car_loan.load();
        });

        //贷款机构列表排序标签切换
        $("#suitlist .h3_tab li").click(function (event) {
            event.preventDefault();
            if (curSortTab) curSortTab.removeClass("current");
            curSortTab = $(this).addClass("current");

            car_loan.options.orderfield = $(this).attr("fieldname");
            car_loan.load();
        });
        curSortTab = $("#suitlist .h3_tab li:first");

        //默认首付
        if (options.payrate > 0) {
            var o = $('#filter-rate a[val="' + options.payrate + '"]');
            if (curRate) curRate.removeClass('curr');
            if (o.length) curRate = o.addClass('curr');
        }

        //默认期限
        if (options.loanmonths > 0) {
            var o = $('#filter-months a[val="' + options.loanmonths + '"]');
            if (curMonths) curMonths.removeClass('curr');
            if (o.length) curMonths = o.addClass('curr');
        }

        //初始化车贷
        var car_loan = new carloan(options);
        car_loan.init();
        car_loan.getapplycount(function (data) {
            if (data == '0') {
                $('#apply_count').hide();
            } else {
                $('#apply_count').hide();
                $('#apply_count').show().find('span').text(data);
            }
        });
    };

    function init_detail() {
        options.get_package_callback = function (data) {
            if (data.Package) {
                var pak = data.Package;
                $('#monthpay').text(pak.MonthlyPayment.toFixed(0) + '元');
                if (pak.TotalInterest > 0)
                    $('#totalpay').text((pak.TotalInterest / 10000.0).toFixed(2) + '万');
                else
                    $('#totalpay').text('无');
                if (pak.FinalPayment > 0)
                    $('#final_pay').text((pak.FinalPayment / 10000.0).toFixed(2) + '万');
                else
                    $('#final_pay').text('无');
            }
        };

        var car_loan = new carloan(options);
        var opts = car_loan.options;

        //获取月供及利率
        car_loan.getpackage();

        //申请车贷
        $('input[name="btn-apply"]').click(function () {
            var url = carloan.common.format(opts.applyposturl, opts.csid, opts.csspell, opts.carid, opts.agentid, opts.payrate * 100, opts.loanmonths, opts.cityid);
            window.location.href = url + '&ref=sq3';
        });
    }

    function init_apply() {
        var tb = $(".dkfa_box table"), container = $('.dkfa_box'), city_list = $("#city_list"), city_layer = $(".city_layer"), loadingDiv = $('.tc_loging'), emptyDiv = $('#emptydata')
        var $hasshow = $("#hasshowall"), curCarOpt = false;
        var cols = tb.html();

        //获取套餐ID
        var getPackIds = function () {
            var arr = [];
            tb.find('input[name="selpacks"]:checked').each(function (k, v) {
                arr.push(this.value);
            });
            $('input[name="packids"]').val(arr.join(","));
        };

        //车型初始化完成回调
        options.carlist_init_callback = function (sender, data) {
            var opts = sender.options;
            //追加年款属性
            var arr = [], cars = $("#cars");
            cars.find("option").each(function (k, v) {
                var o = data["t" + this.value];
                if (o) {
                    if (!(o.referprice > 0)) { arr.push($(this)); return; }
                    $(this).attr('title', o.goname + ' ' + (opts.csname ? opts.csname + ' ' : '') + o.name).attr('carname', o.name);
                }
            });
            for (var i = 0; i < arr.length; i++) arr[i].remove();
            //设置默认车型年款
            if (cars.val() > 0) {
                curCarOpt = cars.find("option:selected");
                curCarOpt.text(curCarOpt.attr('title'));
            } else {
                curCarOpt = cars.find("option[carname]:first");
                if (curCarOpt.length) {
                    cars.val(curCarOpt[0].value);
                    car_loan.options.carid = curCarOpt[0].value;
                    car_loan.options.referprice = data['t' + curCarOpt[0].value].referprice;
                }
            }
            //车型切换事件
            cars.bind('change', function () {
                curCarOpt && curCarOpt.text(curCarOpt.attr('carname'));
                var val = $(this).val();
                if (!(val > 0)) return;
                curCarOpt = cars.find("option:selected");
                curCarOpt.text(curCarOpt.attr('title'));
                car_loan.options.carid = val;
                car_loan.options.referprice = data['t' + val].referprice;
                car_loan.loadunique();
            });
        };

        //显示套餐
        options.after_load_callback = function (sender, data) {
            var opts = sender.options;
            var hasRows = data && data.Total > 0; //是否包含数据
            $(".map_link em").text(hasRows ? data.Total : '0');
            if (!hasRows) {
                loadingDiv.hide();
                emptyDiv.show();
                $('input[name="packids"]').val('');
                return;
            }

            emptyDiv.hide();
            var arr = data.Packages;
            var dicComs = {};
            for (var i = 0; i < data.Companies.length; i++) {
                dicComs[data.Companies[i].Id] = data.Companies[i];
            }
            var html = [];
            html.push(cols);
            for (var i = 0; i < arr.length; i++) {
                var row = arr[i];
                var urlstr = carloan.common.format(opts.loandetailurl, opts.csid, opts.csspell, opts.carid, row.Id, opts.payrate * 100, opts.loanmonths, opts.cityid);
                html.push(carloan.common.format('<tr' + (i < 3 ? '' : ' style="display:none;"') + '>'
                        + '<th scope="row"><input type="' + (opts.single_select === true ? 'radio' : 'checkbox') + '" value="{0}" name="selpacks" {6}></th>'
                        + '<td><p class="tit"><a href="' + urlstr + '" target="_blank">{3}</a></p><p class="txt"><a href="' + urlstr + '" target="_blank">{1}</a></p></td><td><p class="lixi_yuegong"><span class="lixi">{4}</span>总成本</p></td>'
                        + '<td><p class="lixi_yuegong"><span class="yuegong">{5}元</span>月供</p></td>'
                        + "</tr>", row.Id, row.Name, row.CompanyId, (dicComs[row.CompanyId] ? dicComs[row.CompanyId].Name : ''), row.TotalInterest == 0 ? "0元" : (row.TotalInterest * 1 / 1e4).toFixed(2) + "万元", row.MonthlyPayment.toFixed(0), ''));
            }
            tb.html(tb.html() + html.join(""));
            if (opts.agentid > 0) {
                tb.find('th input[value="' + opts.agentid + '"]').attr('checked', true);
            } else {
                if (opts.single_select === true) {
                    tb.find('input[name="selpacks"]:first').attr('checked', true);
                }
                else {
                    tb.find('input[name="selpacks"]:lt(3)').attr('checked', true);
                }
            }
            if (data.Total <= 3) {
                $("#btn-show,#btn-hide").hide();
                $hasshow.show();
            } else {
                $("#btn-show,#btn-hide").show();
                $hasshow.hide();
            }

            tb.find('input[name="selpacks"]').click(function () {
                getPackIds();
            });
            getPackIds();
        };
        options.before_load_callback = function (sender) {
            var opts = sender.options;
            $("#price-firstpay").text((opts.referprice * opts.payrate).toFixed(2));
            $("#price-total").text(opts.referprice);
            tb.empty();
            loadingDiv.show();
        };
        options.load_complete_callback = function () {
            loadingDiv.hide();
        };

        //显示更多
        $("#btn-show").click(function () {
            $(this).hide();
            $("#btn-hide").show();
            tb.find("tr").show();
        });
        //隐藏更多
        $("#btn-hide").click(function () {
            $(this).hide();
            $("#btn-show").show();
            var rows = tb.find("tr");
            if (rows.length > 3) {
                tb.find("tr:gt(2)").hide();
            }
        });
        //初始默认提示
        $('#form_chedai input[name="name"],#form_chedai input[name="age"],#form_chedai input[name="mobile"]').blur(function () {
            this.value = this.value == "" ? this.defaultValue : this.value;
        }).focus(function () {
            this.value = this.value == this.defaultValue ? "" : this.value;
        });

        if (typeof $.validator != "undefined" && $.validator.addMethod) {
            //扩展验证中文
            $.validator.addMethod("chinese", function (value, element, param) {
                return /^[\u4e00-\u9fa5]+$/.test(value);
            });
            // 手机号码验证       
            $.validator.addMethod("mobile", function (value, element) {
                var length = value.length;
                var mobile = /^1[3|4|5|8][0-9]\d{4,8}$/;
                return this.optional(element) || length == 11 && mobile.test(value);
            }, "请正确输入11位手机号码！");
            jQuery.validator.addMethod("defaultInvalid", function (value, element) {
                return !(element.value == element.defaultValue);
            });
            //验证申请
            var chedai_val = $("#form_chedai").validate({
                debug: true,
                //onfocusout: false,
                //onclick: false,
                onkeyup: false,
                ignore: ".ignore",
                errorElement: "em",
                errorContainer: $(".tishi"),
                errorPlacement: function (error, element) {
                    error.appendTo(element.closest("li,.val-field").find(".ts_txt"));
                },
                rules: {
                    cities: { required: true, number: true, min: 1 },
                    period: { required: true, number: true },
                    cars: { required: true, number: true, min: 1 },
                    age: { required: true, defaultInvalid: true, number: true, range: [18, 60] },
                    name: { required: true, defaultInvalid: true, rangelength: [2, 8], chinese: true },
                    mobile: { required: true, maxlength: 11, number: true, mobile: true },
                    packids: { required: true }
                },
                messages: {
                    cities: { required: "请选择城市", min: "请选择城市" },
                    period: { required: "请选择时间！", number: "请选择时间！" },
                    cars: { required: "请选择车型", min: "请选择车型" },
                    age: { required: "请填写年龄！", defaultInvalid: "请填写年龄！", number: "请填写年龄！", range: "对不起，您的年龄尚不具备申请车贷资质！" },
                    name: { required: "请填写您的姓名！", defaultInvalid: "请填写您的姓名！", rangelength: "请填写2~8个汉字的正确姓名！", chinese: "请填写2~8个汉字的正确姓名！" },
                    mobile: { required: "请输入手机号码！", maxlength: '请正确输入11位手机号码！', defaultInvalid: "请输入手机号码！", number: "请正确输入11位手机号码！" },
                    packids: { required: "您至少要选择一种金融套餐！" }
                }
            });
        }
        var btnSub = $('#form_chedai input[name="submit"]');
        //保存订单
        var saveOrder = function (data) {
            $('body').css({ 'cursor': 'wait' });
            var data = {
                CityId: $("#cities").val(),
                CarModelId: $("#cars").val(),
                BuyCarTime: $("#period").val(),
                Name: $('#form_chedai input[name="name"]').val(),
                Gender: $('#form_chedai input[name="gender"]:checked').val(),
                Age: $('#form_chedai input[name="age"]').val(),
                Mobile: $('#form_chedai input[name="mobile"]').val(),
                PackageIds: $('#form_chedai input[name="packids"]').val(),
                Ref: chedai_config.ref
            };
            //验证手机号
            car_loan.submitapply(data, {
                success: function (json) {
                    btnSub.removeAttr("disabled");
                    $('body').css({ 'cursor': 'default' });
                    if (json == true || json.IsSuccess) {

                        //$('#apply-form').hide();
                        //$('#apply-succ').show();
                        if (options.apply_success_url && json.ApplicationId > 0) {
                            var url = carloan.common.format(options.apply_success_url, json.ApplicationId);
                            window.location.href = url;
                        }
                    } else {
                        var msg = '保存失败！';
                        if (carloan.errors['error' + json.ErrorCode]) {
                            msg = carloan.errors['error' + json.ErrorCode].message
                        }
                        alert(msg);
                    }
                },
                error: function () {
                    alert("暂时无法提交申请，请稍候再试！");
                    btnSub.removeAttr("disabled");
                    $('body').css({ 'cursor': 'default' });
                }
            });
        };
        $("#form_chedai").submit(function (e) {
            e.preventDefault();
            $("#cars").removeAttr("disabled");
            if (!$("#form_chedai").valid()) return false;
            btnSub.attr("disabled", true);
            saveOrder(false);
            return false;
        });
        //更新鼠标
        $('body').ajaxStart(function () {
            $(this).css({ 'cursor': 'wait' })
        }).ajaxStop(function () {
            $(this).css({ 'cursor': 'default' })
        });

        var car_loan = new carloan(options);
        car_loan.init();
        $('input[name="cities"]').val(car_loan.options.cityid); //设置当前城市的表单值

        $('#payrate').val(car_loan.options.payrate);
        //切换首付
        $('#payrate').bind('change', function () {
            car_loan.options.payrate = $(this).val();
            car_loan.loadunique();
        });

        $('#loanmonths').val(car_loan.options.loanmonths);
        //切换首付
        $('#loanmonths').bind('change', function () {
            car_loan.options.loanmonths = $(this).val();
            car_loan.loadunique();
        });

        //贷款机构列表排序标签切换
        $("#sorttab a").click(function (event) {
            event.preventDefault();
            if (curSortTab) curSortTab.removeClass("nor_px_curr").addClass('nor_px');
            curSortTab = $(this).addClass("nor_px_curr").removeClass('nor_px');
            car_loan.options.orderfield = $(this).attr("fieldname");
            car_loan.loadunique();
        });
        curSortTab = $("#sorttab a.nor_px_curr");
    }

    if (typeof chedai_config != 'undefined') {
        switch (chedai_config.pagetype) {
            case 'serial':
                init_cs(); break;
            case 'detail':
                init_detail(); break;
            case 'apply':
                init_apply(); break;
        }
    }
});
