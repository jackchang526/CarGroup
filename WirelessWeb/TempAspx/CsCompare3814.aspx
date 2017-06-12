<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CsCompare3814.aspx.cs" Inherits="WirelessWeb.TempAspx.CsCompare3814" %>
<!DOCTYPE HTML>
<html xmlns="http://www.w3.org/1999/xhtml">
<head  runat="server">
    <meta charset="UTF-8">
   <title>【<%=se.SeoName%>配置】<%=se.Brand.SeoName%><%=se.Name%>详细参数介绍-手机易车网</title>
	<meta name="keywords" content="<%=se.SeoName%>参数,<%=se.SeoName%>配置,<%=se.Brand.SeoName%><%=se.Name%>" />
	<meta name="description" content="<%=se.SeoName%>配置:<%=se.Brand.SeoName%><%=se.Name%>频道为您提供<%=se.SeoName%>综合配置信息,包括<%=se.SeoName%>安全装备,<%=se.SeoName%>操控配置,<%=se.SeoName%>内饰配置,<%=se.SeoName%>参数性能,<%=se.SeoName%>车型资料大全等,查<%=se.SeoName%>参数配置,就上易车网" />
    <meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="no" />
    <meta name="format-detection" content="telephone=no"/>
    <!--#include file="~/ushtml/0000/myiche2016_cube_canshupeizhijianhua-1335.shtml"-->
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/CommonJs.js"></script>
</head>
<body>

    <!-- header start -->
    <div class="op-nav">
        <a id="gobackElm" href="javascript:window.history.go(-1);" class="btn-return">返回</a>
        <div class="tt-name">
            <a href="http://m.yiche.com/" class="yiche-logo">汽车</a><h1><%= se.ShowName %></h1>
        </div>
        <!--#include file="/include/pd/2016/wap/00001/201607_wap_common_ejdht_Manual.shtml"-->
    </div>
    <div class="op-nav-mark" style="display: none;"></div>
    <!-- header end -->
    <!-- 互联互通 start -->
    <%=CsHeadHTML %>
    <!-- 互联互通 end -->

    <div class="second-tags" id="m-tabs-head">
        <div class="pd15">
            <ul>
                <li class="current"><a href="javascript:;"><span>2017款</span></a></li>
                <li><a href="javascript:;"><span>2016款</span></a></li>
                <li><a href="javascript:;"><span>2015款</span></a></li>
            </ul>
        </div>
    </div>

    <div class="swiper-container-head">
    <div class="swiper-wrapper">
        <!-- 2017 start -->
        <div class="swiper-slide">
            <div class="peizhi-box">
                <div class="md-jibenxinxi" id="jibenxinxi"></div>

                <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2017_1.png" />
                <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2.png" />

                <div class="pd20">
                    <div class="small-tt">车身颜色</div>
                    <ul class="color-list">
                        <li style="background:#000000"></li>
                        <li style="background:#1358E0"></li>
                        <li style="background:#460A0B"></li>
                        <li style="background:#8C1000"></li>
                        <li style="background:#E0E0E2"></li>
                        <li style="background:#FFFFFF"></li>
                    </ul>
                    
                    <div class="small-tt">内饰颜色</div>
                    <ul class="color-list">
                        <li style="background:#000000"></li>
                    </ul>

                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="fadongji"></div>
                        <!-- <div class="big-tt">
                            1.5T涡轮增压发动机
                        </div>
                        <ul class="fadongji-list">
                            <li>
                                <dl>
                                    <dt><strong>112</strong>kw</dt>
                                    <dd>最大功率</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>152</strong>ps</dt>
                                    <dd>最大马力</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>205</strong>nm</dt>
                                    <dd>最大扭矩</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>9.65</strong>秒</dt>
                                    <dd>百公里加速</dd>
                                </dl>
                            </li>
                        </ul> -->

                        <div class="big-tt">
                            1.6L自然吸气发动机
                        </div>
                        <ul class="fadongji-list">
                            <li>
                                <dl>
                                    <dt><strong>94</strong>kw</dt>
                                    <dd>最大功率</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>128</strong>ps</dt>
                                    <dd>最大马力</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>168</strong>nm</dt>
                                    <dd>最大扭矩</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>12.6</strong>秒</dt>
                                    <dd>百公里加速</dd>
                                </dl>
                            </li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>发动机启动技术</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="luntai"></div>
                        <div class="big-tt">
                            轮胎
                        </div>
                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan3.png" style="margin-top:10px;" />
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>非全尺寸备胎</li>
                            <li>铝合金轮毂</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="xingchefuzhu"></div>
                        <div class="big-tt">
                            行车辅助
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="icon-list">
                            <li>
                                <span class="ico-abs"></span>
                                <dl>
                                    <dt>刹车防抱死</dt>
                                    <dd>ABS</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-esp"></span>
                                <dl>
                                    <dt>动态稳定控制</dt>
                                    <dd>ESP</dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-ebd"></span>
                                <dl>
                                    <dt>电子制动分配</dt>
                                    <dd>EBD</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-qianyinlikongzhi"></span>
                                <dl>
                                    <dt>牵引力控制</dt>
                                    <dd>ASR/TCS/TRC等</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-daocheleida"></span>
                                <dl>
                                    <dt>倒车雷达</dt>
                                    <dd>车后</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-shachefuzhu"></span>
                                <dl>
                                    <dt>刹车辅助</dt>
                                    <dd>EBA/BAS/BA/EVA</dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-shangpofuzhu"></span>
                                <dl>
                                    <dt>上坡辅助</dt>
                                    <dd></dd>
                                </dl>
                            </li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>倒车影像</li>
                            <li>GPS导航系统</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="yingyinxitong"></div>
                        <div class="big-tt">
                            影音系统
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>行车电脑显示屏</li>
                            <li>中控台液晶屏</li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>蓝牙系统</li>
                            <li>USB接口</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="anquanqinang"></div>
                        <div class="big-tt">
                            安全气囊
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>驾驶位安全气囊</li>
                            <li>副驾驶位安全气囊</li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>前排侧安全气囊</li>
                        </ul>

                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2017_5.png" style="margin-top:40px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="chuang"></div>
                        <div class="big-tt">
                            窗/后视镜
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>后雨刷器</li>
                            <li>后视镜带侧转向灯</li>
                            <li>外后视镜电动调节</li>
                            <li>内后视镜防眩目功能</li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>单天窗</li>
                            <li>外后视镜加热功能</li>
                        </ul>

                        <div class="tt tt-yellow">
                            全系无以下配置
                        </div>
                        <ul class="ul-list">
                            <li>外后视镜电动折叠功能</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="deng"></div>
                        <div class="big-tt">
                            灯
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>前大灯延时关闭</li>
                            <li>前大灯照射范围调整</li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>转向辅助灯</li>
                        </ul> -->

                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2017_6.png" style="margin-top:30px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="fangxiangpan"></div>
                        <div class="big-tt">
                            方向盘
                        </div>
                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2017_7.png" style="margin-top:30px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="suo"></div>
                        <div class="big-tt">
                            锁
                        </div>
                        <ul class="ul-list">
                            <li>中控门锁</li>
                            <li>遥控钥匙</li>
                            <li>儿童锁</li>
                            <li>发动机电子防盗</li>
                            <li>无钥匙进入系统</li>
                            <li>无钥匙启动系统</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="zuoyi"></div>
                        <div class="big-tt">
                            座椅
                        </div>
                        <ul class="ul-list">
                            <li>织物/皮革座椅</li>
                            <li>儿童安全座椅固定装置</li>
                            <li>手动调节座椅高低</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="kongtiao"></div>
                        <div class="big-tt">
                            空调
                        </div>
                        <ul class="ul-list">
                            <li>手动空调</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!-- 2017 end -->

        <!-- 2016 start -->
        <div class="swiper-slide">
            <div class="peizhi-box">

                <div class="md-jibenxinxi" id="jibenxinxi"></div>

                <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2016_1.png" />
                <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2.png" />

                <div class="pd20">
                    <div class="small-tt">车身颜色</div>
                    <ul class="color-list">
                        <li style="background:#000000"></li>
                        <li style="background:#1358E0"></li>
                        <li style="background:#460A0B"></li>
                        <li style="background:#8C1000"></li>
                        <li style="background:#E0E0E2"></li>
                        <li style="background:#FFFFFF"></li>
                    </ul>
                    
                    <div class="small-tt">内饰颜色</div>
                    <ul class="color-list">
                        <li style="background:#000000"></li>
                    </ul>

                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="fadongji"></div>
                        <!-- <div class="big-tt">
                            1.5T涡轮增压发动机
                        </div>
                        <ul class="fadongji-list">
                            <li>
                                <dl>
                                    <dt><strong>112</strong>kw</dt>
                                    <dd>最大功率</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>152</strong>ps</dt>
                                    <dd>最大马力</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>205</strong>nm</dt>
                                    <dd>最大扭矩</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>9.65</strong>秒</dt>
                                    <dd>百公里加速</dd>
                                </dl>
                            </li>
                        </ul> -->

                        <div class="big-tt">
                            1.6L自然吸气发动机
                        </div>
                        <ul class="fadongji-list">
                            <li>
                                <dl>
                                    <dt><strong>94</strong>kw</dt>
                                    <dd>最大功率</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>128</strong>ps</dt>
                                    <dd>最大马力</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>168</strong>nm</dt>
                                    <dd>最大扭矩</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>12.6</strong>秒</dt>
                                    <dd>百公里加速</dd>
                                </dl>
                            </li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>发动机启动技术</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="luntai"></div>
                        <div class="big-tt">
                            轮胎
                        </div>
                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan3.png" style="margin-top:10px;" />
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>非全尺寸备胎</li>
                            <li>铝合金轮毂</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="xingchefuzhu"></div>
                        <div class="big-tt">
                            行车辅助
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        
                        <ul class="icon-list">
                            <li>
                                <span class="ico-abs"></span>
                                <dl>
                                    <dt>刹车防抱死</dt>
                                    <dd>ABS</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-esp"></span>
                                <dl>
                                    <dt>动态稳定控制</dt>
                                    <dd>ESP</dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-ebd"></span>
                                <dl>
                                    <dt>电子制动分配</dt>
                                    <dd>EBD</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-qianyinlikongzhi"></span>
                                <dl>
                                    <dt>牵引力控制</dt>
                                    <dd>ASR/TCS/TRC等</dd>
                                </dl>
                            </li>
                            
                            <li>
                                <span class="ico-shachefuzhu"></span>
                                <dl>
                                    <dt>刹车辅助</dt>
                                    <dd>EBA/BAS/BA/EVA</dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-shangpofuzhu"></span>
                                <dl>
                                    <dt>上坡辅助</dt>
                                    <dd></dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-bocheleida"></span>
                                <dl>
                                    <dt>泊车雷达</dt>
                                    <dd>车前</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-daocheyingxiang"></span>
                                <dl>
                                    <dt>倒车影像</dt>
                                    <dd></dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-gps"></span>
                                <dl>
                                    <dt>GPS导航系统</dt>
                                    <dd></dd>
                                </dl>
                            </li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>倒车影像（车手）</li>
                            <li>倒车雷达</li>
                            <li>GPS导航系统</li>
                        </ul> -->
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="yingyinxitong"></div>
                        <div class="big-tt">
                            影音系统
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>行车电脑显示屏</li>
                            <li>中控台液晶屏</li>
                            <li>蓝牙系统</li>
                            <li>USB接口</li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            
                        </ul> -->
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="anquanqinang"></div>
                        <div class="big-tt">
                            安全气囊
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>驾驶位安全气囊</li>
                            <li>副驾驶位安全气囊</li>
                            <li>前排侧安全气囊</li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>前排头部气囊（气帘）</li>
                            <li>后排头部气囊（气帘）</li>
                        </ul>

                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2016_5.png" style="margin-top:40px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="chuang"></div>
                        <div class="big-tt">
                            窗/后视镜
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>单天窗</li>
                            <li>后雨刷器</li>
                            <li>后视镜带侧转向灯</li>
                            <li>外后视镜加热功能</li>
                            <li>外后视镜电动调节</li>
                            <li>内后视镜防眩目功能</li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>外后视镜电动折叠功能</li>
                        </ul> -->

                        <div class="tt tt-yellow">
                            全系无以下配置
                        </div>
                        <ul class="ul-list">
                            <li>外后视镜电动折叠功能</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="deng"></div>
                        <div class="big-tt">
                            灯
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>前大灯延时关闭</li>
                            <li>前大灯照射范围调整</li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>转向辅助灯</li>
                        </ul> -->

                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2016_5.png" style="margin-top:30px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="fangxiangpan"></div>
                        <div class="big-tt">
                            方向盘
                        </div>
                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2016_7.png" style="margin-top:30px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="suo"></div>
                        <div class="big-tt">
                            锁
                        </div>
                        <ul class="ul-list">
                            <li>中控门锁</li>
                            <li>遥控钥匙</li>
                            <li>儿童锁</li>
                            <li>发动机电子防盗</li>
                            <li>无钥匙进入系统</li>
                            <li>无钥匙启动系统</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="zuoyi"></div>
                        <div class="big-tt">
                            座椅
                        </div>
                        <ul class="ul-list">
                            <li>皮革座椅</li>
                            <li>儿童安全座椅固定装置</li>
                            <li>手动调节座椅高低</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="kongtiao"></div>
                        <div class="big-tt">
                            空调
                        </div>
                        <ul class="ul-list">
                            <li>手动空调</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!-- 2016 end -->
        
        <!-- 2015 start -->
        <div class="swiper-slide">
            <div class="peizhi-box">

                <div class="md-jibenxinxi" id="jibenxinxi"></div>
            
                <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2015_1.png" />
                <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2.png" />

                <div class="pd20">
                    <div class="small-tt">车身颜色</div>
                    <ul class="color-list">
                        <li style="background:#000000"></li>
                        <li style="background:#1358E0"></li>
                        <li style="background:#460A0B"></li>
                        <li style="background:#8C1000"></li>
                        <li style="background:#E0E0E2"></li>
                        <li style="background:#FFFFFF"></li>
                    </ul>
                    
                    <div class="small-tt">内饰颜色</div>
                    <ul class="color-list">
                        <li style="background:#000000"></li>
                    </ul>

                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="fadongji"></div>
                        <div class="big-tt">
                            1.5T涡轮增压发动机
                        </div>
                        <ul class="fadongji-list">
                            <li>
                                <dl>
                                    <dt><strong>125</strong>kw</dt>
                                    <dd>最大功率</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>170</strong>ps</dt>
                                    <dd>最大马力</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>230</strong>nm</dt>
                                    <dd>最大扭矩</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>10.69</strong>秒</dt>
                                    <dd>百公里加速</dd>
                                </dl>
                            </li>
                        </ul>

                        <div class="big-tt">
                            1.6L自然吸气发动机
                        </div>
                        <ul class="fadongji-list">
                            <li>
                                <dl>
                                    <dt><strong>92</strong>kw</dt>
                                    <dd>最大功率</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>125</strong>ps</dt>
                                    <dd>最大马力</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>160</strong>nm</dt>
                                    <dd>最大扭矩</dd>
                                </dl>
                            </li>
                            <li>
                                <dl>
                                    <dt><strong>13.64</strong>秒</dt>
                                    <dd>百公里加速</dd>
                                </dl>
                            </li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>发动机启动技术</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="luntai"></div>
                        <div class="big-tt">
                            轮胎
                        </div>
                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/irz3.png" style="margin-top:10px;" />
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>非全尺寸备胎</li>
                            <li>铝合金轮毂</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="xingchefuzhu"></div>
                        <div class="big-tt">
                            行车辅助
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="icon-list">
                            <li>
                                <span class="ico-abs"></span>
                                <dl>
                                    <dt>刹车防抱死</dt>
                                    <dd>ABS</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-esp"></span>
                                <dl>
                                    <dt>动态稳定控制</dt>
                                    <dd>ESP</dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-ebd"></span>
                                <dl>
                                    <dt>电子制动分配</dt>
                                    <dd>EBD</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-qianyinlikongzhi"></span>
                                <dl>
                                    <dt>牵引力控制</dt>
                                    <dd>ASR/TCS/TRC等</dd>
                                </dl>
                            </li>
                            <li>
                                <span class="ico-shachefuzhu"></span>
                                <dl>
                                    <dt>刹车辅助</dt>
                                    <dd>EBA/BAS/BA/EVA</dd>
                                </dl>
                            </li>

                            <li>
                                <span class="ico-shangpofuzhu"></span>
                                <dl>
                                    <dt>上坡辅助</dt>
                                    <dd></dd>
                                </dl>
                            </li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>倒车影像（车手）</li>
                            <li>倒车雷达</li>
                            <li>GPS导航系统</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="yingyinxitong"></div>
                        <div class="big-tt">
                            影音系统
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>行车电脑显示屏</li>
                            <li>中控台液晶屏</li>
                            <li>USB接口</li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>蓝牙系统</li>  
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="anquanqinang"></div>
                        <div class="big-tt">
                            安全气囊
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>驾驶位安全气囊</li>
                            <li>副驾驶位安全气囊</li>
                            <li>前排侧安全气囊</li>
                        </ul>

                        <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>前排头部气囊（气帘）</li>
                            <li>后排头部气囊（气帘）</li>
                        </ul>

                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2015_5.png" style="margin-top:40px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="chuang"></div>
                        <div class="big-tt">
                            窗/后视镜
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>后雨刷器</li>
                            <li>后视镜带侧转向灯</li>
                            <li>外后视镜电动调节</li>
                            <li>内后视镜防眩目功能</li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            
                        </ul> -->

                        <div class="tt tt-yellow">
                            全系无以下配置
                        </div>
                        <ul class="ul-list">
                            <li>单天窗</li>
                            <li>外后视镜加热功能</li>
                            <li>外后视镜电动折叠功能</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="deng"></div>
                        <div class="big-tt">
                            灯
                        </div>
                        <div class="tt tt-blue">
                            全系标配
                        </div>
                        <ul class="ul-list">
                            <li>前大灯延时关闭</li>
                            <li>前大灯照射范围调整</li>
                        </ul>

                        <!-- <div class="tt tt-red">
                            部分车款配置
                        </div>
                        <ul class="ul-list">
                            <li>转向辅助灯</li>
                        </ul> -->

                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2015_6.png" style="margin-top:30px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="fangxiangpan"></div>
                        <div class="big-tt">
                            方向盘
                        </div>
                        <img src="http://image.bitautoimg.com/uimg/mbt2016/canshupeizhi/images/zhixuan2015_7.png" style="margin-top:30px;" />
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="suo"></div>
                        <div class="big-tt">
                            锁
                        </div>
                        <ul class="ul-list">
                            <li>中控门锁</li>
                            <li>遥控钥匙</li>
                            <li>儿童锁</li>
                            <li>发动机电子防盗</li>
                            <li>无钥匙进入系统</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="zuoyi"></div>
                        <div class="big-tt">
                            座椅
                        </div>
                        <ul class="ul-list">
                            <li>皮革座椅</li>
                            <li>儿童安全座椅固定装置</li>
                            <li>手动调节座椅高低</li>
                        </ul>
                    </div>
                </div>

                <div class="pd20">
                    <div class="peizhi-item">
                        <div class="md-jibenxinxi" id="kongtiao"></div>
                        <div class="big-tt">
                            空调
                        </div>
                        <ul class="ul-list">
                            <li>手动空调</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <!-- 2015 end -->
    </div>
