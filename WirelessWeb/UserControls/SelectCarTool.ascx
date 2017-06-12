<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectCarTool.ascx.cs" Inherits="WirelessWeb.UserControls.SelectCarTool" %>
<%--<script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>在调用页面引用1.8.0版本，省略此处--%>
<script src="http://image.bitautoimg.com/carchannel/WirelessJs/selectcartoolV2.min.js?v=201703091610" type="text/javascript"></script>
<%--<script src="/js/selectcartoolV2.js"></script>--%>

<!-- 筛选 start -->
<div id="m-btn-filter">
    <div class="sort sort4 sort-bg-white sort-pop">
        <ul>
            <li id="btnPrice" data-action="price" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/jiage.onclick','WT.ti', '价格')">
                <a>
                    <span>价格</span>
                    <i></i>
                </a>
            </li>
            <li id="btnLevel" data-action="level" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/jibie.onclick','WT.ti', '级别')">
                <a>
                    <span>级别</span>
                    <i></i>
                </a>
            </li>
            <li id="btnBodyform" data-action="bodyform" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/cheshen.onclick','WT.ti', '车身')">
                <a>
                    <span>车身</span>
                    <i></i>
                </a>
            </li>
            <li id="btnBrandType" data-action="brandtype" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/guobie.onclick','WT.ti', '国别')">
                <a>  
                    <span>国别</span>
                    <i></i>
                </a>
            </li>
            <li id="btnDisplacement" data-action="dis" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/pailiang.onclick','WT.ti', '排量')">
                <a>
                    <span>排量</span>
                    <i></i>
                </a>
            </li>
            <li id="btnTransmisstionType" data-action="trans" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/biansuxiang.onclick','WT.ti', '变速箱')">
                <a>
                    <span>变速箱</span>
                    <i></i>
                </a>
            </li>
            <li id="btnCondition" data-action="condition" data-actionflag="1" class="m-btn" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/peizhi.onclick','WT.ti', '配置')">
                <a>
                    <span>配置</span>
                    <i></i>
                </a>
            </li>
            <li id="btnMore" data-action="more" class="m-btn" data-actionflag="1" onclick="javascript:dcsMultiTrack('DCS.dcsuri', '/car/onclick/gengduo.onclick','WT.ti', '更多')">
                <a>
                    <span>更多</span>
                    <i></i>
                </a>
            </li>
        </ul>
    </div>
    <div class="wrap">
        <div class="sort-clear">
            <a href="/xuanchegongju" class="btn-clear">清除条件</a>
            <a href="/brandlist.html" class="btn-jump">品牌选车&gt;</a>
        </div>
    </div>
</div>
<!-- 筛选 end -->

<div id="m-filter-mask" class="leftmask leftmask3" style="display: none;"></div>
<div id="m-filter-price-leftPopup" class="leftPopup price" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-price">
        <ul class="first-list">
            <li id="price0"><a href="<%=this.GetSearchQueryString("p","") %>">不限</a></li>
            <li id="price1"><a href="<%=this.GetSearchQueryString("p","0-5") %>">5万以下</a></li>
            <li id="price2"><a href="<%=this.GetSearchQueryString("p","5-8") %>">5-8万</a></li>
            <li id="price3"><a href="<%=this.GetSearchQueryString("p","8-12") %>">8-12万</a></li>
            <li id="price4"><a href="<%=this.GetSearchQueryString("p","12-18") %>">12-18万</a></li>
            <li id="price5"><a href="<%=this.GetSearchQueryString("p","18-25") %>">18-25万</a></li>
            <li id="price6"><a href="<%=this.GetSearchQueryString("p","25-40") %>">25-40万</a></li>
            <li id="price7"><a href="<%=this.GetSearchQueryString("p","40-80") %>">40-80万</a></li>
            <li id="price8"><a href="<%=this.GetSearchQueryString("p","80-9999") %>">80万以上</a></li>
            <li id="price9" class="filter">
                <div>
                    <input id="p_min" type="number" placeholder="最低" /><b>-</b><input id="p_max" type="number" placeholder="最高" /><b>万</b><input id="btnPriceSubmit" type="button" value="确定" class="btn-go" />
                </div>
                <em id="p_alert" class="alert"></em>
            </li>
        </ul>
    </div>
