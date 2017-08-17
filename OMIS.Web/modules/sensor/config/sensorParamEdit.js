var PageName = '传感器参数';
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

    $('#txtName').focus();
}

function formCancal(isLoad) {
    return page.editCancel(isLoad);
}

function formSubmit() {
    var formData = {
        ParamId: module.getControlValue($I('txtId'), 0),
	    ParamName: module.getControlValue($I('txtName')),
	    ParamCode: module.getControlValue($I('txtCode')),
	    ParamFunc: module.getControlValue($I('txtFunc')),
	    ParamDesc: module.getControlValue($I('txtDesc')),
	    ParamType: module.getControlValue($I('ddlType'), 0),
	    ParamMode: module.getControlValue($I('ddlMode'), 0),
	    ConfigShow: module.getControlValue($I('ddlShow'), 0),
	    ValueType: module.getControlValue($I('ddlValueType'), 0),
	    CharLength: module.getControlValue($I('txtCharLength'), 0),
	    ValueOption: module.getControlValue($I('txtValueOption')),
	    Required: module.getControlValue($I('ddlRequired'), 1),
	    DefaultValue: module.getControlValue($I('txtDefaultValue')),
	    ValueSample: module.getControlValue($I('txtValueSample')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };
    page.submitConfirm(function () {
        var urlparam = 'action=editSensorParam&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.ParamId),
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
    var urlparam = 'action=getSensorParam&id=' + DataId;
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
            $I('txtId'), $I('txtName'), $I('txtCode'), $I('txtFunc'), $I('txtDesc'), $I('ddlType'), $I('ddlMode'), $I('ddlShow'),
            $I('ddlValueType'), $I('txtCharLength'), $I('txtValueOption'), $I('ddlRequired'), $I('txtDefaultValue'),
            $I('txtValueSample'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.ParamId, dr.ParamName, dr.ParamCode, dr.ParamFunc, dr.ParamDesc, dr.ParamType, dr.ParamMode, dr.ConfigShow,
            dr.ValueType, dr.CharLength, dr.ValueOption, dr.Required, dr.DefaultValue, dr.ValueSample, dr.Enabled, dr.SortOrder
        ]);
    }
}