<%@ Page Language="C#" AutoEventWireup="true" CodeFile="A112.aspx.cs" Inherits="test_A112" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>

        
        <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine" Width="500px" Height="200px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
