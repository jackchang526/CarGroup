<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SerialForLevel.aspx.cs"
    Inherits="BitAuto.CarChannel.CarchannelWeb.PageList.SerialForLevel" %>

<% Response.ContentType = "text/html"; %>
<%--<%@ OutputCache Duration="300" VaryByParam="none" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>【按级别找车型_汽车车型大全】-易车网</title>
    <meta name="Keywords" content="车型数据库, 汽车最新报价,车型导购,汽车评测,汽车新闻,汽车图片,汽车问答,汽车点评，汽车经销商，汽车论坛" />
    <meta name="Description" content="易车网(BitAuto.com) 汽车车型为您提供各种汽车车型所有信息。包括汽车报价、最新报价、汽车图片、汽车参数、汽车配置、汽车资讯、汽车点评、汽车问答、汽车论坛等等……" />
     <%
        // 2345 合作 2017-03-23
        string wt = Request.QueryString["WT.mc_id"];
        if (wt == "2345nyif")
        { %>
    <!--#include file="/ushtml/0000/yiche_2014_cube_qichedaquan-1329.shtml"-->
    <%}
        else
        { %>
    <!--#include file="~/ushtml/0000/yiche_2014_cube_chexingdaquan-770.shtml"-->
    <%} %>
    <%--<link rel="stylesheet" type="text/css" href="http://image.bitautoimg.com/uimg/carazbanner/css/carazbanner.css" />--%>
    <script language="javascript" type="text/javascript" charset="gb2312" src="http://image.bitautoimg.com/uimg/carazbanner/js/carazbanner.js"></script>
    <script type="text/javascript">
    <!--
        var tagIframe = null;
        var currentTagId = 4; 	//当前页的标签ID
    -->
    </script>
