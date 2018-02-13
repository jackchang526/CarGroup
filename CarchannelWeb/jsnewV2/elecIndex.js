var ElecSelectCarTool = {
    Parameters: new Array()
    , apiUrl: "http://select.car.yiche.com/selectcartool/searchresult"

    , initPageCondition: function () {
        this.bindFilterClickEvent();
    }
    //绑定条件点击事件
    , bindFilterClickEvent: function () {
        //绑定复选框，单选框的点击事件
        $("input[type='checkbox'],input[type='radio']").click(this.filterClickEvent);
        //绑定查询事件
        $(".confirmButton").click(function () {
            if (SuperSelectCarTool.Parameters.toString().indexOf('page=') > -1) {
                for (var i = 0, l = SuperSelectCarTool.Parameters.length; i < l; i++) {
                    if (SuperSelectCarTool.Parameters[i].indexOf('page=') > -1) {
                        SuperSelectCarTool.Parameters.splice(i, 1);
                        break;
                    }
                }
            }
            SuperSelectCarTool.SearchCarResult();
        });
    }
}