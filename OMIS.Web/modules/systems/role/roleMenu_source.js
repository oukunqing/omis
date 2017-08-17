var PageName = '角色-导航菜单配置';
var RoleId = module.getControlValue($I('txtId'), 0);

var treeTable = null;
var fixedTable = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {

}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    getMenuList(function () {
        $('#formBottom').append([
            '<a class="btn btnform" onclick="formSubmit();"><span>确认，提交</span></a>',
            '<a class="btn btnform" style="margin:0 6px;" onclick="recoveryModulePermission();"><span>重置</span></a>',
            '<a class="btn btnform" onclick="closePage();"><span>关闭</span></a>'
        ].join(''));
    });
}

function getMenuList(callback) {
    var param = { RoleId: RoleId };
    var urlparam = 'action=getRoleMenuConfig&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showMenuList);
            if (page.isFunction(callback)) {
                callback();
            }
        }
    });
}

function showMenuList(jsondata, param) {
    var obj = $('#formContent');
    var arr = {};

    obj.append('<input type="hidden" id="txtMenuIdList" />');
    obj.append('<input type="hidden" id="txtMenuIdListCache" />');

    $('#lblRoleName').html(jsondata.data.role.RoleName);

    var html = [
        '<table cellpadding="0" cellspacing="0" class="tblist" id="tbList" style="display:none;">',
        '<tr class="trheader"><td style="width:100px;">分类</td><td>菜单名称</td><td style="width:120px;">操作</td></tr>'
    ];
    for (var i = 0, c = jsondata.data.type.length; i < c; i++) {
        var dr = jsondata.data.type[i];
        var oper = [
            '<a class="btn btnc22" onclick="selectMenu(' + dr.TypeId + ',1);" style="float:left;margin-right:3px;"><span>全选</span></a>',
            '<a class="btn btnc22" onclick="selectMenu(' + dr.TypeId + ',2);" style="float:left;margin-right:3px;"><span>取消</span></a>',
            '<a class="btn btnc22" onclick="selectMenu(' + dr.TypeId + ',3);" style="float:left;"><span>反选</span></a>'
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
            '<label class="chb-label-nobg"><input type="checkbox" id="chbMenu_{0}" name="chbMenu" class="chb" value="{0}" lang="{1}" onclick="setMenu();" />'.format2([dr.MenuId, dr.TypeId]),
            '<span>{0}</span> <em style="margin-left:3px;">{1}</em></label>'.format2([dr.Enabled == 0 ? '<del>' + dr.MenuName + '</del>' : dr.MenuName, dr.MenuCode])
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
        $('#chbMenu_%s'.format([dr.MenuId])).attr('checked', 'checked');
        ids.push(dr.MenuId);
    }

    $('#tbList').show();

    $('#txtMenuIdList').attr('value', ids.join(','));
    $('#txtMenuIdListCache').attr('value', ids.join(','));

    cms.util.setCheckBoxCheckedColor('chbMenu');
}

function selectMenu(tid, action) {
    var arr = document.getElementsByName('chbMenu');
    for (var i = 0, c = arr.length; i < c; i++) {
        if (arr[i].lang == tid) {
            arr[i].checked = action == 1 ? true : action == 2 ? false : !arr[i].checked;
        }
    }

    setMenu();
}

function setMenu() {
    $('#txtMenuIdList').attr('value', cms.util.getCheckBoxCheckedValue('chbMenu', ','));
    cms.util.setCheckBoxCheckedColor('chbMenu');
}

function recoveryModulePermission() {
    //先取消全部
    cms.util.selectCheckBox('chbMenu', 2);
    //选中指定的
    cms.util.setCheckBoxChecked('chbMenu', ',', $('#txtMenuIdListCache').val());

    setMenu();
}

function closePage() {
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
        MenuIdList: module.getControlValue($I('txtMenuIdList'))
    };

    if (formData.RoleId == 0) {
        alert('未指定角色');
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editRoleMenu&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: '角色-导航菜单配置已更新',
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