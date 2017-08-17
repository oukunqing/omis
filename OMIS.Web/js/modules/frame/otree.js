var module = module || {};
module.tree = module.tree || {};

module.tree.arrDefault = [];
module.tree.dir = webConfig.webDir;

module.tree.buildMenuTree = function (jsonTreeData) {
    var isSimple = jsonTreeData.simple == 1;
    var arrMenu = jsonTreeData.list;

    o = new oTree('o', $I('tree'), {
        showIcon: true,
        showLine: true,
        showSubNode: false,
        skin: 'default',
        isClearHtml: true,
        complete: module.tree.loadCallback
    });

    if (isSimple) {
        for (var i = 0, c = arrMenu.length; i < c; i++) {
            var dr = arrMenu[i];
            var node = { id: dr[0], pid: dr[1], level: dr[2], name: dr[3], url: dr[4], type: dr[5], pic: dr[6], count: dr[7], open: dr[8]};

            module.tree.addNode(i, node);
            
        }
    } else {
        for (var i = 0, c = arrMenu.length; i < c; i++) {
            var node = arrMenu[i];

            module.tree.addNode(i, node);
        }
    }
};

module.tree.addNode = function (i, node) {
    if (i > 0) {
        var isPage = node.count == 0 && node.type == 0;
        var callback = node.url != '' || isPage ? function (param) { module.config.treeAction(param); } : null;
        var param = { type: 'menu', action: node.id, url: node.url, name: node.name };
        var icon = node.pic != '' ? module.tree.dir + node.pic : (isPage ? 'page.gif' : '');
        if (1 == node.open) {
            module.tree.arrDefault.push([node.id, param]);
        }
        o.add({ id: node.id, pid: node.pid, name: node.name, callback: callback, param: param,
            clickToggle: 'open', icon: icon
        });
    } else {
        o.root({ id: node.id, name: node.name });
    }
};

module.tree.loadCallback = function () {
    //o.collapseAll();
    o.openLevel(0);

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
};