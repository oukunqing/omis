var PageName = 'GPS轨迹';
var RootId = module.getControlValue($I('txtRootId'), 0);
var TypeId = module.getControlLang($I('txtTypeId'), 0);

var pwEdit = null;
var isNeedSave = false;

var fixedTable = null;

var tbList = null;

var leftWidth = leftWidthConfig = 400;
page.setSize({
    paddingTop: 4,
    leftWidth: leftWidth,
    leftWidthConfig: leftWidthConfig
});

$(window).load(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {
    var bs = page.getSize();
    var hc = $('#bodyLeft .titlebar').outerHeight() + $('#bodyForm').outerHeight() + $('#statusbar').outerHeight() + 2;
    $('.treebox').height((bs.height - hc));

    $('#bodyListBox').height(bs.height - hc - 20);
    $('#bodyList').height(bs.height - hc - 20);

    setTableControl(true);
}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    $('#bodyLeft').html([
        '<div class="titlebar"><div class="title">数据</div><div class="tools">' + page.buildPageSize() + '</div></div>',
        '<div><div id="bodyForm" class="operform">',

        '</div></div>',
        '<div id="formbox" class="treebox"><div id="tree" class="tree"></div></div>',
        '<div id="statusbar" class="statusbar"><div id="pagination" class="pagination"></div></div>'
    ].join(''));

    $('#formbox').append(page.buildListForm());
    

    $('#ddlEnabled,#ddlOpenType').each(function () {
        $(this).css('margin-right', '3px');
        $(this).change(function () {
            page.loadDataList(true);
        });
    });

    initialMap();
}

var noRailMapStyle = [{
    featureType: "road.highway",
    stylers: [{
        visibility: "off"
    }]
}, {
    featureType: "road.arterial",
    stylers: [{
        visibility: "simplified"
    }, {
        lightness: 48
    }]
}, {
    featureType: "transit.station",
    stylers: [{
        visibility: "off"
    }]
}, {
    featureType: "transit.line",
    stylers: [{
        visibility: "simplified"
    }, {
        lightness: 54
    }]
}, {
    featureType: "administrative.locality",
    elementType: "labels",
    stylers: [{
        visibility: "off"
    }]
}, {
}];
var mapOptions = {
    center: new google.maps.LatLng(40, 116),
    zoom: 5,
    mapTypeControlOptions: {
        mapTypeIds: [google.maps.MapTypeId.HYBRID, google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.SATELLITE, google.maps.MapTypeId.TERRAIN]
    }
};
var RailwayMapTypeOption = {
    getTileUrl: function (coord, zoom) {
        var path = '';
        if (zoom <= 12) {
            //path = cms.util.path + "/tile/" + zoom + "/" + coord.x + "-" + coord.y + ".png";
            path = "http://img.railmap.cn/tile/" + zoom + "/" + coord.x + "-" + coord.y + ".png";
            //return "http://railmap.cn/tile/" + zoom + "/" + coord.x + "-" + coord.y + ".png";
        } else {
            //path = cms.util.path + "/tile/" + zoom + "/" + coord.x + "-" + coord.y + ".png";
            path = "http://img.railmap.cn/tile/" + zoom + "/" + coord.x + "-" + coord.y + ".png";
            //return "http://railmap.cn/tile/" + zoom + "/" + coord.x + "-" + coord.y + ".png";
        }
        //appendImagePath(path);
        return path;
    },
    tileSize: new google.maps.Size(256, 256),
    isPng: true,
    name: '铁路',
    alt: '铁路'
};


function initialMap() {
    var mapCanvas = $I('mapCanvas');
    var myLatlng = new google.maps.LatLng(30.593001325080845, 114.30587768554687);
    var myOptions = {
        zoom: 12,
        center: myLatlng,
        mapTypeControl: true,
        mapTypeControlOptions: {
            mapTypeIds: ['norailgmap', google.maps.MapTypeId.HYBRID, google.maps.MapTypeId.ROADMAP, google.maps.MapTypeId.SATELLITE, google.maps.MapTypeId.TERRAIN],
            style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
            position: google.maps.ControlPosition.TOP_RIGHT
        },
        panControl: true,
        zoomControl: true,
        scaleControl: true,
        streetViewControl: true,
        overviewMapControl: true,
        mapTypeId: google.maps.MapTypeId.HYBRID
    }
    map = new google.maps.Map(mapCanvas, myOptions);

    google.maps.event.addListener(map, 'click', function (event) {
        showClickLatLng(event);

        if (marker != null) {
            marker.setMap(null);
        }
        marker = new google.maps.Marker({
            position: event.latLng,
            map: map,
            title: "Hello World!"
        });
        google.maps.event.addListener(marker, 'click', function () {
            showMarkerLatLng(this);
        });
    });

    var baseNoRailMap = new google.maps.StyledMapType(noRailMapStyle, {
        name: '基本地图'
    });
    map.mapTypes.set('norailgmap', baseNoRailMap);
    //map.setMapTypeId('norailgmap');

    var RailwayMapType = new google.maps.ImageMapType(RailwayMapTypeOption);
    map.overlayMapTypes.insertAt(0, RailwayMapType);

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