<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsonformat.aspx.cs" Inherits="tools_json_jsonformat" %>
<!DOCTYPE html>
<html>
<head>
    <title>JSON格式化</title>
    <style type="text/css">
        body{font-size:12px;}
        #divJson{overflow:auto; max-width:800px;max-height:600px; border:solid 1px #999; padding:5px 15px; line-height:20px; font-size:12px; font-family:Arial,宋体;}
    </style>
</head>
<body>
    <form runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="vertical-align:top;">
                    <asp:TextBox ID="txtJson" runat="server" TextMode="MultiLine" Width="500px" Height="300px"></asp:TextBox>
                    <br />
                    <input id="Submit1" type="submit" value="格式化" onclick="jsonFormat();" />
                </td>
                <td style="vertical-align:top;">
                    <div id="divJson" style="min-width:300px"></div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<script type="text/javascript" src="../../js/common/jquery.js"></script>
<script type="text/javascript" src="../../js/common/jsonformat.js"></script>
<script type="text/javascript">
    $(window).load(function () {
        jsonFormat();
    });

    function jsonFormat() {
        var con = $('#txtJson').val();
        if (con.length > 0) {
            var result = new JSONFormat(con, 4).toString();
            $('#divJson').html(result);
        }
    }
</script>
