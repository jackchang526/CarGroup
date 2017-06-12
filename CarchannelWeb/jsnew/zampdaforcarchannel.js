var _zpq = _zpq || [];
/*
demo
paramObj = {"CsID":2608,"CityID":201,"PageName":"zongshu"}
*/
function getDemandAndSentZampda(paramObj) {
	var csid = 0;
	var cityId = 0;
	var pageName = '';
	if (typeof paramObj.CsID != 'undefined') {
		csid = paramObj.CsID;
	}
	if (typeof paramObj.CityID != 'undefined') {
		cityId = paramObj.CityID;
	}
	if (typeof paramObj.PageName != 'undefined') {
		pageName = paramObj.PageName;
	}
	if (csid > 0 && cityId > 0 && pageName != '') {
		var interfaceForDemand = 'http://api.car.bitauto.com/mai/GetSerialDemand.ashx?serialId=' + csid + '&cityid=' + cityId;
		$.ajax({ url: interfaceForDemand, async: false, cache: true, dataType: "jsonp", jsonpCallback: "newDemandCallback", success:
			function (data) {
				var hasDemand = 0;
				if (typeof data != 'undefined' && data != null
				&& typeof data.DealerCount != 'undefined' && data.DealerCount != null && data.DealerCount > 0)
				{ hasDemand = 1; }

				_zpq.push(['_setPageID', '334']);
				_zpq.push(['_setPageType', 'modelPage']);
				_zpq.push(['_setParams', csid, cityId, '3-' + csid + '-0-' + cityId, pageName, hasDemand]);
				_zpq.push(['_setAccount', '12']);

				(function () {
					var zp = document.createElement('script'); zp.type = 'text/javascript'; zp.async = true;
					zp.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'cdn.zampda.net/s.js';
					var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(zp, s);
				})();
			}
		});
	}
}