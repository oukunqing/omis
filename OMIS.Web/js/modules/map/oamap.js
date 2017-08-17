var oamap = oamap || {};
oamap.zoom = 0;
oamap.scale = 1000000;
oamap.bround = [];
oamap.isInitial = false;
oamap.mapCenter = {lat:0, lng: 0};
oamap.frameId = 'mapGis';
oamap.boxSize = null;
oamap.initialCallback = null;
oamap.eventCallback = null;
oamap.eventTimer = null;

/*
    初始化GIS地图
    callBack: 需要在地图加载完成后 调用的函数
*/
oamap.initialEmap = function (obj, boxSize, callback, eventCallback) {
    oamap.boxSize = boxSize;

    if (obj == undefined) {
        obj = cms.util.$('oamapCanvas');
    }
    var strPage = cms.util.path + '/arcgis/index.aspx?' + new Date().getTime();
    var strHtml = '<iframe name="mapGis" id="mapGis" height="100%" width="100%" frameborder="0" src="' + strPage + '" scrolling="no">'
        + '浏览器不支持嵌入式框架，或被配置为不显示嵌入式框架。'
        + '</iframe>';

    obj.innerHTML = strHtml;

    if (typeof callback == 'function') {
        oamap.initialCallback = callback;
    }
    if (typeof eventCallback == 'function') {
        oamap.eventCallback = eventCallback;
    }
};

//地图加载完成后调用
oamap.returnInitial = function () {
    oamap.isInitial = true;

    if (typeof oamap.initialCallback == 'function') {

        oamap.initialCallback();

        window.setTimeout(oamap.initialCallback, 3000);
    }
};

//检测GIS经纬度值是否正确，纬度 0-90，经度 0-180
oamap.checkLatLng = function (lat, lng) {
    var latVal = Math.abs(typeof lat != 'number' ? parseFloat('0' + lat, 10) : lat);
    var lngVal = Math.abs(typeof lng != 'number' ? parseFloat('0' + lng, 10) : lng);

    return latVal > 0 && latVal <= 90 && lngVal > 0 && lngVal <= 180;
};

//GIS Flash返回的 地图中心坐标
oamap.returnCenter = function(lat, lng){
    oamap.mapCenter = {lat: lat, lng: lng};
};

//地图缩放 事件
oamap.returnZoom = function (zoom, scale) {
    oamap.zoom = zoom;
    oamap.scale = scale;
    if (oamap.eventTimer != null) {
        window.clearTimeout(oamap.eventTimer);
    }
    oamap.eventTimer = window.setTimeout(oamap.mapEventCallback, 1000);
};

//地图移动、地图单击 事件
oamap.returnEvent = function (x1, y1, x2, y2) {
    var arr = [x1, y1, x2, y2];
    //判断地图是否移动了
    if (oamap.checkMapIsMove(oamap.bround, arr)){
        oamap.bround = [x1, y1, x2, y2];
        if (oamap.eventTimer != null) {
            window.clearTimeout(oamap.eventTimer);
        }
        oamap.eventTimer = window.setTimeout(oamap.mapEventCallback, 1000);
    }
};

oamap.mapEventCallback = function () {
    if (oamap.eventCallback != undefined) {
        oamap.eventCallback(oamap.bround, oamap.zoom, oamap.scale);
    }
};

oamap.checkMapIsMove = function (bround, arr) {
    if (bround.length < 4) {
        return true;
    }
    for (var i = 0, c = bround.length; i < c; i++) {
        if (bround[i] != arr[i]) {
            return true;
        }
    }
    return false;
};

oamap.getCenter = function(){
    return oamap.mapCenter;
};

oamap.setCenter = function(lat, lng){
    return $('#' + oamap.frameId)[0].contentWindow.setCenter(lat, lng);
};

oamap.getZoom = function(){
    return oamap.zoom;
};

oamap.setZoom = function(zoom){
    return $('#' + oamap.frameId)[0].contentWindow.setZoom(zoom);
};

oamap.setCenterAndZoom = function (lat, lng, zoom) {
    return $('#' + oamap.frameId)[0].contentWindow.setCenterZoom(lat, lng, zoom);
};

//显示图层控件
oamap.showLayerControl = function(){
    oamap.callMapFunction('layer', null);
}; 

//显示测量控件
oamap.showMeasureControl = function(){
    oamap.callMapFunction('measure', null);
}; 

//显示测量控件
oamap.showDrawControl = function(){
    oamap.callMapFunction('draw', null);
}; 

//打印地图
oamap.printMap = function(){
    oamap.callMapFunction('print', null);
}; 

oamap.callMapFunction = function(action, jsondata){
    return $('#' + oamap.frameId)[0].contentWindow.callMapFunction(action, jsondata);
};

oamap.checkLatLng = function(lat, lng){
    lat = lat == undefined ? '' : '' + lat;
    lng = lng == undefined ? '' : '' + lng;
    return lat != '' && lat != '0' && lng != '' && lng != '0';
};

