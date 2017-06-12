

var scriptSrc = document.getElementsByTagName("script")[document.getElementsByTagName("script").length - 1].src;
function GetArgsFromHref(sHref, sArgName) {
    var args = sHref.split("?");
    var retval = "";

    if (args[0] == sHref) /*参数为空*/
    {
        return retval; /*无需做任何处理*/
    }
    var str = args[1];
    args = str.split("&");
    for (var i = 0; i < args.length; i++) {
        str = args[i];
        var arg = str.split("=");
        if (arg.length <= 1) continue;
        if (arg[0] == sArgName) retval = arg[1];
    }

    return retval;
}

//本地测试
document.write('<script src="http://www.bitauto.com/feedback/js/commentjsloadf.js" type="text/javascript"></script>');
document.write("<script>function loadcommentjs() {LazyLoadjs.js('/jsnew/SatisfyCommentno.js?r=1', function () {comment.show(" + GetArgsFromHref(scriptSrc, "ArticleId") + ",BitAutoFeedBack,'" + GetArgsFromHref(scriptSrc, "DivName") + "');});}</script>");

//线上
//document.write('<script src="http://www.bitauto.com/feedback/js/commentjsloadf.js" type="text/javascript"></script>');
//document.write("<script>function loadcommentjs() {LazyLoadjs.js('http://www.bitauto.com/feedback/js/SatisfyCommentno.js?201302221', function () {comment.show(" + GetArgsFromHref(scriptSrc, "ArticleId") + ",BitAutoFeedBack,'" + GetArgsFromHref(scriptSrc, "DivName") + "');});}</script>");
