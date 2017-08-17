var loc = loc || {};
//地图类型
loc.mapType = ['gis', 'map'];

//地图标记类型
loc.arrLocationType = {};
loc.isAnchor = false;

//地图图标目录
loc.iconDir = webConfig.webDir + '/skin/default/imgs/railway/gis/';

loc.initialLayer = function () {
    var objLayer = $I('mapLayer');
    var strLayer = '<div class="title"><a id="lblAnchor" class="anchor anchor-un"></a>地图标记</div>'
                 + '<div id="locationList"></div>';
    objLayer.innerHTML = strLayer;
    objLayer.onmouseover = function () {
        loc.showLayer(this, true);
    };
    objLayer.onmouseout = function () {
        loc.showLayer(this, false);
    };
    $I('lblAnchor').onclick = function () {
        loc.setLayerAnchor(this);
    };
};

loc.showLayer = function (obj, show) {
    if (show) {
        obj.className = 'map-layer';
    } else {
        if (!loc.isAnchor) {
            obj.className = 'map-layer-hide';
        }
    }
};

loc.setLayerAnchor = function(obj) {
    loc.isAnchor = !loc.isAnchor;
    obj.className = 'anchor' + (loc.isAnchor ? ' anchor-on' : ' anchor-un');
};

loc.getLocationType = function (mapType) {
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/gis.aspx',
        data: 'action=getLocationType',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                loc.showLocationType(jsondata.list, mapType);
            });
        }
    });
};

loc.showLocationType = function (list, mapType) {
    var strHtml = '<ul>';
    for (var i in list) {
        var dt = list[i];
        strHtml += '<li>'
                + '<label class="chb-label-nobg">'
                + '<input type="checkbox" class="chb" onclick="loc.getLocationList(\'%s\', \'%s\', this.checked);" /><span>%s(%s)</span>'.format([dt.type, mapType, dt.name, dt.count])
                + '</label>'
                + '</li>';
        loc.arrLocationType[dt.type] = { type: dt.type, name: dt.name, load: false, check: false, list: [], markers: [] };
    }
    strHtml += '</ul>';

    $('#locationList').html(strHtml);
};

loc.getLocationList = function (type, mapType, isShow) {
    var dr = loc.arrLocationType[type];
    if (!dr.load) {
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/gis.aspx',
            data: 'action=getLocationList&type=' + type,
            callback: function (data, param) {
                module.ajaxResponse(data, param, function (jsondata, param) {
                    for (var i in jsondata.list) {
                        var dt = jsondata.list[i];
                        if (mapType == loc.mapType[0]) {
                            if (checkLatLng(dt.lat, dt.lng)) {
                                //将action参数作为ID使用
                                dt.action = dr.type;
                                //将ID加上类型前缀，因不同的内容会出现相同的ID
                                dt.id = dt.type + '_' + dt.id;
                                dt.type = type;
                                dr.list.push(dt);
                                dr.markers.push(showMarker(dt));
                            }
                        } else {
                            dt.type = type;
                            dr.list.push(dt);
                            if (dt.x > 0 && dt.y > 0) {
                                dr.markers.push(showMarker(dt));
                            }
                        }
                    }
                });
            }
        });
        dr.load = true;
    } else {
        if (isShow) {
            for (var i in dr.list) {
                var dt = dr.list[i];
                if (mapType == loc.mapType[0]) {
                    if (checkLatLng(dt.lat, dt.lng)) {
                        //将action参数作为ID使用
                        dt.action = dr.type;
                        //将ID加上类型前缀，因不同的内容会出现相同的ID
                        dt.id = dt.type + '_' + dt.id;

                        dr.markers.push(showMarker(dt));
                    }
                } else {
                    if (dt.x > 0 && dt.y > 0) {
                        dr.markers.push(showMarker(dt));
                    }
                }
            }
        } else {
            for (var i in dr.list) {
                var dt = dr.list[i];
                if (mapType == loc.mapType[0]) {
                    if (checkLatLng(dt.lat, dt.lng)) {
                        hideMarker(dt);
                    }
                } else {
                    hideMarker(dr.markers[i]);
                }
            }
            dr.markers = null;
            dr.markers = [];
        }
    }
};

loc.initialMenu = function (obj, arrMenu) {
    if (obj == null || typeof obj == 'undefined' || typeof arrMenu == 'undefined') {
        return false;
    }
    var strMenu = '<ul>';
    for (var i in arrMenu) {
        strMenu += loc.buildMenu(arrMenu[i], i);
    }
    strMenu += '</ul>';

    obj.innerHTML = strMenu;
};

loc.buildMenu = function (param, idx) {
    return '<li><a onclick="%s(%s,%s);" id="%s" title="%s" class="btn btnc30"><i class="%s"></i><span>%s</span></a></li>'.format([
        param.func, idx, (param.param || '\'\''), (param.id || ''), (param.title || ''), param.icon, param.name
    ]);
};