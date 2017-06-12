/*FF兼容函数，修改ＦＦ内置方法*/
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
	}
}(/Firefox/.test(window.navigator.userAgent));
//底层
var base={
	$:function(id){return document.getElementById(id)}
	,tagArr:function(o,name){return o.getElementsByTagName(name)}
	,att:function(o,name,fun){return document.all ? o.attachEvent(name,fun) : o.addEventListener(name.substr(2),fun,false);}
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
	,offsetLT:function(o){//元素相对于整个窗口的位置 o对象 返回 x y
        var x = 0, y = 0;
        do { x += o.offsetLeft, y += o.offsetTop; } while (o = o.offsetParent);
        return { 'x' : x, 'y' : y };
    }
	,documentElement:function(type){//兼容DTD头
		return	document.documentElement[type] || document.body[type];
	}
	,position:function(){//获取当前鼠标位置(x,y)
		return {
		'x':event.pageX || (event.clientX + this.documentElement('scrollLeft'))
		,'y':event.pageY || (event.clientY + this.documentElement('scrollTop'))
		}
	}
	,alpha:function(o,num){//设置透明度	
		o.style.filter='alpha(opacity='+num+')';
		o.style.opacity=num/100;
	}
	,rewriteAtt:function(formerObj,newObj){//给对象设置属性
		for(var i in newObj){
			formerObj[i]=newObj[i];
		}
		return formerObj;
	}
	,loop:null
	,rollPlay:function(obj,moveDis,Type,n,callBack){
		var pro=this;
		var moveType={left:'scrollLeft',top:'scrollTop'}
		var len=moveDis,type=moveType[Type];
		if(len==obj[type]){
			obj[type]=len;
			clearTimeout(this.loop);
			if(callBack)callBack();
			return;
		}
		var sp=(len-obj[type])*0.01*n;
		sp=(sp>0)?(sp>1?sp:1):(sp<-1?sp:-1);
		obj[type]+=sp;
		this.loop=setTimeout(function(){pro.rollPlay(obj,moveDis,Type,n,callBack);},10);
	}
	,parse:{//repeater模板解析器
		templet:function(data,templet){
			var key=templet.key,text=templet.text();
			for(var i in key){
				if(key[i]=='str'){
					text=text.replace(eval('/'+i+'/g'),data[i]);
				}else{
					text=this.cycRepeater(data[i],text,key[i],i);
				}
			}
			return text;
		}
		,cycRepeater:function(arr,templet,key,keyName){
			var len=arr.length,i=0,str=[];
			var keyArr=keyName.split('.');
			var text=templet.match(eval('/'+keyArr[0]+'([\\s\\S]*?)'+keyArr[1]+'/'));
			for(;i<len;i++){
				str[str.length]=this.templet(arr[i],{text:function(){return text[1]},key:key});
			}
			return templet.replace(text[0],str.join(''));
		}
	}
	,loadJS:{//数据加载
		lock : false, ranks : []
		, callback : function (startTime, callback) {	
			callback && callback(new Date().valueOf() - startTime.valueOf()); 	
			this.lock = false, this.read(); 
		}
		, read : function () {
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
						, script.parentNode.removeChild(script), script = null; 	
					wc.callback(startTime, ranks.callback), startTime = ranks = null;
				};
				script.charset = ranks.charset || 'gb2312';
				script.src = ranks.src;
				head.appendChild(script);
			}
		}
		, push : function (src, charset, callback) {
			this.ranks.push({ 'src' : src, 'charset' : charset, 'callback' : callback });
			this.read();
		}
	}
};
//图片列表生成类
var picList=function(newObj){
	this.base=base;
	var pro=this;
	pro=this.base.rewriteAtt(pro,newObj);
	this.show();
}
picList.prototype={
	show:function(){//显示内容
		if(this.getTabObj()){this.callBack&&this.callBack();return;}
		var text=this.html();
		var html=this.obj.innerHTML;
		this.obj.innerHTML=this.type?(html+text):(text+html);
		this.objWidth();
		this.callBack&&this.callBack(this.type);
	}
	,html:function(){//组装结构
		return this.base.parse.templet(this.templet.data(this.ulName+this.pageIndex,this.data),this.templet);
	}
	,getTabObj:function(){//判断是否已经加载
		return this.base.$(this.ulName+this.pageIndex);
	}
	,objWidth:function(){
		var ulArr=this.base.tagArr(this.obj,'ul');
		this.obj.style.width=(this.parObj.offsetWidth*ulArr.length)+'px';
		if(!this.type)this.parObj.scrollLeft+=this.parObj.offsetWidth;	
	}
}

