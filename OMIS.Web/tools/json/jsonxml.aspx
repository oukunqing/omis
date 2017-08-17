<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsonxml.aspx.cs" Inherits="test_jsonxml" ValidateRequest="false" %>
<!DOCTYPE html>
<html>
<head>
    <title>JSON转换XML</title>
    <style type="text/css">
        body{font-size:12px; font-family:宋体,Arial;}
        #divJson{border:solid 1px #999;padding:0 10px; font-size:12px; font-family:宋体,Arial; line-height:16px;width:580px;height:304px; overflow:auto;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align:top;">
                    <asp:TextBox ID="txtJson1" runat="server" TextMode="MultiLine" Width="600px" Height="120px"></asp:TextBox>
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="JsonToXml" onclick="Button1_Click" />
                    <br />
                    <asp:TextBox ID="txtXml1" runat="server" TextMode="MultiLine" Width="600px" Height="120px"></asp:TextBox>
                    <br />
                    <asp:Button ID="Button3" runat="server" Text="XmlFormat" onclick="Button3_Click" />
                    <br />
                    <asp:TextBox ID="txtXml11" runat="server" TextMode="MultiLine" Width="600px" Height="300px"></asp:TextBox>
                </td>
                <td style="vertical-align:top;">
                    <asp:TextBox ID="txtXml2" runat="server" TextMode="MultiLine" Width="600px" Height="120px"></asp:TextBox>
                    <br />
                    <asp:Button ID="Button2" runat="server" Text="XmlToJson" onclick="Button2_Click" />
                    <br />
                    <asp:TextBox ID="txtJson2" runat="server" TextMode="MultiLine" Width="600px" Height="120px"></asp:TextBox>
                    <br />
                    <input id="Submit1" type="submit" value="JsonFormat" onclick="jsonFormat();" />
                    <br />
                    <div id="divJson" style=""></div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="../../js/common/jquery.js"></script>
<script type="text/javascript" src="json.js"></script>
<script type="text/javascript">
    $(window).load(function () {
        jsonFormat();
    });

    function jsonFormat() {
        var con = $('#txtJson2').val();
        if (con.length > 0) {
            var result = new JSONFormat(con, 4).toString();
            $('#divJson').html(result);
        }
    }
</script>