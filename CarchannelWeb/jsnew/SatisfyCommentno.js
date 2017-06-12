comment = (function () {



	var articleId;
	var isDisplay;
	var articleType;
	var BitAutoFcVersion;
	var $;
	var BitAutoFeedBack;

	function eventBind() {


		//展示
		BitAutoFeedBack.addEvent('ued_btn', 'click', function (event) {

			var manyi = document.getElementById("manyi");
			manyi.checked = "checked";

			var lay = document.getElementById("ued_lay");
			lay.style.display = 'block';

		});

		//关闭
		BitAutoFeedBack.addEvent('ued_lay_close', 'click', function (event) {
			var lay = document.getElementById("ued_lay");
			lay.style.display = 'none';
			$('txtContent').value = "";
		});

		//提交
		BitAutoFeedBack.addEvent('btnSubmit', 'click', function (event) {
			if (valid()) {
				saveComment();
			}

			BitAutoFeedBack.preventDefault(event);
		});

	}


	//验证
	function valid() {
		var value = $('txtContent').value.trim();
		if (value.length < 6) {


			$('div_cuo').style.display = "";
			$('div_cuo').innerHTML = "<i class='cuo'></i>至少6个字哦！";
			setTimeout(function () {
				$('div_cuo').style.display = "none";
				$('txtContent').focus();
			}, 1000);
			return false;
		}
		var maxLength = 200;
		var len = 0;
		//len = value.replace(/[^\x00-\xff]/g, "**").length;
		len = value.length;
		//alert(len);
		if (len > maxLength) {

			$('div_cuo').style.display = "";
			$('div_cuo').innerHTML = "<i class='cuo'></i>文字已超限制，请删减再提交。";
			setTimeout(function () {
				$('div_cuo').style.display = "none";
				$('txtContent').focus();
				$('lblDefaultValue').focus();
			}, 1000);

			return false;
		}
		return true;
	}

	function saveComment() {

		BitAutoFeedBack.xssRequest('API/CommentNo.ashx?ArticleId=' + articleId + '&txtContent=' + encodeURIComponent($('txtContent').value) + '&satisfy=' + GetRValue('my_test_radio'), {
			//BitAutoFeedBack.xssRequest('http://www.bitauto.com/FeedBack/API/CommentNo.ashx?ArticleId=' + articleId + '&txtContent=' + encodeURIComponent($('txtContent').value) + '&satisfy=' + GetRValue('my_test_radio'), {

			completeListener: function () {
				if (this.responseJSON) {
					var data = this.responseJSON;
					if (data.status == "success") {
						//关闭窗口

						$('div_dui').style.display = "";


						setTimeout(function () {
							$('div_dui').style.display = "none";
							var lay = document.getElementById("ued_lay");
							lay.style.display = 'none';
							$('txtContent').value = "";
							//                            $('txtContent').focus();
							//                            $('txtContent').focus();
							document.getElementById("warntitle").innerHTML = "0/200";
						}, 2000);

						blurOn();
					}
					else {

						document.getElementById("warntitle").innerHTML = "0/200";

						$('div_cuo').style.display = "";
						$('div_cuo').innerHTML = "<i class='cuo'></i>" + data.message;

						// $('txtContent').value = "                                                   _";

						setTimeout(function () {
							$('div_cuo').style.display = "none";
							var lay = document.getElementById("ued_lay");
							lay.style.display = 'none';
							$('txtContent').value = "";
							//                            $('txtContent').focus();
							//                            $('txtContent').focus();
						}, 2000);

						blurOn();
					}
					//alert(data.message);
				}
			},
			errorListener: function () {
				//ShowErrorText("提交失败！请稍候再试。");

			},
			timeout: 30000
		});
	}










	function GetRValue(sel) {
		var radioSelect;
		var test_radio = document.getElementsByName(sel);
		for (i = 0; i < test_radio.length; i++) {
			if (test_radio[i].checked) {
				radioSelect = test_radio[i].value;
			}
		}
		return radioSelect;
	}



	return { // Public members.
		show: function (id, bitAuto, fcdivname) {

			//判断有效期
			//bitAuto.xssRequest('API/CheckPublishTime.ashx?ArticleId=' + id, {
			bitAuto.xssRequest('http://www.bitauto.com/FeedBack/API/CheckPublishTime.ashx?ArticleId=' + id, {

				completeListener: function () {
					if (this.responseJSON) {
						var data = this.responseJSON;
						alert(data.status);
						if (data.status == "success") {

							$ = bitAuto.getElement;
							BitAutoFeedBack = bitAuto;
							articleId = id;
							//填充浮层
							CreateFC(fcdivname)
							eventBind();
						}
					}
				},
				errorListener: function () {
					//ShowErrorText("提交失败！请稍候再试。");
				},
				timeout: 30000
			});


		}

	};


})();


