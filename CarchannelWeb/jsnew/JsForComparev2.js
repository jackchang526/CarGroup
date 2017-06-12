// 车型频道对比

var ComparePageObject = {
    CarInfoPath : "http://car.bitauto.com/car/ajaxnew/GetCarInfoForCompare.ashx?carID=",    // ajax path
    // CarInfoPath : "http://localhost:1036/CarchannelWeb.sln/ajaxnew/GetCarInfoForCompare.ashx?carID=",    // ajax path 
    RootPath : "",    // root path
    ResourceDIR : "", // image dir
    PageDivContentID : "CarCompareContent",  // container id
    PageDivContentObj : null,   // container object
    IsIE : true, // client browser
    IsDelSame : false,  // is delete the same param
    IsDeployAll : false, 
    IsOperateTheSame : false, 
    XmlSrcLength : 0,
    XmlHttpForCompare : null, 
    ArrCarInfo : new Array(),
    ArrPageContent : new Array(), 
    ValidCount : 0,
    MaxTDLeft : 6,
    IsNeedSecTH : false,
    IsNeedSelect : true,
    NeedBlockTD : 0,
    // MasterToCarDataPath : "http://localhost:1036/CarchannelWeb.sln/ajaxnew/GetMasterToCar.ashx",
    MasterToCarDataPath : "http://car.bitauto.com/car/ajaxnew/GetMasterToCar.ashx", 
    PageObjectForSelect : new Array(), 
    XmlHttpForSelect : null,
    XmlObjectForSelect : null,
    ListForMaster : "",
    ListForSerial : "",
    ListForCar : "",
    SelectType : 1,
    SelectCSName : "",
    TableWidth : 1634,
    IsNeedDrag : true,        
    IsUseURL : true,
    CarInfos : "",
    CarIDsForURL : ""
}

// 车型对比信息
function CarCompareInfo(carid,carName,xmlHttpObject,isValid,carInfoXML)
{
    this.CarID = carid;
    this.CarName = carName;
    this.XmlHttpObject = xmlHttpObject;
    this.IsValid = isValid;
    this.CarInfoXML = carInfoXML; 
}

// 主品牌信息
function MasterBrandInfoSelectListForLevel(masterID,masterName,masterFirstSpell,arrSerialInfo)
{
    this.MasterID = masterID;
    this.MasterName = masterName; 
    this.MasterFirstSpell = masterFirstSpell;
    this.ArrSerialInfo = arrSerialInfo; 
}

// 子品牌信息
function SerialInfoSelectListForLevel(csID,csName,csShowName,csAllSpell,arrCarInfo)
{
    this.CsID = csID;
    this.CsName = csName;
    this.CsShowName = csShowName; 
    this.CsAllSpell = csAllSpell;
    this.ArrCarInfo = arrCarInfo; 
}

// 车型信息
function CarInfoSelectListForLevel(carID,carName,carYear)
{
    this.CarID = carID;
    this.CarName = carName;
    this.CarYear = carYear;  
}

// 显示对比
function initPageForCompare(root)
{
    // window.location.hash = "foobar";
    // window.location.search = "foobar";
    // alert(ComparePageObject.IsUseURL);
    
    ComparePageObject.ArrCarInfo.length = 0;
    ComparePageObject.ArrPageContent.length = 0; 
    ComparePageObject.ValidCount = 0; 
    
    var pageDiv = document.getElementById(ComparePageObject.PageDivContentID);
    if(pageDiv)
    {ComparePageObject.PageDivContentObj = pageDiv;}  
    else
    {return;}  
    ComparePageObject.IsIE = CheckBrowserForCompare();
    ComparePageObject.RootPath = root;
    
    var compare_msg;//"id9091,aaa|id6996,dfdf"; 
    // URL
    if(ComparePageObject.IsUseURL)
    {
       compare_msg = ComparePageObject.CarInfos;
    }
    else
    {
        // 检查是否主动对比
        if(checkCurrentActive())
        {
            // compare_msg = CookieForCompare.getCookie("ActiveNewCompare");
        }
        else
        { 
            compare_msg = CookieForCompare.getCookie("PassiveNewCompare");
            // 清除被动对比
            CookieForCompare.clearCookie("isPassiveCompare"); 
        } 
    }
    // alert(compare_msg); 
    if (!compare_msg)
	{
		// 无车型 
    }
    else
    {
        var compare_msg_arr = compare_msg.split("|");
        ComparePageObject.XmlSrcLength = compare_msg_arr.length;  
    }  		
    for (var i=0; i<ComparePageObject.XmlSrcLength; i++)
    {
        var carCookie = compare_msg_arr[i].split(",");
	    var carid = carCookie[0].substring(2,carCookie[0].length);
	    var carName =  carCookie[1]; 
	    var carinfo = new CarCompareInfo(); 
	    carinfo.CarID = carid;
	    carinfo.CarName = carName;  
	    // ComparePageObject.ArrCarInfo.push(carinfo);
	    startRequestForCompare(carinfo);
    } 
   
//    if(ComparePageObject.ValidCount>0)
//    {
        // createPageForCompare(false);
        resetTableWidth();
        createPageForCompare(false); 
//    } 
}

// page method --------------------------

// 取需要对比车数据
function startRequestForCompare(carInfo) 
{ 
    carInfo.XmlHttpObject = createXMLHttpRequestForCompare(); 
    if(ComparePageObject.IsIE)
    {   
        carInfo.XmlHttpObject.onreadystatechange = function(){handleStateChangeForGetCarCompareData(carInfo)}; 
    }  
    carInfo.XmlHttpObject.open("GET", ComparePageObject.CarInfoPath + carInfo.CarID, false);  
    carInfo.XmlHttpObject.send(null);
    if(!ComparePageObject.IsIE)
     {
        addCarXmlObjectToArray(carInfo,carInfo.XmlHttpObject.responseXML);
     }  
}

