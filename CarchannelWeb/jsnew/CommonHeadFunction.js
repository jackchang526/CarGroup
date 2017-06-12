if (typeof (bitLoadScript) == "undefined") {
	bitLoadScript = function (url, callback, charset) {
		var s = document.createElement("script"); s.type = "text/javascript"; if (charset) s.charset = charset;
		if (s.readyState) { s.onreadystatechange = function () { if (s.readyState == "loaded" || s.readyState == "complete") { s.onreadystatechange = null; if (callback) callback(); } }; }
		else { s.onload = function () { if (callback) callback(); }; }
		s.src = url; document.getElementsByTagName("head")[0].appendChild(s);
	};
}

if (typeof (swapDDLink) == "undefined") {
	swapDDLink = function (current, tag) {
		if (current && tag) {
			tag.innerHTML = current.innerHTML;
			tag.href = current.href;
			current.style.display = "none";
		}
	}
}