function CreateFC(fcdivname) {



    //var template = "html{padding:0; margin:0} body{height:100%; overflow-x:hidden;padding:0; margin:0; position:relative} .layer_ued { width:300px; border:1px solid #999; position:fixed; right:-303px; bottom:0; z-index:2011; _position:absolute; _bottom:auto; _top:expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop+(documentElement.clientHeight - this.clientHeight) - 1:document.body.scrollTop+(document.body.clientHeight - this.clientHeight) - 1); font-size:12px;font-family: SimSun Arial, Helvetica, sans-serif; line-height:150%}.layer_ued a:link,.layer_ued a:visited { color:#1e4674; text-decoration:none }.layer_ued a:hover { color:#1e4674; text-decoration:underline }.layer_ued h2 { background:url(http://image.bitauto.com/uimg/ued/images/layer350x205.png) repeat-x 0 -3px; height:32px; line-height:32px; overflow:hidden; font-weight:normal; margin:0 }.layer_ued h2 a:link,.layer_ued h2 a:visited { background:url(http://image.bitauto.com/uimg/ued/images/layer350x205.png) no-repeat -107px -37px;  display:block; height:33px; width:220px;  font-weight:bold; font-size:12px; margin-left:10px; outline:none; color:#333}.layer_ued h2 a:hover{ text-decoration:none; color:#333}.layer_ued .layerbar { background:#fcfcfc; padding:10px 10px 0px;list-style:none; margin:0; position:relative }.layer_ued .layerbar li { overflow:hidden;zoom:1;padding-bottom:10px}.layer_ued .layerbar li label { color:#333; width:60px; display:inline-block; text-align:right }.layer_ued .layerbar li textarea { vertical-align:top; width:273px; height:100px; overflow:auto; margin-bottom:10px }.layer_ued .layerbar li p{ color:#999; line-height:25px; float:left; margin:-10px 0 0 0}.layer_ued .layerbar li input { height:20px; width:55px; vertical-align:middle }.layer_ued .layerbar li span { display:inline-block; width:82px; height:22px; vertical-align:middle; margin:-1px 3px 0 0px; margin:-1px 3px 0 0px\9; *margin:-2px 3px 0 0px;_margin:0px 8px 0 0px }.layer_ued .layerbar li a.btn { background: url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat 0 -190px; border:0; width:54px; height:22px; line-height:22px; text-align:center; color:#fff; margin-left:30px;*margin-left:25px;  cursor:pointer; float:right }.layer_ued .layerbar li a.btn:visited{ color:#fff}.layer_ued .layerbar li a.btn:hover {  text-decoration:none; color:#fff }.layer_ued .layerbar li a.chages, .layerbar li a.chages:visited { color:#164A84 }.layer_ued .layerbar li a.chages:hover { color:#c00 }.layer_ued .reture { position:absolute; top:10px; right:10px; height:11px; line-height:35px; color:#ccc }.layer_ued .reture a.ued_close,.layer_ued .reture a.ued_close:visited { height:11px; width:11px; background:url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat -60px -82px; overflow:hidden; outline:none; text-indent:-999px; display:block }.layer_ued .layerbar li label.text_lab{ width:270px;position:absolute;top:16px;left:18px; color:#999; text-align:left;padding:0; line-height:22px}.layer_ued .layerbar li label.text_lab span{ width:122px;padding-left:154px}.layer_ued .layerbar li label.text_hover{ text-indent:-999px; height:3px; overflow:hidden}.tj_box{ border:1px solid #CCD6DB; width:27px; position:fixed; bottom:99px; right:0;_right:16px;_position:absolute; _bottom:99px; _top:expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop+(documentElement.clientHeight - this.clientHeight) - 1:document.body.scrollTop+(document.body.clientHeight - this.clientHeight) - 1);}.tj_box a,.tj_box a:visited{ background:#EFF4F6; border:1px solid #fff; display:block; width:11px; height:70px; color:#56616D; text-decoration:none; text-align:center;padding:10px 7px; font-size:12px}.tj_box a:hover{ color:#c00}.suz{ background:#FFFFE6; border:1px solid #FFD6A5 ; width:195px; height:14px; line-height:14px;padding:10px; overflow:hidden; position:absolute; left:45px; top:65px; color:#FF6600}.suz i.cuo{ background:url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat 0 -59px; width:15px; height:15px; overflow:hidden; display:inline-block; vertical-align:middle; margin-right:10px}.suz i.dui{ background:url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat 0 -256px; width:20px; height:17px; overflow:hidden; display:inline-block; vertical-align:middle; margin-right:10px}";
    var template = "html { padding:0; margin:0 }body { height:100%; overflow-x:hidden; padding:0; margin:0; position:relative }.layer_ued { width:300px; border:1px solid #999; position:fixed; right:0px; bottom:0; z-index:2011; _position:absolute; _bottom:auto; _top:expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop+(documentElement.clientHeight - this.clientHeight) - 1:document.body.scrollTop+(document.body.clientHeight - this.clientHeight) - 1);font-size:12px; font-family: SimSun Arial, Helvetica, sans-serif; line-height:150% }.layer_ued a:link, .layer_ued a:visited { color:#1e4674; text-decoration:none }.layer_ued a:hover { color:#1e4674; text-decoration:underline }.layer_ued h2 { background:url(http://image.bitauto.com/uimg/ued/images/layer350x205.png) repeat-x 0 -3px; height:32px; line-height:32px; overflow:hidden; font-weight:normal; margin:0 }.layer_ued h2 a:link, .layer_ued h2 a:visited { background:url(http://image.bitauto.com/uimg/ued/images/layer350x205.png) no-repeat -107px -37px; display:block; height:33px; width:220px; font-weight:bold; font-size:12px; margin-left:10px; outline:none; color:#333 }.layer_ued h2 a:hover { text-decoration:none; color:#333 }.layer_ued .layerbar { background:#fcfcfc; padding:10px 10px 0px; list-style:none; margin:0; }.layer_ued .layerbar li { overflow:hidden; zoom:1; padding-bottom:10px; position:relative}.layer_ued .layerbar li label { color:#333; width:60px; display:inline-block; text-align:right }.layer_ued .layerbar li textarea { vertical-align:top; width:273px; height:100px; overflow:auto; margin-bottom:10px }.layer_ued .layerbar li p { color:#999; line-height:25px; float:left; margin:-10px 0 0 0 }.layer_ued .layerbar li input { height:20px; width:55px; vertical-align:middle }.layer_ued .layerbar li span { display:inline-block; width:82px; height:22px; vertical-align:middle; margin:-1px 3px 0 0px; margin:-1px 3px 0 0px\9; *margin:-2px 3px 0 0px;_margin:0px 8px 0 0px }.layer_ued .layerbar li a.btn { background: url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat 0 -190px; border:0; width:54px; height:22px; line-height:22px; text-align:center; color:#fff; margin-left:30px; *margin-left:25px;cursor:pointer; float:right }.layer_ued .layerbar li a.btn:visited { color:#fff }.layer_ued .layerbar li a.btn:hover { text-decoration:none; color:#fff }.layer_ued .layerbar li a.chages, .layerbar li a.chages:visited { color:#164A84 }.layer_ued .layerbar li a.chages:hover { color:#c00 }.layer_ued .reture { position:absolute; top:10px; right:10px; height:11px; line-height:35px; color:#ccc }.layer_ued .reture a.ued_close, .layer_ued .reture a.ued_close:visited { height:11px; width:11px; background:url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat -60px -82px; overflow:hidden; outline:none; text-indent:-999px; display:block }.layer_ued .layerbar li label.text_lab { width:250px; position:absolute; top:14px; left:14px; color:#999; text-align:left; padding:0; line-height:22px }.layer_ued .layerbar li label.text_lab span { width:122px; padding-left:154px }.layer_ued .layerbar li label.text_hover { text-indent:-999px; height:3px; overflow:hidden }.layer_ued .layerbar li.my_test input{ width:auto; vertical-align:middle}.layer_ued .layerbar li.my_test label{ width:auto; vertical-align:middle}.layer_ued .layerbar li.my_test span{ width:auto; margin-right:20px}.layer_ued .layerbar li.my_test span.end{ margin-right:0}.tj_box { border:1px solid #CCD6DB; width:27px; position:fixed; bottom:99px; right:0; _right:16px; _position:absolute; _bottom:99px; _top:expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop+(documentElement.clientHeight - this.clientHeight) - 1:document.body.scrollTop+(document.body.clientHeight - this.clientHeight) - 1);}.tj_box a, .tj_box a:visited { background:#EFF4F6; border:1px solid #fff; display:block; width:11px; height:70px; color:#56616D; text-decoration:none; text-align:center; padding:10px 7px; font-size:12px }.tj_box a:hover { color:#c00 }.suz { background:#FFFFE6; border:1px solid #FFD6A5; width:195px; height:14px; line-height:14px; padding:10px; overflow:hidden; position:absolute; left:45px; top:110px; color:#FF6600 }.suz i.cuo { background:url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat 0 -59px; width:15px; height:15px; overflow:hidden; display:inline-block; vertical-align:middle; margin-right:10px }.suz i.dui { background:url(http://image.bitauto.com/uimg/index120401/images2/form_icons.png) no-repeat 0 -256px; width:20px; height:17px; overflow:hidden; display:inline-block; vertical-align:middle; margin-right:10px }";
    var style1 = document.createElement('style');
    if (navigator.userAgent.indexOf("MSIE") > -1 && !(navigator.userAgent.indexOf("MSIE 9.0")> -1)) {


        //alert(navigator.userAgent);

        //添加样式
        style1.setAttribute("type", "text/css");
        style1.styleSheet.cssText = template;



        //设置侧框格式

        var divcekuang = document.createElement('A'); divcekuang.setAttribute('title', '意见反馈'); divcekuang.setAttribute('id', 'ued_btn'); divcekuang.setAttribute('target', '_self'); divcekuang.setAttribute('href', 'javascript:void(0);');
        var txt42 = document.createTextNode('意见反馈'); divcekuang.appendChild(txt42);

        //设置主框格式
        var divzhukuang = document.createElement("<div id='ued_lay' class='layer_ued' style='display:none'></div>");

        var h21 = document.createElement("<H2></H2>"); divzhukuang.appendChild(h21);
        var a2 = document.createElement("<a  id=\"layer_ued_show1\" target=\"_self\" href=\"javascript:void(0);\"></a>"); h21.appendChild(a2);
        var txt3 = document.createTextNode('您对当前页面满意吗？'); a2.appendChild(txt3);



        var ul1 = document.createElement("<ul class=\"layerbar clearfix\"></ul>"); divzhukuang.appendChild(ul1);


        var li2 = document.createElement("<li class=\"my_test\"></li>"); ul1.appendChild(li2);

        var span1 = document.createElement("<span></span>"); li2.appendChild(span1);

        var input1 = document.createElement("<input type='radio' name='my_test_radio' id='manyi' value='1'  checked='checked' />"); span1.appendChild(input1);


        var label31 = document.createElement('LABEL'); label31.setAttribute('for', 'manyi'); span1.appendChild(label31);
        var txt11 = document.createTextNode(' 满意'); label31.appendChild(txt11);


        var span2 = document.createElement("<span></span>"); li2.appendChild(span2);

        var input2 = document.createElement("<input type='radio' name='my_test_radio' id='bumanyi' value='2' />"); span2.appendChild(input2);
        var label32 = document.createElement('LABEL'); label32.setAttribute('for', 'bumanyi'); span2.appendChild(label32);
        var txt12 = document.createTextNode(' 不满意'); label32.appendChild(txt12);

        var span3 = document.createElement('SPAN'); span3.className = 'end'; li2.appendChild(span3);

        var input3 = document.createElement("<input type='radio' name='my_test_radio' id='wusuowei' value='3' />"); span3.appendChild(input3);
        var label33 = document.createElement('LABEL'); label33.setAttribute('for', 'wusuowei'); span3.appendChild(label33);
        var txt13 = document.createTextNode(' 无所谓'); label33.appendChild(txt13);


        var li1 = document.createElement("<li></li>"); ul1.appendChild(li1);

        var label1 = document.createElement('LABEL'); label1.setAttribute('id', 'lblDefaultValue'); label1.setAttribute('for', 'txtContent'); label1.className = 'text_lab'; label1.onclick = function () { Txt4focusOn(); }; li1.appendChild(label1);

        var txt4 = document.createTextNode('提意见时别忘留下联系方式噢，您的意见一经采纳，将有精美小礼品送出。'); label1.appendChild(txt4);


        var textarea1 = document.createElement("<textarea id=\"txtContent\" onkeyup=\"keyUpOn();\"  onfocus=\"focusOn()\"  onblur=\"blurOn();\"></textarea>"); li1.appendChild(textarea1);

        var p1 = document.createElement("<p id=\"warntitle\"></p>"); li1.appendChild(p1);
        var txt5 = document.createTextNode('0/200'); p1.appendChild(txt5);


        var a3 = document.createElement("<a class=\"btn\" id=\"btnSubmit\" target=\"_self\" href=\"javascript:void(0);\"></a>"); li1.appendChild(a3);
        var txt6 = document.createTextNode('提交'); a3.appendChild(txt6);

        //var div4 = document.createElement('DIV'); div4.style.display = 'none'; div4.className = 'suz'; div4.setAttribute('id', 'div_cuo'); divzhukuang.appendChild(div4);
        var div4 = document.createElement("<div style=\"display:none;\" class=\"suz\" id=\"div_cuo\" ></div>"); divzhukuang.appendChild(div4);

        var i1 = document.createElement("<i class=\"cuo\"></i>"); div4.appendChild(i1);
        var txt8 = document.createTextNode('请留下您的意见'); div4.appendChild(txt8);


        var div5 = document.createElement("<div style=\"display:none\" class=\"suz\" id=\"div_dui\"></div>"); divzhukuang.appendChild(div5);

        var i2 = document.createElement("<i class=\"dui\"></i>"); div5.appendChild(i2);
        var txt9 = document.createTextNode('提交成功！感谢您的反馈。'); div5.appendChild(txt9);




        var div10 = document.createElement("<div class=\"reture\" id=\"ued_lay_close\" ></div>"); divzhukuang.appendChild(div10);

        var a4 = document.createElement("<a class=\"ued_close\" target=\"_self\" href=\"javascript:void(0);\"></a>"); div10.appendChild(a4);
        var txt10 = document.createTextNode('关闭'); a4.appendChild(txt10);


    }
    else {

        //添加样式
        if (navigator.userAgent.indexOf("MSIE 9.0") > -1) {
            style1.setAttribute("type", "text/css");
            style1.styleSheet.cssText = template;
        }
        else {
            var txt1 = document.createTextNode(template);
            style1.appendChild(txt1);
        }
       


        var divcekuang = document.createElement('A'); divcekuang.setAttribute('title', '意见反馈'); divcekuang.setAttribute('id', 'ued_btn'); divcekuang.setAttribute('target', '_self'); divcekuang.setAttribute('href', 'javascript:void(0);');
        var txt42 = document.createTextNode('意见反馈'); divcekuang.appendChild(txt42);

        //设置主框格式
        var divzhukuang = document.createElement('DIV'); divzhukuang.className = 'layer_ued'; divzhukuang.style.display = 'none'; divzhukuang.setAttribute('id', 'ued_lay');

        var h21 = document.createElement('H2'); divzhukuang.appendChild(h21);

        var a2 = document.createElement('A'); a2.setAttribute('target', '_self'); a2.setAttribute('href', 'javascript:void(0);'); a2.setAttribute('id', 'layer_ued_show1'); h21.appendChild(a2);

        var txt3 = document.createTextNode('您对当前页面满意吗？'); a2.appendChild(txt3);



        var ul1 = document.createElement('UL'); ul1.className = 'layerbar clearfix'; divzhukuang.appendChild(ul1);


        var li2 = document.createElement('LI'); li2.className = 'my_test'; ul1.appendChild(li2);

        var span1 = document.createElement('SPAN'); li2.appendChild(span1);
        var input1 = document.createElement('INPUT'); input1.setAttribute('type', 'radio'); input1.setAttribute('name', 'my_test_radio'); input1.setAttribute('id', 'manyi'); input1.setAttribute('value', '1'); input1.setAttribute('checked', 'checked'); span1.appendChild(input1);
        var label31 = document.createElement('LABEL'); label31.setAttribute('for', 'manyi'); span1.appendChild(label31);
        var txt11 = document.createTextNode(' 满意'); label31.appendChild(txt11);

        var span2 = document.createElement('SPAN'); li2.appendChild(span2);
        var input2 = document.createElement('INPUT'); input2.setAttribute('type', 'radio'); input2.setAttribute('name', 'my_test_radio'); input2.setAttribute('id', 'bumanyi'); input2.setAttribute('value', '2'); span2.appendChild(input2);
        var label32 = document.createElement('LABEL'); label32.setAttribute('for', 'bumanyi'); span2.appendChild(label32);
        var txt12 = document.createTextNode(' 不满意'); label32.appendChild(txt12);

        var span3 = document.createElement('SPAN'); span3.className = 'end'; li2.appendChild(span3);
        var input3 = document.createElement('INPUT'); input3.setAttribute('type', 'radio'); input3.setAttribute('name', 'my_test_radio'); input3.setAttribute('id', 'wusuowei'); input3.setAttribute('value', '3'); span3.appendChild(input3);
        var label33 = document.createElement('LABEL'); label33.setAttribute('for', 'wusuowei'); span3.appendChild(label33);
        var txt13 = document.createTextNode(' 无所谓'); label33.appendChild(txt13);

        var li1 = document.createElement('LI'); ul1.appendChild(li1);

        var label1 = document.createElement('LABEL'); label1.setAttribute('id', 'lblDefaultValue'); label1.setAttribute('for', 'txtContent'); label1.className = 'text_lab'; label1.onclick = function () { Txt4focusOn(); }; li1.appendChild(label1);

        var txt4 = document.createTextNode('提意见时别忘留下联系方式噢，您的意见一经采纳，将有精美小礼品送出。'); label1.appendChild(txt4);

        var textarea1 = document.createElement('TEXTAREA'); textarea1.onkeyup = function () { keyUpOn() }; textarea1.onfocus = function () { focusOn() }; textarea1.onblur = function () { blurOn(); }; textarea1.setAttribute('id', 'txtContent'); li1.appendChild(textarea1);

        var p1 = document.createElement('P'); p1.setAttribute('id', 'warntitle'); li1.appendChild(p1);

        var txt5 = document.createTextNode('0/200'); p1.appendChild(txt5);

        var a3 = document.createElement('A'); a3.setAttribute('target', '_self'); a3.setAttribute('id', 'btnSubmit'); a3.className = 'btn'; a3.setAttribute('href', 'javascript:void(0);'); li1.appendChild(a3);

        var txt6 = document.createTextNode('提交'); a3.appendChild(txt6);

        var div4 = document.createElement('DIV'); div4.style.display = 'none'; div4.className = 'suz'; div4.setAttribute('id', 'div_cuo'); divzhukuang.appendChild(div4);
        var i1 = document.createElement('i'); i1.className = 'cuo'; div4.appendChild(i1);
        var txt8 = document.createTextNode('请留下您的意见'); div4.appendChild(txt8);

        var div5 = document.createElement('DIV'); div5.style.display = 'none'; div5.className = 'suz'; div5.setAttribute('id', 'div_dui'); divzhukuang.appendChild(div5);
        var i2 = document.createElement('i'); i2.className = 'dui'; div5.appendChild(i2);
        var txt9 = document.createTextNode('提交成功！感谢您的反馈。'); div5.appendChild(txt9);



        var div10 = document.createElement('DIV'); div10.className = 'reture'; div10.setAttribute('id', 'ued_lay_close'); divzhukuang.appendChild(div10);

        var a4 = document.createElement('A'); a4.className = 'ued_close'; a4.setAttribute('target', '_self'); a4.setAttribute('href', 'javascript:void(0);'); div10.appendChild(a4);

        var txt10 = document.createTextNode('关闭'); a4.appendChild(txt10);



    }




    //设置侧框格式





    //添加样式
    document.getElementsByTagName('head')[0].appendChild(style1);

    //判断填充侧框DIV
    if (fcdivname != "") {
        document.getElementById(fcdivname).appendChild(divcekuang);
    }
    else {
        document.body.appendChild(divcekuang);
    }

    //添加主框
    document.body.appendChild(divzhukuang);

}



