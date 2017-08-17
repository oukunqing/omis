var module = module || {};
module.config = module.config || {};

var isMulti = webConfig.isPageMulti || false;
//最少保留多少个Tab标签页
var minCount = webConfig.tabMinCount || 0;
//最多显示多少个Tab标签页
var maxCount = webConfig.tabMaxCount || 10;
//标签页
var tabs = [];
//当前标签
var curTab = 'home';
//已经关闭的标签页
var closedTabs = [];
//是否启用标签页
var cmenu = null;
var timer = null;
var interval = null;
var boxSize = {};
var strTabPageLabel = '标签页';
var strPageLabel = '标签页';

var tabpanelBoxLeft = 0;
var tabpanelBoxWidth = 0;
var tabpanelId = '#tabpanel';

module.config.curSelect;
module.config.curMenu = '';
module.config.curPageUrl = '';
module.config.funcName = 'module.config.treeAction';
module.config.cmsPath = cms.util.path + '/modules';

var strMenuPath = module.config.cmsPath;
var arrMenu = [
    { id: 'Root', name: '系统管理菜单' },
    { pid: 'Root', id: 'ConfigMenu', name: '系统设置' },
    { pid: 'Root', id: 'SystemMenu', name: '系统管理' },

    { pid: 'ConfigMenu', id: 'ModuleNav', name: '模块导航管理' },
    { pid: 'ModuleNav', name: '导航菜单管理', code: 'menu', url: strMenuPath + '/system/config/menu.aspx' },
    { pid: 'ModuleNav', id: 'ModuleMenu', name: '模块菜单管理', code: 'module_menu', isModel: true, url: strMenuPath + '/system/config/moduleMenu.aspx' },

    { pid: 'ConfigMenu', id: 'Permission', name: '权限管理' },
    { pid: 'Permission', name: '权限管理', code: 'permission', url: strMenuPath + '/system/user/permission.aspx' },
    { pid: 'Permission', name: '模块管理', code: 'module', url: strMenuPath + '/system/user/module.aspx' },

    { pid: 'ConfigMenu', id: 'UserRole', name: '用户管理' },
    { pid: 'UserRole', name: '角色管理', url: strMenuPath + '/system/user/role.aspx' },
    { pid: 'UserRole', name: '用户管理', url: strMenuPath + '/system/user/user.aspx' },

    { pid: 'ConfigMenu', id: 'Dictionary', name: '分类字典管理' },

    { pid: 'SystemMenu', id: 'BasicInfo', name: '基础信息管理' },
    
    { pid: 'BasicInfo', id: 'Department', name: '部门管理' },
    { pid: 'Department', name: '部门分类管理', url: strMenuPath + '/system/basicinfo/department/departmentTypeList.aspx' },
    { pid: 'Department', name: '部门管理', url: strMenuPath + '/system/basicinfo/department/departmentList.aspx' },

    { pid: 'BasicInfo', id: 'Person', name: '人员管理' },
    { pid: 'Person', name: '人员管理', url: strMenuPath + '/system/basicinfo/person/personList.aspx' },

    { pid: 'BasicInfo', id: 'Profession', name: '专业管理' },
    { pid: 'Profession', name: '专业管理', url: strMenuPath + '/system/basicinfo/profession/professionList.aspx' },

    { pid: 'BasicInfo', id: 'Line', name: '线路管理' },
    { pid: 'Line', name: '线路分类管理', url: strMenuPath + '/system/basicinfo/line/lineTypeList.aspx' },
    { pid: 'Line', name: '线路管理', url: strMenuPath + '/system/basicinfo/line/lineList.aspx' },
    { pid: 'Line', name: '行别管理', url: strMenuPath + '/system/basicinfo/line/lineDirectionList.aspx' },

    { pid: 'BasicInfo', id: 'Section', name: '区间站场管理' },
    { pid: 'Section', name: '区间分类管理', url: strMenuPath + '/system/basicinfo/section/sectionTypeList.aspx' },
    { pid: 'Section', name: '区间站场管理', url: strMenuPath + '/system/basicinfo/section/sectionList.aspx' },
    { pid: 'Section', name: '锚段管理', url: strMenuPath + '/system/basicinfo/section/tensionLengthList.aspx' },
    { pid: 'Section', name: '股道管理', url: strMenuPath + '/system/basicinfo/section/trackList.aspx' },


    { pid: 'BasicInfo', id: 'Pillar', name: '支柱管理' },
    { pid: 'Pillar', id: 'PillarType', name: '支柱分类' },
    { pid: 'Pillar', name: '支柱管理', url: strMenuPath + '/system/basicinfo/pillar/pillarList.aspx' },

    { pid: 'BasicInfo', id: 'Substation', name: '所亭管理' },
    { pid: 'Substation', name: '所亭分类管理', url: strMenuPath + '/system/basicinfo/substation/substationTypeList.aspx' },
    { pid: 'Substation', name: '所亭管理', url: strMenuPath + '/system/basicinfo/substation/substationList.aspx' },


    { pid: 'SystemMenu', id: 'DeviceMenu', name: '设备管理' },

    { pid: 'SystemMenu', id: 'EnvironmentMenu', name: '外部环境管理' },
    { pid: 'SystemMenu', id: 'DeviceAccount', name: '设备台帐管理' },

    { pid: 'SystemMenu', id: 'ManufacturerMenu', name: '设备厂家管理' },

    { pid: 'SystemMenu', id: 'DocumentMenu', name: '文档资料管理' },

    { pid: 'SystemMenu', id: 'DatumMenu', name: '数据资料管理' },

    /*
    { pid: 'SystemMenu', id: 'Drawing', name: '图纸管理' },
    { pid: 'Drawing', name: '图纸分类', url: strMenuPath + '/system/drawing/drawingType.aspx' },
    { pid: 'Drawing', name: '图纸管理', url: strMenuPath + '/system/drawing/drawing.aspx' },
    */
    { pid: 'SystemMenu', id: 'Problem', name: '问题库配置' },
    { pid: 'Problem', name: '问题库层级', url: strMenuPath + '/problem/problemLevel.aspx' },
    { pid: 'Problem', name: '问题库分类', url: strMenuPath + '/problem/problemType.aspx' },
    { pid: 'Problem', name: '问题库来源分类', url: strMenuPath + '/problem/problemSourceType.aspx' },

    { pid: 'SystemMenu', id: 'Production', name: '生产作业配置' },
    { pid: 'Production', id: 'PlanType', name: '计划类型' },
    { pid: 'PlanType', name: '计划类型管理', url: strMenuPath + '/production/config/planTypeList.aspx' },
    { pid: 'PlanType', name: '天窗类型管理', url: strMenuPath + '/production/config/tcTypeList.aspx' },
    { pid: 'PlanType', name: '作业类型管理', url: strMenuPath + '/production/config/workTypeList.aspx' },
    { pid: 'Production', id: 'WorkProject', name: '作业项目' },
    { pid: 'WorkProject', name: '作业项目管理', url: strMenuPath + '/production/config/workProjectList.aspx' },
    { pid: 'WorkProject', name: '作业条目管理', url: strMenuPath + '/production/config/workItemList.aspx' }
];

