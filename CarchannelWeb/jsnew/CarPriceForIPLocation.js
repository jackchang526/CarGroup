// 车型IP定向报价

var CarPriceForIPLocation = 
{
    CarID : 0,
    CsAllSpell : "",
    PageDivID : "carIPLocationPrice",
    PageDivObj : null,
    CarID : 0,
    DataSor : "http://car.bitauto.com/carservice/CarIPLocationPrice.aspx?carid=",
    XMLRequest : null,
    IsIE : true
}

function intiPageForCarPriceIPLocation(carID,csAllSpell)
{
    if(carID<1)
    {return;}
    else
    {CarPriceForIPLocation.CarID = carID;}
    if(csAllSpell)
    {CarPriceForIPLocation.CsAllSpell = csAllSpell;}
    if(!CarPriceForIPLocation.PageDivObj)
    {
       if(document.getElementById(CarPriceForIPLocation.PageDivID))
       {CarPriceForIPLocation.PageDivObj=document.getElementById(CarPriceForIPLocation.PageDivID);}
       else
       {return;}
    }
    CarPriceForIPLocation.IsIE = CheckBrowserForCarPriceIPLocation();
    CarPriceForIPLocation.XMLRequest = createXMLHttpRequestForCarPriceIPLocation();
    if (CarPriceForIPLocation.IsIE) 
    {
         CarPriceForIPLocation.XMLRequest.onreadystatechange = handleStateChangeForCarPriceIPLocation;
    }  
    // alert(CarPriceForIPLocation.DataSor + CarPriceForIPLocation.CarID);
    CarPriceForIPLocation.XMLRequest.open("GET", CarPriceForIPLocation.DataSor + CarPriceForIPLocation.CarID, false);//"&"+Math.random()
    CarPriceForIPLocation.XMLRequest.send(null);
     if(!CarPriceForIPLocation.IsIE)
     {
         var requestText = CarPriceForIPLocation.XMLRequest.responseText;
         showForCarPriceIPLocation(requestText); 
     } 
}

function handleStateChangeForCarPriceIPLocation() 
{
	if(CarPriceForIPLocation.XMLRequest.readyState == 4 && CarPriceForIPLocation.XMLRequest.status == 200) 
	{  
	    var requestText = CarPriceForIPLocation.XMLRequest.responseText;
	    showForCarPriceIPLocation(requestText);
	}
}

function showForCarPriceIPLocation(htmlContent)
{
    if(CarPriceForIPLocation.PageDivObj)
    {
       var temp = "<dt>地方报价</dt>";
       temp += htmlContent;
       temp += "<dd class=\"choosecity\"><a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarId="+CarPriceForIPLocation.CarID+"&citycode=0\">更多城市</a></dd>";
       CarPriceForIPLocation.PageDivObj.innerHTML = "" + temp;
    }
}

function createXMLHttpRequestForCarPriceIPLocation() 
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

// 检查是否IE浏览器
function CheckBrowserForCarPriceIPLocation()
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