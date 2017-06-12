<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SummaryTreeNode.aspx.cs" Inherits="BitAuto.CarChannelAPI.Web.Tree.SummaryTreeNode" %>
if(document.getElementById("curObjTreeNode") != null && <%=_masterId %>>0)
{
    document.getElementById("curObjTreeNode").innerHTML = '<%=string.Concat(htmlList.ToArray()) %>';
    scrollToCurrentTreeNode();
}