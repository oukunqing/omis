<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="dictionaryList.aspx.cs" Inherits="modules_systems_dictionary_dictionaryList" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
<div id="pageBody">
    <div id="bodyLeft"></div>
    <div id="bodyLeftSwitch" class="switch-left"></div>
    <div id="bodyMain">
        <div id="bodyTitle"></div>
        <div id="bodyContent">
            <form id="frmQuery" action="" method="post">
            <div id="bodyForm" class="operform">
                <input type="hidden" id="txtTypeId" value="<%=Public.Request("typeId|tid", -1) %>" />
                <input type="hidden" id="txtRootId" value="<%=Public.Request("rootId",0) %>" />
                <select id="ddlEnabled" class="select" title="是否启用"><%=MyForm.BuildEnabledSelect(true)%></select>
            </div>
            </form>
        </div>
        <div id="bodyBottom"><div id="pagination" class="pagination"></div></div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJs("ofixedtable/ofixedtable.js,pagination/pagination.js", "/js/common/")%>
<%=Public.LoadJsCode("dictionaryList.js?{0}")%></asp:Content>