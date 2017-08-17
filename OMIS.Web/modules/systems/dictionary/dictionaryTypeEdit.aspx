<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="dictionaryTypeEdit.aspx.cs" Inherits="modules_systems_dictionary_dictionaryTypeEdit" %>
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
                        <td>最大编号：</td>
                        <td>
                            <input type="text" id="txtMaxNumber" class="txt" maxlength="8" style="width:80px;" disabled="disabled" />
                            <span class="explain">自动生成，不可修改</span>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>是否多选：</td>
                        <td>
                            <select id="ddlMultiSelect" class="select"><%=MyForm.BuildSelectOption(new string[] { "单选", "多选"}, 0)%></select>
                        </td>
                        <td>多选限制：</td>
                        <td><select id="ddlMultiSelectLimit" class="select"><%=MyForm.BuildSelectOption(1,100,1)%><%=MyForm.BuildSelectOption("-1,不限数量")%></select>
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
                    <tr>
                        <td>备注：</td>
                        <td colspan="3"><input type="text" id="txtRemark" class="txt" style="width:510px;" /></td>
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
<%=Public.LoadJsCode("dictionaryTypeEdit.js?{0}")%></asp:Content>