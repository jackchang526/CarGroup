var carloan = function (options) {
    this.options = { loading: false, year: 0, referprice: 0, csid: 0, carid: 0, payrate: .3, loanmonths: 36, page: 1, pagelength: 20, stateid: 2, cityid: 201, provdropdownlist: '#prov-list', citydropdownlist: '#city-list', orderfield: "MonthlyPayment", agentid: 0 };
    this.options.api_get_package_list = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetPackages/?downPaymentRate={4}&repaymentPeriod={5}&carModelId={2}&cityId={6}&orderBy={7}&pageNum={8}";
    this.options.api_get_unique_package = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetUniquePackagesByCompanyAndAppointFirst/?downPaymentRate={4}&repaymentPeriod={5}&carModelId={2}&cityId={6}&orderBy={7}&pageNum={8}&packageId={3}";
    this.options.api_get_package = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/GetPackageWithApplicationCount/?packageId={3}&downPaymentRate={4}&carModelId={2}&cityId={6}";
    this.options.api_submit_apply = "http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/SubmitApplication/?callback=?";
    this.options.api_get_province_list = 'http://mai.bitauto.com/InsuranceAndLoan/Region/GetProvinces';
    this.options.api_get_city_list = 'http://mai.bitauto.com/InsuranceAndLoan/Region/GetCities?provinceid={0}';

    this.options.carlist_init_callback = false; //获取车型列表回调
    this.options.provinces_init_callback = false; //获取省份列表回调
    this.options.cities_init_callback = false; //获取城市列表回调
    this.options.before_load_callback = false;
    this.options.after_load_callback = false;
    this.options.load_failed_callback = false;
    this.options.single_select = false;

    this.options = $.extend(this.options, options);
    this.data = {};
    this.submiting = false;
};

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
    }
};

carloan.errors = {
    error1: { code: 1, message: '未选择套餐' },
    error2: { code: 2, message: '您已申请过该车型，请查看其他车型' },
    error4: { code: 4, message: '保存失败' }
};

window.json_callback_for_prov = function (data) { };
window.json_callback_for_city = function (data) { };
window.json_callback_for_load = function (data) { };

//加载城市
carloan.prototype.getcities = function () {
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
            }
            $city.html(html.join(''));
            $city.change(function () {
                opts.cityid = $city.val();
                _this.load();
            });
            if (opts.cityid > 0) {
                $city.val(opts.cityid);
            }
            opts.cities_init_callback && _this.options.cities_init_callback(_this, data); //回调
        },
        error: function (jqXHR, textStatus, errorThrown) { },
        complete: function (jqXHR, textStatus) { }
    });
};

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
                _this.load();
            }
        }
    });

    //加载省份
    $.ajax({ url: opts.api_get_province_list, cache: true, dataType: "jsonp", jsonpCallback: "json_callback_for_prov",
        success: function (data) {
            var $prov = $(opts.provdropdownlist);
            if ($prov.length) {
                var html = [];
                html.push('<option value="0">请选择</option>');
                for (var i = 0; i < data.length; i++) {
                    var o = data[i];
                    html.push(carloan.common.format('<option value="{0}">{1}</option>', o.ID, o.Name));
                }
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
            opts.provinces_init_callback && _this.options.provinces_init_callback(_this, data); //回调
        },
        error: function (jqXHR, textStatus, errorThrown) {
        },
        complete: function (jqXHR, textStatus) {
        }
    });
};

carloan.prototype.load = function () {
    var _this = this;
    var opts = _this.options;

    //加载前
    opts.before_load_callback && opts.before_load_callback(_this);

    var dataUrl = carloan.common.format(opts.api_get_unique_package, opts.csid, opts.csspell, opts.carid, opts.agentid, opts.payrate, opts.loanmonths, opts.cityid, opts.orderfield, opts.page);

    if (!(opts.carid > 0) || !(opts.loanmonths > 0) || !(opts.payrate > 0) || !(opts.cityid > 0)) {
        opts.after_load_callback && opts.after_load_callback(_this, null);
        return;
    }

    typeof console != 'undefined' && console.log && console.log(dataUrl);
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
        complete: function (jqXHR, textStatus) { }
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
        },
        complete: function (jqXHR, textStatus) {
            ajaxOpts && ajaxOpts.complete && ajaxOpts.complete(jqXHR, textStatus);
            $('body').css({ 'cursor': 'default' });
            _this.submiting = false;
        }
    });
};

