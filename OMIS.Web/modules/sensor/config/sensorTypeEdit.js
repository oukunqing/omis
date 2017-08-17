var PageName = '传感器分类';
var DataId = 0;
var ParentId = 0;

var pwParent = null;
var otParent = null;

$(window).ready(function () {
    page.setBodySize();
    page.initialForm();
});

function setBodySize() {

}

function getDataId() {
    DataId = module.getControlValue($I('txtId'), 0);
    ParentId = module.getControlLang($I('txtParentId'), 0);
    return DataId;
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName, getDataId()), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#txtParentId').focus(function () {
        getParentTree($(this));
    });

    $('.select').width($('#txtName').outerWidth());

    var isAdd = DataId <= 0;
    page.buildContinue(isAdd);
    page.buildFormAction(isAdd);

    if (isAdd && ParentId > 0) {
        getParentData(ParentId);
    }
    getData(function () {
        page.showFormAction(true);
    });
}

function formSubmit() {
    var formData = {
        SensorTypeId: module.getControlValue($I('txtId'), 0),
        ParentId: module.getControlLang($('#txtParentId'), 0),
        TypeName: module.getControlValue($I('txtName')),
        TypeCode: module.getControlValue($I('txtCode')),
        DataUnit: module.getControlValue($I('txtUnit')),
        TypeFunc: module.getControlValue($I('txtTypeFunc')),
        TypeDesc: module.getControlValue($I('txtTypeDesc')),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (ParentId > 0 && formData.ParentId <= 0) {
        cms.box.msgAndFocus($I('txtParentId'), { html: '请选择上级分类' });
        return false;
    }
    if (formData.TypeName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入分类名称' });
        return false;
    }
    if (formData.TypeCode.length == 0) {
        cms.box.msgAndFocus($I('txtCode'), { html: '请输入分类编码' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editSensorType&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.SensorTypeId),
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
    var urlparam = 'action=getSensorType&id=' + DataId;
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
            $I('txtId'), $I('txtParentId'), $I('txtName'), $I('txtCode'), $I('txtUnit'), $I('txtTypeFunc'), $I('txtTypeDesc'), $I('ddlMultiSelectLimit'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.SensorTypeId, dr.Extend.ParentName || '无', dr.TypeName, dr.TypeCode, dr.DataUnit, dr.TypeFunc, dr.TypeDesc, dr.MultiSelectLimit, dr.Enabled, dr.SortOrder
        ]);
        module.setControlLang($I('txtParentId'), dr.ParentId);
    }
}

function getParentTree($obj, reload) {
    if (typeof $obj == 'undefined') {
        $obj = $('#txtParentId');
    }
    if (pwParent != null && !reload) {
        pwParent.Show();
    } else {
        var data = {
            ParentId: ParentId,
            GetSubset: 1,
            ExcludeId: DataId
        };
        var urlparam = 'action=getSensorTypeToParent&data=' + encodeURIComponent(module.toJsonString(data));

        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    pwParent = page.buildParentTreeBox($obj);

                    var _callback = function (param, objTree, $obj) {
                        $obj.prop('lang', param.id);
                        $obj.attr('value', param.name);
                        pwParent.Hide();
                    };

                    otParent = new oTree('otParent', 'divParentTree', {
                        callback: _callback,
                        callbackObj: $obj,
                        loadedCallback: function (objTree) {
                            var pid = $obj.prop('lang');
                            objTree.select(pid);
                            getParentData(pid, function (dr) {
                                if (dr && dr.ParentTree) {
                                    var arr = dr.ParentTree.replace(/[\(\)]/g, '').split(',');
                                    for (var i in arr) {
                                        objTree.open(arr[i]);
                                    }
                                }
                            });
                        }
                    });
                    if (ParentId <= 0) {
                        otParent.add({ id: 'none', pid: 0, name: '不选择上级分类', isRoot: true, callback: _callback, param: { id: 0, name: '无'} });
                    }
                    for (var i = 0, c = jsondata.list.length; i < c; i++) {
                        var dr = jsondata.list[i];
                        otParent.add({ id: dr.id, pid: dr.pid, name: dr.name });
                    }
                });
            }
        });
    }
}

function getParentData(pid, func) {
    if (pid <= 0) {
        return false;
    }
    var urlparam = 'action=getSensorType&id=' + pid;
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                $I('txtParentId').value = jsondata.data.TypeName;

                if (typeof func == 'function') {
                    func(jsondata.data);
                }
            });
        }
    });
}