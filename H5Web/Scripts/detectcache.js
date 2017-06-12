/* use to refresh cache */
(function () {
    var cacheName = "daycache";
    var dateCacheVersion = new Date();
    var versionStr = dateCacheVersion.getFullYear().toString()
		+ (dateCacheVersion.getMonth() + 1).toString()
		+ dateCacheVersion.getDate().toString()
		+ dateCacheVersion.getHours().toString();
    var currentURLLength = window.location.href.length;
    function getQueryStringForCache(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2]; return null;
    }
    if (currentURLLength < 1000) {
        var oldCacheValue = getQueryStringForCache(cacheName);
        var reForCache = new RegExp(cacheName + "=" + oldCacheValue, "gi");
        if (oldCacheValue != versionStr) {
            if (oldCacheValue && oldCacheValue != "") {
                var newURL = window.location.href.replace(reForCache, cacheName + "=" + versionStr)
                window.location.href = newURL;
            }
            else {
                var newURL = window.location.pathname
				+ window.location.search
				+ (window.location.search != "" ? "" : "?")
				+ "&" + cacheName + "=" + versionStr
				+ window.location.hash;
                window.location.href = newURL;
            }
        }
    }
})();