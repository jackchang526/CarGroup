/// <reference path="jquery-1.4.1.min.js" />
/// <reference path="iscroll.js" />
/// <reference path="model.js" />
function isEmptyObject(obj) {
    for (var key in obj) {
        return false;
    }
    return true;
}
var SelectCarTool = {
	Price: ""
	, Level: 0					//0不限，1-10对应各级别
	, LevelName: { 1: '微型车', 2: '小型车', 3: '紧凑型车', 4: '中大型车', 5: '中型车', 6: '豪华车', 7: 'MPV', 8: 'SUV', 9: '跑车', 11: '面包车', 12: '皮卡', 63: '轿车', 13: '小型SUV', 14: '紧凑型SUV', 15: '中型SUV', 16: '中大型SUV', 17: '大型SUV',18:'客车' }		//级别名称
	, Displacement: ""
	, TransmissionType: 0		//0不限，1手动，2自动
    , TransmissionTypeName: { 1: "手动", 126: "自动", 32: "机械自动（AMT）", 2: "自动（AT）", 4: "手自一体", 8: "无级变速", 16: "双离合" }
    , DriveType: 0	//驱动方式
	, DriveTypeName: { 1: "前驱", 2: "后驱", 252: "四驱", 4: "全时四驱", 8: "分时四驱", 16: "适时四驱", 32: "智能四驱", 64: "四轮驱动", 128: "前置四驱" }
	, FuelType: 0	//燃料类型
    , Environment: ""
    , EnvironmentName: {"120":"国5","121":"国4","122":"国3"}
    , FuelTypeName: { 7: "汽油", 8: "柴油", 2: "油电混合", 16: "纯电动", 128: "插电混合", 256:"天然气" }
	//, BodyDoors: ""	//车门数
	, PerfSeatNum: ""	//座位数
	, IsWagon: 0	//是否旅行版
	, MoreCondition: {}   //存放url参数more对象，包含“配置”;   key-value :如果value为0代表是从“配置”中选择的项
	, MoreConditionName: new Array()
	, View: 0					//0默认大图显示，1列表显示
	, Sort: 0					//0默认显示关注高-低，1关注低-高，2按价格排列低-高，3价格高-低
	, Brand: 0					//0不限，1自主，2合资，3进口
	, BrandName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口" }
    , Country: 0
    , CountryName: { 2: "日系", 4: "德系", 8: "美系", 16: "韩系", 484: "欧系" }
	, BodyForm: 0				//0不限，1两厢及掀背，2三厢
	, BodyFormName: { 1: "两厢", 2: "三厢" }
	, Type: "car"
	, Domain: window.location.host
	, currentId: ""
	, MoreMaxNum: 28
	, MoreSelectNum: 0   //统计“配置”层选中项的数量 
    , MoreFilterNum: 0    //统计“更多”层选中项的数量 
    , pageSize:20
    , apiUrl: "http://select24.car.yiche.com/api/selectcartool/searchresult"
	//初始化页面显示
	, initPageCondition: function () {
		this.initShowDefault();
		this.initPrice();
		this.initLevel();
		this.initDisplacement();
		this.initTransmisstionType();
		this.initBodyForm();
		this.initMoreCondition();
		this.initBrandType();
		this.initMoreFilter();
		this.bindFilterClickEvent();
		this.initRightSwipe();
	    //获取数据列表
		this.GetSearchData();
	}
    , initRightSwipe: function () {
        var $body = $('body');
        var $car = $('#m-btn-filter ul li');
        $($car).each(function (index,item) {
            $(item).on("click", function (ev) {
                var curAction = $(this).attr("data-action");
                var $click = $(this);
                ev.preventDefault();

                var actionFlag = $click.data("actionflag");
                if(!actionFlag){
                    rightSwipe1($body, $click, curAction);
                }
                else{
                    rightSwipe2($body,$click,curAction);
                }
            });
        })
    }
	//默认展开条件
	, initShowDefault: function () {
		var hash = window.location.hash;
		switch (hash.toLowerCase()) {
			case "#sd":
				this.currentId = "btnPrice";
				$(".leftPopup").css("zIndex", 199);
				var button = SelectCarTool.returnPopupItemId(this.currentId);
				SelectCarTool.toggle($("#" + button));
				window.setTimeout(function () { $("#p_min").get(0).focus(); }, 500);
				break;
		}
	}
	//绑定条件点击事件
	, bindFilterClickEvent: function () {
	    var that = this;
		//var btnFilter = document.getElementById("m-btn-filter");
		//if (btnFilter) {
		//	var liList = btnFilter.getElementsByTagName("li");
		//	for (var i = 0; i < liList.length; i++) {
		//		//if (liList[i].className === "m-btn" || liList[i].className === "m-btn current" || liList[i].className === "sub m-btn")
		//		//    liList[i].onclick = this.filterClickEvent;
		//	}
		//}

		var btnPriceSubmit = document.getElementById("btnPriceSubmit");
		if (btnPriceSubmit) {
			addEvent(btnPriceSubmit, "click", function () {
				var minP = document.getElementById("p_min").value;
				var maxP = document.getElementById("p_max").value;
				if (((minP == "" || isNaN(minP) || parseInt(minP) < 0) && (maxP == "" || isNaN(maxP) || parseInt(maxP) < 0)))
					document.getElementById("p_alert").innerHTML = "价格不能为空。";
				else if (maxP != "" && parseInt(maxP) <= 0) { document.getElementById("p_alert").innerHTML = "请填写正确的价格区间。"; }
				else {
					if (minP != "" && parseInt(minP) > 0 && (maxP == "" || parseInt(maxP) <= 0)) {
						GotoPage("p" + minP + "-9999"); return false;
					}
					else if (maxP != "" && parseInt(maxP) > 0 && (minP == "" || parseInt(minP) <= 0)) {
						GotoPage("p0-" + maxP + ""); return false;
					}
					else if (minP != "" && parseInt(minP) > 0 && maxP != "" && parseInt(minP) > 0) {
						if (parseInt(maxP) > parseInt(minP)) {
							GotoPage("p" + minP + "-" + maxP + ""); return false;
						}
						else
							document.getElementById("p_alert").innerHTML = "请填写正确的价格区间。";
					}
				}
			}, true);
		}

		var checkList = $("#m-filter-condition-leftPopup .first-list li");
        //绑定复选框选择事件
		$(checkList).each(function (index, item) {
		    $(item).on("click", function (e) {
		        e.preventDefault();
		        var $curCheckBox = $($(this).find(".radio-box [id^='mcCheck']")[0]);
		        var curCheckNo = $curCheckBox.attr("id").replace("mcCheck", "");
		        if ($curCheckBox.hasClass("checked")) {
		            $curCheckBox.removeClass("checked");
		            delete that.MoreCondition[curCheckNo];
		        }
		        else {
		            $curCheckBox.addClass("checked");
		            that.MoreCondition[curCheckNo]=0;
		        }
		        //查找所有选中li的个数
		        var checkedCount = 0;
		        var allCheckedLi = $(checkList).find(".radio-box [id^='mcCheck']");
		        $(allCheckedLi).each(function (liIndex, liItem) {
		            if ($(liItem).hasClass("checked")) {
		                checkedCount++;
		            }
		        })
		        if (checkedCount > 0) {
		            $("#m-btn-condition").html("完成(" + checkedCount + ")");
		            $("#m-btn-condition-clear").addClass("btn-clear-selected");
		        }
		        else {
		            $("#m-btn-condition").html("完成");
		            $("#m-btn-condition-clear").addClass("btn-clear-selected");
		        }
		    });
		})

		$(".leftmask").click(function () {
			$(this).hide();
			$(".swipeLeft").removeClass("swipeLeft-block");
			$(".leftPopup").css("zIndex", 0);
			$(".leftPopup").hide();

		});

		var self = this;
        //"配置"层“完成”事件
		$("#m-btn-condition").on("click", function () {
		    var allCheckedLi = $(checkList).find(".radio-box [id^='mcCheck']");
		    $(allCheckedLi).each(function (liIndex, liItem) {
		        if ($(liItem).hasClass("checked")) {
		            var curCheckNo = $(liItem).attr("id").replace("mcCheck", "");
		            self.MoreCondition[curCheckNo] = 0;
		        }
		    })
		    self.GotoPage();
		})
		$$("m-btn-condition-clear").onclick = function () {
		    var allCheckedLi = $(checkList).find(".radio-box [id^='mcCheck']");
		    $(allCheckedLi).each(function (liIndex, liItem) {
		        if ($(liItem).hasClass("checked")) {
		            $(liItem).removeClass("checked");
		            self.MoreCondition = {};
		        }
		    })
			$$("m-btn-condition").innerHTML = "完成"
			$("#m-btn-condition-clear").removeClass("btn-clear-selected");
		}

	    //"更多"层“完成”事件
		$$("m-btn-more").onclick = function () {
			if (self.MoreFilterNum > 0) {
				$$("btnMore").childNodes[1].childNodes[1].innerHTML = "更多(" + self.MoreFilterNum + ")";
			}
			self.GotoPage();
		}
		$$("m-btn-more-clear").onclick = function () {
			SelectCarTool.DriveType = 0;
			SelectCarTool.FuelType = 0;
			SelectCarTool.Environment = "";
			//SelectCarTool.BodyDoors = "";
			SelectCarTool.PerfSeatNum = "";
			$$("btnDrive").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnFuel").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnEnvironment").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnDoor").childNodes[0].childNodes[1].innerHTML = "";
			$$("btnSeat").childNodes[0].childNodes[1].innerHTML = "";
			$(".morestyle").removeClass("current");

			$(".drivetype" + SelectCarTool.DriveType.toString()).addClass("current");
			$("#fueltype" + SelectCarTool.FuelType.toString()).addClass("current");
			//var doorNum = 0;
			//if (SelectCarTool.BodyDoors != "") {
			//	doorNum = SelectCarTool.BodyDoors.charAt(0);
			//}
			var seatNum = 0;
			if (SelectCarTool.PerfSeatNum != "") {
				seatNum = SelectCarTool.PerfSeatNum.charAt(0);
			}
			$("#doors" + doorNum.toString()).addClass("current");
			$("#seatnum" + seatNum.toString()).addClass("current");
			$$("m-btn-more").innerHTML = "完成";
			$("#m-btn-more-clear").removeClass("btn-clear-selected");
		}
	}

	//点击事件处理
	, filterClickEvent: function () {
		var id = this.id, prevClickElement;
		$(".leftPopup").css("zIndex", 199);

		var button = SelectCarTool.returnPopupItemId(id);
		SelectCarTool.toggle($("#" + button));
		SelectCarTool.currentId = id;
		window.scrollTo(0, 0);
	}
	//返回条件弹出层id
	, returnPopupItemId: function (cid) {
		var result = "";
		switch (cid) {
			case "btnPrice": result = "m-filter-price"; break;
			case "btnLevel": result = "m-filter-level"; break;
			case "btnLevelsuv": result = "m-filter-levelsuv"; break;
			case "btnBrandType": result = "m-filter-brandtype"; break;
			case "btnTransmisstionType": result = "m-filter-trans"; break;
			case "btnDisplacement": result = "m-filter-dis"; break;
			case "btnMore": result = "m-filter-more"; break;
			case "btnCondition": result = "m-filter-condition"; break;
			case "btnBodyform": result = "m-filter-body"; break;

			case "btnDrive": result = "m-filter-drive"; break;
			case "btnFuel": result = "m-filter-fuel"; break;
			case "btnDoor": result = "m-filter-door"; break;
			case "btnSeat": result = "m-filter-seat"; break;
		}
		return result;
	}
	//元素点击显示
	, toggle: function (elem) {
		var elemTemp = elem.get(0);
		var pop = $("#" + elemTemp.id + "-leftPopup");
		var mask = $("#m-filter-mask");
		if (pop && (elemTemp.style.display == "block" || elemTemp.className == "swipeLeft swipeLeft-block")) {
			pop.hide();
			elem.removeClass("swipeLeft-block");
			mask.hide();
		} else {
			pop.show();
			elem.addClass("swipeLeft-block");
			mask.show();
		}
	}
    , GotoPage: function () {
    	var queryString = this.GetSearchQueryString(0);
    	var toUrl = _SearchUrl;
    	if (queryString.length > 0)
    		toUrl += "?" + queryString;
    	location.href = toUrl;
    }
	//设置选中项不可用
	, setDisabled: function (elem) {
		if (!elem) return;
		//var info = elem.firstChild.innerHTML;
		//elem.innerHTML = "<a>" + info + "</a>";
	    //elem.className = "current";
		$(elem).addClass("current");
	}
	//初始化价格
	, initPrice: function () {
		var showName = "";
		switch (this.Price) {
			case "0-5":
				this.setDisabled($$("price1"));
				showName = "5万以下";
				break;
			case "5-8":
				this.setDisabled($$("price2"));
				showName = "5-8万";
				break;
			case "8-12":
				this.setDisabled($$("price3"));
				showName = "8-12万";
				break;
			case "12-18":
				this.setDisabled($$("price4"));
				showName = "12-18万";
				break;
			case "18-25":
				this.setDisabled($$("price5"));
				showName = "18-25万";
				break;
			case "25-40":
				this.setDisabled($$("price6"));
				showName = "25-40万";
				break;
			case "40-80":
				this.setDisabled($$("price7"));
				showName = "40-80万";
				break;
			case "80-9999":
				this.setDisabled($$("price8"));
				showName = "80万以上";
				break;
			default:
				if (this.Price != "") {
					showName = "自定义" + this.Price + "万";
					var arrayPrice = this.Price.split("-");
					if (arrayPrice.length == 2) {
						document.getElementById("p_min").value = arrayPrice[0];
						document.getElementById("p_max").value = arrayPrice[1];
					}
				}
				else
					this.setDisabled($$("price0"));
				break;
		}
		if (this.Price != "") {
			$$("btnPrice").childNodes[1].childNodes[1].innerHTML = showName;
			$$("btnPrice").className = "m-btn current";
		}
	}
	//初始化级别
	, initLevel: function () {
	    if (this.Level == "") 
	        this.Level = "0";
	    this.setDisabled($(".level" + this.Level));
		if (this.Level != "0") {
			$$("btnLevel").childNodes[1].childNodes[1].innerHTML = this.LevelName[this.Level];
			$$("btnLevel").className = "m-btn current";
			if (this.Level == "8" || this.Level == "13" || this.Level == "14" || this.Level == "15" || this.Level == "16" || this.Level == "17") {
				//$$("btnLevelsuv").childNodes[0].childNodes[1].innerHTML = this.LevelName[this.Level];
				if (this.Level == "8") {
					//$$("btnLevelsuv").childNodes[0].childNodes[1].innerHTML = "全部";
				}
			}
		}
	}
	//初始化车身
	, initBodyForm: function () {
		var str = "bodyform", v = this.BodyForm;
		if (this.IsWagon > 0) {
			str = "wagon";
			v = this.IsWagon;
		}
		var bodyFormEle = document.getElementById(str + v);
		if (bodyFormEle) {
			this.setDisabled(bodyFormEle);
		}
		if (this.IsWagon > "0") {
			$$("btnBodyform").childNodes[1].childNodes[1].innerHTML = "旅行版";
			$$("btnBodyform").className = "m-btn current";
		}
		else if (this.BodyForm != "0") {
			$$("btnBodyform").childNodes[1].childNodes[1].innerHTML = this.BodyFormName[this.BodyForm];
			$$("btnBodyform").className = "m-btn current";
		}
	}
	//初始化国别
	, initBrandType: function () {
	    this.setDisabled($$("brandType" + this.Brand.toString()));
	    this.setDisabled($$("countryType" + this.Country.toString()));
		if (this.Brand != "") {
			$$("btnBrandType").childNodes[1].childNodes[1].innerHTML = this.BrandName[this.Brand];
			$$("btnBrandType").className = "m-btn current";
		}
		if (this.Country != "") {
		    $$("btnBrandType").childNodes[1].childNodes[1].innerHTML = this.CountryName[this.Country];
		    $$("btnBrandType").className = "m-btn current";
		}
	}
	//初始化变速箱
	, initTransmisstionType: function () {
		this.setDisabled($$("trans" + this.TransmissionType.toString()));
		if (this.TransmissionType != "0") {
			$$("btnTransmisstionType").childNodes[1].childNodes[1].innerHTML = this.TransmissionTypeName[this.TransmissionType];
			$$("btnTransmisstionType").className = "m-btn current";
		}
	}
	//初始化排量
	, initDisplacement: function () {
		var showName = "";
		switch (this.Displacement) {
			case "0-1.3":
				this.setDisabled($$("dis1"));
				showName = "1.3L以下";
				break;
			case "1.3-1.6":
				this.setDisabled($$("dis2"));
				showName = "1.3-1.6L";
				break;
			case "1.7-2.0":
				this.setDisabled($$("dis3"));
				showName = "1.7-2.0L";
				break;
			case "2.1-3.0":
				this.setDisabled($$("dis4"));
				showName = "2.1-3.0L";
				break;
			case "3.1-5.0":
				this.setDisabled($$("dis5"));
				showName = "3.1-5.0L";
				break;
			case "5.0-9":
				this.setDisabled($$("dis6"));
				showName = "5.0L以上";
				break;
			default:
				this.setDisabled($$("dis0"));
				break;
		}
		if (this.Displacement != "") {
			$$("btnDisplacement").childNodes[1].childNodes[1].innerHTML = showName;
			$$("btnDisplacement").className = "m-btn current";
		}
	}
	//初始化配置选项
	, initMoreCondition: function () {
		var checkList = $("#m-filter-condition-leftPopup .first-list li");
		for (var key in this.MoreCondition) {
		    if (this.MoreCondition[key] == 0) {   //“配置”层处理选中
		        this.MoreSelectNum++;
		        var curCondtionCheckBox = $(checkList).find(".radio-box #mcCheck" + key);
		        if ($(curCondtionCheckBox) && $(curCondtionCheckBox) !== undefined) {
		            if (!$(curCondtionCheckBox).hasClass("checked")) {
		                $(curCondtionCheckBox).addClass("checked");
		            }
		        }
		    }
		}
		if (this.MoreSelectNum > 0) {
		    $$("btnCondition").childNodes[1].childNodes[1].innerHTML = "配置(" + this.MoreSelectNum + ")";
		    $$("m-btn-condition").innerHTML = "完成(" + this.MoreSelectNum + ")";
		    $$("btnCondition").className = "m-btn current";
		}
		else {
		    $("#m-btn-condition-clear").removeClass("btn-clear-selected");
		}
	}
    //初始化更多选项
    , initMoreFilter: function () {
    	this.initMore();
    	if (this.MoreFilterNum > 0) {
    		$$("btnMore").childNodes[1].childNodes[1].innerHTML = "更多(" + this.MoreFilterNum + ")";
    		$$("btnMore").className = "m-btn current";
    	}
    }
    , initMore: function () {
    	var total = 0;
    	if (this.DriveType && this.DriveType > 0) {
    		total++;
    		$$("btnDrive").childNodes[0].childNodes[1].innerHTML = this.DriveTypeName[this.DriveType];
    	}
    	else {
    		$$("btnDrive").childNodes[0].childNodes[1].innerHTML = "";
    	}

    	if (this.FuelType && this.FuelType > 0) {
    		total++;
    		$$("btnFuel").childNodes[0].childNodes[1].innerHTML = this.FuelTypeName[this.FuelType];
    	}
    	else {
    		$$("btnFuel").childNodes[0].childNodes[1].innerHTML = "";
    	}
    	if (this.Environment!="0" && this.Environment.length > 0) {
    	    total++;
    	    $$("btnEnvironment").childNodes[0].childNodes[1].innerHTML = this.EnvironmentName[this.Environment];
    	}
    	else {
    	    $$("btnEnvironment").childNodes[0].childNodes[1].innerHTML = "";
    	}

    	//if (parseInt(this.BodyDoors) > 0 && this.BodyDoors.length > 0) {
    	//	total++;
    	//	$$("btnDoor").childNodes[0].childNodes[1].innerHTML = this.BodyDoors + "门";;
    	//}
    	//else {
    	//	$$("btnDoor").childNodes[0].childNodes[1].innerHTML = "";
    	//}
    	if (parseInt(this.PerfSeatNum) > 0 && this.PerfSeatNum.length > 0) {
    		total++;
    		$$("btnSeat").childNodes[0].childNodes[1].innerHTML = this.PerfSeatNum + "座";
    		if (this.PerfSeatNum == "8-9999") {
    			$$("btnSeat").childNodes[0].childNodes[1].innerHTML = "7座以上";
    		}
    	}
    	else {
    		$$("btnSeat").childNodes[0].childNodes[1].innerHTML = "";
    	}

    	if (total > 0) {
    		$$("m-btn-more").innerHTML = "完成(" + total + ")";
    		this.MoreFilterNum = total;
    		$("#m-btn-more-clear").addClass("btn-clear-selected");
    	}
    	else {
    		$$("m-btn-more").innerHTML = "完成";
    		$("#m-btn-more-clear").removeClass("btn-clear-selected");
    	}
    	$(".morestyle").removeClass("current");
        //$("#drivetype" + this.DriveType.toString()).addClass("current");
    	$(".drivetype" + this.DriveType.toString()).addClass("current");
    	$("#fueltype" + this.FuelType.toString()).addClass("current");
    	//var doorNum = 0;
    	//if (this.BodyDoors != "") {
    	//	doorNum = this.BodyDoors.charAt(0);
    	//}
    	var seatNum = 0;
    	if (this.PerfSeatNum != "") {
    		seatNum = this.PerfSeatNum.charAt(0);
    	}
    	var envrionmentNum = "0";
    	if (this.Environment.length >0) {
    	    envrionmentNum = this.Environment;
    	}
    	$("#envrionmentType" + envrionmentNum).addClass("current");
    	//$("#doors" + doorNum.toString()).addClass("current");
    	$("#seatnum" + seatNum.toString()).addClass("current");
    }
	, SetMoreCondition: function (mcConditionStr) {
	    var more = [];
	    //特殊处理：环保标准(参数下划线不分开处理)
        if (mcConditionStr.indexOf("120") > -1) { this.Environment = "120"; mcConditionStr = mcConditionStr.replace(this.Environment, ""); }
        else if (mcConditionStr.indexOf("121") > -1) { this.Environment = "121"; mcConditionStr = mcConditionStr.replace(this.Environment, ""); }
        else if (mcConditionStr.indexOf("122") > -1) { this.Environment = "122"; mcConditionStr = mcConditionStr.replace(this.Environment, ""); }
	    if (mcConditionStr.length > 0) {
	        //一般处理
	        more =linetrim(mcConditionStr).split('_');
	    }
		for (var i = 0; i < more.length; i++) {
            if (more[i] == 279 || more[i] == 280 || more[i] == 281 || more[i] == 282 || more[i] == 283 || more[i] == 284) { //"更多"选项中 车门、座位对应的参数形式
		        //279:2座  280:4座  281:5座  282:6座  283:7座  284:7座以上
		        //初始化座位
		        switch (more[i])
		        {
		            case "279": this.PerfSeatNum = "2"; break;
                    case "280": this.PerfSeatNum = "4"; break;
                    case "281": this.PerfSeatNum = "5"; break;
		            case "282": this.PerfSeatNum = "6"; break;
		            case "283": this.PerfSeatNum = "7"; break;
		            case "284": this.PerfSeatNum = "8-9999"; break;
		            default: break;
		        }
		    }
		    else{
		        this.MoreCondition[more[i]] = 0;
		    }
		}
	}
	//获取请求参数
	, GetSearchQueryString: function (mode) {  //mode=0  处理url显示，隐藏多选；mode=1处理api接口请求，带上多选参数
		var qsArray = new Array();
		if (this.Price.length > 0)
			qsArray.push("p=" + this.Price);
		if (this.Level != 0)
			qsArray.push("l=" + this.Level.toString());
		if (this.Displacement.length > 0)
			qsArray.push("d=" + this.Displacement);
		if (this.TransmissionType != 0)
		    qsArray.push("t=" + this.TransmissionType.toString());

		var mc = this.MoreCondition;
		var more = '';
		if (!isEmptyObject(mc)) {
		    for (var key in mc)
		    {
		        more += key + "_";
		        if (mode == 1) {
		            //天窗形式-单天窗、双天窗、全景
                    if (key.indexOf("207") > -1) {
		                more += "208_209_";
                    }
                    //自动空调-前排自动空调;双温区自动空调
                    else if (key.indexOf("244") > -1) {
                        more = more + "245_";
                    }
                    else if (key.indexOf("246") > -1) {
                        more = more + "247_248_";
                    }
		        }
		    }
		    more = more.substr(0, more.length - 1);
		}
		if (parseInt(this.BodyDoors) > 0 && this.BodyDoors.length > 0) {
		    if (more.length > 0)
		    { more += "_"; }
		    if (this.BodyDoors.indexOf('2-3') > -1) {
		        more += "268_";    //处理url
		        if (mode == 1) {    //处理api接口参数
		            more += "269_";
		        }
		    }
		    if (this.BodyDoors.indexOf('4-6') > -1) {
		        more += "270_";
		        if (mode == 1) {
		            more += "271_272_";
		        }
		    }
		    more = more.substr(0, more.length - 1);
		}
		if (this.PerfSeatNum && this.PerfSeatNum.length > 0) {
		    if (more.length > 0) {
		        more += "_";
		    }
		    if (this.PerfSeatNum.indexOf('2') > -1) {
		        more += "279_";
		    }
		    if (this.PerfSeatNum.indexOf('4') > -1) {
		        more += "280_";
            }
            if (this.PerfSeatNum.indexOf('5') > -1) {
                more += "281_";
            }
		    if (this.PerfSeatNum.indexOf('6') > -1) {
		        more += "282_";
		    }
		    if (this.PerfSeatNum.indexOf('7') > -1) {
		        more += "283_";
		    }
		    if (this.PerfSeatNum.indexOf('8-9999') > -1) {
		        more += "284_";
		    }
		    more = more.substr(0, more.length - 1);
		}
        //添加环保标准
		if (this.Environment.length > 0 && this.Environment!="0") {
		    if (more.length > 0) {
		        more += "_";
		    }
		    more += this.Environment+"_";
		    more = more.substr(0, more.length - 1);
		}
		if (more.length > 0) {
		    qsArray.push("more=" + more);
		}

		if (this.View != 0)
			qsArray.push("v=" + this.View.toString());
		if (this.Sort != 0)
			qsArray.push("s=" + this.Sort.toString());
		if (this.Brand != 0)
		    qsArray.push("g=" + this.Brand.toString());
		if (this.Country != 0)
		    qsArray.push("c=" + this.Country.toString());
		if (this.BodyForm != 0)
			qsArray.push("b=" + this.BodyForm.toString());
		if (this.IsWagon && this.IsWagon == 1)
			qsArray.push("lv=" + this.IsWagon);
		if (this.DriveType && this.DriveType > 0)
			qsArray.push("dt=" + this.DriveType);
		if (this.FuelType && this.FuelType > 0)
		    qsArray.push("f=" + this.FuelType);

		
		return qsArray.join('&');
	}
    , GetSearchData: function () {
        var that = this;
        var apiQueryString = this.GetSearchQueryString(1);
        var url = this.apiUrl;
        if (apiQueryString.length > 0) {
            url += "?external=Car.m&" + apiQueryString;
        }
        //获取当前排序方式：
        var order = 4;
        var urlParamsDict = that.parseUrlParameters(apiQueryString);
        order = (urlParamsDict['s'] == undefined) ? 4 : urlParamsDict['s'];

        $.ajax({
            url: url,
            dataType: "jsonp",
            jsonpCallback: "jsonpCallback",
            cache: true,
            success: function (json) {
                var result = json;
                if (typeof listSerialAD != "undefined" && listSerialAD.length > 0) {
                    if (result.ResList.length > 0) {
                        result.ResList = GetAdPosition(json);
                    }
                }
                DrawUlContent(result,order);
                var pageCount = Math.ceil(json.Count/ that.pageSize);
                that.bindPageEvent(pageCount);
            }
        });
    }
    , bindPageEvent: function (pageCount) {
        var that = this;
        //当存在车款时，才处理分页
        if (pageCount > 0) {
            $(".m-pages").css("display", "");
            $("#totalPage").html(pageCount);
            var $prevBtn = $(".m-pages-pre");
            var $nextBtn = $(".m-pages-next");
            var $selectOption = $(".m-pages-select");
            if (pageCount > 1) {
                //下拉页列表
                var option='';
                for (var i = 0; i < pageCount; i++) {
                    var curOptNum = i + 1;
                    option += "<option>" + curOptNum + "</option>";
                }
                $(".m-pages-select").append(option);

                //Desc:处理“上一页”事件
                $prevBtn.on("click", function () {
                    var curPageNum = Number($("#curPageIndex").text());    //当前页数
                    if (curPageNum > 1) {
                        curPageNum -= 1;
                        var rqStr = that.GetSearchQueryString(1) + "&page=" + curPageNum;
                        that.getPageData(rqStr,curPageNum, function () {
                            //判断“上一页”可点击状态
                            if (curPageNum <= 1) {
                                $prevBtn.addClass("m-pages-none"); //设置不可点击
                            }
                            //判断“下一页”可点击状态
                            if ($nextBtn.hasClass("m-pages-none") && curPageNum < pageCount) {
                                $nextBtn.removeClass("m-pages-none");//设置可点击
                            }
                            //if (curPageNum == 1) {
                            //    addCarListManual(curPageNum);
                            //}
                        })
                    }
                    else { $prevBtn.addClass("m-pages-none"); }
                    
                });
                //处理“下一页”事件
                $nextBtn.on("click", function () {
                    var curPageNum = Number($("#curPageIndex").text());    //当前页数
                    if (curPageNum < pageCount) {
                        curPageNum += 1;
                        var rqStr = that.GetSearchQueryString(1) + "&page="+curPageNum;
                        that.getPageData(rqStr, curPageNum, function () {
                            //判断“下一页”可点击状态
                            if (curPageNum >= pageCount) {
                                $nextBtn.addClass("m-pages-none");
                            }
                            //判断“上一页”可点击状态
                            if ($prevBtn.hasClass("m-pages-none") && curPageNum > 1) {
                                $prevBtn.removeClass("m-pages-none");
                            }
                        });
                    }
                    else { $nextBtn.addClass("m-pages-none"); }
                });

                //处理下拉列表页数点击事件
                $selectOption.change(function () {
                    var curOptionVal = $(this).val();
                    curPageNum = curOptionVal;

                    //判断“上一页”可点击状态
                    if (curPageNum < 2) { $prevBtn.addClass("m-pages-none"); }
                    else if ($prevBtn.hasClass("m-pages-none")) { $prevBtn.removeClass("m-pages-none"); }
                    //判断“下一页”可点击状态
                    if (curPageNum >= pageCount) { $nextBtn.addClass("m-pages-none"); }
                    else if ($nextBtn.hasClass("m-pages-none")) { $nextBtn.removeClass("m-pages-none"); }                   
                    var rqStr = that.GetSearchQueryString(1) + "&page=" + curPageNum;
                    that.getPageData(rqStr, curPageNum, function () {
                        //if (curPageNum == 1) {
                        //    addCarListManual(curPageNum);
                        //}
                    });                    
                });
            }
            else {
                //如果结果列表页数只有一页，则“下一页”不可点
                $nextBtn.addClass("m-pages-none");
                //下拉框不可选
                $('.m-pages-select').prop("disabled", true);
            }
        }
    }
    , getPageData: function (rqStr, curPageNum, callBack) {
        //获取当前排序方式：
        var order = 4;
        var urlParamsDict =this.parseUrlParameters(rqStr);
        order = (urlParamsDict['s'] == undefined) ? 4 : urlParamsDict['s'];
        $.ajax({
            url: this.apiUrl+ "?external=Car.m&" + rqStr,
            dataType: "jsonp",
            jsonpCallback: "jsonpCallback",
            cache: true,
            success: function (json) {
                var result = json;
                $("#curPageIndex").text(curPageNum);
                if (curPageNum==1&&typeof listSerialAD != "undefined" && listSerialAD.length > 0) {
                    result.ResList = GetAdPosition(json);
                }
                DrawUlContent(result,order);
                callBack();
				//返回顶部
            	//$(document).scrollTop($(".searchResult").offset().top);
                $(document).scrollTop(0);
            },
            error: function (xhr) {
            }
        });
    }
    , parseUrlParameters: function (rqStr) {
        var pattern = /(\w+)=(\w+)/ig;//定义正则表达式 
        var parames = {};//定义数组 
        rqStr.replace(pattern, function (a, b, c) { parames[b] = c; });
        return parames;//返回这个数组. 
    }
}
function rightSwipe1($body,$click,curAction) {
    $body.trigger('fristSwipeOneNb', {  //fristSwipeOne 一级带按钮控件
        $swipe: $body.find('.' + curAction), //弹出浮层
        $click: $click, //点击对象
        fnEnd: function () {
            //层打开后回调
            var $leftPopup = this;
            //$leftPopup.find('.loading').hide();
            $leftPopup.find('.swipeLeft').show();
            //$leftPopup.find('.ap').show();
        },
        closeEnd: function () {
            //关闭层回调
            var $leftPopup = this;
            //$leftPopup.find('.loading').show();
            $leftPopup.find('.swipeLeft').hide();
            //$leftPopup.find('.ap').hide();
        }
    });
}
function rightSwipe2($body, $click, curAction) {
    $body.trigger('fristSwipeOne', {  //fristSwipeOne 一级带按钮控件
        $swipe: $body.find('.' + curAction), //弹出浮层
        $click: $click, //点击对象
        fnEnd: function () {
            //层打开后回调
            var $leftPopup = this;
            //$leftPopup.find('.loading').hide();
            $leftPopup.find('.swipeLeft').show();
            //$leftPopup.find('.ap').show();

            //二级
            $leftPopup.find('li').click(function (ev) {
                var $liclick = $(this);
                ev.preventDefault();
                var hasAction = $liclick.data("action");
                if (hasAction) {
                    rightSwipe1($body, $liclick, hasAction);
                }
            });
        },
        closeEnd: function () {
            //关闭层回调
            var $leftPopup = this;
            //$leftPopup.find('.loading').show();
            $leftPopup.find('.swipeLeft').hide();
            //$leftPopup.find('.ap').hide();
        }
    });
}
function DrawUlContent(result,sort) {
    var h = [];
    if(result.ResList.length >0)
    { 
        h.push("<div class=\"tt-first y2015\">");
        h.push("<ul class=\"tags-sub tags-sub-left\">");
        h.push("<li class=\""+(sort==4?"current":"arrow")+"\"><a href=\"javascript:GotoPage('s4');\">按关注度</a></li>");
        h.push("<li class=\"" + (sort == 3 ? "current" : "arrow") + "\"><a href=\"javascript:GotoPage('s3');\">最贵</a></li>");
        h.push("<li class=\"" + (sort == 2 ? "current" : "arrow") + "\"><a href=\"javascript:GotoPage('s2');\">最便宜</a></li>");
        h.push("</ul>");
        h.push("<span class=\"sub-tt-more\">共" + result.Count + "个车型</span>");
        h.push("</div>");

        h.push("<div class=\"buy-car\">");
        h.push("<ul>");
        $(result.ResList).each(function (index) {
            var serialUrl = "/" + result.ResList[index].AllSpell + "/";
            var shortName = result.ResList[index].ShowName.toString().replace("(进口)", "");
            var curSerialId = result.ResList[index].SerialId;
            var imageUrl = result.ResList[index].ImageUrl.toString().replace("_1.", "_6.");
            var priceRange = result.ResList[index].PriceRange;
            var isAdvertise = result.ResList[index].Pos==undefined?false:true;
            if (curSerialId == 1568) {
                shortName = "索纳塔八";
            }            
            h.push("<li>");
            h.push("<a href=\"" + serialUrl + "\" class=\"car\"><div class=\"img-box\"><img src=\"" + imageUrl + "\" />");
            if (isAdvertise)   //设置“特价”标签
                h.push("<i class=\"recommend\"></i>");
            h.push("</div><strong>" + shortName + "</strong>");
            h.push("<p><em>" + priceRange + "</em></p>");
            h.push("</a></li>");

        });
        h.push("</ul>");
        h.push("</div>");
    }
    else
    {
        h.push("<div class=\"tt-first y2015\"><ul class=\"tags-sub tags-sub-left\"><li><a>选车结果</a></li></ul></div>");
        h.push("<div class=\"wrap\"><div class=\"m-no-result\"><div class=\"face face-fail\"></div><dl><dt>未找到合适的车型！</dt><dd>请调整一下选车条件</dd></dl><div class=\"clear\"></div></div>");
        h.push("<a class=\"btn-one btn-gray\" style=\"border-top:1px solid #f2f2f2\" href=\"javascript:location.href=document.referrer\">返回</a></div>");
    }
    $(".searchResult").html(h.join(''));
    //$(function () { addCarListManual(); });
}
//广告
function GetAdPosition(json) {
    for (var j = 0; j < listSerialAD.length; j++) {
        var flag = false;
        //投放位置
        for (var index = 0; index < json.ResList.length; index++) {
            if (json.ResList[index].SerialId == listSerialAD[j].SerialId) {
                if (listSerialAD[j].Pos > 0) {
                    json.ResList.remove(json.ResList[index]);
                    json.ResList.splice(listSerialAD[j].Pos - 1, 0, listSerialAD[j]);
                    flag = true;
                }
                break;
            }
        }
        if (!flag) {
            json.ResList.splice(json.ResList.length - 1, 1);
            json.ResList.splice(listSerialAD[j].Pos - 1, 0, listSerialAD[j]);
        }
    }
    return json.ResList;
}

