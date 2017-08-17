<%@ Page Language="C#" AutoEventWireup="true" CodeFile="jsencrypt.aspx.cs" Inherits="tools_jsencrypt_jsencrypt" ValidateRequest="false" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>JS加密工具</title>
    <style type="text/css">
        body{margin:0;padding:0; font-size:12px; font-family:宋体,Arial;}
        .document{margin:5px 0 10px 10px;}
        .document p{margin:0;padding:5px 0 2px;}
        #help{float:right;}
        #controls{float:right;}
        #message{float:right;}
    </style>
    <script src="./files/base2.js" type="text/javascript"></script>
    <script src="./files/Packer.js" type="text/javascript"></script>
    <script src="./files/Words.js" type="text/javascript"></script>
    <script src="./files/bindings.js" type="text/javascript"></script>

	<script type="text/javascript">
	    function $I(i) {
	        return document.getElementById(i);
	    }

	    function copy() {
	        $I('output').value = $I('input').value;
	    }

	</script>
</head>
<body id="packer">
    <!--packer_至强的javascript在线加密工具_新浪也在用-->
    <div class="document" style="width:710px;float:left;">
        <div class="content" style="width:710px;">
            <form class="" id="form" action="" method="post" runat="server">
			<p id="help" style="margin-right:5px;">
				<a href="http://dean.edwards.name/packer/usage/">Help</a>
			</p>
			<p>
                <label class="paste">把要加密的代码复制到下面的文本框中:</label><br />
                <textarea spellcheck="false" id="input" name="input" runat="server" rows="1" cols="1" style="width:700px;height:150px;">function add(num,num1){
return num + num1;
}</textarea>
			</p>
            <p id="controls">
                <label for="base62"><input id="base62" name="base62" value="1" type="checkbox" checked="checked" />Base62加密</label>
                &nbsp;
                <label for="shrink"><input id="shrink" name="shrink" value="1" type="checkbox" checked="checked" />混淆变量</label>
            </p>
            <p class="form-buttons" id="input-buttons">
                <button type="button" id="clear-all">清空</button>
                <button type="button" id="Button1" onclick="copy();">复制</button>
                <button type="button" id="pack-script">压缩</button>
            </p>
            <p>
                <label class="copy">加密压缩后的代码:</label>
                <textarea spellcheck="false" id="output" name="output" runat="server" rows="1" cols="1" style="width:700px;height:150px;"></textarea></p>
				<p id="message" class=""></p>
				<p class="form-buttons" id="output-buttons">
                <button type="button" id="decode-script">解码</button>
            </p>
            <p>
                旧文件路径：<asp:TextBox ID="txtOldFilePath" runat="server" Width="538px"></asp:TextBox>
                <asp:TextBox ID="txtBakFilePath" runat="server" Width="500px" Visible="false"></asp:TextBox>
                <input id="btnRead" type="button" value="读取JS文件" runat="server" onserverclick="btnRead_Click" />
            </p>
            <p>
                源文件路径：<asp:TextBox ID="txtSourceFilePath" runat="server" Width="525px"></asp:TextBox>
                <input id="btnSource" type="button" value="生成JS源文件" runat="server" onserverclick="btnSource_Click" />
            </p>
            <p>
                新文件路径：<asp:TextBox ID="txtNewFilePath" runat="server" Width="465px"></asp:TextBox>
                <asp:CheckBox ID="chbBak" runat="server" Checked="false" Text="保存备份" />
                <input id="btnSave" type="button" value="生成JS文件" runat="server" onclick="if(!confirm('确定要生成JS文件吗？')){return false;}" onserverclick="btnSave_Click" />
            </p>
            <asp:Label ID="lblPrompt" runat="server" ForeColor="Red"></asp:Label>
            <fieldset class="hidden" style="display:none;">
                <input name="command" value="" type="hidden" />
                <input name="filename" value="" type="hidden" />
                <input name="filetype" value="" type="hidden" />
            </fieldset>
            </form>
        </div>
    </div>
</body>
</html>