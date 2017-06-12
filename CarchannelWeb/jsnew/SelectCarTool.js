// JavaScript Document


function BtZebraStrips(id,tag) {
	var ListId = document.getElementById(id);
	if(ListId){
	var tags  = ListId.getElementsByTagName(tag);
	for(var i=0;i<tags.length;i++) {
	tags[i].className   += " barry"+i%2;
	tags[i].onmouseover = function(){this.className += " hover"}
	tags[i].onmouseout  = function(){this.className = this.className.replace(" hover","")}}}
}


function addLoadEvent(func) {
  var oldonload = window.onload;
  if (typeof window.onload != 'function') {
    window.onload = func;
  } else {
    window.onload = function() {
      oldonload();
      func();
    }
  }
}

function BtHide(id){var Div = document.getElementById(id);if(Div){Div.style.display="none"}}
function BtShow(id){var Div = document.getElementById(id);if(Div){Div.style.display="block"}}



/**/
function showhide(click_div,show_div){
if (!document.getElementById(click_div)) return false;
var click_div = document.getElementById(click_div);	
var show_div =document.getElementById(show_div);	
click_div.onclick = function(){
		if(show_div.style.display =="none"){
			show_div.style.display = "block";
			click_div.innerHTML ="隐藏更多条件";
		}
		else{
			show_div.style.display = "none";
			click_div.innerHTML ="显示更多条件";
		}
		return false;
	}
}

function allshow(){
        showhide('click_MTAT','table_MTAT');
}

/*****	路径：newInTow.js	********/
/*****	名称：拖拽类	********/
/*****	作者：应龙	********/
/*****	时间：2009-5-5	********/
/*****	介绍：存放各类拖拽特效	********/
!function (bool){
	//兼容FF一些方法
	if (bool){
		//event
		window.constructor.prototype.__defineGetter__("event", function (){//兼容Event对象
			var o=arguments.callee;
			do{
				if (o.arguments[0] instanceof Event)return o.arguments[0];
			}while (o=o.caller);
			return null;
		});
		Node.prototype.__defineGetter__("parentElement",function(){return this.parentNode;});
	}
}(/Firefox/.test(window.navigator.userAgent));
var inTowBase={
	$:function(id){return document.getElementById(id)}
	,tagArr:function(o,name){return o.getElementsByTagName(name)}
	,att:function(o,name,fun){return document.all ? o.attachEvent(name,fun) : o.addEventListener(name.substr(2),fun,false);}
	,style:function(o){	//获取全局样式表、内嵌样式（不能设置）
		return o.currentStyle || document.defaultView.getComputedStyle(o,null);
	}
	,dElement:function(){//兼容DTD头
		return	document.documentElement || document.body;
	}
	,position:function(){//获取当前鼠标位置(x,y)
		return {
		'x':event.pageX || (event.clientX + this.dElement().scrollLeft)
		,'y':event.pageY || (event.clientY + this.dElement().scrollTop)
		}
	}
	,capture:function(obj,num){
		if(document.all){
			num?obj.setCapture():obj.releaseCapture();
		}	
	}
	,cleanOutSelect:function(){
		try {
			document.selection.empty();
		} catch (exp) {
			try {
				window.getSelection().removeAllRanges();
			} catch (exp) {}
		}
	}
	,rewriteAtt:function(formerObj,newObj){//给对象设置属性
		for(var i in newObj){
			formerObj[i]=newObj[i];
		}
		return formerObj;
	}
};
var newInTow=function(newObj){
	this.base=inTowBase;
	var pro=this;
	pro=this.base.rewriteAtt(pro,newObj);
	this.event();
};
newInTow.prototype={
	onOff:false
	,left:-10000,right:10000,top:-10000,bottom:10000
	,event:function(){
		var pro=this;
		this.base.att(document,'onmousemove',function(){if(pro.onOff){pro.move()}});
		this.base.att(document,'onmouseup',function(){if(pro.onOff){pro.up()}});
		if(this.downObj)this.base.att(this.downObj,'onmousedown',function(){pro.down()});
	}
	,down:function(){
		this.base.capture(this.obj,1);
		this.downTop=this.base.position().y-this.obj.offsetTop;
		this.downLeft=this.base.position().x-this.obj.offsetLeft;
		this.onOff=true;
		if(this.downCallBack){this.downCallBack();}
	}
	,move:function(){
		var pos=this.base.position(),pro=this;
		var y=pos.y-this.downTop,x=pos.x-this.downLeft;
		y=y>this.bottom?this.bottom:(y<this.top?this.top:y);
		x=x>this.right?this.right:(x<this.left?this.left:x);
		switch (this.type) {
			case 'vertical' : this.obj.style.top=y+"px";break;
			case 'horizontal' : this.obj.style.left=x+"px";break;
			case 'both' : this.obj.style.top=y+"px";this.obj.style.left=x+"px";break;
		}
		this.base.cleanOutSelect();
		if(this.moveCallBack){this.moveCallBack(x,y,pos);}
	}
	,up:function(){
		this.onOff=false;
		this.base.capture(this.obj,0);	
		if(this.upCallBack){this.upCallBack();}
	}
};


