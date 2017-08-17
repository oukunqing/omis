var page = page || {
    pageTopHeight: 25, pageBottomHeight: 28,
    bodySize: {}, boxSize: {}, formSize: {},
    size: {
        pt: 0,  //paddingTop
        bw: 2,  //borderWidth
        lw: 0, lwc: 0,  //leftWidth,leftWidthConfig
        lsw: 0, //leftSwitchWidth
        rw: 0, rwc: 0,  //rightWidth,rightWidthConfig
        rsw: 0, //rightSwitchWidth
        th: 25, bh: 28  //topHeight,bottomHeight
    },
    dataCount: 0, pageIndex: 1, pageStart: 1, pageSize: 20,
    lang: {
        "add": "新增", "edit": "编辑", "update": "更新", "delete": "删除"
    }
};

page.showPrompt = function (str, obj, type, autoHide, timing) {
    obj = obj || $('#lblPrompt');
    obj.html(str);
    typeof str == 'string' && str != '' ? function () { obj.show(); obj.addClass('lbl-' + type); }() : function () { obj.hide(); obj.removeClass(); }();
    if (autoHide) {
        if (typeof timing != 'number') {
            timing = parseInt('0' + timing, 10);
        }
        timing = timing <= 0 ? 5000 : timing;

        obj.dblclick(function () {
            page.hidePrompt($(this));
        });
        window.setTimeout('page.hidePrompt($(\'#' + obj.prop('id') + '\'))', timing);
    }
};

page.hidePrompt = function (obj) {
    obj.hide();
};

page.loadPage = function (url) {
    location.href = url;
    cms.util.setWindowStatus();
};

page.showPromptWin = function (html, url, isClose, callback, val) {
    html = html || ''; url = url || ''; val = val || '';
    var cfg = { title: '提示信息', html: html, iconType: /(成功|完毕|完成)/g.test(html) ? 'success' : 'warning' };
    if ('' == html && typeof callback == 'function') {
        callback(val);
    }
    if (isClose && typeof callback == 'function') {
        cfg.callback = callback;
        cfg.returnValue = val;
    } else if (url != '') {
        cfg.callback = function (pwo, pwr) {
            window.location.href = pwr.returnValue.url;
        };
        cfg.returnValue = { url: url, param: val };
    }
    if (!cms.util.isMSIE) {
        cms.box.alert(cfg);
    } else {
        //IE8中会出现这个错误：
        //HTML Parsing Error: Unable to modify the parent container element before the child element is closed (KB927917)
        //所以延时50ms
        window.setTimeout(cms.box.alert, 50, cfg);
    }
};

$(window).ready(function () {
    page.setBodySize();

    //清空密码输入框，防止浏览器记住密码
    $("input[type=password]").attr('value', '');
    //禁用input[css=txt-select]输入
    page.disabledInput($('.txt-select'));
    //完全禁用input[css=txt-readonly]
    page.disabledInput($('.txt-readonly'), true);
    //初始化调试
    page._initialDebug();
    //初始化表单
    page._initialForm();
    //设置父窗口参数
    page.setParentNeedSave();

    //列表页表单提交
    var frmQuery = $I('frmQuery') || $I('frmConditoin');
    if (frmQuery != null) { frmQuery.onsubmit = function () { page.loadDataList(true); return false; }; }

    //编辑页表单提交
    var frmEdit = $I('frmEdit');
    if (frmEdit != null) { frmEdit.onsubmit = function () { page.formSubmit(); return false; }; }

    //捕捉异常，使跨域调用时JS代码正常运行
    try {
        //鼠标点击时，关闭父级页面的右键菜单
        if (page.isChild() && typeof parent.hideContextMenu == 'function') {
            document.body.onmousedown = function () { parent.hideContextMenu(); };
        }
    } catch (e) { }

    //$('#reload').html('<a class="btn imgbtn" onclick="location.reload();"><i class="icon-refresh"></i><span>刷新</span></a>');
});

$(window).resize(function () {
    page.setBodySize();
});

