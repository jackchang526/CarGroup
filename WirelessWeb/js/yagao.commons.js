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


//样式查找
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
}

//去重复
Array.prototype.distinct = function (options) {
    var setting = {
        contrast: function (json, i) { //判断表达式
            return !json[this[i]];
        },
        add: function (json, i) {
            json[this[i]] = 1;
        }
    }

    options = Object.extend(options, setting);

    var res = [];
    var json = {};

    for (var i = 0; i < this.length; i++) {
        if (this[i] && options.contrast.call(this, json, i)) {
            res.push(this[i]);
            options.add && options.add.call(this, json, i);
        }
    }
    return res;
}



//图片预加载
window.preload = function (options) {
    var setting = {
        progress: null,
        complete: null,
        fliterImg: function () {
            return $('img');
        }
    };
    options = Object.extend(options, setting);

    function getallBgimages() {
        var url, B = [], A = document.getElementsByTagName('*');
        A = B.slice.call(A, 0, A.length);
        while (A.length) {
            url = document.deepCss(A.shift(), 'background-image');
            if (url) url = /url\(['"]?([^")]+)/.exec(url) || [];
            url = url[1];
            if (url && B.indexOf(url) == -1) B[B.length] = url;
        }
        return B;
    }
    var arr = [], currentIdx = 0;

    //图片查找
    var imgs = options.fliterImg();
    for (var i = 0; i < imgs.length; i++) {
        arr.push({ src: imgs[i].src, type: 'img' });
    }

    var cssImages = getallBgimages();
    for (var i = 0; i < cssImages.length; i++) {
        arr.push({ src: cssImages[i], type: 'img' });
    }

    var exists = {};
    var arr1 = arr.distinct({
        contrast: function (exists, i) {
            return !exists[this[i].src];
        },
        add: function (exists, i) {
            exists[this[i].src] = 1;
        }
    });

    ////视频 / 音频
    $('video,audio').each(function (index, curr) {
        arr1.push({ o: curr, type: 'media' });
    })

    //成功回调
    function callback() {
        options.progress && options.progress.call(this, currentIdx, arr1.length);
        if (currentIdx == arr1.length - 1) {
            options.progress && options.progress.call(this, arr1.length, arr1.length);
            options.complete && options.complete();
        }
        currentIdx++;
    }

    for (var i = 0; i < arr1.length; i++) {
        var curr = arr1[i];
        switch (curr.type) {
            case 'img':
                $('<img>').attr('src', arr1[i].src).onload(function () {
                    callback();
                });
                break;
            case 'media':
                (function (o) {
                    o.addEventListener("canplay", function () {
                        callback();
                        clearTimeout(o.timeout);
                    }, false);
                    clearTimeout(o.timeout);
                    o.timeout = setTimeout(function () {
                        callback();
                    }, 5000);
                })(curr.o);
                break;
        }
    }
};

var classNames = ['Webkit', 'ms', 'Moz', 'O', ''];
var eventNames = ['webkit', 'moz', 'o'];

(function ($) {
    //触摸屏事件
    $.fn.touches = function (options) {
        var setting = {
            init: null,//初始化
            touchstart: null,  //按下
            touchmove: null, //滑动
            touchend: null //抬起
        };
        options = Object.extend(options, setting);
        var $this = this, touchesDiv = $this[0];
        touchesDiv.addEventListener('touchstart', function (ev) {
            options.touchstart && options.touchstart.call($this, ev);

            function fnMove(ev) {
                options.touchmove && options.touchmove.call($this, ev);
            }

            function fnEnd(ev) {
                options.touchend && options.touchend.call($this, ev);
                document.removeEventListener('touchmove', fnMove, false);
                document.removeEventListener('touchend', fnEnd, false);
            }
            document.addEventListener('touchmove', fnMove, false);
            document.addEventListener('touchend', fnEnd, false);
            return false;
        }, false)
        options.init && options.init.call($this);
    }

    //按钮事件
    $.fn.touchclick = function (options) {
        var $this = this,
            setting = {
                current: 'press',
                touchstart: null,
                touchend: null,
                click: null,
                delay: 400,
            }
        options = Object.extend(options, setting);
        $this.touches({
            touchstart: function (ev) {
                //ev.preventDefault();
                $this.addClass(options.current);
                options.touchstart && options.touchstart.call($this, ev);
            },
            touchend: function (ev) {
                setTimeout(function () { $this.removeClass(options.current); }, 1000)
                options.touchend && options.touchend.call($this, ev);
            }
        });
       $this.click(function (ev) {
            options.click && options.click.call($this, ev);
       })
    }

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

    //加载页面动画
    $.fn.loadAnimate = function (options) {
        var setting = {

        }
        options = Object.extend(options, setting);
        var $page = this;
        $page.find('[data-animate]').each(function (index, curr) {
            var $current = $(curr);
            var delay = parseFloat($current.attr('data-animate-delay'));
            if (delay > 0) {
                clearTimeout($current.timeout);
                $current.timeout = setTimeout(function () {
                    $current.addClass($current.attr('data-animate'));
                }, delay * 1000);
            } else {
                $current.addClass($current.attr('data-animate'));
            }

        })
    }

    //清除动画效果
    $.fn.removeAnimate = function (options) {
        var setting = {

        }
        options = Object.extend(options, setting);
        var $page = this;
        $page.find('[data-animate]').each(function (index, curr) {
            var $current = $(curr);
            $current.removeClass($current.attr('data-animate'));
        })
    }

    //页面过渡
    $.fn.pageTransitions = function (options) {
        var setting = {
            fnEnd: null,

        }
        options = Object.extend(options, setting);
        var $body = this,
        pages = $body.find('.page');
        $body.index = 0;
        $body.oindex = -1;
        $body.on('load', function (ev) {
            $body.animate({
                scrollTop: 0
            }, 30);
            if ($body.oindex >= 0) {
                var $opage = pages.eq($body.oindex);
                $opage.hide();
                $opage.removeAnimate();
            }
            var $page = pages.eq($body.index);
            $page.fadeIn();
            $page.loadAnimate();
            options.fnEnd && options.fnEnd.call($body, $page);
        })

        $body.on('next', function (ev) {
            $body.oindex = $body.index;
            $body.index++;
            if ($body.index >= pages.length - 1) { $body.index = pages.length - 1 }
            $body.trigger('load');
        })

        $body.on('prev', function (ev) {
            $body.oindex = $body.index;
            $body.index--;
            if ($body.index <= 0) { $body.index = 0; }
            $body.trigger('load');
        })


        $body.trigger('load');
    }


    //添加事件
    $.fn.addEvent = function (name, fn) {
        var obj = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < eventNames.length; i++) {
            obj.addEventListener(eventNames[i] + cName, fn, false);
        }
        obj.addEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
    }

    //删除事件
    $.fn.removeEvent = function (name, fn) {
        var obj = this[0];
        var cName = name.charAt(0).toUpperCase() + name.substring(1);
        for (var i = 0; i < eventNames.length; i++) {
            obj.removeEventListener(eventNames[i] + cName, fn, false);
        }
        obj.removeEventListener(name.charAt(0).toLowerCase() + name.substring(1), fn, false);
    }

    //transition事件监听
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

    //右侧弹出层
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

    //右侧附加选择层插件
    $.fn.rightSwipe = function (options, $click) {
        var $temp = null;
        var setting = {
            isclick: null,
            zindex: 999999,
            back: '.leftmask',
            alert: '.leftPopup',
            clickEnd: null, //打开关闭层回调事件
            oneEnd: null,
            closeEnd: null,
            currentid: null,
            selected: false, ///如果为真，查找currentid是否相等，如果符合就触发回调事件
            onBeforeScrollStart: function (ev) {
                ev.preventDefault();
            },
            clickCallBack: function (clickEnd) { //点击默认回调方法
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
                     
                        setTimeout(function () { $swipeLeft.addClass('swipeLeft-block'); setTimeout(function () { $this.clicked = false; }, 500); }, 200);

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
                       
                        if ($this.clicked) { ev.preventDefault(); return; }
                        $this.clicked = true;
                        options.onBeforeScrollStart.call($this, ev);

                        options.isclick && ($this.isclick = options.isclick.call($this));
                        if ($this.isclick == false) {
                            return;
                        }
                        options.clickCallBack.call($this, clickEnd);
                    })
                    //查找默认选中值，如果符合就触发回调事件
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
})(jQuery);

$(function ($) {
    var $body = $('body'),
        $car = $body.find('.car'),
        $bar = $body.find('.bar'),
        $barSpan = $bar.find('span'),
        barWidth = $bar.width() - 23,
        $loading = $body.find('.loading');
    $loading.show();
    preload({
        fliterImg: function () {
            return $('img:not([data-loading=false])');
        },
        progress: function (i, count) {
            var b = i / count;
            var w = (barWidth) * b;
            $barSpan.width(w);
            if ((barWidth - 45) >= w) {
                $car.css('left', w);
            } else {
                $car.css('left', barWidth - 45);
            }
        },
        complete: function () {
            $loading.hide();
            $body.pageTransitions({
                fnEnd: function ($page) {
                    $page.parents('body').css('background-color', '#fff');
                    switch ($body.index) {
                        case 0://第一页初始化
                            var $btnstart = $page.find('.btn-start');
                            if (!$btnstart.attr('init')) {
                                $btnstart.attr('init', 'true');
                                $btnstart.touchclick({
                                    click: function (ev) {
                                        setTimeout(function () { $body.trigger('next'); }, 200);
                                    }
                                })
                            }
                            
                            setTimeout(function () {
                                $page.find('.btn-start').removeClass('animated').removeClass('tada');
                            }, 2000);
                            BglogPostLog('27.114.1313');
                            break;
                        case 1: //第二页初始化
                            var $go = $page.find('.go');
                            if (!$go.attr('init')) {
                                $go.attr('init', 'true');
                                $go.click(function (ev) {
                                    ev.preventDefault();
                                    $body.trigger('prev');
                                })
                            }
                            
                            var $btnlist = $page.find('.btn-list');
                            var as = $btnlist.find('a')
                            if (!$btnlist.attr('init')) {
                                $btnlist.attr('init', 'true');
                                as.each(function (index, curr) {
                                    var $a = $(curr);
                                    $a.touchclick({
                                        click: function (ev) {
                                            ev.preventDefault();
                                            if (SelectCar) {
                                                SelectCar.Condition.yagao = $a.attr("data-id");
                                            }
                                            setTimeout(function () { $body.trigger('next'); }, 200);
                                            $page.removeAnimate();
                                        }
                                    })
                                })
                            }
                            as.each(function (index, curr) {
                                var $a = $(curr);
                                setTimeout(function () {
                                    $a.removeClass('animated').removeClass('bounceInOne');
                                }, 1000);
                            });
                            BglogPostLog('27.114.1314');
                            break;
                        case 2://第三页初始化
                            var $go = $page.find('.go');

                            var btns = $page.find('.btns-box');
                            var as = btns.find('a');

                            if (!$go.attr('init')) {
                                $go.attr('init', 'true');
                                $go.click(function (ev) {
                                    ev.preventDefault();
                                    $body.trigger('prev');
                                    as.each(function (index, curr) {
                                        $(curr).css('opacity', 0);
                                    });
                                    $page.removeAnimate();
                                })
                            }

                            if (!btns.attr('init')) {
                                btns.attr('init', 'true');
                                as.each(function (index, curr) {
                                    var $a = $(curr);
                                    $a.touchclick({
                                        click: function (ev) {
                                            ev.preventDefault();
                                            if (SelectCar) {
                                                SelectCar.Condition.price = $a.attr("data-id");
                                            }
                                            setTimeout(function () {
                                                $body.trigger('next');
                                                as.each(function (index, curr) {
                                                    $(curr).css('opacity', 0);
                                                });
                                                $page.removeAnimate();
                                            }, 200);
                                        }
                                    })
                                })
                            }
                            as.each(function (index, curr) {
                                var $a = $(curr);
                                setTimeout(function () {
                                    $a.css('opacity', 1).removeClass('animated').removeClass('bounceInOne');
                                }, 1400);
                            });
                            BglogPostLog('27.114.1315');
                            break;
                        case 3://第四页初始化
                            var $go = $page.find('.go');

                            var btns = $page.find('.list');
                            var as = btns.find('a');

                            if (!$go.attr('init')) {
                                $go.attr('init', 'true');
                                $go.click(function (ev) {
                                    ev.preventDefault();
                                    $body.trigger('prev');
                                    as.each(function (index, curr) {
                                        $page.removeAnimate();
                                        $(curr).css('opacity', 0);
                                    });
                                })
                            }


                            if (!btns.attr('init')) {
                                btns.attr('init', 'true');
                                as.each(function (index, curr) {
                                    var $a = $(curr);
                                    $a.touchclick({
                                        touchstart: function (ev) {
                                            var $click = $(this);
                                            $click.addClass('current');
                                        },
                                        click: function (ev) {
                                            var $click = $(this);
                                            if (SelectCar) {
                                                SelectCar.Condition.stage = $a.attr("data-id");
                                            }
                                            setTimeout(function () {
                                                $body.trigger('next');
                                                $click.removeClass('current');

                                                as.each(function (index, curr) {
                                                    $(curr).css('opacity', 0);
                                                });
                                                $page.removeAnimate();
                                            }, 100);
                                        },
                                        touchend: function (ev) {
                                            var $click = $(this);
                                            $click.removeClass('current');
                                        }
                                    })
                                })
                            }
                            as.each(function (index, curr) {
                                var $a = $(curr);
                                setTimeout(function () {
                                    $a.css('opacity', 1).removeClass('animated').removeClass('bounceInOne');
                                }, 1000);
                            });
                            BglogPostLog('27.114.1316');
                            break;
                        case 4://第五页初始化
                            var $go = $page.find('.go');
                            if (!$go.attr('init')) {
                                $go.attr('init', 'true');
                                $go.click(function (ev) {
                                    ev.preventDefault();
                                    $page.removeAnimate();
                                    $body.trigger('prev');
                                })
                            }
                            var btns = $page.find('.btns ul a');
                            if (!btns.attr('init')) {
                                btns.attr('init', 'true');
                                 
                            }
                            
                            if (SelectCar) {
                                SelectCar.GetSerialData();
                            }
                            $page.parents('body').css('background-color', '#f2f4f9');
                            break;
                    }
                }
            })
        }
    })
})