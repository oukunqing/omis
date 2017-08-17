var module = module || {};
module.frame = module.frame || {};

module.frame.boxSize = {};
module.frame.paddingTop = 4;
module.frame.paddingWidth = 5;
module.frame.borderWidth = 2;
module.frame.switchWidth = 5;
module.frame.bodyLeftWidthConfig = [200, 0];
module.frame.bodyLeftWidth = 200;
module.frame.titleHeight = 25;
module.frame.promptHeight = 0;
module.frame.bodyBottomHeight = 30;
module.frame.showBodyLeft = true;

module.frame.getRootPath = function() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var prePath = strFullPath.substring(0, strFullPath.indexOf(strPath));
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return prePath + (prePath.indexOf('http://localhost') >= 0 ? postPath: '');
}
//获得平台URL Hostd
module.frame.path = module.frame.getRootPath();

module.frame.getBodySize = function(){
	return {
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight
    };
};

module.frame.getParentBodySize = function() {
    return {
        width: parent.document.documentElement.clientWidth,
        height: parent.document.documentElement.clientHeight
    };
};

module.frame.initialForm = function(){
	$('#bodyLeftSwitch').click(function(){
		module.frame.switchLeftDisplay($(this));
	});
};

module.frame.setBodySize = function () {
    var bodySize = module.frame.getBodySize();
    boxSize = module.frame.getBoxSize();
    
    $('#pageBody').css('padding-top', module.frame.paddingTop);

    $('#bodyLeft').width(module.frame.bodyLeftWidth - module.frame.borderWidth);
    $('#bodyLeft').height(boxSize.height - module.frame.borderWidth);

    $('#treebox').height(boxSize.height - module.frame.titleHeight - module.frame.borderWidth);

    $('#bodyLeftSwitch').height(boxSize.height + module.frame.borderWidth);

    $('#bodyMain').width(boxSize.width - module.frame.borderWidth);
    $('#bodyMain').height(boxSize.height - module.frame.borderWidth);

    //$('#bodyPrompt').height(module.frame.promptHeight - 1);
    $('#frmbox').width(boxSize.width - module.frame.borderWidth);

    module.frame.setBoxSize();
};

//为了兼容IE6/IE7下父窗口调用子窗口的方法而特别设置
function setModuleFrameBodySize() {
    module.frame.setBodySize();
}

module.frame.getBoxSize = function(){
    var bodySize = cms.frame.getFrameSize();
    module.frame.bodyLeftWidth = $('#bodyLeft').is(':visible') ? module.frame.bodyLeftWidthConfig[0] : 0;
	return {
		width: bodySize.width - module.frame.bodyLeftWidth - module.frame.paddingWidth,
		height: bodySize.height - module.frame.paddingTop
	};
};

module.frame.getFormSize = function(){
	var bodySize = module.frame.getBoxSize();
	return {
		width: bodySize.width - module.frame.bodyLeftWidth - module.frame.paddingWidth,
		height: bodySize.height - module.frame.titleHeight - module.frame.promptHeight - module.frame.borderWidth
	};
};

module.frame.setBoxSize = function (isShow) {
    boxSize = module.frame.getBoxSize();

    if (isMulti) {
        tabpanelBoxWidth = boxSize.width - module.frame.borderWidth - 16 * 2;
        $('#tabpanelBox').width(tabpanelBoxWidth);
        tabpanelBoxLeft = $('#tabpanelBox').offset().left;

        setTabPanelSize();
    }

    $('#frmbox .frmcon').width(boxSize.width - module.frame.borderWidth);
    $('#frmbox .frmcon').height(boxSize.height - module.frame.borderWidth - module.frame.promptHeight - 25);

    if (typeof module.config.setFormBodySize == 'function') {
        module.config.setFormBodySize();
    }

    if (cms.util.isMSIE && cms.util.ieVersion() <= 8) {
        /*
        var childs = $I('#frmbox').childNodes;
        for (var i = 0, c = childs.length; i < c; i++) {
            var child = window.frames[childs[i].id].window;
            var func = child.setBodySize || child.setBoxSize;
            if (typeof func == 'function') {
                func();
            }
        }
        */
        var childs = $('#frmbox').children();
        childs.each(function (i) {
            var child = $(this)[0].contentWindow;
            var func = child.setBodySize || child.setBoxSize;
            if (typeof func == 'function') {
                func();
            }
        });
    }
};

module.frame.setFrameByShortcutKey = function(){
    document.onkeyup = function(e){
        var e = e||event;
        //当同时按下 Shift + L 键，即触发左栏菜单隐藏/显示
        if(e.shiftKey){
            switch(e.keyCode){
                case 76:
                    if($('#bodyLeft').attr('id') != undefined){
                        module.frame.switchLeftDisplay($('#bodyLeftSwitch'));
                    }
                    break;
                default:
                    //module.config.shortcutKeyAction(e.keyCode);
                    break;
            }
        }
    }
}

module.frame.switchLeftDisplay = function(obj, isShow){
	if(isShow != undefined){
		module.frame.showBodyLeft = !isShow;
	}
	if(module.frame.showBodyLeft){
		module.frame.showBodyLeft = false;
		$('#bodyLeft').hide();
		module.frame.bodyLeftWidth = module.frame.bodyLeftWidthConfig[1];
	}
	else {
		module.frame.showBodyLeft = true;
		$('#bodyLeft').show();
		module.frame.bodyLeftWidth = module.frame.bodyLeftWidthConfig[0];
	}
	if(obj == null || obj == undefined){
		obj = $('#bodyLeftSwitch');
	}
	obj.removeClass();
	obj.addClass(module.frame.showBodyLeft ? 'switch-left' : 'switch-right');
	obj.attr('title', module.frame.showBodyLeft ? '隐藏左栏菜单' : '显示左栏菜单');
	
	//obj.css('background-color', module.frame.showBodyLeft ? '#dfe8f6' : '#99bbe8');

	module.frame.setBodySize();
};

module.frame.setFormTitle = function(title){
	$('#bodyTitle .title').html(title);
};

module.frame.setFormPrompt = function(strHtml){
	$('#bodyPrompt .title').html(strHtml);
};