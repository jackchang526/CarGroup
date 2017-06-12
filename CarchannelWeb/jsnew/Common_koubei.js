/*
通用的方式
*/
/*

//获取口碑投票数量后的回调函数
review_init_cfg.get_topics_callback=function(data){ 
}
--data:{'t478343':{CanVote: true,ID: 478343,Negative_Votes: 0,Positive_Votes: 0},'t478342':{CanVote: true,ID: 478342,Negative_Votes: 0,Positive_Votes: 0}}

//投票成功后回调函数
review_init_cfg.vote_callback=function(param, result){
}
--param:投票时提示字段参数{dtype: "topic",id: "479832",optype: "vote",type: "good",username: "摇啊摇1"}；
--result投票成功的回调函数{errormess: "感谢您的参与!!",errorobj: "",num: 2,run: false},
--this:为投票点击的html元素

初始化页面点评数量时可以页面中声名review_init_cfg配置项，注意此配置应该位common.js前面
var review_init_cfg=　{get_topics_api:'http://koubei.bitauto.com/api/gettopicbyids.ashx?idList={口碑ID列表}'};

review_init_cfg.init_topics_vote : boolen 指示一个值是否调用获取投票数量的接口
*/
var review_init_cfg_default = {
    get_topics_api: 'http://beta.koubei.bitauto.com/api/gettopicbyids.ashx?idList=479815,478343,481080,479294',
    get_topics_callback: false,
    vote_callback: false,
    init_topics_vote: true
};
var review_init_cfg = $.extend(review_init_cfg_default, (typeof review_init_cfg != 'undefined' ? review_init_cfg : {}));
var Common = {
    topicurl: "",
    //userid: "0",
    username: "",
    topicObj: null,
    ShowOperationResult: function (eobj, result) {
        var obj = $(eobj);
        var offset = obj.position();
        var left = offset.left;
        var top = offset.top + obj.width();
        //创建错误提示
        this.CreateErrorMessage(result["errormess"], obj, left, top);

        if (typeof result["num"] == 'undefined') return;
        $(eobj).find("s").each(function (i) {
            $(this).html(result["num"]);
        });
        $(eobj).parent().find("cite").each(function (i) {
            var tag = $(this).attr("tag");
            if (tag == "good" || tag == "bad") {
                $(this).unbind("click").find("a").each(function (i) {
                    $(this).replaceWith($(this).text());
                })
            }
        });

    },
    //创建信息提示框
    CreateErrorMessage: function (mess, obj, left, top) {
        var errorObj = $('#AbsouteErrorSpan');
        if (errorObj.length > 0) {
            errorObj.empty();
            errorObj.remove();
            window.clearTimeout(errorTime);
        }

        $('<div id="AbsouteErrorSpan" class="edit2">' + mess + '</div>')
                    .css({ "top": "25px" })
                    .appendTo($(obj));
        //错误时间
        errorTime = window.setTimeout(function () {
            var eObj = $('#AbsouteErrorSpan');
            if (eObj.length > 0) {
                eObj.empty();
                eObj.remove();
            }
        }, 3 * 1000);
    },
    //投票
    Vote: function () {
        if (this.topicObj == null) return;
        var tobj = $(this.topicObj);
        var dtag = tobj.attr("datatag");
        var tag = tobj.attr("tag");
        var did = tobj.attr("dataid");
        var data = { "id": did, "type": tag, "dtype": dtag, "username": this.username, "optype": "vote" };
        //处理投票
        this.OperationVote(data, this.topicObj);
    },
    //处理投票
    OperationVote: function (edata, eobj) {
        var pro = this;
        $.ajax({
            type: 'get',
            async: false,
            cache: false,
            url: pro.topicurl,
            data: edata,
            dataType: 'json',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                Common.ShowNetWorkError();
            },
            success: function (result) {
                //如果返回数据为空
                if (typeof result == 'undefined' || result == null) {
                    Common.ShowNetWorkError();
                    return;
                }
                Common.ShowOperationResult(eobj, result);
                review_init_cfg.vote_callback && review_init_cfg.vote_callback.call(eobj, edata, result);
                return;
            }
        });
    },
    //显示操作结果
    ShowNetWorkError: function () {

    },
    //登陆成功事件
    LoginSuccess: function (str, obj, userid, username) {
        if (typeof isLogin != 'undefined')
            isLogin = str;

        //Common.username = obj["name"];
        Common.userid = userid;
        Common.username = username;
        Common.Vote();
    },
    //得到内容的实际长度
    getContentRealLength: function (value) {
        if (typeof value == 'undefined'
                    || value == ''
                        || $.trim(value) == '') return 0;

        var length = value.length;
        /*for (var i = 0; i < value.length; i++) {
        if (value.charCodeAt(i) > 127) {
        length++;
        }
        }*/
        return length;
    },
    //初始化好评和差评
    initGoodAndBad: function () {
        var pro = this;
        //好评
        $("cite[tag='good']:has(a)").each(function (i) {
            $(this).click(function (e) {
                e.preventDefault();
                pro.topicObj = this;
                if (typeof Bitauto.Login.result == 'undefined' || Bitauto.Login.result["isLogined"] == false) {
                    AjaxLogin.show();
                    return;
                }
                pro.Vote();
            });
        });
        //差评
        $("cite[tag='bad']:has(a)").each(function (i) {
            $(this).click(function (e) {
                e.preventDefault();
                pro.topicObj = this;
                if (typeof Bitauto.Login.result == 'undefined' || Bitauto.Login.result["isLogined"] == false) {
                    AjaxLogin.show();
                    return;
                }
                pro.Vote();
            });
        });
        //评论
        $("cite[tag='rea']").each(function (i) {
            var dtag = $(this).attr("datatag");
            if (dtag != "post") return;
            $(this).click(function () {
                if ($(this).attr("class") == "current") {
                    $("div").remove('.k_comment_form2');
                    $("cite[tag='rea']").attr("class", "");
                    return;
                }
                //移除已经显示的层
                $("div").remove('.k_comment_form2');
                $("cite[tag='rea']").attr("class", "");
                //显示要显示的层
                var tid = $(this).attr("datapid");
                var pid = $(this).attr("dataid");
                $(this).attr("class", "current");
                var control = $('#divp_' + pid);

                PostControl.init({ "eobj": control
                            , "tid": tid
                            , "pid": pid
                            , "num": "2"
                            , "faceurl": faceurl
                            , "ulogin": isLogin
                            , "ccurl": ccurl
                            , "opurl": Common.topicurl
                            , "purl": psurl
                });
            });
        });

        //初始化顶踩数量
        window.review_json_callback = window.review_json_callback || function () { }
        if (review_init_cfg.init_topics_vote === true && review_init_cfg.get_topics_api.length) {
            $.ajax({ url: review_init_cfg.get_topics_api,
                dataType: 'jsonp',
                cache: true,
                jsonpCallback: "review_json_callback",
                success: function (data) {
                    var obj = {};
                    if (data.length) {
                        for (var i = 0; i < data.length; i++) {
                            obj['t' + data[i].ID] = data[i];
                        }
                    }
                    $("cite[tag='good']:has(a),cite[tag='bad']:has(a)").each(function (k, v) {
                        var $this = $(this);
                        var id = $this.attr("dataid");
                        var tag = $this.attr("tag");
                        if (obj['t' + id]) {
                            if (tag == 'good') $this.find('s').text(obj['t' + id].Positive_Votes);
                            if (tag == 'bad') $this.find('s').text(obj['t' + id].Negative_Votes);
                        }
                    });
                    review_init_cfg.get_topics_callback && review_init_cfg.get_topics_callback(obj);
                },
                complete: function (jqXHR, textStatus) {
                }
            });
        }
    },
    //登陆成功
    loginSuccess: function (data) {
        var pro = this;
        var pid = $(this.topicObj).attr("dataid");
        $.ajax({
            type: "GET",
            url: "/Posts/KouBeiOperation.ashx",
            data: { "id": pid, "optype": "login" },
            dataType: "json",
            async: false,
            success: function (data) {
                if (typeof data == 'undefined' || data == null) return;
                Common.username = data["name"];
                Bitauto.Login.getLoginModule();
                Common.Vote();
            }
        });
        $("#AjaxLoginBox").attr("src", "");
        //登陆关闭
        AjaxLogin.close();
    }
};