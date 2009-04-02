<%@ Page Language="C#" AutoEventWireup="true" CodeFile="t.aspx.cs" Inherits="t" %>
<%@ Import Namespace="ShackToGo.Helper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ShackToGo - Thread</title>
  <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="black">
<form enableviewstate="false" id="form1" runat="server">
<a name="t"></a>
<div class="h"><a href="<%=Page.ResolveUrl("~") %><%=Helper.AppendUserName("?")%>">chatty</a> <a href="p.aspx?i=<%=Request.QueryString["i"] %>&s=<%=Request.QueryString["s"] %>&t=<%=Request.QueryString["i"] %><%=Helper.AppendUserName("&")%>">reply</a> <% if (Helper.AppendUserName("").Length > 0){ Response.Write(Helper.MarkThread(Request.QueryString["i"], Request.QueryString["s"])); %>  <a href="sm.aspx<%=Helper.AppendUserName("?") %>">marks</a> <%} %> <a href="sr.aspx<%=Helper.AppendUserName("?")%>">search</a> <% if (_threadUpdateCount > 0){ %> (<%= _threadUpdateCount%>) <%}%><% if (Helper.AppendUserName("").Length > 0){ %><a href="s.aspx<%=Helper.AppendUserName("?")%>"><img src="set.gif" /></a><%}%></div>
<asp:Repeater runat="server" ID="RepeaterPosts"><ItemTemplate><% if (_threadedView == true){%><div style="margin-left:<%#SetIndent(Convert.ToInt32(((System.Xml.XmlNode)Container.DataItem).Attributes["reply_count"].Value)) %>"><%} %><div class="p"><%if (_threadedTextView == true){%> <%#DrawTextIndent()%><%}%> <%# RenderAnchor(((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value)%> <%#Helper.AdjustforTimeZone(((System.Xml.XmlNode)Container.DataItem).Attributes["date"].Value,_timezoneOffset) %><br /><span class="<%#Helper.GetPostNameCSSClass(((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value) %>"><%#((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value %></span>&nbsp;<%#Helper.DrawCategory(((System.Xml.XmlNode)Container.DataItem).Attributes["category"].Value)%> <%#CheckNewPost(((System.Xml.XmlNode)Container.DataItem).Attributes["date"].Value)%></div>
<div class="c"><%#DoSpoilers(((System.Xml.XmlNode)Container.DataItem).FirstChild.InnerText.ToString().Trim()) %><br /><br /><a href="#t"><img src="t.gif" /></a> <a href="p.aspx?i=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value %>&s=<%=Request.QueryString["s"]%>&t=<%=Request.QueryString["i"]%><%=Helper.AppendUserName("&")%>">reply</a> <%=Helper.RenderThomW(Request.QueryString["i"], Request.QueryString["s"])%></div><div><br />&nbsp;<br /></div><% if (_threadedView == true){%></div><%} %></ItemTemplate></asp:Repeater></form>
</body>
</html>
  