<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="ShackToGo.Helper" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="headtag">
  <title>ShackToGo</title>
  <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="black">
<form id="form1" runat="server" enableviewstate="False">
<a name="t"></a>
<div class="s"><asp:Literal runat="server" id="LiteralStoryTitle"></asp:Literal></div>
<div class="h"><a href="p.aspx?s=<%=_storyID %><%=Helper.AppendUserName("&") %>">new</a> <a href="<%=Helper.AppendUserName("?")%>">refresh</a> <% if (Helper.AppendUserName("").Length > 0) { %> <a href="m.aspx<%=Helper.AppendUserName("?") %>">marks</a> <%} %> <a href="sr.aspx<%=Helper.AppendUserName("?") %>">search</a> <% if (_threadUpdateCount > 0){ %> (<%= _threadUpdateCount%>) <%}%> <% if (Helper.AppendUserName("").Length > 0){ %><a href="s.aspx<%=Helper.AppendUserName("?")%>"><img src="set.gif" /></a><%}%></div>
<asp:Repeater runat="server" ID="RepeaterPosts"><ItemTemplate>
<div class="p <%#((System.Xml.XmlNode)Container.DataItem).Attributes["category"].Value%>"><%#Helper.AdjustforTimeZone(((System.Xml.XmlNode)Container.DataItem).Attributes["date"].Value,_timezoneOffset)%><br /><span class="<%#Helper.GetPostNameCSSClass(((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value) %>"><%#((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value%></span>&nbsp;<%#CheckNewPost(((System.Xml.XmlNode)Container.DataItem).Attributes["date"].Value)%> <%#Helper.DrawCategory(((System.Xml.XmlNode)Container.DataItem).Attributes["category"].Value)%></div>
<div class="c"><%#((System.Xml.XmlNode)Container.DataItem).FirstChild.InnerText.ToString().Trim()%><br /><br />
<a href="#t"><img src="t.gif" /></a>&nbsp;<%#DoReplyLink(Convert.ToInt32(((System.Xml.XmlNode)Container.DataItem).Attributes["reply_count"].Value), Convert.ToInt32(((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value))%> <a href="p.aspx?i=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value %>&s=<%=_storyID %>&t=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value %><%=Helper.AppendUserName("&") %>">reply</a> <%#Helper.MarkThread(((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value,_storyID.ToString())%>  <%#Helper.RenderThomW(((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value,_storyID.ToString())%></div><div><br />&nbsp;<br /></div></ItemTemplate></asp:Repeater>
<asp:Literal runat="server" id="LiteralPages"></asp:Literal>
<br /><br /></form></body></html>
