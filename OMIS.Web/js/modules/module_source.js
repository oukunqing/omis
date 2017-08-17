var module = module || {};
module.loginTimer = null;
module.loginKeepTime = 0;
//1分钟维护一次登录
module.loginKeepInterval = 60 * 1000;
if (typeof webConfig != 'object') {
    webConfig = { webDir: "" };
}
module.path = webConfig != undefined && webConfig != null ? webConfig.webDir : '';
cmsPath = module.path;
//更换令牌的时间间隔
module.replaceTokenInterval = 30 * 60 * 1000;
module.timerToken = null;
//NET框架版本
module.netFrameVersion = $I('lblNetFrameVersion') != null ? parseFloat($I('lblNetFrameVersion').lang, 10) : 4;
module.prefix = module.netFrameVersion >= 4 ? 'cphBody_' : module.netFrameVersion >= 2 ? 'ctl00_cphBody_' : '';

function $$(id, prefix) {
    var ids = module.getIdList(id, prefix);
    return $('#' + ids.join(',#'));
};

function $P(id, prefix) {
    var ids = module.getIdList(id, prefix);
    return $I(ids.join(','));
}

module.$ = function (id, prefix) {
    var ids = module.getIdList(id, prefix);
    return $I(ids.join(','));
};

module.getIdList = function (id, prefix) {
    prefix = prefix || module.prefix;
    var arr = id.split(','), ids = [];
    for (var i in arr) {
        if (arr[i].trim().length > 0) {
            ids.push(prefix + arr[i].substr(arr[i].indexOf('#') == 0 ? 1 : 0));
        }
    }
    return ids;
};

module.openWin = function (code, name, selected, type) {
    if (2 == type) {
        module.openQuickMenuWin(code, name);
        return false;
    }
    var size = module.checkWinSize([600, 400]);
    cms.box.openwin({
        id: 'pwMenuCode', title: name + cms.box.buildIframe(size[0], size[1]),
        html: cms.util.path + '/module.aspx?module=' + code + '&menucode=' + code,
        requestType: 'iframe', noBottom: true, width: size[0], height: size[1],
        maxAble: true, minAble: true, showMinMax: true,
        filter: false, callback: module.closeWin, returnValue: { old: selected, cur: code }
    });
    $('#nav li').removeClass('cur');
    $('#menu_' + code).addClass('cur');
};

module.closeWin = function (pwo, pwr) {
    pwo.Hide();
    $('#menu_' + pwr.returnValue.cur).removeClass('cur');
    $('#menu_' + pwr.returnValue.old).addClass('cur');
};

module.openQuickMenuWin = function (code, name) {
    var size = module.checkWinSize([600, 300]);
    cms.box.openwin({
        id: 'pwQuickMenuCode_' + code, title: name + cms.box.buildIframe(size[0], size[1]),
        html: cms.util.path + '/module.aspx?module=' + code + '&menucode=' + code,
        requestType: 'iframe', noBottom: true, width: size[0], height: size[1],
        maxAble: true, minAble: true, showMinMax: true, filter: false, lock: false
    });
};

module.moduleAction = function (module, p) {
    if ($I('ModuleAction') == null) {
        var label = document.createElement("label");
        label.id = "ModuleAction";
        document.body.appendChild(label);
    }
    var ps = '';
    if (p == null || p == undefined || typeof p !== 'object') {
        p = [];
    }
    for (var i = 0, c = p.length; i < c; i++) {
        ps += '&' + p[i][0] + '=' + p[i][1];
    }
    var form = '<form id="frmMainNav" method="post" action="">'
        + '<input type="hidden" id="module" name="module" />'
        + '<input type="hidden" id="param" name="param" />'
        + '</form>';
    $('#ModuleAction').html(form);
    $('#frmMainNav').attr('action', cms.util.path + '/module.aspx');
    $('#module').attr('value', module);
    $('#param').attr('value', ps);
    $('#frmMainNav').submit();
};

//当菜单没有加链接时，显示一个设计好的向导（400|404）页面
module.checkPageUrl = function (p) {
    if (p.url.trim().length == 0 || p.url.trim().indexOf('about:blank') >= 0) {
        return '/include/400.aspx?' + new Date().getTime() + '&title=' + encodeURIComponent(p.name);
    } else {
        return p.url;
    }
};

//检测返回结果中是否包含数据库连接故障的文字
module.dbConnection = function (s) {
    return s.indexOf('Unable to connect to') >= 0 || s.indexOf('Access denied for user') >= 0 ? 0 : 1;
};

/*令牌相关代码*/
module.getTokenObj = function () {
    var obj = cms.util.$('txtFormToken');
    if (obj == null) {
        obj = cms.util.$('txtAntiCsrfToken');
    }
    return obj;
};

module.buildToken = function () {
    var token = '';
    var obj = module.getTokenObj();
    if (obj != null) {
        token = obj.value.trim();
        if (token.length > 0 && null == module.timerToken) {
            module.timerToken = window.setTimeout(module.replaceToken, module.replaceTokenInterval);
        } else {
            return '&token=';
        }
        token = base64decode(token);
    }
    return '&token=' + encodeURIComponent(token);
};

//定时更换令牌，防止页面停留时间过长导致令牌失效问题
module.replaceToken = function () {
    if (module.timerToken != null) {
        window.clearInterval(module.timerToken);
    }
    module.ajaxRequest({
        url: cmsPath + '/ajax/token.aspx',
        data: 'action=replaceToken',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                var obj = module.getTokenObj();
                if (obj != null) {
                    obj.value = jsondata.token;
                    //令牌更换成功，定时执行下一次更换
                    module.timerToken = window.setTimeout(module.replaceToken, module.replaceTokenInterval);
                }
            });
        }
    });
};

module.toJsonString = function (obj) {
    var isIE8 = navigator.userAgent.indexOf('MSIE 8.0') >= 0;
    /* IE8自带 JSON.stringify方法，并且会将中文编码为 unicode编码 */
    return isIE8 ? JSON2.stringify(obj) : JSON.stringify(obj);
};

