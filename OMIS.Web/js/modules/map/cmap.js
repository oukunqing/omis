var cmap = cmap || {};

cmap.map = null;
cmap.mapBg = null;
cmap.zoom = 1;
cmap.zindex = 10;
//中心缩放
cmap.centerScale = true;
cmap.mapTypeControl = true;
cmap.scaleControl = true;
cmap.zoomControl = true;
cmap.markers = [];
cmap.lines = [];
cmap.icon = '';
cmap.iconSize = [20, 20, 16, 16, 12, 12];
cmap.size = typeof mapImgSize != 'undefined' ? mapImgSize : { width: 4028, height: 6296 };
cmap.boxSize = null;
var strImgPathDir = cmsPath + '/skin/default/imgs/railway';

cmap.ControlPosition = {
    LEFT_TOP: 'LEFT_TOP',
    RIGHT_TOP: 'RIGHT_TOP',
    LEFT_BOTTOM: 'LEFT_BOTTOM',
    RIGHT_BOTTOM: 'RIGHT_BOTTOM'
};

cmap.initialEmap = function (obj, options) {
    var strHtml = '';
    if (options != undefined && typeof options == 'object') {
        cmap.boxSize = options.boxSize;
        if (options.zindex != undefined) {
            cmap.zindex = options.zindex;
        }
        if (options.zoomControl != undefined) {
            cmap.zoomControl = options.zoomControl;
        }
        if (options.mapTypeControl != undefined) {
            cmap.mapTypeControl = options.mapTypeControl;
        }
        if (options.scaleControl != undefined) {
            cmap.scaleControl = options.scaleControl;
        }
        if (options.zoomControlOptions == undefined) {
            options.zoomControlOptions = {};
        }

        if (cmap.zoomControl) {
            if (options.zoomControlOptions == undefined) {
                options.zoomControlOptions = {};
            }
            options.zoomControlOptions.position = options.zoomControlOptions.position || cmap.ControlPosition.LEFT_TOP;
            options.zoomControlOptions.y = options.zoomControlOptions.y || 10;
            options.zoomControlOptions.x = options.zoomControlOptions.x || 10;
        }

        if (cmap.mapTypeControl) {
            if (options.mapTypeControlOptions == undefined) {
                options.mapTypeControlOptions = {};
            }
            options.mapTypeControlOptions.position = options.mapTypeControlOptions.position || cmap.ControlPosition.LEFT_TOP;
            options.mapTypeControlOptions.y = options.mapTypeControlOptions.y || 0;
            options.mapTypeControlOptions.x = options.mapTypeControlOptions.x || 0;
        }

        if (cmap.scaleControl) {
            if (options.scaleControlOptions == undefined) {
                options.scaleControlOptions = {};
            }
            options.scaleControlOptions.position = options.scaleControlOptions.position || cmap.ControlPosition.LEFT_BOTTOM;
            options.scaleControlOptions.y = options.scaleControlOptions.y || 0;
            options.scaleControlOptions.x = options.scaleControlOptions.x || 0;
        }
    } else {
        alert('地图初始化失败，原因：参数错误');
        return false;
    }

    if (obj == undefined) {
        obj = cms.util.$('cmapCanvas');
    }
    if (options.mapBgFilePath == undefined || options.mapBgFilePath == null) {
        options.mapBgFilePath = strImgPathDir + '/map/' + 'map.jpg';
    }
    options.mapBgFilePath = options.mapBgFilePath.replaceAll('//', '/');
    strHtml += cmap.zoomControl ? '<div id="cmapControl" style="position:absolute;' + cmap.setControlPosition(options.zoomControlOptions) + 'z-index:' + (cmap.zindex + 5) + ';">' + cmap.buildZoomControl() + '</div>' : '';
    strHtml += cmap.scaleControl ? '<div id="cmapScaleBox" style="position:absolute;' + cmap.setControlPosition(options.scaleControlOptions) + 'height:20px;line-height:20px;padding:0 3px;background:#fff;z-index:' + (cmap.zindex + 5) + ';"></div>' : '';
    strHtml += cmap.mapTypeControl ? '<div id="cmapBgChange" style="position:absolute;' + cmap.setControlPosition(options.mapTypeControlOptions) + 'height:20px;line-height:20px;background:#fff;z-index:' + (cmap.zindex + 5) + ';border:solid 1px #ccc;"></div>' : '';
    strHtml += ''
        + '<div id="hiddenPic" style="position:absolute; left:0px; top:0; width:0px; height:0px; z-index:1; visibility: hidden;">'
        + '<img id="mapBg2" name="mapBg2" src="' + options.mapBgFilePath + '" border="0" width="' + options.boxSize.width + 'px" />'
        + '</div>'
        + '<div id="mapBox" onmouseout="drag=0" onmouseover="dragObj=mapBox; drag=1;" class="dragAble" '
        + ' style="z-index:' + cmap.zindex + '; height:0; left:0px; position:absolute; top:0; width:0" >'
        + '<img id="mapBg1" name="mapBg1" src="' + options.mapBgFilePath + '" border="0" width="' + options.boxSize.width + 'px" ondblclick="bigit();" '
        + ' style="position:absolute;left:0; top:0;" />'
        + '</div>';
    obj.innerHTML = strHtml;

    cmap.map = cms.util.$('mapBox');
    cmap.mapBg = cms.util.$('mapBg1');
    cmap.getZoom();
    cmap.showBgChange();

    cmap.map.onmousewheel = obj.onmousewheel = wheel;
};

