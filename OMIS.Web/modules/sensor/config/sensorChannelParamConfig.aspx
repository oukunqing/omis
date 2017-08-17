<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorChannelParamConfig.aspx.cs" Inherits="modules_sensor_config_sensorChannelParamConfig" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <style type="text/css">
        #formContent td{height:28px; padding:0 5px;}
        #formContent li{padding:0 5px;}
        #formContent label{margin-right:10px; margin-bottom:5px; display:inline-block; float:left;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"> - <label id="lblChannelName"></label></div>
    <div id="bodyContent">
        <div id="formBody">
            <input type="hidden" id="txtId" value="<%=Public.Request("channelNo|no",0) %>" />
            <div id="formBox"><div id="formContent"></div></div>
        </div>
        <div id="formBottom"></div>
    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJsCode("sensorChannelParamConfig.js?{0}")%></asp:Content>