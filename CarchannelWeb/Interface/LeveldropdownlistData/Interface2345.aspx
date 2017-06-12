<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Interface2345.aspx.cs"
	Inherits="BitAuto.CarChannel.CarchannelWeb.Interface.LeveldropdownlistData.Interface2345" %>

<%@ OutputCache Location="Downstream" Duration="36000" VaryByParam="*" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>2345合作</title>
	<script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jsnew/dropdownlist.js?v=3.0"></script>
	<style type="text/css">
		.car-search { width: 605px; font-size: 12px; }
		.car-search form { float: left; }
		.car-search button { text-align: center; line-height: 23px; height: 28px; margin-right: 3px; display: inline; font-size: 14px; }
		.car-search select { width: 120px; margin-right: 3px; display: inline; padding: 4px 3px; }
		
		
		body, h1, h2, h3, h4, p, dl, dt, dd, ul, li, form, th, td, table, label, article, aside, dialog, footer, header, section, footer, nav, figure, hgroup { margin: 0; padding: 0; border: 0; outline: 0; font-size: 100%; vertical-align: baseline; background: transparent; }
		body, button, input, select, textarea, li, dt, dd, div, p, span { font: 12px/1 Arial; }
		article, aside, dialog, footer, header, section, footer, nav, figure, hgroup { display: block; }
		ul { list-style: none; }
		img { border: none; }
		em, b { font-style: normal; }
		b { font-weight: normal; }
		a { cursor: pointer; }
		button, input, select, textarea { font-size: 100%; outline: 0; vertical-align: middle; margin: 0; }
		button { cursor: pointer; }
		table { border-collapse: collapse; border-spacing: 0; }
		.clearfix:after { content: "\0020"; display: block; height: 0; clear: both; visibility: hidden; }
		.clearfix { clear: both; zoom: 1; }
	</style>
	<meta http-equiv="content-type" content="text/html; charset=gb2312" />
</head>
<body>
	<div class="car-search">
		<form>
		<!---Start:主品牌to子品牌-->
		<select id="master20" style="width: 120px">
		</select><select id="serial20" style="width: 120px"></select>
		<button id="goCarTree" type="button">
			看车型</button>
		</form>
	</div>
</body>
</html>
<script type="text/javascript">
	BitA.DropDownList({
		container: { master: "master20", serial: "serial20" },
		include: { serial: "1" },
		btn: {
			car: {
				id: "goCarTree",
				url: {
					serial: { url: "http://car.bitauto.com/{param1}/?WT.mc_id=2345ss", params: { param1: "urlSpell"} },
					master: { url: "http://car.bitauto.com/tree_chexing/mb_{param1}/?WT.mc_id=2345ss", params: { param1: "id"} }
				},
				defurl: { url: "http://car.bitauto.com/?WT.mc_id=2345ss" }
			}
		},
		bind: "click"
	});
</script>
