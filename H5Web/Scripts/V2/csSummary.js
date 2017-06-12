(function () {
	function prev() {
		$.fn.fullpage.moveSectionUp();
	}
	function next() {
		$.fn.fullpage.moveSectionDown();
	}

	function setHeader(anchor) {
		for (var attr in Config.pages) {
			if (anchor == attr) {
				var title = Config.pages[attr].title
				, share = Config.pages[attr].share;
				cssummary.setHeader(title, share);
				return;
			}
		}
	}


	$(function () {
		var $sectionbox = $('#fullpage'),
		$loading = $('.loading');
		$loading.touches({ touchstart: function (ev) { ev.preventDefault(); } });
		$sectionbox.fullpage({
			css3: true,
			scrollingSpeed: 700,
			controlArrows: false,
			verticalCentered: false,
			anchors: Config.anchors,
			afterLoad: function (anchorLink, index) {
				setHeader(anchorLink);
				setCurrentMenu(anchorLink);
				if (Config.loadeds.indexOf(anchorLink) < 0)
					$loading.show();
				var $page = $('[data-anchor=' + anchorLink + ']', $sectionbox);
				var extend = new fullPageExtend({ $page: $page, anchorLink: Config.anchorLink, index: index });
				if (!extend.isLoadedPage()) {
					Config.loadeds.push(anchorLink);
					extend.createFrame(extend.getSrc());
				}
				if (Config.loadeds.indexOf(anchorLink) < 0 || $loading.css("display") == "block")
					setTimeout(function () { $loading.hide(); }, 700);
			}
		});
	});

	function setCurrentMenu(tag) {
		if (Config.menu1)
			Config.menu1.each(function (index, item) {
				if (!!tag && tag == item) {
					var menu = $("li[data-index='1']");
					menu.siblings().removeClass("current");
					menu.addClass("current");
					return;
				}
			});
		if (Config.menu2)
			Config.menu2.each(function (index, item) {
				if (!!tag && tag == item) {
					var menu = $("li[data-index='2']");
					menu.siblings().removeClass("current");
					menu.addClass("current");
					return;
				}
			});
		if (Config.menu3)
			Config.menu3.each(function (index, item) {
				if (!!tag && tag == item) {
					var menu = $("li[data-index='3']");
					menu.siblings().removeClass("current");
					menu.addClass("current");
					return;
				}
			});
	}
	setCurrentMenu(window.location.hash.substr(1));
	$(".menu.fouth").bind("click", function (e) {
		if (e.target.tagName == "A") {
			var _parent = $(e.target).closest("ul");
			if (!_parent.attr("data-type")) {
				var currentMenu = $(e.target).closest("li");
				//隐藏其他菜单
				currentMenu.siblings().find("ul").slideUp();
				//更改当前状态
				currentMenu.siblings().removeClass("current");
				currentMenu.addClass("current");
				return;
			} else {
				$(e.target).closest("ul").slideUp();
				return;
			}
		}
		var currentMenu = $(e.target).closest("li");
		//隐藏其他菜单
		currentMenu.siblings().find("ul").slideUp();
		//更改当前状态
		currentMenu.siblings().removeClass("current");
		currentMenu.addClass("current");
		var childMenu = currentMenu.find("ul"), _display = childMenu.css("display");
		if (!_display || _display == 'block') {
			childMenu.slideUp();
		} else {
			childMenu.slideDown();
		}
	}
	);
})();