<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorChannelEdit.aspx.cs" Inherits="modules_sensor_config_sensorChannelEdit" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtId" value="<%=Public.Request("channelId|id",0) %>" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td>通道号：</td>
                        <td>
                            <select id="ddlNo" class="select w80"><%=MyForm.BuildSelectOption(1,70,1)%></select>
                        </td>
                        <td>通道组：</td>
                        <td>
                            <select id="ddlGroup" class="select"><%=MyForm.BuildSelectOption(0,10,1)%></select>
                        </td>
                    </tr>
                    <tr>
                        <td class="w90">通道分类：</td>
                        <td class="w200">
                            <select id="ddlType" class="select"><%=MyForm.BuildSelectOption("1_真实通道,0_虚拟通道", "1")%></select>
                        </td>
                        <td class="w80">通道类型：</td>
                        <td>
                            <select id="ddlMode" class="select"></select>
                        </td>
                    </tr>
                    <tr>
                        <td>原始值类型：</td>
                        <td>
                            <select id="ddlOriType" class="select"></select>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>备注：</td>
                        <td colspan="3">
                            <input type="text" id="txtRemark" class="txt w420" style="width:430px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>是否启用：</td>
                        <td>
                            <select id="ddlEnabled" class="select"><%=MyForm.BuildEnabledSelect()%></select>
                        </td>
                        <td>排序编号：</td>
                        <td><input type="text" id="txtSortOrder" class="txt" maxlength="6" style="width:60px;" /></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td class="w80"></td>
                        <td>
                            <div id="formContinue"></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="formBottom"></div>
    </div>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphFoot" Runat="Server">
<%=Public.LoadJsCode("sensorChannelEdit.js?{0}")%></asp:Content>