function handleStateChangeForGetCarCompareData(carInfo) 
{
	if(carInfo.XmlHttpObject.readyState == 4 && carInfo.XmlHttpObject.status == 200) 
	{  
	    addCarXmlObjectToArray(carInfo,carInfo.XmlHttpObject.responseXML);
	}
}	

function addCarXmlObjectToArray(carInfo,xmlObj)
{
    carInfo.CarInfoXML = xmlObj; 
    carInfo.IsValid = true;
    ComparePageObject.ArrCarInfo.push(carInfo); 
    ComparePageObject.ValidCount++; 
}

function createPageForCompare(isDelSame)
{
    ComparePageObject.IsDelSame = isDelSame;
     
    if(ComparePageObject.IsNeedSelect)
    {
        ComparePageObject.NeedBlockTD = ComparePageObject.ValidCount >=6 ? 1:6-ComparePageObject.ValidCount;
        if(ComparePageObject.ValidCount==10)
        {ComparePageObject.NeedBlockTD=0;}  
    }
    else
    {ComparePageObject.NeedBlockTD=0;}    
    ComparePageObject.IsNeedSecTH = ComparePageObject.ValidCount >=6 ? true:false;    
    var loopCount = arrField.length; 
    ComparePageObject.ArrPageContent.push("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" id=\"compareTable\" style=\"width: "+ComparePageObject.TableWidth+"px;\">");
    for(var i = 0;i<loopCount;i++)
    {
        switch (arrField[i].sType)
		{
		    case "fieldPara":
		    if(ComparePageObject.ValidCount>0) createPara(arrField[i]);
		    break; 
		    case "fieldMulti":
		    if(ComparePageObject.ValidCount>0) createMulti(arrField[i]);
		    break;  
		    case "bar":
		    if(ComparePageObject.ValidCount>0) createBar(arrField[i]);
		    break;  
	        case "fieldPrice":
		    if(ComparePageObject.ValidCount>0) createPrice(arrField[i]);
		    break;  
		    case "fieldPic":
		    createPic();
		    break;    
		}
    }
    
    ComparePageObject.ArrPageContent.push("</table>");  
    // end
    ComparePageObject.ArrPageContent.push("<div class=\"car_compare_listbtn\">");   
    ComparePageObject.ArrPageContent.push("<ul>");    
//    ComparePageObject.ArrPageContent.push("<li class=\"s\"><a href=\"javascript:showHidAll();\">收起列表</a></li>");    
//    ComparePageObject.ArrPageContent.push("<li class=\"d\"><a href=\"\">打印列表</a></li>");    
//    ComparePageObject.ArrPageContent.push("<li class=\"b\"><a href=\"\">保存列表</a></li>");   
    ComparePageObject.ArrPageContent.push("</ul><div class=\"clear\"></div></div>");    
   
    if(ComparePageObject.PageDivContentObj)      
    {
         ComparePageObject.PageDivContentObj.innerHTML = ComparePageObject.ArrPageContent.join("");
    } 
    ComparePageObject.ArrPageContent.length = 0;
    if(ComparePageObject.IsNeedSelect)
    {
        if(ComparePageObject.XmlObjectForSelect)
        {
            // not first page load 
            // parseXMLForSelectControl(); 
        }
        else
        {    
            intiPageSelectControl(); 
        }      
    }  
}

