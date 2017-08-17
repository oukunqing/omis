<%@ Page Language="C#" AutoEventWireup="true" CodeFile="showPhoto.aspx.cs" Inherits="modules_upload_showPhoto" %><!DOCTYPE html>
<html>
<head>
    <title></title>
    <style type="text/css">
        body{margin:0; padding:0; font-size:12px;}
        img{display:block;}
        a:hover{ text-decoration:underline;}
        .info{height:25px; line-height:25px; padding:0; margin:0; text-indent:5px; width:100%; background:#fff; position:fixed; top:0; left;0;}
    </style>
</head>
<body oncontextmenu="return false;">
    <div>
        <div class="info">
            <form id="frmPhoto" target="_blank" method="post" action="sourcePhoto.aspx">
                <label id="lblImg"></label>
                <a onclick="showSourcePhoto();" style="cursor:pointer;color:#00f;margin-left:5px;">查看源图片</a>
                <input type="hidden" id="txtPath" name="path" />
            </form>
        </div>
        <img src="<%=photoPath%>" id="imgPhoto" alt="" oncontextmenu="return false;" style="margin-top:25px;" />
    </div>
</body>
</html>
<script type="text/javascript">
    function $I(i) {return document.getElementById(i); }
    function getImageInfo() {
        var objImg = $I('imgPhoto');
        var img = new Image();
        img.src = objImg.src;
        if (img.width > 0) {
            var strPath = img.src;
            var pos = strPath.lastIndexOf('/');
            var strName = strPath.substr(pos + 1).split('?')[0];

            var strInfo = strName + '&nbsp;' + img.width + '×' + img.height;

            $I('lblImg').innerHTML = strInfo;
        }
    }

    window.onload = function () {
        getImageInfo();
    };

    function showSourcePhoto() {
        var frm = $I('frmPhoto');

        var img = $I('imgPhoto');
        var txt = $I('txtPath');
        txt.value = img.src;

        frm.submit();
    }
</script>