//显示配置
var showConfig=function(newObj){
	this.base=base;
	var pro=this;
	pro=this.base.rewriteAtt(pro,newObj);
	this.show();
}
showConfig.prototype={
	show:function(){
		this.bigPic();
		this.title();
		this.nav();
		this.paging();
		this.groupPaging();
		this.callBack&&this.callBack();
	}
	,bigPic:function(){//显示大图
		var obj=this.bigPicObj;
		if(this.initId==this.data.picId)return;
		this.base.alpha(obj,0);
		obj.src=this.data.maxUrl;
		obj.alt=this.subBrand+this.data.car+this.data.picName;
		this.base.$('div_browser').className=this.data.picRate>1?'ba_pic_browser ba_pic_browser_up':'ba_pic_browser'
		this.base.$('img_box').className=this.data.picRate>1?'bpic_box_girl':'bpic_box_car'
	}
	,title:function(){//显示标题
		this.titleObj.innerHTML=this.subBrand+this.data.car+this.data.picName;
	}
	,nav:function(){//显示导航
		this.navObj.innerHTML=(this.data.car!='')?('&gt; '+this.data.car):'';
	}
	,paging:function(){//显示分页
		var text=[
			'<a href="上一张" onclick="return false;" onmousedown="tabNextPic(0);" style="display:{@Previous@}">&lt;&lt;上一张</a>'
			,'&nbsp;<strong>{@list@}</strong>/{@sum@}&nbsp;'
			,'<a href="下一张" onclick="tabNextPic(1);return false;" onmousedown="tabNextPic(1);"  style="display:{@next@}">下一张&gt;&gt;</a>'
		].join('');
		var list=this.data.list-0+1;
		text=text.replace(/{@Previous@}/g,(list!=1?'inline':'none'));
		text=text.replace(/{@next@}/g,(list!=this.sum?'inline':'none'));
		text=text.replace(/{@list@}/g,list);
		text=text.replace(/{@sum@}/g,this.sum);
		this.pagingObj.innerHTML=text;
	}
	,groupPaging:function(){//显示组分页
		var len=parseInt(this.sum/this.pageSize)
		var pageLen=this.sum%this.pageSize==0?len:len+1;
		var text='<strong>'+this.index+'</strong> / '+pageLen+'组'
		this.groupPagingObj[0].innerHTML=this.groupPagingObj[1].innerHTML=base.$('groupTitle2').innerHTML=text;
	}
}


//调用用显示配置
function setShowConfig(data){
	var Base=base;
	var vData=variableData;
	var post=dataPostParameter;
	new showConfig({
		bigPicObj:Base.$(vData.maxPicId)
		,titleObj:Base.$(vData.pageTitleId)
		,navObj:Base.$(vData.navPageId)
		,pagingObj:Base.$(vData.tabPicId)
		,groupPagingObj:[Base.$('groupTitle1'),Base.$('groupTitle2')]
		,sum:vData.sum//总张数
		,index:post.pageIndex//当前组数
		,data:data
		,subBrand:vData.subBrand
		,pageSize:post.pageSize
		,initId:vData.initId
	});
}
//调用图片列表生成类
function setPicList(data,type,callBack){//type 0上一组 1下一组
	if(!data)return;
	var Base=base;
	var vData=variableData;
	var post=dataPostParameter;
	new picList({
		parObj:Base.$(vData.listBoxId)
		,obj:Base.$(vData.listSubBoxId)
		,templet:pageTemplet.picList
		,data:data
		,ulName:'picListUl_'
		,type:type
		,pageIndex:post.pageIndex
		,callBack:callBack
	});
}

