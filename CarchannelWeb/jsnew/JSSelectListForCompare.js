// JScript 文件
var SelectListForCompare = {
    DataPath : "http://car.bitauto.com/car/ajaxnew/SelectListForCompare.aspx",     // 数据源位置
    PageXmlHttpRequest : null,                                                               // HttpRequest 对象
    // PageXmlDocObject : null,                                                                   // XML数据对象
    IsIE : true,
    Type : 1,  // (1：图片对比，2：评测对比)
    Level : 1,  // (1：主品牌级别，2：子品牌级别)
    SelectMasterID : 0,
    MasterSelectID : "selectForMasterSelectListSpan",
    SerialSelectID : "selectForSerialSelectListSpan",
    MasterSelectObj : null,
    SerialSelectObj : null
};

function intiPageMasterSelectList()
{
    if(document.getElementById(SelectListForCompare.MasterSelectID))
    {SelectListForCompare.MasterSelectObj=document.getElementById(SelectListForCompare.MasterSelectID);}
    if(document.getElementById(SelectListForCompare.SerialSelectID))
    {SelectListForCompare.SerialSelectObj=document.getElementById(SelectListForCompare.SerialSelectID);}
    SelectListForCompare.IsIE = CheckBrowser();
    SelectListForCompare.PageXmlHttpRequest = createXMLHttpRequest();
    if(SelectListForCompare.IsIE)
	{
	    SelectListForCompare.PageXmlHttpRequest.onreadystatechange = handleStateChangeForGetMasterSelectListForCompare;
	}
	SelectListForCompare.PageXmlHttpRequest.open("GET", SelectListForCompare.DataPath+"?type="+SelectListForCompare.Type+"&level="+SelectListForCompare.Level, false);
	SelectListForCompare.PageXmlHttpRequest.send(null);
	if(!SelectListForCompare.IsIE)
	{
	    intiMasterList(SelectListForCompare.PageXmlHttpRequest.responseText);
//	   var responseText = SelectListForCompare.PageXmlHttpRequest.responseText;
//	   if(SelectListForCompare.MasterSelectObj)
//	   {SelectListForCompare.MasterSelectObj.innerHTML =  "<select id=\"selectForMasterSelectList\" onchange=\"onchangeMasterForSelectList();\" style=\"width: 120px\">" + responseText + "</select>";}  
	}
}

function handleStateChangeForGetMasterSelectListForCompare()
{
    if(SelectListForCompare.PageXmlHttpRequest.readyState == 4) 
	{
		if(SelectListForCompare.PageXmlHttpRequest.status == 200) 
		{
		    intiMasterList(SelectListForCompare.PageXmlHttpRequest.responseText);
//		    var responseText = SelectListForCompare.PageXmlHttpRequest.responseText;
//	       if(SelectListForCompare.MasterSelectObj)
//	       {SelectListForCompare.MasterSelectObj.innerHTML = "<select id=\"selectForMasterSelectList\" onchange=\"onchangeMasterForSelectList();\" style=\"width: 120px\">" + responseText + "</select>";}  
		}
	}
}

function intiMasterList(responseText)
{
    if(SelectListForCompare.MasterSelectObj)
	{
	    SelectListForCompare.MasterSelectObj.innerHTML = "<select id=\"selectForMasterSelectList\" onchange=\"onchangeMasterForSelectList();\" style=\"width: 120px\">" + responseText + "</select>";
	    // 级联选择过
	    if(CookieForSelectListTemp.getCookie("TempSelectMasterID")) 
	    {
	       var masterSelectControl = document.getElementById("selectForMasterSelectList");
          masterSelectControl.value = CookieForSelectListTemp.getCookie("TempSelectMasterID"); 
	       onchangeMasterForSelectList();
	    } 
    }  
}

function onchangeMasterForSelectList()
{
     var selectSelectValue = document.getElementById('selectForMasterSelectList').options[ document.getElementById('selectForMasterSelectList').selectedIndex].value;
     if(selectSelectValue&&selectSelectValue>0) 
     {
       SelectListForCompare.SelectMasterID = selectSelectValue; 
       CookieForSelectListTemp.setCookie("TempSelectMasterID",SelectListForCompare.SelectMasterID);
       whenSelectMasterToGetSerialForSelectList();
     }
}

function whenSelectMasterToGetSerialForSelectList()
{
    SelectListForCompare.PageXmlHttpRequest = createXMLHttpRequest();
    if(SelectListForCompare.IsIE)
	{
	    SelectListForCompare.PageXmlHttpRequest.onreadystatechange = handleStateChangeForGetSerialSelectListForCompare;
	}
	SelectListForCompare.PageXmlHttpRequest.open("GET", SelectListForCompare.DataPath+"?type="+SelectListForCompare.Type+"&level=2&masterID="+SelectListForCompare.SelectMasterID, false);
	SelectListForCompare.PageXmlHttpRequest.send(null);
	if(!SelectListForCompare.IsIE)
	{
	   var responseText = SelectListForCompare.PageXmlHttpRequest.responseText;
	   if(SelectListForCompare.SerialSelectObj)
	   {SelectListForCompare.SerialSelectObj.innerHTML =  "<select id=\"selectForSerialSelectList\" onchange=\"onchangeSerialForSelectList();\" style=\"width: 120px\">" + responseText + "</select>";}  
	}
}

function handleStateChangeForGetSerialSelectListForCompare()
{
   if(SelectListForCompare.PageXmlHttpRequest.readyState == 4) 
	{
		if(SelectListForCompare.PageXmlHttpRequest.status == 200) 
		{
		    var responseText = SelectListForCompare.PageXmlHttpRequest.responseText;
	       if(SelectListForCompare.SerialSelectObj)
	       {SelectListForCompare.SerialSelectObj.innerHTML = "<select id=\"selectForSerialSelectList\" onchange=\"onchangeSerialForSelectList();\" style=\"width: 120px\">" + responseText + "</select>";}  
		}
	}
}

function onchangeSerialForSelectList()
{
     var selectSelectValue = document.getElementById('selectForSerialSelectList').options[ document.getElementById('selectForSerialSelectList').selectedIndex].value;
     if(selectSelectValue&&selectSelectValue>0) 
     {
       selectThisSerial(selectSelectValue,""); 
     }
}

function createXMLHttpRequest() 
{
	if (window.ActiveXObject) 
	{
		return new ActiveXObject("Microsoft.XMLHTTP");
	}
	else if (window.XMLHttpRequest) 
	{
		return new XMLHttpRequest();
	}
}

function CheckBrowser()
{  
    if (window.ActiveXObject) 
	{
	    return true;
    }
    else
    {
        return false;
    } 
}

//--------------------  Cookie  --------------------
var CookieForSelectListTemp = {
	setCookie : function(name, value)
	{
		document.cookie = name + "=" + escape(value) + "; domain=car.bitauto.com";
	},

	getCookie : function(name)
	{
		var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
		if (arr != null)
		{
			return unescape(arr[2]);
		}
		return null;
	},
	
	clearCookie : function(name)
	{
		if (CookieForCompare.getCookie(name))
		{
			 document.cookie = name + "=;domain=car.bitauto.com";
		}
	}
};