module.ajaxRequest = function (p) {
    var ds = null;
    var _p = {
        type: p.type || 'post',
        async: typeof p.async == 'boolean' ? p.async : true,
        dataType: p.dataType || 'json', //xml,html,script,json,jsonp,text
        url: p.url,
        //data: p.data,
        data: p.data + module.buildToken(),
        error: function (jqXHR, textStatus, errorThrown) {
            module.showAjaxErrorData(jqXHR, textStatus, errorThrown);
        },
        success: function (data, textStatus, jqXHR) {
            var callback = p.callback || p.callBack;
            if (typeof callback == 'function') {
                callback(data, p.param);
            } else {
                ds = data;
                data = null;
            }
        },
        complete: function (jqXHR, textStatus) {
            jqXHR = null;
            if (typeof (CollectGarbage) === 'function') {
                CollectGarbage();
            }
        }
    };

    if (_p.datatype == 'jsonp') {
        _p.jsonp = 'callback';
        _p.jsonpCallback = p.jsonpCallback || 'flightHandler';
    }

    $.ajax(_p);

    if (!_p.async) {
        return ds;
    }
};

module.ajaxResponse = function (data, param, callback, finallyCallback) {
    var jd = data || {};
    if (typeof data == 'string') {
        data = data.replace(/\n|\r\n/g, '<br />');
        if (data.isJsonData()) {
            jd = data.toJson();
        } else {
            module.showJsonErrorData(data, {});
            return false;
        }
    }
    var result = false;
    if (1 == jd.result) {
        if (typeof callback == 'function') {
            callback(jd, param);
            result = true;
        }
    } else {
        module.showErrorInfo(jd.msg, jd.error, {});
    }

    if (typeof finallyCallback == 'function') {
        finallyCallback(param);
    }
    return result;
};

module.dbConnectionError = function (er) {
    if (0 == cms.util.dbConnection(er)) {
        cms.box.alert({ id: 'pwDBConnError', title: '错误信息', html: '数据库连接出现异常，请联系管理员...', iconType: 'error' });
        return true;
    }
    return false;
};

module.getTime = function () {
    return new Date().getTime();
};

module.stringConvertNumber = function (n) {
    return typeof n == 'number' ? n : parseInt(n, 10);
};

module.strToNum = function (n) {
    return typeof n == 'number' ? n : parseInt('0' + n, 10);
};

module.checkWinSize = function (s) {
    var bs = cms.util.getBodySize(), w = 0, h = 0, type = 0;
    if (typeof s == 'object') {
        if (s.length >= 2) {
            w = module.strToNum(s[0]), h = module.strToNum(s[1]), type = 0;
        } else if (s.width != undefined && s.height != undefined) {
            w = module.strToNum(s.width), h = module.strToNum(s.height), type = 1;
        }
    } else if (typeof s == 'string') {
        var arr = s.split(',');
        if (s.length >= 2) {
            w = parseInt(arr[0], 10), h = parseInt(arr[0], 10), type = 2;
        }
    }
    if (w > bs.width) {
        w = bs.width - 20;
    }
    if (h > bs.height) {
        h = bs.height - 20;
    }

    return 0 == type ? [w, h] : 1 == type ? { width: w, height: h} : (w + ',' + h);
};

/*用户登录状态*/

//检测用户是否有权限
module.userAuth = function (s) {
    return s.indexOf('noauth') == 0 ? 0 : 1;
};

module.noAuth = function () {
    module.showNoAuth();
    return false;

    var uinfo = cms.util.getCookie('SPCP_USERLOGININFO');

    if (uinfo == '') {
        module.gotoLogin();
    }
    else if (uinfo.isJsonData()) {
        try {
            var ui = eval('(' + uinfo + ')');
            var nowTime = parseInt(new Date().getTime() / 1000);

            //上次登录时间超过3个小时，直接进入登录页面
            if ((nowTime - ui.loginTime) / 60 / 60 >= 3) {
                module.gotoLogin();
            } else {
                module.showNoAuth();
            }
        } catch (ex) {
            module.gotoLogin();
        }
    }
    else {
        module.showNoAuth();
    }
};

module.showNoAuth = function () {
    cms.box.alert({
        id: 'timeout', title: '提示信息', html: '您还没有登录或登录已超时，确定将返回登录界面。',
        width: 350, height: 180, iconType: 'warning', autoClose: true, timeout: 10 * 1000, filter: false, callback: module.gotoLogin
    });
};

module.showOvertime = function () {
    cms.box.alert({
        id: 'timeout', title: '提示信息', html: '登录已超时，确定将返回登录界面。',
        width: 330, height: 180, iconType: 'warning', autoClose: true, timeout: 10 * 1000, filter: false, callback: module.gotoLogin
    });
};

module.checkOvertime = function (str) {
    return str.indexOf('noauth') >= 0;
};

module.gotoLogin = function () {
    top.location.href = (webConfig.webDir + '/' + webConfig.loginUrl).replaceAll('//', '/');
};

module.pwReLogin = null;

/*重新登录*/
module.reLogin = function () {
    var html = '对不起，登录超时或已退出！请重新登录';
    cms.box.alert({
        id: 'pwReLogin',
        title: '提示信息',
        html: html,
        callback: function (pwo, pwr) {
            module.pwReLogin = cms.box.win({
                id: 'pwReLoginForm', title: '重新登录', html: cmsPath + '/login.aspx?pw=1',
                requestType: 'iframe', width: 360, height: 200, noBottom: true
            });
            pwo.Hide();
        }
    });
};

//重新登录回调函数，供login.js回调
function reLoginCallback(pwobj) {
    if (module.pwReLogin != null) {
        module.pwReLogin.Hide();
    }
    cms.box.confirm({
        id: 'pwReLoginC', title: '提示信息', html: '登录成功，请继续操作！',
        iconType: 'success', buttonText: ['刷新', '继续'],
        callback: function (pwo, pwr) {
            if (pwr.dialogResult) {
                location.reload();
            }
        }
    });
}

/*错误信息*/
module.showAjaxErrorData = function (jqXHR, textStatus, errorThrown, config) {
    if (0 == jqXHR.status) {
        return false;
    }
    if (12031 == jqXHR.status || jqXHR.status > 12000) {
        //jquery ajax 中出现的12031错误状态码的原因没有查到，如果有出现，暂时先屏幕
        return false;
    }
    var g = config || {}, id = g.id || 'pwAjaxErrorDataBox', size = [g.width || 300, g.height || 180];
    var html = '应用程序错误，错误信息如下：<br />'
        + 'status: ' + jqXHR.status + '<br />'
        + 'errorThrown: ' + errorThrown + '<br />'
        + 'textStatus: ' + textStatus + '<br />'
        + module.buildPopWinErrorForm(id + 'AjaxErrorDataResult', jqXHR.responseText);
    cms.box.alert({
        id: id, title: g.title || '错误信息', html: html,
        width: size[0], height: size[1], iconType: 'error', filter: false
    });
};