// create pic for compare
function createPic()
{
    ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" class=\"pic_tr\">"); 
    ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置"); 
    if(ComparePageObject.ValidCount>0)
    {ComparePageObject.ArrPageContent.push("<p class=\"cy\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" /> 只显示差异项目</p>");}
    else
    {ComparePageObject.ArrPageContent.push("<p class=\"cy\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" disabled=\"disabled\" onclick=\"delTheSameForCompare();\" /> 只显示差异项目</p>");}  
    ComparePageObject.ArrPageContent.push("</th>"); 
   
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
         // insert Sec TH 
         if(i==ComparePageObject.MaxTDLeft)
         {
            ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置"); 
            ComparePageObject.ArrPageContent.push("<p class=\"cy\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" /> 只显示差异项目</p>");
            ComparePageObject.ArrPageContent.push("</th>");      
         }  
         if(ComparePageObject.ArrCarInfo[i].CarInfoXML)
         {
            if(i==0)
            {ComparePageObject.ArrPageContent.push("<td id=\"td_"+i+"\" onMouseOver=\"checkIsChange(this);\" class=\"f\">");}  
            else
            {ComparePageObject.ArrPageContent.push("<td id=\"td_"+i+"\" onMouseOver=\"checkIsChange(this);\">");}  
             try
             {
                var pic = "";
                if(ComparePageObject.IsIE)
                { pic = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/CarImg/@PValue").value;}
                else
                {pic = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/CarImg/@PValue");}   
                if(pic.length<1)
                {ComparePageObject.ArrPageContent.push("<div id=\"divImg_"+i+"\" class=\"compare_pic\"><img id=\"img_"+i+"\" name=\"dragImg\" width=\"120\" height=\"80\" src=\"http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif\" alt=\"按住鼠标左键，可拖动到其他列\" /></div>"); }
                else
                {ComparePageObject.ArrPageContent.push("<div id=\"divImg_"+i+"\" class=\"compare_pic\"><img id=\"img_"+i+"\" name=\"dragImg\" width=\"120\" height=\"80\" src=\""+pic+"\" alt=\"按住鼠标左键，可拖动到其他列\" /></div>"); }    
                ComparePageObject.ArrPageContent.push("<div><a href=\"javascript:resetCompareCar('"+ComparePageObject.ArrCarInfo[i].CarID+"');\" class=\"g\"></a><a href=\"javascript:delCarToCompare('"+ComparePageObject.ArrCarInfo[i].CarID+"')\" class=\"del\"></a></div>");
             }
             catch(err)
             {}
            ComparePageObject.ArrPageContent.push("</td>");
         } 
    }  
   
    //when less 
    if(ComparePageObject.NeedBlockTD>0)
    {
         for(var i=0;i<ComparePageObject.NeedBlockTD;i++)
         {
             if((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft)
             {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th width=\"146\"><h2>车型图片</h2>左右拖动图片可改变车型在列表中的位置"); 
                ComparePageObject.ArrPageContent.push("<p class=\"cy\"><input type=\"checkbox\" name=\"checkboxForDelTheSame\" onclick=\"delTheSameForCompare();\" /> 只显示差异项目</p>");
                ComparePageObject.ArrPageContent.push("</th>");      
             } 
             ComparePageObject.ArrPageContent.push("<td><div class=\"compare_blank\"><span>PK</span></div>"); 
             if(i==0)
             {     
                ComparePageObject.ArrPageContent.push("<div id=\"selectForMaster\"><select onchange=\"onchangeMasterForSelect();\"><option>选择品牌</option></select></div>");      
                ComparePageObject.ArrPageContent.push("<div id=\"selectForSerial\"><select onchange=\"onchangeSerialForSelect();\"><option>选择车型</option></select></div>");  
                ComparePageObject.ArrPageContent.push("<div id=\"selectForCar\"><select onchange=\"onchangeCarForSelect();\"><option>选择车款</option></select></div>");  
             }
             else
             {
                ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\"><option>选择品牌</option></select>");      
                ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\"><option>选择车型</option></select>");  
                ComparePageObject.ArrPageContent.push("<select disabled=\"disabled\"><option>选择车款</option></select>");  
             }
             ComparePageObject.ArrPageContent.push("</td>");
         } 
    }  
     
    ComparePageObject.ArrPageContent.push(""); 
    ComparePageObject.ArrPageContent.push("</tr>"); 
}

// create param for compare
function createPara(arrFieldRow)
{    
    var firstSame = true;   
    var isAllunknown = true;
    var arrSame = new Array(); 
    var arrTemp = new Array(); 
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        // insert Sec TH 
         if(i==ComparePageObject.MaxTDLeft)
         {
             arrTemp.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");  
         }   
         if(ComparePageObject.ArrCarInfo[i].CarInfoXML)
         {
            if(i==0)
            {
                arrTemp.push("<td class=\"f\">");
            }  
            else
            {
                arrTemp.push("<td>"); 
            }  
            
            try
            {
                var field = ""; 
                if(ComparePageObject.IsIE)
                {
                    field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/"+arrFieldRow["sFieldName"]+"/@PValue").value;
                    if(field.length>0)
                    {field+=" " + arrFieldRow["unit"];}  
                    if(arrSame.length<1)
                    {arrSame.push(field);}   
                    else
                    {
                        if(field == arrSame[0])
                        {firstSame = firstSame && true;}
                        else
                        {firstSame = firstSame && false;} 
                    }  
                }    
                else
                {
                    field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/"+arrFieldRow["sFieldName"]+"/@PValue");
                    if(arrSame.length<1)
                    {arrSame.push(field);}   
                    else
                    {
                        if(field == arrSame[0])
                        {firstSame = firstSame && true;}
                        else
                        {firstSame = firstSame && false;} 
                    }  
                }  
                if(field.indexOf("待查")>=0)
                {
                    isAllunknown = true && isAllunknown;
                }
                else
                {
                    isAllunknown = false;
                }
                arrTemp.push(field);  
                // modified by chengl Dec.28.2009 for calculator
                if(arrFieldRow["sFieldName"]=="CarReferPrice"&&field!="无"&&field!="待查")
                {
                  arrTemp.push("<a class=\"icon_cal\" title=\"购车费用计算\" href=\"http://car.bitauto.com/gouchejisuanqi/?carid="+ComparePageObject.ArrCarInfo[i].CarID+"\"  target=\"_blank\"></a>"); 
                }
            } 
            catch(err)
            {
                arrTemp.push("-");   
                firstSame = firstSame && false;   
            }   
            arrTemp.push("</td>");  
            // ComparePageObject.ArrPageContent.push(field+"</td>"); 
         }
         else
        {
             arrTemp.push("<td>null");
             arrTemp.push("</td>");  
         }  
    }
    // alert(firstSame + " " + ComparePageObject.IsDelSame); 
    if(firstSame && ComparePageObject.IsDelSame)
    {
        return;
    }
    else
    {
        if(!isAllunknown)
        { 
            ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr"+arrFieldRow["sTrPrefix"]+"_"+arrFieldRow["sFieldName"]+"\"><th>");
            ComparePageObject.ArrPageContent.push(arrFieldRow["sFieldTitle"]);
            ComparePageObject.ArrPageContent.push("</th>");
            ComparePageObject.ArrPageContent.push(arrTemp.join(""));  
        } 
        else
        {
            return; 
        }  
    }    
   
    //when less 
    if(ComparePageObject.NeedBlockTD>0)
    {
         for(var i=0;i<ComparePageObject.NeedBlockTD;i++)
         {
             if((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft)
             {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");      
             } 
             ComparePageObject.ArrPageContent.push("<td>&nbsp;");  
             ComparePageObject.ArrPageContent.push("</td>");
         } 
    }     
    ComparePageObject.ArrPageContent.push("</tr>");  
}

