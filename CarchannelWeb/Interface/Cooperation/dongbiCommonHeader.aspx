<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dongbiCommonHeader.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.Cooperation.dongbiCommonHeader" %>

<style>
@charset "utf-8";
/*reset*/
body .bt_page { font-size:12px; font-family:\5b8b\4f53,Arial, Helvetica, sans-serif; background:#fff; line-height:150%; }
.bt_page table { border-collapse:collapse; border-spacing:0; }
.bt_page  img { border:0; }
.bt_page  em, .bt_page strong{ font-style:normal; font-weight:normal; }
.bt_page ol, .bt_page ul , .bt_page dl, .bt_page dt, .bt_page dd, .bt_page li{ list-style:none; margin:0; padding:0;}
.bt_page a:link, .bt_page a:visited { color:#4d4d4d; text-decoration:none; }
.bt_page a:hover { color:#c00; text-decoration:underline; }
.bt_page a strong { color:#c00; }
.bt_page { width:980px; margin:0 auto; text-align:left; background:#fff; }
.clear { clear:both; height:0; overflow:hidden; }






.model_city { position:relative; z-index:9999; background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_jy2.png) no-repeat 0 0; height:31px; padding:10px 0 0 0; width:980px; margin:0 auto; }
.model_city .select_model { width:130px; float:left; margin-right:5px; color:#666; }
.model_city .select_car { background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_jy2.png) no-repeat -42px -88px; width:53px; border:0; color:#000; text-align:center; font-size:12px;   float:left; margin-right:5px; padding:3px 0 2px; *padding:4px 0 1px;
padding:6px 0 3px 0\0; cursor:pointer; }
.model_city ul { float:left; margin-top:2px; padding-left:5px; display:inline; }
.model_city dl.search_arrow { position:absolute; right:5px; top:10px; }
.model_city dl dt { float:left; color:#fff; font-weight:bold; }
.model_city dl dd { float:left; background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_jy2.png) no-repeat -3px -66px; padding-left:7px; white-space:nowrap; width:43px; overflow:hidden; }
.model_city dl dd a:link, .model_city dl dd a:visited, .model_city dl dt a:link, .model_city dl dt a:visited { color:#fff; }
.model_city .select_sea { width:130px; float:left; margin-right:5px; margin-top:1px; border:1px solid #3B566D; padding:2px 0 1px 5px; color:#666; }
.model_city .select_more { background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_jy2.png) no-repeat 0 -88px; width:42px; border:0; color:#000; text-align:center; font-size:12px;   float:left; padding:3px 0 2px; *padding:4px 0 1px;
padding:5px 0 3px 0\0; cursor:pointer; }
.model_city .search_text { position:relative; float:left; margin:0 20px 0 15px; display:inline; }
.model_city .search_text a.search { float:left; display:block; height:22px; width:65px; background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_jy2.png) no-repeat 0 -43px; line-height:90px; overflow:hidden; }

.c0621_01{ height:107px; overflow:hidden; float:left; width:702px;}
.c0621_01 dl{float:left;width:702px;margin:0 0 0 8px;padding:3px 0;display:inline;background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_dot2.png) 0 100% repeat-x;}
.c0621_01 dt{float:left;width:67px;padding:0;color:#656565;}
.c0621_01 dd{float:left;padding:0 0 1px 0;width:620px;}
.c0621_01 li{float:left;margin-right:23px;display:inline;white-space:nowrap;line-height:20px;height:20px;}
.c0621_01 .current a{padding:0 3px;display:block;background:#9c0808;color:#fff;}



.car_model_list { width:275px; float:left; height:107px; border-left:1px solid #DCDDDD; position:relative; font-family:simSun; }
.car_model_list ul { border-top:1px solid #fff; border-bottom:1px solid #DCDDDD; height:24px; background:#f1f1f1; *overflow:hidden; }
.car_model_list ul li { float:left; border-left:1px solid #fff; border-right:1px solid #DCDDDD; text-align:center; width:90px; }
.car_model_list ul li.current { height:23px; position:relative; background:#fff; border-top:2px #c00 solid; }
.car_model_list ul li.last { border-right:none; }
.car_model_list ul li a { display:block; height:25px; font-weight:bold; line-height:25px; float:left; width:90px; }
.car_model_list ul li a:link, .car_model_list ul li a:visited { text-decoration:underline; }
.car_model_list ul li.current a { color:#CC0000; line-height:23px; }
/*010127*/
.car_model_list dl { background:url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_dot2.png) repeat-x scroll 0 0; }
.car_model_list dl{
display:inline;
float:left;
padding:3px 0;
}
.car_model_list dl dd{
float:left;
padding:0 0 1px 0;
}


.car_model_list dl.fist { background:#F8F8F8 url(http://car.bitauto.com/Interface/Cooperation/images/bt_bg_dot2.png) repeat-x scroll 0 0; }
.car_model_list dl.none{ background:none;}
/*010127*/
.car_model_list dl a { line-height:20px;  }
.car_model_list dl a:link span, .car_model_list dl a:visited span { display:none; }
.car_model_list dl a:hover { margin:0; }
.car_model_list dl a:hover span { display:block; border:1px #164a84 solid; background:#fff; text-decoration:none; }
.car_model_list dl a span { position:absolute; top:14px; left:0; width:173px; color:#000; text-align:left; padding:2px 0 0 5px; cursor:default; line-height:17px; }
.car_model_list dl a span em { color:#c00; font-weight:bold; line-height:20px; }
.car_model_list dl dt { float:left; width:80px; text-align:center; }
.car_model_list dl dd { float:left; width:91px; text-align:center; height:auto;  }
.car_model_list dl dd.more { position:inherit; top:auto; right:auto; padding-right:0; /*width:50px;*/ }
.car_model_list dl dd.more a { font-family:simSun; }


.contain{border:1px solid #DEE3E7;height:107px; }



</style>
<div class="bt_page">
    <div class="model_city">
        <div class="search_text">
            <!--#include file="~/ushtml/block/so/so_css.shtml"-->
            <!--#   include file="~/ushtml/block/so/so_bar_new_cityindex.shtml"-->
            <!--块内容如下-->
            <style  type="text/css"> 
            #sug ul li.ml_l {background:#fff; color:#164A84}
            .sug_reset { background:#fff }
            .sug_onblur { background:#fff url(http://image.bitauto.com/bt/logo/so_search_bg.gif) no-repeat right center }
            </style>
            <a target="_blank" class="search" href="http://www.cheyisou.com/" title="车易搜">车易搜</a> 
            <div style="position: relative; float: left; text-align: left;">
                <form id="sug_form" name="sug_form" target="_blank" method="get" action="http://www.cheyisou.com/post.aspx">    
                    <input type="text" class="select_sea" id="sug_txt_keyword" name="so_keywords" value="" />
                    <input type="submit" value="搜索" id="sug_submit" class="select_more" onclick="BSearchRGo()" />
                    <div onselectstart="return false" id="sug" style="top: 24px; float: left; overflow: hidden"></div>        
                    <input id="sug_hidurl" value="" type="hidden" />
                    <input id="sug_datatype" name="sug_datatype" value="qiche" type="hidden" />
                    <input id="sug_encoding" name="sug_encoding" value="gb2312" type="hidden" />
                </form>        
            </div>
            <script type="text/javascript">
                var sug_txt_name = "sug_txt_keyword";
                var sug_txt_words = [];
                sug_txt_words.push('大众');
                sug_txt_words.push('现代');
                sug_txt_words.push('雪佛兰');
                sug_txt_words.push('别克');
                sug_txt_words.push('丰田');
                sug_txt_words.push('日产');
                var i = Math.round(Math.random() * sug_txt_words.length);
                if (i >= 0 && i <= 5)
                    document.getElementById(sug_txt_name).value = sug_txt_words[i];  
            </script>
        </div>
        <select class="select_model" id="MasterSelectList">
            <option value="-1">请选择品牌</option>
        </select>
        <select class="select_model" id="SerialSelectList">
            <option value="-1">请选择系列</option>
        </select>
        <input type="button" value="看车型" class="select_car" title="看车型" id="GotoCarButton">
        <input type="button" value="查报价" title="查报价" class="select_car" id="GotoPriceButton">
        <dl class="search_arrow">
            <dt><a href="http://car.bitauto.com/" target="_blank">汽车大全</a>：</dt>
            <dd>
                <a href="http://car.bitauto.com/brandlist.html" target="_blank">按品牌</a></dd>
            <dd>
                <a href="http://car.bitauto.com/charlist.html" target="_blank">按车型</a></dd>
            <dd>
                <a href="http://car.bitauto.com/countrylist.html" target="_blank">按国别</a></dd>
            <dd>
                <a href="http://car.bitauto.com/functionlist.html" target="_blank">按用途</a></dd>
        </dl>
    </div>
    <div class="contain">
        <div id="showhideCon" class="c0621_01" style="">
            <dl>
                <dt>价 格：</dt>
                <dd>
                    <ul>
                        <li id="price0" class="current"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=">
                            不限</a></li>
                        <li id="price1"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=0-5">
                            5万以下</a></li>
                        <li id="price2"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=5-8">
                            5-8万</a></li>
                        <li id="price3"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=8-12">
                            8-12万</a></li>
                        <li id="price4"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=12-18">
                            12-18万</a></li>
                        <li id="price5"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=18-25">
                            18-25万</a></li>
                        <li id="price6"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=25-40">
                            25-40万</a></li>
                        <li id="price7"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=40-80">
                            40-80万</a></li>
                        <li id="price8"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?p=80-9999">
                            80万以上</a></li>
                    </ul>
                </dd>
            </dl>
            <dl>
                <dt>级 别：</dt>
                <dd>
                    <ul>
                        <li id="level0" class="current"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=0">
                            不限</a></li>
                        <li id="level1"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=1">
                            微型车</a></li>
                        <li id="level2"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=2">
                            小型车</a></li>
                        <li id="level3"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=3">
                            紧凑型车</a></li>
                        <li id="level5"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=5">
                            中型车</a></li>
                        <li id="level4"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=4">
                            中大型车</a></li>
                        <li id="level6"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=6">
                            豪华车</a></li>
                        <li id="level7"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=7">
                            MPV</a></li>
                        <li id="level8"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=8">
                            SUV</a></li>
                        <li id="level9"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=9">
                            跑车</a></li>
                        <li id="level11"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?l=11">
                            面包车</a></li>
                    </ul>
                </dd>
            </dl>
            <dl>
                <dt>排 量：</dt>
                <dd>
                    <ul>
                        <li id="dis0" class="current"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=">
                            不限</a></li>
                        <li id="dis1"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=0-1.3">
                            1.3以下</a></li>
                        <li id="dis2"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=1.3-1.6">
                            1.3-1.6L</a></li>
                        <li id="dis3"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=1.7-2.0">
                            1.7-2.0L</a></li>
                        <li id="dis4"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=2.1-3.0">
                            2.1-3.0L</a></li>
                        <li id="dis5"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=3.1-5.0">
                            3.1-5.0L</a></li>
                        <li id="dis6"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?d=5.0-9">
                            5.0L以上</a></li>
                    </ul>
                </dd>
            </dl>
            <dl>
                <dt>变速箱：</dt>
                <dd>
                    <ul>
                        <li id="trans0" class="current"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?t=0">
                            不限</a></li>
                        <li id="trans1"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?t=1">
                            手动(MT)</a></li>
                        <li id="trans2"><a target="_blank" href="http://car.bitauto.com/tree_chexing/search/?t=2">
                            自动(AT)</a></li>
                    </ul>
                </dd>
            </dl>
            <div class="hideline">
            </div>
        </div>
        <div class="car_model_list">
            <ul id="car_tab_ul">
                <li class="current"><a href="http://car.bitauto.com/" target="_blank">热门车型</a></li>
                <li><a href="http://kan.bitauto.com/xinche/" target="_blank">上市新车</a></li>
                <li class="last"><a href="http://car.bitauto.com/brandlist.html" target="_blank">热门品牌</a></li>
            </ul>
            <div id="data_table_0">
                <%=m_PvSerailList%>
            </div>
            <div id="data_table_1" style="display: none;">
                <%=m_newSerialList%>
            </div>
            <div id="data_table_2" style="display: none;">
                <dl class="none">
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/volkswagen/">大众</a>
                    </dd>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/toyota/">丰田</a>
                    </dd>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/honda/">本田</a>
                    </dd>
                </dl>
                <dl class="fist">
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/nissan/">日产</a>
                    </dd>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/audi/">奥迪</a>
                    </dd>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/chevrolet/">雪佛兰</a>
                    </dd>
                </dl>
                <dl>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/hyundai/">现代</a>
                    </dd>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/bmw/">宝马</a>
                    </dd>
                    <dd>
                        <a target="_blank" href="http://car.bitauto.com/buick/">别克</a>
                    </dd>
                </dl>
            </div>
        </div>
    </div>
</div>
<!--车型搜索-->
<!--#include file="~/ushtml/block/so/so_bar_js_index.shtml"-->
<%--<script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsCommon/jsGoCarAndPriceJsonNew.js"></script>

        <script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsnew/GoCarTypeAndPrice.js"></script>

        <script type="text/javascript" language="javascript">
            var PageSelectObject = null;
            if (JSonData) {
                PageSelectObject = StartInit("select", "select", "MasterSelectList", "SerialSelectList", "GotoCarButton", "GotoPriceButton", 0);
                PageSelectObject.Init();
            }
        </script>--%>
<script type="text/javascript" language="javascript" charset="UTF-8" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlist.js?v=3.0"></script>
<script>
	BitA.DropDownList({
		container: { master: "MasterSelectList", serial: "SerialSelectList" },
		include: { serial: "1" },
		btn: {
			car: {
				id: "GotoCarButton",
				url: {
					serial: { "url": "http://car.bitauto.com/{param1}/", params: { "param1": "urlSpell"} },
					master: { "url": "http://car.bitauto.com/tree_chexing/mb_{param1}/", params: { "param1": "id"} }
				}
			},
			price: {
				id: "GotoPriceButton",
				url: {
					serial: { "url": "http://price.bitauto.com/brand.aspx?newbrandid={id}", params: { "id": "id"} },
					master: { "url": "http://price.bitauto.com/keyword.aspx?keyword={name}&mb_id={id}", params: { "id": "id", "name": "name"} }
				}
			}
		},
		bind: "click"
	});
</script>
<script type="text/javascript">
	function addLoadEvent(func) {
		var oldonload = window.onload;
		if (typeof window.onload != 'function') {
			window.onload = func;
		} else {
			window.onload = function () {
				oldonload();
				func();
			}
		}
	}

	function addClass(element, value) {
		if (!element.className) {
			element.className = value;
		} else {
			newClassName = element.className;
			newClassName += " ";
			newClassName += value;
			element.className = newClassName;
		}
	}

	function removeClass(element, value) {
		var removedClass = element.className;
		var pattern = new RegExp("(^| )" + value + "( |$)");
		removedClass = removedClass.replace(pattern, "$1");
		removedClass = removedClass.replace(/ $/, "");
		element.className = removedClass;
		return true;
	}


	function all_func() {
		tabs("car_tab_ul", "data_table", null, true);
	}
	addLoadEvent(all_func)

	/*=======================tab=============================*/
	function hide(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "none" } }
	function show(id) { var Div = document.getElementById(id); if (Div) { Div.style.display = "block" } }

	function tabsRemove(index, head, divs, div2s) {
		if (!document.getElementById(head)) return false;
		var tab_heads = document.getElementById(head);
		if (tab_heads) {
			var alis = tab_heads.getElementsByTagName("li");
			for (var i = 0; i < alis.length; i++) {
				removeClass(alis[i], "current");

				hide(divs + "_" + i);
				if (div2s) { hide(div2s + "_" + i) };

				if (i == index) {
					addClass(alis[i], "current");
				}
			}

			show(divs + "_" + index);
			if (div2s) { show(div2s + "_" + index) };
		}
	}

	function tabs(head, divs, div2s, over) {
		if (!document.getElementById(head)) return false;
		var tab_heads = document.getElementById(head);

		if (tab_heads) {
			var alis = tab_heads.getElementsByTagName("li");
			for (var i = 0; i < alis.length; i++) {
				alis[i].num = i;


				if (over) {
					alis[i].onmouseover = function () {
						var thisobj = this;
						thetabstime = setTimeout(function () { changetab(thisobj); }, 150);
					}
					alis[i].onmouseout = function () {
						clearTimeout(thetabstime);
					}
				}
				else {
					alis[i].onclick = function () {
						if (this.className == "current" || this.className == "last current") {
							changetab(this);
							return true;
						}
						else {
							changetab(this);
							return false;
						}

					}
				}

				function changetab(thebox) {
					tabsRemove(thebox.num, head, divs, div2s);
				}

			}
		}
	}

</script>