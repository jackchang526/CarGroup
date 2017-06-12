function browse(v) {
    var userAgent = window.navigator.userAgent.toLowerCase();
    return userAgent.indexOf(v) != -1;
}
if (browse('msie 6.0')) {
    DD_belatedPNG.fix('*');
}