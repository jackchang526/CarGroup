//add format method to String
if (typeof String.prototype.format !== 'function') {
    String.prototype.format = function() {
        var args = arguments;
        return this.replace(/\{(\d+)\}/g,
        function(m, i) {
            return args[i];
        });
    }
}
//add format trim to String
if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function() {
        return this.replace(/^\s+|\s+$/, '');
    }
}

//Element not work in IE
if (typeof Element != 'undefined') {
    Element.prototype.hasClass = function(element, classname) {
        var css = element.className ? element.className : "";
        return css.match(new RegExp('(\\s|^)' + classname + '(\\s|$)'));
    };
}

var utility = {
    hasClass: function(element, classname) {
        var css = element.className ? element.className : "";
        return css.match(new RegExp('(\\s|^)' + classname + '(\\s|$)'));
    },
    addClass: function(element, classname) {
        var css = element.className ? element.className : "";
        if (!this.hasClass(element, classname)) element.className = css + " " + classname;
    },
    removeClass: function(element, classname) {
        var css = element.className ? element.className : "";
        if (this.hasClass(element, classname)) {
            var reg = new RegExp('(\\s|^)' + classname + '(\\s|$)');
            element.className = css.replace(reg, ' ');
        }
    },
    ready: function(fun) {
        var oldonload = window.onload;
        if (typeof window.onload != 'function') {
            window.onload = fun;
        }
        else {
            window.onload = function() {
                oldonload();
                fun();
            }
        }
    },
    getById: function(id) {
        return document.getElementById(id);
    },
    getByClass: function(classname, node, tag) {
        if (!node) {
            node = document.getElementsByTagName('body')[0];
        }
        var a = [], re = new RegExp('\\b' + classname + '\\b');
        els = node.getElementsByTagName(!tag ? '*' : tag);
        for (var i = 0, j = els.length; i < j; i++) {
            if (re.test(els[i].className)) {
                a.push(els[i]);
            }
        }
        return a;
    },
    next: function(node) {
        var o = null;
        if (node) {
            o = node.nextSibling;
            while (o != null && o.nodeType != 1) {
                o = o.nextSibling;
            }
        }
        return o;
    },
    prev: function(node) {
        var o = node.previousSibling;
        while (o.nodeType != 1) {
            o = o.previousSibling;
        }
        return o;
    },
    first: function(node) {
        var o = null;
        if (node) {
            o = node.firstChild;
            while (o && o.nodeType != 1) {
                o = o.nextSibling;
            }
        }
        return o;
    },
    last: function(node) {
        var o = null;
        if (node) {
            o = n.lastChild;
            while (o && o.nodeType != 1) {
                o = o.previousSibling;
            }
        }
        return y;
    },
    parent: function(node, tag, className) {
        var parent = null;
        if (node) {
            if (tag) {
                parent = node.parentNode;
                while (parent) {
                    if (parent.tagName && parent.tagName.toLowerCase() == tag.toLowerCase() && (!className || this.hasClass(parent, className)))
                        break;
                    parent = parent.parentNode;
                }
            }
            else {
                parent = node.parentNode;
            }
        }
        return parent;
    }
};

//from JQuery.cookie
utility.cookie = function(name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === null) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        // CAUTION: Needed to parenthesize options.path and options.domain
        // in the following expressions, otherwise they evaluate to undefined
        // in the packed version for some reason...
        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = null;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = cookies[i].trim();
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

