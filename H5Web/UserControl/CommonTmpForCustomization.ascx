<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonTmpForCustomization.ascx.cs" Inherits="H5Web.UserControl.CommonTmpForCustomization" %>

<!--评测模版-->
<script type="text/x-jquery-tmpl" id="pingcetmp20160413">
    <header>
        <h2>评测导购</h2>
    </header>
    
    <!--第一页1条有广告，最多显示3条数据-->
    {{if !(typeof listgroup[0]!="undefined" && listgroup[0].length < 0 && isCustomization==true)}}
    <!--左右1-->
    <div class="slide" data-anchor="slide4-1">
        <div class="con_top_bg"></div>
        <!--内容容器开始-->
        <div class="contain">
            <ul class="con_list_ul">
                {{if typeof listgroup[0]!="undefined" && listgroup[0].length>0}}
                        {{each listgroup[0]}}
                        {{if $value.CarImage}}
                        <li>
                        {{else}}
                        <li class="nopic">
                            {{/if}}
                            <a href="${$value.PageUrl}">
                                {{if $value.CarImage.length>0}}
                                <div class="con_list_img">
                                    <img src="${$value.CarImage}"/>
                                </div>
                                {{/if}}
                                <div class="con_list">
                                    <h4>${$value.Title}</h4>
                                    {{if $value.NewsCategoryShowName=="车型详解" || $value.NewsCategoryShowName=="购车手册"}}
                                    <p>
                                        <strong>${$value.NewsCategoryShowName}</strong>
                                    </p>
                                    {{else}}
                                    <p>${$value.PublishTime} {{if $value.Author}} / ${$value.Author}{{/if}}</p>
                                    {{/if}}
                                </div>
                            </a>
                        </li>
                        {{/each}}
                {{/if}}
                {{if isCustomization==false}}
                <li id="adfirst">                    
                </li>
                {{/if}}
            </ul>
        </div>
        <!--内容容器结束-->
    </div>
    {{/if}}

    <!--第二页2条有广告，最多显示2条数据-->
    {{if !(typeof listgroup[1]!="undefined" && listgroup[1].length < 0 && isCustomization==true)}}
    <!--左右2-->
    <div class="slide" data-anchor="slide4-2">
        <div class="con_top_bg"></div>
        <!--内容容器开始-->
        <div class="contain">
            <ul class="con_list_ul">
                {{if typeof listgroup[1]!="undefined" && listgroup[1].length>0}}
                        {{each listgroup[1]}}
                        {{if $value.CarImage}}
                        <li>
                        {{else}}
                        <li class="nopic">
                            {{/if}}
                            <a href="${$value.PageUrl}">
                                {{if $value.CarImage.length>0}}
                                <div class="con_list_img">
                                    <img src="${$value.CarImage}"/>
                                </div>
                                {{/if}}
                                <div class="con_list">
                                    <h4>${$value.Title}</h4>
                                    {{if $value.NewsCategoryShowName=="车型详解" || $value.NewsCategoryShowName=="购车手册"}}
                                    <p>
                                        <strong>${$value.NewsCategoryShowName}</strong>
                                    </p>
                                    {{else}}
                                    <p>${$value.PublishTime} {{if $value.Author}} / ${$value.Author}{{/if}}</p>
                                    {{/if}}

                                </div>
                            </a>
                        </li>
                        {{/each}}
                {{/if}}                
                {{if isCustomization==false}}
                <li id="adsecond"></li>
                <li class="con_list_ul_ad" id="adthird"></li>
                {{/if}}
            </ul>
        </div>
        <!--内容容器结束-->
    </div>
    {{/if}}

    <!--第三页无广告，最多显示4条数据-->
    {{if typeof listgroup[2]!="undefined" && listgroup[2].length>0}}
    <!--左右3-->
    <div class="slide" data-anchor="slide4-3">
        <div class="con_top_bg"></div>
        <!--内容容器开始-->
        <div class="contain">
           <ul class="con_list_ul">
                {{each listgroup[2]}}
                        {{if $value.CarImage}}
                        <li>
                        {{else}}
                        <li class="nopic">
                            {{/if}}
                            <a href="${$value.PageUrl}">
                                {{if $value.CarImage.length>0}}
                                <div class="con_list_img">
                                    <img src="${$value.CarImage}"/>
                                </div>
                                {{/if}}
                                <div class="con_list">
                                    <h4>${$value.Title}</h4>
                                    {{if $value.NewsCategoryShowName=="车型详解" || $value.NewsCategoryShowName=="购车手册"}}
                                    <p>
                                        <strong>${$value.NewsCategoryShowName}</strong>
                                    </p>
                                    {{else}}
                                    <p>${$value.PublishTime} {{if $value.Author}} / ${$value.Author}{{/if}}</p>
                                    {{/if}}

                                </div>
                            </a>
                        </li>
                        {{/each}}                
            </ul>
        </div>
        <!--内容容器结束-->
    </div>
    {{/if}}

    <!--下箭头 固定-->
    <div class="arrow_down"></div>
    
