<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_brand.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTreeV2.CX_brand" %>
<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>【<%=_BrandName%>汽车】<%=_BrandName%>汽车报价_介绍_全部<%=_BrandContainsSerialCount%>款车型-易车网</title>
     <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
       <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <meta name="Keywords" content="<%=_BrandName%>,<%=_BrandName%>汽车,易车网" />
    <meta name="Description" content="<%=_BrandName%>汽车:易车网车型频道为您提供<%=_BrandName%>汽车介绍,<%=_BrandName%>汽车全部<%=_BrandContainsSerialCount%>款车型最新汽车报价/价格,图片,参数配置,经销商,导购,评测,图解,行情,更多<%=_BrandName%>汽车信息尽在易车网" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_chexingshuxing_style-1259.shtml" -->
 </head>
<body>
	<span id="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
<!--顶通-->
    <!--#include file="~/htmlV2/header2016.shtml"-->
    <!--#include file="~/include/pd/2016/common/00001/201607_ejdh_common_Manual.shtml" --> 
    <div class="container container cartype-section">
        <div class="col-xs-3">
            <!-- 树形 start -->
            <div class="treeNav" id="treeNav">
                <!-- 字母导航 start -->
                <div class="treeNum-box" id="treeNum">
                </div>
                <!-- 字母导航 end -->
                <!-- 树形列表 start   -->
                <div class="treeWarp">
                    <div class="treeCon">
                        <!-- 热门车列表 start -->
                        <div class="car-list" id="carList">
                        </div>
                        <!-- 热门车列表 end-->
                        <!-- 品牌选择 start -->
                        <div class="tree-list" id="treeList">
                            <ul class="list-con">
                            </ul>
                        </div>
                        <!-- 品牌选择 end -->
                    </div>
                </div>
                <!-- 树形列表 end -->
            </div>
            <!-- 树形 end -->
        </div>
        <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
        <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/c/c.js?s=ibt_userCars"></script>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/lefttree.v2.min.js?v=2016122817"></script>
        <%--   <script type="text/javascript" src="/jsnewV2/lefttree.v2.js"></script>--%>
        <script type="text/javascript">
            BitautoLeftTreeV2.init({
                likeDefLink:"http://car.bitauto.com/{allspell}/",//serailId  allspell
                params: {
                    tagtype: "chexing",
                    pagetype: "brand",
                    objid: <%=_BrandId %>
                }
        });
        </script>
        <!--右侧内容-->
        <div class="col-xs-9">
            <div class="cartype-section-main main-2">
                <%=NavPathHtml%>
                <!-- 子品牌及品牌列表 -->
                <%=_SerialList%>
                <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/CommonFunction.min.js?v=2016122817"></script>
                <script type="text/javascript">
                    var CarCommonBSID = "<%= _BrandEntity.MasterBrandId %>"; //大数据组统计用
                    var CarCommonCBID = '<%= _BrandId.ToString() %>';
                    function all_func() {
                        try {
                            tabs("divCsLevelIndex", "divCsLevel", null, true);
                        }
                        catch (err) { }
                    }
                    addLoadEvent(all_func);
                    // 如果没有选中的 选中全部
                    var firstTab;
                    var hasCurrentTab = false;
                    var tab_headCheck = document.getElementById("divCsLevelIndex");
                    if (tab_headCheck) {
                        var alis = tab_headCheck.getElementsByTagName("li");
                        for (var i = 0; i < alis.length; i++) {
                            if (i == 0)
                            { firstTab = alis[0]; }
                            if (alis[i].className == "current")
                            { hasCurrentTab = true; break; }
                        }
                        if (!hasCurrentTab) {
                            tabsRemove(0, "divCsLevelIndex", "divCsLevel", null);
                        }
                    }
                </script>
            </div>
             <!--#include file="~/htmlV2/treefooter2016.shtml"-->
        </div>
    </div>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://img4.bitautoimg.com/uimg/car/js/tabs_20140512_4.js"></script>
    
<script type="text/javascript">
	var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
	document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
</script>
<!--#include file="~/htmlV2/CommonBodyBottom.shtml"--><!-- 艾瑞 -->
</body>
</html>
