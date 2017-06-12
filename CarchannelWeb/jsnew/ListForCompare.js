// 车型及子品牌列表 添加对比用
var ListForCompare = {
    DataPath : "http://car.bitauto.com/car/ajaxnew/ListForCompare.ashx",     // 数据源位置
    // DataPath : "http://localhost:1036/CarchannelWeb.sln/ajaxnew/ListForCompare.ashx",     // 数据源位置 
    // ResourcePath : "http://192.168.0.10:8080/ued_file/_car/",                                           // IMG资源路径
    PageXmlHttpRequest : null,                                                                                             // HttpRequest 对象
    PageXmlDocObject : null,                                                                                               // XML数据对象
    PageHTMLArray : new Array(), 
    PageDivID : "ListForCompare",                                           // 页面DIV容器ID
    PageDivObject : null,  
    SerialObject : new Array(),           
    PageTagObject : new Array(),                                                                                        
    RootNodeName : "Params",                                                  // 数据根节点名
    Level1TagNodeName : "Level1Group",                                   // 数据1级节点名
    Level1TagAttributeName : "Name",                                      // 数据1级节点属性名          
    Level2TagNodeName : "Level2Group",                                  // 数据2级节点名
    Level2TagAttributeName : "Name",                                     // 数据2级节点属性名  
    Level2TagChildNodeName : "Serial",                                   // 子品牌节点名
    IsIE : true, 
    IsNeedCarList : true,                                                          // 对比时是否需要精确到车型
    IsHidList : false,
    WhenNoNeedCarSerialMethod : "javascript:selectThisSerial(#csID#,'#csName#');",    // 当不需要精确到车型时点击子品牌调用的方法
    CarListDataPath : "http://car.bitauto.com/car/ajaxnew/SerialToCarForCompare.ashx",
    // CarListDataPath : "http://localhost:1036/CarchannelWeb.sln/ajaxnew/SerialToCarForCompare.ashx", 
    CarXmlHttpRequest : null,
    CarXmlDocObject : null,
    SelectSerialID : 0,
    SelectSerialName : "",
    CarListUlObject : null,
    CarListDivObject : null,
    CarListDivXPosition : 0,
    CarListDivYPosition : 0,
    IsUseURL : true
};

// 一级标签对象
function PageTagLevel1Object(tagName,arrNodes){
    this.TagName = tagName;
    this.ArrNodes = arrNodes;
};

// 二级标签对象
function PageTagLevel2Object(tagName,arrNodes){
    this.TagName = tagName;
    this.ArrNodes = arrNodes; 
};

// 页面子品牌对象
function ListForCompareSerial(csID,csName,csShowName,csAllSpell){
    this.CsID = csID;
    this.CsName = csName;
    this.CsShowName = csShowName;
    this.CsAllSpell = csAllSpell;
};

// 不精确到车型时选择子品调用的方法
//function selectThisSerial(csID,csName)
//{
//    alert(" 可以重写此方法:selectThisSerial(csID,csName) current select csID:" + csID + " & csName:" + csName);
//     
//}

// 请求对比子品牌列表
function requestListForCompare(isNeedCar)
{
    if(isNeedCar)
    {ListForCompare.IsNeedCarList=true;}
    else
    {ListForCompare.IsNeedCarList=false;}
    // inti Page Object
    if(document.getElementById(ListForCompare.PageDivID))
    {ListForCompare.PageDivObject = document.getElementById(ListForCompare.PageDivID);} 
    
    ListForCompare.IsIE = CheckBrowser();
	ListForCompare.PageXmlHttpRequest = createXMLHttpRequest();
	if(ListForCompare.IsIE)
	{
	    ListForCompare.PageXmlHttpRequest.onreadystatechange = handleStateChangeForListForCompare;
	}
	ListForCompare.PageXmlHttpRequest.open("GET", ListForCompare.DataPath, false);
	ListForCompare.PageXmlHttpRequest.send(null);
	if(!ListForCompare.IsIE)
	{
	    ListForCompare.PageXmlDocObject = ListForCompare.PageXmlHttpRequest.responseXML;
	    parsePageXmlObjectForCompareList(); 
	}
} 

