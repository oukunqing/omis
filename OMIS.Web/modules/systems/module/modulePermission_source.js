var PageName = '模块-权限配置';
var ModuleId = module.getControlValue($I('txtId'), 0);

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
            '<a class="btn btnform" style="margin-left:6px;" onclick="recoveryModulePermission();"><span>重置</span></a>'
        ].join(''));
    });
}

function getModulePermission(callback) {
    var param = { ModuleId: ModuleId };
    var urlparam = 'action=getModulePermissionConfig&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showPermissionList);

            if (page.isFunction(callback)) {
                callback();
            }
        }
    });
}

function showPermissionList(jsondata, param) {
    var obj = $('#formContent');
    var arr = {};

    obj.append('<input type="hidden" id="txtPermissionIdList" />');
    obj.append('<input type="hidden" id="txtPermissionIdListCache" />');

    var name = '%s <span class="explain">%s</span>'.format([jsondata.data.module.ModuleName, jsondata.data.module.ModuleCode]);
    $('#lblModuleName').html(name);

    var html = [
        '<table cellpadding="0" cellspacing="0" class="tblist" id="tbList" style="display:none;">',
        '<tr class="trheader"><td style="width:100px;">分类</td><td>权限名称</td><td style="width:120px;">操作</td></tr>'
    ];
    for (var i = 0, c = jsondata.data.type.length; i < c; i++) {
        var dr = jsondata.data.type[i];
        var oper = ['<a class="btn btnc22" onclick="selectPermission(' + dr.TypeId + ',1);" style="float:left;margin-right:3px;"><span>全选</span></a>',
        '<a class="btn btnc22" onclick="selectPermission(' + dr.TypeId + ',2);" style="float:left;margin-right:3px;"><span>取消</span></a>',
        '<a class="btn btnc22" onclick="selectPermission(' + dr.TypeId + ',3);" style="float:left;"><span>反选</span></a>',
        ].join('');
        html.push('<tr><td>%s</td><td>%s</td><td>%s</td></tr>'.format([
            dr.TypeName, '<div id="divList_' + dr.TypeId + '"></div>', oper
        ]));
        arr['divList_' + dr.TypeId] = [];
    }
    html.push('</table>');
    obj.append(html.join(''));

    for (var i = 0, c = jsondata.data.list.length; i < c; i++) {
        var dr = jsondata.data.list[i];
        var html = [
            '<label class="chb-label-nobg"><input type="checkbox" id="chbPermission_%s" name="chbPermission"class="chb" value="%s" lang="%s" onclick="setPermission();" />'.format([dr.PermissionId, dr.PermissionId, dr.TypeId]),
            '<span>%s</span><span class="explain" style="margin:0 3px;">%s</span></label>'.format([dr.PermissionName, dr.PermissionCode])
        ].join('');
        arr['divList_' + dr.TypeId].push(html);
    }

    //显示菜单列表
    for (var i in arr) {
        $('#' + i).append(arr[i].join(''));
    }

    var ids = [];
    for (var i = 0, c = jsondata.data.config.length; i < c; i++) {
        var dr = jsondata.data.config[i];
        $('#chbPermission_%s'.format([dr.PermissionId])).attr('checked', 'checked');
        ids.push(dr.PermissionId);
    }

    $('#tbList').show();

    $('#txtPermissionIdList').attr('value', ids.join(','));
    $('#txtPermissionIdListCache').attr('value', ids.join(','));

    cms.util.setCheckBoxCheckedColor('chbPermission');
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
    $('#txtPermissionIdList').attr('value', cms.util.getCheckBoxCheckedValue('chbPermission',','));
    cms.util.setCheckBoxCheckedColor('chbPermission');
}

function recoveryModulePermission() {
    //先取消全部
    cms.util.selectCheckBox('chbPermission', 2);
    //选中指定的
    cms.util.setCheckBoxChecked('chbPermission', ',', $('#txtPermissionIdListCache').val());

    setPermission();
}

function formSubmit() {
    var formData = {
        ModuleId: module.getControlValue($I('txtId'), 0),
        PermissionIdList: module.getControlValue($I('txtPermissionIdList'))
    };

    if (formData.ModuleId == 0) {
        alert('未指定模块');
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editModulePermission&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: '模块-权限配置已更新',
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