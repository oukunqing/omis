<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorTypeList.aspx.cs" Inherits="modules_sensor_config_sensorTypeList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <form id="frmQuery" action="" method="post">
        <div id="bodyForm" class="operform">
            <input type="hidden" id="txtParentId" value="<%=Public.Request("parentId|pid", 0)%>" />
            <select id="ddlEnabled" class="select" title="是否启用"><%=MyForm.BuildEnabledSelect(true)%></select>            
        </div>
        </form>
    </div>
    <div id="bodyBottom"></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("otreetable/otreetable.js,ofixedtable/ofixedtable.js", "/js/common/")%>
<%=Public.LoadJsCode("sensorTypeList.js?{0}")%></asp:Content>