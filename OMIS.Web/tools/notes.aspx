<%@ Page Language="C#" AutoEventWireup="true" CodeFile="notes.aspx.cs" Inherits="tools_notes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        DAL方法 Dictionary&lt;string,object&gt; 参数说明
        <br />
        <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Width="800px" Height="200px"></asp:TextBox>
        <br />
        <asp:Button ID="Button1" runat="server" Text="注释" onclick="Button1_Click" />
        <br />
        <asp:TextBox ID="txtResult" runat="server" TextMode="MultiLine" Width="800px" Height="200px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
