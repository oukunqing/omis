<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jscheck.aspx.cs" Inherits="tools_jscheck" ValidateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtCode" TextMode="MultiLine" Width="800px" Height="400px" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="btnCheck" runat="server" Text="检测JS语法" onclick="btnCheck_Click" />
        <br />
        <asp:Label ID="lblPrompt" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>