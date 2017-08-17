var PageName = '角色';
var DataId = 0;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {

}

function getDataId() {
    DataId = module.getControlValue($I('txtId'), 0);
    return DataId;
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName, getDataId()), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    var isAdd = DataId <= 0;
    page.buildContinue(isAdd);
    page.buildFormAction(isAdd);
    
    getRoleGroup(function () {
        getData(function () {
            page.showFormAction(true);
        });
    });

    $('.select').width($('#txtName').outerWidth());
}

function getRoleGroup(callback) {
    var urlparam = 'action=getRoleGroup&data=';
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var ddl = $I('ddlGroup');
                for (var i = 0, c = jsondata.list.length; i < c; i++) {
                    var dr = jsondata.list[i];
                    cms.util.fillOption(ddl, dr.GroupId, dr.GroupName);
                }
                if (page.isFunction(callback)) {
                    callback();
                }
            });
        }
    });
}

function formCancal(isLoad) {
    return page.editCancel(isLoad);
}

function formSubmit() {
    var formData = {
        RoleId: module.getControlValue($I('txtId'), 0),
        GroupId: module.getControlValue($('#ddlGroup'), 0),
        RoleName: module.getControlValue($I('txtName')),
        RoleCode: module.getControlValue($I('txtCode')),
        RoleDesc: module.getControlValue($I('txtDesc')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (formData.GroupId <= 0) {
        cms.box.msgAndFocus($I('ddlGroup'), { html: '请选择角色组别' });
        return false;
    }
    if (formData.RoleName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入角色名称' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editRole&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.RoleId),
                        callback: function (pwobj, pwReturn) {
                            page.editCallback(true);
                        }
                    });
                });
            }
        });
    });
}

function getData(func) {
    if (getDataId() <= 0) {
        return false;
    }
    var urlparam = 'action=getRole&id=' + DataId;
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showData);
            page.isFunction(func) ? func() : null;
        }
    });
}

function showData(jsondata, param) {
    if (1 == jsondata.result) {
        var dr = jsondata.data;
        module.setControlValue([
            $I('txtId'), $I('ddlGroup'), $I('txtName'), $I('txtCode'), $I('txtDesc'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.RoleId, dr.GroupId, dr.RoleName, dr.RoleCode, dr.RoleDesc, dr.Enabled, dr.SortOrder
        ]);
    }
}