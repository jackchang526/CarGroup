<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectByFeel_Sub_1_V4.aspx.cs" Inherits="H5Web.xuanche.SelectByFeel_Sub_1_V4" %>

<!DOCTYPE HTML>
<html>
<head>
    <title>智选车</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <meta content="yes" name="apple-mobile-web-app-capable"/>
    <meta content="black" name="apple-mobile-web-app-status-bar-style"/>
    <meta content="telephone=no" name="format-detection"/>

    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期再修改/css/style.css"/>
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED产出物/02-移动应用/12-四级/前端/第四极二期再修改/css/zhixuanche.css"/>

</head>
<body class="zhixuanche ganjue">
<header>
    <ul class="xuanche_class">
        <li>
            <a href="/chebiaodang/">车标党</a>
        </li>
        <li class="current">
            <a href="/ganjuekong/">感觉控</a>
        </li>
        <li>
            <a href="/fashaoyou/">发烧友</a>
        </li>
    </ul>
</header>
<div class="ganjue_step2">
    <div class="keyword">
        <div class="box-center box-center1">
            <div>
                <span class="kw1" w="1000">急性子<em></em></span>
            </div>
            <div>
                <span class="kw3" l="9">夜店小王子<em></em></span>
            </div>
        </div>
        <div class="box-center box-center2">
            <div>
                <span class="kw2" w="1003">车里宽敞<em></em></span></div>
            <div>
                <span class="kw1" w="1004">人车合一<em></em></span>
            </div>
            <div>
                <span class="kw2" w="1002">颜控<em></em></span>
            </div>
        </div>
        <div class="box-center box-center1">
            <div>
                <span class="kw3" w="1006">苦逼上班族<em></em></span>
            </div>
            <div>
                <span class="kw2" w="1005">肌肉车<em></em></span>
            </div>
        </div>
        <div class="box-center box-center2">
            <div>
                <span class="kw1" l="8">驴友<em></em></span>
            </div>
            <div>
                <span class="kw1" w="1001">坐着舒服<em></em></span>
            </div>
            <div>
                <span class="kw3" more="174">惜命三郎<em></em></span>
            </div>
        </div>
        <div class="box-center box-center3">
            <div>
                <span class="kw3" g="1">国货当自强<em></em></span>
            </div>
            <div>
                <span class="kw2" f="16,2">节能减排<em></em></span>
            </div>
        </div>
        <div class="box-center box-center4">
            <div>
                <span class="kw2" w="1007">出门零碎多<em></em></span>
            </div>
            <div>
                <span class="kw1" more="266">我家人口多<em></em></span>
            </div>
        </div>
    </div>
    <div class="button_del_box">
        <button class="button_del">删除</button>
        <button class="button_del_other"></button>
    </div>
</div>
</body>
</html>
<script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
<script type="text/javascript">
    
    var wArr = [];
    var moreArr = [];
    var gArr = [];
    var fArr = [];
    var lArr = [];

    function getCondition() {
        var condition = "?page=1&pagesize=6&from=feel";
        if (wArr.length > 0) {
            condition += "&w=" + wArr.join('_');
        }
        if (moreArr.length > 0) {
            condition += "&m=" + moreArr.join(',');
        }
        if (gArr.length > 0) {
            condition += "&g=" + gArr.join(',');
        }
        if (fArr.length > 0) {
            condition += "&f=" + fArr.join(',');
        }
        if (lArr.length > 0) {
            condition += "&l=" + lArr.join(',');
        }
        return condition;
    }

    //更新符合查询条件的车型数
    function GetCarTotalityItem() {
       var apiUrl = "http://select24.car.yiche.com/selectcartoolV2/searchresult";
       $.ajax({
            url: apiUrl+getCondition(),
            //cache: true,
            dataType: 'jsonp',
            jsonp: 'callback',
            jsonpCallback: "jsonpCallback",
            success: function(data) {
                if (typeof data == "undefined") return;
                if (data == null) return;

                var serialCount = data.Count;
                var carCount = data.ResList.count;
                $(".button_del_other").html("有" + serialCount + "款车型符合要求");
            }
        });
    }

    function AddEleToArr(arr,ele) {
        if (typeof ele != "undefined" && ele != "" && arr.indexOf(ele) < 0) {
            arr.push(ele);
        }
    }

    function RemoveEleFromArr(arr,ele) {
        var index = arr.indexOf(ele);
        if (index > -1) {
            arr.splice(index, 1);
        }
    }

    function BindToggle() {
        $(".box-center div").toggle(
            function () {
                $(this).addClass("current");
                var wval = $($(this).children().get(0)).attr("w");
                if (typeof wval != "undefined" && wval != "") {
                    AddEleToArr(wArr, wval);
                }
                var moreval = $($(this).children().get(0)).attr("more");
                if (typeof moreval != "undefined" && moreval != "") {
                    AddEleToArr(moreArr, moreval);
                }

                var gval = $($(this).children().get(0)).attr("g");
                if (typeof gval != "undefined" && gval != "") {
                    AddEleToArr(gArr, gval);
                }

                var fval = $($(this).children().get(0)).attr("f");
                if (typeof fval != "undefined" && fval != "") {
                    var res = fval.split(',');
                    for (var i = 0; i < res.length; i++) {
                        AddEleToArr(fArr, fval[i]);
                    }
                }

                var lval = $($(this).children().get(0)).attr("l");
                if (typeof lval != "undefined" && lval != "") {
                    AddEleToArr(lArr, lval);
                }
                
                GetCarTotalityItem();
            },
            function () {
                $(this).removeClass("current");
                var wval = $($(this).children().get(0)).attr("w");
                if (typeof wval != "undefined" && wval != "") {
                    RemoveEleFromArr(wArr, wval);
                }
                var moreval = $($(this).children().get(0)).attr("more");
                if (typeof moreval != "undefined" && moreval != "") {
                    RemoveEleFromArr(moreArr, moreval);
                }

                var gval = $($(this).children().get(0)).attr("g");
                if (typeof gval != "undefined" && gval != "") {
                    RemoveEleFromArr(gArr, gval);
                }

                var fval = $($(this).children().get(0)).attr("f");
                if (typeof fval != "undefined" && fval != "") {
                    var res = fval.split(',');
                    for (var i = 0; i < res.length; i++) {
                        RemoveEleFromArr(fArr, fval[i]);
                    }
                    
                }

                var lval = $($(this).children().get(0)).attr("l");
                if (typeof lval != "undefined" && lval != "") {
                    RemoveEleFromArr(lArr, lval);
                }
                GetCarTotalityItem();
            }
        );
    }

    $(function () {

        //初始化页面数据
        GetCarTotalityItem();
        //绑定事件
        BindToggle();
        //删除按钮事件绑定
        $(".button_del").bind("click", function() {
            $(".box-center div").removeClass("current");
            wArr = [];
            moreArr = [];
            gArr = [];
            fArr = [];
            lArr = [];
            //初始化数据
            GetCarTotalityItem();
            //解除单击事件
            $(".box-center div").unbind("click");
            //重新绑定
            BindToggle();
        });

        //删除按钮事件绑定
        $(".button_del_other").bind("click", function () {
            window.location.href = "/xuanche/SearchCarListV4.aspx"+getCondition();
        });
    });

</script>