</div>

<!-- <div class="float-catalog-mask" id="popup-menumask" style="display:none;"></div>
<div class="float-catalog" id="popup-menu">
    目录
    <div class="catalog-list" id="popup-menulist" style="display:none;">
        <ul>
            <li><a href="#jibenxinxi"><span>基本信息</span></a></li>
            <li><a href="#fadongji"><span>发动机</span></a></li>
            <li><a href="#luntai"><span>轮胎</span></a></li>
            <li><a href="#xingchefuzhu"><span>行车辅助</span></a></li>
            <li><a href="#yingyinxitong"><span>影音系统</span></a></li>
            <li><a href="#anquanqinang"><span>安全气囊</span></a></li>
            <li><a href="#chuang"><span>窗/后视镜</span></a></li>
            <li><a href="#deng"><span>灯</span></a></li>
            <li><a href="#fangxiangpan"><span>方向盘</span></a></li>
            <li><a href="#suo"><span>锁</span></a></li>
            <li><a href="#zuoyi"><span>座椅</span></a></li>
            <li><a href="#kongtiao"><span>空调</span></a></li>
        </ul>
        <div class="ico-catalog-arrow"></div>
    </div>
</div> -->

<div id="floatPop">
    <a href="/yidongliangxiang/peizhi" class="return-peizhi" data-channelid="27.165.1756">查看车款配置</a>
    <a href="/yidongliangxiang/peizhi" class="return-peizhi" style="display:none;" data-channelid="27.165.1756">查看车款配置</a>
    <a href="/yidongliangxiang/peizhi" class="return-peizhi" style="display:none;" data-channelid="27.165.1756">查看车款配置</a>