module.showJsonErrorData = function (data, config) {
    var g = config || {}, id = g.id || 'pwJsonErrorDataBox', title = g.title || '错误信息', size = [g.width || 280, g.height || 150];
    var html = 'JSON数据格式错误，请检查程序代码。<br />'
        + module.buildPopWinErrorForm(id + 'JsonErrorDataResult', data.replaceAll('<br />', '\r\n'));

    var _g = { id: id, title: title, html: html, width: size[0], height: size[1], filter: false };
    /*
    if ('Token Invalid' == data) {
    _g.callback = function (pwo, pwr) {
    location.reload();
    };
    }
    */
    cms.box.alert(_g);
};

module.showErrorInfo = function (msg, er, config) {
    msg = msg || '';
    er = er || '';
    if (typeof er == 'string' && 'noauth'.equals(er.toLowerCase())) {
        module.showNoAuth();
        return false;
    }
    if (typeof msg == 'string' && 'noauth'.equals(msg.toLowerCase())) {
        module.reLogin();
        return false;
    }
    var g = config || {};
    var id = g.id || 'pwErrorInfoBox';
    var title = g.title || '提示信息';
    var icon = 'warning';
    var size = [g.width || 'auto', g.height || 'auto'];

    if (typeof er == 'string' && er.length > 0) {
        size = [g.width || 280, g.height || 150], title = '错误信息', icon = 'error';
        msg += '<br />';
        msg += module.buildPopWinErrorForm(id + 'ErrorInfoResult', er.replaceAll('<br />', '\r\n'));
    }
    var _g = { id: id, title: title, html: msg, iconType: icon, width: size[0], height: size[1], filter: false };
    if (msg.toLowerCase().equals('token invalid')) {
        _g.callback = function (pwo, pwr) {
            location.reload();
        };
    }
    cms.box.alert(_g);
};

module.buildPopWinErrorForm = function (oid, info) {
    return '<a onclick="module.showErrorDetail($I(\'' + oid + '\').value);">查看错误代码</a>' +
        '<textarea style="display:none;" id="' + oid + '">' + info + '</textarea>';
};

module.showErrorDetail = function (s) {
    var size = [420, 240];
    var type = ' style="width:' + (size[0] - 10) + 'px;height:' + (size[1] - 25) + 'px;'
        + 'border:none;margin:0;padding:0 5px;font-size:12px;line-height:18px;font-family:宋体,Arial;'
        + '" ';
    var html = '<textarea id="txtErrorDetail_PopWin" ' + type + ' readonly="readonly">' + s + '</textarea>';
    cms.box.win({
        id: 'pwErrorDetail', title: '错误信息详情', html: html,
        width: size[0], height: size[1], noBottom: true,
        maxAble: true, showMinMax: true, filter: false,
        callBackByResize: function (winModel, bs) {
            $('#txtErrorDetail_PopWin').width(bs.w - 10);
            $('#txtErrorDetail_PopWin').height(bs.h);
        }
    });
};

module.checkPwdComplexity = function (pwd) {
    if ('' == pwd.trim()) {
        return -1;
    }
    pwd = ('' + pwd).trim();
    var chars = pwd.split('');
    var isSame = ('' == pwd.replaceAll(chars[0], ''));
    if (isSame) {
        return 0;
    }
    var isSequence = true;
    for (var i = 0, c = chars.length - 1; i < c; i++) {
        if (Math.abs(chars[i].charCodeAt(0) - chars[i + 1].charCodeAt(0)) != 1) {
            isSequence = false;
            break;
        }
    }
    if (isSequence) {
        return 1;
    }
    var arrPwdDic = [
        ['admin', 'password', 'master', 'super', 'passwd', 'p@ssword', 'passw0rd', 'superman', 'qwerty', 'qweasd', '1qaz2wsx', 'qazwsx'],
        ['admin123', 'admin1234', 'admin12345', 'admin123456', 'admin888', 'admin654321', 'admin54321', 'admin321', 'abc123', '123qwe', '132546'],
        ['qq123456', 'wang1234', 'abcdef', 'abcabc', 'a1b2c3', 'aaa111', 'iloveyou', '5201314', 'taobao', 'xiaoming'],
        ['root', 'abc', 'pass', 'pwd', 'asdf']
    ];
    for (var i = 0, c = arrPwdDic.length; i < c; i++) {
        for (j = 0, n = arrPwdDic[i].length; j < n; j++) {
            if (pwd.equals(arrPwdDic[i][j])) {
                return 2;
            }
        }
    }
    return 3;
};

