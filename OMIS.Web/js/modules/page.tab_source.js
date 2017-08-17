var page = page || {};
page.tab = page.tab || {
    isMulti: true,  //是否启用标签页
    allowFirstClose: false,  //是否允许关闭第一个加载的选项卡
    minCount: 0,
    maxCount: 20,
    tabs: [],   //标签页列表
    curTab: '', //当前标签
    closedTabs: [], //已经关闭的标签页
    cmenu: null, timer: null, interval: null,
    boxSize: {},
    tabPageLabel: '标签页', pageLabel: '标签页',
    tabWorkMaxLength: 20,    //标签TAB文字长度
    tabpanelBoxLeft: 0, tabpanelBoxWidth: 0, tabpanelId: '#tabpanel',
    isInitial: false
};

page.tab.initialForm = function (p) {
    p = p || {};
    var par = { tabBoxId: p.tabBoxId || 'bodyTitle', conBoxId: p.conBoxId || 'bodyContent' };
    $('#' + par.tabBoxId).html([
        '<div class="titlebar">',
        '<a class="tab-scroll-left"></a>',
        '<div id="tabpanelBox" class="tabpanelbox"><div id="tabpanel" class="tabpanel"></div></div>',
        '<a class="tab-scroll-right"></a>',
        '</div>'
    ].join(''));
    $('#' + par.conBoxId).append('<div id="frmbox" class="frmbox"></div>');

    page.tab.tabpanelId = '#tabpanel';
    $('.tab-scroll-left').mousedown(function () {
        cms.util.tabScroll('sub', $I('tabpanelBox'));
    });
    $('.tab-scroll-left').mouseup(function () {
        cms.util.stopScroll();
    });
    $('.tab-scroll-right').mousedown(function () {
        cms.util.tabScroll('add', $I('tabpanelBox'));
    });
    $('.tab-scroll-right').mouseup(function () {
        cms.util.stopScroll();
    });
    page.tab.showTabScrollControl(false);

    page.tab.isInitial = true;
};

page.tab.buildIframe = function (code, url) {
    return '<iframe class="frmcon" style="display:block;" id="frmMain_' + code + '" frameborder="0" scrolling="auto" width="100%" height="100%" src="' + url + '"></iframe>';
};

page.tab.loadModule = function (param, isClose) {
    if (isClose == undefined) {
        isClose = true;
    }
    $('#nav ul li').removeClass();
    $('#menu_' + param.code).parent().addClass('cur');

    var isFullPath = /^(http[s]?:\/\/)/i.test(param.url);
    var url = (!isFullPath ? webConfig.webDir : '') + param.url;
    page.tab.loadPage({ url: url, code: param.code || param.id, name: param.name, close: isClose }, null);
};