function handleStateChangeForListForCompare()
{
	if(ListForCompare.PageXmlHttpRequest.readyState == 4) 
	{
		if(ListForCompare.PageXmlHttpRequest.status == 200) 
		{
		    ListForCompare.PageXmlDocObject = ListForCompare.PageXmlHttpRequest.responseXML;
		    parsePageXmlObjectForCompareList(); 
		}
	}
}

// 解析文档
function parsePageXmlObjectForCompareList()
{
    if(ListForCompare.PageXmlDocObject)
    {
         var level1 = ListForCompare.PageXmlDocObject.getElementsByTagName(ListForCompare.RootNodeName)[0].childNodes;
         if(level1 && level1.length>0)
         {
            for(var i=0;i<level1.length;i++)
            {
                 if (level1[i].nodeType==1 && level1[i].nodeName == ListForCompare.Level1TagNodeName)
                 {
                    var pt1 = new PageTagLevel1Object();
                    try
                    {
                        pt1.TagName = level1[i].getAttributeNode(ListForCompare.Level1TagAttributeName).nodeValue;
                    }
                    catch(err)
                    {
                        pt1.TagName = "";
                    }
                    pt1ArrNodes = new Array();
                    var level2 = level1[i].getElementsByTagName(ListForCompare.Level2TagNodeName);
                    for(var j=0;j<level2.length;j++)
                    {
                        if (level2[j].nodeType==1)
                        {
                            var pt2 = new PageTagLevel2Object();
                            try
                            {
                                pt2.TagName = level2[j].getAttributeNode(ListForCompare.Level2TagAttributeName).nodeValue;
                            }
                            catch(err)
                            {
                                pt2.TagName = "";
                            }
                            pt2ArrNodes = new Array();
                            var level2ChildNode = level2[j].getElementsByTagName(ListForCompare.Level2TagChildNodeName);
                            for(var k=0;k<level2ChildNode.length;k++)
                            {
                                if (level2ChildNode[k].nodeType==1)
                                {
                                    var level2Child = new ListForCompareSerial();
                                    try
                                    {  
                                        level2Child.CsID = level2ChildNode[k].getAttributeNode("ID").nodeValue; 
                                        level2Child.CsName = level2ChildNode[k].getAttributeNode("Name").nodeValue; 
                                        level2Child.CsShowName = level2ChildNode[k].getAttributeNode("ShowName").nodeValue; 
                                        level2Child.CsAllSpell = level2ChildNode[k].getAttributeNode("CsAllSpell").nodeValue; 
                                        pt2ArrNodes.push(level2Child); 
                                    }
                                    catch(err)
                                    {}    
                                } 
                            }
                            pt2.ArrNodes = pt2ArrNodes;
                            pt1ArrNodes.push(pt2);
                        }
                    }
                    pt1.ArrNodes = pt1ArrNodes;
                    ListForCompare.PageTagObject.push(pt1); 
                 } 
            }
         }  
         showHTMLForCompareList();
    } 
}

// 取子品牌的车型数据
function getCarDataBySerial(csID,csName,objID)
{
    if(objID!="")
    {
        var obj = document.getElementById(objID);
        if(obj)
        {
            var x = obj.offsetLeft,y = obj.offsetTop;      
            var xOffset = obj.offsetWidth;
            var yOffset = obj.offsetHeight;
            while(obj=obj.offsetParent)    
            {    
               x   +=   obj.offsetLeft;      
               y   +=   obj.offsetTop;   
            } 
            ListForCompare.CarListDivXPosition = x + xOffset/3;
            ListForCompare.CarListDivYPosition = y + yOffset/2; 
        }  
    } 
    if(csID<1)
    {return;}  
    ListForCompare.SelectSerialID = csID;
    ListForCompare.SelectSerialName = csName.replace("'","‘").replace("\"","“");
    ListForCompare.CarXmlHttpRequest = createXMLHttpRequest();
	if(ListForCompare.IsIE)
	{
	    ListForCompare.CarXmlHttpRequest.onreadystatechange = handleStateChangeForGetCarListBySerial;
	}
	ListForCompare.CarXmlHttpRequest.open("GET", ListForCompare.CarListDataPath, false);
	ListForCompare.CarXmlHttpRequest.send(null);
	if(!ListForCompare.IsIE)
	{
	    ListForCompare.CarXmlDocObject = ListForCompare.CarXmlHttpRequest.responseXML;
	    parsePageXmlObjectForGetCarListBySerial(); 
	}
}

