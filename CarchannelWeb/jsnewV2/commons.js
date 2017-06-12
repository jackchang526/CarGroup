Object.extend = function (destination, source) {
    if (!destination) return source;
    for (var property in source) {
        if (!destination[property]) {
            destination[property] = source[property];
        }
    }
    return destination;
};

//判断是否IE版本
function isie(v) {
    var b = false;
    var vs = v instanceof Array ? v : [v];
    for (var i = 0; i < vs.length; i++) {
        if (navigator.userAgent.indexOf("MSIE " + vs[i] + ".0") > 0) {
            b = true;
        }
    }
    return b;
}

(function ($) {
    $.fn.dragX = function (options) {
        var setting = {
            tt: '.tt', //拖动容器
            calldrag: null,
            zindex: 99999999
        }
        options = Object.extend(options, setting);
        var $this = this;
        $this.each(function (index, curr) {
            var $current = $(curr);
            (function ($o) {
                var top = parseInt($o.css('top')),
                    $tt = $o.find(options.tt);


                $tt.bind('mousedown', function (ev) {
                    var left = parseInt($o.css('left'));
                    $o.css({ left: left });
                    var oEvent = ev || event;
                    options.zindex = options.zindex + 1;
                    $o.css('zIndex', options.zindex);
                    var disX = 0;
                    if (options.calldrag) {
                        disX = oEvent.clientX - $o[0].offsetLeft;
                    } else {
                        disX = oEvent.clientX - $o[0].offsetLeft - $o.width();
                    }
                    function mousemove(ev) {
                        var oEvent = ev || event;
                        var l = oEvent.clientX - disX;
                        if (options.calldrag) {
                            options.calldrag.call($o, l);
                        } else {
                            if (l < $o.width()) l = $o.width();
                            if (l > document.documentElement.clientWidth)
                                l = document.documentElement.clientWidth;
                            $o.css('left', l);
                        }
                    }

                    function mouseup(ev) {
                        $(document).unbind('mousemove', mousemove);
                        $(document).unbind('mouseup', mouseup);
                        $o[0].releaseCapture && $o[0].releaseCapture();
                        //$o.css({ 'right': parseInt($o.css('right')) });
                        //$o.css({ 'left': 'auto' });

                    }

                    $(document).bind('mousemove', mousemove);
                    $(document).bind('mouseup', mouseup);
                    $o[0].setCapture && $o[0].setCapture();
                    return false;
                });
            })($current);
        });
    }
})($);