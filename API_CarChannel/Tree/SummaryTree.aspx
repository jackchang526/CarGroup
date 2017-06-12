<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryTree.aspx.cs" Inherits="BitAuto.CarChannelAPI.Web.Tree.SummaryTree" %>
var treeHtml = '<%=TreeHtml%>';
var treeBox = document.getElementById("leftTreeBox");
if(treeBox)
{
	treeBox.innerHTML = treeHtml;
}
function scrollToCurrentTreeNode()
{
	var curNode = document.getElementById("curObjTreeNode");
	if(!curNode)
		return;	
	var topHeight = 0;
	while(curNode && curNode.id!="treev1")
	{
		topHeight += curNode.offsetTop;
		curNode = curNode.offsetParent;
	}
	var treeBox = document.getElementById("treev1");//树
	treeBox.scrollTop = topHeight;
}
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
var setTimeoutId;
function moveElementSlide(elementID,final_x,interval){  
	if(!document.getElementById) return false;  
	if(!document.getElementById(elementID)) return false;  
	var elem = document.getElementById(elementID);  
	var xpos = parseInt(elem.style.left);
	if(xpos == final_x){  
		return true;  
	}  
	else
		xpos = final_x; 
	elem.style.left = xpos + "px";  
	var repeat = "moveElementSlide('"+elementID+"',"+final_x+","+interval+")";  
	setTimeoutId = setTimeout(repeat,interval);  
}  
function showSummaryTree(){
	var tree_btn = document.getElementById('tree_btn');
	var car_summary_mask = document.getElementById('car_summary_mask');
	var car_summary_tree = document.getElementById('car_summary_tree');
	var tree_btn_return = document.getElementById('tree_btn_return');
		
	var pageHtml = document.getElementsByTagName('html');
	var pageBody = document.getElementsByTagName('body');
	tree_btn.onclick = function(){
        clearTimeout(setTimeoutId);
		car_summary_mask.style.display = 'block';
		pageBody[0].style.overflow = 'hidden';
		// modified by chengl Dec.21.2011 For FF
		if (window.ActiveXObject) 
		{
			pageHtml[0].style.overflowY = 'hidden';
		}
		moveElementSlide("car_summary_tree", 0, 1);
	}
	tree_btn_return.onclick = function(){
        clearTimeout(setTimeoutId);
		moveElementSlide("car_summary_tree", -250, 1);
		car_summary_mask.style.display = 'none';
		pageBody[0].style.overflow = 'visible';
		// modified by chengl Dec.21.2011 For FF
		if (window.ActiveXObject) 
		{
			pageHtml[0].style.overflowY = 'scroll';
		}
	}
	car_summary_mask.onclick = function(){
        clearTimeout(setTimeoutId);
		moveElementSlide("car_summary_tree", -250, 1);
		car_summary_mask.style.display = 'none';
		pageBody[0].style.overflow = 'visible';
		// modified by chengl Dec.21.2011 For FF
		if (window.ActiveXObject) 
		{
			pageHtml[0].style.overflowY = 'scroll';
		}
	}
}
scrollToCurrentTreeNode();
showSummaryTree();
function expandMaster(obj, id)
{
    if(obj.parentNode.childNodes.length>1)
    {
        window.open("/"+obj.getAttribute("ap"));
    }
    else
    {
        var currentNode = document.getElementById("curObjTreeNode");
        if(currentNode!=null)
        {
            currentNode.removeAttribute("id");
            if(currentNode.getAttribute("t")==null){
                do{
                    currentNode = currentNode.parentNode;
                }while(currentNode.getAttribute("t")==null);
            }
            currentNode.childNodes[0].className = "mainBrand";
            for(var i=currentNode.childNodes.length-1;i>=1;i--)
            {
                currentNode.removeChild(currentNode.childNodes[i]);
            }
        }

        obj.parentNode.setAttribute("id", "curObjTreeNode");
        obj.className = "mainBrand current current_unfold";

        var oHead = document.getElementsByTagName('HEAD')[0];
        var oScript = document.createElement("script");
        oScript.type = "text/javascript";
        oScript.src = "http://api.car.bitauto.com/Tree/SummaryTreeNode.aspx?masterid="+id;
        oScript.charset = "utf-8";
        oHead.appendChild(oScript);
    }
}