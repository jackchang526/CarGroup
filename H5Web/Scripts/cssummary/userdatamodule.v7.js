var user = {

	//外观设计
	waiguansheji: function (name) {

		//console.log(name);

		$.ajax({
			type: "GET",
			url: "/handlers/GetImageAndVideoData.ashx?csid=" + Config.serialId + "&v=" + Config.version + "&",
			success: function (data) {
				if (data !== "" && data != null) {
					$("#picvideotmpl").tmpl({ "data": data, "position": { "6": "bd1", "7": "bd2", "8": "bd3" } }).appendTo("div[data-anchor='" + name + "']");
				} else {
					$("#nodatatmpl").tmpl({ title: "外观图片" }).appendTo("div[data-anchor='" + name + "']");
				}

				if (Config.isAd !== 0) {
					switch (Config.carlevel) {
						case "中大型车":
						case "中型车":
						case "跑车":
						case "豪华车":
							$("#div_cee3f43e-f9a3-45fc-bd68-f8b5579648b8").appendTo("#adimg").show();
							break;
						case "微型车":
						case "小型车":
						case "紧凑型车":
							$("#div_5ea2640f-91cb-4037-9d3e-f4ccbb97ee54").appendTo("#adimg").show();
							break;
						case "概念车":
						case "MPV":
						case "面包车":
						case "皮卡":
						case "其它":
							$("#div_d37f78af-2f63-40ad-b9d7-5f706d567d85").appendTo("#adimg").show();
							break;
						case "SUV":
							$("#div_1b8a4e51-f9c4-403b-954c-196cf3439194").appendTo("#adimg").show();
							break;
					}

					//$("#div_0ef1a167-d9c3-40aa-925f-a87bdd88d052").appendTo("#adimg").show();
				}

				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			},
			complete: function () {
				$("#video-pic-wall img").each(function () {
					$(this).height($(this).width() / 1.7857142857);
				});
			}
		});
	},

	//评测导购
	pingcedaogou: function (name) {

		//console.log(name);

		$.get("/handlers/GetNewsList.ashx?top=10&csid=" + Config.serialId + "&v=" + Config.version + "&", function (data) {
			var length = data.length;
			var obj = {};
			obj["isCustomization"] = false; //是否为定制版
			if (Config.currentCustomizationType !== "User" || Config.isAd === 0) {
				obj["isCustomization"] = true;
			}
			if (length > 0) {
				var listgroup = [];
				listgroup.push(data.slice(0, 3));
				if (length > 3) {
					if (obj["isCustomization"] === true) {
						listgroup.push(data.slice(3, 6));
					} else {
						listgroup.push(data.slice(3, 5));
					}
				}

				if (length > 6) {
					if (obj["isCustomization"] === true) {
						listgroup.push(data.slice(6, 9));
					} else {
						listgroup.push(data.slice(6));
					}
				}

				obj["listgroup"] = listgroup;
				$("#pingcetmp20160413").tmpl(obj).appendTo("div[data-anchor='" + name + "']");
				if (obj["isCustomization"] === false) {

					switch (Config.carlevel) {
						case "中大型车":
						case "中型车":
						case "跑车":
						case "豪华车":
							$("#div_07bdf8d9-56ed-46c3-90d1-76f83ddb1ceb").appendTo("#adfirst").show();
							$("#div_3345655b-89b3-46ba-a760-a27c1d365978").appendTo("#adsecond").show();
							$("#div_b19f03d5-1981-49f8-9483-0557ddccad1a").appendTo("#adthird").show();
							break;
						case "微型车":
						case "小型车":
						case "紧凑型车":
							$("#div_c4926a1b-d58b-4d9e-acea-96b13a22c54b").appendTo("#adfirst").show();
							$("#div_c3134d73-41d3-4424-ba7b-391d5dd8febe").appendTo("#adsecond").show();
							$("#div_f6eddf27-e231-4eab-ae59-89023915b4a0").appendTo("#adthird").show();
							break;
						case "概念车":
						case "MPV":
						case "面包车":
						case "皮卡":
						case "其它":
							$("#div_97386d39-9ab7-495a-bc55-29f9b79a4a74").appendTo("#adfirst").show();
							$("#div_4935c605-d771-43d8-9bd5-ee5af118cc50").appendTo("#adsecond").show();
							$("#div_0b749f3a-2d22-4ebd-aeb4-6b11910547f8").appendTo("#adthird").show();
							break;
						case "SUV":
							$("#div_6760b344-9455-45d2-b8b3-20337510bdfb").appendTo("#adfirst").show();
							$("#div_8e4a5453-dc8d-41dd-8cff-fd66d64db689").appendTo("#adsecond").show();
							$("#div_5203b234-ea46-4a60-b709-8d690b562bde").appendTo("#adthird").show();
							break;
					}


					//非定制版加广告

					//$("#div_29e5455f-c705-4489-bdd8-f24f9607267a").appendTo("#adfirst").show();

					//$("#div_5f3c0f43-0ff7-488d-9575-ca8712bd7727").appendTo("#adsecond").show();

					//$("#div_4d63ae1f-37bc-49e8-81c3-dd2fd040389a").appendTo("#adthird").show();

				}
			} else {
				$("#nodatatmpl").tmpl({ title: "评测导购" }).appendTo("div[data-anchor='" + name + "']");
			}
			var index = Config.auchors.indexOf(name);
			$("#fullpage").fullpage.resetSlides(index);
			//最后一页去掉向下箭头
			if (Config.auchors[Config.auchors.length - 1] === name) {
				$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
			}

		});
	},

	//亮点配置
	liangdianpeizhi: function (name) {

		//console.log(name);

		//$.get("/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId + "&v=" + Config.version + "&", function(data) {
		//    $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='" + name + "']");

		//    var index = Config.auchors.indexOf(name);
		//    $("#fullpage").fullpage.resetSlides(index);

		//    //最后一页去掉向下箭头
		//    if (Config.auchors[Config.auchors.length - 1] === name) {
		//        $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
		//    }
		//});


		$.ajax({
			type: "GET",
			url: "/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId + "&v=" + Config.version + "&",
			success: function (data) {
				$("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='" + name + "']");

				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);

				//最后一页去掉向下箭头
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			},
			complete: function () {

			}
		});
	},

	//热门点评
	remendianping: function (name) {

		//console.log(name);

		$.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&v=" + Config.version + "&", function (data) {

			if (typeof data != "undefined" && data !== "" && data != null) {
				$("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='" + name + "']");
			} else {
				$("#nodatatmpl").tmpl({ title: "网友评分" }).appendTo("div[data-anchor='" + name + "']");
			}

			var index = Config.auchors.indexOf(name);
			$("#fullpage").fullpage.resetSlides(index);

			//最后一页去掉向下箭头
			if (Config.auchors[Config.auchors.length - 1] === name) {
				$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
			}
		});
	},

	//车款列表
	chekuanliebiao: function (name) {

		//console.log(name);

		$.ajax({
			type: "GET",
			url: "/handlers/getcarlist.ashx?top=19&csid=" + Config.serialId + "&v=" + Config.version + "&",
			success: function (data) {
				if (!data) {
					return;
				}
				var length = data.carlist.length;
				var listGroup = [];
				var list = data.carlist;
				for (var i = 0; i < Math.ceil(length / 4) ; i++) {
					listGroup.push({ "index": i, "carlist": list.slice(i * 4, (i + 1) * 4) });
				}
				$("#carlisttmp").tmpl({ "carcount": data.count, "listgroup": listGroup, "taxtag": data.taxtag }).appendTo("div[data-anchor='" + name + "']");

				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);

				//最后一页去掉向下箭头
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			},
			complete: function () {
				if (typeof tele != "undefined" && tele != null && tele != "") {
					$(".car_price a").each(function (index, item) {
						var temphref = $(item).attr("href");
						$(item).attr("href", temphref + "&" + cspara);
					});
				}
				if (typeof getSubsidy != "undefined" && getSubsidy instanceof Function)
				{
					getSubsidy(Config.serialId, bit_locationInfo.cityId);
				}
			}
		});
	},

	//优惠购车
	youhuigouche: function (name) {

		//console.log(name);

		var WTmc_id = "";
		var tempVarid = util.GetQueryStringByName("WT.mc_id");
		if (tempVarid != null) {
			WTmc_id = tempVarid;
		}
		var WTmc_jz = "";
		var tempVarjz = util.GetQueryStringByName("WT.mc_jz");
		if (tempVarjz != null) {
			WTmc_jz = tempVarjz;
		}

		var youhuiCount = 0, existCount = 0;

		//if (WTmc_id && WTmc_id != "") {
		//    $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_id=" + WTmc_id);
		//} else if (WTmc_jz && WTmc_jz != "") {
		//    $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId + "&WT.mc_jz=" + WTmc_jz);
		//} else {
		//    $("#gouchelink").attr("href", "http://gouche.m.yiche.com/home/YiShuBang/?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId);
		//}

		var yhgc = {};

		//惠买车
		$.ajax({
			type: "GET",
			url: "http://www.huimaiche.com/api/GetCarSerialSaleDataNew.ashx?csid=" + Config.serialId + "&ccode=" + bit_locationInfo.cityId,
			async: true,
			cache: true,
			dataType: "jsonp",
			timeout: Config.timeout, //超时时间设置，单位毫秒
			jsonpCallback: "huimaiche",
			success: function (data) {
				if (typeof data["SaveMoney"] != "undefined" && data["SaveMoney"] != "" && typeof data["Description"] != "undefined" && typeof data["OrderUrl"] != "undefined" && data["OrderUrl"] != "") {
					existCount++;
					var huimaiche = {};
					huimaiche.SaveMoney = data["SaveMoney"];
					huimaiche.Description = data["Description"];

					var targetLink = "";
					if (WTmc_id && WTmc_id != "") {
						targetLink = data["OrderUrl"] + "&leads_source=H001001&tracker_u=269_ycdsj_" + WTmc_id;
					} else if (WTmc_jz && WTmc_jz != "") {
						targetLink = data["OrderUrl"] + "&leads_source=H001001&tracker_u=269_ycdsj_" + WTmc_jz;
					} else {
						targetLink = data["OrderUrl"] + "&leads_source=H001001&tracker_u=269_ycdsj";
					}
					huimaiche.OrderUrl = targetLink;

					yhgc.huimaiche = huimaiche;

				}
			},
			complete: function () {
				youhuiCount++;
			}
		});

		//商城
		$.ajax({
			type: "GET",
			url: "http://api.yichemall.com/forth/car/get?csId=" + Config.serialId + "&cityId=" + bit_locationInfo.cityId,
			async: true,
			cache: true,
			dataType: "jsonp",
			timeout: Config.timeout, //超时时间设置，单位毫秒
			jsonpCallback: "shangcheng",
			success: function (data) {
				if (typeof data["Price"] != "undefined" && typeof data["Description"] != "undefined" && typeof data["Url"] != "undefined") {
					existCount++;
					var yicheshangcheng = {};
					yicheshangcheng.Price = data["Price"];
					yicheshangcheng.Description = data["Description"];
					yicheshangcheng.Url = data["Url"];

					yhgc.yicheshangcheng = yicheshangcheng;
				}
			},
			complete: function () {
				youhuiCount++;
			}
		});

		//易车惠
		$.ajax({
			type: "GET",
			//url: "http://api.market.bitauto.com/MessageInterface/DynamicAds/GetHandler.ashx?name=Disiji&csid=" + Config.serialId + "&cityid=" + cityId,
			//url: "http://api.market.bitauto.com/MessageInterface/DynamicAds/GetHandler.ashx?name=disijiceshi&csid=" + Config.serialId + "&cityid=" + cityId,
			url: "http://api.market.bitauto.com/MessageInterface/DynamicAds/GetHandler.ashx?name=disijijiekou&csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId,
			async: true,
			cache: true,
			dataType: "jsonp",
			timeout: Config.timeout, //超时时间设置，单位毫秒
			jsonpCallback: "yichehui",
			success: function (data) {
				if (data["data"] != null && typeof data["data"]["OrderUrl"] != "undefined") {
					existCount++;
					var yichehui = {};
					if (typeof data["data"]["OrderUrl"] != "undefined") {
						yichehui.OrderUrl = data["data"].OrderUrl;
					}
					if (typeof data["data"]["Description"] != "undefined") {
						yichehui.Description = data["data"].Description;
					}
					if (typeof data["data"]["YCHTitle"] != "undefined") {
						yichehui.YCHTitle = data["data"].YCHTitle;
					}
					if (typeof data["data"]["Wenan1"] != "undefined") {
						yichehui.Wenan1 = data["data"].Wenan1;
					}
					if (typeof data["data"]["Wenan2"] != "undefined") {
						yichehui.Wenan2 = data["data"].Wenan2;
					}
					yhgc.yichehui = yichehui;
				}
			},
			complete: function () {
				youhuiCount++;
			}
		});

		//贷款
		$.ajax({
			type: "GET",
			url: "http://api.chedai.bitauto.com/api/other/GetDSJProducts?csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId,
			async: true,
			cache: true,
			dataType: "jsonp",
			timeout: Config.timeout, //超时时间设置，单位毫秒
			jsonpCallback: "daikuai",
			success: function (data) {
				if (data["Data"] != null && typeof data["Data"]["Downpayment"] != "undefined" && typeof data["Data"]["Feature"] != "undefined" && typeof data["Data"]["Monthpay"] != "undefined" && typeof data["Data"]["Url"] != "undefined") {
					existCount++;
					var yichedaikuai = {};
					yichedaikuai.Downpayment = data["Data"]["Downpayment"];
					yichedaikuai.Feature = data["Data"]["Feature"];
					yichedaikuai.Monthpay = data["Data"]["Monthpay"];
					yichedaikuai.Url = data["Data"]["Url"];

					yhgc.yichedaikuai = yichedaikuai;
				}
			},
			complete: function () {
				youhuiCount++;
			}
		});

		var timeFun = setInterval(function () {
			if (youhuiCount === 4) {
				clearInterval(timeFun);
				yhgc.existCount = existCount;
				yhgc.isAd = Config.isAd;
				$("#gouchetmplnew").tmpl(yhgc).appendTo("div[data-anchor='" + name + "']");

				if (Config.isAd !== 0) {

					switch (Config.carlevel) {
						case "中大型车":
						case "中型车":
						case "跑车":
						case "豪华车":
							$("#div_b8b469ed-3f9e-4fdc-a071-da063376531b").appendTo("#youhuiadwrapper").show();
							break;
						case "微型车":
						case "小型车":
						case "紧凑型车":
							$("#div_16f27dc1-e4d2-4ef5-9c2a-59af2c218d37").appendTo("#youhuiadwrapper").show();
							break;
						case "概念车":
						case "MPV":
						case "面包车":
						case "皮卡":
						case "其它":
							$("#div_e33c5435-2c95-449a-a2bf-cae50f75c024").appendTo("#youhuiadwrapper").show();
							break;
						case "SUV":
							$("#div_31a56d4b-db9f-45d2-9fec-02c77c97bbce").appendTo("#youhuiadwrapper").show();
							break;
					}

					//$("#div_655a7bd1-d2bb-48ad-bb54-7a0ab4e99a77").appendTo("#youhuiadwrapper").show();
				}

				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);
				//最后一页去掉向下箭头
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			}
		}, 200);
	},

	//经销商
	jingxiaoshang: function (name) {

		$.ajax({
			type: "GET",
			url: "/handlers/GetDataAsynV3.ashx?service=dealer&method=userdealerstaticmap&csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId + "&",
			success: function (data) {
				if (util.CheckData(data)) {
					var html = $.trim(data);
					var wrapper = $("div[data-anchor='" + name + "']");
					$("#userdealertmpl").tmpl({ html: html }).appendTo("div[data-anchor='" + name + "']");
					$("#map_detail,#map_img", wrapper).
                        attr("href", "/" + Config.allSpell + "/dealermap/" + bit_locationInfo.cityId);

					//$("#map_detail,#map_img", wrapper).attr("href", "/V3/Dealers/DealerMapDetial.aspx?csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId);
				} else {
					$("#userdealertmpl").tmpl({ html: "" }).appendTo("div[data-anchor='" + name + "']");
				}

				if (Config.isAd !== 0) {
					switch (Config.carlevel) {
						case "中大型车":
						case "中型车":
						case "跑车":
						case "豪华车":
							$("#div_ceb34ed5-6167-4ec2-9580-ce4ff147737f").appendTo("#addealer").show();
							break;
						case "微型车":
						case "小型车":
						case "紧凑型车":
							$("#div_f5441702-4e8b-49fe-bb2c-e685727aacfb").appendTo("#addealer").show();
							break;
						case "概念车":
						case "MPV":
						case "面包车":
						case "皮卡":
						case "其它":
							$("#div_40417e40-9d50-42af-ae6b-b50439f2fe99").appendTo("#addealer").show();
							break;
						case "SUV":
							$("#div_15e8def7-8738-4360-a85e-e0bcc69eec84").appendTo("#addealer").show();
							break;
					}

					//$("#div_868a7de4-8ada-493a-bdbf-72731da316e3").appendTo("#addealer").show();
				}

				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);

				//最后一页去掉向下箭头
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			},
			complete: function (xhr, status) {
				var dealerIds = "";
				$(".dealer-list .dealerphonecus").each(function () {
					dealerIds = dealerIds + $(this).attr("id") + ",";
				});
				try {
					mobile_getTel400(730, dealerIds);
				} catch (e) {
				}
			}
		});

	},

	//养护
	yanghu: function (name) {

		//console.log(name);

		$.get("/handlers/GetDataAsynV3.ashx?service=yanghu&method=yanghu&csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId + "&", function (data) {

			var html = data.replace(/\r\n/ig, "");
			if (html.length > 0 && util.CheckData(data)) {
				$("#yanghutmpl").tmpl({ html: html }).appendTo("div[data-anchor='" + name + "']");

				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);

				//最后一页去掉向下箭头
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			} else {
				//$("div[data-anchor='" + name + "']").remove();
				//$("#fullpage").fullpage.reBuild();
				$("#nodatatmpl").tmpl({ title: "车辆养护" }).appendTo("div[data-anchor='" + name + "']");
			}

		});
	},

	//看了还看
	kanlehaikan: function (name) {

		//console.log(name);

		//$.get("/handlers/SeeAgain.ashx?csid=" + Config.serialId + "&v=" + Config.version + "&", function(data) {
		//    if (!data) {
		//        return;
		//    }
		//    $("div[data-anchor='" + name + "']").html(data);
		//    var index = Config.auchors.indexOf(name);
		//    $("#fullpage").fullpage.resetSlides(index);
		//    //最后一页去掉向下箭头
		//    if (Config.auchors[Config.auchors.length - 1] === name) {
		//        $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
		//    }
		//});

		$.get("/handlers/SeeAgainAdLevel.ashx?csid=" + Config.serialId + "&level=" + Config.carlevel + "&v=" + Config.version + "&", function (data) {
			if (!data) {
				return;
			}
			$("div[data-anchor='" + name + "']").html(data);
			var index = Config.auchors.indexOf(name);
			$("#fullpage").fullpage.resetSlides(index);
			//最后一页去掉向下箭头
			if (Config.auchors[Config.auchors.length - 1] === name) {
				$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
			}
		});
	},

	//二手车
	ershouche: function (name) {

		//console.log(name);

		//$.get("/handlers/GetDataAsynV3.ashx?service=ershouche&method=ershouche&csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId + "&", function(data) {
		//    if (data != "" && data != null && util.CheckData(data)) {
		//        $("#ershouchetmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");

		//        var index = Config.auchors.indexOf(name);
		//        $("#fullpage").fullpage.resetSlides(index);

		//        //最后一页去掉向下箭头
		//        if (Config.auchors[Config.auchors.length - 1] === name) {
		//            $("div[data-anchor='" + name + "']").find(".arrow_down").hide();
		//        }

		//    } else {
		//        $("#nodatatmpl").tmpl({ title: "二手车" }).appendTo("div[data-anchor='" + name + "']");
		//    }
		//});

		$.ajax({
			type: "GET",
			url: "/handlers/GetDataAsynV3.ashx?service=ershouche&method=ershouche&csid=" + Config.serialId + "&cityid=" + bit_locationInfo.cityId + "&",
			success: function (data) {
				if (data != "" && data != null && util.CheckData(data)) {
					$("#ershouchetmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");

					var index = Config.auchors.indexOf(name);
					$("#fullpage").fullpage.resetSlides(index);

					//最后一页去掉向下箭头
					if (Config.auchors[Config.auchors.length - 1] === name) {
						$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
					}

				} else {
					$("#nodatatmpl").tmpl({ title: "二手车" }).appendTo("div[data-anchor='" + name + "']");
				}
			},
			complete: function () {
				$(".con_list_ul_ad .button50-l a").click(function () {
					MtaH5.clickStat('be1');
				});
				$(".con_list_ul_ad .button50-r a").click(function () {
					MtaH5.clickStat('be2');
				});
			}
		});
	},

	//保险
	baoxian: function (name) {

		//console.log(name);

		$.ajax({
			type: "GET",
			url: "/handlers/GetDataAsynV3.ashx?service=yixin&method=baoxian&",
			timeout: Config.timeout, //超时时间设置，单位毫秒
			success: function (data) {
				var html = data.replace(/\r\n/ig, "");
				if (html.length > 0 && util.CheckData(data)) {
					$("#baoxiantmpl").tmpl({ html: data }).appendTo("div[data-anchor='" + name + "']");
				} else {
					$("#nodatatmpl").tmpl({ title: "汽车保险" }).appendTo("div[data-anchor='" + name + "']");
				}
				var index = Config.auchors.indexOf(name);
				$("#fullpage").fullpage.resetSlides(index);

				//最后一页去掉向下箭头
				if (Config.auchors[Config.auchors.length - 1] === name) {
					$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
				}
			},
			complete: function (xhr, status) {
				switch (status) {
					case "success":
						break;
						//case "timeout":
						//case "error":
					default:
						$("#nodatatmpl").tmpl({ title: "汽车保险" }).appendTo("div[data-anchor='" + name + "']");
						var index = Config.auchors.indexOf(name);
						$("#fullpage").fullpage.resetSlides(index);
						//最后一页去掉向下箭头
						if (Config.auchors[Config.auchors.length - 1] === name) {
							$("div[data-anchor='" + name + "']").find(".arrow_down").hide();
						}
				}
			}
		});
	}

};