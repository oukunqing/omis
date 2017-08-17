<%@ Page Title="" Language="C#" MasterPageFile="~/masters/mpPage.master" AutoEventWireup="true" CodeFile="sensorParamEdit.aspx.cs" Inherits="modules_sensor_config_sensorParamEdit" %>
<%@ MasterType VirtualPath="~/masters/mpPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <form id="frmEdit" action="" method="post">
    <div id="bodyTitle"></div>
    <div id="bodyContent">
        <div id="formBody">
            <div class="form">
                <input type="hidden" id="txtId" value="<%=Public.Request("paramId|id",0) %>" />
                <table cellpadding="0" cellspacing="0" class="tbform">
                    <tr>
                        <td class="w80">参数名称：</td>
                        <td class="w240">
                            <input type="text" id="txtName" class="txt w200" />
                        </td>
                        <td class="w80">参数编码：</td>
                        <td>
                            <input type="text" id="txtCode" class="txt w200" />
                        </td>
                    </tr>
                    <tr>
                        <td>参数功能：</td>
                        <td><input type="text" id="txtFunc" class="txt w200" /></td>
                        <td>是否显示：</td>
                        <td>
                            <select id="ddlShow" class="select"><%=MyForm.BuildSelectOption("1,显示|0,不显示", "1")%></select>
                        </td>
                    </tr>
                    <tr>
                        <td class="va-top">参数说明：</td>
                        <td colspan="3">
                            <textarea id="txtDesc" class="txt" rows="1" cols="1" style="width:510px;height:80px;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>参数分类：</td>
                        <td>
                            <select id="ddlType" class="select" title="参数分类"><%=MyForm.BuildSelectOption("0_通道参数,1_设备参数")%></select> 
                        </td>
                        <td>参数类型：</td>
                        <td>
                            <select id="ddlMode" class="select" title="参数类型"><%=MyForm.BuildSelectOption("0_通用,1_模拟量,2_数字量")%></select> 
                        </td>
                    </tr>
                    <tr>
                        <td>值类型：</td>
                        <td>
                            <select id="ddlValueType" class="select" title="参数类型"><%=MyForm.BuildSelectOption("0_填写,1_单个值,2_多个值选择,3_范围选择")%></select>
                        </td>
                        <td>字符长度：</td>
                        <td>
                            <input type="text" id="txtCharLength" class="txt w60" />
                            <span class="explain">字节 (最大长度)</span>
                        </td>
                    </tr>
                    <tr>
                        <td>是否必填：</td>
                        <td>
                            <select id="ddlRequired" class="select" title="是否必填"><%=MyForm.BuildSelectOption("0_可不填,1_必填","1")%></select>
                        </td>
                        <td>默认值：</td>
                        <td>
                            <input type="text" id="txtDefaultValue" class="txt w150" />
                        </td>
                    </tr>
                    <tr>
                        <td>备选项：</td>
                        <td colspan="3">
                            <input type="text" id="txtValueOption" class="txt w500" style="width:510px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>示例：</td>
                        <td colspan="3">
                            <input type="text" id="txtValueSample" class="txt w300" />
                            <span class="explain">仅作参考，不参与计算或逻辑</span>
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
<%=Public.LoadJsCode("sensorParamEdit.js?{0}")%></asp:Content>