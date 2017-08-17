function MapCorrect(decimalLength) {
    this.EARTH_RADIUS = 6378245;
    this.E_FACTOR = 0.00669342;
    this.E_RATE = 0.6667;
    if (typeof decimalLength != 'number') {
        decimalLength = 8;
    }
    this.Million = Math.pow(10, decimalLength);
};

MapCorrect.prototype.yj_sin2 = function (B) {
    var $ = 0;
    if (B < 0) {
        B = -B;
        $ = 1;
    }
    var D = parseInt(B / (2 * Math.PI));
    A = B - D * (2 * Math.PI);
    if (A > Math.PI) {
        A = A - Math.PI;
        if ($ == 1) { $ = 0; }
        else if ($ == 0) { $ = 1; }
    }
    B = A;
    var _ = B,
        C = B;
    A = A * A;
    C = C * A;
    _ = _ - C * 0.166666666666667;
    C = C * A;
    _ = _ + C * 0.00833333333333333;
    C = C * A;
    _ = _ - C * 0.000198412698412698;
    C = C * A;
    _ = _ + C * 0.00000275573192239859;
    C = C * A;
    _ = _ - C * 2.50521083854417 * Math.pow(10, -8);
    if ($ == 1) { _ = -_; }
    return _;
};

MapCorrect.prototype.Transform_yj5 = function ($, _) {
    var A = 300 + 1 * $ + 2 * _ + 0.1 * $ * $ + 0.1 * $ * _ + 0.1 * Math.sqrt(Math.sqrt($ * $));
	A += (20 * this.yj_sin2(6 * Math.PI * $) + 20 * this.yj_sin2(2 * Math.PI * $)) * this.E_RATE;
	A += (20 * this.yj_sin2(Math.PI * $) + 40 * this.yj_sin2(Math.PI * $ / 3)) * this.E_RATE;
	A += (150 * this.yj_sin2(Math.PI * $ / 12) + 300 * this.yj_sin2(Math.PI * $ / 30)) * this.E_RATE;
	return A;
};

MapCorrect.prototype.Transform_yjy5 = function ($, _) {
	var A = -100 + 2 * $ + 3 * _ + 0.2 * _ * _ + 0.1 * $ * _ + 0.2 * Math.sqrt(Math.sqrt($ * $));
	A += (20 * this.yj_sin2(6 * Math.PI * $) + 20 * this.yj_sin2(2 * Math.PI * $)) * this.E_RATE;
	A += (20 * this.yj_sin2(Math.PI * _) + 40 * this.yj_sin2(Math.PI * _ / 3)) * this.E_RATE;
	A += (160 * this.yj_sin2(Math.PI * _ / 12) + 320 * this.yj_sin2(Math.PI * _ / 30)) * this.E_RATE;
	return A;
};

MapCorrect.prototype.Transform_jy5 = function ($, B) {
    var A = this.yj_sin2($ * Math.PI / 180);
    var  _ = Math.sqrt(1 - this.E_FACTOR * A * A);
    _ = (B * 180) / (this.EARTH_RADIUS / _ * Math.cos($ * Math.PI / 180) * Math.PI);
    return _;
};

MapCorrect.prototype.Transform_jyj5 = function (A, B) {
    var _ = this.yj_sin2(A * Math.PI / 180);
    var $ = 1 - this.E_FACTOR * _ * _;
    var C = (this.EARTH_RADIUS * (1 - this.E_FACTOR)) / ($ * Math.sqrt($));
    return (B * 180) / (C * Math.PI);
};

MapCorrect.prototype.offset = function (lat, lng) {
    return this.encode(lat, lng);
};

MapCorrect.prototype.encode = function (lat, lng) {
    lat = parseFloat(lat);
    lng = parseFloat(lng);
    var $ = {};
    var _ = this.Transform_yj5(lng - 105, lat - 35);
    var C = this.Transform_yjy5(lng - 105, lat - 35);
    $.lat = (Math.round((lat + this.Transform_jyj5(lat, C)) * this.Million)) / this.Million;
    $.lng = (Math.round((lng + this.Transform_jy5(lat, _)) * this.Million)) / this.Million;
    return $;
};

MapCorrect.prototype.revert = function (lat, lng) {
    return this.decode(lat, lng);
};

MapCorrect.prototype.decode = function (lat, lng) {
    lat = parseFloat(lat);
    lng = parseFloat(lng);
    var E = this.encode(lat, lng);
    var D = 0.00001;
    var _ = 1;
    var A = 0;
    var F = {
        lat: Math.round((lat - (E.lat - lat)) * this.Million) / this.Million,
        lng: Math.round((lng - (E.lng - lng)) * this.Million) / this.Million
    };
    while (_ > D) {
        A++;
        if (A > 1000) { break; }
        var $ = this.encode(F.lat, F.lng);
        var I = F.lat - ($.lat - lat);
        var C = F.lng - ($.lng - lng);
        var H = this.encode(I, C);
        var _ = Math.abs($.lat - lat) + Math.abs($.lng - lng);
        F.lat = (Math.round(I * this.Million)) / this.Million;
        F.lng = (Math.round(C * this.Million)) / this.Million;
    }
    return F;
};
  
MapCorrect.prototype.bd_encode = function(lat, lng){  
    var x_pi = 3.14159265358979324 * 3000.0 / 180.0;
    var x = lng, y = lat;  
    var z = Math.sqrt(x * x + y * y) + 0.00002 * Math.sin(y * x_pi);  
    var theta = Math.atan2(y, x) + 0.000003 * Math.cos(x * x_pi);  
    var bd_lng = z * Math.cos(theta) + 0.0065;  
    var bd_lat = z * Math.sin(theta) + 0.006;  
    
    return {lat: bd_lat, lng: bd_lng};
};
  
MapCorrect.prototype.bd_decode = function(bd_lat, bd_lng){  
    var x_pi = 3.14159265358979324 * 3000.0 / 180.0;
    var x = bd_lng - 0.0065, y = bd_lat - 0.006;  
    var z = Math.sqrt(x * x + y * y) - 0.00002 * Math.sin(y * x_pi);  
    var theta = Math.atan2(y, x) - 0.000003 * Math.cos(x * x_pi);  
    var lng = z * Math.cos(theta);  
    var lat = z * Math.sin(theta); 
    
    return {lat: lat, lng: lng};
};