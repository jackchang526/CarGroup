/*品牌组装*/
$(function () {
    var $body = $('body');
    $body.on('brandinit', function (ev, paras) {
        var setting = {
            selected: false,
            model_hide: false, //隐藏车款层
            noteTouchStart: null,
            noteTouchEnd: null,
            actionName1: null,
            actionName2: null,
            actionName3: null,
            leftmaskalert: null,
            leftmaskback: null,
            masterselect: null,
            carselect: null,
            selectmark:null
        }
        var options = Object.extend(paras, setting);
        var $bc = $('.content[data-key]', $body),
        key = $bc.attr('data-key'),
        param = eval('api.' + key),
        $navs = $('.fixed-nav', $body);

        $bc.on('brandselect', function (ev, n) {
            $bc.find('li').removeClass('current');
            $bc.find('[data-id=' + n + ']').parent().addClass('current');
        });

        $bc.swipeApi({
            url: param.url,
            jsonpCallback: param.callName,
            analysis: function (data) {
                //主品牌模板
                var tp1 = $('#brandTemplate').html();
                var template = _.template(tp1);
                //导航模板
                var tp2 = $('#navTemplate').html();
                var template2 = _.template(tp2);

                return [template(data), template2(data)];
            },
            callback: function (htmls) {
                $bc.html(htmls[0]);
                $('.fixed-nav').html(htmls[1]);
                //笔记本滑动效果
                $body.trigger('note', { noteTouchStart: options.noteTouchStart, noteTouchEnd: options.noteTouchEnd });
                //右侧子品牌效果
                $body.trigger('rightswipe3', { model_hide: options.model_hide, selected: options.selected, actionName1: options.actionName1, actionName2: options.actionName2, actionNameModel: options.actionName3, back: options.leftmaskback, alert: options.leftmaskalert, masterselect: options.masterselect, carselect: options.carselect, selectmark: options.selectmark });
                //自适应页脚
                $body.footer({ footer: '.footer-box' });
            }
        })
    })
})