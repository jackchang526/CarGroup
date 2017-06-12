function GetCityFromSetcookie() {
	var city = 201;
	if (bit_IpRegion) {
		var arrBit_IpRegion = bit_IpRegion.split(";");
		if (arrBit_IpRegion.length > 1) {
			var arrZoneInfo = arrBit_IpRegion[1].split(",");
			if (arrZoneInfo.length >= 3) {
				city = arrZoneInfo[0];
			}
		}
	}
	return city;
}
function DealerLoader(brandId,eleId)
{
	this.BrandId = brandId;
	this.PageElement = document.getElementById(eleId);
	this.AjaxPath = "/interface/GetBrandDealer.aspx";
	this.OnAjaxSuccess = function(res)
	{
		if (!this.PageElement)
			return;
		if (res.length == 0)
			this.PageElement.style.display = "none";
		else
			this.PageElement.innerHTML = res;
	};

	this.LoadBrandDealer = function (cityId) {
		var self = this;
		/*
		var vendorOptions =
		{			
		parameters: "brandid=" + this.BrandId,
		method: "get",
		onSuccess: function(data){self.OnAjaxSuccess(data)}
		}
		new Ajax.Request(this.AjaxPath, vendorOptions);
		*/
		var delearUrl = self.AjaxPath + "?brandid=" + this.BrandId + "&city=" + (cityId || GetCityFromSetcookie());
		$.ajax({
			url: delearUrl,
			type: 'GET',
			dataType: 'html',
			timeout: 1000,
			success: function (data) { self.OnAjaxSuccess(data) }
		});
	};
}