</div>

    <script type="text/javascript" src="//bglog.bitauto.com/getbglog.js?v=20170405"></script>
 <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <%-- 添加停留时长统计代码--%>
    <!--#include file="/include/pd/2016/wap/00001/201606_wap_daogou_tuijian_js_Manual.shtml"-->
    <script src="http://image.bitautoimg.com/uimg/mbt2015/js/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="http://image.bitautoimg.com/carchannel/wirelessjs/v2/swiper-3.3.1.min.js?v=20170405"></script>

<script>
    var tabsSwiperHead = new Swiper('.swiper-container-head', {
        speed: 500,
        autoHeight: true,
        onSlideChangeStart: function () {
            $("#m-tabs-head .current").removeClass('current');
            $("#m-tabs-head li").eq(tabsSwiperHead.activeIndex).addClass('current');
        }
    })
    $("#m-tabs-head li").on('touchstart mousedown', function (e) {
        e.preventDefault();
        $("#m-tabs-head .current").removeClass('current');
        $(this).addClass('current');
        tabsSwiperHead.slideTo($(this).index());

        $('#floatPop a').hide();
        $('#floatPop a').eq($(this).index()).show();    })
    $("#m-tabs-head li").click(function (e) {
        e.preventDefault();
    })


    // $(document).ready(function(){
    //     $('#popup-menu').click(function(){
    //         $('#popup-menumask').toggle();
    //         $('#popup-menulist').toggle();
    //     });
    //     $('#popup-menumask').click(function(){
    //         $(this).hide();
    //         $('#popup-menulist').hide();
    //     });
    // });
</script>

</body>

</html>