//禁用input输入,disable:true-表示完全禁用，等同于disabled=disabled,false-表示禁用内容输入但启用（9,13,38,40）
page.disabledInput = function (o, disable) {
    //屏蔽文本选择框的输入功能
    o.each(function () {
        //当input 的 alt标签值为 edit 时，允许该input可正常输入
        if ($(this).prop('alt') != 'edit') {
            $(this).keydown(function (ev) {
                var keys = disable ? '[9]' : '[9],[13],[38],[40]', e = ev || window.event;
                return keys.indexOf('[' + e.keyCode + ']') >= 0;
            });
        }
    });
};

page._initialDebug = function () {
    page._DebugKey = [];
    /*
    if (typeof webConfig == 'undefined') {
        webConfig = { isDebug: false };
    }*/

    document.body.onkeypress = function (ev) {
        var e = ev || window.event, cn = e.keyCode || e.which, s = String.fromCharCode(cn).toUpperCase(), t = new Date().getTime();
        var keys = '[B],[C],[D],[E],[G],[L],[O],[S],[U]';
        if (keys.indexOf('[' + s + ']') >= 0) {
            page._DebugKey.push([s, t]);
        }
        var debug = function (s, t) {
            var c = 10, k = page._DebugKey;
            if (k.length > c) { k.splice(0, 1); }
            if (c == k.length) {
                var ts = k[c - 1][1] - k[0][1], code = '';
                //在5秒钟内敲击2遍DEBUG启用调试,5秒钟内敲击2遍CLOSE关闭调试
                if (ts > 5000) {
                    k.length = 0, k.push([s, t]);
                } else {
                    for (var i = 0; i < c; i++) { code += k[i][0]; }
                    if ('DEBUGDEBUG' == code) {
                        k.length = 0, alert('Debug Open');
                        webConfig.isDebug = true;
                        alert('GetData');
                        page.getData();
                    } else if ('CLOSECLOSE' == code) {
                        k.length = 0, alert('Debug Close');
                        webConfig.isDebug = false;
                    }
                }
            }
        }(s, t);
    };
};

page.getBodySize = function () {
    if (typeof document.compatMode != 'undefined' && document.compatMode == 'CSS1Compat') {
        return { width: document.documentElement.clientWidth, height: document.documentElement.clientHeight };
    } else if (typeof document.body != 'undefined') {
        return { width: document.body.clientWidth, height: document.body.clientHeight };
    }
};

page.getSize = function () {
    var bs = page.getBodySize();
    return { width: bs.width, height: bs.height - page.size.pt };
};

page.setSize = function (p) {
    p = p || {};
    if (p) {
        var _ = page.size;
        //顶部边距
        _.pt = page.isNumber(p.paddingTop) ? p.paddingTop : _.pt;
        //左边栏宽度
        _.lw = page.isNumber(p.leftWidth) ? p.leftWidth : _.lw;
        //左边栏宽度配置
        _.lwc = page.isNumber(p.leftWidthConfig) ? p.leftWidthConfig : _.lwc;
        if (typeof p.leftWidth == 'number') {
            _.lsw = p.leftSwitchWidth || 5;
            _.lw -= _.lw > _.bw ? _.bw : 0;
            if (_.lwc == 0 && p.leftWidth > 0) {
                _.lwc = p.leftWidth;
            }
        }
        $('#bodyLeftSwitch').removeClass();
        $('#bodyLeftSwitch').addClass(_.lw <= 0 ? 'switch-right' : 'switch-left');

        //右边栏宽度
        _.rw = page.isNumber(p.rightWidth) ? p.rightWidth : _.rw;
        //右边栏宽度配置
        _.rwc = page.isNumber(p.rightWidthConfig) ? p.rightWidthConfig : _.rwc;
        if (typeof p.rightWidth == 'number') {
            _.rsw = p.rightSwitchWidth || 5;
            _.rw -= _.rw > _.bw ? _.bw : 0;
            if (_.rwc == 0 && p.rightWidth > 0) {
                _.rwc = p.rightWidth;
            }
        }
        $('#bodyRightSwitch').removeClass();
        $('#bodyRightSwitch').addClass(_.rw <= 0 ? 'switch-left' : 'switch-right');
    }
};

