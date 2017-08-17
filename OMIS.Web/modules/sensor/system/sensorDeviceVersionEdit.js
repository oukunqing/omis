var PageName = '传感器设备版本';
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

    getChannelList(function () {
        getData(function () {
            page.showFormAction(true);
        });
    });

    $('.select').width($('#txtName').outerWidth());
}

function formCancal(isLoad) {
    return page.editCancel(isLoad);
}

function formSubmit() {
    var formData = {
        VersionId: module.getControlValue($I('txtId'), 0),
        VersionCode: module.getControlValue($I('txtCode')),
        VersionDesc: module.getControlValue($I('txtDesc')),
        VersionConfig: module.getControlValue($I('txtVersionConfig')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (formData.VersionCode.length == 0) {
        cms.box.msgAndFocus($I('txtCode'), { html: '请输入版本编码' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editSensorDeviceVersion&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.VersionId),
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
    var urlparam = 'action=getSensorDeviceVersion&id=' + DataId;
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
            $I('txtId'), $I('txtCode'), $I('txtDesc'), $I('txtVersionConfig'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.VersionId, dr.VersionCode, dr.VersionDesc, dr.VersionConfig, dr.Enabled, dr.SortOrder
        ]);

        cms.util.setCheckBoxChecked('chbChannel', ',', dr.VersionConfig);
        cms.util.setCheckBoxCheckedColor('chbChannel');
    }
}

function getChannelList(func) {
    var param = {
        ChannelType: 1
    };
    var urlparam = 'action=getSensorChannelTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var oper = [
                    '<div>',
                    '<a class="btn btnc22" onclick="selectChannel(1);" style="float:left;margin-right:3px;"><span>全选</span></a>',
                    '<a class="btn btnc22" onclick="selectChannel(2);" style="float:left;margin-right:3px;"><span>取消</span></a>',
                    '<a class="btn btnc22" onclick="selectChannel(3);" style="float:left;"><span>反选</span></a>',
                    '</div>'
                ].join('');

                var html = [];
                html.push(oper);
                html.push('<div style="clear:both;padding-top:5px;">');

                for (var i = 0, c = jsondata.list.length; i < c; i++) {
                    var dr = jsondata.list[i];
                    html.push('<label class="chb-label-nobg" style="margin-right:5px;">');
                    html.push('<input type="checkbox" class="chb" name="chbChannel" onclick="setChannel();" value="%s" /><span>通道%s</span>'.format([dr.id, dr.cn]));
                    html.push('</label>');
                }
                html.push('</div>');

                $('#divChannel').append(html.join(''));
            });

            page.isFunction(func) ? func() : null;
        }
    });
}

function selectChannel(oper) {
    cms.util.selectCheckBox('chbChannel', oper);

    setChannel();
}

function setChannel() {
    $('#txtVersionConfig').attr('value', cms.util.getCheckBoxCheckedValue('chbChannel', ','));
    cms.util.setCheckBoxCheckedColor('chbChannel');
}