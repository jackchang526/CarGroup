<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserTmp.ascx.cs" Inherits="H5Web.UserControl.UserTmp" %>

<!--优惠购车模版-->
<script type="text/x-jquery-tmpl" id="gouchetmplnew">
    <header>
        <h2>贷款购车</h2>
    </header>
    {{if existCount>0}}
    <div class="slide" data-anchor="slide8-1">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <ul class="car_price car_price2">
                <%--{{if typeof huimaiche != "undefined"}}
                <li id="li_huimaiche" style="display: block">
                    <a href="${huimaiche.OrderUrl}">
                        <div class="name">
                            <h6>底价买车</h6>
                            {{each huimaiche.Description}}
                            <p>${$value}</p>
                            {{/each}}
                        </div>
                        <div class="price">
                            <div class="now">平均节省 ${huimaiche.SaveMoney}</div>
                        </div>
                    </a>
                    <section></section>
                </li>
                {{/if}}--%>
                {{if typeof yicheshangcheng != "undefined"}}
                <li id="li_yicheshangcheng" style="display: block">
                    <a href="${yicheshangcheng.Url}">
                        <div class="name">
                            <h6>直销特卖</h6>
                            {{each yicheshangcheng.Description}}
                            <p>${$value}</p>
                            {{/each}}
                        </div>
                        <div class="price">
                            <div class="now">${yicheshangcheng.Price}</div>
                        </div>
                    </a>
                    <section></section>
                </li>
                {{/if}}
                {{if typeof yichehui != "undefined"}}
                <li id="li_yichehui" style="display: block">
                    <a href="${yichehui.OrderUrl}">
                        <div class="name">
                            <h6 <%--class="park11"--%>><%--class="gouche66"--%>超值特惠</h6>
                            {{if typeof yichehui.Wenan1 != "undefined"}}
                            <p>${yichehui.Wenan1}</p>
                            {{/if}}
                            {{if typeof yichehui.Wenan2 != "undefined"}}
                            <p>${yichehui.Wenan2}</p>
                            {{/if}}
                        </div>
                        <div class="price">
                            <div class="now">
                                {{if typeof yichehui.Description != "undefined"}}
                                ${yichehui.Description}
                                {{/if}}
                            </div>
                        </div>
                    </a>
                    <section></section>
                </li>
                {{/if}}
                {{if typeof yichedaikuai != "undefined"}}
                <li id="li_yichedaikuai">
                    <a href="${yichedaikuai.Url}">
                        <div class="name">
                            <h6>贷款购车</h6>
                            <p>首付最低${yichedaikuai.Downpayment}</p>
                            <p>${yichedaikuai.Feature}</p>
                        </div>
                        <div class="price">
                            <div class="now">月供 ${yichedaikuai.Monthpay}起</div>
                        </div>
                    </a>
                    <section></section>
                </li>
                {{/if}}
            </ul>
        </div>
        <!--大背景容器结束-->
    </div>
    {{else}}
        {{if isAd===0}}
            <div class="slide" data-anchor="slide8-1">
                <div class="con_top_bg"></div>
                <div class="message-failure">
                    <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
                    <h2>很遗憾！</h2>
                    <p>数据抓紧完善中，敬请期待！</p>
                </div>
            </div>
    {{/if}}
    {{/if}}
    {{if isAd!==0 }}
    <div class="slide" data-anchor="slide8-2">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box">
                <h2>更多优惠</h2>
                <div id="youhuiadwrapper" class="youhui-ad">
                </div>
                <%--<a href="#" id="gouchelink">
                    <img src="http://img1.bitauto.com/uimg/4th/img/yishu.png">
                </a>--%>
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    <!--下箭头 固定-->
    <div class="arrow_down"></div>
</script>

<!--经销商-->
<script type="text/x-jquery-tmpl" id="userdealertmpl">
    {{if html.length>0}}
    {{html html}}    
    {{else}}
    <header>
        <h2>经销商</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div id="addealer" class="big_bg fullscreen"></div>
    <div class="arrow_down"></div>
    {{/if}}    
</script>

<!--养护-->
<script type="text/x-jquery-tmpl" id="yanghutmpl">
    {{if html.length>0}}
    {{html html}}
    {{else}}
     <header>
         <h2>车辆养护</h2>
     </header>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <div class="arrow_down"></div>
</script>

<!--车款报价模版-->
<script type="text/x-jquery-tmpl" id="carlisttmp">
    <header>
        <h2>车款报价</h2>
    </header>
    <div class="con_top_bg"></div>
    {{if typeof listgroup != "undefined" && listgroup.length>0}}
    {{each listgroup}}
    <!--左右1-->
    <div class="slide" data-anchor="slide7-${index+1}">
        <!--大背景容器开始-->
        <div class="big_bg">
            <ul class="car_price car_dijia">
                {{each $value.carlist}}
                <li id="slide-carlist-${$value.CarID}">
                    <div class="name car-info">
                        <%--<a data-channelid="85.97.889" href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/m${$value.CarID}/">--%>
                        <a data-channelid="85.97.889" onclick="MtaH5.clickStat('bb1');" href="http://dealer.h5.yiche.com/MultiOrder/<%= BaseSerialEntity.Id %>/${$value.CarID}/?leads_source=H001005">
                            <h6>{{if typeof taxtag != "undefined" && taxtag[$value.CarID] != undefined}}
                                <span class="blue_bg">${taxtag[$value.CarID]}</span>
                                {{/if}}
                                ${$value.CarYear}款 ${$value.CarName}</h6>
                            <div class="price">
                                {{if $value.CarPriceRange.length>0}}
                                <div class="now">${$value.CarPriceRange.split('-')[0]}</div>
                                {{/if}}
                                <p>指导价:${$value.ReferPrice}万</p>
                            </div>
                        </a>
                    </div>
                    <div class="car-call">
                        <%--<a data-channelid="85.97.890" href="http://price.m.yiche.com/zuidijia/nc${$value.CarID}/?leads_source=H001005">询底价</a>--%>
                        <a data-channelid="85.97.890" onclick="MtaH5.clickStat('bb2');" href="http://dealer.h5.yiche.com/MultiOrder/<%= BaseSerialEntity.Id %>/${$value.CarID}/?leads_source=H001005">询底价</a>
                    </div>
                </li>
                {{/each}}
                {{if carcount>19 && $index==4}}
                <li>
                    <button class="button_gray">
                        <a href="http://price.m.yiche.com/nb<%= BaseSerialEntity.Id %>/">更多车款报价</a>
                    </button>
                </li>
                {{/if}}
            </ul>

        </div>
    </div>
    {{/each}}
    {{else}}
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <div class="arrow_down"></div>
</script>

<!--保险-->
<script type="text/x-jquery-tmpl" id="baoxiantmpl">
    <header>
        <h2>汽车保险</h2>
    </header>
    <div class="con_top_bg"></div>
    {{if html.length>0}}
    {{html html}}    
    {{/if}} 
    <div class="arrow_down"></div>
</script>

<!--二手车-->
<script type="text/x-jquery-tmpl" id="ershouchetmpl">
    <header>
        <h2>二手车</h2>
    </header>
    <div class="con_top_bg"></div>
    {{if html.length>0}}
    {{html html}}    
    {{/if}}  
    <div class="arrow_down"></div>
</script>
