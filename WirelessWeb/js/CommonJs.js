function addLoadEvent(func) {
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	} else {
		window.onload = function () {
			oldonload();
			func();
		}
	}
}
/*addClass*/
function addClass(element, value) {
	if (!element.className) {
		element.className = value;
	} else {
		newClassName = element.className;
		newClassName += " ";
		newClassName += value;
		element.className = newClassName;
	}
}
/*removeClass*/
function removeClass(element, value) {
	var removedClass = element.className;
	var pattern = new RegExp("(^| )" + value + "( |$)");
	removedClass = removedClass.replace(pattern, "$1");
	removedClass = removedClass.replace(/ $/, "");
	element.className = removedClass;
	return true;
}
function addEvent(elm, type, fn, useCapture) {
	if (!elm) return;
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
function hideElement(id) {
	var Div = document.getElementById(id);
	if (Div) {
		Div.style.display = "none";
	}
}
function showElement(id) {
	var Div = document.getElementById(id);
	if (Div) {
		Div.style.display = "block";
	}
}
function changeElementOrShow(idArray, splitChar) {
	var ids = idArray.split(splitChar || ',');
	if (ids != null && ids.length > 0) {
		for (var i = 0; i < ids.length; i++) {
			var id = ids[i];
			var Div = document.getElementById(id);
			if (Div) {
				if (Div.style.display == "none") {
					Div.style.display = "block";
				}
				else {
					Div.style.display = "none";
				}
			}
		}
	}
}
//异步加载JavaScript
var loadJS = {
	lock: false, ranks: []
		, callback: function (startTime, callback) {
			//载入完成
			this.lock = false;
			callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
			this.read(); //解锁，在次载入
		}
		, read: function () {
			//读取
			if (!this.lock && this.ranks.length) {
				var head = document.getElementsByTagName("head")[0];
				if (!head) {
					ranks.length = 0, ranks = null;
					throw new Error('HEAD不存在');
				}
				var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');
				this.lock = true;
				script.onload = script.onreadystatechange = function () {
					if (script && script.readyState && script.readyState != 'loaded' && script.readyState != 'complete') return;
					script.onload = script.onreadystatechange = script.onerror = null, script.src = ''
				, script.parentNode.removeChild(script), script = null; //清理script标记
					wc.callback(startTime, ranks.callback), startTime = ranks = null;
				};
				script.charset = ranks.charset || 'gb2312';
				script.src = ranks.src;
				head.appendChild(script);
			}
		}
		, push: function (src, charset, callback) {
			//加入队列
			this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
			this.read();
		}
}