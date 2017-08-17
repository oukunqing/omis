<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pinyin.aspx.cs" Inherits="tools_pinyin" ValidateRequest="false" %><!DOCTYPE html>
<html>
<head>
    <title>ZYRH PINYIN TOOLS</title>
    <style type="text/css">
        body{margin:0px;padding:0;font-size:12px;font-family:宋体,Arial;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        DBConnString:<asp:TextBox ID="txtConnString" runat="server" Width="800px">host=127.0.0.1;user id=root;password=123qweQWE456;database=spcp_cms;port=3306;allow zero datetime=true;charset=utf8;</asp:TextBox>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align:top;">
                    <table cellpadding="5" cellspacing="0">
                        <tr>
                            <td>
                                输入文字：<br />
                                <asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" Width="360px" Height="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnConvert" runat="server" Text="转换拼音" 
                                    onclick="btnConvert_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                编码：<br />
                                <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" Width="360px" Height="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                拼音：<br />
                                <asp:TextBox ID="txtPinyin" runat="server" TextMode="MultiLine" Width="360px" Height="120px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;">
                    <table cellpadding="5" cellspacing="0">
                        <tr>
                            <td>未识别的文字及编码：<br />
                                <asp:TextBox ID="txtUnKnown" runat="server" TextMode="MultiLine" Width="420px" Height="201px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                查找拼音：
                                <asp:DropDownList ID="ddlPinyin" runat="server" AutoPostBack="True" onselectedindexchanged="ddlPinyin_SelectedIndexChanged">
                                    <asp:ListItem Value="">请选择拼音</asp:ListItem>
                                </asp:DropDownList>
                                位置：<asp:TextBox ID="txtPinyinIndex" runat="server" Width="50px" MaxLength="3"></asp:TextBox>
                                <asp:TextBox ID="txtPinyinCopy" runat="server" Width="70px" MaxLength="16"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                表名：<asp:TextBox ID="txtTableName" runat="server">z_pinyin_special</asp:TextBox>
                                <asp:Button ID="btnWrite" runat="server" Text="写入数据库" onclick="btnWrite_Click" />

                            </td>
                        </tr>
                        <tr>
                            <td>
                                多音：<br />
                                <asp:TextBox ID="txtMultiPinyin" runat="server" TextMode="MultiLine" Width="420px" Height="72px" ForeColor="Blue"></asp:TextBox>
                                错误信息：<br />
                                <asp:TextBox ID="txtError" runat="server" TextMode="MultiLine" Width="420px" Height="72px" ForeColor="Red"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;">
                    <table cellpadding="5" cellspacing="0">
                        <tr>
                            <td>已保存的未识别文字：<asp:Label ID="lblCount" runat="server" ForeColor="Red"></asp:Label>
                                排序：<asp:DropDownList ID="ddlAsc" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddlAsc_SelectedIndexChanged">
                                    <asp:ListItem Value="code">按编码排序</asp:ListItem>
                                    <asp:ListItem Value="pinyin">按拼音排序</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:TextBox ID="txtSaveUn" runat="server" TextMode="MultiLine" Width="500px" Height="320px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnRead" runat="server" Text="读取数据" onclick="btnRead_Click" />
                                
                                每行：<asp:TextBox ID="txtCols" runat="server" Width="50px">10</asp:TextBox>个
                                <asp:Button ID="btnBuild" runat="server" Text="生成编码" onclick="btnBuild_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table cellpadding="5" cellspacing="0">
            <tr>
                <td>
                    生僻字编码：<br />
                    <asp:TextBox ID="txtSpecialCode" runat="server" TextMode="MultiLine" Width="550px" Height="150px"></asp:TextBox>
                </td>
                <td>
                    生僻字拼音：<br />
                    <asp:TextBox ID="txtSpecialPinyin" runat="server" TextMode="MultiLine" Width="600px" Height="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    生僻字汉字：<br />
                    <asp:TextBox ID="txtSpecialText" runat="server" TextMode="MultiLine" Width="550px" Height="150px"></asp:TextBox>
                </td>
                <td>

                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>