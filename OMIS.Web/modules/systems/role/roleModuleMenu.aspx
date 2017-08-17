<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="roleModuleMenu.aspx.cs" Inherits="modules_systems_role_roleModuleMenu" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"> - <label id="lblRoleName"></label></div>
    <div id="bodyContent">
        <div id="formBody">
            <input type="hidden" id="txtId" value="<%=Public.Request("roleId|id",0) %>" />
            <div id="formBox"><div id="formContent"></div></div>
        </div>
        <div id="formBottom"></div>
    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("otreetable/otreetable.js,ofixedtable/ofixedtable.js", "/js/common/")%>
<%=Public.LoadJsCode("roleModuleMenu.js?{0}")%></asp:Content>