var leftWidth = leftWidthConfig = 180;
page.setSize({
    paddingTop: 0,
    leftWidth: leftWidth,
    leftWidthConfig: leftWidthConfig
});

var webDir = webConfig.webDir;
var arrMenu = [
    { id: 'SystemMenu', name: '系统管理' },
    { pid: 'SystemMenu', id: 'Module', name: '模块管理', url: '/modules/systems/module/moduleList.aspx' },
    { pid: 'SystemMenu', id: 'Permission', name: '权限管理', url: '/modules/systems/permission/permissionList.aspx' },
    { pid: 'SystemMenu', id: 'Role', name: '角色管理', url: '/modules/systems/role/roleList.aspx' },
    { pid: 'SystemMenu', id: 'Menu', name: '导航菜单管理', url: '/modules/systems/menu/menuList.aspx' },
    { pid: 'SystemMenu', id: 'ModuleMenu', name: '模块菜单管理', url: '/modules/systems/menu/moduleMenuList.aspx' }
];

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

    for (var i in arrMenu) {
        if (typeof arrMenu[i].url == 'undefined') {
            arrMenu[i].url = '';
        }
    }

    showModuleMenu(arrMenu, {});
}

function getModuleMenu() {
    var param = { ParentCode: 'Water', GetSubset: 1, IncludeSelf: 0 };
    var urlparam = 'action=getUserModuleMenuTree&data=' + encodeURIComponent(module.toJsonString(param));
    module.ajaxRequest({
        url: webConfig.webDir + '/ajax/system/system.aspx',
        data: urlparam,
        callback: function (data, param) {
            module.showDebugInfo(urlparam, module.toJsonString(data));
            module.ajaxResponse(data, param, showModuleMenu);
        }
    });
}

function showModuleMenu(list, param) {
    var tabs = [];

    page.tree = new oTree('page.tree', $I('tree'), {
        showLine: true,
        loadedCallback: function (objTree) {
            objTree.openAll();

            for (var i in tabs) {
                page.tab.loadModule(tabs[i]);
                page.tree.select(tabs[i].id);
            }
        }
    });

    for (var i in list) {
        var dr = list[i];
        var icon = dr.url ? 'page.gif' : '';
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