function handleStateChangeForGetCarListBySerial()
{
    if(ListForCompare.CarXmlHttpRequest.readyState == 4) 
	{
		if(ListForCompare.CarXmlHttpRequest.status == 200) 
		{
		    ListForCompare.CarXmlDocObject = ListForCompare.CarXmlHttpRequest.responseXML;
		    parsePageXmlObjectForGetCarListBySerial(); 
		}
	}
}

// 解析子品牌->车型数据
function parsePageXmlObjectForGetCarListBySerial()
{
    ListForCompare.CarListDivObject = document.getElementById("pop_compare_forcarlist");  
    ListForCompare.CarListUlObject = document.getElementById("pop_compare_forcarlist_ul");  
    if(!ListForCompare.CarListUlObject || !ListForCompare.CarListDivObject)
    {return;}  
    if(ListForCompare.CarXmlDocObject)
    {  
        var serialNode;
        if(ListForCompare.IsIE)
        {
            serialNode = ListForCompare.CarXmlDocObject.documentElement.selectNodes("/Params/Serial[@CsID=\""+ListForCompare.SelectSerialID+"\"]"); 
        }
        else
        {
            serialNode = selectSingleNodeForListCompare(ListForCompare.CarXmlDocObject, "/Params/Serial[@CsID=\""+ListForCompare.SelectSerialID+"\"]");
        }     
        if(serialNode)
        {
            var carsNode; 
            if(ListForCompare.IsIE)
            { 
                 if(serialNode.length<1||!serialNode[0].childNodes)
                 {return;}
                carsNode = serialNode[0].childNodes;
            }
            else
            {
                 if(!serialNode.childNodes)
                 {return;}
                carsNode = serialNode.childNodes; 
            }   
            var carsNadeHTML = new Array(); 
            for(var i=0;i<carsNode.length;i++)
            {
                if (carsNode[i].nodeType==1)
                {
                    var carID = carsNode[i].getAttributeNode("CarID").nodeValue;
                    var carName = carsNode[i].getAttributeNode("CarName").nodeValue.replace("'","’").replace("\"","”");
                    var carNameNoYear = "";
                    if(carName.length>3)
                    {
                        var tempyear = carName.substr(carName.length-3,2);
                        var tempName = carName.substr(carName.length-1,1);
                        if(tempName=="款"&& !isNaN(tempyear))
                        {carNameNoYear=carName.substring(0,carName.length-3);}
                        else
                        {carNameNoYear=carName;}
                    }
                    else
                    {carNameNoYear=carName;}
                    carsNadeHTML.push("<li><a href=\"javascript:addCarToCompareFromlist("+carID+",'"+carNameNoYear+"','"+ListForCompare.SelectSerialName+"');\" class=\"c\"></a>");
                    carsNadeHTML.push("<a href=\"javascript:addCarToCompareFromlist("+carID+",'"+carNameNoYear+"','"+ListForCompare.SelectSerialName+"');\">");
                    carsNadeHTML.push(carsNode[i].getAttributeNode("CarName").nodeValue+"</a></li>");
                } 
            }  
            ListForCompare.CarListUlObject.innerHTML = carsNadeHTML.join("");   
            showCarListDivAndResetPosition(true,ListForCompare.CarListDivObject.id); 
            if(ListForCompare.IsIE) 
            { 
                ListForCompare.CarListDivObject.style.top = ListForCompare.CarListDivYPosition;
                ListForCompare.CarListDivObject.style.left = ListForCompare.CarListDivXPosition; 
            }
            else
            {
                ListForCompare.CarListDivObject.style.top = ListForCompare.CarListDivYPosition + "px";
                ListForCompare.CarListDivObject.style.left = ListForCompare.CarListDivXPosition + "px";
            }        
        }  
    }    
}

// 添加车型到对比
function addCarToCompareFromlist(carID,carName,csName)
{
    showCarListDivAndResetPosition(false,ListForCompare.CarListDivObject.id); 
    if(carID>0)
    {
        // alert("car_id:"+carID+" car_name:"+carName+" cs_name:"+csName);
        try
        {
            addCarToCompareForSelect(carID,carName,csName)
            // addCarToCompare(carID,carName,csName);   
        } 
        catch(err)
        {}  
    }  
}

