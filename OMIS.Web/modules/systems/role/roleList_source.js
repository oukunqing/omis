var PageName = '角色';

var pwEdit = null;
var isNeedSave = false;

var fixedTable = null;

var tbList = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {

    setTableControl(true);
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildButton('editData(0);'), page.buildReload(), page.buildPageSize(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#bodyContent').append(page.buildListForm());

    page.buildSearch([
        ['Name', '按名称搜索'], ['Code', '按编码搜索'], ['Id', '按ID搜索']
    ]);

    $('#ddlEnabled,#ddlGroup').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });

    getRoleGroup(function () {
        getDataList();
    });
}

function getRoleGroup(callback) {
    var urlparam = 'action=getRoleGroup&data=';
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var ddl = $I('ddlGroup');
                cms.util.fillOption(ddl, "-1", "请选择");
                for (var i = 0, c = jsondata.list.length; i < c; i++) {
                    var dr = jsondata.list[i];
                    cms.util.fillOption(ddl, dr.GroupId, dr.GroupName);
                }
                if (page.isFunction(callback)) {
                    callback();
                }
            });
        }
    });
}

function getDataList(isReload) {
    var param = {
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        GroupId: module.getControlValue($I('ddlGroup'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField')),
        PageIndex: page.pageIndex - page.pageStart,
        PageSize: page.pageSize
    };
    var urlparam = 'action=getRoleList&data=' + encodeURIComponent(module.toJsonString(param));
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
    tbList = page.buildListTable('min-width:1100px;');
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '30px']] });
    rd.push({ html: '角色组别', style: [['minWidth', '100px']] });
    rd.push({ html: '角色名称', style: [['minWidth', '200px']] });
    rd.push({ html: '角色描述', style: [['minWidth', '120px']] });
    rd.push({ html: '操作', style: [['minWidth', '70px']] });
    rd.push({ html: '模块权限', style: [['minWidth', '60px']] });
    rd.push({ html: '导航菜单', style: [['minWidth', '60px']] });
    rd.push({ html: '模块菜单', style: [['minWidth', '60px']] });
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

        row.lang = dr.RoleId;
        row.ondblclick = function (e, i) {
            editData(this.lang);
        };

        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.RoleId, page.lang["edit"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.RoleId, dr.RoleName, page.lang["delete"]])
        ];
        var name = '<div class="con">%s <em>%s</em></div>'.format([dr.RoleName, dr.RoleCode]);
        var desc = '<div class="con">%s</div>'.format([dr.RoleDesc]);
        var set1 = '<a onclick="setConfig(%s, 1);">%s</a>'.format([dr.RoleId, '设置']);
        var set2 = '<a onclick="setConfig(%s, 2);">%s</a>'.format([dr.RoleId, '设置']);
        var set3 = '<a onclick="setConfig(%s, 3);">%s</a>'.format([dr.RoleId, '设置']);

        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.Extend.GroupName, style: [] });
        rd.push({ html: name, style: [] });
        rd.push({ html: desc, style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: set1, style: [] });
        rd.push({ html: set2, style: [] });
        rd.push({ html: set3, style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.RoleId, style: [] });

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
    var title = page.buildTitle(PageName, id);
    var url = 'roleEdit.aspx?roleId=%s&groupId=%s'.format([id]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([650, 450]);
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
        var urlparam = 'action=deleteRole&id=' + id;
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

function setConfig(id, type) {
    var title = page.buildTitle('角色-模块-权限配置');
    var url = 'roleModulePermission.aspx?roleId=%s'.format([id]);
    var bodySize = cms.util.getBodySize();
    var size = page.checkWinSize([800, 550]);
    switch (type) {
        case 1:
            title = page.buildTitle('角色-模块-权限配置');
            url = 'roleModulePermission.aspx?roleId=%s'.format([id]);
            break;
        case 2:
            title = page.buildTitle('角色-导航菜单配置');
            url = 'roleMenu.aspx?roleId=%s'.format([id]);
            size = page.checkWinSize([700, 500]);
            break;
        case 3:
            title = page.buildTitle('角色-模块菜单配置');
            url = 'roleModuleMenu.aspx?roleId=%s'.format([id]);
            size = page.checkWinSize([500, 520]);
            break;
    }

    pwConfig = cms.box.winform({
        id: 'pwConfig' + type, title: title, html: url, width: size[0], height: size[1],
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