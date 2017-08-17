var PageName = '导航菜单';

var pwEdit = null;
var isNeedSave = false;

var fixedTable = null;

var tbList = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
    getDataList();
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
}

function getDataList(isReload) {
    var param = {
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        OpenType: module.getControlValue($I('ddlOpenType'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField')),
        PageIndex: page.pageIndex - page.pageStart,
        PageSize: page.pageSize
    };
    var urlparam = 'action=getMenuList&data=' + encodeURIComponent(module.toJsonString(param));
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
    tbList = page.buildListTable('min-width:1150px;');
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '30px']] });
    rd.push({ html: '菜单分类', style: [['minWidth', '70px']] });
    rd.push({ html: '菜单名称', style: [['minWidth', '150px']] });
    rd.push({ html: '操作', style: [['minWidth', '80px']] });
    rd.push({ html: '打开方式', style: [['minWidth', '60px']] });
    rd.push({ html: '启用', style: [['minWidth', '50px']] });
    rd.push({ html: '排序', style: [['minWidth', '50px']] });
    rd.push({ html: 'URL地址', style: [['minWidth', '240px']] });
    rd.push({ html: '图标', style: [['minWidth', '30px']] });
    rd.push({ html: '创建时间', style: [['minWidth', '135px']] });
    rd.push({ html: 'ID', style: [['minWidth', '30px']] });

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
        var rnum = page.getRowNum(rid);

        row.lang = dr.MenuId;
        row.ondblclick = function (e, i) {
            editData(this.lang);
        };

        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.MenuId, page.lang["edit"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.MenuId, dr.MenuName, page.lang["delete"]])
        ];
        var name = '<div class="con">{0} <em>{1}</em></div>'.format2([dr.MenuName, dr.MenuCode]);
        var url = dr.MenuUrl.length > 0 ? '<div class="con"><a href="{0}" target="_blank" title="{1}">{2}</a></div>'.format2([page.buildUrl(dr.MenuUrl), dr.MenuUrl, showUrl ? dr.MenuUrl : page.getUrlName(dr.MenuUrl)]) : '';
        var pic = dr.MenuPic.length > 0 ? (showPic ? '<img src="' + webConfig.webDir + dr.MenuPic + '" style="width:25px;display:block;padding:1px;margin:0 auto;" />' : '有') : '无';

        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.Extend.TypeName, style: [] });
        rd.push({ html: name, style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: page.parseStatus(['链接跳转', '弹出窗口', '新窗口'], dr.OpenType), style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: url, style: [] });
        rd.push({ html: pic, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.MenuId, style: [] });

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
        fixedTable = new oFixedTable('ofix1', tbList, { rows: 1, cols: 3 });

        cms.util.setTableStyle(tbList);
    }
}

function editData(id) {
    var title = page.buildTitle(PageName, id);
    var url = 'menuEdit.aspx?menuId=%s'.format([id]);
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
        var urlparam = 'action=deleteMenu&id=' + id;
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