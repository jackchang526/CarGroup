<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsCompare.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageSerialV2.CsCompare" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <title>【<%=cbe.SeoName%>】<%=cbe.Brand.MasterBrand.Name %><%=cbe.SeoName%>参数配置表_<%=cbe.SeoName%>发动机配置-易车网</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta name="keywords" content="<%=cbe.SeoName%>参数,<%=cbe.SeoName%>配置,<%=cbe.Brand.MasterBrand.Name+cbe.SeoName%>,<%=cbe.SeoName%>参数配置表,<%=cbe.SeoName%>发动机配置,易车网,car.bitauto.com" />
    <meta name="description" content="<%=cbe.Brand.MasterBrand.Name+cbe.SeoName%>配置，易车网提供<%=cbe.Brand.MasterBrand.Name+cbe.SeoName%>配置参数表,包括,<%=cbe.SeoName%>发动机,<%=cbe.SeoName%>变速箱,<%=cbe.SeoName%>车轮,<%=cbe.SeoName%>灯光等配置等参数。" />
    <meta name="mobile-agent" content="format=html5; url=http://car.m.yiche.com/<%=cbe.AllSpell %>/peizhi/" />
    <meta name="mobile-agent" content="format=xhtml; url=http://m.bitauto.com/g/carserial.aspx?serialid=<%=csID.ToString() %>&key=carlist" />
    <link rel="canonical" href="http://car.bitauto.com/<%=cbe.AllSpell %>/peizhi/" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->

    <!--#include file="~/ushtml/0000/yiche_2016_cube_peizhiye_part-1261.shtml"-->
    <script type="text/javascript">
        if (typeof (bitLoadScript) == "undefined")
            bitLoadScript = function (url, callback, charset) {
                var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
                if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
                else { s.onload = function () { if (callback) callback(); }; }
                s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
            };
    </script>
