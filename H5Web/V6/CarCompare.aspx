<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarCompare.aspx.cs" Inherits="H5Web.V6.CarCompare" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <title>车型对比</title>
    <!--#include file="/ushtml/0000/4th_2015-2_chexinduibi_style-1120.shtml" -->
</head>
<body>
    <div id="container">
        <%= TitleHtml %>
        <div class="flex-block"></div>
        <div class="block">
            <h3>谁更值</h3>
            <%= PriceHtml %>
            <%= KoubeiHtml %>
            <%= BaoZhiLvHtml %>
        </div>

        <!--销量对比 start-->
        <div class="block" data-type="saleCount" style="display: none">
        </div>
        <!--9月销量对比 end-->
        <!--口碑差异 start-->
        <div class="block" data-type="koubei" style="display: none">
        </div>
        <!--口碑差异 end-->
        <!--口碑差异 end-->
        <%= BottomTitleHtml %>
        <!--二维码start-->
        <div class="erweima">
            <!--#include file="~/include/pd/2014/disiji/00001/201506_gg_gzgzh_gb2312_Manual.shtml"-->
        </div>
        <!--二维码end-->
        <div id="master_container" style="z-index: 888888; display: none" class="brandlayer mthead">
            <!--#include file="~/inc/compareCarTemplate.html"-->
        </div>
    </div>
    <!--详细层 start-->
    <div class="mark mask3" style="display: none;"></div>
    <div class="mark-details" data-back="mask3" style="display: none;">
        <h3>购车费用</h3>
        <div class="mark-list">
            <ul>
                <li>
                    <label>厂商指导价：</label><span id="spanReferPrice"></span>
                </li>
                <li>
                    <label>购置税：</label><span id="spanGouZhiShui"></span>
                </li>
                <li>
                    <label>车船税：</label><span id="spanCheChuanShui"></span>
                </li>
                <li>
                    <label>保险：</label><span id="spanBaoXian"></span>
                </li>
                <li>
                    <label>上牌费（平均）：</label><span id="spanChePai"></span>
                </li>
            </ul>
        </div>
    </div>
    <!--详细层 end-->
    <script type="text/javascript">
        var CarDetailJson = <%= PriceDetailJson %>;
        <%= AllCarJsArray %>
    </script>
    <script src="http://image.bitautoimg.com/uimg/wap/js/jquery-1.8.0.min.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="/Scripts/carcompare/countUp.min.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/model.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/radialIndicator.min.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/radiaindicator.extend.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/carcompare.v1.js"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/carcompare/countUp.min.v1.js,carchannel/h5/js/carcompare/model.v1.js,carchannel/h5/js/carcompare/radialIndicator.min.v1.js,carchannel/h5/js/carcompare/radiaindicator.extend.v1.js,carchannel/h5/js/carcompare/carcompare.v1.js?v=201601111413"></script>
    <script type="text/javascript">
        $(function () {
            var $flex = $('.flex'), $flexblock = $('.flex-block');
            $flexblock.height($flex.height());
            var otop = 0;
            //浮动头
            $(window).scrollListener({
                scrollTo: function () {
                    var site = 'down';
                    if (this.scrollTop > otop) {
                        site = 'up';
                    }
                    if (this.scrollTop >= ($flex.height() / 2)) {
                        $flexblock.show();
                        $flex[0].className = 'flex-header-duibi flex min';
                    } else if (this.scrollTop <= $flex.height()) {
                        $flexblock.hide();
                        $flex[0].className = 'flex-header-duibi flex';
                    }
                    otop = this.scrollTop;
                }
            })

            //初始化圆环
            $('.canvas-box').circle({ animate: true });

            //详情层
            $('[data-action=mark-details]').model({
                clickEnd: function () {
                    var $click = this, $model = $click.$model, $mark = $click.$mark;
                    var dataindex = $click.attr("data-index");
                    $("#spanReferPrice").html(CarDetailJson[dataindex]["ReferPrice"] + "万元");
                    $("#spanGouZhiShui").html(CarDetailJson[dataindex]["GouZhiShui"] + "元");
                    $("#spanCheChuanShui").html(CarDetailJson[dataindex]["CheChuanShui"] + "元");
                    $("#spanBaoXian").html(CarDetailJson[dataindex]["BaoXian"] + "元");
                    $("#spanChePai").html(CarDetailJson[dataindex]["ChePai"] + "元");

                    $model.touches({
                        touchmove: function (ev) { ev.preventDefault(); }
                    })
                    $model.click(function (ev) {
                        $mark.trigger('close');
                    })
                }
            });

            //解决影响canvas加载问题
            $('[data-src]').each(function (index, curr) {
                $(curr).attr('src', $(curr).attr('data-src'));
            })

            //动态加载横条
            $('[data-bar]').lazybar();

            CarCompare.Init();
        })
    </script>
    <%--<script type="text/javascript" src="/Scripts/carcompare/iscroll.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/underscore.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/note.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/rightswipe.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/brand.v1.js"></script>
    <script type="text/javascript" src="/Scripts/carcompare/changecar.v1.js"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/mergejs,s=carchannel/h5/js/carcompare/iscroll.v1.js,carchannel/h5/js/carcompare/underscore.v1.js,carchannel/h5/js/carcompare/note.v1.js,carchannel/h5/js/carcompare/rightswipe.v1.js,carchannel/h5/js/carcompare/brand.v1.js,carchannel/h5/js/carcompare/changecar.v1.js?v=201601111413"></script>
    <script type="text/javascript">
        $(function () {
            duibiCarDataIds.push(<%= CarId1 %>);
            duibiCarDataIds.push(<%= CarId2 %>);
            changecar.init({callback: changecar.ChangeCarCallback});
        });
    </script>
    <script type="text/javascript" src="http://g.yccdn.com"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/anchor.js?v=20150630"></script>
    <script type="text/javascript">
        var summary = { serialId: 0,IsNeedShare:true };
        var XCWebLogCollector = { area: '201' };
        if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
            XCWebLogCollector.area = bit_locationInfo.cityId;
        }
        if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
            XCWebLogCollector.serial = summary.serialId;
        }
    </script>
    <script type="text/javascript">
        var __zp_tag_params = {
            modelId: summary.serialId,
            carId: 0
        };
    </script>
    <script type="text/javascript">
        var forweixinObj = {
            debug: false,
            appId: 'wx0c56521d4263f190',
            jsApiList: ['checkJsApi', 'onMenuShareTimeline', 'onMenuShareAppMessage', 'onMenuShareQQ', 'onMenuShareWeibo']
        }
        var pageShareContent = {
            title: '【汽车大全_热门车型大全】-易车网',
            keywords: '掌上车型手册，360°解读你所关注的车型。',
            desc: '掌上车型手册，360°解读你所关注的车型。',
            link: 'http://car.h5.yiche.com/chexingduibi/?carids=<%= CarId1 %>,<%= CarId2 %>&',
            imgUrl: 'http://image.bitautoimg.com/carchannel/pic/yiche100.jpg'
        }
    </script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/H5Stat.js?20150716"></script>
</body>
</html>