// create multi param for compare
function createMulti(arrFieldRow)
{
    ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr"+arrFieldRow["sTrPrefix"]+"_"+arrFieldRow["sFieldName"]+"\">");
    ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        // insert Sec TH 
         if(i==ComparePageObject.MaxTDLeft)
         {
            ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");  
         }   
         if(ComparePageObject.ArrCarInfo[i].CarInfoXML)
         {
            if(i==0)
            {ComparePageObject.ArrPageContent.push("<td class=\"f\">");}  
            else
            {ComparePageObject.ArrPageContent.push("<td>");}  
            
            try
            {
                var field = ""; 
                var arrMuti = new Array(); 
                var arrPara = arrFieldRow["sFieldName"].split(",");
                var arrUnit = arrFieldRow["unit"].split(","); 
                if(ComparePageObject.IsIE)
                {
                    for(var j=0;j<arrPara.length;j++)
                    {
                        arrMuti.push(ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/"+arrPara[j]+"/@PValue").value + "" + arrUnit[j]); 
                    } 
                }    
                else
                {
                    for(var j=0;j<arrPara.length;j++)
                    {
                        arrMuti.push(selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/"+arrPara[j]+"/@PValue") + "" + arrUnit[j]);
                    }  
                }  
            } 
            catch(err)
            { }   
            ComparePageObject.ArrPageContent.push(arrMuti.join(arrFieldRow["joinCode"])+"</td>"); 
         }
    }
   
    //when less 
    if(ComparePageObject.NeedBlockTD>0)
    {
         for(var i=0;i<ComparePageObject.NeedBlockTD;i++)
         {
             if((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft)
             {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");      
             } 
             ComparePageObject.ArrPageContent.push("<td>&nbsp;");  
             ComparePageObject.ArrPageContent.push("</td>");
         } 
    }
    ComparePageObject.ArrPageContent.push("</tr>"); 
}

// create bar for compare
function createBar(arrFieldRow)
{
    ComparePageObject.ArrPageContent.push("<tr class=\"car_compare_name\">"); 
    ComparePageObject.ArrPageContent.push("<th><h2><a id=\"TRForSelect_"+arrFieldRow["sTrPrefix"]+"_0\" href=\"javascript:hiddenTR('"+arrFieldRow["sTrPrefix"]+"');\">"+arrFieldRow["sFieldTitle"]+"</a></h2></th>"); 
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        // insert Sec TH 
         if(i==ComparePageObject.MaxTDLeft)
         {
            ComparePageObject.ArrPageContent.push("<th><h2><a id=\"TRForSelect_"+arrFieldRow["sTrPrefix"]+"_"+i+"\" href=\"javascript:hiddenTR('"+arrFieldRow["sTrPrefix"]+"');\">"+arrFieldRow["sFieldTitle"]+"</a></h2></th>");  
         }   
         if(ComparePageObject.ArrCarInfo[i].CarInfoXML)
         {
            if(i==0)
            {ComparePageObject.ArrPageContent.push("<td class=\"f\">");}  
            else
            {ComparePageObject.ArrPageContent.push("<td>");}  
            var cs_Name = "";
            var car_year = ""; 
            var cs_allSpell = "";  
            try
            {
                if(ComparePageObject.IsIE)
                {
                    cs_Name = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_ShowName/@PValue").value;
                    car_year = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Car_YearType/@PValue").value;
                    cs_allSpell = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/Cs_AllSpell/@PValue").value.toLowerCase(); 
                }    
                else
                {
                    cs_Name = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/Cs_ShowName/@PValue");
                    car_year = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/Car_YearType/@PValue");
                    cs_allSpell = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/Cs_AllSpell/@PValue").toLowerCase(); 
                }  
                if(car_year.length>=4)
                {
                    car_year = " " + car_year.substring(2,car_year.length) + "款";
                }
                else
                {
                    car_year = "";
                } 
            } 
            catch(err)
            {}   
            ComparePageObject.ArrPageContent.push("<a href=\"/"+cs_allSpell+"/m"+ComparePageObject.ArrCarInfo[i].CarID+"/\" target=\"_blank\">" + cs_Name + " " + ComparePageObject.ArrCarInfo[i].CarName + car_year + "</a></td>"); 
         }
    }
   
    //when less 
    if(ComparePageObject.NeedBlockTD>0)
    {
         for(var i=0;i<ComparePageObject.NeedBlockTD;i++)
         {
             if((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft)
             {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th><h2><a href=\"javascript:hiddenTR('"+arrFieldRow["sTrPrefix"]+"');\">"+arrFieldRow["sFieldTitle"]+"</a></h2></th>");      
             } 
             ComparePageObject.ArrPageContent.push("<td>&nbsp;");  
             ComparePageObject.ArrPageContent.push("</td>");
         } 
    }         
    ComparePageObject.ArrPageContent.push("</tr>");  
}

// create price for compare
function createPrice(arrFieldRow)
{
    ComparePageObject.ArrPageContent.push("<tr onmouseover=\"changeTRColorWhenOnMouse(this,'#E7EEF2')\" onmouseout=\"changeTRColorWhenOnMouse(this,'')\" id=\"tr"+arrFieldRow["sTrPrefix"]+"_"+arrFieldRow["sFieldName"]+"\">");
    ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");
    // alert(ComparePageObject.ArrPageContent.join(""));
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        // insert Sec TH 
         if(i==ComparePageObject.MaxTDLeft)
         {
            ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");  
         }   
         if(ComparePageObject.ArrCarInfo[i].CarInfoXML)
         {
            if(i==0)
            {ComparePageObject.ArrPageContent.push("<td class=\"f\">");}  
            else
            {ComparePageObject.ArrPageContent.push("<td>");}  
            
            try
            {
                var field = ""; 
                if(ComparePageObject.IsIE)
                {
                    field = ComparePageObject.ArrCarInfo[i].CarInfoXML.documentElement.selectSingleNode("/CarParams/"+arrFieldRow["sFieldName"]+"/@PValue").value;
                    
                }    
                else
                {
                    field = selectSingleNodeForFirefox(ComparePageObject.ArrCarInfo[i].CarInfoXML,"/CarParams/"+arrFieldRow["sFieldName"]+"/@PValue");
                }  
            } 
            catch(err)
            { ComparePageObject.ArrPageContent.push("-"); }   
            if(field.length<1||field=="无")
            {
                 ComparePageObject.ArrPageContent.push("无");
            } 
            else
            {
                 ComparePageObject.ArrPageContent.push("<a target=\"_blank\" href=\"http://price.bitauto.com/car.aspx?newcarid="+ComparePageObject.ArrCarInfo[i].CarID+"&citycode=0\">" + field.replace('～','-') + "</a>");  
            }  
            ComparePageObject.ArrPageContent.push("</td>"); 
         }
    }
   
    //when less 
    if(ComparePageObject.NeedBlockTD>0)
    {
         for(var i=0;i<ComparePageObject.NeedBlockTD;i++)
         {
             if((ComparePageObject.ValidCount + i) == ComparePageObject.MaxTDLeft)
             {
                // insert Sec TH
                ComparePageObject.ArrPageContent.push("<th>"+arrFieldRow["sFieldTitle"]+"</th>");      
             } 
             ComparePageObject.ArrPageContent.push("<td>&nbsp;");  
             ComparePageObject.ArrPageContent.push("</td>");
         } 
    }     
    ComparePageObject.ArrPageContent.push("</tr>");  
}

