<%@ Page Language="C#" AutoEventWireup="true" CodeFile="uploadPhoto.aspx.cs" Inherits="modules_upload_uploadPhoto" %><!DOCTYPE html>
<html>
<head>
    <title><%=title%></title>
    <link rel="stylesheet" type="text/css" href="<%=Config.WebDir%>/skin/css/page.css" />
    <%=Public.LoadJs("jquery.js?v1.7.2,popwin/popwin.js,cms.box.js", "/js/common/")%>
    <%=Public.LoadJs("module.js,page.js", "/js/modules/")%>
    <script type="text/javascript">
        var title = '<%=title%>', name = '<%=name%>';
    </script>
    <%=Public.LoadJsCode("uploadPhoto.js?{0}")%>
</head>
<body>
    <form id="frmUpload" runat="server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtDir" runat="server" />
                <input type="hidden" id="txtType" runat="server" />
                <input type="hidden" id="txtWidth" runat="server" />
                <input type="hidden" id="txtHeight" runat="server" />
                <input type="hidden" id="txtThumbWidth" runat="server" />
                <input type="hidden" id="txtThumbHeight" runat="server" />
                <input type="hidden" id="txtDeleteSource" runat="server" />
                <input type="hidden" id="txtThumbMode" runat="server" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td>
                            请选择<%=name%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:FileUpload ID="fuPhoto" runat="server" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" class="tbform" id="tbAction" style="margin-top:5px;">
                    <tr>
                        <td>
                            <label class="chb-label-nobg"><input type="checkbox" id="chbThumb" runat="server" class="chb" /><span>生成缩略图</span></label>

                            <a class="btn btnc24" id="button" onclick="formSubmit();" style="margin-left:20px;"><span class="w60">上传</span></a>
                            <asp:Button ID="btnUpload" runat="server" Text="上传" onclick="btnUpload_Click" style="visibility:hidden;" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="formBottom"></div>
    </div>
    </form>
    <span id="lblPrompt" runat="server" class="prompt"></span>
    <span id="lblResponse" runat="server" class="prompt"></span>
</body>
</html>