//切换图片
function tabPic(obj){
	var post=dataPostParameter;
	var vData=variableData;
	var Base=base;
	if(!obj){obj=Base.$('picList_'+picData[post.pageIndex][0].picId)}
	if(vData.focusObjId)Base.$(vData.focusObjId).className='';
	vData.focusObjId=obj.id;
	Base.$(vData.focusObjId).className='current';
	var listNum=obj.getAttribute('listNum');
	setShowConfig(picData[post.pageIndex][listNum]);
	var iframeObj=base.$(vData.iframeId);
	var src=iframeObj.src;
	iframeObj.src='';
	iframeObj.src=src;
}
//切换上下张图片
function tabNextPic(type){
	var Base=base;
	var post=dataPostParameter;
	var vData=variableData;
	type-=0;
	var obj=Base.$(vData.focusObjId);
	if(!obj)return;
	var listNum=obj.getAttribute('listNum')-0;
	var liName='picList_';
	var data=picData[post.pageIndex];
	listNum=type?(listNum+1):(listNum-1);
	if(!data[listNum]){
		if(type){
			loadData(1,1);
		}else{
			loadData(0,1);
		}
		return;
	}
	tabPic(Base.$(liName+data[listNum].picId));
}

//清除过多数据
var clearData=function(){
	var post=dataPostParameter;
	var vData=variableData,name=vData.previousDataName;
	if(!name)return;
	if(name==post.pageIndex)return;
	delete picData[name];
	var obj=base.$('picListUl_'+name);
	var parObj=obj.parentNode;
	parObj.removeChild(obj);
	pObj=parObj.parentNode;
	pObj.scrollLeft-=pObj.offsetWidth;
}



//使用滚动
function setRoll(obj,moveDis){
	var Base=base;
	var vData=variableData;
	var post=dataPostParameter;
	tagPlay();
	var obj=Base.$(vData.listBoxId);
	var left=Base.offsetLT(obj).x
	var moveDis=Base.offsetLT(Base.$('picListUl_'+post.pageIndex)).x-left;
	base.rollPlay(obj,moveDis,'left',20,function(){
		tabPageOnOff=true;
		clearData();
		tagPlay();
	});
}

//判断光标 跳转上下张图片
function cursorMove(obj){
    
	var vData=variableData;
	var left=base.offsetLT(obj).x+obj.width/2;
	if(left>=base.position().x){
		if(obj.style.cursor=="url("+vData.preAni+"),auto"){return false;}
		obj.style.cursor="url("+vData.preAni+"),auto";
		return false;
	}else{
		if(obj.style.cursor=="url("+vData.nextAni+"),auto"){return true;}
		obj.style.cursor="url("+vData.nextAni+"),auto";
		return true;
	}
}
var tabPageOnOff=true;	


function tabPageNum(type,index){
	if(!tabPageOnOff)return;
	var post=dataPostParameter;
	var vData=variableData;
	tabPageOnOff=false;
	if(type){
		post.pageIndex+=1;
	}else{
		post.pageIndex-=1;
	}
	if(post.pageIndex<1){post.pageIndex=1;return;}
	var len=vData.sum>=post.pageSize?parseInt(vData.sum/post.pageSize):0;
	len=vData.sum%post.pageSize==0?len:len+1;
	if(post.pageIndex>len)post.pageIndex=len;
}

