var PageName = '模块菜单';
var ParentId = 0;

var pwEdit = null;
var isNeedSave = false;

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

    $('#bodyForm').append([
        '<label class="chb-label-nobg" style="float:right;height:20px;margin:0;padding:0;">',
        '<input type="checkbox" id="chbShowPic" class="chb" onclick="getDataList(true);" />',
        '<span>显示图标</span></label>',
        '<label class="chb-label-nobg" style="float:right;height:20px;margin:0 5px 0 0;padding:0;">',
        '<input type="checkbox" id="chbShowUrl" class="chb" onclick="getDataList(true);" />',
        '<span>显示完整URL</span></label>'
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

    ParentId = module.getControlValue($I('txtParentId'), 0);
}

function getDataList(isReload) {
    var param = {
        ParentId: ParentId,
        GetSubset: 1,
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        OpenType: module.getControlValue($I('ddlOpenType'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField'))
    };
    var urlparam = 'action=getModuleMenuList&data=' + encodeURIComponent(module.toJsonString(param));
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
    tbList = page.buildListTable('min-width:1250px;');
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '35px']] });
    rd.push({ html: '菜单名称', style: [['minWidth', '320px']] });
    rd.push({ html: '操作', style: [['minWidth', '110px']] });
    rd.push({ html: '菜单分类', style: [['minWidth', '60px']] });
    rd.push({ html: '打开方式', style: [['minWidth', '60px']] });
    rd.push({ html: '层级', style: [['minWidth', '40px']] });
    rd.push({ html: '启用', style: [['minWidth', '45px']] });
    rd.push({ html: '排序', style: [['minWidth', '45px']] });
    rd.push({ html: 'URL地址', style: [['minWidth', '240px']] });
    rd.push({ html: '图标', style: [['minWidth', '26px']] });
    rd.push({ html: '创建时间', style: [['minWidth', '135px']] });
    rd.push({ html: 'ID', style: [['minWidth', '40px']] });

    cms.util.fillTable(row, rd);
}

function showDataList(jsondata, param) {
    var rid = 1;
    cms.util.clearDataRow(tbList, rid);

    var showPic = $I('chbShowPic').checked;
    var showUrl = $I('chbShowUrl').checked;

    for (var i = 0, c = jsondata.list.length; i < c; i++) {
        var dr = jsondata.list[i];
        var row = tbList.insertRow(rid);
        var rd = [];

        row.lang = '{id:%s, pid:%s, level:%s}'.format([dr.MenuId, dr.ParentId, dr.Level]);

        row.ondblclick = function (e, i) {
            editData(this.lang.toJson().id);
        };
        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.MenuId, page.lang["edit"]]),
            '<a onclick="editData(0, %s);">%s</a>'.format([dr.MenuId, page.lang["add"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.MenuId, dr.MenuName, page.lang["delete"]])
        ];

        var name = '<div class="con">{0} <em>{1}</em></div>'.format2([dr.MenuName, dr.MenuCode]);
        var url = dr.MenuUrl.length > 0 ? '<div class="con"><a href="{0}" target="_blank" title="{1}">{2}</a></div>'.format2([page.buildUrl(dr.MenuUrl), dr.MenuUrl, showUrl ? dr.MenuUrl : page.getUrlName(dr.MenuUrl)]) : '';
        var pic = dr.MenuPic.length > 0 ? (showPic ? '<img src="' + webConfig.webDir + dr.MenuPic + '" style="width:18px;display:block;padding:2px 4px;" />' : '有') : '无';

        rd.push({ html: rid, style: [] });
        rd.push({ html: name, style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: page.parseStatus(['页面', '模块'], dr.MenuType), style: [] });
        rd.push({ html: page.parseStatus(['-', '默认打开'], dr.OpenType), style: [] });
        rd.push({ html: dr.Level, style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: url, style: [] });
        rd.push({ html: pic, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.MenuId, style: [] });

        cms.util.fillTable(row, rd);

        rid++;
    }

    var dataCount = jsondata.dataCount;
    page.showLoadPrompt(dataCount <= 0);

    setPagination(dataCount);
    setTableControl(true, true);
}

function setPagination(dataCount) {
    $('#bodyBottom').html('<span style="margin:0 5px;">共有' + dataCount + '个模块菜单</span>');
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
    var url = 'moduleMenuEdit.aspx?menuId=%s&parentId=%s'.format([id, pid || ParentId]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([760, 500]);
    pwEdit = cms.box.winform({
        id: 'pwEdit', title: title, html: url, width: size[0], height: size[1],
        callback: function (pwo, pwr) {
            if (typeof isNeedSave == 'boolean' && isNeedSave) {
                cms.box.editSaveConfirm(pwo, function () {
                    if (!id) { $("#pwIFramepwEdit")[0].contentWindow.deleteTempPhoto(); }
                    getDataList(true);
                });
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
        var urlparam = 'action=deleteModuleMenu&id=' + id;
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