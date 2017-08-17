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

/* 
    ''.format2  String.format  String.Format  ''.format1(%s方式的简单版) 
    示例：
    var str = '{0:D4},{1}';
    console.log(String.format(str, 20, 'abc'));
    console.log(String.Format(str, 20, 'abc'));

    //''.format1 方法
    console.log('你好，%s，今天是%s'.format1('小明', '星期天'));

    //''.format2 方法
    console.log('{0:D4}'.format2(20));
    var str = '你好:{0}，这是用{0}写的一个仿C#的{1}函数';
    console.log(str.format2(['JS','format']));
    console.log(str.format2('JS','format'));
    console.log("{0}{{正则{0:F5}{{表达式}}{1:F4}".format2(123.5, 12.5))
    console.log("{0}{{正则{0:F5}{{表达式}}{1:0D12}".format2(123.5, 2))
    console.log("{0:00:00:00}".format2(1234567));
    console.log("{0:0000年00月00日 00:00:00}".format2(20160824172215));
    console.log("{0:0000年00月00日}".format2(20160824));
    console.log("{0:0DF5}".format2(1234567));
    console.log("{0:0000}".format2(2));
    console.log('<tr lang="{{id:{0},pid:{1},level:{2}}}">'.format2([1,2,0]));

    //对象嵌套使用，可以设置默认值（字符串）
    var data = {id:123, name:'张三', obj:{num:321, con:{val:'acc'}}};
    var s = 'Id:{id}, Val:{obj.con.val3|asd}, Code:{1}, Name:{name}'.format2(data, 'Test');
    console.log(s);
    var s = 'Id:{id}, Val:{obj.con.val3|12}, Code:{1}, Name:{name}'.format2(data, 'Test');
    console.log(s);
    var s = 'Id:{id}, Val:{obj.con.val}, Code:{1}, Num:{obj.num}, Name:{name}'.format2(data, 'Test');
    console.log(s);

    var s = 'lang="{{id:{obj.id},pid:{obj.pid},level:{obj.level}}}"'.format2({obj:{id:12, pid:1, level: 1}});
    console.log(s);
    var s = 'lang="{{id:{id},pid:{pid},level:{level}}}"'.format2({id:12, pid:1, level: 1});
    console.log(s);
    var s = 'lang="{{id:{id},pid:{pid},level:{level|0}}}"'.format2({id:12, pid:0});
    console.log(s);
*/
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

