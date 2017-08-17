var PageName = '权限';

var pwEdit = null;
var isNeedSave = false;

var treeTable = null;
var fixedTable = null;

var tbList = null;

var leftWidth = leftWidthConfig = 180;
page.setSize({
    paddingTop: 0,
    leftWidth: leftWidth,
    leftWidthConfig: leftWidthConfig
});

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
    getTypeTree(function () { getDataList(); });
});

function setBodySize() {
    var bs = page.getSize();
    $('#treebox').height(bs.height - $('#bodyLeft .titlebar').outerHeight());
    setTableControl(true);
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildButton('editData();'), page.buildReload(), page.buildPageSize(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#bodyContent').append(page.buildListForm());

    $('#bodyLeft').html([
        '<div class="titlebar"><div class="title">权限分类</div></div>',
        '<div id="treebox" class="treebox"><div id="tree" class="tree"></div></div>'
    ].join(''));

    page.buildSearch([
        ['Name', '按名称搜索'], ['Code', '按编码搜索'], ['Id', '按ID搜索']
    ]);

    $('#ddlEnabled,#ddlOpenType').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });

    $('.select').width($('#txtName').outerWidth());
}

function getDataList(isReload) {
    var param = {
        TypeId: module.getControlValue($I('txtTypeId'), -1),
        GetSubset: 1,
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField')),
        PageIndex: page.pageIndex - page.pageStart,
        PageSize: page.pageSize
    };
    var urlparam = 'action=getPermissionList&data=' + encodeURIComponent(module.toJsonString(param));
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

    rd.push({ html: '序号', style: [['minWidth', '30px']] });
    rd.push({ html: '权限分类', style: [['minWidth', '80px']] });
    rd.push({ html: '权限名称/编码', style: [['minWidth', '150px']] });
    rd.push({ html: '权限描述', style: [['minWidth', '100px']] });
    rd.push({ html: '提示信息', style: [['minWidth', '100px']] });
    rd.push({ html: '操作', style: [['minWidth', '70px']] });
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
        var rnum = page.getRowNum(rid);

        row.lang = dr.PermissionId;
        row.ondblclick = function (e, i) {
            editData(this.lang);
        };

        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.PermissionId, page.lang["edit"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.PermissionId, dr.PermissionName, page.lang["delete"]])
        ];
        var name = '<div class="con">%s <em>%s</em></div>'.format([dr.PermissionName, dr.PermissionCode]);

        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.Extend.TypeName, style: [] });
        rd.push({ html: name, style: [] });
        rd.push({ html: dr.PermissionDesc, style: [] });
        rd.push({ html: dr.PermissionPrompt, style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.PermissionId, style: [] });

        cms.util.fillTable(row, rd);

        rid++;
    }
    page.dataCount = jsondata.dataCount;
    page.showLoadPrompt(page.dataCount <= 0);
    page.setPagination();

    setTableControl(true);
}

function setTableControl(isControl) {
    if (tbList != null) {
        if (!page.checkIsNull(fixedTable)) {
            fixedTable = null;
        }
        fixedTable = new oFixedTable('ofix1', tbList, { rows: 1, cols: 0 });

        window.setTimeout(function () { cms.util.setTableStyle(tbList); }, 300);
    }
}

function editData(id) {
    var tid = id > 0 ? 0 : module.getControlValue($I('txtTypeId'), -1);
    var title = page.buildTitle(PageName, id);
    var url = 'permissionEdit.aspx?permissionId=%s&typeId=%s'.format([id, tid]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([650, 400]);
    pwEdit = cms.box.winform({
        id: 'pwEdit', title: title, html: url, width: size[0], height: size[1],
        callback: function (pwo, pwr) {
            if (typeof isNeedSave == 'boolean' && isNeedSave) {
                cms.box.editSaveConfirm(pwo, function () { getDataList(true); });
            } else {
                pwo.Hide();
                getDataList(true);
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
        var urlparam = 'action=deletePermission&id=' + id;
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

function getTypeTree(callback) {
    var param = {};
    var urlparam = 'action=getPermissionTypeTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var config = {
                    callback: function (param, objTree) {
                        $('#txtTypeId').attr('value', param.id);
                        getDataList();
                    }
                };
                page.tree = new oTree('page.tree', $I('tree'), config);
                page.tree.add({ id: 'root', pid: 0, name: '权限分类', isRoot: true, callback: config.callback });
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    page.tree.add({ id: dr.id, pid: dr.pid, name: dr.name });
                }
            });
            if (page.isFunction(callback)) {
                callback();
            }
        }
    });
}