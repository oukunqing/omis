var page = page || {};
page.photo = page.photo || {
    dir: webConfig.webDir,
    name: '图片',
    pwUpload: null,
    photoCount: 0,
    maxCount: -1,   //图片数量最大值，-1表示不限制数量
    updateCallback: null,
    photoSize: { width: 100, height: 100 }
};

page.photo.initial = function (p) {
    p = p || {};
    if (typeof p.name == 'string' && p.name.length > 0) {
        page.photo.name = p.name;
    }
    if (typeof p.updateCallback == 'function') {
        page.photo.updateCallback = p.updateCallback;
    }
    if (!typeof p.photoSize == 'undefined') {
        page.photo.photoSize = p.photoSize;
    }
    if (typeof p.maxCount == 'number' || typeof p.max == 'number') {
        page.photo.maxCount = p.maxCount || max || -1;
    }
};

page.photo.uploadPhoto = function (dir, type, p) {
    p = p || {};
    var par = 'w={0}&h={1}&tw={2}&th={3}&mode={4}&deleteSource={5}&name={6}'.format2([
        p.width || p.w || 400, p.height || p.h || 400, p.thumbWidth || p.tw || 200, p.thumbHeight || p.th || 200,
        p.mode || 'HW', p.deleteSource || p.delSource ? 1 : 0, p.name || page.photo.name || ''
    ]);
    var data = module.toJsonString(par);
    var url = webConfig.webDir + '/modules/upload/uploadPhoto.aspx?';
    var urlparam = 'dir={0}&type={1}&title={2}&{3}'.format2([dir, type, p.title || '', par]);

    page.photo.pwUpload = cms.box.winform({
        id: 'pwUpload',
        title: '上传' + page.photo.name,
        html: url + urlparam,
        width: 500,
        height: 320
    });
};

function uploadPhotoCallback(filePath) {
    page.photo.uploadPhotoCallback(filePath);
}

page.photo.uploadPhotoCallback = function (filePath) {
    page.photo.showPhoto(filePath);

    var fileName = filePath.getFileName();
    $('#lblPhotoUpload').html('{0}（{1}）上传成功!{2}'.format2([page.photo.name, fileName, '请保存!']));

    page.photo.getPhotoList(true);

    page.photo.pwUpload.Hide();
};

page.photo.showPhoto = function (filePath) {
    if (filePath.length == 0) {
        return false;
    }
    var _ps = page.photo.photoSize;
    var _pc = page.photo.photoCount;
    var _dir = page.photo.dir;
    var _css = 'width:204px;height:' + (_ps.height + 24) + 'px;display:block;float:left;background:#f8f8f8;border:solid 1px #ddd; margin:0 10px 10px 0;';

    var paths = filePath.split('|');
    var html = [];
    for (var i = 0, c = paths.length; i < c; i++) {
        if (paths[i].length > 0) {
            var _p = paths[i];
            var _path = _dir + _p;
            var _name = _p.getFileName();
            html.push([
                '<div id="divPhotoBox_' + _pc + '" style="' + _css + '">',
                '<div style="height:', _ps.height, 'px;overflow:hidden;">',
                '<img src="', _path, '" lang="', _p, '" ',
                ' onload="module.setImageSize(this,', _ps.width, ',', _ps.height, ');" ',
                ' name="imgPhoto" id="imgPhoto_', _pc, '" style="width:', _ps.width, 'px;padding:2px;" />',
                '</div>',
                '<div style="height:24px;padding:0 5px;">',
                '<a onclick="page.photo.deletePhoto(', _pc, ');" style="float:right;">删除</a>', _name,
                '</div>',
                '</div>'
            ].join(''));

            _pc++;
        }
    }
    page.photo.photoCount = _pc;

    $('#divPhotoList').append(html.join(''));

    page.photo.setUploadEnabled(page.photo.maxCount, paths.length);
};

page.photo.setUploadEnabled = function (mc, c) {
    if (mc > 0) {
        $('#btnUploadPhoto').css('display', c >= mc ? 'none' : '');
    }
};

page.photo.getPhotoList = function (isUpdate) {
    var arrObj = $N('imgPhoto');
    var arrName = [];
    for (var i = 0; i < arrObj.length; i++) {
        arrName.push(arrObj[i].lang.trim());
    }
    page.photo.setUploadEnabled(page.photo.maxCount, arrObj.length);

    var photos = arrName.join('|');
    cms.util.setControlValue($I('txtPhoto'), photos);

    if (isUpdate) {
        if (typeof page.photo.updateCallback == 'function') {
            page.photo.updateCallback(photos);
        }
    }
};

page.photo.deletePhoto = function (pid) {
    cms.box.confirm({
        id: 'pwDelPhoto',
        title: '提示信息',
        html: '删除后不可恢复，确定要删除{0}吗？'.format2(page.photo.name),
        callback: function (pwobj, pwReturn) {
            if (pwReturn.dialogResult) {
                var path = $.trim($('#imgPhoto_' + pid).prop('lang'));
                module.ajaxRequest({
                    url: page.photo.dir + '/modules/upload/uploadPhoto.aspx',
                    data: 'action=deletePhoto&filePath=' + path,
                    callback: function (data, param) {
                        module.ajaxResponse(data, param, function (jsondata, param) {
                            var box = $I('divPhotoBox_' + pid);

                            if (box != null) {
                                $I('divPhotoList').removeChild(box);
                            }

                            $('#lblPhotoUpload').html('{0}（{1}）已删除!'.format2([page.photo.name, path.getFileName()]));

                            page.photo.getPhotoList(true);
                        });
                    }
                });
            }
        }
    });
};

page.photo.deleteTempPhoto = function (path) {
    module.ajaxRequest({
        url: page.photo.dir + '/modules/upload/uploadPhoto.aspx',
        data: 'action=deletePhoto&filePath=' + path,
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {

            });
        }
    });
};

page.photo.showUpdatePrompt = function (msg) {
    $('#lblPhotoUpdate').html(msg || (page.photo.name + '已更新! [' + new Date().toString() + ']'));
};

page.photo.buildForm = function (o, p, f) {
    f = f || {};
    var html = [
        '<input type="hidden" id="', (f.id || 'txtPhoto'), '" style="width:600px;" />',
        '<div id="divPhotoList" style="clear:both; overflow:hidden;"></div>',
        '<div style="clear:both; overflow:hidden;">',
        '<a id="btnUploadPhoto" class="btn btnc26" style="float:left;margin-right:10px;"><span class="w65">', (f.name || '上传图片'), '</span></a>',
        '<span id="lblPhotoUpload" style="float:left;margin-right:10px;"></span>',
        '<span id="lblPhotoUpdate" style="float:left;"></span>',
        '</div>'
    ].join('');

    o.append(html);

    $('#btnUploadPhoto').click(function () {
        page.photo.uploadPhoto(p.dir, p.type || 'photo', p);
    });
};