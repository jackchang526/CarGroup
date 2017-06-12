<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeftTreeJs.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.Tree.LeftTreeJs" %>
if(typeof CookieHelper == "undefined"){
	var CookieHelper = {
		trim: function (b) {
			return (b || "").replace(/^\s+|\s+$/g, "")
		},
		setCookie: function (b, d, a) {
			if (typeof d != "undefined") {
				a = a || {};
				if (d === null) {
					d = "";
					a.expires = -1
				}
				var c = "";
				if (a.expires && (typeof a.expires == "number" || a.expires.toUTCString)) {
					if (typeof a.expires == "number") {
						c = new Date;
						c.setTime(c.getTime() + a.expires * 24 * 60 * 60 * 1E3)
					} else c = a.expires;
					c = "; expires=" + c.toUTCString()
				}
				var e = a.path ? "; path=" + a.path : "",
        f = a.domain ? "; domain=" + a.domain : "";
				a = a.secure ? "; secure" : "";
				document.cookie = [b, "=", encodeURIComponent(d), c, e, f, a].join("")
			}
		},
		readCookie: function (b) {
			var d = null;
			if (document.cookie && document.cookie != "") for (var a = document.cookie.split(";"), c = 0; c < a.length; c++) {
				var e = this.trim(a[c]);
				if (e.substring(0, b.length + 1) == b + "=") {
					d = decodeURIComponent(e.substring(b.length + 1));
					break
				}
			}
			return d
		}
	}
}
var IE6 = !!window.ActiveXObject && !window.XMLHttpRequest;
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
function addLoadEvent(func)
{
	var oldonload = window.onload;
	if(typeof window.onload != 'function')
	{
        if(window.addEventListener)
        {
            window.addEventListener('load',func,false);
        }
        else
        {
            window.attachEvent('onload', func);  
        }
	}
	else
	{
		window.onload = function()
		{
    		oldonload();
    		func();
    	}
  	}
}

function addResizeEvent(func)
{
	var oldResize = window.onresize;
	if(typeof window.onresize != 'function')
	{
        if(window.addEventListener)
        {
            window.addEventListener('resize',func,false);
        }
        else
        {
            window.attachEvent('onresize', func);  
        }
	}
	else
	{
		window.onresize = function()
		{
    		oldResize();
    		func();
    	}
  	}
}


//导航更多
function showMore(){document.getElementById("bt_car_public_moreList").style.display = "block";}
function hideMore(){document.getElementById("bt_car_public_moreList").style.display = "none";}

function treeHref(curLitterNum)
{
	var hideItemAllHeight = 0;
	for(var i=1; i<curLitterNum; i++)
	{
		var hideItem = document.getElementById("letter" + i);
		if(!hideItem)
			continue;
		var hideItemHeight = hideItem.offsetHeight-1;
		hideItemAllHeight += hideItemHeight;
	}
	var treeBox = document.getElementById("treev1");//树
	treeBox.scrollTop = hideItemAllHeight;
}

