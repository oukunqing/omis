<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="menuEdit.aspx.cs" Inherits="modules_systems_menu_menuEdit" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtId" value="<%=Public.Request("menuId|id",0) %>" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td>菜单分类：</td>
                        <td>
                            <select id="ddlType" class="select"><%=MyForm.BuildSelectOption(new string[] { "模块导航菜单", "快捷导航菜单" }, 1)%></select>
                        </td>
                    </tr>
                    <tr>
                        <td class="w80">菜单名称：</td>
                        <td class="w240"><input type="text" id="txtName" class="txt w200" /></td>
                        <td class="w80">菜单编码：</td>
                        <td><input type="text" id="txtCode" class="txt w200" /></td>
                    </tr>
                    <tr>
                        <td>URL地址：</td>
                        <td colspan="3">
                            <input type="text" id="txtUrl" class="txt" style="width:560px;float:left;" />
                            <a class="btn btnc22" onclick="$('#txtUrl').attr('value','');" style="float:left;margin-left:3px;"><span>清除</span></a>
                        </td>
                    </tr>
                    <tr>
                        <td>打开方式：</td>
                        <td colspan="3">
                            <select id="ddlOpenType" class="select"><%=MyForm.BuildSelectOption(new string[] { "链接跳转", "弹出窗口","新窗口" }, 0)%></select>
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
                        <td style="vertical-align:top;">菜单图标：</td>
                        <td colspan="3">
                            <div id="divPhotoForm"></div>
                        </td>
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
<%=Public.LoadJs("page.photo.js?{0}", "/js/modules/")%>
<%=Public.LoadJsCode("menuEdit.js?{0}")%></asp:Content>