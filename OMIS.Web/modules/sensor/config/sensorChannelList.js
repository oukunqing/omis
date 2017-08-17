var PageName = '传感器通道';

var pwEdit = null;
var isNeedSave = false;
var pwConfig = null;

var isShowPermission = false;

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
        '<div class="tools">', page.buildButton('editData(0);'), page.buildReload(), page.buildPageSize(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#bodyContent').append(page.buildListForm());

    page.buildSearch([
        ['Number', '按通道号搜索'], ['Id', '按ID搜索']
    ]);

    $('#ddlType,#ddlMode,#ddlEnabled').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });

    getModeList();
}

function getModeList() {
    var param = {
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField'))
    };
    var urlparam = 'action=getSensorChannelModeTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, function (jsondata, param) {
                var obj = $I('ddlMode');
                for (var i = 0, c = jsondata.list.length; i < c; i++) {
                    var dr = jsondata.list[i];
                    cms.util.fillOption(obj, dr.id, dr.name);
                }
            });
            page.showLoading(false);
        }
    });
}

function getDataList(isReload) {
    var param = {
        ChannelType: module.getControlValue($I('ddlType'), -1),
        ModeId: module.getControlValue($I('ddlMode'), -1),
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField')),
        PageIndex: page.pageIndex - page.pageStart,
        PageSize: page.pageSize
    };
    var urlparam = 'action=getSensorChannelList&data=' + encodeURIComponent(module.toJsonString(param));
    if (page.checkLoadEnabled(urlparam, isReload)) {
        showDataHead();
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
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
    tbList = page.buildListTable('min-width:1200px;width:auto;');
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '35px']] });
    rd.push({ html: '通道号', style: [['minWidth', '60px']] });
    rd.push({ html: '通道分类', style: [['minWidth', '80px']] });
    rd.push({ html: '通道类型', style: [['minWidth', '100px']] });
    rd.push({ html: '通道组', style: [['minWidth', '50px']] });
    rd.push({ html: '原始值类型', style: [['minWidth', '100px']] });
    rd.push({ html: '操作', style: [['minWidth', '70px']] });
    rd.push({ html: '参数配置', style: [['minWidth', '60px']] });
    rd.push({ html: '备注', style: [['minWidth', '120px']] });
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

        row.lang = dr.ChannelId;

        row.ondblclick = function (e, i) {
            editData(this.lang);
        };
        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.ChannelId, page.lang["edit"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.ChannelId, dr.ChannelNo, page.lang["delete"]])
        ];
        var config = ['<a onclick="setConfig(%s);">%s</a>'.format([dr.ChannelNo, '配置'])];
        var remark = '<div class="con">%s</div>'.format([dr.Remark]);

        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.ChannelNo, style: [] });
        rd.push({ html: page.parseOption(['虚拟通道', '真实通道'], dr.ChannelType), style: [] });
        rd.push({ html: dr.Extend.ModeName, style: [] });
        rd.push({ html: dr.ChannelGroup, style: [] });
        rd.push({ html: dr.Extend.OriTypeName, style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: config.join('|'), style: [] });
        rd.push({ html: remark, style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.ChannelId, style: [] });

        cms.util.fillTable(row, rd);

        rid++;
    }

    page.dataCount = jsondata.dataCount;
    page.showLoadPrompt(page.dataCount <= 0);
    page.setPagination();

    setTableControl(true, true);
}

function setTableControl(isControl, isTree) {
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
    var url = 'sensorChannelEdit.aspx?channelId=%s'.format([id]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([600, 400]);
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
        var urlparam = 'action=deleteSensorChannel&id=' + id;
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
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

function setConfig(no) {
    var title = page.buildTitle('通道-参数配置');
    var url = 'sensorChannelParamConfig.aspx?channelNo=%s'.format([no]);
    var bodySize = cms.util.getBodySize();
    var size = page.checkWinSize([800, 550]);
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