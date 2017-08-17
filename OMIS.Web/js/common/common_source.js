var _st = window.setTimeout;
window.setTimeout = function (fRef, mDelay) {
    if (typeof fRef == 'function') {
        var argu = Array.prototype.slice.call(arguments, 2);
        var f = (function () {
            fRef.apply(null, argu);
        });
        return _st(f, mDelay);
    }
    return _st(fRef, mDelay);
};

var _si = window.setInterval;
window.setInterval = function (fRef, mDelay) {
    if (typeof fRef == 'function') {
        var argu = Array.prototype.slice.call(arguments, 2);
        var f = (function () {
            fRef.apply(null, argu);
        });
        return _si(f, mDelay);
    }
    return _si(fRef, mDelay);
};

var string = String;
string.empty = '';

String.prototype.trim = function () {
    return this.replace(/(^[\s]*)|([\s]*$)/g, '');
};

String.prototype.ltrim = function () {
    return this.replace(/(^[\s]*)/g, '');
};

String.prototype.rtrim = function () {
    return this.replace(/([\s]*$)/g, '');
};

String.prototype.equals = function (s) {
    return this.replace(s, 'a') == 'a' ? true : false;
};

String.prototype.len = function () {
    return this.replace(/([^\x00-\xff])/g, 'aa').length;
};

String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, 'gm'), s2);
};

String.prototype.padLeft = function (totalWidth, paddingChar) {
    var s = this;
    if (paddingChar == undefined) {
        paddingChar = '0';
    }
    for (var i = 0, c = totalWidth - s.length; i < c; i++) {
        s = paddingChar + s;
    }
    return s;
};

String.prototype.padRight = function (totalWidth, paddingChar) {
    var s = this;
    if (paddingChar == undefined) {
        paddingChar = '0';
    }
    for (var i = 0, c = totalWidth - s.length; i < c; i++) {
        s += paddingChar;
    }
    return s;
};

Number.prototype.padLeft = function (totalWidth, paddingChar) {
    return ('' + this).padLeft(totalWidth, paddingChar);
};

Number.prototype.padRight = function (totalWidth, paddingChar) {
    return ('' + this).padRight(totalWidth, paddingChar);
};

String.prototype.toDate = function () {
    return new Date(Date.parse(this.replace(/-/g, "/")));
};

String.prototype.toNumber = function (defaultValue) {
    var num = parseFloat(this, 10);
    return isNaN(num) ? (defaultValue || 0) : num;
};

function $I(id) {
    if (0 == id.indexOf('#')) {
        id = id.substr(1);
    }
    if (id.indexOf(',') < 0) {
        return document.getElementById(id);
    } else {
        var obj = document.getElementById(id);
        if (obj != null) {
            return obj;
        } else {
            var ids = id.split(',');
            var arr = [];
            for (var i in ids) {
                var sid = ids[i].substr(0 == ids[i].indexOf('#') ? 1 : 0);
                var obj = document.getElementById(sid);
                if (obj != null) {
                    arr.push(obj);
                }
            }
            return 0 == arr.length ? null : 1 == arr.length ? arr[0] : arr;
        }
    }
}

function $ID(id) {
    return $I(id);
}

/*
通过Name获得控件元素
name: 名称，
tag(tagName:type): 控件标签，如：div,table,span,label,p,li,select,input:text|checkbox|radio|password|hidden|file|image|button|reset|submit等
parent: 父容器
*/
function $N(name, tag, parent, val) {
    var el = document.getElementsByName(name);
    if (el.length > 0) {
        if (typeof tag == 'string' && tag != '') {
            var arr = tag.split(':');
            if (arr[0] != '') {
                el = $CheckTag(el, arr[0]);
            }
            if (arr.length > 1 && arr[1] != '') {
                el = $CheckType(el, arr[1]);
            }
        }
        if (typeof parent != 'undefined') {
            el = $CheckParent(el, parent);
        }
        if (typeof val == 'string') {
            el = $CheckValue(el, val);
        }
    }
    return el;
}

/*
通过TagName获得控件元素
tag(tagName:type):控件标签，如：div,table,span,label,p,li,select,input:text|checkbox|radio|password|hidden|file|image|button|reset|submit等
parent: 父容器
*/
function $T(tag, parent) {
    /*
    var arr = typeof tag == 'string' ? tag.split(':') : [];
    var el = document.getElementsByTagName(arr[0]);
    if (el.length > 0) {
    if (arr.length > 1 && arr[1] != '') {
    el = $CheckType(el, arr[1]);
    }
    if (typeof parent != 'undefined') {
    el = $CheckParent(el, parent);
    }
    }
    return el;
    */
    var arr = typeof tag == 'string' ? tag.split(':') : [];
    if (typeof parent != 'object' || parent == null) {
        parent = document;
    }
    var el = parent.getElementsByTagName(arr[0]);
    if (el.length > 0) {
        if (arr.length > 1 && arr[1] != '') {
            el = $CheckType(el, arr[1]);
        }
    }
    return el;
}

