<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="modules_emap_track_default" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="pageBody">
    <div id="bodyLeft"></div>
    <div id="bodyLeftSwitch" class="switch-left"></div>
    <div id="bodyMain">
        <div id="bodyTitle"></div>
        <div id="bodyContent"><div id="mapCanvas"></div></div>
        <div id="bodyBottom"></div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<script type="text/javascript" src="http://ditu.google.cn/maps/api/js?v=3.8&sensor=false&key=AIzaSyDURiiLRcVt9ib0x8fM8uDMpKJQDWKdnwQ"></script>
<%=Public.LoadJs("mapcorrect.js,ofixedtable/ofixedtable.js,pagination/pagination.js", "/js/common/")%>
<%=Public.LoadJsCode("default.js?{0}")%></asp:Content>