page.tab.loadPage = function (param, obj, update) {
    var _ = this;
    if (typeof param != 'object') {
        cms.box.alert({ title: '提示', html: '<div>页面参数错误。</div>' });
        return false;
    }
    var url = param.url;

    if (typeof url == 'string') {
        url += url.urlConnector() + new Date().getTime();
    }

    if (_.isMulti) {
        if (_.tabs.length >= _.maxCount && !_.checkPageIsLoad(param.code)) {
            var html = '<div>打开的' + _.pageLabel + '太多了，请关闭一些' + _.pageLabel + '。同时最多只能打开' + _.maxCount + '个' + _.pageLabel + '。</div>';
            cms.box.alert({ title: '提示', html: html });
            return false;
        }
        $('#frmbox iframe').hide();
        $(_.tabpanelId + ' a').removeClass();
        $.each($(_.tabpanelId + ' a'), function () {
            $(this).addClass($(this).children('span').eq(0).prop('lang').split(',')[0] || 'tab');
        });
        //是否允许关闭
        var isClose = false;
        if (!_.checkPageIsLoad(param.code)) {
            _.tabs.push({ code: param.code, name: param.name, url: param.url, load: true, close: param.close == undefined ? true : param.close });
            //var maxlen = 20; //TAB项文字最长显示20个字符(10个汉字)
            var maxlen = _.tabWorkMaxLength;
            isClose = param.close || param.close == undefined;
            var tab = cms.jquery.buildTab(param.code, param.name, maxlen, '#frmMain_' + param.code, isClose, 'page.tab.delTab(event,\'' + param.code + '\');', null,
                'page.tab.buildContextMenu(event, this, \'' + param.code + '\');', param.style, '点击关闭标签');
            $(_.tabpanelId).append(tab);

            $('#frmbox').append(_.buildIframe(param.code, param.url));
            cms.util.setWindowStatus();
            _.setBoxSize();
        } else {
            isClose = $(_.tabpanelId + ' a[lang=' + param.code + ']').children('i').prop('class') != undefined;
            if (update) {
                _.updateIframe(param.code, param.url, true);
                cms.util.setWindowStatus();
            }
        }
        $('#frmMain_' + param.code).show();
        $(_.tabpanelId + ' a[lang=' + param.code + ']').addClass(isClose ? 'cur-c' : 'cur');
        cms.jquery.tabs('.titlebar .tabpanel', '#frmbox', '.frmcon', 'page.tab.gotoPage');
        cms.util.setWindowStatus();

        _.setTabPanelSize();
        _.showCurrentTabItem(param.code);
    } else {
        $('#frmMain').attr('src', url);
        cms.util.setWindowStatus();

        _.setPageTitle(param.name);

        _.setBoxSize();

        _.setTabPanelSize();
    }
    _.curTab = param.code;

    return true;
};

page.tab.setPageTitle = function (title) {
    $('#bodyTitle .title').html(title);
};

page.tab.showCurrentTabItem = function(tab) {
    var objBox = cms.util.$('tabpanelBox');
    var distance = 0;
    var margin = 5;

    var boxLeft = page.tab.tabpanelBoxLeft;
    var boxRight = boxLeft + page.tab.tabpanelBoxWidth;
    var tabLeft = $(page.tab.tabpanelId + ' a[lang=' + tab + ']').offset().left;
    var tabRight = tabLeft + $(page.tab.tabpanelId + ' a[lang=' + tab + ']').width();

    if (tabLeft > boxRight) {
        distance = tabLeft - boxLeft + margin;
    } else if (tabRight > boxRight) {
        distance = tabRight - boxRight + margin;
    } else if (tabLeft < boxLeft) {
        distance = -(boxLeft - tabLeft + margin);
    }
    objBox.scrollLeft += distance;
};


page.tab.setBoxSize = function () {
    if (!page.tab.isInitial) {
        return false;
    }
    var fs = page.formSize;
    page.tab.tabpanelBoxWidth = fs.width - 2 - 16 * 2;
    $('#tabpanelBox').width(page.tab.tabpanelBoxWidth);
    tabpanelBoxLeft = $('#tabpanelBox').offset().left;

    page.tab.setTabPanelSize();

    $('#frmbox .frmcon').width(fs.width);
    $('#frmbox .frmcon').height(fs.height);
    $('#frmbox').height(fs.height);
};

page.tab.setTabPanelSize = function () {
    var maxWidth = page.formSize.width - 2 - 16 * 2;
    var tabWidth = cms.jquery.getTabWidth($(page.tab.tabpanelId), maxWidth);
    $(page.tab.tabpanelId).width(tabWidth);

    page.tab.showTabScrollControl(tabWidth > maxWidth);
};

page.tab.showTabScrollControl = function (show) {
    if (show) {
        $('.tab-scroll-left').show();
        $('.tab-scroll-right').show();
    } else {
        $('.tab-scroll-left').hide();
        $('.tab-scroll-right').hide();
    }
};



/*
isUpdate: 是否强制更新
*/
page.tab.updateIframe = function (code, url, isUpdate) {
    var oldUrl = $('#frmMain_' + code).prop('src');
    //如果页面URL更新了，则重新加载页面
    if ((url != undefined && url != '' && url != oldUrl.replace(cms.util.getHost(oldUrl), '')) || isUpdate) {
        $('#frmMain_' + code).attr('src', url);
    }
};