/*
通过ClassName获得控件元素
className: 样式名称
tag(tagName:type): 控件标签，如：div,table,span,label,p,li,select,input:text|checkbox|radio|password|hidden|file|image|button|reset|submit等
parent: 父容器
*/
function $C(className, tag, parent) {
    if (typeof className == 'undefined') {
        return [];
    } else {
        className = className.replace(/[.]/g, '');
        if (0 == className.length) {
            return [];
        }
    }
    var arr = typeof tag == 'string' ? tag.split(':') : [];
    /*
    var el = document.getElementsByTagName(arr[0] || '*');
    if (el.length > 0) {
    if (arr.length > 1 && arr[1] != '') {
    el = $CheckType(el, arr[1]);
    }
    if (typeof parent != 'undefined') {
    el = $CheckParent(el, parent);
    }
    }
    */
    if (typeof parent != 'object' || parent == null) {
        parent = document;
    }
    var el = parent.getElementsByTagName(arr[0] || '*');
    if (el.length > 0) {
        if (arr.length > 1 && arr[1] != '') {
            el = $CheckType(el, arr[1]);
        }
    }
    return $CheckClassName(el, className);
}

function $CheckParent(_el, parent) {
    var pNode = typeof parent == 'undefined' ? null : typeof parent == 'string' ? document.getElementById(parent) : parent;
    if (pNode != null) {
        var el = [];
        for (var i = 0, c = _el.length; i < c; i++) {
            if (_el[i].parentNode == pNode) {
                el.push(_el[i]);
            }
        }
        return el;
    }
    return _el;
}

function $CheckType(_el, type) {
    if (typeof type == 'undefined' || '' == type) {
        return _el;
    }
    type = type.toLowerCase();
    var el = [];
    for (var i = 0, c = _el.length; i < c; i++) {
        alert(_el[i].type);
        if (_el[i].type == type) {
            el.push(_el[i]);
        }
    }
    return el;
}

function $CheckTag(_el, tagName) {
    if (typeof tagName == 'undefined' || '' == tagName) {
        return _el;
    }
    tagName = tagName.toUpperCase();
    var el = [];
    for (var i = 0, c = _el.length; i < c; i++) {
        if (_el[i].tagName == tagName) {
            el.push(_el[i]);
        }
    }
    return el;
}

function $CheckClassName(_el, className) {
    if (typeof className == 'undefined' || '' == className) {
        return _el;
    }
    var el = [];
    for (var i = 0, c = _el.length; i < c; i++) {
        var css = _el[i].className.split(' ');
        for (var j in css) {
            if (css[j] == className) {
                el.push(_el[j]);
                break;
            }
        }
    }
    return el;
}

function $CheckValue(_el, val) {
    if (typeof val != 'string') {
        return _el;
    }
    var el = [];
    for (var i = 0, c = _el.length; i < c; i++) {
        if (_el[i].value == val) {
            el.push(_el[i]);
        }
    }
    return el;
}

Date.prototype.toArray = function () {
    var year = this.getFullYear();
    if (year < 1900) { year += 1900; }
    var d = ['' + year,
        ('' + (this.getMonth() + 1)).padLeft(2, '0'),
        ('' + this.getDate()).padLeft(2, '0'),
        ('' + this.getHours()).padLeft(2, '0'),
        ('' + this.getMinutes()).padLeft(2, '0'),
        ('' + this.getSeconds()).padLeft(2, '0'),
        ('' + this.getMilliseconds()).padLeft(3, '0')
    ];
    return d;
};

Date.prototype.toString = function (format) {
    var year = this.getFullYear();
    if (isNaN(year)) {
        return '';
    }
    if (year < 1900) { year += 1900; }
    var d = ['' + year,
        ('' + (this.getMonth() + 1)).padLeft(2, '0'),
        ('' + this.getDate()).padLeft(2, '0'),
        ('' + this.getHours()).padLeft(2, '0'),
        ('' + this.getMinutes()).padLeft(2, '0'),
        ('' + this.getSeconds()).padLeft(2, '0'),
        ('' + this.getMilliseconds()).padLeft(3, '0')
    ];
    switch (format) {
        case 'timestamp':
        case 'ts':
            return parseInt(this.valueOf() / 1000, 10);
            break;
        case 'yyyyMM':
            return d[0] + d[1];
            break;
        case 'yyyyMMdd':
            return d[0] + d[1] + d[2];
            break;
        case 'yyyy-MM-dd':
            return d[0] + '-' + d[1] + '-' + d[2];
            break;
        case 'yyyy-MM-dd HH':
            return d[0] + '-' + d[1] + '-' + d[2] + ' ' + d[3];
            break;
        case 'yyyy-MM-dd HH:mm':
            return d[0] + '-' + d[1] + '-' + d[2] + ' ' + d[3] + ':' + d[4];
            break;
        case 'yyyy-MM-dd HH:mm:ss fff':
            return d[0] + '-' + d[1] + '-' + d[2] + ' ' + d[3] + ':' + d[4] + ':' + d[5] + ' ' + d[6];
            break;
        case 'yyyyMMddHHmmssfff':
            return d[0] + d[1] + d[2] + d[3] + d[4] + d[5] + d[6];
            break;
        case 'yyyy-MM-dd HH:mm:ss':
        default:
            return d[0] + '-' + d[1] + '-' + d[2] + ' ' + d[3] + ':' + d[4] + ':' + d[5];
            break;
        case '年月日':
            return d[0] + '年' + d[1] + '月' + d[2] + '日';
            break;
    }
};

