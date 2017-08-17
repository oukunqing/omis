var cms = cms || {};
cms.util = cms.util || {};

cms.util.ieVersion = function () {
    if (navigator.userAgent.indexOf('MSIE 6.0') >= 0) {
        return 6;
    } else if (navigator.userAgent.indexOf('MSIE 7.0') >= 0) {
        return 7;
    } else if (navigator.userAgent.indexOf('MSIE 8.0') >= 0) {
        return 8;
    } else if (navigator.userAgent.indexOf('MSIE 9.0') >= 0) {
        return 9;
    } else if (navigator.userAgent.indexOf('MSIE 10.0') >= 0) {
        return 10;
    } else if (navigator.userAgent.indexOf('MSIE 11.0') >= 0 || (navigator.userAgent.indexOf('Trident/7.0') >= 0 && navigator.userAgent.indexOf('rv:11.0') >= 0)) {
        return 11;
    }
    return 5.5;
};
/*
Internet Explorer 11 修改了user-agent，去掉了MSIE关键字，需要判断Trident来确定是否是IE(11)
*/
cms.util.isMSIE = navigator.userAgent.indexOf('MSIE') >= 0 || navigator.userAgent.indexOf('Trident') >= 0;
cms.util.isIE6 = cms.util.ieVersion() == 6;
cms.util.isIE11 = cms.util.ieVersion() == 11;
cms.util.isIELowVersion = navigator.userAgent.indexOf('MSIE 6.0') >= 0 || navigator.userAgent.indexOf('MSIE 7.0') >= 0;

cms.util.isFirefox = navigator.userAgent.indexOf('Firefox') >= 0;
cms.util.isChrome = navigator.userAgent.indexOf('Chrome') >= 0;
cms.util.isMobile = navigator.userAgent.indexOf('Mobile') >= 0;

cms.util.$ = cms.util.$I = function (id) {
    return $I(id);
};

cms.util.$N = function (name, tag, parent, val) {
    return $N(name, tag, parent, val);
};

cms.util.$T = function (tag) {
    return $T(tag);
};

/*
通过ClassName获得控件元素
className: 样式名称
tag(tagName:type): 控件标签，如：div,table,span,label,p,li,select,input:text|checkbox|radio|password|hidden|file|image|button|reset|submit等
parent: 父容器
*/
cms.util.getElementsByClassName = function (className, tag, parent) {
    return $C(className, tag, parent);
};

cms.util.$C = function (className, tag, parent) {
    return $C(className, tag, parent);
};

cms.util.Try = {
    these: function () {
        var returnValue;
        for (var i = 0; i < arguments.length; i++) {
            var lambda = arguments[i];
            try {
                returnValue = lambda();
                break;
            }
            catch (e) { }
        }
        return returnValue;
    }
};
cms.util.xmlHttp = cms.util.Try.these(
    function () { return new ActiveXObject('Msxml2.XMLHTTP') },
    function () { return new ActiveXObject('Microsoft.XMLHTTP') },
    function () { return new XMLHttpRequest() }
) || false;

cms.util.ajax = function (config) {
    var _cfg = config || { async: true };
    var strData = null;
    var type = _cfg.type || 'POST';
    var xhr = cms.util.xmlHttp;
    var callback = _cfg.callback || _cfg.callBack || _cfg.success;
    if (typeof callback != 'function') {
        callback = null;
    }
    xhr.open(type.toUpperCase(), _cfg.url, _cfg.async);
    xhr.onreadystatechange = function () {
        if (4 == xhr.readyState) {
            if (200 == xhr.status) {
                var data = xhr.responseText;
                if (_cfg.dataType != undefined) {
                    switch (_cfg.dataType.toUpperCase()) {
                        case 'XML':
                            data = xhr.responseXML;
                            break;
                        case 'JSON':
                            data = eval('(' + xhr.responseText + ')');
                            break;
                    }
                }
                if (callback != null) {
                    callback(data, _cfg.param);
                } else {
                    strData = data;
                    delete data;
                }
            } else {
                if (callback != null) {
                    callback('error', _cfg.param);
                }
            }
        }
    };
    if ('POST' == type.toUpperCase()) {
        xhr.setRequestHeader("content-type", "application/x-www-form-urlencoded");
        xhr.send(_cfg.data);
    } else {
        xhr.send(null);
    }
    if (_cfg.async != undefined && !_cfg.async) {
        return strData;
    }
};

