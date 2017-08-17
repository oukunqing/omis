var cms = cms || {};
cms.jquery = cms.jquery || {};
cms.jquery.isIE6 = navigator.userAgent.indexOf('MSIE 6.0') >= 0; // $.browser.msie && '6.0' == $.browser.version;
cms.jquery.isMSIE = navigator.userAgent.indexOf('MSIE') >= 0 || navigator.userAgent.indexOf('Trident') >= 0;

cms.jquery.selectCheckBox = function (obj) {
    var items = $(obj + ' tr input[type="checkbox"]');
    items.each(function () {
        if ($(this).attr("checked") == 'checked') {
            $(this).attr("checked", false);
            $(this).parents('tr').removeClass('selected');
        } else {
            $(this).parents('tr').addClass('selected');
            $(this).attr("checked", true);
        }
    });
};

cms.jquery.getCheckBoxCheckedValue = function (obj) {
    var arr = [];
    var items = $(obj + ' input[type="checkbox"]');
    items.each(function () {
        if ($(this).is(":checked")) {
            arr.push($(this).val());
        }
    });
    return arr.join(',');
};

cms.jquery.getCheckBoxChecked = function (obj) {
    var arr = [];
    var chbitems = $(obj + ' input[type="checkbox"]:checked');
    return chbitems;
};

cms.jquery.isChecked = function (obj) {
    return $(obj).attr("checked") == 'checked';
};

cms.jquery.buildTab = function (code, name, maxlen, tabContainerId, isClose, func, arrCss, contextmenu, style, closeText) {
    var len = name.len();
    var strTitle = len > maxlen ? name : '';
    var strName = len > maxlen ? name.gbtrim(maxlen, '..') : name;
    if (tabContainerId.indexOf('#') != 0) tabContainerId = '#' + tabContainerId;
    var css = arrCss || (isClose ? ['tab-c', 'cur-c'] : ['tab', 'cur']);
    if (isClose) {
        if (closeText == undefined) {
            closeText = '关闭';
        }
        var strFunc = '(event, \'' + code + '\');';
        func = func == undefined || func == '' ? 'delTab' + strFunc : func.indexOf('(') < 0 ? func + strFunc : func;
    }
    var strStyle = style != undefined ? ' style="' + style + '"' : '';
    var strTab = '<a class="' + css[0] + '" lang="' + code + '" rel="' + tabContainerId + '"'
        + (contextmenu != undefined && contextmenu != null ? ' oncontextmenu="' + contextmenu + '"' : '')
        + ' onselectstart="return false" unselectable="on" '
        + '>'
        + '<span title="' + strTitle + '" lang="' + css[0] + ',' + css[1] + '"' + strStyle + '>' + strName + '</span>'
        + (isClose ? '<i class="c" title="' + closeText + '" onclick="' + func + '"></i>' : '')
        + '</a>';
    return strTab;
};


//Tab标签页方式
cms.jquery.tabs = function (tab, tabContainer, lstContainer, func, isHover) {
    if (typeof lstContainer == 'undefined') {
        lstContainer = '.tabcon';
    }
    var hasCur = false;
    var $arrTab = $(tab + ' a');

    //默认仅显示当前选项卡内容，隐藏其余的
    $.each($arrTab, function () {
        var objConId = $(this).prop('rel');
        if ($(this).attr('class').indexOf('cur') >= 0) {
            hasCur = true;
            $(objConId).show();
        } else {
            $(objConId).hide();
        }
    });

    //没有设置默认的当前选项，自动设置一个默认选项
    if (!hasCur) {
        $.each($arrTab, function () {
            if ($(this).is(':visible')) {
                cms.jquery.tabs._action($(this), tab, tabContainer, lstContainer, func);
                return false;
            }
        });
    }
    if (isHover) {
        $arrTab.mouseover(function (e) {
            cms.jquery.tabs._action($(this), tab, tabContainer, lstContainer, func);
            cms.jquery.stopBubble(e);
        });
    } else {
        $arrTab.click(function (e) {
            cms.jquery.tabs._action($(this), tab, tabContainer, lstContainer, func);
            cms.jquery.stopBubble(e);
        });
    }
};

//禁止事件冒泡
cms.jquery.stopBubble = function (ev) {
    try {
        ev = ev || window.event || arguments.callee.caller.arguments[0];
        if (ev.stopPropagation) { ev.stopPropagation(); } else { ev.cancelBubble = true; }
        if (ev.preventDefault) { ev.preventDefault(); } else { ev.returnValue = false; }
    } catch (ex) { }
};

cms.jquery.tabs._action = function (item, tab, tabContainer, lstContainer, func) {
    var $arrTab = $(tab + ' a');
    $arrTab.removeClass();
    $.each($arrTab, function () {
        $(this).addClass($(this).children('span').eq(0).prop('lang').split(',')[0] || 'tab');
    });
    item.addClass(item.children('span').eq(0).prop('lang').split(',')[1] || 'cur');

    var container = item.prop('rel');
    if (lstContainer != null) {
        var parent = typeof tabContainer == 'string' && tabContainer.indexOf('#') == 0 ? tabContainer : '';
        $(parent + ' ' + lstContainer).hide();

        if (container.length > 1 && container.indexOf('#') == 0) {
            $(parent + ' ' + container).show();
        }
    }

    if (typeof func == 'function') {
        var code = item.prop('lang');
        func({ action: code, code: code, container: container });
    } else if (typeof func == 'string' && func.length > 0) {
        var code = item.prop('lang');
        eval(func)({ action: code, code: code, container: container });
    }
};