cmap.setControlPosition = function (option) {
    var strPosition = '';
    switch (option.position) {
        case 'LEFT_TOP':
            strPosition = 'left:' + option.x + 'px;top:' + option.y + 'px;';
            break;
        case 'RIGHT_TOP':
            strPosition = 'right:' + option.x + 'px;top:' + option.y + 'px;';
            break;
        case 'LEFT_BOTTOM':
            strPosition = 'left:' + option.x + 'px;bottom:' + option.y + 'px;';
            break;
        case 'RIGHT_BOTTOM':
            strPosition = 'right:' + option.x + 'px;bottom:' + option.y + 'px;';
            break;
    }
    return strPosition;
};

cmap.getZoom = function () {
    cmap.zoom = parseInt(cmap.mapBg.width, 10) / parseInt(cmap.size.width, 10);
    cmap.showScaleRate();
    return cmap.zoom;
};

cmap.setZoom = function (zoom) {
    if (typeof zoom == 'number') {
        cmap.mapBg.style.width = (parseInt(cmap.size.width, 10) * parseFloat(zoom, 10)) + 'px';
        cmap.showScaleRate();
        cmap.moveMarker();
        if (cmap.line != undefined) {
            cmap.line.moveLine(cmap.map);
        }
        return cmap.getZoom();
    }
};

cmap.showScaleRate = function () {
    if (cmap.scaleControl) {
        cms.util.$('cmapScaleBox').innerHTML = '比例:' + (cmap.zoom <= 1 ? '1<span style="padding:0 2px;">:</span>' + Math.round(1 / cmap.zoom * 100) / 100 : Math.round(cmap.zoom * 100) / 100 + '<span style="padding:0 2px;">:</span>' + 1);
    }
};

cmap.showBgChange = function () {
    if (cmap.mapTypeControl) {
        cms.util.$('cmapBgChange').innerHTML = ''
        + '<span style="display:block;float:left;padding:0 3px;cursor:pointer;background:#dbebfe;" onclick="cmap.chageMapBg(\'\',this);">彩色</span>'
        + '<span style="float:left;border-left:solid 1px #ccc;display:block;width:0;">&nbsp;</span>'
        + '<span style="float:left;display:block;padding:0 3px;cursor:pointer;" onclick="cmap.chageMapBg(\'gray\',this);">灰色</span>'
        + '<span style="float:left;border-left:solid 1px #ccc;display:block;width:0;">&nbsp;</span>'
        + '<span style="float:left;display:block;padding:0 3px;cursor:pointer;" onclick="cmap.chageMapBg(\'clean\',this);">线路图</span>';
    }
};

cmap.chageMapBg = function (action, btn, strMapBgFilePath) {
    if (strMapBgFilePath != undefined) {
        cmap.mapBg.src = strMapBgFilePath.replaceAll('//', '/');
    } else {
        var strFilePath = strImgPathDir + '/map/' + (action == 'gray' ? 'map_gray.jpg' : action == 'clean' ? 'map_clean.jpg' : 'map.jpg');

        cmap.mapBg.src = strFilePath.replaceAll('//', '/');

        var childs = cms.util.$('cmapBgChange').childNodes;
        for (var i = 0; i < childs.length; i++) {
            childs[i].style.background = '#fff';
        }
        if (btn != null && btn != undefined) {
            btn.style.background = '#dbebfe';
        } else {
            childs[action == 'gray' ? 2 : action == 'clean' ? 4 : 0].style.background = '#dbebfe';
        }
    }
};

