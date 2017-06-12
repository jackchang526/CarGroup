
function Baa(mId, sId, cFId, cSId, goAsk, goBaa, goFriend) {
    SelectInitObject.call(this, mId, sId, null, null);
    this.m_ProvinceId = cFId;
    this.m_CityId = cSId;
    this.m_AskId = goAsk;
    this.m_BaaId = goBaa;
    this.m_FriendId = goFriend;
    this.m_Ask = "http://ask.bitauto.com/browse/@ID@/";
    this.m_Baa = "http://api.baa.com.cn/go2baa.aspx?brandid=@ID@";
    this.m_Friend = "http://i.bitauto.com/FriendSeach_@CityID@_@ID@.html";
    this.cityList = "http://car.bitauto.com/car/ajaxnew/GetCityList.ashx?type=c&pId=@ID@&@rad@"
}

Baa.prototype = new SelectInitObject();
Baa.prototype.Init = function() {
    var pro = this;
    var mainBrandObject = document.getElementById(this.m_MainId);
    if (!mainBrandObject || mainBrandObject.nodeName.toLowerCase() != "select") {
        alert("init MainBrand Fault!");
        return;
    }
    var secondBrandObject = document.getElementById(this.m_SecondId);
    if (!secondBrandObject == null || secondBrandObject.nodeName.toLowerCase() != "select") {
        alert("init CarSerial Fault!")
        return;
    }
    if (typeof JSonData == "undefined" || JSonData.length < 1) return;
    var provinceObject = document.getElementById(this.m_ProvinceId);
    if (provinceObject) { pro.BindProviceSelect(); }
    var cityObject = document.getElementById(this.m_CityId);
    if (cityObject) { pro.BindCitySelect(null); }
    this.BindMainObject(JSonData.masterBrand, mainBrandObject);
    this.BindSecondObject(null, secondBrandObject);
    var pBtn = document.getElementById(this.m_AskId);
    var aBtn = document.getElementById(this.m_BaaId);
    var dBtn = document.getElementById(this.m_FriendId);

    if (pBtn) pBtn.onclick = function() { pro.BindAskClickObject(); }
    if (aBtn) aBtn.onclick = function() { pro.BindBaaClickObject(); }
    if (dBtn) dBtn.onclick = function() { pro.BindFriendClickObject(); }
}
//绑定省市列表
Baa.prototype.BindProviceSelect = function() {
    
    var pObj = document.getElementById(this.m_ProvinceId);
    if (!pObj || typeof provincelist == 'undefined') return;
    DomHelper.clearControlNode(pObj); 
    pObj.appendChild(DomHelper.createOption("0", "全国"));
    for (var i in provincelist) {
        pObj.appendChild(DomHelper.createOption(i.replace(/p/g, ""), provincelist[i]));
    }

    var pro = this;
    pObj.onchange = function() { pro.ProviceChange(); }
}
//绑定城市列表
Baa.prototype.BindCitySelect = function(data) {
    var cObj = document.getElementById(this.m_CityId);
    if (!cObj) return;
    DomHelper.clearControlNode(cObj);
    cObj.appendChild(DomHelper.createOption("-1", "请选择城市"));

    for (var i in data) {
        cObj.appendChild(DomHelper.createOption(i.replace(/c/g, ""), data[i]));
    }
}
//绑定事件改变
Baa.prototype.ProviceChange = function() {
    var pObj = document.getElementById(this.m_ProvinceId);
    if (!pObj) return;
    var currentValue = parseInt(pObj.options[pObj.selectedIndex].value);
    if (currentValue < 1) { this.BindCitySelect(null); return; }

    var url = this.cityList;
    url = url.replace(/@ID@/g, currentValue);
    url = url.replace(/@rad@/g, Math.random());
    if (typeof loadJS == 'undefined') return;
    var pro = this;
    loadJS.push(url, "utf-8", function() { pro.OperationRequest(); });
}
//处理回调请求
Baa.prototype.OperationRequest = function() {
    if (typeof citylist == 'undefined') { this.BindCitySelect(null); return; }
    this.BindCitySelect(citylist);
}
Baa.prototype.BindAskClickObject = function() {
    var sObj = document.getElementById(this.m_SecondId);
    if (!sObj) return;
    var currenValue = parseInt(sObj.options[sObj.selectedIndex].value);
    if (currenValue < 1) return;

    var url = this.m_Ask;
    url = url.replace(/@ID@/g, currenValue);
    window.open(url, "", "", "");
}
Baa.prototype.BindBaaClickObject = function() {
    var sObj = document.getElementById(this.m_SecondId);
    if (!sObj) return;
    var currenValue = parseInt(sObj.options[sObj.selectedIndex].value);
    if (currenValue < 1) return;

    var url = this.m_Baa;
    url = url.replace(/@ID@/g, currenValue);
    window.open(url, "", "", "");
}
Baa.prototype.BindFriendClickObject = function() {
    var cityId = 0;
    var sObj = document.getElementById(this.m_SecondId);
    if (!sObj) return;
    var currensValue = parseInt(sObj.options[sObj.selectedIndex].value);
    if (currensValue < 1) return;
    var cObj = document.getElementById(this.m_CityId);
    if (cObj) {
        var currentcValue = parseInt(cObj.options[cObj.selectedIndex].value);
        cityId = currentcValue > 0 ? currentcValue : 0;
    }

    var url = this.m_Friend; //http://i.bitauto.com/FriendSeach_@CityID@_@ID@.html
    url = url.replace(/@CityID@/g, cityId);
    url = url.replace(/@ID@/g, currensValue);
    window.open(url, "", "", "");
}