// 显示或隐藏对象
function showCarListDivAndResetPosition(isShow,objID)
{
    var obj = document.getElementById(objID);
    if(obj)
    {
        if(isShow)
        {
             obj.style.display = "";
        }  
        else
        {
            obj.style.display = "none";
        }  
    } 
}

// 页面内容呈现
function showHTMLForCompareList()
{
    if(ListForCompare.PageTagObject.length>0 && ListForCompare.PageDivObject)
    {
        // ListForCompare.PageHTMLArray.push("<div class=\"line_box add_compare\">"); 
        ListForCompare.PageHTMLArray.push("<div class=\"add_compare\" style=\"float: left;\">");
        ListForCompare.PageHTMLArray.push("<div class=\"line_box\">"); 
        ListForCompare.PageHTMLArray.push("<ul class=\"tab\">");
        ListForCompare.PageHTMLArray.push("<li class=\"t\">添加对比车型</li>"); 
        for(var i=0;i<ListForCompare.PageTagObject.length;i++)
        {
            ListForCompare.PageHTMLArray.push("<li id=\"ListForCompare_li"+i+"\" class=\"\"><a href=\"javascript:pageSelectTagForList("+i+");\">"); 
            ListForCompare.PageHTMLArray.push(ListForCompare.PageTagObject[i].TagName+"</a></li>");  
        }  
        // ListForCompare.PageHTMLArray.push("<li class=\"last\"><a href=\"javascript:showOrHideElementById('ListForCompare_divContent','ListForCompare_imgShow');\"><img id=\"ListForCompare_imgShow\" src=\""+ListForCompare.ResourcePath+"images/btn-show.gif\" alt=\"展开\" title=\"展开\" />展开</a></li>");
       ListForCompare.PageHTMLArray.push("<li class=\"last\"><a class=\"show\" id=\"showOrHideElementA\" href=\"javascript:showOrHideElementById('ListForCompare_divContent');\">展开</a></li>"); 
        ListForCompare.PageHTMLArray.push("</ul>");
         ListForCompare.PageHTMLArray.push("<div id=\"ListForCompare_divContent\" style=\"display:none;\" class=\"cx\"></div></div></div></div>");  
        // ListForCompare.PageHTMLArray.push("<div id=\"ListForCompare_divContent\" style=\"display:none;\" class=\"cx\"></div></div></div>");  
        // if need car List start
        if(ListForCompare.IsNeedCarList)
        {  
            ListForCompare.PageHTMLArray.push("<div id=\"pop_compare_forcarlist\" class=\"line_box pop_compare\" style=\"position:absolute;top:120px;left:500px;z-index:111;display:none;\">");
            ListForCompare.PageHTMLArray.push("<iframe style=\"z-index:-1;position:absolute;width:100%;height:500px;_filter:alpha(opacity=0);opacity=0;border-style:none;\"></iframe>");
            ListForCompare.PageHTMLArray.push("<h3>点击对比添加到列表</h3>"); 
            ListForCompare.PageHTMLArray.push("<ul id=\"pop_compare_forcarlist_ul\" ></ul><div class=\"more\"><a href=\"javascript:showCarListDivAndResetPosition(false,'pop_compare_forcarlist');\">关闭</a></div></div>");
        }    
        // if need car List end
        ListForCompare.PageDivObject.innerHTML = ListForCompare.PageHTMLArray.join("");  
        try
        {
            // if(CookieForCompare.getCookie("ActiveNewCompare")||ListForCompare.IsHidList)
            if(ListForCompare.IsUseURL||ListForCompare.IsHidList)
            {ListForCompare.IsHidList = true;}
            else
            {
              pageSelectTagForList(0); 
              ListForCompare.IsHidList = false; 
            }  
        }
        catch(err)
        {}
    } 
}

