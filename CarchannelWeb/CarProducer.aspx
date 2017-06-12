<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarProducer.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.CarProducer" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【<%=strCpShortName%>】<%=strCpShortName%>官网网址_<%=strCPName%>简介_新闻-易车网</title>
    <meta name="Keywords" content="<%=strCpShortName%>, <%=strCpShortName%>网址,<%=strCPName%>" />
    <meta name="Description" content="<%=strCpShortName%>:易车网为您提供<%=strCPName%>最新简介,<%=strCpShortName%>相关新闻,旗下品牌列表以及<%=strCpShortName%>官网地址等,是您了解<%=strCPName%>的首选网站" />
    <!--#include file="~/ushtml/0000/car_common_v2_B-306.shtml"-->
</head>
<body>
    <!--#include file="~/html/header2012.shtml"-->
    <div class="bt_page">
        <div class="bt_smenuNew">
            <div class="bt_navigatev1New">
                <div>
                    <span>您当前的位置：</span> <a href="http://www.bitauto.com" target="_blank">易车网</a> &gt; <a href="http://news.bitauto.com">新闻</a> &gt; <a href="http://news.bitauto.com/pinpai/">品牌</a> &gt; <strong>
                        <%=strCPName%></strong>
                </div>
            </div>
            <div class="bt_searchNew">
                <!--#include file="~/html/bt_searchNew.shtml"-->
            </div>
        </div>
    </div>
    <!--page start-->
    <div class="bt_page">
        <div class="auto_info">
            <%=strCp_InfoTop %>
        </div>
        <div class="col-all">
            <div class="line_box">
                <h3><span>
                    <%=strCpShortName%>旗下品牌</span></h3>
                <div class="pp_auto">
                    <ul class="list_pic">
                        <%=strCBPhotoListHTML %>
                    </ul>
                    <div class="clear"></div>
                </div>
            </div>
        </div>
        <div class="col-all">
            <div class="line_box">
                <h3><span>
                    <%=strCpShortName%>企业介绍</span></h3>
                <p class="qy_info">
                    <%=strCp_Info%>
                </p>
            </div>
        </div>
        <!--新闻列表-->
        <%=strNewsHtml %>
        <!--Seo块-->
        <!--#    include file="~/include/special/seo/00001/bitauto_news_hotpinpai_Manual.shtml"-->
        <!--直接嵌入页面如下-->
        <div class="col-all">
            <div class="line_box link">
                <div class="h3_tab">
                    <!--友情链接区_切换标签-->
                    <ul id="data_tab5">
                        <li class="current"><a target="_blank" href="#">热门品牌</a></li>
                        <li><a target="_blank" href="#">国内品牌</a></li>
                        <li class="last"><a target="_blank" href="#">国外品牌</a></li>
                    </ul>
                </div>
                <h3>&nbsp;</h3>
                <div class="clear">&nbsp;</div>
                <div class="co">
                    <div style="display: block" id="data_box5_0">
                        <ul>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/beijingbenchi/">北京奔驰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/beijingxiandai/">北京现代</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/biyadi/">比亚迪</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongfengrichan/">东风日产</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongfengyuedaqiya/">东风悦达起亚</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/guangqibentian/">广汽本田</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/huachenbaoma/">华晨宝马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jili/">吉利</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/qirui/">奇瑞</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shangqirongwei/">上汽荣威</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shanghaidazhong/">上海大众</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shanghaitongyongxuefolan/">上海通用雪佛兰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shanghaitongyongbieke/">上海通用别克</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqidazhong/">一汽大众</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongfengbiaozhi/">东风标致</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/aodi/">奥迪</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/baoma/">进口宝马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/baoshijie/">保时捷</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/lanbojini/">兰博基尼</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/mazida/">马自达</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/xuefolan/">雪佛兰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/bieke/">别克</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/woerwo/">沃尔沃</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dazhong/">大众</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/binli/">进口宾利</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/wuling/">五菱</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqibenteng/">奔腾</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/bentian/">本田</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongfengxuetielong/">东风雪铁龙</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/guangqi/">广汽</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongfengbentian/">东风本田</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changhelingmu/">昌河铃木</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changanfute/">长安福特</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqiaodi/">一汽奥迪</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqimazida/">一汽马自达</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/feiyate/">菲亚特</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/fengtian/">丰田</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/lingmu/">进口铃木</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/gmc/">GMC</a></li>
                        </ul>
                    </div>
                    <div style="display: none" id="data_box5_1">
                        <ul>
                            <li><a target="_blank" href="http://www.bitauto.com/">汽车</a></li>
                            <li><a target="_blank" href="http://price.bitauto.com/">汽车报价</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/beibenkelaisile/">北京克莱斯勒</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/beiqi/">北汽</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changanmazida/">长安马自达</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changanwoerwo/">长安沃尔沃</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changanlingmu/">长安铃木</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changanweiche/">长安威驰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changanjiaoche/">长安轿车</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changcheng/">长城</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changhe/">昌河</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/fengxing/">风行</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/fengshen/">风神</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongfengyuan/">东风渝安</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongnan/">东南</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongnansanling/">东南三菱</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongnandaoqi/">东南道奇</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dongnankelaisile/">东南克莱斯勒</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/fujianbenchi/">福建奔驰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/futianqiche/">福建汽车</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changfengliebao/">长丰猎豹</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/changfengsanling/">长丰三菱</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/guangqifengtian/">广汽丰田</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/huachenjinbei/">华晨金杯</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/huachenzhonghua/">华晨中华</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/hafei/">哈飞</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/haima/">哈马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/huatai/">华泰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jiao/">吉奥</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jianghuai/">江淮</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/quanqiuying/">全球鹰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shanghaiyinglun/">上海英伦</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/dihao/">帝豪</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jiangling/">江陵</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jiulongshangwuche/">九龙</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jiangnan/">江南</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/lufeng/">陆风</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/lifan/">力帆</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/qingnianlianhua/">青年莲花</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/ruiqi/">瑞麒</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/kairui/">开瑞</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/weilin/">威麟</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/sichuanyiqifengtian/">四川一汽丰田</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shanghaidazhongsikeda/">上海大众斯柯达</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/huapu/">华普</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shuanghuan/">双环</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shangqimingjue/">MG</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shanghaitongyongkaidilake/">上海通用凯迪拉克</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shangqitongyongwulingxuefolan/">上汽通用雪佛兰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/tianjinyiqi/">天津一汽</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/tianjinyiqifengtian/">天津一汽丰田 </a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/tianma/">天马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yema/">野马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqifengyue/">一汽丰越</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqihuali/">一汽华利</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/hongqi/">红旗</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqijiqi/">一汽吉汽</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yiqijilindafa/">一汽吉林大发</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yongyuanqicheufo/">永源汽车UFO </a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/zhongxing/">中兴</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/zhongtai/">众泰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/fushida/">郑州海马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/zhengzhourichan/">郑州日产</a></li>
                        </ul>
                    </div>
                    <div style="display: none" id="data_box5_2">
                        <ul>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/aerfaluomiou/">进口阿尔法&middot;罗米欧 </a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/aerte/">阿尔特</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/asidunmading/">阿斯顿.马丁</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/bujiadi/">布嘉迪</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/biaozhi/">进口标致 </a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/xuetielong/">进口雪铁龙 </a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/falali/">法拉利</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/sibalu/">斯巴鲁</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/leikesasi/">雷克萨斯</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/fute/">福特</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/linken/">林肯</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/lanqiya/">蓝旗亚</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/haiku/">海酷</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/hanma/">悍马</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jiebao/">捷豹</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/daoqi/">道奇</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/jipu/">吉普</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/kelaisile/">克莱斯勒</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/kenisaige/">柯尼赛格</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/lianhua/">进口莲花</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/luhu/">进口路虎</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/leinuo/">进口雷诺</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/laosilaisi/">进口劳斯莱斯</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/maibahe/">进口迈巴赫</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/MINImini/">进口迷你(MINI)</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/benchi/">进口奔驰</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/mashaladi/">玛莎拉蒂汽</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/oubao/">欧宝</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/qiya/">进口起亚</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/guanggang/">进口光冈</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/richan/">进口日产</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/yingfeinidi/">进口英菲尼迪</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/sabo/">萨博</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shijue/">世爵</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/sikeda/">进口斯柯达</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/sanling/">进口三菱</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/shuanglong/">进口双龙</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/simateSmart/">斯玛特</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/kaidilake/">进口凯迪拉克</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/pangdike/">进口旁蒂克</a></li>
                            <li><a target="_blank" href="http://news.bitauto.com/pinpai/xiandai/">现代</a></li>
                        </ul>
                    </div>
                </div>
                <div class="clear">&nbsp;</div>
            </div>
        </div>
        <div>&nbsp;</div>
    </div>
    <!-- 弹出 -->
    <div id="popWin" style="display: none">
        <div class="line_box">
            <h3><span><a>
                <%=strCPName %></a></span></h3>
            <div class="more" id="closebox" style="width: 14px; height: 16px; overflow: hidden; padding: 0; margin: 0; background: url(/car/images/bg-icons.gif) no-repeat scroll 0 -268px;"></div>
            <!-- A内容 -->
            <div id="aa" style="display: none" class="carintropop">
                <%= strCPIntroducPop%>
            </div>
            <style type="text/css">
                .carintropop {
                    height: 180px;
                    overflow: auto;
                    padding: 10px;
                }

                    .carintropop p {
                        padding: 0 0 15px 0;
                    }
            </style>
        </div>
    </div>
    <!--page end-->
    <!--本站统计代码-->
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsnew/window.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
    <script type="text/javascript" language="javascript" src="http://image.bitautoimg.com/carchannel/jsStat/StatisticJsOldPV.js"></script>
    <script type="text/javascript" language="javascript">
        OldPVStatistic.ID1 = "<%=CPID.ToString() %>";
        OldPVStatistic.ID2 = "0";
        OldPVStatistic.Type = 1;
        mainOldPVStatisticFunction();
    </script>
    <!-- 调用尾 -->
    <!--#include file="~/html/footer2012.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
</body>
</html>