function $$(id) { return document.getElementById(id); }

function addEvent(elm, type, fn, useCapture) {
	if (elm.addEventListener) {
		elm.addEventListener(type, fn, useCapture);
		return true;
	} else if (elm.attachEvent) {
		var r = elm.attachEvent('on' + type, fn);
		return r;
	} else {
		elm['on' + type] = fn;
	}
}
function GotoPage(conditionStr) {
	if (conditionStr.length >= 1) {
		var conType = conditionStr.charAt(0);
		var conStr = conditionStr.substr(1);
		switch (conType) {
			case 'p':
				SelectCarTool.Price = conStr;
				break;
			case 'l':
				SelectCarTool.Level = parseInt(conStr);
				break;
			case 'd':
				SelectCarTool.Displacement = conStr;
				break;
			case 't':
				SelectCarTool.TransmissionType = parseInt(conStr);
				break;
			case 'v':
				SelectCarTool.View = parseInt(conStr);
				break;
			case 's':
				SelectCarTool.Sort = parseInt(conStr);
				break;
			case 'g':
                SelectCarTool.Brand = parseInt(conStr);
                SelectCarTool.Country = "";
				break;
			case 'b':
				//var tmp_bodyform = 0;
                //for (var key in SelectCarTool.BodyFormName) {
                //    var tempId = "bodyform" + key;
                //    var bodyform = document.getElementById(tempId);
				//	if (bodyform && bodyform.checked) {
				//		tmp_bodyform = parseInt(tmp_bodyform) + parseInt(key);
				//	}
				//}
                //SelectCarTool.BodyForm = tmp_bodyform;

                SelectCarTool.BodyForm = parseInt(conStr);
                SelectCarTool.IsWagon = 0;
				break;
			case 'm':
				SelectCarTool.SetMoreCondition();
                break;
            case 'c'://国别
                SelectCarTool.Country = parseInt(conStr);
                SelectCarTool.Brand = "";
                break;
		}
	}
	var queryString = SelectCarTool.GetSearchQueryString(0);
	var toUrl = _SearchUrl;
	if (queryString.length > 0)
		toUrl += "?" + queryString;
	window.location.href = toUrl;
}
function GotoPage1(conditionStr) {
    if (conditionStr.length >= 1) {
        var conType = conditionStr.substring(0,2);
        var conStr = conditionStr.substr(2);
        switch (conType) {
            case 'lv':
                SelectCarTool.IsWagon = conStr;
                SelectCarTool.BodyForm = 0;
                break; 
        }
    }
    var queryString = SelectCarTool.GetSearchQueryString(0);
    var toUrl = _SearchUrl;
    if (queryString.length > 0)
        toUrl += "?" + queryString;
    window.location.href = toUrl;
}

