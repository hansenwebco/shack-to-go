<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sm.aspx.cs" Inherits="sm" %>
<%@ Import Namespace="ShackToGo.Helper" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ShackMarks</title>
   <link href="main.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
     <div class="h">
    <a href="<%=Page.ResolveUrl("~") %><%=Helper.AppendUserName("?")%>">chatty</a></div>
      <asp:Repeater runat="server" ID="RepeaterBookMarks">
        <ItemTemplate>
          <div class="p">Posted: <%#Helper.AdjustforTimeZone(((System.Xml.XmlNode)Container.DataItem).Attributes["date"].Value, _timezoneOffset)%><br /><span class="a"><%#((System.Xml.XmlNode)Container.DataItem).Attributes["author"].Value%></span>
          <div class="c"><%#FormatBookMark(((System.Xml.XmlNode)Container.DataItem).FirstChild.InnerText.ToString().Trim())%><br /><br />
              <a href="t.aspx?i=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value%>&s=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value%><%=Helper.AppendUserName("&") %>&j=t#<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value%>"> view</a> <a href="sm.aspx?c=d&i=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value%>&s=<%#((System.Xml.XmlNode)Container.DataItem).Attributes["id"].Value%><%=Helper.AppendUserName("&") %>"> delete</a></div>
          </div><div><br />&nbsp;<br />
          </div>
        </ItemTemplate>
      </asp:Repeater>
      <asp:Literal ID="LiteralError" runat="server"></asp:Literal>
    </form>
</body>
</html>