if (loginUser.userId == 1) {
    arrMenu.push({ pid: 'DeviceMenu', name: '设备类别管理', url: strMenuPath + '/system/device/categoryList.aspx' });
    arrMenu.push({ pid: 'DocumentMenu', name: '文档类别管理', url: strMenuPath + '/system/document/categoryList.aspx' });
    arrMenu.push({ pid: 'DatumMenu', name: '数据类别管理', url: strMenuPath + '/system/datum/categoryList.aspx' });
}
arrMenu.push({ pid: 'DeviceMenu', name: '自定义参数', url: strMenuPath + '/system/device/customParamList.aspx' });

arrMenu.push({ pid: 'DeviceMenu', id: 'DeviceType', name: '设备分类管理', isModel: true });
arrMenu.push({ pid: 'DeviceMenu', id: 'DeviceManage', name: '设备管理', isModel: true });
arrMenu.push({ pid: 'DeviceAccount', id: 'DeviceAccountType', name: '设备台帐分类', url: strMenuPath + '/system/device/deviceTypeList.aspx?category=Account' });
arrMenu.push({ pid: 'DeviceAccount', id: 'DeviceAccountManage', name: '设备台帐管理'});


arrMenu.push({ pid: 'DocumentMenu', id: 'DocumentType', name: '文档分类管理', isModel: true });
arrMenu.push({ pid: 'DocumentMenu', id: 'DocumentManage', name: '文档管理', isModel: true });
arrMenu.push({ pid: 'DocumentMenu', id: 'DocumentView', name: '文档浏览', isModel: true });

