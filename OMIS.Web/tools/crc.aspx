﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="crc.aspx.cs" Inherits="tools_crc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtContent" runat="server" Width="600px"></asp:TextBox><br />
        <asp:Button ID="btnConvert" runat="server" Text="转换" onclick="btnConvert_Click" /><br />
        <asp:TextBox ID="txtCrc" runat="server" Width="600px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
