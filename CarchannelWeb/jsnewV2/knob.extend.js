/*默认值*/
Object.extend = function (destination, source) {
    if (!destination) return source;
    for (var property in source) {
        if (!destination[property]) {
            destination[property] = source[property];
        }
    }
    return destination;
};

(function ($) {
    $.fn.extend_knob = function (options) {
        var setting = {
            min: 0,
            max: 100,
            width: 220,
            height: 220,
            thickness: 0.13,
            readonly: true,
            change: function (value) {
                //console.log("change : " + value);
            },
            release: function (value) {
                //console.log(this.$.attr('value'));
                //console.log("release : " + value);
            },
            cancel: function () {
                //console.log("cancel : ", this);
            },
            /*format : function (value) {
                return value + '%';
            },*/
            draw: function () {

            },
            speed: 30,
            changeFn: null
        }
        var $this = this;
        options = Object.extend(options, setting);
        $this.max = parseInt($this.attr('data-value'));
        if ($this.max == 0) {
            options.max = 0;
        }
        $this.knob(options);
        $this.interval = 0;
        $this.i = 0;
        clearInterval($this.interval);

        $this.interval = setInterval(function () {
            options.changeFn && options.changeFn.call($this);
            if ($this.i >= $this.max) {
                clearInterval($this.interval);
            }
            $this.val($this.i).trigger("change");
            $this.i++;
        }, options.speed);
    }
})(jQuery);