arrMenu.push({ pid: 'DatumMenu', id: 'DatumType', name: '数据分类管理', isModel: true });
arrMenu.push({ pid: 'DatumMenu', id: 'Datum', name: '数据管理', isModel: true });

arrMenu.push({ pid: 'PillarType', name: '支柱分类', url: strMenuPath + '/system/device/deviceTypeList.aspx?category=Pillar' });
arrMenu.push({ pid: 'PillarType', name: '支柱用途', url: strMenuPath + '/system/device/deviceTypeList.aspx?category=PillarUse' });
arrMenu.push({ pid: 'EnvironmentMenu', name: '外部环境分类', url: strMenuPath + '/system/device/deviceTypeList.aspx?category=Environment' });
arrMenu.push({ pid: 'EnvironmentMenu', name: '外部环境管理', url: strMenuPath + '/system/device/environmentList.aspx?category=Environment' });

arrMenu.push({ pid: 'ManufacturerMenu', name: '设备厂家分类', url: strMenuPath + '/system/device/deviceTypeList.aspx?category=Manufacturer' });
arrMenu.push({ pid: 'ManufacturerMenu', name: '设备厂家管理', url: strMenuPath + '/system/device/manufacturerList.aspx?category=Manufacturer' });



arrMenu.push({ pid: 'SystemMenu', id: 'ContentMenu', name: '内容管理' });
arrMenu.push({ pid: 'ContentMenu', name: '内容类别管理', code: 'menu', url: strMenuPath + '/system/content/categoryList.aspx' });
arrMenu.push({ pid: 'ContentMenu', id: 'ContentType', name: '内容分类管理', isModel: true });
arrMenu.push({ pid: 'ContentMenu', id: 'Content', name: '内容管理', isModel: true });    

module.config.arrTreeData = arrMenu;

module.config.appendTreeNode = function () {
    window.setTimeout(function () {
        getModuleMenu();
        getDeviceMenu();
        getContentMenu();
        getDocumentMenu();
        getDatumMenu();
    }, 200);

    window.setTimeout(function () {
        getDictionaryMenu();
    }, 300);
};

function getModuleMenu() {
    module.ajaxRequest({
        url: cmsPath + '/ajax/menu.aspx',
        data: 'action=getModuleMenu',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    var pid = 0 == dr.pid ? 'ModuleMenu' : 'ModuleMenu' + dr.pid;

                    dr.url = module.config.cmsPath + '/system/config/moduleMenu.aspx?parentId=' + dr.id;
                    dr.name = '模块菜单-' + dr.name;

                    var hasUrl = dr.url != undefined && dr.url != '';
                    var callback = hasUrl ? function (param) { module.config.treeAction(param); } : null;
                    var param = { type: 'menu', action: dr.id, url: dr.url, name: dr.name };
                    var icon = hasUrl ? 'page.gif' : '';
                    o.add({ id: 'ModuleMenu' + dr.id, pid: pid, name: dr.name, callback: callback, param: param, clickToggle: 'open', icon: icon });
                }
            });
        }
    });
}

