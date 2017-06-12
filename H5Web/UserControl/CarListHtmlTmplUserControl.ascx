<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarListHtmlTmplUserControl.ascx.cs" Inherits="H5Web.UserControl.CarListHtmlTmplUserControl" %>
<!-- 车款列表模板 -->
<script type="text/x-jquery-tmpl" id="carListTmpl">
    <header class="header2">
        <a href="javascript:void(0);" id="goBack" class="back-step">上一步</a>
        <ul class="tags-sub tags-sub-left">
            <li><a href="javascript:void(0);" id="followSort">按关注</a></li>
            <li class="arrow arrow-d-t"><a href="javascript:void(0)" id="priceSort">价格<s></s><i></i></a></li>
            <%--<li><a href="javascript:void(0)" id="wordSort">按好评</a></li>--%>
        </ul>        
    </header>
    <div class="car_list_box">
        <div class="car_list_num">总共${Count}个车型</div>
        {{if ResList.length>0}}
        <ul class="car_list">
            {{each ResList}}           
                <li id="${$value.SerialId}">
                    <a href="/${$value.AllSpell}/">
                        <img src="${$value.ImageUrl.replace('_1.','_6.')}" />
                        <span>${$value.ShowName}</span>
                        <p>${$value.PriceRange}</p>
                    </a>
                </li>
            {{/each}}
        </ul>
        {{/if}}
        <!--翻页 开始-->
        <div class="m-pages" style="display:none">
            <a href="javascript:void(0)" class="m-pages-pre m-pages-none" id="prevPage">上一页</a>
            <div class="m-pages-num">
                <div id="currentPage" class="m-pages-num-con">999/999</div>
                <div class="m-pages-num-arrow"></div>
            </div>
            <select id="selectPage">
                <option>1</option>
                <option>2</option>
                <option>3</option>
                <option>4</option>
                <option>5</option>
            </select>
            <a href="javascript:void(0);" class="m-pages-next" id="nextPage">下一页</a>
        </div>
        <!--翻页结束-->
    </div>
</script>

<!-- 广告模板 -->
<script type="text/x-jquery-tmpl" id="adtmpl">    
    <li>
        <a href="/${AllSpell}/">
            <img src="${ImageUrl.replace('_1.','_6.')}" />
            <span>${ShowName}</span>
            <p>${PriceRange}</p>
        </a>
    </li> 
</script>
