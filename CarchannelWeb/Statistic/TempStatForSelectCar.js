// 选车工具临时统计
var divIDForTempClickStat = "showhideCon";
if (document.getElementById(divIDForTempClickStat)) {
	try {
		var dls = document.getElementById(divIDForTempClickStat).getElementsByTagName("dl");
		for (var i = 0; i < dls.length; i++) {
			var aLinks = dls[i].getElementsByTagName("a");
			for (var j = 0; j < aLinks.length; j++) {
				document.all ? aLinks[j].attachEvent('onclick', statForSelectCarTempString) : aLinks[j].addEventListener('click', statForSelectCarTempString, false);
			}
		}
	}
	catch (err)
	{ }
}

function statForSelectCarTempString(ev) {
	ev = ev || window.event;
	var _eventSrc = ev.target || ev.srcElement;
	if (_eventSrc.innerHTML != "") {
		var _sentImg = new Image(1, 1);
		_sentImg.src = "http://carstat.bitauto.com/weblogger/urlrecord.aspx?logtype=temptypestring&objid=2&str2=&str1=" + encodeURIComponent(_eventSrc.innerHTML) + "&" + Math.random();
		return;
	}
}