page._switch = function (objSwitch, show, type) {
    page.setSize(type == 'left' ? { leftWidth: show ? page.size.lwc : 0 } : { rightWidth: show ? page.size.rwc : 0 });
    page.setBodySize();
    objSwitch = type == 'left' ? page.checkJqObj(objSwitch, $('#bodyLeftSwitch')) : page.checkJqObj(objSwitch, $('#bodyRightSwitch'));
    if (objSwitch) {
        objSwitch.removeClass();
        objSwitch.addClass(type == 'left' ? (show ? 'switch-left' : 'switch-right') : (show ? 'switch-right' : 'switch-left'));
    }
};

page.switchLeft = function (objSwitch, show) {
    page._switch(objSwitch, page.isUndefined(show) ? $('#bodyLeft').is(':hidden') : show, 'left');
};

page.switchRight = function (objSwitch, show) {
    page._switch(objSwitch, page.isUndefined(show) ? $('#bodyRight').is(':hidden') : show, 'right');
};

page._formInitialled = false;
page._initialForm = function () {
    if (!page._formInitialled) {
        page._formInitialled = true;

        $('#bodyLeftSwitch').click(function () {
            cms.util.stopBubble();
            page.switchLeft($(this), $('#bodyLeft').is(':hidden'));
        });
        $('#bodyRightSwitch').click(function () {
            cms.util.stopBubble();
            page.switchRight($(this), $('#bodyRight').is(':hidden'));
        });
        page._setBodyByShortcutKey();

        if (typeof initialForm == 'function') {
            initialForm();
        }
    }
};
page.initialForm = page._initialForm;

page._setBodySize = function () {
    var _b = page.bodySize, _x = page.boxSize, _s = page.size;
    var _bh = _b.height - _s.pt;

    var _bp = page.checkJqObj($('#pageBody'), $('#bodyPage'));
    if (_bp && _s.pt > 0) {
        _bp.css('padding-top', _s.pt);
    }
    var _bl = page.checkJqObj($('#bodyLeft'));
    if (_bl) {
        _bl.css('display', _s.lw > 0 ? '' : 'none');
        if (_s.lw > 0) {
            _bl.width(_s.lw - _s.bw);
        }
        _bl.height(_bh - _s.bw);
    }
    var _bls = page.checkJqObj($('#bodyLeftSwitch'));
    if (_bls) {
        _bls.width(_s.lsw);
        _bls.height(_bh);
    }
    var _br = page.checkJqObj($('#bodyRight'));
    if (_br) {
        _br.css('display', _s.rw > 0 ? '' : 'none');
        if (_s.rw > 0) {
            _br.width(_s.rw - _s.bw);
        }
        _br.height(_bh - _s.bw);
    }
    var _brs = page.checkJqObj($('#bodyRightSwitch'));
    if (_brs) {
        _brs.width(_s.rsw);
        _brs.height(_bh);
    }
    var _bm = page.checkJqObj($('#bodyMain'));
    if (_bm) {
        _bm.width(_x.width - _s.bw);
    }
    return _bm ? true : false;
};

page._getOuterHeight = function ($o) {
    return $o.length > 0 && $o.is(':visible') ? $o.outerHeight() : 0;
};

page.setBodySize = function () {
    page.bodySize = page.getBodySize();
    var _ = page, _b = page.bodySize, _x = page.boxSize, _s = page.size, _f = page.formSize;
    if (0 == _b.width) {
        return false;
    }
    _s.th = _.pageTopHeight = _._getOuterHeight($('#bodyTitle'));
    _s.bh = _.pageBottomHeight = _._getOuterHeight($('#bodyBottom'));

    _x.width = _b.width - (_s.lw + _s.lsw + _s.rw + _s.rsw);
    _x.height = _b.height - (_s.pt + _s.th + _s.bh);

    _f.width = _x.width;
    _f.height = _x.height - _._getOuterHeight($('#formBottom'));

    var bw = _._setBodySize() ? _s.bw : 0;

    if (_.checkJqObj($('#bodyTitle'))) {
        $('#bodyTitle').width(_x.width - bw);
    }
    if (_.checkJqObj($('#bodyContent'))) {
        $('#bodyContent').width(_x.width - bw);
        $('#bodyContent').height(_x.height);
    }
    //列表页
    if (_.checkJqObj($('#bodyList'))) {
        var lh = _x.height - _._getOuterHeight($('#bodyForm'));
        $('#bodyListBox').height(lh);
        $('#bodyList').height(lh);
    }

    //编辑页
    if (_.checkJqObj($('#formBody'))) {
        $('#formBody').height(_f.height);
        $('#formBox').height(_f.height);
    }

    if (typeof setBodySize == 'function') { setBodySize(); } else if (typeof setBoxSize == 'function') { setBoxSize(); }
};

