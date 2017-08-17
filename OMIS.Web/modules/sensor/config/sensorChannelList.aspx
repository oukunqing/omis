<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorChannelList.aspx.cs" Inherits="modules_sensor_config_sensorChannelList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <form id="frmQuery" action="" method="post">
        <div id="bodyForm" class="operform">
            <select id="ddlType" class="select" title="通道分类"><%=MyForm.BuildSelectOption("-1_请选择,1_真实通道,0_虚拟通道")%></select>
            <select id="ddlMode" class="select" title="通道类型"><%=MyForm.BuildSelectOption("-1_请选择")%></select>
            <select id="ddlEnabled" class="select" title="是否启用"><%=MyForm.BuildEnabledSelect(true)%></select>            
        </div>
        </form>
    </div>
    <div id="bodyBottom"><div id="pagination" class="pagination"></div></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("ofixedtable/ofixedtable.js,pagination/pagination.js", "/js/common/")%>
<%=Public.LoadJsCode("sensorChannelList.js?{0}")%></asp:Content>