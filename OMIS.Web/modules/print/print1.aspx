<%@ Page Language="C#" AutoEventWireup="true" CodeFile="print1.aspx.cs" Inherits="modules_print_print1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>打印<%=strTitle%></title>
    <link type="text/css" rel="Stylesheet" href="<%=Config.WebDir%>/skin/css/print.css" />
    <style type="text/css">
        body{font-size:12px; margin:5px; padding:0; overflow:auto;}
        .fs10{font-size:10px;}
    </style>
    <style type="text/css" id="cssPrint" runat="server">
    </style>
    <script type="text/javascript">
        var isMSIE = navigator.userAgent.indexOf('MSIE') >= 0 || navigator.userAgent.indexOf('Trident') >= 0;
        function printPage(isAutoPrint) {
            if (isAutoPrint) {
                if (window.print) {
                    window.print();

                    if (isMSIE) {
                        window.opener = null;
                        window.close();
                    }
                }
            }
        }
    </script>
</head>
<body onload="printPage(<%=isAuto?"true":"false"%>);">
    <div id="divData" runat="server"></div>
</body>
</html>