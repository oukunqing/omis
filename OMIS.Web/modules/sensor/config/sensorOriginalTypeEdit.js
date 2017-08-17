var PageName = '原始值类型';
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

    getData(function () {
        page.showFormAction(true);
    });

    $('.select').width($('#txtName').outerWidth());
}

function formCancal(isLoad) {
    return page.editCancel(isLoad);
}

function formSubmit() {
    var formData = {
        OriTypeId: module.getControlValue($I('txtId'), 0),
        OriTypeName: module.getControlValue($I('txtName')),
        OriTypeCode: module.getControlValue($I('txtCode')),
        DefaultUnit: module.getControlValue($I('txtUnit')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (formData.OriTypeName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入分类名称' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editSensorOriginalType&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.OriTypeId),
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
    var urlparam = 'action=getSensorOriginalType&id=' + DataId;
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
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
            $I('txtId'), $I('txtName'), $I('txtCode'), $I('txtUnit'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.OriTypeId, dr.OriTypeName, dr.OriTypeCode, dr.DefaultUnit, dr.Enabled, dr.SortOrder
        ]);
    }
}