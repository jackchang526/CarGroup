var DomHelper = {
    cancelClick: function(e) {
        if (window.event && window.event.cancelBubble
            && window.event.returnValue) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
            return;
        }
        if (e && e.stopPropagation && e.preventDefault) {
            e.stopPropagetion();
            e.preventDefault();
        }
    },
    addEvent: function(elm, evType, fn, useCapture) {
        if (elm.addEventListener) {
            elm.addEventListener(evType, fn, useCapture);
            return true;
        }
        else if (elm.attachEvent) {
            var r = elm.attachEvent('on' + evType, fn);
            return r;
        }
        else {
            elm['on' + evType] = fn;
        }
    }
}