var BrandTree = function(treeId, config) {
    this.$ = function(id) { return utility.getById(id); };
    config = config || {};
    config.saveState = config.saveState || false;
    config.enableCookie = config.enableCookie || false;
    config.letterBox = config.letterBox || 'tree_letter';
    config.cookieName = config.cookieName || 'frame_browse';

    var topbox = this.$("tree_box");
    var topboxHeight = topbox.offsetHeight;
    this.config = config;
    this.data = { node: "default", id: "0", text: "", spell: "" };
    this.links = typeof UrlStore != 'undefined' ? UrlStore : null || {};
    this.letterBox = this.$("tree_letter");
    this.context = utility.getById(treeId);
    this.charList = {};
    this.frameTop = 'topframe';
    this.frameRight = 'main';
    this.unselect = function() {//remove current selected
        var selectedlinks = utility.getByClass("current", this.context, "a");
        for (var i = 0; i < selectedlinks.length; i++) {
            utility.removeClass(selectedlinks[i], "current");
        }
    };
    this.select = function(nodeId) {
        if (!this.$(nodeId)) return false;
        var node = this.$(nodeId);
        this.unselect();
        utility.addClass(node, "current");
        var data = { node: node.getAttribute("name"), id: node.getAttribute("id"), text: node.getAttribute("title"), spell: node.getAttribute("allspell") };
        //this.saveState(data);
    };
    this.collapseAll = function() {//close all expanded node
        var items = document.getElementsByName("master");
        for (var i = 0; i < items.length; i++) {
            items[i].className = "";
            if (utility.next(items[i])) { utility.next(items[i]).className = "none"; }
        }
    };
    this.expandAll = function() {
        var items = document.getElementsByName("master");
        for (var i = 0; i < items.length; i++) {
            items[i].className = "open";
            if (utility.next(items[i])) { utility.next(items[i]).className = ""; }
        }
    };
    this.getFirstChild = function(node) {
        var result = null;
        var o = utility.next(node);
        if (o) {
            result = utility.first(o);
        }
        return result ? utility.first(result) : null;
    };
//     this.getCityLink = function(tab, data, orgurl, node, sender) {
//         var url = orgurl, spell = data.spell;
//         var cityCookie = utility.cookie('bitauto_framecity'); //for price and dealar
//         var firstChild = this.getFirstChild(node);
//         var childSpell = firstChild ? firstChild.getAttribute("allspell") : "";
//         //if (!this.links[tab] || !node || !firstChild) return url;
//         if (!this.links[tab] || !node) return url;
//         var urlbase = this.links[tab]["default"];
//         if (orgurl.substr(0, 7) == 'http://') {
//             var paths = orgurl.substr(7, orgurl.length).split("/");
//             if (paths.length >= 2) { urlbase = "http://" + paths[0]; }
//         }
//         var cityData = (cityCookie ? cityCookie : '').split(",");
//         if (cityData.length == 2 && cityData[0] != 0 && cityData[0].length > 0) {
//             if (tab == "price" || tab == 'baoyang') {
//                 if (orgurl.indexOf('citycode=') < 0)
//                     url = orgurl + (orgurl.indexOf('?') < 0 ? '?' : '&') + "citycode=" + cityData[0];
//                 else
//                     url = orgurl.replace(/citycode=(\d+|.?)/, "citycode=" + cityData[0]);
//             }
//             if (tab == "dealer") {
//                 if (node.name != "master") {
//                     url = urlbase + "/{0}/{1}/";
//                     url = url.format(cityData[1], spell);
//                 }
//                 else {
//                     if (firstChild) {
//                         url = urlbase + "/{0}/{1}/";
//                         url = url.format(cityData[1], childSpell);
//                     }
//                 }
//             }
//         }
//         if (tab == "dealer") {
//             if (node.name == "master") {
//                 if (firstChild && !sender) {
//                     url = urlbase + "/{0}/";
//                     url = url.format(childSpell);
//                 }
//                 else {
//                     url = orgurl;
//                 }
//             }
//         }
//         return url;
//     };
//     this.getLink = function(tab, data, sender) {
//         var node = data.node, id = data.id.replace("n", ""), text = data.text, spell = data.spell;
//         tab = tab || "default";
//         if (!this.links[tab] || !this.links[tab][node]) return this.links[tab]['default'];
//         var url = this.links[tab][node];
//         var selectedNode = this.$("n" + id);
//         url = url.format(id, encodeURIComponent(text), spell);
//         if (tab == 'price' || tab == 'dealer' || 'baoyang') {
//             url = this.getCityLink(tab, data, (sender ? sender.href : url), selectedNode, sender);
//         }
//         return url.toLowerCase();
//     };
//     this.setLink = function(data) {
//         if (!self.parent) return;
//         var frameTop = self.parent.frames[this.frameTop];
//         var frameMain = self.parent.frames[this.frameRight];
//         var names = '';
//         if (typeof tree_app != 'undefined') {
//             var current_left = document.getElementById('tab_' + tree_app);
//             if (current_left) current_left.parentNode.className = 'on';
//         }
//         if (frameMain && frameTop) {
//             var current_top = frameTop.document.getElementById('tab_' + tree_app);
//             var current_right = frameTop.document.getElementById('tab_' + tree_app);
//             if (current_top) current_top.parentNode.className = 'current';
//             if (current_right) current_top.parentNode.className = 'current';
//             //var node = data.node, id = data.id.replace('n', ''), text = data.text, spell = data.spell;
//             for (var k in this.links) {
//                 var url = this.getLink(k, data);
//                 var tabLeft = document.getElementById('tab_' + k);
//                 if (tabLeft) { tabLeft.href = url; tabLeft.target = '_parent'; /*if (url == this.links["dealer"]["default"]) { tabLeft.target = "_blank"; }*/ }
//                 var tabTop = frameTop.document.getElementById('tab_' + k);
//                 if (tabTop) { tabTop.href = url; /* tabTop.target = '_parent';if (url == this.links["dealer"]["default"]) { tabTop.target = "_blank"; }*/ }
//             }
//         }
//     };
    this.moveTo = function(rootId, expand, selecetedId) {
		//if (!this.$(rootId)) return false;
		//var root = this.$(rootId);
		//var offsetTop = root.offsetTop; //a offsettop
        //var scrollTop = offsetTop - topboxHeight; //different
		
		var topbox = this.$("tree_box");
    	var topboxHeight = topbox.offsetHeight;

        if (!this.$(rootId)) return false;
        var root = this.$(rootId);
        var offsetTop = root.offsetTop; //a offsettop
        var scrollTop = offsetTop - topboxHeight; //different
		var container = this.$('tree_content');
		container.scrollTop = offsetTop;
		
        if (expand == true) {//expand the root node
            utility.addClass(root, "open");
            if (utility.next(root)) { utility.next(root).className = ""; }
        }
        if (selecetedId) {//select current node
            this.select(selecetedId);
        }
    };
    this.removeSelection = function() {
        var masterLinks = document.getElementsByName("master");
        for (var j = 0; j < masterLinks.length; j++) {
            utility.removeClass(masterLinks[j].parentNode, 'series');
        }
    };
    this.charSwitch = function() {
        var self = this;
        if (!this.letterBox) return false;
        var charLinks = this.letterBox.getElementsByTagName("a");
        for (var i = 0; i < charLinks.length; i++) {
            charLinks[i].onclick = function() {
                var fchar = this.innerHTML;
                self.moveTo(self.charList[fchar], false, null); //scroll up
                var masterLinks = document.getElementsByName("master");
                var c = 0;
                for (var j = 0; j < masterLinks.length; j++) {
                    var spell = masterLinks[j].getAttribute("fchar");
                    if (spell == fchar) {
                        utility.addClass(masterLinks[j].parentNode, 'series');
                    }
                    else {
                        if (utility.hasClass(masterLinks[j].parentNode, 'series')) {
                            utility.removeClass(masterLinks[j].parentNode, 'series');
                        }
                    }
                }
                return false;
            }
        }
    };
//     this.saveState = function(data) {
//         var value = "";
//         if (data) {
//             value = '{node:"{0}",id:"{1}",text:"{2}",spell:"{3}"}';
//             value = value.format(data.node, data.id, encodeURIComponent(data.text), data.spell);
//             if (this.config.enableCookie == true) {
//                 utility.cookie(this.config.cookieName, value, { path: '/', domain: 'bitauto.com' });
//             }
//             else {
//                 this.setLink(data);
//             }
//         }
//     },
//     this.readState = function() {
//         if (this.config.enableCookie == true) {
//             var s = utility.cookie(this.config.cookieName);
//             var data = null;
//             try { data = eval("(" + s + ")"); } catch (e) { }
//             if (data && typeof ("tree_app") != 'undefined') {
//                 if (self.parent && self.parent.frames[this.frameRight]) {
//                     self.parent.frames[this.frameRight].document.location = this.getLink(tree_app, data);
//                 }
//             }
//         }
//         self.parent.frames[this.frameRight].src = this.getLink(tree_app, this.data);
//     }
}

