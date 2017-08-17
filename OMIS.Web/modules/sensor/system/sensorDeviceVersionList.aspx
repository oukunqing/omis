<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorDeviceVersionList.aspx.cs" Inherits="modules_sensor_system_sensorDeviceVersionList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <form id="frmQuery" action="" method="post">
        <div id="bodyForm" class="operform">            
            <select id="ddlEnabled" class="select" title="是否启用"><%=MyForm.BuildEnabledSelect(true)%></select>            
        </div>
        </form>
    </div>
    <div id="bodyBottom"><div id="pagination" class="pagination"></div></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("ofixedtable/ofixedtable.js,pagination/pagination.js", "/js/common/")%>
<%=Public.LoadJsCode("sensorDeviceVersionList.js?{0}")%></asp:Content>