function inTowSet(info,type){
	var base=inTowBase;
	var leftObj=base.$(info.leftId);
	var rightObj=base.$(info.rightId);
	var bgObj=base.$(info.bgId);
	var textLeft=base.$(info.txtLeftId);
	var textRight=base.$(info.txtRightId);

	var obj=type?leftObj:rightObj;
	var dataArr=info.dataArr;
	var n=info.n;
	var i=0;
	new newInTow({
		obj:obj
		,downObj:obj
		,type:'horizontal'
		,left:0
		,right:info.length
		,moveCallBack:function(){
			this.seBgWL();
			if(type){
				i=Math.round(leftObj.offsetLeft/n);
				textLeft.value=dataArr[i]!=null?dataArr[i]:'';
			}else{
				i=Math.round((rightObj.offsetLeft-info.poor)/n);
				textRight.value=dataArr[i]!=null?dataArr[i]:'';
			}
		}
		,downCallBack:function(){
			if(type){
				this.right=rightObj.offsetLeft-leftObj.offsetWidth;
			}else{
				this.left=leftObj.offsetLeft+leftObj.offsetWidth;
			}
		}
		,upCallBack:function(){
			if(type){
				var left=i*n;	
				leftObj.style.left=left+'px';
			}else{
				rightObj.style.left=(i*n+info.poor)+'px';
			}
			this.seBgWL();
			info.callBack();
		}
		,seBgWL:function(){
			var width=rightObj.offsetLeft-leftObj.offsetLeft-leftObj.offsetWidth;
			bgObj.style.width=(width>=0?width:0)+'px';
			bgObj.style.left=(leftObj.offsetLeft+leftObj.offsetWidth)+'px';
			if(bgObj.id == "priceScroll_bg" && conditionObj.prePriceElement)
				conditionObj.prePriceElement.className = "";
			if(bgObj.id == "disScroll_bg" && conditionObj.preDisElement)
				conditionObj.preDisElement.className = "";
		}
	})
}

