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
 for (i = 0, j = 0; i < elsLen; i++) {
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


/*onclick function*/
function focus_onclick(){	
	if (!document.getElementById("focus_num")) return false;
	var focus_box = document.getElementById("focus_num");
	var lis = focus_box.getElementsByTagName("li");
	
		for(var i=0;i<lis.length;i++){
			
			lis[i].onmouseover = function(){atuokey = true;};
			lis[i].onmouseout = function(){atuokey = false;};
			
			get_firstChild(lis[i]).onclick = function(){
					var getc = getElementsByClass("current",focus_box,"li");
					if(getc[0].style.width !="192px"){return false;}
					widthElement(getc[0].id,0,f_interval);
					widthElement(this.parentNode.id,f_width,f_interval);
					removeClass(getc[0],"current");
					addClass(this.parentNode,"current");
					focusbigimg(this.parentNode);
			}

		}
}
addLoadEvent(focus_onclick);

/*auto*/
function focusAuto(){
	
	if(atuokey) {return false;}	
	if (!document.getElementById("focus_num")) return false;
	var focus_box = document.getElementById("focus_num");
	var lis = focus_box.getElementsByTagName("li");	
	var getc = getElementsByClass("current",focus_box,"li");
	li_active();	
	var getc = getElementsByClass("current",focus_box,"li");
	focusbigimg(getc[0]);
	
	function li_active(){
			var nowID = document.getElementById("c_now");
			if(get_lastChild(focus_box).className == "current" && get_lastChild(focus_box).style.width == f_width+"px" ){
					removeClass(get_lastChild(focus_box), "current");		
					addClass(get_firstChild(focus_box),"current")	
					widthElement(get_lastChild(focus_box).id,0,f_interval);
					widthElement(get_firstChild(focus_box).id,f_width,f_interval);
					return false;
				}
			if(getc[0].style.width == f_width+"px"){
				removeClass(getc[0], "current");			
				addClass(get_nextSibling(getc[0]),"current")
				widthElement(getc[0].id,0,f_interval);
				widthElement(get_nextSibling(getc[0]).id,f_width,f_interval);
				}
	}
}

	var settime = setInterval('focusAuto()',3000);
	var atuokey = false;
	var f_interval = 30;
	var f_width = 192;

/*imgchange*/
function focusbigimg(obj){	
	var focus_box = document.getElementById("focus_num");
	var lis = focus_box.getElementsByTagName("li");
	
	var focus_pic = document.getElementById("focus_pic");
	var imgs = focus_pic.getElementsByTagName("img");		
	
	for(var i=0;i<imgs.length;i++){
		if (lis[i]==obj){imgs[i].style.display = "block";}
		else{imgs[i].style.display = "none";}
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



/*tree*/
function nav_tree(){	
	if (!document.getElementById("nav_tree")) return false;
	var nav_tree = document.getElementById("nav_tree");
	var tree_li = nav_tree.getElementsByTagName("li");
	var tree_a = nav_tree.getElementsByTagName("a");

for(var i=0;i<tree_a.length;i++){
		
			tree_a[i].onclick = function(){

						var cu = getElementsByClass("current",nav_tree,"a");

						if(cu.length==1){						
							removeClass(cu[0],"current");					
						}
						if(this.className==""){
							addClass(this,"open");
							addClass(this,"current");
							get_nextSibling(this).className ="";
						}
						else if(this.className=="open"){
							removeClass(this,"open");
							addClass(this,"current");
							get_nextSibling(this).className ="none";
						}
						else if(this.className=="opened"){
							addClass(this,"current");
						}
						else if(this.className=="last"){
							addClass(this,"current");
						}
						
						//if(this.getAttribute("id")){return false}//if root letter li

						//return false;
					
			}
	}
	
}
addLoadEvent(nav_tree);

/*tree_box offsetheight*/
var tree_box = document.getElementById("tree_box");
var tree_box_height = tree_box.offsetHeight;// tree top box height	

/*tree letter*/
function tree_letter(){
	
	if (!document.getElementById("tree_letter")) return false;
	var tree_l = document.getElementById("tree_letter");
	var tree_l_a = tree_l.getElementsByTagName("a");

	
	var CharList={"A":"letter_A","B":"letter_B","C":"letter_C","D":"letter_D","F":"letter_F","G":"letter_G","H":"letter_H","J":"letter_J","K":"letter_K","L":"letter_L","M":"letter_M","O":"letter_O","P":"letter_P","Q":"letter_Q","R":"letter_R","S":"letter_S","T":"letter_T","W":"letter_W","X":"letter_X","Y":"letter_Y","Z":"letter_Z"};	

	var nav_tree = document.getElementById("nav_tree_base");
	nav_tree.style.height = document.documentElement.clientHeight + nav_tree.offsetHeight - tree_box_height + "px";

	
	for(var i=0;i<tree_l_a.length;i++){
		tree_l_a[i].onclick = function(){
			
			var char=this.innerHTML;
			//alert(char);
			//alert(CharList[char]);
			scroll_up(CharList[char]);//scroll up			
			return false;

		}

		
	}

}

addLoadEvent(tree_letter);


/*scroll up*/
function scroll_up(masterID,openli){
			if (!document.getElementById(masterID)) return false;
			var letter_a = document.getElementById(masterID);
			
			//alert(letter_a);
			var scrollheight = letter_a.offsetTop;//a offsettop
			//alert(letter_a.offsetTop);
			
	
			//if(scrollheight==0){
//				scrollheight = letter_a.clientHeight;
//			}
			
	
			var scroll_num = scrollheight- tree_box_height;//different
			
		
			//alert(document.documentElement.scrollTop);
			document.documentElement.scrollTop = scroll_num;
			//alert(document.documentElement.scrollTop);
			
			
			if(document.documentElement.scrollTop!=0){
				document.documentElement.scrollTop = scroll_num;
			}
			else{//chrome
				document.body.scrollTop = scroll_num;	
			}
			if(openli){
				addClass(letter_a,"open");
				get_nextSibling(letter_a).className ="";
			}		
}

/*scroll current*/
function scroll_select(aID){
	//alert("asdfsdf");
	if (!document.getElementById(aID)) return false;
	var ID = document.getElementById(aID);
	addClass(ID,"current");
}


/*focus pic*/
function focus_pic(img_box_id,list_box_id){
	if (!document.getElementById(img_box_id)) return false;
	var pic = document.getElementById(img_box_id);
	var list = document.getElementById(list_box_id);	
	var listli = list.getElementsByTagName("li");
	var thediv = pic.getElementsByTagName("div");
	for(var i=0;i<listli.length;i++){
		listli[i].onmouseover = function(){
				theChange(this);
			}
			
	}	
	function theChange(obj){
                            for(var i=0;i<listli.length;i++){
                            if (listli[i]==obj){
                                     listli[i].className = "current";
                                     thediv[i].className = "block";
                            }
                            else{
                                     listli[i].className = "";
                                     thediv[i].className = "";
                            }
                   }
	}
}
function focus_pic_all(){
	focus_pic('lantern_pic','lantern_list');	
	focus_pic('lantern_pic_up','lantern_list_up');
}
addLoadEvent(focus_pic_all);



/*car list*/
function car_list(){
	if (!document.getElementById("pageTop")) return false;
	var tabid = document.getElementById("pageTop");	
	var tabli = tabid.getElementsByTagName("li");	
	var ctab_1 = document.getElementById("ctab_1");	
	var ctab_2 = document.getElementById("ctab_2");	
	var ctab_3 = document.getElementById("ctab_3");	
	var ctab_4 = document.getElementById("ctab_4");	
	var ctab_5 = document.getElementById("ctab_5");	
	var ctab_6 = document.getElementById("ctab_6");	
	
	for(var i=0;i<tabli.length;i++){
		tabli[i].onmouseover = function(){
				theChange(this);
			}
			
	}
	var thediv = [ctab_1,ctab_2,ctab_3,ctab_4,ctab_5,ctab_6];
	function theChange(obj){
                            for(var i=0;i<tabli.length;i++){
                            if (tabli[i]==obj){
                                     tabli[i].className = "on";
                                     thediv[i].style.display = "block";
                            }
                            else{
                                     tabli[i].className = "";
                                     thediv[i].style.display = "none";
                            }
                   }
	}
}
addLoadEvent(car_list);



/*overshow*/
function overshow(button,box){
	if (!document.getElementById(button)) return false;
	var button = document.getElementById(button);
	var box = document.getElementById(box);
	
	button.onmouseover = function(){
		box.style.display = "block";
	}
	button.onmouseout = function(){
		box.style.display = "none";
	}
	
}
function overshowall(){
	overshow('browser_help','browser_help_con');
}
addLoadEvent(overshowall)