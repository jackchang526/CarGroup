var current_province = 0, //默认选中
    current_city = 201;
var provinces;
var citys;
//设置城市cookies
function changeCity(id) {
	$.ajax({
		url: "http://ip.yiche.com/service/setcity.ashx", cache: true, data: { cityid: id }, dataType: "jsonp", jsonpCallback: "setCity", success: function (data) {
			if (data.city.regionid == id) {}
			document.location.reload();
		}
	});
}
$(function () {
	provinces = cityJsonV2.ProvinceList;
	citys = cityJsonV2.CityList;
	var childs = [];
	var $body = $('body');
	//二级连选  publicselect2 是二级连选控件
	$body.trigger('publicselect2',
        {
        	controlName1: 'publicswipe1', //调用公共一级控件
        	actionName1: '[data-action=province]', //绑定第一次属性
        	actionName2: '[data-action=city]', //绑定第二次属性
        	ds1: provinces,//第一层数据源
        	ds2: citys,//第二层数据源
        	templateid1: '#provinceTemplate', //第一层模板ID
        	templateid2: '#cityTemplate',// 第二层模板ID
        	snap1: 'dt,dd', //自动对齐标签
        	flatFn1: function (data) { //不修改数据源个格式这块删除
        		//修改第一层数据源格式 
        		return data;
        	},
        	flatFn2: function (data) { //不修改数据源个格式这块删除
        		//console.log(data)
        		//修改第二层数据源格式
        		return data;
        	},
        	fliterTemplate: function (templateid, f) {
        		//返回默认模板
        		return templateid;
        	},
        	fliterData: function (ds, f) {
        		var data = [];
        		for (var n in ds) { //筛选 第二层数据
        			if (n.toString() == f.$current.attr('data-id').toString()) {
        				data = ds[n];
        			}
        		}
        		return { citys: data };
        	},
        	fnEnd1: function (paras) {
        		//console.log('打开第一层后回调')
        		var $swipeLeft = this,
                    $leftPopupModels = $swipeLeft.parent(),
                    $back = $('.' + $leftPopupModels.attr('data-back'));
        		$swipeLeft.find('a.nbg').click(function (ev) {
        			var $click = $(this);
        			ev.preventDefault();
        			current_province = $click.attr('data-id');
        			//直辖市
        			var province = cityJsonV2.CityList[current_province];
        			var cityId = province[0].id;
        			//var hashObj = {};
        			//hashObj["city"] = cityId;
        			//window.location.hash = getHash(hashObj);
        			changeCity(cityId);
        			//window.location.reload();
        			$back.trigger('close');
        		});
        	},
        	fnEnd2: function (paras) {
        		//console.log('打开第二层后回调')
        		var $swipeLeft = this,
                    $leftPopupModels = $swipeLeft.parent(),
                    $back = $('.' + $leftPopupModels.attr('data-back')),
                    $a = $swipeLeft.find('.root a');
        		$a.html(paras.$current.html());

        		//返回按钮事件
        		$swipeLeft.find('.root a').click(function (ev) {
        			ev.preventDefault();
        			$back.trigger('close', { leftPopup: $leftPopupModels });
        		});

        		//选中按钮事件
        		$swipeLeft.find('li:not(.root) a').click(function (ev) {
        			ev.preventDefault();
        			var $current = $(this);
        			current_province = paras.$current.attr('data-id');
        			current_city = $current.attr('data-id');
        			//关闭层
        			//var hashObj = {};
        			//hashObj["city"] = current_city;
        			//window.location.hash = getHash(hashObj);
        			changeCity(current_city);
        			//window.location.reload();
        			$back.trigger('close');
        		});
        	}
        });
});