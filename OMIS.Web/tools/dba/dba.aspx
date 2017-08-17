<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dba.aspx.cs" Inherits="tools_dba" ValidateRequest="false" %><!DOCTYPE html>
<html>
<head>
    <title>ZYRH DBA TOOLS</title>
    <style type="text/css">
        body{margin:5px; padding:0; font-size:12px; color:#555; font-family:宋体,Arial;}
        table{font-size:12px; line-height:24px;}
        table td{padding:0; height:24px; line-height:24px;}
        table td input{margin-right:5px; font-size:12px; color:#555;}
        b{color:#f00;}
        i{font-size:14px; color:#555; font-weight:bold; font-style:normal;}
        a{color:#00f; cursor:pointer;}
        input{border:solid 1px #ccc; height:18px; line-height:18px;}
        select{border:solid 1px #ccc; color:#555; height:22px; line-height:22px;}
        textarea {font-size:12px;line-height:1.25em; background:#fafafa; border:solid 1px #ccc; color:#666; font-family:宋体,Arial; min-height:50px;resize:vertical;}
        textarea.flag{border:solid 1px #00f;}
        textarea.flag1{border:solid 1px #f00;}
        
        .btn{height:22px; padding:0 10px; line-height:18px;}
        .blue{background:#005eac; border:solid 1px #00f; color:#fff; font-family:宋体,Arial;}
        .green{background:#008000; border:solid 1px #000;color:#fff; font-family:宋体,Arial;}
        .yellow{background:#fe0; border:solid 1px #f00; color:#555; font-family:宋体,Arial;}
        
        .gv{min-width:700px;}
        .gv span{padding:0 2px;}
        .gv div{padding:0 2px; line-height:18px; color:#666;}
        
        .tbform{}
        .tbform td{height:24px;}
        
        .tbdata{}
        .tbdata td{vertical-align:top;}
        
        .box{border:solid 1px #ccc; overflow:auto; padding: 0 5px;}
        .box p{}
        
        .div-txt{margin:0;padding:0;height:25px;line-height:25px;float:left;clear:both;}
        
        .tbdata td .txt{margin:0;padding:0;float:left;}
        
        .chb{margin-top:0px;}
        .chb input{float:left;}
        .chb label{float:left;}
        
        .chb-label{display:block;}
        .chb-label .chb{float:left; margin:4px 2px 0 5px;}
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-bottom:100px;">
        <table cellpadding="0" cellspacing="0" class="tbform">
            <tr>
                <td>Host:</td>
                <td><asp:TextBox ID="txtHost" runat="server" Width="120px">127.0.0.1</asp:TextBox></td>
                <td>Port:</td>
                <td><asp:TextBox ID="txtPort" runat="server" Width="45px" MaxLength="5">3306</asp:TextBox></td>
                <td>User Id:</td>
                <td><asp:TextBox ID="txtUser" runat="server" Width="120px">root</asp:TextBox></td>
                <td>Password:</td>
                <td><asp:TextBox ID="txtPwd" runat="server" Width="120px">123qweQWE456</asp:TextBox></td>
                <td>Database:</td>
                <td><asp:TextBox ID="txtDatabase" runat="server" Width="120px">spcp_dev</asp:TextBox></td>
                <td>Charset:</td>
                <td><asp:TextBox ID="txtCharset" runat="server" Width="50px">utf8</asp:TextBox></td>
                <td><asp:Button ID="btnConnect" CssClass="btn yellow" runat="server" Text="Connect" onclick="btnConnect_Click" /></td>
                <td><asp:Label ID="lblTable" runat="server"></asp:Label></td>
                <td>
                    <asp:DropDownList ID="ddlDbType" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlDbType_SelectedIndexChanged">
                    <asp:ListItem Value="1">MySQL</asp:ListItem>
                    <asp:ListItem Value="2">MsSQL</asp:ListItem>
                    <asp:ListItem Value="3">Oracle</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Tables:</td>
                <td colspan="12">
                    <asp:DropDownList ID="ddlTable" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddlTable_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Label ID="lblField" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <div id="divError" runat="server" style="color:#f00;"></div>
        <table cellpadding="0" cellspacing="0" class="tbdata">
            <tr>
                <td style="width:450px;padding-top:5px;">
                    <span style="float:left;"><i>Fields</i>:</span>
                    <input type="button" class="btn blue" value="Select All" onclick="selectDataRow(false);" style="float:left;" />
                    <input type="button" class="btn blue" value="Select Invert" onclick="selectDataRow(true);"  style="float:left;" />
                    <asp:TextBox ID="txtRowsId" Visible="true" runat="server" style="visibility:hidden;width:36px;"></asp:TextBox>
                    <div style="width:500px; margin-top:5px; overflow:auto;background:#f8f8f8;">
                        <asp:GridView ID="gvField" CssClass="gv" runat="server" BorderWidth="0" BorderStyle="None" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <input type="button" class="btn blue" style="padding:0 5px;float:left;" value="A" onclick="selectDataRow(false);" />
                                    <input type="button" class="btn blue" style="padding:0 5px;float:left;" value="I" onclick="selectDataRow(true);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <label class="chb-label" for="chbRow<%#Container.DataItemIndex%>">
                                        <input type="checkbox" class="chb" name="chbRow" lang="<%#Eval("Extra")%>" id="chbRow<%#Container.DataItemIndex%>" value="<%#Container.DataItemIndex%>" onclick="getCheckedRow(this);" />
                                        <span><%#Container.DataItemIndex+1%></span>
                                    </label>
                                </ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Field</HeaderTemplate>
                                <ItemTemplate>
                                    <span><%#Eval("Field")%></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Type</HeaderTemplate>
                                <ItemTemplate>
                                    <span><%#Eval("Type")%></span>
                                </ItemTemplate>
                                <ItemStyle Width="85px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Null</HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align:center;"><%#Eval("Null")%></div>
                                </ItemTemplate>
                                <ItemStyle Width="35px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Key</HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align:center;"><%#Eval("Key")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>Default</HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align:center;"><%#Eval("Default")%></div>
                                </ItemTemplate>
                                <ItemStyle Width="55px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span style="text-align:left; float:left;">Comment</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div><%#Eval("Comment")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <span style="text-align:left; float:left;">Extra</span>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div><%#Eval("Extra")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate></HeaderTemplate>
                                <ItemTemplate></ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>                        
                    </div>
                    <i>Data Operate(DAL)</i>:
                    <br />
                    <asp:TextBox ID="txtDataOperateDAL" CssClass="txt flag1" runat="server" Width="498px" Height="200px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                    <i>Data Operate(BLL)</i>:
                    <br />
                    <asp:TextBox ID="txtDataOperateBLL" CssClass="txt flag1" runat="server" Width="498px" Height="180px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                </td>
                <td style="padding-left:10px;">
                    <table cellpadding="0" cellspacing="0" class="tbdata">
                        <tr>
                            <td colspan="2">
                                <span style="float:left;">Class Comment:</span>
                                <asp:TextBox ID="txtClassComment" runat="server" Width="150px" style="float:left;" spellcheck="false"></asp:TextBox>
                                <span style="float:left;">Class Name:</span>
                                <asp:TextBox ID="txtClassName" runat="server" Width="180px" style="float:left;" spellcheck="false"></asp:TextBox>
                                <asp:Button ID="btnBuild" CssClass="btn green" runat="server" Text="Build Data" onclick="btnBuild_Click" style="float:left;" />
                                <asp:CheckBox ID="chbCutFirstPrefix" runat="server" Text="Remove first prefix" 
                                    AutoPostBack="True" oncheckedchanged="chbCutFirstPrefix_CheckedChanged" CssClass="chb" style="font-weight:bold;font-size:14px;" />  
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div-txt"><i>ClassInfo</i>:</div>
                                <asp:TextBox ID="txtClassData" CssClass="txt flag1" runat="server" Width="400px" Height="180px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <div class="div-txt" style="width:400px;">
                                    <span style="float:left;"><i>Fill Data</i>:</span>
                                    <asp:CheckBox ID="chbOnlyField" runat="server" Text="Only show field convert" 
                                        AutoPostBack="True" oncheckedchanged="chbOnlyField_CheckedChanged" CssClass="chb" style="float:left;margin-left:98px; font-weight:bold;font-size:14px;" />
                                </div>
                                <div class="div-txt">Info Name:<asp:TextBox ID="txtInfoName" runat="server" 
                                        Width="60px">o</asp:TextBox>
                                <!--Convert Func:<asp:TextBox ID="txtConvertFunc1" runat="server" Width="180px">DataConvert.ConvertValue</asp:TextBox>-->
                                Convert Func:<asp:TextBox ID="txtConvertFunc" runat="server" Width="180px">ConvertValue</asp:TextBox>
                                </div>
                                <asp:TextBox ID="txtFillData" CssClass="txt flag1" runat="server" Width="400px" Height="155px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtDBConnString" Visible="false" runat="server">DBConnectionString</asp:TextBox>                            
                            </td>                            
                            <td style="padding-left:10px;">
                                <asp:TextBox ID="txtDBConnString1" runat="server" style="float:left;width:100px;margin-right:50px;">DBConnString</asp:TextBox>
                                <asp:TextBox ID="txtConvertTime" runat="server" Width="150px" style="float:left;margin-right:10px;">ConvertDateTime</asp:TextBox>
                                <asp:CheckBox ID="chbConvertTime" runat="server" Text="转换时间" Checked="true" AutoPostBack="True" CssClass="chb" oncheckedchanged="chbConvertTime_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i>Get Single Info</i>:<br />
                                <asp:TextBox ID="txtGetInfo1" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <i>Get Multi Info</i>:<br />
                                <asp:TextBox ID="txtGetInfo2" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i>Get Info</i>:<br />
                                <asp:TextBox ID="txtGetInfo3" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <i>Add Method</i>:<br />
                                <asp:TextBox ID="txtAddMethod" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i>Update Method</i>:<asp:TextBox ID="txtUpdateMethod" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                                <br />
                            </td>
                            <td style="padding-left:10px;">
                                <i>Delete Method</i>:<br />
                                <asp:TextBox ID="txtDelMethod" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><i>Get Level</i>:<br />
                                <asp:TextBox ID="txtGetLevel" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;"><i>Update ParentTree</i>:<br />
                                <asp:TextBox ID="txtUpdateParentTree" CssClass="txt flag1" runat="server" 
                                    Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td><i>Add Sql</i>:<br />
                                <asp:TextBox ID="txtAddSql" CssClass="txt" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;"><i>Update Sql</i>:<br />
                                <asp:TextBox ID="txtUpdateSql" CssClass="txt" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <i>Add Sql Param</i>:<br />
                                <asp:TextBox ID="txtAddSqlParam" CssClass="txt flag" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <i>Add Sql Param-1</i>:<br />
                                <asp:TextBox ID="txtAddSqlParam1" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td>
                                <i>Update Sql Param</i>:<br />
                                <asp:TextBox ID="txtUpdateSqlParam" CssClass="txt flag" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <i>Update Sql Param-1</i>:<br />
                                <asp:TextBox ID="txtUpdateSqlParam1" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i>Get Single Info(BLL)</i>:<br />
                                <asp:TextBox ID="txtGetInfo11" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <i>Get Multi Info(BLL)</i>:<br />
                                <asp:TextBox ID="txtGetInfo22" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i>Get Info(BLL)</i>:<br />
                                <asp:TextBox ID="txtGetInfo33" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;">
                                <i></i>Check Data Is Exist:<br />
                                <asp:TextBox ID="txtCheckData" CssClass="txt flag1" runat="server" Width="400px" Height="80px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><i>Add Sql String</i>:<br />
                                <asp:TextBox ID="txtAddSqlString" CssClass="txt" runat="server" Width="812px" Height="100px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><i>Batch Add Sql String</i>:<br />
                                <asp:TextBox ID="txtAddSqlStringBatch" CssClass="txt" runat="server" Width="812px" Height="100px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td><i>Json Format</i>:<br />
                                <asp:TextBox ID="txtJsonFormat" CssClass="txt" runat="server" Width="400px" Height="200px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                            <td style="padding-left:10px;"><i>Json Build</i>:<br />
                                <asp:TextBox ID="txtJsonBuild" CssClass="txt" runat="server" Width="400px" Height="200px" TextMode="MultiLine" spellcheck="false"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function selectDataRow(isInvert) {
        var arr = document.getElementsByName('chbRow');
        for (var i in arr) {
            if (arr[i].lang == 'auto_increment') {
                arr[i].checked = true;
            } else {
                arr[i].checked = isInvert ? !arr[i].checked : true;
            }
        }
        getCheckedRow(null);
    }

    function getCheckedRow(obj) {
        if (obj != null && obj.lang == 'auto_increment') {
            obj.checked = true;
        }
        var strIds = '';
        var arr = document.getElementsByName('chbRow');
        var n = 0;
        for (var i in arr) {
            if (undefined == arr[i].value) {
                continue;
            }
            if (arr[i].checked) {
                strIds += n > 0 ? ',' : '';
                strIds += arr[i].value;
                n++;
            }
        }
        document.getElementById('txtRowsId').value = strIds;
    }

    function checkRow() {
        var strIds = document.getElementById('txtRowsId').value;
        if ('' == strIds) {
            return false;
        }
        var arr = strIds.split(',');
        var arrObj = document.getElementsByName('chbRow');

        for (var i = 0, c = arrObj.length; i < c; i++) {
            for (var j = 0; j < arr.length; j++) {
                if (arrObj[i].value == arr[j]) {
                    arrObj[i].checked = true;
                    arr.splice(j, 1);
                    break;
                }
            }
        }
    }

    window.onload = function () {
        checkRow();
    };
</script>