</div>
<!-- popup end -->
<!-- 级别 popup start -->
<div id="m-filter-level-leftPopup" class="leftPopup level" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft swipeLeft-sub" id="m-filter-level">
        <div class="ap">
            <ul class="first-list rp">
               <li id="level0" class="level0"><a href="<%=this.GetSearchQueryString("l","0") %>">不限</a></li>
               <li id="level63" class="level63"><a href="<%=this.GetSearchQueryString("l","63") %>">轿车</a></li>
            </ul>
            <ul class="second-list ">
                <li class="level63"><a href="<%=this.GetSearchQueryString("l","63") %>" class="cSubItem">全部轿车</a></li>
                <li id="level1" class="level1"><a href="<%=this.GetSearchQueryString("l","1") %>" class="cSubItem">微型车</a></li>
                <li id="level2" class="level2"><a href="<%=this.GetSearchQueryString("l","2") %>" class="cSubItem">小型车</a></li>
                <li id="level3" class="level3"><a href="<%=this.GetSearchQueryString("l","3") %>" class="cSubItem">紧凑型车</a></li>
                <li id="level5" class="level5"><a href="<%=this.GetSearchQueryString("l","5") %>" class="cSubItem">中型车</a></li>
                <li id="level4" class="level4"><a href="<%=this.GetSearchQueryString("l","4") %>" class="cSubItem">中大型车</a></li>
                <li id="level6" class="level6"><a href="<%=this.GetSearchQueryString("l","6") %>" class="cSubItem">豪华车</a></li>
            </ul>
            <ul class="first-list rp">
                <li class="level8"><a href="<%=this.GetSearchQueryString("l","8") %>">SUV</a></li>
            </ul>
            <ul class="second-list ">
                 <li class="level8"><a href="<%=this.GetSearchQueryString("l","8") %>">全部SUV</a></li>
                <li class="level13"><a href="<%=this.GetSearchQueryString("l","13") %>">小型SUV</a></li>
                <li class="level14"><a href="<%=this.GetSearchQueryString("l","14") %>">紧凑型SUV</a></li>
                <li class="level15"><a href="<%=this.GetSearchQueryString("l","15") %>">中型SUV</a></li>
                <li class="level16"><a href="<%=this.GetSearchQueryString("l","16") %>">中大型SUV</a></li>
                <li class="level17"><a href="<%=this.GetSearchQueryString("l","17") %>">大型SUV</a></li>
            </ul>
            <ul class="first-list rp">
                <li class="level7"><a href="<%=this.GetSearchQueryString("l","7") %>">MPV</a></li>
            </ul>
           
            <ul class="first-list rp">
                <li class="level9"><a href="<%=this.GetSearchQueryString("l","9") %>">跑车</a></li>
            </ul>
           
            <ul class="first-list rp">
                <li class="level11"><a href="<%=this.GetSearchQueryString("l","11") %>">面包车</a></li>
            </ul>
           
            <ul class="first-list rp">
                <li class="level12"><a href="<%=this.GetSearchQueryString("l","12") %>">皮卡</a></li>
            </ul>

            <ul class="first-list rp">
                <li class="level18"><a href="<%=this.GetSearchQueryString("l","18") %>">客车</a></li>
            </ul>
            
        </div>
    </div>
</div>
<%--<div id="m-filter-levelsuv-leftPopup" class="leftPopup sublevel" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-levelsuv">
        <ul class="first-list">
            <li class="root" onclick=" GotoPageSuv('l','-1');"><a>SUV</a></li>
            <li id="level8"><a href="<%=this.GetSearchQueryString("l","8") %>">全部</a></li>
            <li id="level13"><a href="<%=this.GetSearchQueryString("l","13") %>">小型SUV</a></li>
            <li id="level14"><a href="<%=this.GetSearchQueryString("l","14") %>">紧凑型SUV</a></li>
            <li id="level15"><a href="<%=this.GetSearchQueryString("l","15") %>">中型SUV</a></li>
            <li id="level16"><a href="<%=this.GetSearchQueryString("l","16") %>">中大型SUV</a></li>
            <li id="level17"><a href="<%=this.GetSearchQueryString("l","17") %>">大型SUV</a></li>
        </ul>
    </div>
</div>--%>
<!-- popup end -->
<!-- 车身 popup start -->
<div id="m-filter-body-leftPopup" class="leftPopup bodyform" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-body">
        <ul class="first-list">
            <li id="bodyform0"><a href="<%=this.GetSearchQueryString("b","0") %>">不限</a></li>
            <li id="bodyform1"><a href="<%=this.GetSearchQueryString("b","1") %>">两厢</a></li>
            <li id="bodyform2"><a href="<%=this.GetSearchQueryString("b","2") %>">三厢</a></li>
            <li id="wagon1"><a href="<%=this.GetSearchQueryString("lv","1") %>">旅行版</a></li>
        </ul>
    </div>
