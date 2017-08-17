/*编辑器相关*/
page.$obj = null;
page.editorHeight = 0;
page.editorWidth = 0;
page.editorMinHeight = 100;
page.editorMinWidth = 580;
page.editorConfig = null;
page.onlyChangeHeight = true;

page.initialEditor = function ($obj, config) {
    $obj.xheditor(false);
    page.editorConfig = config;
    page.$obj = $obj;
    return $obj.xheditor(config);
};

page.initialEditorResizeControl = function (param) {
    if (param == undefined) {
        return false;
    }
    var id_H = param.objId + 'H';
    var id_WH = param.objId + 'WH';
    page.onlyChangeHeight = param.isOnlyChangeHeight == undefined || param.isOnlyChangeHeight;

    var conW = param.w - (page.onlyChangeHeight ? 0 : 12) - 2;
    var strHtml = '<div id="' + id_H + '" class="editor-resize-h" style="width:' + conW + 'px;" ondblclick="page.setEditorHeight(null,100, true);"></div>';
    if (!page.onlyChangeHeight) {
        strHtml += '<div id="' + id_WH + '" class="editor-resize-wh"></div>';
    }
    $('#' + param.objId).html(strHtml);

    page.editorWidth = param.w;
    page.editorHeight = param.h;
    if (param.minH != undefined && typeof param.minH == 'number') {
        page.editorMinHeight = param.minH;
    }
    if (param.minW != undefined && typeof param.minW == 'number') {
        page.editorMinWidth = param.minW;
    }

    var objH = cms.util.$(id_H);
    objH.onmousedown = function (e) {
        var scrollTop = document.body.scrollTop || document.documentElement.scrollTop;
        var disY = (e || event).clientY;
        document.onmousemove = function (e) {
            var y = ((e || event).clientY - disY - scrollTop);
            page.setEditorHeight(page.$obj, y, true);
        };
        document.onmouseup = function () {
            document.onmousemove = null;
            document.onmouseup = null;
            objH.releaseCapture && objH.releaseCapture();

            page.editorHeight = page.$obj.height();
            page.initialEditor(page.$obj, page.editorConfig);
        };
        objH.setCapture && objH.setCapture();
        return false
    };

    if (!page.onlyChangeHeight) {
        var objWH = cms.util.$(id_WH);
        objWH.onmousedown = function (e) {
            var scrollTop = document.body.scrollTop || document.documentElement.scrollTop;
            var scrollLeft = document.body.scrollLeft || document.documentElement.scrollLeft;
            var evt = e || event;
            var disX = evt.clientX;
            var disY = evt.clientY;
            document.onmousemove = function (e) {
                evt = e || event;
                var x = (evt.clientX - disX - scrollLeft);
                var y = (evt.clientY - disY - scrollTop);
                page.setEditorSize(page.$obj, x, y, true, id_H);
            };
            document.onmouseup = function () {
                document.onmousemove = null;
                document.onmouseup = null;
                objWH.releaseCapture && objWH.releaseCapture();

                page.editorWidth = page.$obj.width();
                page.editorHeight = page.$obj.height();
                page.initialEditor(page.$obj, page.editorConfig);
            };
            objWH.setCapture && objWH.setCapture();
            return false
        };
    }
};

page.setEditorHeight = function ($obj, height, isAppend) {
    if ($obj == null) {
        $obj = page.$obj;
    }
    var h = page.editorHeight;
    if (isAppend) {
        h += height;
    } else {
        h = Math.abs(height);
    }
    if (h < page.editorMinHeight) {
        h = page.editorMinHeight;
    }
    $obj.height(h);

    page.initialEditor($obj, page.editorConfig);
};

page.setEditorSize = function ($obj, width, height, isAppend, controlId) {
    var hasWidth = width != null && typeof width == 'number';
    var hasHeight = height != null && typeof height == 'number';
    var w = page.editorWidth;
    var h = page.editorHeight;
    if (!hasWidth && !hasHeight) {
        return false;
    }

    if (hasWidth) {
        if (isAppend) {
            w += width;
        } else {
            w = Math.abs(width);
        }
        if (w < page.editorMinWidth) {
            w = page.editorMinWidth;
        }
        $obj.width(w);
    }

    if (hasHeight) {
        if (isAppend) {
            h += height;
        } else {
            h = Math.abs(height);
        }
        if (h < page.editorMinHeight) {
            h = page.editorMinHeight;
        }
        $obj.height(h);
    }

    page.initialEditor($obj, page.editorConfig);

    if (controlId != undefined) {
        $('#' + controlId).width($obj.width() - (page.onlyChangeHeight ? 0 : 12) - 2);
    }
};