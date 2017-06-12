<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperSelectCarTool.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.SuperSelectCarTool" %>

<% Response.ContentType = "text/html"; %>
<!DOCTYPE html>
<html>
<head>
	<title>【高级选车工具_汽车高级搜索_按条件找车】车型大全-易车网</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit" />
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/html5shiv.min.js"></script>
    <script src="http://img1.bitautoimg.com/uimg/2016/yiche/js/Respond.min.js"></script>
    <![endif]-->
	<meta name="Keywords" content="选车,选车工具,车型筛选,车型查询,易车网" />
	<meta name="Description" content="易车网高级选车工具频道为您提供按照各种条件查询车型功能，包括按汽车价格、参数配置、汽车级别、国产进口、变速方式、汽车排量等，如何挑选一款符合您心意的好车，易车网高级选车工具帮您解决。" />    <!--#include file="~/ushtml/0000/yiche_2016_cube_gaojisousuo_style-1265.shtml"-->
</head> 
<body data-offset="-130" data-target=".left-nav" data-spy="scroll">
    <span id="yicheAnchor" name="yicheAnchor" style="display: block; height: 0; width: 0; line-height: 0; font-size: 0"></span>
    <!--#include file="~/htmlV2/header2016.shtml"--> 
    <header class="header-main special-header2">
        <div class="container section-layout top" id="topBox">
            <div class="col-xs-4 left-box">
                <div class="section-left">
                    <h1 class="logo"><a href="http://www.yiche.com">汽车</a></h1>
                    <h2 class="title">高级选车工具</h2>
                </div>
            </div>
            <div class="col-xs-8 right-box">
                <ul class="list keyword-list">
                    <li class="dt">热门搜索：</li>
                    <li><a href="http://v.bitauto.com/tags/list8180_new.html" target="_blank">易车体验</a></li>
                    <li><a href="http://v.bitauto.com/ycjm.shtml" target="_blank">原创节目</a></li>
                    <li><a href="http://jiangjia.bitauto.com/" target="_blank">降价</a></li>
                </ul>
                <!--#include file="~/htmlV2/bt_searchV3.shtml"-->
            </div>
        </div>
    </header>

    <div class="tjbox" id="topfixed">
            <div class="tiaojianbox" id="filterContent" style="display: none">
                <ul class="list" id="parameters" style="display: ">
             
                </ul>
                <div class="sumbit">
                    <a class="btn btn-primary2 btn-sm pull-left confirmButton" href="javascript:void(0);">确定</a>
                    <div class="text_chexing" id="styleTotal">
                    
                    </div>
                    <div class="search-note-box">
                        <input type="text" class="input" placeholder="快速搜配置，如：自动泊车" name="txtsearch" value="" maxlength="20"/>
                        <a class="btn btn-primary2 btn-sm" href="javascript:;" name="btnsearch">搜索</a>
                    </div>
                </div>
            </div>
            <div class="tiaojianbox" id="nofilterContent">
                <div class="sumbit">
                    <a class="btn btn-sm pull-left disabled" href="javascript:void(0);">确定</a>
                    <div class="text_chexing">
                        <p class="text-light">请选择查询条件</p>
                    </div>
                    <div class="search-note-box">
                        <input type="text" class="input" placeholder="快速搜配置，如：自动泊车" name="txtsearch" value="" maxlength="20"/>
                        <a class="btn btn-primary2 btn-sm" href="javascript:;" name="btnsearch">搜索</a>
                    </div>
                </div>
            </div>
        </div>
    <div class="container advanced-selectcar-section" id="box">
            <div class="line-box tiaojian_allbox" id="main_box">
                <div class="section-header header2 mb0">
                    <div class="box">
                        <h2>基本条件</h2>
                    </div>
                    <div class="more"><a href="http://car.bitauto.com/">切换到简版选车工具&gt;&gt;</a></div>
                </div>
                <div class="tiaojianlistbox" id="params-carinfo">
                    <dl>
                        <dt>热门品牌</dt>
                        <dd>
                            <ul class="tj_list" id="masterbrandList">
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_8" />大众</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_7" />丰田</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_26" />本田</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_30" />日产</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_3" />宝马</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_127" />别克</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_9" />奥迪</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_49" />雪佛兰</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_13" />现代</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_42" />奇瑞</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_15" />比亚迪</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_17" />福特</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_18" />马自达</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_34" />吉利</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_2" />奔驰</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_16" />铃木</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_196" />哈弗</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_28" />起亚</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_5" />标致</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_10" />斯柯达</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="mid_6" />雪铁龙</label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>价格</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_0" />不限</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_1" />5万以下</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_2" />5-8万</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_3" />8-12万</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_4" />12-18万</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_5" />18-25万</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_6" />25-40万</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_7" />40-80万</label></li>
                                <li>
                                    <label>
                                        <input name="price" type="radio" id="p_8" />80万以上</label></li>
                                <li>
                                    <div class="popup-price">
                                        <div class="jiagefanwei" id="autoPrice">
                                            <input name="price" type="radio" id="p_9" /><input id="p_min" onkeyup="value=value.replace(/(\D|\d{5})/g,'')" maxlength="4" class="inputborder input36" />至
                                            <input id="p_max" onkeyup="value=value.replace(/(\D|\d{5})/g,'')" maxlength="4" class="inputborder input36" />万</div>
                                        <a id="btnPriceSubmit" href="javascript:;" class="btn btn-primary2 btn-sm pull-left">确定</a>
                                        <span class="comment_red" id="p_alert"></span>
                                    </div> 
                                    </li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>级别</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_1" />微型车  
                                        <span class="popup-layout-1">
                                        <span class="info">A00级车，大多数轴距在2.0米至2.3米之间，车身长度在3.65米以内，排量多在1.2L以下，好开好停。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_2" />小型车 
                                        <span class="popup-layout-1">
                                        <span class="info">A0级车，大多数轴距在2.2米至2.5米之间，排量多在1-1.3L之间。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_3" />紧凑型车
                                         <span class="popup-layout-1">
                                        <span class="info">A级车，是最常见的家用型车，轴距一般在2.5-2.7米之间，发动机排量一般1.6-2.0L左右，国内市场主流车型。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_5" />中型车
                                        <span class="popup-layout-1">
                                        <span class="info">即B级车，轴距一般为2.6-2.8米，车身4.6-4.9米，排量通常在2.0-3.0L之间，一般在内部空间、配置、舒适性等方面都要比紧凑型车高一个档次，更加适合于商务用途。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_4" />中大型车     
                                        <span class="popup-layout-1">
                                        <span class="info">中大型车即C级车，也被成为“行政级”车型，轴距普遍都超过2.8米，发动机排量通常在2.5L以上。在内部空间及配置上都要比中型车更加出色。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_6" />豪华车
                                         <span class="popup-layout-1">
                                        <span class="info">D级车，车长度普遍在5米或5米以上，轴距基本上在3米左右。一般6缸以上，有非常好的安全设施，内部配置非常高，乘坐舒适感强。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_7" />MPV
                                        <span class="popup-layout-1">
                                        <span class="info">MPV是从旅行轿车逐渐演变而来的，它集旅行车宽大乘员空间、轿车的舒适性、和厢式货车的功能于一身，一般为两厢式结构，即多用途车。通俗地说，就是可以坐7-8人的小客车。</span></span>
                                    </label></li>
                                <li class="ico-arrow">
                                    <label>
                                        <input type="checkbox" id="l_8" />SUV
                                        <span class="popup-layout-1">
                                        <span class="info">又叫运动型多用途汽车，是一种拥有旅行车般的空间机能，配以货卡车的越野能力的车型。一般前悬架是轿车型的独立悬架，后悬架是非独立悬架，离地间隙较大，在一定程度上既有轿车的舒适性又有越野车的越野性能。</span></span>
                                    </label><i class="sanjiao"></i></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_9" />跑车</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_11" />面包车</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_12" />皮卡</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_18" />客车</label></li>
                            </ul>
                            <div class="clear"></div>
                            <div class="openlayer">
                                <ul id="suvList">
                                    <li><label><input type="checkbox" id="l_13" />小型SUV
                                        <span class="popup-layout-1">
                                         <span class="info">通常指长度小于4米的SUV。</span></span>
                                        </label></li>
                                    <li><label><input type="checkbox" id="l_14" />紧凑型SUV
                                         <span class="popup-layout-1">
                                         <span class="info">通常指长度在4-4.3米的SUV。</span></span>
                                        </label></li>
                                    <li><label><input type="checkbox" id="l_15" />中型SUV 
                                        <span class="popup-layout-1">
                                        <span class="info">通常指长度在4.3-4.6米的SUV。</span></span>
                                        </label></li>
                                    <li><label><input type="checkbox" id="l_16" />中大型SUV
                                    <span class="popup-layout-1">
                                         <span class="info">通常指长度在4.6-5米的SUV。</span></span>
                                        </label></li>
                                    <li><label><input type="checkbox" id="l_17" />全尺寸SUV
                                        <span class="popup-layout-1">
                                         <span class="info">通常指长度在大于5米的SUV。</span></span>
                                        </label></li>
                                </ul>
                                 <div class="clear"></div>
                            </div>
                        </dd>
                    </dl>
                    <dl>
                        <dt>厂商</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="g_1" />自主</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="g_2" />合资</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="g_4" />进口</label></li>

                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>国别</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="c_4" />德系</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="c_2" />日系</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="c_16" />韩系</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="c_8" />美系</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="c_484" />欧系</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="c_509" />非日系</label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>排量</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_0" />不限</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_1" />1.4L以下</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_2" />1.4-1.6L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_3" />1.6-1.8L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_4" />1.8-2.0L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_5" />2.0-3.0L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="dis" id="d_6" />3.0L以上</label></li>
                                <li>
                                    <div class="jiagefanwei">
                                        <input type="radio" name="dis" id="d_7" /><input id="d_min" maxlength="4" class="inputborder input36" />至
                                        <input id="d_max" maxlength="4" class="inputborder input36" />L</div>
                                    <a  id="btnDisSubmit" class="btn btn-primary2 btn-sm pull-left" href="javascript:;">确定</a>
                                    <span class="comment_red" id="d_alert"></span></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>变速箱</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="t_1" />手动<span class="popup-layout-1">
                                        <span class="info">用手拨动变速杆改变变速器内的齿轮啮合位置，改变传动比，从而达到变速的目的。手动变速器的传动效率要比自动变速器的高，手动变速的汽车在加速、超车时比自动变速车快，也省油。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="t_32" />半自动
                                        <span class="popup-layout-1">
                                        <span class="info">介于手动与自动之间的变速器，它不同于手动变速器传统的“H”型挡位结构，其换挡手柄只能前后移动进行升挡或降挡。目前市面上半自动变速器的车型比较少。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="t_2" />自动
                                        <span class="popup-layout-1">
                                        <span class="info">顾名思义就是不用驾驶者去手动换挡，车辆会根据行驶的速度和交通情况自动选择合适的挡位行驶。驾驶时把档位放到前进挡，直接控制油门和刹车即可，但是自动挡不能空挡滑行。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="t_4" />手自一体
                                        <span class="popup-layout-1">
                                        <span class="info">“手自一体”就是将汽车的手动换挡和自动换挡结合在一起的变速方式。手动挡因为自己可以自由调节挡位及转速，驾驶起来有种畅快的感觉，运动感十足。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="t_8" />无极变速
                                        <span class="popup-layout-1">
                                        <span class="info">自动挡的一种，无级变速指可以连续获得变速范围内任何传动比的变速系统。通过无级变速可以得到传动系与发动机工况的最佳匹配，无级变速的加速过程十分流畅，没有换挡的顿挫感。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="t_16" />双离合
                                        <span class="popup-layout-1">
                                        <span class="info">双离合变速器有别于一般的自动变速器系统，它既属于手动变速器而又属于自动变速器，除了拥有手动变速器的灵活性及自动变速器的舒适性外，还能提供无间断的动力输出。而传统的手动变速器使用一台离合器，当换挡时，驾驶员须踩下离合器踏板，使不同挡的齿轮做出啮合动作，而动力就在换挡期间出现间断，令输出表现有所断续。</span></span>
                                    </label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>驱动</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="dt_1" />前驱
                                        <span class="popup-layout-1">
                                        <span class="info">中、小型轿车多使用前置前驱。优点：结构简单，比前置后驱节省了从前置马达向驱动后轮传递动力的传动轴。节省燃油。缺点：由于前轮同时承担了驱动和转向两项功能，因此在高速行驶时稳定性较差。并且，由于驱动轮在车体负重较轻的前端，上坡时驱动轮容易打滑，下坡时容易翻车。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="dt_2" />后驱
                                        <span class="popup-layout-1">
                                        <span class="info">汽车的后轮驱动，在拼合良好的路面上启动、加速或爬坡时，驱动轮的负荷增大(即驱动轮的附着压力增大)，其牵引性能比前置前驱型式优越，但是会增加车重，影响乘坐的舒适性。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="dt_4" />全时四驱
                                        <span class="popup-layout-1">
                                        <span class="info">全时四驱就是任何时间，车辆都是四个轮子独立推动的驱动装置，全时四驱通过一个柔性连接的中央差速器，再通过前轴和后轴的独立差速器，把驱动力分配到四个轮胎。费油，但是在所有路况条件下都具有更大的牵引力，特别是在湿滑和冬季条件下，爬坡能力强。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="dt_8" />分时四驱
                                        <span class="popup-layout-1">
                                        <span class="info">是四轮驱动的一种高级版本。驾驶者根据路面情况，通过接通或断开分动器来变化两轮驱动或是四轮驱动模式，从而实现两驱和四驱自由转换的驱动方式。同时兼顾坦途省油，和越野的机动性，这也是越野车或是四驱SUV最常见的驱动模式。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="dt_16" />适时四驱
                                        <span class="popup-layout-1">
                                        <span class="info">只有在适当的时候才会转换为四轮驱动，而在其它情况下仍然是两轮驱动的驱动系统。系统会根据车辆的行驶路况自动切换为两驱或四驱模式，不需要人为操作。</span></span>
                                    </label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>燃料</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="f_7" />汽油</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="f_8" />柴油</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="f_16" />纯电动</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="f_2" />油电混合</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="f_4" />油气混合</label></li>
                               <%-- <li>
                                    <label>
                                        <input type="checkbox" id="f_32" />LPG</label></li>--%>
                                <li>
                                    <label>
                                        <input type="checkbox" id="f_64" />CNG
                                        <span class="popup-layout-1">
                                        <span class="info">燃料的一种，CNG(Compressed Natural Gas),即压缩天然气，是天然气加压(超过3,600磅/平方英寸)并以气态储存在容器中。</span></span>
                                    </label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>油耗</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_0" />不限</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_1" />6L以下</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_2" />6-8L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_3" />8-10L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_4" />10-12L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_5" />12-15L</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="fuelcon" id="fc_6" />15L以上</label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>供油方式</dt>
                        <dd>
                            <ul class="tj_list">
                               <%-- <li>
                                    <label>
                                        <input type="checkbox" id="more_275" />单点电喷</label></li>--%>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_276" />多点电喷
                                        <span class="popup-layout-1">
                                        <span class="info">多点电喷是相对于单点电喷来说的，就是在每个气缸上都加装了喷油器，而单点电喷是多缸使用同一个喷油器。并且多点喷射将喷射器设在进气门处，燃油在热的进气门上进一步蒸发与空气充分混合后立即通过进气门进入燃烧室，不受到进气歧管结构的影响，可以保证一致的混合气分配，更加省油。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_277" />混合喷射
                                         <span class="popup-layout-1">
                                        <span class="info">混合喷射就是把歧管喷射，加入到直喷发动机之中。低负荷工况时，歧管喷油嘴在气缸进气行程时喷油，混合气进入气缸，再配合压缩行程时气缸内喷油嘴喷油，从而实现分层燃烧；高负荷工况时，只在压缩行程进行缸内直喷。发动机工作效率高，多见于中级及以上的车。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_278" />直喷
                                         <span class="popup-layout-1">
                                        <span class="info">缸内直喷（GDI）就是直接将燃油喷入气缸内与进气混合的技术。优点是油耗量低，升功率大，压缩比高达12，与同排量的一般发动机相比功率与扭矩都提高了10%。目前的劣势是零组件复杂，而且价格通常要更贵。</span></span>
                                    </label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>车身</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="radio" name="body" id="b_0" />不限</label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="body" id="b_1" />两厢
                                        <span class="popup-layout-1">
                                        <span class="info">一种将驾驶室和后备厢做成同一个厢体，并且发动机独立的布置形式。这种布局形式能增加车内空间，相应的后备箱空间就有所减少。因此多用于小型车和紧凑型车。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="body" id="b_2" />三厢
                                        <span class="popup-layout-1">
                                        <span class="info">我们一般常见的轿车都是三厢车，三厢是指前部的发动机舱、车身中部的乘员舱和后部的行李舱。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="radio" name="body" id="lv_1" />旅行版
                                        <span class="popup-layout-1">
                                        <span class="info">在英语中，旅行车称为“wagon”，大多数旅行车都是以轿车为基础，把轿车的后备厢加高到与车顶齐平，用来增加行李空间。Wagon的魅力在于它既有轿车的舒适，也有相当大的行李空间，外形也相当的稳重，有成熟的魅力。</span></span>
                                    </label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>车门数</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_268" />2门
                                        <span class="popup-layout-1">
                                        <span class="info">2门多是三厢跑车，后备箱门不算门。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_269" />3门
                                        <span class="popup-layout-1">
                                        <span class="info">3门是指有两个乘客门加一个掀背门的两箱车。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_270" />4门
                                        <span class="popup-layout-1">
                                        <span class="info">4门是最常见普通三厢轿车，后备箱门不算门。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_271" />5门
                                        <span class="popup-layout-1">
                                        <span class="info">两箱轿车及suv都算5门，掀背式的后备箱算是一个门。</span></span>
                                    </label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_272" />6门
                                        <span class="popup-layout-1">
                                        <span class="info">6门多见于加长豪车或概念车，后备箱如果是对开门的车算是2个门，再加上常规4个乘客门就是6个门。</span></span>
                                    </label></li>
                            </ul>
                        </dd>
                    </dl>
                    <dl>
                        <dt>座位数</dt>
                        <dd>
                            <ul class="tj_list">
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_262" />2座</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_263" />4座</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_264" />5座</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_265" />6座</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_266" />7座</label></li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="more_267" />7座以上</label></li>
                            </ul>
                        </dd>
                    </dl>
                </div>
                <div class="section-header header2 mb0">
                    <div class="box">
                        <h2>更多条件</h2>
                    </div>
                </div>
                <div class="tiaojianlistbox">
                  <h6 id="params-carengine">发动机</h6>
                    <dl><dt>进气形式</dt>
				    <dd>
				        <ul class="tj_list">
					        <li><label><input type="radio" name="airadmission" id="more_0"/>不限</label></li>
					        <li><label><input type="radio" name="airadmission" id="more_100"/>自然吸气
                                <span class="popup-layout-1">
                                <span class="info">自然吸气是汽车进气的一种，是在不通过任何增压器的情况下，大气压将空气压入燃烧室的一种形式,自然吸气发动机在动力输出上的平顺性与响应的直接性上，要远优于增压发动机，但是相对增压发动机更费油。</span></span>
                            </label></li>
					        <li class="ico-arrow"><label><input type="radio" name="airadmission" id="more_101"/>增压</label><i class="sanjiao"></i></li>
					    </ul>
					    <div class="clear"></div>
					    <div class="openlayer">
						    <ul>
							    <li><label><input type="checkbox" id="more_102"/>涡轮增压
                                    <span class="popup-layout-1">
                                    <span class="info">涡轮增压是在发动机上加一个增压器，使废气重新进入发动机，增加进气量，从而提高发动机的功率和扭矩，让车子更有劲。一台发动机装上涡轮增压器后，其最大功率与未装增压器的时候相比可以增加40%甚至更高。它的燃油经济性和尾气排放都很好，但是对发动机压力大，动力输出也不够平顺。</span></span>
                                    </label></li>
							    <li><label><input type="checkbox" id="more_103"/>双涡轮增压
                                    <span class="popup-layout-1">
                                     <span class="info">双涡轮增压是涡轮增压的方式之一。针对废气涡轮增压的涡轮迟滞现象，串联一大一小两只涡轮或并联两只同样的涡轮，在发动机低转速的时候，较少的排气即可驱动涡轮高速旋转以产生足够的进气压力，减小涡轮迟滞效应，从而提升燃油效率。</span></span>
                                    </label></li>
							    <li><label><input type="checkbox" id="more_104"/>机械增压
                                    <span class="popup-layout-1">
                                    <span class="info">机械增压是通过发动机输出增加空气压力，它完全解决了涡轮增压中油门响应滞后，涡轮迟滞和动力输出突然现象，达到瞬时油门响应，动力随转速线性输出，增加驾驶性能。此外，在低速高扭、瞬间加速，机械增压技术都优于涡轮增压技术。但是它也存在加速效果不明显，噪音大的问题。</span></span>
                                    </label></li>
							    <li><label><input type="checkbox" id="more_105"/>涡轮机械双增压
                                    <span class="popup-layout-1">
                                    <span class="info">使用两种不同的增压方式，并且中和了两种增压方式的不足，低转时充分利用机械增压系统对扭力的帮助，保持线性加速，没有顿挫感；当发动机高转时启动涡轮增压，达到明显提速的作用。</span></span>
                                    </label></li>
						    </ul>
                             <div class="clear"></div>
					    </div>
				    </dd></dl>
                    <dl><dt>发动机位置</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_106"/>前置
                               <span class="popup-layout-1">
                               <span class="info">前置发动机，即发动机位于前轮轴之前。前置发动机的优点是简化了车子变速器与驱动桥的结构，特别是对于目前占绝对主流的前轮驱动车型而言，发动机将动力直接输送到前轮上，省略了长长的传动轴，不但减少了功率传递损耗，也大大降低了动力传动机构的复杂性和故障率。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_107"/>中置
                               <span class="popup-layout-1">
                               <span class="info">轿车的中置发动机一般是超跑使用，即发动机位于车辆的前后轴之间。中置发动机的汽车肯定是后轮驱动或者四轮驱动。发动机中置的特点就是将车辆中惯性最大的发动机置于车体的中央，这样可以使车身重量分布接近理想平衡状态。此外面包小客也会使用这种布局。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_108"/>后置
                               <span class="popup-layout-1">
                               <span class="info">将发动机、离合器、变速器都横向布置于车辆驱动桥（后桥）之后。大、中型客车中较多使用这种布置方案，因为这样会使其前后轴获得合理的载荷分配；车内噪声也会因此降低。但是，发动机冷却条件较差，发动机和离合器、变速器的操纵机构较为复杂。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>气缸排列</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_109"/>L型
                               <span class="popup-layout-1">
                               <span class="info">直列布局是如今使用最为广泛的气缸排列形式，尤其是在2.5L以下排量的发动机上。这种布局的发动机的所有气缸均是按同一角度并排成一个平面，并且只使用了一个气缸盖，同时其缸体和曲轴的结构也要相对简单，好比气缸们站成了一列纵队。但是6缸以上的车不适用。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_110"/>V型
                               <span class="popup-layout-1">
                               <span class="info">将所有汽缸分成两组，把相邻汽缸以一定夹角布置一起，使两组汽缸形成一个60°夹角的平面。V型发动机的机体长度和高度都更短，安装位置低，所以风阻系数低，发动机运转也更平顺。多见于中级及以上的车。</span></span>
                        </label></li>
                <%--    <li><label><input type="checkbox" id="more_111"/>B型</label></li>--%>
                    <li><label><input type="checkbox" id="more_112"/>W型
                              <span class="popup-layout-1">
                               <span class="info">V型发动机的一个变种，德国大众专属发动机技术，W型与V型发动机相比可将发动机做得更短一些，曲轴也可短些，这样就能节省发动机所占的空间，同时重量也可轻些，但它的宽度更大，使得发动机舱更满。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_113"/>H型
                               <span class="popup-layout-1">
                               <span class="info">发动机活塞平均分布在曲轴两侧，在水平方向上左右运动。可以使发动机的整体高度降低、长度缩短、整车的重心降低，车辆行驶更加平稳，降低车辆在行驶中的振动。但是水平对置结构较为复杂，使得机油润滑等问题很难解决。目前只有斯巴鲁和保时捷在使用该技术。</span></span>
                        </label></li>
                <%--    <li><label><input type="checkbox" id="more_114"/>R型</label></li>
                    <li><label><input type="checkbox" id="more_115"/>转子</label></li>--%>
                    </ul></dd></dl>
                    <dl><dt>气缸数</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_116"/>3缸以下
                            <span class="popup-layout-1">
                            <span class="info">3缸以下的车非常少，主要是个别手动挡的柴油车，以及宝马i3这样的电动车。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_117"/>3缸 
                           <span class="popup-layout-1">
                            <span class="info">排量1L以下的发动机通常用3缸，一般来说，在同等缸径下，缸数越多，排量越大，功率越高；在同等排量下，缸数越多，缸径越小，转速可以提高，从而获得较大的提升功率。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_118"/>4缸 
                            <span class="popup-layout-1">
                            <span class="info">排量1-2.5L的发动机常用4缸，一般来说，在同等缸径下，缸数越多，排量越大，功率越高；在同等排量下，缸数越多，缸径越小，转速可以提高，从而获得较大的提升功率。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_119"/>5缸
                            <span class="popup-layout-1">
                            <span class="info">排量在2.5-3L的发动机可能会使用5缸，但是奇数缸数发动机并不常用，目前主要是沃尔沃的汽车多使用此技术。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_120"/>6缸
                            <span class="popup-layout-1">
                            <span class="info">排量3L左右的发动机常用6缸，一般来说，在同等缸径下，缸数越多，排量越大，功率越高；在同等排量下，缸数越多，缸径越小，转速可以提高，从而获得较大的提升功率。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_121"/>8缸
                            <span class="popup-layout-1">
                            <span class="info">排量4L左右的发动机常用8缸，一般来说，在同等缸径下，缸数越多，排量越大，功率越高；在同等排量下，缸数越多，缸径越小，转速可以提高，从而获得较大的提升功率。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>环保标准</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_122"/>欧5
                            <span class="popup-layout-1">
                            <span class="info">欧5标准是欧洲汽车尾气排放第五代标准，也是最严格的一项标准。已从2009年开始在欧洲实施。欧5标准限定汽车最大颗粒排放为0.005克/公里，氮氧化物排放量为0.2克/公里。实行“欧5”标准后，所有的柴油新车将必须增加颗粒物滤网。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_123"/>欧4
                            <span class="popup-layout-1">
                            <span class="info">欧4标准是指欧洲第四代标准，该标准2005年底开始实施，是汽车废气排放的一个重要标准，目前在多数国家施行。该标准要求柴油轿车每公里氮氧化物排放量不得超过250毫克；面包车和SUV每公里氮氧化物排放量不得超过390毫克。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_124"/>欧3
                            <span class="popup-layout-1">
                            <span class="info">欧3标准是由欧盟组织制定的汽车废气排放标准，实施时间为2000年至2005年。按照这一标准，商用汽车废气中碳氢化合物、一氧化碳、氮氧化合物和微粒的限值分别为0.66%、2.1%、5%和0.1%。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_125"/>国5
                            <span class="popup-layout-1">
                            <span class="info">将于2017年1月1日起在全国实施第五阶段国家机动车排放标准，相比国4标准，新标准轻型车氮氧化物排放可以降低25%，重型车氮氧化物排放可以降低43%。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_126"/>国4</label></li>
                    <li><label><input type="checkbox" id="more_127"/>京5
                            <span class="popup-layout-1">
                            <span class="info">2013年3月1日，经国务院批准，北京在国5标准出台前正式实施的机动车排放标准京5标准，此标准相当于欧5级别。比国5标准更严格一些，是目前国内最高的排放标准。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <h6 id="params-bottomstop">底盘制动</h6>
                    <dl><dt>底盘结构</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_128"/>承载式
                            <span class="popup-layout-1">
                            <span class="info">承载式车身没有刚性车架，只是加强了车头、侧围、车尾、底板等部位，发动机、前后悬架、传动系统的一部分等总成部件装配在车身上设计要求的位置。优点：安全、上下车方便、省油；缺点：噪音大、成本高，受力不均易变形。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_129"/>非承载式
                               <span class="popup-layout-1">
                                <span class="info">古老的技术。有一个刚性的车架，车架承载着整个车体，发动机、悬挂和车身都安装在车架上，车架上有用于固定车身的螺孔以及固定弹簧的基座的一种底盘形式。优点：车身和底盘强度高、越野性强、平稳安全。缺点：跑公路不平稳、会震动，翻车容易压扁车身，质量大，费油。</span></span>
                            </label></li>
                    <li><label><input type="checkbox" id="more_130"/>半承载式
                             <span class="popup-layout-1">
                            <span class="info">半承载式车身是一种介于非承载式车身与承载式车身之间的结构形式，他拥有独立完整的车架，并且车架与车身刚性连接，因此车身壳体可以承受部分载荷。有部分的骨架如单独的支柱，拱形梁，加固件等，它们彼此连接或借蒙皮连接。半承载式车身一般用于大客车。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>悬架</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_131"/>独立前悬架
                            <span class="popup-layout-1">
                            <span class="info">独立前悬架的优点是：质量轻，车身受冲击小，车轮附着力高；可以使发动机位置降低，汽车重心也降低，从而提高汽车稳定性。左右车轮单独跳动，互不相干，能减少车身的倾斜和震动。独立前悬挂目前是家用轿车的主流配置。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_132"/>独立后悬架
                        <span class="popup-layout-1">
                            <span class="info">后轮采用独立悬架，它有以下优点：质量轻，减少了车身受到的冲击，并提高了车轮的地面附着力；可以使发动机位置降低，汽车重心也得到降低，从而提高汽车的行驶稳定性；左右车轮单独跳动，互不相干，能减少车身的倾斜和震动。但是，价格昂贵，多见于高级轿车。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_133"/>非独立前悬架
                            <span class="popup-layout-1">
                            <span class="info">车的前车轮由一根整体式车架相连，车轮连同车桥一起通过弹性悬架系统悬架在车架或车身的下面，结构简单、成本低。但是由于其舒适性及操纵稳定性都相对较差，现代轿车的前轮中使用极少。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_134"/>非独立后悬架
                            <span class="popup-layout-1">
                            <span class="info">车的后车轮由一根整体式车架相连，车轮连同车桥一起通过弹性悬架系统悬架在车架或车身的下面。该悬架结构简单、成本低、强度高、保养容易、但由于其舒适性及操纵稳定性都相对较差，使用该类悬架主要为了控制成本。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_135"/>可调悬架
                            <span class="popup-layout-1">
                            <span class="info">可调悬挂是指其性能可根据路面状况和汽车行驶状态而进行调整的一种悬挂。分为主动悬挂和半主动悬挂两种。全主动悬挂可以根据汽车的运动状态和路面状况，适时地调节悬挂的刚度和阻尼，使其处于最佳减震状态。半主动悬挂不考虑改变悬挂的刚度，而只考虑改变悬挂的阻尼。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_136"/>空气悬架
                            <span class="popup-layout-1">
                            <span class="info">空气悬挂系统是能实现suv舒适性和越野型的兼容，它可以根据路况的不同以及距离传感器的信号，行车电脑会判断出车身高度变化，再控制空气压缩机和排气阀门，使弹簧自动压缩或伸长，从而降低或升高底盘离地间隙，以增加高速车身稳定性或复杂路况的通过性。</span></span>
                        </label></li>
                  <%--  <li><label><input type="checkbox" id="more_137"/>四轮独立悬架 <span class="popup-layout-1">
                            <span class="info">顾名思义，四个轮胎均采用独立悬架，费用较高多见于中级以上的车。</span></span>
                        </label></li>--%>
                    </ul></dd></dl>
                    <dl><dt>差速锁位置</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_138"/>中央差速器锁
                            <span class="popup-layout-1">
                            <span class="info">中央差速器锁是安装在中央差速器上的一种锁止机构，用于四轮驱动车。 其作用是为了提高汽车在坏路面上的通过能力，即当汽车的一个驱动桥空转时，能迅速锁死差速器，使两驱动桥变为刚性联接。这样就可以把大部分的扭矩甚至全部扭矩传给不滑转的驱动桥，充分利用它的附着力而产生足够牵引力，使汽车能够继续行驶。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_139"/>前轴差速器锁
                                <span class="popup-layout-1">
                                <span class="info">前轴差速器锁是安装在前轴上的一种锁止机构，为了提高车辆在坏路面上的通过能力，作用同中央差速器锁，目前市面上只有少数几款车型使用。</span></span>
                            </label></li>
                    <li><label><input type="checkbox" id="more_140"/>后轴差速器锁
                            <span class="popup-layout-1">
                            <span class="info">后轴差速器锁是安装在后轴上的一种锁止机构，为了提高车辆在坏路面上的通过能力，作用同中央差速器锁，目前市面上只有少数几款车型使用。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>前轮制动</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_141"/>盘式
                            <span class="popup-layout-1">
                            <span class="info">盘式刹车是以静止的刹车盘片，夹住随着轮胎转动的刹车碟盘以产生摩擦力，使车轮转动速度降低的刹车装置。它的优点是水稳定性高，适用于高速车。缺点是造价高，需要频繁更换刹车片。是目前主流的制动模式。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_142"/>鼓式
                            <span class="popup-layout-1">
                            <span class="info">鼓式制动器的主流是内张式，它是将制动块(刹车蹄)置于制动轮内侧，在刹车的时候制动块向外张开，摩擦制动轮的内侧，达到刹车的目的。优点是便宜，缺点是制动效能和散热性差，所以不同路况不是很稳定，现在轿车领域使用的已经较少。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_143"/>通风盘
                            <span class="popup-layout-1">
                            <span class="info">通风刹车盘内部是中空的，汽车在行使当中冷空气可以从中间穿过，使空气对流，达到散热的目的。从外表看，它在圆周上有许多通向圆心的洞空，因此要比普通盘式散热效果好许多，但是造价昂贵。很多高级轿车前盘使用通风盘，后盘使用普通盘。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_144"/>实心盘
                            <span class="popup-layout-1">
                            <span class="info">盘式制动的一种，相比于通风盘实心盘不容易散热，但是造价便宜。因为车在制动的过程中前轮制动对整车贡献85%，所以为了控制成本，很多高级轿车的前盘使用通风盘，后盘使用实心盘。</span></span>
                        </label></li>
            <%--        <li><label><input type="checkbox" id="more_145"/>盘鼓结合</label></li>--%>
                    </ul></dd></dl>
                    <dl><dt>后轮制动</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_146"/>盘式
                            <span class="popup-layout-1">
                            <span class="info">盘式刹车是以静止的刹车盘片，夹住随着轮胎转动的刹车碟盘以产生摩擦力，使车轮转动速度降低的刹车装置。它的优点是水稳定性高，适用于高速车。缺点是造价高，需要频繁更换刹车片。是目前主流的制动模式。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_147"/>鼓式
                            <span class="popup-layout-1">
                            <span class="info">鼓式制动器的主流是内张式，它是将制动块(刹车蹄)置于制动轮内侧，在刹车的时候制动块向外张开，摩擦制动轮的内侧，达到刹车的目的。优点是便宜，缺点是制动效能和散热性差，所以不同路况不是很稳定，现在轿车领域使用的已经较少。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_148"/>通风盘
                            <span class="popup-layout-1">
                            <span class="info">通风刹车盘内部是中空的，汽车在行使当中冷空气可以从中间穿过，使空气对流，达到散热的目的。从外表看，它在圆周上有许多通向圆心的洞空，因此要比普通盘式散热效果好许多，但是造价昂贵。很多高级轿车前盘使用通风盘，后盘使用普通盘。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_149"/>实心盘
                            <span class="popup-layout-1">
                            <span class="info">盘式制动的一种，相比于通风盘实心盘不容易散热，但是造价便宜。因为车在制动的过程中前轮制动对整车贡献85%，所以为了控制成本，很多高级轿车的前盘使用通风盘，后盘使用实心盘。</span></span>
                        </label></li>
                    <%--<li><label><input type="checkbox" id="more_150"/>盘鼓结合</label></li>--%>
                    </ul></dd></dl>
                    <dl><dt>手刹类型</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_151"/>手刹
                         <span class="popup-layout-1">
                            <span class="info">手刹的专业称呼是辅助制动器，与制动器的原理不同，其是采用钢丝拉线连接到后制动蹄上，以对车子进行制动。长期使用手刹会使钢丝产生塑性变形，由于这种变形是不可恢复的，所以长期使用会降低效用，手刹的行程也会增加。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_152"/>脚刹
                         <span class="popup-layout-1">
                            <span class="info">就是用脚来操纵的驻车制动，一般多见于自动挡车型。其实它也是手刹的一种。这对很多手劲小的女司机来说，脚刹很适合她们。但是也要注意，不能踩得过猛，时间也不能太长，否则会导致刹车拉线断裂。比较常见的是出现在B级车型上。</span></span>
                        </label></li>
                 <%--   <li><label><input type="checkbox" id="more_153"/>中央拉索式</label></li>--%>
                    <li><label><input type="checkbox" id="more_154"/>电子驻车
                         <span class="popup-layout-1">
                            <span class="info">电子驻车是指由电子控制方式实现停车制动的技术,可以在发动机熄火后自动施加驻车制动。驻车方便、可靠，可防止意外的释放。也可在紧急状态下做制动使用。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_155"/>机械驻车
                         <span class="popup-layout-1">
                            <span class="info">机械驻车制动是驻车制动器是通过机械传动来实现的。手刹或者脚刹都是机械驻车的一种。</span></span>
                        </label></li>
                <%--    <li><label><input type="checkbox" id="more_156"/>强力弹簧驻车</label></li>--%>
                    </ul></dd></dl>
                    <dl><dt>转向助力</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_157"/>电子
                         <span class="popup-layout-1">
                            <span class="info">汽车在转向时，电子转矩传感器会“感觉”到转向盘的力矩和拟转动的方向，这些信号会通过数据总线发给电子控制单元，电控单元会根据传动力矩、拟转的方向等数据信号，向电动机控制器发出动作指令，从而电动机就会根据具体的需要输出相应大小的转动力矩，从而产生了助力转向。不转向时不工作。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_158"/>液压
                         <span class="popup-layout-1">
                            <span class="info">机械式的液压动力转向系统一般由液压泵、油管、压力流量控制阀体、V型传动皮带、储油罐等部件构成。无论是否转向，这套系统都要工作，而且在大转向车速较低时，需要液压泵输出更大的功率以获得比较大的助力。所以，也在一定程度上浪费了资源。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_159"/>电子液压
                         <span class="popup-layout-1">
                            <span class="info">相比于机械液压，电子液压的转向油泵不由发动机驱动，不会消耗发动机动力，它是由电动机来驱动，并且在之前的基础上加装了电控系统，使得转向辅助力的大小不光与转向角度有关，还与车速相关。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_160"/>电子电传
                         <span class="popup-layout-1">
                            <span class="info">电传操纵是航空领域中一种将航空器驾驶员的操纵输入，通过转换器转变为电信号，经计算机或电子控制器处理，再通过电缆传输到执行机构一种操纵系统。它省掉了传统操纵系统中的机械传动装置和液压管路。目前只有极少数车使用该技术。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>变速杆</dt><dd><ul class="tj_list">
                    <li><label><input type="radio" name="gearlever" id="more_1"/>不限</label></li>
                    <li><label><input type="radio" name="gearlever" id="more_161"/>电子挡杆
                         <span class="popup-layout-1">
                            <span class="info">电子挡杆与变速箱的连接并非传统的机械方式，而是采用了更加安全、快捷的电子控制模式。电子档杆的优势就在于驾驶者的换档错误操作会由于电脑判断出是否会对变速器造成损伤，从而更好的保护变速器和纠正驾驶者的不良换档习惯。</span></span>
                        </label></li>
                    <li><label><input type="radio" name="gearlever" id="more_162"/>机械挡杆
                         <span class="popup-layout-1">
                            <span class="info">普通的档杆与变速器的连接方式是机械式的，也就是说你手动变动档杆的位置，档杆下方的档轴是由于你手的作用力在变速器内发生位移的。</span></span>
                        </label></li>
                    <li><label><input type="radio" name="gearlever" id="more_163"/>方向盘拨片
                         <span class="popup-layout-1">
                            <span class="info">换挡拨片是对于汽车半自动离合器所安装的换档设备，作用是不用脚踩离合器，直接通过换档拨片上拨来实现换档。这一装置在赛车中应用广泛。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>挡位个数</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_164"/>3</label></li>
                    <li><label><input type="checkbox" id="more_165"/>4</label></li>
                    <li><label><input type="checkbox" id="more_166"/>5</label></li>
                    <li><label><input type="checkbox" id="more_167"/>6</label></li>
                    <li><label><input type="checkbox" id="more_168"/>7</label></li>
                    <li><label><input type="checkbox" id="more_169"/>8</label></li>
                    <li><label><input type="checkbox" id="more_170"/>9</label></li>
                    </ul></dd></dl>
                    <h6 id="params-safeconfig">安全配置</h6>
                    <dl><dt>气囊位置</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_171"/>驾驶位</label></li>
                    <li><label><input type="checkbox" id="more_172"/>副驾驶位</label></li>
                    <li><label><input type="checkbox" id="more_173"/>前排头部</label></li>
                    <li><label><input type="checkbox" id="more_174"/>前排侧</label></li>
                    <li><label><input type="checkbox" id="more_175"/>后排头部</label></li>
                    <li><label><input type="checkbox" id="more_176"/>后排侧</label></li>
                    </ul></dd></dl>
                    <dl><dt>安全系统</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_177"/>胎压监测
                        <span class="popup-layout-1">
                            <span class="info">它的作用是在汽车行驶过程中对轮胎气压进行实时自动监测，并对轮胎漏气和低气压进行报警，以确保行车安全。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_178"/>零压续行
                         <span class="popup-layout-1">
                            <span class="info">零压续航使用的轮胎一般是防爆轮胎，在遭到刺扎后，不会漏气或者漏气非常缓慢，从而保证汽车能够长时间或者暂时稳定行驶。是一项重要的安全行驶配置，一般零压续航的车子都不再有备胎。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_179"/>无钥匙启动
                         <span class="popup-layout-1">
                            <span class="info">无钥匙启动系统，即启动车辆不用掏拧钥匙，把钥匙放在包内或口袋里，按下车内按键或拧动导板即可使发动机点火。更加便捷，也使豪华感、科技感倍增。但是注意不要和其他电子设备放到一起，易损坏。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_180"/>儿童锁
                         <span class="popup-layout-1">
                            <span class="info">汽车儿童锁又称车门锁儿童保险，设置在汽车的后门锁上，打开后车门在门锁的下方有一小拔杆（保险机构），拨向有儿童图标的那端，再关上车门，此时车门在车内就无法打开，而只能在车外打开。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_181"/>儿童座椅固定
                         <span class="popup-layout-1">
                            <span class="info">有专门固定儿童座椅的接口，一般在车的后排，接入儿童座椅后，可以减小在车辆碰撞的时候对儿童造成的伤害。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <h6 id="params-drivingassistance">行车辅助</h6>
                    <dl><dt>操控辅助</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_182"/>刹车防抱死(ABS)
                         <span class="popup-layout-1">
                            <span class="info">制动防抱死系统（antilock brake system）简称ABS。作用就是在汽车制动时，自动控制制动器制动力的大小，使车轮不被抱死，处于边滚边滑（滑移率在20%左右）的状态，以保证车轮与地面的附着力在最大值。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_183"/>刹车辅助(EBA/BAS/BA/EVA等)
                         <span class="popup-layout-1">
                            <span class="info">指能够通过判断驾驶者的刹车动作（力量及速度），在紧急刹车时增加刹车力度，从而将制动距离缩短。对于像老人或女性这种脚踝及腿部力量不是很足的驾驶者来说，该系统的优势则会表现得更加明显。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_184"/>动态稳定控制系统(ESP/DSC/VSC/ESC)
                         <span class="popup-layout-1">
                            <span class="info">动态稳定系统可在汽车高速运动时，提供良好的操控性，防止车辆发生甩尾或者漂移现象，从而获得精准的操控性。是电子主动安全保护系统的一种。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_185"/>弯道制动控制系统(CBC)
                         <span class="popup-layout-1">
                            <span class="info">弯道制动控制系统（CBC）辅助驾驶员在转向时制动柔和，如果在转弯时刹车，汽车重量的重新分配可能会产生转向过度的现象。CBC通过给汽车另一侧施加制动压力，CBC可以增强汽车的转向时的稳定性，即使驾驶员制动超过了ABS的正常范围也能起到稳定作用。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_186"/>随速助力转向调节(EPS)
                         <span class="popup-layout-1">
                            <span class="info">随着车辆速度的快慢调节转向的轻重，车速越慢方向越轻，车速越快方向越重，这样可以更好的保证驾驶的舒适性与安全性。EPS的构成，不同的车尽管结构部件不一样，但大体是雷同。一般是由转矩(转向)传感器、电子控制单元、电动机、减速器、机械转向器、以及蓄电池电源所构成。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_187"/>电子制动力分配系统(EBD/CBC/EBV)
                         <span class="popup-layout-1">
                            <span class="info">该系统可以在制动时控制制动力在各轮间的分配，更好的利用车轮的附着系数，不仅提高了汽车制动的稳定性和操纵性，而且使各个车轮能够获得更好的制动性能，缩短制动距离，提高安全性。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>电子辅助</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_188"/>上坡辅助
                         <span class="popup-layout-1">
                            <span class="info">车辆在陡峭或光滑坡面上起步时，驾驶员从制动踏板切换至油门踏板车辆将向后下滑，从而导致起步困难。为防止此情况发生，上坡起步辅助控制暂时（最长约2秒）对四个车轮施加制动以阻止车辆下滑。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_189"/>自动泊车
                         <span class="popup-layout-1">
                            <span class="info">自动泊车是指汽车自动泊车入位不需要人工控制，但并不是说以后就能完全自动泊车了，具体还是需要驾驶员自己开启、然后根据提示刹车、挂前进和倒车挡的，这些步骤因各个汽车品牌的设计而不一样。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_190"/>自动驻车
                         <span class="popup-layout-1">
                            <span class="info">自动驻车系统是一种自动替你拉手刹的功能，启动该功能之后，比如在停车等红绿灯的时候，就相当于不用拉手刹了，这个功能特别适应于上下坡以及频繁起步停车的时候。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_191"/>主动安全
                         <span class="popup-layout-1">
                            <span class="info">主动安全系统包括ABS、ESP等电子设备，最为典型的是主动刹车系统。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_192"/>并线辅助
                         <span class="popup-layout-1">
                            <span class="info">并线辅助也可以称为盲区监测，这一装置的形式是在汽车尾部隐藏式的感应器及A柱下方的角灯或者其他方式提醒驾驶者后方有来车。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_193"/>夜视系统
                         <span class="popup-layout-1">
                            <span class="info">夜视系统是一种源自军事用途的汽车驾驶辅助系统。在这个辅助系统的帮助下，驾驶者在夜间或弱光线的驾驶过程中将获得更高的预见能力，它能够针对潜在危险向驾驶者提供更加全面准确的信息或发出早期警告。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_194"/>定速巡航
                         <span class="popup-layout-1">
                            <span class="info">一种定速行驶的系统，其作用是：按司机要求的速度合开关之后，不用踩油门踏板就自动地保持车速，使车辆以固定的速度行驶。采用了这种装置，当在高速公路上长时间行车后，司机就不用再去控制油门踏板，减轻了疲劳，同时减少了不必要的车速变化，可以节省燃料。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_195"/>全景摄像头
                         <span class="popup-layout-1">
                            <span class="info">为了解决倒车影像系统不能全面照顾周围视角的问题，有些厂家开发了全景摄像头。这套系统的核心就在于在车头、车侧增加了多个摄像头，从而能够获取车辆周边的实时影像。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_196"/>GPS导航</label></li>                         
                    <li><label><input type="checkbox" id="more_197"/>自适应巡航
                         <span class="popup-layout-1">
                            <span class="info">在车辆行驶过程中，安装在车辆前部的车距雷达持续扫描车辆前方道路，当与前车之间的距离过小时，车轮会进行适当制动，并使发动机的输出功率下降，以使车辆与前方车辆始终保持安全距离。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_198"/>启停系统
                         <span class="popup-layout-1">
                            <span class="info">当车辆处于停止状态（非驻车状态）时，发动机将暂停工作（而非传统的怠速保持），暂停的同时，发动机内的润滑油会持续运转，使发动机内部保持润滑；当松开制动踏板后，发动机将再次启动，此时，因润滑油一直循环，即使频繁的停车和起步，也不会对发动机内部造成磨损。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_199"/>倒车雷达
                         <span class="popup-layout-1">
                            <span class="info">倒车雷达是汽车驻车或者倒车时的安全辅助装置，能以声音或者更为直观的显示告知驾驶员周围障碍物的情况，解除了驾驶员驻车、倒车和起动车辆时前后左右探视所引起的困扰，并帮助驾驶员扫除了视野死角和视线模糊的缺陷。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_200"/>倒车影像</label></li>                      
                    <li><label><input type="checkbox" id="more_201"/>陡坡缓降
                         <span class="popup-layout-1">
                            <span class="info">使驾驶员能在不踩制动踏板的完全控制情况下，平稳的通过陡峭的下坡坡段。根据需要，制动装置自动控制各车轮，以略快于行走速度向前移动，此时驾驶员可完全专注于控制方向盘。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_202"/>车前泊车雷达</label></li>                       
                    <li><label><input type="checkbox" id="more_203"/>HUD抬头数字
                         <span class="popup-layout-1">
                            <span class="info">抬头数字显示仪(Heads Up Display)，风窗玻璃仪表显示，又叫平视显示系统，它可以把重要的信息，映射在风窗玻璃上的全息半镜上，使驾驶员不必低头，就能看清重要的信息。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_279"/>电子限速
                         <span class="popup-layout-1">
                            <span class="info">电子限速的作用是限制车速过高，防止因车速过高造成事故。电子限速器可以实时监测车辆的速度，当车速达到一定值的时候，它就会控制供油系统和发动机的转速，这时即使踏下油门踏板，供油系统也不会供油。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <h6 id ="params-bodyparts">车身</h6>
                    <dl><dt>车窗</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_204"/>单天窗</label></li>
                    <li><label><input type="checkbox" id="more_205"/>双天窗</label></li>
                    <li><label><input type="checkbox" id="more_206"/>全景天窗</label></li>
                    <li><label><input type="checkbox" id="more_207"/>电动车窗</label></li>
                    <li><label><input type="checkbox" id="more_209"/>车窗防夹</label></li>
                    <li><label><input type="checkbox" id="more_260"/>隔热玻璃
                           <span class="popup-layout-1">
                            <span class="info">车窗采用吸热玻璃可以减少车辆热负荷，降低能耗，减少空调的工作时间，延长空调寿命，而且能见度并不会降低。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>后视镜</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_210"/>记忆功能
                         <span class="popup-layout-1">
                            <span class="info">后视镜的镜面调节设计与驾驶员座椅、方向盘、后视镜构成一个系统，每个驾驶员可根据个人身高与驾驶习惯的不同来调节后视镜的最佳视角，座椅、方向盘最佳舒适性，然后进行记忆储存。在被调整后可以通过调取存储立即恢复原来的设置。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_211"/>加热功能
                         <span class="popup-layout-1">
                            <span class="info">后视镜加热功能是指当汽车在雨、雪、雾等天气行驶时，后视镜可以通过镶嵌于镜片后的电热丝加热，确保镜片表面清晰。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_212"/>电动折叠
                           <span class="popup-layout-1">
                            <span class="info">后视镜电动折叠是指汽车两侧的后视镜在必要时可以折叠收缩起来。这种功能在城市路边停车时特别有用，后视镜折叠后能节省很大的空间，同时也可避免自己的爱车受“断耳”之痛。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_213"/>电动调节
                           <span class="popup-layout-1">
                            <span class="info">后视镜电动调节是指车外两侧的后视镜，在需要调节视角时驾驶员可以不必下车，而在车内通过电动按钮就可以调节。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_214"/>内后视镜防眩目
                         <span class="popup-layout-1">
                            <span class="info">在内后视镜镜面后面安装了光敏二极管，二极管感应到强光时控制电路将施加电压到镜面的电离层上，在电压的作用下镜片就会变暗以达到防眩目的目的。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>雨刷器</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_215"/>雨量传感
                         <span class="popup-layout-1">
                            <span class="info">雨量传感器暗藏在前风挡玻璃后面，它能根据落在玻璃上雨水量的大小来调整雨刷的动作，因而大大减少了开车人的烦恼。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_216"/>车速传感
                         <span class="popup-layout-1">
                            <span class="info">车速传感器是用于检测车子速度 ， 位置在差速器附近轮速传感器，用于ABS等辅助作用的， 位置在车轮里面转速传感器，俗称曲轴位置传感器，用于启动汽车，判断活塞止点位置，没有它一般汽车发动不了， 位置在飞轮处。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_217"/>雨量及车速传感
                         <span class="popup-layout-1">
                            <span class="info">兼具雨量传感器和车速传感器。</span></span>
                        </label></li>
                        <li><label><input type="checkbox" id="more_282"/>其他感应雨刷
                         <span class="popup-layout-1">
                            <span class="info">感应雨刷能通过雨量传感器感应雨滴的大小，自动调节雨刷运行速度，为驾驶者提供良好的视野，从而大大提高雨天驾驶的方便性和安全性。</span></span>
                        </label></li>
                    </ul></dd></dl>
   
			        <dl><dt>车顶</dt>
				    <dd>
				        <ul class="tj_list">
					        <li><label><input type="radio" name="roof" id="more_2""/>不限</label></li>
					        <li><label><input type="radio" name="roof" id="more_219"/>硬顶</label></li>
					        <li class="ico-arrow"><label><input type="radio" name="roof" id="more_220"/>敞篷</label><i class="sanjiao"></i></li>
					    </ul>
					    <div class="clear"></div>
					    <div class="openlayer">
						    <ul>
							    <li><label><input type="checkbox" id="more_221"/>软顶敞篷</label></li>
							    <li><label><input type="checkbox" id="more_222"/>硬顶敞篷</label></li>
