<%@ Page Language="C#" AutoEventWireup="true" CodeFile="p.aspx.cs" Inherits="p" %>
<%@ Import Namespace="ShackToGo.Helper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Post</title>
    <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="PanelPostForm" runat="server">
    <div class="h">
    <a href="<%=Page.ResolveUrl("~") %><%=Helper.AppendUserName("?")%>">chatty</a></div>
        Login:<br />
        <asp:TextBox Width="75" ID="TextBoxLogin" runat="server"></asp:TextBox><br />
        Pass:<br />
        <asp:TextBox Width="75" ID="TextBoxPassword" TextMode="Password" runat="server"></asp:TextBox>
        <asp:CheckBox runat="server" ID="CheckBoxRemember" Text="Save" />
         <br />
        Post:<br />
        <asp:TextBox runat="server" ID="TextBoxPost" TextMode="MultiLine" Columns="25" Rows="5"></asp:TextBox>
        <br />
        <asp:Button ID="ButtonSubmit" runat="server" Text="Post" OnClick="ButtonSubmit_Click" /><br />
        <asp:TextBox ID="TextBoxCode" style="font-size:x-small;" runat="server"></asp:TextBox>
        <asp:DropDownList style="font-size:x-small" runat="server" ID="DropDownShackTags">
          <asp:ListItem Value="red">red</asp:ListItem>
          <asp:ListItem Value="green">green</asp:ListItem>
          <asp:ListItem Value="blue">blue</asp:ListItem>
          <asp:ListItem Value="yellow">yellow</asp:ListItem>
          <asp:ListItem Value="lime">lime</asp:ListItem>
          <asp:ListItem Value="orange">orange</asp:ListItem>
          <asp:ListItem Value="multi">multi</asp:ListItem>
          <asp:ListItem Value="olive">olive</asp:ListItem>
          <asp:ListItem Value="italic">italic</asp:ListItem>
          <asp:ListItem Value="bold">bold</asp:ListItem>
          <asp:ListItem Value="quote">quote</asp:ListItem>
          <asp:ListItem Value="sample">sample</asp:ListItem>
          <asp:ListItem Value="under">under</asp:ListItem>
          <asp:ListItem Value="strike">strike</asp:ListItem>
          <asp:ListItem Value="spoiler">spoiler</asp:ListItem>
          
        </asp:DropDownList>
        <asp:Button runat="server" id="ButtonTag" OnCommand="ButtonTag_Command" Text="Tag"></asp:Button>        
    </asp:Panel>
    <asp:Literal ID="LiteralPostResult" runat="server"></asp:Literal>
    <br />
      <asp:Literal ID="LiteralThreadURL" runat="server"></asp:Literal>    <br />
    <asp:Literal ID="LiteralChattyURL" runat="server"></asp:Literal>
    
    </form>
</body>
</html>