function getDeviceMenu() {
    module.ajaxRequest({
        url: cmsPath + '/ajax/menu.aspx',
        data: 'action=getDeviceMenu&contentType=1',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                var arr = [
                    ['DeviceType', '分类', module.config.cmsPath + '/system/device/deviceTypeList.aspx'],
                    ['DeviceManage', '管理', module.config.cmsPath + '/system/device/deviceList.aspx']
                ];
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    for (var j in arr) {
                        var url = arr[j][2] + '?categoryId=' + dr.id;
                        var callback = function (param) { module.config.treeAction(param); };
                        var param = { type: 'menu', action: arr[j][0] + dr.id, url: url, name: dr.name + arr[j][1] };
                        o.add({ id: arr[j][0] + dr.id, pid: arr[j][0], name: dr.name + arr[j][1], callback: callback, param: param, clickToggle: 'open', icon: 'page.gif' });
                    }
                }
            });
        }
    });
}

function getContentMenu() {
    module.ajaxRequest({
        url: cmsPath + '/ajax/menu.aspx',
        data: 'action=getCategoryMenu',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                var arr = [
                        ['ContentType', '分类', module.config.cmsPath + '/system/content/contentTypeList.aspx'],
                        ['Content', '管理', module.config.cmsPath + '/system/content/contentList.aspx']
                    ];
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    for (var j in arr) {
                        var url = arr[j][2] + '?categoryId=' + dr.id;
                        var callback = function (param) { module.config.treeAction(param); };
                        var param = { type: 'menu', action: arr[j][0] + dr.id, url: url, name: dr.name + arr[j][1] };
                        o.add({ id: arr[j][0] + dr.id, pid: arr[j][0], name: dr.name + arr[j][1], callback: callback, param: param, clickToggle: 'open', icon: 'page.gif' });
                    }
                }
            });
        }
    });
}

function getDocumentMenu() {
    module.ajaxRequest({
        url: cmsPath + '/ajax/menu.aspx',
        data: 'action=getDocumentMenu',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                var arr = [
                    ['DocumentType', '分类', module.config.cmsPath + '/system/document/documentTypeList.aspx'],
                    ['DocumentManage', '管理', module.config.cmsPath + '/system/document/documentList.aspx'],
                    ['DocumentView', '浏览', module.config.cmsPath + '/system/document/documentLibrary.aspx']                    
                ];
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    for (var j in arr) {
                        var url = arr[j][2] + '?categoryId=' + dr.id;
                        var callback = function (param) { module.config.treeAction(param); };
                        var param = { type: 'menu', action: arr[j][0] + dr.id, url: url, name: dr.name + arr[j][1] };
                        o.add({ id: arr[j][0] + dr.id, pid: arr[j][0], name: dr.name + arr[j][1], callback: callback, param: param, clickToggle: 'open', icon: 'page.gif' });
                    }
                }
            });
        }
    });
}

function getDatumMenu() {
    module.ajaxRequest({
        url: cmsPath + '/ajax/menu.aspx',
        data: 'action=getDatumMenu',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                var arr = [
                    ['DatumType', '分类', module.config.cmsPath + '/system/datum/datumTypeList.aspx'],
                    ['Datum', '管理', module.config.cmsPath + '/system/datum/datumList.aspx']
                ];
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    for (var j in arr) {
                        var url = arr[j][2] + '?categoryId=' + dr.id;
                        var callback = function (param) { module.config.treeAction(param); };
                        var param = { type: 'menu', action: arr[j][0] + dr.id, url: url, name: dr.name + arr[j][1] };
                        o.add({ id: arr[j][0] + dr.id, pid: arr[j][0], name: dr.name + arr[j][1], callback: callback, param: param, clickToggle: 'open', icon: 'page.gif' });
                    }
                }
            });
        }
    });
}

function getDictionaryMenu() {
    module.ajaxRequest({
        url: cmsPath + '/ajax/menu.aspx',
        data: 'action=getDictionaryMenu',
        callback: function (data, param) {
            module.ajaxResponse(data, param, function (jsondata, param) {
                for (var i in jsondata.list) {
                    var dr = jsondata.list[i];
                    var pid = 0 == dr.pid ? 'Dictionary' : 'Dictionary' + dr.pid;
                    var hasUrl = dr.url != undefined && dr.url != '';
                    var callback = hasUrl ? function (param) { module.config.treeAction(param); } : null;
                    var param = { type: 'menu', action: dr.id, url: dr.url, name: dr.name };
                    var icon = hasUrl ? 'page.gif' : '';
                    o.add({ id: 'Dictionary' + dr.id, pid: pid, name: dr.name, callback: callback, param: param, clickToggle: 'open', icon: icon });
                }
            });
        }
    });
}