Date.prototype.toWeekDay = function () {
    var wd = this.getDay();
    var arr = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
    return arr[wd];
};

Date.prototype.toTimeStamp = function () {
    return parseInt(this.valueOf() / 1000, 10);
};

Date.prototype.timeDifference = function (date) {
    return this - date;
};

Date.prototype.dateDifference = function (date) {
    return parseInt((this - date) / 86400 / 1000, 10);
};

Date.prototype.addDays = function (days) {
    this.setDate(this.getDate() + days);
    return this;
};

//时间格式字符串转换分钟数
String.prototype.toMinuteNumber = function () {
    var _m = this.split(':');
    var m = 0;
    var mu = [60, 1];
    for (var i in _m) {
        if (i > 1) { break; }
        m += parseInt('0' + _m[i], 10) * mu[i];
    }
    return m;
};

//时间格式字符串转换秒数
String.prototype.toSecondNumber = function () {
    var _s = this.split(':');
    var s = 0;
    var mu = [3600, 60, 1];
    for (var i in _s) {
        if (i > 2) { break; }
        s += parseInt('0' + _s[i], 10) * mu[i];
    }
    return s;
};


String.prototype.append = function (str) {
    return this + str;
};

String.prototype.insert = function (str) {
    return str + this;
};

function StringBuild() {
    this.list = [];
    this.length = 0;
}
StringBuild.prototype = {
    initial: function () {
        if (this.length <= 0) {
            this.list = null;
            this.list = [];
        }
    },
    append: function (str) {
        this.initial();
        this.list.push(str);
        this.length = this.list.length;
    },
    insert: function (idx, str) {
        this.initial();

        this.length = this.list.length;
    },
    clear: function () {
        this.list = null;
        this.list = [];
    },
    toString: function () {
        this.initial();
        return this.list.join('');
    }
};

String.prototype.instr = function (par) {
    var s = this;
    if (typeof par == 'object') {
        for (var i in par) {
            if ('' + par[i] == s) {
                return true;
            }
        }
    } else if (par.indexOf(',') >= 0) {
        var arr = par.split(',');
        for (var i in arr) {
            if ('' + arr[i] == s) {
                return true;
            }
        }
    } else {
        return '' + par == s;
    }
    return false;
};

String.prototype.format = function (args) {
    var s = this, sep = '%s', vals = [], rst = [];
    if (arguments.length > 1) {
        for (var i = 0, c = arguments.length; i < c; i++) {
            if (arguments[i] != undefined) {
                vals.push(arguments[i]);
            }
        }
    } else if (Object.prototype.toString.call(args) === '[object Array]') {
        vals = args;
    } else if (args != undefined && args != null) {
        vals.push(args);
    }
    var arr = s.split(sep);
    for (var i = 0, c = arr.length; i < c; i++) {
        rst.push(arr[i]);
        if (i < c - 1) {
            rst.push(vals[i]);
        }
    }
    return rst.join('');
};