</head>
<body>
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0;
		line-height: 0; font-size: 0"></span>
    <div class="bt_pageBox">
 <!--头 start-->
        <div class="bit_top990">
            <div class="bit990">
                <ul class="bitweb">
                    <li class="bityiche"><a href="http://www.bitauto.com/" target="_blank">易车网</a></li>
                    <li><a rel="nofollow" href="http://www.huimaiche.com" target="_blank">惠买车</a></li>
                    <li><a rel="nofollow" href="http://www.taoche.com" target="_blank">淘车网</a></li>
                    <li><a rel="nofollow" href="http://app.yiche.com/" target="_blank">移动应用</a></li>
                </ul>
                <!--网站导航 start-->
                <div class="bt_login_box990">
                    <!--start-->
                    <ul>
                        <!--网站地图-->
                        <li onmouseout="document.getElementById('login_mapsite').style.display='none';this.className='bit_link';"
                            onmouseover="document.getElementById('login_mapsite').style.display='';this.className='bit_link bit_hover';"
                            id="login_navhover" class="last bit_link"><a rel="nofollow" href="http://www.bitauto.com/map/map.shtml"
                                target="_blank">网站地图<em></em></a>
                            <dl style="display: none;" id="login_mapsite" class="login_mapsite">
                                <dt class="noborder">全部频道：</dt>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/" target="_blank">车型</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://photo.bitauto.com/" target="_blank">图片</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://v.bitauto.com/" target="_blank">视频</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://www.bitauto.com/huati/" target="_blank">话题</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://dealer.bitauto.com/" target="_blank">经销商</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/tree_pingce/" target="_blank">评测</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/tree_daogou/" target="_blank">导购</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://price.bitauto.com/" target="_blank">报价</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://jiangjia.bitauto.com/" target="_blank">降价</a></dd>
                                <dd>
                                    <a href="http://www.taoche.com/" target="_blank">二手车</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://ask.bitauto.com/" target="_blank">问答</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://koubei.bitauto.com/tree/" target="_blank">口碑</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/tree_baoyang/" target="_blank">养护</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://baa.bitauto.com/" target="_blank">论坛</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://zijia.bitauto.com/" target="_blank">自驾游</a></dd>
                                <dt>汽车论坛：</dt>
                                <dd>
                                    <a rel="nofollow" href="http://baa.bitauto.com/foruminterrelated/brandforumlist.html"
                                        target="_blank">车型论坛</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://baa.bitauto.com/foruminterrelated/grouplist_3.html"
                                        target="_blank">地区论坛</a></dd>
                                <dd class="noborder">
                                    <a rel="nofollow" href="http://baa.bitauto.com/foruminterrelated/grouplist_2.html"
                                        target="_blank">主题论坛</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://baa.bitauto.com/special/huodong/" target="_blank">网友活动</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://baa.bitauto.com/bao/" target="_blank">每周精选</a></dd>
                                <dt>实用工具：</dt>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/chexingduibi/" target="_blank">车型对比</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/tupianduibi/" target="_blank">图片对比</a></dd>
                                <dd class="noborder">
                                    <a rel="nofollow" href="http://car.bitauto.com/gouchejisuanqi/" target="_blank">购车计算器</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://www.bitauto.com/zhuanti/daogou/gsqgl/" target="_blank">
                                        购车流程</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://car.bitauto.com/qichebaoxianjisuan/" target="_blank">
                                        车险计算</a></dd>
                                <dd class="noborder">
                                    <a rel="nofollow" href="http://car.bitauto.com/qichedaikuanjisuanqi/" target="_blank">
                                        贷款计算器</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://i.bitauto.com/apps/!jz/" target="_blank">用车记账</a></dd>
                                <dd>
                                    <a rel="nofollow" href="http://www.bitauto.com/shouce/" target="_blank">驾驶手册</a></dd>
                                <dd class="noborder">
                                    <a rel="nofollow" href="http://m.yiche.com/" target="_blank">手机易车网</a></dd>
                                <dt class="dtmore"><a rel="nofollow" href="http://www.bitauto.com/map/map.shtml"
                                    target="_blank">更多&gt;&gt;</a></dt>
                            </dl>
                        </li>
                    </ul>
                    <!--end-->
                    <!--baa start-->
                    <ul id="divLoginDivID">
                        <li class="noborderl">
                            <input type="text" onkeypress="Bitauto.Login.onEnter(event,this,document.getElementById('txtUserPwdHeader'),document.getElementById('btnUserLoginHeader'));"
                                id="txtUserNameTopHeader" name="txtUserNameTopHeader" class="bit_loginInput bit_loginInputaccount"
                                value="帐号">
                            <input type="password" onkeypress="Bitauto.Login.onEnter(event,document.getElementById('txtUserNameTopHeader'),this,document.getElementById('btnUserLoginHeader'));"
                                id="txtUserPwdHeader" name="txtUserPwdHeader" class="bit_loginInput" style="display: none">
                            <input type="text" id="txtUserPwdHeader_init" name="txtUserPwdHeader" class="bit_loginInput"
                                value="密码">
                            <input type="button" onclick="Bitauto.Login.login(document.getElementById('txtUserNameTopHeader'),document.getElementById('txtUserPwdHeader'),document.getElementById('txtUserPwdHeader_init'))"
                                id="btnUserLoginHeader" class="bit_logintop" value="登录">
                            <a href="http://i.qichetong.com/authenservice/register/default.aspx" target="_blank">
                                注册</a></li>
                        <li onmouseout="document.getElementById('login_account').style.display='none';this.className='bit_link';"
                            onmouseover="document.getElementById('login_account').style.display='';this.className='bit_link bit_hover';"
                            class="bit_link"><a href="javascript:void(0)">第3方登录<em></em></a>
                            <dl style="display: none;" id="login_account" class="login_account">
                                <dd>
                                    <a href="javascript:Bitauto.Login.OtherWebSiteLogin('Sina');" class="sina">微博</a></dd>
                                <dd class="noborder">
                                    <a href="javascript:Bitauto.Login.OtherWebSiteLogin('Tencent');" class="qq">QQ帐号</a></dd>
                                <dd>
                                    <a href="javascript:Bitauto.Login.OtherWebSiteLogin('Renren');" class="renren">人人网</a></dd>
                                <dd class="noborder">
                                    <a href="javascript:Bitauto.Login.OtherWebSiteLogin('Kaixin');" class="kaixin">开心网</a></dd>
                                <dd>
                                    <a href="javascript:Bitauto.Login.OtherWebSiteLogin('Baidu');" class="baidu">百度</a></dd>
                                <dd class="noborder">
                                    <a href="javascript:Bitauto.Login.OtherWebSiteLogin('Taobao');" class="taobao">淘宝网</a></dd>
                            </dl>
                        </li>
                    </ul>
                    <!--baa end-->
                </div>
                <!--网站导航 end-->
            </div>
        </div>
    <!--#include file="~/include/pd/2014/common/00001/201402_cxzsy_xdh_utf8_Manual.shtml"-->
        <!--头 end-->        
        
        <!--公共_LOGO热搜通栏-->
        <div class="header_style">
            <div class="bitauto_logo">
            </div>
            <!--页头导航_yiche_LOGO开始-->
            <div class="yiche_logo">
                <a href="http://www.bitauto.com">易车网</a></div>
            <!--页头导航_yiche_LOGO结束-->
            <div class="yiche_lanmu">
                <em>|</em><span>品牌大全</span></div>
            <div class="bt_searchNew">
				<!--#include file="~/html/bt_searchV3.shtml"-->
            </div>
            <div class="bt_hot">
                热门搜索：<a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a>
                <a href="http://v.bitauto.com/original/list1.html" target="_blank">原创节目</a> <a href="http://jiangjia.bitauto.com/"
                    target="_blank">降价</a></div>
        </div>

        <%if (wt == "2345nyif")
        { %>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
    <script type="text/javascript">
        $(".header_style,.nav_small,.bit_top990").hide();
    </script>
    <%} %>
        <!--主导航-->
        <div id="treeFiexdNav" class="publicTabNew">
            <ul class="tab" id="ulForTempClickStat">
                <li id=""><a href="/brandlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按品牌</a></li>
                <li id="Li1"><a href="/charlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按车型</a></li>
                <li id="Li2"><a href="/price/<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按价格</a></li>
                <li id="Li3" class="current"><a href="/levellist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按级别</a></li>
                <li id="Li4"><a href="/countrylist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按国别</a></li>
                <li id="Li5"><a href="/functionlist.html<%=(wt=="2345nyif"?"?WT.mc_id=2345nyif":"") %>">按用途</a></li>
            </ul>
        </div>
        
    </div>
    <div class="bt_page">
        <div id="theanchor">
        </div>
        <div style="width:100%; height:50px; overflow:hidden;">
        <div id="theid">
        <div class="pinpai_box">
            <div class="pinpai_menu">
                <ul class="list11">
                    <li class=" "><a href="#a">微型车<span>|</span></a></li>
                    <li class=" "><a href="#b">小型车<span>|</span></a></li>
                    <li class=" "><a href="#c">紧凑型车<span>|</span></a></li>
                    <li class=" "><a href="#d">中型车<span>|</span></a></li>
                    <li class=" "><a href="#e">中大型车<span>|</span></a></li>
                    <li class=" "><a href="#f">豪华车<span>|</span></a></li>
                    <li class=" "><a href="#g">MPV<span>|</span></a></li>
                    <li class=" "><a href="#h">SUV<span>|</span></a></li>
                    <li class=" "><a href="#i">跑车<span>|</span></a></li>
                    <li class=" "><a href="#l">面包车<span>|</span></a></li>
                    <li class="last"><a href="#m">皮卡</a></li>
                </ul>
            </div>
        </div>
        </div>
        </div>
        <asp:Literal ID="lrContent" EnableViewState="false" runat="Server" />
        <!-- 调用尾 -->
        <!--# include file="~/html/friendLink.shtml"-->
    </div>
    <!--#include file="~/html/footer2014.shtml"-->
    <!--提意见浮层-->
	<!--# include file="~/include/pd/2014/00001/201701_cxzsy_ycfdc_Manual.shtml"-->
    <!--#include file="~/html/CommonBodyBottom.shtml"-->
     <%if (wt == "2345nyif")
        { %>
    <script type="text/javascript">
        $(".foot_box").hide();
        
    </script>
    <iframe id="iframeC" name="iframeC" src="" width="0" height="0" style="display:none;" ></iframe>

    <script type="text/javascript"> 
        function sethash() {
            hashH = document.documentElement.scrollHeight;
            urlC = "//www.2345.com/car/brand_iframe_guodu.htm";
            document.getElementById("iframeC").src = urlC + "#" + hashH;
        }
        window.onload = sethash;
    </script>
    <%} %>
</body>
</html>
