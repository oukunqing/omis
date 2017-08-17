<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jse.aspx.cs" Inherits="tools_jse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" Width="600px" Height="200px"></asp:TextBox><br />
        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" /><br />
        <asp:TextBox ID="txtCon" runat="server" TextMode="MultiLine" Width="600px" Height="200px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