page.tab.getIframeId = function (code) {
    return '#frmMain_' + code;
};

page.tab.buildContextMenu = function (e, obj, tab) {
    var _ = page.tab;
    var ev = e || window.event;
    var item = [];
    var idx = 0;
    var allowClose = false;
    var count = _.tabs.length;

    for (var i = count - 1; i >= 0; i--) {
        if (_.tabs[i].close && tab == _.tabs[i].code) {
            item[idx++] = {
                text: '关闭当前' + _.tabPageLabel, func: function () {
                    _.delTab(ev, tab, true);
                }
            };
            allowClose = true;
            break;
        }
    }
    if (allowClose) {
        item[idx++] = {
            text: 'separator'
        };
    }
    item[idx++] = {
        text: '关闭全部' + _.tabPageLabel, func: function () {
            for (var i = count - 1; i >= 0; i--) {
                if (_.tabs[i].close) {
                    _.delTab(ev, _.tabs[i].code, true);
                }
            }
        }
    };

    if (count > 1) {
        var tabOther = 0;
        for (var i = 0; i < count; i++) {
            if (_.tabs[i].code != tab && _.tabs[i].close) {
                tabOther++;
                break;
            }
        }
        if (tabOther > 0) {
            item[idx++] = {
                text: '关闭其他' + _.tabPageLabel, func: function () {
                    for (var i = count - 1; i >= 0; i--) {
                        if (_.tabs[i].code != tab && _.tabs[i].close) {
                            _.delTab(ev, _.tabs[i].code, true);
                        }
                    }
                }
            };
        }
        var tabLeft = 0;
        var tabLeftClose = 0;
        for (var i = 0; i < count; i++) {
            if (_.tabs[i].code == tab) {
                break;
            }
            tabLeftClose += _.tabs[i].close ? 1 : 0;
            tabLeft++;
        }
        if (tabLeftClose > 0) {
            item[idx++] = {
                text: '关闭左侧' + _.tabPageLabel, func: function () {
                    for (var i = tabLeft - 1; i >= 0; i--) {
                        if (_.tabs[i].close) {
                            _.delTab(ev, _.tabs[i].code, true);
                        }
                    }
                }
            };
        }
        var tabRight = 0;
        for (var i = count - 1; i >= 0; i--) {
            if (_.tabs[i].code == tab) {
                break;
            }
            tabRight += _.tabs[i].close ? 1 : 0;
        }
        if (tabRight > 0) {
            item[idx++] = {
                text: '关闭右侧' + _.tabPageLabel, func: function () {
                    for (var i = count - 1; i >= 0; i--) {
                        if (_.tabs[i].code == tab) {
                            break;
                        } else if (_.tabs[i].close) {
                            _.delTab(ev, _.tabs[i].code, true);
                        }
                    }
                }
            };
        }
    }
    if (count > 0) {
        item[idx++] = {
            text: 'separator'
        };
        for (var i = count - 1; i >= 0; i--) {
            if (tab == _.tabs[i].code) {
                item[idx++] = {
                    text: '重新加载', func: function () {
                        //加载初始URL
                        _.updateIframe(_.tabs[i].code, _.tabs[i].url, true);
                        /* 刷新(不重新加载)
                        var iframeId = _.getIframeId(_.tabs[i].code);
                        $(iframeId)[0].contentWindow.location.reload();
                        */
                    }
                };
                break;
            }
        }
    }
    if (count > 1) {
        item[idx++] = {
            text: '全部重新加载', func: function () {
                for (var i = count - 1; i >= 0; i--) {
                    _.updateIframe(_.tabs[i].code, _.tabs[i].url, true);
                }
            }
        };
    }

    item[idx++] = {
        text: 'separator'
    };
    var lastClosedTab = _.getClosedTab();
    if (lastClosedTab != null) {
        item[idx++] = {
            text: '重新打开关闭的标签页', func: function () {
                if (_.loadPage(lastClosedTab, obj, false)) {
                    _.deleteClosedTab();
                }
            }
        };
        item[idx++] = {
            text: '重新打开关闭的全部标签页', func: function () {
                for (var i = 0, c = _.closedTabs.length; i < c; i++) {
                    lastClosedTab = _.getClosedTab();
                    if (_.loadPage(lastClosedTab, obj, false)) {
                        _.deleteClosedTab();
                    } else {
                        break;
                    }
                }
            }
        };
    } else {
        item[idx++] = {
            text: '重新打开关闭的标签页', disabled: true
        };
        item[idx++] = {
            text: '重新打开关闭的全部标签页', disabled: true
        };
    }

    var config = {
        width: 100,
        zindex: 1000,
        opacity: 1,
        item: item
    };
    _.cmenu = new ContextMenu(e, config, obj);
};