cms.util.Hashtable = function () {
    this._hash = new Object();
    this.add = function (key, value) {
        if (typeof (key) != "undefined") {
            if (this.contains(key) == false) {
                this._hash[key] = typeof (value) == "undefined" ? null : value;
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    };
    this.remove = function (key) {
        delete this._hash[key];
    };
    this.count = function () {
        var i = 0;
        for (var k in this._hash) {
            i++;
        }
        return i;
    };
    this.items = function (key) {
        return this._hash[key];
    };
    this.contains = function (key) {
        return typeof (this._hash[key]) != "undefined";
    };
    this.clear = function () {
        for (var k in this._hash) {
            delete this._hash[k];
        }
    };
    this.item = function (i) {
        if (i < 0) {
            return null;
        } else {
            var p = 0;
            for (var k in this._hash) {
                if (p++ == i) return (k);
            }
        }
    };
    this.show = function () {
        var result = [];
        for (var k in this._hash) {
            result.push(k);
        }
        return result;
    };
};

//获取网站根目录
//这个方法要更改，因为VS2013之后的本地站点不是虚拟目录了，而是单独站点模式了
cms.util.getRootPath = function () {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var prePath = strFullPath.substring(0, strFullPath.indexOf(strPath));
    var postPath = '';
    if (prePath.indexOf('http://localhost') >= 0 || prePath.indexOf('http://127.0.0.1') >= 0) {
        if ('undefined' != typeof cmsPath) {
            postPath = cmsPath;
        } else if ('undefined' != typeof webDir) {
            postPath = webDir;
        } else if ('undefined' != typeof webConfig && webConfig.webDir != undefined) {
            postPath = webConfig.webDir;
        } else {
            postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
        }
    }
    return prePath + postPath;
};

cms.util.path = cms.util.getRootPath();

cms.util.createTable = function (id, css) {
    var tb = document.createElement('table');
    tb.id = id;
    tb.className = css;
    return tb;
};

cms.util.buildTable = function (id, css) {
    return '<table cellpadding="0" cellspacing="0" id="' + id + '" class="' + css + '"></table>';
};

//填充表格内容
cms.util.fillTable = function (row, rowdata) {
    for (var i = 0, rc = rowdata.length; i < rc; i++) {
        var dr = rowdata[i];
        var cell = row.insertCell(i);

        if (typeof dr.title != 'undefined') {
            cell.title = dr.title;
        }
        if (typeof dr.lang != 'undefined') {
            cell.lang = dr.lang;
        }
        if (typeof dr.css == 'string' || typeof dr.className == 'string') {
            cell.className = dr.css || dr.className;
        }
        if (typeof dr.rowspan == 'number' && dr.rowspan > 0) {
            cell.rowSpan = dr.rowspan;
        }
        if (typeof dr.colspan == 'number' && dr.colspan > 0) {
            cell.colSpan = dr.colspan;
        }
        cell.innerHTML = typeof dr.html == 'number' ? dr.html : (dr.html || '');

        var style = rowdata[i].style;
        if (typeof (style) == 'object') {
            for (var j = 0, c = style.length; j < c; j++) {
                cell.style[style[j][0]] = style[j][1];
            }
        }
        if (typeof dr.action == 'object') {
            var func = dr.action.func;
            if (typeof func == 'function') {
                if (typeof dr.action.param != 'undefined') {
                    cell.param = dr.action.param;
                }
                cell.onclick = function () {
                    func(this.param);
                };
            }
        }
    }
    rowdata = null;

    return true;
};

//清除表格行  obj:表格对象  i:剩余行数
cms.util.clearDataRow = function (obj, i) {
    for (var k = obj.rows.length - 1; k >= i; k--) {
        obj.deleteRow(k);
    }
};

//移除指定的表格行
cms.util.removeDataRow = function (obj, i) {
    if (i < obj.rows.length) {
        obj.deleteRow(i);
    }
};

//设置表格交替行样式、鼠标悬浮样式
cms.util.setTableStyle = function (tb, param) {
    var rows = tb.rows;
    param = param || {};
    var par = {
        start: param.start || 1,
        setHead: typeof param.setHead != 'undefined' ? param.setHead : true,
        css: ['', 'alternating', 'hover', 'selected']
    };

    if (par.start > 0 && par.setHead) {
        for (i = 0; i < par.start; i++) {
            rows[i].className = 'trheader';
        }
    }

    for (var i = par.start, c = rows.length; i < c; i++) {
        if (rows[i].rowIndex % 2 == 0) {
            rows[i].className = par.css[1];
        }
        rows[i].onmouseover = function () {
            this.className = par.css[2];
        };
        rows[i].onmouseout = function () {
            this.className = this.selected ? par.css[3] : (this.rowIndex % 2 == 0 ? par.css[1] : par.css[0]);
        };
    }
};

//设置表格选中行样式
cms.util.selectTableRow = function (tr, selected) {
    if (typeof selected == 'undefined') {
        selected = tr.selected || false;
    }
    if (selected) {
        tr.className = 'selected';
        tr.selected = true;
    } else {
        tr.className = tr.rowIndex % 2 == 0 ? 'alternating' : '';
        tr.selected = false;
    }
};

//填充下拉列表项 object
cms.util.fillOption = function (obj, val, txt) {
    obj.options.add(new Option(txt, val));
};

//填充下拉列表项 object
cms.util.fillOptions = function (obj, arrValTxt, selectedVal) {
    for (var i = 0, c = arrValTxt.length; i < c; i++) {
        obj.options.add(new Option(arrValTxt[i][1], arrValTxt[i][0]));
    }
    if (typeof selectedVal != 'undefined') {
        obj.value = '' + selectedVal;
    }
    return true;
};

cms.util.getSelectedText = function (obj) {
    if (obj != null) {
        return obj.options[obj.selectedIndex].text;
    }
    return '';
};

//填充下拉列表项 object
cms.util.fillNumberOptions = function (obj, min, max, selectedVal, step, totalWidth, paddingChar, add) {
    if (typeof step == 'undefined') {
        step = 1;
    }
    var start = min;
    var end = max;
    if (min > max) {
        start = max;
        end = min;
    }
    if (typeof totalWidth != 'undefined' && totalWidth != null) {
        totalWidth = parseInt(totalWidth, 10);
        paddingChar = typeof paddingChar == 'undefined' ? '0' : paddingChar;
        for (var i = start; i <= end; i += step) {
            var strNum = ('' + i).padLeft(totalWidth, paddingChar);
            obj.options.add(new Option(strNum, strNum));
        }
    } else {
        if (typeof add == 'number') {
            for (var i = start; i <= end; i += step) {
                obj.options.add(new Option(i + add, i));
            }
        } else {
            for (var i = start; i <= end; i += step) {
                obj.options.add(new Option(i, i));
            }
        }
    }
    if (typeof selectedVal != 'undefined') {
        obj.value = '' + selectedVal;
    }
    return true;
};

//清除下拉列表项 object
cms.util.clearOption = function (obj, i) {
    obj.options.length = i;
};

cms.util.buildOption = function (val, txt, selectedValue) {
    return '<option value="' + val + '"' + (val == selectedValue ? ' selected="selected"' : '') + '>' + txt + '</option>';
};
//创建下拉列表项 string
cms.util.buildOptions = function (arrValTxt, selectedValue) {
    var options = [];
    for (var i in arrValTxt) {
        var isObj = typeof arrValTxt[i] == 'object', val = arrValTxt[i], txt = arrValTxt[i];
        if (isObj) {
            val = arrValTxt[i][0];
            txt = arrValTxt[i][1] || arrValTxt[i][0];
        }
        options.push('<option value="' + val + '"' + (val == selectedValue ? ' selected="selected"' : '') + '>' + txt + '</option>');
    }
    return options.join('');
};

//创建下拉列表项 string
cms.util.buildNumberOptions = function (min, max, selectedValue, step, totalWidth, paddingChar, add) {
    if (step == undefined) {
        step = 1;
    }
    var strOption = '';
    var start = min;
    var end = max;
    if (min > max) {
        start = max;
        end = min;
    }
    if (totalWidth != undefined && totalWidth != null) {
        totalWidth = parseInt(totalWidth, 10);
        paddingChar = paddingChar == undefined ? '0' : paddingChar;
        for (var i = start; i <= end; i += step) {
            var strNum = ('' + i).padLeft(totalWidth, paddingChar);
            strOption += '<option value="' + strNum + '"' + (i == selectedValue || strNum == selectedValue ? ' selected="selected"' : '') + '>' + strNum + '</option>';
        }
    } else {
        if (add != undefined && typeof add == 'number') {
            for (var i = start; i <= end; i += step) {
                strOption += '<option value="' + i + '"' + (i == selectedValue ? ' selected="selected"' : '') + '>' + (i + add) + '</option>';
            }
        } else {
            for (var i = start; i <= end; i += step) {
                strOption += '<option value="' + i + '"' + (i == selectedValue ? ' selected="selected"' : '') + '>' + i + '</option>';
            }
        }
    }
    return strOption;
};

//获得选中的Radio单选按钮的值
cms.util.getRadioCheckedValue = function (name, defaultValue) {
    var arrObj = document.getElementsByName(name);
    for (var i = 0, c = arrObj.length; i < c; i++) {
        if (arrObj[i].checked) {
            return arrObj[i].value;
        }
    }
    return defaultValue;
};

//设置Radio选中
cms.util.setRadioChecked = function (name, value, disabled) {
    var arrObj = document.getElementsByName(name);
    disabled = typeof disabled == 'undefined' ? null : ((typeof disabled == 'boolean' && disabled) || (typeof disabled == 'number' && disabled == 1) ? true : false);
    if (disabled == null) {
        for (var i = 0, c = arrObj.length; i < c; i++) {
            if (arrObj[i].value == value) {
                arrObj[i].checked = true;
                break;
            }
        }
    } else {
        for (var i = 0, c = arrObj.length; i < c; i++) {
            if (arrObj[i].disabled == disabled && arrObj[i].value == value) {
                arrObj[i].checked = true;
                break;
            }
        }
    }
};

//选择复选框
//oper: 1-全选(all)，2-取消(cancel)，3-反选(inverse)
//var: 指定的内容
cms.util.selectCheckBox = function (name, oper, val, disabled) {
    var arr = document.getElementsByName(name);
    var oper = (oper == 1 || oper == 'all') ? 1 : (oper == 3 || oper == 'inverse') ? 3 : 2;
    var checked = oper == 1;
    disabled = typeof disabled == 'boolean' ? disabled : (typeof disabled == 'number' ? 1 == disabled : null);
    if (typeof val == 'undefined' || val == null) {
        if (disabled == null) {
            for (var i = 0, c = arr.length; i < c; i++) {
                arr[i].checked = oper == 3 ? arr[i].checked == true ? false : true : checked;
            }
        } else {
            for (var i = 0, c = arr.length; i < c; i++) {
                if (arr[i].disabled == disabled) {
                    arr[i].checked = oper == 3 ? arr[i].checked == true ? false : true : checked;
                }
            }
        }
    } else if (typeof val == 'string' || typeof val == 'number') {
        val = typeof val == 'number' ? '' + val : val;
        if (disabled == null) {
            for (var i = 0, c = arr.length; i < c; i++) {
                if (arr[i].value == val) {
                    arr[i].checked = oper == 3 ? arr[i].checked == true ? false : true : checked;
                    break;
                }
            }
        } else {
            for (var i = 0, c = arr.length; i < c; i++) {
                if (arrObj[i].disabled == disabled && arr[i].value == val) {
                    arr[i].checked = oper == 3 ? arr[i].checked == true ? false : true : checked;
                    break;
                }
            }
        }
    }
};

//设置指定内容的复选框为选中
cms.util.setCheckBoxChecked = function (name, delimiter, values) {
    var arr = document.getElementsByName(name);
    var items = values.replace(/\(|\)/g, "").split(delimiter);
    for (var i in items) {
        if (items[i] != '' && items[i] != undefined) {
            for (var j = 0, c = arr.length; j < c; j++) {
                if (arr[j].value == items[i]) {
                    arr[j].checked = true;
                }
            }
        }
    }
};

//设置指定ID的复选框选中或取消选中
cms.util.setCheckBoxCheckStatus = function (id, checked) {
    var obj = document.getElementById(id);
    if (obj != null) {
        if (typeof checked == 'boolean') {
            obj.checked = checked;
        } else if (checked == 0 || checked == 1) {
            obj.checked = checked == 1;
        } else {
            var isChecked = obj.checked;
            obj.checked = isChecked ? false : true;
        }
    }
};

//获得选中的复选框
cms.util.getCheckBoxChecked = function (name) {
    var arr = document.getElementsByName(name);
    var items = [];
    for (var i = 0, c = arr.length; i < c; i++) {
        if (arr[i].checked) {
            items.push(arr[i]);
        }
    }
    return items;
};

//获得选中的复选框内容 列表
cms.util.getCheckBoxCheckedValue = function (name, delimiter) {
    delimiter = delimiter || ',';
    var arr = document.getElementsByName(name);
    var items = [];
    for (var i = 0, c = arr.length; i < c; i++) {
        if (arr[i].checked) {
            items.push(arr[i].value);
        }
    }
    return items.join(delimiter);
};

//获得选中的复选框个数
cms.util.getCheckBoxCheckedCount = function (name) {
    var arr = document.getElementsByName(name);
    var count = 0;
    for (var i = 0, c = arr.length; i < c; i++) {
        count += arr[i].checked ? 1 : 0;
    }
    return count;
};

//设置选中的复选框的文字颜色
cms.util.setCheckBoxCheckedColor = function (name, param) {
    //var arr = cms.util.getCheckBoxChecked(name);
    var arr = cms.util.$N(name || 'chbItem');
    param = param || {};
    var _param = { color: param.color || ['#f00', '#000'], background: param.background || [] };
    for (var i = 0, c = arr.length; i < c; i++) {
        if (arr[i].checked) {
            arr[i].parentNode.style.color = _param.color[0] || '#f00';
            if (_param.background[0] != undefined) {
                arr[i].parentNode.style.background = _param.background[0];
            }
        } else {
            arr[i].parentNode.style.color = _param.color[1] || '#000';
            if (_param.background[1] != undefined) {
                arr[i].parentNode.style.background = _param.background[1] || '#fff';
            }
        }
    }
};

//将连续的数字用 - 组合
/*
1,2,3,4,5,6,7,8,9,10,12,15,20,21,22,23,24,25,26,27,28,36,42,50,51,52,53,54,55
以上数据 经过转换 变成 
1-10,12,15,20-28,36,42,50-55
*/
cms.util.numberToStructString = function (strNums) {
    var str = '';
    var rst = [];
    var move = 0;
    var idx = 0;
    var arr = strNums.split(',');
    for (var i = 0; i < arr.length - 1; i++) {
        if (arr[i + 1] - arr[i] == 1) {
            if (move == 0) {
                rst[idx] = [arr[i], arr[i + 1]];
                move = arr[i];
            }
            rst[idx][1] = arr[i + 1];
        } else {
            idx++;
            if (idx == 0) {
                rst[idx] = [arr[i], arr[i]];
            } else {
                rst[idx] = [arr[i + 1], arr[i + 1]];
            }
            move = 0;
        }
    }
    for (var j = 0; j < rst.length; j++) {
        str += ',' + (rst[j][0] == rst[j][1] ? rst[j][0] : rst[j][0] + '-' + rst[j][1]);
    }
    return str != '' ? str.substr(1) : str;
};

//将组合的数字还原成原始数字排列
/*
1-10,12,15,20-28,36,42,50-55
以上数据 经过转换 变成 
1,2,3,4,5,6,7,8,9,10,12,15,20,21,22,23,24,25,26,27,28,36,42,50,51,52,53,54,55
*/
cms.util.structStringToNumber = function (strStruct) {
    var rst = [];
    var arr = strStruct.split(',');
    for (var i = 0, c = arr.length; i < c; i++) {
        var tmp = arr[i].split('-');
        if (tmp.length == 2) {
            for (var j = parseInt(tmp[0], 10); j <= parseInt(tmp[1], 10); j++) {
                rst.push(j);
            }
        } else {
            rst.push(arr[i]);
        }
    }
    return rst.join(',');
};

//将数字转换成GPS经纬度坐标
cms.util.numberConvertLatLng = function (num, pn) {
    if (num == '' || num == '0' || num == '-') {
        return num;
    }
    var latlng = parseInt(num, 10) / 3600 / 100;
    var p = Math.pow(10, pn);
    return Math.round(latlng * p) / p;
};

//将度分秒转换成经纬度
cms.util.degreeConvertLatLng = function (lat, lng) {
    lat = cms.util.latLngDegreeConvertNumber(lat);
    lng = cms.util.latLngDegreeConvertNumber(lng);
    return [lat, lng];
};

//度分秒转换
cms.util.latLngDegreeConvertNumber = function (latlng, num) {
    if (latlng == '' || latlng == '0' || latlng == '-') {
        return latlng;
    }
    latlng = '' + latlng;
    var arr = latlng.split('.');
    var a = arr[0];
    var b = arr[1].substring(0, 2) / 60;
    var c = arr[1].substring(2);
    c = c / Math.pow(10, c.length - 2) / 3600;

    return Math.round((parseInt(a, 10) + b + c) * Math.pow(10, num)) / Math.pow(10, num);
};

//固定宽度并省略显示
/*
str: 内容
w: 宽度
id: 控件ID，用于取内容
tooltip: 是否显示提示
*/
cms.util.fixedCellWidth = function (str, w, showTitle, title, id) {
    var strTitle = showTitle != undefined && showTitle ? (title != undefined ? title : str) : '';
    var strId = id != undefined ? ' id="' + id + '"' : '';
    return '-' == str ? '-' : '<div class="word-ellipsis" style="width:' + w + 'px;" title="' + strTitle + '"><nobr' + strId + '>' + str + '</nobr></div>';
};

//JS加载JS文件
cms.util.loadJs = function (jsDir, jsName) {
    var jsTag = document.createElement('script');
    jsTag.setAttribute('type', 'text/javascript');
    jsTag.setAttribute('src', jsDir + jsName);
    document.getElementsByTagName('head')[0].appendChild(jsTag);
};

//DIV滚动同步
cms.util.scrollSync = function (obj, objSync) {
    objSync.scrollLeft = obj.scrollLeft;
};

cms.util.html = function (obj, str) {
    if (str == undefined) {
        return obj.innerHTML;
    } else {
        obj.innerHTML = str;
    }
};

//获得窗体尺寸大小
cms.util.getBodySize = function () {
    if (typeof document.compatMode != 'undefined' && document.compatMode == 'CSS1Compat') {
        return { width: document.documentElement.clientWidth, height: document.documentElement.clientHeight };
    } else if (typeof document.body != 'undefined') {
        return { width: document.body.clientWidth, height: document.body.clientHeight };
    }
};

//获得父窗体尺寸大小
cms.util.getParentBodySize = function () {
    /*
    return {
    width: parent.document.documentElement.clientWidth,
    height: parent.document.documentElement.clientHeight
    };
    */
    if (typeof parent.document.compatMode != 'undefined' && parent.document.compatMode == 'CSS1Compat') {
        return { width: parent.document.documentElement.clientWidth, height: parent.document.documentElement.clientHeight };
    } else if (typeof parent.document.body != 'undefined') {
        return { width: parent.document.body.clientWidth, height: parent.document.body.clientHeight };
    }
};

cms.util.setFocus = function (obj) {
    obj.focus();
};

cms.util.setCookie = function (name, value, expireHours) {
    if (expireHours == 0) {
        document.cookie = name + "=" + escape(value) + ";";
    } else {
        if (expireHours == undefined) {
            expireHours = 7 * 24;
        }
        var expDate = new Date();
        expDate.setTime(expDate.getTime() + (8 * 60 * 60 * 1000) + expireHours * 60 * 60 * 1000);
        document.cookie = name + "=" + escape(value) + ";expires=" + expDate.toGMTString();
    }
};

cms.util.getCookie = function (name) {
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$$)"));
    if (arr != null) return unescape(arr[2]);
    return '';
};

cms.util.delCookie = function (name) {
    var expDate = new Date();
    expDate.setTime(expDate.getTime() - 1);
    var cval = cms.util.getCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + expDate.toGMTString();
};

//屏蔽shift 以及 ';等特殊字符 允许shift + tab(键值为9)
cms.util.checkFormInput = function (e, str, nostr, isShift) {
    if (!isShift && e.shiftKey && str != '9') {
        return false;
    } else {
        return !str.instr(nostr);
    }
    return true;
};

//输入框控制
//禁止粘贴、禁止输入 '=,-/\ shift 以及 空格
cms.util.inputControl = function (obj, isShift, isAllowPaste) {
    obj.onkeydown = function (e) {
        var e = e || event;
        //屏蔽 '=,-/\ shift 以及 空格;
        var check = cms.util.checkFormInput(e, '' + e.keyCode, ['32', '186', '187', '188', '189', '191', '220', '222'], isShift);
        obj.focus();
        return check;
    };
    if (!isAllowPaste || typeof isAllowPaste == 'undefined') {
        obj.onpaste = function (e) {
            return false;
        };
    }
};

//禁止粘贴
cms.util.inputUnPaste = function (obj) {
    obj.onpaste = function (e) {
        return false;
    };
};

//检测浏览器cookie是否启用
cms.util.cookieEnabled = function () {
    if (navigator.cookieEnabled) {
        document.cookie = 'cookie=zyrh-cookie-test;';
        var cookieSet = document.cookie;
        if (cookieSet.indexOf('cookie=zyrh-cookie-test') > -1) {
            document.cookie = '';
            return true;
        }
        return false;
    } else {
        return false;
    }
};

cms.util.dateCompare = function (dateStart, dateEnd) {
    var ts = dateStart - dateEnd;
    return ts > 0 ? 1 : ts < 0 ? -1 : 0;
};

cms.util.timeDifference = function (dateStart, dateEnd) {
    var ts = dateStart - dateEnd;
    return ts;
};

cms.util.isTop = function () {
    try {
        return top.location == self.location;
    } catch (e) {
        return false;
    }
};

cms.util.getHost = function (url) {
    url = '' + url;
    var http = url.indexOf('http://') == 0 ? 'http://' : '';
    return http + url.replace('http://', '').split('/')[0];
};

//检测返回结果中是否包含数据库连接故障的文字
cms.util.dbConnection = function (str) {
    return str.indexOf('Unable to connect to') >= 0 ? 0 : 1;
};

//检测用户是否有权限
cms.util.userAuth = function (str) {
    return str.indexOf('noauth') == 0 ? 0 : 1;
};

cms.util.pageCount = function (dataCount, pageSize) {
    return dataCount <= 0 ? 1 : Math.ceil(dataCount / pageSize);
};

//设置文本框回车键提交
cms.util.setKeyEnter = function (arrObj, func) {
    for (var i = 0; i < arrObj.length; i++) {
        if (typeof arrObj[i] == 'object') {
            arrObj[i].onkeypress = function (e) {
                var e = e || event;
                if (e.keyCode == '13') {
                    eval(func)();
                }
            };
        }
    }
};

cms.util.buildTr = function (arrTd, code) {
    var strTr = '<tr' + (code != undefined && code != '' ? ' ' + code : '') + '>';
    for (var i = 0, c = arrTd.length; i < c; i++) {
        strTr += arrTd[i];
    }
    strTr += '</tr>';
    return strTr;
};

cms.util.buildTd = function (html, code) {
    return '<td' + (code != undefined && code != '' ? ' ' + code : '') + '>' + html + '</td>';
};

//设置表格交替行的样式
cms.util.setTableAlternatingRowStyle = function (tbObj, css, showError) {
    if (typeof tbObj != 'object' || tbObj.tagName != 'TABLE' || css == undefined) {
        if (showError) {
            alert('设置表格交替行的样式，参数错误！');
        }
        return false;
    }
    var isClass = css.indexOf(':') < 0;
    if (isClass) {
        for (var i = 0, c = tbObj.rows.length; i < c; i++) {
            if ((i + 1) % 2 == 0) {
                tbObj.rows[i].className = css;
            }
        }
    } else {
        for (var i = 0, c = tbObj.rows.length; i < c; i++) {
            if ((i + 1) % 2 == 0) {
                tbObj.rows[i].style.cssText = css;
            }
        }
    }
};
//设置表格指定某一行的样式
cms.util.setTableRowStyle = function (tbObj, rowIndex, css, showError) {
    if (typeof tbObj != 'object' || tbObj.tagName != 'TABLE' || typeof rowIndex != 'number' || css == undefined) {
        if (showError) {
            alert('设置表格指定行的样式，参数错误！');
        }
        return false;
    }
    var isClass = css.indexOf(':') < 0;
    if (tbObj.rows.length < rowIndex) {
        if (isClass) {
            tbObj.rows[rowIndex].cssName = css;
        } else {
            tbObj.rows[rowIndex].style.cssText = css;
        }
    } else if (showError) {
        alert('指定行的序号超出表格行数！');
    }
};

cms.util.setFocusClearBlankSpace = function (obj) {
    obj.onfocus = function () {
        if (this.value.indexOf(' ') >= 0) {
            this.value = this.value.trim();
        }
    };
};

//禁止事件冒泡
cms.util.stopBubble = function (ev) {
    try {
        ev = ev || window.event || arguments.callee.caller.arguments[0];
        if (ev.stopPropagation) { ev.stopPropagation(); } else { ev.cancelBubble = true; }
        if (ev.preventDefault) { ev.preventDefault(); } else { ev.returnValue = false; }
    } catch (ex) { }
};

/*TAB项过多超出宽度范围时 需要左右滚动*/
cms.util.tabScroll = function (type, obj) {
    cms.util.tabBoxScroll(type, obj, 30);
    cms.util.tab_timer = window.setTimeout(cms.util.tabInterval, 500, type, obj, 20);
};

cms.util.tabBoxScroll = function (type, obj, x) {
    if ('add' == type) {
        obj.scrollLeft += x;
    } else {
        obj.scrollLeft -= x;
    }
};

cms.util.tabInterval = function (type, obj) {
    cms.util.tab_interval = window.setInterval(cms.util.tabBoxScroll, 50, type, obj, 10);
};

cms.util.stopScroll = function () {
    clearTimeout(cms.util.tab_timer);
    clearInterval(cms.util.tab_interval);
};

cms.util.setWindowStatus = function (str) {
    if (str == undefined) {
        window.status = '';
    } else if (str != null) {
        window.status = str;
    }
};

cms.util.repairSelectWidth = function (obj, objRefer) {
    if (obj == undefined || obj == null) {
        obj = 'select';
    }
    if (cms.util.isMSIE && cms.util.ieVersion() < 10) {
        if (typeof objRefer == 'number') {
            var w = parseInt(objRefer, 10);
            $(obj).each(function () {
                $(this).width($(this)[0].offsetWidth + w);
            });
        } else if (typeof objRefer == 'object') {
            try {
                $(obj).each(function () {
                    $(this).width(objRefer[0].offsetWidth);
                });
            } catch (e) { return false; }
        } else if (typeof objRefer == 'string') {
            $(obj).each(function () {
                $(this).width($(objRefer)[0].offsetWidth);
            });
        }
    }
};

//onkeyup onblur onchange
cms.util.showNumberFormat = function (obj, delimiter, separator) {
    var arr = obj.value.trim().replaceAll(' ', '').split('');
    var arrNew = [];
    var len = delimiter.length;
    var n = 0;
    var count = 0;
    var idx = 0;
    var sep = separator || ' ';
    var result = '';
    if (typeof delimiter != 'object') {
        delimiter = [4];
    } else {
        for (var i = 0, c = delimiter.length; i < c; i++) {
            count += delimiter[i];
        }
    }
    if (len > 1) {
        for (var i = 0, c = arr.length; i < c; i++) {
            arrNew.push(arr[i]);
            n++;
            if (n == delimiter[idx] && idx < len) {
                arrNew.push(sep);
                idx++;
                n = 0;
            }
        }
        result = arrNew.join('').substr(0, count + len - 1);
    } else {
        for (var i = 0, c = arr.length; i < c; i++) {
            arrNew.push(arr[i]);
            if (i > 0 && (0 == (i + 1) % delimiter[0])) {
                arrNew.push(sep);
            }
        }
        result = arrNew.join('');
    }

    obj.value = result;

    return result;
};

cms.util.showFileSize = function (fs) {
    var _fs = typeof fs != 'number' ? parseFloat(fs, 10) : fs;
    if (_fs >= 1024 * 1024) {
        return (Math.round(_fs / 1024 / 1024 * 100) / 100) + 'MB';
    } else if (_fs >= 1024) {
        return (Math.round(_fs / 1024 * 100) / 100) + 'KB';
    } else {
        return _fs + '字节';
    }
};

cms.util.each = function (arrEl, action, func) {
    if (typeof arrEl == 'object' && arrEl.length == undefined) {
        arrEl[action] = func;
    } else {
        for (var i = 0, c = arrEl.length; i < c; i++) {
            arrEl[i][action] = func;
        }
    }
};

cms.util.getNumberValue = function (obj, val, isFloat) {
    val = val || 0.0;
    if (null == obj) {
        return typeof val == 'number' ? val : 0.0;
    }
    var objVal = typeof obj == 'string' ? obj : obj.value;

    //值不是数字，+0避免出现NaN
    if (isNaN(parseFloat(objVal, 10))) {
        objVal = val + objVal;
    }

    return isFloat ? parseFloat(objVal, 10) : parseInt(objVal, 10);
};

cms.util.getFloatValue = function (obj, val) {
    return cms.util.getNumberValue(obj, val, true);
};

cms.util.getStringValue = function (obj, val) {
    if (null == obj) {
        return typeof val == 'string' ? val : '';
    }
    return typeof obj == 'string' ? obj.trim() : obj.value.trim();
};

cms.util.getControlValue = function (obj, val, isFloat) {
    if (null == obj) {
        return typeof val == 'string' || typeof val == 'number' ? val : '';
    }
    if (typeof val == 'number') {
        var objVal = typeof obj == 'string' ? obj : obj.value;

        //值不是数字，+0避免出现NaN
        if (isNaN(parseFloat(objVal, 10))) {
            objVal = val + objVal;
        }
        return isFloat ? parseFloat(objVal, 10) : parseInt(objVal, 10);
    }
    return obj.value.trim();
};

cms.util.getControlLang = function (obj, val, isFloat) {
    if (null == obj) {
        return typeof val == 'string' || typeof val == 'number' ? val : '';
    }
    if (typeof val == 'number') {
        var objLang = typeof obj == 'string' ? obj : obj.lang;

        //值不是数字，+0避免出现NaN
        if (isNaN(parseFloat(objLang, 10))) {
            objLang = val + objLang;
        }
        return isFloat ? parseFloat(objLang, 10) : parseInt(objLang, 10);
    }
    return obj.lang.trim();
};

cms.util.setControlValue = function (obj, val) {
    if (obj != null) {
        obj.value = val;
        return true;
    }
    return false;
};

cms.util.setControlLang = function (obj, val) {
    if (obj != null) {
        obj.lang = val;
        return true;
    }
    return false;
};

cms.util.setControlHtml = function (obj, html) {
    if (obj != null) {
        obj.innerHTML = html;
        return true;
    }
    return false;
};

cms.util.isChinese = function (str) {
    if (str == undefined) {
        return 0;
    }
    return str.charCodeAt(0) > 127 || str.charCodeAt(0) == 94;
};

cms.util.getStartEnd = function (arrContent, start, end) {
    var pos = 0;
    var c = arrContent.length;
    for (var i = 0; i < c; i++) {
        if (pos >= start) {
            start = i;
            break;
        }
        pos += cms.util.isChinese(arrContent[i]) ? 2 : 1;
    }
    for (var i = start; i < c; i++) {
        if (i == c - 1) {
            end = i + 1;
        } else if (pos >= end) {
            end = i;
            break;
        }
        pos += cms.util.isChinese(arrContent[i]) ? 2 : 1;
    }
    return [start, end];
};

/*截取字符串*/
cms.util.interceptString = function (strContent, len) {
    if (strContent.len() > len) {
        var arrContent = strContent.split('');
        var pos = cms.util.getStartEnd(arrContent, 0, len)[1];

        return strContent.substr(0, pos) + '...';
    }
    return strContent;
};

//过滤HTML标签
cms.util.filterHtmlTag = function (str) {
    str = str.replace(/<\/?[^>]*>/g, ''); //去除HTML tag
    str = str.replace(/[ | ]*\n/g, '\n'); //去除行尾空白
    str = str.replace(/\n[\s| | ]*\r/g, '\n'); //去除多余空行
    //str=str.replace(/&nbsp;/ig,'');//去掉&nbsp;
    return str;
};

/*
cms.sort.js
七种排序算法:quickSort,heapSort,mergeSort,shellSort,binaryInsertSort,insertSort,bubbleSort,selectSort
基于数字比较大小，若为字符型数字需转换为数字
一般情况下 选择快速排序（quickSort），当数组长度小于10时，可选择插入排序（insertSort)
*/
eval(function(p,a,c,k,e,d){e=function(c){return(c<a?"":e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};if(!''.replace(/^/,String)){while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1;};while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p;}('8 6=6||{};6.7={};6.7.p=g(5){e S 5==\'Y\'||(S 5==\'V\'&&5>=0)};6.7.U=g(4,5){f(0==4.o){e 4};e 6.7.p(5)?6.7.F(4,5):6.7.K(4)};6.7.K=g(4){f(0==4.o){e[]};8 E=[],y=[],z=4[0],c=4.o;9(8 i=1;i<c;i++){4[i]<z?E.J(4[i]):y.J(4[i])};e 6.7.K(E).O(z,6.7.K(y))};6.7.F=g(4,5){f(0==4.o){e[]};8 E=[],y=[],z=4[0],c=4.o;9(8 i=1;i<c;i++){4[i][5]<z[5]?E.J(4[i]):y.J(4[i])};e 6.7.F(E,5).O(z,6.7.F(y,5))};6.7.X=g(4,5){8 c=4.o,b=6.7.p(5);9(8 i=B.A(c/2);i>=0;i--){6.7.L(4,i,c-1,b,5)};9(8 i=c-1;i>=0;i--){6.7.D(4,0,i);6.7.L(4,0,i-1,b,5)};e 4};6.7.L=g(4,s,m,b,5){8 a=4[s];f(b){9(8 j=2*s;j<=m;j*=2){j+=j<m&&4[j][5]<4[j+1][5]?1:0;f(a[5]>=4[j][5]){P};4[s]=4[j],s=j}}v{9(8 j=2*s;j<=m;j*=2){j+=j<m&&4[j]<4[j+1]?1:0;f(a>=4[j]){P};4[s]=4[j],s=j}};4[s]=a};6.7.W=g(4,5){e 6.7.H(4,4,0,4.o-1,6.7.p(5),5)};6.7.H=g(h,l,s,t,b,5){8 m=0,I=[];f(s==t){l[s]=h[s]}v{m=B.A((s+t)/2);6.7.H(h,I,s,m,b,5);6.7.H(h,I,m+1,t,b,5);6.7.G(I,l,s,m,t,b,5)};e l};6.7.G=g(h,l,s,m,n,b,5){8 i=0,k=0;f(b){9(i=m+1,k=s;s<=m&&i<=n;k++){l[k]=h[h[s][5]<h[i][5]?s++:i++]}}v{9(i=m+1,k=s;s<=m&&i<=n;k++){l[k]=h[h[s]<h[i]?s++:i++]}};f(s<=m){9(8 j=0;j<=m-s;j++){l[k+j]=h[s+j]}};f(i<=n){9(8 j=0;j<=n-i;j++){l[k+j]=h[i+j]}}};6.7.13=g(4,5){8 c=4.o,k=1,l=[];C(k<c){6.7.M(4,l,k,c-1,6.7.p(5),5);k=2*k;6.7.M(l,4,k,c-1,6.7.p(5),5);k=2*k};e 4};6.7.M=g(h,l,s,t,b,5){8 i=0,j=0;C(i<=t-2*s+1){6.7.G(h,l,i,i+s-1,i+2*s-1,b,5);i+=2*s};f(i<t-s+1){6.7.G(h,l,i,i+s-1,t,b,5)}v{9(j=i;j<=t;j++){l[j]=h[j]}}};6.7.Z=g(4,5){f(!6.7.p(5)){8 a=0,c=4.o,d=c;Q{d=B.A(d/3)+1;9(8 i=d;i<c;i++){f(4[i]<4[i-d]){a=4[i];9(8 j=i-d;j>=0&&4[j]>a;j-=d){4[j+d]=4[j]};4[j+d]=a}}}C(d>1);e 4}v{e 6.7.R(4,5)}};6.7.R=g(4,5){8 a=0,c=4.o,d=c;Q{d=B.A(d/3)+1;9(8 i=d;i<c;i++){f(4[i][5]<4[i-d][5]){a=4[i];9(8 j=i-d;j>=0&&4[j][5]>a[5];j-=d){4[j+d]=4[j]};4[j+d]=a}}}C(d>1);e 4};6.7.14=g(4,5){8 a=0,c=4.o,b=6.7.p(5);f(b){9(8 i=1;i<c;i++){a=4[i];9(8 j=i;j>0&&4[j-1][5]>a[5];j--){4[j]=4[j-1]};4[j]=a}}v{9(8 i=1;i<c;i++){a=4[i];9(8 j=i;j>0&&4[j-1]>a;j--){4[j]=4[j-1]};4[j]=a}};e 4};6.7.11=g(4,5){8 c=4.o,w=0,r=0,u=0,a=0,b=6.7.p(5);9(8 i=1;i<c;i++){a=4[i];w=0;r=i-1;f(b){C(w<=r){u=B.A((w+r)/2);a[5]>4[u][5]?w=u+1:r=u-1}}v{C(w<=r){u=B.A((w+r)/2);a>4[u]?w=u+1:r=u-1}};9(8 j=i-1;j>r;j--){4[j+1]=4[j]};4[r+1]=a};e 4};6.7.D=g(4,i,j){8 a=4[i];4[i]=4[j];4[j]=a};6.7.10=g(4,5){8 c=4.o,q=0,b=6.7.p(5);f(b){9(8 i=0;i<c-1;i++){q=i;9(8 j=i+1;j<c;j++){4[q][5]>4[j][5]?q=j:0};i!=q?6.7.D(4,i,q):0}}v{9(8 i=0;i<c-1;i++){q=i;9(8 j=i+1;j<c;j++){4[q]>4[j]?q=j:0};i!=q?6.7.D(4,i,q):0}};e 4};6.7.12=g(4,5){8 c=4.o,x=N,b=6.7.p(5);f(b){9(8 i=0;i<c-1&&x;i++){x=T;9(8 j=c-1;j>i;j--){4[j][5]<4[j-1][5]?x=6.7.D(4,j,j-1)||N:0}}}v{9(8 i=0;i<c-1&&x;i++){x=T;9(8 j=c-1;j>i;j--){4[j]<4[j-1]?x=6.7.D(4,j,j-1)||N:0}}};e 4};',62,67,'||||arr|key|cms|sort|var|for|tmp|hasKey||increment|return|if|function|arrSource||||arrTarget|||length|checkKey|min|high|||mid|else|low|flag|right|pivot|floor|Math|while|swap|left|_quickSortKey|_merge|_mergeSort|arrTarget2|push|_quickSort|heapAdjust|_mergeSort2|true|concat|break|do|shellSortKey|typeof|false|quickSort|number|mergeSort|heapSort|string|shellSort|selectSort|binaryInsertSort|bubbleSort|mergeSort2|insertSort'.split('|'),0,{}))
/*
json2.js

JSON.stringify(object);
JSON.parse(str);
*/
eval(function(p,a,c,k,e,d){e=function(c){return(c<a?"":e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};if(!''.replace(/^/,String)){while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1;};while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p;}('3(6 h!==\'x\'){h={}};1X=h;(5(){\'1E 1M\';z 1y=/^[\\],:{}\\s]*$/,1c=/\\\\(?:["\\\\\\/1I]|u[0-1O-1K-F]{4})/g,1b=/"[^"\\\\\\n\\r]*"|1L|1J|C|-?\\d+(?:\\.\\d*)?(?:[1P][+\\-]?\\d+)?/g,18=/(?:^|:|,)(?:\\s*\\[)+/g,P=/[\\\\\\"\\1t-\\1N\\1H-\\1C\\1q\\1x-\\1z\\1A\\1B\\1o\\1a-\\19\\17-\\1d\\1j-\\1n\\1m\\1g-\\1i]/g,R=/[\\1t\\1q\\1x-\\1z\\1A\\1B\\1o\\1a-\\19\\17-\\1d\\1j-\\1n\\1m\\1g-\\1i]/g;5 f(n){7 n<10?\'0\'+n:n};5 O(){7 q.1k()};3(6 1h.o.w!==\'5\'){1h.o.w=5(){7 1l(q.1k())?q.1F()+\'-\'+f(q.1G()+1)+\'-\'+f(q.1D())+\'T\'+f(q.20())+\':\'+f(q.21())+\':\'+f(q.1Z())+\'Z\':C};23.o.w=O;1S.o.w=O;M.o.w=O};z 8,I,X,l;5 N(m){P.1s=0;7 P.11(m)?\'"\'+m.G(P,5(a){z c=X[a];7 6 c===\'m\'?c:\'\\\\u\'+(\'1p\'+a.1r(0).14(16)).1w(-4)})+\'"\':\'"\'+m+\'"\'};5 D(y,A){z i,k,v,e,H=8,9,2=A[y];3(2&&6 2===\'x\'&&6 2.w===\'5\'){2=2.w(y)};3(6 l===\'5\'){2=l.L(A,y,2)};1T(6 2){J\'m\':7 N(2);J\'U\':7 1l(2)?M(2):\'C\';J\'1Q\':J\'C\':7 M(2);J\'x\':3(!2){7\'C\'};8+=I;9=[];3(S.o.14.1Y(2)===\'[x 1W]\'){e=2.e;E(i=0;i<e;i+=1){9[i]=D(i,2)||\'C\'};v=9.e===0?\'[]\':8?\'[\\n\'+8+9.Q(\',\\n\'+8)+\'\\n\'+H+\']\':\'[\'+9.Q(\',\')+\']\';8=H;7 v};3(l&&6 l===\'x\'){e=l.e;E(i=0;i<e;i+=1){3(6 l[i]===\'m\'){k=l[i];v=D(k,2);3(v){9.15(N(k)+(8?\': \':\':\')+v)}}}}Y{E(k 1v 2){3(S.o.1u.L(2,k)){v=D(k,2);3(v){9.15(N(k)+(8?\': \':\':\')+v)}}}};v=9.e===0?\'{}\':8?\'{\\n\'+8+9.Q(\',\\n\'+8)+\'\\n\'+H+\'}\':\'{\'+9.Q(\',\')+\'}\';8=H;7 v}};3(6 h.W!==\'5\'){X={\'\\b\':\'\\\\b\',\'\\t\':\'\\\\t\',\'\\n\':\'\\\\n\',\'\\f\':\'\\\\f\',\'\\r\':\'\\\\r\',\'"\':\'\\\\"\',\'\\\\\':\'\\\\\\\\\'};h.W=5(2,B,K){z i;8=\'\';I=\'\';3(6 K===\'U\'){E(i=0;i<K;i+=1){I+=\' \'}}Y 3(6 K===\'m\'){I=K};l=B;3(B&&6 B!==\'5\'&&(6 B!==\'x\'||6 B.e!==\'U\')){1f 1e 1U(\'h.W\')};7 D(\'\',{\'\':2})}};3(6 h.V!==\'5\'){h.V=5(p,12){z j;5 13(A,y){z k,v,2=A[y];3(2&&6 2===\'x\'){E(k 1v 2){3(S.o.1u.L(2,k)){v=13(2,k);3(v!==1V){2[k]=v}Y{1R 2[k]}}}};7 12.L(A,y,2)};p=M(p);R.1s=0;3(R.11(p)){p=p.G(R,5(a){7\'\\\\u\'+(\'1p\'+a.1r(0).14(16)).1w(-4)})};3(1y.11(p.G(1c,\'@\').G(1b,\']\').G(18,\'\'))){j=24(\'(\'+p+\')\');7 6 12===\'5\'?13({\'\':j},\'\'):j};1f 1e 22(\'h.V\')}}}());',62,129,'||value|if||function|typeof|return|gap|partial|||||length|||JSON2||||rep|string||prototype|text|this||||||toJSON|object|key|var|holder|replacer|null|str|for||replace|mind|indent|case|space|call|String|quote|this_value|rx_escapable|join|rx_dangerous|Object||number|parse|stringify|meta|else|||test|reviver|walk|toString|push||u2028|rx_four|u200f|u200c|rx_three|rx_two|u202f|new|throw|ufff0|Date|uffff|u2060|valueOf|isFinite|ufeff|u206f|u17b5|0000|u00ad|charCodeAt|lastIndex|u0000|hasOwnProperty|in|slice|u0600|rx_one|u0604|u070f|u17b4|u009f|getUTCDate|use|getUTCFullYear|getUTCMonth|u007f|bfnrt|false|fA|true|strict|u001f|9a|eE|boolean|delete|Number|switch|Error|undefined|Array|JSON|apply|getUTCSeconds|getUTCHours|getUTCMinutes|SyntaxError|Boolean|eval'.split('|'),0,{}))
/*
base64.js
    
base64encode(str);
base64decode(str);
*/
eval(function(p,a,c,k,e,d){e=function(c){return(c<a?"":e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};if(!''.replace(/^/,String)){while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\w+'};c=1;};while(c--)if(k[c])p=p.replace(new RegExp('\\b'+e(c)+'\\b','g'),k[c]);return p;}('n g="U+/";n q=V W(-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,T,-1,-1,-1,Q,R,S,X,1c,1d,1e,1b,Y,Z,v,-1,-1,-1,-1,-1,-1,-1,0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,1a,C,E,B,D,F,-1,-1,-1,-1,-1,-1,M,L,N,P,O,H,G,I,K,J,1t,1v,1w,1x,1u,1r,1s,1C,1B,1A,1z,1y,1i,1j,1k,1f,-1,-1,-1,-1,-1);y 1n(f){n a,i,d;n c,e,b;d=f.x;i=0;a="";m(i<d){c=f.l(i++)&p;j(i==d){a+=g.h(c>>2);a+=g.h((c&w)<<4);a+="==";o};e=f.l(i++);j(i==d){a+=g.h(c>>2);a+=g.h(((c&w)<<4)|((e&z)>>4));a+=g.h((e&A)<<2);a+="=";o};b=f.l(i++);a+=g.h(c>>2);a+=g.h(((c&w)<<4)|((e&z)>>4));a+=g.h(((e&A)<<2)|((b&1m)>>6));a+=g.h(b&1l)};r a};y 1q(f){n c,e,b,k;n i,d,a;d=f.x;i=0;a="";m(i<d){s{c=q[f.l(i++)&p]}m(i<d&&c==-1);j(c==-1)o;s{e=q[f.l(i++)&p]}m(i<d&&e==-1);j(e==-1)o;a+=t.u((c<<2)|((e&1p)>>4));s{b=f.l(i++)&p;j(b==v)r a;b=q[b]}m(i<d&&b==-1);j(b==-1)o;a+=t.u(((e&1o)<<4)|((b&1h)>>2));s{k=f.l(i++)&p;j(k==v)r a;k=q[k]}m(i<d&&k==-1);j(k==-1)o;a+=t.u(((b&1g)<<6)|k)};r a}',62,101,'||||||||||out|c3|c1|len|c2|str|base64EncodeChars|charAt||if|c4|charCodeAt|while|var|break|0xff|base64DecodeChars|return|do|String|fromCharCode|61|0x3|length|function|0xF0|0xF|23|21|24|22|25|32|31|33|35|34|27|26|28|30|29|63|52|53|62|ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789|new|Array|54|59|60|||||||||||20|58|55|56|57|51|0x03|0x3C|48|49|50|0x3F|0xC0|base64encode|0XF|0x30|base64decode|41|42|36|40|37|38|39|47|46|45|44|43'.split('|'),0,{}))