!function () {
    String.prototype.format2 = function (args) {
        //console.log(this);
        var s = this, vals = [], rst = [], pattern = /({|})/g, ms = s.match(pattern);
        var err = ['输入字符串的格式不正确。', '索引(从零开始)必须大于或等于零，且小于参数列表的大小。', '值不能为null（或undefined）。', '格式说明符无效。'];

        if (arguments.length > 1) {
            for (var i = 0, c = arguments.length; i < c; i++) {
                if (arguments[i] !== undefined && arguments[i] !== null) {
                    vals.push(arguments[i]);
                } else {
                    var err = err[2] + '第' + (i + 1) + '个参数值为：' + arguments[i];
                    _throwFormatError(err, s, args);
                }
            }
        } else if (_objectType(args) === '[object Array]') {
            vals = args;
        } else if (args != undefined && args != null) {
            vals.push(args);
        }

        if (ms.length % 2 != 0) {
            _throwFormatError(err[0], s, vals);
        }

        //var matchs = s.match(/({+[-\d]+(:[\D\d]*?)*?}+)|({+([\D]*?|[:\d]*?)}+)|([{]{1,2}[\w]*?)|([\w]*?[}]{1,2})/g);
        var matchs = s.match(/({+[-\d]+(:[\D\d]*?)*?}+)|({+([\D]*?|[:\d]*?)}+)|({+([\w\.\|]*?)}+)|([{]{1,2}[\w]*?)|([\w]*?[}]{1,2})/g);
        if (matchs == null) {
            return s;
        }

        var len = vals.length, isObject = typeof vals[0] == 'object', obj = isObject ? vals[0] : {};

        for (var i = 0, c = matchs.length; i < c; i++) {
            var _t = matchs[i], _tv = _t.replace(pattern, ''), _p = s.indexOf(_t), idx = parseInt(_tv, 10);
            var _c = /{/g.test(_t) ? _t.match(/{/g).length : 0, _d = /}/g.test(_t) ? _t.match(/}/g).length : 0;
            if ((_c + _d) % 2 != 0) {
                _throwFormatError(err[0], s, vals);
            }
            var _t1 = _t.replace(/{{/g, '{').replace(/}}/g, '}');
            var _odd = _c % 2 != 0 || _d % 2 != 0, _single = _c <= 2 && _d <= 2;

            if (!isNaN(idx)) {
                var _v = _formatNumber(_tv, vals[idx], err, _throwFormatError, s, vals);
                if (typeof _v == 'boolean' && !_v) {
                    return false;
                }
                if (/^-\d$/g.test(_tv) && _odd) {
                    _throwFormatError(err[0], s, vals);
                } else if (idx >= len) {
                    _throwFormatError(err[1], s, vals);
                } else if (_v == null || _v == undefined) {
                    _throwFormatError(err[2], s, vals);
                }
                rst.push(s.substr(0, _p) + (_c > 1 || _d > 1 ? (_c % 2 != 0 || _d % 2 != 0 ? _t1.replace('{' + idx + '}', _v) : _t1) : _v));
            } else if (_odd) {
                /*
                if (isObject && vals[0][_tv] != undefined && _single) {
                    _v = vals[0][_tv];
                    rst.push(s.substr(0, _p) + (_c > 1 || _d > 1 ? (_c % 2 != 0 || _d % 2 != 0 ? _t1.replace('{' + idx + '}', _v) : _t1) : _v));
                } else {
                    _throwFormatError(err[0], s, vals);
                }
                */
                if (_c == 1 && _d == 1) {
                    if (!isObject || !_single) {
                        _throwFormatError(err[0], s, vals);
                    }
                    _v = _distillObjVal(_tv, obj, _throwFormatError);
                    rst.push(s.substr(0, _p) + (_c > 1 || _d > 1 ? (_c % 2 != 0 || _d % 2 != 0 ? _t1.replace('{' + idx + '}', _v) : _t1) : _v));
                } else {
                    var _mcs = _t1.match(/({[\w\.\|]+})/g);
                    if (_mcs != null && _mcs.length > 0) {
                        rst.push(s.substr(0, _p) + _t1.replace(_mcs[0], _distillObjVal(_mcs[0].replace(/({|})/g, ''), obj, _throwFormatError)));
                    } else {
                        _throwFormatError(err[0], s, vals);
                    }
                }
            } else {
                rst.push(s.substr(0, _p) + _t1);
            }
            s = s.substr(_p + _t.length);
        }
        rst.push(s);

        return rst.join('');
    };

    //简单的以%s作为格式符，格式也比较简单
    String.prototype.format1 = function (args) {
        var s = this, sep = '%s', vals = [], rst = [];
        if (arguments.length > 1) {
            for (var i = 0, c = arguments.length; i < c; i++) {
                if (arguments[i] != undefined) {
                    vals.push(arguments[i]);
                }
            }
        } else if (_objectType(args) === '[object Array]') {
            vals = args;
        } else if (args != undefined && args != null) {
            vals.push(args);
        }
        var arr = s.split(sep);
        for (var i = 0, c = arr.length; i < c; i++) {
            rst.push(arr[i]);
            if (i < c - 1) {
                rst.push(vals[i]);
            }
        }
        return rst.join('');
    };

    function _throwFormatError(err, str, args) {
        try {
            console.trace();
            if (typeof str != 'undefined') {
                console.log('str:\r\n\t', str, '\r\nargs:\r\n\t', args);
            }
        } catch (e) { }
        throw new Error(err);
    }

    function _formatNumberZero(_arv, _arn) {
        var _arr = [], _idx = _arn.length - 1;
        for (var i = _arv.length - 1; i >= 0; i--) {
            _arr.push(_arv[i] == '0' ? (_idx >= 0 ? _arn[_idx] : _arv[i]) : (function () { ++_idx; return _arv[i]; })());
            _idx--;
        }
        for (var i = _idx; i >= 0; i--) {
            _arr.push(_arn[i]);
        }
        _arr = _arr.reverse();
        return _arr.join('');
    }

    function _formatNumberSwitch(_v, _f1, _f, _n, _dn, err, func, str, args) {
        if (_dn[1] == undefined && (_f == 'C' || _f == 'F')) {
            _dn[1] = '';
            _v += _n > 0 ? '.' : '';
        }
        switch (_f) {
            case 'C':
                _vc = '¥' + _v;
                var _m = _dn[0].length % 3;
                if (_dn[0].length > 3) {
                    _v = _vc.substr(0, _m + 1);
                    var _pos = _m + 1;
                    while (_pos < _dn[0].length - 1) {
                        _v += ',' + _vc.substr(_pos, 3);
                        _pos += 3;
                    }
                    _v += _vc.substr(_pos);
                } else {
                    _v = _vc;
                }
                for (var i = 0, c = _n - (_dn[1]).length; i < c; i++) { _v += '0'; }
                break;
            case 'D':
                if (/([.])/g.test(_v)) {
                    func(err[3], str, args);
                }
                for (var i = 0, c = _n - ('' + _v).length; i < c; i++) { _v = '0' + _v; }
                break;
            case 'E':
                var _n = _dn[0].length - 1, _fn = parseInt(('' + _v).substr(0, 1), 10), _num = Math.pow(10, _n), _e = Math.pow(10, 5);
                var _cn = (Math.round((_v - _num) / _num * _e) / _e + '').split('.')[1], _ln = '';
                for (var i = ('' + _cn).length; i < 5; i++) { _cn += '0'; }
                for (var i = ('' + _n).length; i < 3; i++) { _n = '0' + _n; }
                _v = _fn + '.' + _cn + _f1 + '+' + _n;
                break;
            case 'F':
                for (var i = 0, c = _n - _dn[1].length; i < c; i++) { _v += '0'; }
                break;
        }
        return _v;
    }

    function _formatNumber(_tv, _v, err, func, str, args) {
        var _hasColon = /[:]/g.test(_tv);
        if (!_hasColon) {
            return _v;
        }
        var _isNum = typeof _v == 'number', _sc = _tv.match(/(:)/g).length;
        if (_sc > 1) {
            if (_isNum) {
                var _nv = Math.round(_v, 10), _pos = _tv.indexOf(':'), _arv = _tv.substr(_pos + 1).split(''), _arn = ('' + _nv).split('');
                _v = _formatNumberZero(_arv, _arn);
            } else {
                _v = _tv.substr(_tv.indexOf(':') + 1);
            }
        } else if (_isNum) {
            var _ss = _tv.split(':')[1];
            var _p1 = /([CDEFG])/ig, _p2 = /([A-Z])/ig, _p3 = /^([CDEFG][\d]+)$/ig, _p4 = /^([A-Z]{1}[\d]+)$/ig;
            if ((_ss.length == 1 && _p1.test(_ss)) || (_ss.length >= 2 && _p3.test(_ss))) {
                var _f1 = _ss.substr(0, 1), _f = _f1.toUpperCase();
                var _n = parseInt(_ss.substr(1), 10) || (_f == 'D' ? 0 : 2), _dn = ('' + _v).split('.');
                _v = _formatNumberSwitch(_v, _f1, _f, _n, _dn, err, func, str, args);
            } else if ((_ss.length == 1 && _p2.test(_ss)) || (_ss.length >= 2 && _p4.test(_ss))) {
                func(err[3], str, args);
            } else if (/([0]+)/g.test(_ss)) {
                var _nv = Math.round(_v, 10), _arv = _ss.split(''), _arn = ('' + _nv).split('');
                _v = _formatNumberZero(_arv, _arn);
            } else {
                _v = _ss;
            }
        }
        return _v;
    }

    function _distillObjVal(key, obj, func) {
        var _v;
        if (obj[key] != undefined) {
            _v = obj[key];
        } else if (key.indexOf('.') > 0 || key.indexOf('|') > 0) {
            //嵌套对象，格式: obj.key.key|dv(默认值，因某些key可能不存在或允许为空)
            var _arr = key.split('|'), _dv = _arr[1], _ks = _arr[0].split('.'), _o = obj;
            for (var _i in _ks) {
                _o = _o[_ks[_i]], _v = _o;
                if (typeof _o == 'undefined') {
                    _v = typeof _dv != 'undefined' ? _dv : func(err[0], s, vals);
                }
            }
        } else {
            func(err[0], s, vals);
        }
        return _v;
    }

    function _objectType(obj) {
        return Object.prototype.toString.call(obj);
    }

    String.format = String.Format = function (str) {
        if (typeof str == 'string') {
            var arr = [];
            for (var i = 1; i < arguments.length; i++) {
                arr.push(arguments[i]);
            }
            return str.format2(arr);
        }
        function type(obj) {
            var type = _objectType(obj);
            return type ? (type.replace(/(\[|\])/g, '').split(' ')[1] || 'object') : typeof obj;
        }
        _throwFormatError(type(str) + '.format is not a function');
    };
}();

