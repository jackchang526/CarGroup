
var DefaultAttension = {
    BtHide: function(id) {
        var Div = document.getElementById(id); if (Div) { Div.style.display = "none" }
    },
    BtShow: function(id) {
        var Div = document.getElementById(id); if (Div) { Div.style.display = "block" }
    },
    BtTabRemove: function(index, head, divs) {
        var tab_heads = document.getElementById(head);
        if (tab_heads) {
            var lis = tab_heads.getElementsByTagName("li"); var as = tab_heads.getElementsByTagName("a");
            for (var i = 0; i < as.length; i++) {
                lis[i].className = ""; DefaultAttension.BtHide(divs + "_" + i); if (i == index) { lis[i].className = "current"; }
            }
            DefaultAttension.BtShow(divs + "_" + index)
        }
    },
    BtTabOn: function(head, divs) {
        var tab_heads = document.getElementById(head);
        if (tab_heads) {
            DefaultAttension.BtTabRemove(0, head, divs);
            var alis = tab_heads.getElementsByTagName("a");
            for (var i = 0; i < alis.length; i++) {
                alis[i].num = i;
                alis[i].onclick = function() {
                    var parent = this.parentNode;
                    if (parent != null
                        && parent.nodeType == 1
                        && parent.className == "current"
                        && this.href.indexOf(window.location.href) < 0) {
                        return true;
                    }
                    DefaultAttension.BtTabRemove(this.num, head, divs); this.blur(); return false;

                }
                //alis[i].onfocus = function() { DefaultAttension.BtTabRemove(this.num, head, divs) }
            }
        }
    },

    BtZebraStrips: function(id, tag) {
        var ListId = document.getElementById(id);
        if (ListId) {
            var tags = ListId.getElementsByTagName(tag);
            for (var i = 0; i < tags.length; i++) {
                tags[i].className += " barry" + i % 2;
                tags[i].onmouseover = function() { this.className += " hover" }
                tags[i].onmouseout = function() { this.className = this.className.replace(" hover", "") }
            }
        }
    },
    init: function() {
        DefaultAttension.BtTabOn("rank1_tab", "rank1_tab");
        DefaultAttension.BtTabOn("rank2_tab", "rank2_tab");
        DefaultAttension.BtTabOn("rank3_tab", "rank3_tab");
    }
}
DomHelper.addEvent(window, "load", DefaultAttension.init, false);