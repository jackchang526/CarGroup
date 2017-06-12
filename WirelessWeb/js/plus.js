$(function () {
    var $root = $('.m-tool-compare');
    //浮动层
    (function ($root) {
        var $flex = $root.find('.flex'),
            $header = $flex.find('.section-tt table'),
            $headerLi = $header.find('span'),
            $append = $root.find('.flex-append');
        var flexTop = $flex[0].offsetTop, flexHeight = $flex.height();

        var rows = $('.phone-title', $root);
        var arr = [];
        var startTop = 0;
        for (var i = 0; i < rows.length; i++) {
            arr.push(rows[i].getAttribute('data-key'));
        }
        //导航插件
        rows.navigate({
            speed: 30,
            top: $flex.offsetHeight() - 35,
            init: function () {
                $(document.body).touches({
                    touchstart: function (ev) {
                        startTop = $(window).scrollTop();
                    }
                })
            },
            selectFn: function (idx) {
                var $this = this;
                var k = arr[idx];
                clearTimeout($this.timeout)
                $this.timeout = setTimeout(function () { $headerLi.html(k); }, 50);
            },
            scrollTo: function () {
                var scrollT = document.documentElement.scrollTop || document.body.scrollTop;
                $flex.css({ 'position': 'initial' });
                $append.hide();
                if (scrollT >= flexTop) {
                    $flex.css({ 'position': 'fixed', 'top': 0, 'left': 0 });
                    $append.show();
                }
            }
        });
    })($root);

    //左右滑动
    (function ($root) {
        var $contflex = $('.section-flex .flex-right', $root);
        var $contTx = $('.section-tx .cont', $root);

        function flexScrollTo(ev) {
            if ($contflex.site == '') {
                $contflex.site = Math.abs(this.x - $contflex.x) > Math.abs(this.y - $contflex.y) ? 'x' : 'y';
            }
            if ($contflex.site == 'x') {
                ev.preventDefault();
                txScroll.scrollTo(this.x, 0, 0, false);
            } else {
                this.disable();
            }
        }

        function flexTouchEnd() {
            var $this = this;
            clearInterval($this.interval);
            $this.interval = setInterval(function () {
                txScroll.scrollTo($this.x, 0, 0, false);
                if ($this.ox == $this.x) {
                    clearInterval($this.interval)
                }
                $this.ox = $this.x;
            }, 30);
        }

        var flexScroll = new iScroll($contflex[0], {
            scrollX: true,
            scrollY: false,
            mouseWheel: true,
            scrollbarClass: 'nonebar',
            bounce: false,
            momentum: true,
            lockDirection: true,
            snap: 'td',
            userTransiton: true,
            onScrollMove: flexScrollTo,
            onTouchEnd: flexTouchEnd,
            onBeforeScrollStart: function (ev) {
                ev.preventDefault();
                $contflex.x = this.x;
                $contflex.y = this.y;
            },
            onScrollStart: function () {
                $contflex.site = '';

            }
        });



        $contflex.touches({ touchend: function () { flexScroll.enable(); } })

        function txScrollTo(ev) {
            if ($contTx.site == '') {
                $contTx.site = Math.abs(this.x - $contTx.x) > Math.abs(this.y - $contTx.y) ? 'x' : 'y';
            }
            if ($contTx.site == 'x') {
                ev.preventDefault();
                flexScroll.scrollTo(this.x, 0, 0, false);
            } else {
                this.disable();
            }

        }

        function txTouchEnd() {
            var $this = this;
            clearInterval($this.interval);
            $this.interval = setInterval(function () {
                flexScroll.scrollTo($this.x, 0, 0, false);
                if ($this.ox == $this.x) {
                    clearInterval($this.interval)
                }
                $this.ox = $this.x;
            }, 30);

        }

        var txScroll = new iScroll($contTx[0], {
            scrollX: true,
            scrollY: false,
            mouseWheel: true,
            scrollbarClass: 'nonebar',
            momentum: true,
            bounce: false,
            lockDirection: true,
            snap: 'td',
            userTransiton: true,
            onScrollMove: txScrollTo,
            onTouchEnd: txTouchEnd,
            onBeforeScrollStart: function (ev) {
                $contTx.x = this.x;
                $contTx.y = this.y;
            },
            onScrollStart: function () {
                $contTx.site = '';
            }
        });
        $contTx.touches({ touchend: function () { txScroll.enable(); } });
    })($root);

    //分类按钮
    var $peizhi = $('.more-peizhi-list');
    $('.category').on('click', function (ev) {
        ev.preventDefault();
        //按下后处理事件
        if ($peizhi[0].style.display == 'none') {
            $peizhi.show();
        } else {
            $peizhi.hide();
        }
    })

})