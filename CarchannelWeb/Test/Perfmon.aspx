<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfmon.aspx.cs" Inherits="BitAuto.CarChannel.CarchannelWeb.Test.Perfmon" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>无标题文档</title>
    <link href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/common.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/二级990头.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/标题.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/按钮扁平.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/表单.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/星.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/文字列表.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/文章列表.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/工作/前端/Y_2014改版/A-模块规范/车型区块.css">
    <link rel="stylesheet" href="http://192.168.0.10:8888/A-UED产出物/01-网站产品/17-对比工具/201408-车型综合对比落地页/03-页面/对比.css">
    <script type="text/javascript" src="http://image.bitautoimg.com/carchannel/jscommon/juqery/jquery-1.9.1.min.js"></script>

    <script type="text/javascript" src="/jsnew/jquery.autoslider.js"></script>
</head>

<body>
    <script type="text/javascript">
        var serialId1=<%=serialId1%>,serialId2=<%=serialId2%>;
    </script>
    <div class="bt_page">
        <!--互联互通 start-->
        <div class="bt_pageBox">
            <br />
            <br />
            <br />
            <br />





            <div class="publicTabNew">
                <ul id="" class="tab">
                    <li id="xinche_index" class="current"><a href="http://www.bitauto.com/xinche/2014/">车型对比</a></li>
                    <li id="xinche_ssxc"><a href="http://www.bitauto.com/xinche/2014/ssxc.shtml">图片对比</a></li>
                    <li id="xinche_1822"><a href="http://www.bitauto.com/xinche/c1822/">综合对比</a></li>
                </ul>
                <div class="more_duibi"><a href="#">保险计算</a> | <a href="#">贷款购车</a> | <a href="#">全款购车</a></div>
            </div>


        </div>


        <!--互联互通 end-->
        <div class="line-box line-box-t20">

            <div class="top_input_box fl">
                <select name="" class="f-w200 f-curr">
                    <option>大众</option>
                </select>&nbsp;
     <select name="" class="f-w200 f-curr">
         <option>朗逸</option>
     </select>
            </div>
            <div class="top_input_box fr">
                <select name="" class="f-w200 f-curr">
                    <option>雪弗来</option>
                </select>&nbsp;
     <select name="" class="f-w200 f-curr">
         <option>克鲁兹</option>
     </select>
            </div>
            <!------------车型对比焦点开始-------------->
            <div class="vs_focus">
                <a href="javascript:;" class="focus_left">上一张</a>
                <a href="javascript:;" class="focus_right">下一张</a>
                <div class="foucs_cont">
                    <div class="car_box fl">
                        <h5>朗逸</h5>
                        <div class="photo_box" id="photobox-sl">
                            <ul id="photo-sl">
                            </ul>
                        </div>
                        <p class="link_box"><a href="#">更多朗逸图片</a></p>
                    </div>
                    <div class="car_box fr">
                        <h5>克鲁兹</h5>
                        <div class="photo_box" id="photobox-sr">
                            <ul id="photo-sr">
                            </ul>
                        </div>
                        <p class="link_box"><a href="#">更多克鲁兹图片</a></p>
                    </div>
                </div>
            </div>
            <!------------车型对比焦点结束-------------->

            <!------------谁更便宜开始-------------->
            <h3>谁更便宜</h3>
            <div class="com_tabs">
                <ul>
                    <li class="current"><em></em>两车对比最低配</li>
                    <li>两车对比最高配</li>
                </ul>
            </div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box fl">
                    <div class="head">
                        <a href="#" class="fl">朗逸</a>
                        <span class="jiage fr">10.88万</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box dao">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多朗逸报价</a></p>
                </div>
                <div class="cont_box win fr">
                    <div class="head">
                        <a href="#" class="fr">科鲁兹</a>
                        <span class="jiage fl">9.50万</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box dao">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多克鲁兹报价</a></p>
                </div>
            </div>

            <!------------谁更便宜结束-------------->

            <!------------谁的口碑更好开始-------------->
            <h3>谁的口碑更好</h3>
            <div class="com_line_box"></div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box fl">
                    <div class="head">
                        <a href="#" class="fl">朗逸</a>
                        <span class="jiage fr">10.88万<div class="mid_start_box">
                            <span class="mid_start"><em style="width: 74%"></em></span>
                        </div>
                        </span>
                    </div>
                    <div class="youque_box">
                        <dl>
                            <dt>优点</dt>
                            <dd><span>最多五个字</span><span>最多五个字</span><span>五个字</span><span>个字</span><span>字</span><span>最多个字</span></dd>
                        </dl>
                        <dl class="gary">
                            <dt>缺点</dt>
                            <dd><span>最多五个字</span><span>最多五个字</span><span>五个字</span><span>个字</span><span>字</span><span>最多个字</span></dd>
                        </dl>
                        <em></em>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="cont_box win fr">
                    <div class="head">
                        <a href="#" class="fr">科鲁兹</a>
                        <span class="jiage fl">9.50万<div class="mid_start_box">
                            <span class="mid_start"><em style="width: 74%"></em></span>
                        </div>
                        </span>
                    </div>
                    <div class="youque_box">
                        <dl>
                            <dt>优点</dt>
                            <dd><span>最多五个字</span><span>最多五个字</span><span>五个字</span><span>个字</span><span>字</span><span>最多个字</span></dd>
                        </dl>
                        <dl class="gary">
                            <dt>缺点</dt>
                            <dd><span>最多五个字</span><span>最多五个字</span><span>五个字</span><span>个字</span><span>字</span><span>最多个字</span></dd>
                        </dl>
                        <em></em>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="clear"></div>
                <ul class="bfb">
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">5.0分</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">0.0分</p>
                        </div>
                        <div class="leibie"><span>外观</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box">
                            <div class="baifenbi" style="background: #999; width: 170px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">2.0分</p>
                        </div>
                        <div class="fr_baif_box win">
                            <div class="baifenbi" style="width: 300px; background: #c00"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">4.0分</p>
                        </div>
                        <div class="leibie"><span>内饰</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">5.0分</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">0.0分</p>
                        </div>
                        <div class="leibie"><span>空间</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">5.0分</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">0.0分</p>
                        </div>
                        <div class="leibie"><span>操控</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">5.0分</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">0.0分</p>
                        </div>
                        <div class="leibie"><span>动力</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">5.0分</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">0.0分</p>
                        </div>
                        <div class="leibie"><span>舒适</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">5.0分</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">0.0分</p>
                        </div>
                        <div class="leibie"><span>性价比</span></div>
                    </li>
                </ul>
                <div class="cont_box fl">
                    <p class="link padd_t0"><a href="#">更多朗逸报价</a></p>
                </div>
                <div class="cont_box fr">
                    <p class="link padd_t0"><a href="#">更多克鲁兹报价</a></p>
                </div>
            </div>
            <!------------谁的口碑更好结束-------------->


            <!------------谁卖的更好开始-------------->
            <h3>谁卖的更好</h3>
            <div class="com_line_box"></div>
            <div class="com_tabs">
                <ul class="tabs_six">
                    <li class="current"><em></em>3月</li>
                    <li>4月</li>
                    <li>5月</li>
                    <li>6月</li>
                    <li>7月</li>
                    <li>8月</li>
                </ul>
            </div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box win fl">
                    <div class="head">
                        <a href="#" class="fl">朗逸</a>
                        <span class="jiage fr">27,000,000</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box mai">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多朗逸销量</a></p>
                </div>
                <div class="cont_box fr">
                    <div class="head">
                        <a href="#" class="fr">科鲁兹</a>
                        <span class="jiage fl">189,000</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box mai">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多克鲁兹销量</a></p>
                </div>
            </div>
            <!------------谁卖的更好结束-------------->


            <!------------谁更受观注开始-------------->
            <h3>谁更受观注</h3>
            <div class="com_line_box"></div>
            <div class="com_tabs">
                <ul class="tabs_six">
                    <li class="current"><em></em>3月</li>
                    <li>4月</li>
                    <li>5月</li>
                    <li>6月</li>
                    <li>7月</li>
                    <li>8月</li>
                </ul>
            </div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box win fl">
                    <div class="head">
                        <a href="#" class="fl">朗逸</a>
                        <span class="jiage fr">27,000,000</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box xin">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多朗逸指数</a></p>
                </div>
                <div class="cont_box fr">
                    <div class="head">
                        <a href="#" class="fr">科鲁兹</a>
                        <span class="jiage fl">189,000</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box xin">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多克鲁兹指数</a></p>
                </div>
            </div>
            <!------------谁更受观注结束-------------->



            <!------------谁空间更大开始-------------->
            <h3>谁空间更大</h3>
            <div class="com_line_box"></div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box fl">
                    <div class="head">
                        <a href="#" class="fl">朗逸</a>
                        <span class="jiage fr">10.88万<div class="mid_start_box">
                            <span class="mid_start"><em style="width: 74%"></em></span>
                        </div>
                        </span>
                    </div>
                </div>
                <div class="cont_box win fr">
                    <div class="head">
                        <a href="#" class="fr">科鲁兹</a>
                        <span class="jiage fl">9.50万<div class="mid_start_box">
                            <span class="mid_start"><em style="width: 74%"></em></span>
                        </div>
                        </span>
                    </div>
                </div>
                <div class="clear"></div>
                <ul class="bfb">
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">4850mm</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">4590mm</p>
                        </div>
                        <div class="leibie"><span>长</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box">
                            <div class="baifenbi" style="background: #999; width: 170px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">2900mm</p>
                        </div>
                        <div class="fr_baif_box win">
                            <div class="baifenbi" style="width: 300px; background: #c00"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">3000mm</p>
                        </div>
                        <div class="leibie"><span>宽</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">2900mm</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">2900mm</p>
                        </div>
                        <div class="leibie"><span>高</span></div>
                    </li>
                    <li>
                        <div class="fl_baif_box win">
                            <div class="baifenbi" style="background: #c00; width: 370px"><em style="display: none; width: 74%"></em></div>
                            <p class="fen">2900mm</p>
                        </div>
                        <div class="fr_baif_box">
                            <div class="baifenbi" style="width: 10px; background: #999"><em style="width: 44%; display: none"></em></div>
                            <p class="fen">2900mm</p>
                        </div>
                        <div class="leibie"><span>轴距</span></div>
                    </li>

                </ul>
                <div class="cont_box fl">
                    <p class="link padd_t0"><a href="#">更多朗逸报价</a></p>
                </div>
                <div class="cont_box fr">
                    <p class="link padd_t0"><a href="#">更多克鲁兹报价</a></p>
                </div>
            </div>
            <!------------谁空间更大结束-------------->


            <!------------谁更省油开始-------------->
            <h3>谁更省油</h3>
            <div class="com_tabs">
                <ul>
                    <li class="current"><em></em>两车对比最低配</li>
                    <li>两车对比最高配</li>
                </ul>
            </div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box fl">
                    <div class="head">
                        <a href="#" class="fl">朗逸</a>
                        <span class="jiage fr">8.50L</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box you">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多朗逸油耗</a></p>
                </div>
                <div class="cont_box win fr">
                    <div class="head">
                        <a href="#" class="fr">科鲁兹</a>
                        <span class="jiage fl">9.50L</span>
                    </div>
                    <div class="clear"></div>
                    <div class="com_icon_box you">
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                    <p class="link"><a href="#">更多克鲁兹油耗</a></p>
                </div>
            </div>

            <!------------谁更省油结束-------------->


            <!------------网友更喜欢谁开始-------------->
            <h3>网友更喜欢谁</h3>
            <div class="com_line_box"></div>
            <div class="clear"></div>
            <div class="com_vs">
                <div class="cont_box fl" id="con-sl">
                    <div class="head">
                        <a href="javascript:;" class="fl">朗逸</a>
                        <a href="javascript:;" class="btn fr" id="vote-vl">我支持它</a>
                        <span class="gray fr" id="vote-gray-vl" style="display: none;">我支持它</span>
                        <strong class="jia1" style="display: none;">+1</strong>
                    </div>
                </div>
                <div class="cont_box fr" id="con-sr">
                    <div class="head">
                        <a href="javascript:;" class="fr">科鲁兹</a>
                        <a href="javascript:;" class="btn fl" id="vote-vr">我支持它</a>
                        <span class="gray fl" id="vote-gray-vr" style="display: none;">我支持它</span>
                        <strong class="jia1" style="display: none;">+1</strong>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="xihuan_bfb" id="vote-width">
                    <div class="bft_left" style="width: 50%"><span>50%</span></div>
                    <span class="bft_right">50%</span>
                </div>
            </div>
            <script type="text/javascript">
                var queryCsIdArr = [serialId1, serialId2];

                Array.prototype.indexOf = function (value) { for (var i = 0, l = this.length; i < l; i++) if (this[i] == value) return i; return -1; }
                Array.prototype.clone = function () { return this.slice(0); }
                //初始化投票数据
                function initVote() {
                    var cookieName = "serialpkvote", key = queryCsIdArr.clone().sort().join("_"), cookieV = getCookie(cookieName);
                    if (cookieV != null && (cookieArr = cookieV.split(','), cookieArr.indexOf(key) != -1)) {
                        $("#vote-vl,#vote-vr").hide();
                        $("#vote-gray-vl,#vote-gray-vr").show();
                    }

                    $("#vote-vl").click(function () {
                        if (queryCsIdArr[0] < queryCsIdArr[1])
                            voteSerial(1, true);
                        else
                            voteSerial(2, true);
                    });
                    $("#vote-vr").click(function () {
                        if (queryCsIdArr[0] < queryCsIdArr[1])
                            voteSerial(2, false);
                        else
                            voteSerial(1, false);
                    });


                    $.ajax({
                        url: "http://api01.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=getvote&csid=" + queryCsIdArr.join(','), cache: false, dataType: "jsonp", jsonpCallback: "getVoteCallback", success: function (data) {
                            showVote(data);
                        }
                    });
                }
                initVote();
                //投票 flag id小1大2 
                function voteSerial(flag, isLeft) {
                    var cookieName = "serialpkvote", key = queryCsIdArr.clone().sort().join("_"), cookieV = getCookie(cookieName), cookieArr = [];
                    if (cookieV != null && (cookieArr = cookieV.split(','), cookieArr.indexOf(key) != -1)) {
                        return;
                    }
                    $.ajax({
                        url: "http://api01.car.bitauto.com/carinfo/serialcomparevote.ashx?csids=" + queryCsIdArr.join(',') + "&flag=" + flag, cache: false, dataType: "jsonp", jsonpCallback: "voteCallback", success: function (data) {
                            showVote(data);
                            //按钮状态
                            var voteId = isLeft ? "con-sl" : "con-sr";
                            $("#" + voteId).find(".jia1").show().fadeOut(2000);
                            $("#vote-vl,#vote-vr").hide();
                            $("#vote-gray-vl,#vote-gray-vr").show();
                            //
                            cookieArr.push(key);
                            if (cookieArr.length > 9) cookieArr.splice(0, 1);
                            setCookie(cookieName, cookieArr.join(','), 1);
                        }
                    });
                }
                //展示投票条长度
                function showVote(data) {
                    var left = data[queryCsIdArr[0]], right = data[queryCsIdArr[1]],
                        leftper = (left == 0 && right == 0) ? 50 : parseInt((left / (left + right)) * 100),
                        rigthper = 100 - leftper;
                    if (left >= right) {
                        $("#con-sl").addClass("win");
                        $("#con-sr").removeClass("win");
                    } else {
                        $("#con-sr").addClass("win");
                        $("#con-sl").removeClass("win");
                    }
                    $("#vote-width .bft_left").animate({ width: leftper + "%" }, 500, function () { }).html(leftper + "%");
                    $("#vote-width .bft_right").html(rigthper + "%");
                }
                //取Cookie
                function getCookie(name) {
                    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
                    if (arr != null) {
                        return unescape(arr[2]);
                    }
                    return null;
                }
                //设置Cookies
                function setCookie(name, value, expires, path, domain, secure) {
                    var today = new Date();
                    today.setTime(today.getTime());
                    if (expires) {
                        expires = expires * 1000 * 60 * 60 * 24;
                    }
                    var expires_date = new Date(today.getTime() + (expires));
                    document.cookie = name + '=' + escape(value) +
                            ((expires) ? ';expires=' + expires_date.toGMTString() : '') + //expires.toGMTString() 
                            ((path) ? ';path=' + path : '') +
                            ((domain) ? ';domain=' + domain : '') +
                            ((secure) ? ';secure' : '');
                }
                //删除Cookies
                function deleteCookie(name, path, domain) {
                    if (getCookie(name)) document.cookie = name + '=' +
                                ((path) ? ';path=' + path : '') +
                                ((domain) ? ';domain=' + domain : '') +
                                ';expires=Thu, 01-Jan-1970 00:00:01 GMT';
                }

                function initFocusImage() {
                    var imageArray = new Array(2);
                    $.ajax({
                        url: "http://api01.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=carbaseinfo&csid=" + queryCsIdArr[0], cache: false, dataType: "jsonp", jsonpCallback: "carbaseinfoCallback1", success: function (data) {
                            imageArray[0] = data;
                            if (imageArray[1] != undefined) {
                                fillImageBox(imageArray);
                            }
                        }
                    });

                    $.ajax({
                        url: "http://api01.car.bitauto.com/carinfo/serialcompareforpk.ashx?action=carbaseinfo&csid=" + queryCsIdArr[1], cache: false, dataType: "jsonp", jsonpCallback: "carbaseinfoCallback2", success: function (data) {
                            imageArray[1] = data;
                            if (imageArray[1] != undefined) {
                                fillImageBox(imageArray);
                            }
                        }
                    });
                }
                initFocusImage();
                //处理焦点图片
                function fillImageBox(imageArray) {
                    var slHtml = [], srHtml = [];
                    $.each(imageArray[0].imagelist, function (i, n) {
                        slHtml.push("<li><a href=\"" + n.imglink + "\"><img src=\"" + n.imgurl + "\" alt=\"\"></a></li>");
                    });
                    $("#photo-sl").html(slHtml.join(''));
                    $.each(imageArray[1].imagelist, function (i, n) {
                        srHtml.push("<li><a href=\"" + n.imglink + "\"><img src=\"" + n.imgurl + "\" alt=\"\"></a></li>");
                    });
                    $("#photo-sr").html(srHtml.join(''));

                    $("#photobox-sl").autoslider();
                    $("#photobox-sr").autoslider();

                }
            </script>
            <!------------网友更喜欢谁结束-------------->


            <!------------看看编辑怎么说开始-------------->

            <h3>看看编辑怎么说</h3>
            <div class="com_line_box"></div>
            <div class="clear"></div>
            <div class="tuwenlistbox">
                <ul>
                    <li>
                        <a href="#" class="img" target="_blank">
                            <img src="http://img2.bitautoimg.com/newsimg_210_w0_1/bitauto/2014/04/11/e10d0f55-4e04-473f-af07-d3b9721e72b0_630.jpg" width="210" height="140">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                        <div class="textcont">
                            <h3><a href="#" target="_blank">哈弗H6 1.5T自动挡车型路试谍照曝光</a></h3>
                            <p class="info"><span>来源：<em>易车网</em></span><span>作者：<em>谢迪</em></span><span>2014-04-11 14:28</span><span><em class="liulan">5645</em><em class="huifu">464564</em></span></p>
                            <ul class="text-list">
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                            </ul>
                            <ul class="text-list">
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">查看更多&gt;&gt;</a></li>
                            </ul>
                        </div>
                    </li>
                    <li>
                        <a href="#" class="img" target="_blank">
                            <img src="http://img2.bitautoimg.com/newsimg_210_w0_1/bitauto/2014/04/11/e10d0f55-4e04-473f-af07-d3b9721e72b0_630.jpg" width="210" height="140">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                        <div class="textcont">
                            <h3><a href="#" target="_blank">哈弗H6 1.5T自动挡车型路试谍照曝光</a></h3>
                            <p class="info"><span>来源：<em>易车网</em></span><span>作者：<em>谢迪</em></span><span>2014-04-11 14:28</span><span><em class="liulan">5645</em><em class="huifu">464564</em></span></p>
                            <ul class="text-list">
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                            </ul>
                            <ul class="text-list">
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">查看更多&gt;&gt;</a></li>
                            </ul>
                        </div>
                    </li>
                    <li>
                        <a href="#" class="img" target="_blank">
                            <img src="http://img2.bitautoimg.com/newsimg_210_w0_1/bitauto/2014/04/11/e10d0f55-4e04-473f-af07-d3b9721e72b0_630.jpg" width="210" height="140">
                            <div class="title">这里是标题 <em>vs</em> 这里是标题</div>
                            <span></span>
                        </a>
                        <div class="textcont">
                            <h3><a href="#" target="_blank">哈弗H6 1.5T自动挡车型路试谍照曝光</a></h3>
                            <p class="info"><span>来源：<em>易车网</em></span><span>作者：<em>谢迪</em></span><span>2014-04-11 14:28</span><span><em class="liulan">5645</em><em class="huifu">464564</em></span></p>
                            <ul class="text-list">
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                            </ul>
                            <ul class="text-list">
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">一汽奔腾亮相2014北京车展北京车展北京车展</a></li>
                                <li><a href="#">查看更多&gt;&gt;</a></li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>



            <!------------看看编辑怎么说结束-------------->


            <!------------看过这两辆车的还看开始-------------->
            <div class="line-box">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4><a href="#">看过这两辆车的还看</a></h4>

                    </div>
                </div>

                <div class="carpic_list box_990">
                    <ul>
                        <li><a target="_blank" href="brand.aspx?newbrandid=3044">
                            <img alt="慕尚汽车报价_价格" src="http://img2.bitautoimg.com/autoalbum/files/20120306/838/0537198387_2.jpg">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                            <div class="txt"><em>9.39万-15.09万</em></div>
                        </li>
                        <li><a target="_blank" href="brand.aspx?newbrandid=3044">
                            <img alt="慕尚汽车报价_价格" src="http://img2.bitautoimg.com/autoalbum/files/20120306/838/0537198387_2.jpg">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                            <div class="txt"><em>9.39万-15.09万</em></div>
                        </li>
                        <li><a target="_blank" href="brand.aspx?newbrandid=3044">
                            <img alt="慕尚汽车报价_价格" src="http://img2.bitautoimg.com/autoalbum/files/20120306/838/0537198387_2.jpg">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                            <div class="txt"><em>9.39万-15.09万</em></div>
                        </li>
                        <li><a target="_blank" href="brand.aspx?newbrandid=3044">
                            <img alt="慕尚汽车报价_价格" src="http://img2.bitautoimg.com/autoalbum/files/20120306/838/0537198387_2.jpg">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                            <div class="txt"><em>9.39万-15.09万</em></div>
                        </li>
                        <li><a target="_blank" href="brand.aspx?newbrandid=3044">
                            <img alt="慕尚汽车报价_价格" src="http://img2.bitautoimg.com/autoalbum/files/20120306/838/0537198387_2.jpg">
                            <div class="title">这里是标题</div>
                            <span></span>
                        </a>
                            <div class="txt"><em>9.39万-15.09万</em></div>
                        </li>
                    </ul>
                </div>
            </div>
            <!------------看过这两辆车的还看结束-------------->

            <!------------相关文章开始-------------->
            <div class="line-box">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4><a href="#">看过这两辆车的还看</a></h4>

                    </div>
                </div>

                <div class="text-list-box-b text-list-box-b-990">
                    <div class="text-list-box">
                        <ul class="text-list text-list-float text-list-float3 text-list-w1010">
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                            <li><a href="#">福克斯三厢配置福克斯三厢配置福克斯三厢配置</a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <!------------相关文章结束-------------->


            <!------------相关对比开始-------------->
            <div class="line-box">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4><a href="#">相关对比</a></h4>
                    </div>
                </div>
                <div class="vs_photo_box">
                    <div class="vs_photo">
                        <div class="car_box fl">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="car_box fr">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="vs_img"></div>
                    </div>
                    <div class="vs_photo vs_hover">
                        <div class="car_box fl">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="car_box fr">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="vs_img"></div>
                    </div>
                    <div class="vs_photo">
                        <div class="car_box fl">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="car_box fr">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="vs_img"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <!------------相关对比结束-------------->

            <!------------热门对比开始-------------->
            <div class="line-box">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4><a href="#">相关对比</a></h4>
                    </div>
                </div>
                <div class="vs_photo_box">
                    <div class="vs_photo">
                        <div class="car_box fl">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="car_box fr">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="vs_img"></div>
                    </div>
                    <div class="vs_photo vs_hover">
                        <div class="car_box fl">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="car_box fr">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="vs_img"></div>
                    </div>
                    <div class="vs_photo">
                        <div class="car_box fl">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="car_box fr">
                            <a href="#">
                                <img src="images/pic_300x200.png" alt="" /></a>
                            <p class="title"><a href="#">哈佛h6</a></p>
                            <p class="txt">9.39万-16.68万</p>
                        </div>
                        <div class="vs_img"></div>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <!------------热门对比结束-------------->



            <!------------最新对比开始-------------->
            <%if (!string.IsNullOrEmpty(serialNewCompareHtml))
              { %>
            <div class="line-box">
                <div class="title-con">
                    <div class="title-box title-box2">
                        <h4><a href="#">最新对比</a></h4>

                    </div>
                </div>

                <div class="text-list-box-b text-list-box-b-990">
                    <div class="text-list-box">
                        <ul class="text-list text-list-float text-list-float3 text-list-w1010">
                            <%=serialNewCompareHtml %>
                        </ul>
                    </div>
                </div>
            </div>
            <%} %>
            <!------------最新对比结束-------------->


        </div>



    </div>
</body>
</html>

