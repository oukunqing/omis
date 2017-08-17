var PageName = '角色';
var DataId = 0;

$(window).ready(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {

    $('#tabContent').height(page.formSize.height - $('.tabpanel').outerHeight());
}

function getDataId() {
    DataId = module.getControlValue($I('txtId'), 0);
    return DataId;
}

function initialForm() {
    //cms.jquery.tabs('.tabpanel', '#tabContent', '.tabcon', '');
    cms.jquery._tabs('#tabItem', '#tabContent', '.tabcon', '');

    var html = [
        '<div class="title">', page.buildTitle(PageName, getDataId()), '</div>',
        '<div class="tools">', page.buildReload(), '</div>'
    ];
    $('#bodyTitle').append(html.join(''));

    var isAdd = DataId <= 0;
    page.buildContinue(isAdd);
    page.buildFormAction(isAdd);


    $('.select').width($('#txtName').outerWidth());
}

function formCancal(isLoad) {
    return page.editCancel(isLoad);
}

function formSubmit() {
    
}

function getData(func) {
    if (getDataId() <= 0) {
        return false;
    }
    
}
