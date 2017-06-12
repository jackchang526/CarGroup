<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content.aspx.cs" Inherits="H5Web.Test.Broker.Content" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>经纪人测试内容</title>
    <style>
        .textBox {
            height: 200px;
            width: 90%;
        }

        .tableGroup {
            width: 80%;
            border: 1px solid #d2d6de;
        }

            .tableGroup tr td {
                background: #f5faff none repeat scroll 0 0;
            }

        .date td, th {
            border: 1px solid #808080;
            height: 30px;
            padding: 0 5px;
            text-align: center;
        }
    </style>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"> </script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/gouche/pc/jquery.qrcode.min.js?v=20150424"> </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 17px">
        </div>
         <table class="tableGroup" border="1">
            <tr>
                <td>
                    host:192.168.200.24 <input id="inputURLForTest" type="text" value="http://car24.h5.yiche.com/test/broker/testpage.aspx?id=2370" style="width: 500px;" />
                    <br />
                    测试地址：
                    <div id="erweima"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TextBoxForBroker" runat="server" TextMode="MultiLine" CssClass="textBox"></asp:TextBox>
                    <br />
                    <asp:Button ID="ButtonForBroker" runat="server" OnClick="ButtonForBroker_Click" Text="经纪人块Html内容更新" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<script>
    function resetURLerweima() {
        var urlForerweima = toUtf8($("#inputURLForTest").val());
        if (urlForerweima) {
            $("#erweima").empty();
            $('#erweima').qrcode({ render: "img", typeNumber: -1, width: 100, height: 100, correctLevel: 0, text: urlForerweima });
        }
    }

    function toUtf8(str) {
        var out, i, len, c;
        out = "";
        len = str.length;
        for (i = 0; i < len; i++) {
            c = str.charCodeAt(i);
            if ((c >= 0x0001) && (c <= 0x007F)) {
                out += str.charAt(i);
            } else if (c > 0x07FF) {
                out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));
                out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            } else {
                out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));
                out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));
            }
        }
        return out;
    }

    $('#inputURLForTest').change(function () {
        resetURLerweima();
    });
    resetURLerweima();
</script>