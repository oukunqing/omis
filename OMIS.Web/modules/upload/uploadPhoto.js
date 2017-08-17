$(window).load(function () {
    var html = [
        '<div class="title">', page.buildTitle(title), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));
});

function formSubmit() {
    $('#btnUpload').click();
}

function uploadCallback(filePath) {
    try {
        if (typeof parent.uploadPhotoCallback == 'function') {
            parent.uploadPhotoCallback(filePath);
        } else {
            cms.box.alert({
                title: '提示信息',
                html: name + '上传成功',
                callback: function (pwo, pwr) {
                    location.href = location.href;
                }
            });
        }
    } catch (e) { }
}

function showErrorInfo(info) {
    cms.box.alert(info);
}