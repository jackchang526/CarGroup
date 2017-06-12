(function(){

    var treeNav=document.getElementById("treeNav");
    var carList=document.getElementById("carList");
    var treeList=document.getElementById("treeList");
    var topBox=document.getElementById("topBox");
    var navBox=document.getElementById("navBox");
    var treeNum=document.getElementById("treeNum").getElementsByTagName("a");
    var setTreeFun=function(item){
        var winH=document.documentElement.clientHeight||document.body.clientHeight;
        var scrollTop=document.documentElement.scrollTop||document.body.scrollTop;
        var addVal=carList.offsetHeight+navBox.offsetHeight+topBox.offsetHeight;
        if(item){
            var diffHeight=winH-addVal;
        }else{
            var diffHeight=(winH-addVal)+scrollTop;
            if(scrollTop>=topBox.offsetHeight){
                diffHeight=winH-(navBox.offsetHeight+carList.offsetHeight);
            }
        }
        treeList.style.height=diffHeight+"px";
    };
    var setTreeFix=function(){
        var scrollTop=document.documentElement.scrollTop||document.body.scrollTop;
        if(scrollTop>=topBox.offsetHeight){
            treeNav.style.top=navBox.offsetHeight+"px";
            treeNav.className="treeNav treeFix";
            navBox.className="navBox navFix";
        }else{
            treeNav.style.top=0;
            treeNav.className="treeNav";
            navBox.className="navBox";
        }

    }
    var addEvent = function(obj, type, fn ) {
        if (obj.addEventListener){
            obj.addEventListener(type, fn, false);
            console.log(1);
        }else{
            obj.attachEvent( "on"+type,fn);
        }
    };
    var treeScrollFun=function(num){
        var carListObj=document.getElementById("carlist"+num);
        var numHeight=0;
        for(var i= 0,len=treeNum.length;i<len;i++){
            treeNum[i].className="";
        }
        treeNum[num].className="current";
        if(carListObj){
             for(var i=0;i<num;i++){
                 numHeight+=document.getElementById("carlist"+i).offsetHeight;
             }
            treeList.scrollTop=numHeight;
        }
    };

   setTreeFun(true);
   setTreeFix();
   addEvent(window,"scroll",setTreeFix);
   addEvent(window,"resize",function(){
       setTreeFun(false);
   });
   addEvent(window,"scroll",function(){
       setTreeFun(false);
   });
    for(var i= 0,len=treeNum.length;i<len;i++){
        (function(i){
            addEvent(treeNum[i],"click",function(){
                treeScrollFun(i);
            });
        })(i)
    }

})()