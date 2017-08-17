<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sensor.aspx.cs" Inherits="test_sensor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="txtData" runat="server" TextMode="MultiLine" Width="600px" Height="300px"></asp:TextBox>
        <br />
        <asp:Button ID="btnUpload" runat="server" Text="上传数据" 
            onclick="btnUpload_Click" />
    
    </div>
    </form>
</body>
</html>
