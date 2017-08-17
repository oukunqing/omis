var PageName = '传感器通道';
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


    getModeList(function () {
        getOriType(function () {
            getData(function () {
                page.showFormAction(true);
            });
        });
    });

    //$('.select').width($('#txtName').outerWidth());
    $('.select').width(150);
}

function formCancal(isLoad) {
    return page.editCancel(isLoad);
}

function formSubmit() {
    var formData = {
        ChannelId: module.getControlValue($I('txtId'), 0),
        ChannelNo: module.getControlValue($I('ddlNo'), 0),
        ChannelType: module.getControlValue($I('ddlType'), 1),
        ChannelGroup: module.getControlValue($I('ddlGroup'), 0),
        ModeId: module.getControlValue($I('ddlMode'), 1),
        OriTypeId: module.getControlValue($I('ddlOriType'), 0),
        Remark: module.getControlValue($I('txtRemark')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    page.submitConfirm(function () {
        var urlparam = 'action=editSensorChannel&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.ChannelId),
                        callback: function (pwobj, pwReturn) {
                            page.editCallback(true);
                        }
                    });
                });
            }
        });
    });
}

function getModeList(func) {
    var param = {
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField'))
    };
    var urlparam = 'action=getSensorChannelModeTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var obj = $I('ddlMode');
                for (var i = 0, c = jsondata.list.length; i < c; i++) {
                    var dr = jsondata.list[i];
                    cms.util.fillOption(obj, dr.id, dr.name);
                }
            });
            page.showLoading(false);

            page.isFunction(func) ? func() : null;
        }
    });
}

function getOriType(func) {
    var param = {
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField'))
    };
    var urlparam = 'action=getSensorOriginalTypeTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var obj = $I('ddlOriType');
                cms.util.fillOption(obj, "0", "请选择");
                for (var i = 0, c = jsondata.list.length; i < c; i++) {
                    var dr = jsondata.list[i];
                    cms.util.fillOption(obj, dr.id, dr.name);
                }
            });
            page.showLoading(false);

            page.isFunction(func) ? func() : null;
        }
    });
}

function getData(func) {
    if (getDataId() <= 0) {
        return false;
    }
    var urlparam = 'action=getSensorChannel&id=' + DataId;
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
            $I('txtId'), $I('ddlNo'), $I('ddlType'), $I('ddlGroup'), $I('ddlMode'), $I('ddlOriType'), $I('txtRemark'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.ChannelId, dr.ChannelNo, dr.ChannelType, dr.ChannelGroup, dr.ModeId, dr.OriTypeId, dr.Remark, dr.Enabled, dr.SortOrder
        ]);
    }
}