BrandTree.prototype.search = function(id, keywords) {
    this.collapseAll();
    var data = null, node = null;
    if (parseInt(id) > 0) {
        node = this.$("n" + id);
    }
    else {
        if (keywords && keywords.trim().length > 0) {
            var links = this.context.getElementsByTagName("a");
            for (var i = 0; i < links.length; i++) {
                var text = links[i].getAttribute("title");
                if (text.trim().toLowerCase() == keywords.trim().toLowerCase()) {
                    node = links[i];
                    break;
                }
            }
        }
    }
    if (node) {
        data = { node: node.getAttribute("name"), id: node.getAttribute("id"), text: node.getAttribute("title"), spell: node.getAttribute("allspell") };
        if (data.node != "master") {
            var li = utility.parent(node, "li", "root");
            if (li) {
                var firstNode = utility.first(li);
                if (firstNode && firstNode.name == 'master') {
                    var rootId = firstNode.getAttribute("id");
                    this.moveTo(rootId, true, data.id);
                }
            }
        }
        else {
            this.moveTo(data.id, true, data.id);
        }
    }
//     else {
//         this.setLink(this.data);
//     }
}

// BrandTree.prototype.selectTab = function() {
//     //设置标签选中
//     var frameLeft = self;
//     var frameTop = self.parent.frames[this.frameTop];
//     var frameMain = self.parent.frames[this.frameRight];
//     if (frameLeft && frameTop) {
//         if (typeof tree_app != 'undefined' && tree_app) {
//             try {
//                 var current_top = frameTop.document.getElementById('tab_' + tree_app);
//                 if (current_top) current_top.parentNode.className = 'current';
//                 var current_left = frameLeft.document.getElementById('tab_' + tree_app);
//                 if (current_left) current_left.parentNode.className = 'on';
//                 /*var current_right = frameMain.document.getElementById('tab_' + tree_app);
//                 if (current_right) current_right.parentNode.className = 'current';*/
//             }
//             catch (exp) {
//             }
//         }
//     }
// }

