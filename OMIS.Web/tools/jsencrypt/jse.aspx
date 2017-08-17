<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jse.aspx.cs" Inherits="tools_jsencrypt_jse" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>JS代码加密工具</title>
    <style type="text/css">
        body{margin:0;padding:0; font-size:12px; font-family:宋体,Arial;}
    </style>
</head>
<body>
<form action="" method="post" runat="server">
    <div style="padding:5px;clear:both;">
        <div style="float:left;width:580px;">
            <div style="height:30px;">
                根目录：<asp:TextBox ID="txtRootDir" runat="server" Width="150px">/modules</asp:TextBox>
                <asp:CheckBox ID="chbJs" runat="server" Text="显示JS文件" Checked="true" />
                <asp:CheckBox ID="chbSource" runat="server" Text="显示源文件" Checked="true" />
                <asp:CheckBox ID="chbBak" runat="server" Text="显示备份文件" />
                <asp:Button ID="btnRead" runat="server" Text="读取文件" onclick="btnRead_Click" />
            </div>
            <div id="tree" style="width:560px;height:550px; overflow:auto; padding:5px; border:solid 1px #ccc;"></div>
        </div>
        <div style="float:left;width:740px;">
            <div style="height:30px;">
            JS代码加密
            </div>
            <iframe id="frmEncrypt" style="width:730px;height:560px;border:solid 1px #00f;"  src="jsencrypt.aspx"></iframe>
        </div>
    </div>
</form>
</body>
</html>
<%=Public.LoadJs("jquery.js?v1.7.2,common.js,cms.util.js,cms.jquery.js,popwin/popwin.js,cms.box.js,otree/otree.js", "/js/common/")%>
<script type="text/javascript">
    var JsonData = <%=JsonData%>;
</script>
<script type="text/javascript">
    var pd = {};
    $(window).ready(function () {
        showFileTree();
    });

    function initialData(list) {
        var arr = [];

        for (var i in list) {
            list[i].childs = [];
            arr[list[i].path] = list[i];
        }

        for (var i in arr) {
            arr[i].pid = getParentId(arr, arr[i].parent);
            if (arr[i].pid > 0) {
                arr[arr[i].parent].childs.push(arr[i].id);
            }
        }

        return arr;
    }

    function showFileTree() {
        if (typeof JsonData != 'object') {
            return false;
        }
        var list = initialData(JsonData);

        pd.o = new oTree('pd.o', $I('tree'), {
            isCache: true,
            loadedCallback: function (objTree) {
                //objTree.openLevel(1);
            }
        });

        for (var i in list) {
            var dr = list[i];
            var icon = dr.type == 0 ? '' : 'page.gif';
            var callback = dr.type == 1 ? function (param, objTree) {
                if (param.type == 1) {
                    loadPage(param.name);
                }
            } : null;

            //显示文件和不为空的目录
            if (dr.type == 1 || dr.childs.length > 0) {
                pd.o.add({ type: dr.type, id: dr.id, pid: dr.pid, name: dr.path, icon: icon, callback: callback });
            }
        }
    }

    function getParentId(list, path) {
        if (list[path] != undefined) {
            return list[path].id;
        }
        return 0;
    }

    function loadPage(filePath) {
        $('#frmEncrypt').attr('src', 'jsencrypt.aspx?filePath=' + filePath + '&' + new Date().getTime());
    }
</script>