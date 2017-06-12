(function () {
	var testclass = function (e, t) {
		return (new RegExp("(?:^|\\s)" + t + "(?:\\s|$)", "i")).test(e.className);
	},
    changeclass = function (t, n) {
    	testclass(t, n) || (t.className = n.replace(/^\s+|\s+$/g, ""));
    },
    allclass = function (o, c) {
    	for (var i = 0; i < o.length; i++) {
    		changeclass(o[i], c);
    	}
    },
    getdata = function (e, callbackName, callbackFunc) {
    	var t = document.getElementsByTagName("head")[0] || document.documentElement,
            n = document.createElement("script"),
            r = false;
    	if (callbackName && callbackFunc instanceof Function) window[callbackName] = function (data) { callbackFunc(data); }
    	n.src = e,
        n.charset = "utf-8",
        n.onerror = n.onload = n.onreadystatechange = function () {
        	!r && (!this.readyState || this.readyState == "loaded" || this.readyState == "complete") && (r = true, t.removeChild(n));
        },
        t.insertBefore(n, t.firstChild);
    },
    addevent = function (elm, evType, fn) {
    	if (elm.addEventListener) {
    		elm.addEventListener(evType, fn, false); //DOM2.0
    	}
    	else if (elm.attachEvent) {
    		elm.attachEvent('on' + evType, function (e) {
    			fn.call(elm, e); //改变this指向
    		});
    	}
    	else {
    		elm["on" + evType] = fn;
    	}
    },
    listaddevent = function (elms, evType, fn) {
    	if (window.NodeList && elms instanceof NodeList || elms.length > 0 && elms[elms.length - 1]) {
    		for (var r = 0, s = elms.length; r < s; r++) {
    			addevent(elms[r], evType, fn);
    		}
    	}
    	else {
    		addevent(elms, evType, fn);
    	}
    },
    keycode = function (e) {
    	return e = e || window.event, e.which || e.keyCode || e.charCode;
    },
    stopDefault = function (e) {
    	e = e || window.event, e.preventDefault && e.preventDefault() || (e.returnValue = false);
    },
	extend = function () {
		var options, name, src, copy, copyIsArray, clone, target = arguments[0] || {}, i = 1, length = arguments.length, deep = false;
		if (typeof target === "boolean") {
			deep = target;
			target = arguments[1] || {};
			i = 2;
		}
		if (typeof target !== "object" && typeof target != "function") {
			target = {};
		}
		if (length === i) {
			target = this;
			--i;
		}
		for (; i < length; i++) {
			if ((options = arguments[i]) != null) {
				for (var name in options) {
					src = target[name];
					copy = options[name];
					if (target === copy) {
						continue;
					}
					if (deep && copy && (copy instanceof Object || (copyIsArray = copy instanceof Array)) && !(copy instanceof Function)) {
						if (copyIsArray) {
							copyIsArray = false;
							clone = src && src instanceof Array ? src : [];
						} else {
							clone = src && src instanceof Object ? src : {};
						}
						target[name] = extend(deep, clone, copy);
					} else if (copy !== undefined) {
						target[name] = copy;
					}
				}
			}
		}
		return target;
	},
	format = function () {
		if (arguments.length == 0)
			return null;
		var str = arguments[0], obj = arguments[1];
		for (var key in obj) {
			var re = new RegExp('\\{' + key + '\\}', 'gi');
			str = str.replace(re, obj[key]);
		}
		return str;
	},
	mathOffsetTop = function (curNode, id) {
		var topHeight = 0;
		if (!curNode)
			return topHeight;
		while (curNode && curNode.id != id) {
			topHeight += curNode.offsetTop;
			curNode = curNode.offsetParent;
		}
		return topHeight;
	},
	siblings = function (elem) {
		var r = [], n = elem.parentNode.firstChild;
		for (; n; n = n.nextSibling) {
			if (n.nodeType === 1 && n !== elem) {
				r.push(n);
			}
		}
		return r;
	},
	arrayIndexOf = function (array, value) { for (var i = 0, l = array.length; i < l; i++) if (array[i] == value) return i; return -1; },
	$$ = function (id) { return document.getElementById(id); },
	yicheAutoComplete = function (options) {
		return new yicheAutoComplete.prototype.init(options);
	};
	yicheAutoComplete.prototype = {
		datacache: {},
		init: function (options) {
			this.defaults = {
				keyWord: "sug_txtkeyword",
				btnSubmit: "sug_submit",
				hiddenCsUrl: "sugcsurl",
				sugContainer: "sug_container",
				selectOptions: { container: { master: "master-index", serial: "serial-index"} },
				randomSugKey: null,
				encode: "utf-8",
				callbackName: "yicheIndexSug",
				overclass: "current",
				outclass: "",
				sugSize: 10,
				interval: 100,
				urlPrefix: "http://59.151.102.96/yicheIndexSug.php?callback=yicheIndexSug&en=%encode%&d=" + new Date().getTime() + "&k="
			};
			extend(this.defaults, options);
			this.selectedIndex = -1;
			this.filteredValue = null;
			this.filteringValue = null;
			this.timer = null;
			this.isFocus = 0;
			this.defaults.callbackName = this.defaults.urlPrefix.match(/callback=(\w+)/)[1];
			this.input = $$(this.defaults.keyWord);
			this.sugdiv = $$(this.defaults.sugContainer);
			this.submit = $$(this.defaults.btnSubmit);
			this.csurl = $$(this.defaults.hiddenCsUrl);
			this.selectObj = yicheSelect(this.defaults.selectOptions);
			this.hide();
			this.bindEvents();
			return this;
		},
		highlight: function (value, term) {
			return value.indexOf(term) === 0 ? term + "<strong>" + value.substr(term.length) + "</strong>" : value;
		},
		refreshData: function (k) {
			var self = this;
			this.datacache[k] ? this.callback(this.datacache[k]) : getdata(this.defaults.urlPrefix.replace(/%encode%/, this.defaults.encode) + encodeURIComponent(k.replace(/\s+/g, '')), this.defaults.callbackName, function (data) {
				self.callback(data);
			});
		},
		onselectitem: function (e) {
			//e.setLog();
			e.input.form.submit();
			return false;
		},
		callback: function (t) {
			var e = this;
			if (!t.p || !t.s || !t.s[0]) {
				e.hide();
				return;
			}
			e.datacache[t.q] = t;
			e.filteredValue = e.filteringValue;
			var n = [];
			for (var i = 0, s = Math.min(t.s.length, e.defaults.sugSize); i < s; i++) {
				var csname = t.s[i].name;
				n.push('<li class="' + e.defaults.outclass + '" data-index="' + i + '" data-value="' + csname + '" data-hyperlink="' + t.s[i].url + '"><a href=\"javascript:;\">' + e.highlight(csname, e.filteredValue) + (t.s[i].url.length > 0 ? " <em>\u9891\u9053 &gt;</em>" : "") + '</a></li>');
			}
			n.length > 0 ? (e.sugdiv.innerHTML = "<ul class=\"sug_box\">" + n.join("") + "</ul>", e.bindEventsForDynamicDoms(), e.isFocus && e.show()) : (e.sugdiv.innerHTML = "", e.hide());
			e.selectedIndex = -1;
		},
		show: function () {
			this.sugdiv.style.display = "block";
		},
		hide: function () {
			this.sugdiv.style.display = "none";
		},
		bindEvents: function () {
			var e = this;
			addevent(e.input, "keydown", function (t) {
				var n = keycode(t);
				switch (n) {
					case 40:
						stopDefault(t);
						e.moveSelected(e.selectedIndex + 1);
						break;
					case 38:
						stopDefault(t);
						e.moveSelected(e.selectedIndex - 1);
						break;
					default:
						if (e.timer) clearTimeout(e.timer);
						e.timer = setTimeout(function () {
							e.checkInput(e);
						}, e.defaults.interval);
						break;
				}
			});
			addevent(e.input, "focus", function () {
				e.isFocus = 1, e.input.value = "";
				if (!e.selectObj.isShow() && e.sugdiv.style.display == "none") {
					e.selectObj.hide();
					e.selectObj.bindList();
				}
			});
			addevent(e.input, "blur", function () {
				if (e.input.value.length == 0) {
					if (e.defaults.randomSugKey && e.defaults.randomSugKey instanceof Function) e.defaults.randomSugKey();
				}
				e.isFocus = 0, clearTimeout(e.timer);
			});
			addevent(e.input, "input", function () {
				e.checkInput(e);
				e.selectObj.hide();
			}),
            addevent(e.submit, "click", function () {
            	e.csurl.value = "";
            	e.onselectitem(e);
            }),
            addevent(e.input.form, "submit", function () {
            	//e.setLog();
            	e.input.blur();
            	e.hide();
            	return true;
            }),
            addevent(window.document, "click", function (event) {
            	e.isFocus || (clearTimeout(e.timer), e.hide());
            	event = event || window.event;
            	var target = event.srcElement || event.target;
            	e.isFocus || (target && target.tagName == 'A') || e.selectObj.clear();
            });
		},
		bindEventsForDynamicDoms: function () {
			var e = this,
                r = e.sugdiv.getElementsByTagName("li");
			listaddevent(r, "mouseover", function () {
				e.selectedIndex > -1 && changeclass(r[e.selectedIndex], e.defaults.outclass), changeclass(this, e.defaults.overclass), e.selectedIndex = parseInt(this.getAttribute("data-index"));
			});
			listaddevent(r, "mouseout", function () {
				changeclass(this, e.defaults.outclass);
			});
			listaddevent(r, "click", function () {
				e.moveSelected(parseInt(this.getAttribute("data-index")));
				clearTimeout(e.timer);
				e.hide();
				e.onselectitem(e);
				return false;
			});
		},
		checkInput: function (e) {
			var s = e.input.value.replace(/\s+/g, '');
			e.csurl.value = "";
			e.input.value != "" ? e.selectObj.hide() : (e.selectObj.show(), e.sugdiv.innerHTML = '');
			if (s === e.filteringValue) return;
			//添加隐藏品牌
			e.filteringValue = s;
			e.refreshData(s);
		},
		moveSelected: function (t) {
			var e = this,
            txt = e.input,
            r = e.sugdiv.getElementsByTagName("li");
			r.length ? (e.selectedIndex > -1 && allclass(r, e.defaults.outclass), t = (t + r.length + 1) % (r.length + 1), t == r.length ? (txt.value = e.filteringValue, t = -1, e.setSubmit()) : (txt.value = r[t].getAttribute("data-value"), r[t].getAttribute("data-hyperlink").length > 0 ? e.setSubmit(r[t].getAttribute("data-hyperlink")) : e.setSubmit(""), changeclass(r[t], e.defaults.overclass))) : t = -1,
            e.selectedIndex = t;
		},
		setSubmit: function (l) {
			var e = this;
			e.csurl.value = l;
		},
		setLog: function () {
			var t = this;
			if (t.csurl.value.length > 0)
				(new Image()).src = "http://www.cheyisou.com/SearchLog.aspx?kw=" + encodeURIComponent(t.input.value) + "&iszhida=1&ct=-100&bc=7&rurl=" + encodeURIComponent(t.csurl.value) + "&refurl=" + encodeURIComponent(document.URL) + "&ts=" + (new Date()).getTime();
		}
	}
	yicheAutoComplete.prototype.init.prototype = yicheAutoComplete.prototype;
	yicheSelect = function (options) {
		return new yicheSelect.prototype.init(options);
	};
	yicheSelect.prototype = {
		dataCache: {},
		init: function (options) {
			this.defaults = {
				url: "http://api.car.bitauto.com/CarInfo/masterbrandtoserialforsug.ashx",
				container: { master: "master-index", serial: "serial-index" },
				datatype: 2,
				condition: "type={type}&pid={pid}&rt={rt}&callback={callback}",
				callbackName: "callback",
				callback: {},
				dvalue: {},
				dGoUrl: "http://car.bitauto.com/{allspell}/"
			};
			this.lock = false;

			this.parentDataCache = {};
			extend(true, this.defaults, options);
			return this;
		},
		bindList: function () {
			this.getData("master", 0);
		},
		getData: function (type, parentId) {
			var self = this;
			var cacheKey = type + "_" + parentId;
			if (typeof this.dataCache[cacheKey] != 'undefined') {
				self.callback(type, parentId, this.dataCache[cacheKey]);
				return;
			}
			var conditions = format(this.defaults.condition, { "pid": parentId, "type": "" + this.defaults.datatype + "", "rt": "" + type + "", "callback": "" + this.defaults.callbackName + "" });
			var url = this.defaults.url + "?" + conditions;
			getdata(url, this.defaults.callbackName, function (data) { self.callback(type, parentId, data); });
		},
		callback: function (type, parentId, data) {
			this.lock = false;
			var cacheKey = type + "_" + parentId;
			if (typeof this.dataCache[cacheKey] == 'undefined') {
				this.dataCache[cacheKey] = data;
			}
			this.fillContainer(type, data, parentId);
		},
		bindEvents: function (type) {
			var self = this, brand;
			if (type != "master")
				brand = $$(this.defaults.container[type]);
			else
				brand = $$(this.defaults.container[type] + "_list");
			var brandList = brand.getElementsByTagName("a");
			if (type == "master") {
				var letters = $$(this.defaults.container[type] + "_letters"),
				letterList = letters.getElementsByTagName("span");
				listaddevent(letterList, "click", function () {
					var elem = $$(self.defaults.container[type] + "letters_" + this.getAttribute("data-char"));
					//					this.className = "current";
					//					var n = siblings(this);
					//					for (var i = 0; i < n.length; i++) {
					//						n[i].className = "";
					//					}
					var top = mathOffsetTop(elem, self.defaults.container[type]);
					$$(self.defaults.container[type] + "_list").scrollTop = top;
				});
			}
			listaddevent(brandList, "click", function (e) {
				stopDefault(e);
				var pid = this.getAttribute("data-id"),
				allSpell = this.getAttribute("data-allspell");
				if (type == "master") {
					if (self.lock) return;
					self.lock = true;
					this.className = "current";
					//var n = siblings(this.parentNode);
					for (var i = 0; i < brandList.length; i++) {
						if (brandList[i] != this)
							brandList[i].className = "";
					}
					//父级信息设置
					self.parentDataCache[pid] = { id: pid, allSpell: allSpell };
					self.getData("serial", pid);
				} else {
					var gourl = format(self.defaults.dGoUrl, { "allspell": allSpell });
					self.hide();
					window.open(gourl, "", "", "")
				}
			});
		},
		hide: function () {
			this.showByType(null, false);
		},
		show: function () {
			this.showByType(null, true);
		},
		clear: function () {
			for (var type in this.defaults.container) {
				var c = $$(this.defaults.container[type]);
				if (c) { c.innerHTML = ''; c.style.display = "none"; }
			}
		},
		showByType: function (type, isShow) {
			for (var ttype in this.defaults.container) {
				if (!type || ttype == type) {
					var c = $$(this.defaults.container[ttype]);
					if (c && c.innerHTML.replace(/\s+/g, "") != "") c.style.display = isShow ? "block" : "none";
					if (type) break;
				}
			}
		},
		isShow: function () {
			var flag = false;
			for (var type in this.defaults.container) {
				var c = $$(this.defaults.container[type]);
				if (c && c.style.display != "none") { flag = true; break; }
			}
			return flag;
		},
		fillContainer: function (type, data, parentId) {
			var c = $$(this.defaults.container[type]), n = [];
			if (!c) return;
			if (type == "master")
				n = this.fillMaster(type, data, parentId);
			else
				n = this.fillSerial(type, data, parentId);
			c.innerHTML = n.join("");
			c.style.display = "block";
			this.bindEvents(type);
		},
		fillMaster: function (type, data, parentId) {
			var n = [], spell = "";
			if (data.CharList.length <= 0) return n;
			n.push("<div class=\"brand_letters\" id=\"" + this.defaults.container[type] + "_letters\">");
			for (var index in data.CharList) {
				n.push("<span data-char=\"" + data.CharList[index] + "\"><a href=\"javascript:;\">" + data.CharList[index] + "</a></span>")
			}
			n.push("</div>");
			data = data.DataList;

			n.push("<div class=\"brand_name_bg\">");
			n.push("<div class=\"brand_name\" id=\"" + this.defaults.container[type] + "_list\">");
			for (var i = 0; i < data.length; i++) {
				var tempSpell = data[i]["tSpell"].toUpperCase();
				if (spell != tempSpell) {
					n.push("</dl>");
					n.push("<dl id=\"" + this.defaults.container[type] + "letters_" + tempSpell + "\">");
				}
				n.push("<dd><a href=\"javascript:;\" data-id=\"" + data[i]["id"] + "\" data-allspell=\"" + data[i]["urlSpell"] + "\">" + tempSpell + " " + data[i]["name"] + "</a></dd>");
				spell = tempSpell;
			}
			if (data.length > 0) n.push("</dl>");
			n.push("</div>");
			n.push("</div>");
			return n;
		},
		fillSerial: function (type, data, parentId) {
			var n = [];
			if (data.length <= 0) return n;
			n.push("<h6><a data-id=\"" + this.parentDataCache[parentId].id + "\" data-allspell=\"" + this.parentDataCache[parentId].allSpell + "\" href=\"javascript:;\">全部车型</a></h6>");
			n.push("<div class=\"models_detail_bg\" style=\"height:330px;\">");
			n.push("<div class=\"models_detail\" id=\"" + this.defaults.container[type] + "_list\">");
			for (var j = 0; j < data.length; j++) {
				n.push("<dl>");
				n.push("<dt><a data-id=\"" + data[j]["gid"] + "\" data-allspell=\"" + data[j]["gspell"] + "\" href=\"javascript:;\" title=\"\">" + data[j]["gname"] + "</a></dt>");
				for (var i = 0; i < data[j].child.length; i++) {
					if (arrayIndexOf([3595, 3598, 3594, 3596, 3961, 4238, 4123, 4243], data[j].child[i]["id"]) != -1)
						n.push("<dd><a data-id=\"" + data[j].child[i]["id"] + "\" data-allspell=\"" + data[j].child[i]["urlSpell"] + "\" href=\"javascript:;\" title=\"" + data[j].child[i]["name"] + "\">" + data[j].child[i]["name"] + "</a></dd>");
					else
						n.push("<dd><a data-id=\"" + data[j].child[i]["id"] + "\" data-allspell=\"" + data[j].child[i]["urlSpell"] + "\" href=\"javascript:;\">" + data[j].child[i]["name"] + "</a></dd>");
				}
				n.push("</dl>");
			}
			n.push("</div>");
			n.push("</div>");
			return n;
		}
	}
	yicheSelect.prototype.init.prototype = yicheSelect.prototype;
	window.BitautoAutoComplete = yicheAutoComplete;
})();