define(["jquery", "anchors"], function ($,anchors) {

    var targetAuchors = anchors.Auchors;
    var defaultAuchors = anchors.DefaultAuchors;

    $.each(defaultAuchors, function(index, name) {
        if (targetAuchors.indexOf(name) < 0) {
            $("div[data-anchor='" + name + "']").remove();
        }
    });
})