</head>
<body data-offset="-130" data-target=".left-nav" data-spy="scroll">
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlv2/header2016.shtml"-->
    <!--a_d start-->
    <div class="bt_ad" style="margin: 10px auto 10px; width: 1200px;">
        <ins id="div_7e48ab6a-f563-413a-8427-5578aa3416f9" type="ad_play" adplay_ip="" adplay_areaname=""
            adplay_cityname="" adplay_brandid="<%=csID.ToString() %>" adplay_brandname=""
            adplay_brandtype="" adplay_blockcode="7e48ab6a-f563-413a-8427-5578aa3416f9"></ins>
    </div>
    <!--a_d end-->
    <%= CsHeadHTML %>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>

    <!--左上小浮动层开始-->
    <div id="smallfixed" class="floatLayer floatLayer_peizhi w170" style="display: none;">
        <table cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <th class="pd0">
                        <div class="tableHead_left">
                            <div class="check-box-item">
                                <input type="checkbox" id="left_chkAdvantage" name="chkAdvantage" onclick="advantageForCompare();" />
                                <label for="left_chkAdvantage">
                                    标识优势项 <em></em>
                                </label>
                            </div>
                            <div class="check-box-item">
                                <input type="checkbox" id="left_checkboxForDiff" name="checkboxForDiff" onclick="showDiffForCompare();" />
                                <label for="left_checkboxForDiff">
                                    高亮不同项</label>
                            </div>
                            <div class="check-box-item">
                                <input type="checkbox" name="checkboxForDelTheSame" onclick="delTheSameForCompare();"
                                    id="left_checkDiffer" />
                                <label for="left_checkDiffer">
                                    隐藏相同项</label>
                            </div>
                            <div class="dashline">
                            </div>
                            <p>
                                ●标配 ○选配&nbsp;&nbsp;- 无
                            </p>
                        </div>
                    </th>
                </tr>
            </tbody>
        </table>
    </div>
    <!--左上小浮动层结束-->
    <div class="floatLayer floatLayer_peizhi line_box_compare line_box_compare_peizhi y2015" id="topfixed" style="display: none;"></div>
    <!--参数纠错-->
    <div class="modal modal-md" id="popup-box" style="display: none; position: fixed; left: 50%; top: 50%;">
        <div class="modal-header">
            <h4>参数纠错</h4>
            <span class="close"></span>
        </div>
        <div class="modal-content" id="popup-box-content">
            <div class="form-group">
                <textarea id="correctError" class="input textarea input-block error"></textarea>
                <span class="help help-block error" id="alert-center" style="display: none;">请输入提交内容</span>
            </div>
            <div class="form-group foot-btn">
                <a class="btn btn-primary btn-md" href="javascript:;" name="btnCorrectError">提交</a>
                <a class="btn btn-default btn-md" href="javascript:;" id="btnErrorCancel">取消</a>
            </div>
            <div class="note-box note-ok type-2" style="display: none;" id="popup-box-success">
                <div class="ico"></div>
                <div class="info">
                    <h3>提交成功</h3>
                    <p class="tip">您提交的纠错信息我们已经收集到，感谢您的纠错。</p>
                </div>
                <div class="action">
                    <!--按钮间不留空格-->
                    <a href="javascript:;" id="btn-success-close" class="btn btn-default">关闭</a>
                </div>
            </div>
        </div>

    </div>
    <!--/参数纠错-->
    <!--content-->
    <div class="container config-section summary" id="box">
        <div class="config-section-main">
            <div class="td-tips">
                <div class="ts-box">
                    以下数据仅供参考，不构成任何买卖协议，实际情况以店内销售车辆为准。如果发现信息有误，<a target="_blank" href="http://www.bitauto.com/feedback/">欢迎您及时指正！</a>
                </div>
            </div>

            <div class="config-box">
                <div class="peizhi-filter">
                    <span class="peizhi" id="spanFilterForYear"></span>
                    <span class="peizhi" id="spanFilterForTT"></span>
                    <span class="peizhi" id="spanFilterForEE"></span>
                </div>
                <div id="main_box" class="line_box_compare line_box_compare_peizhi y2015">
                    <div id="leftfixed" style="display: none;"></div>
                    <div id="CarCompareContent"></div>
                    <em class="btn-show-left-nav" style="display: none;" id="show-left-nav"></em>
                    <!-- 左侧浮动层 start -->
                    <div class="left-nav" id="left-nav" style="position: absolute; top: 145px; left: -70px;display:none;">
                       <%-- <a href="https://survey01.sojump.com/jq/11683998.aspx" target="_blank" style="display:block;height:26px;line-height:26px;position:absolute;top:-36px;width:65px;font-size:12px;background:#ff4f53;text-align: center;color:#fff;">问卷调查</a>--%>
                        <ul>
                        </ul>
                        <a href="javascript:;" class="close-left-nav" id="close-left-nav" style="display: none;">关闭浮层</a>
                    </div>
                    <!-- 左侧浮动层 end -->
                   <%-- <div class="td-tips">
                        <div class="ts-box">
                            以上参数配置信息仅供参考，实际请以店内销售车辆为准。如果发现信息有误，<a target="_blank" href="http://www.bitauto.com/feedback/">欢迎您及时指正！</a>
                        </div>
                    </div>--%>
                </div>
            </div>

        </div>
    </div>
    <!--/content-->
    <script type="text/javascript">
        var CarCommonBSID = "<%= cbe.Brand == null ? 0 : cbe.Brand.MasterBrandId %>"; //大数据组统计用
        var CarCommonCBID = "<%= cbe.Brand == null ? 0 : cbe.Brand.Id %>";
        var CarCommonCSID = '<%= csID.ToString() %>';
    </script>
    <% if (carIDAndName != "")
       { %>
    <script type="text/javascript">
        <%= jsContent %>
        <%= packageJsContent %>
    </script>
    <% } %>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/draggable.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/CarChannelBaikeJson.js?v=20150831"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/commons.min.js"></script>
    <%--<script type="text/javascript" src="/jsnewv2/carcompareforminiV3.js?v=20150805"></script>
        <script type="text/javascript" src="/jsnew/carSelectSimpleV3.js"></script>--%>
       <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/getareaprice.min.js?v=201712110"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewv2/JsForParameterv2.min.js?v=201712110"></script>
