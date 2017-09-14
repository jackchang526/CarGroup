function funcGetEsc() {
        var cityId = 201, cityAllSpell = 'beijing';
        if (bit_locationInfo && bit_locationInfo.cityId) {
            cityId = bit_locationInfo.cityId;
            cityAllSpell = bit_locationInfo.engName;
        }
        //商城接口(包销)
        $.ajax({
            url: "http://api.car.bitauto.com/mai/GetSerialParallelAndSell.ashx?serialId=" + serialId + "&cityid=" + cityId,
            async: false,
            dataType: "jsonp",
            jsonpCallback: "mall",
            success: function(data) {
                var mallPrice = "暂无";
                //包销车或者平行进口车
                if (data.CarList.length > 0) {
                    for (var i = 0; i < data.CarList.length; i++) {
                        if (data.CarList[i].CarId == $("#hidCarID").val()) {
                            if (data.CarList[i].CarType == 0) {
                                $("#hidCarType").val("包销");
                                mallPrice = data.CarList[i].Price;
                                break;
                            } else if (data.CarList[i].CarType == 1) {
                                $("#hidCarType").val("平行进口");
                                break;
                            }
                        }
                    }
                }

                //价格区
                if ($("#hidCarType").val() == "停销") {
                    var erShouCheHtml = [];
                    erShouCheHtml.push("<li>二手车报价：");
                    erShouCheHtml.push("<a href=\"http://yiche.taoche.com/buycar/b-"+serialAllSpell+"/?page=1&carid="+carId+"\"><strong>"+ucarPrice+"</strong></a>");
                    erShouCheHtml.push("<em>(指导价"+(referPrice>0?referPrice+"万元":"暂无")+")</em></li>");
                    erShouCheHtml.push("<li>");
                    $("#ulJiaGeInfo").html(erShouCheHtml.join(""));
                    $("#erShouCheDiv").show();
                } else {
                    var contentHtml = [];
                    if ($("#hidCarType").val() == "包销") {
                        contentHtml.push("<li><i>商城直销价：</i>");
                        contentHtml.push("<a href=\"http://m.yichemall.com/car/Detail/Index?carId="+carId+"&source=100546\"><strong>" + mallPrice + "万元</strong></a>");
                    } else {
                        contentHtml.push("<li><i>全国参考价：</i>");
                        contentHtml.push("<a data-channelid=\"27.24.1329\" href=\"/"+serialAllSpell+"/m"+carId+"/baojia/\">"+(cankaoPrice=="暂无"?"暂无":"<strong>"+cankaoPrice+"元</strong>")+"</a>");
                    }

                    contentHtml.push("<em>(指导价"+(referPrice>0?referPrice+"万元":"暂无")+")</em></li>");
                    contentHtml.push("<li>全款：<span>"+(totalPay=="暂无"?totalPay:"约"+totalPay)+"</span> <em>(仅供参考)</em><a href=\"/gouchejisuanqi/?carid="+carId+"\" data-channelid=\"27.24.1330\" class=\"m-ico-calculator\"></a></li>");
                    contentHtml.push("<li>贷款：<span>首付"+loanDownPay+"，月供"+monthPay+"</span> <em>(36期)</em></li>");
                    if (taxContent != "" && serialSaleState == "在销") {
                        contentHtml.push("<li><em class=\"m-ico-jianshui\">减税</em>" + taxContent + "</li>");
                    }
                    $("#ulJiaGeInfo").html(contentHtml.join(""));
                    if ($("#hidCarType").val() == "包销") {
                        $("#baoxiaoDiv").show();
                    } else if (serialSaleState == "待销") { //车系的销售状态cssalestate=“待销”时，车款为“未上市”状态;未上市不显示按钮
                        return;
                    } else {
                        $("#notBaoXiaoDiv").show();
                    }
                }

                //二手车源
                if ($("#hidCarType").val() == "停销") {
                    $("#ershouCheT,#ershouCheContent").show();
                    $.ajax({
                        url: "http://yicheapi.taoche.cn/CarSourceInterface/ForJson/CarListForYiChe.ashx?sid="+serialId,
                        cache: true,
                        dataType: "jsonp",
                        jsonpCallback: "callback",
                        success: function(data) {
                            data = data.CarListInfo;
                            if (data.length <= 0) return;
                            var strHtml1 = [];
                            var strHtml2 = [];
                            strHtml1.push("<div class=\"tt-first\">");
                            strHtml1.push("<h3>二手车源</h3>");
                            strHtml1.push("<div class=\"opt-more opt-more-gray\"><a href=\"http://m.taoche.com/" + $("#hidAllSpell").val() + "/?WT.mc_id=\">更多</a></div>");
                            strHtml1.push("</div>");

                            strHtml2.push("<div class=\"pic-txt pic-txt-car\">");
                            strHtml2.push("<ul>");
                            $.each(data, function(i, n) {
                                if (i > 3) return;
                                strHtml2.push("<li>");
                                strHtml2.push("<a href=\"" + n.CarlistUrl.replace(".taoche.com", ".m.taoche.com") + "\">");
                                strHtml2.push("<span>");
                                strHtml2.push("<img src=\"" + n.PictureUrl + "\">");
                                strHtml2.push("</span>");
                                strHtml2.push("<p>" + n.CarName + "</p>");
                                strHtml2.push("<b>" + n.DisplayPrice + "</b>");
                                strHtml2.push("<em>" + n.BuyCarDate + "/" + n.DrivingMileage + "公里</em>");
                                strHtml2.push("</a>");
                                strHtml2.push("</li>");
                            });
                            strHtml2.push("</ul>");
                            strHtml2.push("</div>");

                            $("#ershouCheT").html(strHtml1.join(""));
                            $("#ershouCheContent").html(strHtml2.join(""));
                        }
                    });
                }

                //购车服务
                //GetCarServiceData();
                //统计
                var channelIDs = { "3": "27.24.125", "12": "27.24.132", "9": "27.24.129", "10": "27.24.130", "11": "27.24.131", "13": "27.24.133" };
                var urlEndPartCode = { "3": "?ref=mchekuanzshuan&leads_source=m003002", "9": "&tracker_u=318_ckzs&leads_source=m003004", "10": "&source=100546&leads_source=m003005", "11": "?ref=mcar2&rfpa_tracker=2_23&leads_source=m003006", "12": "?ref=mchekuanzska&leads_source=m003007" };

                //按钮统计
                var global_busbtn_arr = ["1", "8", "0", "6", "5", "7"];
                var global_busbtn_channelids = { "0": "27.24.126", "1": "27.24.129", "8": "27.24.124", "5": "27.24.128", "6": "27.24.127", "7": "27.24.130" };
                var global_busbtn_code = { "0": "?leads_source=m003003", "1": "&tracker_u=614_ckzs&leads_source=m003004", "8": "?from=ycm34&leads_source=m003001", "5": "?ref=mchekuanzsmai&leads_source=m003013", "6": "?ref=mchekuanzsgu", "7": "&source=100546&leads_source=m003012" };

                $.ajax({
                    url: "http://api.car.bitauto.com/api/GetBusinessService.ashx?date=20160721&action=mcar&cityid=" + cityId + "&serialid=" + serialId + "&carid=" + carId,
                    async: false,
                    dataType: "jsonp",
                    jsonpCallback: "businessCarCallBack",
                    cache: true,
                    success: function(data) {
                        if (data && data != null) {
                            var serviceHtml = [],
                                btnHtml = [],
                                btnCount = 0;
                            btnHtml.push("<ul>");
                            $.each(data.Button, function(i, n) {
                                if (i > 2) return;
                                if (global_busbtn_arr.indexOf(n.BusinessId) != -1) {
                                    btnHtml.push("<li " + ((n.BusinessId == "0" || n.BusinessId == "5" || n.BusinessId == "7") ? "class=\"btn-org\"" : "") + "><a data-channelid=\"" + (global_busbtn_channelids[n.BusinessId] || "") + "\" href=\'" + n.MobileUrl + (global_busbtn_code[n.BusinessId] || "") + "\'>" + n.LongTitle + "</a></li>");
                                    btnCount++;
                                }
                            });
                            btnHtml.push("</ul>");
                            if (btnCount == 2) {
                                $("#btn-business").html(btnHtml.join('')).addClass("sum-btn-two");
                            } else {
                                $("#btn-business").html(btnHtml.join(''));
                            }
                            if (data.BigButton) {
                                var bigBtnCnt = data.BigButton.length; //购车服务数量
                                var forend = bigBtnCnt; //循环终止标记
                                if (bigBtnCnt > 2) {
                                    forend = 3;
                                    serviceHtml.push("<ul class=\"three-service\">");
                                } else if (bigBtnCnt > 1) {
                                    serviceHtml.push("<ul class=\"two-service\">");
                                } else {
                                    //购车服务块不显示
                                    //$('.car-service').hide();
                                    //return;
                                    serviceHtml.push("<ul class=\"one-service\">");
                                }
                                var i;
                                for (i = 0; i < forend; i++) {
                                    var curChannelId = channelIDs[data.BigButton[i].BusinessId]; //统计
                                    serviceHtml.push("<li>");

                                    if (data.BigButton[i].BusinessId == "13") {
                                        serviceHtml.push("<a data-channelid=\"" + curChannelId + "\" href=\"" + data.BigButton[i].MobileUrl + "\"><strong>" + data.BigButton[i].Title + "</strong>");
                                        serviceHtml.push("<em class=\"cGray\">" + data.BigButton[i].Price + "</em>");
                                    } else {
                                        serviceHtml.push("<a data-channelid=\"" + curChannelId + "\" href=\"" + data.BigButton[i].MobileUrl + urlEndPartCode[data.BigButton[i].BusinessId] + "\"><strong>" + data.BigButton[i].LongTitle + "</strong>");
                                        serviceHtml.push("<em>" + data.BigButton[i].Price + "</em>");
                                    }
                                    serviceHtml.push("</a>");
                                    serviceHtml.push("</li>");
                                }
                                serviceHtml.push("</ul>");
                                $(".car-service").html(serviceHtml.join('')).show();
                                //add log statistic
                                if(typeof Bglog_InitPostLog === "function")
                                    Bglog_InitPostLog();
                            }
                        }
                    }
                });

                //经销商
                if ($("#hidCarType").val() == "停销" || $("#hidCarType").val() == "包销") {
                    $("#tuiJianHeader,#ep_union_119,#moreJingXS").hide();
                }
            }
        });
}

funcGetEsc();