BrandTree.prototype.init = function()
{
	var self = this;
	//this.selectTab();
	if (!this.context) return;
	var icons = this.context.getElementsByTagName("em");
	for (var i = 0; i < icons.length; i++)
	{
		icons[i].onmouseover = function()
		{
			this.parentNode.onclick = function()//使节点可点击
			{
				return false;
			}
			this.parentNode.setAttribute("selectme", "false");
		};
		icons[i].onmouseout = function()
		{
			this.parentNode.onclick = null; //使节点不可点击
			this.parentNode.setAttribute("selectme", "true");
		};
	}
	var links = this.context.getElementsByTagName("a");
	for (var i = 0; i < links.length; i++)
	{
		//links[i].setAttribute("orgurl", links[i].href);
		if (links[i].name == 'master')
		{
			var spell = links[i].getAttribute("fchar");
			if (spell && spell.length > 0)
			{
				var firstChar = spell.substr(0, 1);
				if (!self.charList[firstChar])
				{
					self.charList[firstChar] = links[i].id;
				}
			}
		}
		links[i].onmousedown = function()
		{
			self.removeSelection();
			var att = this.getAttribute("selectme");
			var selected = (att == null ? "true" : att) == "true";
			self.unselect();
			if (selected) self.select(this.id);
			if (utility.hasClass(this, 'open') && !(utility.hasClass(this, "opened") || utility.hasClass(this, "last")))
			{
				utility.removeClass(this, "open");
				//if (selected) self.select(this.id);
				if (utility.next(this)) { utility.next(this).className = "none"; }
			}
			/*else if (utility.hasClass(this, "opened") || utility.hasClass(this, "last")) {
			self.select(this.id);
			}*/
			else
			{
				utility.addClass(this, "open");
				//if (selected) self.select(this.id);
				if (utility.next(this)) { utility.next(this).className = ""; }
			}

		};
		/*
		links[i].onclick = function() {
		var data = { node: this.getAttribute("name"), id: this.getAttribute("id"), text: this.getAttribute("title"), spell: this.getAttribute("allspell") };
		if (typeof tree_app != 'undefined' && (tree_app == "dealer" || tree_app == "price" || tree_app == 'baoyang')) {
		this.href = self.getLink(tree_app, data, this);
		}
		};
		*/
	}
	this.charSwitch();
	if (this.config.enableCookie == false)
	{
		if (typeof currentId != 'undefined' && parseInt(currentId.replace('n', '')) > 0)
		{
			var curId = currentId.replace('n', '');
			this.search(curId);
		}
// 		else
// 		{
// 			this.setLink(this.data);
// 		}
	}
	else
	{
		this.readState();
	}
}