</div>
<!--国别 popup start -->
<div id="m-filter-brandtype-leftPopup" class="leftPopup brandtype" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-brandtype">
        <div class="ap">
            <ul class="first-list rp">
              <%--  <li id="brandType0"><a href="<%=this.GetSearchQueryString("g","0") %>">不限</a></li>
                <li id="brandType1"><a href="<%=this.GetSearchQueryString("g","1") %>">自主</a></li>
                <li id="brandType2"><a href="<%=this.GetSearchQueryString("g","2") %>">合资</a></li>
                <li id="brandType4"><a href="<%=this.GetSearchQueryString("g","4") %>">进口</a></li>
                <li id="brandType8"><a href="<%=this.GetSearchQueryString("g","8") %>">德系</a></li>
                <li id="brandType12"><a href="<%=this.GetSearchQueryString("g","12") %>">日系</a></li>
                <li id="brandType16"><a href="<%=this.GetSearchQueryString("g","16") %>">韩系</a></li>
                <li id="brandType10"><a href="<%=this.GetSearchQueryString("g","10") %>">美系</a></li>
                <li id="brandType11"><a href="<%=this.GetSearchQueryString("g","11") %>">欧系</a></li>--%>

                <li id="brandType0"><a href="<%=this.GetSearchQueryString("g","0") %>">不限</a></li>
                <li id="brandType1"><a href="<%=this.GetSearchQueryString("g","1") %>">自主</a></li>
                <li id="brandType2"><a href="<%=this.GetSearchQueryString("g","2") %>">合资</a></li>
                <li id="brandType4"><a href="<%=this.GetSearchQueryString("g","4") %>">进口</a></li>
                <li id="countryType4"><a href="<%=this.GetSearchQueryString("c","4") %>">德系</a></li>
                <li id="countryType2"><a href="<%=this.GetSearchQueryString("c","2") %>">日系</a></li>
                <li id="countryType16"><a href="<%=this.GetSearchQueryString("c","16") %>">韩系</a></li>
                <li id="countryType8"><a href="<%=this.GetSearchQueryString("c","8") %>">美系</a></li>
                <li id="countryType484"><a href="<%=this.GetSearchQueryString("c","484") %>">欧系</a></li>
            </ul>
        </div>
    </div>
</div>
<!-- popup end -->
<!-- 排量 popup start -->
<div id="m-filter-dis-leftPopup" class="leftPopup dis" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-dis">
        <ul class="first-list rp">
            <li id="dis0"><a href="<%=this.GetSearchQueryString("d","") %>">不限</a></li>
            <li id="dis1"><a href="<%=this.GetSearchQueryString("d","0-1.3") %>">1.3L以下</a></li>
            <li id="dis2"><a href="<%=this.GetSearchQueryString("d","1.3-1.6") %>">1.3-1.6L</a></li>
            <li id="dis3"><a href="<%=this.GetSearchQueryString("d","1.7-2.0") %>">1.7-2.0L</a></li>
            <li id="dis4"><a href="<%=this.GetSearchQueryString("d","2.1-3.0") %>">2.1-3.0L</a></li>
            <li id="dis5"><a href="<%=this.GetSearchQueryString("d","3.1-5.0") %>">3.1-5.0L</a></li>
            <li id="dis6"><a href="<%=this.GetSearchQueryString("d","5.0-9") %>">5.0L以上</a></li>
        </ul>
    </div>
</div>
<!-- black popup end -->
<!-- 变速箱 popup start -->
<div id="m-filter-trans-leftPopup" class="leftPopup trans" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-trans">
        <div class="ap">
            <ul class="first-list rp">
                <li id="trans0"><a href="<%=this.GetSearchQueryString("t","0") %>">不限</a></li>
                <li id="trans1"><a href="<%=this.GetSearchQueryString("t","1") %>">手动</a></li>
                <li id="trans62"><a href="<%=this.GetSearchQueryString("t","126") %>">自动</a></li>
            </ul>
            <ul class="second-list ">
                <li id="trans32"><a href="<%=this.GetSearchQueryString("t","32") %>" class="cSubItem">半自动（AMT）</a></li>
                <li id="trans2"><a href="<%=this.GetSearchQueryString("t","2") %>" class="cSubItem">自动（AT）</a></li>
                <li id="trans4"><a href="<%=this.GetSearchQueryString("t","4") %>" class="cSubItem">手自一体</a></li>
                <li id="trans8"><a href="<%=this.GetSearchQueryString("t","8") %>" class="cSubItem">无级变速</a></li>
                <li id="trans16"><a href="<%=this.GetSearchQueryString("t","16") %>" class="cSubItem">双离合</a></li>
            </ul>
        </div>
    </div>
