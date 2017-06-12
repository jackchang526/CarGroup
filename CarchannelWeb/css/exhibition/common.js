function addLoadEvent(func) {
  var oldonload = window.onload;
  if (typeof window.onload != 'function') {
	window.onload = func;
	}else{
	window.onload = function() {
    oldonload();
    func();
    }
  }
}

function addClass(element,value) {
  if (!element.className) {
    element.className = value;
  } else {
    newClassName = element.className;
    newClassName+= " ";
    newClassName+= value;
    element.className = newClassName;
  }
}

function removeClass(element, value){
  var removedClass = element.className;
  var pattern = new RegExp("(^| )" + value + "( |$)");
  removedClass = removedClass.replace(pattern, "$1");
  removedClass = removedClass.replace(/ $/, "");
  element.className = removedClass;
  return true;
}

/*className */

function getElementsByClass(searchClass,node,tag) {
 var classElements = new Array();
 if ( node == null )
  node = document;
 if ( tag == null )
  tag = '*';
 var els = node.getElementsByTagName(tag);
 var elsLen = els.length;
 var pattern = new RegExp("(^|\\s)"+searchClass+"(\\s|$)");
 for (var i = 0, j = 0; i < elsLen; i++) {
  if ( pattern.test(els[i].className) ) {
   classElements[j] = els[i];
   j++;
  }
 }
 return classElements;
}

/*nextSibling*/
function get_nextSibling(n){
	var y = n.nextSibling;
	while (y.nodeType != 1) {
		y = y.nextSibling;
	}
	return y;	
}
/*firstChild*/
function get_firstChild(n){
	var y = n.firstChild;
	while (y.nodeType != 1) {
		y = y.nextSibling;
	}
	return y;		
}
/*lastChild*/
function get_lastChild(n){
	var y = n.lastChild;
	while (y.nodeType != 1) {
		y = y.previousSibling;
	}
	return y;		
}

/*previousSibling*/
function get_previousSibling(n){
	var y=n.previousSibling;
	while (y.nodeType!=1){
	y=y.previousSibling;
	}
	return y;
}


/*=======================tab=============================*/

function hide(id){var Div = document.getElementById(id);if(Div){Div.style.display="none"}}  
function show(id){var Div = document.getElementById(id);if(Div){Div.style.display="block"}}  

function tabsRemove(index,head,divs,div2s) { 		
	if (!document.getElementById(head)) return false;
	var tab_heads = document.getElementById(head);
	if (tab_heads) {
		var alis = tab_heads.getElementsByTagName("li");  
		for(var i=0;i<alis.length;i++){
			removeClass(alis[i], "current");
			
			hide(divs+"_"+i);
			if(div2s){hide(div2s+"_"+i)};

			if (i==index) {
				addClass(alis[i],"current");
			}
			}
	
			show(divs+"_"+index);
			if(div2s){show(div2s+"_"+index)};
		}
}



function tabs(head,divs,div2s,over){
	if (!document.getElementById(head)) return false;
	var tab_heads=document.getElementById(head);
	
	if (tab_heads) {
	   var alis=tab_heads.getElementsByTagName("li");
	   for(var i=0;i<alis.length;i++) {
		alis[i].num=i;
		
		
		if(over){
				alis[i].onmouseover = function(){
					var thisobj = this;
					thetabstime = setTimeout(function(){changetab(thisobj);},150);
					}
				alis[i].onmouseout = function(){
					clearTimeout(thetabstime);
					}			
		}
		else{			
					alis[i].onclick = function(){
						if(this.className == "current" || this.className == "last current"){
							changetab(this);
							return true;
						}
						else{
							changetab(this);						
							return false;
						}
					
				}
		}
		
		function changetab(thebox){
			tabsRemove(thebox.num,head,divs,div2s);			
		}
  
     } 
  }
}


function all_func(){
	//breakul('break_shangjia',5);
	tabs("IDtab1","IDbox1",null,true);
	tabs("IDtab2","IDbox2",null,true);
	tabs("IDtab3","IDbox3",null,true);
	tabs("IDtab4","IDbox4",null,true);
	tabs("IDtab5","IDbox5",null,true);
	f_div_onclick("focus_chart1");
	f_div_onclick("focus_chart2");
}

addLoadEvent(all_func)











/*==================focus=====================*/

function f_div_onmouse(){
}

