// JScript 文件
	var initMainSelectValue=-1;
    /*
    *基类对象
    */
    function InitObject(mainId,secondId,carTypeId,carPriceId)
    {
        this.m_MainId = mainId;
        this.m_SecondId = secondId;
        this.m_CarTypeId = carTypeId;
        this.m_CarPriceId = carPriceId;
    }
    /*
    *当用户使用的是下拉列表时，请使用此类型
    */
    function SelectInitObject(mainId,secondId,carTypeId,carPriceId,carCityId)
    {
        InitObject.call(this,mainId,secondId,carTypeId,carPriceId);               
        this.m_CityId = carCityId;
        this.openUrl = { "car": {"d": "http://car.bitauto.com/"
                        , "m": "http://car.bitauto.com/tree_chexing/mb_@id@/"
                        , "s": "http://car.bitauto.com/@allspell@/"}
                        , "price": { "d": "http://price.bitauto.com/"
                        , "m": "http://price.bitauto.com/keyword.aspx?keyword=@key@&mb_id=@id@&citycode=@city@"
                        , "s": "http://price.bitauto.com/frame.aspx?newbrandid=@id@&citycode=@city@"}
                        };
    }
    SelectInitObject.prototype = new InitObject();
    //初始化对象
    SelectInitObject.prototype.Init = function()
    {     
        var mainBrandObject = document.getElementById(this.m_MainId);
        if(mainBrandObject==null || mainBrandObject=="undefined" || mainBrandObject.nodeName.toString().toLowerCase()!="select")
        {
            alert("init MainBrand Fault!");
        }
        var secondBrandObject = document.getElementById(this.m_SecondId);
        if(secondBrandObject==null || secondBrandObject=="undefined" || secondBrandObject.nodeName.toString().toLowerCase()!="select")
        {
            alert("init CarSerial Fault!")
        }  
        if(JSonData ==null||JSonData.masterBrand==null||JSonData.masterBrand.length<1)
        {
            //alert("init Data Fault!");
        }
        this.BindMainObject(JSonData.masterBrand,mainBrandObject);
        this.BindSecondObject(null,secondBrandObject);
        this.BindClickObject();
        
    }    
    //绑定主下拉列表
    SelectInitObject.prototype.BindMainObject = function (objectData,mbObject)
    {
        var optionItem;
        var iLength = 0;
        if(mbObject.options!=null||mbObject.options.length>0)
        {           
           mbObject.innerHTML='';    
        }
        optionItem = new Option("请选择品牌",-1);
        mbObject.options.add(optionItem);
        if(objectData==null||objectData.length<1)
        {
            return;
        }
        iLength = objectData.length;
        for(var i=0;i<iLength;i++)
        {
            if(objectData[i].id!=-1)
            {   
                optionItem = new Option(objectData[i].name,objectData[i].id);                
                mbObject.options.add(optionItem);
            }            
        } 
        setTimeout(function(){mbObject.value=initMainSelectValue;},1);      
        var pro=this;
        mbObject.onchange=function()
        {
            pro.BindMainObjectChange(pro,pro.m_MainId,pro.m_SecondId);
        }
        
    }   
    //主列表绑定事件  
    SelectInitObject.prototype.BindMainObjectChange = function(currentobj,eventid,eventsid)
    {   
        var operationObject = document.getElementById(eventid);
        var currentValue = operationObject.options[operationObject.selectedIndex].value;
        if(parseInt(currentValue)==-1)
        {
            currentobj.BindSecondObject(null,document.getElementById(eventsid));
        }
        
        var iLength = 0;
        iLength = JSonData.masterBrand.length;
        for(var i =0;i<iLength;i++)
        {
            if(JSonData.masterBrand[i].id==parseInt(currentValue))
            {
               //alert(typeof BrandJsonData);
                if(typeof BrandJsonData != 'undefined')
                {
					//alert(2);
                    SelectInitObject.prototype.BindSecondObjectType(JSonData.masterBrand[i],document.getElementById(eventsid),1);
                    break;
                }
                //alert(3);
                currentobj.BindSecondObject(JSonData.masterBrand[i].carSerial,document.getElementById(eventsid));
                break;
            }
        }
    }
    //初始化第二个下拉列表 
    SelectInitObject.prototype.BindSecondObject = function(objectData,mbObject)
    {
		//alert(1);
        var optionItem;
        var iLength = 0;
        if(mbObject.options!=null||mbObject.options.length>0)
        {  
            for(var i=0;i<mbObject.options.length;i++)
            {
                mbObject.options.length = 0;
            }
        }
        optionItem = new Option("请选择系列",-1);
        mbObject.options.add(optionItem);
        if(objectData==null||objectData.length<1)
        {
            return;
        }
        iLength = objectData.length;
        for(var i=0;i<iLength;i++)
        {
            if(objectData[i].id!=-1)
            {   
                optionItem = new Option(objectData[i].name,objectData[i].id);                
                mbObject.options.add(optionItem);
            }            
        }
    }
    //初始化第二个下拉列表--包含品牌
    SelectInitObject.prototype.BindSecondObjectType = function(objectData,mbObject,type)
    {
        var optionItem;
        var iLength = 0;
        
        var eachGroup = mbObject.firstChild;
        while(eachGroup!=null)
        {
            mbObject.removeChild(eachGroup);
            eachGroup = mbObject.firstChild;
        }
        
        optionItem = new Option("请选择系列",-1);
        mbObject.options.add(optionItem);
        if(objectData==null)
        {
            return;
        }
        
        var tempMasterBrand ;
        //得到有排列顺序的主品牌对象
        for(var i=0;i<BrandJsonData.masterBrand.length;i++)
        {
            if(BrandJsonData.masterBrand[i].id!=objectData.id)
            {
                continue;
            }
            tempMasterBrand = BrandJsonData.masterBrand[i];
            break;
        }
        //绑定品牌
        for(var j=0;j<tempMasterBrand.carBrand.length;j++)
        {
			if(tempMasterBrand.carBrand.length > 1)
			{
				optionItem = document.createElement("OPTGROUP");
				optionItem.label = tempMasterBrand.carBrand[j].name;
				optionItem.style.fontStyle="normal";
				optionItem.style.background="#CCCCCC";
				optionItem.style.textAlign="center";
				mbObject.appendChild(optionItem);
            }
            //绑定子品牌
            for(var z=0;z<objectData.carSerial.length;z++)
            {
                if(objectData.carSerial[z].brandid != tempMasterBrand.carBrand[j].id)
                {
                    continue;
                }
                optionItem = document.createElement("OPTION");
                optionItem.setAttribute("value", objectData.carSerial[z].id);
                optionItem.appendChild(document.createTextNode(objectData.carSerial[z].name));            
                mbObject.appendChild(optionItem);
            }            
        }
    }
    //初始化点击控件
    SelectInitObject.prototype.BindClickObject = function()
    {
        var pro = this;
        var carTypeControl = document.getElementById(this.m_CarTypeId);
        if(carTypeControl == null || carTypeControl=="undefined")
        {
            //alert("Bind CarType Fault!");
            return;
        }
        carTypeControl.onclick = function()
        {
            pro.CarTypeClick(pro,pro.m_MainId,pro.m_SecondId);
        }
        if(this.m_CarPriceId=="")
        {
            return;
        }
        var carPriceControl = document.getElementById(this.m_CarPriceId);
        if(carPriceControl == null||carPriceControl == "undefined")
        {
            alert("Bind CarPrice Fault!");
        }
        carPriceControl.onclick = function()
        {
            pro.CarPriceClick(pro,pro.m_MainId,pro.m_SecondId,pro.m_CityId);
        }
    }
    //查看车型Click事件
    SelectInitObject.prototype.CarTypeClick = function(currenObject, mainid, secondid) {
        var mcontrolobject = document.getElementById(mainid);
        var scontrolobject = document.getElementById(secondid);
        var mindexValue = mcontrolobject.options[mcontrolobject.selectedIndex].value
        var sindexValue = scontrolobject.options[scontrolobject.selectedIndex].value;
        var gourl = "";
        if (sindexValue < 1 && mindexValue < 1) {
            this.StaticScript(currenObject.m_CarTypeId, 0, "d");
            window.open(currenObject.openUrl["car"]["d"], "", "");
            return;
        }
        else if (sindexValue < 1) {
            gourl = currenObject.openUrl["car"]["m"].replace(/@id@/g, mindexValue);
            this.StaticScript(currenObject.m_CarTypeId, mindexValue, "m");
            window.open(gourl, "", "", "");
            return;
        }
        var iLength = 0;
        iLength = JSonData.masterBrand.length;
        var url = "";
        for (var i = 0; i < iLength; i++) {
            if (JSonData.masterBrand[i].id == parseInt(mindexValue)) {
                //alert(JSonData.masterBrand[i].carSerial);
                var tempLength = JSonData.masterBrand[i].carSerial.length;
                for (var j = 0; j < tempLength; j++) {
                    if (JSonData.masterBrand[i].carSerial[j].id == parseInt(sindexValue)) {
                        url = JSonData.masterBrand[i].carSerial[j].allspell;
                        break;
                    }
                }
                break;
            }
        }
        if (url == "") {
            return false;
        }

        //统计代码
        this.StaticScript(currenObject.m_CarTypeId, sindexValue, "s");
        //跳转页面
        gourl = currenObject.openUrl["car"]["s"].replace(/@allspell@/g, url);
        window.open(gourl, "", "", "");
    }
    //查看报价Click事件
    SelectInitObject.prototype.CarPriceClick = function(currenObject, mainid, secondid, citycode) {
        var mcontrolobject = document.getElementById(mainid);
        var scontrolobject = document.getElementById(secondid);
        var mindexValue = mcontrolobject.options[mcontrolobject.selectedIndex].value
        var sindexValue = scontrolobject.options[scontrolobject.selectedIndex].value;
        var gourl = "";
        if (sindexValue < 1 && mindexValue < 1) {
            this.StaticScript(currenObject.m_CarPriceId, 0, "d");
            window.open(currenObject.openUrl["price"]["d"], "", "");
        }
        else if (sindexValue < 1) {
            var mastertext = mcontrolobject.options[mcontrolobject.selectedIndex].text;
            gourl = currenObject.openUrl["price"]["m"].replace(/@id@/g, mindexValue).replace(/@city@/g, citycode);
            gourl = gourl.replace(/@key@/g, encodeURI(mastertext.slice(1, mastertext.length)));
            this.StaticScript(currenObject.m_CarPriceId, mindexValue, "m");
            window.open(gourl, "", "", "");
        }
        if (parseInt(sindexValue) < 0) {
            return false;
        }
        //统计代码
        this.StaticScript(currenObject.m_CarPriceId, sindexValue, "s");
        var url = currenObject.openUrl["price"]["s"].replace(/@id@/g, sindexValue).replace(/@city@/g, citycode);
        //跳转页面        
        window.open(url, "", "", "");
    }
    //统计脚本
    SelectInitObject.prototype.StaticScript = function(objid, elementid, usertype) {
        if (typeof CarStaticDataObject == 'undefined' || CarStaticDataObject["isStaticPage"] == "") return;
        var staticType = CarStaticDataObject["isStaticPage"];
        var staticObj = CarStaticDataObject["StaticObject"][staticType];
        if (staticObj == null) return;
        var paramsList = []; 
        //循环赋值参数
        for (var parmName in staticObj) {
            paramsList.push(parmName + "=" + staticObj[parmName]);
        }
        for (var pn in CarStaticDataObject["StaticObject"][usertype][objid]) {
            paramsList.push(pn + "=" + CarStaticDataObject["StaticObject"][usertype][objid][pn]);
        }
        //添加子品牌ID
        paramsList.push("serialid=" + elementid);

        var url = CarStaticDataObject["StaticPageUrl"] + "?" + paramsList.join("&"); 
        //发送请求
        var _sentImg = new Image(1, 1);
        _sentImg.src = url;
    }
    /*
    *初始化对象
    */
    function StartInit(mainIdType,secondIdType,mainId,secondId,carTypeId,carPriceId,carCityID)
    {
        if(mainIdType.toLowerCase()=="select" && secondIdType.toLowerCase()=="select")
        {
            var CarTypeSelectObject = new SelectInitObject(mainId,secondId,carTypeId,carPriceId,carCityID);
            return CarTypeSelectObject;
        }
        alert("init Fault!");
    }