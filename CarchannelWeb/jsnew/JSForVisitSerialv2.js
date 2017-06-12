// JScript 文件

var VisitSerialObject = {
    DataURL : "/car/Ajaxnew/GetVisitSerial.aspx",
    ArrVisitSerial:new Array(),
    ArrVisitSerialForShow: new Array(),
    CurrentCSID: 0,
    MaxVisitLength: 10,
    XmlHttpForVisitSerial : null,
    IsIE : true,
    PageDivID : "ulForVisitSerial"
}

function initPageForVisitSerial(csID)
{
    VisitSerialObject.IsIE = CheckBrowserForVisitSerial();
    VisitSerialObject.CurrentCSID = csID;
    var visitSerial_msg = CookieForVisitSerial.getCookie("visitSerial");
	 if (!visitSerial_msg)
	 {
	    if(VisitSerialObject.CurrentCSID<=0)
	    {return;} 
		// 无浏览过子品牌
	    CookieForVisitSerial.setCookie("visitSerial", "id" + VisitSerialObject.currentCSID);  
	 }
	 else
	 {
	    // 浏览过子品牌
	     var visitSerial_msg_arr = visitSerial_msg.split("|"); 
        if(visitSerial_msg.indexOf("id" +VisitSerialObject.CurrentCSID)<0)
        {
           if(VisitSerialObject.CurrentCSID>0)
           {
	            if(visitSerial_msg_arr.length < VisitSerialObject.MaxVisitLength)
                {	    
                     // 不足上线数 
                     visitSerial_msg_arr.reverse();
                     visitSerial_msg_arr.push("id" + VisitSerialObject.CurrentCSID);
                     visitSerial_msg_arr.reverse(); 
                } 
                else
                {
                     visitSerial_msg_arr.pop();
                     visitSerial_msg_arr.reverse();
                     visitSerial_msg_arr.push("id" + VisitSerialObject.CurrentCSID);
                     visitSerial_msg_arr.reverse(); 
                }
            }    
            
            var maxCount = visitSerial_msg_arr.length > VisitSerialObject.MaxVisitLength ? VisitSerialObject.MaxVisitLength:visitSerial_msg_arr.length; 
            for(var i=0;i<maxCount;i++)
            {   
                 VisitSerialObject.ArrVisitSerial.push(visitSerial_msg_arr[i]);
            } 
            CookieForVisitSerial.clearCookie("visitSerial"); 
	         CookieForVisitSerial.setCookie("visitSerial", VisitSerialObject.ArrVisitSerial.join("|")); 
        }
	}	
   startRequestForVisitSerial(VisitSerialObject.DataURL);
}

//--------------------  Ajax  --------------------

function createXMLHttpRequestForVisitSerial() 
{
	if (window.ActiveXObject) 
	{
		xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	else if (window.XMLHttpRequest) 
	{
		xmlHttp = new XMLHttpRequest();
	}
	return xmlHttp;
}
function startRequestForVisitSerial(xmlUrl) 
{
	    VisitSerialObject.XmlHttpForVisitSerial = createXMLHttpRequestForVisitSerial();
	    if (VisitSerialObject.IsIE) 
	    {
	         VisitSerialObject.XmlHttpForVisitSerial.onreadystatechange = handleStateChangeForVisitSerial;
	    }  
	    VisitSerialObject.XmlHttpForVisitSerial.open("GET", xmlUrl + "?" + Math.random(), false);
	    VisitSerialObject.XmlHttpForVisitSerial.send(null);
        if(!VisitSerialObject.IsIE)
        {
            var requestText = VisitSerialObject.XmlHttpForVisitSerial.responseText;
            showVisitSerial(requestText); 
        } 
}
function handleStateChangeForVisitSerial() 
{
	if(VisitSerialObject.XmlHttpForVisitSerial.readyState == 4 && VisitSerialObject.XmlHttpForVisitSerial.status == 200) 
	{  
	    var requestText = VisitSerialObject.XmlHttpForVisitSerial.responseText;
	    showVisitSerial(requestText);
	}
}

// 显示浏览过子品牌
function showVisitSerial(request)
{
    var divObj = document.getElementById(VisitSerialObject.PageDivID);
    if(!divObj)
    {return;}
    if(request && request != "")
    { 
        var arrSerial = request.split("|");
        if(arrSerial.length>0)
        {
            for(var i=0;i<arrSerial.length;i++)
            { 
                VisitSerialObject.ArrVisitSerialForShow.push("<li><a href=\"/"+arrSerial[i].split("^")[2]+"/\" target=\"_blank\" >"+arrSerial[i].split("^")[1]+"</a></li>");  
            } 
        }
        divObj.innerHTML = VisitSerialObject.ArrVisitSerialForShow.join("");
    }     
}

// 检查是否IE浏览器
function CheckBrowserForVisitSerial()
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
var CookieForVisitSerial = {
	setCookie : function(name, value, expires, path, domain, secure)
	{
        expiryday=new Date();
        expiryday.setTime(expiryday.getTime()+30*30*24*60*60*1*1000);
		document.cookie = name + "=" + escape(value) + "; expires=" + expiryday.toGMTString() + 
			((path) ? "; path=" + path : "; path=/") +
			"; domain=.bitauto.com" +
			((secure) ? "; secure" : "");
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
	
	clearChildCookie : function(name, path, domain)
	{
		if (CookieForVisitSerial.getCookie(name))
		{
			 document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=car.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
		}
	},

	clearCookie : function(name, path, domain)
	{
		if (CookieForVisitSerial.getCookie(name))
		{
			 document.cookie = name + "=" +
				((path) ? "; path=" + path : "; path=/") +
				"; domain=.bitauto.com" +
				";expires=Fri, 02-Jan-1970 00:00:00 GMT";
		}
	}
};