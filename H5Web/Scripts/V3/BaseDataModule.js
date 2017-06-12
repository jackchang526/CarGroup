define("basedata", function (require) {

    function checkPage(pagename) {
        var index = Config.auchors.indexOf(pagename);
        if (index < 0) {
            $("div[data-anchor='" + pagename + "']").remove();
            return false;
        }
        return true;
    }

	//获取评测导购新闻
    function GetPingceNews() {
        if (!checkPage("page4")) {
            return;
        }
		$.get("/handlers/GetNewsList.ashx?top=10&csid=" + Config.serialId+"&", function (data) {
            
		    var length = data.length;
		    var obj = {};

		    obj["isCustomization"] = false;//是否为定制版
		    if (Config.currentCustomizationType!="User"||Config.isAd == 0) {
		        obj["isCustomization"] = true;
		    }

		    if (length > 0) {
		        if (length >= 3)
		            obj["isSecondAd"] = true;
		        else
		            obj["isSecondAd"] = false;

		        var listgroup = [];
				listgroup.push(data.slice(0, 3));
		        if (length > 3) {
		            listgroup.push(data.slice(3, 6));
		        }

		        if (length > 6) {
		            if (obj["isCustomization"] == true) {
		                listgroup.push(data.slice(6,9));
		            } else {
		                listgroup.push(data.slice(6));
		            }
		        }
		        
				obj["listgroup"] = listgroup;
			} else {
				obj["listgroup"] = [];
			}

		    $("#pingcenewstmp").tmpl(obj).appendTo("div[data-anchor='page4']");
		    var index = Config.auchors.indexOf('page4');
		    $("#fullpage").fullpage.resetSlides(index);

		    //如果只定制一页直接去掉向箭头
		    if (Config.auchors.length == 1) {
		        $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
		    }

		    if (obj["isCustomization"] == false) {
                //非定制版加广告
		        $("#div_29e5455f-c705-4489-bdd8-f24f9607267a").appendTo("#addaogoufirst").show();
		        $("#div_4d63ae1f-37bc-49e8-81c3-dd2fd040389a").appendTo("#addaogousecond").show();
		    } else {
		        //如果是定制版则移除广告标签
		        $("#addaogoufirst").remove();
		        $("#addaogousecond").remove();
		    }
		});
	}

	//亮点配置
	function GetPeizhi() {
	    if (!checkPage("page5")) {
	        return;
	    }
	    $.get("/handlers/GetSerialSparkle.ashx?top=11&csid=" + Config.serialId+"&", function (data) {
	        $("#peizhitmpl").tmpl({ "list": data }).appendTo("div[data-anchor='page5']");

	        //如果只定制一页直接去掉向箭头
	        if (Config.auchors.length == 1) {
	            $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
	        }
		});
	}

	GetPingceNews();
	GetPeizhi();
});