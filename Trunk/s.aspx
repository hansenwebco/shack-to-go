<%@ Page Language="C#" AutoEventWireup="true" CodeFile="s.aspx.cs" Inherits="s" %>

<%@ Import Namespace="ShackToGo.Helper" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>ShackToGo - Settings</title>
  <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
  <div class="h">
    <a href="<%=Page.ResolveUrl("~") %><%=Helper.AppendUserName("?")%>">chatty</a></div>
  <asp:Panel ID="PanelForm" runat="server">
    <br />
    <asp:CheckBox ID="CheckBoxThreading" runat="server" />
    Enable threaded display
    <br />
    <br />
    <asp:CheckBox runat="server" id="CheckBoxTextThreading"></asp:CheckBox>
    Enable textual threading<br />
    (textual threading shows text indicators of thread depth for phones that
    can't render the view properly)
    
  <br /><br />
    Adjust Shack Time:
    <asp:DropDownList ID="DropDownListTimeZone" runat="server">
    </asp:DropDownList>
    <br />
    <br />
  Current Shack Time:<br />
    <%=DateTime.Now.AddHours(-1) %><br />
    <br />    
    <asp:Button Text="save" OnClick="ButtonSave_Click" ID="ButtonSave" runat="server" />
  </asp:Panel>
  <asp:Panel ID="PanelDone" Visible="false" runat="server">
    <br />
    <br />
    Settings saved. Note, threading may not render on all phone browsers.
  </asp:Panel>
  </form>
</body>
</html>
