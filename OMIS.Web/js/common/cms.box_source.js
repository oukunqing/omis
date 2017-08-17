var cms = cms || {};
cms.box = cms.box || {};

cms.box.win = function (config, autoSize) {
    var _config = config;

    _config.id = config.id || 'pwWinBox';
    _config.title = config.title || '提示信息';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 'auto';
        _config.height = config.height || 'auto';
    }
    _config.minWidth = config.minWidth || 200;

    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.winform = function (config, autoSize) {
    var _config = config;

    _config.id = config.id || 'pwWinForm';
    _config.title = config.title || '提示信息';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.requestType = config.requestType || 'iframe';
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 600;
        _config.height = config.height || 400;
    }
    _config.minWidth = config.minWidth || 200;

    _config.minAble = true;
    _config.maxAble = true;
    _config.showMinMax = true;
    _config.noBottom = true;

    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.editSaveConfirm = function (pwobj, callback) {
    cms.box.confirm({ title: '提示信息', html: '您还没有保存，确定要关闭窗口吗',
        callback: function (pwobj, pwReturn) {
            if (pwReturn.dialogResult) {
                pwobj.Hide();
                if (typeof callback == 'function') {
                    callback();
                }

                if (typeof pwReturn.returnValue == 'object') {
                    pwReturn.returnValue.Hide();
                }
            }
        }, returnValue: pwobj
    });
};

cms.box.alert = function (config, autoSize) {
    var _config = typeof config == 'string' ? { title: '提示信息', html: config} : config;

    _config.id = config.id || 'pwAlertBox';
    _config.title = config.title || '提示信息';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.boxType = 'alert';
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 'auto';
        _config.height = config.height || 'auto';
    }
    _config.realFixed = true;
    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    var pwobj = new PopWin(_config);

    pwobj.Focus();

    return pwobj;
};

cms.box.confirm = function (config, autoSize) {
    var _config = typeof config == 'string' ? { title: '确认信息', html: config} : config;

    _config.id = config.id || 'pwConfirmBox';
    _config.title = config.title || '提示信息';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.boxType = 'confirm';
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 'auto';
        _config.height = config.height || 'auto';
    }
    _config.realFixed = true;
    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.confirmForm = function (config, autoSize) {
    var _config = typeof config == 'string' ? { title: '确认信息', html: config} : config;

    _config.id = config.id || 'pwConfirmBox';
    _config.title = config.title || '提示信息';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.boxType = 'confirmform';
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 'auto';
        _config.height = config.height || 'auto';
    }
    _config.realFixed = true;
    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.msgAndFocus = function (obj, config, autoSize) {
    var _config = config;

    _config.boxType = 'alert';
    _config.callBack = cms.box.setFocus;
    _config.returnValue = obj;

    _config.id = config.id || 'pwMsgBox';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.title = config.title || "提示信息";
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 'auto';
        _config.height = config.height || 'auto';
    }
    _config.realFixed = true;
    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    var pwobj = new PopWin(_config);
    pwobj.Focus();
    return pwobj;
};

cms.box.openwin = function (config) {
    var _config = config;

    _config.boxType = 'window';

    _config.id = config.id || 'pwOpenWinBox';
    _config.title = config.title || '提示信息';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.requestType = config.requestType || 'iframe';
    if (autoSize) {
        _config.width = _config.height = 'auto';
    } else {
        _config.width = config.width || 620;
        _config.height = config.height || 400;
    }
    _config.minWidth = config.minWidth || 200;

    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.form = function (config) {
    var _config = config;

    _config.boxType = 'form';

    _config.id = config.id || 'pwFormBox';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.noBottom = config.noBottom || false;

    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.message = function (config) {
    var _config = config;

    _config.boxType = 'message';

    _config.id = config.id || 'pwMessageBox';
    _config.closeType = config.closeType || 'hide';
    _config.build = config.build || 'refurbish';
    _config.width = config.width || 'auto';
    _config.height = config.height || 'auto';

    if (config.noTitle == undefined) {
        _config.noTitle = true;
    }
    if (config.noBottom == undefined) {
        _config.noBottom = true;
    }

    _config.filter = config.filter || false;
    _config.callBack = config.callback || config.callBack || null;

    return new PopWin(_config);
};

cms.box.close = function (pwobj, pwReturn) {
    pwReturn.returnValue.pwobj.Close();
    pwobj.Close();
};

cms.box.setFocus = function (pwobj, pwReturn) {
    pwobj.Hide();
    var obj = pwReturn.returnValue;
    if (obj != null && obj.style.display != 'none') {
        try {
            var strVal = obj.value.trim();
            obj.value = strVal;
            obj.focus();
        } catch (e) { }
    }
};

cms.box.singleLine = function (str) {
    return '<div style="padding:10px 0 0;">' + str + '</div.';
};

cms.box.buildIframe = function (w, h, isBuild) {
    return cms.util.$('pwAboveOcx') != null || isBuild ? '<iframe src="about:blank" style="position:absolute; visibility:inherit; top:0px; left:-2px; width:' + (w + 4) + 'px; height:' + h + 'px; border:none; z-index:-1; filter=\'progid:DXImageTransform.Microsoft.Alpha(style=0,opacity=0)\';"></iframe>' : '';
};

cms.box.IEUpgrade = function (config) {
    var strHtml = '<div style="line-height:24px; padding:0 10px;">您使用的是IE6.0版本的浏览器，为达到最佳浏览效果，请升级您的浏览器，谢谢！</div>';
    var _config = {
        id: 'ieUpgradeBox',
        noTitle: true,
        boxType: 'window',
        html: strHtml,
        noBottom: true,
        position: 9,
        width: '100%',
        height: 24,
        closeButtonType: 'text',
        lock: false,
        autoClose: true,
        timeout: 10000,
        boxBgColor: '#fffff7',
        borderStyle: 'solid 1px #fc0'
    };
    if (config == undefined || config.closeType == undefined) {
        _config.closeType = 'hide';
    }
    if (config == undefined || config.build == undefined) {
        _config.build = 'refurbish';
    }
    return new PopWin(_config);
};