function f_div_onclick(focusID){
	if (!document.getElementById(focusID)) return false;
	var focus_box = document.getElementById(focusID);
	var f_p = focus_box.getElementsByTagName("p");
	var f_em = focus_box.getElementsByTagName("em");
	var f_div = focus_box.getElementsByTagName("div");
	var f_img = focus_box.getElementsByTagName("img");
	var getc = getElementsByClass("current",focus_box,"div");
	
	getc[0].style.width=getc[0].clientWidth + "px";//load width
	
	for(var i=0;i<f_em.length;i++){

//		f_div[i].onmouseover = function(){thekey = true; };//key
//		f_div[i].onmouseout = function(){thekey = false; };//key	
//		
//		f_p[i].onmouseover = function(){thekey = true;};//key
//        f_p[i].onmouseout = function(){thekey = false;};//key 


		var theid = f_div[i].parentNode.id;
		var thei = theid.replace(/^[^\d]*(\d+)[^\d]*$/,"$1");

		f_div[i].onmouseover = function(){
				if(thei == 1){thekeys1 = true;}
				if(thei == 2){thekeys2 = true;}
			};//key
		f_div[i].onmouseout = function(){
				if(thei == 1){thekeys1 = false;}
				if(thei == 2){thekeys2 = false;}
			};//key	

		f_p[i].style.display = "none";
		f_p[0].style.display = "block";
		
		f_em[i].num = i;
		f_em[i].onclick = function(){		
			//alert(focusID);
			
			var getc = getElementsByClass("current",focus_box,"div");
			if(getc[0].style.width != bt_max_width+"px"){return false};//unable quick click
			if(getc[0]==this.parentNode.parentNode){return false};//unable click current
			widthElement(getc[0].id,bt_min_width,bt_interval);			
			widthElement(f_div[this.num].id,bt_max_width,bt_interval);
			removeClass(getc[0],"current");
			addClass(f_div[this.num],"current");
			focusbigimg(f_div[this.num],focusID)
			
			return false;
		}
	
	}

}




/*imgchange*/
function focusbigimg(obj,focusID){	
	var focus_box = document.getElementById(focusID);
	var divs = focus_box.getElementsByTagName("div");
	var ps = focus_box.getElementsByTagName("p");		
	
	for(var i=0;i<ps.length;i++){
		if (divs[i]==obj){ps[i].style.display = "block";}
		else{ps[i].style.display = "none";}
	}
}

/*wdith*/
function widthElement(elementID,final_width,interval) {
  if (!document.getElementById) return false;
  if (!document.getElementById(elementID)) return false;
  var elem = document.getElementById(elementID);
  if (elem.movement) {
    clearTimeout(elem.movement);
  }
  if (!elem.style.width) {
    elem.style.width = "0px";
  }
  var xpos = parseInt(elem.style.width);
  if (xpos == final_width) {
	 return true;	
  }  
  if (xpos < final_width) {
    var dist = Math.ceil((final_width - xpos)/3);
    xpos = xpos + dist;
  }

 if (xpos > final_width) {
    var dist = Math.ceil((xpos - final_width)/3);
    xpos = xpos - dist;
  }
  elem.style.width = xpos + "px";
  var repeat = "widthElement('"+elementID+"',"+final_width+","+interval+")";
  elem.movement = setTimeout(repeat,interval);
}

/*auto*/
function focusAuto(focusID,thekeys){
	
	if(thekeys) {return false;}//key
	
	if (!document.getElementById(focusID)) return false;
	var focus_box = document.getElementById(focusID);
	var f_div = focus_box.getElementsByTagName("div");

	var getc = getElementsByClass("current",focus_box,"div");
	li_active();
	
	var getc = getElementsByClass("current",focus_box,"div");
	focusbigimg(getc[0],focusID)
	
	function li_active(){
			//var nowID = document.getElementById("c_now");
			if(get_lastChild(focus_box).className == "current" && get_lastChild(focus_box).style.width == bt_max_width+"px" ){
					removeClass(f_div[f_div.length-1], "current");		
					addClass(f_div[0],"current")	
					widthElement(f_div[f_div.length-1].id,bt_min_width,bt_interval);
					widthElement(f_div[0].id,bt_max_width,bt_interval);
					return false;
				}
			if(getc[0].style.width == bt_max_width+"px"){
				removeClass(getc[0], "current");			
				addClass(get_nextSibling(get_nextSibling(getc[0])),"current")
				widthElement(getc[0].id,bt_min_width,bt_interval);
				widthElement(get_nextSibling(get_nextSibling(getc[0])).id,bt_max_width,bt_interval);
				}
	}
}
//addLoadEvent(focusAuto);
	
	var bt_settime1 = setInterval('focusAuto("focus_chart1",thekeys1)',3000);
	var bt_settime2 = setInterval('focusAuto("focus_chart2",thekeys2)',3000);
	var thekeys1 = false;
	var thekeys2 = false;
	var bt_interval = 30;	
	var bt_min_width = 18;
	var bt_max_width = 210;
	