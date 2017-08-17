var PageName = '角色-模块-权限配置';
var RoleId = module.getControlValue($I('txtId'), 0);

var treeTable = null;
var fixedTable = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {
    $('#fromBox').height(page.formSize.height);
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    getModulePermission(function () {
        $('#formBottom').append([
            '<a class="btn btnform" onclick="formSubmit();"><span>确认，提交</span></a>',
            '<a class="btn btnform" style="margin:0 6px;" onclick="recoveryModulePermission();"><span>重置</span></a>',
            '<a class="btn btnform" onclick="closePage();"><span>关闭</span></a>'
        ].join(''));
    });
}

function getModulePermission(callback) {
    var param = { RoleId: RoleId };
    var urlparam = 'action=getRoleModulePermissionConfig&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showModulePermission);

            if (page.isFunction(callback)) {
                callback();
            }
        }
    });
}

function showModulePermission(jsondata, param) {
    var obj = $('#formContent');
    var arr = {};

    obj.append('<input type="hidden" id="txtPermissionIdList" />');
    obj.append('<input type="hidden" id="txtPermissionIdListCache" />');

    $('#lblRoleName').html(jsondata.data.role.RoleName);

    var table = [
        '<table cellpadding="0" cellspacing="0" class="tblist" id="tbList" style="display:none;">',
        '<tr class="trheader">',
        '<td style="min-width:180px;">模块</td><td>权限</td>',
        '<td style="width:120px;">操作</td>',
        '</tr>'
    ];
    for (var i = 0, c = jsondata.data.module.length; i < c; i++) {
        var dr = jsondata.data.module[i];
        var oper = [
            '<a class="btn btnc22" onclick="selectPermission(' + dr.ModuleId + ',1);" style="float:left;margin-right:3px;"><span>全选</span></a>',
            '<a class="btn btnc22" onclick="selectPermission(' + dr.ModuleId + ',2);" style="float:left;margin-right:3px;"><span>取消</span></a>',
            '<a class="btn btnc22" onclick="selectPermission(' + dr.ModuleId + ',3);" style="float:left;"><span>反选</span></a>'
        ].join('');
        table.push('<tr lang="{id:%s,pid:%s,level:%s}"><td>%s <em>%s</em></td><td>%s</td><td>%s</td></tr>'.format([
            dr.ModuleId, dr.ParentId, dr.Level, dr.ModuleName, dr.ModuleCode, '<div class="list" id="divList_' + dr.ModuleId + '"></div>', oper
        ]));
        arr['divList_' + dr.ModuleId] = [];
    }
    table.push('</table>');

    obj.append(table.join(''));

    //显示模块-权限
    for (var i = 0, c = jsondata.data.list.length; i < c; i++) {
        var dr = jsondata.data.list[i];
        var html = [
        '<label class="chb-label-nobg">',
        '<input type="checkbox" class="chb" name="chbPermission" id="chbPermission_%s_%s" value="%s_%s" lang="%s" onclick="setPermission();" />'.format([
        dr.ModuleId, dr.PermissionId, dr.ModuleId, dr.PermissionId, dr.ModuleId
        ]),
        '<span style="margin-right:3px;">', dr.PermissionName, '</span>',
        '<em>', dr.PermissionCode, '</em>',
        '</label>'
        ].join('');
        arr['divList_' + dr.ModuleId].push(html);
    }
    //显示权限列表
    for (var i in arr) {
        $('#' + i).append(arr[i].join(''));
    }

    $('#tbList').show();

    var ids = [];
    for (var i = 0, c = jsondata.data.config.length; i < c; i++) {
        var dr = jsondata.data.config[i];
        $('#chbPermission_%s_%s'.format([dr.ModuleId, dr.PermissionId])).attr('checked', 'checked');

        ids.push('%s_%s'.format([dr.ModuleId, dr.PermissionId]));
    }

    $('#txtPermissionIdList').attr('value', ids.join(','));
    $('#txtPermissionIdListCache').attr('value', ids.join(','));

    cms.util.setCheckBoxCheckedColor('chbPermission');

    setTableControl(true, true);
}

function setTableControl(isControl, isTree) {
    var tbList = $I('tbList');
    if (tbList != null) {
        if (isTree) {
            treeTable = new oTreeTable('otb', tbList, {
                skin: 'default', showIcon: true, cellIndex: 0
            });
        }

        window.setTimeout(function () { cms.util.setTableStyle(tbList); }, 300);
    }
}

function selectPermission(tid, action) {
    var arr = document.getElementsByName('chbPermission');
    for (var i = 0, c = arr.length; i < c; i++) {
        if (arr[i].lang == tid) {
            arr[i].checked = action == 1 ? true : action == 2 ? false : !arr[i].checked;
        }
    }

    setPermission();
}

function setPermission() {
    $('#txtPermissionIdList').attr('value', cms.util.getCheckBoxCheckedValue('chbPermission', ','));
    cms.util.setCheckBoxCheckedColor('chbPermission');
}

function recoveryModulePermission() {
    //先取消全部
    cms.util.selectCheckBox('chbPermission', 2);
    //选中指定的
    cms.util.setCheckBoxChecked('chbPermission', ',', $('#txtPermissionIdListCache').val());

    setPermission();
}

function closePage(){
    page.cancelConfirm(function () {
        try {
            if (page.isChild() && typeof parent.setConfigCallback == 'function') {
                parent.setConfigCallback(true, false);
            }
        } catch (e) { }
    }, false);
}

function formSubmit() {
    var formData = {
        RoleId: module.getControlValue($I('txtId'), 0),
        PermissionIdList: module.getControlValue($I('txtPermissionIdList'))
    };

    if (formData.RoleId == 0) {
        alert('未指定角色');
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editRoleModulePermission&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: '角色-模块-权限配置已更新',
                        callback: function (pwobj, pwReturn) {
                            try {
                                if (page.isChild() && typeof parent.setConfigCallback == 'function') {
                                    parent.setConfigCallback(true);
                                }
                            } catch (e) { }
                        }
                    });
                });
            }
        });
    });
}