page._setBodyByShortcutKey = function (callback) {
    document.body.onkeyup = function (e) {
        var e = e || event;
        if (19 == e.keyCode) {  //向上键
            if (typeof callback == 'function') { callback(e.keyCode); }
            else if (typeof keyFunction == 'function') { keyFunction(e.keyCode); }
        } else {
            //当同时按下 Shift + L 键，即触发左栏菜单隐藏/显示
            if (e.shiftKey) {
                switch (e.keyCode) {
                    case 76: //L left
                        page.switchLeft($('#bodyLeftSwitch'));
                        break;
                    case 82: //R right
                        page.switchRight($('#bodyRightSwitch'));
                        break;
                    case 84: //T top

                        break;
                    case 70: //F fullscreen

                        break;
                    default:
                        if (typeof callback == 'function') { callback(e.keyCode); }
                        else if (typeof keyFunction == 'function') { keyFunction(e.keyCode); }
                        break;
                }
            }
        }
    }
};

page.setPageTitle = function (title) {
    $('#bodyTitle .title').html(title);
};


page.parseFileSize = function (fs) {
    var _fs = page.isNumber(fs) ? fs : parseInt(fs, 10);
    return _fs > 1024 * 1024 ? ((Math.round(_fs / 1024 / 1024 * 100) / 100) + ' MB') : ((Math.round(_fs / 1024 * 100) / 100) + ' KB');
};

page.strToNum = function (n) {
    return page.isNumber(n) ? n : parseInt('0' + n, 10);
};

page.checkWinSize = function (s) {
    var bs = cms.util.getBodySize(), w = 0, h = 0, type = 0;
    if (typeof s == 'object') {
        if (s.length >= 2) {
            w = page.strToNum(s[0]), h = page.strToNum(s[1]), type = 0;
        } else {
            w = page.strToNum(s.width), h = page.strToNum(s.height), type = 1;
        }
    } else if (typeof size == 'string') {
        var arr = s.split(',');
        if (arr.length >= 2) {
            w = parseInt(arr[0], 10), h = parseInt(arr[1], 10), type = 2;
        }
    }
    w = w > bs.width ? bs.width - 20 : w, h = h > bs.height ? bs.height - 20 : h;

    return 0 == type ? [w, h] : 1 == type ? { width: w, height: h } : (w + ',' + h);
};

page.getDistanceLength = function (start, end, rate, d) {
    return page.isNumber(start) && page.isNumber(end) ? Math.round((end - start) * rate * d) / d : 0;
};

/*拼音相关*/
page.getPinyin = function (s, o, callback, action) {
    if (s.trim().length > 0) {
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/system/common.aspx',
            data: 'action=' + (action || 'getPinyin') + '&content=' + encodeURIComponent(s),
            callback: function (data, param) {
                module.ajaxResponse(data, param, function (jd, param) {
                    param.obj.attr('value', jd.pinyin);

                    if (typeof callback == 'function') {
                        callback(jd.pinyin);
                    }
                });
            },
            param: { obj: o }
        });
    }
};

page.getPinyinInitial = function (str, o, callback) {
    page.getPinyin(str, o, callback, 'getPinyinInitial');
};

