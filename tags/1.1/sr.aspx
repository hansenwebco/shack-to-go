<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sr.aspx.cs" Inherits="sr" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="ShackToGo.Helper" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ShackToGo - Search</title>
   <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" enableviewstate="false" runat="server">
    <div class="h"><a href="<%=Page.ResolveUrl("~") %><%=Helper.AppendUserName("?")%>">chatty</a> <% if (Helper.AppendUserName("").Length > 0){ %> <a href="m.aspx<%=Helper.AppendUserName("?") %>">marks</a> <%} %> <a href="sr.aspx<%=Helper.AppendUserName("?")%>">search</a>
    </div>
    
    <asp:Panel runat="server" id="PanelSearchForm">
      Search For:<br />
      <asp:TextBox runat="server" id="TextBoxSearch"></asp:TextBox>  <br />
      Author:<br />
      <asp:TextBox runat="server" id="TextBoxAuthor"></asp:TextBox><br />
      Parent Author:<br />
      <asp:TextBox runat="server" id="TextBoxParentAuthor"></asp:TextBox><br/>
      <asp:Button runat="server" OnClick="ButtonSearch_Search" id="ButtonSearch" Text="Search"></asp:Button>      
      <% if (Helper.AppendUserName("").Length > 0){ %>
      <br />
      - or - <br />
      <a href="sr.aspx?s=<%=Request.QueryString["u"]%>&a=&pa=&p=1<%=Helper.AppendUserName("&") %>">Vanity Search</a><br />
      <a href="sr.aspx?s=&a=&pa=<%=Request.QueryString["u"]%>&p=1<%=Helper.AppendUserName("&") %>">Parent Author</a><br />
      <a href="sr.aspx?s=&a=<%=Request.QueryString["u"]%>&pa=&p=1<%=Helper.AppendUserName("&") %>">Your Posts</a><br />
      <%} %>
    </asp:Panel>    
    
    <asp:Panel runat="server" id="PanelSearchResults" Visible="false">
      <div class="s">
      <asp:Literal runat="server" id="LiteralPagePrev"></asp:Literal>
      <asp:Literal runat="server" id="LiteralCurrentPage"></asp:Literal>        
      <asp:Literal runat="server" id="LiteralPageNext"></asp:Literal>
      (<asp:Literal runat="server" id="LiteralResults"></asp:Literal>)
      </div>
      <asp:Repeater runat="server" ID="RepeaterPosts"><ItemTemplate><div class="p"><span class="<%#Helper.GetPostNameCSSClass(((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value) %>"><%#((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value %></span>&nbsp;<%#Helper.AdjustforTimeZone(((System.Xml.XmlNode)Container.DataItem).Attributes["date"].Value,0)%> CST<br /><%#((System.Xml.XmlNode)Container.DataItem).FirstChild.InnerText.ToString().Trim() %><br /><%#DoViewLink(((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value, ((System.Xml.XmlNode)Container.DataItem).Attributes["story_id"].Value)%></div><div><br />&nbsp;<br /></div></ItemTemplate></asp:Repeater>
    </asp:Panel>      
    </form>
</body>
</html>
