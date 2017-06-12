<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DealerTmp.ascx.cs" Inherits="H5Web.UserControl.DealerTmp" %>

<script type="text/x-jquery-tmpl" id="dealermoretmpl">
    <header>
        <h2>热销车型</h2>
    </header>
    <div class="con_top_bg"></div>
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <div class="arrow_down"></div>
</script>

<script type="text/x-jquery-tmpl" id="dealeractivity">
    <header>
        <h2>热门促销</h2>
    </header>
    <div class="con_top_bg"></div>
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <div class="arrow_down"></div>
</script>

<script type="text/x-jquery-tmpl" id="dealercarlisttmpl">
    <header>
        <h2>车款报价</h2>
    </header>
    <div class="con_top_bg"></div>
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <div class="arrow_down"></div>
</script>

<script type="text/x-jquery-tmpl" id="dealersalecarprice">
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <header>
        <h2>车款报价</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
    {{/if}}
</script>

<script type="text/x-jquery-tmpl" id="dealersalenews">
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <header>
        <h2>热门促销</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
    {{/if}}
</script>

<script type="text/x-jquery-tmpl" id="dealersaleserial">
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <header>
        <h2>热销车型</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
    {{/if}}
</script>

<script type="text/x-jquery-tmpl" id="dealersaleshop">
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <header>
        <h2>商家简介</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
    {{/if}}
</script>

<script type="text/x-jquery-tmpl" id="dealeryanghu">
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <header>
        <h2>车辆养护</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
    {{/if}}
</script>

<script type="text/x-jquery-tmpl" id="dealerending">
    {{if html.length>0}}
    {{html html}}
    {{else}}
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
    {{/if}}
</script>