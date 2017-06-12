/*公共功能*/
(function ($) {

    window.YICHE_COMMON_FUNC = function () {

        /*
        * tab容器.js-tab-container
        * tab选项.js-tab-menu
        * tab内容.js-tab-content
        * */
        var _yiche_tab = function (options) {
            var settings = {
                removeClassName: "current", //切换classname 高亮当前选项
                controlType: "click" //默认为点击事件
            };
            $.extend(settings, options);
            var $jsTabContainer = $(".js-tab-container");
            $jsTabContainer.on(settings.controlType, ".js-tab-menu", function (e) {
                e.preventDefault();
                var index = $(this).parent().find(".js-tab-menu").index(this);
                $(this).siblings(".js-tab-menu").removeClass(settings.removeClassName).end().addClass(settings.removeClassName);
                $(this).closest(".js-tab-container").find(".js-tab-content").addClass("hide").eq(index).removeClass("hide");
            })
        }

        //返回核心方法
        return {
            yicheTabFunc: _yiche_tab
        }
    }();

})(jQuery);
