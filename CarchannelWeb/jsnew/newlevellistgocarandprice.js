/*
点击对象
_select_;下拉列表绑定所用到的对象
_btnobject_:按钮事件所用到的函数
_url_:请求数据用到的对象
_encode_:脚本的编码方式
_checkdata_:用于数据筛选的方式
_selectObj_:绑定控件用得对象
*/
function ButtonClick(sList, bObj, initUrl, encode, checkData) {
    this._select_ = sList;
    this._btnobject_ = bObj;
    this._url_ = initUrl;
    this._encode_ = encode;
    this._checkdata_ = checkData;
    this._selectObj_ = null;
}
/*
控件初始化
*/
ButtonClick.prototype.Init = function() {
    //初始化下拉列表
    this._selectObj_ = new InitDropDownList(this._select_, this._url_, this._encode_, this._checkdata_);
    this._selectObj_.InitBindSelect();
    //初始化控件onclick绑定事件
    this.InitButtonOnClickEvent();
}
/*
初始化控件onclick绑定事件
*/
ButtonClick.prototype.InitButtonOnClickEvent = function() {
    if (typeof this._btnobject_ == 'undefined' || this._btnobject_ == null) return;
    //循环为对象添加属性
    for (var id in this._btnobject_) {
        var controlObj = document.getElementById(id);
        if (!controlObj) continue;
        var pro = this;
        controlObj.onclick = function(btnid) { return function() { pro.Click(btnid); } } (id)
    }
}
/*
用户点击控件处理的事件
*/
ButtonClick.prototype.Click = function(id) {
    if (id == "") return;
    var opObj = this._btnobject_[id]; //按钮操作对象
    if (opObj == null) return;
    /*
    循环看用户选择了那个品牌
    */
    var gourl = "";
    for (var type in opObj) {
        if (type == "default") { gourl = opObj[type]["url"]; break; }
        var selectObject = this._selectObj_.BindSelect; //得到绑定对象的数据
        if (typeof selectObject == 'undefined' || selectObject == null) continue;
        var userSelectValue = selectObject.GetValue(type);
        if (parseInt(userSelectValue) == 0) continue;
        /*拼接用户需要达到的链接*/
        gourl = this.BindUrl(opObj[type], selectObject, type);
        break;
    }
    //如果地址不为空，跳转
    if (gourl != "") window.open(gourl, "", "", "");
}
/*
拼接用户链接
*/
ButtonClick.prototype.BindUrl = function(urlObj, selectObj, type) {
    if (urlObj == null || selectObj == null || type == "") return "";
    var urltemp = urlObj["url"]; //得到链接模板
    var bindObject = selectObj.GetValueObject(type); //得到绑定值对应的对象
    if (bindObject == null) return "";
    var pattern;
    for (var id in urlObj) {
        if (id == "url") continue;
        if (id == "definedparam") {
            for (var param in urlObj[id]) {
                pattern = new RegExp("@" + param + "@", "gi");
                urltemp = urltemp.replace(pattern, urlObj[id][param]); //得到要绑定的url
            }
            continue;
        }
        //要替换的规则
        pattern = new RegExp("@" + id + "@", "gi");
        var value = bindObject[urlObj[id]];
        if (id == "name") value = encodeURI(value);
        urltemp = urltemp.replace(pattern, value); //得到要绑定的url
    }
    return urltemp;

}