function checkCurrentActive()
{
    return true;
}

// 排除相同项
function delTheSameForCompare()
{
    if(ComparePageObject.ValidCount>1)
    {
       if(!ComparePageObject.IsOperateTheSame)
       {
           createPageForCompare(true);
           ComparePageObject.IsOperateTheSame = true;
           changeCheckBoxStateByName("checkboxForDelTheSame",true);  
       }
       else
       {
           createPageForCompare(false);
           ComparePageObject.IsOperateTheSame = false;
           changeCheckBoxStateByName("checkboxForDelTheSame",false);   
       }    
       // 更改排重文字
//       var thesameOperateObj = document.getElementById('thesameOperate');
//       if(thesameOperateObj)
//       {
//           if(isOperateTheSame)
//           {
//              if(CheckBrowser())
//              {thesameOperateObj.innerText = '恢复排除内容';}
//              else
//              {thesameOperateObj.textContent = '恢复排除内容';}
//           }
//           else
//           {
//              if(CheckBrowser())
//              {thesameOperateObj.innerText = '排除相同内容';}
//              else
//              {thesameOperateObj.textContent = '排除相同内容';}
//           }
//       }
    }   
}

// change checkbox state for delete the same param
function changeCheckBoxStateByName(name,state)
{
    var checkBoxs = document.getElementsByName(name);
    // alert(checkBoxs.length); 
    if(checkBoxs && checkBoxs.length>0)
    {
        for(var i=0;i<checkBoxs.length;i++)
        {
            checkBoxs[i].checked = state;
        }  
    }  
}

// hidd or show TR
function hiddenTR(prefixName)
{
    var boolIsHid = true;
    var tableObject = document.getElementById('compareTable');
    for(var i=0;i<tableObject.rows.length;i++)
    {
        if(tableObject.rows[i].id.indexOf("tr"+prefixName+"_")==0)
        {
            if(tableObject.rows[i].style.display == "none")
            {
                 tableObject.rows[i].style.display = ""; 
                 boolIsHid = false;  
            }
            else
            {
                  tableObject.rows[i].style.display = "none"; 
                 boolIsHid = true;  
            }    
        }
    }
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        var tableObject = document.getElementById('TRForSelect_'+prefixName+'_'+i);
        if(tableObject)
        {
           if(boolIsHid)
           {
               tableObject.className = "close";
           }
           else
           {
               tableObject.className = "";
           }
        }
    }
}

// show or hid compare list
function showHidAll()
{
    var tableObject = document.getElementById('compareTable');
    for(var i=0;i<tableObject.rows.length;i++)
    {
        if(tableObject.rows[i].id.indexOf("tr")==0)
        {
            if(ComparePageObject.IsDeployAll)
            { 
              tableObject.rows[i].style.display = "block"; 
            }
            else
            {
              tableObject.rows[i].style.display = "none"; 
            }
        }
    }
    ComparePageObject.IsDeployAll = !ComparePageObject.IsDeployAll;
    // FF 刷table
    if(!ComparePageObject.IsIE)
    {
       createPageForCompare(false);
    }
//    // 展开全部文字切换
//    var isDeployObj = document.getElementById('isDeploy');
//    if(isDeployObj)
//    {
//       if(isDeployAll)
//       {
//           if(CheckBrowser())
//           {isDeployObj.innerText = '全部展开';}
//           else
//           {isDeployObj.textContent = '全部展开';}
//       }
//       else
//       {
//           if(CheckBrowser())
//           {isDeployObj.innerText = '全部折叠';}
//           else
//           {isDeployObj.textContent = '全部折叠';}
//       }
//    }
}

// reset compare list
function resetCompareCar(id)
{
    if(ComparePageObject.ValidCount<2)
    {
        // only 1 car 
        return; 
    }
    var num = -1;
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        if(ComparePageObject.ArrCarInfo[i].CarID == id)
        {
            num = i;
        } 
    }
    // not first object
    if(num >0)
    {
        swapArray(ComparePageObject.ArrCarInfo,0,num);
        // update the same car 
        createPageForCompare(false); 
    }
}
// swap Array object
function swapArray(obj,index1,index2)
{
    var temp = obj[index1];
    obj[index1] = obj[index2];
    obj[index2] = temp;
}

function createXMLHttpRequestForCompare() 
{
	if (window.ActiveXObject) 
	{
		var xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	}
	else if (window.XMLHttpRequest) 
	{
		var xmlHttp = new XMLHttpRequest();
	}
	return xmlHttp;
}

function CheckBrowserForCompare()
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

function selectSingleNodeForFirefox(xmldom,path)
{
     var xpe = new XPathEvaluator();
     var nsResolver = xpe.createNSResolver(xmldom.ownerDocument == null ? xmldom.documentElement : xmldom.ownerDocument.documentElement); 
     var results = xpe.evaluate(path,xmldom,nsResolver,XPathResult.FIRST_ORDERED_NODE_TYPE, null); 
     return results.singleNodeValue.value;  
}

