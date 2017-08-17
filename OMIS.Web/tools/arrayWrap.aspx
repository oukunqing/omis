<%@ Page Language="C#" AutoEventWireup="true" CodeFile="arrayWrap.aspx.cs" Inherits="test_arrayWrap" ValidateRequest="false" %><!DOCTYPE html>
<html>
<head>
    <title>ZYRH ARRAY WRAP TOOLS</title>
    <style type="text/css">
        body{margin:0px;padding:0;font-size:12px;font-family:宋体,Arial;}
        .tbform{}
        .tbform td{height:25px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="5" cellspacing="0" style="width:96%;">
            <tr>
                <td style="width:500px;">
                    输入要转换的内容：<br />
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="600px" Height="160px"></asp:TextBox>
                </td>
                <td style="vertical-align:top; text-align:left;">
                    <table cellpadding="0" cellspacing="0" class="tbform">
                        <tr>
                            <td><b>预处理内容：</b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDelRow" runat="server" Text="清除换行" onclick="btnDelRow_Click" />
                                <asp:Button ID="btnDelRow2" runat="server" Text="换行替换为," onclick="btnDelRow2_Click" />
                                <asp:Button ID="btnDelRow1" runat="server" Text="换行替换为|" onclick="btnDelRow1_Click" />
                                
                                要保留的空格数：<asp:TextBox ID="txtSpaceCount" runat="server" Width="40px"></asp:TextBox>
                                <asp:Button ID="btnClearSpace" runat="server" Text="保留空格" onclick="btnClearSpace_Click" />                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                替换：<asp:TextBox ID="txtReplace1" runat="server" style="width:60px;">&nbsp;</asp:TextBox>
                                为
                                <asp:TextBox ID="txtReplace2" runat="server" style="width:60px;">,</asp:TextBox>
                                <asp:Button ID="btnClear" runat="server" Text="预处理" onclick="btnClear_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td><b>转换内容：</b></td>
                        </tr>
                        <tr>
                            <td>
                                每行：<asp:DropDownList ID="ddlCols" runat="server" AutoPostBack="True" onselectedindexchanged="ddlCols_SelectedIndexChanged"></asp:DropDownList>
                                个
                                <asp:CheckBox ID="chbString" runat="server" Text="字符串" Checked="true" AutoPostBack="True" oncheckedchanged="chbString_CheckedChanged" />
                                <asp:CheckBox ID="chbSpace" runat="server" Text="间隔空一格" Checked="true" AutoPostBack="True" oncheckedchanged="chbSpace_CheckedChanged" />
                                <asp:CheckBox ID="chbDistinct" runat="server" Text="去除重复" AutoPostBack="True" 
                                    oncheckedchanged="chbDistinct_CheckedChanged"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btnConvert" runat="server" Text="开始转换" onclick="btnConvert_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>            
            <tr>
                <td colspan="2">
                    转换后的内容：<br />
                    <asp:TextBox ID="txtResult" runat="server" Width="100%" Height="400px" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
