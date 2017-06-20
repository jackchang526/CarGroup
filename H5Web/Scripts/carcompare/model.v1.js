/*
�����ߣ�����
����:�ƶ��������װ
ʱ��:2014.5.5
*/

/*��������*/
//��ʽ����
document.deepCss = function (who, css) {
    if (!who || !who.style) return '';
    var sty = css.replace(/\-([a-z])/g, function (a, b) {
        return b.toUpperCase();
    });
    if (who.currentStyle) {
        return who.style[sty] || who.currentStyle[sty] || '';
    }
    var dv = document.defaultView || window;
    return who.style[sty] ||
    dv.getComputedStyle(who, "").getPropertyValue(css) || '';
};

/*�ӿ�Ĭ������ datatype=0 ������ ��1 �ǰ���ͣ��*/
var api = {
    'brand': {
        url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=master&datatype=0', callName: 'businessBrandCallBack', templteName: '#brandTemplate',
        currentid: ''
    },
    'car': {
        url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=serial&pid={0}&datatype=0', callName: 'businessCarCallBack', templteName: '#carTemplate',
        currentid: '',
        clickEnd: null
    },
    'model': {
        url: 'http://api.car.bitauto.com/CarInfo/GetCarDataJson.ashx?action=car&pid={0}&datatype=0', callName: 'businessModelCallBack', templteName: '#modelTemplate',
        currentid: '',
        clickEnd: null
    }
};

//����������
var settings = {
    iscroll: {
        bonuce: true, //�Ƿ񳬳�����
        align: false //���Զ�����
    },
    sliderBox: {
        onlyOne: true //�۵��Ƿ�ʼ�մ�һ��
    }
}

/*Ĭ��ֵ*/
Object.extend = function (destination, source) {
    if (!destination) return source;
    for (var property in source) {
        if (!destination[property]) {
            destination[property] = source[property];
        }
    }
    return destination;
};

/*����*/
Array.prototype.sortValue || (Array.prototype.sortValue = function (sortby) {
    var temp = null;
    //�Ӹߵ���
    function desc(i, j) {
        if (this[i] < this[j]) {
            temp = this[i];
            this[i] = this[j];
            this[j] = temp;
        }
    };

    //�ӵ͵���
    function asc(i, j) {
        if (this[i] > this[j]) {
            temp = this[i];
            this[i] = this[j];
            this[j] = temp;
        }
    };

    var c = sortby == 'desc' ? desc : asc;
    function each(arr) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] instanceof Array) {
                each(arr[i]);
            } else {
                for (var j = i + 1; j < arr.length; j++) {
                    c.call(arr, i, j);
                }
            }
        }
    };
    each(this);
    return this;
});

/*�����״γ���*/
Array.prototype.indexOf || (Array.prototype.indexOf = function (v) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == v) {
            return i;
        }
    }
    return -1;
});

/*����ɾ������*/
Array.prototype.removeIndex || (Array.prototype.removeIndex = function (index) {
    for (var i = 0; i < this.length; i++) {
        if (i == index) {
            this.splice(i, 1);
            i--;
            break;
        }
    }
});

/*ɾ����������*/
Array.prototype.remove || (Array.prototype.remove = function (v) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == v) {
            this.splice(i, 1);
            i--;
        }
    }
});

//ʱ��Ƚϴ�С
Date.prototype.compareValue || (Date.prototype.compareValue = function (date2) {
    var date1 = this;
    if (date1.getFullYear() > date2.getFullYear()) {
        return 1;
    } else if (date1.getFullYear() < date2.getFullYear()) {
        return -1;
    }

    if (date1.getDate() > date2.getDate()) {
        return 1;
    } else if (date1.getDate() < date2.getDate()) {
        return -1;
    }

    if (date1.getDay() > date2.getDay()) {
        return 1;
    } else if (date1.getDay() < date2.getDay()) {
        return -1;
    }

    if (date1.getHours() > date2.getHours()) {
        return 1;
    } else if (date1.getHours() < date2.getHours()) {
        return -1;
    }

    if (date1.getMinutes() > date2.getMinutes()) {
        return 1;
    } else if (date1.getMinutes() < date2.getMinutes()) {
        return -1;
    }

    if (date1.getSeconds() > date2.getSeconds()) {
        return 1;
    } else if (date1.getSeconds() < date2.getSeconds()) {
        return -1;
    }

    if (date1.getMilliseconds() > date2.getMilliseconds()) {
        return 1;
    } else if (date1.getMilliseconds() < date2.getMilliseconds()) {
        return -1;
    }

    return 0;

});

/*�洢*/
/*�洢��װ*/
function storage(options) {
    var setting = {
        expires: 30, //����ʱ��
        path: '', //·��
        domain: '', //��
        secure: '', //������Ƿ񴫸�������
        type: 'cookie'
    }
    options = Object.extend(options, setting);
    var date = new Date();
    if (typeof options.expires == 'number') {
        date.setTime(date.getTime() + options.expires * 24 * 60 * 60 * 1000);//��Ч��1Сʱ
    } else {
        date = options.expires;
    }
    this.date = date;
    this.type = options.type;
    this.options = options;
}

storage.prototype.set = function (name, value) {
    eval('this.set' + this.type).call(this, name, value);
}
storage.prototype.get = function (name) {
    return eval('this.get' + this.type).call(this, name);
}
storage.prototype.del = function (name) {
    eval('this.del' + this.type).call(this, name);
}