/*图片相关*/
//mw: minWidth
page.showImageView = function (img, name, mw) {
    var imgName = img.substr(img.lastIndexOf('/') + 1);
    var html = [
        '<div id="pwImgBox" style="overflow:hidden;display:block;background:#f1f1f1;">',
        '<img id="pwImg" src="' + img + '" style="display:block;border:none;margin:0 auto;padding:1px;" />',
        '<span style="background:#dbebfe;line-height:20px;border-top:solid 1px #99bbe8;padding:0 5px;display:block;">', name, '<br />',
        '<nobr>', imgName, '</nobr>', '<br />', '<a href="' + img + '" target="_blank">查看源图</a>', '</span>', '</div>'
    ];
    var config = {
        id: 'pwPageImage', title: '查看图片', html: html.join(''),
        width: 'auto', height: 'auto', noBottom: true, filter: false, clickBgClose: 'click'
    };
    window.setTimeout(page.imageResize, 100, cms.box.win(config), $I('pwImgBox'), $I('pwImg'), mw);
};

page.imageResize = function (pwo, box, img, mw) {
    var s = { width: box.offsetWidth > img.offsetWidth ? box.offsetWidth : img.offsetWidth, height: box.offsetHeight + 25 };
    if (s.width < mw) {
        s.width = mw;
    }
    pwo.Resize(s);
};


page.reload = function () {
    cms.box.confirm({
        title: '刷新', html: '确定要刷新（重新加载）当前页面内容吗？',
        callback: function (pwo, pwr) {
            if (pwr.dialogResult) {
                window.location.href = location.href;
            }
            pwo.Hide();
        }
    });
};

/*以下这些是新增的方法*/

page.buildListForm = function () {
    var html = [
        '<div id="bodyListBox" class="listbox" style="position:relative;">',
        '<div id="bodyList" class="list"></div>',
        '<div id="bodyPrompt" class="div-prompt" style="display:none;">对不起，没有找到相关的信息。</div>',
        '<div id="bodyLoading" class="div-loading" style="display:none;">正在加载，请稍候...</div>',
        '</div>'
    ];

    return html.join('');
};

page.buildListTable = function (cssText, id, theadRows) {
    var style = typeof cssText == 'string' ? ' style="' + cssText + '"' : '';
    id = id || 'tbList';
    $('#bodyList').html('<table id="' + id + '" class="tblist" cellpadding="0" cellspacing="0"' + style + '></table>');

    return page._setTableWidth($I(id), typeof theadRows == 'number' ? theadRows : 1);
};

page._setTableWidth = function (tb, theadRows) {
    var w = parseInt('0' + tb.style.width, 10);
    if (cms.util.isMSIE && w <= 0) {
        switch (cms.util.ieVersion()) {
            case 8:
            case 7:
            case 6:
                var min = parseInt('0' + tb.style.minWidth, 10);
                tb.style.width = min > 0 ? min + 'px' : '100%';
                //延时设置表格列宽，因表头还未设置
                window.setTimeout(function () { page._setTableColumnWidth(tb, theadRows); }, 100);
                break;
        }
    }
    return tb;
};

page._setTableColumnWidth = function (tb, theadRows) {
    for (var i = 0, c = tb.rows.length; i < c; i++) {
        if (i >= theadRows) {
            break;
        }
        for (var j = 0, cc = tb.rows[i].cells.length; j < cc; j++) {
            var cell = tb.rows[i].cells[j];
            var max = parseInt('0' + cell.style.maxWidth, 10);
            var min = parseInt('0' + cell.style.minWidth, 10);
            var w = parseInt('0' + cell.style.width, 10);

            if (w <= 0 && (max > 0 || min > 0)) {
                cell.style.width = (max || min) + 'px';
            }
        }
    }
};

page.isChild = function () {
    //return top.window.location != window.location;
    return top.location != self.location;
};

page.isFunction = function (func) {
    return typeof func == 'function';
};

page.isNumber = function (n) {
    return typeof n == 'number';
};

page.isString = function (s) {
    return typeof s == 'string';
};

page.isUndefined = function (o) {
    return typeof o == 'undefined';
};

page.isNull = function (o) {
    return page.isUndefined(o) || o == null;
};

page.checkIsNull = function (o) {
    return page.isUndefined(o) || o == null;
};

page.checkJqObj = function (o, $co) {
    return o && o.length > 0 ? o : ($co || false);
};

page.parseStatus = function (arr, status) {
    return arr[status] || '-';
};

page.parseEnabled = function (enabled) {
    return enabled == 1 ? '启用' : '<em>不启用</em>';
};