//设置报价对象
var priceSet = {
	initSet : function()
	{
		this.leftObj = document.getElementById("priceLeftIco");
		this.rightObj = document.getElementById("priceRightIco");
		this.bgObj = document.getElementById("priceScroll_bg");
		this.leftText = document.getElementById("txtPriceLeft");
		this.rightText = document.getElementById("txtPriceRight");
		this.dataArr = [0,3,4,5,6,7,8,9,10,12,14,16,18,20,25,30,35,40,45,50,60,70,80,90,100,'9999'];
		this.n = 8;
		this.poor = 11;
	}
	,setTruePrice : function(sender,minPrice,maxPrice)
	{
		if(sender != null)
		{
			if(conditionObj.prePriceElement != null)
				conditionObj.prePriceElement.className = "";
			if(sender)
				sender.className = "current";
			conditionObj.prePriceElement = sender;
		}
		if(maxPrice == 0)
			maxPrice = '9999';
		
		var leftPos = this.computeMinPos(minPrice);
		var rightPos = this.computeMaxPos(maxPrice) + this.poor;
		this.setPriceIcoPosition(leftPos,rightPos);
		this.setPriceValue(minPrice,maxPrice);			
	}
	,computeMinPos :function(price)
	{
		if(price == 0)
			return 0;
		else
			return this.computePos(price);
	}
	,computeMaxPos:function(price)
	{
		if(price == 0)
			return (this.dataArr.length - 1) * this.n;
		else
			return  this.computePos(price);
	}
	,computePos : function(price)
	{
		var leftI = 0;
		var rightI = 0;
		for(dataIndex=0;dataIndex<this.dataArr.length;dataIndex++)
		{
			if(price > parseInt(this.dataArr[dataIndex]))
				continue;
			else
			{
				leftI = dataIndex-1;
				if(leftI < 0)
					leftI = 0;
				rightI = dataIndex;
				break;
			}
		}
		
		var diffN = (price - parseInt(this.dataArr[leftI])) / (parseInt(this.dataArr[rightI]) - parseInt(this.dataArr[leftI]));
		return leftI * this.n + this.n * diffN;
	}
	,setPrice : function(sender,price)
	{
		if(conditionObj.prePriceElement != null)
			conditionObj.prePriceElement.className = "";
		if(sender)
			sender.className = "current";
		conditionObj.prePriceElement = sender;
		switch(price)
		{
			case 1:
				this.setPriceIcoPosition(0,34);
				this.setPriceValue(0,5);
				break;		
			case 2:
				this.setPriceIcoPosition(24,74);
				this.setPriceValue(5,10);
				break;
			case 3:
				this.setPriceIcoPosition(64,94);
				this.setPriceValue(10,15);
				break;
			case 4:
				this.setPriceIcoPosition(84,114);
				this.setPriceValue(15,20);
				break;
			case 5:
				this.setPriceIcoPosition(104,123);
				this.setPriceValue(20,25);
				break;
			case 6:
				this.setPriceIcoPosition(113,147);
				this.setPriceValue(25,40);
				break;
		}
	}
	
	,setPriceIcoPosition : function(leftPos,rightPos)
	{
		this.leftObj.style.left = leftPos + "px";
		this.rightObj.style.left = rightPos + "px";
		this.bgObj.style.left = this.leftObj.offsetLeft + "px";
		this.bgObj.style.width = (this.rightObj.offsetLeft - this.leftObj.offsetLeft) + "px";
	}
	,setPriceValue :function(leftPrice,rightPrice)
	{
		this.leftText.value = leftPrice;
		this.rightText.value = rightPrice; 
	}
	,getMinPrice:function()
	{
		var minPrice = 0;
		if(this.leftText.value.length != 0)
			minPrice = this.leftText.value;
		return minPrice;
	}
	,getMaxPrice:function()
	{
		var maxPrice = 0;
		if(this.rightText.value.length != 0 && this.rightText.value != "9999")
			maxPrice = this.rightText.value;
		return maxPrice;
	}
}


