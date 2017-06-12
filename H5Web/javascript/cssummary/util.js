define(function() {

    return {
        GetQueryStringByName: function(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return r[2];
            return null;
        },

        GetIntValue: function(num) {
            num = num.toString().replace(/\,/g, "");
            return parseInt(num);
        },

        CheckData: function(data) {
            return (data.indexOf("</head>") < 0) && (data.lastIndexOf("</body>") < 0);
        }

    };

});