<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarLoanApply.aspx.cs" Inherits="WirelessWeb.CarLoanApply" %>
<% 
	//页面变量说明
	/**
	 * rateList : 页面显示的贷款比例列表
	 * monthList :　贷款期限列表
	 * IsAdCs : 是否只显示通用汽车金融公司（广告投放需要，如果该子品牌通用汽车金融已投放广告，车型接口中只显示该公司的贷款套餐，并且套餐选择模式改为单选）
	 * showHeader　：显示页面头，内嵌页面时控制
	 * showFooter：　是否显示页尾，内嵌页面时控制
	 * SerialInfo：对象，子品牌信息
	 * SerialInfo.AllSpell：子品牌全拼
	 * SerialInfo.Brand.MasterBrand.SeoName　：主品牌SEO名
	 * SerialInfo.Brand.SeoName：品牌SEO名
	 * CarInfo：对象，车款信息
	 * CarInfo.Name：车款名称
	 * SerialId ：子品牌ID
	 * MasterId ： 主品牌ID
	 * CarId : 传递到页面的车款ID,如果未传应该为最热门车款的ID
	 * CityId :IP定向后的城市ID
	 * Request.QueryString["ref"]：传递到页面ref参数，申请区分来源类型
	 * Request.QueryString["returnurl"]：提交完成返回的链接地址
	 * scriptDomain　：web.config中配置appSettings节点“CarLoanApiDoamin”，用于控制调用车贷接口的域名
	 * ConfigurationManager.AppSettings["UseLocalJS"] ： web.config，是否使用本地址脚本(切换线上脚本和本地脚本)
	 * Request.QueryString["debug"] ：控制是否使用非压缩脚本(debug=true)
	 **/
%>
<%
	var scriptDomain = ConfigurationManager.AppSettings["CarLoanApiDoamin"];
	List<int> rateList = new List<int>() { 20, 30, 40, 50 };
    List<ListItem> monthList = new List<ListItem>() { new ListItem("1年", "12"), new ListItem("2年", "24"), new ListItem("3年", "36"), new ListItem("5年", "60") };
    bool showHeader = true;
    bool.TryParse(Request.QueryString["showheader"] ?? "true", out showHeader);
    bool showFooter = true;
    bool.TryParse(Request.QueryString["showfooter"] ?? "true", out showFooter);
	bool IsAdCs = (Request["ref"] == "mdk1" || Request["ref"] == "mdk2") && AdCsIds.Contains(SerialId);
%>
<!doctype html>
<html lang="zh-cn">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0">
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>
        <%= "【" + this.SerialInfo.SeoName + (CarInfo == null ? "" : " " + CarInfo.Name) + "汽车贷款】" + SerialInfo.Brand.MasterBrand.SeoName + SerialInfo.Brand.SeoName + "车贷_免费在线提交-手机易车网"%></title>
    <meta name="Keywords" content="<%=  SerialInfo.Brand.MasterBrand.SeoName %>,<%= SerialInfo.Brand.SeoName%>,<%= SerialInfo.Brand.SeoName %>,汽车贷款<%= (CarInfo == null ? "" : " " + CarInfo.Name+",") %>,易车网" />
    <meta name="Description" content="<%= SerialInfo.Brand.SeoName%><%= SerialInfo.Brand.SeoName %>汽车贷款频道为您提供<%= SerialInfo.Brand.SeoName %>汽车贷款方案,<%= SerialInfo.Brand.SeoName %>贷计算,首付比例,贷款期限和年利率查询服务,<%= SerialInfo.Brand.SeoName %>车贷免费在线申请,<%= SerialInfo.Brand.SeoName %>车贷问题答疑,查<%= SerialInfo.Brand.SeoName %>最新车贷方案,就上手机易车网" />
    <!--#include file="~/ushtml/0000/yidongzhan_chedai-644.shtml"-->
