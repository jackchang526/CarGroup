var LastWatchCar = {
	contentId: 'lastWatchCar',
	hrefFormat: '<a href="@url@" target="_blank">@name@</a>',
	initWatchCar: function()
	{
		var contentObject = document.getElementById(LastWatchCar.contentId);
		if (contentObject == null)
		{
			return;
		}
		if (typeof go_data == 'undefined'
            || go_data == null
            || go_data.result == null
            || go_data.result.length < 1)
		{
			contentObject.style.display = "none";
			return;
		}
		var content = "";
		if (go_data.result.length > 0)
		{
			contentObject.style.display = "none";
			for (var i = 0; i < go_data.result.length; i++)
			{
				content += LastWatchCar.hrefFormat.replace(/@url@/g, go_data.result[i].url);
				content = content.replace(/@name@/g, go_data.result[i].name);
			}
			contentObject.innerHTML = contentObject.innerHTML + content;
		}
		else
			contentObject.style.display = "block";
	},
	load: function()
	{
		if (!document.getElementById || !document.createTextNode) { return; }
		LastWatchCar.initWatchCar();
	}
}
DomHelper.addEvent(window, "load", LastWatchCar.load, false);
