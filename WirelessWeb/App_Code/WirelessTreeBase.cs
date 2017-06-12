using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.Utils;
using BitAuto.CarChannel.Common.Enum;
using BitAuto.CarChannel.BLL;
using BitAuto.CarChannel.Model;
using System.Collections.Specialized;
using System.Web;
using BitAuto.CarChannel.BLL.CarTreeData;
using BitAuto.CarUtils.Define;

namespace WirelessWeb
{
	public class WirelessTreeBase : BitAuto.CarChannel.Common.PageBase
	{
		protected string _ConditionsHtml = string.Empty;
		protected string _SearchUrl = string.Empty;

        #region
        //protected SelectCarParameters GetSelectCarParas()
        //{
        //    SelectCarParameters selectParas = new SelectCarParameters();
        //    string conditionStr = "";
        //    //价格
        //    string tmpStr = HttpContext.Current.Request.QueryString["p"];
        //    if (!String.IsNullOrEmpty(tmpStr))
        //    {
        //        string[] pc = tmpStr.Split('-');
        //        if (pc.Length == 2)
        //        {
        //            selectParas.MinPrice = ConvertHelper.GetInteger(pc[0]);
        //            selectParas.MaxPrice = ConvertHelper.GetInteger(pc[1]);
        //            if (selectParas.MaxPrice == 9999)
        //                selectParas.MaxPrice = 0;
        //        }

        //        switch (tmpStr)
        //        {
        //            case "0-5":
        //                selectParas.PriceFlag = 1;
        //                conditionStr = "5万以下";
        //                break;
        //            case "5-8":
        //                selectParas.PriceFlag = 2;
        //                conditionStr = "5万-8万";
        //                break;
        //            case "8-12":
        //                selectParas.PriceFlag = 3;
        //                conditionStr = "8万-12万";
        //                break;
        //            case "12-18":
        //                selectParas.PriceFlag = 4;
        //                conditionStr = "12万-18万";
        //                break;
        //            case "18-25":
        //                selectParas.PriceFlag = 5;
        //                conditionStr = "18万-25万";
        //                break;
        //            case "25-40":
        //                selectParas.PriceFlag = 6;
        //                conditionStr = "25万-40万";
        //                break;
        //            case "40-80":
        //                selectParas.PriceFlag = 7;
        //                conditionStr = "40万-80万";
        //                break;
        //            case "80-9999":
        //                selectParas.PriceFlag = 8;
        //                conditionStr = "80万以上";
        //                break;
        //        }
        //    }
        //    //级别
        //    selectParas.Level = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["l"]);
        //    if (selectParas.Level > 0)
        //    {
        //        string levelName = "";
        //        //63是轿车的级别集合，
        //        if (selectParas.Level == 63)
        //        {
        //            levelName = "轿车";
        //        }
        //        else
        //        {
        //            //EnumCollection.SerialLevelEnum level = (EnumCollection.SerialLevelEnum)selectParas.Level;
        //            //levelName = Car_LevelBll.LevelNameDic[level.ToString()];
        //            levelName = CarLevelDefine.GetSelectCarLevelNameById(selectParas.Level);
        //            selectParas.Level = (int)Math.Pow(2, selectParas.Level - 1);
        //        }
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += levelName;
        //        selectParas.PriceFlag = 0;
        //    }

        //    //品牌类型，自主，合资，进口
        //    int brandType = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["g"]);
        //    if (brandType > 0)
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        switch (brandType)
        //        {
        //            case 1:
        //                selectParas.BrandType = 1;
        //                conditionStr += "自主";
        //                break;
        //            case 2:
        //                selectParas.BrandType = 2;
        //                conditionStr += "合资";
        //                break;
        //            case 4:
        //                selectParas.BrandType = 4;
        //                conditionStr += "进口";
        //                break;
        //            default:
        //                selectParas.BrandType = brandType;
        //                conditionStr += Enum.GetName(typeof(EnumCollection.FlagsBrandType), brandType);
        //                break;
        //        }
        //    }
        //    //品牌类型，自主，合资，进口
        //    int countryType = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["c"]);
        //    if (countryType > 0)
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        switch (countryType)
        //        {
        //            case 4:
        //                selectParas.Country = 4;
        //                conditionStr += "德系";
        //                break;
        //            case 2:
        //                selectParas.Country = 2;
        //                conditionStr += "日系";
        //                break;
        //            case 16:
        //                selectParas.Country = 16;
        //                conditionStr += "韩系";
        //                break;
        //            case 8:
        //                selectParas.Country = 8;
        //                conditionStr += "美系";
        //                break;
        //            case 484:
        //                selectParas.Country = 484;
        //                conditionStr += "欧系";
        //                break;
        //            default:
        //                selectParas.Country = countryType;
        //                conditionStr += Enum.GetName(typeof(EnumCollection.AreaCountries), countryType);
        //                break;
        //        }
        //    }
        //    //排量
        //    tmpStr = HttpContext.Current.Request.QueryString["d"];
        //    if (!String.IsNullOrEmpty(tmpStr))
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += tmpStr + "L";