//设置排量对象
var disSet = 
{
	initSet : function()
	{
		this.leftDis = document.getElementById("disLeftIco");
		this.rightDis = document.getElementById("disRightIco");
		this.bgDis = document.getElementById("disScroll_bg");
		this.leftText = document.getElementById("txtDisLeft");
		this.rightText = document.getElementById("txtDisRight");
		this.dataArr = [0,'1.0',1.3,1.6,1.8,'2.0',2.5,'3.0','4.0','9'];
		this.n = 23;
		this.poor = 10;
	}
	,setTrueDis : function(sender,minDis,maxDis)
	{
		if(sender != null)
		{
			if(conditionObj.preDisElement != null)
				conditionObj.preDisElement.className = "";
			if(sender)
				sender.className = "current";
			conditionObj.preDisElement = sender;
		}
		
		if(maxDis == 0)
			maxDis = '9';
		
		var leftPos = this.computeMinPos(minDis);
		var rightPos = this.computePos(maxDis) + this.poor;
		this.setLeftDis(leftPos);
		this.setRightDis(rightPos);
		this.setDisValue(minDis,maxDis);
	}
	,computeMinPos:function(disValue)
	{
		if(disValue == 0)
			return 0;
		else
			return this.computePos(disValue);
	}
	,computePos : function(disValue)
	{
		var leftI = 0;
		var rightI = 0;
		for(dataIndex=0;dataIndex<this.dataArr.length;dataIndex++)
		{
			if(parseFloat(disValue) > parseFloat(this.dataArr[dataIndex]))
				continue;
			else
			{
				leftI = dataIndex-1;
				if(leftI < 0)
					leftI = 0;
				rightI = dataIndex;
				break;
			}
		}		

		var diffN = (parseFloat(disValue) - parseFloat(this.dataArr[leftI])) / (parseFloat(this.dataArr[rightI]) - parseFloat(this.dataArr[leftI]));
		return leftI * this.n + this.n * diffN;
	}
	,setDis : function(sender,dis)
	{
		if(conditionObj.preDisElement != null)
			conditionObj.preDisElement.className = "";
		if(sender)
			sender.className = "current";
		conditionObj.preDisElement = sender;
		switch(dis)
		{
			case 1:
				this.setLeftDis(0);
				this.setRightDis(65);
				this.setDisValue(0,1.4);
				break;
			case 2:
				this.setLeftDis(55);
				this.setRightDis(78);
				this.setDisValue(1.4,1.6);
				break;
			case 3:
				this.setLeftDis(68);
				this.setRightDis(126);
				this.setDisValue(1.6,"2.0");
				break;
			case 4:
				this.setLeftDis(116);
				this.setRightDis(148);
				this.setDisValue("2.0",2.5);
				break;
			case 5:
				this.setLeftDis(138);
				this.setRightDis(225);
				this.setDisValue(2.5,"9");
				break;
		}
	}
	,setLeftDis : function(pos)
	{
		this.leftDis.style.left = pos + "px";
	}
	,setRightDis : function(pos)
	{
		this.rightDis.style.left = pos + "px";
		this.bgDis.style.left = this.leftDis.offsetLeft + "px";
		this.bgDis.style.width = (this.rightDis.offsetLeft - this.leftDis.offsetLeft) + "px";
	}
	,setDisValue : function(minDis,maxDis)
	{
		this.leftText.value = minDis;
		this.rightText.value = maxDis;
	}
	,getMinDis:function()
	{
		var minDis = 0;
		if(this.leftText.value.length != 0)
			minDis = this.leftText.value;
		return minDis;
	}
	,getMaxDis:function()
	{
		var maxDis = 0;
		if(this.rightText.value.length != 0 && this.rightText.value != "9")
			maxDis = this.rightText.value;
		return maxDis;
	}
}