</div>
<!-- black popup end -->
<!-- 配置 popup start -->
<div id="m-filter-condition-leftPopup" class="leftPopup condition" data-back="leftmask3" style="z-index: 999999; display: none;">
    <div class="swipeLeft" id="m-filter-condition">
        <div class="y2015-car-02">
            <div class="slider-box" style="height: 947px; overflow: hidden">
                <ul class="first-list" style="transition-property: transform; transform-origin: 0px 0px 0px; transform: translate(0px, 0px) translateZ(0px);">
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck204" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>天窗</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck196" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>GPS导航</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck200" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>倒车影像</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck180" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>儿童锁</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck101" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>涡轮增压</span>
                            </label>
                        </div>
                    </li>
                     <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck179" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>无钥匙启动</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck141" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>四轮碟刹</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck250" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>真皮座椅</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck184" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>ESP</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck224" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>氙气大灯</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck194" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>定速巡航</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck274" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>自动空调</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck177" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>胎压监测</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck189" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>自动泊车</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck249" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>空气净化器</span>
                            </label>
                        </div>
                    </li>
                     <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck163" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>换挡拨片</span>
                            </label>
                        </div>
                    </li>
                    <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck236" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>电动座椅</span>
                            </label>
                        </div>
                    </li>
                     <li>
                        <div class="radio-box">
                            <label>
                                <div id="mcCheck181" class="radio-normal">
                                    <input type="checkbox" />
                                </div>
                                <span>儿童座椅接口</span>
                            </label>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="swipeLeft-header" style="display: block;">
            <a id="m-btn-condition-clear" class="btn-clear ">清除</a>
            <a id="m-btn-condition" href="#" class="btn-comparison">完成</a>
            <div class="clear"></div>
        </div>
    </div>
</div>
<!-- popup end -->
<!-- 更多 popup start -->
<div id="m-filter-more-leftPopup" class="leftPopup more" data-back="leftmask3" style="z-index: 999999; display: none;">
    <div class="swipeLeft" id="m-filter-more">
        <div class="y2015-car-02 ">
            <div class="slider-box" style="height: 947px; overflow: hidden;">
                <ul class="first-list">
                    <li id="btnDrive" class="sub m-btn" data-action="subDrive"><a>驱动<em></em></a></li>
                    <li id="btnFuel" class="sub m-btn" data-action="subFuel"><a>燃料<em></em></a></li>
                     <li id="btnEnvironment" class="sub m-btn" data-action="subEnvironment"><a>排放<em></em></a></li>
                    <li id="btnDoor" class="sub m-btn" data-action="subDoor"><a>车门数<em></em></a></li>
                    <li id="btnSeat" class="sub m-btn" data-action="subSeat"><a>座位数<em></em></a></li>
                </ul>
            </div>
        </div>
      
        <div class="swipeLeft-header" style="display: block">
            <a id="m-btn-more-clear" class="btn-clear">清除</a>
            <a id="m-btn-more" class="btn-comparison">完成</a>
            <div class="clear"></div>
        </div>
    </div>
</div>
<div id="m-filter-drive-leftPopup" class="leftPopup model2 subDrive" data-back="leftmask3" style="display: none;">
    <div id="m-filter-drive" class="swipeLeft">  <%--swipeLeft-sub--%>
        <div class="ap">
            <ul class="first-list rp">
                <li class="root" onclick="GotoPageMore('dt','-1');"><a>驱动</a></li>
                <li id="drivetype0" class="morestyle drivetype0" onclick="GotoPageMore('dt','0');"><a>不限</a></li>
                <li id="drivetype1" class="morestyle drivetype1" onclick="GotoPageMore('dt','1');"><a>前驱</a></li>
                <li id="drivetype2" class="morestyle drivetype2" onclick="GotoPageMore('dt','2');"><a>后驱</a></li>
                <li id="drivetype252" class="morestyle drivetype252" onclick="GotoPageMore('dt','252');"><a>四驱</a></li>
            </ul>
            <ul class="second-list">
                <li class="morestyle drivetype252" onclick="GotoPageMore('dt','252');"><a>全部</a></li>
                <li id="drivetype4" class="morestyle drivetype4" onclick="GotoPageMore('dt','4');"><a>全时四驱</a></li>
                <li id="drivetype8" class="morestyle drivetype8" onclick="GotoPageMore('dt','8');"><a>分时四驱</a></li>
                <li id="drivetype16" class="morestyle drivetype16" onclick="GotoPageMore('dt','16');"><a>适时四驱</a></li>
            </ul>
      </div>
    </div>