        //        string[] dc = tmpStr.Split('-');
        //        if (dc.Length == 2)
        //        {
        //            selectParas.MinDis = ConvertHelper.GetDouble(dc[0]);
        //            selectParas.MaxDis = ConvertHelper.GetDouble(dc[1]);
        //            if (selectParas.MaxDis == 9.0)
        //                selectParas.MaxDis = 0.0;
        //        }
        //        selectParas.PriceFlag = 0;
        //    }

        //    //变速箱
        //    selectParas.TransmissionType = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["t"]);
        //    string transType = "";
        //    if (selectParas.TransmissionType >= 2)
        //    {
        //        transType = "自动";
        //        //selectParas.TransmissionType = 2 + 4 + 8 + 16;		//合并了自动，手自一体，CVT及双离合
        //    }
        //    else if (selectParas.TransmissionType == 1)
        //        transType = "手动";

        //    if (selectParas.TransmissionType != 0)
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += transType;
        //        selectParas.PriceFlag = 0;
        //    }
        //    //驱动
        //    string driveType = "";
        //    selectParas.DriveType = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["dt"]);
        //    if (selectParas.DriveType != 0) {
        //        switch (selectParas.DriveType) 
        //        {
        //            case 1: driveType = "前驱"; break;
        //            case 2: driveType = "后驱"; break;
        //            case 252: driveType = "四驱"; break;
        //            case 4: driveType = "全时四驱"; break;
        //            case 8: driveType = "分时四驱"; break;
        //            case 16: driveType = "适时四驱"; break;
        //            default: break;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(driveType))
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += driveType;
        //    }
        //    //燃料
        //    string fuelType = "";
        //    selectParas.FuelType = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["f"]);
        //    if (selectParas.FuelType != 0)
        //    {
        //        switch (selectParas.FuelType)
        //        {
        //            case 7: fuelType = "汽油"; break;
        //            case 8: fuelType = "柴油"; break;
        //            case 16: fuelType = "纯电动"; break;
        //            case 2: fuelType = "油电混合"; break;
        //            case 4: fuelType = "油气混合"; break;
        //            default: break;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(fuelType))
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += fuelType;
        //    }
        //    //车门数
        //    var bodyDoors = HttpContext.Current.Request.QueryString["more"];
        //    if (!string.IsNullOrEmpty(bodyDoors))
        //    {
        //        if (bodyDoors.IndexOf("268") > -1)
        //        {
        //            selectParas.MinBodyDoors = 2;
        //            selectParas.MaxBodyDoors = 3;
        //        }
        //        if (bodyDoors.IndexOf("270") > -1)
        //        {
        //            selectParas.MinBodyDoors = 4;
        //            selectParas.MaxBodyDoors = 6;
        //        }
        //    }
        //    if (selectParas.MinBodyDoors>0&&selectParas.MaxBodyDoors>0)
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += selectParas.MinBodyDoors+"门-"+selectParas.MaxBodyDoors+"门";
        //    }
        //    //座位数
        //    var perfSeatNum = HttpContext.Current.Request.QueryString["more"];
        //    if (!string.IsNullOrEmpty(perfSeatNum))
        //    {
        //        if (perfSeatNum.IndexOf("262") > -1)
        //        {
        //            selectParas.MinPerfSeatNum = 2;
        //        }
        //        else if (perfSeatNum.IndexOf("263") > -1)
        //        {
        //            selectParas.MinPerfSeatNum = 4;
        //            selectParas.MaxPerfSeatNum = 5;
        //        }
        //        else if (perfSeatNum.IndexOf("265") > -1)
        //        {
        //            selectParas.MinPerfSeatNum = 6;
        //        }
        //        else if (perfSeatNum.IndexOf("266") > -1)
        //        {
        //            selectParas.MinPerfSeatNum = 7;
        //        }
        //        else if (perfSeatNum.IndexOf("267") > -1)
        //        {
        //            selectParas.MinPerfSeatNum = 8;
        //            selectParas.MaxPerfSeatNum = 9999;
        //        }
        //        else { }
        //    }
        //    if (selectParas.MinPerfSeatNum > 0)
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        if (selectParas.MinPerfSeatNum == 8) {
        //            conditionStr +=  "7座以上";
        //        }
        //        else{
        //            conditionStr += selectParas.MinPerfSeatNum + "座";
        //        }
        //        if (selectParas.MaxPerfSeatNum > 0)
        //        {
        //            if (selectParas.MaxPerfSeatNum != 9999)
        //            {
        //                conditionStr += "-" + selectParas.MaxPerfSeatNum + "座";
        //            }
        //        }
        //    }

        //    //是否旅行版/箱式
        //    string carBody = "";
        //    selectParas.IsWagon = ConvertHelper.GetInteger(HttpContext.Current.Request.QueryString["lv"]);
        //    if (selectParas.IsWagon == 1)
        //    {
        //        carBody = "旅行版";
        //    }
        //    tmpStr = HttpContext.Current.Request.QueryString["b"];
        //    int bodyForm = 0;
        //    Int32.TryParse(tmpStr, out bodyForm);
        //    selectParas.BodyForm = bodyForm;
        //    if (selectParas.BodyForm != 0)
        //    {
        //        switch (selectParas.BodyForm)
        //        {
        //            case 1: carBody = "两厢"; break;
        //            case 2: carBody = "三厢"; break;
        //            default: break;
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(carBody))
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += carBody;   
        //    }
        //    //更多条件
        //    tmpStr = HttpContext.Current.Request.QueryString["more"];
        //    if (!String.IsNullOrEmpty(tmpStr))
        //    {
        //        //int mcLength = tmpStr.Length;
        //        //if (mcLength > 30)
        //        //    mcLength = 30;
        //        //for (int i = 0; i < mcLength; i++)
        //        //{
        //        //    if (tmpStr[i] == '1')
        //        //    {
        //        //        selectParas.CarConfig += (int)Math.Pow(2, i);
        //        //        selectParas.PriceFlag = 0;
        //        //    }
        //        //}
        //        tmpStr = tmpStr.Replace("262", "").Replace("263", "").Replace("265", "").Replace("266", "").Replace("267", "").Replace("268", "").Replace("270", "");
        //        string[] morePeizhi = tmpStr.Split('_');
        //        foreach (string item in morePeizhi)
        //        {
        //            if (!string.IsNullOrEmpty(item))
        //            {
        //                selectParas.CarConfig += Convert.ToInt32(item);
        //                selectParas.PriceFlag = 0;
        //            }
        //        }
        //    }
        //    if (selectParas.CarConfig>0)
        //    {
        //        if (conditionStr.Length > 0)
        //            conditionStr += "_";
        //        conditionStr += selectParas.CarConfig;
        //    }
			
