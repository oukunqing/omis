var PageName = '权限分类';
var DataId = 0;

$(window).ready(function () {
    page.setBodySize();
    page.initialForm();
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
    
    $('.select').width($('#txtName').outerWidth());

    var isAdd = DataId <= 0;
    page.buildContinue(isAdd);
    page.buildFormAction(isAdd);

    getData(function () {
        page.showFormAction(true);
    });
}

function formSubmit() {
    var formData = {
        TypeId: module.getControlValue($I('txtId'), 0),
        TypeName: module.getControlValue($I('txtName')),
        TypeCode: module.getControlValue($I('txtCode')),
        TypeDesc: module.getControlValue($I('txtDesc')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (formData.TypeName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入分类名称' });
        return false;
    }
    if (formData.TypeCode.length == 0) {
        cms.box.msgAndFocus($I('txtCode'), { html: '请输入分类编码' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editPermissionType&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.TypeId),
                        callback: function (pwobj, pwReturn) {
                            debugger;
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
    var urlparam = 'action=getPermissionType&id=' + DataId;
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
            $I('txtId'), $I('txtName'), $I('txtCode'), $I('txtDesc'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.TypeId, dr.TypeName, dr.TypeCode, dr.TypeDesc, dr.Enabled, dr.SortOrder
        ]);
    }
}