// 下拉列表
function intiPageSelectControl(type,id)
{
    if(!type)
    {ComparePageObject.SelectType=1;}
    else
    {ComparePageObject.SelectType=type;}
    var url = ComparePageObject.MasterToCarDataPath+"?type="+ComparePageObject.SelectType+"&id=";
    if(!id)
    {url+="0";}
    else
    {url+=id;}
	ComparePageObject.XmlHttpForSelect = createXMLHttpRequest();
	if(ComparePageObject.IsIE)
	{
	    ComparePageObject.XmlHttpForSelect.onreadystatechange = handleStateChangeForSelectControl;
	}
	ComparePageObject.XmlHttpForSelect.open("GET", url, false);
	ComparePageObject.XmlHttpForSelect.send(null);
	if(!ComparePageObject.IsIE)
	{
	    var temp = ComparePageObject.XmlHttpForSelect.responseText;
	    if(ComparePageObject.SelectType==1)
	    {ComparePageObject.ListForMaster = temp}
	    if(ComparePageObject.SelectType==2)
	    {ComparePageObject.ListForSerial = temp}
	    if(ComparePageObject.SelectType==3)
	    {ComparePageObject.ListForCar = temp}
	    parseXMLForSelectControl(); 
	}
}

function handleStateChangeForSelectControl()
{
    if(ComparePageObject.XmlHttpForSelect.readyState == 4) 
	{
		if(ComparePageObject.XmlHttpForSelect.status == 200) 
		{
		    var temp = ComparePageObject.XmlHttpForSelect.responseText;
		    if(ComparePageObject.SelectType==1)
		    {ComparePageObject.ListForMaster = temp}
		    if(ComparePageObject.SelectType==2)
		    {ComparePageObject.ListForSerial = temp}
		    if(ComparePageObject.SelectType==3)
		    {ComparePageObject.ListForCar = temp}
		    parseXMLForSelectControl(); 
		}
	}
}

function parseXMLForSelectControl()
{
    if(ComparePageObject.SelectType && ComparePageObject.SelectType==1)
    {
        // 主品牌
        var selectMaster = document.getElementById("selectForMaster");
        if(ComparePageObject.ListForMaster && selectMaster)
        {
           selectMaster.innerHTML = "<select id=\"masterSelectControl\" onchange=\"onchangeMasterForSelect();\">"+ComparePageObject.ListForMaster+"</select>";
        }
        // 级联选择过
        if(CookieForSelectListTemp.getCookie("TempSelectMasterID"))
        {
          var masterSelectControl = document.getElementById("masterSelectControl");
          masterSelectControl.value = CookieForSelectListTemp.getCookie("TempSelectMasterID");
          onchangeMasterForSelect(); 
        }
    }
    if(ComparePageObject.SelectType && ComparePageObject.SelectType==2)
    {
       // 子品牌
       var selectSerial = document.getElementById("selectForSerial");
       if(ComparePageObject.ListForSerial && selectSerial)
       {
          selectSerial.innerHTML = "<select id=\"serialSelectControl\" onchange=\"onchangeSerialForSelect();\">"+ComparePageObject.ListForSerial+"</select>";
       }
       // 级联选择过
       if(CookieForSelectListTemp.getCookie("TempSelectSerialID"))
        {
          var serialSelectControl = document.getElementById("serialSelectControl");
          serialSelectControl.value = CookieForSelectListTemp.getCookie("TempSelectSerialID");
          onchangeSerialForSelect(); 
        }
    }
    if(ComparePageObject.SelectType && ComparePageObject.SelectType==3)
    {
       // 车型
       var selectCar = document.getElementById("selectForCar");
       if(ComparePageObject.ListForCar && selectCar)
       {
          selectCar.innerHTML = "<select id=\"carSelectControl\" onchange=\"onchangeCarForSelect();\">"+ComparePageObject.ListForCar+"</select>";
       }
    }
}


function onchangeMasterForSelect()
{
     var masterSelectValue = document.getElementById('masterSelectControl').options[ document.getElementById('masterSelectControl').selectedIndex].value;
     if(masterSelectValue&&masterSelectValue>0) 
     {
       if(CookieForSelectListTemp.getCookie("TempSelectMasterID")&&CookieForSelectListTemp.getCookie("TempSelectMasterID")!=masterSelectValue&&CookieForSelectListTemp.getCookie("TempSelectSerialID")) 
       {CookieForSelectListTemp.clearCookie("TempSelectSerialID");}
       CookieForSelectListTemp.setCookie("TempSelectMasterID",masterSelectValue);
       intiPageSelectControl(2,masterSelectValue);
     }
}

function onchangeSerialForSelect()
{
     var selectSelectValue = document.getElementById('serialSelectControl').options[ document.getElementById('serialSelectControl').selectedIndex].value;
     if(selectSelectValue&&selectSelectValue>0) 
     {
       CookieForSelectListTemp.setCookie("TempSelectSerialID",selectSelectValue);  
       intiPageSelectControl(3,selectSelectValue);
     }
}

function onchangeCarForSelect()
{
    if(document.getElementById('carSelectControl').selectedIndex >= 0)
    {
        var carCar = document.getElementById('carSelectControl').options[ document.getElementById('carSelectControl').selectedIndex].value;  
        var carName = document.getElementById('carSelectControl').options[ document.getElementById('carSelectControl').selectedIndex].text 
        addCarToCompareForSelect(carCar,carName,""); 
    } 
}

