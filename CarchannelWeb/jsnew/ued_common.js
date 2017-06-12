$(function () {
	$("#bt_car_spcar").hover(
				function () {
					//$(this).addClass("car_sp_hover");
					$(this).find("dd").show();
				},
				function () {
					//$(this).removeClass("car_sp_hover");
					$(this).find("dd").hide();
				}
			);
	$("#bt_car_spcar_table").hover(
				function () {
					//$(this).addClass("car_sp_hover");
					$(this).find("dd").show();
				},
				function () {
					//$(this).removeClass("car_sp_hover");
					$(this).find("dd").hide();
				}
			);
});