<%if(needExpandMaster){ %>
	//异步加载JavaScript
	var loadJS = {
		lock: false, ranks: []
		, callback: function(startTime, callback) {
		//载入完成
		this.lock = false;
		callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
		this.read(); //解锁，在次载入
		}
		, read: function() {
		//读取
		if (!this.lock && this.ranks.length) {
		var head = document.getElementsByTagName("head")[0];

		if (!head) {
			ranks.length = 0, ranks = null;
			throw new Error('HEAD不存在');
		}

		var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');

		this.lock = true;

		script.onload = script.onreadystatechange = function() {
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
		, push: function(src, charset, callback) {
		//加入队列
		this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
		this.read();
		}
	}
//展开或关闭主品牌	
function expandMaster(sender,masterId)
{
	if(!sender || masterId <= 0)
		return;		
	//alert(sender.tagName + "-" + masterId);
	//查找父级<li>
	var masterRootLi = sender.parentNode;
	while(masterRootLi.tagName != "LI" && masterRootLi != null)
	{
		masterRootLi = masterRootLi.parentNode;
	}
	
	if(masterRootLi.tagName != "LI")
		return;
	
	var contentULs = masterRootLi.getElementsByTagName("UL");
	if(contentULs.length == 0)
	{
		var cityCode = '<%=cityCode%>';
		var keyWord = '<%=keyWord%>';
		//需要获取数据
		var dataUrl = "http://car.bitauto.com/interface/tree/lefttree.js?tagtype=<%=tagType %>&objid=" + masterId + "&expand=1";
		if(cityCode.length > 0)
			dataUrl += "&cityCode=" + cityCode;
		if(keyWord.length > 0)
			dataUrl += "&keyword=" + keyWord;
		loadJS.push(dataUrl,"uft-8",function(){firstExpandMaster(masterRootLi);});
	}
	else
	{
		//如果已经存在，判断是否展开，如果展开则关闭
		var conUL = contentULs[0];
		if(conUL.style.display == "none")
			conUL.style.display = "block";
		else
			conUL.style.display = "none";
	}
	
}

function firstExpandMaster(masterRootLi)
{
	if(masterContent == 'undefined')
		return;
	if(masterRootLi == null)
		return;
	masterRootLi.innerHTML = masterRootLi.innerHTML + masterContent;
}
<%} %>

<%if (needSetSearchParas)
  { %>
  
var CookieOper = {
    setCookie: function(name, value)
    {
		document.cookie = name + "=" + encodeURIComponent(value) + "; path=/;domain=.bitauto.com";
    }
    ,getCookie: function(name)
    {
        var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
        if (arr != null)
        {
        	return decodeURIComponent(arr[2]);
        }
        return null;
    }
    ,clearCookie: function(name)
    {
		if (CookieForSelectDate.getCookie(name))
		{
    		document.cookie = name + "=;path=/;domain=.bitauto.com"; 
		}
    }
};

//保存选车参数的Cookie
function setSearchCookie()
{
	var url = window.location.href;
	var paraPos = url.indexOf("?");
	if(paraPos < 0)
		return;
	var queryStr = url.substr(paraPos + 1);
	CookieOper.setCookie("treeSearchParameters",queryStr);
}

//设置标签的选车Url
function setSearchUrl(tagType,searchUrl)
{
	var queryStr = CookieOper.getCookie("treeSearchParameters");
	var tagEle = document.getElementById("treeNav_" + tagType);
	if(queryStr != null && tagEle != null)
	{
		tagEle.href = searchUrl.replace("@para@",queryStr);
	}
}
  
<%} %>

<%=SearchParametersScript%>
<%=SetUrlCityScript%>

<%if (needSetUrlCity)
  { %>
function SetTagUrlCity(tagType,tagUrl,otherPara)
{
	var cityId = '<%=cityId %>';
	var cityCode = '<%=cityCode%>';
	var keyWord = '<%=keyWord%>';
	var objId = '<%=objId %>';
	var allSpell = '<%=allSpell %>';
	var tagEle = document.getElementById("treeNav_" + tagType);
	if(tagEle)
	{
		tagUrl = tagUrl.replace("@objid@",objId);
		tagUrl = tagUrl.replace("@objspell@",allSpell);
		tagUrl = tagUrl.replace("@citycode@",cityCode);
		tagUrl = tagUrl.replace("@cityid@",cityId);
		tagUrl = tagUrl.replace("@keyword@",keyWord);
		if(otherPara != null && otherPara.length > 0 && cityId.length >0)
		{
			tagUrl += otherPara.replace("@cityid@",cityId);
			tagUrl = tagUrl.replace("@citycode@",cityCode);
		}
		tagEle.href = tagUrl;
	}
}

function modifyCrumbs(siteName, lastUrl, cityName) {
    var span = document.getElementById("treeCrumbs");    
    if (span) {        
        var originalHtml = span.innerHTML;
        var index = originalHtml.lastIndexOf('&gt;');    
        if (index > 0) {
            var lastText = originalHtml.substring(index + 4);
            originalHtml = originalHtml.substring(0, index + 4);          
            var linkHtml = "<a href=\"" + lastUrl + "\">" + lastText + "</a>";
            span.innerHTML = originalHtml + linkHtml + " > " + cityName;
                      
        }
    }
    else {
        var h1s = document.getElementsByTagName("h1");
        if (h1s.length > 0) {
            var h1 = h1s[0];
            var aTag = h1.parentNode;
            var divTag = aTag.parentNode;
            var linkHtml = "<a href=\"" + lastUrl + "\">" + siteName + "</a>";
            var span=document.createElement("span");
            span.setAttribute("id","treeCrumbs");
            span.setAttribute("class","treeCrumbs");
            span.innerHTML = linkHtml + " > " + cityName;        
            divTag.removeChild(aTag);
            divTag.appendChild(span);           
            
        }
    }
}
<%} %>
//计算当前节点到树形顶部距离
function MathCurrentTreeNodeTop(curNode){
	var topHeight = 0;
	if(!curNode)
		return topHeight;	
	while(curNode && curNode.id!="treev1")
	{
		topHeight += curNode.offsetTop;
		curNode = curNode.offsetParent;
	}
	return topHeight;
}
//滚动到指定位置
function scrollToCurrentTreeNode()
{
	var currentNode = document.getElementById("curObjTreeNode");
	
	var topHeight = MathCurrentTreeNodeTop(currentNode);
	var treeBox = document.getElementById("treev1");//树
	
	if(typeof CookieHelper == "undefined"){
		treeBox.scrollTop = topHeight;
		return;
	}
	var ScrollTreeNodeTop = CookieHelper.readCookie("ScrollTreeNodeTop");
	var isMaster = 0;
	if(currentNode){
		var child=currentNode.firstChild;
		if(child.className&&child.className.indexOf("mainBrand")!=-1){
			isMaster = 1;
		}
	}else{
		ScrollTreeNodeTop = 0;
	}
	//复制黏贴以及外部点击进来的树形链接
	if(isMaster == 0 && /(mb_|b_|sb_|master|brand|serial)/gi.test(document.referrer) && document.referrer != ""){
		topHeight = parseInt(ScrollTreeNodeTop);
	}
	treeBox.scrollTop = topHeight;
	if(topHeight > parseInt(treeBox.scrollTop)){
		var treeBottom = document.getElementById("tree-bottom");
		treeBottom.style.height = parseInt(treeBottom.style.height) + (topHeight-parseInt(treeBox.scrollTop)) +"px";
		treeBox.scrollTop = topHeight;
	}
	CookieHelper.setCookie("ScrollTreeNodeTop", parseInt(treeBox.scrollTop), { "expires": 360, "path": "/", "domain": "bitauto.com" });
}
//获取当前节点所在主品牌节点
function GetCurrentNodeMaster(curNode){
	if(!curNode){
		return null;
	}
	while(curNode){
		var child=curNode.firstChild;
		if(child.className&&child.className.indexOf("mainBrand")!=-1){
			break;  
		}
		curNode = curNode.parentNode;
	}
	return curNode;
}
//计算当前展开容器的高度
function MathCurrentNodeMasterHeight(curNode){
	var elemHeight = 0;
	if(!curNode){
		return elemHeight;
	}
	while(curNode){
		var child=curNode.firstChild;
		if(child.className&&child.className.indexOf("mainBrand")!=-1){
			break;  
		}
		curNode = curNode.parentNode;
	}
	elemHeight = curNode.offsetHeight;
	return elemHeight;
}

var treeHtml = '<%=treeHtml %>';
var treeBox = document.getElementById("leftTreeBox");
if(treeBox)
	treeBox.innerHTML = treeHtml;

function treeBoxHeight(){
   
	var treeBox = document.getElementById("treev1");//树
	var winH;
    var topHeight = 0;
    
    var bodyClientHeight = document.documentElement.clientHeight;
    var treeSubNavv1Height = getElementByClassName("div","treeSubNavv1");
    var IFrameTomHeight = getElementByClassName("div","treeNavv1");
    var treev1 = document.getElementById("treev1");
    var isIE6 = !-[1,]&&!window.XMLHttpRequest;

	
    if(treeBox==null){
        return;
    }

    while(treeBox)
    {
        topHeight += treeBox.offsetTop;
        treeBox = treeBox.offsetParent;
    }
    winH = MathWinHeigth();

    if(topHeight<1){
        topHeight=0;
    }
    if(winH <= topHeight){
        winH = topHeight;
    }
    
    if(treev1!=null && treev1.nodeType==1){
        var tree1Height=winH - topHeight;
        if(tree1Height<1){tree1Height=0;}
        treev1.style.height = tree1Height+"px" ;
        
    }
    if(treeSubNavv1Height!=null&&IFrameTomHeight!=null&&treev1!=null){
        if(isIE6){
            if(topHeight>(treeSubNavv1Height.clientHeight + IFrameTomHeight.clientHeight)){
                if(bodyClientHeight - treeSubNavv1Height.clientHeight - IFrameTomHeight.clientHeight-10<0){
                    treev1.style.height = 0;
                }
                else{
                    treev1.style.height = bodyClientHeight - treeSubNavv1Height.clientHeight - IFrameTomHeight.clientHeight-10;
                }
            }
        }
    }
	//树形高度 by sk 2012.07.16
	var tempBar = getElementByClassName("div","bt_smenuNew");
    var leftTreeBox = document.getElementById("leftTreeBox");
	var bt_smenuNewHeight = getOffsetTop(tempBar);
	<%--while (tempBar) {
		bt_smenuNewHeight += tempBar.offsetTop;
		tempBar = tempBar.offsetParent;
	}--%>
    bt_smenuNewHeight += bt_navigateNewHeight;
    var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
	var treeFixedNav = document.getElementById("treeFixedNav");
	if(attrStyle(treeFixedNav,"position") == "fixed"){ scrollHeight += treeFixedNavHeight; }//fixed 影响滚动高度
	var leftTreeHeight=winH - (bt_smenuNewHeight - bt_navigateNewHeight) + scrollHeight;
	leftTreeBox.style.height = leftTreeHeight + "px";
}   
//计算屏幕高度
function MathWinHeigth(){
    var winH = 0;
      if(window.innerHeight) {
		winH =  Math.min(window.innerHeight, document.documentElement.clientHeight);
	} else if (document.documentElement && document.documentElement.clientHeight) {
		winH = document.documentElement.clientHeight;
	} else if (document.body) {
		winH = document.body.clientHeight;
	}
    return winH;
}
scrollToCurrentTreeNode();
addLoadEvent(treeBoxHeight);
addResizeEvent(treeBoxHeight);
//树滚动事件
var treeBox = document.getElementById("treev1");
addEvent(treeBox,"scroll",function(){
		if(typeof CookieHelper == "undefined") return;
		CookieHelper.setCookie("ScrollTreeNodeTop", parseInt(treeBox.scrollTop), { "expires": 360, "path": "/", "domain": "bitauto.com" });
	},false);

//根据样式名称获取元素
function getElementByClassName(tagName,className){
    var classObj = document.getElementsByTagName(tagName);
    var len = classObj.length;
    for(var i = 0; i < len; i++){
        if(classObj[i].className==className){
            return classObj[i];
            break;
        }
    }
}
var isTreeHeight=false;
var bitmap;
var treeFixedNavHeight = parseInt(attrStyle(document.getElementById("treeFixedNav"),"height"));//36; 导航栏高度
var bt_navigateNewHeight = parseInt(attrStyle(getElementByClassName("div","bt_smenuNew"),"height")) + 5;//39 ;头部图标栏高度
//获取样式属性
function attrStyle(elem, attr) {
	if(!elem) {return;}
	if (elem.style[attr]) {
		return elem.style[attr];
	} else if (elem.currentStyle) {
		return elem.currentStyle[attr];
	} else if (document.defaultView && document.defaultView.getComputedStyle) {
		attr = attr.replace(/([A-Z])/g, '-$1').toLowerCase();
		return document.defaultView.getComputedStyle(elem, null).getPropertyValue(attr);
	} else {
		return null;
	}
}
//获取元素top位置
function getOffsetTop(element){
	var top = 0;
	while (element) {
			if (attrStyle(element,"position") === "fixed" ) {
				break;
			}
            top += element.offsetTop;
            element = element.offsetParent;
        }
	return top;
}

//根据导航栏位置设置树形距顶高度
function treeFixedNavTop() {
    var treeFixedNav = document.getElementById("treeFixedNav");
	var bt_smenuNew = getElementByClassName("div","bt_smenuNew");
	var toptreeFixedNavHeight = getOffsetTop(bt_smenuNew) + bt_navigateNewHeight;//获取导航栏距离顶部距离
    if(treeFixedNav.className == "treeNavv1")//判断导航条是否是fixed by songkai 2013.01.21
    {            
        document.getElementById("leftTreeBox").style.top = -bt_navigateNewHeight + "px";
        return;
    }
    var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
    document.getElementById("leftTreeBox").style.top = toptreeFixedNavHeight - scrollHeight - treeFixedNavHeight + "px";
}
//计算滚动对导航栏和设置
function MathScrollSettingTagBar(){ 
    var topHeight = 0;
    var carTagBar = document.getElementById("treeFixedNav");
	var tempBar = getElementByClassName("div","bt_smenuNew");
    var leftTreeBox = document.getElementById("leftTreeBox");
	while (tempBar) {
		topHeight += tempBar.offsetTop;
		tempBar = tempBar.offsetParent;
	}
    topHeight += bt_navigateNewHeight;
    var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
    var winH = MathWinHeigth();//alert(winH);
	//页面较短时 定位跳动 2012.11.05 by sk
	if (attrStyle(carTagBar, "position") == "fixed") {
		scrollHeight += treeFixedNavHeight;
		carTagBar.style.zIndex = "9999";
	}
	//浮动导航栏
	if (scrollHeight > topHeight) {
        carTagBar.className = "treeNavv1";
	} else {
		carTagBar.className = "treeNavv1-org";
	}
	<%--if(scrollHeight > (topHeight - bt_navigateNewHeight)){
		leftTreeBox.style.height = "2000px";
	}else{
		leftTreeBox.style.height = "100%";
	}--%>
    bitmap=window.setTimeout(function(){treeBoxHeight();},250);
    //计算树的高度
    treeFixedNavTop();
}
//调整树形导航的位置与树的高度
function treeHeight() {
	if(!IE6){
		MathScrollSettingTagBar();
	}
	else{
		setTimeout(function(){leftTreeBox.style.top = 29 + ie6AdHeight + "px";},500);
	}
};
var ie6AdHeight = 0;
//滚动事件
addEvent(window,"scroll",function(){
		if(!IE6){
		MathScrollSettingTagBar();
		}
		else{
			var leftTreeBox = document.getElementById("leftTreeBox");
			leftTreeBox.style.top = 29 + ie6AdHeight + "px";//29:登陆条高度
		}
	},false);
treeHeight();

//广告调用调整树高度
function pullTopAd(adHeight) {
    var leftTreeBox = document.getElementById("leftTreeBox").offsetTop;
	if(IE6){
		var scrollHeight = document.body.scrollTop || document.documentElement.scrollTop;
		leftTreeBox = leftTreeBox - scrollHeight;
		ie6AdHeight = adHeight
	}
    document.getElementById("leftTreeBox").style.top = leftTreeBox + adHeight + "px";
}
function pullFullScreenAd(adHeight) {
    treeFixedNavTop();
    MathScrollSettingTagBar();
    treeHeight();
}

