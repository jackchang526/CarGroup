//异步加载javascript
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



var tagChangeObject = { "pvTagForLevelPage": ["alljb", "showAllSerial"], "brandTagForLevelPage": ["brandcharlist", "brandContent"], "charTagForLevelPage": ["charlist", "charContent"] };
//显示标签内容
function InitTagList(level) {
    var index = 0;
    for (var entity in tagChangeObject) {
        var def = document.getElementById(entity);
        def.onmouseover = function () {
            ShowTagContent(this);
        };
        if (index == 0) { index++; continue; }
        var url = "http://api.car.bitauto.com/Level/GetLevelHtmlCode.ashx?level="+level+"&type=" + entity.replace('ForLevelPage', '') + "";
        loadJS.push(url, "utf-8", function ()
        {
        	//var viewcontent = document.getElementById("brand_char_content");
        	if (typeof levelHtml != 'undefined' || levelHtml != null)
        	{
        		var newdiv = document.createElement("div");
        		newdiv.innerHTML = levelHtml;
        		var clearDiv = document.getElementById("divClear");
        		if (clearDiv)
        		{
        			while (newdiv.childNodes.length > 0)
        			{
        				clearDiv.parentNode.insertBefore(newdiv.childNodes[0], clearDiv);
        			}

        		}
        		//viewcontent.insertBefore(newdiv, null);
        	}
        });
    }
}

//显示标签内容
function ShowTagContent(obj) {
    for (var entity in tagChangeObject) {
        for (var i = 0; i < tagChangeObject[entity].length; i++) {
            var isShow = false;
            var objId = tagChangeObject[entity][i];
            var objele = document.getElementById(objId);
            var entityele = document.getElementById(entity);
            if (objele == null || entityele == null)
                continue;
            if (entity == obj.id) {
                objele.style.display = "block";
                entityele.className = "current";
                continue;
            }
            objele.style.display = "none";
            entityele.className = "";
        }
    }
}



function selectCarChanel(level) {
    var levelId = level;
    var levelTag = document.getElementById("level" + levelId);
    if (!levelTag)
        levelTag = document.getElementById("level0");
    else {
        switch (levelId) {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                var jiaocheTag = document.getElementById("level63");
                if (jiaocheTag)
                    jiaocheTag.className = "current";
                break;
        }
    }
    if (levelTag)
    	levelTag.className = "current";

    if (levelId > 6 && levelId != 63)
    {
    	var carDiv = document.getElementById("jiaocheBox");
    	if (carDiv)
    		carDiv.style.display = "none";
    }

    var lpCondition = document.getElementById("lpCondition");
    if (lpCondition) {
        var aLinks = lpCondition.getElementsByTagName("a");
        for (var i = 0; i < aLinks.length; i++) {
            if (aLinks[i].href.indexOf("l=") == -1) {
                aLinks[i].href += "&l=" + levelId;
            }
        }
    }
}

