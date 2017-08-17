var PageName = '通道-参数配置';
var ChannelNo = module.getControlValue($I('txtId'), 0);

var treeTable = null;
var fixedTable = null;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {

}

function initialForm() {
    var html = [
        '<div class="title">', page.buildTitle(PageName), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    getParamList(function () {
        $('#formBottom').append([
            '<a class="btn btnform" onclick="formSubmit();"><span>确认，提交</span></a>',
            '<a class="btn btnform" style="margin:0 6px;" onclick="recoveryChannelParam();"><span>重置</span></a>',
            '<a class="btn btnform" onclick="closePage();"><span>关闭</span></a>'
        ].join(''));
    });
}

function getParamList(callback) {
    var param = { ChannelNo: ChannelNo, ParamType: 0 };
    var urlparam = 'action=getChannelParamConfig&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
        data: urlparam, dataType: 'json',
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showParamList);
            if (page.isFunction(callback)) {
                callback();
            }
        }
    });
}

function showParamList(jsondata, param) {
    var obj = $('#formContent');
    var arr = {};

    obj.append('<input type="hidden" id="txtParamIdList" />');
    obj.append('<input type="hidden" id="txtParamIdListCache" />');

    $('#lblChannelName').html('通道' + jsondata.data.channel.ChannelNo);

    var oper = [
        '<div style="float:right;">',
        '<a class="btn btnc22" onclick="selectParam(1);" style="float:left;margin-right:3px;"><span>全选</span></a>',
        '<a class="btn btnc22" onclick="selectParam(2);" style="float:left;margin-right:3px;"><span>取消</span></a>',
        '<a class="btn btnc22" onclick="selectParam(3);" style="float:left;"><span>反选</span></a>',
        '</div>'
    ].join('');

    var html = [
        '<table cellpadding="0" cellspacing="0" class="tblist" id="tbList" style="display:none;">',
        '<tr style="background:#f8f8f8;"><td style="min-width:150px;">%s参数</td><td style="width:65px;">分类</td><td>参数说明</td></tr>'.format([
            oper
        ])
    ];    
        
    for (var i = 0, c = jsondata.data.list.length; i < c; i++) {
        var dr = jsondata.data.list[i];
        var chb = '<label class="chb-label-nobg"><input type="checkbox" id="chbParam_%s" name="chbParam"class="chb" value="%s" onclick="setParam();" />'.format([dr.ParamId, dr.ParamId]);
        html.push('<tr><td>%s%s</td><td>%s</td><td>%s</td></tr>'.format([
            chb, dr.ParamName, page.parseOption(['通道参数', '设备参数'], dr.ParamType), '<div class="con">' + dr.ParamDesc + '<div>'
        ]));
    }
    html.push('</table>');
    obj.append(html.join(''));

    var ids = [];
    for (var i = 0, c = jsondata.data.config.length; i < c; i++) {
        var dr = jsondata.data.config[i];
        $('#chbParam_%s'.format([dr.ParamId])).attr('checked', 'checked');
        ids.push(dr.ParamId);
    }
    $('#tbList').show();

    $('#txtParamIdList').attr('value', ids.join(','));
    $('#txtParamIdListCache').attr('value', ids.join(','));

    cms.util.setCheckBoxCheckedColor('chbParam');
}

function selectParam(oper) {
    cms.util.selectCheckBox('chbParam', oper);

    setParam();
}

function setParam() {
    $('#txtParamIdList').attr('value', cms.util.getCheckBoxCheckedValue('chbParam', ','));
    cms.util.setCheckBoxCheckedColor('chbParam');
}

function recoveryModulePermission() {
    //先取消全部
    cms.util.selectCheckBox('chbParam', 2);
    //选中指定的
    cms.util.setCheckBoxChecked('chbParam', ',', $('#txtParamIdListCache').val());

    setParam();
}

function closePage() {
    page.cancelConfirm(function () {
        try {
            if (page.isChild() && typeof parent.setConfigCallback == 'function') {
                parent.setConfigCallback(true, false);
            }
        } catch (e) { }
    }, false);
}

function formSubmit() {
    var formData = {
        ChannelNo: module.getControlValue($I('txtId'), 0),
        ParamIdList: module.getControlValue($I('txtParamIdList'))
    };

    if (formData.ChannelNo == 0) {
        alert('未指定通道');
        return false;
    }
    page.submitConfirm(function () {
        var urlparam = 'action=editChannelParam&data=' + encodeURIComponent(module.toJsonString(formData));
        module.ajaxRequest({
            url: webConfig.webDir + '/ajax/sensor/sensor.aspx',
            data: urlparam, dataType: 'json',
            callback: function (data, param) {
                module.showDebugInfo(urlparam, module.toJsonString(data));
                module.ajaxResponse(data, param, function (jsondata, param) {
                    cms.box.alert({
                        html: '通道-参数配置已更新',
                        callback: function (pwobj, pwReturn) {
                            try {
                                if (page.isChild() && typeof parent.setConfigCallback == 'function') {
                                    parent.setConfigCallback(true);
                                }
                            } catch (e) { }
                        }
                    });
                });
            }
        });
    });
}