//查询条件对象
var conditionObj = 
{
	prePriceElement:null
	,preDisElement:null
	,preTransmissionElement:document.getElementById("transAll")
	,preBodyFormElement:document.getElementById("bodyFormAll")
	,preLevelElement:document.getElementById("levelAll")
	,minPrice:0
	,maxPrice:0
	,minDis:0
	,maxDis:0
	,transmissionType:0
	,bodyForm:0
	,serialLevel:0
	,serialPurpose:0
	,serialCountry:0
	,serialComfortable:0
	,serialSafety:0
	,setTransmission : function(sender,transType)
	{
		if(this.preTransmissionElement != null)
			this.preTransmissionElement.className = "";
		sender.className = "current";
		this.preTransmissionElement = sender;
		this.transmissionType = transType;
	}
	,setBodyForm:function(sender,bodyForm)
	{
		if(this.preBodyFormElement != null)
			this.preBodyFormElement.className = "";
		sender.className = "current";
		this.preBodyFormElement = sender;
		this.bodyForm = bodyForm;
	}
	,setLevel:function(sender,level)
	{
		if(this.preLevelElement != null)
			this.preLevelElement.className = "";
		sender.className = "current";
		this.preLevelElement = sender;
		this.serialLevel = level;
	}
	,setPurpose:function(sender)
	{
		if(sender.checked)
			this.serialPurpose += parseInt(sender.value);
		else
			this.serialPurpose -= parseInt(sender.value);
	}
	,setCountry:function(sender)
	{
		if(sender.checked)
			this.serialCountry += parseInt(sender.value);
		else
			this.serialCountry -= parseInt(sender.value);
	}
	,setComfortable:function(sender)
	{
		if(sender.checked)
			this.serialComfortable += parseInt(sender.value);
		else
			this.serialComfortable -= parseInt(sender.value);
	}
	,setSafety:function(sender)
	{
		if(sender.checked)
			this.serialSafety += parseInt(sender.value);
		else
			this.serialSafety -= parseInt(sender.value);
	}
	,getPriceAndDis:function()
	{
		this.minPrice = priceSet.getMinPrice();
		this.maxPrice = priceSet.getMaxPrice();
		this.minDis = disSet.getMinDis();
		this.maxDis = disSet.getMaxDis();
	}	
}


function InitRangeBar()
{
	//初始化价格选择条
	var priceInfo={
		leftId:'priceLeftIco'
		,rightId:'priceRightIco'
		,bgId:'priceScroll_bg'
		,txtLeftId:'txtPriceLeft'
		,txtRightId:'txtPriceRight'
		,dataArr:[0,3,4,5,6,7,8,9,10,12,14,16,18,20,25,30,35,40,45,50,60,70,80,90,100,'9999']
		,n:8
		,length:214
		,poor:11
		,callBack:function(){}
	}
	inTowSet(priceInfo,1);
	inTowSet(priceInfo,0);
	//初始化排量选择条
	var disInfo={
				leftId:'disLeftIco'
				,rightId:'disRightIco'
				,bgId:'disScroll_bg'
				,txtLeftId:'txtDisLeft'
				,txtRightId:'txtDisRight'
				,dataArr:[0,'1.0',1.3,1.6,1.8,'2.0',2.5,'3.0','4.0','9']
				,n:23
				,length:220
				,poor:10
				,callBack:function(){}
			}
			inTowSet(disInfo,1);
			inTowSet(disInfo,0);
	priceSet.initSet();
	disSet.initSet();
}

//获取热点车型
function UpdateHotSerial()
{
	var hotOptions = 
	{
		parameters:"datatype=hot",
		method:"get",
		onSuccess:onGotHotSerialHtml
	}
	new Ajax.Request("/SelectCarTool.aspx", hotOptions);
}

//生成Get参数列表
function makeQueryString(dataType)
{
	conditionObj.getPriceAndDis();
	var paras = new Array();
	paras.push("datatype=" + dataType);
	if(conditionObj.minPrice != 0)
		paras.push("minPrice=" + conditionObj.minPrice);
	if(conditionObj.maxPrice != 0)
		paras.push("maxPrice=" + conditionObj.maxPrice);
	if(conditionObj.minDis != 0)
		paras.push("minDis=" + encodeURI(conditionObj.minDis));
	if(conditionObj.maxDis != 0)
		paras.push("maxDis=" + encodeURI(conditionObj.maxDis));
	if(conditionObj.transmissionType != 0)
		paras.push("trans=" + conditionObj.transmissionType);
	if(conditionObj.bodyForm != 0)
		paras.push("body=" + conditionObj.bodyForm);
	if(conditionObj.serialLevel != 0)
		paras.push("level=" + conditionObj.serialLevel);
	if(conditionObj.serialPurpose != 0)
		paras.push("purpose=" + conditionObj.serialPurpose);
	if(conditionObj.serialCountry != 0)
		paras.push("country=" + conditionObj.serialCountry);
	if(conditionObj.serialComfortable != 0)
		paras.push("comfortable=" + conditionObj.serialComfortable);
	if(conditionObj.serialSafety != 0)
		paras.push("safety=" + conditionObj.serialSafety);
	
	return paras.join('&');
}