//字符串比较大小，当前字符串是否小于目标字符串
String.prototype.compare = function (str1) {
    var pattern = /^[-]?[0-9]+(.[0-9]+)?$/;

    var ns = pattern.test(this) ? parseFloat(this, 10) : this;
    var n1 = pattern.test(str1) ? parseFloat(str1, 10) : str1;

    return !isNaN(ns) && !isNaN(n1) ? ns < n1 : this < str1;
};

String.prototype.setDefault = function (val) {
    var s = this;
    if (typeof val != 'undefined') {
        return '' == s ? val : (typeof val == 'number' ? (s.indexOf('.') >= 0 ? parseFloat(s, 10) : parseInt(s, 10)) : s);
    }
    return s;
};

//onkeyup onblur onchange
String.prototype.showNumberFormat = function (delimiter, separator, keep) {
    var arr = this.trim().replaceAll(' ', '').split('');
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
        result = keep ? arrNew.join('') : arrNew.join('').substr(0, count + len - 1);
    } else {
        for (var i = 0, c = arr.length; i < c; i++) {
            arrNew.push(arr[i]);
            if (i > 0 && (0 == (i + 1) % delimiter[0])) {
                arrNew.push(sep);
            }
        }
        result = arrNew.join('');
    }

    return result;
};

/* char length */
String.prototype.gblen = function () {
    var len = 0;
    for (var i = 0, c = this.length; i < c; i++) {
        if (this.charCodeAt(i) > 127 || this.charCodeAt(i) == 94) {
            len += 2;
        } else {
            len++;
        }
    }
    return len;
};

