<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="modules_sensor_default" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="pageBody">
    <div id="bodyLeft"></div>
    <div id="bodyLeftSwitch" class="switch-left"></div>
    <div id="bodyMain">
        <div id="bodyTitle"></div>
        <div id="bodyContent" style="overflow:hidden;"></div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("contextmenu/contextmenu.js","/js/common/")%>
<%=Public.LoadJs("page.tab.js?{0}", "/js/modules/")%>
<%=Public.LoadJsCode("default.js?{0}")%></asp:Content>