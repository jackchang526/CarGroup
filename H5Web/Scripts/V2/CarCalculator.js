function GetIntValue(num) {
    num = num.toString().replace(/\,/g, '');
    return parseInt(num);
}
var CarCalculator = {//所有险种均按最低价算
    ReferPrice: 0,//车款裸价
    Compulsory: 0,//交强险
    CommonTotal: 0,//商业险合计
    Init: function () {
        if (carMinReferPrice && parseFloat(carMinReferPrice) > 0) {
            CarCalculator.ReferPrice = parseFloat(carMinReferPrice) * 10000;
        }
        else {
            return;
        }
        document.getElementById("lblCompulsory").innerHTML = CarCalculator.GetCompulsory();
        document.getElementById("lblCommonTotal").innerHTML = CarCalculator.GetCommonTotal();
    },
    GetCommonTotal: function () {
        var calcTPL = CarCalculator.CalcTPL();
        var calcCarDamage = CarCalculator.CalcCarDamage();
        var calcAbatement = CarCalculator.CalcAbatement(calcCarDamage, calcTPL);
        var calcCarTheft = CarCalculator.CalcCarTheft();
        var calcBreakageOfGlass = CarCalculator.CalcBreakageOfGlass();
        var calcCarCalculatorignite = CarCalculator.CalcCarCalculatorignite();
        var calcCarEngineDamage = CarCalculator.CalcCarEngineDamage(calcCarDamage);
        var calcCarDamageDW = CarCalculator.CalcCarDamageDW();
        var calcLimitofDriver = CarCalculator.CalcLimitofDriver();
        var calcLimitofPassenger = CarCalculator.CalcLimitofPassenger();
        return calcTPL + calcCarDamage + calcAbatement + calcCarTheft + calcBreakageOfGlass + calcCarCalculatorignite + calcCarEngineDamage + calcCarDamageDW + calcLimitofDriver + calcLimitofPassenger;
    },
    CalcTPL: function () {//第三者责任险
        return 710;
    },
    CalcCarDamage: function () {//车辆损失险
        return Math.round(CarCalculator.ReferPrice * 0.0095) + 285;
    },
    CalcAbatement: function (calcCarDamage, calcTPL) {//不计免赔特约险
        return Math.round((GetIntValue(calcCarDamage) + GetIntValue(calcTPL)) * 0.2);
    },
    CalcCarTheft: function () {//全车盗抢险
        return Math.round(CarCalculator.ReferPrice * 0.0049 + 120);
    },
    CalcBreakageOfGlass: function () {//玻璃单独破碎险
        return Math.round(CarCalculator.ReferPrice * 0.0019);
    },
    CalcCarCalculatorignite: function () {//自燃损失险
        return Math.round(CarCalculator.ReferPrice * 0.0015);
    },
    CalcCarEngineDamage: function (calcCarDamage) {//发动机特别损失险(车损险*5%)
        return Math.round(GetIntValue(calcCarDamage) * 0.05);
    },
    CalcCarDamageDW: function () {//车身划痕险
        return 400;
    },
    CalcLimitofDriver: function () {//司机座位责任险
        return 42;//10000*0.0042
    },
    CalcLimitofPassenger : function(){
        return 10000 * 0.0027 * 4;//默认4个座
    },
    GetCompulsory: function () {//交强险
        return 950;
    }
}