page.tab.hideContextMenu = function () {
    if (page.tab.cmenu != null) {
        page.tab.cmenu.Close(page.tab.cmenu.id);
    }
};

page.tab.checkPageIsLoad = function (tab) {
    for (var i = 0; i < page.tab.tabs.length; i++) {
        if (page.tab.tabs[i].code == tab) {
            return true;
        }
    }
    return false;
};

page.tab.gotoPage = function (param) {
    page.tab.curTab = param.action;

    page.tab.showCurrentTabItem(page.tab.curTab);
};

page.tab.delTab = function (ev, tab, isContextMenu) {
    var _ = page.tab;
    if (_.tabs.length <= _.minCount) {
        cms.box.alert({ title: '提示', html: '不要全部都删了，至少保留' + _.minCount + '个' + _.pageLabel + '吧。' });
        return false;
    }
    if (tab == undefined) {
        tab = _.curTab;
    }
    for (var i = 0; i < _.tabs.length; i++) {
        if (tab == _.tabs[i].code) {
            //记录被关闭的标签页
            _.closedTabs.push(_.tabs[i]);

            //删除数组中的标签
            _.tabs.splice(i, 1);
            $('#frmbox iframe').remove('.frmcon[id=frmMain_' + tab + ']');
            $('#tabpanel a').remove('[lang=' + tab + ']');
            break;
        }
    }
    if (tab == _.curTab) {
        for (var j = _.tabs.length - 1; j >= 0; j--) {
            var _t = _.tabs[j];
            if (!_t.deleted && tab != _t.code) {
                _.loadPage({ code: _t.code, url: _t.url, name: _t.name }, null);
                _.curTab = _t.code;
                break;
            }
            delete _t;
        }
    }

    _.setTabPanelSize();

    //setNewSelectedNode(tab);

    if (!isContextMenu) {
        //阻止单击事件冒泡
        if (ev.stopPropagation) { ev.stopPropagation(); } else { ev.cancelBubble = true; }
        if (ev.preventDefault) { ev.preventDefault(); } else { ev.returnValue = false; }
    }
};
/*
page.tab.setNewSelectedNode = function (tab) {
    if (o != null && o != undefined) {
        var treeSelectId = o.getSelected().id;
        if (treeSelectId == tab) {
            var newTab = tabs.length > 0 ? tabs[0].code : -1;
            o.select(newTab);
            module.config.curSelect = newTab;
        }
    }
};
*/

//获取最后一次被删除的标签
page.tab.getClosedTab = function () {
    if (page.tab.closedTabs.length > 0) {
        var idx = page.tab.closedTabs.length - 1;
        var tab = page.tab.closedTabs[idx];
        //page.tab.closedTabs.splice(idx, 1);

        return tab;
    }
    return null;
};

//删除被删除标签中 已经重新加载的标签页
page.tab.deleteClosedTab = function () {
    if (page.tab.closedTabs.length > 0) {
        var idx = page.tab.closedTabs.length - 1;
        page.tab.closedTabs.splice(idx, 1);

        return true;
    }
    return false;
};