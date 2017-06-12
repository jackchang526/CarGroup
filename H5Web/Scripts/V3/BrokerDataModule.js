define("brokerdata", function (require) {
	$(".fixed_box").remove();

	function checkPage(pagename) {
	    var index = Config.auchors.indexOf(pagename);
	    if (index < 0) {
	        $("div[data-anchor='" + pagename + "']").remove();
	        return false;
	    }
	    return true;
	}

	function getBroker() {
	    if (!checkPage("page8")) {
	        return;
	    }
        $.get("/handlers/GetDataAsynV3.ashx?service=agent&method=brokerinfo&csid=" + Config.serialId + "&brokerid=" + Config.brokerId + "&type=0&", function(data) {
            if (checkData(data)) {
                $("div[data-anchor='page8']").html(data);
            } else {
                $("div[data-anchor='page8']").html("<div class='message-failure'><img src='http://img1.bitautoimg.com/uimg/4th/img2/failure.png' /> <h2>很遗憾！</h2> <p>数据抓紧完善中，敬请期待！</p> </div>");
            }

            //如果只定制一页直接去掉向箭头
            if (Config.auchors.length == 1) {
                $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
            }
        });
    }

	getBroker();

	//热门点评
	function getKouBei() {
	    if (!checkPage("page6")) {
	        return;
	    }
	    $.get("/handlers/GetEditorComment.ashx?csid=" + Config.serialId + "&", function (data) {
			$("#koubeitmpl").tmpl(data).appendTo("div[data-anchor='page6']");
			var index = Config.auchors.indexOf("page6");
			$("#fullpage").fullpage.resetSlides(index);

	        //如果只定制一页直接去掉向箭头
			if (Config.auchors.length == 1) {
			    $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
			}
		});
	}

	getKouBei();

	//获取车款列表
	function getCarList() {
	    if (!checkPage("page7")) {
	        return;
	    }
	    $.get("/handlers/getcarlist.ashx?top=19&csid=" + Config.serialId + "&", function (data) {
			if (!data) {
				return;
			}
			var length = data.carlist.length;
			var listGroup = [];
			var list = data.carlist;
			for (var i = 0; i < Math.ceil(length / 4) ; i++) {
				listGroup.push({ "index": i, "carlist": list.slice(i * 4, (i + 1) * 4) });
			}
			$("#agentcarlisttmp").tmpl({ "carcount": data.count, "listgroup": listGroup}).appendTo("div[data-anchor='page7']");
			var index = Config.auchors.indexOf("page7");
			$("#fullpage").fullpage.resetSlides(index);

	        //如果只定制一页直接去掉向箭头
			if (Config.auchors.length == 1) {
			    $("div[data-anchor='" + Config.auchors[0] + "']").find(".arrow_down").hide();
			}
		});
	}

	getCarList();

});