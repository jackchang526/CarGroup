<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="interfaceDemo.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.LeveldropdownlistData.interfaceDemo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style >
        body select{width:150px;}    
    </style>
</head>
<body>    
    <!--#include file="~/include/special/yc/00001/Car_DropDownList_PublicScriptsV2_Manual.shtml"-->
    <!--Start:主品牌to品牌to子品牌-->
    主品牌to品牌to子品牌:<select id="master1"></select><select id="brand1"></select><select id="serial1"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "master": { "selectid": "master1", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=master&serias=m&key=master_0_4_m" }
        , "brand": { "selectid": "brand1", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=@pid@&rt=brand&serias=m&key=brand_@pid@_4_m" }
        , "serial": { "selectid": "serial1", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=@pid@&rt=serial&serias=m&key=serial_@pid@_4_m" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
    	var dropDownBindObj1 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj1.InitBindSelect();
    </script>
    <!--End:主品牌to品牌to子品牌-->
    
    <!--Start:主品牌to品牌to子品牌to车型-->
    主品牌to品牌to子品牌to车型:<select id="master2"></select><select id="brand2"></select><select id="serial2"></select><select id="cartype2"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "master": { "selectid": "master2", "value": "id", "text": "name", "serias": "m", "datatype": "0", "condition": "type=0&pid=0&rt=master&serias=m&key=master_0_0_m" }
        , "brand": { "selectid": "brand2", "value": "id", "text": "name", "serias": "m", "datatype": "0", "condition": "type=0&pid=@pid@&rt=brand&serias=m&key=brand_@pid@_0_m" }
        , "serial": { "selectid": "serial2", "value": "id", "text": "name", "serias": "m", "datatype": "0", "condition": "type=0&pid=@pid@&rt=serial&serias=m&key=serial_@pid@_0_m" }
        , "cartype": { "selectid": "cartype2", "value": "id", "text": "name", "datatype": "0", "serias": "m", "condition": "type=0&pid=@pid@&include=1&rt=cartype&serias=p&key=cartype_@pid@_0_m" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
    	var dropDownBindObj2 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj2.InitBindSelect();
    </script>
    <!--End:主品牌to品牌to子品牌to车型-->  
    
    <!--Start:主品牌to子品牌to车型-->
    主品牌to子品牌to车型:<select id="master4"></select><select id="serial4"></select><select id="cartype4"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "master": { "selectid": "master4", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=master&serias=m&key=master_0_4_m" }
        , "serial": { "selectid": "serial4", "value": "id", "text": "name", "serias": "m", "include": "1", "datatype": "4", "condition": "type=4&pid=@pid@&include=1&rt=serial&serias=m&key=serial_@pid@_4_m" }
        , "cartype": { "selectid": "cartype4", "value": "id", "text": "name", "datatype": "4", "serias": "m", "condition": "type=4&pid=@pid@&include=1&rt=cartype&serias=p&key=cartype_@pid@_4_m" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
    	var dropDownBindObj4 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj4.InitBindSelect();
    </script>
    <!--End:主品牌to子品牌to车型--> 
     
    <!--Start:品牌to子品牌-->
    品牌to子品牌:<select id="brand5"></select><select id="serial5"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "brand": { "selectid": "brand5", "value": "id", "text": "name", "include": "1", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=brand&include=1&serias=m&key=brand_0_4_m" }
        , "serial": { "selectid": "serial5", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=@pid@&rt=serial&serias=m&key=serial_@pid@_4_m" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
    	var dropDownBindObj5 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj5.InitBindSelect();
    </script>
    <!--End:品牌to子品牌-->     
     
    <!--Start:品牌to子品牌to车型-->
    品牌to子品牌to车型:<select id="brand6"></select><select id="serial6"></select><select id="cartype6"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "brand": { "selectid": "brand6", "value": "id", "text": "name", "include": "1", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=brand&include=1&serias=m&key=brand_0_4_m" }
        , "serial": { "selectid": "serial6", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=@pid@&rt=serial&serias=m&key=serial_@pid@_4_m" }
        , "cartype": { "selectid": "cartype6", "value": "id", "text": "name", "datatype": "4", "serias": "m", "condition": "type=4&pid=@pid@&include=1&rt=cartype&serias=p&key=cartype_@pid@_4_m" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";
    	var dropDownBindObj6 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj6.InitBindSelect();
    </script>
    <!--End:品牌to子品牌to车型-->  
      
    <!--Start:厂商to品牌to子品牌-->
    厂商to品牌to子品牌:<select id="produce7"></select><select id="brand7"></select><select id="serial7"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "producer": { "selectid": "produce7", "value": "id", "text": "name", "serias": "p", "datatype": "4", "condition": "type=4&pid=0&rt=producer&serias=p&key=producer_0_4_p" }
        , "brand": { "selectid": "brand7", "value": "id", "text": "name", "serias": "p", "datatype": "4", "condition": "type=4&pid=@pid@&rt=brand&serias=m&key=brand_@pid@_4_p" }
        , "serial": { "selectid": "serial7", "value": "id", "text": "name", "datatype": "4", "serias": "p", "condition": "type=4&pid=@pid@&serias=p&rt=serial&key=serial_@pid@_4_p" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/ProduceToSerial.aspx";
    	var dropDownBindObj7 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj7.InitBindSelect();
    </script>
    <!--End:厂商to品牌to子品牌-->   
    
    <!--Start:厂商to品牌to子品牌to车型-->
    厂商to品牌to子品牌to车型:<select id="produce8"></select><select id="brand8"></select><select id="serial8"></select><select id="cartype8"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "producer": { "selectid": "produce8", "value": "id", "text": "name", "serias": "p", "datatype": "4", "condition": "type=4&pid=0&rt=producer&serias=p&key=producer_0_4_p" }
        , "brand": { "selectid": "brand8", "value": "id", "text": "name", "serias": "p", "datatype": "4", "condition": "type=4&pid=@pid@&rt=brand&serias=m&key=brand_@pid@_4_p" }
        , "serial": { "selectid": "serial8", "value": "id", "text": "name", "datatype": "4", "serias": "p", "condition": "type=4&pid=@pid@&serias=p&rt=serial&key=serial_@pid@_4_p" }
        , "cartype": { "selectid": "cartype8", "value": "id", "text": "name", "datatype": "4", "serias": "p", "condition": "type=4&pid=@pid@&include=1&rt=cartype&serias=p&key=cartype_@pid@_4_p" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/ProduceToSerial.aspx";
    	var dropDownBindObj8 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj8.InitBindSelect();
    </script>
    <!--End:厂商to品牌to子品牌to车型-->  
    
    <!--Start:厂商to品牌-->
    厂商to子品牌:<select id="produce9"></select><select id="serial9"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "producer": { "selectid": "produce9", "value": "id", "text": "name", "serias": "p", "datatype": "4", "condition": "type=4&pid=0&rt=producer&serias=p&key=producer_0_4_p" }
        , "serial": { "selectid": "serial9", "value": "id", "text": "name", "datatype": "4", "serias": "p", "include": "1", "condition": "type=4&pid=@pid@&serias=p&include=1&rt=serial&key=serial_@pid@_4_p" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/ProduceToSerial.aspx";
    	var dropDownBindObj9 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj9.InitBindSelect();
    </script>
    <!--End:厂商to品牌-->    
      
    <!--Start:厂商to子品牌to车型-->
    厂商to子品牌to车型:<select id="produce10"></select><select id="serial10"></select><select id="cartype10"></select><br /><br />
    <script language="javascript" type="text/javascript">
    	var selectList = { "producer": { "selectid": "produce10", "value": "id", "text": "name", "serias": "p", "datatype": "4", "condition": "type=4&pid=0&rt=producer&serias=p&key=producer_0_4_p" }
        , "serial": { "selectid": "serial10", "value": "id", "text": "name", "datatype": "4", "serias": "p", "include": "1", "condition": "type=4&pid=@pid@&serias=p&include=1&rt=serial&key=serial_@pid@_4_p" }
        , "cartype": { "selectid": "cartype10", "value": "id", "text": "name", "datatype": "4", "serias": "p", "condition": "type=4&pid=@pid@&include=1&rt=cartype&serias=p&key=cartype_@pid@_4_p" }
    	};
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/ProduceToSerial.aspx";
    	var dropDownBindObj10 = new InitDropDownList(selectList, url, "utf-8", null);
    	dropDownBindObj10.InitBindSelect();
    </script>
    <!--End:厂商to子品牌to车型--> 
      
    
    <!--Start:主品牌to子品牌-->
    <script language="javascript" type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/newlevellistgocarandprice.js"></script>
    主品牌to子品牌:<select id="master3"></select><select id="serial3"></select>
    去车型&&看报价:<input id="goCar" type="button" value="去车型" /> <input id="goPrice" type="button" value="看报价" /> <br /><br />
    <script language="javascript" type="text/javascript">
    	//点击对象
    	var btnObj = { "goCar": { "serial": { "url": "http://car.bitauto.com/@param1@/", "param1": "urlSpell" }
                                , "master": { "url": "http://car.bitauto.com/tree_chexing/mb_@param1@/", "param1": "id" }
                                , "default": { "url": "http://car.bitauto.com/"}
    	}
                     , "goPrice": { "serial": { "url": "http://price.bitauto.com/frame.aspx?newbrandid=@id@&citycode=@city@", "id": "id", "definedparam": { "city": "201"} }
                                , "master": { "url": "http://price.bitauto.com/keyword.aspx?keyword=@name@&mb_id=@id@&citycode=@city@", "id": "id", "name": "name", "definedparam": { "city": "201"} }
                                , "default": { "url": "http://price.bitauto.com/"}
                     }
    	}
    	//选择列表绑定对象
    	var selectList = { "master": { "selectid": "master3", "value": "id", "text": "name", "serias": "m", "datatype": "4", "condition": "type=4&pid=0&rt=master&serias=m&key=master_0_4_m" }
            , "serial": { "selectid": "serial3", "value": "id", "text": "name", "serias": "m", "include": "1", "datatype": "4", "condition": "type=4&pid=@pid@&include=1&rt=serial&serias=m&key=serial_@pid@_4_m" }
    	};
    	//要请求的地址
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/MasterBrandToSerialNew.aspx";

    	//初始化对象
    	var btnClickObject = new ButtonClick(selectList, btnObj, url, "utf-8", null);
    	btnClickObject.Init();
    </script>
    
    <!---Start:主品牌to子品牌-->
    主品牌to子品牌:<select id="master20"></select><select id="serial20"></select>
    去车型&&看报价:<input id="goCarTree" type="button" value="直达车型" />
    <script language="javascript" type="text/javascript">
    	//点击对象
    	var btnObj = { "goCarTree": { "serial": { "url": "http://car.bitauto.com/tree_chexing/sb_@param1@", "param1": "id" }
                                , "master": { "url": "http://car.bitauto.com/tree_chexing/mb_@param1@/", "param1": "id" }
                                , "default": { "url": "http://car.bitauto.com/" }
    	}
    	}
    	//选择列表绑定对象
    	var selectList = { "master": { "selectid": "master20", "value": "id", "text": "name", "serias": "m", "datatype": "chexing", "condition": "type=chexing&pid=0&rt=master&serias=m&key=master_0_chexing_m" }
            , "serial": { "selectid": "serial20", "value": "id", "text": "name", "serias": "m", "include": "1", "datatype": "chexing", "condition": "type=chexing&pid=@pid@&include=1&rt=serial&serias=m&key=serial_@pid@_chexing_m" }
    	};
    	//要请求的地址
    	var url = "http://car.bitauto.com/Interface/LeveldropdownlistData/watchcardirectdatainterface.aspx";

    	//初始化对象
    	var btnClickObject = new ButtonClick(selectList, btnObj, url, "utf-8", null);
    	btnClickObject.Init();
    </script>
</body>
</html>