</head>
<body>
    <!--内容-->
    <div class="mbt-page" style="<%= IsAdCs ? "min-height:461px;height:530px;":"" %>">
        <!--头部-->
        <%if (showHeader) {  %>
        <div class="b-return">
	    <a href="javascript:void(0);" class="btn-return" id="gobackElm">返回</a>
	    <span>免费申请贷款</span>
       </div>
        <%} %>
        <!--头部-->
		<section class="m-line-box m-btn-filter m-one-btn m-popup-arrow m-form-chedai-new mgt25">
		<%if (IsAdCs){ %>
		<iframe marginheight="0" marginwidth="0" frameborder="0" height="461px" width="320" scrolling="no" src="http://www.gmacsaic.net/Wap/applyIntention/iframe_bitauto_ApplyIntention_wap.aspx"></iframe>
			<%}
	else
	{ %><!--筛选条件-->
            <div class="search-box">
                <dl>
                    <dt>车款：</dt>
                    <dd>
                        <div class="m-cd-jxs-pop-box">
                            <ul>
                                <li id="m-show-item" class="m-btn-gray"><span></span><s></s><b style="display: none;"></b></li>
                            </ul>
                            <ul id="car-list-box" style="display: none;">
                                <li class="m-popup-item  m-popup-item-line">
                                    <div class="m-popup-box">
                                        <div class="m-popup">
                                            <ul id="car-list">
                                                <li name="car" data="101722"><a href="#">正在加载...</a></li>
                                            </ul>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </dd>
                    <dt>首付：</dt>
                    <dd id="rate-list">
                        <% foreach (var rate in rateList)
						   { %>
                        <a href="#" class="tj-sty" val="<%= rate /100.00 %>">
                            <%= rate%>%</a>
                        <%} %>
                    </dd>
                    <dt>期限：</dt>
                    <dd id="month-list">
                        <% foreach (var month in monthList)
						   { %>
                        <a href="#" class="tj-sty" val="<%= month.Value %>">
                            <%= month.Text%></a>
                        <%} %>
                    </dd>
                </dl>
            </div>
            <!--筛选条件-->
            <div class="pp-price-box">
                <span class="left-box">裸车价格：<em id="price-total">0万元</em></span> <span class="right-box">首付金额：<em id="price-firstpay">0万元</em></span>
            </div>
            <!--选择金融机构-->
            <form id="form_chedai" action="">
            <div class="m-cd-jxs-form m-chedai-box">
                <div class="m-changecity-box">
                    请选择金融机构：
                    <div class="m-btn-changecity-box">
                        <select id="prov-list" name="stateid">
                            <option value="0">请选择</option>
                        </select>
                        <select id="city-list" name="cityid">
                            <option value="0">请选择</option>
                        </select>
                    </div>
                </div>
                <div id="msg-select-pack" class="m-btn-text">
                    <div id="val-selectedpak">
                    </div>
                    <input id="packids" name="packids" value="" type="hidden" />
                </div>
                <div id="load-layer" class="load-box" style="display: none;">
                    努力加载中...
                </div>
                <!--错误提示-->
                <div id="empty-list" class="wrong-box" style="display: none;">
                    <div class="img-box">
                    </div>
                    <div class="txt-box">
                        抱歉，这款车在当前条件下暂无金融机构！
                    </div>
                </div>
                <!--错误提示-->
                <div id="pack-list" style="display: none;">
                </div>
                <div id="pack-more" style="display: none;">
                </div>
                <a id="btn-show-more" style="display: none;" class="m-more-dealer" href="#"><span id="restTotal">查看更多产品</span><s></s> </a><a id="btn-hide-more" style="display: none;" class="m-more-dealer m-more-dealer-hide" href="#"><span>收起</span><s></s> </a>
                <!--联系方式-->
                <div class="m-cd-jxs-form-img">
                    您的联系方式：</div>
                <div class="border-warp">
                    <div class="border-item border-w-item">
                        <div class="border-item-name">
                        </div>
                        <input name="name" type="text" placeholder="您的姓名" maxlength="6" />
                    </div>
                    <div class="border-sex">
                        <label>
                            <input name="gender" type="radio" checked="checked" value="1" />男</label>
                        <label>
                            <input name="gender" type="radio" value="2" />女</label>
                    </div>
                </div>
                <div id="val-name">
                </div>
                <div class="border-item">
                    <div class="border-item-phone">
                    </div>
                    <input type="text" name="mobile" placeholder="手机号码" maxlength="11" />
                </div>
                <div id="val-mobile">
                </div>
                <!--联系方式-->
            </div>
            <div class="m-cd-btn-box">
                <div style="display: block;" class="m-cd-jxs-form">
                    <a id="btn-submit" href="#" class="m-btn-line-pd10 m-btn-blue">免费申请贷款</a>
                </div>
            </div>
			<!--#include file="/include/pd/2012/wap/00001/20140604_wap_daikuan_tuiguang_Ad_Auto.shtml"-->
            </form>
            <!--选择金融机构-->
			<%} %>
        </section>
    </div>
    <!--内容-->
    <!--底部-->
    <script type="text/javascript" charset="utf-8" src="http://ip.bitauto.com/iplocation/setcookie.ashx"></script>
     <script type="text/javascript">
         var url = "http://car.m.yiche.com/";
    </script>
    <% if (showFooter){ %><!--#include file="~/html/footer.shtml"--><%} else { %> <!--#include file="~/html/footer_for_app.shtml"--> <%} %>
    <script type="text/javascript" src="http://image.bitautoimg.com/autoalbum/common/photo/js/jquery.js"></script>
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/WirelessJs/anchor.js?v=201209"></script>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlist.js?v=3.0"></script>
    <script src="http://image.bitautoimg.com/carchannel/jsnew/jquery.validate.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var chedai_config = { masterid: '<%= MasterId %>', csid: '<%= SerialId %>', carid: '<%= CarId %>', loanmonths: 36, payrate: .3, csspell: '<%=SerialInfo.AllSpell  %>', ref: '<%= Request.QueryString["ref"] %>',orderfield: "Default", returnurl: '<%= Request.QueryString["returnurl"] %>', debug: '<%= (Request.QueryString["debug"]=="true").ToString().ToLower() %>', localscript: '<%= (ConfigurationManager.AppSettings["UseLocalJS"]=="true").ToString().ToLower() %>' };
        chedai_config.single_select = <%= IsAdCs ? "true" : "false" %>
        <% if(!string.IsNullOrEmpty( scriptDomain)){ %>
            chedai_config.api_get_unique_package = 'http://<%= scriptDomain %>/InsuranceAndLoan/Api/Loan/<%= IsAdCs ? "GetPackagesForCompany" : "GetUniquePackagesByCompanyAndAppointFirst" %>/?downPaymentRate={4}&repaymentPeriod={5}&carModelId={2}&cityId={6}&orderBy={7}&pageNum={8}&packageId={3}<%= IsAdCs ? "&companyId=28" : ""  %>';
            chedai_config.api_submit_apply='http://<%= scriptDomain %>/InsuranceAndLoan/Api/Loan/SubmitApplication/?callback=?';
        <%}else{ %>
            chedai_config.api_get_unique_package = 'http://mai.bitauto.com/InsuranceAndLoan/Api/Loan/<%= IsAdCs ? "GetPackagesForCompany" : "GetUniquePackagesByCompanyAndAppointFirst" %>/?downPaymentRate={4}&repaymentPeriod={5}&carModelId={2}&cityId={6}&orderBy={7}&pageNum={8}&packageId={3}<%= IsAdCs ? "&companyId=28" : ""  %>';
		<%} %>
    </script>
    <script type="text/javascript">
    	(function () {
    		var ver = '20140708';
    		var url = 'http://image.bitautoimg.com/carchannel/chedai/chedaiv2.min.js?v=' + ver;
    		if (typeof chedai_config != 'undefined') {
    			if (chedai_config.debug == 'true') {
    				url = chedai_config.localscript == 'true' ? '/js/chedaiv2.mobile.js' : 'http://image.bitautoimg.com/carchannel/WirelessJs/chedaiv2.mobile.js?v=' + ver
    			}
    			else {
    				url = chedai_config.localscript == 'true' ? '/js/chedaiv2.mobile.min.js' : 'http://image.bitautoimg.com/carchannel/WirelessJs/chedaiv2.mobile.min.js?v=' + ver
    			}
    		}
    		document.write('<scri' + 'pt type="text/javascript" src="' + url + '" type="text\/javascript"></scr' + 'ipt>');
    	})();
    </script>
</body>
</html>