//加载数据 
function loadData(type,index){
	if(!tabPageOnOff)return;
	var post=dataPostParameter;
	var vData=variableData;
	
	if(index)vData.previousDataName=post.pageIndex;
	if(index)tabPageNum(type);
	var post=dataPostParameter;
	var url=post.postUrl;
	if(post.classId=='0'){
		url+='?serialid='+post.brandId;
	}else{
		url+='?classid='+post.classId;
	}
	url+='&groupid='+post.groupId;
	url+='&pageindex='+post.pageIndex;
	url+='&pagesize='+post.pageSize;
	if(vData.previousDataName){
		if(vData.previousDataName==post.pageIndex){
			if(type){
				if(vData.FinallyUrl)location.href=vData.FinallyUrl;
			}else{
				if(vData.firstUrl)location.href=vData.firstUrl;
			}
			tabPageOnOff=true;
			return;
		}
	}
	if(type){
		post.pageIndex-=1;
	}else{
		post.pageIndex+=1;
	}
	base.loadJS.push(url,'utf-8',function(){
		//数据加载完成后，初始化图片列表
		if(type){
		    post.pageIndex+=1;
	    }else{
		    post.pageIndex-=1;
	}
		var data=picData[post.pageIndex];
		setPicList(data,type,function(){
			var num=0;
			if(vData.previousDataName&& vData.previousDataName>post.pageIndex){
				num=data.length-1;
			}
			
			setRoll();
			if(vData.initId){
				tabPic(base.$('picList_'+vData.initId));
				vData.initId='';
			}else{
			    tabPic(base.$('picList_'+data[num].picId));
			}
			getPagePlayParameter();
		})
	})
}

//键盘事件
function keyDowm(key){
	if(!tabPageOnOff)return;
	switch (key) {
		case 37 :
			tabNextPic(0);//上一张
			break;
		case 39 :
			tabNextPic(1);//下一张
			break;
		case 100 :
			tabNextPic(0);//上一张
			break;
		case 102 :
			tabNextPic(1);//下一张
			break;
	} 
}
//页面模板
var pageTemplet={//pageTemplet.picList.data();
	picList:{//图片列表模板
		text:function(){//结构
			return [
				'<ul id="{@listId@}" style="width:158px;float:left;">'
            	,'{@repeater}<li id="picList_{@picId@}" listNum="{@listNum@}" onmousedown="tabPic(this)">'
				,'<a href="{@subBrand@} {@car@} {@picName@}" onclick="return false;">'
				,'<img src="{@picMinUrl@}" alt="{@subBrand@} {@car@} {@picName@}" style="filter:alpha(opacity=0);opacity:0;{@setWH@}" onload="base.alpha(this,100);" />'
				,'</a></li>{repeater@}</ul>'
			].join('');
		}
		,key:{//关键字
			'{@listId@}':'str'
			,'{@repeater}.{repeater@}':{
				'{@listNum@}':'str'
				,'{@picId@}':'str'
				,'{@picMinUrl@}':'str'
				,'{@subBrand@}':'str'
				,'{@car@}':'str'
				,'{@picName@}':'str'
				,'{@setWH@}':'str'
			}
		}
		,data:function(listId,arr){//把普通数据装换成模板认得数据格式
			var data=variableData;
			var object={
				'{@listId@}':listId
				,'{@repeater}.{repeater@}':[]
			};
			if(!arr)return;
			var len=arr.length,i=0,str=[];
			for(;i<len;i++){
				var obj={},rArr=arr[i];
				obj['{@listNum@}']=i;
				obj['{@picId@}']=rArr['picId'];
				obj['{@picMinUrl@}']=data.picUrl+rArr['minUrl'];
				obj['{@subBrand@}']=data.subBrand;
				obj['{@car@}']=rArr['car'];
				obj['{@picName@}']=rArr['picName'];
				obj['{@setWH@}']=rArr['picRate']>1?'width:70px;':'height:70px;margin-left:'+-parseInt((70/(rArr['picRate']-0)-70)/2)+'px'
				str[str.length]=obj;
			}
			object['{@repeater}.{repeater@}']=str;
			return object;
		}
	}
};


