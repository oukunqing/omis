<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorDeviceVersionEdit.aspx.cs" Inherits="modules_sensor_system_sensorDeviceVersionEdit" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtId" value="<%=Public.Request("versionId|id",0) %>" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td class="w80">版本编码：</td>
                        <td colspan="3"><input type="text" id="txtCode" class="txt w200" /></td>
                    </tr>
                    <tr>
                        <td class="va-top">设置通道：</td>
                        <td colspan="3">
                            <input type="hidden" id="txtVersionConfig" class="txt w200" />
                            <div id="divChannel"></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="va-top">版本描述：</td>
                        <td colspan="3">
                            <textarea id="txtDesc" class="txt w500" rows="1" cols="1" style="height:100px;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="w80">是否启用：</td>
                        <td class="w200">
                            <select id="ddlEnabled" class="select"><%=MyForm.BuildEnabledSelect()%></select>
                        </td>
                        <td class="w80">排序编号：</td>
                        <td><input type="text" id="txtSortOrder" class="txt" maxlength="6" style="width:60px;" /></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td class="w80"></td>
                        <td>
                            <div id="formContinue"></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="formBottom"></div>
    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJsCode("sensorDeviceVersionEdit.js?{0}")%></asp:Content>