$(window).load(function () {
    cms.frame.setFrameSize(module.frame.bodyLeftWidth, 0);

    //捕捉异常，为了跨域调用时JS代码运行正常
    try {
        if (window.location != top.window.location) {
            if (parent.hideContextMenu == 'function') {
                document.onmousedown = function () {
                    parent.hideContextMenu();
                };
            }
        }
    } catch (e) { }

    module.frame.initialForm();
    module.frame.setBodySize();
    module.frame.setFrameByShortcutKey();
    module.config.loadTreeMenu();

    module.config.initialForm();

    module.frame.setBodySize();

    cms.frame.setFrameByShortcutKey({ resizeFunc: module.frame.setBodySize, keyFunc: null, shiftKeyFunc: null });
});

$(window).resize(function(){
	module.frame.setBodySize();
});

module.config.initialForm = function () {
    $('#frmbox').html('<iframe src="" frameborder="0" scrolling="auto" width="100%" height="100%" class="frmcon" name="frmMain" id="frmMain" ></iframe>');

    if (isMulti) {
        $('#tabpanelBox').html('<div id="tabpanel" class="tabpanel nobg"></div>');
        tabpanelId = '#tabpanel';

        $('#bodyLeftSwitch').click(function () {
            cms.frame.setLeftDisplay($('#bodyLeft'), $(this));
        });
        $('#bodyLeft .titlebar .title').dblclick(function () {
            //setSwitchItem();
        });
        $('.tab-scroll-left').mousedown(function () {
            cms.util.tabScroll('sub', cms.util.$('tabpanelBox'));
        });
        $('.tab-scroll-left').mouseup(function () {
            cms.util.stopScroll();
        });
        $('.tab-scroll-right').mousedown(function () {
            cms.util.tabScroll('add', cms.util.$('tabpanelBox'));
        });
        $('.tab-scroll-right').mouseup(function () {
            cms.util.stopScroll();
        });
        showTabScrollControl(false);
    }
};

module.config.loadTreeMenu = function () {
    $('#treebox').html('<div id="tree" class="tree">正在加载，请稍候...</div>');
    module.tree.buildMenuTree(module.config.arrTreeData, module.config.funcName);
};

module.config.treeAction = function (param) {
    if (module.config.curSelect == param.action) {
        return false; //不重复加载
    }
    var url = '';
    if (param.type == 'menu') {
        //改变框架中标题栏的名称
        module.frame.setFormTitle(param.name);
        module.frame.setFormPrompt('');
        url = param.url;
    }
    if (typeof url == 'string' && url.trim().length > 0) {
        url += url.urlConnector() + new Date().getTime() + '&title=' + escape(param.name);
    }
    module.config.curPageUrl = url;

    var close = param.close == undefined ? true : param.close;

    loadPage({ code: param.action, name: param.name, url: url, close: close }, null, false);

    module.config.curSelect = param.action;
};

module.config.setFormBodySize = function (callback) {
    var formSize = module.frame.getFormSize();
    var conHeight = formSize.height - module.frame.paddingTop - module.frame.bodyBottomHeight;
    $('#divFormContent').height(conHeight);

    if (typeof callback == 'function') {
        module.config.resizeCallbackFunc = callback;
    } else if (typeof callback == 'string' && typeof eval('(' + callback + ')') == 'function') {
        module.config.resizeCallbackFunc = eval('(' + callback + ')');
    }
    if(module.config.resizeCallbackFunc != null){
        module.config.resizeCallbackFunc(formSize, conHeight);
    }
};

