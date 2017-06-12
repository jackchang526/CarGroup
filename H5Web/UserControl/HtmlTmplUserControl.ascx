<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HtmlTmplUserControl.ascx.cs" Inherits="H5Web.UserControl.HtmlTmplUserControl" %>
<!--评测模版-->
<script type="text/x-jquery-tmpl" id="pingcenewstmp">
	<header>
		<h2>评测导购</h2>
	</header>
	{{if listgroup.length>0}}
	    {{each listgroup}}
			    <div class="slide" data-anchor="slide4-${$index+1}">
				    <div class="con_top_bg"></div>
				    <!--内容容器开始-->
				    <div class="contain">
					    <ul class="con_list_ul">
						    {{each $value}}
						        {{if $value.CarImage}}
						            <li>
						        {{else}}
							        <li class="nopic">
                                {{/if}}
							    <a href="${$value.PageUrl}">
                                    {{if $value.CarImage.length>0}}
								        <div class="con_list_img">
									        <img src="${$value.CarImage}" />
								        </div>
								    {{/if}}
								    <div class="con_list">
									    <h4>${$value.Title}</h4>
                                        
                                        {{if $value.NewsCategoryShowName=="车型详解" || $value.NewsCategoryShowName=="购车手册"}}
                                            <p><strong>${$value.NewsCategoryShowName}</strong></p>
                                        {{else}}
									        <p>${$value.PublishTime} {{if $value.Author}} / ${$value.Author}{{/if}}</p>
                                        {{/if}}

								    </div>
							    </a>
							    </li>
						    {{/each}}
						    {{if $index==0}}
						        <li id="addaogoufirst"></li>
						    {{/if}}
						    {{if !isSecondAd }}
						        <li id="addaogousecond" class="con_list_ul_ad"></li>
						    {{/if}}
						    {{if isSecondAd && $index==1 }}
						        <li id="addaogousecond" class="con_list_ul_ad"></li>
						    {{/if}}
					    </ul>
				    </div>
				    <!--内容容器结束-->
			    </div>
	    {{/each}}
	    {{if isCustomization==false}}
            {{if isSecondAd==true&&listgroup.length==1}}
	            <div class="slide" data-anchor="slide4-2">
		            <div class="con_top_bg"></div>
		            <div class="contain">
			            <ul class="con_list_ul">
				            <li id="addaogousecond" class="con_list_ul_ad"></li>
			            </ul>
		            </div>
	            </div>
	        {{/if}}
        {{/if}}
	{{else}}
        {{if isCustomization==false}}
	        <div class="slide" data-anchor="slide4-1">
		        <div class="con_top_bg"></div>
		        <!--内容容器开始-->
		        <div class="contain">
			        <ul class="con_list_ul">
				        <li id="addaogoufirst"></li>
				        <li id="addaogousecond" class="con_list_ul_ad"></li>
			        </ul>
		        </div>
	        </div>
        {{/if}}
	{{/if}}
	<div class="arrow_down"></div>
</script>

<!--亮点配置模版-->
<script type="text/x-jquery-tmpl" id="peizhitmpl">
	<header>
		<h2>亮点配置</h2>
	</header>
	<div class="con_top_bg"></div>
	{{if list.length>0}}
			<div class="contain contain_config">
				<ul class="highlight">
					{{each list}}
					    {{if ($index+1)%4==1}}
					        <li>
                        {{/if}}
						    <span><img src="http://image.bitautoimg.com/carchannel/pic/sparkle/${$value.H5SId}.png" />${$value.Name}</span>
						{{if ($index+1)%4==0}}
					        </li>
					    {{/if}}
					{{/each}}
					<span>
						<a href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/peizhi/">
							<img src="http://image.bitautoimg.com/carchannel/pic/sparkle/0.png">全部配置
						</a>
					</span>					
					{{if list.length% 4 >0 }}
					    </li>
					{{/if}}					
				</ul>
			</div>
	{{else}}
		<div class="message-failure">
			<img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
			<h2>很遗憾！</h2>
			<p>数据抓紧完善中，敬请期待！</p>
		</div>
	{{/if}}
	<!--内容容器结束-->
	<!--下箭头 固定-->
	<div class="arrow_down"></div>
</script>

