var PageName = '传感器参数';

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
        '<div class="tools">', page.buildButton('editData(0);'), page.buildReload(), page.buildPageSize(10), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#bodyContent').append(page.buildListForm());

    page.buildSearch([
        ['Name', '按名称搜索'], ['Code', '按编码搜索'], ['Id', '按ID搜索']
    ]);

    $('#ddlParamType,#ddlParamMode,#ddlEnabled').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });
}

function getDataList(isReload) {
    var param = {
        ParamType: module.getControlValue($I('ddlParamType'), -1),
        ParamMode: module.getControlValue($I('ddlParamMode'), -1),
        Enabled: module.getControlValue($I('ddlEnabled'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField')),
        PageIndex: page.pageIndex - page.pageStart,
        PageSize: page.pageSize
    };
    var urlparam = 'action=getSensorParamList&data=' + encodeURIComponent(module.toJsonString(param));
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
    tbList = page.buildListTable('min-width:1600px;', 'tbList', 1);
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '35px']] });
    rd.push({ html: '参数名称', style: [['minWidth', '120px']] });
    rd.push({ html: '参数编码', style: [['minWidth', '80px']] });
    rd.push({ html: '参数功能', style: [['minWidth', '100px']] });
    rd.push({ html: '参数分类', style: [['minWidth', '70px']] });
    rd.push({ html: '参数类型', style: [['minWidth', '60px']] });
    rd.push({ html: '是否显示', style: [['minWidth', '60px']] });
    rd.push({ html: '值类型', style: [['minWidth', '70px']] });
    rd.push({ html: '值选项', style: [['minWidth', '120px']] });
    rd.push({ html: '是否必填', style: [['minWidth', '60px']] });
    rd.push({ html: '默认值', style: [['minWidth', '80px']] });
    rd.push({ html: '字符长度', style: [['minWidth', '60px']] });
    rd.push({ html: '操作', style: [['minWidth', '70px']] });
    rd.push({ html: '启用', style: [['minWidth', '45px']] });
    rd.push({ html: '参数说明', style: [['minWidth', '100px'], ['maxWidth', '400px']] });
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

        row.lang = dr.ParamId;

        row.ondblclick = function (e, i) {
            editData(this.lang);
        };
        var oper = [
            '<a onclick="editData(%s);">%s</a>'.format([dr.ParamId, page.lang["edit"]]),
            '<a onclick="deleteData(%s,\'%s\');">%s</a>'.format([dr.ParamId, dr.ChannelNo, page.lang["delete"]])
        ];
        var desc = '<div class="con">%s</div>'.format([dr.ParamDesc]);
        var option = '<div class="con">%s</div>'.format([dr.ValueOption]);

        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.ParamName, style: [] });
        rd.push({ html: dr.ParamCode, style: [] });
        rd.push({ html: dr.ParamFunc, style: [] });
        rd.push({ html: page.parseOption(['通道参数', '设备参数'], dr.ParamType), style: [] });
        rd.push({ html: page.parseOption(['通用', '模拟量', '数字量'], dr.ParamMode), style: [] });
        rd.push({ html: page.parseOption(['不显示', '显示'], dr.ConfigShow), style: [] });
        rd.push({ html: page.parseOption(['填写', '单个值', '多个值选择', '范围选择'], dr.ValueType), style: [] });
        rd.push({ html: option, style: [] });
        rd.push({ html: page.parseOption(['可不填', '必填'], dr.Required), style: [] });
        rd.push({ html: dr.DefaultValue, style: [] });
        rd.push({ html: dr.CharLength > 0 ? dr.CharLength : '-', style: [] });
        rd.push({ html: oper.join('|'), style: [] });
        rd.push({ html: page.parseEnabled(dr.Enabled), style: [] });
        rd.push({ html: desc, style: [] });
        rd.push({ html: dr.SortOrder, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.ParamId, style: [] });

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
        var cols = 3;
        cols > 0 ? cms.util.setTableStyle(tbList) : window.setTimeout(function () { cms.util.setTableStyle(tbList); }, 300);

        fixedTable = new oFixedTable('ofix1', tbList, { rows: 1, cols: cols });
    }
}

function editData(id) {
    var title = page.buildTitle(PageName, id);
    var url = 'sensorParamEdit.aspx?paramId=%s'.format([id]);
    var bodySize = cms.util.getBodySize();

    var size = page.checkWinSize([720, 500]);
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
        var urlparam = 'action=deleteSensorParam&id=' + id;
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