module.config.setFormTableStyle = function(){
    $(".tbhelp tr").each(function(){
        $(this).children("td:first").addClass('tdr');
    });
};


var setPageTitle = function(title){
    $('#bodyMain .title').html(title);
};

//page = {url:'',code:'',name:''};
var loadPage = function (page, obj, update) {
    if (typeof page != 'object') {
        var strHtml = '<div>页面参数错误。</div>';
        cms.box.alert({ title: '提示', html: strHtml });
        return false;
    }
    page.url = module.checkPageUrl(page);
    if (isMulti) {
        if (tabs.length >= maxCount && !checkPageIsLoad(page.code)) {
            var strHtml = '<div>打开的' + strPageLabel + '太多了，请关闭一些' + strPageLabel + '。同时最多只能打开' + maxCount + '个' + strPageLabel + '。</div>';
            cms.box.alert({ title: '提示', html: strHtml });
            return false;
        }
        $('#frmbox iframe').hide();
        $(tabpanelId + ' a').removeClass();
        $.each($(tabpanelId + ' a'), function () {
            $(this).addClass($(this).children('span').eq(0).prop('lang').split(',')[0] || 'tab');
        });
        //是否允许关闭
        var isClose = false;
        if (!checkPageIsLoad(page.code)) {
            tabs.push({ code: page.code, name: page.name, url: page.url, load: true, close: page.close == undefined ? true : page.close });
            //var maxlen = 20; //TAB项文字最长显示20个字符(10个汉字)
            var maxlen = webConfig.tabWorkMaxLength;
            isClose = page.close || page.close == undefined;
            var strTab = cms.jquery.buildTab(page.code, page.name, maxlen, '#frmMain_' + page.code, isClose, 'delTab(event,\'' + page.code + '\');', null,
                'buildContextMenu(event, this, \'' + page.code + '\');', page.style, '点击关闭标签');
            $(tabpanelId).append(strTab);

            $('#frmbox').append(buildIframe(page.code, page.url));
            cms.util.setWindowStatus();
            module.frame.setBoxSize();
        } else {
            isClose = $(tabpanelId + ' a[lang=' + page.code + ']').children('i').prop('class') != undefined;
            if (update) {
                updateIframe(page.code, page.url, true);
                cms.util.setWindowStatus();
            }
        }
        $('#frmMain_' + page.code).show();
        $(tabpanelId + ' a[lang=' + page.code + ']').addClass(isClose ? 'cur-c' : 'cur');
        cms.jquery.tabs('#bodyMain .tabpanel', '#frmbox', '.frmcon', 'gotoPage');
        cms.util.setWindowStatus();

        setPageTitle(page.name);

        setTabPanelSize();
        showCurrentTabItem(page.code);
    } else {
        $('#frmMain').attr('src', page.url);
        cms.util.setWindowStatus();
        setPageTitle(page.name);

        setTabPanelSize();
    }
    curTab = page.code;

    return true;
};

var showCurrentTabItem = function(tab){
    var objBox = cms.util.$('tabpanelBox');
    var distance = 0;
    var margin = 5;
    
    var boxLeft = tabpanelBoxLeft;
    var boxRight = boxLeft + tabpanelBoxWidth;
    var tabLeft = $(tabpanelId + ' a[lang=' + tab + ']').offset().left;
    var tabRight = tabLeft + $(tabpanelId + ' a[lang=' + tab + ']').width();
    
    if(tabLeft > boxRight){
        distance = tabLeft - boxLeft + margin;
    } else if(tabRight > boxRight){
        distance = tabRight - boxRight + margin;
    } else if(tabLeft < boxLeft){
        distance = - (boxLeft - tabLeft + margin);
    }
    objBox.scrollLeft += distance;
};

var setTabPanelSize = function(){
    var maxWidth = boxSize.width - 16*2;
    var tabWidth = cms.jquery.getTabWidth($(tabpanelId), maxWidth);
    $(tabpanelId).width(tabWidth);
    
    showTabScrollControl(tabWidth > maxWidth);
};

