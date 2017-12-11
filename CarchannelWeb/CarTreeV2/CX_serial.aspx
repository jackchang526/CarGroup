<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CX_serial.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarTreeV2.CX_serial" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
	<title>【<%=_SerialSeoName %>】<%=_BrandName%><%=_SerialName%>_<%=_SerialSeoName %>报价_导购_评测_行情-易车网
	</title>
     <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
        <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
	<meta name="Keywords" content="<%=_SerialSeoName %>,<%=_SerialSeoName %>报价,<%=_MetaKeyWordArea %>,易车网" />
	<meta name="Description" content="<%=_SerialSeoName %>:易车网车型频道为您提供<%=_SerialSeoName %>报价/价格,<%=_SerialSeoName %>图片,<%=_SerialSeoName %>参数配置,经销商,视频,导购,评测,图解,行情,更多<%=_SerialSeoName %>汽车信息尽在易车网" />
	<!--#include file="~/ushtml/0000/yiche_2016_cube_chexingshuxing_style-1259.shtml" -->
	<script type="text/javascript">
		var _SearchUrl = '<%=this._SearchUrl %>';
	</script>
	<style type="text/css">
		.onsale { overflow: hidden; }
		.c0624_06 label { font-weight: normal; color: #999999; }
		.comparetable { float: left; margin: 0 0 -1px -1px; overflow: hidden; width: 718px; }
		.compare2 { margin: -1px 0 0 1px; width: 718px; overflow: hidden; }
			.compare2 td { border-left: none; border-right: none; }
			.compare2 th { background: #F5F7F9; border: none; color: #999999; font-weight: normal; height: 27px; text-align: center; }
				.compare2 th.firstItem { color: #333333; padding-left: 10px; text-align: left; }
		.tc-popup-box { position: fixed; margin: -131px 0 0 -249px; z-index: 99999; left: 50%; top: 50%; }
	</style>
</head>
<body>
	<span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
	<!--顶通-->
	<!--#include file="~/htmlV2/header2016.shtml"-->
	<!--#include file="~/include/pd/2016/common/00001/201607_ejdh_common_Manual.shtml" -->

    <div class="container cartype-section">
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
        <%--     <script type="text/javascript" src="/jsnewV2/lefttree.v2.js"></script>--%>
        <script type="text/javascript">
            var CarCommonBSID = "<%= _serialEntity.Brand != null ? _serialEntity.Brand.MasterBrandId : 0 %>"; //大数据组统计用
            var CarCommonCBID = "<%= _serialEntity.Brand != null ? _serialEntity.Brand.Id : 0 %>";
            var CarCommonCSID = '<%= _SerialId.ToString() %>';
            BitautoLeftTreeV2.init({
                likeDefLink:"http://car.bitauto.com/{allspell}/",//serailId  allspell
                params: {
                    tagtype: "chexing",
                    pagetype: "serial",
                    objid: CarCommonCSID
                }
            });
        </script>
        <!--右侧内容-->
        <div class="col-xs-9">
            <div class="cartype-section-main main-3">
                <%=NavPathHtml %>
                <div class="main-inner-section main-box">
                    <div class="card-layout <%=sic.CsPriceRange=="未上市"?"unsale":""%>">
                        <div class="section-header header1">
                            <div class="box">
                                <h2><a href="/<%=_SerialSpell %>/" target="_blank"><%=_SerialName %></a></h2>
                            </div>
                            <div class="more">
                                  <a href="/<%=_SerialSpell %>/" target="_blank">频道</a>
                                <a href="/<%=_SerialSpell %>/peizhi/" target="_blank">参数</a>  <a href="/<%=_SerialSpell %>/tupian/"
                                    target="_blank">图片</a>  <a href="/<%=_SerialSpell %>/baojia/" target="_blank">报价</a>
                                 <a href="/<%=_SerialSpell %>/jiangjia/" target="_blank">降价</a> <a href="http://fenqi.taoche.com/www/<%=_SerialSpell%>/?from=2155" target="_blank">贷款</a>  <a href="/<%=_SerialSpell %>/koubei/"
                                    target="_blank">口碑</a>  
                                 <a href="<%=baaUrl %>" target="_blank">论坛</a>  <%--<a href="http://www.taoche.com/<%=_SerialSpell %>/" target="_blank">二手车</a>--%>
                            </div>
                        </div>
                        <div class="content clearfix">
                            <div class="img">
                                <a href="/<%=_SerialSpell %>/" target="_blank"><img src="<%=serialImageUrl %>" alt="" /></a>
                            </div>
                            <div class="desc">
                                <div class="top"><h5 id="cs-area-price">
                                    <%if (!(sic.CsPriceRange == "未上市" || sic.CsPriceRange == "停售" || string.IsNullOrEmpty(sic.CsPriceRange) || sic.CsPriceRange == "暂无报价"))
                                      { %>
							            参考成交价：<a href="http://price.bitauto.com/brand.aspx?newbrandId=<%=_SerialId %>"
                                                target="_blank"><em><%=  sic.CsPriceRange%></em></a><%}
                                      else
                                      { %>
                                        参考成交价：<em>
                                        <%=string.IsNullOrEmpty(sic.CsPriceRange) ? "暂无报价":sic.CsPriceRange %></em>
                                    <%} %>
                                    <%if (sic.CsPriceRange != "停售")
                                      { %>
                                    <a id="btnDirectSell" style="display: none;" href="#" target="_blank" class="ico-shangchengtehui"><em>直销</em></a><%} %>
                                    </h5>
                                   <%-- <a id="serial_guanzhu" href="javascript:;" title="点击关注"><span class="no-sc"></span></a>--%>
                                </div>
                                <div class="mid row">
                                    <div class="col-xs-4">
                                        <em><%= sic.CsPriceRange == "未上市"?"预售价":"厂商指导价"%></em>
                                        <h5><%=_serialEntity.ReferPrice%></h5>
                                    </div>
                                   <div class="col-xs-4">
                                        <em>二手车</em>
                                        <h5><a href="http://www.taoche.com/buycar/serial/<%=_SerialSpell %>/" target="_blank"><%=serialUCarPrice %></a></h5>
                                    </div>
                                    <div class="col-xs-4">
                                        <%if (isElectrombile)
                                          {%>
                                           <em>续航里程</em><h5><%=mileageRange%></h5>
                                        <%}
                                          else
                                          { %>
                                            <em>油耗</em>
                                            <h5><a href="/<%=_SerialSpell %>/youhao/" target="_blank"><%=string.IsNullOrEmpty(sic.CsSummaryFuelCost) ? "暂无" : sic.CsSummaryFuelCost%></a></h5>
                                        <%} %>
                                    </div>
                                </div>
                                <div class="foot">
                                    <div class="btn-group">
                                    <a id="car_zixun" href="http://dealer.bitauto.com/zuidijia/nb<%=_SerialId %>/?T=1&leads_source=p015001" target="_blank" class="btn" data-channelid="2.22.108">询底价</a>
                                    <a id="divDemandCsBut" class="btn" style="display: none;" href="#" data-channelid="">特卖</a>
                                    <%-- <%=shijiaOrHuimaiche%>--%>
                                    <a id="car_chedai" href="http://fenqi.taoche.com/www/<%=_SerialSpell %>/?from=yc9&leads_source=p015003" class="btn" target="_blank" data-channelid="2.22.110">贷款</a>
                                    <a id="btnZhihuan" href="http://maiche.taoche.com/zhihuan/?serial=<%=_SerialId %>&leads_source=p015004&ref=chexisxhuan" class="btn" target="_blank" data-channelid="2.22.111">置换</a>
                                    <a href="http://www.taoche.com/<%=_SerialSpell %>/?ref=chexizsmai&leads_source=p015005" class="btn" target="_blank" data-channelid="2.22.112">二手车</a>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%=carListTableHtml%>
                </div>
			<script type="text/javascript">
				//添加统计链接末尾参数
				var tongJiEndUrlParam='&ref=tree1&rfpa_tracker=1_9';
				
			</script>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/serialexternalcommon.min.js?v=2016122817"></script>
              <%--  <script type="text/javascript" src="/jsnewV2/serialexternalcommon.js"></script>--%>
            <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/getareaprice.min.js?v=201712110"></script>
              
			<script type="text/javascript">
                GetSerialTreeAreaPriceRange(<%=_SerialId %>);
				//直销
				getDirectSell(<%=_SerialId %>,'<%=_SerialSpell %>',bit_locationInfo.cityId,'yc-car-tree-1');
				//getDemandAndJiangJia(<%=_SerialId %>,'<%=_SerialSpell %>',bit_locationInfo.cityId);
                GetCarAreaPriceRange();
			    $(function () {
			        var timer=null;
				    $("#pop_nosale").hover(
		                function () { 
		                    $("#pop_nosalelist").show();
		                    clearTimeout(timer);
		                },
		                function () {
		                    timer=setTimeout(function () {
		                        $("#pop_nosalelist").hide();
		                    }, 500);
		                }
	                );
				});
				(function () {
					//如果是停售车系，关于商机按钮的处理
					var saleState = '<%=sic.CsPriceRange.ToString()%>';
                	if (saleState == "停售") {
                		var lianJieArray = [];
                		lianJieArray.push("<span class=\"button_orange btn-xj-w\">");
                		lianJieArray.push("<a target=\"_blank\" data-channelid=\"2.22.112\" href=\"http://yiche.taoche.com/<%=_SerialSpell %>/?ref=chexizsmai&leads_source=p015005\"" + ">买二手车</a>");
		                lianJieArray.push("</span>");
		                lianJieArray.push("<span class=\"button_gray btn-qt-w btn-bq-w\">");
		                lianJieArray.push("<a target=\"_blank\" data-channelid=\"\" href=\"http://www.taoche.com/pinggu/?ref=chexizsgu\">二手车估价</a>");
		                lianJieArray.push("</span>");
		                $('.sc-btn-box').html(lianJieArray.join(""));
				}
                })();
            </script>
			<script type="text/javascript" src="http://d2.yiche.com/js/senseNew.js"></script>
		    </div>
             <!--#include file="~/htmlV2/rightbar.shtml"-->
             <!--#include file="~/htmlV2/treefooter2016.shtml"-->
        </div>
	</div>

	<script type="text/javascript">
		var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
		document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
	</script>

	<!--#include file="~/htmlV2/CommonBodyBottom.shtml"--><!-- 艾瑞 -->
</body>
</html>
<!--本站统计代码-->
<%--<script type="text/javascript" src="http://bglog.bitauto.com/bglogpostlog.js"></script>--%>
<%--应顾晓要求，更新日志记录文件的引用如下--%>
<!--#include file="~/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
<script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
<script type="text/javascript">
	OldPVStatistic.ID1 = "<%=_SerialId.ToString() %>";
	OldPVStatistic.ID2 = "0";
	OldPVStatistic.Type = 4;
	mainOldPVStatisticFunction();
</script>
<!--本站统计结束-->