//-------------------- add car to compare 
// add compare to cookie
function addCarToCompareForSelect(id,name,csName)
{
    var compare = ComparePageObject.CarInfos;
    // var compare = CookieForCompare.getCookie("ActiveNewCompare");
    var com_arr_carID = null;
    var com_arr = null;
    if (compare)
    {
       com_arr_carID = ComparePageObject.CarIDsForURL.split("|");
	    com_arr = compare.split("|");
	    if(com_arr.length>=10)
	    {
	       alert("对比车型不能多于10个");
	       return; 
	    } 
	    if(compare.indexOf("id" + id+",")>=0)
        {
            alert("您选择的车型,已经在对比列表中!");
            return; 
        }   
    }
    else
    {
       com_arr_carID = new Array();
	    com_arr = new Array();
    }
    com_arr_carID.push(id);
    com_arr.push('id' + id + ',' + name);
    document.location.href = "/chexingduibi/?carIDs="+com_arr_carID.join(",");
//    CookieForCompare.clearCookie("ActiveNewCompare"); 
//    CookieForCompare.setCookie("ActiveNewCompare", com_arr.join("|")); 
//    initPageForCompare("");
}

function delCarToCompare(caiID)
{
    var newCarIDArr = new Array();
    if(ComparePageObject.ValidCount<1)
    {
        alert('没有可删的了');
        return; 
    }
    var num = -1;
    for(var i=0;i<ComparePageObject.ValidCount;i++)
    {
        if(ComparePageObject.ArrCarInfo[i].CarID == caiID)
        {
            num = i;//alert(' yes :' + id + ' ' + i); 
        } 
        else
        {
              newCarIDArr.push(ComparePageObject.ArrCarInfo[i].CarID);
        }
    }
    if(num >=0)
    {
        // changeComparedShow(Compare.carID[num],Compare.carName[num]);
//        ComparePageObject.ArrCarInfo.splice(num,1);
//        ComparePageObject.ValidCount--; 
        if(newCarIDArr.length>0)
        {document.location.href = "/chexingduibi/?carIDs="+newCarIDArr.join(",");}
        else
        {document.location.href = "/chexingduibi/";}
        // for reset compare cookie 
//        resetCookieForCompareCar(caiID); 
//        resetTableWidth();
//        createPageForCompare(false); 
    }
}

function resetCookieForCompareCar(id)
{
    var arrForNewCar = new Array();
    for(var i=0;i<ComparePageObject.ArrCarInfo.length;i++)
    {
        arrForNewCar.push("id" + ComparePageObject.ArrCarInfo[i].CarID + "," + ComparePageObject.ArrCarInfo[i].CarName);
    }
    CookieForCompare.clearCookie("ActiveNewCompare"); 
	CookieForCompare.setCookie("ActiveNewCompare", arrForNewCar.join("|")); 

	// reset csName cookie
	var arrForNewCs = new Array();
	var carSerial = CookieForCompare.getCookie("ActiveCarSerialCompare");
    var carSerial_arr = null;
    if (carSerial)
    {
	    var idForCs;
	    var nameForCs;  
	    carSerial_arr = carSerial.split("|"); 
	    for(var i=0;i<carSerial_arr.length;i++)
	    {
	        idForCs = carSerial_arr[i].split(",")[0];
	        nameForCs = carSerial_arr[i].split(",")[1]; 
	        if(idForCs!="id"+id) 
	        {arrForNewCs.push("id"+idForCs+","+nameForCs);} 
	    }  
	    CookieForCompare.clearCookie("ActiveCarSerialCompare"); 
	    CookieForCompare.setCookie("ActiveCarSerialCompare", arrForNewCs.join("|")); 
    }
}

function resetTableWidth()
{
    if(ComparePageObject.ValidCount>0)
    {
       if(ComparePageObject.ValidCount>5)
       {
           if(ComparePageObject.ValidCount>=10)
           {ComparePageObject.TableWidth = 1634;}
           else
           {ComparePageObject.TableWidth = (146*2) + (134*(ComparePageObject.ValidCount+1)) + 2;}
       }
       else
       {
           ComparePageObject.TableWidth = 146 + (134*6) + 2;
       }
    }
    else
    {
       ComparePageObject.TableWidth = 146 + (134*6) + 2;
    }
}

function checkIsChange(tdObj)
{
    targetNum = tdObj.id.replace('td_','');
    if(targetNum == currentTD || targetNum == "" || currentTD == "" )
    { 
    }
    else
    {
       swapArray(ComparePageObject.ArrCarInfo,currentTD,targetNum);
       targetNum = ""; 
       currentTD = "";
       createPageForCompare(false); 
    }
}

function changeTRColorWhenOnMouse(obj,color)
{
    if(obj)
    {obj.style.background=color;}
}

//--------------------  Drag  --------------------
var ie=document.all;
var nn6=document.getElementById&&!document.all;

var isdrag=false;
var x,y;
var dobj;
var currentTD = "";
var targetNum = "";

function movemouse(e)
{
  if (isdrag)
  {
    dobj.style.left = nn6 ? tx + e.clientX - x +"px": tx + event.clientX - x;
    dobj.style.top  = nn6 ? ty + e.clientY - y +"px": ty + event.clientY - y;
    return false;
  }
}

