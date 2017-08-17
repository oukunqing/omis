var PageName = '字典';
var RootId = 0;
var TypeId = 0;
var DataId = getDataId();

var pwType = null;
var otType = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {

}

function getDataId() {
    DataId = module.getControlValue($I('txtId'), 0);
    RootId = module.getControlValue($I('txtRootId'), 0);
    TypeId = module.getControlLang($I('txtTypeId'), 0);
    return DataId;
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName, getDataId()), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#txtTypeId').focus(function () {
        getTypeTree($(this));
    });

    $('.select').width($('#txtName').outerWidth());

    var isAdd = DataId <= 0;
    page.buildContinue(isAdd);
    page.buildFormAction(isAdd);

    if (isAdd && TypeId > 0) {
        getTypeData(TypeId);
    }
    getData(function () {
        page.showFormAction(true);
    });
}

function formSubmit() {
    var formData = {
        DictionaryId: module.getControlValue($I('txtId'), 0),
        TypeId: module.getControlLang($('#txtTypeId'), 0),
        DictionaryName: module.getControlValue($I('txtName')),
        DictionaryCode: module.getControlValue($I('txtCode')),
        DictionaryNumber: module.getControlValue($I('txtNumber'), 0),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (formData.TypeId <= 0) {
        cms.box.msgAndFocus($I('txtTypeId'), { html: '请选择字典分类' });
        return false;
    }
    if (formData.DictionaryName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入字典名称' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editDictionary&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.DictionaryId),
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
    var urlparam = 'action=getDictionary&id=' + DataId;
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
            $I('txtId'), $I('txtTypeId'), $I('txtName'), $I('txtCode'), $I('txtNumber'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.DictionaryId, dr.Extend.TypeName || '无', dr.DictionaryName, dr.DictionaryCode, dr.DictionaryNumber, dr.Enabled, dr.SortOrder
        ]);
        module.setControlLang($I('txtTypeId'), dr.TypeId);
    }
}

function getTypeTree($obj, reload) {
    if (typeof $obj == 'undefined') {
        $obj = $('#txtTypeId');
    }
    if (pwType != null && !reload) {
        pwType.Show();
    } else {
        var data = {
            ParentId: RootId,
            GetSubset: 1
        };
        var urlparam = 'action=getDictionaryTypeTree&data=' + encodeURIComponent(module.toJsonString(data));

        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    pwType = page.buildTypeTreeBox($obj, { title: '选择字典分类' });

                    var _callback = function (param, objTree, $obj) {
                        $obj.prop('lang', param.id);
                        $obj.attr('value', param.name);
                        pwType.Hide();
                    };

                    otType = new oTree('otType', 'divTypeTree', {
                        callback: _callback,
                        callbackObj: $obj,
                        loadedCallback: function (objTree) {
                            var pid = $obj.prop('lang');
                            objTree.select(pid);
                            getTypeData(pid, function (dr) {
                                if (dr && dr.ParentTree) {
                                    var arr = dr.ParentTree.replace(/[\(\)]/g, '').split(',');
                                    for (var i in arr) {
                                        objTree.open(arr[i]);
                                    }
                                }
                            });
                        }
                    });
                    otType.root({ id: 'none', pid: 0, name: '字典分类' });
                    for (var i = 0, c = jsondata.list.length; i < c; i++) {
                        var dr = jsondata.list[i];
                        otType.add({ id: dr.id, pid: dr.pid, name: dr.name });
                    }
                });
            }
        });
    }
}

function getTypeData(pid, func) {
    if (pid <= 0) {
        return false;
    }
    var urlparam = 'action=getDictionaryType&id=' + pid;
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                $I('txtTypeId').value = jsondata.data.TypeName;

                if (typeof func == 'function') {
                    func(jsondata.data);
                }
            });
        }
    });
}