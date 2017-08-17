<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="menuList.aspx.cs" Inherits="modules_systems_menu_menuList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <form id="frmQuery" action="" method="post">
        <div id="bodyForm" class="operform">
            <select id="ddlEnabled" class="select" title="是否启用"><%=MyForm.BuildEnabledSelect(true)%></select>
            <select id="ddlOpenType" class="select" title="打开方式"><%=MyForm.BuildSelectOption("-1_请选择,0_链接跳转,1_弹出窗口,2_新窗口")%></select>
        </div>
        </form>
    </div>
    <div id="bodyBottom"><div id="pagination" class="pagination"></div></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("ofixedtable/ofixedtable.js,pagination/pagination.js", "/js/common/")%>
<%=Public.LoadJsCode("menuList.js?{0}")%></asp:Content>