/* cut char */
String.prototype.gbtrim = function (len, s) {
    var str = '';
    var sp = s || '';
    var len2 = this.len();
    if (len2 <= len) {
        return this;
    }
    len2 = 0;
    len = (len > sp.length) ? len - sp.length : len;
    for (var i = 0, c = this.length; i < c; i++) {
        len2 += this.charCodeAt(i) > 127 || this.charCodeAt(i) == 94 ? 2 : 1;
        if (len2 > len) {
            str += sp;
            break;
        }
        str += this.charAt(i);
    }
    return str;
};

String.prototype.getQueryString = function (name) {
    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
    //var str = window.location.search;
    var str = this.substr(this.indexOf('?'));
    if (str.indexOf('?') >= 0) {
        var par = str.substr(1).match(reg);
        return par != null ? unescape(par[2]) : null;
    }
    return null;
};

String.prototype.getRequestString = function (name) {
    var params = this.substr(this.indexOf('?')); // location.search; //获取字符串中'?'符后的字串
    var arr = [];
    if (params.indexOf('?') >= 0) {
        var strs = params.substr(1).split('&');
        for (var i = 0, c = strs.length; i < c; i++) {
            var pos = strs[i].indexOf('=');
            var key = strs[i].split('=')[0];
            if (key != '') {
                arr[key] = pos > 0 ? unescape(strs[i].substr(pos + 1)) : '';
            }
        }
    }
    return name != undefined ? arr[name] : arr;
};

String.prototype.isNumber = function () {
    return /^\-?[0-9]+\.?([0-9]{0,9})?$/.test(this);
};

String.prototype.isInteger = function () {
    return /^\-?[0-9]+$/.test(this);
};

//千位数显示
String.prototype.toThousand = function () {
    var arr = this.split('.');
    return arr[0].replace(/\B(?=(?:\d{3})+$)/g, ',') + (arr.length > 1 ? '.' + arr[1] : '');
};

//千位数显示
Number.prototype.toThousand = function () {
    var str = '' + this;
    var arr = str.split('.');
    return arr[0].replace(/\B(?=(?:\d{3})+$)/g, ',') + (arr.length > 1 ? '.' + arr[1] : '');
};

//判断当前字符串是否以str开始
String.prototype.startWith = function (str) {
    return this.slice(0, str.length) == str;
};

//判断当前字符串是否以str结束
String.prototype.endWith = function (str) {
    return this.slice(-str.length) == str;
};

String.prototype.isJsonData = function () {
    var s = this;
    if (s == '' || s.length < 2) {
        return false;
    }
    var arr = s.trim().split('');
    var c = arr.length;
    var isJson = (arr[0] == '{' || arr[0] == '[') && (arr[c - 1] == '}' || arr[c - 1] == ']');
    if (isJson) {
        try {
            eval('(' + s + ')');
        } catch (e) {
            return false;
        }
    }
    return isJson;
};