</div>
<div id="m-filter-fuel-leftPopup" class="leftPopup model2 subFuel" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-fuel">
        <ul class="first-list absolute">
            <li class="root" onclick="GotoPageMore('f','-1');"><a>燃料</a></li>
            <li id="fueltype0" class="morestyle" onclick="GotoPageMore('f','0');"><a>不限</a></li>
            <li id="fueltype7" class="morestyle" onclick="GotoPageMore('f','7');"><a>汽油</a></li>
            <li id="fueltype8" class="morestyle" onclick="GotoPageMore('f','8');"><a>柴油</a></li>
            <li id="fueltype16" class="morestyle" onclick="GotoPageMore('f','16');"><a>纯电动</a></li>
            <li id="fueltype2" class="morestyle" onclick="GotoPageMore('f','2');"><a>油电混合</a></li>
            <li id="fueltype4" class="morestyle" onclick="GotoPageMore('f','4');"><a>油气混合</a></li>
        </ul>
    </div>
</div>
<div id="m-filter-environment-leftPopup" class="leftPopup model2 subEnvironment" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-environment">
        <ul class="first-list absolute">
            <li class="root" onclick="GotoPageMore('environment','-1');"><a>排放</a></li>
            <li id="envrionmentType0" class="morestyle" onclick="GotoPageMore('environment','0');"><a>不限</a></li>
            <li id="envrionmentType126" class="morestyle" onclick="GotoPageMore('environment','126');"><a>国4</a></li>
            <li id="envrionmentType125" class="morestyle" onclick="GotoPageMore('environment','125');"><a>国5</a></li>
            <li id="envrionmentType127" class="morestyle" onclick="GotoPageMore('environment','127');"><a>京5</a></li>
            <li id="envrionmentType123" class="morestyle" onclick="GotoPageMore('environment','123');"><a>欧4</a></li>
            <li id="envrionmentType122" class="morestyle" onclick="GotoPageMore('environment','122');"><a>欧5</a></li>
            <li id="envrionmentType126_123" class="morestyle" onclick="GotoPageMore('environment','126_123');"><a>国4/欧4</a></li>
            <li id="envrionmentType125_122" class="morestyle" onclick="GotoPageMore('environment','125_122');"><a>国5/欧5</a></li>
        </ul>
    </div>
</div>
<div id="m-filter-door-leftPopup" class="leftPopup model2 subDoor" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-door">
        <ul class="first-list absolute">
            <li class="root" onclick="GotoPageMore('bd','-1');"><a>车门数</a></li>
            <li id="doors0" class="morestyle" onclick="GotoPageMore('bd','0');"><a>不限</a></li>
            <li id="doors2" class="morestyle" onclick="GotoPageMore('bd','2-3');"><a>2-3门</a></li>
            <li id="doors4" class="morestyle" onclick="GotoPageMore('bd','4-6');"><a>4-6门</a></li>
        </ul>
    </div>
</div>
<div id="m-filter-seat-leftPopup" class="leftPopup model2 subSeat" data-back="leftmask3" style="display: none;">
    <div class="swipeLeft" id="m-filter-seat">
        <ul class="first-list absolute">
            <li class="root" onclick="GotoPageMore('sn','-1');"><a>座位数</a></li>
            <li id="seatnum0" class="morestyle" onclick="GotoPageMore('sn','0');"><a>不限</a></li>
            <li id="seatnum2" class="morestyle" onclick="GotoPageMore('sn','2');"><a>2座</a></li>
            <li id="seatnum4" class="morestyle" onclick="GotoPageMore('sn','4-5');"><a>4-5座</a></li>
            <li id="seatnum6" class="morestyle" onclick="GotoPageMore('sn','6');"><a>6座</a></li>
            <li id="seatnum7" class="morestyle" onclick="GotoPageMore('sn','7');"><a>7座</a></li>
            <li id="seatnum8" class="morestyle" onclick="GotoPageMore('sn','8-9999');"><a>7座以上</a></li>
        </ul>
    </div>
</div>
<!-- popup end -->
<script type="text/javascript">
	
	<%=GenerateSearchInitScript() %>
    SelectCarTool.initPageCondition();

</script>
