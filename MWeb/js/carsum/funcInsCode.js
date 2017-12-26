if ($("#isElectric").val() == "True") {
    $("#hidCarType").val("电动");
} else if ($("#isTingShou").val() == "停销") {
    $("#hidCarType").val("停销");
}

function showNewsInsCode(dxc, xxc, mpv, suv) {
    var adBlockCode = xxc;
    if (carlevel == '中大型车' || carlevel == '中型车' || carlevel == '豪华车') {
        adBlockCode = dxc;
    } else if (carlevel == '微型车' || carlevel == '小型车' || carlevel == '紧凑型车') {
        adBlockCode = xxc;
    } else if (carlevel == '概念车' || carlevel == 'MPV' || carlevel == '面包车' || carlevel == '皮卡' || carlevel == '卡车' || carlevel == '跑车' || carlevel == '客车' || carlevel == '其它') {
        adBlockCode = mpv;
    } else if (carlevel == 'SUV') {
        adBlockCode = suv;
    }
    document.write('<ins id="div_' + adBlockCode + '" type="ad_play" adplay_blockcode="' + adBlockCode + '"></ins>');
}
