// JScript 文件
//显示经销商信息
function ShowVendorInfo(serialId, cityId,cityName) {
    var url = "http://api.car.bitauto.com/carinfo/iframefordealer.aspx?csId=" + serialId + "&cityId=" + cityId + "";
    loadJS.push(url, "utf-8", function () {
        var vendor = document.getElementById("vendorInfo");
        if (typeof dealerHtml == 'undefined' || dealerHtml == null || dealerHtml == "")
            vendor.style.display = "none";
        else {
            var newEle = document.createElement("div");
            newEle.innerHTML = dealerHtml;
            if (newEle.getElementsByTagName("h3").length > 0) {
                var header_H3 = newEle.getElementsByTagName("h3")[0];
                if (header_H3) {
                    if (header_H3.getElementsByTagName("span").length > 0) {
                        var header_span = header_H3.getElementsByTagName("span")[0];
                        if (header_span) {
                            if (header_span.getElementsByTagName("a").length > 0) {
                                var header_a = header_span.getElementsByTagName("a")[0];
                                if (header_a) {
                                    var header_title = header_a.innerHTML;
                                    header_a.innerHTML = cityName + header_title.replace("推荐", "").replace("-", "");
                                }
                            }
                        }
                    }
                }
            }
            vendor.innerHTML = newEle.innerHTML;
        }
    });
}


//异步加载JavaScript
var loadJS = {
    lock: false, ranks: []
		, callback: function (startTime, callback) {
		    //载入完成
		    this.lock = false;
		    callback && callback(new Date().valueOf() - startTime.valueOf()); //回调	
		    this.read(); //解锁，在次载入
		}
		, read: function () {
		    //读取
		    if (!this.lock && this.ranks.length) {
		        var head = document.getElementsByTagName("head")[0];

		        if (!head) {
		            ranks.length = 0, ranks = null;
		            throw new Error('HEAD不存在');
		        }

		        var wc = this, ranks = this.ranks.shift(), startTime = new Date, script = document.createElement('script');

		        this.lock = true;

		        script.onload = script.onreadystatechange = function () {
		            if (script && script.readyState && script.readyState != 'loaded' && script.readyState != 'complete') return;

		            script.onload = script.onreadystatechange = script.onerror = null, script.src = ''
				, script.parentNode.removeChild(script), script = null; //清理script标记

		            wc.callback(startTime, ranks.callback), startTime = ranks = null;
		        };

		        script.charset = ranks.charset || 'gb2312';
		        script.src = ranks.src;

		        head.appendChild(script);
		    }
		}
		, push: function (src, charset, callback) {
		    //加入队列
		    this.ranks.push({ 'src': src, 'charset': charset, 'callback': callback });
		    this.read();
		}
}


