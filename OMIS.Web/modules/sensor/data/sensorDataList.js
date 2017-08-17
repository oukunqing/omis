var PageName = '传感器数据';

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
        ['DeviceCode', '按设备编号搜索'], ['SensorCode', '按传感器编码搜索'], ['ChannelNo', '按通道号搜索']
    ]);
    
    $("#txtStartTime").focus(function () {
        WdatePicker({ skin: 'ext', minDate: '2016-01-01', dateFmt: 'yyyy-MM-dd HH:mm:ss' });
    });
    $("#txtEndTime").focus(function () {
        WdatePicker({ skin: 'ext', minDate: '2016-01-01', dateFmt: 'yyyy-MM-dd HH:mm:ss' });
    });

    $('#txtStartTime,#txtEndTime').change(function () {
        page.loadDataList(true);
    });

    $('#ddlDataType').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });
}

function getDataList(isReload) {
    var param = {
        DataType: module.getControlValue($I('ddlDataType'), -1),
        Keywords: module.getControlValue($I('txtKeywords')),
        SearchField: module.getControlValue($I('ddlSearchField')),
        StartTime: module.getControlValue($I('txtStartTime')),
        EndTime: module.getControlValue($I('txtEndTime')),
        PageIndex: page.pageIndex - page.pageStart,
        PageSize: page.pageSize
    };
    var urlparam = 'action=getSensorDataList&data=' + encodeURIComponent(module.toJsonString(param));
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
    tbList = page.buildListTable('min-width:1200px;', 'tbList', 1);
    cms.util.clearDataRow(tbList, 0);
    var row = tbList.insertRow(0);
    row.className = 'trheader';

    var rd = [];

    rd.push({ html: '序号', style: [['minWidth', '35px']] });
    rd.push({ html: '设备编号', style: [['minWidth', '120px']] });
    rd.push({ html: '通道号', style: [['minWidth', '60px']] });
    rd.push({ html: '数据类型', style: [['minWidth', '70px']] });
    rd.push({ html: '传感器类型', style: [['minWidth', '100px']] });
    rd.push({ html: '传感器编码', style: [['minWidth', '100px']] });
    rd.push({ html: '传感器数据', style: [['minWidth', '80px']] });
    rd.push({ html: '原始值', style: [['minWidth', '70px']] });
    rd.push({ html: '单位', style: [['minWidth', '60px']] });
    rd.push({ html: '采集时间', style: [['minWidth', '135px']] });
    rd.push({ html: '上传时间', style: [['minWidth', '135px']] });
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

        row.lang = dr.Id;

        row.ondblclick = function (e, i) {

        };

        rd.push({ html: rnum, style: [] });
        rd.push({ html: dr.DeviceCode, style: [] });
        rd.push({ html: dr.ChannelNo, style: [] });
        rd.push({ html: page.parseOption(['传感器数据', '原始值'], dr.DataType), style: [] });
        rd.push({ html: dr.Extend.TypeName, style: [] });
        rd.push({ html: dr.SensorCode, style: [] });
        rd.push({ html: dr.SensorValue, style: [] });
        rd.push({ html: dr.OriginalValue, style: [] });
        rd.push({ html: dr.Extend.DataUnit, style: [] });
        rd.push({ html: dr.CollectTime, style: [] });
        rd.push({ html: dr.UploadTime, style: [] });
        rd.push({ html: dr.CreateTime, style: [] });
        rd.push({ html: dr.Id, style: [] });

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