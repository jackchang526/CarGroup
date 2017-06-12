<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NextToSee.ascx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.UserControls.NextToSee" %>
<div class="layout-1 aftersee-section" data-channelid="2.21.825">
    <div class="section-header header2">
        <div class="box">
            <h2>接下来要看</h2>
        </div>
    </div>
    <div class="row">
        <div class="list-txt list-txt-m list-txt-default list-txt-style6">
            <ul>
                <li>
                    <div class="txt">
                        <a href="http://car.bitauto.com/<%= serialSpell %>/peizhi/">
                            <%= serialShowName%>参数配置</a>
                    </div>
                </li>
                <li>
                    <div class="txt">
                        <a href="http://car.bitauto.com/<%= serialSpell %>/tupian/">
                            <%= serialShowName%>图片</a>
                    </div>
                </li>
                <%=nextSeePingceHtml %>
                <li>
                    <div class="txt">
                        <a href="http://car.bitauto.com/<%= serialSpell %>/baojia/">
                            <%= serialShowName%>报价</a>
                    </div>
                </li>
                <li>
                    <div class="txt">
                        <a href="http://www.taoche.com/<%= serialSpell %>/">
                            <%= serialShowName%>二手车</a>
                    </div>
                </li>
                <li>
                    <div class="txt">
                        <a href="http://car.bitauto.com/<%= serialSpell %>/koubei/">
                            <%= serialShowName%>怎么样</a>
                    </div>
                </li>
                <li>
                    <div class="txt">
                        <a href="http://car.bitauto.com/<%= serialSpell %>/youhao/">
                            <%= serialShowName%>油耗</a>
                    </div>
                </li>
                <li>
                    <div class="txt">
                        <a href="<%= baaUrl %>">
                            <%= serialShowName%>论坛</a>
                    </div>
                </li>
                <%=nextSeeDaogouHtml %>
                <%= pingceTagHtml %>
            </ul>
        </div>
    </div>
</div>