function GotoPageMore(conditionStr, value) {
	var btnType = "";
	if (conditionStr.length >= 1) {
		switch (conditionStr) {
			case 'dt':
				if (value != "-1") {
					SelectCarTool.DriveType = parseInt(value);
				}
				btnType = "m-filter-drive";
				break;
			case 'f':
				if (value != "-1") {
					SelectCarTool.FuelType = parseInt(value);
				}
				btnType = "m-filter-fuel";
				break;
			case 'bd':
				if (value != "-1") {
					SelectCarTool.BodyDoors = value;
				}
				btnType = "m-filter-door";
				break; 
			case 'sn':
				if (value != "-1") {
					SelectCarTool.PerfSeatNum = value;
				}
				btnType = "m-filter-seat";
				break;
		    case 'environment':
		        if (value != "-1") {
		            SelectCarTool.Environment = value;
		        }
		        btnType = "m-filter-environment";
		        break;
		}
	}
	var pop = document.getElementById(btnType);
	var pop1 = document.getElementById(btnType + "-leftPopup");
	if (pop && (pop.style.display == "block" || pop.className == "swipeLeft swipeLeft-block")) {
		pop.className = "swipeLeft";
		$$("m-filter-more").className = "swipeLeft swipeLeft-block";
		$$("m-filter-more-leftPopup").style.display = "block";
		pop1.style.display = "none";
	}
	if (value != "-1") {
		SelectCarTool.initMore();
	}
}
function GotoPageSuv(conditionStr, value) {
	var btnType = "m-filter-levelsuv"
	if (conditionStr.length >= 1 && conditionStr == "l") {
		var pop = document.getElementById(btnType);
		var pop1 = document.getElementById(btnType + "-leftPopup");
		if (pop && (pop.style.display == "block" || pop.className == "swipeLeft swipeLeft-block")) {
			pop.className = "swipeLeft";
			$$("m-filter-level").className = "swipeLeft swipeLeft-block";
			$$("m-filter-level-leftPopup").style.display = "block";
			pop1.style.display = "none";
		}
	}
}