<%--							    <li><label><input type="checkbox" id="more_223"/>软硬双顶敞篷</label></li>--%>
						    </ul>
                            <div class="clear"></div>
					    </div>
				    </dd>
			        </dl>
                    <h6 id="params-lights">灯光</h6>
                    <dl><dt>前照灯类型</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_224"/>氙气大灯
                        <span class="popup-layout-1">
                            <span class="info">在灯管里填充氙气等惰性气体，通过23000伏高压电离氙气产生光源。氙灯的性能较卤素灯有了显著提升，它的光通量是卤素灯的2倍以上，电能转化为光能的效率也比卤素灯提高了70%以上。寿命长，耗电少，色温高。但是反应速度慢，用于近光灯较多。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_225"/>卤素大灯
                        <span class="popup-layout-1">
                            <span class="info">卤素大灯是新一代白炽灯，充有溴碘等卤族元素或卤化物的钨灯。光照强度没有氙气大灯强，但是反应速度快，常用于远光灯。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_226"/>LED大灯
                        <span class="popup-layout-1">
                            <span class="info">LED大灯有亮度高、寿命长、发光单位小、造型丰富等优点。最大的优点就是漂亮、漂亮、漂亮。但是维修昂贵，坏了任何一个小灯，得整个更换。LED大灯多见于普通轿车的高配版本和高级轿车。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>前大灯</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_227"/>自动开闭
                        <span class="popup-layout-1">
                            <span class="info">前大灯到了光线较暗的地方或者天黑的时候自己就开了，到了较亮的地方又自动关闭。灯光有个AUTO的位置，它是根据在前仪表台靠右侧对这前挡玻璃的一个感光器控制的。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_228"/>随动转向
                        <span class="popup-layout-1">
                            <span class="info">也被称为自适应大灯，它能够不断对大灯进行动态调节，保持与汽车的当前行驶方向一致，以确保驾驶员在任何时刻都拥有最佳的可见度，夜晚行车，弯道容易出现“盲区”，此功能会提升驾驶安全性。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <dl><dt>其他灯光</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_229"/>车内氛围灯
                        <span class="popup-layout-1">
                            <span class="info">车内氛围灯是一种起到装饰作用的照明灯，通常是红色、蓝色、绿色等，主要为了使车厢在夜晚时更加绚丽。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_230"/>日间行车灯
                        <span class="popup-layout-1">
                            <span class="info">日间行车灯是指使车辆在白天行驶时更容易被识别的灯具，装在车身前部。也就是说这个灯具不是照明灯，不是为了使驾驶员能看清路面，而是为了让别人知道有一辆车开过来了，是属于信号灯的范畴。</span></span>
                        </label></li>
                    </ul></dd></dl>
                    <h6 id ="params-innerconfig">内部配置</h6>
                    <dl><dt>方向盘</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_231"/>多功能方向盘
                        <span class="popup-layout-1">
                            <span class="info">多功能方向盘是指在方向盘两侧或者下方设置一些功能键，让驾驶员更方便操作的方向盘。多功能方向盘，空调调节，车载电话等等，还有的将定速巡航键也设置在方向盘上。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_232"/>真皮方向盘</label></li>
                    </ul></dd></dl>
                    <dl><dt>座椅功能</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_233"/>座椅加热</label></li>
                    <li><label><input type="checkbox" id="more_234"/>座椅按摩功能</label></li>
                    <li><label><input type="checkbox" id="more_235"/>电动座椅记忆</label></li>
                    <li><label><input type="checkbox" id="more_236"/>电动座椅调节</label></li>
                    <li><label><input type="checkbox" id="more_237"/>驾驶座椅通风</label></li>
                    <li><label><input type="checkbox" id="more_238"/>前排座椅通风</label></li>
                    <li><label><input type="checkbox" id="more_239"/>后排座椅通风</label></li>
                    </ul></dd></dl>
                    <dl id="anchorTarget"><dt>娱乐通讯</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_240"/>蓝牙系统</label></li>
                    <li><label><input type="checkbox" id="more_241"/>AUX音源接口</label></li>
                    <li><label><input type="checkbox" id="more_242"/>USB音源接口</label></li>
                    <li><label><input type="checkbox" id="more_243"/>中控液晶屏</label></li>
                    <li><label><input type="checkbox" id="more_244"/>220V电压电源</label></li>
                    </ul></dd></dl>
                    <dl><dt>空气调节</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_245"/>空调</label></li>
                    <li><label><input type="checkbox" id="more_246"/>温度分区</label></li>
                    <li><label><input type="checkbox" id="more_247"/>后排独立空调</label></li>
                    <li><label><input type="checkbox" id="more_248"/>后排出风口</label></li>
                    <li><label><input type="checkbox" id="more_249"/>空气净化（PM2.5过滤）</label></li>
                    </ul></dd></dl>
                    <dl><dt>座椅材料</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_250"/>真皮</label></li>
                    <li><label><input type="checkbox" id="more_251"/>皮革</label></li>
                    <li><label><input type="checkbox" id="more_252"/>织物</label></li>
                    <li><label><input type="checkbox" id="more_253"/>真皮+织物</label></li>
                    <li><label><input type="checkbox" id="more_254"/>皮革+织物</label></li>
                    <li><label><input type="checkbox" id="more_281"/>真皮+皮革</label></li>
                    </ul></dd></dl>
                    <dl><dt>其他</dt><dd><ul class="tj_list">
                    <li><label><input type="checkbox" id="more_255"/>运动座椅
                        <span class="popup-layout-1">
                            <span class="info">运动座椅是指将座椅的椅背及椅垫这两个部分加以强化，使人体两侧的腰部/肩部以及背部能够有良好的侧向支撑性，在过弯时能够更准确的感受到轮胎与路面的抓地性，使驾驶人员能够对车辆进行精确的操控。</span></span>
                        </label></li>
                    <li><label><input type="checkbox" id="more_256"/>车顶行李箱架</label></li>
                    </ul></dd></dl>
                </div>
                <div style="position: fixed; left: 200px;" id="left-nav" class="left-nav left-nav-duibi">
                    <!--浮层宽度85px；此处left值是变化的，需要计算；-->
                    <ul>
                        <li class="current"><a href="javascript:;" data-target="params-carinfo">基本信息</a></li>
                        <li class=""><a href="javascript:;" data-target="params-carengine">发动机</a></li>
                        <li class=""><a href="javascript:;" data-target="params-bottomstop">底盘制动</a></li>
                        <li class=""><a href="javascript:;" data-target="params-safeconfig">安全配置</a></li>
                        <li class=""><a href="javascript:;" data-target="params-drivingassistance">行车辅助</a></li>
                        <li class=""><a href="javascript:;" data-target="params-bodyparts">车身</a></li>
                        <li class=""><a href="javascript:;" data-target="params-lights">灯光</a></li>
                        <li class=""><a href="javascript:;" data-target="params-innerconfig">内部配置</a></li>
                    </ul>
                    <a style="display: none;" id="close-left-nav" class="close-left-nav" href="javascript:;">关闭浮层</a>
                </div>
            </div>
            <div class="cartype-list" id="params-styleList">
                <div class="section-header header2 h-default2">
                    <div class="box">
                        <h2>车型列表</h2>
                        <span id="styleCount" class="header-note1"></span>
                    </div>
                    <div class="more">
                        <%=sortModeHtml %>
                    </div>
                </div>

                <div class="row block-5col-180" id="divContent">
                </div>
                <div class="row">
                    <div class="pagination">
                        <div id="divPage">
                        </div>
                    </div>
                </div>
            </div>
            <div id="noResult" style="display: none;">
                <div class="section-header header2 h-default2">
                    <div class="box">
                        <h2>车型列表</h2>
                    </div>
                </div>
                <div class="note-box note-empty type-2"  style="margin-top: 30px; margin-bottom: 50px; ">
                    <div class="ico"></div>
                    <div class="info">
                        <h3>抱歉，未找到合适的车型</h3>
                        <p class="tip">请修改条件再次查询，或者去 <a href="http://www.taoche.com" target="_blank">易车二手车</a> 看看</p>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery.min.js"></script>
     
     <%--   <script type="text/javascript" src="/jsnewV2/jquery.pagination.js"></script>--%>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/jquery.pagination.min.js?v=20161215"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnewV2/superselectcartool.min.js?v=20170329"></script>
    <%--<script type="text/javascript" src="/jsnewV2/superselectcartool.js"></script>--%>
    <script type="text/javascript">
        SuperSelectCarTool.Parameters = <%=configParaHtml%>;
        SuperSelectCarTool.initPageCondition();
    </script>
    <!--#include file="/include/special/stat/00001/bglogpostlog_Manual.shtml"-->
    <!--#include file="~/htmlv2/rightbar.shtml"-->
    <!--#include file="~/htmlV2/footer2016.shtml"-->
</body>
</html>