page.parseOption = function (arr, option) {
    return arr[option] || '-';
};

page.parseKeyValue = function (object, key) {
    return object[key] || '-';
};

page.buildEditPrompt = function (name, id) {
    return name + (id > 0 ? '修改成功' : '添加成功');
};

page.buildDeletePrompt = function (name, id) {
    id = id || 1;
    return name + (id > 0 ? '已删除' : '删除失败');
};

page.buildTitle = function (name, id) {
    return (!isNaN(parseInt(id, 10)) ? (id > 0 ? page.lang.edit : page.lang.add) : '') + name;
};

page.buildReload = function () {
    return '<a class="btn imgbtn" onclick="page.reload();"><i class="icon-refresh"></i><span>刷新</span></a>';
};

page.buildButton = function (action, name) {
    return '<a class="btn imgbtn" onclick="%s"><i class="icon-add"></i><span>%s</span></a>'.format([action || 'editData();', name || '新增']);
};

page.buildContinue = function (show, o) {
    o = page.checkJqObj(o, $('#formContinue'));
    if (o && page.showFormAction(show, o)) {
        var _k = ['完成后继续添加', '完成后继续添加并保留表单内容', '保留表单内容'];
        var html = [
            '<label class="chb-label-nobg"><input type="checkbox" class="chb" id="chbContinue" /><span>' + _k[0] + '</span></label>',
            '<label class="chb-label-nobg" style="margin-left:20px;" title="' + _k[1] + '"><input type="checkbox" class="chb" id="chbKeepForm" /><span>' + _k[2] + '</span></label>'
        ].join('');
        o.append(html);
    }
};

page.buildFormAction = function (show, o) {
    var html = [
        '<a class="btn btnform" onclick="page.formSubmit();return false;"><span>确认，提交</span></a>',
        '<a class="btn btnform" onclick="page.editCancel(true);" style="margin-left:6px;"><span>取消</span></a>',
        '<a class="btn btnform" onclick="page.editCancel(false);" style="margin-left:6px;"><span>关闭</span></a>',
        '<input id="btnSubmit" type="submit" value="提交" style="visibility:hidden;" />'
    ].join('');
    o = page.checkJqObj(o, $('#formBottom'));
    page.showFormAction(show, o);
    o.append(html);
};

page.showFormAction = function (show, o) {
    o = page.checkJqObj(o, $('#formBottom'));
    show = show || page.isUndefined(show);
    o.css('display', show ? '' : 'none');
    return show;
};

page.checkContinue = function () {
    var isKeepForm = $('#chbKeepForm').is(':checked');
    var isContinue = $('#chbContinue').is(':checked');
    return { isKeep: isKeepForm, isContinue: isContinue };
};

page.formSubmit = function () {
    if (typeof formSubmit == 'function') {
        formSubmit();
    } else {
        cms.box.alert('未找到formSubmit函数，请检查代码!');
    }
};

page.submitConfirm = function (callback, html) {
    cms.box.confirm({
        html: html || '您确定要提交吗？',
        callback: function (pwo, pwr) {
            if (pwr.dialogResult) {
                callback();
            }
            pwo.Hide();
        }
    });
};

page.cancelConfirm = function (callback, isLoad) {
    page.submitConfirm(callback, isLoad ? '您确定要取消吗？' : '您确定要关闭吗？');
};

page.deleteConfirm = function (callback, name, html) {
    if (typeof name == 'string') {
        name = '“' + name + '”';
    }
    cms.box.confirm({
        html: html || '删除后不可恢复，您确定要删除' + name + '吗？',
        callback: function (pwo, pwr) {
            if (pwr.dialogResult) {
                callback();
            }
            pwo.Hide();
        }
    });
};

page.editCallback = function (isLoad, isClose) {
    var kc = page.checkContinue();
    //捕捉异常，使跨域调用时JS代码正常运行
    try {
        if (page.isChild() && typeof parent.editCallback) {
            parent.editCallback(isClose || (!kc.isContinue && !kc.isKeep), isLoad);
        }
    } catch (e) { }
    if (!kc.isKeep) {
        location.href = location.href;
    }
};

