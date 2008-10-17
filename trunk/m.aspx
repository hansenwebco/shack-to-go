<%@ Page Language="C#" AutoEventWireup="true" CodeFile="m.aspx.cs" Inherits="m" %>
<%@ Import Namespace="ShackToGo.Helper" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Book Marks</title>
  <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body bgcolor="black">
  <form id="form1" enableviewstate="false" runat="server">
  <div class="h">
    <a href="<%=Page.ResolveUrl("~") %><%=Helper.AppendUserName("?")%>">chatty</a></div>
  <asp:Repeater runat="server" ID="RepeaterBookMarks">
    <ItemTemplate>
      <div class="p">Posted: <%#Helper.AdjustforTimeZone(Eval("b.PostCreated").ToString(), _timezoneOffset)%><br /><span class="a"><%#Eval("b.PosterName") %></span>
      <div class="c"><%#FormatBookMark(Eval("b.Desc").ToString())%><br /><br />
          <a href="t.aspx?i=<%#Eval("b.ThreadID")%>&s=<%#Eval("b.StoryID")%><%=Helper.AppendUserName("&") %>"> view</a> <a href="m.aspx?c=d&i=<%#Eval("b.ThreadID")%>&s=<%#Eval("b.StoryID")%><%=Helper.AppendUserName("&") %>"> delete</a></div>
      </div><div><br />&nbsp;<br />
      </div>
    </ItemTemplate>
  </asp:Repeater>
  <asp:Literal ID="LiteralError" runat="server"></asp:Literal>
  </form>
</body>
</html>