<!--口碑模版-->
<script type="text/x-jquery-tmpl" id="koubeitmpl">
	<header>
		<h2>热门点评</h2>
	</header>
	<div class="con_top_bg"></div>
	{{if editorComment.length>0 }}
	<!--左右1-->
	<div class="slide" data-anchor="slide6-1">
		<!--大背景容器开始-->
		<div class="big_bg">
			<h4 class="con_box">编辑点评</h4>
			<!--内容容器开始-->
			<div class="contain">
				<a href="#">
					<div class="koubei">
						<div class="koubei_img">
							<img src="${editorComment[0].SquarePhoto}" />
						</div>
						<ul>
							<li><span>编辑：</span>${editorComment[0].Name}</li>
							{{if editorComment[0].Car}}
                                    <li><span>车款：</span>${editorComment[0].Car.CarName}</li>
							{{/if}}
						</ul>
					</div>
					<div class="koubei_txt">
						<p>${editorComment[0].Comment}</p>
					</div>
				</a>
			</div>
			<!--内容容器结束-->
		</div>
		<!--大背景容器结束-->
	</div>
	{{/if}}

	<!--左右2-->
	<div class="slide" data-anchor="slide6-2">
		<!--大背景容器开始-->
		<div class="big_bg">
			<h4 class="con_box">网友评分</h4>
			<!--内容容器开始-->
			<div class="contain">
				<!--分数开始-->
				<ul class="score_box">
					{{if score>0 }}
							<li>
								<a href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/koubei/">${parseFloat(score).toFixed(1)}分
									<span class="big-star"><em style="width: ${score/5*100}%"></em></span>
									<section></section>
								</a>
							</li>
					{{/if}}
					{{if GuestFuelCost.length>0 }}
							<li>
								<a href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/youhao/">${GuestFuelCost}
									<div>网友平均油耗</div>
									<section></section>
								</a>
							</li>
					{{/if}}
				</ul>
				<!--分数结束-->
				{{if koubeiImpression.goods.length>0||koubeiImpression.bad.length>0}}
				<!--关键字开始-->
				<div class="wy-kb-box">
					{{if koubeiImpression.goods.length>0}}
						<div class="yd-box">
							{{each koubeiImpression.goods}}
						        <a class="" href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/koubei/word/${$value.Keyword}/#acWord">
                                    ${$value.Keyword}<em>(${$value.VoteCount})</em>
						        </a>
							{{/each}}
						</div>
					{{/if}}
					{{if koubeiImpression.bad.length>0}}
					    <div class="qd-box">
						    {{each koubeiImpression.bad}}
						        <a class="" href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/koubei/word/${$value.Keyword}/#acWord">
                                    ${$value.Keyword}<em>(${$value.VoteCount})</em>
						        </a>
						    {{/each}}
					    </div>
					{{/if}}
				</div>
                <!--全部开始-->
				<button class="button_gray"><a href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/koubei/">查看完整口碑</a></button>
				<!--全部结束-->
				<!--关键字结束-->
                {{/if}}
			</div>			
			<!--内容容器结束-->
		</div>
		<!--大背景容器结束-->
	</div>
	{{if artic.length>0}}
		<div class="slide" data-anchor="slide6-3">
			<!--大背景容器开始-->
			<div class="big_bg">
				<h4 class="con_box">网友点评</h4>
				<!--内容容器开始-->
				<div class="contain">
					<a href="#">
						<div class="koubei">
							<div class="koubei_img">
								<img src="${artic[0].UserImage}" />
							</div>
							<ul>
								<li><span>网友：</span>${artic[0].UserName}</li>
								<li><span>车款：</span>${artic[0].TrimName}</li>
							</ul>
						</div>
						<div class="koubei_txt koubei_txt_netfriend">
							<span>${artic[0].CreateTime}&nbsp;&nbsp;已行驶${artic[0].Mileage}公里&nbsp;&nbsp;实测油耗：${parseFloat(artic[0].Fuel).toFixed(1)}L</span>
							<h4>${artic[0].Title}</h4>
							<p>${artic[0].Contents}</p>
						</div>
						<button class="button_gray"><a href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/koubei/">查看全部${TopicCount}条口碑</a></button>
					</a>
				</div>
			</div>
		</div>
	{{/if}}
	<!--左右3-->
	<!--下箭头 固定-->
	<div class="arrow_down"></div>
</script>

