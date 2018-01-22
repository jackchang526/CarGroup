var page = 1, sort = 4, pageSize =10;
var SelectPhotoTool = {
    Parameters: new Array()
    , FuelTypeName: { 16: "纯电动", 128: "插电混合" }
    , BrandName: { 0: "不限", 1: "自主", 2: "合资", 4: "进口" }
    , BodyFormName: { 1: "两厢", 2: "三厢", 64: "SUV" }
    , initPageCondition: function () {
        this.initFilterCondition();

        if (SelectPhotoTool.Parameters.toString().indexOf('page=') == -1) {
            SelectPhotoTool.Parameters.push("page=" + page);
        }
        if (SelectPhotoTool.Parameters.toString().indexOf("pagesize=") == -1) {
            SelectPhotoTool.Parameters.push("pagesize=" + pageSize);
        }
    }
    //初始化查询条件
    , initFilterCondition: function () {
        for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
            var idTemp = SelectPhotoTool.Parameters[i].split('=')[1];
            var typeTemp = SelectPhotoTool.Parameters[i].split('=')[0];
           
            if (typeTemp == "page") {
                page = idTemp;
            }
            var elementTemp = getElementById(typeTemp + '_' + idTemp);
            if (elementTemp) {
                setDisabled(elementTemp);                
            }
            if (idTemp > 0) {
                switch (typeTemp) {
                    case "f":
                        $("#btnFuelType").find("span").eq(0).html(this.FuelTypeName[idTemp]);
                        $("#btnFuelType").parent().addClass("current");
                        break;
                    case "g":
                        $("#btnBrandType").find("span").eq(0).html(this.BrandName[idTemp]);
                        $("#btnBrandType").parent().addClass("current");
                        break;
                    case "b":
                        $("#btnBodyform").find("span").eq(0).html(this.BodyFormName[idTemp]);
                        $("#btnBodyform").parent().addClass("current");
                        break;
                    default:
                        break;
                }
            }
        }
    }
    //设置选中项不可用
    , setDisabled: function (elem) {
        if (!elem) return;
        $(elem).parent().addClass("current").siblings().removeClass("current");
    }
    //获取请求参数
    , GetSearchQueryString: function () {
        var qsArray = new Array();
        for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
            var typeId = SelectPhotoTool.Parameters[i].split('=')[0];
            var id = SelectPhotoTool.Parameters[i].split('=')[1];
            if (typeId == "pagesize") {
                continue;
            }
            if (typeId == "page") {
                continue;
            }
            qsArray.push(typeId + "=" + id);            
        }
        return qsArray.join('&');
    }
}
function getElementById(i) { return document.getElementById(i); }
Array.prototype.remove = function (b) { var a = this.indexOf(b); if (a >= 0) { this.splice(a, 1); return true; } return false; };
Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }

function GotoPage(conditionStr) {
    if (conditionStr.length >= 1) {
        var conType = conditionStr.split('_')[0];
        var conStr = conditionStr.split('_')[1];
        var strType = conType + "=";
        if (SelectPhotoTool.Parameters.toString().indexOf(strType) > -1) {
            for (var i = 0, l = SelectPhotoTool.Parameters.length; i < l; i++) {
                if (SelectPhotoTool.Parameters[i].indexOf(strType) > -1) {
                    var delId = SelectPhotoTool.Parameters[i].split('=')[1]
                    SelectPhotoTool.Parameters.splice(i, 1);
                    break;
                }
            }
        }
        if (conStr != 0) {
            SelectPhotoTool.Parameters.push(conType + "=" + conStr);
        }
    }
    var queryString = SelectPhotoTool.GetSearchQueryString(0);
    var toUrl = 'http://' + window.location.host + window.location.pathname;;
    if (queryString.length > 0)
        toUrl += "?" + queryString;
    window.location.href = toUrl;
}