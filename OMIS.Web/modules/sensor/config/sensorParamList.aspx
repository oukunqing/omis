<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorParamList.aspx.cs" Inherits="modules_sensor_config_sensorParamList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <form id="frmQuery" action="" method="post">
        <div id="bodyForm" class="operform">
            <select id="ddlParamType" class="select" title="参数分类"><%=MyForm.BuildSelectOption("-1_请选择,0_通道参数,1_设备参数")%></select> 
            <select id="ddlParamMode" class="select" title="参数类型"><%=MyForm.BuildSelectOption("-1_请选择,0_通用,1_模拟量,2_数字量")%></select> 
            <select id="ddlEnabled" class="select" title="是否启用"><%=MyForm.BuildEnabledSelect(true)%></select>            
        </div>
        </form>
    </div>
    <div id="bodyBottom"><div id="pagination" class="pagination"></div></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("ofixedtable/ofixedtable.js,pagination/pagination.js", "/js/common/")%>
<%=Public.LoadJsCode("sensorParamList.js?{0}")%></asp:Content>