﻿var PageName = '模块';
var ParentId = 0;

var pwEdit = null;
var isNeedSave = false;
var pwConfig = null;

var isShowPermission = false;

var treeTable = null;
var fixedTable = null;

var tbList = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
    getDataList();
});

function setBodySize() {
    setTableControl(true, false);
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildButton('editData(0);'), page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#bodyContent').append(page.buildListForm());

    page.buildSearch([
        ['Name', '按名称搜索'], ['Code', '按编码搜索'], ['Id', '按ID搜索']
    ]);

    $('#bodyForm').append(['<label class="chb-label-nobg" style="float:right;">',
    '<input type="checkbox" id="chbShowPermission" class="chb" onclick="ShowPermission();" />',
    '<span>显示权限配置</span>',
    '</label>'].join(''));

    $('#ddlEnabled').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });

    ParentId = module.getControlValue($I('txtParentId'), 0);
}

function ShowPermission() {
    isShowPermission = $I('chbShowPermission').checked;
    getDataList();
}

function getDataList(isReload) {
    var param = {
        ParentId: ParentId,
        GetSubset: 1,
        ShowPermission: isShowPermission ? 1 : 0,
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField'))
    };
    var urlparam = 'action=getModuleList&data=' + encodeURIComponent(module.toJsonString(param));
    if (page.checkLoadEnabled(urlparam, isReload)) {
        showDataHead();
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, showDataList);
                page.showLoading(false);
            }
        });
    }
}

function showDataHead() {
    if (page.isLoadHead()) {
        return false;
    }
    tbList = page.buildListTable('min-width:1000px;');
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '35px']] });
    rd.push({ html: '模块名称', style: [['minWidth', '240px']] });
    rd.push({ html: '模块描述', style: [['minWidth', '120px']] });
    if (isShowPermission) {
        rd.push({ html: '权限配置', style: [['minWidth', '120px']] });
    }
    rd.push({ html: '操作', style: [['minWidth', '110px']] });
    rd.push({ html: '权限配置', style: [['minWidth', '60px']] });
    rd.push({ html: '层级', style: [['minWidth', '40px']] });
    rd.push({ html: '启用', style: [['minWidth', '45px']] });
    rd.push({ html: '排序', style: [['minWidth', '45px']] });
    rd.push({ html: '创建时间', style: [['minWidth', '135px']] });
    rd.push({ html: 'ID', style: [['minWidth', '40px']] });

    cms.util.fillTable(row, rd);
}

function showDataList(jsondata, param) {
    var rid = 1;
    cms.util.clearDataRow(tbList, rid);

    for (var i = 0, c = jsondata.list.length; i < c; i++) {
        var dr = jsondata.list[i];
        var row = tbList.insertRow(rid);
        var rd = [];

        row.lang = '{id:%s, pid:%s, level:%s}'.format([dr.ModuleId, dr.ParentId, dr.Level]);

        row.ondblclick = function (e, i) {
            editData(this.lang.toJson().id);
        };
        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.ModuleId, page.lang["edit"]]),
            '<a onclick="editData(0, %s);">%s</a>'.format([dr.ModuleId, page.lang["add"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.ModuleId, dr.ModuleName, page.lang["delete"]])
        ];
        var set = '<a onclick="setConfig(%s);">%s</a>'.format([dr.ModuleId, '设置']);

        var name = '<div class="con">%s <em>%s</em></div>'.format([dr.ModuleName, dr.ModuleCode]);
        
        rd.push({ html: rid, style: [] });
        rd.push({ html: name, style: [] });
        rd.push({ html: dr.ModuleDesc, style: [['textAlign', 'left']] });
        if (isShowPermission) {
            var per = '<div class="con">%s</div>'.format([dr.Extend.PermissionName || '无']);
            rd.push({ html: per, style: [] });
        }
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: set, style: [] });
        rd.push({ html: dr.Level, style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.ModuleId, style: [] });

        cms.util.fillTable(row, rd);

        rid++;
    }

    var dataCount = jsondata.dataCount;
    page.showLoadPrompt(dataCount <= 0);

    setPagination(dataCount);
    setTableControl(true, true);
}

function setPagination(dataCount) {
    $('#bodyBottom').html('<span style="margin:0 5px;">共有' + dataCount + '个模块</span>');
}

function setTableControl(isControl, isTree) {
    if (tbList != null) {
        if (!page.checkIsNull(fixedTable)) {
            fixedTable = null;
        }
        fixedTable = new oFixedTable('ofix1', tbList, { rows: 1, cols: 0 });

        if (isTree) {
            treeTable = new oTreeTable('otb', tbList, {
                skin: 'default', showIcon: true, cellIndex: 1, rowNumber: { setRowNumber: true, cellIndex: 0, rowIndex: 1 }
            });

            //treeTable.expandLevel(0);
        }

        window.setTimeout(function () { cms.util.setTableStyle(tbList); }, 300);
    }
}

function treeTableExpand(level) {
    if (!page.checkIsNull(treeTable)) {
        treeTable.expandLevel(level);
    }
}

function editData(id, pid) {
    var title = page.buildTitle(PageName, id);
    var url = 'moduleEdit.aspx?moduleId=%s&parentId=%s'.format([id, pid || ParentId]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([760, 500]);
    pwEdit = cms.box.winform({
        id: 'pwEdit', title: title, html: url, width: size[0], height: size[1],
        callback: function (pwo, pwr) {
            if (typeof isNeedSave == 'boolean' && isNeedSave) {
                cms.box.editSaveConfirm(pwo, function () { getDataList(true); });
            } else {
                pwo.Hide();
            }
        }
    });

    cms.util.setWindowStatus();

    //每次打开编辑窗口时，清除保存提示记录
    isNeedSave = false;
}

function editCallback(isClose, isLoad) {
    if (pwEdit != null && (typeof isClose == 'undefined' || isClose)) {
        pwEdit.Hide();
    }
    if (typeof isLoad == 'undefined' || isLoad) {
        getDataList(true);
    }
}

function deleteData(id, name) {
    if (id <= 0) {
        return false;
    }
    page.deleteConfirm(function () {
        var urlparam = 'action=deleteModule&id=' + id;
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/system.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    getDataList(true);
                    cms.box.alert(page.buildDeletePrompt(name));
                });
            }
        });
    }, name);
}

function setConfig(id) {
    var title = page.buildTitle('模块-权限配置');
    var url = 'modulePermission.aspx?moduleId=%s'.format([id]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([760, 500]);
    pwConfig = cms.box.winform({
        id: 'pwConfig', title: title, html: url, width: size[0], height: size[1],
        callback: function (pwo, pwr) {
            pwo.Hide();
        }
    });
}

function setConfigCallback(isClose, isLoad) {
    if (pwConfig != null && (typeof isClose == 'undefined' || isClose)) {
        pwConfig.Hide();
    }
    if (typeof isLoad == 'undefined' || isLoad) {
        getDataList(true);
    }
}