//cookie�洢
storage.prototype.setcookie = function (name, value) {
    var expires = '; expires=' + this.date.toUTCString();
    var path = this.options.path ? '; path=' + (this.options.path) : '';
    var domain = this.options.domain ? '; domain=' + (this.options.domain) : '';
    var secure = this.options.secure ? '; secure' : '';
    document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
}

//��ȡ�洢
storage.prototype.getcookie = function (name) {
    var cookieValue = null;
    if (document.cookie && document.cookie != '') {
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = (cookies[i]).trim();
            if (cookie.substring(0, name.length + 1) == (name + '=')) {
                cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                break;
            }
        }
    }
    return cookieValue;
}

//ɾ���洢
storage.prototype.delcookie = function (name) {
    var date = new Date();
    date.setTime(date.getTime() - 1);
    var expires = ";expires=" + date.toUTCString();
    document.cookie = [name, '=', '', expires, this.path, this.domain, this.secure].join('');
}


//localStorage
storage.prototype.setlocal = function (name, value) {
    localStorage.setItem(name, [this.date.toUTCString() + '[%%]', value]);
}

storage.prototype.getlocal = function (name) {
    var date = new Date();
    var v = localStorage.getItem(name);
    if (v) {
        var spt = v.split('[%%]');
        var startDate = new Date(spt[0]);
        if (startDate.compareValue(date) >= 0) {
            return spt[1];
        }
    }
    return null;
}

storage.prototype.dellocal = function (name) {
    localStorage.removeItem(name);
}

//localStorage
storage.prototype.setsession = function (name, value) {
    sessionStorage.setItem(name, [this.date.toUTCString() + '[%%]', value]);
}

storage.prototype.getsession = function (name) {
    var date = new Date();
    var v = sessionStorage.getItem(name);
    if (v) {
        var spt = v.split('[%%]');
        var startDate = new Date(spt[0]);
        if (startDate.compareValue(date) >= 0) {
            return spt[1];
        }
    }
    return null;
}

storage.prototype.delsession = function (name) {
    sessionStorage.removeItem(name);
}


//��Ļ��ת����¼�(����:horizontal|����:vertical)
$.rotateEnd = function (fn) {
    function toResize() {
        var winW = document.documentElement.clientWidth,
           winH = document.documentElement.clientHeight;
        setTimeout(function () { fn && fn(winW > winH ? "horizontal" : "vertical"); }, 200);
    };
    $(window).resize(toResize).trigger('resize');
};

$.rnd = function (n, m) {
    return Math.floor(Math.random() * (m + 1 - n) + n);
};

var rdArr = {};
//�����
function useRandom(min, max, key) {
    if (min > max) { alert('����ֵ����ȷ'); }
    var key = key || 'random';
    rdArr[key] = rdArr[key] || [];
    if (rdArr[key].length == 0) {
        for (var i = min; i <= max; i++) {
            rdArr[key].push(i);
        }
    }
    var random = -1;
    var arr = rdArr[key];
    do {
        random = $.rnd(min, max);
    } while (arr.indexOf(random) == -1 && arr.length != 0);
    arr.remove(random);
    return random;
}

