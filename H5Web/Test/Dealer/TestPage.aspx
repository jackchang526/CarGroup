<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="H5Web.Test.Dealer.TestPage" %>


<%@ Import Namespace="BitAuto.CarChannel.Model" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>【<%=BaseSerialEntity.SeoName %>】车型手册-易车网</title>
    <meta charset="utf-8" />
    <meta name="Keywords" content="<%=BaseSerialEntity.SeoName %>,<%=BaseSerialEntity.SeoName %>报价,<%=BaseSerialEntity.SeoName %>图片,<%=BaseSerialEntity.SeoName %>口碑" />
    <meta name="Description" content="易车网提供<%=BaseSerialEntity.SeoName %>价格，口碑，评测，图片，易车网独家优惠。时时获得最新报价,对比选择最满意的车款，<%=BaseSerialEntity.SeoName %>优惠行情、<%=BaseSerialEntity.SeoName %>导购信息，最新<%=BaseSerialEntity.SeoName %>降价促销活动尽在易车网。" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" />
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <script type="text/javascript">
        var date= new Date();
        var version= date.getFullYear().toString()+(date.getMonth()+1).toString()+date.getDate().toString()+date.getHours().toString();
        document.write(unescape('%3Cscript type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/detectcache.js?v='+version+'"%3E%3C/script%3E'));
    </script>
    <script type="text/javascript">
        var summary = { serialId:<%=serialId%>};
    </script>
    <!-- include file="~/ushtml/0000/4th_2015_car_style-974.shtml"-->
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED%E4%BA%A7%E5%87%BA%E7%89%A9/02-%E7%A7%BB%E5%8A%A8%E5%BA%94%E7%94%A8/12-%E5%9B%9B%E7%BA%A7/%E5%89%8D%E7%AB%AF/css/jquery.fullPage.css">
    <link rel="stylesheet" type="text/css" href="http://192.168.0.10:8888/A-UED%E4%BA%A7%E5%87%BA%E7%89%A9/02-%E7%A7%BB%E5%8A%A8%E5%BA%94%E7%94%A8/12-%E5%9B%9B%E7%BA%A7/%E5%89%8D%E7%AB%AF/css/style.css">
    <script src="http://image.bitautoimg.com/carchannel/h5/js/jquery-2.1.4.min.js"></script>
    <script src="http://image.bitautoimg.com/carchannel/h5/js/jquery.fullPage.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
