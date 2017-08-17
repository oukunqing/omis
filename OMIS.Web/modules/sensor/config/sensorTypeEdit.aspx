<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorTypeEdit.aspx.cs" Inherits="modules_sensor_config_sensorTypeEdit" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtId" value="<%=Public.Request("typeId|id",0) %>" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td>上级分类：</td>
                        <td colspan="3">
                            <input type="text" id="txtParentId" class="txt w200" readonly="readonly" lang="<%=Public.Request("parentId|pid",0) %>" />
                        </td>
                    </tr>
                    <tr>
                        <td class="w80">分类名称：</td>
                        <td class="w240"><input type="text" id="txtName" class="txt w200" /></td>
                        <td class="w80">分类编码：</td>
                        <td><input type="text" id="txtCode" class="txt w200" /></td>
                    </tr>
                    <tr>
                        <td>数据单位：</td>
                        <td>
                            <input type="text" id="txtUnit" class="txt w200" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="va-top">功能说明：</td>
                        <td colspan="3">
                            <textarea id="txtTypeFunc" class="txt" style="width:520px;height:50px;" cols="1" rows="1"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class="va-top">分类描述：</td>
                        <td colspan="3">
                            <textarea id="txtTypeDesc" class="txt" style="width:520px;height:80px;" cols="1" rows="1"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>是否启用：</td>
                        <td>
                            <select id="ddlEnabled" class="select"><%=MyForm.BuildEnabledSelect()%></select>
                        </td>
                        <td>排序编号：</td>
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
<%=Public.LoadJsCode("sensorTypeEdit.js?{0}")%></asp:Content>