oamap.buildMarkerData = function(dataList, type, showLabel){
    var strData = '[';
    var picUrl = cms.util.path + '/skin/default/imgs/railway/gis/';
    var num = 0;
    for(var i=0,c=dataList.length; i<c; i++){
        var dr = dataList[i];
        if(oamap.checkLatLng(dr.lat, dr.lng)){
            var typeId = dr.type == undefined ? type : dr.type;
            var disLabel = (dr.showLabel != undefined ? dr.showLabel : showLabel) ? 1 : 0;
            var disInfo = (dr.showInfo != undefined ? dr.showInfo : false) ? 1 : 0;
            //var strIcon = (dr.icon != undefined ? dr.icon : picUrl + typeId + (typeId == 2 ? '.swf' : '.gif'));
            //var strIcon = (dr.icon != undefined ? dr.icon : picUrl + typeId + (typeId == 3 ? '.swf' : '.gif'));
            var strIcon = (dr.icon != undefined ? dr.icon : picUrl + typeId + '.gif');
            var strLabel = '';
            if(disLabel == 1){
                strLabel = dr.label != undefined ? dr.label : dr.name;
            }
            var strAction = dr.action != undefined ? dr.action : (dr.code != undefined ? dr.code : '');
            
            strData += num > 0 ? ',' : '';
            strData += '{';
            strData += '"type":"' + typeId + '"'
                + ',"id":"' + dr.id + '"'
                + ',"name":"' + dr.name + '"'
                + ',"GPS_X":"' + dr.lng + '"'
                + ',"GPS_Y":"' + dr.lat + '"'
                + ',"action":"' + strAction + '"'
                + ',"picUrl":"' + strIcon + '"'
                + ',"label":"' + strLabel + '"'
                + ',"disLabel":"' + disLabel + '"'
                + ',"disInfo":"' + disInfo + '"'
                + '}';
            num++;
        }
    }
    strData += ']';
    
    return strData;
};

oamap.createMarker = function(param, markerDataList){
    var jsondata = '{"id": "' + param.id + '", "scale": "' + param.scale + '", "pointsArray": ' + markerDataList + '}';
    return oamap.callMapFunction('createMarker', jsondata);
};

oamap.showMarker = function(id){
    var jsondata = '{"id": "' + id + '", "controlNum": "0"}';
    return oamap.callMapFunction('showMarker', jsondata);
};

oamap.hideMarker = function(id){
    var jsondata = '{"id": "' + id + '", "controlNum": "1"}';
    return oamap.callMapFunction('showMarker', jsondata);
};

oamap.removeMarker = function(){

};

oamap.buildLinePoint = function(dataList, showPoint, showInfo){
    var strData = '[';
    var picUrl = cms.util.path + '/skin/default/images/railway/gis/';
    var num = 0;
    for(var i=0,c=dataList.length; i<c; i++){
        var dr = dataList[i];
        if(oamap.checkLatLng(dr.lat, dr.lng)){
            var disPoint = (showPoint != undefined ? showPoint : false) ? 1 : 0;
            var disInfo = (showInfo != undefined ? showInfo : false) ? 1 : 0;
            var strIcon = picUrl + 'icon.gif';
            
            var strAction = dr.action != undefined ? dr.action : (dr.code != undefined ? dr.code : '');
            
            strData += i > 0 ? ',' : '';
            strData += '{';
            strData += '"id":"' + dr.id + '"'
                + ',"name":"' + dr.name + '"'
                + ',"GPS_X":"' + dr.lng + '"'
                + ',"GPS_Y":"' + dr.lat + '"'
                + ',"action":"' + strAction + '"'
                + ',"picUrl":"' + strIcon + '"'
                + ',"disPoint":"' + disPoint + '"'
                + ',"disInfo":"' + disInfo + '"'
                + '}';
            num++;
        }
    }
    strData += ']';
    
    return strData;
};

oamap.createLine = function(param, linePointList){
    var jsondata = '{'
        + '"id": "' + param.id + '"'
        + ',"name":"' + param.name + '"'
        + ',"type": "' + param.type + '"'
        + ',"scale": "' + param.scale + '"'
        + ',"color":"' + param.color.replace('#','0x') + '"'
        + ',"width":"' + param.width + '"'
        + ',"alpha":"' + param.alpha + '"'
        + ',"action": "' + param.action + '"'
        + ',"pointsArray": ' + linePointList
        + '}';
        
    return oamap.callMapFunction('createLine', jsondata);
};

oamap.showLine = function(layerId){
    var jsondata = '{"id": "' + layerId + '", "controlNum": "0"}';
    return oamap.callMapFunction('showLine', jsondata);
};

oamap.hideLine = function(layerId){
    var jsondata = '{"id": "' + layerId + '", "controlNum": "1"}';
    return oamap.callMapFunction('showLine', jsondata);
};
