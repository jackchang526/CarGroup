using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MWeb.RouteConstraint;

namespace MWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //==iis 重写组件
            //品牌选车
            routes.MapRoute(
                name: "brandlist",
                url: "brandlist.html",
                defaults: new { controller = "SelectMasterBrand", action = "Index", id = UrlParameter.Optional }
            );
			//品牌选车
			routes.MapRoute(
				name: "brandlist2",
				url: "brandlist/",
				defaults: new { controller = "SelectMasterBrand", action = "Index", id = UrlParameter.Optional }
			);

            //情景选车
            routes.MapRoute(
                            name: "scenelist",
                            url: "scenelist.html",
                            defaults: new { controller = "SelectCarOfScene", action = "Index", id = UrlParameter.Optional }
                        );
            //情景选车
            routes.MapRoute(
                            name: "selectscenecar",
                            url: "selectscenecar",
                            defaults: new { controller = "SelectSceneCar", action = "Index", id = UrlParameter.Optional }
                        );
            //车型对比
            routes.MapRoute(
                            name: "chexingduibi",
                            url: "chexingduibi",
                            defaults: new { controller = "CarCompareTool", action = "Index", id = UrlParameter.Optional }
                        );
            //全款计算详情页
            routes.MapRoute(
                            name: "gouchejisuanqi",
                            url: "gouchejisuanqi",
                            defaults: new { controller = "Calc", action = "CalcAutoCashDetail", id = UrlParameter.Optional }
                        );
            //全款计算选车页
            routes.MapRoute(
                            name: "gouchejisuanqical",
                            url: "gouchejisuanqical",
                            defaults: new { controller = "Calc", action = "CalcAutoCash", id = UrlParameter.Optional }
                        );
            //贷款计算选车页
            routes.MapRoute(
                            name: "qichedaikuanjisuanqical",
                            url: "qichedaikuanjisuanqical",
                            defaults: new { controller = "Calc", action = "CalcAutoLoan", id = UrlParameter.Optional }
                        );
            //贷款计算详情页
            routes.MapRoute(
                            name: "qichedaikuanjisuanqi",
                            url: "qichedaikuanjisuanqi",
                            defaults: new { controller = "Calc", action = "CalcAutoLoanDetail", id = UrlParameter.Optional }
                        );
            //保险计算选车页
            routes.MapRoute(
                            name: "qichebaoxianjisuancal",
                            url: "qichebaoxianjisuancal",
                            defaults: new { controller = "Calc", action = "CalcInsurance", id = UrlParameter.Optional }
                        );
            //保险计算详情页
            routes.MapRoute(
                            name: "qichebaoxianjisuan",
                            url: "qichebaoxianjisuan",
                            defaults: new { controller = "Calc", action = "CalcInsuranceDetail", id = UrlParameter.Optional }
                        );
            //选车工具
            routes.MapRoute(
               name: "xuanchegongju",
               url: "xuanchegongju/",
               defaults: new { controller = "SelectCar", action = "Index", id = UrlParameter.Optional }
           );

			//选车工- 级别
			routes.MapRoute(
			   name: "level",
			   url: "{level}/",
			   defaults: new { controller = "SelectCar", action = "Level", id = UrlParameter.Optional },
			   constraints: new { level = @"(weixingche|xiaoxingche|jincouxingche|zhongxingche|zhongdaxingche|haohuaxingche|mpv|suv|paoche|mianbaoche|pika|qita)" }
		   );
            //保值率
            routes.MapRoute(
                name:"baozhilv",
                url:"{level}/baozhilv/",
                defaults:new { Controller = "BaoZhiLv", action="Index",id = UrlParameter.Optional },
                constraints: new { level = @"(weixingche|xiaoxingche|jincouxingche|zhongxingche|zhongdaxingche|haohuaxingche|mpv|suv|paoche|mianbaoche)" }
                );

            //suv
            routes.MapRoute(
               name: "suvchannel",
               url: "suv/all/",
               defaults: new { controller = "SUVChannel", action = "Index", id = UrlParameter.Optional }
           );
            //suv 列表
            routes.MapRoute(
               name: "suvchannellist",
               url: "suv/all/list/",
               defaults: new { controller = "SUVChannel", action = "List", id = UrlParameter.Optional }
           );
            //牙膏选车
            routes.MapRoute(
               name: "yagaoxuanche",
               url: "yagaoxuanche",
               defaults: new { controller = "YaGaoSelectCar", action = "Index", id = UrlParameter.Optional }
           );
            //==URL rewrite 组件 
			//品牌列表
			routes.MapRoute(
			   name: "selectbrand",
			   url: "brandlist/{allspell}",
			   defaults: new { controller = "SelectBrand", action = "Index", id = UrlParameter.Optional },
			   constraints: new { allspell = new MasterAllSpellConstraint() }
		   );
            //文章列表
            routes.MapRoute(
               name: "newslist",
               url: "{allspell}/{newstags}",
               defaults: new { controller = "NewsList", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = new SerialAllSpellConstraint(), newstags = @"(wenzhang|xinwen|daogou|hangqing|yongche|shijia|pingce|gaizhuang|anquan|keji|wenhua)" }
           );
            //车系配置
            routes.MapRoute(
               name: "cspeizhi",
               url: "{allspell}/peizhi",
               defaults: new { controller = "CsCompare", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = new SerialAllSpellConstraint() }
           );
		   // //车款图片
		   // routes.MapRoute(
		   //	name: "carphoto",
		   //	url: "{allspell}/m{carid}/tupian",
		   //	defaults: new { controller = "CarPhoto", action = "Index", id = UrlParameter.Optional },
		   //	constraints: new { allspell = @"([\w-]+)", carid = @"\d+" }
		   //);
            //车款配置
            routes.MapRoute(
               name: "carcompare",
               url: "{allspell}/m{carid}/peizhi",
               defaults: new { controller = "CarCompare", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = @"([\w-]+)", carid = @"\d+" }
           );
            //车款综述
            routes.MapRoute(
               name: "carsummary",
               url: "{allspell}/m{carid}",
               defaults: new { controller = "CarSummary", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = @"([\w-]+)", carid = @"\d+" }
           );

            //主品牌
            routes.MapRoute(
               name: "master",
               url: "{allspell}",
               defaults: new { controller = "Master", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = new MasterAllSpellConstraint() }
           );
            //品牌
            routes.MapRoute(
               name: "brand",
               url: "{allspell}",
               defaults: new { controller = "Brand", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = new BrandAllSpellConstraint() }
           );
            //子品牌
            routes.MapRoute(
               name: "cssummary",
               url: "{allspell}",
               defaults: new { controller = "CsSummary", action = "Index", id = UrlParameter.Optional },
               constraints: new { allspell = new SerialAllSpellConstraint() }
               //constraints: new { allspell = @"([\w-]+)" } 
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}