</script>

<!--口碑模版-->
<script type="text/x-jquery-tmpl" id="koubeitmpl">
    {{if TopicCount=="0" && artic.length==0 && editorComment.length==0}}
    <header>
        <h2>网友评分</h2>
    </header>
    <div class="con_top_bg"></div>
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}  
    {{if typeof TopicCount != "undefined" && TopicCount>0}}
    <!--左右1-->
    <div class="slide use-dp" data-anchor="slide6-1">
        <header>
            <h2>网友评分</h2>
        </header>
        <div class="con_top_bg"></div>
        <!--大背景容器开始-->
        <div class="big_bg">
            <%--<h4 class="con_box">网友评分</h4>--%>
            <!--内容容器开始-->
            <div class="contain">
                <!--分数开始-->
                <ul class="score_box">
                    {{if typeof score != "undefined" && score>0 }}
                    <li>
                        <a href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/koubei/">
                            ${parseFloat(score).toFixed(1)}分
                            <span class="big-star"><em style="width: $ {score/5*100}%"></em></span>
                            <section></section>
                        </a>
                    </li>
                    {{/if}}
                    {{if typeof GuestFuelCost != "undefined" && GuestFuelCost.length>0 }}
                    <li>
                        <a href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/youhao/">
                            ${GuestFuelCost}
                            <div>网友平均油耗</div>
                            <section></section>
                        </a>
                    </li>
                    {{/if}}
                </ul>
                <!--分数结束-->
                
                {{if typeof koubei != "undefined"}}
                
                <!--柱状图start-->
                <div class="clear"></div>
                <div class="kb-tb-box">
                    <div class="kb-conter">
                        <!--图表 start-->

                        <div class="wrap">
                            <div class="draw-canvas">
                                <div class="canvas-bg">
                                    <ul>
                                        <li></li>
                                        <li></li>
                                        <li></li>
                                        <li></li>
                                        <li></li>
                                    </ul>
                                </div>
                                <div class="canvas-content" id="canObj">
                                    <ul>
                                        <li>
                                            <strong>${parseFloat(koubei.KongJian).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.KongJian/10*80}px;"></i>
                                            </div>
                                            <span>空间</span>
                                        </li>
                                        <li>
                                            <strong>${parseFloat(koubei.DongLi).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.DongLi/10*80}px;"></i>
                                            </div>
                                            <span>动力</span>
                                        </li>
                                        <li>
                                            <strong>${parseFloat(koubei.CaoKong).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.CaoKong/10*80}px;"></i>
                                            </div>
                                            <span>操控</span>
                                        </li>
                                        <li>
                                             <strong>${parseFloat(koubei.PeiZhi).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.PeiZhi/10*80}px;"></i>
                                            </div>
                                            <span>配置</span>
                                        </li>
                                        <li>
                                            <strong>${parseFloat(koubei.ShuShiDu).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.ShuShiDu/10*80}px;"></i>
                                            </div>
                                            <span>舒适</span>
                                        </li>
                                        <li>
                                            <strong>${parseFloat(koubei.XingJiaBi).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.XingJiaBi/10*80}px;"></i>
                                            </div>
                                            <span>性价比</span>
                                        </li>
                                        <li>
                                            <strong>${parseFloat(koubei.WaiGuan).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.WaiGuan/10*80}px;"></i>
                                            </div>
                                            <span>外观</span>
                                        </li>
                                        <li>
                                            <strong>${parseFloat(koubei.NeiShi).toFixed(1)}</strong>
                                            <div class="cylinder">
                                                <i style="height: ${koubei.NeiShi/10*80}px;"></i>
                                            </div>
                                            <span>内饰</span>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="clear"></div>

                        </div>
                        <!--图表 end-->
                    </div>

                </div>
                <!--柱状图end-->
                {{/if}}                  

                {{if typeof koubeiImpression != "undefined" && (koubeiImpression.goods.length>0||koubeiImpression.bad.length>0)}}
                <!--关键字开始-->
                <div class="wy-kb-box">
                    {{if koubeiImpression.goods.length>0}}
                    <div class="yd-box">
                        {{each koubeiImpression.goods}}
                        <a class="" href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/koubei/word/${$value.Keyword}/#acWord">
                            ${$value.Keyword}<em>(${$value.VoteCount})</em>
                        </a>
                        {{/each}}
                    </div>
                    {{/if}}
                    {{if koubeiImpression.bad.length>0}}
                    <div class="qd-box">
                        {{each koubeiImpression.bad}}
                        <a class="" href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/koubei/word/${$value.Keyword}/#acWord">
                            ${$value.Keyword}<em>(${$value.VoteCount})</em>
                        </a>
                        {{/each}}
                    </div>
                    {{/if}}
                </div>
                <!--全部开始-->
                <%--<button class="button_gray">
                    <a href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/koubei/">查看完整口碑</a>
                </button>--%>
                <!--全部结束-->
                <!--关键字结束-->
                {{/if}}
            </div>
            <!--内容容器结束-->
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    {{if typeof artic != "undefined" && artic.length>0}}
    <div class="slide" data-anchor="slide6-2">
        <header>
            <h2>网友点评</h2>
        </header>
        <div class="con_top_bg"></div> 
        <!--大背景容器开始-->
        <div class="big_bg">
            <%--<h4 class="con_box">网友点评</h4>--%>
            <!--内容容器开始-->
            <div class="contain">
                <a href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/koubei/${artic[0].TopicId}/">
                    <div class="koubei">
                        <div class="koubei_img">
                            <img src="${artic[0].UserImage}"/>
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
                    <%--<button class="button_gray">
                        <a href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/koubei/">查看全部${TopicCount}条口碑</a>
                    </button>--%>
                </a>
            </div>
        </div>
    </div>
    {{/if}}
    <!--左右2-->
    {{if typeof editorComment != "undefined"&& editorComment.length>0 }}
    <!--左右3-->
    <div class="slide" data-anchor="slide6-3">
        <header>
            <h2>编辑点评</h2>
        </header>
        <div class="con_top_bg"></div>   
        <!--大背景容器开始-->
        <div class="big_bg">
            <%--<h4 class="con_box">编辑点评</h4>--%>
            <!--内容容器开始-->
            <div class="contain">
                <div class="koubei">
                    <div class="koubei_img">
                        <img src="${editorComment[0].SquarePhoto}"/>
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
            </div>
            <!--内容容器结束-->
        </div>
        <!--大背景容器结束-->
    </div>
    {{/if}}
    <!--下箭头 固定-->
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
						<a href="http://car.m.yiche.com/<%= BaseSerialEntity.AllSpell %>/peizhi/" onclick="MtaH5.clickStat('bc1')">
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
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    {{/if}}
    <!--内容容器结束-->
    <!--下箭头 固定-->
    <div class="arrow_down"></div>
