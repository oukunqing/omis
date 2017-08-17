<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorDataList.aspx.cs" Inherits="modules_sensor_data_sensorDataList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <form id="frmQuery" action="" method="post">
        <div id="bodyForm" class="operform">
            <span>采集时间：</span>
            <input type="text" id="txtStartTime" readonly="readonly" class="txt w135" value="<%=DateTime.Now.ToString("yyyy-MM-dd 00:00:00") %>" />
            <span style="padding:0 5px 0 5px;">-</span>
            <input type="text" id="txtEndTime" readonly="readonly" class="txt w135" value="<%=DateTime.Now.ToString("yyyy-MM-dd 23:59:59") %>" style="margin-right:5px;" />
            <select id="ddlDataType" class="select" title="数据类型"><%=MyForm.BuildSelectOption("-1_请选择,0_传感器数据,1_原始值")%></select>
        </div>
        </form>
    </div>
    <div id="bodyBottom"><div id="pagination" class="pagination"></div></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("ofixedtable/ofixedtable.js,pagination/pagination.js,datepicker/WdatePicker.js", "/js/common/")%>
<%=Public.LoadJsCode("sensorDataList.js?{0}")%></asp:Content>