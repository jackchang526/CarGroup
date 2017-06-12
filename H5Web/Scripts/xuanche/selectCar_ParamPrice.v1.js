$(function () {
	var $ruler = $('.ruler'),
		$sliderbar = $ruler.find('.sliderbar'),
		$labels = $sliderbar.find('.line'),
		min = parseInt($sliderbar.attr('data-min')),
		max = parseInt($sliderbar.attr('data-max')),
		labelW = parseInt($labels.attr('data-width')),
		currents = $sliderbar.find('.current'),
		$dot = $sliderbar.find('.touch-dot');
	$sliderbar.scrollbarX({
		touchstart: function (v) {
			$dot.css('left', v.currentIndex + '%');
			// $dot.html(v.currentIndex);
			$dot.show();

			//100+
			if (v.currentIndex == 100) {
				$dot.html(v.currentIndex + "+");
			} else {
				$dot.html(v.currentIndex);
			}
		},
		touchmove: function (v) {
			var $current = this,
				$not = null;
			$span = $current.find('span');
			if (currents.length > 1) {
				currents.each(function (index, curr) {
					if ($(curr).attr('data-bar') != $current.attr('data-bar')) {
						$not = $(curr);
					}
				})

				if ($current.attr('data-bar') == '1' && v.currentIndex >= parseInt($not.attr('data-index'))) {
					return;
				} else if ($current.attr('data-bar') == '2' && v.currentIndex <= parseInt($not.attr('data-index'))) {
					return;
				}
				//console.log($current);
			}

			$dot.css('left', v.currentIndex + '%');
			$current.width(v.x);
			$current.attr('data-index', v.currentIndex);

			//100+
			if (v.currentIndex == 100) {
				$span.html(v.currentIndex + "+");
				$dot.html(v.currentIndex + "+");
			} else {
				$span.html(v.currentIndex);
				$dot.html(v.currentIndex);
			}

			//console.log(v.currentIndex);
		},
		touchend: function () {
			$dot.hide();
			var max = parseInt($sliderbar.find('.max-dot span').html()),
				min = parseInt($sliderbar.find('.min-dot span').html());
			if (min < 0) min = 0;
			if (max == 100) max = 9999;
		    
		    SelectByParam.paramObj["p"] = min + "-" + max;

		    //if (SelectByParam.params.toString().indexOf("p=") > -1) {
			//	for (var i = 0; i < SelectByParam.params.length; i++) {
			//		if (SelectByParam.params[i].split("=")[0] == "p") {
			//			SelectByParam.params.splice(i,1);
			//		}					
			//	}
			//	SelectByParam.params.push("p=" + min + "-" + max);
			//} else {
			//	SelectByParam.params.push("p=" + min + "-" + max);
			//}
			
			SelectByParam.UpdateCarResult();

			//console.log(min, max);
		}
	});

	$ruler.on('setvalue', function (ev, paras) {
	    $sliderbar.find('[data-bar=1]').attr('data-index', paras.min).find('span').html(paras.min);
	    $sliderbar.find('[data-bar=2]').attr('data-index', paras.max).find('span').html(paras.max);
	    $sliderbar.trigger('refresh');
	});
});