//查询车型
function selectSerial(pageIndex)
{
	//查询参数
	var paras = makeQueryString("select");
	
	paras += "&pageIndex=" + pageIndex;
	//alert(paras);
	waitWindow.show();
	var selectOptions = 
	{
		parameters:paras,
		method:"get",
		onSuccess:onSelectCarSuccess
	}
	new Ajax.Request("/SelectCarTool.aspx", selectOptions);
}

//查询结果返回处理
function onGotHotSerialHtml(res)
{
	var listElement = document.getElementById("serialList");
	listElement.innerHTML = res.responseText;
}

//处理选车结果
function onSelectCarSuccess(res)
{
	dataDoc.loadXml(res.responseText);
	var rootEle = dataDoc.selectSingleNode("root");
	var pageCount = rootEle.getAttribute("page");
	var serialNum = rootEle.getAttribute("serila");
	var carNum = rootEle.getAttribute("car");
	var titleEle = document.getElementById("carTitle");
	titleEle.innerHTML = "<span><span class=\"caption\">选车结果</span></span><small>（共<em>" + serialNum + "</em>个车型，含<em>" + carNum + "</em>个车款）</small>";
	if(parseInt(pageCount) > 0)
	{
		SelectCarPage(pageCount);
	}
	else
	{
		document.getElementById("serialList").innerHTML = getNoResultHtml();
		document.getElementById("pageNav").style.display = "none";
	}
	
	carDetailInfo.preLayerEle = null;
	carDetailInfo.preViewButton = null;
	
	waitWindow.close();
}

//显示指定页的选车结果
function SelectCarPage(pageCount)
{
	window.setTimeout("SetPageContent(" + pageCount + ");",50);
}

function SetPageContent(pageCount)
{
	var listElement = document.getElementById("serialList");
	var pageEle = dataDoc.selectSingleNode("/root/Page");
	var pageIndex = pageEle.getAttribute("pageNum");
	var htmlCodeElel = pageEle.selectSingleNode("HtmlCode");
	listElement.innerHTML = htmlCodeElel.text;
	SetPageNav(pageCount,pageIndex);
}

//设置页号导航
function SetPageNav(pageCount,pageNum)
{
	var pageNav = document.getElementById("pageNav");
	if(pageCount == 1)
		pageNav.style.display = "none";
	else
	{
		pageNav.style.display = "block";
		//生成
		var startNum = pageNum - 5;
		if(startNum < 1)
			startNum = 1;
		var endNum = startNum + 10 - 1;
		if(endNum > pageCount)
		{
			startNum -= (endNum - pageCount);
			if(startNum < 1)
				startNum = 1;
			endNum = pageCount;
		}
		
		
		var navArray = new Array();
		navArray.push("<div>");
		if(pageNum > 1)
			navArray.push("<a href=\"javascript:void(0);\" class=\"preview_on\" onclick=\"selectSerial(" + (pageNum - 1) + ");\">&lt;&lt;上一页</a>");
		for(var j=startNum;j<=endNum;j++)
		{
			if(j == pageNum)
				navArray.push("<a class=\"linknow\">" + j + "</a>");
			else
				navArray.push("<a href=\"javascript:void(0);\" onclick=\"selectSerial(" + j + ");\">" + j + "</a>");
		}
		if(pageNum < pageCount)
			navArray.push("<a href=\"javascript:void(0);\" onclick=\"selectSerial(" + (parseInt(pageNum) + 1) + ");\" class=\"next_on\">下一页&gt;&gt;</a>");
		
		pageNav.innerHTML = navArray.join("");
	}
}

