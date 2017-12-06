$(function () {
    var uploadApi = "http://mps.yiche.com/pic/upload?v=1" //图片上传接口
    var checkApiLocal = "http://mps219.yiche.com/url/?callback=funtest&transformurl="; // 本地图片识别接口
    var checkApi = "http://car.m.yiche.com/recognition/api?img=" //图片识别接口
    var selectApi = "http://api.car.bitauto.com/carinfo/getserialinfo.ashx?dept=getserialbaseinfobyidjson&csid=" //车型数据接口

    // add by hepw 20171206 为支持https
    if ('https:' == document.location.protocol)
    {
        var uploadApi = "https://mps.yiche.com/pic/upload?v=1" //图片上传接口
        var checkApiLocal = "https://mps219.yiche.com/url/?callback=funtest&transformurl="; // 本地图片识别接口
        var checkApi = "https://car.m.yiche.com/recognition/api?img=" //图片识别接口
        var selectApi = "https://ngcar.yiche.com/carapi/carinfo/getserialinfo.ashx?dept=getserialbaseinfobyidjson&csid=" //车型数据接口
    }

    var curImgShoutCut = ""; //当前识别的图片的缩略图
    var orientation; //当前图片方向
    var recognitionCount = 0; //当前识别出的车型
    var recognitionRightCount = 0; //当前识别出的而且能在车型库取到数据的车型

    // 点击图片上传
    //input type = file中发生change事件时执行的函数
    $("#file_upload").change(function (ev) { fileUploadChange(ev); });

    function fileUploadChange(ev) {
        //如果不是点击的取消
        if (($('#file_upload').get(0).files[0])) {
            //$("#content strong").text("正在读取图片"); 
            var input = ev.target;
            var file = input.files[0];
            //console.log(file);
            setTimeout('$("#content .sc-pic-box").is(":hidden") ? $("#content .ico-sc-up").hide() : $("#content .ico-sc-up").show();', 1000);
            showErrorMsg("default");
            $("#file_upload").hide();
            //获取图片的方向
            getOrientation(input.files[0], function (data) {
                orientation = data;
                //console.log("orientation" + orientation);
            });

            //对图片做压缩并上传和结果显示
            readFile(file);

        } //end点击的不是取消
        else {
            //此处点击了取消，没有反应
        }
    }

    //获取图片的方向
    function getOrientation(file, callback) {
        var reader = new FileReader();
        reader.onload = function (e) {
            try {
                var view = new DataView(e.target.result);
                if (view.getUint16(0, false) != 0xFFD8) return callback(-2);
                var length = view.byteLength,
                    offset = 2;
                while (offset < length) {
                    var marker = view.getUint16(offset, false);
                    offset += 2;
                    if (marker == 0xFFE1) {
                        if (view.getUint32(offset += 2, false) != 0x45786966) return callback(-1);
                        var little = view.getUint16(offset += 6, false) == 0x4949;
                        offset += view.getUint32(offset + 4, little);
                        var tags = view.getUint16(offset, little);
                        offset += 2;
                        for (var i = 0; i < tags; i++)
                            if (view.getUint16(offset + (i * 12), little) == 0x0112)
                                return callback(view.getUint16(offset + (i * 12) + 8, little));
                    } else if ((marker & 0xFF00) != 0xFF00) break;
                    else offset += view.getUint16(offset, false);
                }
                return callback(-1);

            } catch (e) {
                //console.log(e);
                return callback(-1);

            }
        };
        reader.readAsArrayBuffer(file.slice(0, 64 * 1024));
    }

    //对图片做压缩旋转和上传
    function readFile(file) {
        var formdata = new FormData();
        if (!/image\/\w+/.test(file.type)) {
            showErrorMsg("timeout");
            InitStatus();
            return false;
        }

        var reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onload = function (e) {
            getImgData(e.target.result, orientation, file, function (canvas) {

                var f_size = file.size
                console.log("f_size:" + f_size);
                /*将转换后的图片下载到本地*/
                /*
                var imgUri = data; // 获取生成的图片的url
                var saveLink = document.createElement('a');　　　　
                saveLink.href = imgUri;　　　　
                saveLink.download = "download.png";
                saveLink.click();
                */
                var data = canvas.toDataURL("image/jpeg", .8)



                if (navigator.userAgent.match(/iphone/i)) {
                    f_size -= 10 * 1024;
                }
                var minImageSize = 30 * 1024;
                var maxImageSize = 300 * 1024;

                /*
                if (parseFloat(f_size) < parseFloat(minImageSize)) {
                    showErrorMsg("timeout");
                    //alert("您传入的图片太小了,请上传30k以上的图片");
                    InitStatus();
                    return false;
                }
                */

                blob = dataURLtoBlob(data);
                console.log("blob8:" + blob.size);

                if (blob.size >= 1024 * 300) {
                    data = canvas.toDataURL("image/jpeg", .75)
                    blob = dataURLtoBlob(data);
                    console.log("blob75:" + blob.size);
                }
                console.log("图片压缩后：" + (blob.size / 1024) + "K");
                if (blob.size < minImageSize) {
                    ////alert("blob0:" + blob.size);
                    //data = canvas.toDataURL("image/jpeg", 1);
                    //blob = dataURLtoBlob(data);
                    ////alert("您传入的图片太小了,请上传30k以上的图片.");
                    //showErrorMsg("timeout");
                    //InitStatus();
                    //return false;
                } else if (blob.size > maxImageSize) {
                    //alert("blob1:" + blob.size);
                    showErrorMsg("timeout");
                    //alert("您传入的图片太大了");
                    InitStatus();
                    return false;
                }
                //console.log(blob.size);
                //console.log(file.size);
                //console.log(blob.size / file.size);

                uploadCar({
                    file: data
                }); //调用上传接口
            });
        }
    }

    /**
     *  修正 IOS 拍照上传图片裁剪时角度偏移（旋转）问题
     *
     *  img 图片的 base64
     *  dir exif 获取的方向信息
     *  next 回调方法，返回校正方向后的 base64
     * 
     **/
    function getImgData(img, dir, file, next) {

        var image = new Image();
        image.onload = function () {
            var degree = 0,
                maxHW = 800,
                drawWidth,
                drawHeight,
                width,
                height;

            if (file.size <= (1024 * 1024 * 2)) {
                maxHW = 1200;
            } else if (file.size <= (1024 * 1024 * 3)) {
                maxHW = 1000;
            }

            drawWidth = this.naturalWidth;
            drawHeight = this.naturalHeight;

            // 改变图片大小，如果图片的最大长度大于1200要进行等比缩放
            if (drawHeight < drawWidth) {
                if (drawWidth > maxHW) {
                    drawHeight = (drawHeight * maxHW / drawWidth);
                    drawWidth = maxHW;
                }
            } else {
                if (drawHeight > maxHW) {
                    drawWidth = (drawWidth * maxHW / drawHeight);
                    drawHeight = maxHW;
                }
            }
            /*
            if (drawWidth > maxHW) {
                drawHeight = (drawHeight * maxHW / drawWidth);
                drawWidth = maxHW;
            } else if (drawHeight > maxHW) {
                drawWidth = (drawWidth * maxHW / drawHeight);
                drawHeight = maxHW;
            }
            */

            // 创建画布
            var canvas = document.createElement("canvas");
            canvas.width = width = drawWidth;
            canvas.height = height = drawHeight;
            var context = canvas.getContext("2d");

            // 判断图片方向，重置 canvas 大小，确定旋转角度，iphone 默认的是 home 键在右方的横屏拍摄方式
            //console.log("dir" + dir);
            switch (dir) {
                // iphone 横屏拍摄，此时 home 键在左侧
                case 3:
                    // alert(3);
                    degree = 180;
                    drawWidth = -width;
                    drawHeight = -height;
                    break;
                // iphone 竖屏拍摄，此时 home 键在下方(正常拿手机的方向)
                case 6:
                    // alert(6);
                    canvas.width = height;
                    canvas.height = width;
                    degree = 90;
                    drawWidth = width;
                    drawHeight = -height;
                    break;
                // iphone 竖屏拍摄，此时 home 键在上方
                case 8:
                    // alert(8);
                    canvas.width = height;
                    canvas.height = width;
                    degree = 270;
                    drawWidth = -width;
                    drawHeight = height;
                    break;
                default:
                    break;
            }

            // 使用 canvas 旋转校正
            context.rotate(degree * Math.PI / 180);
            context.drawImage(this, 0, 0, drawWidth, drawHeight);


            var data = canvas.toDataURL("image/jpeg", 1)

            togglePage("upload");
            $(".sc-pic-box img").attr("src", data);
            $(".sc-pic-box-sao img").attr("src", data);

            //console.log(drawHeight + "X" + drawWidth) 
            next(canvas);
        }

        image.src = img;
    }

    //将base64转化成blob
    function dataURLtoBlob(dataurl) {
        var arr = dataurl.split(','),
            mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]),
            n = bstr.length,
            u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new Blob([u8arr], {
            type: mime
        });
    }

    // 上传图片
    function uploadCar(formdata) {
        //console.log(formdata);
        $.ajax({
            url: uploadApi,
            type: "POST",
            cache: false,
            data: formdata,
            crossDomain: true,
            dataType: 'json',
            timeout: 3000,
            beforeSend: function () {
            },
            success: function (data) {
                var resultdata = new FormData();
                console.log(data);
                togglePage("recg");
                curImgShoutCut = data.url.replace("cargroup", "newsimg-180-w0/cargroup");
                if ('https:' == document.location.protocol) {
                    curImgShoutCut = curImgShoutCut.replace("http:", "https:");
                }
                //console.log(curImgShoutCut);
                recognition(data.url);
                //recognitionLocal(data.url);
            },
            error: function (data) {
                //console.log(data); 
            },
            complete: function (XMLHttpRequest, status) {
                //console.log(status);
                //请求完成后最终执行参数
                if (status != 'success') { //超时,status还有success,error等值的情况
                    showErrorMsg("timeout");
                    InitStatus();
                }
            }

        }) //end upload ajax
    }

    // 切换页面动态显示效果
    function togglePage(pidx) {

        $("#content .ico-sc-car").hide();
        $("#face").hide();
        $("#content strong").hide();
        $("#content .sc-pic-box").hide();
        $("#content .clear").hide();
        $("#content").removeClass("shiche-pic-up");

        $("#content .sc-pic-box-sao").hide();
        $("#content .ico-sc-up").hide();
        if (pidx == "default") {
            $("#content .ico-sc-car").show();
            $("#face").show();
            $("#content strong").text("拍摄/上传相册中车图");
            $("#content strong").show();
        } else if (pidx == "upload") {
            $("#content .sc-pic-box").show();
            $("#content .clear").show();
            $("#content").addClass("shiche-pic-up");
        } else if (pidx == "recg") {
            $("#content .sc-pic-box-sao").show();
            $("#content .clear").show();
        }
    }

    // 显示错误信息
    function showErrorMsg(type) {
        if (type == "default") {
            $("#recg .shiche-box h1").text("拍照识车");
            $("#recg .shiche-box p").text("给我一张汽车图，我帮您找出TA是哪辆哦！");
            $("#face").removeClass("ico-sc-camera-cry");
            $("#face").addClass("ico-sc-camera");
        } else if (type == "fail") {
            $("#recg .shiche-box h1").text("识别失败");
            $("#recg .shiche-box p").text("有可能拍的是一辆假车，点击重新拍照");
            $("#face").removeClass("ico-sc-camera");
            $("#face").addClass("ico-sc-camera-cry");
        } else if (type == "timeout") {
            $("#recg .shiche-box h1").text("上传失败");
            $("#recg .shiche-box p").text("网络原因上传失败，点击重新上传");
            $("#face").removeClass("ico-sc-camera");
            $("#face").addClass("ico-sc-camera-cry");
        } else {
            $("#recg .shiche-box h1").text("拍照识车");
            $("#recg .shiche-box p").text("给我一张汽车图，我帮您找出TA是哪辆哦！");
            $("#face").removeClass("ico-sc-camera-cry");
            $("#face").addClass("ico-sc-camera");
        }
    }

    // 调用车辆识别接口返回识别结果 本地测试JSONP方式
    function recognitionLocal(url) {
        var gurl = checkApiLocal + encodeURI("http://ml.yiche.com/recognize/wp/submit?img=" + url);
        $.ajax({
            url: gurl,
            type: "GET",
            cache: false,
            crossDomain: true,
            dataType: 'jsonp',
            jsonp: "callback",
            jsonpCallback: "funtest",
            contentType: false,
            processData: false,
            timeout: 3000,
            beforeSend: function () {
            },
            success: function (data) {
                //console.log(data);
                if (data.msg === "OK") {
                    ProcessRecogNew(data.result.model.data);
                } else {
                    showErrorMsg("fail");
                    InitStatus();
                    //console.log("recognition识别失败...");
                }
            },
            error: function (error) {
                //console.log("recognition识别失败error...");
            },
            complete: function (XMLHttpRequest, status) {
                //请求完成后最终执行参数
                if (status != 'success') { //超时timeout,status还有success,error等值的情况
                    showErrorMsg("fail");
                    InitStatus();
                }
            }
        }) //end check ajax
    }


    // 调用车辆识别接口返回识别结果 线上
    function recognition(url) {
        var gurl = checkApi + url;
        $.ajax({
            url: gurl,
            type: "GET",
            cache: false,
            timeout: 3000,
            beforeSend: function () {
            },
            success: function (data) {
                console.log(data);
                if (data.msg === "OK") {
                    //ProcessRecog(data.result.model.data);
                    ProcessRecogNew(data.result.model.data);
                } else {
                    showErrorMsg("fail");
                    InitStatus();
                    //console.log("recognition识别失败...");
                }
            },
            error: function (error) {
                showErrorMsg("fail");
                //console.log("recognition识别失败error...");
            },
            complete: function (XMLHttpRequest, status) {
                //请求完成后最终执行参数
                if (status != 'success') { //超时timeout,status还有success,error等值的情况
                    showErrorMsg("fail");
                    InitStatus();
                }
            }
        }) //end check ajax
    }

    // 为了处理返回数据中存在已删除的车系所以要先找到返回正确结果的概率最大的数据，然后再逐个匹配其他数据。
    function ProcessRecogNew(data) {
        if (data.length == 1) {
            if (data[0]["carModelId"] != undefined) {
                showErrorMsg("fail");
                InitStatus();
            }
        }
        if (data != null && data.length > 0) {
            RequestFirstCarModelData(data, 0);
        } else {
            showErrorMsg("fail");
            InitStatus();
        }
    }


    // 异步请求车型数据 idx参数是为区分当前是第几辆车，因为第一辆车要特殊处理
    function RequestFirstCarModelData(model, idx) {
        if (isEmptyObject(model[idx])) //当前为空直接跳到下一个
        {
            idx++;
            RequestFirstCarModelData(model, idx);
            return;
        }
        var prob = parseInt(model[idx].prob * 100)
        if (prob == 0) {
            showErrorMsg("fail");
            InitStatus();
            return;
        }
        var gurl = selectApi + model[idx]["carModelId"];
        idx++;
        $.ajax({
            url: gurl,
            type: "GET",
            cache: false,
            crossDomain: true,
            dataType: 'jsonp',
            timeout: 3000,
            beforeSend: function () { },
            success: function (car) {
                //console.log("------------0------------")
                if (isEmptyObject(car)) {
                    //console.log("返回数据出错...");
                    if (idx <= model.length) {
                        RequestFirstCarModelData(model, idx);
                        return;
                    } else {
                        showErrorMsg("fail");
                        InitStatus();
                        return;
                    }
                }
                console.log(car);
                //console.log(model);
                showMainPic(car, model[idx - 1]);
                ProcessCarModelDataList(model, idx);
            },
            error: function (error) {
                //console.log("请求失败...");
                showErrorMsg("fail");
                InitStatus();
            },
            complete: function (XMLHttpRequest, status) {
                //请求完成后最终执行参数
                if (status != 'success') { //超时,status还有success,error等值的情况
                    showErrorMsg("timeout");
                    InitStatus();
                }
            }
        })
    }


    // 处理识别结果
    function ProcessRecog(data) {
        if (data.length == 1) {
            if (data[0]["carModelId"] != undefined) {
                //alert("主人，换个姿势再来一张试试！");
                showErrorMsg("fail");
                InitStatus();
            }
        }
        if (data != null && data.length > 0) {
            // 请求匹配度最高的车型
            RequestFirstCarModelData(data[0]);

            // 请求其他数据
            ProcessCarModelDataList(data, 1);
        }
    }

    // 逐个处理请求匹配度降序的数据
    function ProcessCarModelDataList(data, idx) {
        if (idx >= data.length) {
            CheckResultNumber();
            return;
        }
        if (data[idx]["carModelId"] == undefined) {
            return;
        }
        var prob = parseInt(data[idx].prob * 100)
        if (prob == 0) {
            CheckResultNumber();
            return;
        }
        var model = data[idx];
        idx++;
        var gurl = selectApi + model["carModelId"];
        $.ajax({
            url: gurl,
            type: "GET",
            cache: false,
            crossDomain: true,
            dataType: 'jsonp',
            timeout: 3000,
            beforeSend: function () { },
            success: function (car) {
                //console.log("------------" + idx + "------------")
                if (isEmptyObject(car)) {
                    return;
                }
                console.log(car);
                //console.log(model);
                showListPic(car, model);
            },
            error: function (error) {
                //console.log("请求失败...");
                showErrorMsg("fail");
                InitStatus();
            },
            complete: function (XMLHttpRequest, status) {
                //请求完成后最终执行参数
                if (status != 'success') { //超时,status还有success,error等值的情况 
                    showErrorMsg("timeout");
                    InitStatus();
                    return;
                }
                ProcessCarModelDataList(data, idx);
            }
        })

    }

    // 显示首张图片
    function showMainPic(data, model) {
        // 右上角小图
        $("#curImgShoutCut").attr("src", curImgShoutCut);
        $("#mainpic a").attr("href", "http://car.m.yiche.com/" + data.AllSpell);
        if ('https:' == document.location.protocol) {
            $("#mainpic a img").attr("src", data.Image.replace("http:", "https:").replace("_6.", "_8."));
        } else {
            $("#mainpic a img").attr("src", data.Image.replace("_6.", "_8."));
        }
        $("#mainpic .sc-car-con .sc-car-num strong").text(parseInt(model.prob * 100) + '%');
        $("#mainpic .sc-car-con a dl dt").text(eval("'" + data.ShowName + "'"));
        $("#mainpic .sc-car-con a dl dd").text(data.Price);
        $("#recg").hide();
        $("#result").show();
        $("body").addClass("bgW");
    }

    // 显示其他识别结果
    function showListPic(data, model) {
        var li = '';
        li += '<li>';
        li += '     <a target="_blank" href="http://car.m.yiche.com/' + data.AllSpell + '">';
        if ('https:' == document.location.protocol) {
            li += '         <img src="' + data.Image.replace("http:", "https:") + '">';
        } else {
            li += '         <img src="' + data.Image + '">';
        }
        li += '         <dl>';
        li += '             <dt>' + eval("'" + data.ShowName + "'") + '</dt>';
        li += '             <dd>' + data.Price + '</dd>';
        li += '         </dl>';
        li += '         <em>相似度 ' + parseInt(model.prob * 100) + '%</em>';
        li += '     </a>';
        li += ' </li>';

        $("#result .sc-car-list ul").append(li);
    }


    //回到初始化状态事件
    $(".btn-change").click(function () {
        showErrorMsg("default");
        InitStatus();
    });

    //回到初始化状态
    function InitStatus() {
        $("#recg").show();
        $("#othercartab").show();
        $(".sc-pic-box img").attr("src", "http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif");
        $(".sc-pic-box-sao img").attr("src", "http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif");
        $("#curImgShoutCut").attr("src", "http://image.bitautoimg.com/autoalbum/V2.1/images/150-100.gif");
        $("#mainpic a img").attr("src", "");
        $("#result").hide();
        $("body").removeClass("bgW");
        $("#result .sc-car-list ul").html("");
        curImgShoutCut = "";
        togglePage("default");
        recognitionCount = 0; //当前识别出的车型
        recognitionRightCount = 0; //当前识别出的而且能在车型库取到数据的车型 
        $("#file_upload").show();
        $("#file_upload").click();
    }

    // 判断json是否是空对象
    function isEmptyObject(e) {
        var t;
        for (t in e)
            return !1;
        return !0
    }

    function CheckResultNumber() {

        var ll = $("#result .sc-car-list ul li");
        if (ll.length == 0) {
            $("#othercartab").hide();
        } else if (ll.length % 2 == 0) {
        } else if (ll.length == 1) {
            $("#othercartab").hide();
            ll[0].remove();
        } else {
            ll[ll.length - 1].remove();
        }
    }
});