cmap.setMapBg = function (strFilePath) {
    if (strFilePath != '') {
        cmap.chageMapBg(null, null, strFilePath);
    }
};

//页面画布缩放时，改变地图背景尺寸，后续
cmap.changeSize = function (boxSize) {
    if (cmap.mapBg != null) {
        //功能后续完成


        cmap.getZoom();
    }
};

cmap.showMarker = function (param) {
    var option = {
        title: param.name,
        icon: param.icon,
        pos: { left: param.left || 0, top: param.top || 0 },
        showLabel: param.showLabel || false,
        labelBg: param.labelBg || '#fff',
        labelBorder: param.labelBorder || 'solid 1px #ddd',
        labelColor: param.labelColor || '#000',
        func: param.func || null,
        val: param.val || {}
    };
    var marker = cmap.buildMarker(option);
    cmap.markers.push(marker);
    return marker;
};

cmap.clearMarker = function () {
    for (var i = 0, c = cmap.markers.length; i < c; i++) {
        cmap.removeMarker(cmap.markers[i]);
        cmap.markers.splice(i, 1);
    }
};

cmap.removeMarker = function (marker) {
    if (marker != null && marker != undefined && typeof marker == 'object') {
        try {
            cmap.map.removeChild(marker);
        }
        catch (e) { }
    }
};

cmap.moveMarker = function (marker) {
    var size = cmap.zoom > 0.5 ? [cmap.iconSize[0], cmap.iconSize[1]] : cmap.zoom < 0.3 ? [cmap.iconSize[4], cmap.iconSize[5]] : [cmap.iconSize[2], cmap.iconSize[3]];
    var w = parseInt(size[0], 10);
    var h = parseInt(size[1], 10);
    if (marker != null && marker != undefined) {

    } else {
        for (var i = 0, c = cmap.markers.length; i < c; i++) {
            var marker = cmap.markers[i];
            var pos = eval('(' + marker.lang + ')');
            var left = parseInt(pos[0], 10);
            var top = parseInt(pos[1], 10);

            left = left * cmap.zoom - w / 2;
            top = top * cmap.zoom - h / 2;

            marker.style.left = left + 'px';
            marker.style.top = top + 'px';

            if (marker.childNodes.length >= 1) {
                marker.childNodes[0].style.width = w + 'px';
                marker.childNodes[0].style.height = h + 'px';

                if (marker.childNodes.length >= 2) {
                    marker.childNodes[1].style.top = h + 'px';
                    marker.childNodes[1].style.left = size[0] / 2 + 'px';
                }
            }
        }
    }
};

cmap.buildMarker = function (option) {
    var marker = document.createElement('DIV');
    var size = cmap.zoom > 0.5 ? [cmap.iconSize[0], cmap.iconSize[1]] : cmap.zoom < 0.3 ? [cmap.iconSize[4], cmap.iconSize[5]] : [cmap.iconSize[2], cmap.iconSize[3]];
    var left = option.pos.left * cmap.zoom - size[0] / 2;
    var top = option.pos.top * cmap.zoom - size[1] / 2;

    marker.style.position = 'absolute';
    marker.style.left = left + 'px';
    marker.style.top = top + 'px';
    marker.title = option.title;
    marker.lang = '[' + option.pos.left + ',' + option.pos.top + ']';

    var img = document.createElement('IMG');
    img.src = option.icon.replaceAll('//', '/');
    img.title = option.title;
    img.style.width = size[0] + 'px';
    img.style.height = size[1] + 'px';
    img.style.cursor = 'pointer';
    img.style.zIndex = 10001;
    img.lang = option.val;

    if (typeof option.func == 'function') {
        img.onclick = function () {
            option.func(this.lang);
        }
    }

    marker.appendChild(img);

    if (option.showLabel) {
        var label = document.createElement('SPAN');
        label.style.cssText = 'position:absolute; display:block;height:12px;line-height:12px;'
            + 'width:' + (option.title.len() * 6) + 'px;top:' + size[1] + 'px;left:' + size[0] / 2 + 'px;minWidth:100px;'
            + 'padding:2px;cursor:pointer;z-index:10000;';
        label.style.background = option.labelBg;
        label.style.border = option.labelBorder;
        label.style.color = option.labelColor;

        label.innerHTML = option.title;
        label.lang = option.val;
        if (typeof option.func == 'function') {
            label.onclick = function () {
                option.func(this.lang);
            }
        }
        marker.appendChild(label);
    }

    cmap.map.appendChild(marker);

    return marker;
};

