var module = module || {};
module.tree = module.tree || {};
module.tree.arrData = [];
module.tree.nodeid = 0;
module.tree.arrDefault = [];

module.tree.buildMenuTree = function (treeData, funcName) {
    o = new oTree('o', $I('tree'), {
        showIcon: true,
        showLine: true,
        showSubNode: false,
        skin: 'default',
        isClearHtml: true,
        complete: module.tree.loadCallback
    });

    var arrMenu = treeData;
    for (var i = 0, c = arrMenu.length; i < c; i++) {
        var node = arrMenu[i];

        module.tree.addNode(i, node);
    }
};

module.tree.addNode = function (i, node) {
    if (typeof node.name != 'undefined') {
        if (typeof node.id == 'undefined') {
            node.id = 'n' + i;
        }
        var hasUrl = node.url != undefined && node.url != '';
        var callback = hasUrl ? function (param) { module.config.treeAction(param); } : null;
        var param = { type: 'menu', action: node.id, url: node.url, name: node.name };
        var icon = node.icon || (node.isModel ? '' : (hasUrl ? 'page.gif' : ''));

        if (1 == node.open) {
            module.tree.arrDefault.push([node.id, param]);
        }
        o.add({ id: node.id, pid: node.pid, name: node.name, callback: callback, param: param,
            clickToggle: 'open', icon: icon, isRoot: node.isRoot || 0 == i
        });
    }
};

module.tree.loadCallback = function (objTree) {
    if (objTree.loadTimes <= 1) {
        var arr = module.tree.arrDefault;
        for (var i in arr) {
            o.select(arr[i][0]);
            module.config.treeAction(arr[i][1]);
        }
        var strHtml = '<span style="float:right;margin:1px 3px 0 0;">'
        + '<a class="" onclick="o.expandAll();" style="margin-right:2px;"><span>展开</span></a>'
        + '<a class="" onclick="o.collapseAll();"><span>收缩</span></a>'
        + '</span>';
        $I('treetitle').innerHTML += strHtml;

        window.setTimeout(module.config.appendTreeNode, 100);
    }

    window.setTimeout(function () {
        objTree.openLevel(0);
    }, 500);
};

module.tree.buildNodeId = function (data) {
    for (var i = 0; i < data.length; i++) {

        module.tree.nodeid++;

        if (typeof data[i].id == 'undefined') {
            data[i].id = module.tree.nodeid;
        }

        if (data[i].list != undefined) {
            module.tree.buildNodeId(data[i].list);
        }
    }
    return data;
};

module.tree.buildParentId = function (data, pid) {
    for (var i = 0; i < data.length; i++) {
        data[i].pid = pid;
        var dr = data[i];

        module.tree.arrData.push({ id: dr.id, pid: dr.pid, code: dr.code, name: dr.name, url: dr.url, open: dr.open, isRoot: dr.isRoot });

        if (data[i].list != undefined) {
            module.tree.buildParentId(data[i].list, data[i].id);
        }
    }
    return module.tree.arrData;
};