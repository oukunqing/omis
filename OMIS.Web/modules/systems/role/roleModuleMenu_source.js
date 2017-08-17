var PageName = '角色-模块菜单配置';
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
    var urlparam = 'action=getRoleModuleMenuConfig&data=' + encodeURIComponent(module.toJsonString(param));
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

    var ids = [];
    for (var i = 0, c = jsondata.data.config.length; i < c; i++) {
        var dr = jsondata.data.config[i];
        ids.push(dr.MenuId);
    }

    $('#txtMenuIdList').attr('value', ids.join(','));
    $('#txtMenuIdListCache').attr('value', ids.join(','));


    page.tree = new oTree('page.tree', $I('formContent'), {
        showCheckBox: true,
        checkboxCallback: function (param, objTree) {
            var ids = cms.util.getCheckBoxCheckedValue('chbMenu', ',');
            $('#txtMenuIdList').attr('value', ids);
        },
        loadedCallback: function (objTree) {
            var ids = $('#txtMenuIdList').val();
            cms.util.setCheckBoxChecked('chbMenu', ',', ids);

            objTree.openLevel(0);
        }
    });

    for (var i = 0, c = jsondata.data.list.length; i < c; i++) {
        var dr = jsondata.data.list[i];
        page.tree.add({
            id: dr.MenuId, pid: dr.ParentId, name: dr.MenuName, icon: dr.MenuUrl == '1' ? 'page.gif' : '',
            checkbox: { name: 'chbMenu' }
        });
    }    
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
        var urlparam = 'action=editRoleModuleMenu&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: '角色-模块菜单配置已更新',
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