cmap.buildZoomControl = function (obj) {
    var strHtml = '<style type="text/css">'
        + '#tbMapControl td{width:20px;height:20px; padding:0 1px;}'
        + '#tbMapControl .zoom{width:20px;height:20px;margin:0;padding:0;border:none;cursor:pointer;display:block;background:url("' + strImgPathDir + '/map/map_zoom.gif") no-repeat;}'
        + '#tbMapControl .zoom-up{background-position:-20px 0;}'
        + '#tbMapControl .zoom-right{background-position:-40px -20px;}'
        + '#tbMapControl .zoom-down{background-position:-20px -40px;}'
        + '#tbMapControl .zoom-left{background-position:0 -20px;}'
        + '#tbMapControl .zoom-zoom{background-position:-20px -20px;}'
        + '#tbMapControl .zoom-in{background-position:-20px -60px;}'
        + '#tbMapControl .zoom-out{background-position:-20px -80px;}'
        + '#tbMapControl .zoom-original{background-position:-20px -100px;}'
        + '#tbMapControl img{width:20px;height:20px;cursor:pointer;display:block;}'
        + '</style>';
    strHtml += '<table cellpadding="0" cellspacing="0" id="tbMapControl">'
        + '<tr>'
        + '<td>&nbsp;</td>'
        + '<td><div class="zoom zoom-up" onclick="clickMove(\'up\')" title="向上"></div></td>'
        + '<td>&nbsp;</td>'
        + '</tr>'
        + '<tr>'
        + '<td><div class="zoom zoom-left" onclick="clickMove(\'left\')" title="向左"></div></td>'
        + '<td><div class="zoom zoom-zoom" onclick="realsize();" title="还原"></div></td>'
        + '<td><div class="zoom zoom-right" onclick="clickMove(\'right\')" title="向右"></div></td>'
        + '</tr>'
        + '<tr>'
        + '<td>&nbsp;</td>'
        + '<td><div class="zoom zoom-down" onclick="clickMove(\'down\')" title="向下"></div></td>'
        + '<td>&nbsp;</td>'
        + '</tr>'
        + '<tr>'
        + '<td>&nbsp;</td>'
        + '<td><div class="zoom zoom-in" onclick="bigit();" title="放大"></div></td>'
        + '<td>&nbsp;</td>'
        + '</tr>'
        + '<tr>'
        + '<td>&nbsp;</td>'
        + '<td><div class="zoom zoom-out" onclick="smallit();" title="缩小"></div></td>'
        + '<td>&nbsp;</td>'
        + '</tr>'
        + '<tr>'
        + '<td>&nbsp;</td>'
        + '<td><div class="zoom zoom-original" onclick="showOriginalSize();" title="1:1原始大小"></div></td>'
        + '<td>&nbsp;</td>'
        + '</tr>'
        + '</table>';
    if (obj != undefined && typeof obj == 'object') {
        obj.innerHTML = strHtml.replaceAll('//', '/');
    }
    return strHtml.replaceAll('//', '/');
};


var drag = 0;
var move = 0;
// 拖拽对象
var ie = document.all;
var nn6 = document.getElementById && !document.all;
var isdrag = false;
var y, x;
var oDragObj;

function moveMouse(e) {
    if (isdrag) {
        oDragObj.style.top = (nn6 ? nTY + e.clientY - y : nTY + event.clientY - y) + "px";
        oDragObj.style.left = (nn6 ? nTX + e.clientX - x : nTX + event.clientX - x) + "px";
        return false;
    }

    //alert(oDragObj.style.top + ',' + oDragObj.style.left);
}

function initDrag(e) {
    try {
        var oDragHandle = nn6 ? e.target : event.srcElement;
        var topElement = "HTML";
        while (oDragHandle.tagName != topElement && oDragHandle.className != "dragAble") {
            oDragHandle = nn6 ? oDragHandle.parentNode : oDragHandle.parentElement;
        }
        if (oDragHandle.className == "dragAble") {
            isdrag = true;
            oDragObj = oDragHandle;
            nTY = parseInt(oDragObj.style.top + 0);
            y = nn6 ? e.clientY : event.clientY;
            nTX = parseInt(oDragObj.style.left + 0);
            x = nn6 ? e.clientX : event.clientX;
            document.onmousemove = moveMouse;
            return false;
        }
    } catch (e) { }
}

document.onmousedown = initDrag;
document.onmouseup = new Function("isdrag=false");