String.prototype.isXmlDom = function () {
    var s = this;
    if (s.length < 7 || !s.startWith('<') || !s.endWith('>')) {
        return false;
    }
    var xmlDoc = null;
    try {
        if (window.ActiveXObject) {
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            if (!xmlDoc) xmldoc = new ActiveXObject("MSXML2.DOMDocument.3.0");
            xmlDoc.async = false;
            xmlDoc.loadXML(s);
        } else if (document.implementation && document.implementation.createDocument) {
            //xmlDoc = document.implementation.createDocument('', '', null);
            //xmlDoc.load(s);
            var domParser = new DOMParser();
            xmlDoc = domParser.parseFromString(s, 'text/xml');
        } else {
            xmlDoc = null;
        }
    } catch (e) {
        var oParser = new DOMParser();
        xmlDoc = oParser.parseFromString(s, "text/xml");
    }
    if (xmlDoc != null && xmlDoc.documentElement != null) {
        var nodeList = xmlDoc.documentElement.childNodes;
        return nodeList.length > 0;
    } else {
        return false;
    }
};

String.prototype.toParam = function () {
    return this.replace(new RegExp("'", 'gm'), "\\'");
};

String.prototype.toParamValue = function () {
    return this.replace(new RegExp("'", 'gm'), "\\'");
};

//转义单引号
String.prototype.escapeSingleQuote = function () {
    return this.replace(new RegExp("'", 'gm'), "\\'");
};

String.prototype.toJsonValue = function () {
    return this.replace(new RegExp('"', 'gm'), '\\"');
};

//转义双引号
String.prototype.escapeDoubleQuote = function () {
    return this.replace(new RegExp('"', 'gm'), '\\"');
};

//转义双引号
String.prototype.escape = function () {
    return escape(this);
};

//转义特殊字符
//服务端 需要 UrlDecode解码
String.prototype.encode = function () {
    return encodeURIComponent(this);
};

//转义特殊字符
//服务端 需要 UrlDecode解码
String.prototype.encodeURI = function () {
    return encodeURIComponent(this);
};

String.prototype.round = function (v) {
    v = v == undefined ? 0 : typeof v == 'number' ? v : parseInt(v, 10);
    var pv = Math.pow(10, v);
    return Math.round(this * pv) / pv;
};


String.prototype.toXML = String.prototype.toXml = function () {
    var s = this;
    var xmlDoc = null;
    try {
        if (window.ActiveXObject) {
            xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
            if (!xmlDoc) xmldoc = new ActiveXObject("MSXML2.DOMDocument.3.0");
            xmlDoc.async = false;
            xmlDoc.loadXML(s);
        } else if (document.implementation && document.implementation.createDocument) {
            //xmlDoc = document.implementation.createDocument('', '', null);
            //xmlDoc.load(s);
            var domParser = new DOMParser();
            xmlDoc = domParser.parseFromString(s, 'text/xml');
        } else {
            xmlDoc = null;
        }
    } catch (e) {
        var oParser = new DOMParser();
        xmlDoc = oParser.parseFromString(s, "text/xml");
    }
    return xmlDoc;
};

String.prototype.toHtmlCode = function () {
    return this.replaceAll('<', '&lt;').replaceAll('>', '&gt;');
};

String.prototype.toHtml = function () {
    return this.replaceAll('\r\n', '<br />').replaceAll('\n', '<br />');
};

String.prototype.toValue = function () {
    return this.replaceAll('<br />', '\r\n');
};

String.prototype.toJson = function () {
    try {
        return JSON.parse(this);
    } catch (e) {
        try {
            return eval('(' + this + ')');
        } catch (ex) {
            return {};
        }
    }
};

String.prototype.toInt = function (val) {
    return /^[-+]?[0-9]+(\.[0-9]+)?$/.test(this) ? parseInt(this) : val;
};

String.prototype.toSingle = function (val) {
    return /^[-+]?[0-9]+(\.[0-9]+)?$/.test(this) ? parseInt(this) : val;
};

String.prototype.urlConnector = function () {
    return this.indexOf('?') >= 0 ? '&' : '?';
};

String.prototype.clearSpace = function (maxSpaceCount) {
    return this.replace(/[ ]/g, '');
};

//移除空格，但不移除换行
//所以这里的正则不用 \s表示空格
String.prototype.removeSpace = function (maxSpaceCount) {
    var s = this;
    if (typeof maxSpaceCount == 'number' && maxSpaceCount >= 0) {
        var strSpace = '';
        for (var j = 0; j < maxSpaceCount; j++) {
            strSpace += ' ';
        }
        var reg = new RegExp('[ ]{' + maxSpaceCount + ',}', 'gi'); // /[ ]{1,}/g;
        return s.replace(reg, strSpace);
    } else {
        return s.replace(/[ ]/g, '');
    }
};