</head>
<body>
    <!--固定层开始-->
    <div class="fixed_box" id="logo">
        <div class="img">
            <img src="http://image.bitautoimg.com/bt/car/default/images/logo/masterbrand/png/100/m_<%=BaseSerialEntity.Brand.MasterBrandId %>_100.png" />
        </div>
        <h1><%=BaseSerialEntity.ShowName %></h1>
        <p>厂商指导价：<%= BaseSerialEntity.SaleState == "停销"?"暂无":BaseSerialEntity.ReferPrice%></p>
    </div>
    <!--固定层结束-->
    <!--菜单开始-->
    <div class="menu">
        <div class="button" id="menubutton" style="display: none"></div>
    </div>
    <div class="menu share">
        <div class="button" id="sharebutton" style="display:none;"></div>
    </div>
    <div class="standard_wx_pop" id="standard_wx_pop">
        <em></em>请点击这里，选择对应的分享方式，“发送给朋友”、“分享到朋友圈”或者“收藏”阅读。
    </div>
    <div class="menu_box_bg" id="menu_box_bg"></div>
    <div class="menu_box" id="menu_box">
        <ul>
            <li><a href="#page1">封面</a></li>
            <% if (IsExistAppearance)
               { %>
            <li data-menuanchor=""><a href="#page3">外观</a></li>
            <% }
               else
               { %>
            <li class="link_none">外观</li>
            <% } %>
            <% if (IsExistInner)
               { %>
            <li><a href="#page4">内饰</a></li>
            <% }
               else
               { %>
            <li class="link_none">内饰</li>
            <% } %>
            <% if (IsExistPingCe)
               { %>
            <li><a href="#page5">评测</a></li>
            <% }
               else
               { %>
            <li class="link_none">评测</li>
            <% } %>
            <% if (IsExistPeizhi)
               { %>
            <li><a href="#page6">配置</a></li>
            <% }
               else
               { %>
            <li class="link_none">配置</li>
            <% } %>
            <% if (IsExistKouBei)
               { %>
            <li><a href="#page7">口碑</a></li>
            <% }
               else
               { %>
            <li class="link_none">口碑</li>
            <% } %>
            <% if (IsExistBaoJia)
               { %>
            <li><a href="#page8">报价</a></li>
            <% }
               else
               { %>
            <li class="link_none">报价</li>
            <% } %>
            <li><a href="#page9">优惠</a></li>
        </ul>
    </div>
    <!--菜单结束-->
    <div id="fullpage">
        <!--第一屏开始-->
        <div class="section page1" data-anchor="page1">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <!--车型反白图-->
            <div class="standard_car_pic">
                <img src="http://image.bitautoimg.com/carchannel/pic/cspic/<%=serialId %>.jpg" />
            </div>
            <!--锚点-->
            <ul class="indexmenu">
                <% if (IsExistAppearance)
                   { %>
                <li data-menuanchor=""><a href="#page3">外观</a></li>
                <% }
                   else
                   { %>
                <li data-menuanchor="" class="link_none">外观</li>

                <% } %>
                <% if (IsExistInner)
                   { %>
                <li data-menuanchor=""><a href="#page4">内饰</a></li>
                <% }
                   else
                   { %>
                <li data-menuanchor="" class="link_none">内饰</li>
                <% } %>
                <% if (IsExistPingCe)
                   { %>
                <li data-menuanchor=""><a href="#page5">评测</a></li>
                <% }
                   else
                   { %>
                <li data-menuanchor="" class="link_none">评测</li>
                <% } %>
                <% if (IsExistPeizhi)
                   { %>
                <li data-menuanchor="" class="menu_last"><a href="#page6">配置</a></li>
                <% }
                   else
                   { %>
                <li data-menuanchor="" class="menu_last link_none">配置</li>
                <% } %>
                <% if (IsExistKouBei)
                   { %>
                <li data-menuanchor="" class="menu_2line"><a href="#page7">口碑</a></li>
                <% }
                   else
                   { %>
                <li data-menuanchor="" class="menu_2line link_none">口碑</li>
                <% } %>
                <% if (IsExistBaoJia)
                   { %>
                <li data-menuanchor=""><a href="#page8">报价</a></li>
                <% }
                   else
                   { %>
                <li data-menuanchor="" class="link_none">报价</li>
                <% } %>
                <li data-menuanchor=""><a href="#page9">优惠</a></li>
            </ul>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <!--第一屏结束-->
        <!--第二屏 颜色列表 开始-->
        <% if (IsExistColor)
           { %>
        <div class="section page2" data-anchor="page2">
            <!--换色车型图-->
            <div class="standard_car_pic standard_car_pic_1" id="standard_car_pic">
                <% for (var i = 0; i < SerialColorList.Count; i++)
                   {
                       var item = SerialColorList[i];
                %>
                <img data-src="http://image.bitautoimg.com/newsimg-600-w0-1-q80/<%=item.ImageUrl.Substring(27) %>" style="display: <%=i==0?"display":"none"%>;" />
                <% } %>
            </div>
            <!--颜色名称-->
            <div class="car_color_text" id="car_color_text">
                <%   var itemName = SerialColorList[0];
                %>
                <span><%=itemName.ColorName %></span>
            </div>
            <!--颜色切换-->
            <ul class="changecolor" id="changecolor">
                <% for (int i = 0; i < SerialColorList.Count; i++)
                   {
                       var color = SerialColorList[i];
                %>
                <li <%=i==0?"class='current'":string.Empty %>><span style="background: <%= color.ColorRGB%>;" data-value="<%=color.ColorName %>"></span></li>
                <% } %>
            </ul>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第二屏结束-->
        <!--第三屏 外观设计 开始-->
        <% 
            if (IsExistAppearance)
            { %>
        <div class="section page3" data-anchor="page3">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>外观设计</h2>
            </header>
            <!--左右翻页开始-->
            <!--左右1-->
            <% for (int i = 0; i < ExteriorList.Count; i++)
               {
                   var item = ExteriorList[i];
            %>
            <% if (ExteriorList.Count > 1 || ExteriorImageList.Count > 0)
               { %>
            <div class="slide" data-anchor="slide3-<%= i + 1 %>">
                <% } %>
                <div class="slide_box">
                    <img data-src="http://image.bitautoimg.com/newsimg-750-w0-1-q80/<%=item.ImageUrl %>" />
                    <div class="slide_con">
                        <h3><%=item.Title %></h3>
                        <p><%=item.Description %></p>
                    </div>
                </div>
                <% if (ExteriorList.Count > 1 || ExteriorImageList.Count > 0)
                   { %>
            </div>
            <% }
               } %>
            <!--左右2-->
            <% if (ExteriorImageList != null && ExteriorImageList.Count > 0)
               { %>
            <% if (ExteriorList.Count > 0)
               { %>
            <div class="slide" data-anchor="slide3-<%= (ExteriorList != null ? ExteriorList.Count : 0) + 1 %>">
                <% } %>
                <div class="con_top_bg"></div>
                <div class="big_bg">
                    <h4 class="con_box con_box_pic">图集</h4>
                    <div class="contain">
                        <ul class="piclist">
                            <% foreach (var image in ExteriorImageList)
                               {
                                   var url = string.Format("http://photo.m.yiche.com/picture/{0}/{1}/", serialId, image.ImageId);
                            %>
                            <li><a href="<%=url %>">
                                <img data-src="<%=string.Format(image.ImageUrl,4) %>" /></a></li>
                            <% } %>
                        </ul>
                    </div>
                </div>
                <% if (ExteriorList.Count > 0)
                   { %>
            </div>
            <% }
               } %>
            <!--左右3-->
            <!--左右翻页结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第三屏结束-->
        <!--第四屏 内饰设计 开始-->
        <% if (IsExistInner)
           { %>
        <div class="section page4" data-anchor="page4">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>内饰设计</h2>
            </header>
            <!--左右翻页开始-->
            <!--左右1-->
            <% for (int i = 0; i < InteriorList.Count; i++)
               {
                   var item = InteriorList[i];
            %>
            <% if (InteriorList.Count > 1 || InteriorImageList.Count > 0)
               { %>
            <div class="slide" data-anchor="slide4-<%= i + 1 %>">
                <% } %>
                <div class="slide_box">
                    <img data-src="http://image.bitautoimg.com/newsimg-750-w0-1-q80/<%=item.ImageUrl %>" />
                    <div class="slide_con">
                        <h3><%=item.Title %></h3>
                        <p><%=item.Description %></p>
                    </div>
                </div>
                <% if (InteriorList.Count > 1 || InteriorImageList.Count > 0)
                   { %>
            </div>
            <% } %>
            <% } %>
            <!--左右2-->
            <% if (InteriorImageList != null && InteriorImageList.Count > 0)
               { %>
            <% if (InteriorList.Count > 0)
               { %>
            <div class="slide" data-anchor="slide4-<%= (InteriorList != null ? InteriorList.Count : 0) + 1 %>">
                <% } %>
                <div class="con_top_bg"></div>
                <div class="big_bg">
                    <h4 class="con_box con_box_pic">图集</h4>
                    <div class="contain">
                        <ul class="piclist">
                            <% foreach (var image in InteriorImageList)
                               {
                                   var url = string.Format("http://photo.m.yiche.com/picture/{0}/{1}/", serialId, image.ImageId);
                            %>
                            <li><a href="<%=url %>">
                                <img data-src="<%=string.Format(image.ImageUrl,4) %>" /></a></li>
                            <% } %>
                        </ul>
                    </div>
                </div>
                <% if (InteriorList.Count > 0)
                   { %>
            </div>
            <% } %>
            <% } %>
            <!--左右翻页结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第四屏结束-->
        <!--第五屏 评测导购 开始-->
        <% if (IsExistPingCe)
           { %>
        <div class="section page5" data-anchor="page5">
            <header>
                <h2>评测导购</h2>
            </header>
            <!--左右1-->
            <div class="con_top_bg"></div>
            <!--内容容器开始-->
            <div class="contain">
                <% if (SerialNewsList.Count == 1)
                   {
                       var item = SerialNewsList[0];
                %>
                <a href="<%=item.PageUrl %>">
                    <img src="<%= item.CarImage %>" />
                    <h3><%= item.Title %></h3>
                    <p><%= item.PublishTime.ToString("yyyy-MM-dd") %><%=!string.IsNullOrEmpty(item.Author)?" / "+item.Author:string.Empty %></p>
                </a>
                <% }
                   else
                   { %>
                <ul class="con_list_ul">
                    <% foreach (var news in SerialNewsList)
                       {
                    %>
                    <li>
                        <a href="<%=news.PageUrl %>">
                            <div class="con_list_img">
                                <img data-src="<%=news.CarImage %>" />
                            </div>
                            <div class="con_list">
                                <h4><%=news.Title %></h4>
                                <p><%=news.PublishTime.ToString("yyyy-MM-dd") %><%=!string.IsNullOrEmpty(news.Author)?" / "+news.Author:string.Empty %></p>
                            </div>
                        </a>
                    </li>
                    <%} %>
                </ul>
                <% } %>
            </div>
            <!--内容容器结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第五屏结束-->
        <!--第六层 亮点配置 开始-->
        <% if (IsExistPeizhi)
           { %>
        <div class="section page6" data-anchor="page6">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>亮点配置</h2>
            </header>
            <div class="con_top_bg"></div>
            <!--内容容器开始-->
            <div class="contain contain_config">
                <ul class="highlight">
                    <%= PeiZhiHtml%>
                </ul>
            </div>
            <!--内容容器结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第六层结束-->
        <!--第七层 口碑列表 开始-->
        <% if (IsExistKouBei)
           { %>
        <div class="section page7" data-anchor="page7">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>精选口碑</h2>
            </header>
            <div class="con_top_bg"></div>
            <% if (!string.IsNullOrEmpty(WriterKoubei))
               { %>
            <!--左右1-->
            <% if (!string.IsNullOrEmpty(KoubeiHtml))
               { %>
            <div class="slide" data-anchor="slide7-1">
                <% } %>
                <!--大背景容器开始-->
                <div class="big_bg">
                    <h4 class="con_box">编辑口碑</h4>
                    <!--内容容器开始-->
                    <%= WriterKoubei %>
                    <!--内容容器结束-->
                </div>
                <!--大背景容器结束-->
                <% if (!string.IsNullOrEmpty(KoubeiHtml))
                   { %>
            </div>
            <% } %>
            <% } %>
            <!--左右2-->
            <% if (!string.IsNullOrEmpty(KoubeiHtml))
               { %>
            <% if (!string.IsNullOrEmpty(WriterKoubei))
               { %>
            <div class="slide" data-anchor="slide7-2">
                <% } %>
                <!--大背景容器开始-->
                <div class="big_bg">
                    <h4 class="con_box">网友口碑</h4>
                    <!--内容容器开始-->
                    <div class="contain">
                        <div class="koubei koubei_list">
                            <%= KoubeiHtml %>
                        </div>
                    </div>
                    <!--内容容器结束-->
                </div>
                <!--大背景容器结束-->
                <% if (!string.IsNullOrEmpty(WriterKoubei))
                   { %>
            </div>
            <% } %>
            <% } %>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第七层结束-->
        <!--第八层 车型报价 开始-->
        <% if (IsExistBaoJia)
           { %>
        <div class="section page8" data-anchor="page8">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>车款报价</h2>
            </header>
            <div class="con_top_bg"></div>
             <%= DealerCarListHtml %>
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第八层结束-->
        <!--第九层 优惠购车 开始-->
        <div class="section page9" data-anchor="page9">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>优惠购车</h2>
            </header>
            <div class="con_top_bg"></div>
            <%= DealerInfoHtml %>
            <!--左右1-->
            <!--大背景容器开始-->
            <div class="big_bg" id="youhuidiv" style="display:none;">
              <!--  <ul class="sale">
                    <li id="priceinfowrapper"></li>
                </ul> -->
                <a href="#" id="gouchelink">
                    <img src="http://img1.bitauto.com/uimg/4th/img/yishu.png" alt="购车" />
                </a>
            </div>
            <!--大背景容器结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <!--第九层结束-->
        <!--第十层 看了还看 开始-->
        <% if (IsExistAttention)
           { %>
        <div class="section page10" data-anchor="page10">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>经销商还卖</h2>
            </header>
            <div class="con_top_bg"></div>
            <!--大背景容器开始-->
            <%= DealerSaleHtml %>
            <!--大背景容器结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <% } %>
        <!--第十层结束-->
        <!--第十一层 精彩推荐 开始-->
        <div class="section page11" data-anchor="page11">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <header>
                <h2>经销商新闻</h2>
            </header>
            <div class="con_top_bg"></div>
            <!--大背景容器开始-->
           <%= DealerNewsHtml %>
            <!--大背景容器结束-->
            <!--下箭头 固定-->
            <div class="arrow_down"></div>
        </div>
        <!--第十一层结束-->
        <!--第十二层开始-->
        <div class="section page12" data-anchor="page12">
            <!--<div style="height: 64px; background: #000;"></div>-->
            <!--#include file="~/include/pd/2014/disiji/00001/201506_gg_gzgzh_gb2312_Manual.shtml"-->
        </div>
        <!--第十二层结束-->
    </div>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/h5/js/base.js"></script>
    <script type="text/javascript">
        var summary = { serialId:<%=serialId%>};
    </script>
    <script type="text/javascript" charset="utf-8" src="/Scripts/h5_cssummary.js?v=2015070316"></script>
    <% 
        var pages = new StringBuilder();
        pages.Append("'page1',");
        if (IsExistColor)
            pages.Append("'page2',");
        if (IsExistAppearance)
            pages.Append("'page3',");
        if (IsExistInner)
            pages.Append("'page4',");
        if (IsExistPingCe)
            pages.Append("'page5',");
        if (IsExistPeizhi)
            pages.Append("'page6',");
        if (IsExistKouBei)
            pages.Append("'page7',");
        if (IsExistBaoJia)
            pages.Append("'page8',");
        pages.Append("'page9',");
        if (IsExistAttention)
            pages.Append("'page10',");
        pages.Append("'page11',");
        pages.Append("'page12'");
    %>
    <script type="text/javascript">
        (function () {
            var mysummary = new csSummary({
                auchors: [<%=pages%>],
                serialId:<%=serialId%>
                });
        })();
    </script>
    <script type="text/javascript">
        var XCWebLogCollector = { area: '201' };
        if (typeof (bit_locationInfo) != "undefined" && typeof (bit_locationInfo.cityId) != "undefined") {
            XCWebLogCollector.area = bit_locationInfo.cityId;
        }
        if (typeof (summary) != "undefined" && typeof (summary.serialId) != "undefined") {
            XCWebLogCollector.serial = summary.serialId;
        }
    </script>
    <!--#include file="~/include/pd/2014/disiji/00001/201506_gg_tjdm_Manual.shtml"-->
</body>
</html>