//--装载页面中的树，并处理标签切换-------------------------------
var treeHtml = "";
var treeLoaded = false;
var curObjId = 0;
var curPageId = "";

function addLoadEvent(func)
{
	var oldonload = window.onload;
	if (typeof window.onload != 'function')
	{
		window.onload = func;
	} else
	{
		window.onload = function()
		{
			oldonload();
			func();
		}
	}
}

function showTree()
{
	var winW, winH;
	if (window.innerHeight)
	{ // all except IE
		winW = window.innerWidth;
		winH = window.innerHeight;
	} else if (document.documentElement && document.documentElement.clientHeight)
	{// IE 6 Strict Mode
		winW = document.documentElement.clientWidth;
		winH = document.documentElement.clientHeight;
	} else if (document.body)
	{ // other
		winW = document.body.clientWidth;
		winH = document.body.clientHeight;
	}

	var summaryTree = document.getElementById("summaryTree");
	var summaryWrap = document.getElementById("summaryWrap");
	if (!summaryTree) return false;
	if (!summaryWrap) return false;

	if (winW > 1250)
	{
		if (treeHtml.length == 0)
		{
			loadLeftTreeHtml();
			$("#summaryTree").bind("click", statisticClick);
		}
		else
		{
			summaryWrap.className = "summaryWrap";
			if (!treeLoaded)
			{
				summaryTree.innerHTML = treeHtml;
				//热门品牌，子品牌标签切换处理
				var brandTag = document.getElementById("brandTag");
				var HotbrandTag = document.getElementById("HotbrandTag");
				var HotSerialTag = document.getElementById("HotSerialTag");
				var nav_tree = document.getElementById("nav_tree");
				var hotMasterBrand = document.getElementById("hotMasterBrand");
				var hotSerial = document.getElementById("hotSerial");
				if (!brandTag) return false;
				if (!HotbrandTag) return false;
				if (!HotSerialTag) return false;
				if (!nav_tree) return false;
				if (!hotMasterBrand) return false;
				if (!hotSerial) return false;

				hotMasterBrand.style.display = "none";
				hotSerial.style.display = "none";

				if (!document.getElementById("tree_letter")) return false;
				var tree_letter = document.getElementById("tree_letter");

				brandTag.onmouseover = function()
				{
					this.className = "on";
					nav_tree.style.display = "block";
					HotbrandTag.className = "";
					HotSerialTag.className = "";
					hotMasterBrand.style.display = "none";
					hotSerial.style.display = "none";

					tree_letter.style.display = "block";
					treeContent.style.top = "65px";
					treeContent.style.height = (winH - 65) + "px";
				}
				HotbrandTag.onmouseover = function()
				{
					this.className = "on";
					brandTag.className = "";
					HotSerialTag.className = "";
					nav_tree.style.display = "none";
					hotMasterBrand.style.display = "block";
					hotSerial.style.display = "none";

					tree_letter.style.display = "none";
					treeContent.style.top = "29px";
					treeContent.style.height = (winH - 30) + "px";
				}
				HotSerialTag.onmouseover = function()
				{
					this.className = "on";
					HotbrandTag.className = "";
					brandTag.className = "";
					nav_tree.style.display = "none";
					hotMasterBrand.style.display = "none";
					hotSerial.style.display = "block";

					tree_letter.style.display = "none";
					treeContent.style.top = "29px";
					treeContent.style.height = (winH - 30) + "px";
				}
				//
				var config = { saveState: true, enableCookie: false, letterBox: 'tree_letter' };
				var tree_brand = new BrandTree("nav_tree", config);
				tree_brand.init();
				treeLoaded = true;
				summaryTree.style.display = "block";
				if (curObjId > 0)
					tree_brand.search(curObjId);
			}
			var treeContent = document.getElementById("tree_content");
			if (!treeContent) return;
			treeContent.style.height = (winH - 65) + "px";
			summaryTree.style.display = "block";
		}
	}
	else
	{
		summaryTree.style.display = "none";
		summaryWrap.className = "summaryWrapNormal";
	}
}