function clickMove(s) {
    if (s == "up") {
        dragObj.style.top = parseInt(dragObj.style.top) + 100;
    } else if (s == "down") {
        dragObj.style.top = parseInt(dragObj.style.top) - 100;
    } else if (s == "left") {
        dragObj.style.left = parseInt(dragObj.style.left) + 100;
    } else if (s == "right") {
        dragObj.style.left = parseInt(dragObj.style.left) - 100;
    }
}

function smallit() {
    var mapBg1 = cms.util.$('mapBg1');
    var height1 = mapBg1.height;
    var width1 = mapBg1.width;
    mapBg1.height = height1 / 1.1;
    mapBg1.width = width1 / 1.1;

    mapBgMove(0.1, 'small');

    cmap.getZoom();
    cmap.moveMarker();
    if (cmap.line != undefined) {
        cmap.line.moveLine(cmap.map);
    }
}

function bigit() {
    var mapBg1 = cms.util.$('mapBg1');
    var height1 = mapBg1.height;
    var width1 = mapBg1.width;
    mapBg1.height = height1 * 1.1;
    mapBg1.width = width1 * 1.1;

    mapBgMove(0.1, 'big');

    cmap.getZoom();
    cmap.moveMarker();
    if (cmap.line != undefined) {
        cmap.line.moveLine(cmap.map);
    }
}

function showOriginalSize() {
    var mapBg1 = cms.util.$('mapBg1');
    mapBg1.height = cmap.size.height;
    mapBg1.width = cmap.size.width;

    //mapBgMove(0.1, 'big');

    cmap.map.style.left = 0;
    cmap.map.style.top = 0;

    cmap.getZoom();
    cmap.moveMarker();
    if (cmap.line != undefined) {
        cmap.line.moveLine(cmap.map);
    }
}

function mapBgMove(rate, action) {
    if (cmap.centerScale) {
        var w = parseInt(cmap.mapBg.width, 10);
        var h = parseInt(cmap.mapBg.height, 10);
        var left = parseInt(cmap.map.style.left, 10);
        var top = parseInt(cmap.map.style.top, 10);
        if ('big' == action) {
            cmap.map.style.left = (left - w / 2 * cmap.zoom * rate) + 'px';
            cmap.map.style.top = (top - h / 2 * cmap.zoom * rate) + 'px';
        } else {
            cmap.map.style.left = (left + w / 2 * cmap.zoom * rate) + 'px';
            cmap.map.style.top = (top + h / 2 * cmap.zoom * rate) + 'px';
        }
    }
}

function realsize() {
    var mapBg1 = cms.util.$('mapBg1');
    var mapBg2 = cms.util.$('mapBg2');
    mapBg1.height = mapBg2.height;
    mapBg1.width = mapBg2.width;
    mapBox.style.left = 0;
    mapBox.style.top = 0;

    cmap.getZoom();
    cmap.moveMarker();
}

function featsize() {
    var mapBg1 = cms.util.$('mapBg1');
    var mapBg2 = cms.util.$('mapBg2');
    var width1 = mapBg2.width;
    var height1 = mapBg2.height;
    var width2 = 360;
    var height2 = 200;
    var h = height1 / height2;
    var w = width1 / width2;
    if (height1 < height2 && width1 < width2) {
        mapBg1.height = height1;
        mapBg1.width = width1;
    } else {
        if (h > w) {
            mapBg1.height = height2;
            mapBg1.width = width1 * height2 / height1;
        } else {
            mapBg1.width = width2;
            mapBg1.height = height1 * width2 / width1;
        }
    }
    mapBox.style.left = 0;
    mapBox.style.top = 0;
}

/*鼠标滚动事件 */
var wheel = function (event) {
    var delta = 0;
    if (!event) /* For IE. */
        event = window.event;
    if (event.wheelDelta) { /* IE/Opera. */
        delta = event.wheelDelta / 110;
    } else if (event.detail) {
        delta = -event.detail / 3;
    }
    if (delta)
        handle(delta);
    if (event.preventDefault)
        event.preventDefault();
    event.returnValue = false;
}

if (window.addEventListener) {
    /** DOMMouseScroll is for mozilla. */
    window.addEventListener('DOMMouseScroll', wheel, false);
}

var handle = function (delta) {
    var random_num = Math.floor((Math.random() * 100) + 50);
    if (delta < 0) {
        //alert("鼠标滑轮向下滚动：" + delta + "次！"); // 1  
        smallit();
        //$("btn_next_pic").onclick(random_num);
        return;
    } else {
        //alert("鼠标滑轮向上滚动：" + delta + "次！"); // -1  
        bigit();
        //$("btn_last_pic").onclick(random_num);
        return;
    }
}