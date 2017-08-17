var PageName = '模块菜单';
var DataId = 0;
var ParentId = 0;

var pwParent = null;
var otParent = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
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

    page.photo.initial({ updateCallback: photoUpdateCallback, maxCount: 1 });
    page.photo.buildForm($('#divPhotoForm'), { dir: 'modulemenu', type: 'icon', w: 20, h: 20, mode: 'HWR', delSource: true }, { id: 'txtPhoto' });

    if (isAdd && ParentId > 0) {
        getParentData(ParentId);
    }
    getData(function () {
        page.showFormAction(true);
    });
}

function formSubmit() {
    var formData = {
        MenuId: module.getControlValue($I('txtId'), 0),
        ParentId: module.getControlLang($('#txtParentId'), 0),
        MenuName: module.getControlValue($I('txtName')),
        MenuCode: module.getControlValue($I('txtCode')),
        MenuUrl: module.getControlValue($I('txtUrl')),
        MenuPic: module.getControlValue($I('txtPhoto')),
        MenuType: module.getControlValue($('#ddlMenuType'), 0),
        OpenType: module.getControlValue($I('ddlOpenType'), 0),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (ParentId > 0 && formData.ParentId <= 0) {
        cms.box.msgAndFocus($I('txtParentId'), { html: '请选择上级菜单' });
        return false;
    }
    if (formData.MenuName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入菜单名称' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editModuleMenu&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: page.buildEditPrompt(PageName, formData.MenuId),
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
    var urlparam = 'action=getModuleMenu&id=' + DataId;
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
            $I('txtId'), $I('txtParentId'), $I('txtName'), $I('txtCode'), $I('txtUrl'), $I('txtPhoto'), $I('ddlMenuType'), $I('ddlOpenType'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.MenuId, dr.Extend.ParentName || '无', dr.MenuName, dr.MenuCode, dr.MenuUrl, dr.MenuPic, dr.MenuType, dr.OpenType, dr.Enabled, dr.SortOrder
        ]);
        module.setControlLang($I('txtParentId'), dr.ParentId);

        page.photo.showPhoto(dr.MenuPic);
    }
}

function photoUpdateCallback(paths) {
    if (DataId > 0) {
        var urlparam = 'action=updateModuleMenuPhoto&path=%s&id=%s'.format([paths, DataId]);
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    page.photo.showUpdatePrompt();
                });
            }
        });
    } else if (page.isChild()) {
        try { parent.isNeedSave = true; } catch (e) { }
    }
}

function deleteTempPhoto() {
    var path = module.getControlValue($I('txtPhoto'));
    page.photo.deleteTempPhoto(path);
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
        var urlparam = 'action=getModuleMenuToParent&data=' + encodeURIComponent(module.toJsonString(data));

        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
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
                        otParent.add({ id: 'none', pid: 0, name: '不选择上级菜单', isRoot: true, callback: _callback, param: { id: 0, name: '无'} });
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
    var urlparam = 'action=getModuleMenu&id=' + pid;
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                $I('txtParentId').value = jsondata.data.MenuName;

                if (typeof func == 'function') {
                    func(jsondata.data);
                }
            });
        }
    });
}