<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BrokerTmp.ascx.cs" Inherits="H5Web.UserControl.BrokerTmp" %>

<!--车款报价模版 经纪人版-->
<script type="text/x-jquery-tmpl" id="agentcarlisttmp">
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
            <ul class="car_price">
                {{each $value.carlist}}
                <li>
                    <a href="#page8">
                        <div class="name">
                            <h6>${$value.CarYear}款 ${$value.CarName}</h6>
                            <p>${$value.UnderPan_ForwardGearNum+$value.TransmissionType}</p>
                        </div>
                        <div class="price">
                            <p>指导价:${$value.ReferPrice}万</p>
                        </div>
                    </a>
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
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <div class="arrow_down"></div>
</script>

