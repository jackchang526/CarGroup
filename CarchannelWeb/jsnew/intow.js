var inTow=function(newObj){
	this.base=base;
	var pro=this;
	pro=this.base.rewriteAtt(pro,newObj);
	this.event();
};
inTow.prototype={
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
	,moveOnOff:true
	,move:function(){
		if(!this.moveOnOff){return;}
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


function intow(objId,textId){
	var Base=base;
	var obj=Base.$(objId);
	new inTow({
		obj:obj
		,downObj:obj
		,type:'horizontal'
		,left:0
		,right:43
		,moveCallBack:function(x,y){
			Base.$(textId).innerHTML=1+parseInt(14/43*x)+'秒';
			variableData.playCycle=1+parseInt(14/43*x);
		}
		,downCallBack:function(){
			stopPic();
		}
		,upCallBack:function(){
			playPic();
			setTreeCookie('playTime',variableData.playCycle);
			playId='';
			getPagePlayParameter();
		}
	})
}
var playPicInt=null;
function playPic(){
	playPicInt=setInterval(function(){RedirectUrl(null,'1');},variableData.playCycle*1000);
	base.$('PlayBox').className='pause';
}
function stopPic(){
	clearInterval(playPicInt);
	playPicInt=null;
	base.$('PlayBox').className='play';
	
}
function tagPlay(){
	if(playPicInt){
		stopPic();
		setTreeCookie('playState',0);
	}else{
		playPic();
		setTreeCookie('playState',1);
	}
}