//Cookie存储
function setTreeCookie(sName, sValue)
{
        var date = new Date;
		
		date.setDate(date.getDate() + 1);
		document.cookie = sName + "=" + sValue + ";expires=" + date.toGMTString()+";path=/";
		//document.cookie = sName + "=" + sValue + ";path=/";
}
//Cookie获取
function getTreeCookie(sName)
{
    var cookieString = new String(document.cookie);
    var cookieHeader = sName+"=";
    var beginPosition = cookieString.indexOf(cookieHeader);
    if (beginPosition != -1){
        
        return cookieString.match(eval('/'+cookieHeader+'([^;]+)/'))[1]
       // return cookieString.substring(beginPosition + cookieHeader.length) 
    }else {
        return null;
    }
}
function writeFlash(serialId,imageId,imageIndex,groupId,sliderEnabled,sliderInterval,classId){
    var post=dataPostParameter;
	var container = document.getElementById("FullScreen");
    var html = "";
    html+="<object id=\"fullscreenflash\" classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0\" width=\"44\" height=\"13\" id=\"browser\" align=\"middle\" style=\"position:absolute;top:5px;left:10px;\">\r\n";
    html+="            <param name=\"allowScriptAccess\" value=\"always\" />\r\n";
    html+="            <param name=\"allowFullScreen\" value=\"true\" /> \r\n";
    html+="            <param name=\"movie\" value=\""+post.flashSrc+"?zppId=" + serialId +"&classid="+classId + "&picId=" + imageId + "&imageindex="+imageIndex+"&groupid="+groupId+"&url="+post.flashXmlUrl+"&autoplay="+sliderEnabled+"&interval="+sliderInterval+"\" />";
    html+="            <param name=\"quality\" value=\"high\" />";
	html+="            <param name=\"wmode\" value=\"transparent\">"; 
    html+="            <embed allowFullScreen=\"true\" src=\""+post.flashSrc+"?zppId=" +serialId+"&classid="+classId + "&picId=" + imageId + "&imageindex="+imageIndex+"&groupid="+groupId+"&url="+post.flashXmlUrl+"&autoplay="+sliderEnabled+"&interval="+sliderInterval+"\" quality=\"high\" width=\"44\" height=\"13\" wmode=\"transparent\" name=\"browser\" align=\"middle\" allowScriptAccess=\"always\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />\r\n";
    html+="</object>\r\n";
    container.innerHTML=html;
}
var playId='';
function getPagePlayParameter(){
	var post=dataPostParameter;
	var vData=variableData;
	var Ti=getTreeCookie('playTime');
    var sT=getTreeCookie('playState');
	writeFlash(dataPostParameter.brandId,variableData.imageId,variableData.imageIndex,dataPostParameter.groupId,(sT?sT-0:1),(Ti?Ti:8),dataPostParameter.classId);
}

//点击flash时执行的方法
function AutoPlay(isEnabled){
    stopPic();
}
//退出flash全屏时执行的方法
function setPlayByFlash(mautoplay,minterval,mserialid,mimageid)
{
	setTreeCookie('playTime',minterval);
	setTreeCookie('playState',mautoplay?1:0);
    if(mserialid>0){
		window.location.href='http://'+ window.location.host+'/picture/'+mserialid+'/'+mimageid;
	}else{
		window.location.href=window.location.href.replace(/[^\/]+$/,mimageid);
	}
}

//图片数据
var picData={};
function initPlayTime(btnType,playCycle){
	base.$('ShowSecond').innerHTML=playCycle+'秒';
	variableData.playCycle=playCycle;
	base.$('ImageSlider_bar').style.left=parseInt((playCycle-0)*43/14)+'px';
	if(btnType){
		playPic();
	}else{
		stopPic();
	}
}