/**
* 删除左右两端的-(横线)
*/
function linetrim(str)
{
    return str.replace(/(^_*)|(_*$)/g,'');
}
/*
function addCarListManual(curPageN) {
    var curPageNum;
    if (curPageN == null) {
        curPageNum = Number($("#curPageIndex").text())
    }
    else {
        curPageNum = curPageN
    }        
    var queryString = SelectCarTool.GetSearchQueryString(1);
    //按关注度，贵，便宜排序&&第一页 第四台车显示手动块  8到12万
    if (curPageNum == 1 && (queryString == "p=8-12" || queryString == "p=8-12&s=3" || queryString == "p=8-12&s=2" || queryString == "p=8-12&s=4")) {
        var contentHtml = $("#p_8to12").html();
        $(".buy-car li").each(function (index) {
            if (index == 3) {
                $(contentHtml).insertBefore(($(".buy-car li")[index]))
            }
        })
    }
    //按关注度，贵，便宜排序&&第一页 第四台车显示手动块 12-18万
    if (curPageNum == 1 && (queryString == "p=12-18" || queryString == "p=12-18&s=3" || queryString == "p=12-18&s=2" || queryString == "p=12-18&s=4")) {
        var contentHtml = $("#p_12to18").html();
        $(".buy-car li").each(function (index) {
            if (index == 3) {
                $(contentHtml).insertBefore(($(".buy-car li")[index]))
            }
        })
    }
    //按关注度，贵，便宜排序&&第一页 第四台车显示手动块 5-8万
    if (curPageNum == 1 && (queryString == "p=5-8" || queryString == "p=5-8&s=3" || queryString == "p=5-8&s=2" || queryString == "p=5-8&s=4")) {
        var contentHtml = $("#p_5to8").html();
        $(".buy-car li").each(function (index) {
            if (index == 3) {
                $(contentHtml).insertBefore(($(".buy-car li")[index]))
            }
        })
    }
    //按关注度，贵，便宜排序&&第一页 第四台车显示手动块  紧凑型
    if (curPageNum == 1 && (queryString == "l=3" || queryString == "l=3&s=3" || queryString == "l=3&s=2" || queryString == "l=3&s=4")) {
        var contentHtml = $("#l_3").html();
        $(".buy-car li").each(function (index) {
            if (index == 3) {
                $(contentHtml).insertBefore(($(".buy-car li")[index]))
            }
        })
    }
    //按关注度，贵，便宜排序&&第一页 第四台车显示手动块 suv
    if (curPageNum == 1 && (queryString == "l=8" || queryString == "l=8&s=3" || queryString == "l=8&s=2" || queryString == "l=8&s=4")) {
        var contentHtml = $("#l_8").html();
        $(".buy-car li").each(function (index) {
            if (index == 3) {
                $(contentHtml).insertBefore(($(".buy-car li")[index]))
            }
        })
    }
    //按关注度，贵，便宜排序&&第一页 第四台车显示手动块 mpv
    if (curPageNum == 1 && (queryString == "l=7" || queryString == "l=7&s=3" || queryString == "l=7&s=2" || queryString == "l=7&s=4")) {
        var contentHtml = $("#l_7").html();
        $(".buy-car li").each(function (index) {
            if (index == 3) {
                $(contentHtml).insertBefore(($(".buy-car li")[index]))
            }
        })
    }
}
*/