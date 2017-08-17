var user = user || {};

user.logout = function () {
    var config = {
        id: 'pwlogout',
        title: '退出',
        html: '确定要退出系统吗？',
        callBack: user.logoutAction
    };
    cms.box.confirm(config);
};

user.logoutAction = function (pwobj, pwReturn) {
    if (pwReturn.dialogResult) {
        var urlparam = 'action=logout';
        module.ajaxRequest({
            url: module.path + '/ajax/user.aspx',
            data: urlparam,
            callBack: user.logoutCallBack,
            param: {
                pwobj: pwobj
            }
        });
    }
    pwobj.Hide();
};

user.logoutCallBack = function (data, param) {
    if (!data.isJsonData()) {
        module.showJsonErrorData(data);
        return false;
    }
    var jsondata = eval('(' + data + ')');
    if (1 == jsondata.result) {
        top.location.href = (webConfig.webDir + '/' + webConfig.loginUrl).replaceAll('//','/');
    } else if (-1 == jsondata.result) {
        if (!module.dbConnectionError(jsondata.error)) {
            module.showErrorInfo(jsondata.msg, jsondata.error);
            return false;
        }
    } else {
        module.showErrorInfo(jsondata.msg, jsondata.error);
    }
};

user.updateUserPwd = function () {
    var strHtml = '<div style="padding:5px 10px;">'
        + '<table cellpadding="0" cellspacing="0" class="tbform">'
        + '<tr><td style="width:75px;"><em>*</em>旧密码：</td><td><input type="password" class="txt pwd w180" id="txtOldPwd_PWFORM" name="txtOldPwd_PWFORM" maxlength="25" /></td></tr>'
        + '<tr><td><em>*</em>新密码：</td><td><input type="password" class="txt pwd w180" id="txtNewPwd_PWFORM" name="txtNewPwd_PWFORM" maxlength="25" /></td></tr>'
        + '<tr><td><em>*</em>确认密码：</td><td><input type="password" class="txt pwd w180" id="txtConfirmPwd_PWFORM" name="txtConfirmPwd_PWFORM" maxlength="25" /></td></tr>'
        + '</table>'
        + '</div>';
    var config = {
        id: 'pwUpdatePwd',
        title: '修改密码',
        html: strHtml,
        height: 180,
        callBack: user.updateUserPwdAction
    };
    cms.box.form(config);

    cms.util.inputControl(cms.util.$('txtOldPwd_PWFORM'), true);
    cms.util.inputControl(cms.util.$('txtNewPwd_PWFORM'), true);
    cms.util.inputControl(cms.util.$('txtConfirmPwd_PWFORM'), true);

    if (webConfig.enabledPasswordFlash) {
        $("input[type=password]").iPass();
    }
};

user.updateUserPwdAction = function (pwobj, pwReturn) {
    if (pwReturn.dialogResult) {
        var strOldPwd = $('#txtOldPwd_PWFORM').val().trim();
        var strNewPwd = $('#txtNewPwd_PWFORM').val().trim();
        var strConfirmPwd = $('#txtConfirmPwd_PWFORM').val().trim();
        if (strOldPwd.equals(String.empty)) {
            cms.box.msgAndFocus(cms.util.$('txtOldPwd_PWFORM'), { title: '提示', html: '请输入旧密码！' });
        } else if (!module.patternPwd.exec(strOldPwd)) {
            cms.box.msgAndFocus(cms.util.$('txtOldPwd_PWFORM'), { html: module.promptPwd });
        } else if (strNewPwd.equals(String.empty)) {
            cms.box.msgAndFocus(cms.util.$('txtNewPwd_PWFORM'), { title: '提示', html: '请输入新密码！' });
        } else if (!module.patternPwd.exec(strNewPwd)) {
            cms.box.msgAndFocus(cms.util.$('txtNewPwd_PWFORM'), { html: module.promptPwd });
        } else if (module.checkPwdComplexity(strNewPwd) < 3) {
            cms.box.msgAndFocus(cms.util.$('txtNewPwd_PWFORM'), { title: '提示', html: module.promptPwdSet });
        } else if (strConfirmPwd.equals(String.empty)) {
            cms.box.msgAndFocus(cms.util.$('txtConfirmPwd_PWFORM'), { title: '提示', html: '请再次输入新密码！' });
        } else if (!strConfirmPwd.equals(strNewPwd)) {
            cms.box.msgAndFocus(cms.util.$('txtConfirmPwd_PWFORM'), { title: '提示', html: '两次输入的密码不一样！' });
        } else {
            //修改密码
            var urlparam = 'action=updateUserPwd&oldPwd=' + strOldPwd + '&newPwd=' + strNewPwd;
            module.ajaxRequest({
                url: module.path + '/ajax/user.aspx',
                data: urlparam,
                callBack: user.updateUserPwdCallBack,
                param: {
                    pwobj: pwobj
                }
            });
        }
    } else {
        pwobj.Hide();
    }
};

user.updateUserPwdCallBack = function (data, param) {
    if (!data.isJsonData()) {
        module.showJsonErrorData(data);
        return false;
    }
    var jsondata = eval('(' + data + ')');
    if (1 == jsondata.result) {
        cms.box.confirm({ id: 'modifypwd', title: '提示信息', html: '密码修改成功，是否重新登录？',
            callBack: user.logoutAction,
            returnValue: {
                pwobj: param.pwobj
            }
        });
        param.pwobj.Hide();
    } else if (-1 == jsondata.result) {
        if (!module.dbConnectionError(jsondata.error)) {
            module.showErrorInfo(jsondata.msg, jsondata.error);
            return false;
        }
    } else {
        module.showErrorInfo(jsondata.msg, jsondata.error);
    }
};

user.showUserInfo = function (strUserName) {
    module.showInfoDetail('User', '用户信息', 'userName=' + strUserName, {});
    /*
    //获得用户信息
    var urlparam = 'action=getUserInfo&userName=' + (strUserName || '');
    module.ajaxRequest({
        url: module.path + '/ajax/user.aspx',
        data: urlparam,
        callBack: user.showUserInfoCallBack,
        param: null
    });
    */
};
/*
user.showUserInfoCallBack = function (data, param) {
    if (!data.isJsonData()) {
        module.showJsonErrorData(data);
        return false;
    }
    var jsondata = eval('(' + data + ')');
    if (1 == jsondata.result) {
        var user = jsondata.user;
        var strHtml = '<table cellpadding="0" cellspacing="5" style="line-height:20px;">'
            + '<tr><td>用户权限:</td><td>' + user.roleName + '</td></tr>'
            + '<tr><td>用户名:</td><td>' + user.userName + '</td></tr>'
            + '<tr><td>真实姓名:</td><td>' + user.realName + '</td></tr>'
            + '<tr><td>登录次数:</td><td>' + user.loginTimes + '</td></tr>'
            + '<tr><td>最后登录时间:</td><td>' + user.lastLoginTime + '</td></tr>'
            + '<tr><td>最后登录IP:</td><td>' + user.lastLoginIp + '</td></tr>'
            + '<tr><td>修改密码时间:</td><td>' + user.updatePwdTime + '</td></tr>'
            + '</table>';
        var config = {
            id: 'pwUserInfo',
            title: '用户帐户信息',
            html: strHtml,
            width: 360,
            height: 240,
            noBottom: true
        };
        cms.box.win(config);
    } else if (-1 == jsondata.result) {
        if (!module.dbConnectionError(jsondata.error)) {
            module.showErrorInfo(jsondata.msg, jsondata.error);
            return false;
        }
    } else {
        module.showErrorInfo(jsondata.msg, jsondata.error);
    }
};
*/