cms.jquery.tabSwitch = function (tab, i) {
    $(tab + ' a').eq(i).click();
};

cms.jquery.getTabItem = function (tabs, code) {
    var idx = -1;
    $.each(tabs, function (i, o) {
        if (o.code == code) {
            idx = i;
            return false;
        }
    });
    return idx;
};

cms.jquery.getTabWidth = function (objTabBox, minWidth) {
    var total = 0;
    var w = 0;
    var list = objTabBox.children();
    var c = list.length;
    //获取每个TAB项的实际宽度
    for (var i = 0; i < c; i++) {
        //w = list.eq(i).width();
        w = list.eq(i).outerWidth();
        total += w;
    }
    total += (c + 1) * 3;
    if (total <= minWidth) {
        return minWidth;
    }
    return total;
};

cms.jquery.scrollSync = function (obj, objSync) {
    $(objSync).scrollLeft($(obj).scrollLeft());
};


//Tab滚动方式
cms.jquery._tabs = function (tab, tabContainer, lstContainer, func, isHover) {
    var $arrTab = $(tab + ' a');
    var curIndex = -1;
    var lstTag = 'DIV' + (lstContainer || '.tabcon');

    var $box = $(tabContainer);
    var $child = $box.children(lstTag);
    var c = $child.length;

    //先隐藏全部内容，防止出现多余的内容项
    $child.each(function () {
        $(this).hide();
    });

    $arrTab.each(function (i) {
        if ($(this).is(':visible')) {
            var objConId = $(this).prop('rel');
            $(objConId).show();

            if ($(this).attr('class').indexOf('cur') >= 0) {
                curIndex = i;
            }
        }

        if ($(this).prop('href') != '') {
            $(this).prop('href', 'javascript:void(0);');
        }
        if (isHover) {
            $(this).mouseover(function () {
                cms.jquery._tabAction(tab, $box, i, $(this), lstTag);
            });
        } else {
            $(this).click(function () {
                cms.jquery._tabAction(tab, $box, i, $(this), lstTag);
            });
        }
    });

    //没有设置默认选项
    if (curIndex < 0) {
        $.each($arrTab, function (i) {
            if ($(this).is(':visible')) {
                cms.jquery._tabAction(tab, $box, i, $(this), lstTag);
                return false;
            }
        });
    } else {
        window.setTimeout(function () {
            cms.jquery._tabAction(tab, $box, curIndex, $arrTab.eq(curIndex), lstTag);
        }, 10);
    }

    /*
    var $next = $child.eq(c - 1).next();
    if ($next.length <= 0) {
    $box.append('<div style="height:1px;margin:0;padding:0;overflow:hidden;border:none 0;clear:both;"></div>');
    }
    */
    $box.append('<div style="height:1px;margin:0;padding:0;overflow:hidden;border:none 0;clear:both;"></div>');

    $box.scroll(function () {
        var curHeight = $(this).scrollTop();
        var arrHeight = cms.jquery._getChildHeight($box, lstTag);
        for (var i in arrHeight) {
            if (curHeight < arrHeight[i].height) {
                cms.jquery._setCurTabCss(tab, i);
                break;
            }
        }
    });
};

cms.jquery._tabAction = function (tab, $box, i, $obj, lstTag) {
    var arrHeight = cms.jquery._getChildHeight($box, lstTag);
    var top = 0 == i ? 0 : arrHeight[i - 1].height;

    $box.scrollTop(top);

    cms.jquery._setCurTabCss(tab, 0, $obj);
};

cms.jquery._getChildHeight = function ($box, lstTag) {
    var arrHeight = [];
    var totalHeight = 0;
    var $childs = $box.children(lstTag || 'DIV.tabcon');
    var len = $childs.length;

    $childs.each(function (i) {
        if ($(this).is(':visible')) {
            totalHeight += $(this).outerHeight();
            arrHeight.push({ obj: $(this), height: totalHeight });

            if (i == len - 1) {
                cms.jquery._setLastHeight($box, $(this));
            }
        }
    });

    return arrHeight;
};

cms.jquery._setLastHeight = function ($box, $obj) {
    var nextSibling = $obj.next();
    if (nextSibling.length > 0) {
        var boxHeight = $box.outerHeight();
        var lastHeight = $obj.outerHeight() + nextSibling.outerHeight();
        if (lastHeight < boxHeight) {
            nextSibling.css('margin-bottom', boxHeight - lastHeight - 1);
        }
    }
};

cms.jquery._setCurTabCss = function (tab, idx, $obj) {
    var $arrTab = $(tab + ' a');
    $arrTab.removeClass();
    $arrTab.addClass('tab');
    if ($obj != undefined) {
        $obj.addClass('cur');
    } else {
        $arrTab.eq(idx).addClass('cur');
    }
};

cms.jquery.setToggle = function (el, tagName) {
    tagName = tagName || 'div';
    $(el).click(function () {
        cms.jquery.toggle($(this), tagName);
    });
};

cms.jquery.toggle = function ($obj, tagName) {
    tagName = tagName || 'DIV';
    var t = $obj.next(tagName);
    t.toggle();

    var a = $obj.children('a.switch').eq(0);
    if (typeof a.prop('lang') != 'undefined') {
        a.removeClass();
        a.addClass(a.prop('lang').split(',')[t.is(':hidden') ? 1 : 0]);
    }
};