<!--车款报价模版 经纪人版-->
<script type="text/x-jquery-tmpl" id="agentcarlisttmp">
	<header>
		<h2>车款报价</h2>
	</header>
	<div class="con_top_bg"></div>
	{{if listgroup.length>0}}
	    {{each  listgroup}}
	    <!--左右1-->
	    <div class="slide" data-anchor="slide7-${index+1}">
		    <!--大背景容器开始-->
		    <div class="big_bg">
			    <ul class="car_price">
				    {{each  $value.carlist}}
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
						    <button class="button_gray"><a href="http://price.m.yiche.com/nb<%=BaseSerialEntity.Id %>/">更多车款报价</a></button>
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

<!--车款报价模版 用户版-->
<script type="text/x-jquery-tmpl" id="carlisttmp">
	<header>
		<h2>车款报价</h2>
	</header>
	<div class="con_top_bg"></div>
	{{if listgroup.length>0}}
	    {{each  listgroup}}
	    <!--左右1-->
	    <div class="slide" data-anchor="slide7-${index+1}">
		    <!--大背景容器开始-->
		    <div class="big_bg">  		    
                <ul class="car_price car_dijia">                    {{each  $value.carlist}}                    <li>
			            <div class="name car-info">			
				            <a href="http://car.m.yiche.com/<%=BaseSerialEntity.AllSpell %>/m${$value.CarID}/">
	                            <h6>${$value.CarYear}款 ${$value.CarName}</h6>								
	                            <div class="price">
	                                {{if $value.CarPriceRange.length>0}}
									    <div class="now">${$value.CarPriceRange.split('-')[0]}</div>
									{{/if}}
	                                <p>指导价:${$value.ReferPrice}万</p>
	                            </div>
						    </a>
						</div>
						<div class="car-call"><a href="http://price.m.yiche.com/zuidijia/nc${$value.CarID}/?leads_source=32001">询底价</a></div>							
					</li>
                    {{/each}}
                    {{if carcount>19 && $index==4}}
					    <li>
						    <button class="button_gray"><a href="http://price.m.yiche.com/nb<%=BaseSerialEntity.Id %>/">更多车款报价</a></button>
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

<!--优惠购车模版-->
<script type="text/x-jquery-tmpl" id="gouchetmpl">
    <header>
        <h2>优惠购车</h2>
    </header>
    
    {{if huimaiche.html.length>0}}
    <div class="slide" data-anchor="slide8-1">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box" id="huimaiche-div">
                <h2>${huimaiche.title}</h2>
                <div class="youhui-box-pic">
                    <img src="${logo}"/>
                    <!--<div class="dijia">底价</div>-->
                </div>
                {{html huimaiche.html}}
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    <!--左右2-->
    {{if yichemall.html.length>0}}
    <div class="slide" data-anchor="slide8-2">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box">
                <h2>${yichemall.title}</h2>
                <div class="youhui-box-pic">
                    <img src="${logo}"/>
                    <%--<div class="zhixiao">热卖</div>--%>
                </div>
                {{html yichemall.html}}
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    <!--左右3-->
    {{if yichehui.html.length>0}}
    <div class="slide" data-anchor="slide8-3">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box">
                <h2>${yichehui.title}</h2>
                <div class="youhui-box-pic">
                    <img src="${logo}"/>
                </div>
                {{html yichehui.html}}
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}

    {{if yicheloan.html.length>0}}
    <!--左右4-->
    <!--左右4-->
    <div class="slide" data-anchor="slide8-4">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box">
                <h2>${yicheloan.title}</h2>
                <div class="youhui-box-pic">
                    <img src="${logo}"/>
                </div>
                {{html yicheloan.html}}
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    <!--左右5-->
    {{if yicheershou.html.length>0}}
    <div class="slide" data-anchor="slide8-5">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box">
                <h2>${yicheershou.title}</h2>
                <div class="youhui-box-pic">
                    <img src="${logo}"/>
                </div>
                {{html yicheershou.html}}
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    <!--左右6-->
    <div class="slide" data-anchor="slide8-6">
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <div class="youhui-box">
                <h2>更多优惠</h2>
                <div id="youhuiadwrapper" class="youhui-ad">
                </div>
                <a href="#" id="gouchelink">
                    <img src="http://img1.bitauto.com/uimg/4th/img/yishu.png">
                </a>
            </div>
        </div>
        <!--大背景容器结束-->
    </div>
    <!--下箭头 固定-->
    <div class="arrow_down"></div>