function statisticClick(event)
{
	var tagTypeDic = { "master": "650", "brand": "651", "serial": "652", "treehotbrand": "653", "treehotSerial": "654" };
	var srcEle = event.target;
	var srcType = "";
	var srcId = "";
	var srcTypeId = "";
	if (srcEle.tagName.toUpperCase() == "EM")
		return;
	if (srcEle.tagName.toUpperCase() == "A")
	{
		srcType = srcEle.getAttribute("stattype");
		if (srcEle.id.length > 0)
			srcId = srcEle.id.substr(3);
	}
	else
	{
		srcEle = srcEle.parentNode;
		srcType = srcEle.name;
		if(srcType!="master" && srcType != "brand")
			srcType = "serial";
		srcId = srcEle.id.substr(1);
	}
	srcTypeId = tagTypeDic[srcType];
	var frameUrl = "http://carstat.bitauto.com/weblogger/urlrecord.aspx?logtype=tabclick&tagid=@tagId@&currenttagid=@sourceId@&tagname=@tagName@&SerialId=@serialId@&m=@math@";
	frameUrl = frameUrl.replace(/@tagId@/g, srcTypeId);
	frameUrl = frameUrl.replace(/@tagName@/g, srcType);
	frameUrl = frameUrl.replace(/@sourceId@/g, curPageId);
	frameUrl = frameUrl.replace(/@serialId@/g, srcId);
	frameUrl = frameUrl.replace(/@math@/g, Math.random());
	
	var statFrame = document.getElementById("statisticsIframe");
	if (!statFrame)
	{
		statFrame = document.createElement("IFRAME");
		statFrame.id = "statisticsIframe";
		statFrame.style.display = "none";
		document.body.appendChild(statFrame);
	}
	statFrame.src = frameUrl;
}

//异步获取树形的HTML
function loadLeftTreeHtml()
{
	/*
	var treeUrl = "/LeftTree.aspx";
	var treeOptions =
	{
	parameters: "",
	method: "get",
	onSuccess: treeHtmlLoaded
	}
	new Ajax.Request(treeUrl, treeOptions);
	*/
	//JQuery 实现
	$.ajax({
	url: '/LeftTree.aspx',
	type: 'GET',
	dataType: 'html',
	timeout: 1000,
	success: treeHtmlLoaded
	});
}

//在页面中显示树形
function treeHtmlLoaded(res)
{
	//treeHtml = res.responseText;
	treeHtml = res;	//Jquery
	if (treeHtml.length == 0)
		return;
	showTree();
}

addLoadEvent(showTree);
window.onresize = showTree;