page.editCancel = function (isLoad) {
    page.cancelConfirm(function () { page.editCallback(isLoad, true); }, isLoad);
};

page.buildParentTreeBox = function (o, g) {
    g = g || {};
    if (page.isUndefined(g.id)) {
        g.id = 'divParentTree';
    }
    var p = page.isUndefined(o) ? { position: 5, w: 200, h: 200, x: 0, y: 0 } : {
        position: 'custom',
        w: g.width || o.outerWidth(),
        h: g.height || 280,
        x: o.offset().left,
        y: o.offset().top + o.outerHeight()
    };

    return cms.box.win({
        title: g.title || '选择上级目录', html: g.html || '<div id="' + (g.id) + '" style="padding:5px;"></div>',
        noBottom: true, lock: g.lock || true,
        width: p.w, height: p.h, position: p.position, x: p.x, y: p.y, clickBgClose: 'click', bgOpacity: 0.3
    });
};

page.buildTypeTreeBox = function (o, g) {
    g = g || { title: '选择分类' };
    if (page.isUndefined(g.id)) {
        g.id = 'divTypeTree';
    }
    return page.buildParentTreeBox(o, g);
};

page.buildUrl = function (url) {
    var reg = new RegExp('^(http[s]?://|' + webConfig.webDir + ')');
    return url.trim().length > 0 ? (reg.test(url) ? url : webConfig.webDir + url) : '';
};

page.getUrlName = function (url, isGetFullName) {
    var arr = url.split('/');
    var len = arr.length;
    return arr[len - 1] || arr[len - 2] || '';
};

page.buildOptions = function (arr, val, isNum) {
    var html = [];
    for (var i = 0, c = arr.length; i < c; i++) {
        var sd = (isNum ? arr[i] == val : arr[i][0] == val) ? ' selected="selected"' : '';
        html.push('<option value="%s"%s>%s</option>'.format(isNum ? [arr[i], sd, arr[i]] : [arr[i][0], sd, arr[i][1]]));
    }
    return html.join('');
};

page.buildSelect = function (id, options, func) {
    return ['<select id="%s" class="select" onchange="%s" style="margin-right:3px;">'.format([id, func]), options, '</select>'].join('');
};

page.buildSearch = function (arr, o) {
    var html = [
        '<select id="ddlSearchField" class="select">', page.buildOptions(arr), '</select>',
        '<input type="text" class="txt" id="txtKeywords" />',
        '<a onclick="page.loadDataList(true);" class="btn btnsearch" style="margin:0;"></a>',
        '<a onclick="page.loadDataList(false);" class="btn btnsearch-cancel" style="margin:0;"></a>'
    ].join('');
    page.checkJqObj(o, $('#bodyForm')).append(html);

    return html;
};

//加载数据（从第一页开始）
page.loadDataList = function (isSearch) {
    page.initialPageIndex();
    page.getDataList(isSearch);
};

//查询数据列表
page.getDataList = function (isSearch) {
    if (typeof isSearch == 'boolean' && !isSearch) {
        $('#ddlSearchField option:first').prop('selected', 'selected');
        $('#txtKeywords').attr('value', '');
    }
    if (typeof getDataList == 'function') {
        getDataList();
    }
};

page.getData = function () {
    if (typeof getData == 'function') {
        getData();
    }
};

//在相同的查询条件下，间隔5秒才能再次查询，防止狂点按钮
page.formParam = { enabled: true, param: '', timing: 5 * 1000 };
page.checkLoadEnabled = function (urlparam, reload, timing) {
    var _ = page.formParam;
    if (!reload || page.isUndefined(reload)) {
        if (urlparam == _.param && !_.enabled) {
            //alert('防止狂点按钮');
            return false;
        } else {
            _.param = urlparam, _.enabled = page.isNumber(timing) && timing <= 0;
            if (!_.enabled) {
                window.setTimeout(function () { _.enabled = true; }, (page.strToNum(timing) || _.timing));
            }

            page._clearLoadingTimer();
            //若50毫秒后数据还未加载，则显示“正在加载”提示
            page._timer = window.setTimeout(function () { page.showLoading(true); }, 50);
        }
    }
    return true;
};