function blurOn() {
    var curLbl = document.getElementById('lblDefaultValue');
    var curTxt = document.getElementById("txtContent");
    if (curLbl) {
        if (curTxt && curTxt.value.trim().length <= 0)
            curLbl.className = "text_lab";
    }
}
function focusOn() {

    var curLbl = document.getElementById('lblDefaultValue');
    if (curLbl) {
        curLbl.className = "text_lab text_hover";
    }
}


function keyUpOn() {

    var b = document.getElementById("txtContent").value;

    document.getElementById("warntitle").innerHTML = b.length.toString() + "/200";

}


function Txt4focusOn() {
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        var curLbl = document.getElementById('lblDefaultValue');
        if (curLbl) {
            curLbl.className = "text_lab text_hover";
        }
        document.getElementById('txtContent').focus();
    }

}



function showSummaryTree() {
    var tree_btn = document.getElementById('ued_btn');
    var car_summary_tree = document.getElementById('ued_lay');
    var tree_btn_return = document.getElementById('ued_lay_close');

    var pageHtml = document.getElementsByTagName('html');
    var pageBody = document.getElementsByTagName('body');

    var isIE = !!window.ActiveXObject;
    var isIE6 = isIE && !window.XMLHttpRequest;
    var isIE8 = isIE && !!document.documentMode;
    var isIE7 = isIE && !isIE6 && !isIE8;

}
if (typeof addLoadEvent == "function") {
    addLoadEvent(showSummaryTree);
}
else {
    window.onload = showSummaryTree;
}