</script>

<!--看了还看模版-->
<script type="text/x-jquery-tmpl" id="seeagaintmp">
	<header>
		<h2>看了还看</h2>
	</header>
	<div class="con_top_bg"></div>
	{{if list.length>0}}
	    <div class="big_bg big_bg_car_list">
		    <ul class="car_list">
			    {{each list}}
					<li>
                    {{if $value.IsFourthStage}}
						<a href="/${$value.AllSpell}/">
                    {{else}}
						<a href="http://car.m.yiche.com/${$value.AllSpell}/">
                    {{/if}}						
						<img src="${$value.ImageUrl}"><span>${$value.ShowName}</span><p>${$value.PriceRange}</p>
						</a>
                    </li>
			    {{/each}}
		    </ul>
		    <button class="button_gray"><a href="http://car.h5.yiche.com">查看全部车型</a></button>
	    </div>
	{{else}}
		<div class="message-failure">
			<img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
			<h2>很遗憾！</h2>
			<p>数据抓紧完善中，敬请期待！</p>
		</div>
	{{/if}}
	<!--下箭头 固定-->
	<div class="arrow_down"></div>
</script>

<script type="text/x-jquery-tmpl" id="dealermoretmpl">
	<header>
		<h2>热销车型</h2>
	</header>
	<div class="con_top_bg"></div>
	{{if html.length>0}}
	    {{html html}}
	{{else}}
		<div class="message-failure">
			<img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
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
			<img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
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
		    <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
		    <h2>很遗憾！</h2>
		    <p>数据抓紧完善中，敬请期待！</p>
	    </div>
	{{/if}}
	<div class="arrow_down"></div>
</script>

<script type="text/x-jquery-tmpl" id="yanghutmpl">
	<header>
		<h2>养护</h2>
	</header>
	{{if html.length>0}}
	    {{html html}}
	{{else}}
	    <div class="message-failure">
		    <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
		    <h2>很遗憾！</h2>
		    <p>数据抓紧完善中，敬请期待！</p>
	    </div>
	{{/if}}
	<div class="arrow_down"></div>
</script>

<script type="text/x-jquery-tmpl" id="baoxiantmpl">
	<header>
		<h2>汽车保险</h2>
	</header>
	{{if Compulsory>0||CommonTotal>0}}
	    <div class="con_top_bg"></div>
	    <div class="big_bg">
		    <!--保险开始-->
		    <div class="baoxian-box">
			    <h2><%=BaseSerialEntity.ShowName %></h2>
			    <p>厂商指导价：<%= BaseSerialEntity.SaleState == "停销"?"暂无":BaseSerialEntity.ReferPrice%></p>
		    </div>
		    <div class="youhui_box baoxian_price">
			    <ul class="two">
				    <li>
					    <em>${Compulsory}</em>元起
				    			    <span>交强险指导价</span>
				    </li>
				    <li>
					    <em>${CommonTotal}</em>元起
				    			    <span>商业险指导价</span>
				    </li>
			    </ul>
		    </div>
		    <div class="box baoxian_logo">
			    <ul>
				    <li><a href="http://mchanxian.sinosig.com/activity/yiche/yiche.html">
					    <img src="http://img1.bitautoimg.com/uimg/4th/img2/baoxian_yg.png"><span>阳光车险</span></a></li>
				    <li><a href="http://www.epicc.com.cn/wap/cooperE/?cityCode=110100&comName=BDyichewang0100&carrier=002&channel=WAP01">
					    <img src="http://img1.bitautoimg.com/uimg/4th/img2/baoxian_rb.png"><span>人保车险</span></a></li>
				    <li><a href="http://chexian.axatp.com/m/m-auto?isAgent=68&ms=yiche?carid=0&tagname=#acWord">
					    <img src="http://img1.bitautoimg.com/uimg/4th/img2/baoxian_as.png"><span>安盛天平</span></a></li>
			    </ul>
		    </div>
		    <!--保险结束-->
	    </div>
	{{else}}
	    <div class="message-failure">
		    <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
		    <h2>很遗憾！</h2>
		    <p>数据抓紧完善中，敬请期待！</p>
	    </div>
	{{/if}}
	<div class="arrow_down"></div>
</script>

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
	    <div class="arrow_down"></div>
	{{/if}}
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
		    <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png" />
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
        <h2>看了还看</h2>
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