//表现层
$(function () {
    var scope_vars = { car_displayer: $('#m-show-item'), car_list_box: $('#car-list-box'), car_selector: $('#car-list'), load_layer: $('#load-layer'), cur_select_car: false, cur_select_rate: false, cur_select_month: false };
    var emptylist = $('#empty-list'), packlist = $('#pack-list'), morepacks = $('#pack-more'), showmore = $('#btn-show-more'), hidemore = $('#btn-hide-more'), btnsubmit = $('#btn-submit');
    var options = typeof chedai_config != 'undefined' ? chedai_config : {};

    var reset = function () {
        packlist.empty();
        emptylist.hide();
        packlist.hide();
        morepacks.hide();
        showmore.hide();
        hidemore.hide();
    };

    options.carlist_init_callback = function (sender, data) {
        scope_vars.car_list_box.hide();
        var html = [];
        scope_vars.car_displayer.find('span').text('请选择');
        for (var name in data) {
            var o = data[name];
            if (o.referprice > 0) {
                html.push('<li name="car" data="' + o.id + '"><a href="#">' + o.goname + ' ' + o.name + '</a></li>');
            }
        }
        scope_vars.car_selector.html(html.join(''));
        //选中车型
        scope_vars.car_selector.find('li').click(function (event) {
            event.preventDefault();
            event.stopPropagation();
            if (scope_vars.cur_select_car) {
                scope_vars.cur_select_car.removeClass('current').html('<a href="#">' + scope_vars.cur_select_car.text() + '</a>');
            }
            scope_vars.cur_select_car = $(this).addClass('current').html($(this).text());
            scope_vars.car_displayer.find('span').text($(this).text());

            scope_vars.car_list_box.hide();
            scope_vars.car_displayer.find('b').hide();

            sender.options.carid = $(this).attr('data'); //设置选中车型ID
            sender.load();
        });
        //选择车型
        scope_vars.car_displayer.click(function () {
            var hasShown = scope_vars.car_list_box.css('display') == 'none';
            if (hasShown) {
                scope_vars.car_displayer.find('b').show();
                scope_vars.car_list_box.show();
            }
            else {
                scope_vars.car_displayer.find('b').hide();
                scope_vars.car_list_box.hide();
            }
        });
        //点击空白处隐藏车型列表
        $(document).click(function (event) {
            event.stopPropagation();
            if (event.target != scope_vars.car_list_box[0] && scope_vars.car_list_box.has(event.target).length === 0 && event.target != scope_vars.car_displayer[0] && scope_vars.car_displayer.has(event.target).length === 0) {
                scope_vars.car_displayer.find('b').hide();
                scope_vars.car_list_box.hide();
            }
        });

        //设置默认车型
        if (scope_vars.car_selector.find('li[data]').length) {
            var cur_car = scope_vars.car_selector.find('li[data="' + options.carid + '"]');
            if (cur_car.length == 0) { //页面默认车型不存在取第一个车型
                var cur_car = scope_vars.car_selector.find('li[data]:eq(0)');
                car_loan.options.carid = cur_car.attr('data');
            }
            if (cur_car.length) {
                scope_vars.cur_select_car = cur_car.addClass('current').html(cur_car.text());
                scope_vars.car_displayer.find('span').text(cur_car.text());
            }
        }
    };

    options.before_load_callback = function (sender) {
        reset();
        scope_vars.load_layer.show(); //显示加载层
    };

    options.after_load_callback = function (sender, data) {
        scope_vars.load_layer.hide(); //隐藏加载层
        var displayCount = 3;
        var car = sender.data.carlist['t' + sender.options.carid];
        if (car) {
            $("#price-total").text(car.referprice + "万元"); //裸车总价
            $("#price-firstpay").text((car.referprice * sender.options.payrate).toFixed(2) + "万元"); //首付
        }
        if (!data || data.Total == 0) {
            $('#empty-list').show();
            return;
        }

        var arr = data.Packages;
        var dicComs = {};
        for (var i = 0; i < data.Companies.length; i++) {
            dicComs[data.Companies[i].Id] = data.Companies[i];
        }
        var html = [];
        var html2 = [];
        for (var i = 0; i < arr.length; i++) {
            var row = arr[i];

            var s = carloan.common.format(
                '<div class="m-cd-jxs-item">' +
                    '<div class="m-cd-jxs-item-check"><input name="selectedpak" type="' + (sender.options.single_select ? 'radio' : 'checkbox') + '" value="{0}"></div>' +
                    '<div class="m-cd-jxs-item-info">{6}' +
                        '<p><span class="interese-box">总利息：<em>{3}</em></span> <span class="interese-box">月 供：<em>{4}</em></span></p>' +
                    '</div>' +
                '</div>', row.Id, row.Name, row.PromotionTitle, row.TotalInterest == 0 ? "0元" : (row.TotalInterest * 1 / 1e4).toFixed(2) + "万元", row.MonthlyPayment.toFixed(0) + '元',
                          row.CompanyId, (dicComs[row.CompanyId] ? dicComs[row.CompanyId].Name : ''), (row.InterestRate / 0.12).toFixed(2));
            if (i < displayCount) {
                html.push(s);
            }
            else {
                html2.push(s);
            }
        }
        $('#empty-list').hide();
        packlist.html(html.join('')).show();
        morepacks.html(html2.join(''));

        var pak_items = $('.m-cd-jxs-item');
        pak_items.click(function (event) {
            event.stopPropagation();
            var o = $(this);
            var ckb = o.find('input[name="selectedpak"]');

            var checked = false;
            if (ckb.length) {
                checked = ckb[0].checked;
                if (event.target != ckb[0]) ckb.attr('checked', !checked);
                if (sender.options.single_select) {
                    pak_items.removeClass('m-cd-jxs-item-selected');
                }
                if ((event.target == ckb[0] && checked) || (event.target != ckb[0] && !checked)) {
                    o.addClass('m-cd-jxs-item-selected');
                }
                else {
                    o.removeClass('m-cd-jxs-item-selected');
                }
            }
        });

        if (sender.options.single_select) {
            packlist.find('.m-cd-jxs-item:first').click();
        }
        else {
            packlist.find('.m-cd-jxs-item').click();
        }

        if (html2.length) {
            showmore.show();
            showmore.click(function (event) {
                event.preventDefault();
                morepacks.show();
                hidemore.show();
                showmore.hide();
            });
            hidemore.click(function () {
                event.preventDefault();
                morepacks.hide();
                hidemore.hide();
                showmore.show();
            });
        }
    };

    options.load_failed_callback = function (sender, jqXHR, textStatus, errorThrown) {
        reset();
        $('#empty-list').show();
        return;
    }

    //初始化车贷
    var car_loan = new carloan(options);
    car_loan.init();

    //选中期限
    $('#month-list a').click(function () {
        event.preventDefault();
        if (scope_vars.cur_select_month) {
            scope_vars.cur_select_month.removeClass('tj-sty-checked');
        }
        scope_vars.cur_select_month = $(this).addClass('tj-sty-checked');

        car_loan.options.loanmonths = $(this).attr('val'); //设置选中车型ID
        car_loan.load();
    });
    //默认期限
    if (options.loanmonths > 0) {
        var o = $('#month-list a[val="' + options.loanmonths + '"]');
        o.length && (scope_vars.cur_select_month = o.addClass('tj-sty-checked'));
    }

    //选中首付
    $('#rate-list a').click(function () {
        event.preventDefault();
        if (scope_vars.cur_select_rate) {
            scope_vars.cur_select_rate.removeClass('tj-sty-checked');
        }
        scope_vars.cur_select_rate = $(this).addClass('tj-sty-checked');

        car_loan.options.payrate = $(this).attr('val'); //设置选中车型ID
        car_loan.load();
    });
    //默认首付
    if (options.payrate > 0) {
        var o = $('#rate-list a[val="' + options.payrate + '"]');
        o.length && (scope_vars.cur_select_rate = o.addClass('tj-sty-checked'));
    }

    //防止默认提示
    $("#form_chedai").submit(function (event) {
        event.preventDefault();
    });

    //验证申请
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
        var chedai_val = $("#form_chedai").validate({
            debug: true,
            //onfocusout: false,
            //onclick: false,
            onkeyup: false,
            ignore: ".ignore",
            errorElement: "div",
            errorContainer: $(".tishi"),
            errorPlacement: function (error, element) {
                error.addClass('alert').appendTo($('#val-' + element.attr('name')));
            },
            rules: {
                name: { required: true, rangelength: [2, 8], chinese: true },
                mobile: { required: true, number: true, mobile: true },
                //packids: { required: true },
                selectedpak: { required: true }
            },
            messages: {
                name: { required: "请输入姓名！", rangelength: "请正确输入姓名！", chinese: "请正确输入姓名！" },
                mobile: { required: "请输入手机号！", number: "请正确输入手机号！", mobile: "请正确输入手机号！" },
                //packids: { required: "请选择金融机构！" },
                selectedpak: { required: "请选择金融机构！" }
            },
            submitHandler: function (form) {
                var data = { Name: $('#form_chedai input[name="name"]').val(),
                    Gender: $('#form_chedai input[name="gender"]:checked').val(),
                    Mobile: $('#form_chedai input[name="mobile"]').val(),
                    PackageIds: $('#form_chedai input[name="packids"]').val()
                };
                car_loan.submitapply(data, {
                    success: function (data) {
                        var msg = '提交失败！';
                        if (data == true || data.IsSuccess) {
                            msg = '提交成功！';
                        } else {
                            if (carloan.errors['error' + data.ErrorCode]) {
                                msg = carloan.errors['error' + data.ErrorCode].message
                            }
                        }
                        alert(msg);
                        //跳转
                        if (data == true || data.IsSuccess) {
                            var opts = car_loan.options;
                            var url = '/' + opts.csspell + '/';
                            if (opts.ref && opts.ref.substr(0, 3) == 'app') {
                                url += '?end=' + opts.ref;
                            }
                            window.location.href = url;
                        }
                    }
                });
                return false;
            }
        });
    }

    //提交按钮
    btnsubmit.click(function (event) {
        event.preventDefault();
        var arr = [];
        $('input[name="selectedpak"]:checked').each(function (k, v) {
            arr.push(this.value);
        });
        $('#form_chedai input[name="packids"]').val(arr.join(','));
        $("#form_chedai").submit();
    });

    //返回链接
    $('#prebtn').click(function () {
        var backUrl = '/' + car_loan.options.csspell + '/';
        if (car_loan.options.returnurl && car_loan.options.returnurl.length) {
            backUrl = car_loan.options.returnurl;
        }
        else if (document.referrer && document.referrer.length) {
            backUrl = document.referrer;
        }
        window.location.href = backUrl;
    });
});