//将连续的多个空格替换成回车换行
String.prototype.convertMultiSpaceToNewline = function (minSpaceNum, maxSpaceNum, delimiter) {
    var s = this;
    var d = delimiter;
    if (typeof minSpaceNum != 'number' || minSpaceNum < 1 || typeof maxSpaceNum != 'number' || maxSpaceNum < 1) {
        return s;
    }
    var arr = (typeof d == 'object' && d[0] != undefined) ? d : typeof d != 'string' ? [' '] : d.split('|');
    for (var i = 0, c = arr.length; i < c; i++) {
        if ('' == arr[i] || typeof arr[i] != 'string') {
            continue;
        }
        for (var j = maxSpaceNum; j >= minSpaceNum; j--) {
            var reg = new RegExp('[' + arr[i] + ']{' + j + ',}', 'gi'); // /[ ]{1,}/g;
            s = s.replace(reg, '\r\n');
        }
    }
    return s;
};

String.prototype.cleanParagraph = function (indent, strIndent) {
    var s = this;
    var arrContent = s.split(/\n|\r\n/g);
    var strSpace = '';
    if (typeof indent == 'number' && indent > 0 && typeof strIndent == 'string') {
        for (var j = 0; j < indent; j++) {
            strSpace += strIndent;
        }
    }
    for (var i = 0, c = arrContent.length; i < c; i++) {
        //清除每个数组元素中文字内容的开头结尾的空格 并填充首行缩进的字符
        arrContent[i] = strSpace + arrContent[i].trim();
    }
    return arrContent.join('\r\n');
};

String.prototype.clearSpecificCharacter = function (chars, delimiter) {
    var s = this;
    return s.replaceAll(chars, typeof delimiter != 'string' ? '' : delimiter);
};

//铁路公里标 转换
Number.prototype.toKilometerPost = function (prefix) {
    prefix = prefix || 'K';
    var arr = this.toString().split('.');
    var num = arr[0];
    var dnum = arr.length >= 2 ? arr[1].padRight(3, '0') : '000';

    return prefix + num + '+' + dnum;
};

String.prototype.toKilometerPost = function (prefix) {
    prefix = prefix || 'K';
    var pattern = /^[-+]?[0-9]+(\.[0-9]+)?$/;
    if (!pattern.test(this)) { return this; }
    var arr = this.split('.');
    var num = arr[0];
    var dnum = arr.length >= 2 ? arr[1].padRight(3, '0') : '000';

    return prefix + num + '+' + dnum;
};

String.prototype.toKilometer = function () {
    var pattern = /^[-+]?[0-9]+(\.[0-9]+)?$/;
    if (pattern.test(this)) {
        return parseFloat(this, 10);
    }
    pattern = /^(K|JYK)(\d+)(\+(\d+)?)?$/i;

    if (!pattern.test(this)) { return -1; }

    var md = this.split('+');
    var num = parseFloat(md[0].replace(/(K|JYK)/i, ''), 10);
    if (md.length > 1) {
        return num + parseFloat(parseInt('0' + md[1], 10) / 1000, 10);
    } else {
        return num;
    }
};

String.prototype.isKilometerPost = function () {
    var s = this;
    var pattern = /^(K|JYK)(\d+)(\+(\d+)?)?$/i;
    if (pattern.test(s)) {
        s = s.toKilometer().toString();
    }
    pattern = /^[-+]?[0-9]+(\.[0-9]+)?$/;
    return pattern.test(s);
};

String.prototype.getFileName = function () {
    var arr = this.split('/');
    return arr[arr.length - 1];
};

String.prototype.getExtension = function () {
    if (this.indexOf('.') < 0) {
        return '';
    }
    var arr = this.split('.');
    return arr[arr.length - 1];
};

String.prototype.timeToNumberArray = function () {
    var arr = this.split(/[\/\s-:]/g);
    for (var i = 0, c = arr.length; i < c; i++) {
        arr[i] = parseInt(arr[i], 10);
    }
    return arr;
};

String.prototype.timeToNumber = function () {
    var arr = this.split(/[\/\s-:]/g);
    for (var i = 0, c = arr.length; i < c; i++) {
        if (parseInt(arr[i], 10) < 10) {
            arr[i] = '0' + arr[i];
        }
    }
    return parseInt(arr.join(''), 10);
};

String.prototype.timeToMinute = function () {
    if (!/[\d]{2}[:]?[\d]{2}/.test(this)) {
        return -1;
    }
    var h, m;
    if (this.indexOf(':') > 0) {
        var arr = this.split(':');
        h = parseInt(arr[0], 10), m = parseInt(arr[1], 10);
    } else {
        h = parseInt(this.substr(0, 2), 10), m = parseInt(this.substr(2), 10);
    }
    var t = h * 60 + m;
    return t > 1440 ? -1 : t;
};

//处理console兼容性
var console = console || { log: function () { }, trace: function () { } };
if (typeof console.dir == 'undefined') {
    var ks = [
        'dir', 'count', 'info', 'error', 'warn', 'debug', 'dirxml', 'assert', 'time', 'timeEnd',
        'timeLine', 'timeLineEnd', 'group', 'groupEnd', 'profile', 'profileEnd', 'table'
    ];
    for (var i in ks) {
        console[ks[i]] = function () { };
    }
}