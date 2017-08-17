var PageName = '字典';
var RootId = module.getControlValue($I('txtRootId'), 0);
var TypeId = module.getControlLang($I('txtTypeId'), 0);

var pwEdit = null;
var isNeedSave = false;

var fixedTable = null;

var tbList = null;

var leftWidth = leftWidthConfig = 180;
page.setSize({
    paddingTop: 0,
    leftWidth: leftWidth,
    leftWidthConfig: leftWidthConfig
});

$(window).load(function () {
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
        '<div class="titlebar"><div class="title">字典分类</div></div>',
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
    var urlparam = 'action=getDictionaryList&data=' + encodeURIComponent(module.toJsonString(param));
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
    tbList = page.buildListTable('width:auto;');
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '30px']] });
    rd.push({ html: '字典分类', style: [['minWidth', '100px']] });
    rd.push({ html: '字典名称', style: [['minWidth', '150px']] });
    rd.push({ html: '字典编码', style: [['minWidth', '120px']] });
    rd.push({ html: '字典编号', style: [['minWidth', '70px']] });
    rd.push({ html: '操作', style: [['minWidth', '80px']] });
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

        row.lang = dr.DictionaryId;
        row.ondblclick = function (e, i) {
            editData(this.lang);
        };

        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.DictionaryId, page.lang["edit"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.DictionaryId, dr.DictionaryName, page.lang["delete"]])
        ];
        var name = '<div class="con">%s <em>%s</em></div>'.format([dr.DictionaryName, dr.DictionaryCode]);
        
        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.Extend.TypeName, style: [] });
        rd.push({ html: name, style: [] });
        rd.push({ html: dr.DictionaryCode, style: [] });
        rd.push({ html: dr.DictionaryNumber, style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.DictionaryId, style: [] });

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
    var url = 'dictionaryEdit.aspx?dictionaryId=%s&typeId=%s'.format([id, tid]);
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
        var urlparam = 'action=deleteDictionary&id=' + id;
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
    var param = { ParentId: module.getControlValue($I('txtTypeId'), -1), GetSubset: 1 };
    var urlparam = 'action=getDictionaryTypeToParent&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var config = {
                    callback: function (param, objTree) {
                        $('#txtTypeId').attr('value', param.id);
                        objTree.isCacheCallback ? window.setTimeout(function () { getDataList() }, 100) : getDataList();
                    },
                    loadedCallback: function (objTree) {
                        //objTree.openLevel(0);
                    },
                    isCache: true
                };
                page.tree = new oTree('page.tree', $I('tree'), config);
                page.tree.add({ id: 'root', pid: 0, name: '字典分类', isRoot: true, callback: config.callback });
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    page.tree.add({ id: dr.id, pid: dr.pid, name: dr.name });
                }
                if (page.isFunction(callback)) {
                    callback();
                }
            });
        }
    });
}