//显示子品牌中的车型信息
function showCarInfo(sender,serialId,layerNum)
{
	carDetailInfo.clickViewButton(sender,serialId,layerNum);
}
//显示车型具体信息的对象
var carDetailInfo = 
{
	preLayerEle : null
	,preViewButton:null
	,carDetails:new Object()
	,serialId:0
	,currentLayer:null
	,clickViewButton: function(sender,serialId,layerNum)
	{
		this.serialId = serialId;
		this.currentLayer = document.getElementById("carInfoLayer" + layerNum);
		
		if(sender.innerHTML == "查看车型")		//打开
		{
			sender.innerHTML = "收起";
			sender.parentElement.className = "hide_model";
			this.currentLayer.style.display = "block";
			if(this.preViewButton && this.preViewButton != sender)
			{
				//关闭前一个
				if(this.preLayerEle && this.preLayerEle != this.currentLayer)
				{
					this.preLayerEle.style.display = "none";				
				}
				this.preViewButton.innerHTML = "查看车型";
				this.preViewButton.parentElement.className = "view_model";
			}
			//显示数据
			this.getDetailInfo();
			this.preViewButton = sender;
			this.preLayerEle = this.currentLayer;
		}
		else		//关闭
		{
			sender.innerHTML = "查看车型";
			sender.parentElement.className = "view_model";
			this.currentLayer.style.display = "none";
		}
		
	}
	,getDetailInfo:function()
	{
		if(this.carDetails[this.serialId])
		{
			this.onGetCarDetailInfo(this.carDetails[this.serialId]);
		}
		else
		{
			var serialNode = dataDoc.selectSingleNode("/root/Page/Serial[@id=\"" + this.serialId + "\"]");
			if(serialNode)
			{
				var cars = serialNode.getAttribute("cars");
				waitWindow.show();
				var selectOptions = 
				{
					parameters:"datatype=carinfo&cars=" + cars,
					method:"get",
					onSuccess:this.onGetCarDetailInfo
				}
				new Ajax.Request("/SelectCarTool.aspx", selectOptions);
				
			}		
		}
	}
	,onGetCarDetailInfo:function(res)
	{
		if(!carDetailInfo.carDetails[carDetailInfo.serialId])
		{
			 carDetailInfo.carDetails[carDetailInfo.serialId] = res;
		}
		if(carDetailInfo.currentLayer)
		{
			window.setTimeout("carDetailInfo.SetCarDetailsInfo()",50);
		}
		waitWindow.close();
	}
	,SetCarDetailsInfo:function()
	{
		var res = carDetailInfo.carDetails[carDetailInfo.serialId];
		if(carDetailInfo.currentLayer)
		{
			carDetailInfo.currentLayer.innerHTML = res.responseText;
		}
	}
}

function addSelectEvent()
{
	var btnSelect = document.getElementById("btnSelect");
	if(btnSelect)
		btnSelect.onclick = function()
		{
			carDetailInfo.carDetails = {};
			selectSerial(1);
			//记录报价与排量的选择区间*alert(conditionObj.minPrice);*/
			var priceRange = conditionObj.minPrice + "-" + conditionObj.maxPrice;
			var disRange = conditionObj.minDis + "-" + conditionObj.maxDis;
			//alert(priceRange + "==>" + disRange);
			SelectedRange(priceRange,disRange);
		}
}

//添加主页中选择按钮的功能
function addHomepageSelectEvent()
{
	var btnSelect = document.getElementById("btnSelect");
	if(btnSelect)
		btnSelect.onclick = function()
		{
			var selectUrl = "http://car.bitauto.com/xuanchegongju/?" + makeQueryString("queryselect");
			//alert(selectUrl);
			window.open(selectUrl);
		}
}

function getNoResultHtml()
{
	var noRet = "<div class=\"error_page\">"
		+ "<h1>抱歉，未找到合适的车型！</h1>"
		+ "<p><h4>建议您：</h4></p>"
		+ "<ul><li>调整一下选车条件，再尝试。</li>"
		+ "</ul>"
		+ "</div>";
	return noRet;
}


//初始化页面
function Initpage()
{
	addLoadEvent(allshow);
	//BtHide("table_MTAT");
	InitRangeBar();
	//priceSet.setPrice(null,3);
	//UpdateHotSerial();
}
var dataDoc = new BAXmlDom();
Initpage();