var showTabScrollControl = function(show){
    if(show){
        $('.tab-scroll-left').show();
        $('.tab-scroll-right').show();
    } else {
        $('.tab-scroll-left').hide();
        $('.tab-scroll-right').hide();
    }
};

var buildIframe = function(code, url){
    return '<iframe class="frmcon" id="frmMain_' + code + '" frameborder="0" scrolling="auto" width="100%" height="100%" src="' + url + '"></iframe>';
};

/*
isUpdate: 是否强制更新
*/
var updateIframe = function(code, url, isUpdate){
    var oldUrl = $('#frmMain_' + code).prop('src');
    //如果页面URL更新了，则重新加载页面
    if((url != undefined && url != '' && url != oldUrl.replace(cms.util.getHost(oldUrl), '')) || isUpdate){
        $('#frmMain_' + code).attr('src', url);
    }
};

var getIframeId = function(code){
    return '#frmMain_' + code;
};

var buildContextMenu = function(e, obj, tab){
    var ev = e || window.event;
    var item = [];
	var idx = 0;	
    var count = tabs.length;
	var allowClose = false;
	for(var i=count-1; i>=0; i--){
        if(tabs[i].close && tab == tabs[i].code){
	        item[idx++] = {
		        text:'关闭当前' + strTabPageLabel, func:function(){
			        delTab(ev, tab, true);
		        }
	        };
	        allowClose = true;
            break;
        }
    }
    if(allowClose){
        item[idx++] = {
            text:'separator'
        };    
    }
    item[idx++] = {
        text:'关闭全部' + strTabPageLabel, func:function(){
            for(var i=count-1; i>=0; i--){
                if(tabs[i].close){
	                delTab(ev, tabs[i].code, true);
	            }
            }
        }
    };
    
    
	if(count > 1){
	    var tabOther = 0;
	    for(var i=0; i<count; i++){
            if(tabs[i].code != tab && tabs[i].close){
                tabOther++;
                break;
	        }
        }
        if(tabOther > 0){
	        item[idx++] = {
		        text:'关闭其他' + strTabPageLabel, func:function(){
		            for(var i=count-1; i>=0; i--){
		                if(tabs[i].code != tab && tabs[i].close){
			                delTab(ev, tabs[i].code, true);
			            }
		            }
		        }
	        };
	    }
	    var tabLeft = 0;
	    var tabLeftClose = 0;
	    for(var i=0; i<count; i++){
            if(tabs[i].code == tab){
                break;
	        }
	        tabLeftClose += tabs[i].close ? 1 : 0;
	        tabLeft++;
        }
        if(tabLeftClose > 0){
	        item[idx++] = {
		        text:'关闭左侧' + strTabPageLabel, func:function(){
		            for(var i=tabLeft-1; i>=0; i--){
		                if(tabs[i].close){
			                delTab(ev, tabs[i].code, true);
			            }
		            }
		        }
	        };
	    }
	    var tabRight = 0;
	    for(var i=count-1; i>=0; i--){
            if(tabs[i].code == tab){
                break;
	        }
	        tabRight += tabs[i].close ? 1 : 0;
        }
        if(tabRight > 0){
	        item[idx++] = {
		        text:'关闭右侧' + strTabPageLabel, func:function(){
        			for(var i=count-1; i>=0; i--){
		                if(tabs[i].code == tab){
		                    break;
			            } else if(tabs[i].close){
			                delTab(ev, tabs[i].code, true);
			            }
		            }
		        }
	        };
	    }	    
	}
	
	if(count > 0){	    
        item[idx++] = {
            text:'separator'
        };
	    for(var i=count-1; i>=0; i--){
            if(tab == tabs[i].code){
	            item[idx++] = {
		            text:'重新加载', func:function(){
		                //加载初始URL
			            updateIframe(tabs[i].code, tabs[i].url, true);
			            /* 刷新(不重新加载)
			            var iframeId = getIframeId(tabs[i].code);
			            $(iframeId)[0].contentWindow.location.reload();
			            */
		            }
	            };
                break;
            }
        }
	}
	
	if(count > 1){
        item[idx++] = {
            text:'全部重新加载', func:function(){
                for(var i=count-1; i>=0; i--){
                    updateIframe(tabs[i].code, tabs[i].url, true);
                }
            }
        };	
	}
	
	item[idx++] = {
        text:'separator'
    };
	var lastClosedTab = getClosedTab();
	if(lastClosedTab != null){
        item[idx++] = {
            text:'重新打开关闭的标签页', func:function(){
                if(loadPage(lastClosedTab, obj, false)){
                    deleteClosedTab();
                }
            }
        };
        item[idx++] = {
            text:'重新打开关闭的全部标签页', func:function(){
                for(var i=0,c=closedTabs.length; i<c; i++){
                    lastClosedTab = getClosedTab();
                    if(loadPage(lastClosedTab, obj, false)){
                        deleteClosedTab();
                    } else {
                        break;
                    }
                }
            }
        }; 
	} else {
        item[idx++] = {
            text:'重新打开关闭的标签页', disabled: true
        };
        item[idx++] = {
            text:'重新打开关闭的全部标签页', disabled: true
        };
	}
	
    var config = {
	    width: 100,
	    zindex: 1000,
	    opacity: 1,
	    item: item
    };
    cmenu = new ContextMenu(e, config, obj);
};