//用户名密码正则表达式
module.patternName = /^[a-zA-Z\u4e00-\u9fa5]{1}[a-zA-Z0-9\u4e00-\u9fa5_]{1,29}$/;
module.promptName = '用户名长度为2-30个字符，由中英文字母或数字组成，以中英文字母开头。';
module.patternPwd = /^[\w.!@#$%^&_]{5,30}$/;
module.promptPwd = '密码长度为5-30个字符，由英文字母或数字组成！';
module.promptPwdSet = '您的密码设置过于简单，可能会导致帐户不安全！';
module.patternEmail = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
module.patternMobile = /^(13[0-9]|14[7]|15[^4]|18[0|2|6|8|9])\d{8}$/;
module.patternTelephone = /^0\d{2,3}(\-)?\d{7,8}$/;


/*帮助信息*/
module.showHelpInfo = function (config, action) {
    if (action == 'close' || action == 'hide') {
        popwin.Hide('pwHelpInfo');
        return false;
    }
    var cfg = config || {}, size = [cfg.width || 1000, cfg.height || 600];
    var strFrame = ''; //cms.box.buildIframe(size[0], size[1]);
    var _config = {
        id: 'pwHelpInfo', title: (cfg.title || '帮助信息') + strFrame,
        html: cmsPath + '/modules/help/default.aspx',
        requestType: 'iframe', width: size[0], height: size[1], position: 3, x: 68, y: 4
    };
    return cms.box.winform(_config);
};

module.showVersionInfo = function (config) {
    var cfg = config || {};
    var size = [cfg.width || 420, cfg.height || 280];
    var strFrame = cms.box.buildIframe(size[0], size[1]);
    var url = cmsPath + '/common/aspx/version.aspx?' + module.getTime();
    var _config = {
        id: 'pwCmsVersion', title: '版本信息' + strFrame, html: url,
        requestType: 'iframe', width: size[0], height: size[1], noBottom: true, filter: false
    };
    return cms.box.win(_config);
};

window.onerror = function (msg, url, line) {
    cms.box.alert('Error:' + msg + '\nRawUrl:' + url + '\nStackTrace:' + line);
    return true;
};

/*以下代码为打印功能*/
module.printRequestSubmit = function (p) {
    var url = p.url || cmsPath + '/modules/print/print.aspx';
    if (typeof p.urlparam == 'string' && p.urlparam.length > 0) {
        url += '?' + p.urlparam;
    }
    var form = '<form id="frmPrintRequest_Print" target="_blank" method="post" action="' + url + '">'
        + '<input type="hidden" id="Data_Print" name="Data_Print" value="' + encodeURIComponent(p.data || '') + '" />'
        + '<input type="hidden" id="Css_Print" name="Css_Print" value="' + encodeURIComponent(p.css || '') + '" />'
        + '<input type="hidden" id="Title_Print" name="Title_Print" value="' + encodeURIComponent(p.title || '') + '" />'
        + '<input type="hidden" id="Auto_Print" name="Auto_Print" value="1" />'
        + '</form>';

    if ($I('divPrint_PrintRequest') == null) {
        var op = document.createElement('DIV');
        op.id = 'divPrint_PrintRequest';
        op.style.display = 'none';
        op.innerHTML = form;

        document.body.appendChild(op);
    } else {
        $I('divPrint_PrintRequest').innerHTML = form;
    }

    if (p.reload) {
        window.setTimeout(function () {
            window.location.reload();
        }, 200);
    }

    $('#frmPrintRequest_Print').submit();
};

module.printRequest = function (p) {
    if (typeof p == 'string') {
        p = { urlparam: '', data: p, css: '', title: '' };
    } else if (typeof p == 'undefined') {
        p = { urlparam: '', data: '', css: '', title: '' };
    }
    //默认不再确认
    if (typeof p.confirm == 'function') {
        p.confirm = false;
    }

    if (!p.confirm) {
        module.printRequestSubmit(p);
    } else {
        cms.box.confirm({
            id: 'pwPrintRequest',
            title: '打印',
            html: '确定要打印' + (p.title || '') + '吗？',
            callback: function (pwo, pwr) {
                if (pwr.dialogResult) {
                    module.printRequestSubmit(pwr.returnValue);
                }
            },
            returnValue: p
        });
    }
};

//打印表单
module.printForm = function (p) {
    p = p || {};
    p.url = p.url || cmsPath + '/modules/print/print.aspx';

    module.printRequest(p);
};

//打印设备台帐
module.printDeviceAccount = function (p) {
    p = p || {};
    p.url = p.url || cmsPath + '/modules/print/print1.aspx';

    module.printRequest(p);
};

//打印表格
module.printTable = function (p) {
    p = p || {};
    p.url = p.url || cmsPath + '/modules/print/print1.aspx';

    module.printRequest(p);
};

//解析列Index，因列Index 若为-1表示倒数第1列，需要解析成正数
module.parseCols = function (arrCols, totalCols) {
    for (var i in arrCols) {
        if (arrCols[i] < 0) {
            arrCols[i] += totalCols;
        }
    }
    return arrCols;
};

/*
表格列表打印过滤
设置表格总宽度（为100%），过滤需要隐藏的列
param:{
obj: table对象,
//表格标题，显示在表格上方
title: '',
caption: '', //同title
//后缀，显示在表格下方
postfix: '',
//要隐藏的列Index
cols: []
}
*/
module.tablePrintFilter = function (p) {
    var tb = p.obj || null;
    if (typeof tb != 'object' || tb == null || tb.tagName != 'TABLE') {
        return false;
    }
    var rows = tb.rows.length;
    if (rows <= 0) {
        return false;
    }
    tb.style.width = '100%';

    //给表格加标题
    var title = p.title || p.caption;
    if (typeof title == 'string' && title.trim().length > 0) {
        var div = document.createElement('DIV');
        div.innerHTML = title;
        tb.parentNode.insertBefore(div, tb);
    }
    //给表格加尾缀说明
    var postfix = p.postfix;
    if (typeof postfix == 'string' && postfix.trim().length > 0) {
        var div = document.createElement('DIV');
        div.innerHTML = postfix;
        if (tb.nextSibling != null) {
            tb.parentNode.insertBefore(div, tb.nextSibling);
        } else {
            tb.parentNode.appendChild(div);
        }
    }

    var totalCols = tb.rows[0].cells.length;
    //解析列Index
    var arrCols = module.parseCols(p.cols || [], totalCols);
    if (arrCols.length > 0) {
        var htCols = new cms.util.Hashtable();
        for (var i in arrCols) {
            htCols.add(arrCols[i]);
        }
        //移除指定的列
        for (var i = rows - 1; i >= 0; i--) {
            var cols = tb.rows[i].cells.length;
            for (var j = cols - 1; j >= 0; j--) {
                if (htCols.contains(j)) {
                    //tb.rows[i].cells[j].style.display = 'none';
                    tb.rows[i].removeChild(tb.rows[i].cells[j]);
                }
            }
        }
    }
};

//复制表单控件DOM
module.copyFormControl = function (op, p) {
    var oc = document.createElement('DIV');
    oc.appendChild(op.cloneNode(true));

    var opc = oc.childNodes[0];
    opc.style.width = 'auto', opc.style.height = 'auto';
    opc.style.minWidth = '', opc.style.minHeight = '';

    //由于selectedIndex和value都是操作性属性
    //cloneNode 无法复制SELECT的操作性属性
    var asc = oc.getElementsByTagName('SELECT'), aso = op.getElementsByTagName('SELECT');
    for (var i = 0, c = asc.length; i < c; i++) {
        asc[i].value = aso[i].value, asc[i].selectedIndex = aso[i].selectedIndex;
    }

    var atc = oc.getElementsByTagName('TEXTAREA'), ato = op.getElementsByTagName('TEXTAREA');
    for (var i = 0, c = atc.length; i < c; i++) {
        atc[i].style.width = ato[i].offsetWidth + 'px', atc[i].style.height = ato[i].offsetHeight + 'px', atc[i].value = ato[i].value;
    }

    //处理表格
    if (typeof p == 'object' && p.obj != null && p.obj.tagName == 'TABLE') {
        document.body.appendChild(oc);

        //更改复制的TABLE的ID
        var tbs = oc.getElementsByTagName('TABLE');
        for (var i = 0, c = tbs.length; i < c; i++) {
            tbs[i].id += '_Print_Copy';
            tbs[i].style.width = '100%', tbs[i].style.minWidth = '', tbs[i].style.minHeight = '';

            if (p.showBorder) {
                tbs[i].border = '1';
            }

            for (var j = 0, jc = tbs[i].rows.length; j < jc; j++) {
                if (p.clearMinWidth && j <= 2) {
                    for (var k = 0, kc = tbs[i].rows[j].cells.length; k < kc; k++) {
                        tbs[i].rows[j].cells[k].style.minWidth = '';
                    }
                }
                if (tbs[i].rows[j].lang != undefined) {
                    tbs[i].rows[j].lang = '';
                }
            }
        }

        //获取复制的新表格
        p.obj = document.getElementById(p.obj.id + '_Print_Copy');
        //过滤表格
        module.tablePrintFilter(p);

        //处理完成后，移除复制的表格
        document.body.removeChild(oc);
    }

    return oc;
};

/*
表单元素转换为打印内容
为简化表单或表格打印功能
objParent: html元素（div|table等）
p: {
//清除指定样式名称的 A或SPAN标签,样式名可写多个
className:[],
//是否移除输入控件（如Input/Select)，或不移动，将会被隐藏不显示，默认为移除
removeChild: true|false,
//是否复制后再处理转换，若是表格，默认会复制后处理
isCopy: true|false,
//是否保持输入控件元素的尺寸
keepInputSize: true | false,
//是否自动控制控件元素的尺寸（跟随内容的多少显示高度宽度），优先级高于keepInputSize
//即当autoInputSize 设为true时，不管keepInputSize设置为true|false，都视keepInputSize为无效
autoInputSize: true | false,
//是否仅打印纯内容（输入控件元素内的内容）
onlyPrintContent: true|false,
//是否显示签名（或内容）下划线
showSignLine: true|false,
pattern: 正则表达式对象，有时需要清除特定的字符，例：/(<em>\*<\/em>|<i>\*<\/i>)/g
}
tableFilterParam: {
obj: document.getElementById(TableId),
//需要隐藏的列Index，-1表示倒数第一列
cols: [-1, 0, 1],
//清除单元格最小宽度限制，防止表格宽度太宽
clearMinWidth: true|false,
//显示表格边框线（用于导出表格数据）
showBorder: true,
//表格标题 相当于caption
title: '',
//备注后缀，位于表格下方
postfix: ''
}
*/
module.formConvertContent = function (objParent, p, tableFilterParam) {
    if (objParent == null) {
        return false;
    }
    if (typeof p != 'object') {
        p = { className: ['explain', 'btn'], removeChild: true };
    }
    if (typeof p.className == 'undefined') {
        p.className = ['explain', 'btn'];
    } else if (typeof p.className == 'string') {
        p.className = [p.className];
    }
    if (typeof p.textColor == 'undefined') {
        p.textColor = '#000';
    }

    var isCopy = p.isCopy || false;
    if (isCopy || (typeof tableFilterParam == 'object' && tableFilterParam.obj != null && tableFilterParam.obj.tagName == 'TABLE')) {
        //复制整个表单
        objParent = module.copyFormControl(objParent, tableFilterParam);
        isCopy = true;
    }

    //是否移除元素，默认为移除
    if (typeof p.removeChild != 'boolean') {
        p.removeChild = true;
    }
    //是否保持输入元素的尺寸（input/textarea/select等元素）
    if (typeof p.keepInputSize != 'boolean') {
        p.keepInputSize = false;
    }
    //元素尺寸是否自动（随内容）,优先级高于 param.keepInputSize
    if (typeof p.autoInputSize != 'boolean') {
        p.autoInputSize = false;
    }
    //是否只打印内容，当打印的纸张带格式时，仅需要打印内容
    if (typeof p.onlyPrintContent != 'boolean') {
        p.onlyPrintContent = false;
    }
    //是否显示控件元素下划线
    if (typeof p.showUnderline != 'boolean') {
        p.showUnderline = false;
    }
    //控件元素下划线样式
    if (typeof p.underLineStyle != 'string') {
        p.underLineStyle = '';
    }
    //需要处理的元素
    var arrTagName = ['A', 'SPAN', 'INPUT', 'TEXTAREA', 'SELECT', 'BUTTON'];

    for (var t in arrTagName) {
        var arrObj = objParent.getElementsByTagName(arrTagName[t]);
        var c = arrObj.length;
        //遍历元素的时候需要从后面开始遍历，因为在处理的过程中要移除DOM元素
        for (var i = c - 1; i >= 0; i--) {
            var obj = arrObj[i];
            //检测元素是否有效，是否被隐藏， printHidden属性为隐藏后加的标记
            if (typeof obj != 'undefined' && obj != null && typeof obj.printHidden == 'undefined') {
                var parent = obj.parentNode;
                if (obj.tagName == 'SPAN' || obj.tagName == 'A') {
                    if (obj.title != undefined && obj.title.length > 0) {
                        obj.title = '';
                    }
                    if (obj.alt != undefined && obj.alt.length > 0) {
                        obj.alt = '';
                    }

                    //仅打印内容，隐藏SPAN标签（为打印而转换的SPAN除外）
                    if (p.onlyPrintContent && obj.tagName == 'SPAN' && obj.convert != 1) {
                        //先设置内容尺寸，保持格式位置不变
                        obj.style.width = obj.offsetWidth + 'px';
                        //不指定高度
                        //obj.style.height = obj.offsetHeight + 'px';
                        obj.style.display = 'inline-block';
                        //清除内容
                        obj.innerHTML = '';
                    } else {
                        //隐藏指定样式的SPAN、A
                        for (var j in p.className) {
                            var arrCss = obj.className.split(' ');
                            for (var k in arrCss) {
                                if (arrCss[k] == p.className[j]) {
                                    if (p.removeChild && obj.parentNode != null) {
                                        obj.parentNode.removeChild(obj);
                                    } else {
                                        obj.style.display = 'none';
                                        //设置转换标记
                                        obj.printHidden = 1;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                } else {
                    var isConvert = true;
                    var strValue = '';
                    //获取HTML控件元素内容
                    if (obj.tagName == 'INPUT' && obj.type == 'text') {
                        strValue = obj.value;
                    } else if (obj.tagName == 'TEXTAREA') {
                        strValue = obj.value.replace(/(\n|\r\n)/g, '<br />');
                    } else if (obj.tagName == 'SELECT') {
                        if (obj.style.display != 'none' && obj.options.length > 0) {
                            var op = obj.options[obj.selectedIndex];
                            strValue = op != undefined ? obj.options[obj.selectedIndex].text : '';
                            //清除空格，以便判断开头文字
                            strValue = strValue.replace(/(^[\s]*)|([\s]*$)/g, '');

                            //下拉列表的文本若以“请选择”开头，则置为空
                            if (strValue.indexOf('请选择') == 0 || strValue.indexOf('选择') == 0) {
                                strValue = '';
                            }
                        }
                    } else {
                        isConvert = false;
                    }

                    if (isConvert) {
                        var label = document.createElement('SPAN');
                        //为打印创建的元素
                        label.printCreate = 1;
                        label.innerHTML = strValue + (strValue.length > 0 ? '' : '&nbsp;');

                        var isTextarea = obj.tagName == 'TEXTAREA';

                        //显示下划线，仅限于单行文本框
                        if (p.showUnderline && !isTextarea) {
                            var borderBottom = module.getElementStyle(obj, 'borderBottom');
                            if (borderBottom.length > 0) {
                                label.style.borderBottom = p.underLineStyle || borderBottom;
                            }
                        }

                        //多行文本或无内容，则保留元素的宽度高度，以保持内容排版的尺寸
                        if (!p.autoInputSize && (p.keepInputSize || isTextarea || strValue.replace(/(^[\s]*)|([\s]*$)/g, '').length == 0)) {

                            label.style.display = 'inline-block';
                            //保留输入文本的水平对齐方式
                            label.style.textAlign = module.getElementStyle(obj, 'textAlign');
                            //保留输入文本的字体大小
                            label.style.fontSize = module.getElementStyle(obj, 'fontSize');
                            //保留输入文本的字体行距
                            label.style.lineHeight = module.getElementStyle(obj, 'lineHeight');

                            if (obj.disabled) {
                                label.style.color = p.textColor;
                            } else {
                                //保留文本的前景色
                                label.style.color = module.getElementStyle(obj, 'color');
                            }

                            if (isCopy) {
                                //因为clone后 offsetWidth 会等于0
                                //创建HTML元素之前，需要先指定宽度（为了兼容IE浏览器）
                                label.style.width = obj.offsetWidth > 0 ? obj.offsetWidth + 'px' : obj.style.width; //obj.clientWidth + 'px';
                                //插入元素，这里不能用appendChild方法
                                parent.insertBefore(label, obj);

                                //label.style.height = (isTextarea ? (obj.offsetHeight > 0 ? obj.offsetHeight + 'px' : obj.style.height) : '12px');
                                if (isTextarea) {
                                    module.setTextareaContentHeight(label, label.offsetHeight, obj.offsetHeight);
                                    label.style.height = obj.offsetHeight > 0 ? obj.offsetHeight + 'px' : obj.style.height;
                                }
                            } else {
                                //创建HTML元素之前，需要先指定宽度（为了兼容IE浏览器）
                                label.style.width = isTextarea ? obj.offsetWidth + 'px' : obj.clientWidth + 'px';
                                //插入元素，这里不能用appendChild方法
                                parent.insertBefore(label, obj);

                                //label.style.height = isTextarea ? obj.offsetHeight + 'px' : 12 + 'px';
                                if (isTextarea) {
                                    module.setTextareaContentHeight(label, label.offsetHeight, obj.offsetHeight);
                                    label.style.height = obj.offsetHeight + 'px';
                                }
                            }

                            //设置转换标记，二次转换的时候会用到（仅打印内容时会用到）
                            label.convert = 1;
                        } else {
                            //插入元素，这里不能用appendChild方法
                            parent.insertBefore(label, obj);
                        }
                    }
                    if (p.removeChild) {
                        obj.parentNode.removeChild(obj);
                    } else {
                        obj.style.display = 'none';
                        //设置转换隐藏标记
                        obj.printHidden = 1;
                    }
                }
            }
        }
    }

    //导出数据时，有时需要删除图片
    if (p.isExport && p.removeImage) {
        var arrImg = objParent.getElementsByTagName('IMG');
        for (var i = arrImg.length - 1; i >= 0; i--) {
            var parent = arrImg[i].parentNode;
            var label = document.createElement('SPAN');
            label.innerHTML = "&nbsp;&nbsp;";
            parent.insertBefore(label, arrImg[i]);
            parent.removeChild(arrImg[i]);
        }
    }

    //清除DIV的Title
    var arrDiv = objParent.getElementsByTagName('DIV');
    for (var i = 0, c = arrDiv.length; i < c; i++) {
        var div = arrDiv[i];
        if (div.title.length > 0) {
            div.title = '';
        }
    }

    var pattern = p.pattern;
    /*
    清除*号提示的默认正则： /(<em>\*<\/em>|<i>\*<\/i>)/g
    */
    if (typeof pattern == 'object') {
        objParent.innerHTML = objParent.innerHTML.replace(pattern, '');
    }

    var html = objParent.innerHTML;

    objParent = null;
    delete objParent;

    return html;
};

module.setTextareaContentHeight = function (label, conHeight, objHeight) {
    var ch = conHeight - objHeight;
    var lh = 18;
    var fs = 12;
    var isControl = ch > 10;
    if (isControl) {
        var r = parseInt(Math.round(objHeight / ch), 10);
        lh = r <= 1 ? 12 : (r <= 3 ? 13 : (r == 4 ? 14 : 15));
        //fs = r <= 1 ? 10 : 12;

        label.style.lineHeight = lh + 'px';
        /*
        label.style.fontSize = fs + 'px';
        if (fs < 12) {
        label.className += ' fs10';
        }
        */
    }
};

module.getElementStyle = function (obj, styleName) {
    if (obj.currentStyle) {	//IE浏览器
        return obj.currentStyle[styleName];
    } else {
        return obj.ownerDocument.defaultView.getComputedStyle(obj, null)[styleName];
    }
};

/*
转换日期时间格式，格式可相对自定义
obj: html控件元素
param: { format:['年','月', '日'] }
例：2015-12-01 10:15:12 转换成 2015年12月01日10时15分
*/
module.datetimeConvertFormatToPrint = function (obj, param) {
    if (obj == null) {
        return false;
    }
    var defaultFormat = ['年', '月', '日', '时', '分'];
    if (typeof param != 'object' || param == null) {
        param = {
            format: defaultFormat
        };
    }
    if (typeof param.start == 'undefined') {
        param.start = 0;
    }
    if (typeof param.cssText != 'string') {
        param.cssText = '';
    }
    var strVal = obj.value || obj.innerHTML || '';
    //检测并转换日期格式
    strVal = strVal.trim().toDate().toString('yyyy-MM-dd HH:mm:ss');
    if (strVal.length == 0) {
        //strVal = '---::';
        strVal = '-----';
    }
    var objParent = obj.parentNode;

    //var arr = strVal.split(/(\-|\s|\:)/);  // m=2;
    //用正则方式分割字符串，存在IE浏览器兼容性问题，故改用下面的方式处理  // m=1;
    var arr = strVal.replace(/(\s|\:)/g, '-').split('-');
    var len = arr.length;
    var m = 1;
    var n = 0;
    //宽度
    var w = param.width || 20;
    var arrTxt = param.format || defaultFormat;

    var frag = document.createDocumentFragment();
    for (var i = param.start; i < len; i += m) {
        if (n >= arrTxt.length) {
            break;
        }
        var txt = document.createElement('Input');
        txt.type = 'text';
        txt.className = 'txt';
        txt.style.cssText = 'border:solid 1px #ddd;height:12px;' + param.cssText;
        //设置年、月、日内容宽度，年宽度为月宽度的2倍
        txt.style.width = (i == 0 ? (w * 2) : w) + 'px';
        txt.style.textAlign = 'center';
        txt.value = arr[i];

        var label = document.createElement('Span');
        label.innerHTML = arrTxt[n];
        label.style.width = '16px';

        frag.appendChild(txt);
        frag.appendChild(label);
        n++;
    }

    objParent.insertBefore(frag, obj);

    objParent.removeChild(obj);
};


/*以下代码为导出功能*/
module.exportRequestSubmit = function (param) {
    var url = param.url || cmsPath + '/ajax/export.aspx?action=export';
    if (typeof param.urlparam == 'string' && param.urlparam.length > 0) {
        url += '&' + param.urlparam;
    }
    var strForm = '<form id="frmExportRequest_Export" target="_blank" method="post" action="' + url + '">'
        + '<input type="hidden" id="Data_Export" name="Data_Export" value="' + encodeURIComponent(param.data || '') + '" />'
        + '<input type="hidden" id="Title_Export" name="Title_Export" value="' + encodeURIComponent(param.title || '') + '" />'
        + '<input type="hidden" id="Name_Export" name="Name_Export" value="' + encodeURIComponent(param.name || '') + '" />'
        + '<input type="hidden" id="FileName_Export" name="FileName_Export" value="' + encodeURIComponent(param.fileName || '') + '" />'
        + '</form>';

    if ($I('divExport_ExportRequest') == null) {
        var divExport = document.createElement('DIV');
        divExport.id = 'divExport_ExportRequest';
        divExport.style.display = 'none';
        divExport.innerHTML = strForm;

        document.body.appendChild(divExport);
    } else {
        $I('divExport_ExportRequest').innerHTML = strForm;
    }

    if (param.reload) {
        window.setTimeout(function () {
            window.location.reload();
        }, 200);
    }

    $('#frmExportRequest_Export').submit();
};

module.exportRequest = function (param) {
    if (typeof param == 'string') {
        param = { urlparam: '', data: param, name: '', title: '', fileName: '' };
    } else if (typeof param == 'undefined') {
        param = { urlparam: '', data: '', name: '', title: '', fileName: '' };
    }
    //默认不再确认
    if (typeof param.confirm == 'function') {
        param.confirm = false;
    }

    if (!param.confirm) {
        module.exportRequestSubmit(param);
    } else {
        cms.box.confirm({
            id: 'pwExportRequest',
            title: '导出',
            html: '确定要导出' + (param.title || '') + '吗？',
            callback: function (pwobj, pwReturn) {
                if (pwReturn.dialogResult) {
                    module.exportRequestSubmit(pwReturn.returnValue);
                }
            },
            returnValue: param
        });
    }
};

module.getTableColumns = function (tb) {
    if (typeof tb != 'object') {
        return 0;
    }
    if (tb.rows.length > 0) {
        var cells = tb.rows[0].cells.length;
        for (var i = 0, c = cells; i < c; i++) {
            cells += tb.rows[0].cells[i].colSpan - 1;
        }
        return cells;
    }
    return 0;
};

module.buildExportCaption = function (caption, tb) {
    var colspan = module.getTableColumns(tb);
    return '<table><tr><td colspan="' + colspan + '">' + caption + '</td></tr></table>';
};

/*以下代码为合并表格单元格*/

module.setMergeCellRemoveFlag = function (cell) {
    if (cell != null) {
        cell.style.display = 'none';
        cell.mergeCellRemove = 1;
    }
};

/*
合并表格单元格
tb: 表格对象
arr: [
{rowIndex:0, colIndex:0, rowSpan:0, colSpan:0, isRemove: true}
]
*/
module.mergeTableCell = function (tb, arr) {
    for (var m = 0, n = arr.length; m < n; m++) {
        var par = arr[m];
        par.isRemove = typeof par.isRemove == 'boolean' ? par.isRemmove : true;

        for (var i = 0, c = tb.rows.length; i < c; i++) {
            if (i == par.rowIndex) {
                for (var j = 0, cc = tb.rows[i].cells.length; j < cc; j++) {
                    if (j == par.colIndex) {
                        var cell = tb.rows[i].cells[j];
                        var r_cell = null;
                        if (par.rowSpan > 0) {
                            cell.rowSpan = par.rowSpan;
                            if (par.isRemove) {
                                for (var k = i + 1; k < (i + par.rowSpan); k++) {
                                    module.setMergeCellRemoveFlag(tb.rows[k].cells[j]);
                                }
                            }
                        }
                        if (par.colSpan > 0) {
                            cell.colSpan = par.colSpan;
                            if (par.isRemove) {
                                for (var k = j + 1; k < (j + par.colSpan); k++) {
                                    module.setMergeCellRemoveFlag(tb.rows[i].cells[k]);

                                    for (var kk = 0; kk < par.rowSpan; kk++) {
                                        module.setMergeCellRemoveFlag(tb.rows[i + kk].cells[k]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    for (var i = tb.rows.length - 1; i >= 0; i--) {
        var row = tb.rows[i];
        for (var j = row.cells.length - 1; j >= 0; j--) {
            if (row.cells[j].mergeCellRemove == 1) {
                row.removeChild(row.cells[j]);
            }
        }
    }
};

module.setImageSize = function (obj, w, h) {
    var img = new Image();
    img.src = obj.src;

    var nw = w;
    var nh = h;

    img.onload = function () {
        var ow = img.width;
        var oh = img.height;
        if (ow <= w && oh <= h) {
            w = ow;
            h = oh;
        } else {
            if (ow > oh) {
                h = parseInt(oh * (w / ow), 10);
            } else {
                w = parseInt(ow * (h / oh), 10);
            }
        }
        //宽度减去2个px边框
        w = w - 2;
        h = h - 2;
        obj.style.width = w + 'px';
        obj.style.height = h + 'px';

        var pos = {
            left: parseInt((nw - w) / 2, 10),
            top: parseInt((nh - h) / 2, 10)
        };

        obj.style.marginLeft = pos.left + 'px';
        obj.style.marginTop = pos.top + 'px';
    };
};

module.isArray = function (arr) {
    return Object.prototype.toString.call(arr) === '[object Array]';
};

module.setControlValue = function (arrObj, arrVal) {
    if (module.isArray(arrObj)) {
        for (var i = 0, c = arrObj.length; i < c; i++) {
            cms.util.setControlValue(arrObj[i], arrVal[i]);
        }
    } else {
        cms.util.setControlValue(arrObj, arrVal);
    }
};

module.setControlLang = function (arrObj, arrVal) {
    if (module.isArray(arrObj)) {
        for (var i = 0, c = arrObj.length; i < c; i++) {
            cms.util.setControlLang(arrObj[i], arrVal[i]);
        }
    } else {
        cms.util.setControlLang(arrObj, arrVal);
    }
};

module.getControlValue = function (val, dval, isLang) {
    if (typeof dval == 'undefined' || dval == null) {
        dval = '';
    }
    if (typeof val == 'undefined' || val == null) {
        return dval;
    } else {
        var isNum = typeof dval == 'number' || !isNaN(parseFloat(dval, 10));
        var isFloat = isNum ? ('' + dval).indexOf('.') >= 0 : false;
        if (typeof val == 'object') {
            val = typeof val.selector == 'undefined' ? (isLang ? val.lang : val.value) : (isLang ? val.prop('lang') : val.val());
        }
        if (typeof val == 'string') {
            if (isNum) {
                var num = isFloat ? parseFloat(val, 10) : parseInt(val, 10);
                return !isNaN(num) ? num : dval;
            } else {
                return val.trim() || dval;
            }
        } else {
            return dval;
        }
    }
};

module.getControlLang = function (val, dval) {
    return module.getControlValue(val, dval, true);
};

module.showDebugInfo = function (param, result) {
    if (webConfig.isDebug) {
        try {
            console.log('[Request]\n' + decodeURIComponent(param));
            console.log('[Response]\n' + result);
        } catch (e) {
            alert('[Request]\n' + decodeURIComponent(param));
            alert('[Response]\n' + result);
        }
    }
};

module.getControlValues = function (element, p) {
    var data = {}, tagPattern = /input|select|textarea/, typePattern = /text|password|checkbox/;
    if (typeof element != 'object') {
        element = document.getElementById(element);
    }
    if (element == null) {
        throw new Error('参数输入错误');
    }
    if (typeof p != 'object') {
        p = {};
    }
    //需要指定数值类型的控件ID(数值类型)列表
    p.types = p.types || { "all": "string" }; //默认取值类型为string
    //需要转换Key的控件ID(Key)列表
    p.ids = p.ids || {};
    //需要设置默认值的控件ID(及默认值)列表
    p.vals = p.vals || {};
    //需要获取lang值的控件ID列表（字符串）
    p.langs = p.langs || '';
    //需要排除的控件name列表（字符串）
    p.excludes = p.excludes || '';
    //验证回调函数
    p.validate = p.validate || null;
    //提示信息回调函数
    p.prompt = p.prompt || null;

    if (typeof p.removePrefix == 'undefined') {
        p.removePrefix = true;
    }
    if (typeof p.checkNumber == 'undefined') {
        p.checkNumber = true;
    }
    if (typeof p.defaultValue == 'undefined') {
        p.defaultValue = true;
    }
    var needValidate = typeof p.validate == 'function';

    var arr = element.getElementsByTagName("*");
    for (var i = 0, c = arr.length; i < c; i++) {
        var obj = arr[i], t = obj.tagName.toLowerCase();
        if (tagPattern.test(t) && (t != 'input' || typePattern.test(obj.type))) {
            var id = obj.id || '', name = obj.name || '';
            //只处理有ID的(并且未被排除name的)控件
            if (id.replace(/(^[\s]*)|([\s]*$)/g, '').length > 0 && !_check(p.excludes, name)) {
                var dataType = p.types['all'] || (_check(p.types['int'], id) ? 'int' : _check(p.types['float'], id) ? 'float' : 'string');
                var val = (_check(p.langs, id) ? obj.lang : obj.value).replace(/(^[\s]*)|([\s]*$)/g, '');
                var dval = p.vals[id] || null;
                var key = p.ids[id] || _removePrefix(id, p);
                if (dataType != 'string') {
                    switch (dataType) {
                        case 'int': val = parseInt(val, 10); break;
                        case 'float': val = parseFloat(val, 10); break;
                    }
                    val = isNaN(val) && p.defaultValue ? (dval || 0) : val;
                    if (p.checkNumber && isNaN(val)) {
                        if (typeof p.prompt == 'function') {
                            p.prompt('数字格式输入错误');
                        } else {
                            alert('数字格式输入错误');
                        }
                        obj.focus();
                        return false;
                    }
                }
                //没有指定默认值的控件需要验证
                if (dval == null && needValidate) {
                    var vr = p.validate(obj, key, val);
                    if (typeof vr == 'boolean' && !vr) {
                        obj.focus();
                        return false;
                    }
                }
                data[key] = val;
            }
        }
    }
    function _removePrefix(key, p) {
        if (p.removePrefix) {
            switch (p.prefix) {
                case '_': //下划线模式，如txt_Name
                    var pos = key.indexOf('_');
                    return key.substr(pos >= 0 ? pos + 1 : 0);
                    break;
                case 'aA': //大小写模式，如：txtName
                default:
                    return key.replace(/^[a-z]+/g, '');
                    break;
            }
        }
        return key;
    }
    function _check(str, key) {
        return (str || '').length > 0 ? (',' + str + ',').indexOf(',' + key + ',') >= 0 : false;
    }
    return data;
};