<%--    <script type="text/javascript" src="/jsnewv2/getareaprice.js?v=2017120401"></script>
    <script type="text/javascript" src="/jsnewv2/JsForParameterv2.js?v=2017120401"></script>--%>
    <script type="text/javascript">
			<%= JsTagForYear %>
        ComparePageObject.CarIDAndNames = "<%= carIDAndName %>";
        initPageForCompare();

        //解决chrome左侧菜单滑动影响
        (function () {
            if (window.navigator.userAgent.indexOf("Chrome") !== -1) {
                var count = 0;
                var timer = setInterval(function () {
                    count++;
                    if (count > 60) clearInterval(timer);
                    var obj = $("body>div>object[data*='irs01.net']");
                    if (obj.length > 0) {
                        clearInterval(timer);
                        obj.parent("div").css({ right: "" });
                    }
                }, 500);
            }
        })();
        $(function () {
            $("#backtop").attr("href", "javascript:;").bind("click", function () {
                $("html,body").animate({ scrollTop: 0 }, 300, function () {
                    //modified by 2014.07.17 ie6 7 8 修改 滚动监听 回不到基本信息
                    if (!-[1, ]) {
                        $("#left-nav li:first").addClass("current").siblings().removeClass("current");
                    }
                });
            });
        });
    </script>
    <!--#include file="~/htmlv2/rightbar.shtml"-->
    <!--#include file="~/htmlv2/footer2016.shtml"-->
    <!--本站统计代码-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript">
        OldPVStatistic.ID1 = "<%=csID.ToString() %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 0;
        mainOldPVStatisticFunction();
    </script>
    <!--本站统计结束-->
    <!-- baa 浏览过的车型-->
    <script type="text/javascript" src="http://js.inc.baa.bitautotech.com/201001/usercars.js"></script>
    <script type="text/javascript">
        try {
            Bitauto.UserCars.addViewedCars('<%=csID.ToString() %>');
	    }
	    catch (err)
	    { }
    </script>
    <%if (csID == 2370 || csID == 2608 || csID == 3398 || csID == 3023 || csID == 2388)
      { %>
    <!--百度热力图-->
    <script type="text/javascript">
        var _bdhmProtocol = (("https:" == document.location.protocol) ? " https://" : " http://");
        document.write(unescape("%3Cscript src='" + _bdhmProtocol + "hm.baidu.com/h.js%3F7b86db06beda666182190f07e1af98e3' type='text/javascript'%3E%3C/script%3E"));
    </script>
    <%} %>
    <!--#include file="~/htmlv2/CommonBodyBottom.shtml"-->
    <script type="text/javascript">
        var zamCityId = (typeof bit_locationInfo != "undefined") ? bit_locationInfo.cityId : "201",
			modelStr = '<%=csID%>-' + (zamCityId.length >= 4 ? zamCityId.substring(0, 2) : zamCityId.substring(0, 1)) + '-' + zamCityId + '';
	    var zamplus_tag_params = {
	        modelId: modelStr,
	        carId: 0
	    };
    </script>
    <script type="text/javascript">
        var _zampq = [];
        _zampq.push(["_setAccount", "12"]);
        (function () {
            var zp = document.createElement("script"); zp.type = "text/javascript"; zp.async = true;
            zp.src = ("https:" == document.location.protocol ? "https://" : "http://") + "cdn.zampda.net/s.js";
            var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(zp, s);
        })();
    </script>
</body>
</html>
<!-- 用户行为统计 -->
<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJs.js"></script>
<!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
