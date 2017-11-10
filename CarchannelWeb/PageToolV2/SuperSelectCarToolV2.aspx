<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuperSelectCarToolV2.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.PageToolV2.SuperSelectCarToolV2" %>

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
    <meta name="Description" content="易车网高级选车工具频道为您提供按照各种条件查询车型功能，包括按汽车价格、参数配置、汽车级别、国产进口、变速方式、汽车排量等，如何挑选一款符合您心意的好车，易车网高级选车工具帮您解决。" />
    <!--#include file="~/ushtml/0000/yiche_2016_cube_gaojisousuo_style-1265.shtml"-->
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
                    <input type="text" class="input" placeholder="快速搜配置，如：自动泊车" name="txtsearch" value="" maxlength="20" />
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
                    <input type="text" class="input" placeholder="快速搜配置，如：自动泊车" name="txtsearch" value="" maxlength="20" />
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
                                            <input id="p_max" onkeyup="value=value.replace(/(\D|\d{5})/g,'')" maxlength="4" class="inputborder input36" />万
                                    </div>
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
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="l_2" />小型车 
                                        <span class="popup-layout-1">
                                            <span class="info">A0级车，大多数轴距在2.2米至2.5米之间，排量多在1-1.3L之间。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="l_3" />紧凑型车
                                         <span class="popup-layout-1">
                                             <span class="info">A级车，是最常见的家用型车，轴距一般在2.5-2.7米之间，发动机排量一般1.6-2.0L左右，国内市场主流车型。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="l_5" />中型车
                                        <span class="popup-layout-1">
                                            <span class="info">即B级车，轴距一般为2.6-2.8米，车身4.6-4.9米，排量通常在2.0-3.0L之间，一般在内部空间、配置、舒适性等方面都要比紧凑型车高一个档次，更加适合于商务用途。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="l_4" />中大型车     
                                        <span class="popup-layout-1">
                                            <span class="info">中大型车即C级车，也被成为“行政级”车型，轴距普遍都超过2.8米，发动机排量通常在2.5L以上。在内部空间及配置上都要比中型车更加出色。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="l_6" />豪华车
                                         <span class="popup-layout-1">
                                             <span class="info">D级车，车长度普遍在5米或5米以上，轴距基本上在3米左右。一般6缸以上，有非常好的安全设施，内部配置非常高，乘坐舒适感强。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="l_7" />MPV
                                        <span class="popup-layout-1">
                                            <span class="info">MPV是从旅行轿车逐渐演变而来的，它集旅行车宽大乘员空间、轿车的舒适性、和厢式货车的功能于一身，一般为两厢式结构，即多用途车。通俗地说，就是可以坐7-8人的小客车。</span></span>
                                </label>
                            </li>
                            <li class="ico-arrow">
                                <label>
                                    <input type="checkbox" id="l_8" />SUV
                                        <span class="popup-layout-1">
                                            <span class="info">又叫运动型多用途汽车，是一种拥有旅行车般的空间机能，配以货卡车的越野能力的车型。一般前悬架是轿车型的独立悬架，后悬架是非独立悬架，离地间隙较大，在一定程度上既有轿车的舒适性又有越野车的越野性能。</span></span>
                                </label>
                                <i class="sanjiao"></i></li>
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
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_13" />小型SUV
                                        <span class="popup-layout-1">
                                            <span class="info">通常指长度小于4米的SUV。</span></span>
                                    </label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_14" />紧凑型SUV
                                         <span class="popup-layout-1">
                                             <span class="info">通常指长度在4-4.3米的SUV。</span></span>
                                    </label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_15" />中型SUV 
                                        <span class="popup-layout-1">
                                            <span class="info">通常指长度在4.3-4.6米的SUV。</span></span>
                                    </label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_16" />中大型SUV
                                    <span class="popup-layout-1">
                                        <span class="info">通常指长度在4.6-5米的SUV。</span></span>
                                    </label>
                                </li>
                                <li>
                                    <label>
                                        <input type="checkbox" id="l_17" />全尺寸SUV
                                        <span class="popup-layout-1">
                                            <span class="info">通常指长度在大于5米的SUV。</span></span>
                                    </label>
                                </li>
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
                                        <input id="d_max" maxlength="4" class="inputborder input36" />L
                                </div>
                                <a id="btnDisSubmit" class="btn btn-primary2 btn-sm pull-left" href="javascript:;">确定</a>
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
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="t_32" />机械自动式
                                        <span class="popup-layout-1">
                                            <span class="info">介于手动与自动之间的变速器，它不同于手动变速器传统的“H”型挡位结构，其换挡手柄只能前后移动进行升挡或降挡。目前市面上半自动变速器的车型比较少。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="t_2" />自动
                                        <span class="popup-layout-1">
                                            <span class="info">顾名思义就是不用驾驶者去手动换挡，车辆会根据行驶的速度和交通情况自动选择合适的挡位行驶。驾驶时把档位放到前进挡，直接控制油门和刹车即可，但是自动挡不能空挡滑行。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="t_4" />手自一体
                                        <span class="popup-layout-1">
                                            <span class="info">“手自一体”就是将汽车的手动换挡和自动换挡结合在一起的变速方式。手动挡因为自己可以自由调节挡位及转速，驾驶起来有种畅快的感觉，运动感十足。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="t_8" />无极变速
                                        <span class="popup-layout-1">
                                            <span class="info">自动挡的一种，无级变速指可以连续获得变速范围内任何传动比的变速系统。通过无级变速可以得到传动系与发动机工况的最佳匹配，无级变速的加速过程十分流畅，没有换挡的顿挫感。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="t_16" />双离合
                                        <span class="popup-layout-1">
                                            <span class="info">双离合变速器有别于一般的自动变速器系统，它既属于手动变速器而又属于自动变速器，除了拥有手动变速器的灵活性及自动变速器的舒适性外，还能提供无间断的动力输出。而传统的手动变速器使用一台离合器，当换挡时，驾驶员须踩下离合器踏板，使不同挡的齿轮做出啮合动作，而动力就在换挡期间出现间断，令输出表现有所断续。</span></span>
                                </label>
                            </li>
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
                                    <input type="checkbox" id="f_4" />插电混合</label></li>
                            <%-- <li>
                                    <label>
                                        <input type="checkbox" id="f_32" />LPG</label></li>--%>
                            <li>
                                <label>
                                    <input type="checkbox" id="f_32" />天然气
                                        <span class="popup-layout-1">
                                            <span class="info">燃料的一种，CNG(Compressed Natural Gas),即压缩天然气，是天然气加压(超过3,600磅/平方英寸)并以气态储存在容器中。</span></span>
                                </label>
                            </li>
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
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="radio" name="body" id="b_2" />三厢
                                        <span class="popup-layout-1">
                                            <span class="info">我们一般常见的轿车都是三厢车，三厢是指前部的发动机舱、车身中部的乘员舱和后部的行李舱。</span></span>
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="radio" name="body" id="lv_1" />旅行版
                                        <span class="popup-layout-1">
                                            <span class="info">在英语中，旅行车称为“wagon”，大多数旅行车都是以轿车为基础，把轿车的后备厢加高到与车顶齐平，用来增加行李空间。Wagon的魅力在于它既有轿车的舒适，也有相当大的行李空间，外形也相当的稳重，有成熟的魅力。</span></span>
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>

                <dl>
                    <dt>座位数</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_279" />2座</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_280" />4座</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_281" />5座</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_282" />6座</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_283" />7座</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_284" />7座以上</label></li>
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
                <h6 id="params-carengine">动力系统</h6>
                <dl>
                    <dt>进气形式</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="radio" name="airadmission" id="more_0" />不限</label></li>
                            <li>
                                <label>
                                    <input type="radio" name="airadmission" id="more_100" />自然吸气</label></li>
                            <li>
                                <label>
                                    <input type="radio" name="airadmission" id="more_101" />涡轮增压</label></li>
                            <li>
                                <label>
                                    <input type="radio" name="airadmission" id="more_102" />机械增压</label></li>
                            <li>
                                <label>
                                    <input type="radio" name="airadmission" id="more_103" />双增压</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>供油方式</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_104" />多点电喷
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_105" />直喷
                                </label>
                            </li>

                            <li>
                                <label>
                                    <input type="checkbox" id="more_106" />单点电喷</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_107" />化油器
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>气缸排列</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_108" />直列
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_109" />V型</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_110" />W型
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_111" />水平对置
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_112" />转子
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>气缸数</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_113" />2缸 
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_114" />3缸 
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_115" />4缸 
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_116" />5缸
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_117" />6缸
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_118" />8缸
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_119" />8缸以上
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>环保标准</dt>
                    <dd>
                        <ul class="tj_list">

                            <li>
                                <label>
                                    <input type="checkbox" id="more_120" />国5
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_121" />国4
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_122" />国3
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>挡位个数</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_123" />1</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_124" />4</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_125" />5</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_126" />6</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_127" />7</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_128" />8</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_129" />9</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_130" />10</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>燃油标号</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_131" />92</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_132" />95</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_133" />98</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_134" />柴油</label></li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-bottomstop">底盘制动</h6>
                <dl>
                    <dt>底盘结构</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_135" />承载式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_136" />非承载式
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>前悬架类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_137" />扭力梁式非独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_138" />拖曳臂式半独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_139" />双叉臂式独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_140" />多连杆式独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_141" />麦弗逊独立悬架
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>后悬架类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_142" />扭力梁式非独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_143" />拖曳臂式半独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_144" />双叉臂式独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_145" />多连杆式独立悬架
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_146" />麦弗逊独立悬架
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>前轮制动类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_147" />鼓式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_148" />盘式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_149" />通风盘
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_150" />碳纤维陶瓷
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>后轮制动类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_151" />鼓式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_152" />盘式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_153" />通风盘
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_154" />碳纤维陶瓷
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>驱车制动类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_155" />手拉式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_156" />脚踩式
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_157" />电子式
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>差速器/差速锁</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_158" />前桥
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_159" />后桥
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_160" />中央
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>驱动</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_161" />前轮驱动
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_162" />后轮驱动
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_163" />全时四驱
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_164" />分时四驱
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_165" />适时四驱</label></li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-safeconfig">安全配置</h6>
                <dl>
                    <dt>安全辅助系统</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_166" />防抱死制动(ABS)
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_167" />制动力分配(EBD/CBC等)
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_168" />制动辅助(BA/EBA/BAS等)
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_169" />牵引力控制(ARS/TCS/TRC等)
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_170" />车身稳定控制(ESP/DSC/VSC等) 
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>气囊位置</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_171" />主驾驶气囊</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_172" />副驾驶气囊</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_173" />前侧气囊</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_174" />后侧气囊</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_175" />侧安全气帘</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_176" />膝部气囊</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_177" />安全带气囊</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_178" />后排中央气囊</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>安全配置</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_179" />胎压监测</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_180" />零压续行轮胎</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_181" />儿童座椅接口</label></li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-drivingassistance">驾驶辅助</h6>
                <dl>
                    <dt>行车辅助</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_182" />定速巡航
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_183" />ACC自适应巡航
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_184" />
                                    主动刹车
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_185" />并线辅助
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_186" />疲劳提醒
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_187" />自动驾驶辅助
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_188" />夜视系统
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_189" />
                                    陡坡缓降
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_190" />
                                    驾驶模式选择
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>停车辅助</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_191" />自动泊车
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_192" />自动驻车
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_193" />遥控泊车
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_194" />上坡辅助
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_195" />车前倒车雷达
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_196" />车后倒车雷达
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_197" />倒车影像
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_198" />360全景影像
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-bodyparts">外部配置</h6>
                <dl>
                    <dt>前大灯类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_199" />卤素</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_200" />氙灯</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_201" />LED</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_202" />激光</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>自动大灯</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_203" />自动开闭</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_204" />自动远近光</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_205" />自动转向</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_206" />转向辅助灯</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>天窗类型</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_207" />单天窗</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_208" />双天窗</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_209" />全景天窗</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>后视镜</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_210" />后视镜电动调节
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_211" />后视镜电动折叠
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_212" />后视镜电动记忆
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_213" />外后视镜加热
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_214" />外后视镜防炫目
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_215" />内后视镜防炫目
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>防晒相关</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_216" />隐私玻璃</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_217" />后排侧遮阳帘</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_218" />后遮阳帘</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>雨刷器</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_219" />前感应雨刷
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_220" />后感应雨刷
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_221" />后雨刷
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>外观</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_222" />运动外观套件
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_223" />扰流板
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_224" />车顶行李架
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>智能钥匙</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="radio" name="roof" id="more_225" />无钥匙进入</label></li>
                            <li>
                                <label>
                                    <input type="radio" name="roof" id="more_226" />无钥匙启动</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>车门</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="radio" name="roof" id="more_227" />电动侧滑门</label></li>
                            <li>                                             
                                <label>                                      
                                    <input type="radio" name="roof" id="more_228" />电吸门</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>行李厢</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="radio" name="roof" id="more_229" />电动开合</label></li>
                            <li>
                                <label>
                                    <input type="radio" name="roof" id="more_230" />感应开合</label></li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-innerconfig">内部配置</h6>
                <dl>
                    <dt>内部配置</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_231" />车内氛围灯</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_232" />遮阳板化妆镜</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_233" />香氛系统</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_234" />车载冰箱</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_235" />空气净化</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>方向盘</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_236" />多功能方向盘
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_237" />上下调整
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_238" />左右调整
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_239" />电动上下左右调整
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_240" />记忆
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_241" />方向盘加热
                                </label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_242" />换挡拨片
                                </label>
                            </li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>前排空调</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_243" />手动空调</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_244" />自动空调</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_245" />双温区自动空调</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>后排空调</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_246" />手动空调</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_247" />自动空调</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_248" />双温区自动空调</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>内饰材质</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_249" />塑料</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_250" />皮质</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_251" />木质</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_252" />金属</label></li>
                              <li>
                                <label>
                                    <input type="checkbox" id="more_253" />碳纤维</label></li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-seatconfig">座椅配置</h6>
                <dl>
                    <dt>座椅材质</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_254" />织物</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_255" />皮质</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_256" />织物皮质混合</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>主驾驶位座椅</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_257" />电动调节</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_258" />记忆</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_259" />靠背</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_260" />高低</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_261" />腰部</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_262" />肩部</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_263" />腿托</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>副驾驶位座椅</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_264" />电动调节</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_265" />记忆</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_266" />靠背</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_267" />腰部</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_268" />腰部</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_269" />肩部</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_270" />腿托</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>座椅放倒方式</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_271" />全部放倒</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_272" />按比例放倒</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>前排座椅功能</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_273" />加热</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_274" />通风</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_275" />按摩</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>后排座椅功能</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_276" />加热</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_277" />通风</label></li>
                             <li>
                                <label>
                                    <input type="checkbox" id="more_278" />按摩</label></li>
                        </ul>
                    </dd>
                </dl>
                <h6 id="params-pastime">娱乐通讯</h6>
                <dl id="anchorTarget">
                    <dt>驾驶辅助</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_285" />全液晶仪表盘</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_286" />中控液晶屏</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_287" />gps</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_288" />行车电脑</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_289" />hud平视</label></li>
                            <li>                                    
                                <label>                             
                                    <input type="checkbox" id="more_290" />智能互联定位</label></li>
                        </ul>
                    </dd>
                </dl>
                <dl>
                    <dt>连接</dt>
                    <dd>
                        <ul class="tj_list">
                            <li>
                                <label>
                                    <input type="checkbox" id="more_291" />carplay</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_292" />android</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_293" />手机无线充电</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_294" />蓝牙</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_295" />wifi</label></li>
                            <li>
                                <label>
                                    <input type="checkbox" id="more_296" />220v电源</label></li>
                        </ul>
                    </dd>
                </dl>
            </div>
            <div style="position: fixed; left: 200px;" id="left-nav" class="left-nav left-nav-duibi">
                <!--浮层宽度85px；此处left值是变化的，需要计算；-->
                <ul>
                    <li class="current"><a href="javascript:;" data-target="params-carinfo">基本信息</a></li>
                    <li class=""><a href="javascript:;" data-target="params-carengine">动力系统</a></li>
                    <li class=""><a href="javascript:;" data-target="params-bottomstop">底盘制动</a></li>
                    <li class=""><a href="javascript:;" data-target="params-safeconfig">安全配置</a></li>
                    <li class=""><a href="javascript:;" data-target="params-drivingassistance">驾驶辅助</a></li>
                    <li class=""><a href="javascript:;" data-target="params-bodyparts">外部配置</a></li>
                    <li class=""><a href="javascript:;" data-target="params-innerconfig">内部配置</a></li>
                    <li class=""><a href="javascript:;" data-target="params-seatconfig">座椅配置</a></li>
                    <li class=""><a href="javascript:;" data-target="params-pastime">娱乐通讯</a></li>
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
            <div class="note-box note-empty type-2" style="margin-top: 30px; margin-bottom: 50px;">
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

