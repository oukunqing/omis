var PageName = '导航菜单';
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

    page.photo.initial({ updateCallback: photoUpdateCallback, maxCount:1 });
    page.photo.buildForm($('#divPhotoForm'), { dir: 'menu', type: 'icon', w: 60, h: 60, mode: 'HWR', delSource: true }, { id: 'txtPhoto' });

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
        MenuId: module.getControlValue($I('txtId'), 0),
        TypeId: module.getControlValue($('#ddlType'), 0),
        MenuName: module.getControlValue($I('txtName')),
        MenuCode: module.getControlValue($I('txtCode')),
        MenuUrl: module.getControlValue($I('txtUrl')),
        MenuPic: module.getControlValue($I('txtPhoto')),
        OpenType: module.getControlValue($I('ddlOpenType'), 0),
        Enabled: module.getControlValue($I('ddlEnabled'), 1),
        SortOrder: module.getControlValue($I('txtSortOrder'), 0)
    };

    if (formData.MenuName.length == 0) {
        cms.box.msgAndFocus($I('txtName'), { html: '请输入菜单名称' });
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editMenu&data=' + encodeURIComponent(module.toJsonString(formData));
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
    var urlparam = 'action=getMenu&id=' + DataId;
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
            $I('txtId'), $I('ddlType'), $I('txtName'), $I('txtCode'), $I('txtUrl'), $I('txtPhoto'), $I('ddlOpenType'), $I('ddlEnabled'), $I('txtSortOrder')
        ], [
            dr.MenuId, dr.TypeId, dr.MenuName, dr.MenuCode, dr.MenuUrl, dr.MenuPic, dr.OpenType, dr.Enabled, dr.SortOrder
        ]);

        page.photo.showPhoto(dr.MenuPic);
    }
}

function photoUpdateCallback(paths) {
    if (DataId > 0) {
        var urlparam = 'action=updateMenuPhoto&path=%s&id=%s'.format([paths, DataId]);
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