var hideContextMenu = function(){
    if(cmenu != null){
        cmenu.Close(cmenu.id);
    }
};

var checkPageIsLoad = function(tab){
    for(var i=0; i<tabs.length; i++){
        if(tabs[i].code == tab){
            return true;
        }
    }
    return false;
};

var gotoPage = function(param){
    curTab = param.action;
    var idx = cms.jquery.getTabItem(tabs, curTab);

    setPageTitle(tabs[idx].name);
    
    showCurrentTabItem(curTab);
};

var delTab = function (ev, tab, isContextMenu) {
    if (tabs.length <= minCount) {
        cms.box.alert({ title: '提示', html: '不要全部都删了，至少保留' + minCount + '个' + strPageLabel + '吧。' });
        return false;
    }
    if (tab == undefined) tab = curTab;
    for (var i = 0; i < tabs.length; i++) {
        if (tab == tabs[i].code) {
            //记录被关闭的标签页
            closedTabs.push(tabs[i]);

            //删除数组中的标签
            tabs.splice(i, 1);
            $('#frmbox iframe').remove('.frmcon[id=frmMain_' + tab + ']');
            $('#tabpanel a').remove('[lang=' + tab + ']');
            break;
        }
    }
    if (tab == curTab) {
        for (var j = tabs.length - 1; j >= 0; j--) {
            if (!tabs[j].deleted && tab != tabs[j].code) {
                loadPage({ code: tabs[j].code, url: tabs[j].url, name: tabs[j].name }, null);
                curTab = tabs[j].code;
                break;
            }
        }
    }

    if (0 == tabs.length) {
        setPageTitle('');
    }

    setTabPanelSize();

    setNewSelectedNode(tab);

    if (!isContextMenu) {
        //阻止单击事件冒泡
        if (ev.stopPropagation) { ev.stopPropagation(); } else { ev.cancelBubble = true; }
        if (ev.preventDefault) { ev.preventDefault(); } else { ev.returnValue = false; }
    }
};

var setNewSelectedNode = function (tab) {
    var treeSelectId = o.getSelected().id;
    if (treeSelectId == tab) {
        var newTab = tabs.length > 0 ? tabs[0].code : -1;
        o.select(newTab);
        module.config.curSelect = newTab;
    }
};

//获取最后一次被删除的标签
var getClosedTab = function(){
    if(closedTabs.length > 0){
        var idx = closedTabs.length - 1;
        var tab = closedTabs[idx];
        //closedTabs.splice(idx, 1);
        
        return tab;
    }
    return null;
};

//删除被删除标签中 已经重新加载的标签页
var deleteClosedTab = function(){
    if(closedTabs.length > 0){
        var idx = closedTabs.length - 1;
        closedTabs.splice(idx, 1);
        
        return true;
    }
    return false;
};