function selectmouse(e) 
{    
    try
    { 
       var fobj = nn6 ? e.target : event.srcElement;
       var topelement = nn6 ? "HTML" : "BODY";
       if(fobj.tagName);
       while (fobj.tagName != topelement && fobj.name != "dragImg")
       {if(fobj.tagName);
          fobj = nn6 ? fobj.parentNode : fobj.parentElement;
       }
       if (fobj.name=="dragImg")
       {  
        currentTD = fobj.id.replace('img_','');
        
        isdrag = true;
        fobj.style.position = 'relative'; 
        fobj.style.zIndex= '90';
        dobj = fobj;
        tx = parseInt(dobj.style.left+0);
        ty = parseInt(dobj.style.top+0);
        x = nn6 ? e.clientX : event.clientX;
        y = nn6 ? e.clientY : event.clientY;
        document.onmousemove=movemouse;
        return false;
      }
    }
    catch(err)
    {return false;}  
}
function resetImg(e)
{ 
    try
    {
        var fobj = nn6 ? e.target : event.srcElement;
        isdrag=false;
        if(fobj.name == "dragImg")
        {
           fobj.style.position = '';   
           fobj.style.left = 0;
           fobj.style.top = 0;
           setTimeout("currentTD = '';",200);
         }
        else
       {
           if(currentTD != '')
           {
               var fobj2 = document.getElementById("img_" + currentTD);
               fobj2.style.position = '';   
               fobj2.style.left = 0;
               fobj2.style.top = 0;
               currentTD = '';
           }
       }  
         return false;
     }
     catch(err)
     {
       return false;
       }
}
window.onload = function() {
    if(ComparePageObject.IsNeedDrag)
    {
        document.onmousedown=selectmouse;
        document.onmouseup=resetImg;
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

// page method --------------------------

var arrField = [
    {sType:"fieldPic",sFieldName:"",sFieldTitle:"图片",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"bar",sFieldName:"",sFieldTitle:"参数",sTrPrefix:"1",unit:"",joinCode:""},
    { sType: "fieldPara", sFieldName: "CarReferPrice", sFieldTitle: "厂家指导价", sTrPrefix: "1", unit: "", joinCode: "" },
    {sType:"fieldPrice",sFieldName:"AveragePrice",sFieldTitle:"经销商报价",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"Engine_Exhaust",sFieldTitle:"排量",sTrPrefix:"1",unit:"mL",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_TransmissionType",sFieldTitle:"变速器型式",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutSet_Length",sFieldTitle:"长",sTrPrefix:"1",unit:"mm",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutSet_Width",sFieldTitle:"宽",sTrPrefix:"1",unit:"mm",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutSet_Height",sFieldTitle:"高",sTrPrefix:"1",unit:"mm",joinCode:""},  
    {sType:"fieldPara",sFieldName:"OutSet_WheelBase",sFieldTitle:"轴距",sTrPrefix:"1",unit:"mm",joinCode:""},
    {sType:"fieldPara",sFieldName:"Oil_FuelCapacity",sFieldTitle:"燃油箱容积",sTrPrefix:"1",unit:"L",joinCode:""},
    {sType:"fieldMulti",sFieldName:"Engine_MaxPower,Engine_PowerSpeed",sFieldTitle:"最大功率",sTrPrefix:"1",unit:"kW,rpm",joinCode:"/"},
    {sType:"fieldMulti",sFieldName:"Engine_MaxNJ,Engine_NJSpeed",sFieldTitle:"最大扭矩",sTrPrefix:"1",unit:"Nm,rpm",joinCode:"/"},
    {sType:"fieldPara",sFieldName:"Oil_FuelTab",sFieldTitle:"燃油标号",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_FrontSuspensionType",sFieldTitle:"前悬挂类型",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_RearSuspensionType",sFieldTitle:"后悬挂类型",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_FrontBrakeType",sFieldTitle:"前制动类型",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_RearBrakeType",sFieldTitle:"后制动类型",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_FrontTyreStandard",sFieldTitle:"前轮胎规格",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_RearTyreStandard",sFieldTitle:"后轮胎规格",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_BodyColor",sFieldTitle:"车身颜色",sTrPrefix:"1",unit:"",joinCode:""},
    {sType:"bar",sFieldName:"",sFieldTitle:"配置",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"Safe_DriverGasBag",sFieldTitle:"驾驶位安全气囊",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"Safe_SubDriverGasBag",sFieldTitle:"副驾驶位安全气囊",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"Safe_ABS",sFieldTitle:"ABS(刹车防抱死制动系统)",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"Safe_ESP",sFieldTitle:"ESP(电子稳定程序)",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"Safe_BeltPreTighten",sFieldTitle:"安全带预收紧功能",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_AsistTurnTune",sFieldTitle:"随速助力转向调节(EPS)",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_DriveAsistTurn",sFieldTitle:"转向助力",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_CenterControlLock",sFieldTitle:"中控门锁",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_RRadar",sFieldTitle:"倒车雷达",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"UnderPan_RImage",sFieldTitle:"倒车影像",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_CDPlayer",sFieldTitle:"CD",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_DVDPlayer",sFieldTitle:"DVD",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_SpeedCruise",sFieldTitle:"定速巡航系统",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_SteerTuneDirection",sFieldTitle:"方向盘调节方向",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_SteerTuneType",sFieldTitle:"方向盘调节方式",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_GPS",sFieldTitle:"GPS电子导航",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_Bluetooth",sFieldTitle:"蓝牙系统",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_AirCType",sFieldTitle:"空调控制方式",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_TemperAreaCount",sFieldTitle:"温区个数",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_SeatMaterial",sFieldTitle:"座椅面料",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_BCenterArmrest",sFieldTitle:"后座中央扶手",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_DSeatHot",sFieldTitle:"驾驶座座椅加热",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_DSeatProp",sFieldTitle:"驾驶座腰部支撑调节",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"InStat_DSeatTuneType",sFieldTitle:"驾驶座座椅调节方式",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_FrontLightType",sFieldTitle:"前照灯类型",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_FLightHeightTune",sFieldTitle:"前照灯照射高度调节",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_FLightSteer",sFieldTitle:"前大灯随动转向",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_FLightClose",sFieldTitle:"前大灯自动开闭",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_FLightAutoClean",sFieldTitle:"前照灯自动清洗功能",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_ReMirrorElecTune",sFieldTitle:"外后视镜电动调节",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_ReMirrorHot",sFieldTitle:"外后视镜加热功能",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_ReMirrorFold",sFieldTitle:"外后视镜电动折叠功能",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_CarWindow",sFieldTitle:"车窗",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_AvoidNipHead",sFieldTitle:"电动窗防夹功能",sTrPrefix:"2",unit:"",joinCode:""},
    {sType:"fieldPara",sFieldName:"OutStat_FBrushSensor",sFieldTitle:"雨刷传感器",sTrPrefix:"2",unit:"",joinCode:""}
];