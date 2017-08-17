﻿<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="roleEdit.aspx.cs" Inherits="modules_systems_role_roleEdit" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtId" value="<%=Public.Request("roleId|id",0) %>" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td>角色组别：</td>
                        <td>
                            <select id="ddlGroup" class="select"></select>
                        </td>
                    </tr>
                    <tr>
                        <td class="w80">角色名称：</td>
                        <td class="w240"><input type="text" id="txtName" class="txt w200" /></td>
                        <td class="w80">角色编码：</td>
                        <td><input type="text" id="txtCode" class="txt w200" /></td>
                    </tr>
                    <tr>
                        <td>角色描述：</td>
                        <td colspan="3">
                            <input type="text" id="txtDesc" class="txt" style="width:510px;" />
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
<%=Public.LoadJsCode("roleEdit.js?{0}")%></asp:Content>