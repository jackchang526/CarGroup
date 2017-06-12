/*
*disc:ajax 请求
*auth:songkai@bitauto.com
*data:2012-09-13
*/
function BitAjax(options) {
	return (this instanceof BitAjax) ? this.init(options) : new BitAjax(options);
}
//初始化参数
BitAjax.prototype.init = function (options) {
	this.url = options.url || "";
	this.async = options.async !== false;
	this.type = options.type || 'GET';
	this.datatype = options.datatype || 'text';
	this.encode = options.encode || 'UTF-8';
	this.data = options.data || null;
	this.cache = options.cache || false;
	this.success = options.success;
	this.error = options.error;
	this.type = this.type.toUpperCase();
	if (this.url == "") {
		return this;
	}
	this.HttpRequest = null;
	this.HttpRequest = this.createXMLHttpRequest();
	if (this.HttpRequest == null) {
		return;
	}
	this.send();
	return this;
}
//创建请求对象
BitAjax.prototype.createXMLHttpRequest = function () {
	try { return new ActiveXObject("Msxml2.XMLHTTP"); } catch (e) { }
	try { return new ActiveXObject("Microsoft.XMLHTTP"); } catch (e) { }
	try { return new XMLHttpRequest(); } catch (e) { }
	return null;
}
//发送请求
BitAjax.prototype.send = function () {
	var data = null;
	if (this.data && typeof this.data == 'object') {
		data = this.getQueryString(this.data);
	} else {
		data = this.data;
	}
	if (!this.cache && this.type === "GET") {
		var unionChar = this.url.indexOf('?') == -1 ? '?' : '&';
		this.url = this.url + unionChar + "ajaxtimestamp=" + (new Date()).getTime();
	}
	if (this.HttpRequest != null) {
		this.HttpRequest.open(this.type, this.url, this.async);
		if (this.type != "GET") {
			this.HttpRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		} else {
			data = null;
		}
		var self = this;
		var xreq = this.HttpRequest;
		this.HttpRequest.onreadystatechange = function () {
			if (xreq.readyState == 4) {
				var result;
				if (xreq.status == 200) {
					switch (self.datatype) {
						case 'text':
							result = xreq.responseText;
							break;
						case 'json':
							result = function (str) {
								return (new Function('return ' + str))();
							} (xreq.responseText);
							break;
						case 'xml':
							result = xreq.responseXML;
							break;
					}
					self.success(result);
				} else {
					self.error(xreq.responseText);
				}
			} else {
				// Others
			}
		}
		this.HttpRequest.send(data);
	}
}
//格式话请求数据
BitAjax.prototype.getQueryString = function (data) {
	var result = '';
	for (var key in data) {
		result += "&" + encodeURIComponent(key) + "=" + encodeURIComponent(data[key]);
	}
	result = result.replace(/^&/g, "");
	return result;
}