page._clearLoadingTimer = function (show) {
    if (page._timer != null && !show) {
        window.clearTimeout(page._timer);
    }
};

page.showLoading = function (show, msg) {
    page._clearLoadingTimer(show);
    var o = $('#bodyLoading');
    show ? function () { o.show(); o.html(msg || '正在加载，请稍候...'); }() : o.hide();
};

page.showLoadPrompt = function (show, msg) {
    var o = $('#bodyPrompt');
    show ? function () { o.show(); o.html(msg || '对不起，没有找到相关的信息。'); }() : o.hide();
};

/*分页*/
page.setPagination = function (dataCount, o) {
    var _ = page, pager = new Pagination();
    _.dataCount = dataCount || _.dataCount;
    pager.Show({
        dataCount: _.dataCount, pageSize: _.pageSize, pageIndex: _.pageIndex, pageStart: _.pageStart, markType: 'Symbol',
        showType: 'nolist', callback: 'page.Paging', showDataStat: true, showPageCount: false, keyAble: true
    }, o || $I('pagination'));
};

page.Paging = function (p) {
    page.pageIndex = parseInt(p, 10);
    page.getDataList();
};

page.initialPageIndex = function (ps) {
    page.pageIndex = page.pageStart, page.pageSize = page.strToNum(ps) || page.pageSize;
};

page.checkPageSize = function (arr, val) {
    var exist = ('[' + arr.join('],[') + ']').indexOf('[' + val + ']') >= 0;
    if (!exist) {
        arr.push(val);
        return cms.sort._quickSort(arr);
    }
    return arr;
};

page.isArray = function (arr) {
    return Object.prototype.toString.call(arr) === '[object Array]';
};

page.buildPageSize = function (val, par) {
    page.pageSize = typeof val == 'number' ? val : page.pageSize;
    var arr = [1, 5, 10, 15, 20, 30, 50, 100, 200];
    if (typeof par == 'object') {
        if (page.isArray(par.append)) {
            arr = arr.concat(par.append);
        } else if (page.isArray(par.list)) {
            arr = par.list;
        }
    }
    arr = page.checkPageSize(arr, page.pageSize);
    var html = ['<span style="margin-left:5px;">每页:</span>',
        '<select id="ddlPageSize" class="select" onchange="page.initialPageIndex(this.value);page.getDataList();" style="margin-right:2px;">',
        page.buildOptions(arr, page.pageSize, true), '</select>'
    ].join('');
    return html;
};

page.getRowNum = function (rid) {
    return (page.pageIndex - page.pageStart) * page.pageSize + rid;
};

//关闭未保存的编辑页面时，设置是否提示
page.setParentNeedSave = function (o) {
    if (page.isChild()) {
        o = page.checkJqObj(o, $('#bodyContent'));
        o.keypress(function () {
            try { parent.isNeedSave = true; } catch (e) { alert(location.href + ": " + e); }
        });
    }
};

page.isLoadHead = function () {
    if (page._isLoadHead) {
        return true;
    }
    page._isLoadHead = true;
    return false;
};

page.buildIframe = function (par) {
    par = par || {};
    return '<iframe id="' + (par.id || 'frmMain_') + '" class="' + (par.css || 'frmcon') + '" style="display:block;' + (par.style || '') + '" frameborder="0" scrolling="auto" width="100%" height="100%" src="' + (par.url || 'about:blank') + '"></iframe>';
};

page.setCookie = function (name, value, expireMinutes) {
    if (typeof expireMinutes == 'undefined' || expireMinutes <= 0) {
        document.cookie = name + "=" + escape(value) + ";";
    } else {
        var expDate = new Date();
        expDate.setTime(expDate.getTime() + (8 * 60 * 60 * 1000) + expireMinutes * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + expDate.toGMTString();
    }
};

page.getCookie = function (name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$$)"));
    return arr != null ? unescape(arr[2]) : '';
};

page.delCookie = function (name) {
    var expDate = new Date();
    expDate.setTime(expDate.getTime() - 1);
    var val = page.getCookie(name);
    if (val != null) {
        document.cookie = name + "=" + val + ";expires=" + expDate.toGMTString();
    }
};