// 切换标签
function pageSelectTagForList(tagIndex)
{
    ListForCompare.IsHidList = false; 
    if(tagIndex<ListForCompare.PageTagObject.length)
    {
        // 切换标签 
        for(var i=0;i<ListForCompare.PageTagObject.length;i++) 
        {
            var tags = document.getElementById("ListForCompare_li"+i);
            if(tags)
            {
                if(i==tagIndex)
                {tags.className = "on";}  
                else
                {tags.className = "";}  
            }  
        } 
        // 更新list数据 
        if(ListForCompare.PageTagObject[tagIndex].ArrNodes.length>0)
        {
            var loopDL = 0;
            var tempArr = new Array();
            for(var j=0;j<ListForCompare.PageTagObject[tagIndex].ArrNodes.length;j++)
            {
                if(ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes.length<1)
                {continue;}
                if(loopDL%2==0)
                { 
                 if(loopDL==ListForCompare.PageTagObject[tagIndex].ArrNodes.length-1)
                 {tempArr.push("<dl class=\"no_line\">")}   
                 else
                 {tempArr.push("<dl>"); } 
                 loopDL++; 
                }
                else
                { tempArr.push("<dl class=\"j\">"); }  
                tempArr.push("<dt>"+ListForCompare.PageTagObject[tagIndex].ArrNodes[j].TagName+"</dt>");
                tempArr.push("<dd><ul>");
                for(var k=0;k<ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes.length;k++)
                {
                     // if need Car List start
                     if(ListForCompare.IsNeedCarList)
                     {
                        tempArr.push("<li id=\"serialListLi_j"+j+"_k"+k+"\"><a href=\"javascript:getCarDataBySerial("+ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsID+",'"+ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsName+"','serialListLi_j"+j+"_k"+k+"');\"");
                        tempArr.push(" title=\""+ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsShowName+"\" >")
                     }
                     // if need Car List end
                     else
                     {
                        tempArr.push("<li><a href=\""+ListForCompare.WhenNoNeedCarSerialMethod.replace("#csID#",ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsID).replace("#csName#",ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsName.replace("'","‘").replace("\"","“"))+"\"");
                        tempArr.push(" title=\""+ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsShowName+"\" >")
                     }   
                     tempArr.push(ListForCompare.PageTagObject[tagIndex].ArrNodes[j].ArrNodes[k].CsName+"</a></li>");
                }   
                tempArr.push("</ul></dd></dl>"); 
                var divContent = document.getElementById("ListForCompare_divContent"); 
                if(divContent)
                {
                    divContent.innerHTML = tempArr.join("");
                    showCarListDivAndResetPosition(true,divContent.id); 
                }
            } 
            var showOrHideElementA = document.getElementById("showOrHideElementA");
            var pageObj = document.getElementById("ListForCompare_divContent");
            if(pageObj)
            {
              pageObj.style.display = "";
              if(showOrHideElementA)
              {
                  showOrHideElementA.className = "close";
                  if(ListForCompare.IsIE)
                  {showOrHideElementA.innerText = '收起';}
                  else
                  {showOrHideElementA.textContent = '收起';}  
              }  
              if(ListForCompare.IsHidList)
              {pageSelectTagForList(0);return;}  
            } 
        }  
    } 
}

// 显示隐藏标记
function showOrHideElementById(id)
{
    var showOrHideElementA = document.getElementById("showOrHideElementA");
    var pageObj = document.getElementById(id);
    if(pageObj) 
    {
        if(pageObj.style.display == "none")
        {
          pageObj.style.display = "";
          if(showOrHideElementA)
          {
              showOrHideElementA.className = "close";
              if(ListForCompare.IsIE)
              {showOrHideElementA.innerText = '收起';}
              else
              {showOrHideElementA.textContent = '收起';}  
          }  
          if(ListForCompare.IsHidList)
          {pageSelectTagForList(0);return;}  
        } 
        else
        {
          pageObj.style.display = "none";
          if(showOrHideElementA)
          {
              showOrHideElementA.className = "show";
              if(ListForCompare.IsIE)
              {showOrHideElementA.innerText = '展开';}
              else
              {showOrHideElementA.textContent = '展开';}   
          }   
        } 
    } 
}

// xpath for ff
function selectSingleNodeForListCompare(xmldom,path)
{
     var xpe = new XPathEvaluator();
     var nsResolver = xpe.createNSResolver(xmldom.ownerDocument == null ? xmldom.documentElement : xmldom.ownerDocument.documentElement); 
     // var results = xpe.evaluate(path,xmldom,nsResolver,XPathResult.FIRST_ORDERED_NODE_TYPE, null); 
     var results = xpe.evaluate(path,xmldom,nsResolver,XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);  
     var node = results.snapshotItem(0);
     return node; 
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

