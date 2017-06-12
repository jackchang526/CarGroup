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
function BtShow(id) {
	var thisColor = document.getElementById(id);
	thisColor.style.display = "block";
}
function BtHide(id) {
	var thisColor = document.getElementById(id);
	thisColor.style.display = "none";
}

function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

function tabsRemove(index, head, divs, div2s) {
	if (!document.getElementById(head)) return false;
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");
		for (var i = 0; i < alis.length; i++) {
			removeClass(alis[i], "current");
			hide(divs + "_" + i);
			if (div2s) { hide(div2s + "_" + i) };
			if (i == index) {
				addClass(alis[i], "current");
			}
		}
		show(divs + "_" + index);
		if (div2s) { show(div2s + "_" + index) };
	}
}

function tabs(head, divs, div2s, over) {
	if (!document.getElementById(head)) return false;
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");
		for (var i = 0; i < alis.length; i++) {
			alis[i].num = i;
			if (over) {
				alis[i].onmouseover = function () {
					var thisobj = this;
					thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
				}
				alis[i].onmouseout = function () {
					clearTimeout(thetabstime);
				}
			}
			else {
				alis[i].onclick = function () {
					if (this.className == "current" || this.className == "last current") {
						changetab(this);
						return true;
					}
					else {
						changetab(this);
						return false;
					}
				}
			}
			function changetab(thebox) {
				tabsRemove(thebox.num, head, divs, div2s);
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