        //    selectParas.ConditionString = conditionStr;

        //    return selectParas;
        //}
        #endregion
        /// <summary>
        /// 根据url参数获取xml广告车款 (自定义xml name)
        /// </summary>
        /// <returns></returns>
        /// 
        protected SelectCarParameters GetSelectCarParas()
        {
            SelectCarParameters selectParas = new SelectCarParameters();
            string conditionStr = "";
            
            string price = HttpContext.Current.Request.QueryString["p"];
            string level = HttpContext.Current.Request.QueryString["l"];
            string brandType = HttpContext.Current.Request.QueryString["g"];
            string countryType = HttpContext.Current.Request.QueryString["c"];
            string sweptVolume = HttpContext.Current.Request.QueryString["d"];
            string transmissionType = HttpContext.Current.Request.QueryString["t"];
            string driveType = HttpContext.Current.Request.QueryString["dt"];
            string fuelType = HttpContext.Current.Request.QueryString["f"];
            string isWagon = HttpContext.Current.Request.QueryString["lv"];
            string bodyForm = HttpContext.Current.Request.QueryString["b"];
            string configMore = HttpContext.Current.Request.QueryString["more"];
            conditionStr = makeCondtionStrForAdXml(conditionStr, "p", price);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "l", level);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "g", brandType);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "c", countryType);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "d", sweptVolume);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "t", transmissionType);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "dt", driveType);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "f", fuelType);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "lv", isWagon);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "b", bodyForm);
            conditionStr = makeCondtionStrForAdXml(conditionStr, "more", configMore);
            selectParas.ConditionString = conditionStr;
            return selectParas;
        }
        public string makeCondtionStrForAdXml(string condition, string key, string value)
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(value))
            {
                condition += string.Format("#{0}={1}", key, value);
            }
            if (!String.IsNullOrEmpty(condition))
            {
                result = condition;
            }
            return result;
        }

		protected string InitSearchUrl(string tagType)
		{
			_SearchUrl = String.Empty;
			Dictionary<string, TagData> tagDic = TagData.GetTagDataDictionary();
			if (tagDic.ContainsKey(tagType))
				_SearchUrl = tagDic[tagType].UrlDictionary["search"].UrlRule;
			int position = _SearchUrl.IndexOf('?');
			if (position >= 0)
				_SearchUrl = _SearchUrl.Remove(position, _SearchUrl.Length - position);
			return _SearchUrl;
		}
	}
}
