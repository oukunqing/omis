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
                    var err = err[2] + '第' + (i+1) + '个参数值为：' + arguments[i];
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


//常规格式化

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

 


//增加了字面量对象参数，这个字面量对象参数必须放在参数的第1个位置，如果加了字面量对象参数，则数字参数索引必须从1开始

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

 

//增加 String.format 和 String.Format 方法
var str = '{0:D4},{1}';
console.log(String.format(str, 20, 'abc'));
console.log(String.Format(str, 20, 'abc'));

console.log('你好，%s，今天是%s'.format1('小明', '星期天'));

