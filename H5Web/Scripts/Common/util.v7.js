var util = {
    GetQueryStringByName: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2];
        return null;
    },

    GetIntValue: function (num) {
        num = num.toString().replace(/\,/g, "");
        return parseInt(num);
    },

    CheckData: function (data) {
        return (data.indexOf("/head>") < 0) && (data.lastIndexOf("/body>") < 0);
    },

    IsMicroMessager: function () {
        var ua = window.navigator.userAgent.toLowerCase();
        if (ua.match(/MicroMessenger/i) == "micromessenger") {
            return true;
        } else {
            return false;
        }
    },

    GetLength: function (str) {
        ///<summary>获得字符串实际长度，中文2，英文1</summary>  
        ///<param name="str">要获得长度的字符串</param>  
        var realLength = 0, len = str.length, charCode = -1;  
        for (var i = 0; i < len; i++) {  
            charCode = str.charCodeAt(i);  
            if (charCode >= 0 && charCode <= 128) realLength += 1;  
            else realLength += 2;  
        }  
        return realLength;  
    },

    GetKeyValueString: function (arr) {
        var res = "";
        for (var i = 0; i < arr.length; i++) {
            if (util.GetQueryStringByName(arr[i]) != null) {
                if (res !== "") {
                    res += "&";
                }
                res += arr[i] + "=" + util.GetQueryStringByName(arr[i]);
            }
        }
        return res;
    }

};