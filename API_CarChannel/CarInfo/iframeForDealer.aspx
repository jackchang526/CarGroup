<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iframeForDealer.aspx.cs" Inherits="BitAuto.CarChannelAPI.Web.CarInfo._iframeForDealer" %>
<% if(serialDealer.Length>0){ %>
var dealerHtml = '<h3><span><a href="http://dealer.bitauto.com/<%= csAllSpell %>/" target="_blank"><%= csSeoName%>-经销商推荐</a></span></h3><div class="c"><%=  serialDealer %></div><div class="clear"></div><div class="more"><a href="http://dealer.bitauto.com/<%= csAllSpell %>/" target="_blank">更多>></a></div>';
<%}else{ %>
var dealerHtml='';
<%} %>