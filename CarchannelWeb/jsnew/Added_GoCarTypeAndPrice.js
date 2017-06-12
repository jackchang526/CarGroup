// JScript 文件
/*
*Author:Wangzt
*CreateTime:2010-01-13
*Purpose:用于补充GoCarTypeAndPrice.js的一些附加逻辑，采用覆盖GoCarTypeAndPrice.js
中一些方法为手段。
注：请与GoCarTypeAndPrice.js一起使用
*/

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
            //子品牌是否包含车型信息
            var isNoContainsCarType = true;
            if(typeof IsNoContainsTypeSerial != 'undefined' && IsNoContainsTypeSerial.length > 0)
            {
                for(var i =0;i<IsNoContainsTypeSerial.length;i++)
                {
                    if(objectData.carSerial[z].id == IsNoContainsTypeSerial[i].id)
                    {
                        isNoContainsCarType = false;
                        break;
                    }
                }
            }
            if(isNoContainsCarType)
            {
                optionItem = document.createElement("OPTION");
                optionItem.setAttribute("value", objectData.carSerial[z].id);
                optionItem.appendChild(document.createTextNode(objectData.carSerial[z].name));            
                mbObject.appendChild(optionItem);
            }
        }            
    }
}