</script>

<!--数据为空的模版-->
<script type="text/x-jquery-tmpl" id="nodatatmpl">
    <header>
        <h2>${title}</h2> 
    </header>
    <div class="con_top_bg"></div>    
    <div class="message-failure">
        <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
        <h2>很遗憾！</h2>
        <p>数据抓紧完善中，敬请期待！</p>
    </div>
    <div class="arrow_down"></div>
</script>

<!--图片视频模版-->
<script type="text/x-jquery-tmpl" id="picvideotmpl">
    <!--<div style="height: 64px; background: #000;"></div>-->
    <!--左右翻页开始-->
    <!--左右1-->
    <div class="slide" data-anchor="slide3-1">
        <header>
            <h2>车型图片</h2>
        </header>
        {{if typeof data != "undefined"&& data["img"].length>0}}
        <div class="slide_box">
            {{if typeof data['img'][0] != "undefined"}}
            <a href="http://photo.m.yiche.com/picture/<%= BaseSerialEntity.Id %>/${data["img"][0].ImageId}/?fromh5=85.117.1310" class="pic-wall-re" onclick="MtaH5.clickStat('${position[data["img"][0].PositionId]}');">
                <span class="gradient-bg">
                    <em>${data["img"][0].PositionId=="6"?"外观":data["img"][0].PositionId=="7"?"内饰":data["img"][0].PositionId=="8"?"空间":""}</em>
            	</span>                
                <img src="${data["img"][0].ImageUrl.replace("autoalbum", "newsimg-750-w0-1-q80/autoalbum")}"/>
            </a>
            {{/if}}
            <div class="pic-wall">
                {{if typeof data['img'][1] != "undefined"}}
                <div>
                    <a href="http://photo.m.yiche.com/picture/<%= BaseSerialEntity.Id %>/${data["img"][1].ImageId}/?fromh5=85.117.1310" class="pic-wall-re" onclick="MtaH5.clickStat('${position[data["img"][1].PositionId]}');">
                        <span class="gradient-bg">
                            <em>
                                ${data["img"][1].PositionId=="6"?"外观":data["img"][1].PositionId=="7"?"内饰":data["img"][1].PositionId=="8"?"空间":""}
                            </em>
                        </span>                        
                        <img src="${data["img"][1].ImageUrl}"/>
                    </a>
                </div>
                {{/if}}
                {{if typeof data['img'][2] != "undefined"}}
                <div>
                    <a href="http://photo.m.yiche.com/picture/<%= BaseSerialEntity.Id %>/${data["img"][2].ImageId}/?fromh5=85.117.1310" class="pic-wall-re" onclick="MtaH5.clickStat('${position[data["img"][2].PositionId]}');">
                        <span class="gradient-bg">
                            <em>
                                ${data["img"][2].PositionId=="6"?"外观":data["img"][2].PositionId=="7"?"内饰":data["img"][2].PositionId=="8"?"空间":""}
                            </em>
                        </span>                       
                        <img src="${data["img"][2].ImageUrl}"/>
                    </a>
                </div>
                {{/if}}
            </div>
        </div>
        {{else}}
        <div class="message-failure">
            <%--<img src="http://img1.bitautoimg.com/uimg/4th/img2/404-600x400.jpg"/>--%>
            <img src="http://img1.bitautoimg.com/uimg/4th/img2/failure.png"/>
            <h2>很遗憾！</h2>
            <p>数据抓紧完善中，敬请期待！</p>
        </div>
        {{/if}}
        <!--广告 start -->
        <div id="adimg" class="fullscreen mt5"></div>        
        <!--广告 end -->
    </div>
    
    <!--左右2-->
    {{if typeof data != "undefined" && data['video'].length>0}}        
    <div class="slide" data-anchor="slide3-2">        
        <header>
    		<h2>车型视频</h2>
		</header>		
		<div class="slide_box" id="video-pic-wall">
            {{if typeof data['video'][0] != "undefined"}}
            <a href="${data['video'][0].ShowPlayUrl}" class="pic-wall-re">
        	    <span class="icon-video"></span>               
                <%--<img src="${data['video'][0].ImageLink.replace("Video", "newsimg_750x420/Video")}" />--%>
                <img src="${data['video'][0].ImageLink.replace("Video", "newsimg-750-w0/Video")}" />
            </a>
            <span class="pic-video-txt">
                <a href="${data['video'][0].ShowPlayUrl}">
                    <em>${data['video'][0].ShortTitle}</em>
                </a>
            </span>   
             {{/if}}
            <div class="pic-wall">   
                {{if typeof data['video'][1] != "undefined"}}     		
        	    <div>
        		    <a href="${data['video'][1].ShowPlayUrl}" class="pic-wall-re">
                        <span class="icon-video"></span>
                        <%--<img src="${data['video'][1].ImageLink.replace("Video", "newsimg_367x206/Video")}" />--%>
                        <img src="${data['video'][1].ImageLink.replace("Video", "newsimg-367-w0/Video")}" />
        		    </a>
        		    <span class="pic-video-txt">
                        <a href="${data['video'][1].ShowPlayUrl}">
                            <em>${data['video'][1].ShortTitle}</em>
                        </a>
        		    </span>
        	    </div>  
                {{/if}}     
                {{if typeof data['video'][2] != "undefined"}}     	     
                <div>
            	    <a href="${data['video'][2].ShowPlayUrl}" class="pic-wall-re">
                        <span class="icon-video"></span>
                        <%--<img src="${data['video'][2].ImageLink.replace("Video", "newsimg_367x206/Video")}" />--%>
                        <img src="${data['video'][2].ImageLink.replace("Video", "newsimg-367-w0/Video")}" />
            	    </a>
            	    <span class="pic-video-txt">
                        <a href="${data['video'][2].ShowPlayUrl}">
                            <em>${data['video'][2].ShortTitle}</em>
                        </a>
            	    </span>
                </div>
                {{/if}}
            </div>       
        </div>
        <div class="big_bg">
            <button class="button_gray">
                <a href="http://v.m.yiche.com/car/serial/<%= BaseSerialEntity.Id %>_0_0.html">查看更多视频</a>
            </button>
        </div>       
    </div>
    {{/if}}
    <!--左右翻页结束-->
    <!--下箭头 固定-->
    <div class="arrow_down"></div>
</script>