var classNames = ['Webkit', 'ms', 'Moz', 'O', ''];
var eventNames = ['webkit', 'moz', 'o'];
(function ($) {
    //���css3��ʽ
    $.fn.addClass3 = function (name, value) {
        var o = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < classNames.length; i++) {
            o.style[classNames[i] + cName] = value;
        }
        return $(o);
    }

    //transition�¼�����
    $.fn.transitionEnd = function (options) {
        var setting = {
            listen: 'TransitionEnd',
            end: null
        }
        options = Object.extend(options, setting);
        var $this = this;
        function seatTransitionEnd() {
            for (var i = 0; i < eventNames.length; i++) {
                if (eventNames[i] == 'moz') {
                    $this.removeEvent(options.listen.toLocaleLowerCase(), seatTransitionEnd);
                } else {
                    $this.removeEvent(eventNames[i] + options.listen, seatTransitionEnd);
                }
            }
            options.end && options.end.call($this);
        }
        for (var i = 0; i < eventNames.length; i++) {
            if (eventNames[i] == 'moz') {
                $this.addEvent(options.listen.toLocaleLowerCase(), seatTransitionEnd);
            } else {
                $this.addEvent(eventNames[i] + options.listen, seatTransitionEnd);
            }
        }
    }

    //����¼�
    $.fn.addEvent = function (name, fn) {
        var obj = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < eventNames.length; i++) {
            obj.addEventListener(eventNames[i] + cName, fn, false);
        }
        obj.addEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
    }

    //ɾ���¼�
    $.fn.removeEvent = function (name, fn) {
        var obj = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < eventNames.length; i++) {
            obj.removeEventListener(eventNames[i] + cName, fn, false);
        }
        obj.removeEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
    }

    $.fn.offsetHeight = function () {
        return this.height() + parseInt(this.css('padding-top')) + parseInt(this.css('padding-bottom')) + parseInt(this.css('margin-top')) + parseInt(this.css('margin-bottom'));
    }

    //��������
    $.fn.scrollListener = function (options) {
        var setting = {
            scrollTo: null
        };
        options = Object.extend(options, setting);
        var $this = this;

        function scrollTo() {
            clearTimeout($this.timeout);
            $this.timeout = setTimeout(function () {
                var scrollT = document.documentElement.scrollTop || document.body.scrollTop;
                var h = document.documentElement.clientHeight || document.body.clientHeight;
                $this.scrollTop = scrollT;
                $this.clientHeight = h;
                options.scrollTo && options.scrollTo.call($this);
            }, 10);
        }

        $this.touches({
            touchmove: function () {
                scrollTo();
            }
        })

        $this.scroll(function () {
            scrollTo();
        }).trigger('scroll');
    }

    //�ڵ��ʼ�����
    $.fn.onload = function (fnEnd) {
        var obj = this[0];
        obj.onload = function () {
            fnEnd && fnEnd.call(obj);
            obj.onreadystatechange = null;
        }
        obj.onreadystatechange = function (ev) {
            if (obj.readyState == 'complete') {
                fnEnd && fnEnd.call(obj);
                obj.onload = null;
            }
        }
        obj.onerror = function () {
            fnEnd && fnEnd.call(obj);
            obj.onreadystatechange = null;
            obj.onload = null;
        }
    }

    //�������¼�
    $.fn.touches = function (options) {
        var setting = {
            init: null,//��ʼ��
            touchstart: null,  //����
            touchmove: null, //����
            touchend: null //̧��
        };
        options = Object.extend(options, setting);
        var $this = this, touchesDiv = $this[0];
        if (!$this[0]) return;
        touchesDiv.addEventListener('touchstart', function (ev) {
            options.touchstart && options.touchstart.call($this, ev);
            function fnMove(ev) {
                options.touchmove && options.touchmove.call($this, ev);
            };

            function fnEnd(ev) {
                options.touchend && options.touchend.call($this, ev);
                document.removeEventListener('touchmove', fnMove, false);
                document.removeEventListener('touchend', fnEnd, false);
            };
            document.addEventListener('touchmove', fnMove, false);
            document.addEventListener('touchend', fnEnd, false);
            return false;
        }, false)
        options.init && options.init.call($this);
    }

    //�������
    $.fn.navigate = function (options) {
        var setting = {
            sandAjax: null,
            selectFn: null,
            scrollTo: null,
            init: null,
            upsort: null,
            downsort: null,
            speed: 100,
            top: 0,
            toOffsetY: 0
        };
        options = Object.extend(options, setting);
        var $this = this, sections = [];
        $this.rows = 0;
        var site = [], f1 = 0, otop = 0, t = 0;
        var $first = $this.first();
        $first.on('to', function (event, r) {
            var offsetY = options.toOffsetY || 0;
            var site = sections.sortValue(function (i, j) {
                if (this[i].top > this[j].top) {
                    temp = this[i];
                    this[i] = this[j];
                    this[j] = temp;
                }
            });

            var section = $(site).eq(r), pos = section[0].top;

            //������Ļ
            $("html,body").animate({
                scrollTop: pos + offsetY
            }, options.speed);

        })
        var winh = document.body.clientHeight > document.documentElement.clientHeight ? document.documentElement.clientHeight != 0 ? document.documentElement.clientHeight : document.body.clientHeight : document.body.clientHeight,
            scrollh = document.body.scrollHeight > document.documentElement.scrollHeight ? document.documentElement.scrollHeight != 0 ? document.documentElement.scrollHeight : document.body.scrollHeight : document.body.scrollHeight;

        var upsections = downsections = 0;

        function toscroll() {
            options.scrollTo && options.scrollTo.call($this);
            var temp = null;
            //���� ��С����
            site = sections.sortValue(function (i, j) {
                if (this[i].top > this[j].top) {
                    temp = this[i];
                    this[i] = this[j];
                    this[j] = temp;
                }
            });
            $this.site = site;
            var currentTOP = $(window).scrollTop() + options.top,
                f1 = site[0].top;
            var st = currentTOP > otop ? 'down' : 'up';
            $this.currentTOP = currentTOP;

            try {
                switch (st) {
                    case 'down': //����
                        $this.sections = sections;
                        //console.log('down');
                        //����Ƿ����������

                        //$('.navs a:last').html((lastArr.maxValue() + winh) + ' ' + (sections[sections.length - 1].top));
                        if ((currentTOP + winh) >= downsections[upsections.length - 1].top) {
                            //$('.navs a:last').html(sections.length - 1)
                            options.selectFn && options.selectFn.call(sections[sections.length - 1], sections.length - 1);
                        } else {
                            for (var i = 0; i < downsections.length; i++) {
                                var index = downsections[i + 1] ? i + 1 : i;
                                if (currentTOP >= downsections[i].top - downsections[i].h && currentTOP <= downsections[index].top) {
                                    //$('.navs a:last').html(i)
                                    options.selectFn && options.selectFn.call($this, i);
                                }
                            }
                        }

                        for (var i = 0; i < downsections.length; i++) {
                            var index = downsections[i + 1] ? i + 1 : i;
                            if (currentTOP >= downsections[i].top - downsections[i].h && currentTOP <= downsections[index].top) {
                                //$('.navs a:last').html(i)
                                options.selectFn && options.selectFn.call($this, i);
                            }
                        }
                        break;
                    default:  //����
                        $this.sections = sections;
                        for (var i = 0; i < upsections.length; i++) {
                            var index = upsections[i - 1] ? i - 1 : i;
                            if (currentTOP <= upsections[i].top && currentTOP >= upsections[index].top) {
                                options.selectFn && options.selectFn.call($this, index);
                            }
                        }
                        break;
                }
                otop = currentTOP;
            }
            catch (e) { }
        }

        $this.each(function (index, curr) {
            var $current = $(curr);
            $current.on('succeed', function (event, obj) {
                var $o = $(obj);
                if ($this.length - 1 == $this.rows) { //���سɹ�
                    $this.each(function (index, c) {
                        var $c = $(c);
                        sections.push({ section: $c, top: $c.offset().top, h: $c.height() });
                    });
                    upsections = downsections = sections;
                    if (options.downsort) {
                        downsections = options.downsort.call(sections);
                    }
                    if (options.upsort) {
                        upsections = options.upsort.call(sections);
                    }
                    //$(window).scroll(toscroll).trigger('scroll');

                    $(window).scrollListener({
                        scrollTo: function () {
                            toscroll();
                        }
                    });
                    options.init && options.init.call($first);
                }
                $this.rows++;
            });
            $current.index = index;
            if ($current.attr('data-ajax') == 'true') {
                options.sandAjax && options.sandAjax.call($current);
            } else {
                $current.trigger('succeed', $current);
            }
        });
    }

    //���Ʋ��
    $.fn.gesture = function (options) {
        var setting = {
            items: 'a',
            selectFn: null,
            top: 0,
            offsetTop: 0,
            init: null,
            delay: 60
        };
        options = Object.extend(options, setting);
        var items = this.find(options.items);
        var itemH = items.eq(0).height();
        var $this = this, otop = 0, okey = '', timeout = 0, secs = [];
        $this.on('compareTop', function (event, ev) {
            var isdelay = ev.type != 'touchstart';
            if (!isdelay) {
                secs.length = 0;
                items.each(function (idx, c) {
                    secs.push({ key: c.innerHTML, top: c.offsetTop + options.offsetTop });
                });
            }
            var currentTOP = ev.targetTouches[0].pageY - (document.body.scrollTop + options.top);
            var st = currentTOP > otop ? 'down' : 'up';
            if (st == 'down') { //����
                var downs = [];
                for (var i = 0; i < secs.length; i++) {
                    if (currentTOP >= secs[i].top) {
                        downs.push(secs[i].key);
                    }
                };
                var key = downs[downs.length - 1];
                if (key && key != okey) {
                    clearTimeout(timeout);
                    if (isdelay) {
                        timeout = setTimeout(function () {
                            options.selectFn && options.selectFn(key);
                        }, options.delay);
                    } else {
                        options.selectFn && options.selectFn(key);
                    }

                }
                okey = key;
            } else { //����
                var ups = [];
                for (var i = 0; i < secs.length; i++) {
                    var index = secs[i - 1] ? i - 1 : i;
                    if (currentTOP < itemH) {
                        ups.push(secs[0].key);
                    }
                    else if (currentTOP - itemH <= secs[i].top && currentTOP - itemH >= secs[index].top) {
                        ups.push(secs[i].key);
                    }
                };

                var key = ups[ups.length - 1];
                if (key && key != okey) {
                    clearTimeout(timeout);
                    if (isdelay) {
                        timeout = setTimeout(function () {
                            options.selectFn && options.selectFn(key);
                        }, options.delay);
                    } else {
                        options.selectFn && options.selectFn(key);
                    }
                }
                okey = key;
            }
            otop = currentTOP;
        })
        options.init && options.init.call($this);
    }

    //top
    $.fn.top = function (options) {
        var setting = {
            speed: 40,
            scroll: null
        };

        options = Object.extend(options, setting);
        var $this = this;
        function scrollTo() {
            var scrollT = document.documentElement.scrollTop || document.body.scrollTop;
            var h = document.documentElement.clientHeight || document.body.clientHeight;
            if (scrollT - h >= 0) {
                $this.show();
            } else {
                $this.hide();
            }
            options.scroll && options.scroll.call($this, scrollT);
        }
        $this.click(function () {
            var h = document.documentElement.clientHeight || document.body.clientHeight;
            var scrollh = document.documentElement.scrollHeight || document.body.scrollHeight;
            clearInterval($this.interval);
            $this.interval = setInterval(function () {
                var scrollT = document.documentElement.scrollTop || document.body.scrollTop;
                if (scrollT <= 0) {
                    clearInterval($this.interval);
                }
                window.scrollBy(0, -(scrollh / h * options.speed));
            }, 30);
        });

        $(window).scroll(scrollTo).trigger('scroll');
    }

    //����Ӧҳ��
    $.fn.footer = function (options) {
        var setting = {
            footer: '.footer-box',
            bottom: 0
        }
        options = Object.extend(options, setting);
        var $body = this;
        function autoheight() {
            var $footer = $body.find(options.footer);
            if ($body.height() <= $(window).height() - 5) {
                $footer.css('position', 'absolute').css('bottom', options.bottom).css('width', '100%');
            } else {
                $footer.css('position', 'relative');
            }
            $footer.show();
        };
        $(this).resize(autoheight).trigger('resize');
    }

    //�Ҳ൯����
    $.fn.rightSwipeAction = function (options) {
        var setting = {
            show: 'swipeLeft-block',
            clickEnd: null
        };
        options = Object.extend(options, setting);
        var $child = $(this.children(1)), display = 'none';
        if ($child.hasClass(options.show)) {
            display = 'none';
        } else {
            display = 'block';
        }
        options.clickEnd && options.clickEnd.call($child, display);
    };

    //�Ҳ฽��ѡ�����
    $.fn.rightSwipe = function (options, $click) {
        var $temp = null;
        var setting = {
            isclick: null,
            zindex: 999999,
            back: '.leftmask',
            alert: '.leftPopup',
            clickEnd: null, //�򿪹رղ�ص��¼�
            oneEnd: null,
            closeEnd: null,
            currentid: null,
            selected: false, ///���Ϊ�棬����currentid�Ƿ���ȣ�������Ͼʹ����ص��¼�
            onBeforeScrollStart: function (ev) {
                ev.preventDefault();
            },
            clickCallBack: function (clickEnd) { //���Ĭ�ϻص�����
                clickEnd.call(this);
            }
        };

        function clickEnd() {
            var $this = this;
            var $leftPopup = $(options.alert + '.' + $this.attr('data-action'));
            options.zindex = !$leftPopup.attr('data-zindex') ? options.zindex : parseInt($leftPopup.attr('data-zindex'));
            $leftPopup[0].style.zIndex = options.zindex;
            options.oneEnd && options.oneEnd.call($leftPopup);
            var $back = $('.' + $leftPopup.attr('data-back'));
            $back.count = 0;
            if ($back.length > 0) {
                $back.css('z-index', options.zindex - 10000);
                $back.show();
            }
            $leftPopup.rightSwipeAction({
                clickEnd: function (display) {
                    if (display != 'none') {
                        isopen = true;
                        var $swipeLeft = $leftPopup.find('.swipeLeft');
                        $leftPopup.show();
                        setTimeout(function () { $swipeLeft.addClass('swipeLeft-block'); }, 200);
                        if ($back.length > 0) {
                            $back.parents('body').css('overflow', 'hidden');
                            function closeEnd(ev, params) {
                                if (!params || !params.leftPopup) {
                                    setTimeout(function () {
                                        $(options.back).each(function (index, curr) {
                                            curr.style.display = 'none';
                                        })
                                    }, 300);
                                    var $alert = $(options.alert).children().removeClass('swipeLeft-block').end();
                                    $back.parents('body').css('overflow', 'inherit');
                                    setTimeout(function () { $alert.hide(); }, 200);

                                    options.closeEnd && options.closeEnd.call($swipeLeft, $back);

                                } else {
                                    var $alert = params.leftPopup.children().removeClass('swipeLeft-block').end();
                                    setTimeout(function () { $alert.hide(); params.leftPopup[0].style.display = 'none'; }, 200);
                                    options.closeEnd && options.closeEnd.call(params.leftPopup.children(), $back);
                                }
                            }
                            if (!$back.attr('data-close')) {
                                $back.on('close', closeEnd);
                                $back.touches({
                                    touchstart: function () {
                                        $back.trigger('close');
                                    }
                                });
                            }
                            $back.attr('data-close', 'true');
                        }
                        $swipeLeft.transitionEnd({ end: function () { options.clickEnd && options.clickEnd.call($leftPopup, true, $this); } });
                    } else {
                        options.clickEnd && options.clickEnd.call($leftPopup, true, $this);
                    }
                }
            });
        }

        options = Object.extend(options, setting);
        if (this.length == 0) { return; }
        if (!$click) {
            this.each(function (index, curr) {
                var $curr = $(curr);
                (function ($this) {
                    $this.isclick = true;
                    $this.click(function (ev) {
                        options.onBeforeScrollStart.call($this, ev);
                        options.isclick && ($this.isclick = options.isclick.call($this));
                        if ($this.isclick == false) {
                            return;
                        }
                        options.clickCallBack.call($this, clickEnd);
                    })
                    //����Ĭ��ѡ��ֵ��������Ͼʹ����ص��¼�
                    if (options.selected && options.currentid && options.currentid.toString() == $curr.attr('data-id')) {
                        options.clickCallBack.call($this, clickEnd);
                        return;
                    }
                })($curr);
            });
        } else {
            options.clickCallBack.call($click, clickEnd);
        }
    }

    //iscroll��չ
    $.fn.iScroll = function (options) {
        var setting = {
            init: null,
            snap: null
        }
        options = Object.extend(options, setting);
        var $this = this;

        var myScroll = new iScroll($this[0], {
            snap: settings.iscroll.align ? options.snap : null,
            momentum: true,
            click: true,
            bounce: settings.iscroll.bonuce,
            bounceLock: true,
            checkDOMChanges: true,
            onBeforeScrollStart: function (ev) { }
        });

        options.init && options.init.call($this);
    }

    //��ǩ�л�
    $.fn.tag = function (options) {
        var setting = {
            tagName: '.tag_board',
            tag_select: 'current',
            tag_content_active: 'active',
            index: 0,
            fnEnd: null
        };

        options = Object.extend(options, setting);
        var $tag_board = this.find(options.tagName), tags = this.find(options.tagName.split('_')[0]), tagli = $tag_board.find('li');

        options.index = parseInt($tag_board.attr('data-index')) || options.index;
        var $temp_tag = null, $temp_content = null;

        if (!$tag_board.attr('isinit')) {
            $tag_board.on('selectTag', function (event, idx) {
                var $current = $(tagli[idx]);
                if ($temp_tag == $current) return;
                
                if ($temp_tag && $temp_tag.length > 0) {
                    $temp_tag.removeClass(options.tag_select);
                }
                $current.addClass(options.tag_select);
                $tag_board.trigger('selectContent', idx);
                $temp_tag = $current;
            });


            $tag_board.on('selectContent', function (event, idx) {
                if ($temp_content && $temp_content.length > 0) {
                    $temp_content.removeClass(options.tag_content_active);
                }
                $(tags[idx]).addClass(options.tag_content_active);
                $temp_content = $(tags[idx]);
                options.fnEnd && options.fnEnd.call($temp_content, idx);
            })

            $tag_board.find('li').each(function (index, curr) {
                $(curr).click(function (ev) {
                    ev.preventDefault();
                    $tag_board.trigger('selectTag', $(this).index())
                });
            })
            $tag_board.attr('isinit', 'true');
        }

        $tag_board.trigger('selectTag', options.index);
    }

    //�۵��˵�
    $.fn.sliderBox = function (options) {
        var setting = {
            index: -1,
            onlyOne: false,
            loadEnd: null,
            click: null,
            clickEnd: null,
            animateEnd: null,
            heightFn: function (idx) { return this.height(); },
            isCloseFn: function (idx, index) { return idx != index && idx != 'all'; }
        }
        options = Object.extend(options, setting);
        var idx = -1;
        if (options.index) {
            if (typeof options.index == 'number') {
                idx = options.index - 1;
            } else {
                idx = options.index;
            }
        } else {
            this.parents('ul,div').each(function (index, curr) {
                var $curr = $(curr);
                if ($curr.attr('data-index') != undefined) {
                    idx = parseInt($curr.attr('data-index'));
                }
            })
        }
        var $first = $(this[0]);
        var header = this;
        header.each(function (index, curr) {
            var $next = $(curr).next();
            $next.attr('data-height', options.heightFn.call($next, index));
        })

        var s = [], $temp = null, $temp_k = null;

        function fnSlider(event, current) {
            var $this = $(current);
            var $box = $this.next(), height = parseInt($box.attr('data-height'));

            if (options.onlyOne && $temp && $temp.attr('index') != $this.attr('index')) {
                $temp.next().stop().animate({ height: 0 }, 'fast', function () {
                    options.animateEnd && options.animateEnd.call($this);
                });
                $temp_k = $temp;
            }

            $box.animate({ height: $box.height() == 0 ? height : 0 }, 'fast', function () {
                if ($box.height() > 0) {
                    $box.height('100%').removeClass('height');
                }
                options.animateEnd && options.animateEnd.call($this);
            });
            options.clickEnd && options.clickEnd.call($this, { k: $box.height() == 0 ? 'down' : 'up', temp: $temp_k });

            $temp = $this;
        }

        if (!$first.attr('isinit')) {
            $first.on('slider', fnSlider);
            this.each(function (index, curr) {
                var $curr = $(curr);
                $curr.unbind('click').bind('click', function (ev) {
                    ev.preventDefault();
                    if (options.click == null) {
                        $first.trigger('slider', $(this));
                    } else {
                        options.click.call($curr, $first);
                    }
                    return false;
                })

                if (options.isCloseFn.call($curr.next(), idx, index)) {
                    $curr.next().height(0);
                } else {
                    $temp = $curr;
                    s.push(index);
                }
                $curr.attr('index', index);
            })
            $first.attr('isinit', 'true');
        }
        if (s.length > 0)
            options.loadEnd && options.loadEnd.call(this, s);
    }

    //ajax������HTML
    $.fn.swipeApi = function (options) {
        var setting = {
            id: 0,
            url: '',
            templateid: '#modelTemplate',
            jsonpCallback: 'a',
            flatFn: function (data) { return data },
            analysis: function (data) {
                var tp1 = $(options.templateid).html();
                var template = _.template(tp1);
                var jb = options.flatFn(data);
                return template(jb);
            },
            callback: null
        }
        options = Object.extend(options, setting);

        /*ģ������*/
        _.templateSettings = {
            evaluate: /{([\s\S]+?)}/g,
            interpolate: /{=([\s\S]+?)}/g,
            escape: /{-([\s\S]+?)}/g
        };

        var $leftPopup = this;
        try {
            $.ajax({
                url: options.url.replace('{0}', options.id),
                dataType: "jsonp",
                jsonpCallback: options.jsonpCallback,
                cache: true,
                success: function (data) {
                    options.callback && options.callback.call($leftPopup, options.analysis && options.analysis.call($leftPopup, data));
                }
            });
        }
        catch (ev) { }
    }

    //json������HTML
    $.fn.swipeData = function (options) {
        var setting = {
            data: null,
            callback: null,
            templateid: '',
            flatFn: function (data) { return data },
            analysis: function (data) {
                var tp1 = $(options.templateid).html();
                var template = _.template(tp1);
                var jb = options.flatFn(data);
                return template(jb);
            }
        }
        options = Object.extend(options, setting);

        /*ģ������*/
        _.templateSettings = {
            evaluate: /{([\s\S]+?)}/g,
            interpolate: /{=([\s\S]+?)}/g,
            escape: /{-([\s\S]+?)}/g
        };

        var $this = this;
        options.callback && options.callback.call($this, options.analysis && options.analysis.call($this, options.data));
    }

    //�����˶�
    $.fn.arcAnimation = function ($target, options) {
        var element = this[0], target = $target[0];
        /*
         * ��ҳģ����ʵ��Ҫһ��������
         * �������1���ؾ���1�����㣬��Ȼ�����ʣ���Ϊҳ�涯�����ͼ�������
         * ҳ���ϣ����Ƿ��������壬200~800����֮�䣬���ǿ���ӳ��Ϊ��ʵ�����2�׵�8�ף�Ҳ����100:1
         * ������������û�жԴ��������֣���˲�������
        */
        var setting = {
            speed: 300.67, // ÿ֡�ƶ������ش�С��ÿ֡�����ڴ󲿷���ʾ������Լ16~17����
            curvature: 0.001,  // ʵ��ָ���㵽׼�ߵľ��룬����Գ�������ʣ�����ģ��������������ߣ�����ǿ������µ�
            progress: null,
            complete: null
        };

        var params = Object.extend(options, setting);

        var exports = {
            mark: function () { return this; },
            position: function () { return this; },
            move: function () { return this; },
            init: function () { return this; }
        };

        /* ȷ���ƶ��ķ�ʽ 
         * IE6-IE8 ��marginλ��
         * IE9+ʹ��transform
        */
        var moveStyle = "margin", testDiv = document.createElement("div");
        if ("oninput" in testDiv) {
            ["", "ms", "webkit"].forEach(function (prefix) {
                var transform = prefix + (prefix ? "T" : "t") + "ransform";
                if (transform in testDiv.style) {
                    moveStyle = transform;
                }
            });
        }

        // �������������Լ�����ȷ���˶����ߺ�����Ҳ����ȷ��a, b��ֵ��
        /* ��ʽ�� y = a*x*x + b*x + c;
        */
        var a = params.curvature, b = 0, c = 0;

        // �Ƿ�ִ���˶��ı�־��
        var flagMove = true;

        if (element && target && element.nodeType == 1 && target.nodeType == 1) {
            var rectElement = {}, rectTarget = {};

            // �ƶ�Ԫ�ص����ĵ�λ�ã�Ŀ��Ԫ�ص����ĵ�λ��
            var centerElement = {}, centerTarget = {};

            // Ŀ��Ԫ�ص�����λ��
            var coordElement = {}, coordTarget = {};

            // ��ע��ǰԪ�ص�����
            exports.mark = function () {
                if (flagMove == false) return this;
                if (typeof coordElement.x == "undefined") this.position();
                element.setAttribute("data-center", [coordElement.x, coordElement.y].join());
                target.setAttribute("data-center", [coordTarget.x, coordTarget.y].join());
                return this;
            }

            //�Ͱ汾�����requestAnimationFrame���ݷ���
            window.requestAnimationFrame = (function () {
                return window.requestAnimationFrame ||
                        window.webkitRequestAnimationFrame ||
                        window.mozRequestAnimationFrame ||
                        function (callback) {
                            window.setTimeout(callback, 1000 / 60);
                        };
            })();

            exports.position = function () {
                if (flagMove == false) return this;

                var scrollLeft = document.documentElement.scrollLeft || document.body.scrollLeft,
                    scrollTop = document.documentElement.scrollTop || document.body.scrollTop;

                // ��ʼλ��
                if (moveStyle == "margin") {
                    element.style.marginLeft = element.style.marginTop = "0px";
                } else {
                    element.style[moveStyle] = "translate(0, 0)";
                }

                // �ı�Ե������
                rectElement = element.getBoundingClientRect();
                rectTarget = target.getBoundingClientRect();

                // �ƶ�Ԫ�ص����ĵ�����
                centerElement = {
                    x: rectElement.left + (rectElement.right - rectElement.left) / 2 + scrollLeft,
                    y: rectElement.top + (rectElement.bottom - rectElement.top) / 2 + scrollTop
                };

                // Ŀ��Ԫ�ص����ĵ�λ��
                centerTarget = {
                    x: rectTarget.left + (rectTarget.right - rectTarget.left) / 2 + scrollLeft,
                    y: rectTarget.top + (rectTarget.bottom - rectTarget.top) / 2 + scrollTop
                };

                // ת�����������λ��
                coordElement = {
                    x: 0,
                    y: 0
                };
                coordTarget = {
                    x: -1 * (centerElement.x - centerTarget.x),
                    y: -1 * (centerElement.y - centerTarget.y)
                };

                /*
                 * ��Ϊ����(0, 0), ���c = 0
                 * ���ǣ�
                 * y = a * x*x + b*x;
                 * y1 = a * x1*x1 + b*x1;
                 * y2 = a * x2*x2 + b*x2;
                 * ���õڶ������꣺
                 * b = (y2+ a*x2*x2) / x2
                */
                // ����
                b = (coordTarget.y - a * coordTarget.x * coordTarget.x) / coordTarget.x;

                return this;
            };

            // ������������˶�
            exports.move = function () {
                // ��������˶���û�н���������ִ���µ��˶�
                if (flagMove == false) return this;

                var startx = 0, rate = coordTarget.x > 0 ? 1 : -1;

                var step = function () {
                    // ���� y'=2ax+b
                    var tangent = 2 * a * startx + b; // = y / x
                    // y*y + x*x = speed
                    // (tangent * x)^2 + x*x = speed
                    // x = Math.sqr(speed / (tangent * tangent + 1));
                    startx = startx + rate * Math.sqrt(params.speed / (tangent * tangent + 1));
                    // ��ֹ����
                    if ((rate == 1 && startx > coordTarget.x) || (rate == -1 && startx < coordTarget.x)) {
                        startx = coordTarget.x;
                    }
                    var x = startx, y = a * x * x + b * x;

                    // ��ǵ�ǰλ�ã������в���ʹ�õ����ɣ�ʵ��ʹ�ÿ��Խ���һ��ע��
                    element.setAttribute("data-center", [Math.round(x), Math.round(y)].join());

                    // x, yĿǰ�����꣬��Ҫת���ɶ�λ������ֵ
                    if (moveStyle == "margin") {
                        element.style.marginLeft = x + "px";
                        element.style.marginTop = y + "px";
                    } else {
                        element.style[moveStyle] = "translate(" + [x + "px", y + "px"].join() + ")";
                    }

                    if (startx !== coordTarget.x) {
                        params.progress && params.progress(x, y);
                        window.requestAnimationFrame(step);
                    } else {
                        // �˶��������ص�ִ��
                        params.complete && params.complete();
                        flagMove = true;
                    }
                };
                window.requestAnimationFrame(step);
                flagMove = false;

                return this;
            };
            // ��ʼ������
            exports.init = function () {
                this.position().mark().move();
            };
        }
        return exports;
    };

    //������Ч��
    $.fn.rightSwipeAnimation = function (options) {
        var setting = {
            model: '.brandlayer',
            cache: false,
            site: '', //��������
            fnEnd: null
        }
        options = Object.extend(options, setting);
        var $this = this;
        var clientHeight = document.documentElement.clientHeight,
            clientWidth = document.documentElement.clientWidth;
        var $model = $(options.model);



        switch (options.site) {
            case 'up':
                $model.addClass('selectModelsUp');
                break;
            case 'right':
                $model.addClass('selectModelsRight');
                break;
            default:
                $model.addClass('selectModels');
                break;
        }

        //������ص�
        function clickEnd($current) {
            var $model = this;
            $model.addClass('swipeModels-block');
            options.fnEnd && options.fnEnd.call($model, $current);
        }

        $model.on('closeWindow', function (ev) {
            $model.removeClass('swipeModels-block');
        })

        this.each(function (index, curr) {
            var $current = $(this);
            (function ($o) {
                $o.click(function (ev) {
                    var $current = $(this);
                    if (options.cache && $model.html() != '') {
                        clickEnd.call($model, $current, ev);
                    } else {
                        $model.loadHtml({
                            end: function (html) {
                                $model.html(html);
                                clickEnd.call($model, $current, ev);
                            }
                        })
                    }
                })
            })($current);
        })
    }

    //ajax����
    $.fn.loadHtml = function (options) {
        var setting = {
            end: null,
            url: ''
        }
        options = Object.extend(options, setting);
        var $this = this,
        url = $this.attr('data-url') || options.url;
        if (!url) {
            options.end && options.end.call($this);
        } else {
            $.get(url, function (data) {
                options.end && options.end.call($this, data);
            });
        }
    }

    var tips = {};
    //������ʾ��
    $.fn.tip = function (json, options) {
        var setting = {
            tempateName: '#tipTempate', //ģ������
            delay: 1000,
            site: function () { //����λ��
                var $model = this;
                $model.css('position', 'fixed');
            }
        }
        options = Object.extend(options, setting);
        var $this = this,
            $tipTempate = $(options.tempateName);

        if (tips[options.tempateName]) {
            return;
        }

        var tempateText = $tipTempate.html()
        for (var n in json) {
            tempateText = tempateText.replace(eval('/{' + n + '}/g'), json[n]);
        };

        tips[options.tempateName] = true;
        var $model = $('<div>').html(tempateText).children(0);
        options.site && options.site.call($model);
        $this[0].appendChild($model[0]);
        setTimeout(function () {
            $model.remove();
            tips[options.tempateName] = false;
        }, options.delay);
    }

    //������
    $.fn.model = function (options) {
        var setting = {
            clickEnd: null
        }
        options = Object.extend(options, setting);
        this.each(function (index, current) {
            (function ($current) {
                $current.click(function (ev) {
                    ev.preventDefault();
                    var $click = $(this);
                    $click.$model = $('.' + $click.attr('data-action'));
                    if ($click.$model.length > 0) {
                        $click.$mark = $('.' + $click.$model.attr('data-back'));

                        if (!$click.$model.attr('data-init')) {
                            $click.$mark.on('close', function (ev) {
                                $click.$model.hide();
                                $click.$mark.hide();
                            })
                            $click.$model.attr('data-init', 'true');
                        }

                        options.clickEnd && options.clickEnd.call($click);

                        $click.$mark.show();
                        $click.$model.show();
                    }
                })
            })($(current));
        })
    }
})(jQuery);
