//是否启用标签页
var isMulti = true;
//是否允许关闭第一个加载的选项卡
var allowFirstClose = false;
//最少保留多少个Tab标签页
var minCount = 0;
//最多显示多少个Tab标签页
var maxCount = 20;
//标签页
var tabs = [];
//当前标签
var curTab = 'home';
//已经关闭的标签页
var closedTabs = [];
var cmenu = null;
var timer = null;
var interval = null;
var boxSize = {};
var strTabPageLabel = '标签页';
var strPageLabel = '标签页';

var tabpanelBoxLeft = 0;
var tabpanelBoxWidth = 0;
var tabpanelId = '#tabpanel';


var leftWidth = leftWidthConfig = 180;
page.setSize({
    paddingTop: 0,
    leftWidth: leftWidth,
    leftWidthConfig: leftWidthConfig
});

$(window).load(function () {
    page.initialForm();
    page.setBodySize();
});

function setBodySize() {
    var bs = page.getSize();
    $('#treebox').height(bs.height - $('#bodyLeft .titlebar').outerHeight());

    page.tab.setBoxSize();
}

function setBoxSize() {
    //page.tab.setBoxSize();
}

function initialForm() {    
    $('#bodyLeft').html([
        '<div class="titlebar"><div class="title">操作菜单</div></div>',
        '<div id="treebox" class="treebox"><div id="tree" class="tree"></div></div>'
    ].join(''));

    page.tab.initialForm();

    getModuleMenu();
}

function getModuleMenu() {
    var param = { ParentCode: 'Water', GetSubset: 1,IncludeSelf:0 };
    var urlparam = 'action=getModuleMenuTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam,
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showModuleMenu);
        }
    });
}

function showModuleMenu(jsondata, param) {
    var tabs = [];

    page.tree = new oTree('page.tree', $I('tree'), {
        loadedCallback: function (objTree) {
            objTree.openAll();

            for (var i in tabs) {
                page.tab.loadModule(tabs[i]);
                page.tree.select(tabs[i].id);
            }
        }
    });

    for (var i in jsondata.tree) {
        var dr = jsondata.tree[i];
        var icon = '';
        var callback = dr.url.length > 0 ? function (param, objTree) { page.tab.loadModule(param); } : null;
        page.tree.add({
            id: dr.id, name: dr.name, pid: dr.pid, icon: icon, url: dr.url, callback: callback, param: {
                url: dr.url, name: dr.name, id: dr.id, code: dr.code || dr.id
            }
        });
        if (dr.open == 1 && dr.url.length > 0) {
            tabs.push(dr);
        }
    }
}