/* JSON */
if (typeof JSON2 !== 'object') {
    JSON2 = {};
}
if (typeof JSON == 'undefind') {
    JSON = JSON2;
}
!function () {
    'use strict';
    var rx_one = /^[\],:{}\s]*$/,
        rx_two = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g,
        rx_three = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g,
        rx_four = /(?:^|:|,)(?:\s*\[)+/g,
        rx_escapable = /[\\\"\u0000-\u001f\u007f-\u009f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        rx_dangerous = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g;

    function f(n) {
        return n < 10 ? '0' + n : n;
    }

    function this_value() {
        return this.valueOf();
    }

    if (typeof Date.prototype.toJSON !== 'function') {
        Date.prototype.toJSON = function () {
            return isFinite(this.valueOf())
                ? this.getUTCFullYear() + '-' +
                        f(this.getUTCMonth() + 1) + '-' +
                        f(this.getUTCDate()) + 'T' +
                        f(this.getUTCHours()) + ':' +
                        f(this.getUTCMinutes()) + ':' +
                        f(this.getUTCSeconds()) + 'Z'
                : null;
        };

        Boolean.prototype.toJSON = this_value;
        Number.prototype.toJSON = this_value;
        String.prototype.toJSON = this_value;
    }

    var gap, indent, meta, rep;

    function quote(string) {
        rx_escapable.lastIndex = 0;
        return rx_escapable.test(string)
            ? '"' + string.replace(rx_escapable, function (a) {
                var c = meta[a];
                return typeof c === 'string' ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
            })
            + '"' : '"' + string + '"';
    }

    function str(key, holder) {
        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];
        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }
        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }
        switch (typeof value) {
            case 'string':
                return quote(value);
            case 'number':
                return isFinite(value) ? String(value) : 'null';
            case 'boolean':
            case 'null':
                return String(value);
            case 'object':
                if (!value) {
                    return 'null';
                }
                gap += indent;
                partial = [];
                if (Object.prototype.toString.apply(value) === '[object Array]') {
                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null';
                    }
                    v = partial.length === 0 ? '[]' : gap ? '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' : '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }
                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        if (typeof rep[i] === 'string') {
                            k = rep[i];
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }
                v = partial.length === 0 ? '{}' : gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' : '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }
    if (typeof JSON2.stringify !== 'function') {
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        };
        JSON2.stringify = function (value, replacer, space) {
            var i;
            gap = '';
            indent = '';
            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }
            } else if (typeof space === 'string') {
                indent = space;
            }
            rep = replacer;
            if (replacer && typeof replacer !== 'function' && (typeof replacer !== 'object' || typeof replacer.length !== 'number')) {
                throw new Error('JSON2.stringify');
            }
            return str('', { '': value });
        };
    }

    if (typeof JSON2.parse !== 'function') {
        JSON2.parse = function (text, reviver) {
            var j;
            function walk(holder, key) {
                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }
            text = String(text);
            rx_dangerous.lastIndex = 0;
            if (rx_dangerous.test(text)) {
                text = text.replace(rx_dangerous, function (a) {
                    return '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }
            if (rx_one.test(text.replace(rx_two, '@').replace(rx_three, ']').replace(rx_four, ''))) {
                j = eval('(' + text + ')');
                return typeof reviver === 'function' ? walk({ '': j }, '') : j;
            }
            throw new SyntaxError('JSON2.parse');
        };
    }
}();

/* ajax */
!function () {
    var jsonp_idx = 1;

    var ajax = function (url, options) {
        if (typeof url === "object") {
            options = url;
            url = undefined;
        }
        var o = {
            async: options.async === false ? false : true,
            url: url || options.url || '',
            data: options.data || null,
            type: (options.type || 'POST').toUpperCase(),
            dataType: (options.dataType || 'JSON').toUpperCase(), //text,json,jsonp,html,xml
            contentType: options.contentType || "application/x-www-form-urlencoded; charset=utf-8",
            jsonp: options.jsonp || "callback",
            jsonpCallback: options.jsonpCallback || '',
            callback: options.callback || options.success || null,
            error: options.error || null,
            timeout: options.timeout || 4000,
            result: ''
        };

        if (o.dataType === 'JSONP' && o.jsonp !== false) {
            return ajaxJSONP(o.url, o.jsonp, o.jsonpCallback, o.callback) || false;
        }

        if (o.async === true && typeof o.callback !== 'function') {
            return false;
        }

        if (o.dataType === 'HTML' || checkStaticFile(o.url)) {
            //由于大多数WEB服务器不允许静态文件响应POST请求（会返回 405 Method Not Allowed），所以改为GET请求
            o.type = 'GET';
            o.url += (/\?/.test(o.url) ? "&" : "?") + new Date().getTime();
        }

        var xhr = new XmlHttpRequest();

        if (o.async === true) {
            xhr.timeout = o.timeout;
        }

        xhr.open(o.type, o.url, o.async);

        xhr.onreadystatechange = function () {
            if (4 === xhr.readyState) {
                o.result = xhr.responseText;
                if (200 === xhr.status) {
                    switch (o.dataType) {
                        case 'JSON':
                            o.result = parseJSON(o.result);
                            break;
                        case 'XML':
                            o.result = parseXML(xhr.responseXML);
                            break;
                    }
                    if (typeof o.callback === 'function') {
                        o.callback(o.result, xhr.statusText, xhr);

                        if (o.dataType === 'HTML') {
                            //执行HTML文件中的JS代码
                            execScript(o.result);
                        }
                    }
                } else {
                    typeof o.error === 'function' ? function () { o.error(o.result, xhr.statusText, xhr); }() : throwError(o.result);
                }
                xhr = null;
            }
        };

        if ('POST' === o.type) {
            xhr.setRequestHeader("content-type", o.contentType);
        }

        xhr.send(o.data);

        if (o.async === false) {
            return o.result;
        }
    };

    function XmlHttpRequest() {
        return function () {
            var len = arguments.length;
            for (var i = 0; i < len; i++) {
                try { return arguments[i](); } catch (e) { }
            }
        }(function () { return new XMLHttpRequest() },
		function () { return new ActiveXObject('Msxml2.XMLHTTP') },
		function () { return new ActiveXObject('Microsoft.XMLHTTP') });
    }

    function ajaxJSONP(url, jsonp, jsonpCallback, callback) {
        //if (!jsonpCallback) {
        //不管有没有指定JSONP回调函数，都自动生成回调函数，然后取出数据给ajax回调函数
        if (!jsonpCallback || true) {
            jsonpCallback = 'jsonpCallback_' + new Date().getTime() + '_' + jsonp_idx++;

            window[jsonpCallback] = function (result) {
                removeScript(jsonpCallback);
                callback(result);
            };
        }

        url += (/\?/.test(url) ? "&" : "?") + jsonp + "=" + jsonpCallback;

        return createScript(jsonpCallback, url);
    }

    function createScript(id, src) {
        var obj = document.createElement("script");
        obj.id = id;
        obj.type = "text/javascript";
        obj.src = src;
        document.getElementsByTagName("head")[0].appendChild(obj);

        return obj;
    }

    function removeScript(id) {
        var script = document.getElementById(id), head = document.getElementsByTagName("head")[0];
        if (head && script != null && script.parentNode) {
            head.removeChild(script);
        }
    }

    function checkStaticFile(url) {
        url = (url || '').split('?')[0];
        var ext = url.substr(url.lastIndexOf('.'));

        return /(html|htm|txt|json)/ig.test(ext);
    }

    function execScript(html) {
        var ms = html.match(/<script(.|\n)*?>(.|\n|\r\n)*?<\/script>/ig);
        if (ms) {
            var len = ms.length;
            for (var i = 0; i < len; i++) {
                var m = ms[i].match(/<script(.|\n)*?>((.|\n|\r\n)*)?<\/script>/im);
                if (m[2]) {
                    if (window.execScript) {
                        window.execScript(m[2]);
                    } else {
                        //window.eval(m[2]);
                        (function (data) {
                            return (new Function("return " + data))();
                        })(m[2].replace(/(^[\s]*)|([\s]*$)/g, ''));
                    }
                }
            }
        }
    }

    function throwError(err) {
        try {
            console.trace();
            console.log('data:\r\n\t', err);
        } catch (e) { }
        throw new Error(err);
    }

    function parseJSON(data) {
        if (typeof data !== "string" || !data) {
            return null;
        }
        if (typeof JSON2 === 'object') {
            return JSON2.parse(data);
        } else if (typeof JSON === 'object') {
            return JSON.parse(data);
        } else {
            return (new Function("return " + data))();
        }

        throwError("Invalid JSON: " + data);
    }

    function parseXML(data) {
        if (typeof data !== "string" || !data) {
            return null;
        }
        var xml, tmp;
        try {
            if (window.DOMParser) { // Standard
                tmp = new DOMParser();
                xml = tmp.parseFromString(data, "text/xml");
            } else { // IE
                xml = new ActiveXObject("Microsoft.XMLDOM");
                xml.async = "false";
                xml.loadXML(data);
            }
        } catch (e) {
            xml = undefined;
        }
        if (!xml || !xml.documentElement || xml.getElementsByTagName("parsererror").length) {
            throwError("Invalid XML: " + data);
        }
        return xml;
    }

    window.ajax = ajax;
}();

var ArrayProto = Array.prototype, ObjProto = Object.prototype, FuncProto = Function.prototype;

FuncProto.bind = FuncProto.bind || function (context) {
    var self = this;
    return function () {
        return self.apply(context, arguments);
    };
};

FuncProto.getName = function () {
    return this.name || this.toString().match(/function\s*([^(]*)\(/)[1];
};

var getObjectType = function (obj) {
    return ObjProto.toString.call(obj);
};

//是否是函数
var isFunction = function (obj) {
    return getObjectType(obj) === '[object Function]';
};
//是否是字符串
var isString = function (obj) {
    return getObjectType(obj) === '[object String]';
};
//是否是object对象
var isObject = function (obj) {
    return getObjectType(obj) === '[object Object]';
};
//是否是数组
var isArray = function (obj) {
    return getObjectType(obj) === '[object Array]';
};
//是否是window对象
var isWindow = function (obj) {
    return obj != null && obj == obj.window;
};
//是否是纯对象
var isPlainObject = function (obj) {
    return isObject(obj) && !isWindow(obj) && Object.getPrototypeOf(obj) == Object.prototype;
};

//扩展对象
var extend = function (target, source, deep) {
    var key = null;
    for (key in source) {
        if (deep && (isPlainObject(source[key]) || isArray(source[key]))) {
            if (isPlainObject(source[key]) && !isPlainObject(target[key])) {
                target[key] = {};
            }
            if (isArray(source[key]) && !isArray(target[key])) {
                target[key] = [];
            }
            extend(target[key], source[key], deep);
        }
        else if (source[key] !== undefined) {
            target[key] = source[key];
        }
    }
    return target;
};